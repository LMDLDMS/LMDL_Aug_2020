using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverymangementsystem.Models
{
    public class jobofferlistModel
    {
        public int emp_id { get; set; }

        public int d_id { get; set; }

        public string d_name { get; set; }

        public string d_email { get; set; }

        public string d_phone { get; set; }

        public bool isemailid { get; set; }

        public bool isbackgroundcheck { get; set; }
        public bool isonbordinglink { get; set; }
        public bool isdrugtest { get; set; }

        public bool isADPMVR { get; set; }
    }
}