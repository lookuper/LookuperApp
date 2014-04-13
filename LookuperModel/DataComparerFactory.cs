using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookuperModel
{
    public class DataComparerFactory //: ILookuperComparer
    {
        public string OldData { get; private set; }
        public string NewData { get; private set; }
        public string XPath { get; private set; }

        public DataComparerFactory(string oldData, string newData, string xpath)
        {
            OldData = oldData;
            NewData = newData;
            XPath = xpath;
        }

        public static IList<String> Mappings
        {
            get
            {
                return new List<String>()
                {
                    "Default",
                    "DiffPlex",
                    "DiffMatch",
                    "HtmlDiff",
                    "CustomDataComparer" // "XPath Comparer
                };
            }
        }

        public ILookuperDataComparer GetDataComparer(string comparerName)
        {
            switch (comparerName)
            {
                case "Default":
                    return new SimpleDataComparer(OldData, NewData);
                //case "DiffPlex":
                //    return new DiffPlexDataComparer(OldData, NewData);
                //case "DiffMatch":
                //    return new DiffMatchDataComparer(OldData, NewData);
                //case "HtmlDiff":
                //    return new HtmlDiffDataComparer(OldData, NewData);
                case "CustomDataComparer":
                    return new CustomDataComparer(OldData, NewData, XPath);
                default:
                    return new SimpleDataComparer(OldData, NewData);
            }
        }
    }

}
