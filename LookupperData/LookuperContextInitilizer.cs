using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookupperData
{
    public class LookuperContextInitilizer : CreateDatabaseIfNotExists<LookuperContext>
    {
        public LookuperContextInitilizer()
        {
            var db = new LookuperContext();

            this.Seed(db);
        }
        protected override void Seed(LookuperContext context)
        {
            SQLiteConnection.CreateFile("lookuper.sqlite");
            string createTableQuery = @"CREATE TABLE IF NOT EXISTS [LookuperItem] (
                          [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [Name] TEXT  NOT NULL,
                          [AddressUrl] TEXT NOT NULL,
                          [IsActive] INTEGER NOT NULL,
                          [UpdateAvaliable] INTEGER NOT NULL,
                          [Data] TEXT  NULL,
                          [CreatedDate] TEXT NOT  NULL,
                          [CheckInterval] TEXT NOT NULL,
                          [Comparer] TEXT NOT NULL,
                          [XPath] TEXT NULL
                          )";

            using (var con = new System.Data.SQLite.SQLiteConnection("data source=lookuper.sqlite"))
            {
                using (var com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();                             // Open the connection to the database

                    com.CommandText = createTableQuery;     // Set CommandText to our query that will create the table
                    com.ExecuteNonQuery();                  // Execute the query

                    con.Close();        // Close the connection to the database
                }
            }

            var firstItem = new LookuperItem
            {
                Name = "Google",
                IsActive = true,
                CreatedDate = DateTime.Now,
                AddressUrl = @"http://www.google.com",
                CheckInterval = TimeSpan.FromMinutes(1),
                Comparer = "Default",
            };

            var secondItem = new LookuperItem
            {
                Name = "Slando lookup",
                IsActive = false,
                CreatedDate = DateTime.Now,
                AddressUrl = @"http://kiev.ko.slando.ua/",
                CheckInterval = TimeSpan.FromMinutes(1),
                Comparer = "Default",
            };

            context.LookuperItems.Add(firstItem);
            context.LookuperItems.Add(secondItem);

            context.SaveChanges();
        }
    }
}
