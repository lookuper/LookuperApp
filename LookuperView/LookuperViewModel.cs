using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LookuperModel;

namespace LookuperView
{
    public sealed class LookuperViewModel : BaseViewModel
    {
        private readonly LookuperModel.LookuperModel model = new LookuperModel.LookuperModel();
        private String mainError;
        private Visibility warningLabelVisibility = Visibility.Hidden;
        private String errorStackTrace;

        public ObservableCollection<LookupItemDto> LookupItems { get; set; }
        public ObservableCollection<String> RightNotifications { get; set; }

        #region Properties
        public String MainError
        {
            get { return mainError; }
            set { mainError = value; OnPropertyChanged("MainError"); }
        }

        public Visibility WarningLabelVisability
        {
            get { return warningLabelVisibility; }
            set { warningLabelVisibility = value; OnPropertyChanged("WarningLabelVisability"); }
        }

        public String ErrorStackTrace
        {
            get { return errorStackTrace; }
            set { errorStackTrace = value; OnPropertyChanged("ErrorStackTrace"); }
        }
        #endregion

        public ICommand AddNewItemCommand { get; set; }
        public ICommand RemoveItemCommand { get; set; }
        public ICommand RefreshItemCommand { get; set; }
        public ICommand StartLookuperForItemCommand { get; set; }
        public ICommand StopLookuperForItemCommand { get; set; }
        public ICommand OnViewButtonClick { get; set; }
        public ICommand ConfigureItemCommand { get; set; }
        public ICommand OnDiffButtonClick { get; set; }
        public ICommand OnHyperlinkClick { get; set; }
        public ICommand ClearErrorCommand { get; set; }

        public LookuperViewModel()
        {
            LookupItems = new ObservableCollection<LookupItemDto>(model.AvaliableLookupItems);

            RightNotifications = new ObservableCollection<string>();
            RightNotifications.Add("Test Notification");

            AddNewItemCommand = new RelayCommand<object>(HandleAddNewItem);
            RemoveItemCommand = new RelayCommand<LookupItemDto>(HandleRemoveItem);
            RefreshItemCommand = new RelayCommand<LookupItemDto>(HandleRefreshItem);
            StartLookuperForItemCommand = new RelayCommand<LookupItemDto>(HandleStart);
            StopLookuperForItemCommand = new RelayCommand<LookupItemDto>(HandleStop);
            OnViewButtonClick = new RelayCommand<LookupItemDto>(HandleNavigateToUpdate);
            ConfigureItemCommand = new RelayCommand<LookupItemDto>(HandleConfigureImte);
            OnDiffButtonClick = new RelayCommand<LookupItemDto>(HandleOnDiffButtonClick);
            OnHyperlinkClick = new RelayCommand<string>(HandleOnHyperlinkClick);
            ClearErrorCommand = new RelayCommand<Object>(OnClearErrorCommand);
        }

        private void OnClearErrorCommand(object obj)
        {
            ClearError();
        }

        private void HandleOnHyperlinkClick(string address)
        {
            model.NavigateToLookupItem(address);
        }

        private void HandleOnDiffButtonClick(LookupItemDto item)
        {
            throw new NotImplementedException();
            //var diffForm = new DiffForm()
            //{
            //    Owner = Application.Current.MainWindow,
            //    ShowInTaskbar = false,
            //    WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //    DataContext = new DiffFormViewModel(item),
            //};

            //var diffFormViewModel = diffForm.DataContext as DiffFormViewModel;

            //if (diffFormViewModel != null)
            //{
            //    diffFormViewModel.OnHtmlDiffButtonClick(null);
            //    model.NavigateToUpdatedItemVisit(item);
            //}
            //diffForm.ShowDialog();
        }

        private void HandleConfigureImte(LookupItemDto item)
        {
            if (item == null)
                return;

            var inputForm = new InputForm()
            {
                Title = "Configure item",
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                DataContext = new InputFormViewModel(item),
            };

            if (inputForm.ShowDialog().Value)
            {
                model.UpdateItem(item);
            }

        }

        private void HandleNavigateToUpdate(LookupItemDto item)
        {
            model.NavigateToUpdatedItem(item);
        }

        private void HandleStart(LookupItemDto item)
        {
            if (item == null || item.IsActive)
                return;

            model.Start(item);
        }

        private void HandleStop(LookupItemDto item)
        {
            if (item == null || !item.IsActive)
                return;

            model.Stop(item);
        }

        private void HandleRefreshItem(LookupItemDto item)
        {
            if (item != null)
                model.RefreshItem(item);
            else
            {
                return;
                // refresh all
            }
        }

        private void HandleAddNewItem(object parameter)
        {
            var inputForm = new InputForm()
            {
                Title = "Add lookup item",
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            if (inputForm.ShowDialog().Value)
            {
                var viewModel = inputForm.DataContext as InputFormViewModel;
                var item = viewModel.FormItem;

                model.AddItem(item);
                LookupItems.Add(item);
            }
        }

        private void HandleRemoveItem(LookupItemDto item)
        {
            model.DeleteItem(item);
            LookupItems.Remove(item);
        }

        public void SetError(string errorMessage, string stackTrace = null)
        {
            WarningLabelVisability = Visibility.Visible;
            MainError = errorMessage;
            ErrorStackTrace = stackTrace;
        }

        public void ClearError()
        {
            WarningLabelVisability = Visibility.Hidden;
            MainError = null;
            ErrorStackTrace = null;
        }
    }
}
