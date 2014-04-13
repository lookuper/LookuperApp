using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookuperModel
{
    public static class DtoExtensions
    {
        public static void ResetCountdownTime(this LookupItemDto item)
        {
            item.TimeUntilUpdate = item.CheckInterval;
        }
    }
}
