using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverymangementsystem.Models
{
    public class ScheduleTraningModel
    {
        public string d_name { get; set; }
        public string d_number { get; set; }
        public decimal traning_id { get; set; }
        public Nullable<decimal> d_id { get; set; }
        public Nullable<System.DateTime> training_date { get; set; }
        public Nullable<System.TimeSpan> training_time { get; set; }
        public Nullable<decimal> user_id { get; set; }
        public Nullable<System.DateTime> entry_date { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> inclassdateone { get; set; }
        public Nullable<System.DateTime> inclassdatetwo { get; set; }
        public Nullable<System.DateTime> inclassridedate { get; set; }
        public string ridealongname { get; set; }


        public Nullable<System.DateTime> ridealongdate { get; set; }
        public int emp_id { get; set; }
        public string training_type { get; set; }
        public string inClassTimeTwo { get; set; }
        public Nullable<System.DateTime> VirtualDateOne { get; set; }
        public string VirtualTime { get; set; }
        public string RideAlongTrainee { get; set; }
    }
}