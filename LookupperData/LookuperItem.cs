using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookupperData
{
    public class LookuperItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string AddressUrl { get; set; }
        public bool IsActive { get; set; }
        public bool UpdateAvaliable { get; set; }
        public string Data { get; set; }
        public string createdDate { get; set; }
        public string checkInterval { get; set; }
        public string Comparer { get; set; }
        public string XPath { get; set; }

        [NotMapped]
        public DateTime CreatedDate
        {
            get { return DateTime.Parse(createdDate); }
            set { createdDate = value.ToString(); }
        }
        [NotMapped]
        public TimeSpan CheckInterval
        {
            get { return TimeSpan.Parse(checkInterval); }
            set { checkInterval = value.ToString(); }
        }
    }
}
