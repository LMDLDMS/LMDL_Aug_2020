using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverymangementsystem.Models
{
    public class Assignauth
    {
        public int assign_id { get; set; }
        public string module_id { get; set; }
        public Nullable<int> sub_admin_id { get; set; }
        public Nullable<System.DateTime> entry_date { get; set; }
        public Nullable<int> entry_user_id { get; set; }
        public List<ModuleCheck> listmodule { get; set; }

    }
}