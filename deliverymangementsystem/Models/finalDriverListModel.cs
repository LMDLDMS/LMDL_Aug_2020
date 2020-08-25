using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverymangementsystem.Models
{
    public class finalDriverListModel
    {
        public decimal final_driver_id { get; set; }
        public string final_driver_name { get; set; }
        public string final_driver_personal_email { get; set; }
        public string final_driver_company_email { get; set; }
        public string personal_mobile_no { get; set; }
        public string company_mobile_no { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public string driver_license_no { get; set; }
        public string driver_license_state { get; set; }
        public string driver_full_address { get; set; }
        public string driver_ssn { get; set; }
        public Nullable<System.DateTime> job_offer_date { get; set; }
        public Nullable<System.DateTime> join_date { get; set; }
        public string driver_adp_id { get; set; }
        public string driver_prohibition_period { get; set; }
        public string driver_license_photo { get; set; }
        public string driver_profile_pic { get; set; }
        public string verified_by_previous_if_yes { get; set; }
        public string total_exp { get; set; }
        public Nullable<System.DateTime> traning_date_one { get; set; }
        public Nullable<System.DateTime> traning_date_two { get; set; }
        public Nullable<System.DateTime> ride_along_date { get; set; }
        public string ride_along_driver_name { get; set; }
        public Nullable<decimal> d_id { get; set; }
        public string driver_status { get; set; }
        public string transport_id { get; set; }
    }
}