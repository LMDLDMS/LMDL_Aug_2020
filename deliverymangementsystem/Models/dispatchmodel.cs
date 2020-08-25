using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverymangementsystem.Models
{
    public class dispatchmodel
    {

        public string back_res { get; set; }
        public DateTime dispatchdate { get; set; }
        public string routes { get; set; }
        public string van_number { get; set; }
        public int sr_no { get; set; }
        public int bages { get; set; }
        public string vantype { get; set; }
        public string route { get; set; }
        public string driverName { get; set; }
        public string cat { get; set; }
        public string package { get; set; }
        public string Stop { get; set; }
        public string level { get; set; }
        public string rideralong { get; set; }
        public string notes { get; set; }
        public string phone2 { get; set; }

        public string IsCheckedIN { get; set; }
    }
}