using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookuperModel
{
    public class SimpleDataComparer : ILookuperDataComparer
    {
        public string OldText { get; private set; }
        public string NewText { get; private set; }

        public string HtmlDiff { get { throw new NotImplementedException(); } }
        public string CustomNode { get; set; }

        public SimpleDataComparer(string oldText, string newText)
        {
            OldText = oldText;
            NewText = newText;
        }

        public DataComparsionResultEnum Compare()
        {
            if (OldText == null)
                return DataComparsionResultEnum.None;

            if (NewText == null)
                throw new ArgumentNullException("NewText");

            if (String.Equals(OldText, NewText, StringComparison.OrdinalIgnoreCase))
                return DataComparsionResultEnum.NoChanges;
            else
                return DataComparsionResultEnum.ChangesAvaliable;
        }
    }
}
