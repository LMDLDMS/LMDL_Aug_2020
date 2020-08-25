using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverymangementsystem.Models
{
    public class AssignedJoin
    {
        public int assign_id { get; set; }

        public string module_name { get; set; }

        public string sub_admin_id { get; set; }
        

    }
    public class degilist
    {

        public string degi_name { get; set; }
        public bool  degi_checked { get; set; }
        

    }
}