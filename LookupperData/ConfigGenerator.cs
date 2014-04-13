using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookupperData
{
    class ConfigGenerator
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new LookuperContextInitilizer());
            var db = new LookuperContext();
            db.Database.Initialize(true);
        }
    }
}
