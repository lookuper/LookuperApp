using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CefSharp.Wpf;

namespace LookuperView
{
    public class WebBrowserUtil
    {
        //local:WebBrowserUtil.BindableSource="{Binding FormItem.AddressUrl}"
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource",
                                                typeof(String),
                                                typeof(WebBrowserUtil),
                                                new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        //local:WebBrowserUtil.CurrentHtml="{Binding CurrentHtml}"
        //public readonly DependencyProperty CurrentHtmlProperty =
        //    DependencyProperty.RegisterAttached("CurrentHtml",
        //                                typeof(String),
        //                                typeof(WebBrowserUtil),
        //                                new UIPropertyMetadata(null, CurrentHtmlPropertyChanged));

        //public String GetCurrentHtmlProperty(DependencyObject obj)
        //{
        //    return obj.GetValue(CurrentHtmlProperty) as String;
        //}

        //public void SetCurrentHtmlProperty(DependencyObject obj, string value)
        //{
        //    obj.SetValue(CurrentHtmlProperty, value);
        //}

        public static String GetBindableSource(DependencyObject obj)
        {
            return obj.GetValue(BindableSourceProperty) as String;
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        private static void BindableSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var contentPresenter = d as ContentPresenter;

            if (contentPresenter == null)
                return;

            var webBrowser = contentPresenter.Content as WebView;

            if (webBrowser != null)
            {
                //HideScriptErrors(webBrowser, true);
                string url = e.NewValue as string;

                if (String.IsNullOrEmpty(url))
                    return;

                if (String.IsNullOrEmpty(webBrowser.Address))
                {
                    webBrowser.Address = url;

                    if (webBrowser.IsLoaded)
                        webBrowser.Load(webBrowser.Address);
                }
                else
                {
                    if (webBrowser.Address.Equals(url, StringComparison.OrdinalIgnoreCase))
                        return;

                    webBrowser.Address = url;

                    if (webBrowser.IsLoaded)
                        webBrowser.Load(webBrowser.Address);
                }

                //inject xpath selector here
            }
        }

        private void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);

            if (fiComWebBrowser == null)
                return;

            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }
    }
}
