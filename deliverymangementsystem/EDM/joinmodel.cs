using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverymangementsystem.EDM
{

    public partial class tblticket
    {
        public string assignedusername { get; set; }
        
    }
    public partial class tblticketconveration
    {

        public string fromusername { get; set; }
        public string tousername { get; set; }
       


    }

    public class joinmodel
    {
    }
}