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
    
    public partial class tblticketconveration
    {
        public decimal sr_no { get; set; }
        public Nullable<decimal> ticket_id { get; set; }
        public string ticket_conversation { get; set; }
        public Nullable<decimal> from_user_id { get; set; }
        public Nullable<decimal> to_user_id { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<System.TimeSpan> time { get; set; }
        public string ticket_status { get; set; }
    }
}
