using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CefSharp;

namespace LookuperView
{
    /// <summary>
    /// Interaction logic for InputForm.xaml
    /// </summary>
    public partial class InputForm : Window
    {
        private readonly InputFormViewModel Presenter;
        public InputForm()
        {
            //webView.souAddress
            InitializeComponent();

            //webView.PropertyChanged += webView_PropertyChanged;
            //webView.LoadCompleted += webView_LoadCompleted;

            //if (!WebCore.IsRunning)
            //{
            //    WebCore.Initialize(
            //       new WebConfig()
            //       {
            //           LogLevel = LogLevel.Verbose,
            //       });
            //}

            Presenter = this.DataContext as InputFormViewModel;
            //webControl.Loaded += webControl_Loaded;
            //webControl.DocumentReady += webControl_DocumentReady;
            //webControl.ViewType = WebViewType.Window;


            //webControl.SelectAll();
            //webControl.Copy();
            //webControl.SelectionChanged += webControl_SelectionChanged;
            //webControl.SelectionComplete += webControl_SelectionComplete;

            //webControl.Source = @"http://www.google.com".ToUri();
            //webControl.Visibility = System.Windows.Visibility.Visible;

            //webControl.of
            //var view = webControl as WebView;
        }

        void webView_LoadCompleted(object sender, LoadCompletedEventArgs url)
        {
            //webView.ShowDevTools();
        }

        void webView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
    }
}
