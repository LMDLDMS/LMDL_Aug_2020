using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deliverymangementsystem.EDM;
using deliverymangementsystem.Models;


namespace deliverymangementsystem.Controllers
{
   
    public class RouteTrackingController : Controller
    {
        common comm = new common();
        dsdatabaseEntities dc = new dsdatabaseEntities();
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult CreateRouteTrack()
        {

            return View();

        }
        [HttpPost]
        public ActionResult CreateRouteTrack(FormCollection fc)
        {
            var selectdt = DateTime.Parse(fc["selectdt"]);
            Session["sdate"] = selectdt;
            var dailyrouteobject = dc.tbldailyroutetracks.Where(c => c.date == selectdt).ToList();
            
            List<RouteTrackModel> routelist = new List<RouteTrackModel>();
            if(dailyrouteobject.Count>0)
            {
                var pp = dc.tbldailyroutetracks.Where(c => c.date == selectdt).ToList();
                foreach (var item in pp)
                {
                    RouteTrackModel objmodel = new RouteTrackModel();
                    objmodel.sr_no = item.sr_no;
                    objmodel.route = item.route;
                    objmodel.driver_name = item.driver_name;
                    objmodel.level = item.d_level;
                    objmodel.package = item.packages;
                    objmodel.stops = item.stops;
                    objmodel.packege1st = comm.show(TimeSpan.Parse(item.first_packages.ToString()));
                    objmodel.lastpackage = comm.show(TimeSpan.Parse(item.last_packages.ToString()));
                    objmodel.duration = item.duration;
                    objmodel.SPH = item.sph;
                    objmodel.unknowstops = item.unknowsstops;
                    objmodel.totalunknowstop = item.totalunknowsstops;
                    objmodel.adp_in = comm.show(TimeSpan.Parse(item.adp_puch_in.ToString()));
                    objmodel.adp_out = comm.show(TimeSpan.Parse(item.adp_puch_out.ToString()));
                    objmodel.unpaidbrak = item.unpaidbreak;
                    objmodel.calculatetime = item.calculated_time;
                    objmodel.flexlogin = comm.show(TimeSpan.Parse(item.flex_login.ToString()));
                    objmodel.flexout = comm.show(TimeSpan.Parse(item.flex_out.ToString()));
                    objmodel.route_date = selectdt;
                    routelist.Add(objmodel);
                    
                }
                ViewData["objlist"] = routelist;

            }
            else
            {
                var dispatchlist = dc.tbldispatches.Where(c => c.dispatch_date == selectdt && c.stops!="").ToList();

                foreach (var item in dispatchlist)
                {
                    RouteTrackModel dailyobj = new RouteTrackModel();
                    dailyobj.route = item.route;
                    dailyobj.driver_name = item.driver_name;
                    dailyobj.level = item.leval_details;
                    dailyobj.package = item.packages;
                    dailyobj.stops = item.stops;
                    dailyobj.route_date = selectdt;
                    routelist.Add(dailyobj);
                }

                ViewData["objlist"] = routelist;

            }
        
          return  View();

        }
  
        [HttpPost]
        public ActionResult InsertData(FormCollection fc)
        {
            var selectdt = DateTime.Parse(fc["selectdt"]);
            Session["sdate"] = selectdt;
            var dailyrouteobject = dc.tbldailyroutetracks.Where(c => c.date == selectdt).ToList();

            if (dailyrouteobject.Count ==0)
            {
                var dispatchlist = dc.tbldispatches.Where(c => c.dispatch_date == selectdt && (c.route != "BK" || c.route != "bk" || c.route != "late cancel")).ToList();
                var cnt = 1;
                for (int i = 0; i <dispatchlist.Count; i++)
                {
                    tbldailyroutetrack obj = new tbldailyroutetrack();
    
                    obj.date = selectdt;
                    
                   // var finalroute = ;
                    obj.route = dispatchlist[i].route;
                    var driver_name = "driver_name" + cnt;
                    obj.driver_name = dispatchlist[i].driver_name;
                    var level = "level" + cnt;
                    obj.d_level = fc[level];
                    var package = "package" + cnt;
                    obj.packages = fc[package];
                    var stops = "stops" +cnt;
                    obj.stops = fc[stops];

                    var firstpackage = "firstpackage"+cnt;
                    var time = DateTime.Parse(fc[firstpackage]);
                    var dd = time.ToString("HH:mm");
                    var ss = TimeSpan.Parse(dd.ToString());
                    obj.first_packages =  ss;
               
                    var lastpackage = "lastpackage" + cnt;
                    var time1 = DateTime.Parse(fc[firstpackage]);
                    var dd1 = time1.ToString("HH:mm");
                    var ss1 = TimeSpan.Parse(dd1.ToString());
                    obj.last_packages = ss1;
                    var duration = "duration" + cnt;
                    obj.duration = fc[duration];
                    var sph = "sph" + cnt;
                    obj.sph = fc[sph];
                    var unknows = "unknows" + cnt;
                    obj.unknowsstops = fc[unknows];
                    var totalunknow = "totalunknows"+cnt;
                    obj.totalunknowsstops = fc[totalunknow];

                    var adppuchin = "adppuchin" + cnt;
                    var time2 = DateTime.Parse(fc[adppuchin]);
                    var dd2 = time2.ToString("HH:mm");
                    var ss2 = TimeSpan.Parse(dd2.ToString());
                    obj.adp_puch_in = ss2;
                    var adppuchout = "adppuchout" + cnt;
                    var time3 = DateTime.Parse(fc[adppuchout]);
                    var dd3 = time3.ToString("HH:mm");
                    var ss3 = TimeSpan.Parse(dd3.ToString());
                    obj.adp_puch_out = ss3;
                    var break12 = "break" + cnt;
                    obj.unpaidbreak = fc[break12];
                    var calculate = "calculate" + cnt;
                    obj.calculated_time = fc[calculate];
                    var flagin = "flexlogin" + cnt;
                    var time4 = DateTime.Parse(fc[flagin]);
                    var dd4 = time4.ToString("HH:mm");
                    var ss4 = TimeSpan.Parse(dd4.ToString());
                    obj.flex_login = ss4;
                    var flageout = "flexout" + cnt;
                    var time5 = DateTime.Parse(fc[flageout]);
                    var dd5 = time5.ToString("HH:mm");
                    var ss5 = TimeSpan.Parse(dd5.ToString());
                    obj.flex_out = ss5;

                    dc.tbldailyroutetracks.Add(obj);
                    dc.SaveChanges();
               
                    cnt++;

                }

            }
             else
            {

                var driverlistcnt = dc.tbldailyroutetracks.Where(c => c.date == selectdt).ToList();
                var cnt = 1;
                for (int i = 0; i < driverlistcnt.Count(); i++)
                {
                    var obj = driverlistcnt[i];
                    obj.date = selectdt;

                   
                    var level = "level" + cnt;
                    obj.d_level = fc[level];
                    var package = "package" + cnt;
                    obj.packages = fc[package];
                    var stops = "stops-" + cnt;
                    obj.stops = fc[stops];

                    var firstpackage = "firstpackage" + cnt;
                    var time = DateTime.Parse(fc[firstpackage]);
                    var dd = time.ToString("HH:mm");
                    var ss = TimeSpan.Parse(dd.ToString());
                    obj.first_packages = ss;

                    var lastpackage = "lastpackage" + cnt;
                    var time1 = DateTime.Parse(fc[firstpackage]);
                    var dd1 = time1.ToString("HH:mm");
                    var ss1 = TimeSpan.Parse(dd1.ToString());
                    obj.last_packages = ss1;
                    var duration = "duration" + cnt;
                    obj.duration = fc[duration];
                    var sph = "sph" + cnt;
                    obj.sph = fc[sph];
                    var unknows = "unknows" + cnt;
                    obj.unknowsstops = fc[unknows];
                    var totalunknow = "totalunknows" + cnt;
                    obj.totalunknowsstops = fc[totalunknow];

                    var adppuchin = "adppuchin" + cnt;
                    var time2 = DateTime.Parse(fc[adppuchin]);
                    var dd2 = time2.ToString("HH:mm");
                    var ss2 = TimeSpan.Parse(dd2.ToString());
                    obj.adp_puch_in = ss2;
                    var adppuchout = "adppuchout" + cnt;
                    var time3 = DateTime.Parse(fc[adppuchout]);
                    var dd3 = time3.ToString("HH:mm");
                    var ss3 = TimeSpan.Parse(dd3.ToString());
                    obj.adp_puch_in = ss3;
                    var break12 = "break" + cnt;
                    obj.unpaidbreak = fc[break12];
                    var calculate = "calculate" + cnt;
                    obj.calculated_time = fc[calculate];
                    var flagin = "flexlogin" + cnt;
                    var time4 = DateTime.Parse(fc[flagin]);
                    var dd4 = time4.ToString("HH:mm");
                    var ss4 = TimeSpan.Parse(dd4.ToString());
                    obj.flex_login = ss4;
                    var flageout = "flexout" + cnt;
                    var time5 = DateTime.Parse(fc[flageout]);
                    var dd5 = time5.ToString("HH:mm");
                    var ss5 = TimeSpan.Parse(dd5.ToString());
                    obj.flex_out = ss5;
                    cnt++;
                    dc.SaveChanges();
                }

            }


            return RedirectToAction("CreateRouteTrack");
        }
        
        public ActionResult PrintDailyRouteTrack()
        {

            return View();
 
        }
        [HttpPost]
        public ActionResult PrintDailyRouteTrack(FormCollection fc)
        {

            var selectdt = DateTime.Parse(fc["selectdt"]);

            var dailyrouteobject = dc.tbldailyroutetracks.Where(c => c.date == selectdt).ToList();

            List<RouteTrackModel> routelist = new List<RouteTrackModel>();
            if (dailyrouteobject.Count > 0)
            {
                var pp = dc.tbldailyroutetracks.Where(c => c.date == selectdt).ToList();
                foreach (var item in pp)
                {
                    RouteTrackModel objmodel = new RouteTrackModel();
                    objmodel.route = item.route;
                    objmodel.driver_name = item.driver_name;
                    objmodel.level = item.d_level;
                    objmodel.package = item.packages;
                    objmodel.stops = item.stops;
                    objmodel.packege1st = comm.show(TimeSpan.Parse(item.first_packages.ToString()));
                    objmodel.lastpackage = comm.show(TimeSpan.Parse(item.last_packages.ToString()));
                    objmodel.duration = item.duration;
                    objmodel.SPH = item.sph;
                    objmodel.unknowstops = item.unknowsstops;
                    objmodel.totalunknowstop = item.totalunknowsstops;
                    objmodel.adp_in = comm.show(TimeSpan.Parse(item.adp_puch_in.ToString()));
                    objmodel.adp_out = comm.show(TimeSpan.Parse(item.adp_puch_out.ToString()));
                    objmodel.unpaidbrak = item.unpaidbreak;
                    objmodel.calculatetime = item.calculated_time;
                    objmodel.flexlogin = comm.show(TimeSpan.Parse(item.flex_login.ToString()));
                    objmodel.flexout = comm.show(TimeSpan.Parse(item.flex_out.ToString()));

                    routelist.Add(objmodel);

                }
                ViewData["objlist"] = routelist;

            }



            return View();

        }
        //public ActionResult RescueDetails(string driver_name)
        //{
        //    tblrescue obj = new tblrescue();
            
        //    Session["d_name"] = driver_name;
           
        //    obj.driver_name = driver_name;
        //    return View(obj);
        //}
        //[HttpPost]
        //public ActionResult RescueDetails(tblrescue obj)
        //{
        //    var d_name = Session["d_name"];
        //    var d_date = DateTime.Parse(Session["sdate"].ToString());
        //    obj.driver_name = d_name.ToString();
        //    obj.date = d_date;
        //    dc.tblrescues.Add(obj);
        //    dc.SaveChanges();
        //    return RedirectToAction("CreateRouteTrack");

        //}

    }
}