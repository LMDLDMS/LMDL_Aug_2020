//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace deliverymangementsystem.EDM
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblterminate
    {
        public decimal termination_id { get; set; }
        public Nullable<decimal> final_driver_id { get; set; }
        public Nullable<System.DateTime> termination_date { get; set; }
        public Nullable<System.DateTime> last_day_wrok { get; set; }
        public Nullable<System.DateTime> last_day_terminate { get; set; }
        public string eligibal_for_hire { get; set; }
        public string isvalountry { get; set; }
        public string resion { get; set; }
        public string account_deactive { get; set; }
        public string comment { get; set; }
    }
}