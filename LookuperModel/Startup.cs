using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookuperModel
{
    class Startup
    {
        static void Main(string[] args)
        {
            var model = new LookuperModel();
            var data = model.AvaliableLookupItems;
        }
    }
}
