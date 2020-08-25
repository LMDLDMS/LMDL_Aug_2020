using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using deliverymangementsystem.EDM;

namespace deliverymangementsystem.Controllers
{
    public class MainController : Controller
    {
        dsdatabaseEntities dc = new dsdatabaseEntities();
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {

            return View();


        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {

            var username = fc["username"];
            var password = fc["password"];

            var p = dc.tbladmins.Where(c => c.admin_username == username && c.admin_password == password).ToList();
            var p1 = dc.tblsubadmins.Where(c => c.sub_admin_name == username && c.sub_admin_password == password && c.sub_admin_status =="active").ToList();
            if (p.Count == 1)
            {

               /* FormsAuthentication.SetAuthCookie(p.FirstOrDefault().admin_username, false);

                var authTicket = new FormsAuthenticationTicket(1, "10001", DateTime.Now, DateTime.Now.AddDays(365), false, "Admin");
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                */
                Session["admin"] = "10001";
                Session["username"] = p.FirstOrDefault().admin_username;
                Session["email"] = p.FirstOrDefault().admin_username;


                return RedirectToAction("Home", "Admin");
            }
            else if (p1.Count == 1)
            {

               /* FormsAuthentication.SetAuthCookie(p1.FirstOrDefault().sub_admin_email_id, false);

                var authTicket = new FormsAuthenticationTicket(1, p1.FirstOrDefault().sub_admin_id.ToString(), DateTime.Now, DateTime.Now.AddDays(365), false, "subadmin");
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                */
                Session["admin"] = p1.FirstOrDefault().sub_admin_id;
                Session["username"] = p1.FirstOrDefault().sub_admin_name;
                Session["email"] = p1.FirstOrDefault().sub_admin_email_id;

                return RedirectToAction("Home", "Admin");
                
            }
            else
            {
                ViewBag.msg = "Invalid User ID And Password";
                return View();
                
            }

            

        }
    }
}