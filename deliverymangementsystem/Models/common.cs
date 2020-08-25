using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using deliverymangementsystem.EDM;

namespace deliverymangementsystem.Models
{
    public class common
    {
        //dsdatabaseEntities dc = new dsdatabaseEntities();
        dsdatabaseEntities dc = new dsdatabaseEntities();
        public  string show(TimeSpan timeSpan)
        {
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            var amPmDesignator = "AM";
            if (hours == 0)
                hours = 12;
            else if (hours == 12)
                amPmDesignator = "PM";
            else if (hours > 12)
            {
                hours -= 12;
                amPmDesignator = "PM";
            }
            var formattedTime =
              String.Format("{0}:{1:00} {2}", hours, minutes, amPmDesignator);

            return formattedTime;
        }
     
        public List<SelectListItem> getselected(string drivername)
        {
            List<SelectListItem> driverlist = new List<SelectListItem>();
            var objlist = dc.tblfinaldrivermasters.ToList();
            foreach (var item in objlist)
            {
                SelectListItem obj = new SelectListItem();
                if (item.final_driver_name == drivername)
                {
                    obj.Text = item.final_driver_name;
                    obj.Value = item.final_driver_name.ToString();
                    obj.Selected = true;
                }
                else
                {
                    obj.Text = item.final_driver_name;
                    obj.Value = item.final_driver_name.ToString();
                    obj.Selected = false;
                }
                driverlist.Add(obj);
            }

            return driverlist;
        }
    }

    

    public class ListtoDataTable
    {
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}