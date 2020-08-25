using deliverymangementsystem.EDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Drawing;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp;
using System.IO.Compression;
using PdfSharp.Pdf.IO;
using deliverymangementsystem.Models;
using System.Net.Mail;
using System.Net;
using System.Text;


//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Graphics;


namespace deliverymangementsystem.Controllers
{
    public class PerformansController : Controller
    {
        //dsdatabaseEntities dc = new dsdatabaseEntities();
        dsdatabaseEntities dc = new dsdatabaseEntities();
        private DateTime time;
        public string TrackingID = "";
        public string DriverName = "";
        // GET: Performans
        public ActionResult Index()
        {
            return View();
        }

        public void resetdata(DateTime startdate, DateTime enddte)
        {
            var fromdate = startdate;
            var enddate = enddte;
            var table1 = dc.tblperfromense1.Where(p => p.fromdate == fromdate && p.todate == enddate).FirstOrDefault();
            var table2 = dc.tblperformence2.Where(p => p.fromdate == fromdate && p.todate == enddate).FirstOrDefault();
            dc.tblperfromense1.RemoveRange(dc.tblperfromense1.Where(p => p.fromdate == fromdate && p.todate == enddate));
            dc.SaveChanges();
            dc.tblperformence2.RemoveRange(dc.tblperformence2.Where(p => p.fromdate == fromdate && p.todate == enddate));
            dc.SaveChanges();



        }


        [HttpPost]
        public ActionResult Index(FormCollection fc, HttpPostedFileBase table1, HttpPostedFileBase table2, string btnname)
        {

            //var name = fc["re"];
            //if(btnname == "Create")
            //{
            if (table1 != null)
            {


                var fromdate = DateTime.Parse(fc["start"]);
                var enddate = DateTime.Parse(fc["end"]);

                //if (postedFile.ContentType == "application/octet-stream")
                //{

                string filePath = string.Empty;
                if (table1 != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(table1.FileName);
                    string extension = Path.GetExtension(table1.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        table1.SaveAs(filePath);
                    }
                    else
                    {
                        ViewBag.msg = "File already uploaded";
                        // uploadafter(selecteddate);

                        return View();
                    }
                    //Create a DataTable.
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[19] {
                                new DataColumn("transport_id", typeof(string)),

                                 new DataColumn("rank",typeof(string)),
                                  new DataColumn("name",typeof(string)),
                                  new DataColumn("overalltier",typeof(string)),
                                  new DataColumn("delivered",typeof(string))
                                   ,

                                    new DataColumn("dcr",typeof(string)),
                                     new DataColumn("key_focus_area",typeof(string)),
                                    new DataColumn("dar",typeof(string)),
                                    new DataColumn("swc_pod",typeof(string)),
                                     new DataColumn("swc_cc",typeof(string)),
                                      new DataColumn("swc_sc",typeof(string)),
                                       new DataColumn("swc_ad",typeof(string)),
                                       new DataColumn("blank",typeof(string)),
                                        new DataColumn("dnrs",typeof(string)),
                                         new DataColumn("pod_oops",typeof(string)),
                    new DataColumn("cc_oops",typeof(string)),
                    new DataColumn("fromdate",typeof(string)),
                    new DataColumn("todate",typeof(string)),
                    new DataColumn("remarks",typeof(string))});



                    //Read the contents of CSV file.
                    string csvData = System.IO.File.ReadAllText(filePath);

                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n'))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                dt.Rows.Add();
                                int i = 0;

                                try
                                {
                                    foreach (string cell in row.Split(','))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = cell;
                                        i++;
                                    }

                                }
                                catch (Exception ex)
                                {

                                }
                                //Execute a loop over the columns.

                            }
                        }
                        catch (Exception ex)
                        {

                            ViewBag.msg = "File formate is not proper according to system";
                            // return View();
                        }

                    }

                    if (dt.Rows.Count > 0)
                    {

                    }

                    string conString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(conString))
                    {

                        int sss = 0;
                        for (int i = 7; i < dt.Rows.Count; i++)
                        {
                            sss++;
                            try
                            {

                                string transport_id = "";
                                if (dt.Rows[i]["transport_id"] != null)
                                {
                                    transport_id = (dt.Rows[i]["transport_id"].ToString().Trim());

                                }
                                else
                                {
                                    transport_id = "";
                                }
                                string rank = "";
                                if (dt.Rows[i]["rank"] != null)
                                {
                                    rank = (dt.Rows[i]["rank"].ToString().Trim());

                                }
                                else
                                {
                                    rank = "";
                                }

                                string overalltier = "";
                                if (dt.Rows[i]["overalltier"] != null)
                                {
                                    overalltier = (dt.Rows[i]["overalltier"].ToString().Trim());

                                }
                                else
                                {
                                    overalltier = "";
                                }


                                string delivered = "";
                                if (dt.Rows[i]["delivered"] != null)
                                {
                                    delivered = (dt.Rows[i]["delivered"].ToString().Trim());

                                }
                                else
                                {
                                    delivered = "";
                                }


                                string dcr = "";
                                if (dt.Rows[i]["dcr"] != null)
                                {
                                    dcr = (dt.Rows[i]["dcr"].ToString().Trim());

                                }
                                else
                                {
                                    dcr = "";
                                }

                                string dar = "";
                                if (dt.Rows[i]["dar"] != null)
                                {
                                    dar = (dt.Rows[i]["dar"].ToString().Trim());

                                }
                                else
                                {
                                    dar = "";
                                }


                                string swc_pod = "";
                                if (dt.Rows[i]["swc_pod"] != null)
                                {
                                    swc_pod = (dt.Rows[i]["swc_pod"].ToString().Trim());

                                }
                                else
                                {
                                    swc_pod = "";
                                }


                                string swc_cc = "";

                                if (dt.Rows[i]["swc_cc"] != null)
                                {
                                    swc_cc = (dt.Rows[i]["swc_cc"].ToString().Trim());

                                }
                                else
                                {
                                    swc_cc = "";

                                }



                                string swc_sc = "";
                                if (dt.Rows[i]["swc_sc"] != null)
                                {
                                    swc_sc = (dt.Rows[i]["swc_sc"].ToString().Trim());

                                }
                                else
                                {
                                    swc_sc = "";
                                }

                                string swc_ad = "";
                                if (dt.Rows[i]["swc_ad"] != null)
                                {

                                    swc_ad = (dt.Rows[i]["swc_ad"].ToString().Trim());

                                }
                                else
                                {
                                    swc_ad = "";

                                }
                                string dnrs = "";
                                if (dt.Rows[i]["dnrs"] != null)
                                {

                                    dnrs = (dt.Rows[i]["dnrs"].ToString().Trim());

                                }
                                else
                                {
                                    dnrs = "";

                                }

                                string pod_oops = "";
                                if (dt.Rows[i]["pod_oops"] != null)
                                {

                                    pod_oops = (dt.Rows[i]["pod_oops"].ToString().Trim());

                                }
                                else
                                {
                                    pod_oops = "";

                                }

                                string cc_oops = "";
                                if (dt.Rows[i]["cc_oops"] != null)
                                {

                                    cc_oops = (dt.Rows[i]["cc_oops"].ToString().Trim());

                                }
                                else
                                {
                                    cc_oops = "";

                                }
                                string key_focus_area = "";
                                if (dt.Rows[i]["key_focus_area"] != null)
                                {

                                    key_focus_area = (dt.Rows[i]["key_focus_area"].ToString().Trim());

                                }
                                else
                                {
                                    key_focus_area = "";

                                }

                                string remarks = "";
                                if (dt.Rows[i]["remarks"] != null)
                                {

                                    remarks = (dt.Rows[i]["remarks"].ToString().Trim());

                                }
                                else
                                {
                                    remarks = "";

                                }

                                var driver_name = (dt.Rows[i]["name"].ToString().Trim());
                                var tranport_id1 = (dt.Rows[i]["transport_id"].ToString().Trim());

                                //here we are updating trasport id if not or check 
                                updatetranstionID(driver_name, tranport_id1);


                                /* string bag = "";
                                 if (dt.Rows[i]["bag"] != null)
                                 {
                                     bag = (dt.Rows[i]["bag"].ToString().Trim());
                                 }
                                 else
                                 {
                                     bag = "";
                                 }
                                */
                                SqlCommand cmd = new SqlCommand();
                                cmd.CommandText = "insert into tblperfromense1(transport_id,rank,overalltier,delivered,dcr,dar,swc_pod,swc_cc,swc_sc,swc_ad,dnrs,pod_oops,cc_oops,fromdate,todate,remaks,key_focus_area)values('" + transport_id + "','" + rank + "','" + overalltier + "','" + delivered + "','" + dcr + "','" + dar + "','" + swc_pod + "','" + swc_cc + "','" + swc_sc + "','" + swc_ad + "','" + dnrs + "','" + pod_oops + "','" + cc_oops + "','" + fromdate + "','" + enddate + "','" + remarks + "','" + key_focus_area + "')";
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();

                            }
                            catch (Exception ex)
                            {
                                ViewBag.msg = "File formate is not proper according to system";

                                // return View();
                            }
                        }

                    }



                }

            }
            if (table2 != null)
            {


                var fromdate = DateTime.Parse(fc["start"]);
                var enddate = DateTime.Parse(fc["end"]);

                //if (postedFile.ContentType == "application/octet-stream")
                //{

                string filePath = string.Empty;
                if (table2 != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(table2.FileName);
                    string extension = Path.GetExtension(table2.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        table2.SaveAs(filePath);
                    }
                    else
                    {
                        ViewBag.msg = "File already uploaded";
                        // uploadafter(selecteddate);

                        return View();
                    }
                    //Create a DataTable.
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[17] {
                                new DataColumn("transport_id", typeof(string)),

                                 new DataColumn("rank",typeof(string)),
                                 new DataColumn("name",typeof(string)),
                                  new DataColumn("average_tier",typeof(string)),
                                  new DataColumn("dcr",typeof(string)),
                                  new DataColumn("dar",typeof(string))
                                   ,

                                    new DataColumn("swc_pod",typeof(string)),
                                     new DataColumn("swc_cc",typeof(string)),
                                    new DataColumn("swc_sc",typeof(string)),
                                    new DataColumn("swc_ad",typeof(string)),
                                     new DataColumn("per_status",typeof(string)),
                                     new DataColumn("blank",typeof(string)),
                                      new DataColumn("fant",typeof(string)),
                                       new DataColumn("great",typeof(string)),
                                       new DataColumn("fair",typeof(string)),
                                        new DataColumn("poor",typeof(string)),
                                         new DataColumn("remarks",typeof(string))

                   });



                    //Read the contents of CSV file.
                    string csvData = System.IO.File.ReadAllText(filePath);

                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n'))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                dt.Rows.Add();
                                int i = 0;

                                try
                                {
                                    foreach (string cell in row.Split(','))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = cell;
                                        i++;
                                    }

                                }
                                catch (Exception ex)
                                {

                                }
                                //Execute a loop over the columns.

                            }
                        }
                        catch (Exception ex)
                        {

                            ViewBag.msg = "File formate is not proper according to system";
                            return View();
                        }

                    }


                    if (dt.Rows.Count > 0)
                    {

                    }

                    string conString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(conString))
                    {

                        int sss = 0;
                        for (int i = 8; i < dt.Rows.Count; i++)
                        {
                            sss++;
                            try
                            {

                                string transport_id = "";
                                if (dt.Rows[i]["transport_id"] != null)
                                {
                                    transport_id = (dt.Rows[i]["transport_id"].ToString().Trim());

                                }
                                else
                                {
                                    transport_id = "";
                                }
                                string rank = "";
                                if (dt.Rows[i]["rank"] != null)
                                {
                                    rank = (dt.Rows[i]["rank"].ToString().Trim());

                                }
                                else
                                {
                                    rank = "";
                                }

                                string average_tier = "";
                                if (dt.Rows[i]["average_tier"] != null)
                                {
                                    average_tier = (dt.Rows[i]["average_tier"].ToString().Trim());

                                }
                                else
                                {
                                    average_tier = "";
                                }


                                string dcr = "";
                                if (dt.Rows[i]["dcr"] != null)
                                {
                                    dcr = (dt.Rows[i]["dcr"].ToString().Trim());

                                }
                                else
                                {
                                    dcr = "";
                                }


                                string dar = "";
                                if (dt.Rows[i]["dar"] != null)
                                {
                                    dar = (dt.Rows[i]["dar"].ToString().Trim());

                                }
                                else
                                {
                                    dar = "";
                                }

                                string swc_pod = "";
                                if (dt.Rows[i]["swc_pod"] != null)
                                {
                                    swc_pod = (dt.Rows[i]["swc_pod"].ToString().Trim());

                                }
                                else
                                {
                                    swc_pod = "";
                                }


                                string swc_cc = "";
                                if (dt.Rows[i]["swc_cc"] != null)
                                {
                                    swc_cc = (dt.Rows[i]["swc_cc"].ToString().Trim());

                                }
                                else
                                {
                                    swc_cc = "";
                                }






                                string swc_sc = "";
                                if (dt.Rows[i]["swc_sc"] != null)
                                {
                                    swc_sc = (dt.Rows[i]["swc_sc"].ToString().Trim());

                                }
                                else
                                {
                                    swc_sc = "";
                                }

                                string swc_ad = "";
                                if (dt.Rows[i]["swc_ad"] != null)
                                {

                                    swc_ad = (dt.Rows[i]["swc_ad"].ToString().Trim());

                                }
                                else
                                {
                                    swc_ad = "";

                                }
                                string per_status = "";
                                if (dt.Rows[i]["per_status"] != null)
                                {

                                    per_status = (dt.Rows[i]["per_status"].ToString().Trim());

                                }
                                else
                                {
                                    per_status = "";

                                }

                                string fant = "";
                                if (dt.Rows[i]["fant"] != null)
                                {

                                    fant = (dt.Rows[i]["fant"].ToString().Trim());

                                }
                                else
                                {
                                    fant = "";

                                }

                                string great = "";
                                if (dt.Rows[i]["great"] != null)
                                {

                                    great = (dt.Rows[i]["great"].ToString().Trim());

                                }
                                else
                                {
                                    great = "";

                                }
                                string fair = "";
                                if (dt.Rows[i]["fair"] != null)
                                {

                                    fair = (dt.Rows[i]["fair"].ToString().Trim());

                                }
                                else
                                {
                                    fair = "";

                                }

                                string poor = "";
                                if (dt.Rows[i]["poor"] != null)
                                {

                                    poor = (dt.Rows[i]["poor"].ToString().Trim());

                                }
                                else
                                {
                                    poor = "";

                                }
                                //remarks
                                string remarks = "";
                                if (dt.Rows[i]["remarks"] != null)
                                {

                                    remarks = (dt.Rows[i]["remarks"].ToString().Trim());

                                }
                                else
                                {
                                    remarks = "";

                                }



                                SqlCommand cmd = new SqlCommand();
                                cmd.CommandText = "insert into tblperformence2(transport_id,rank,average_tier,dcr,dar,swc_pod,swc_cc,swc_sc,swc_ad,per_status,fant,great,fair,poor,fromdate,todate,remakes)values('" + transport_id + "','" + rank + "','" + average_tier + "','" + dcr + "','" + dar + "','" + swc_pod + "','" + swc_cc + "','" + swc_sc + "','" + swc_ad + "','" + per_status + "','" + fant + "','" + great + "','" + fair + "','" + poor + "','" + fromdate + "','" + enddate + "','" + remarks + "')";
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();

                            }
                            catch (Exception ex)
                            {
                                ViewBag.msg = "File formate is not proper according to system";

                                return View();
                            }
                        }

                    }
                    ViewBag.msg = "CSV file Uploaded Successfully..";


                    return View();

                }
                //}
                //else
                //{
                //    ViewBag.msg = "Only uplaod csv file";
                //}



            }
            //}
            //else
            //{
            //    var fromdate = DateTime.Parse(fc["start"]);
            //    var enddate = DateTime.Parse(fc["end"]);
            //    resetdata(fromdate, enddate);

            //    return RedirectToAction("Index");
            //}

            return View();

        }

        public void updatetranstionID(string driver_name, string transport_id)
        {

            var fdriver_name = driver_name.Trim();
            var ftranstionid = transport_id.Trim();
            var d = dc.tblfinaldrivermasters.FirstOrDefault(c => c.transport_id == transport_id) != null;
            if (d)
            {

            }
            else
            {

                var record = dc.tblfinaldrivermasters.FirstOrDefault(c => c.final_driver_name == driver_name) != null;

                if (record)
                {
                    var ss = dc.tblfinaldrivermasters.Where(c => c.final_driver_name == driver_name).FirstOrDefault();

                    ss.transport_id = transport_id;
                    dc.SaveChanges();
                }


            }

        }


        public void driverdata()
        {

            var driverlist = dc.tblfinaldrivermasters.Where(c => c.driver_status == "active").ToList();
            List<SelectListItem> objlist = new List<SelectListItem>();
            foreach (var item in driverlist)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.final_driver_name;
                obj.Value = item.transport_id;

                objlist.Add(obj);

            }
            ViewData["dlist"] = objlist;


        }

        public ActionResult dareport()
        {

            driverdata();

            return View();

        }
        [HttpPost]
        public ActionResult dareport(FormCollection fc)
        {
            var t_id = fc["t_id"];

            var fromdate = DateTime.Parse(fc["start"]);
            var enddate = DateTime.Parse(fc["end"]);

            Session["t_id"] = t_id;

            Session["fromdate"] = fromdate;

            Session["enddate"] = enddate;


            return RedirectToAction("DaPdf");
            // TempData["table1"] =




        }

        public string getweeknumber(DateTime start, DateTime end)
        {
            var d1 = start;
            var d2 = end;
            var currentCulture = CultureInfo.CurrentCulture;
            var weeks = new List<int>();

            for (var dt = d1; dt < d2; dt = dt.AddDays(1))
            {
                var weekNo = currentCulture.Calendar.GetWeekOfYear(
                                      dt,
                                      currentCulture.DateTimeFormat.CalendarWeekRule,
                                      currentCulture.DateTimeFormat.FirstDayOfWeek);
                if (!weeks.Contains(weekNo))
                    weeks.Add(weekNo);
            }
            return weeks[0].ToString();


        }


        public ActionResult DaPdf()
        {
            if (Session["t_id"] != null)
            {

                var t_id = Session["t_id"].ToString();
                var fromdate = DateTime.Parse(Session["fromdate"].ToString());
                var todate = DateTime.Parse(Session["enddate"].ToString());
                ViewBag.weekno = getweeknumber(fromdate, todate);

                gettable2andtable3data(t_id, fromdate, todate);

                return View();
            }

            return RedirectToAction("dareport");

        }

        public void gettable2andtable3data(string t_id, DateTime start_date, DateTime end_date)
        {
            var table1 = dc.tblperfromense1.Where(c => c.transport_id == t_id && c.fromdate == start_date && c.todate == end_date).FirstOrDefault();
            ViewData["tbl2list"] = table1;

            var table2 = dc.tblperformence2.Where(c => c.transport_id == t_id && c.fromdate == start_date && c.todate == end_date).FirstOrDefault();
            ViewData["tbl3list"] = table2;


        }


        //con CONCESSION

        public ActionResult daconcession(int? id)
        {

            if (id == 1)
            {
                var weekno = int.Parse(Session["weekno"].ToString());


                Getdata(weekno.ToString());
            }
            return View();

        }

        public void Getdata(string weekno)
        {
            var p = dc.tblconcessions.Where(c => c.week_no == weekno && c.tracking_id != null).ToList();
            ViewData["cnt"] = p.Count;
            ViewData["templist"] = p;
        }
        [HttpPost]
        public ActionResult daconcession(FormCollection fc, HttpPostedFileBase table1)
        {

            var fromdate = DateTime.Parse(fc["start"]);
            var enddate = DateTime.Parse(fc["end"]);



            var weekno = getweeknumber(fromdate, enddate);
            if (table1 != null)
            {





                //if (postedFile.ContentType == "application/octet-stream")
                //{

                string filePath = string.Empty;
                if (table1 != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(table1.FileName);
                    string extension = Path.GetExtension(table1.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        table1.SaveAs(filePath);
                    }
                    else
                    {
                        ViewBag.msg = "File already uploaded";
                        // uploadafter(selecteddate);

                        return View();
                    }
                    //Create a DataTable.
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[7] {
                                new DataColumn("tracking_id", typeof(string)),

                                 new DataColumn("sevvice_area",typeof(string)),
                                  new DataColumn("dsp",typeof(string)),
                                  new DataColumn("delivery_date",typeof(string)),
                                  new DataColumn("concession_date",typeof(string))
                                   ,

                                    new DataColumn("deliver_associate",typeof(string)),
                                     new DataColumn("deliver_associate_name",typeof(string)),
                                   });



                    //Read the contents of CSV file.
                    string csvData = System.IO.File.ReadAllText(filePath);

                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n'))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                dt.Rows.Add();
                                int i = 0;

                                try
                                {
                                    foreach (string cell in row.Split(','))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = cell;
                                        i++;
                                    }

                                }
                                catch (Exception ex)
                                {

                                }
                                //Execute a loop over the columns.

                            }
                        }
                        catch (Exception ex)
                        {

                            ViewBag.msg = "File formate is not proper according to system";
                            // return View();
                        }

                    }


                    if (dt.Rows.Count > 0)
                    {

                    }

                    string conString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(conString))
                    {

                        int sss = 0;
                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            sss++;
                            try
                            {

                                string tracking_id = "";
                                if (dt.Rows[i]["tracking_id"] != null)
                                {
                                    tracking_id = (dt.Rows[i]["tracking_id"].ToString().Trim());

                                }
                                else
                                {
                                    tracking_id = "";
                                }
                                string sevvice_area = "";
                                if (dt.Rows[i]["sevvice_area"] != null)
                                {
                                    sevvice_area = (dt.Rows[i]["sevvice_area"].ToString().Trim());

                                }
                                else
                                {
                                    sevvice_area = "";
                                }

                                string dsp = "";
                                if (dt.Rows[i]["dsp"] != null)
                                {
                                    dsp = (dt.Rows[i]["dsp"].ToString().Trim());

                                }
                                else
                                {
                                    dsp = "";
                                }


                                DateTime delivery_date = new DateTime();

                                if (dt.Rows[i]["delivery_date"] != null)
                                {
                                    delivery_date = DateTime.Parse(dt.Rows[i]["delivery_date"].ToString().Trim());

                                }
                                else
                                {

                                }


                                DateTime concession_date = new DateTime();
                                if (dt.Rows[i]["concession_date"] != null)
                                {
                                    concession_date = DateTime.Parse(dt.Rows[i]["concession_date"].ToString().Trim());

                                }


                                string deliver_associate = "";
                                if (dt.Rows[i]["deliver_associate"] != null)
                                {
                                    deliver_associate = (dt.Rows[i]["deliver_associate"].ToString().Trim());

                                }
                                else
                                {
                                    deliver_associate = "";
                                }


                                string deliver_associate_name = "";
                                if (dt.Rows[i]["deliver_associate_name"] != null)
                                {
                                    deliver_associate_name = (dt.Rows[i]["deliver_associate_name"].ToString().Trim());

                                }
                                else
                                {
                                    deliver_associate_name = "";
                                }


                                //here we are updating trasport id if not or check 
                                //updatetranstionID(driver_name, tranport_id1);


                                /* string bag = "";
                                 if (dt.Rows[i]["bag"] != null)
                                 {
                                     bag = (dt.Rows[i]["bag"].ToString().Trim());
                                 }
                                 else
                                 {
                                     bag = "";
                                 }
                                */

                                var driver_name = deliver_associate_name.Trim();

                                updatetranstionID(driver_name, deliver_associate);

                                var tackid = dc.tblconcessions.FirstOrDefault(c => c.tracking_id == tracking_id) != null;

                                if (tackid)
                                {


                                }
                                else
                                {

                                    SqlCommand cmd = new SqlCommand();
                                    cmd.CommandText = "insert into tblconcession(tracking_id,sevvice_area,dsp,delivery_date,concession_date,deliver_associate,deliver_associate_name,week_no,from_date,to_date)values('" + tracking_id + "','" + sevvice_area + "','" + dsp + "','" + delivery_date + "','" + concession_date + "','" + deliver_associate + "','" + deliver_associate_name + "','" + weekno + "','" + fromdate + "','" + enddate + "')";
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }




                            }
                            catch (Exception ex)
                            {
                                ViewBag.msg = "File formate is not proper according to system";

                                // return View();
                            }
                        }

                    }



                }

            }


            Session["weekno"] = weekno;

            Getdata(weekno);


            return View();
        }


        public string base64char(string path)
        {
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }


        }

        public ActionResult UploadImage(int id)
        {
            Session["con_id"] = id;

            return View();
        }
        [HttpPost]

        public ActionResult UploadImage(HttpPostedFileBase file1, HttpPostedFileBase file2, FormCollection fc)
        {

            if (file1 != null)
            {

                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = path + Path.GetFileName(file1.FileName);
                string extension = Path.GetExtension(file1.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    file1.SaveAs(filePath);
                }
                else
                {
                    ViewBag.msg = "File already uploaded";
                    // uploadafter(selecteddate);


                }

                var con_id = int.Parse(Session["con_id"].ToString());

                var conrecord = dc.tblconcessions.Find(con_id);

                conrecord.image_one = file1.FileName;
                dc.SaveChanges();



            }
            if (file2 != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = path + Path.GetFileName(file2.FileName);
                string extension = Path.GetExtension(file2.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    file2.SaveAs(filePath);
                }
                else
                {
                    ViewBag.msg = "File already uploaded";
                    // uploadafter(selecteddate);


                }
                var con_id = int.Parse(Session["con_id"].ToString());
                var conrecord = dc.tblconcessions.Find(con_id);
                conrecord.image_two = file2.FileName;
                dc.SaveChanges();
            }
            else if (fc["hdnImg2"] != null) // Added by ViBeS on 15/04/2020 for adding new functionality of deleting second image.
            {
                if (fc["hdnImg2"].ToString().Trim() == "")
                {
                    var con_id = int.Parse(Session["con_id"].ToString());
                    var conrecord = dc.tblconcessions.Find(con_id);
                    var fileName = conrecord.image_two;
                    conrecord.image_two = null;
                    dc.SaveChanges();

                    string path = Server.MapPath("~/Uploads/");
                    if (Directory.Exists(path))
                    {
                        string filePath = path + Path.GetFileName(fileName);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }


                }
            }

            Session["flg"] = "1";
            return RedirectToAction("daconcession", new { @id = 1 });

        }


        public void getdriverlist()
        {

            List<SelectListItem> objlist = new List<SelectListItem>();

            var p = dc.tblfinaldrivermasters.Where(c => c.driver_status == "active").OrderBy(c => c.final_driver_name).ToList();

            foreach (var item in p)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.final_driver_name;
                obj.Value = item.transport_id;
                objlist.Add(obj);

            }
            ViewData["drivelist"] = objlist;

        }

        [HttpGet]
        public JsonResult getfiledata(string t_id, string weekno)
        {

            var pp = dc.tblconcessions.Where(c => c.deliver_associate == t_id && c.week_no == weekno).ToList();

            foreach (var item in pp)
            {
                if (item.image_one != null)
                {
                    var fullpath = Server.MapPath("~/Uploads/");
                    string filePath = fullpath + item.image_one;
                    // SaveImageAsPdf(filePath, "dd.pdf");


                    //  item.image_one = base64char(filePath);

                }
            }

            return Json(pp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Getfinalprint()
        {

            getdriverlist();
            return View();
        }

        [HttpPost]
        public ActionResult Getfinalprint(FormCollection fc)
        {
            int k = 0;
            var tid = fc["track_id"];

            var fromdate = DateTime.Parse(fc["start"]);
            var enddate = DateTime.Parse(fc["end"]);
            var weekno = getweeknumber(fromdate, enddate);


            var pp = dc.tblconcessions.Where(c => c.deliver_associate == tid && c.week_no == weekno).ToList();



            if (pp.Count > 0)
            {
                PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();
                int i = 0;
                foreach (var item in pp)
                {
                    if (item.image_one != null)
                    {
                        PdfSharp.Pdf.PdfPage page = document.AddPage();


                        k = 1;

                        var fullpath = Server.MapPath("~/Uploads/");
                        string filePath = fullpath + item.image_one;

                        page.Size = PageSize.A4;
                        page.Orientation = PageOrientation.Landscape;
                        //page.Width = XUnit.FromMillimeter(200);
                        //page.Height = XUnit.FromMillimeter(200);
                        XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                        XImage img = XImage.FromFile(filePath);
                        // Create graphics object and draw clock
                        // XGraphics gfx = XGraphics.FromPdfPage(page);

                        //xgr.DrawImage(img, 40, 20);
                        xgr.DrawImage(img, 50, 20, 700, 550);
                        //SaveImageAsPdf(filePath, "dd.pdf");

                        //  item.image_one = base64char(filePath);

                        i++;
                    }
                }

                if (k == 1)
                {
                    MemoryStream stream = new MemoryStream();
                    document.Save(stream, false);
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", stream.Length.ToString());

                    Response.BinaryWrite(stream.ToArray());

                    Response.Flush();
                    stream.Close();
                    Response.End();
                    getdriverlist();
                }

            }
            else
            {

                ViewBag.msg = "No Concession Found for this Driver Or Not Update tranport ID for Driver ";
            }



            getdriverlist();
            return View();

        }

        // function added by ViBeS on 10/04/2020 for generating PDF in new formats.
        public ActionResult GetfinalprintNew(int? id)
        {
            if (id == 1)
            {

                var fdate = DateTime.Parse(Session["SDate"].ToString());
                var edate = DateTime.Parse(Session["EDate"].ToString());
                var wno = Session["WNumber"].ToString();

                //var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).OrderBy(o => o.deliver_associate_name).ToList();

                //var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).Select(
                //col => new ConcessionModel { image_one = col.image_one, deliver_associate = col.deliver_associate, deliver_associate_name = col.deliver_associate_name, DriverReportFileName = col.DriverReportFileName }).Distinct().OrderBy(o => o.deliver_associate_name).ToList();

                var p = dc.tblfinaldrivermasters.Where(c => c.driver_status == "active" && c.transport_id != null).Select(
                    col => new ConcessionModel { deliver_associate = col.transport_id, deliver_associate_name = col.final_driver_name /*, DriverReportFileName = col.DriverReportFileName*/ }).Distinct().OrderBy(o => o.deliver_associate_name).ToList();

                ViewData["tmpDriverList"] = p;

            }
            return View("GetfinalprintNew");
        }

        [HttpPost]
        public ActionResult GetfinalprintNew(string btnProceed, string btnGenerate, string btnSendMail, FormCollection fc, IEnumerable<string> chkDriver, string id)
        {
            if (btnProceed != null)
            {
                var fromdate = DateTime.Parse(fc["start"]);
                Session["SDate"] = fromdate;

                var enddate = DateTime.Parse(fc["end"]);
                Session["EDate"] = enddate;

                var weekno = getweeknumber(fromdate, enddate);
                Session["WNumber"] = weekno;

                List<ConcessionModel> objConcessionModel = new List<ConcessionModel>();
                //var p = from c in dc.tblconcessions where c.week_no == weekno && c.from_date == fromdate && c.to_date == enddate select (col => new { col.ass});
                var p = dc.tblfinaldrivermasters.Where(c => c.driver_status == "active" && c.transport_id != null).Select(
                    col => new ConcessionModel { deliver_associate = col.transport_id, deliver_associate_name = col.final_driver_name /*, DriverReportFileName = col.DriverReportFileName*/ }).Distinct().OrderBy(o => o.deliver_associate_name).ToList();
                ///var p = dc.tblconcessions.Where(c => c.week_no == weekno && c.from_date == fromdate && c.to_date == enddate).OrderBy(o => o.deliver_associate_name).ToList();

                ViewData["tmpDriverList"] = p;

                var WeekFrom = fc["start"].ToString();
                var WeekTo = fc["end"].ToString();

                ViewBag.SelectedWeek = "(" + WeekFrom + " to " + WeekTo + ")";

                return View(p);
            }

            else if (btnGenerate != null)
            {
                var fromdate = DateTime.Parse(Session["SDate"].ToString());
                var enddate = DateTime.Parse(Session["EDate"].ToString());
                var weekno = Session["WNumber"].ToString();

                #region  Working Code

                if (chkDriver == null) return View();

                IEnumerable<string> DeliverAssociateID = chkDriver.Distinct<string>();


                string DriverName = "";
                string WeekNo = "_W" + weekno.ToString();
                string Year = DateTime.Now.Year.ToString();

                string sZipFileName = "DriverConcessionData_" + DateTime.Now.Date.ToShortDateString().ToString().Replace("/", "_");
                // Get the first file in the list so we can get the root directory
                string strRootDirectory = Path.GetDirectoryName(Server.MapPath("~/DriverConcessionPDFs/"));

                // Set up a temporary directory to save the files to (that we will eventually zip up)
                DirectoryInfo dirTemp = Directory.CreateDirectory(strRootDirectory + "/" + DateTime.Now.ToString("yyyyMMddhhmmss"));


                for (int j = 0; j < DeliverAssociateID.Count(); j++)
                {
                    var dAssoID = DeliverAssociateID.ElementAt(j);

                    var pp = dc.tblconcessions.Where(c => c.from_date == fromdate && c.to_date == enddate && c.week_no == weekno && c.deliver_associate == dAssoID).ToList(); //c.deliver_associate == tid

                    //Create a new PDF document.
                    PdfDocument document = new PdfDocument();
                    PdfDocument ScoreCard = new PdfDocument();
                    int P = 0;
                    if (pp.Count > 0)
                    {
                        int i = 0;
                        foreach (var item in pp)
                        {
                            if (i == 0)
                            {
                                if (item.DriverReportFileName != null)
                                {
                                    P = 1;

                                    var fullpath = Server.MapPath("~/DriverConcessionPDFs/");
                                    string filePathPerformance = fullpath + item.DriverReportFileName;

                                    if (System.IO.File.Exists(filePathPerformance))
                                    {
                                        ScoreCard = PdfReader.Open(filePathPerformance, PdfDocumentOpenMode.Import);
                                        int pageCount = ScoreCard.PageCount;

                                        for (int Spage = 0; Spage < pageCount; Spage++)
                                        {
                                            PdfPage page = ScoreCard.Pages[Spage];
                                            document.AddPage(page);
                                            i++;
                                        }
                                    }

                                }
                            }
                            if (item.image_one != null)
                            {
                                P = 1;


                                var fullpath = Server.MapPath("~/Uploads/");
                                string filePathImgOne = fullpath + item.image_one;

                                if (System.IO.File.Exists(filePathImgOne))
                                {
                                    PdfPage page = document.AddPage();
                                    page.Size = PageSize.A4;
                                    page.Orientation = PageOrientation.Landscape;

                                    XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                    XImage img = XImage.FromFile(filePathImgOne);

                                    xgr.DrawImage(img, 50, 20, 700, 550);

                                    i++;
                                }
                            }
                            if (item.image_two != null)
                            {
                                P = 1;


                                var fullpath = Server.MapPath("~/Uploads/");
                                string filePathImgTwo = fullpath + item.image_two;
                                if (System.IO.File.Exists(filePathImgTwo))
                                {
                                    PdfPage page = document.AddPage();
                                    page.Size = PageSize.A4;
                                    page.Orientation = PageOrientation.Landscape;

                                    XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                    XImage img = XImage.FromFile(filePathImgTwo);

                                    xgr.DrawImage(img, 50, 20, 700, 550);

                                    i++;
                                }
                            }
                            DriverName = item.deliver_associate_name.Replace(" ", "_");
                        }

                        if (P == 1)
                        {
                            var savePath = Server.MapPath("~/DriverConcessionPDFs/") + DriverName + WeekNo + "_" + Year + ".pdf";
                            document.Save(savePath);
                            document.Close();


                            // Copy all files to the temporary directory

                            string strDestinationFilePath = Path.Combine(dirTemp.FullName, Path.GetFileName(savePath));
                            System.IO.File.Copy(savePath, strDestinationFilePath);
                            //File.Copy(strFilePath, strDestinationFilePath);

                            //var fileName = DriverName + WeekNo + "_" + Year + ".pdf";
                            //var script = "<script> window.open('../DriverConcessionPDFs/" + fileName + "', '_newtab'); </script>";
                            //Response.Write(script);
                            //Response.Flush();

                        }

                    }
                }

                // Create the zip file using the temporary directory
                if (!sZipFileName.EndsWith(".zip"))
                {
                    sZipFileName += ".zip";
                }

                string strZipPath = Path.Combine(strRootDirectory, sZipFileName);

                if (System.IO.File.Exists(strZipPath))
                {
                    System.IO.File.Delete(strZipPath);
                }

                ZipFile.CreateFromDirectory(dirTemp.FullName, strZipPath, CompressionLevel.Fastest, false);

                // Delete the temporary directory
                dirTemp.Delete(true);

                // Downlode zip file...
                string Host = Server.MapPath("~/DriverConcessionPDFs/") + sZipFileName;
                FileInfo file = new FileInfo(Host);
                //Response.Clear();
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                //Response.AddHeader("Content-Length", file.Length.ToString());
                //Response.ContentType = "application/zip";
                //Response.WriteFile(file.FullName);
                //Response.Flush();
                //Response.End();

                var filePath = Host;
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "filename=" + sZipFileName);

                Response.TransmitFile(filePath);

                Response.End();


                #endregion


                // regain table data

                var fdate = DateTime.Parse(Session["SDate"].ToString());
                var edate = DateTime.Parse(Session["EDate"].ToString());
                var wno = Session["WNumber"].ToString();

                //var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).OrderBy(o => o.deliver_associate_name).ToList();
                var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).Select(
                col => new ConcessionModel { image_one = col.image_one,deliver_associate = col.deliver_associate, deliver_associate_name = col.deliver_associate_name, DriverReportFileName = col.DriverReportFileName }).Distinct().OrderBy(o => o.deliver_associate_name).ToList();

                ViewData["tmpDriverList"] = p;
                return View();
            }

            else if(id != null)
            {
                if (id != "")
                {
                    var fromdate = DateTime.Parse(Session["SDate"].ToString());
                    var enddate = DateTime.Parse(Session["EDate"].ToString());
                    var weekno = Session["WNumber"].ToString();

                    #region  Working Code

                    string DriverName = "";
                    string WeekNo = "_W" + weekno.ToString();
                    string Year = DateTime.Now.Year.ToString();

                    var pp = dc.tblconcessions.Where(c => c.from_date == fromdate && c.to_date == enddate && c.week_no == weekno && c.deliver_associate == id).ToList(); //c.deliver_associate == tid

                    //Create a new PDF document.
                    PdfDocument document = new PdfDocument();
                    PdfDocument ScoreCard = new PdfDocument();
                    int P = 0;
                    if (pp.Count > 0)
                    {
                        int i = 0;
                        foreach (var item in pp)
                        {
                            if (i == 0)
                            {
                                if (item.DriverReportFileName != null)
                                {
                                    P = 1;

                                    var fullpath = Server.MapPath("~/DriverConcessionPDFs/");
                                    string filePath = fullpath + item.DriverReportFileName;

                                    ScoreCard = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
                                    int pageCount = ScoreCard.PageCount;

                                    for (int Spage = 0; Spage < pageCount; Spage++)
                                    {
                                        PdfPage page = ScoreCard.Pages[Spage];
                                        document.AddPage(page);
                                        i++;
                                    }


                                }
                            }
                            if (item.image_one != null)
                            {
                                P = 1;


                                var fullpath = Server.MapPath("~/Uploads/");
                                string filePath = fullpath + item.image_one;

                                if (System.IO.File.Exists(filePath))
                                {
                                    PdfPage page = document.AddPage();
                                    page.Size = PageSize.A4;
                                    page.Orientation = PageOrientation.Landscape;

                                    XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                    XImage img = XImage.FromFile(filePath);

                                    xgr.DrawImage(img, 50, 20, 700, 550);

                                    i++;
                                }
                            }
                            if (item.image_two != null)
                            {
                                P = 1;


                                var fullpath = Server.MapPath("~/Uploads/");
                                string filePath = fullpath + item.image_two;
                                if (System.IO.File.Exists(filePath))
                                {
                                    PdfPage page = document.AddPage();
                                    page.Size = PageSize.A4;
                                    page.Orientation = PageOrientation.Landscape;

                                    XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                    XImage img = XImage.FromFile(filePath);

                                    xgr.DrawImage(img, 50, 20, 700, 550);

                                    i++;
                                }
                            }
                            DriverName = item.deliver_associate_name.Replace(" ", "_");
                        }

                        if (P == 1)
                        {
                            var savePath = Server.MapPath("~/DriverConcessionPDFs/") + DriverName + WeekNo + "_" + Year + ".pdf";
                            document.Save(savePath);
                            document.Close();


                            var fileName = DriverName + WeekNo + "_" + Year + ".pdf";
                            var script = "'/DriverConcessionPDFs/" + fileName + "'";
                            Response.Write(script);
                            Response.Flush();

                            //System.Diagnostics.Process.Start(savePath);

                            //MemoryStream stream = new MemoryStream();
                            //document.Save(stream, false);
                            //Response.Clear();
                            //Response.ContentType = "application/pdf";
                            //Response.AddHeader("content-length", stream.Length.ToString());
                            //Response.BinaryWrite(stream.ToArray());
                            //Response.Flush();
                            //stream.Close();
                            //Response.End();

                            //var fileName = DriverName + WeekNo + "_" + Year + ".pdf";
                            //var script = "<script> window.open('../DriverConcessionPDFs/" + fileName + "', '_newtab'); </script>";
                            //Response.Write(script);
                            //Response.Flush();

                        }

                    }

                    #endregion


                    // regain table data
                    var fdate = DateTime.Parse(Session["SDate"].ToString());
                    var edate = DateTime.Parse(Session["EDate"].ToString());
                    var wno = Session["WNumber"].ToString();

                    //var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).OrderBy(o => o.deliver_associate_name).ToList();
                    var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).Select(
                    col => new ConcessionModel { image_one = col.image_one, deliver_associate = col.deliver_associate, deliver_associate_name = col.deliver_associate_name, DriverReportFileName = col.DriverReportFileName }).Distinct().OrderBy(o => o.deliver_associate_name).ToList();

                    ViewData["tmpDriverList"] = p;

                }
            }

            else if (btnSendMail != null)
            {
                if (chkDriver != null)
                {
                    var fromdate = DateTime.Parse(Session["SDate"].ToString());
                    var enddate = DateTime.Parse(Session["EDate"].ToString());
                    var weekno = Session["WNumber"].ToString();

                    #region  Working Code

                    if (chkDriver == null) { ViewBag.EmaiMsg = "Please select any driver."; return View(); }

                    IEnumerable<string> DeliverAssociateID = chkDriver.Distinct<string>();


                    string DriverName = "";
                    string WeekNo = "_W" + weekno.ToString();
                    string Year = DateTime.Now.Year.ToString();
                    bool result = false;

                    for (int j = 0; j < DeliverAssociateID.Count(); j++)
                    {
                        var dAssoID = DeliverAssociateID.ElementAt(j);

                        var pp = dc.tblconcessions.Where(c => c.from_date == fromdate && c.to_date == enddate && c.week_no == weekno && c.deliver_associate == dAssoID).ToList(); //c.deliver_associate == tid

                        //Create a new PDF document.
                        PdfDocument document = new PdfDocument();
                        PdfDocument ScoreCard = new PdfDocument();
                        int P = 0;
                        if (pp.Count > 0)
                        {
                            int i = 0;
                            foreach (var item in pp)
                            {
                                if (i == 0)
                                {
                                    if (item.DriverReportFileName != null)
                                    {
                                        P = 1;

                                        var fullpath = Server.MapPath("~/DriverConcessionPDFs/");
                                        string filePathPerformance = fullpath + item.DriverReportFileName;

                                        if (System.IO.File.Exists(filePathPerformance))
                                        {
                                            ScoreCard = PdfReader.Open(filePathPerformance, PdfDocumentOpenMode.Import);
                                            int pageCount = ScoreCard.PageCount;

                                            for (int Spage = 0; Spage < pageCount; Spage++)
                                            {
                                                PdfPage page = ScoreCard.Pages[Spage];
                                                document.AddPage(page);
                                                i++;
                                            }
                                        }

                                    }
                                }
                                if (item.image_one != null)
                                {
                                    P = 1;


                                    var fullpath = Server.MapPath("~/Uploads/");
                                    string filePathImgOne = fullpath + item.image_one;

                                    if (System.IO.File.Exists(filePathImgOne))
                                    {
                                        PdfPage page = document.AddPage();
                                        page.Size = PageSize.A4;
                                        page.Orientation = PageOrientation.Landscape;

                                        XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                        XImage img = XImage.FromFile(filePathImgOne);

                                        xgr.DrawImage(img, 50, 20, 700, 550);

                                        i++;
                                    }
                                }
                                if (item.image_two != null)
                                {
                                    P = 1;


                                    var fullpath = Server.MapPath("~/Uploads/");
                                    string filePathImgTwo = fullpath + item.image_two;
                                    if (System.IO.File.Exists(filePathImgTwo))
                                    {
                                        PdfPage page = document.AddPage();
                                        page.Size = PageSize.A4;
                                        page.Orientation = PageOrientation.Landscape;

                                        XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                        XImage img = XImage.FromFile(filePathImgTwo);

                                        xgr.DrawImage(img, 50, 20, 700, 550);

                                        i++;
                                    }
                                }
                                DriverName = item.deliver_associate_name.Replace(" ", "_");
                            }

                            if (P == 1)
                            {
                                var finalDriveData = dc.tblfinaldrivermasters.Where(e => e.transport_id == dAssoID).ToList();

                                if (finalDriveData.Count > 0)
                                {
                                    var savePath = Server.MapPath("~/DriverConcessionPDFs/") + DriverName + WeekNo + "_" + Year + ".pdf";

                                    if (System.IO.File.Exists(savePath))
                                    {
                                        System.IO.File.Delete(savePath);
                                    }

                                    document.Save(savePath);
                                    document.Close();

                                    string toEmail = finalDriveData.FirstOrDefault().final_driver_personal_email;
                                    string mailSubject = "WEEK "+ weekno.ToString() + " - PERFORMANCE/CONCESSIONS REPORT";
                                    string Driver = finalDriveData.FirstOrDefault().final_driver_name;
                                    string mailBody = "<p>Dear " + Driver + ",</p><p>Please find attatched your Performance Score Card and Concession report.</p> <p> Thank You...</p>";
                                    string attachmentPath = savePath.ToString();

                                    result = SendEmail_Fixed(toEmail, mailSubject, mailBody, attachmentPath);

                                }
                            }

                        }
                    }

                    if (result) { ViewBag.EmaiMsg = "Mail sent succesfully !!!"; } else { ViewBag.EmaiMsg = "Error occured while sending some mail."; }

                    #endregion

                }
                else
                {
                    ViewBag.EmaiMsg = "Please select drivers to send email.";
                }

                // regain table data

                var fdate = DateTime.Parse(Session["SDate"].ToString());
                var edate = DateTime.Parse(Session["EDate"].ToString());
                var wno = Session["WNumber"].ToString();

                //var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).OrderBy(o => o.deliver_associate_name).ToList();
                //var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).Select(
                //col => new ConcessionModel { image_one = col.image_one, deliver_associate = col.deliver_associate, deliver_associate_name = col.deliver_associate_name, DriverReportFileName = col.DriverReportFileName }).Distinct().OrderBy(o => o.deliver_associate_name).ToList();

                var p = dc.tblfinaldrivermasters.Where(c => c.driver_status == "active" && c.transport_id != null).Select(
                    col => new ConcessionModel { deliver_associate = col.transport_id, deliver_associate_name = col.final_driver_name /*, DriverReportFileName = col.DriverReportFileName*/ }).Distinct().OrderBy(o => o.deliver_associate_name).ToList();

                ViewData["tmpDriverList"] = p;
                return View();
            }

            return View();
        }

        public string getDriverNamesByWeek(DateTime startDate, DateTime endDate, string weekNo)
        {
            try
            {
                var p = dc.tblconcessions.Where(c => c.week_no == weekNo && c.from_date == startDate && c.to_date == endDate).ToList();
                ViewData["tmpDriverList"] = p;

            }
            catch (Exception)
            {

            }
            return "success";
        }


        // function added by ViBeS on 08/04/2020 to add delete perticular record functionality
        public string DeleteRecord(int cons_id)
        {
            try
            {
                dc.tblconcessions.Remove(dc.tblconcessions.Find(cons_id));
                dc.SaveChanges();
                return "success";
            }
            catch (Exception)
            {
                return "error";
            }
        }

        public ActionResult UploadDriverReportFile(string id)
        {
            Session["Asso_id"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult UploadDriverReportFile(HttpPostedFileBase file1, FormCollection fc)
        {

            if (file1 != null)
            {

                string path = Server.MapPath("~/DriverConcessionPDFs/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = path + Path.GetFileName(file1.FileName);
                string extension = Path.GetExtension(file1.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    file1.SaveAs(filePath);
                }
                else
                {
                    ViewBag.msg = "File already uploaded";
                }

                var Asso_id = Session["Asso_id"].ToString();
                var fdate = DateTime.Parse(Session["SDate"].ToString());
                var edate = DateTime.Parse(Session["EDate"].ToString());
                var wno = Session["WNumber"].ToString();

                List<tblconcession> objtblconcession = dc.tblconcessions.Where(e => e.deliver_associate == Asso_id && e.from_date == fdate && e.to_date == edate && e.week_no == wno).ToList();

                if (objtblconcession.Count > 0)
                {
                    foreach (tblconcession t in objtblconcession)
                    {
                        t.DriverReportFileName = file1.FileName;
                    }
                }
                else
                {
                    var objFinalDriverMaster = dc.tblfinaldrivermasters.Where(e => e.transport_id == Asso_id && e.driver_status == "active").ToList().Distinct();

                    if (objFinalDriverMaster != null)
                    {
                        if (objFinalDriverMaster.Count() > 0)
                        {
                            tblconcession objtblcon = new tblconcession();
                            //objtblcon.tracking_id = objFinalDriverMaster.FirstOrDefault().transport_id;
                            objtblcon.deliver_associate = Asso_id;
                            objtblcon.deliver_associate_name = objFinalDriverMaster.FirstOrDefault().final_driver_name;
                            objtblcon.week_no = wno;
                            objtblcon.from_date = fdate;
                            objtblcon.to_date = edate;
                            objtblcon.DriverReportFileName = file1.FileName;

                            dc.tblconcessions.Add(objtblcon);
                        }
                    }
                }
                dc.SaveChanges();



            }

            Session["flg"] = "1";
            return RedirectToAction("GetfinalprintNew", new { @id = 1 });

        }
                
        public ActionResult RemoveDriverReport(string id)
        {

            if (id != null)
            {
                var fdate = DateTime.Parse(Session["SDate"].ToString());
                var edate = DateTime.Parse(Session["EDate"].ToString());
                var wno = Session["WNumber"].ToString();

                List<tblconcession> objtblconcession = dc.tblconcessions.Where(e => e.deliver_associate == id && e.from_date == fdate && e.to_date == edate && e.week_no == wno).ToList();
                int i = 0;
                foreach (tblconcession t in objtblconcession)
                {
                    var DriverReportFileName = t.DriverReportFileName;

                    if (i == 0)
                    {
                        string path = Server.MapPath("~/DriverConcessionPDFs/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string filePath = path + Path.GetFileName(DriverReportFileName);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    t.DriverReportFileName = null;
                    dc.SaveChanges();
                    i++;
                }

            }

            return RedirectToAction("GetfinalprintNew", new { @id = 1 });

        }
        
        public void ShowPDF(string id)
        {
            if (id != "")
            {
                var fromdate = DateTime.Parse(Session["SDate"].ToString());
                var enddate = DateTime.Parse(Session["EDate"].ToString());
                var weekno = Session["WNumber"].ToString();

                #region  Working Code

                string DriverName = "";
                string WeekNo = "_W" + weekno.ToString();
                string Year = DateTime.Now.Year.ToString();

                var pp = dc.tblconcessions.Where(c => c.from_date == fromdate && c.to_date == enddate && c.week_no == weekno && c.deliver_associate == id).ToList(); //c.deliver_associate == tid

                //Create a new PDF document.
                PdfDocument document = new PdfDocument();
                PdfDocument ScoreCard = new PdfDocument();
                int P = 0;
                if (pp.Count > 0)
                {
                    int i = 0;
                    foreach (var item in pp)
                    {
                        if (i == 0)
                        {
                            if (item.DriverReportFileName != null)
                            {
                                P = 1;

                                var fullpath = Server.MapPath("~/DriverConcessionPDFs/");
                                string filePath = fullpath + item.DriverReportFileName;

                                if (System.IO.File.Exists(filePath))
                                {
                                    ScoreCard = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
                                    int pageCount = ScoreCard.PageCount;

                                    for (int Spage = 0; Spage < pageCount; Spage++)
                                    {
                                        PdfPage page = ScoreCard.Pages[Spage];
                                        document.AddPage(page);
                                        i++;
                                    }
                                }

                            }
                        }
                        if (item.image_one != null)
                        {
                            P = 1;


                            var fullpath = Server.MapPath("~/Uploads/");
                            string filePath = fullpath + item.image_one;

                            if (System.IO.File.Exists(filePath))
                            {
                                PdfPage page = document.AddPage();
                                page.Size = PageSize.A4;
                                page.Orientation = PageOrientation.Landscape;

                                XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                XImage img = XImage.FromFile(filePath);

                                xgr.DrawImage(img, 50, 20, 700, 550);

                                i++;
                            }
                        }
                        if (item.image_two != null)
                        {
                            P = 1;


                            var fullpath = Server.MapPath("~/Uploads/");
                            string filePath = fullpath + item.image_two;
                            if (System.IO.File.Exists(filePath))
                            {
                                PdfPage page = document.AddPage();
                                page.Size = PageSize.A4;
                                page.Orientation = PageOrientation.Landscape;

                                XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                XImage img = XImage.FromFile(filePath);

                                xgr.DrawImage(img, 50, 20, 700, 550);

                                i++;
                            }
                        }
                        DriverName = item.deliver_associate_name.Replace(" ", "_");
                    }

                    if (P == 1)
                    {
                        var savePath = Server.MapPath("~/DriverConcessionPDFs/") + DriverName + WeekNo + "_" + Year + ".pdf";
                        document.Save(savePath);
                        document.Close();

                        //System.Diagnostics.Process.Start(savePath);

                        //MemoryStream stream = new MemoryStream();
                        //document.Save(stream, false);
                        //Response.Clear();
                        //Response.ContentType = "application/pdf";
                        //Response.AddHeader("content-length", stream.Length.ToString());

                        //Response.BinaryWrite(stream.ToArray());

                        //Response.Flush();
                        //stream.Close();
                        //Response.End();

                        var fileName = DriverName + WeekNo + "_" + Year + ".pdf";
                        var script = "/DriverConcessionPDFs/" + fileName;
                        Response.Write(script);
                        Response.Flush();

                    }

                }

                #endregion


                // regain table data
                var fdate = DateTime.Parse(Session["SDate"].ToString());
                var edate = DateTime.Parse(Session["EDate"].ToString());
                var wno = Session["WNumber"].ToString();

                //var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).OrderBy(o => o.deliver_associate_name).ToList();
                var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).Select(
                col => new ConcessionModel { image_one = col.image_one, deliver_associate = col.deliver_associate, deliver_associate_name = col.deliver_associate_name, DriverReportFileName = col.DriverReportFileName }).Distinct().OrderBy(o => o.deliver_associate_name).ToList();

                ViewData["tmpDriverList"] = p;

            }

            //return RedirectToAction("GetfinalprintNew", new { @id = 1 });
        }

        public string SendPerformanceReport(string id)
        {
            string finalResult = "";

            if (id != "")
            {
                var fromdate = DateTime.Parse(Session["SDate"].ToString());
                var enddate = DateTime.Parse(Session["EDate"].ToString());
                var weekno = Session["WNumber"].ToString();

                #region  Working Code

                string DriverName = "";
                string WeekNo = "_W" + weekno.ToString();
                string Year = DateTime.Now.Year.ToString();

                var pp = dc.tblconcessions.Where(c => c.from_date == fromdate && c.to_date == enddate && c.week_no == weekno && c.deliver_associate == id).ToList(); //c.deliver_associate == tid

                //Create a new PDF document.
                PdfDocument document = new PdfDocument();
                PdfDocument ScoreCard = new PdfDocument();
                int P = 0;
                if (pp.Count > 0)
                {


                    int i = 0;
                    foreach (var item in pp)
                    {
                        if (i == 0)
                        {
                            if (item.DriverReportFileName != null)
                            {
                                P = 1;

                                var fullpath = Server.MapPath("~/DriverConcessionPDFs/");
                                string filePath = fullpath + item.DriverReportFileName;

                                if (System.IO.File.Exists(filePath))
                                {
                                    ScoreCard = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
                                    int pageCount = ScoreCard.PageCount;

                                    for (int Spage = 0; Spage < pageCount; Spage++)
                                    {
                                        PdfPage page = ScoreCard.Pages[Spage];
                                        document.AddPage(page);
                                        i++;
                                    }
                                }

                            }
                        }
                        if (item.image_one != null)
                        {
                            P = 1;


                            var fullpath = Server.MapPath("~/Uploads/");
                            string filePath = fullpath + item.image_one;

                            if (System.IO.File.Exists(filePath))
                            {
                                PdfPage page = document.AddPage();
                                page.Size = PageSize.A4;
                                page.Orientation = PageOrientation.Landscape;

                                XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                XImage img = XImage.FromFile(filePath);

                                xgr.DrawImage(img, 50, 20, 700, 550);

                                i++;
                            }
                        }
                        if (item.image_two != null)
                        {
                            P = 1;


                            var fullpath = Server.MapPath("~/Uploads/");
                            string filePath = fullpath + item.image_two;
                            if (System.IO.File.Exists(filePath))
                            {
                                PdfPage page = document.AddPage();
                                page.Size = PageSize.A4;
                                page.Orientation = PageOrientation.Landscape;

                                XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                XImage img = XImage.FromFile(filePath);

                                xgr.DrawImage(img, 50, 20, 700, 550);

                                i++;
                            }
                        }
                        DriverName = item.deliver_associate_name.Replace(" ", "_");
                    }

                    if (P == 1)
                    {
                        var finalDriveData = dc.tblfinaldrivermasters.Where(e => e.transport_id == id && e.driver_status == "active").ToList();

                        if (finalDriveData.Count > 0)
                        {
                            var savePath = Server.MapPath("~/DriverConcessionPDFs/") + DriverName + WeekNo + "_" + Year + ".pdf";

                            if(System.IO.File.Exists(savePath))
                            {
                                System.IO.File.Delete(savePath);
                            }

                            document.Save(savePath);
                            document.Close();

                            string toEmail = finalDriveData.FirstOrDefault().final_driver_personal_email;
                            string mailSubject = "WEEK " + weekno.ToString() + " - PERFORMANCE/CONCESSIONS REPORT";
                            string Driver = finalDriveData.FirstOrDefault().final_driver_name;
                            string mailBody = "<p>Dear " + Driver + ",</p><p>Please find attatched your Performance Score Card and Concession report.</p> <p> Thank You...</p>";
                            string attachmentPath = savePath.ToString();

                            bool result = SendEmail_Fixed(toEmail, mailSubject, mailBody, attachmentPath);

                            if (result) { finalResult = "Mail sent succesfully !!!"; } else { finalResult = "Error occured while sending mail."; }

                            //var fileName = DriverName + WeekNo + "_" + Year + ".pdf";
                            //var script = "/DriverConcessionPDFs/" + fileName;
                            //Response.Write(script);
                            //Response.Flush();
                        }
                    }

                }

                #endregion


                //// regain table data
                //var fdate = DateTime.Parse(Session["SDate"].ToString());
                //var edate = DateTime.Parse(Session["EDate"].ToString());
                //var wno = Session["WNumber"].ToString();

                //var p = dc.tblconcessions.Where(c => c.week_no == wno && c.from_date == fdate && c.to_date == edate).Select(
                //col => new ConcessionModel { image_one = col.image_one, deliver_associate = col.deliver_associate, deliver_associate_name = col.deliver_associate_name, DriverReportFileName = col.DriverReportFileName }).Distinct().OrderBy(o => o.deliver_associate_name).ToList();

                //ViewData["tmpDriverList"] = p;
            }
            else
            {
                finalResult = "No ID available to get Email details.";
            }
            return finalResult;
        }

        public string SendPerformanceReportAll(IEnumerable<string> chkDriver)
        {
            string finalResult = "";

            if (chkDriver != null)
            {
                var fromdate = DateTime.Parse(Session["SDate"].ToString());
                var enddate = DateTime.Parse(Session["EDate"].ToString());
                var weekno = Session["WNumber"].ToString();

                #region  Working Code

                if (chkDriver == null) return finalResult = "Please select any driver.";

                IEnumerable<string> DeliverAssociateID = chkDriver.Distinct<string>();


                string DriverName = "";
                string WeekNo = "_W" + weekno.ToString();
                string Year = DateTime.Now.Year.ToString();
                bool result = false;

                for (int j = 0; j < DeliverAssociateID.Count(); j++)
                {
                    var dAssoID = DeliverAssociateID.ElementAt(j);

                    var pp = dc.tblconcessions.Where(c => c.from_date == fromdate && c.to_date == enddate && c.week_no == weekno && c.deliver_associate == dAssoID).ToList(); //c.deliver_associate == tid

                    //Create a new PDF document.
                    PdfDocument document = new PdfDocument();
                    PdfDocument ScoreCard = new PdfDocument();
                    int P = 0;
                    if (pp.Count > 0)
                    {
                        int i = 0;
                        foreach (var item in pp)
                        {
                            if (i == 0)
                            {
                                if (item.DriverReportFileName != null)
                                {
                                    P = 1;

                                    var fullpath = Server.MapPath("~/DriverConcessionPDFs/");
                                    string filePathPerformance = fullpath + item.DriverReportFileName;

                                    if (System.IO.File.Exists(filePathPerformance))
                                    {
                                        ScoreCard = PdfReader.Open(filePathPerformance, PdfDocumentOpenMode.Import);
                                        int pageCount = ScoreCard.PageCount;

                                        for (int Spage = 0; Spage < pageCount; Spage++)
                                        {
                                            PdfPage page = ScoreCard.Pages[Spage];
                                            document.AddPage(page);
                                            i++;
                                        }
                                    }

                                }
                            }
                            if (item.image_one != null)
                            {
                                P = 1;


                                var fullpath = Server.MapPath("~/Uploads/");
                                string filePathImgOne = fullpath + item.image_one;

                                if (System.IO.File.Exists(filePathImgOne))
                                {
                                    PdfPage page = document.AddPage();
                                    page.Size = PageSize.A4;
                                    page.Orientation = PageOrientation.Landscape;

                                    XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                    XImage img = XImage.FromFile(filePathImgOne);

                                    xgr.DrawImage(img, 50, 20, 700, 550);

                                    i++;
                                }
                            }
                            if (item.image_two != null)
                            {
                                P = 1;


                                var fullpath = Server.MapPath("~/Uploads/");
                                string filePathImgTwo = fullpath + item.image_two;
                                if (System.IO.File.Exists(filePathImgTwo))
                                {
                                    PdfPage page = document.AddPage();
                                    page.Size = PageSize.A4;
                                    page.Orientation = PageOrientation.Landscape;

                                    XGraphics xgr = XGraphics.FromPdfPage(document.Pages[i]);

                                    XImage img = XImage.FromFile(filePathImgTwo);

                                    xgr.DrawImage(img, 50, 20, 700, 550);

                                    i++;
                                }
                            }
                            DriverName = item.deliver_associate_name.Replace(" ", "_");
                        }

                        if (P == 1)
                        {
                            var finalDriveData = dc.tblfinaldrivermasters.Where(e => e.transport_id == dAssoID).ToList();

                            if (finalDriveData.Count > 0)
                            {
                                var savePath = Server.MapPath("~/DriverConcessionPDFs/") + DriverName + WeekNo + "_" + Year + ".pdf";

                                if (System.IO.File.Exists(savePath))
                                {
                                    System.IO.File.Delete(savePath);
                                }

                                document.Save(savePath);
                                document.Close();

                                string toEmail = finalDriveData.FirstOrDefault().final_driver_personal_email;
                                string mailSubject = "Driver performance + concession report.";
                                string Driver = finalDriveData.FirstOrDefault().final_driver_name;
                                string mailBody = "<p>Dear " + Driver + ",</p><p>We have shared performance report attachment, please have a look.</p> <p> Thank You...</p>";
                                string attachmentPath = savePath.ToString();

                                result = SendEmail_Fixed(toEmail, mailSubject, mailBody, attachmentPath);
                                
                            }
                        }

                    }
                }

                if (result) { finalResult = "Mail sent succesfully !!!"; } else { finalResult = "Error occured while sending some mail."; }

                #endregion

            }
            else
            {
                finalResult = "Please select drivers to send email.";
            }
            return finalResult;
        }

        public bool SendEmail_Fixed(string toEmail, string mailSubject, string mailBody, string attachmentPath)
        {
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["smtpUser"].ToString();
                string fromPassword = ConfigurationManager.AppSettings["smtpPass"].ToString();
                string smtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
                string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
                string ccMail = ConfigurationManager.AppSettings["CCEmailID"].ToString();
                string IsEnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString();
                bool EnableSsl = IsEnableSSL == "true" ? true : false;

                SmtpClient smtp = new SmtpClient(smtpServer, Convert.ToInt32(smtpPort));
                smtp.EnableSsl = EnableSsl;
                smtp.Timeout = 100000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);

                MailMessage ms = new MailMessage(fromEmail, toEmail, mailSubject, mailBody);
                ms.IsBodyHtml = true;
                ms.BodyEncoding = UTF8Encoding.UTF8;
                ms.CC.Add(ccMail);

                Attachment attachment = new Attachment(attachmentPath);
                ms.Attachments.Add(attachment);
                

                smtp.Send(ms);
                attachment.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //internal void SaveImageAsPdf(string imageFileName, string pdfFileName, int width = 600, bool deleteImage = false)
        //{





        //    page.Size = PageSize.A4;
        //    page.Orientation = PageOrientation.Landscape;
        //    page.Width = XUnit.FromMillimeter(200);
        //    page.Height = XUnit.FromMillimeter(200);
        //    XGraphics xgr = XGraphics.FromPdfPage(document.Pages[0]);

        //    XImage img = XImage.FromFile(imageFileName);
        //    // Create graphics object and draw clock
        //   // XGraphics gfx = XGraphics.FromPdfPage(page);

        //    xgr.DrawImage(img, 20, 20);

        //    // document.Save()
        //    // Send PDF to browser


        //}


    }

}
