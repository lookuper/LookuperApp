using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LookuperModel
{
    public class LookupItemDto : INotifyPropertyChanged, IDataErrorInfo
    {
        public DispatcherTimer Timer { get; set; }

        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        private string addressUrl;
        public string AddressUrl
        {
            get { return addressUrl; }
            set { addressUrl = value; OnPropertyChanged("AddressUrl"); }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; OnPropertyChanged("IsActive"); }
        }

        private string data;
        public string Data
        {
            get { return data; }
            set { data = value; OnPropertyChanged("Data"); }
        }

        private DateTime createdDate;
        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; OnPropertyChanged("CreatedDate"); }
        } // when item created

        private TimeSpan checkInterval;
        public TimeSpan CheckInterval
        {
            get { return checkInterval; }
            set
            {
                checkInterval = value;
                timeUntilUpdate = checkInterval;
                OnPropertyChanged("CheckInterval");
            }
        }

        private bool hasUpdate;
        public bool HasUpdate
        {
            get { return hasUpdate; }
            set { hasUpdate = value; OnPropertyChanged("HasUpdate"); }
        }

        private TimeSpan timeUntilUpdate;
        public TimeSpan TimeUntilUpdate
        {
            get { return timeUntilUpdate; }
            set { timeUntilUpdate = value; OnPropertyChanged("TimeUntilUpdate"); }
        }

        private string comparer;
        public String Comparer
        {
            get { return comparer; }
            set { comparer = value; OnPropertyChanged("Comparer"); }
        }

        private string xpath;
        public String XPath
        {
            get { return xpath; }
            set { xpath = value; OnPropertyChanged("XPath"); }
        }

        public string NewData
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler == null)
                return;

            var eventArgs = new PropertyChangedEventArgs(propertyName);
            handler(this, eventArgs);
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string propertyName]
        {
            get
            {
                string validationResult = null;

                switch (propertyName)
                {
                    case "Name":
                        if (String.IsNullOrEmpty(Name))
                            return "ERROR";
                        break;
                    case "AddressUrl":
                        if (String.IsNullOrEmpty(AddressUrl))
                            return "ERROR";
                        break;
                    case "CheckInterval":
                        if (TimeSpan.MinValue == CheckInterval)
                            return "ERROR";
                        break;
                    case "Comparer":
                        if (String.IsNullOrEmpty(Comparer))
                            return "ERROR";
                        break;
                    case "XPath":
                        if (String.IsNullOrEmpty(XPath))
                            return "ERROR";
                        break;
                }

                return validationResult;
            }
        }
    }

}
