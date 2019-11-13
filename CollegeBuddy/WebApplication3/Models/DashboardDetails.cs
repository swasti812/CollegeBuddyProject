using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class DashboardDetails
    {
        public int QID { get; set; }
        public string Question { get; set; }
        public System.DateTime Datetime { get; set; }
        public string ID { get; set; }
        public int AID { get; set; }
        public string Answer { get; set; }

    }
}