using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace LookuperModel
{
    public class CustomDataComparer : ILookuperDataComparer
    {
        public string OldText { get; private set; }
        public string NewText { get; private set; }

        public string HtmlDiff { get { throw new NotImplementedException(); } }
        public string CustomNode { get; set; }

        public CustomDataComparer(string dataOld, string dataNew)
        {
            OldText = dataOld;
            NewText = dataNew;
        }

        public CustomDataComparer(string dataOld, string dataNew, string xpath)
        {
            OldText = dataOld;
            NewText = dataNew;
            CustomNode = xpath;
        }

        public DataComparsionResultEnum Compare()
        {
            if (OldText == null)
                return DataComparsionResultEnum.None;

            var oldDoc = new HtmlDocument() { OptionFixNestedTags = true, };
            oldDoc.LoadHtml(OldText);

            var newDoc = new HtmlDocument() { OptionFixNestedTags = true, };
            newDoc.LoadHtml(NewText);

            //if (oldDoc.ParseErrors != null && oldDoc.ParseErrors.Count() != 0)
            //{
            //    throw new NotImplementedException();
            //}

            //if (newDoc.ParseErrors != null && newDoc.ParseErrors.Count() != 0)
            //{
            //    throw new NotImplementedException();
            //}

            var oldData = oldDoc.DocumentNode.SelectNodes(CustomNode); // null
            var newData = newDoc.DocumentNode.SelectNodes(CustomNode);

            if (oldData == null)
                return DataComparsionResultEnum.None;

            if (newData == null)
                return DataComparsionResultEnum.Error;

            string oldString = oldData.FirstOrDefault().InnerText;
            string newString = newData.FirstOrDefault().InnerText;

            if (oldString.Equals(newString, StringComparison.OrdinalIgnoreCase))
                return DataComparsionResultEnum.NoChanges;
            else
                return DataComparsionResultEnum.ChangesAvaliable;
        }

        public static string GetHtml(string htmlPage, string xpath)
        {
            var doc = new HtmlDocument() { OptionFixNestedTags = true, };
            doc.LoadHtml(htmlPage);

            string data = doc.DocumentNode
                .SelectNodes(xpath)
                .FirstOrDefault()
                .InnerHtml;

            return data;
        }

        public static string GetText(string url, string xpath)
        {
            HtmlNodeCollection xpathNodes;
            string htmlPage;
            var client = DownloadDataFactory.Instance.WebClient;

            try
            {
                htmlPage = client.DownloadString(url);

                //var w = WebCore.CreateWebView(640, 480);
                //w.Source = new Uri(url);

                //w.DocumentReady += (s, e) =>
                //{
                //    dynamic document = (JSObject)w.ExecuteJavascriptWithResult("document");

                //    if (document == null)
                //        return;
                //};
                //w.ShowJavascriptDialog += (Object s, JavascriptDialogEventArgs e) =>
                //{
                //    e.Cancel = false;
                //    e.Handled = true;

                //    bool buttons;

                //    if ((e.DialogFlags & JSDialogFlags.HasCancelButton) != 0)
                //        buttons = true;

                //    string caption = (e.DialogFlags & JSDialogFlags.HasPromptField) != 0 ?
                //                      e.Prompt :
                //                      e.DefaultPrompt;

                //    if (string.IsNullOrEmpty(caption))
                //        caption = "My Prompt's Title Goes Here :)";
                //};

                //while (w.IsLoading)
                //{
                //    WebCore.Update();
                //}
            }
            catch (WebException ex)
            {
                return ex.Message;
            }

            var doc = new HtmlDocument() { OptionFixNestedTags = true };
            doc.LoadHtml(htmlPage);

            try
            {
                xpathNodes = doc.DocumentNode.SelectNodes(xpath);
            }
            catch (XPathException ex)
            {
                return ex.Message;
            }

            if (xpathNodes == null || xpathNodes.Count == 0)
            {
                //check for http responce
                return "Incorect XPath. Maybe Authorization problem.";
            }

            string data = doc.DocumentNode
                .SelectNodes(xpath)
                .FirstOrDefault()
                .InnerText;

            return data;
        }
    }
}
