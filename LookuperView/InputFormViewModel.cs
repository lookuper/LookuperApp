using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CefSharp;
using CefSharp.Wpf;
using LookuperModel;


namespace LookuperView
{
    public class InputFormViewModel : BaseViewModel
    {
        private static int fuckCounter = 0;
        private volatile bool alreadyHitted = false;

        private readonly LookuperModel.LookuperModel model = new LookuperModel.LookuperModel();
        private WebView webBrowser;

        public String CurrentHtml { get; private set; }
        public WebView WebBrowser
        {
            get { return webBrowser; }
            set { webBrowser = value; OnPropertyChanged("WebBrowser"); }
        }

        public LookupItemDto FormItem { get; private set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancellCommand { get; set; }
        public ICommand PreviewCommand { get; set; }

        private string previewData;
        public string PreviewData
        {
            get { return previewData; }
            set { previewData = value; OnPropertyChanged("PreviewData"); }
        }

        public IList<String> AvaliableComparers
        {
            get { return DataComparerFactory.Mappings; }
        }

        public InputFormViewModel()
        {
            FormItem = new LookupItemDto()
            {
                IsActive = true,
                CreatedDate = DateTime.Now,
                CheckInterval = TimeSpan.FromMinutes(10),
                Data = String.Empty,
                XPath = String.Empty,
            };

            SaveCommand = new RelayCommand<Window>(HandleSave);
            CancellCommand = new RelayCommand<Window>(CancellHandler);
            PreviewCommand = new RelayCommand<Object>(PreviewHandler);

            // deleted in new version of cefSharp
            //var settings = new CefSharp.Settings
            //{
            //    //PackLoadingDisabled = true,
            //    AutoDetectProxySettings = true,
            //};

            //if (CEF.Initialize(settings))
            //{ }

            var settingsForView = new CefSharp.BrowserSettings
            {
                DefaultEncoding = "UTF-8",
            };
            
            var settings = new CefSettings()
            {
                PackLoadingDisabled = true,   
                
            };
            if (!Cef.IsInitialized)
                Cef.Initialize(settings);


            var bSettings = new BrowserSettings()
            {
                DefaultEncoding = "UTF-8",                
            };
            WebBrowser = new WebView(); //new WebView(String.Empty, settingsForView);
            WebBrowser.BrowserSettings = bSettings;

            //webBrowser.PropertyChanged += webBrowser_PropertyChanged;
            //webBrowser.Loaded += webBrowser_Loaded;
            //webBrowser.LoadCompleted += webBrowser_LoadCompleted;
        }

        public InputFormViewModel(LookupItemDto item) : this()
        {
            this.FormItem = item;

            WebBrowser.Address = FormItem.AddressUrl;
            WebBrowser.SetAddress(FormItem.AddressUrl);
        }

        void webBrowser_LoadCompleted(object sender, LoadCompletedEventArgs url)
        {
            if (!alreadyHitted)
            {
                //WebBrowser.ExecuteScript(Properties.Resources.JQueryXPathByClick);
                //WebBrowser.ExecuteScript(Properties.Resources.LoadJQuery);

                //WebBrowser.ExecuteScript(Properties.Resources.AddCss);

                //bool isJQueryLoaded = (bool)WebBrowser.EvaluateScript("typeof jQuery != 'undefined'");

                //if (!isJQueryLoaded)
                //{
                //    WebBrowser.EvaluateScript(Properties.Resources.LoadJQuery);
                //}

                //WebBrowser.ExecuteScript(Properties.Resources.LoadJQuery);
                //WebBrowser.ExecuteScript(Properties.Resources.HighlightCode);                                 

                //WebBrowser.ExecuteScript(Properties.Resources.HighlightCode);
                //WebBrowser.ExecuteScript(Properties.Resources.JQueryXPathByClick2);

                alreadyHitted = true;
            }
            else
            {
                var html = WebBrowser.EvaluateScript(@"document.documentElement.innerHTML") as String;
            }
            //WebBrowser.ExecuteScript(Properties.Resources.JQueryXPathByClick);
        }

        void webBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            int i = 5;
        }

        void webBrowser_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsLoaded", StringComparison.OrdinalIgnoreCase))
            {
                int i = 6;
            }

            if (e.PropertyName.Equals("IsBrowserInitialized", StringComparison.OrdinalIgnoreCase))
            {
                // inject
                int i = 5;
                //WebBrowser.ExecuteScript(Properties.Resources.HighlightCss);
                //WebBrowser.ExecuteScript(Properties.Resources.HighlightElement);
                //WebBrowser.ExecuteScript(Properties.Resources.JQueryXPathByClick);
            }

            if (e.PropertyName.Equals("IsLoading", StringComparison.OrdinalIgnoreCase))
            {
                int i = 6;
            }
        }

        public String GetPageHtml()
        {
            string html = null;

            if (WebBrowser.IsLoaded)
                html = WebBrowser.EvaluateScript(@"document.getElementsByTagName ('html')[0].innerHTML") as String;

            return html;
        }

        private void HandleSave(Window parameter)
        {
            parameter.DialogResult = true;
            CancellHandler(parameter);
        }

        private void CancellHandler(Window parameter)
        {
            parameter.Close();
        }

        private void PreviewHandler(Object param)
        {
            var url = FormItem.AddressUrl;
            var xpath = FormItem.XPath;

            string content = model.GetLookupContent(url, xpath);
            PreviewData = content;
        }
    }
}
