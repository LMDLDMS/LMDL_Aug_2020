using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverymangementsystem.Models
{
    [Serializable]
    public class modelDuplucateDrivers
    {
        public string ApplicantName { get; set; }
        public string ApplicantNumber { get; set; }
        public string ApplicantStatus { get; set; }
    }
}