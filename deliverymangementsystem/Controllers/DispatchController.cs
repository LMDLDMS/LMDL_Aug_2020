using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deliverymangementsystem.EDM;
using deliverymangementsystem.Models;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;

namespace deliverymangementsystem.Controllers
{
    public class DispatchController : Controller
    {

        //dsdatabaseEntities dc = new dsdatabaseEntities();
        dsdatabaseEntities dc = new dsdatabaseEntities();
        // GET: Dispatch
        public ActionResult Index()
        {
            return View();
        }

        public void updatedata()
        {
            List<SelectListItem> driverlist = new List<SelectListItem>();
            var objlist = dc.tblfinaldrivermasters.Where(c => c.driver_status == "active").OrderBy(c => c.final_driver_name).ToList();
            foreach (var item in objlist)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.final_driver_name;
                obj.Value = item.final_driver_name.ToString();
                driverlist.Add(obj);
            }
            ViewData["driverlist"] = driverlist;

        }

        public void getdata()
        {
            List<SelectListItem> driverlist = new List<SelectListItem>();
            var objlist = dc.tblfinaldrivermasters.Where(c => c.driver_status == "active").OrderBy(c => c.final_driver_name).ToList();
            foreach (var item in objlist)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.final_driver_name;
                obj.Value = item.final_driver_name.ToString();
                driverlist.Add(obj);
            }
            ViewData["driverlist"] = driverlist;

        }


        public void setvalafteradd()
        {
            var records = 1;
            var sdt = DateTime.Parse(Session["currentdt"].ToString());


            List<dispatchmodel> listobj = new List<dispatchmodel>();
            var cc = dc.tbldispatches.Where(c => c.dispatch_date == sdt);
            var cnt = 1;
            foreach (var item in cc)
            {
                dispatchmodel obj = new dispatchmodel();
                obj.routes = item.routes;
                obj.rideralong = item.ride_along;
                obj.route = item.route;
                obj.bages = item.bag == null ? 0 : Convert.ToInt32(item.bag);
                obj.cat = item.cat;
                obj.driverName = item.driver_name;
                obj.level = item.leval_details;
                obj.notes = item.notes;
                obj.Stop = item.stops;
                obj.vantype = item.vantype;
                obj.package = item.packages;
                obj.van_number = item.van_number;
                obj.sr_no = int.Parse(item.dispatch_id.ToString());
                obj.back_res = item.re_backup;

                listobj.Add(obj);
                cnt++;
            }

            ViewBag.countdt = cnt;
            ViewData["objlist"] = listobj;
            getdata();
            ViewBag.data = records;

        }

        public ActionResult CreateDispatchSheet(int? val)
        {
            var records1 = ViewBag.data;
            if (records1 == 1)
            {

                return View();
            }
            else if (val == 1)
            {
                var records = 1;

                var selecteddate = DateTime.Parse(Session["currentdt"].ToString());

                List<dispatchmodel> listobj = new List<dispatchmodel>();
                var cc = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).OrderBy(o1 => o1.bag == 0 ? 99999 : o1.bag).ThenBy(o => o.re_backup.Trim() == "" ? "W" : o.re_backup == "Rescue" ? "X" : o.re_backup == "Backup" ? "Y" : o.re_backup == "Callout" ? "Z" : o.re_backup);
                var cnt = 1;
                foreach (var item in cc)
                {
                    // code updated to populate privious day van numbers
                    var previousday = DateTime.Parse(Session["currentdt"].ToString()).AddDays(-1);
                    var c = dc.tbldispatches.Where(e => e.dispatch_date == previousday && e.driver_name == item.driver_name && e.re_backup != "callout" && e.re_backup != "Backup").ToList();

                    dispatchmodel obj = new dispatchmodel();
                    obj.routes = item.routes;
                    obj.rideralong = item.ride_along;
                    obj.route = item.route;
                    obj.bages = item.bag == null ? 0 : Convert.ToInt32(item.bag);
                    obj.cat = item.cat;
                    obj.phone2 = item.phone2;
                    obj.driverName = item.driver_name;
                    obj.level = item.leval_details;
                    obj.notes = item.notes;
                    obj.Stop = item.stops;
                    obj.vantype = item.vantype;
                    obj.package = item.packages;

                    if (c.Count > 0 && c.FirstOrDefault().van_number.Trim() != "" && item.re_backup != "callout" && item.re_backup != "Backup" && item.re_backup != "Rescue")
                    {
                        obj.van_number = c.FirstOrDefault().van_number;
                    }
                    else
                    {
                        obj.van_number = item.van_number;
                    }

                    obj.back_res = item.re_backup;
                    obj.sr_no = int.Parse(item.dispatch_id.ToString());

                    obj.IsCheckedIN = item.IsCheckedIn;

                    listobj.Add(obj);
                    cnt++;
                }

                ViewBag.countdt = cnt;
                ViewData["objlist"] = listobj;

                getdata();
                ViewBag.data = records;
                return View();
            }
            else
            {
                var records = 0;

                Session["currentdt"] = "";
                getdata();
                ViewBag.data = records;
                return View();
            }

        }
        [HttpPost]
        public ActionResult CreateDispatchSheet(FormCollection fc, HttpPostedFileBase postedFile)
        {
            var records = 0;

            Session["currentdt"] = (DateTime.Parse(fc["selectdt"])).ToShortDateString();

            var selecteddate = DateTime.Parse(fc["selectdt"]);
            var check = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).Count();
            if (check == 0)
            {
                records = 0;
                if (postedFile != null)
                {

                    //if (postedFile.ContentType == "application/octet-stream")
                    //{

                    string filePath = string.Empty;
                    if (postedFile != null)
                    {
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        filePath = path + Path.GetFileName(postedFile.FileName);
                        string extension = Path.GetExtension(postedFile.FileName);
                        if (!System.IO.File.Exists(filePath))
                        {
                            postedFile.SaveAs(filePath);
                        }
                        else
                        {
                            ViewBag.msg = "File already uploaded";
                            uploadafter(selecteddate);

                            return View();
                        }
                        //Create a DataTable.
                        DataTable dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[12] {
                            new DataColumn("backup_res", typeof(string)),
                            new DataColumn("vantype",typeof(string)),
                            new DataColumn("route",typeof(string)),
                            new DataColumn("driver_name",typeof(string)),
                            new DataColumn("van_number",typeof(string)),
                            new DataColumn("bag",typeof(string)),
                            new DataColumn("cat",typeof(string)),
                            new DataColumn("packages",typeof(string)),
                            new DataColumn("stops",typeof(string)),
                            new DataColumn("leval_details",typeof(string)),
                            new DataColumn("ride_along",typeof(string)),
                            new DataColumn("notes",typeof(string))
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

                                    //Execute a loop over the columns.
                                    foreach (string cell in row.Split(','))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = cell;
                                        i++;
                                    }
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
                            for (int i = 2; i < dt.Rows.Count; i++)
                            {
                                sss++;
                                try
                                {
                                    string driver_name = "";
                                    if (dt.Rows[i]["driver_name"] != null)
                                    {
                                        driver_name = (dt.Rows[i]["driver_name"].ToString().Trim());

                                    }
                                    else
                                    {
                                        driver_name = "";
                                    }

                                    var previousday = DateTime.Parse(fc["selectdt"]).AddDays(-1);
                                    var c = dc.tbldispatches.Where(e => e.dispatch_date == previousday && e.driver_name == driver_name && e.re_backup != "callout" && e.re_backup != "Backup").ToList();

                                    string notes = "";
                                    if (dt.Rows[i]["notes"] != null)
                                    {
                                        notes = (dt.Rows[i]["notes"].ToString().Trim());

                                    }
                                    else
                                    {
                                        notes = "";
                                    }

                                    string ride_along = "";
                                    if (dt.Rows[i]["ride_along"] != null)
                                    {
                                        ride_along = (dt.Rows[i]["ride_along"].ToString().Trim());

                                    }
                                    else
                                    {
                                        ride_along = "";
                                    }

                                    string leval_details = "";
                                    if (dt.Rows[i]["leval_details"] != null)
                                    {
                                        leval_details = (dt.Rows[i]["leval_details"].ToString().Trim());

                                    }
                                    else
                                    {
                                        leval_details = "";
                                    }
                                    
                                    string route = "";
                                    if (dt.Rows[i]["route"] != null)
                                    {
                                        route = (dt.Rows[i]["route"].ToString().Trim());

                                    }
                                    else
                                    {
                                        route = "";
                                    }
                                    
                                    string vantype = "";
                                    if (dt.Rows[i]["vantype"] != null)
                                    {
                                        vantype = (dt.Rows[i]["vantype"].ToString().Trim());

                                    }
                                    else
                                    {
                                        vantype = "";
                                    }
                                    
                                    string backup_res = "";
                                    if (dt.Rows[i]["backup_res"] != null)
                                    {
                                        backup_res = (dt.Rows[i]["backup_res"].ToString().Trim());

                                    }
                                    else
                                    {
                                        backup_res = "";

                                    }
                                    backup_res = backup_res.Trim();

                                    string bag = "";
                                    if (backup_res == "Backup")
                                    {

                                    }
                                    else if (backup_res == "Rescue")
                                    {

                                        bag = sss.ToString();

                                    }
                                    else
                                    {
                                        bag = sss.ToString();
                                    }

                                    string cat = "";
                                    if (dt.Rows[i]["cat"] != null)
                                    {
                                        cat = (dt.Rows[i]["cat"].ToString().Trim());

                                    }
                                    else
                                    {
                                        cat = "";
                                    }

                                    string package = "";
                                    if (dt.Rows[i]["packages"] != null)
                                    {

                                        package = (dt.Rows[i]["packages"].ToString().Trim());

                                    }
                                    else
                                    {
                                        package = "";

                                    }

                                    string stops = "";
                                    if (dt.Rows[i]["stops"] != null)
                                    {

                                        stops = (dt.Rows[i]["stops"].ToString().Trim());

                                    }
                                    else
                                    {
                                        stops = "";

                                    }

                                    string van_number = "";
                                    if (dt.Rows[i]["van_number"].ToString().Trim() != "")
                                    {
                                        van_number = (dt.Rows[i]["van_number"].ToString().Trim());

                                    }
                                    else if (c.Count > 0 && c.FirstOrDefault().van_number.Trim() != "" && backup_res != "callout" && backup_res != "Backup" && backup_res != "Rescue")
                                    {
                                        van_number = c.FirstOrDefault().van_number;
                                    }
                                    else
                                    {
                                        van_number = "";
                                    }




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
                                    cmd.CommandText = "insert into tbldispatch(dispatch_date,re_backup,bag,vantype,route,driver_name,cat,packages,stops,leval_details,ride_along,notes,van_number)values('" + selecteddate + "','" + backup_res + "','" + bag + "','" + vantype + "','" + route + "','" + driver_name + "','" + cat + "','" + package + "','" + stops + "','" + leval_details + "','" + ride_along + "','" + notes + "','" + van_number + "')";
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

                        uploadafter(selecteddate);
                        return View();

                    }
                    //}
                    //else
                    //{
                    //    ViewBag.msg = "Only uplaod csv file";
                    //}
                }
                else
                {

                    ViewBag.msg = "Please Uplaod csv file only";
                }

                var driverlist = int.Parse(fc["totalroute"].ToString());

                var totalrescue = int.Parse(fc["recscue"].ToString());

                var totalbackup = int.Parse(fc["backup"].ToString());
                List<dispatchmodel> objlist = new List<dispatchmodel>();
                var cnt = 1;
                for (int c = 0; c < driverlist; c++)
                {
                    var objmodel = new dispatchmodel();
                    objmodel.routes = cnt.ToString();
                    objmodel.bages = Convert.ToInt32(cnt.ToString());
                    // objmodel.driverName = "";
                    // objmodel.vantype = "Rental";
                    objlist.Add(objmodel);
                    cnt = cnt + 1;
                }
                for (int c = 0; c < totalrescue; c++)
                {
                    var objmodel = new dispatchmodel();
                    objmodel.routes = cnt.ToString();
                    objmodel.bages = Convert.ToInt32(cnt.ToString());
                    // objmodel.driverName = "";
                    // objmodel.vantype = "Rental";
                    //objmodel.bages = cnt.ToString();
                    objmodel.back_res = "Rescue";
                    objlist.Add(objmodel);
                    cnt = cnt + 1;
                }
                for (int i = 0; i < totalbackup; i++)
                {
                    var objmodel = new dispatchmodel();
                    objmodel.routes = cnt.ToString();
                    //objmodel.bages = cnt.ToString();
                    // objmodel.driverName = "";
                    // objmodel.vantype = "Rental";
                    objmodel.back_res = "Backup";
                    objlist.Add(objmodel);
                    cnt = cnt + 1;
                }



                ViewBag.countdt = cnt;
                ViewData["objlist"] = objlist;
                getdata();
            }
            else
            {
                records = 1;

                List<dispatchmodel> listobj = new List<dispatchmodel>();

                var cc = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).OrderBy(o1 => o1.bag == 0 ? 99999 : o1.bag).ThenBy(o => o.re_backup.Trim() == "" ? "W" : o.re_backup == "Rescue" ? "X" : o.re_backup == "Backup" ? "Y" : o.re_backup == "Callout" ? "Z" : o.re_backup).ToList();

                //var cc1 = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).ToList();
                //var co = cc1.OrderBy(o => o.bag);
                //var cc = co.OrderBy(o => o.re_backup == "Rescue" ? "X" : o.re_backup == "Backup" ? "Y" : o.re_backup == "Callout" ? "Z" : o.re_backup);

                var cnt = 1;
                var currentVan = "";
                foreach (var item in cc)
                {
                    // code updated to populate privious day van numbers
                    var previousday = DateTime.Parse(fc["selectdt"]).AddDays(-1);
                    var c = dc.tbldispatches.Where(e => e.dispatch_date == previousday && e.driver_name == item.driver_name && e.re_backup != "callout" && e.re_backup != "Backup").ToList();

                    dispatchmodel obj = new dispatchmodel();
                    obj.routes = item.routes;
                    obj.rideralong = item.ride_along;
                    obj.route = item.route;
                    obj.bages = item.bag == null ? 0 : Convert.ToInt32(item.bag);
                    obj.cat = item.cat;
                    obj.phone2 = item.phone2;
                    obj.driverName = item.driver_name;
                    obj.level = item.leval_details;
                    obj.notes = item.notes;
                    obj.Stop = item.stops;
                    obj.vantype = item.vantype;
                    obj.package = item.packages;

                    currentVan = item.van_number == null ? "" : item.van_number;

                    if (c.Count > 0 && c.FirstOrDefault().van_number.Trim() != "" && item.re_backup != "callout" && item.re_backup != "Backup" && item.re_backup != "Rescue" && currentVan.Trim() == "")
                    {
                        obj.van_number = c.FirstOrDefault().van_number;
                    }
                    else
                    {
                        obj.van_number = item.van_number;
                    }

                    obj.back_res = item.re_backup;
                    obj.sr_no = int.Parse(item.dispatch_id.ToString());

                    obj.IsCheckedIN = item.IsCheckedIn;

                    listobj.Add(obj);
                    cnt++;
                }

                ViewBag.countdt = cnt;
                ViewData["objlist"] = listobj;
                getdata();
            }

            ViewBag.data = records;
            return View();


        }

        public void uploadafter(DateTime selecteddate)
        {
            List<dispatchmodel> listobj = new List<dispatchmodel>();
            var cc = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate);
            var cnt = 1;
            foreach (var item in cc)
            {
                dispatchmodel obj = new dispatchmodel();
                obj.routes = item.routes;
                obj.rideralong = item.ride_along;
                obj.route = item.route;
                obj.bages = item.bag == null ? 0 : Convert.ToInt32(item.bag);
                obj.cat = item.cat;
                obj.driverName = item.driver_name;
                obj.level = item.leval_details;
                obj.notes = item.notes;
                obj.Stop = item.stops;
                obj.vantype = item.vantype;
                obj.package = item.packages;
                obj.van_number = item.van_number;
                obj.back_res = item.re_backup;

                listobj.Add(obj);
                cnt++;
            }

            ViewBag.countdt = cnt;
            ViewData["objlist"] = listobj;
            getdata();

        }
        public ActionResult InsertData(FormCollection fc)
        {
            var selecteddate = DateTime.Parse(fc["selectdt"]);

            var check = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).Count();
            if (check > 0)
            {
                var driverlistcnt = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).ToList();
                var cnt = 1;

                for (int i = 0; i < driverlistcnt.Count(); i++)
                {
                    var obj = driverlistcnt[i];
                    obj.dispatch_date = selecteddate;

                    obj.vantype = fc["vantype" + cnt];
                    obj.route = fc["route" + cnt];
                    var bagg = fc["bag" + cnt];
                    //if (bagg == "")
                    //{

                    //}
                    //else
                    //{
                    //    obj.bag = fc["bag" + cnt];
                    //}
                    obj.bag = Convert.ToInt32(fc["bag" + cnt]);
                    obj.driver_name = fc["driverid" + cnt];
                    obj.cat = fc["cat" + cnt];
                    obj.phone2 = fc["Phone2" + cnt];
                    obj.packages = (fc["package" + cnt]);
                    obj.stops = fc["stops" + cnt];
                    obj.leval_details = fc["level" + cnt];
                    obj.ride_along = fc["ridealong" + cnt];
                    obj.notes = fc["notes" + cnt];
                    obj.van_number = fc["vannumber" + cnt];
                    obj.re_backup = fc["res_backup" + cnt];

                    obj.IsCheckedIn = fc["IsCheckedIN" + cnt];

                    cnt++;


                }

                dc.SaveChanges();
            }
            else
            {
                List<tbldispatch> templist = new List<tbldispatch>();

                var driverlistcnt = dc.tblfinaldrivermasters.ToList().Count;
                for (int i = 1; i <= driverlistcnt; i++)
                {
                    tbldispatch obj = new tbldispatch();
                    obj.dispatch_date = selecteddate;

                    obj.routes = i.ToString();

                    // obj.bag = i.ToString();
                    obj.vantype = fc["vantype" + i];
                    obj.route = fc["route" + i];
                    obj.driver_name = fc["driverid" + i];


                    var bagg = fc["bag" + i];
                    if (bagg == "")
                    {

                    }
                    else
                    {
                        obj.bag = Convert.ToInt32(fc["bag" + i]);
                    }

                    // obj.bag = fc["bag" + i];
                    obj.cat = (fc["cat" + i]);
                    obj.phone2 = fc["Phone2" + i];
                    obj.packages = (fc["package" + i]);
                    obj.stops = (fc["stops" + i]);
                    obj.leval_details = fc["level" + i];
                    obj.ride_along = fc["ridealong" + i];
                    obj.notes = fc["notes" + i];
                    obj.van_number = fc["vannumber" + i];
                    obj.re_backup = fc["res_backup" + i];

                    var drivname = fc["driverid" + i];
                    if (drivname == "" || drivname == null)
                    {

                    }
                    else
                    {
                        templist.Add(obj);
                        //dc.tbldispatches.Add(obj);
                        //dc.SaveChanges();
                    }
                }

                storedata(templist);


            }

            return RedirectToAction("CreateDispatchSheet");

        }


        public void storedata(List<tbldispatch> ss)
        {

            dsdatabaseEntities context = null;
            try
            {
                context = new dsdatabaseEntities();
                context.Configuration.AutoDetectChangesEnabled = false;

                int count = 0;
                foreach (var entityToInsert in ss)
                {
                    ++count;
                    context = AddToContext(context, entityToInsert, count, ss.Count, true);
                }

                context.SaveChanges();
            }
            finally
            {
                if (context != null)
                    context.Dispose();
            }





        }

        private dsdatabaseEntities AddToContext(dsdatabaseEntities context,
        tbldispatch entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<tbldispatch>().Add(entity);

            if (count % commitCount == 0)
            {
                context.SaveChanges();
                if (recreateContext)
                {
                    context.Dispose();
                    context = new dsdatabaseEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return context;
        }


        public ActionResult Printdispatchsheet()
        {

            return View();

        }
        [HttpPost]
        public ActionResult Printdispatchsheet(FormCollection fc)
        {
            var selecteddate = DateTime.Parse(fc["selectdate"]);

            ViewBag.selecteddate = selecteddate.ToShortDateString();

            List<dispatchmodel> listobj = new List<dispatchmodel>();
            //var cc = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate && c.re_backup != "callout");

            //  30/05/2020
            //var cc = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).OrderBy(o => o.dispatch_id);
            var cc = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).OrderBy(o => o.re_backup == "Rescue" ? "X" : o.re_backup == "Backup" ? "Y" : o.re_backup == "Callout" ? "Z" : o.re_backup);
            //   var cnt = 1;
            foreach (var item in cc)
            {
                dispatchmodel obj = new dispatchmodel();
                obj.routes = item.routes;
                obj.rideralong = item.ride_along;
                obj.route = item.route;
                obj.bages = item.bag == null ? 0 : Convert.ToInt32(item.bag);
                obj.cat = item.cat;
                obj.driverName = item.driver_name;
                obj.level = item.leval_details;
                obj.notes = item.notes;
                obj.Stop = item.stops;
                obj.vantype = item.vantype;
                obj.package = item.packages;
                obj.van_number = item.van_number;
                obj.back_res = item.re_backup;
                obj.phone2 = item.phone2;


                listobj.Add(obj);
                // cnt++;
            }

            ViewData["objlist"] = listobj;
            getdata();
            return View();
        }

        public ActionResult currentday(int? id = 0)
        {
            if (id == 1)
            {
                var sdate = DateTime.Parse(Session["currentdt"].ToString());
                List<dispatchmodel> listobj = new List<dispatchmodel>();
                var cc = dc.tbldispatches.Where(c => c.dispatch_date == sdate);
                var cnt = 1;
                foreach (var item in cc)
                {
                    dispatchmodel obj = new dispatchmodel();
                    obj.routes = item.routes;
                    obj.rideralong = item.ride_along;
                    obj.route = item.route;
                    obj.bages = item.bag == null ? 0 : Convert.ToInt32(item.bag);
                    obj.cat = item.cat;
                    obj.driverName = item.driver_name;
                    obj.level = item.leval_details;
                    obj.notes = item.notes;
                    obj.Stop = item.stops;
                    obj.vantype = item.vantype;
                    obj.package = item.packages;
                    obj.van_number = item.van_number;
                    obj.sr_no = int.Parse(item.dispatch_id.ToString());
                    listobj.Add(obj);
                    cnt++;
                }
                ViewBag.countdt = cnt;
                ViewData["objlist"] = listobj;
                getdata();
                return View();

            }
            else
            {
                Session["currentdt"] = "";
                getdata();
                return View();
            }




        }

        [HttpPost]
        public ActionResult currentday(FormCollection fc)
        {

            Session["currentdt"] = (DateTime.Parse(fc["selectdt"])).ToShortDateString();
            var selecteddate = DateTime.Parse(fc["selectdt"]);
            var check = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).Count();
            if (check == 0)
            {

                getdata();
            }
            else
            {

                List<dispatchmodel> listobj = new List<dispatchmodel>();
                var cc = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate);
                var cnt = 1;
                foreach (var item in cc)
                {
                    dispatchmodel obj = new dispatchmodel();
                    obj.routes = item.routes;
                    obj.rideralong = item.ride_along;
                    obj.route = item.route;
                    obj.bages = item.bag == null?0:Convert.ToInt32(item.bag);
                    obj.cat = item.cat;
                    obj.driverName = item.driver_name;
                    obj.level = item.leval_details;
                    obj.notes = item.notes;
                    obj.Stop = item.stops;
                    obj.vantype = item.vantype;
                    obj.package = item.packages;
                    obj.van_number = item.van_number;
                    obj.sr_no = int.Parse(item.dispatch_id.ToString());
                    listobj.Add(obj);
                    cnt++;

                }

                ViewBag.countdt = cnt;
                ViewData["objlist"] = listobj;
                getdata();
            }

            return View();
        }


        public string removerow(int id)
        {
            dc.tbldispatches.Remove(dc.tbldispatches.Find(id));
            dc.SaveChanges();
            return "record remove";

        }
        [HttpPost]
        public ActionResult CurrentDayInsert(FormCollection fc)
        {
            var selecteddate = DateTime.Parse(fc["selectdt"]);
            var check = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).Count();
            if (check > 0)
            {
                var driverlistcnt = dc.tbldispatches.Where(c => c.dispatch_date == selecteddate).ToList();
                var cnt = 1;
                for (int i = 0; i < driverlistcnt.Count(); i++)
                {

                    var obj = driverlistcnt[i];
                    obj.dispatch_date = selecteddate;
                    obj.vantype = fc["vantype" + cnt];
                    obj.route = fc["route" + cnt];
                    obj.bag = Convert.ToInt32(fc["bag" + cnt]);
                    obj.driver_name = fc["driverid" + cnt];
                    obj.cat = fc["cat" + cnt];
                    obj.packages = (fc["package" + cnt]);
                    obj.stops = fc["stops" + cnt];
                    obj.leval_details = fc["level" + cnt];
                    obj.ride_along = fc["ridealong" + cnt];
                    obj.notes = fc["notes" + cnt];
                    obj.van_number = fc["vannumber" + cnt];
                    cnt++;
                    dc.SaveChanges();
                }

            }
            else
            {
                var driverlistcnt = dc.tblfinaldrivermasters.ToList().Count;
                for (int i = 1; i <= driverlistcnt; i++)
                {
                    tbldispatch obj = new tbldispatch();
                    obj.dispatch_date = selecteddate;

                    obj.routes = i.ToString();

                    obj.bag = i;
                    obj.vantype = fc["vantype" + i];
                    obj.route = fc["route" + i];
                    obj.driver_name = fc["driverid" + i];
                    obj.bag = Convert.ToInt32(fc["bag" + i]);
                    obj.cat = (fc["cat" + i]);
                    obj.packages = (fc["package" + i]);
                    obj.stops = (fc["stops" + i]);
                    obj.leval_details = fc["level" + i];
                    obj.ride_along = fc["ridealong" + i];
                    obj.notes = fc["notes" + i];
                    obj.van_number = fc["vannumber" + i];

                    var drivname = fc["driverid" + i];
                    if (drivname == "" || drivname == null)
                    {

                    }
                    else
                    {
                        dc.tbldispatches.Add(obj);
                        dc.SaveChanges();
                    }
                }

            }

            return RedirectToAction("currentday");

        }

        public ActionResult addmorerow(int id)
        {
            Session["ss"] = id;

            return View();

        }
        [HttpPost]
        public ActionResult addmorerow(FormCollection fc)
        {

            var ss = DateTime.Parse(Session["currentdt"].ToString());
            var cmt = int.Parse(fc["noofrow"].ToString());

            var MaxPouchNumber = dc.tbldispatches.Where(w => w.dispatch_date == ss && (w.bag == null ? 0 : w.bag) != 0).Count() + 1;
            //var MaxPouchNumber = dc.tbldispatches.Where(w => w.dispatch_date == ss && w.bag != (w.bag == null ? "" : w.bag.Trim())).Count() + 1;
            //var MaxPouchNumber = Convert.ToInt32(dc.tbldispatches.Max(m => m.bag));

            for (int i = 0; i < cmt; i++)
            {
                tbldispatch obj = new tbldispatch();
                obj.dispatch_date = ss;
                obj.bag = MaxPouchNumber;
                dc.tbldispatches.Add(obj);
                dc.SaveChanges();
                MaxPouchNumber++;
            }

            var sts = int.Parse(Session["ss"].ToString());
            if (sts == 1)
            {
                setvalafteradd();
                return RedirectToAction("CreateDispatchSheet", new { val = "1" });
            }
            else
            {
                return RedirectToAction("currentday", new { id = "1" });
            }



        }

        public void getdriverlist()
        {

            var currentdate = DateTime.Now.Date;
            //var ss = (from n in dc.tbldispatches where n.dispatch_date == currentdate && n.van_number == "" && (n.re_backup != "callout" && n.re_backup != "Backup" && n.re_backup != "Rescue") select n).OrderBy(c => c.driver_name).ToList();
            var ss = (from n in dc.tbldispatches where n.dispatch_date == currentdate && n.IsCheckedIn != "Yes" && (n.re_backup != "callout" && n.re_backup != "Backup") select n).OrderBy(c => c.driver_name).ToList();
            // var ss = (from n in dc.tbldispatches where n.route != "" select n).ToList();
            List<SelectListItem> objlist = new List<SelectListItem>();
            foreach (var item in ss)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.driver_name;
                obj.Value = item.driver_name;
                objlist.Add(obj);

            }

            ViewBag.data = objlist;

        }

        public JsonResult GetVans(string vanType)
        {
            var currentdate = DateTime.Now.Date;
            var dispatchData = dc.tbldispatches.Where(e => e.dispatch_date == currentdate && e.van_number != "").Select(s => s.van_number);
            var vans = dc.tblVanMasters.Where(e => e.VanType == vanType).Select(e => e.VanNumber).Except(dispatchData).ToList();

            List<SelectListItem> objVanList = new List<SelectListItem>();
            foreach (var item in vans)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item;
                obj.Value = item;
                objVanList.Add(obj);
            }
            return Json(objVanList, JsonRequestBehavior.AllowGet);
        }

        public void FillAllVans()
        {
            var AllVans = dc.tblVanMasters.Select(s => s.VanNumber).ToList();

            List<SelectListItem> objVans = new List<SelectListItem>();
            foreach (var EachVan in AllVans)
            {
                SelectListItem sl = new SelectListItem();
                sl.Text = EachVan;
                sl.Value = EachVan;

                objVans.Add(sl);
            }

            ViewBag.Vandata = objVans;
        }

        public void getdriverlistforedit()
        {

            var currentdate = DateTime.Now.Date;
            //var ss = (from n in dc.tbldispatches where n.dispatch_date == currentdate && (n.re_backup != "callout" && n.re_backup != "Backup" && n.re_backup != "Rescue") && n.van_number != "" select n).OrderBy(c => c.driver_name).ToList();
            var ss = (from n in dc.tbldispatches where n.dispatch_date == currentdate && (n.re_backup != "callout" && n.re_backup != "Backup") && n.IsCheckedIn == "Yes" select n).OrderBy(c => c.driver_name).ToList();
            List<SelectListItem> objlist = new List<SelectListItem>();
            foreach (var item in ss)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.driver_name;
                obj.Value = item.driver_name;
                objlist.Add(obj);

            }

            ViewBag.data1 = objlist;

        }

        public ActionResult Assignvan()
        {
            getdriverlist();
            FillAllVans();

            //getdriverlistfromroute();
            //assignedphone();
            //getdriverlistforedit();
            filldata();
            filldata1();
            GetCount();
            return View();

        }

        public void GetCount()
        {
            try
            {
                var currentdate = DateTime.Now.Date;
                var ss = (from n in dc.tbldispatches where n.dispatch_date == currentdate && (n.re_backup != "callout" && n.re_backup != "Backup") select n).ToList();

                if(ss.Count > 0)
                {
                    ViewBag.TotalRoutes = ss.Count();
                }
                else
                {
                    ViewBag.TotalRoutes = 0;
                }

                var ss1 = (from n in dc.tbldispatches where n.dispatch_date == currentdate && n.IsCheckedIn == "Yes" && (n.re_backup != "callout" && n.re_backup != "Backup") select n).ToList();

                if (ss1.Count > 0)
                {
                    ViewBag.CheckedIn = ss1.Count();
                }
                else
                {
                    ViewBag.CheckedIn = 0;
                }

                var ss2 = (from n in dc.tbldispatches where n.dispatch_date == currentdate && n.IsCheckedIn != "Yes" && n.re_backup != "callout" && n.re_backup != "Backup" select n).ToList();

                if (ss2.Count > 0)
                {
                    ViewBag.CheckedOut = ss2.Count();
                }
                else
                {
                    ViewBag.CheckedOut = 0;
                }


            }
            catch (Exception ex)
            {
                
            }
        }

        [HttpPost]
        public ActionResult Assignvan(FormCollection fc)
        {

            var driver_name = fc["driver_name"];
            var current_date = DateTime.Now.Date;
            var van_type = fc["van_type"];
            var van_number = fc["van_number"];
            var routenumber = fc["routenumber"];
            var pouchnumber = fc["pouchnumber"];

            var checkIn = fc["checkIn"] != null ? "Yes" : "No";

            var p = dc.tbldispatches.Where(c => c.driver_name == driver_name && c.dispatch_date == current_date).FirstOrDefault();

            p.vantype = van_type;
            p.van_number = van_number;
            p.route = routenumber;
            p.bag = Convert.ToInt32(pouchnumber);
            p.IsCheckedIn = checkIn;

            dc.SaveChanges();
            getdriverlist();
            FillAllVans();
            
            assignedphone();
            filldata1();
            filldata();
            GetCount();
            return View();

        }

        public ActionResult editvaninfo()
        {

            getdriverlistforedit();

            return View();

        }

        [HttpPost]
        public ActionResult editvaninfo(FormCollection fc)
        {

            var driver_name = fc["driver_name1"];
            var current_date = DateTime.Now.Date;
            var van_type = fc["van_type1"];
            var van_number = fc["van_number1"];
            var routenumber = fc["routenumber1"];
            var pouchnumber = fc["pouchnumber1"];
            var p = dc.tbldispatches.Where(c => c.driver_name == driver_name && c.dispatch_date == current_date).FirstOrDefault();

            p.vantype = van_type;
            p.van_number = van_number;
            p.route = routenumber;
            p.bag = Convert.ToInt32(pouchnumber);

            dc.SaveChanges();
            getdriverlist();
            return RedirectToAction("assignvan");

        }

        [HttpGet]
        public JsonResult getinfo(string d_name)
        {
            var currentdt = DateTime.Now.Date;
            var ss = dc.tbldispatches.Where(c => c.driver_name == d_name && c.dispatch_date == currentdt).ToList();
            //GetVans(ss.FirstOrDefault().vantype);
            //FillAllVans();
            return Json(ss, JsonRequestBehavior.AllowGet);

        }

        public ActionResult generatePicklist()
        {


            return View();

        }
        public JsonResult getpicklist(string sdate)
        {
            var finaldate = DateTime.Parse(sdate);
            // Updated on 18 May 2020 by ViBeS as per requirement as not to generate CalledOut drivers report.
            //var ss = dc.tbldispatches.Where(c => c.dispatch_date == finaldate && c.re_backup != "Backup").ToList();
            var ss = dc.tbldispatches.Where(c => c.dispatch_date == finaldate && c.re_backup != "Backup" && c.re_backup != "callout").ToList();
            return Json(ss, JsonRequestBehavior.AllowGet);

        }
        public void assignedphone()
        {
            var currdate = DateTime.Now.Date;
            var ss = dc.tbldispatches.Where(c => c.dispatch_date == currdate && c.bag != 0).ToList();

            ViewData["objlist"] = ss;

        }

        [HttpPost]
        public ActionResult updatePhone(FormCollection fc)
        {

            var currdate = DateTime.Now.Date;
            var ss = dc.tbldispatches.Where(c => c.dispatch_date == currdate && c.bag != 0).ToList();
            for (int i = 0; i < ss.Count; i++)
            {
                var p = ss[i];

                //  p.bag = fc["bag" + i];

                p.cat = fc["phone1" + i];

                p.phone2 = fc["Phone2" + i];

            }

            dc.SaveChanges();
            return RedirectToAction("dailyroutetrack");

        }

        public ActionResult dailyroutetrack()
        {
            getdriverlistfromroute();
            assignedphone();

            return View();
        }

        [HttpPost]
        public ActionResult dailyroutetrack(FormCollection fc)
        {

            var currentdate = DateTime.Parse(fc["currdate"]);
            var route = fc["route"];
            var driver_name = fc["driver_name"];
            var stops = fc["stops"];
            var package = fc["package"];

            var ss = dc.tbldispatches.Where(c => c.route == route && c.dispatch_date == currentdate).Count();

            if (ss == 0)
            {
                var pp = dc.tbldispatches.Where(c => c.dispatch_date == currentdate && c.driver_name == driver_name).FirstOrDefault();
                pp.route = route;
                pp.driver_name = driver_name;
                pp.stops = stops;
                pp.packages = package;
                dc.SaveChanges();

            }
            else
            {
                var pp = dc.tbldispatches.Where(c => c.dispatch_date == currentdate && c.route == route).FirstOrDefault();

                pp.driver_name = driver_name;
                pp.stops = stops;
                pp.packages = package;
                dc.SaveChanges();


            }



            getdriverlistfromroute();
            assignedphone();
            return View();





        }
        public void getdriverlistfromroute()
        {

            var currentdate = DateTime.Now.Date;
            var ss = (from n in dc.tbldispatches where n.dispatch_date == currentdate && n.route == "" && (n.re_backup != "callout" && n.re_backup != "Backup") select n).OrderBy(c => c.driver_name).ToList();
            List<SelectListItem> objlist = new List<SelectListItem>();
            foreach (var item in ss)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.driver_name;
                obj.Value = item.driver_name;
                objlist.Add(obj);

            }

            ViewBag.data = objlist;

        }

        public JsonResult getdatabyroute(string route, string dateval)
        {
            try
            {


                var finaldate = DateTime.Parse(dateval);
                var pp = dc.tbldispatches.Where(c => c.dispatch_date == finaldate && c.route == route).ToList();

                return Json(pp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("");


            }
        }

        public ActionResult editrouteassign()
        {
            filldata();
            filldata1();

            return View();

        }

        public string switchroute(string driver1, string driver2, string notes)
        {
            var currentdt1 = DateTime.Now.Date;
            var pp = dc.tbldispatches.Where(c => c.driver_name == driver1 && c.dispatch_date == currentdt1).FirstOrDefault().route;

            var pp1 = dc.tbldispatches.Where(c => c.driver_name == driver2 && c.dispatch_date == currentdt1).FirstOrDefault().route;

            var first = dc.tbldispatches.Where(c => c.route == pp && c.dispatch_date == currentdt1).FirstOrDefault();

            var second = dc.tbldispatches.Where(c => c.route == pp1 && c.dispatch_date == currentdt1).FirstOrDefault();
            first.driver_name = driver2;
            first.notes = notes;
            dc.SaveChanges();

            second.driver_name = driver1;
            dc.SaveChanges();

            return "Driver Route Change Successfully ..";


        }

        public string callout(string callout1, string callout2, string notes)
        {
            var currentdt1 = DateTime.Now.Date;
            var pp = dc.tbldispatches.Where(c => c.driver_name == callout1 && c.dispatch_date == currentdt1).FirstOrDefault();

            var pp1 = dc.tbldispatches.Where(c => c.driver_name == callout2 && c.dispatch_date == currentdt1).FirstOrDefault();

            pp1.route = pp.route;
            pp1.stops = pp.stops;
            pp1.packages = pp.packages;
            pp1.vantype = pp.vantype;
            pp1.van_number = pp.van_number;
            pp1.leval_details = pp.leval_details;
            pp1.bag = pp.bag;
            pp1.cat = pp.cat;
            pp1.notes = pp.notes;
            pp1.ride_along = pp.ride_along;
            pp1.phone2 = pp.phone2;
            //pp1.notes = notes;
            pp1.re_backup = "";
            dc.SaveChanges();

            pp.route = "";
            pp.stops = "";
            pp.packages = "";
            pp.re_backup = "callout";

            pp.vantype = "";
            pp.van_number = "";
            pp.leval_details = "";
            pp.bag = 0;
            pp.cat = "";
            pp.notes = notes;
            pp.ride_along = "";
            pp.phone2 = "";

            dc.SaveChanges();
            return "Driver Setup CallOut Successfully ..";

        }

        [HttpPost]
        public ActionResult editrouteassign(FormCollection fc)
        {

            return View();

        }

        public void filldata1()
        {
            var currentdate = DateTime.Now.Date;
            //var ss = (from n in dc.tbldispatches where n.dispatch_date == currentdate && n.route == "" && (n.re_backup == "Backup" || n.re_backup == "Rescue") select n).OrderBy(c => c.driver_name).ToList();
            var ss = (from n in dc.tbldispatches where n.dispatch_date == currentdate && (n.re_backup == "Backup" || n.re_backup == "Rescue") select n).OrderBy(o => o.re_backup == "Rescue" ? "X" : o.re_backup == "Backup" ? "Y" : o.re_backup == "Callout" ? "Z" : o.re_backup).ToList();
            List<SelectListItem> objlist = new List<SelectListItem>();
            foreach (var item in ss)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.driver_name;
                obj.Value = item.driver_name;
                objlist.Add(obj);

            }

            ViewBag.data3 = objlist;


        }
        public void filldata()
        {

            var currentdate = DateTime.Now.Date;
            //var ss = (from n in dc.tbldispatches where n.dispatch_date == currentdate && n.route != "" && n.re_backup != "callout" && n.re_backup != "Backup" && n.re_backup != "Rescue" select n).OrderBy(c => c.driver_name).ToList();
            var ss = (from n in dc.tbldispatches where n.dispatch_date == currentdate && n.re_backup != "callout" && n.re_backup != "Backup" select n).OrderBy(c => c.driver_name).ToList();
            List<SelectListItem> objlist = new List<SelectListItem>();
            foreach (var item in ss)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.driver_name;
                obj.Value = item.driver_name;
                objlist.Add(obj);

            }

            ViewBag.data1 = objlist;

        }


        public ActionResult ResetData(FormCollection fc)
        {
            var dt = DateTime.Parse(fc["selectdt"]);

            dc.tbldispatches.RemoveRange(dc.tbldispatches.Where(c => c.dispatch_date == dt));

            //db.ProRel.RemoveRange(db.ProRel.Where(c => c.ProjectId == Project_id));
            dc.SaveChanges();
            return RedirectToAction("CreateDispatchSheet");


        }

    }

}