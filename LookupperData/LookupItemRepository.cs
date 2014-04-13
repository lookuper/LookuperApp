using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookupperData
{
    public class LookupItemRepository : SqliteReposiroty<LookuperItem>
    {
        public LookupItemRepository()
            : base(new LookuperContext())
        {

        }
    }
}
