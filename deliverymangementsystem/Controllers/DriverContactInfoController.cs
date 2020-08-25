using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deliverymangementsystem.EDM;

namespace deliverymangementsystem.Controllers
{
    public class DriverContactInfoController : Controller
    {
        //dsdatabaseEntities dc = new dsdatabaseEntities();
        dsdatabaseEntities dc = new dsdatabaseEntities();
        // GET: DriverContactInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DisplaycontactInfo()
        {
            getdriverdata();

            return View();

        }

        public JsonResult getdriverinfo()
        {
            var ss = dc.tblfinaldrivermasters.Where(c=>c.driver_status =="active").ToList();


            return Json(ss, JsonRequestBehavior.AllowGet);


        }

        public JsonResult contactinfo(string d_name)
        {
            var ss = (from n in dc.tblfinaldrivermasters where n.final_driver_name.Contains(d_name) && n.driver_status == "active" select n).ToList();


            return Json(ss,JsonRequestBehavior.AllowGet);

        }
        public void getdriverdata()
        {
            var c = dc.tblfinaldrivermasters.ToList();

            List<SelectListItem> objlist = new List<SelectListItem>();
            foreach (var item in c)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.final_driver_name;
                obj.Value = item.final_driver_id.ToString();
                objlist.Add(obj);
                
            }
            ViewBag.data = objlist;

        }

        
        


       

    }
}