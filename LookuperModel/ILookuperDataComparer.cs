using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookuperModel
{
    public interface ILookuperDataComparer
    {
        string OldText { get; }
        string NewText { get; }

        string HtmlDiff { get; }
        string CustomNode { get; set; }

        DataComparsionResultEnum Compare();
    }
}
