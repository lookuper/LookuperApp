using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using AutoMapper;
using Helpers;
using LookupperData;
using LM = LookuperModel;

namespace LookuperModel
{
    public class LookuperModel
    {
        private readonly IRepository<LookuperItem> lookuperItemsRepository = new LookupItemRepository();

        public IEnumerable<LookupItemDto> AvaliableLookupItems
        {
            get { return GetAvaliableItems(); }
        }

        public LookuperModel()
        {
            Mapper.CreateMap<LookuperItem, LookupItemDto>();
            Mapper.CreateMap<LookupItemDto, LookuperItem>();
        }

        public void AddItem(LookupItemDto item)
        {
            var duplicate = FindItem(item);

            if (duplicate != null)
                throw new InvalidOperationException("such object already exists");

            TimerConfiguration(item);
            var unmappedItem = Mapper.Map<LookuperItem>(item);

            lookuperItemsRepository.Insert(unmappedItem);
        }

        public void Start(LookupItemDto item)
        {
            item.IsActive = true;
            item.Timer.Start();

            RefreshItem(item);
        }

        public void Stop(LookupItemDto item)
        {
            item.IsActive = false;
            item.Timer.Stop();

            RefreshItem(item);
        }

        public void UpdateItem(LookupItemDto item)
        {
            var lookup = FindItem(item);

            if (lookup == null) // new item            
                AddItem(item);

            var itemToUpdate = lookup;

            itemToUpdate.Name = item.Name;
            itemToUpdate.AddressUrl = item.AddressUrl;
            itemToUpdate.CheckInterval = item.CheckInterval;
            itemToUpdate.CreatedDate = item.CreatedDate;
            itemToUpdate.Data = item.Data;
            itemToUpdate.IsActive = item.IsActive;
            itemToUpdate.UpdateAvaliable = item.HasUpdate;
            itemToUpdate.Comparer = item.Comparer;
            itemToUpdate.XPath = item.XPath;

            lookuperItemsRepository.Save();
            //item.ResetCountdownTime();
        }

        public void DeleteItem(LookupItemDto item)
        {
            var itemToDelete = FindItem(item);

            if (itemToDelete == null)
                throw new InvalidOperationException("no such item to delete");

            item.Timer.Stop();
            var unmappedItem = Mapper.Map<LookuperItem>(itemToDelete);
            lookuperItemsRepository.Delete(unmappedItem);
        }

        public async void RefreshItem(LookupItemDto item)
        {
            if (item.IsActive)
            {
                item.Timer.Stop();
                var oldData = item.Data;

                Task<String> future = DownloadDataFactory.Instance.WebClient.DownloadStringTaskAsync(item.AddressUrl);
                var newData = await future; //DownloadContentString(item.AddressUrl);

                item.ResetCountdownTime();
                item.Timer.Start();

                var dataComparer = new DataComparerFactory(oldData, newData, item.XPath).GetDataComparer(item.Comparer);
                var comparsionResult = dataComparer.Compare();

                switch (comparsionResult)
                {
                    case DataComparsionResultEnum.None:
                        {
                            item.Data = newData;
                            UpdateItem(item);

                            break;
                        }
                    case DataComparsionResultEnum.NoChanges:
                        //item.ResetCountdownTime();
                        break;
                    case DataComparsionResultEnum.Error:
                        var htmlDiff = new HtmlDiff(oldData, newData);
                        string htmlGeneratedDiff = htmlDiff.Build();

                        break;
                    case DataComparsionResultEnum.ChangesAvaliable:
                        {
                            item.HasUpdate = true;
                            item.NewData = newData;

                            UpdateItem(item); // just for NewData
                            break;
                        }
                    default:
                        break;
                }
            }
            else // not active
            {
                UpdateItem(item);
            }
        }

        private LookuperItem FindItem(LookupItemDto item)
        {
            var findedItem = lookuperItemsRepository
                .Find(x => x.Name == item.Name);

            if (findedItem == null || findedItem.Count() == 0)
                return null;

            return findedItem.First();
        }

        private ObservableCollection<LookupItemDto> GetAvaliableItems()
        {
            var avaliableItemsDto = new ObservableCollection<LookupItemDto>();

            foreach (var item in lookuperItemsRepository.GetAll())
            {
                var mappedItem = Mapper.Map<LookupItemDto>(item);
                TimerConfiguration(mappedItem);

                if (mappedItem.IsActive)
                    mappedItem.Timer.Start();

                avaliableItemsDto.Add(mappedItem);
            }

            return avaliableItemsDto;
        }

        private void TimerConfiguration(LookupItemDto item)
        {
            item.Timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Render, delegate
            {
                if (item.TimeUntilUpdate == TimeSpan.Zero)
                {
                    item.TimeUntilUpdate = item.CheckInterval;
                    RefreshItem(item);
                    //(sender as DispatcherTimer).Start();
                }
                item.TimeUntilUpdate = item.TimeUntilUpdate.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            if (!item.IsActive)
                item.Timer.Stop();
        }

        private IEnumerable<LookupItemDto> UpdateData(IEnumerable<LookupItemDto> items)
        {
            var itemList = new List<LookupItemDto>();

            foreach (var item in items)
            {
                item.Data = DownloadContentString(item.AddressUrl);
                itemList.Add(item);
            }

            return itemList;
        }

        public string DownloadContentString(string url)
        {
            return DownloadDataFactory.Instance.WebClient.DownloadString(url);
        }

        public void NavigateToLookupItem(string address)
        {
            using (var browserProcess = new Process())
            {
                browserProcess.StartInfo.UseShellExecute = true;
                browserProcess.StartInfo.FileName = address;

                browserProcess.Start();
            }
        }

        public void NavigateToUpdatedItem(LookupItemDto item)
        {
            if (item.HasUpdate)
            {
                using (var browserProcess = new Process())
                {
                    browserProcess.StartInfo.UseShellExecute = true;
                    browserProcess.StartInfo.FileName = item.AddressUrl;

                    browserProcess.Start();
                }

                NavigateToUpdatedItemVisit(item);
            }
        }

        public void NavigateToUpdatedItemVisit(LookupItemDto item)
        {
            item.Data = item.NewData;
            item.NewData = null;
            item.HasUpdate = false;

            UpdateItem(item);
        }

        public string GetHtmlDiff(string oldData, string newData)
        {
            var htmlDiff = new HtmlDiff(oldData, newData);
            return htmlDiff.Build();
        }

        public string GetHtmlDiff(LookupItemDto Item)
        {
            string data1 = CustomDataComparer.GetHtml(Item.Data, Item.XPath);
            string data2 = CustomDataComparer.GetHtml(Item.NewData, Item.XPath);
            string header = CustomDataComparer.GetHtml(Item.Data, @"//head");

            //string highlightPattern = "<style type=\"text/css\"> ins { background-color: #cfc; text-decoration: none; } del { color: #999;  background-color:#FEC8C8; } </style>";

            var diff = GetHtmlDiff(data1, data2);

            string highlightPattern = LM.Properties.Resources.HtmlDiffPattern
                .Replace("{0}", header)
                .Replace("{1}", diff);

            return highlightPattern;
        }

        public string GetLookupContent(string url, string xpath)
        {
            return CustomDataComparer.GetText(url, xpath);
        }
    }
}
