using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverymangementsystem.Models
{
    public class RouteTrackModel
    {
         public decimal sr_no { get; set; }

        public DateTime route_date { get; set; }
        public string route { get; set; }
        public string driver_name { get; set; }
        public string level { get; set; }
        public string package { get; set; }
        public string stops { get; set; }
        public string packege1st { get; set; }
        public string lastpackage { get; set; }
        public string duration { get; set; }
        public string SPH { get; set; }
        public string unknowstops { get; set; }
        public string totalunknowstop { get; set; }
        public string adp_in { get; set; }
        public string adp_out { get; set; }
        public string unpaidbrak { get; set; }
        public string calculatetime { get; set; }
        public string donotdeletecol { get; set; }
        public string flexlogin { get; set; }
        public string flexout { get; set; }
        public string van_number { get; set; }
    }
}