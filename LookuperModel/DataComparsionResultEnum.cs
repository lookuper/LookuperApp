using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookuperModel
{
    public enum DataComparsionResultEnum
    {
        None = 0,
        NoChanges,
        ChangesAvaliable,
        Error, // mostly error in parsing xpath
    }
}
