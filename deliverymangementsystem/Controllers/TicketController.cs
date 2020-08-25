using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using deliverymangementsystem.EDM;

namespace deliverymangementsystem.Controllers
{
    public class TicketController : Controller
    {
        //dsdatabaseEntities dc = new dsdatabaseEntities();
        dsdatabaseEntities dc = new dsdatabaseEntities();
        // GET: Ticket
        public ActionResult Index()
        {
           
            return View();
        }

        public void  getusername()
        {
            var id = int.Parse(Session["admin"].ToString());

            var p = dc.tblsubadmins.ToList();

            List<SelectListItem> objlist = new List<SelectListItem>();
            foreach (var item in p)
            {
                SelectListItem obj = new SelectListItem();
      
                 if(id != item.sub_admin_id)
                  {
                    obj.Text = item.sub_admin_name;
                    obj.Value = item.sub_admin_id.ToString();
                    objlist.Add(obj);
                  }
                 
                
               
            }

            SelectListItem ss = new SelectListItem();
            ss.Text = "Dispatcher";
            ss.Value = "000";
            objlist.Add(ss);

            ViewData["users"] = objlist;
            
        }

        public ActionResult OpenTicket()
        {

            getusername();
            return View();
        }
        [HttpPost]
        public ActionResult OpenTicket(tblticket obj)
        {
             obj.ticket_open_by_id = int.Parse(Session["admin"].ToString());
             obj.ticket_open_date = DateTime.Parse(DateTime.Now.ToString());
             obj.ticket_open_time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
             obj.ticket_status = "Open";
             dc.tbltickets.Add(obj);
             dc.SaveChanges();

            if(obj.ticket_assigned_user_id == 0)
            {
                string openby = "";
                if(obj.ticket_open_by_id == 10001)
                {
                    openby = "admin";
                }
                else
                {
                    openby = dc.tblsubadmins.Find(obj.ticket_open_by_id).sub_admin_name;
                }

                var ss = dc.tblsubadmins.Where(c => c.user_type == "Yes").ToList();
                foreach (var item in ss)
                {
                    var subject = "NEW TICKET ASSIGNED. TICKET# --> FOR STATUS: OPEN";
                    var body = "Hi  " + item.sub_admin_name + " A new ticket is assigned to you. Please login to the system and pick up the ticket <a href = 'http://cftclients.com/Main/Login'> Click Open </a> ";
                 //   sendfinalemail(item.sub_admin_email_id, subject, body);

                }
                

            }
            else
            {
                string openby = "";
                if (obj.ticket_open_by_id == 10001)
                {
                    openby = "admin";
                }
                else
                {
                    openby = dc.tblsubadmins.Find(obj.ticket_open_by_id).sub_admin_name;
                }
                var subject = "NEW TICKET ASSIGNED. TICKET# --> FOR STATUS: OPEN";
                var username = dc.tblsubadmins.Find(obj.ticket_assigned_user_id).sub_admin_name;
                var emailid = dc.tblsubadmins.Find(obj.ticket_assigned_user_id).sub_admin_email_id;
                var body = "Hi  " + username +" A new ticket is assigned to you. Please login to the system and pick up the ticket <a href = 'http://cftclients.com/Main/Login'> Click Open </a>";
               // sendfinalemail(emailid, subject, body);
              
            }
            
            tblticketconveration objconv = new tblticketconveration();
             objconv.from_user_id = int.Parse(Session["admin"].ToString());
             objconv.ticket_id = obj.ticket_id;
             objconv.to_user_id = obj.ticket_assigned_user_id;
             objconv.ticket_conversation = obj.issue;
             objconv.time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
             objconv.date = DateTime.Parse(DateTime.Now.ToString());
             objconv.ticket_status = "Open";
             dc.tblticketconverations.Add(objconv);
             dc.SaveChanges();
             ModelState.Clear();
             ViewBag.msg = "Ticket Open & Assigned Successfully ..";
             getusername();
             return View();
            
        }
        public string sendfinalemail(string toemail, string subject, string emailbody)
        {
            try
            {

                SmtpClient client = new SmtpClient("hgws27.win.hostgator.com", 465);

                // SmtpClient client = new SmtpClient("relay-hosting.secureserver.net", 25);
                MailMessage message = new MailMessage();
                message.From = new MailAddress("support@lastmiled.com");

                message.To.Add(new MailAddress(toemail));

                message.Subject = subject;
                message.IsBodyHtml = true;

                message.Body = emailbody;
                client.Send(message);
                return "Yes";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public ActionResult ViewOpenTicket(string id)
        {
            var userid = int.Parse(Session["admin"].ToString());
           
            if(id==null)
            {

                if (userid == 10001)
                {
                    var p = (from n in dc.tbltickets
                             where n.ticket_status == "Open" || n.ticket_status == "Picked" || n.ticket_status == "Done"|| n.ticket_status == "Replied"|| n.ticket_status == "Revert" || n.ticket_status == "ReOpen"

                             select new
                             {
                                 n.ticket_id,
                                 n.ticket_type,
                                 n.ticket_open_date,
                                 n.ticket_open_time,
                                 n.ticket_assigned_user_id,
                                 n.cat_phone_number,
                                 n.gas_card_number,
                                 n.issue,
                                 n.vendor_name,
                                 n.van_type,
                                 n.van_number,
                                 n.supply_needed,
                                 n.Quantity,
                                 n.ticket_open_by_id,
                                 n.ticket_status,
                                 n.ticket_closed_date,
                                 n.ticket_closed_time,
                                 n.ticket_picked_date,
                                 n.ticket_picked_time,
                                 n.mac_address
                             }).ToList();
                    List<tblticket> objlist = new List<tblticket>();

                    foreach (var item in p)
                    {
                        tblticket obj = new tblticket();
                        obj.ticket_id = item.ticket_id;
                        obj.ticket_type = item.ticket_type;
                        obj.ticket_status = item.ticket_status;
                        obj.ticket_open_date = item.ticket_open_date;
                        obj.ticket_open_time = item.ticket_open_time;
                        obj.ticket_closed_date = item.ticket_closed_date;
                        obj.ticket_closed_time = item.ticket_closed_time;
                        // obj.assignedusername = item.sub_admin_name;
                        obj.ticket_open_by_id = item.ticket_open_by_id;
                        obj.supply_needed = item.supply_needed;
                        obj.Quantity = item.Quantity;
                        obj.issue = item.issue;
                        obj.vendor_name = item.vendor_name;
                        obj.van_type = item.van_type;
                        obj.van_number = item.van_number;
                        obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                        obj.ticket_picked_date = item.ticket_picked_date;
                        obj.ticket_picked_time = item.ticket_picked_time;
                        obj.mac_address = item.mac_address;
                        obj.cat_phone_number = item.cat_phone_number;
                        obj.gas_card_number = item.gas_card_number;
                        objlist.Add(obj);
                        dc.SaveChanges();

                    }

                    return View(objlist);

                }
                else
                {
                    bool isdisp = false;

                    var isdispacher = dc.tblsubadmins.Where(c => c.sub_admin_id == userid).FirstOrDefault().user_type;
                    var secondid = 1;
                    if (isdispacher == "Yes")
                    {
                        isdisp = true;
                        secondid = 0;

                        var p = (from n in dc.tbltickets
                                 where n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid && (n.ticket_status == "Open" || n.ticket_status == "Picked" || n.ticket_status == "Revert"|| n.ticket_status == "Replied"|| n.ticket_status == "Done"|| n.ticket_status == "ReOpen")
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.gas_card_number,
                                     n.cat_phone_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        foreach (var item in p)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            obj.gas_card_number = item.gas_card_number;
                            obj.cat_phone_number = item.cat_phone_number;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }


                        return View(objlist);
                    }
                    else
                    {
                        secondid = 1;
                        isdisp = false;
                        var p = (from n in dc.tbltickets

                                 where
                                 n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid && ( n.ticket_status == "Open")
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.gas_card_number,
                                     n.cat_phone_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        foreach (var item in p)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            obj.gas_card_number = item.gas_card_number;
                            obj.cat_phone_number = item.cat_phone_number;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }

                        return View(objlist);
                    }






                }
            }

            else if (id =="All")
            {
                if (userid == 10001)
                {
                    var p = (from n in dc.tbltickets
                             

                             select new
                             {
                                 n.ticket_id,
                                 n.ticket_type,
                                 n.ticket_open_date,
                                 n.ticket_open_time,
                                 n.ticket_assigned_user_id,
                                 n.gas_card_number,
                                 n.cat_phone_number,
                                 n.issue,
                                 n.vendor_name,
                                 n.van_type,
                                 n.van_number,
                                 n.supply_needed,
                                 n.Quantity,
                                 n.ticket_open_by_id,
                                 n.ticket_status,
                                 n.ticket_closed_date,
                                 n.ticket_closed_time,
                                 n.ticket_picked_date,
                                 n.ticket_picked_time,
                                 n.mac_address
                             }).ToList();
                    List<tblticket> objlist = new List<tblticket>();

                    foreach (var item in p)
                    {
                        tblticket obj = new tblticket();
                        obj.ticket_id = item.ticket_id;
                        obj.ticket_type = item.ticket_type;
                        obj.ticket_status = item.ticket_status;
                        obj.ticket_open_date = item.ticket_open_date;
                        obj.ticket_open_time = item.ticket_open_time;
                        obj.ticket_closed_date = item.ticket_closed_date;
                        obj.ticket_closed_time = item.ticket_closed_time;
                        // obj.assignedusername = item.sub_admin_name;
                        obj.ticket_open_by_id = item.ticket_open_by_id;
                        obj.supply_needed = item.supply_needed;
                        obj.Quantity = item.Quantity;
                        obj.issue = item.issue;
                        obj.vendor_name = item.vendor_name;
                        obj.van_type = item.van_type;
                        obj.van_number = item.van_number;
                        obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                        obj.ticket_picked_date = item.ticket_picked_date;
                        obj.ticket_picked_time = item.ticket_picked_time;
                        obj.mac_address = item.mac_address;
                        obj.gas_card_number = item.gas_card_number;
                        obj.cat_phone_number = item.cat_phone_number;
                        objlist.Add(obj);
                        dc.SaveChanges();

                    }

                    return View(objlist);

                }
                else
                {
                    bool isdisp = false;

                    var isdispacher = dc.tblsubadmins.Where(c => c.sub_admin_id == userid).FirstOrDefault().user_type;
                    var secondid = 1;
                    if (isdispacher == "Yes")
                    {
                        isdisp = true;
                        secondid = 0;

                        var p = (from n in dc.tbltickets
                                 where n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid 
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.cat_phone_number,
                                     n.gas_card_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        foreach (var item in p)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            obj.gas_card_number = item.gas_card_number;
                            obj.cat_phone_number = item.cat_phone_number;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }


                        return View(objlist);
                    }
                    else
                    {
                        secondid = 1;
                        isdisp = false;
                        var p = (from n in dc.tbltickets

                                 where
                                 n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid 
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.cat_phone_number,
                                     n.gas_card_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        foreach (var item in p)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            obj.gas_card_number = item.gas_card_number;
                            obj.cat_phone_number = item.cat_phone_number;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }

                        return View(objlist);
                    }






                }

            }
            else
            {
                if (userid == 10001)
                {
                    var p = (from n in dc.tbltickets
                             where  n.ticket_status == id

                             select new
                             {
                                 n.ticket_id,
                                 n.ticket_type,
                                 n.ticket_open_date,
                                 n.ticket_open_time,
                                 n.ticket_assigned_user_id,
                                 n.cat_phone_number,
                                 n.gas_card_number,
       
                                 n.issue,
                                 n.vendor_name,
                                 n.van_type,
                                 n.van_number,
                                 n.supply_needed,
                                 n.Quantity,
                                 n.ticket_open_by_id,
                                 n.ticket_status,
                                 n.ticket_closed_date,
                                 n.ticket_closed_time,
                                 n.ticket_picked_date,
                                 n.ticket_picked_time,
                                 n.mac_address
                             }).ToList();
                    List<tblticket> objlist = new List<tblticket>();

                    foreach (var item in p)
                    {
                        tblticket obj = new tblticket();
                        obj.ticket_id = item.ticket_id;
                        obj.ticket_type = item.ticket_type;
                        obj.ticket_status = item.ticket_status;
                        obj.ticket_open_date = item.ticket_open_date;
                        obj.ticket_open_time = item.ticket_open_time;
                        obj.ticket_closed_date = item.ticket_closed_date;
                        obj.ticket_closed_time = item.ticket_closed_time;
                        // obj.assignedusername = item.sub_admin_name;
                        obj.ticket_open_by_id = item.ticket_open_by_id;
                        obj.supply_needed = item.supply_needed;
                        obj.Quantity = item.Quantity;
                        obj.issue = item.issue;
                        obj.vendor_name = item.vendor_name;
                        obj.van_type = item.van_type;
                        obj.van_number = item.van_number;
                        obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                        obj.ticket_picked_date = item.ticket_picked_date;
                        obj.ticket_picked_time = item.ticket_picked_time;
                        obj.mac_address = item.mac_address;
                        obj.gas_card_number = item.gas_card_number;
                        obj.cat_phone_number = item.cat_phone_number;
                        objlist.Add(obj);
                        dc.SaveChanges();

                    }

                    return View(objlist);

                }
                else
                {
                    bool isdisp = false;

                    var isdispacher = dc.tblsubadmins.Where(c => c.sub_admin_id == userid).FirstOrDefault().user_type;
                    var secondid = 1;
                    if (isdispacher == "Yes")
                    {
                        isdisp = true;
                        secondid = 0;

                        var p = (from n in dc.tbltickets
                                 where n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid && ( n.ticket_status == id)
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.gas_card_number,
                                     n.cat_phone_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();
                        
                        List<tblticket> objlist = new List<tblticket>();
                        var sss = p.Where(c => c.ticket_status == id);
                        foreach (var item in sss)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            obj.cat_phone_number = item.cat_phone_number;
                            obj.gas_card_number = item.gas_card_number;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }


                        return View(objlist);
                    }
                    else
                    {
                        secondid = 1;
                        isdisp = false;
                        var p = (from n in dc.tbltickets

                                 where
                                 n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid && (n.ticket_status == id)
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.gas_card_number,
                                     n.cat_phone_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        var ppp = p.Where(c => c.ticket_status == id);
                        foreach (var item in ppp)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            obj.gas_card_number = item.gas_card_number;
                            obj.cat_phone_number = item.cat_phone_number;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }

                        return View(objlist);
                    }






                }

            }


           
           

        }


        public JsonResult getdataUserInfo()
        {
            int id = int.Parse(Session["ticketId"].ToString());
            var p = dc.tblticketconverations.Where(c => c.ticket_id == id).ToList();

            List<tblticketconveration> listdata = new List<tblticketconveration>();

            foreach (var item in p)
            {
                tblticketconveration obj = new tblticketconveration();
                obj.ticket_id = item.ticket_id;
                obj.ticket_conversation = item.ticket_conversation;
                obj.from_user_id = item.from_user_id;
                obj.ticket_status = item.ticket_status;
                if (obj.from_user_id == 10001)
                {
                    obj.fromusername = "Admin";
                }
                else if(obj.from_user_id == 0)
                {
                    var ss = int.Parse(Session["admin"].ToString());
                    obj.fromusername = dc.tblsubadmins.Find(ss).sub_admin_name;
                }
                else
                {
                    obj.fromusername = dc.tblsubadmins.Find(obj.from_user_id).sub_admin_name;

                }
                obj.to_user_id = item.to_user_id;

                if (obj.to_user_id == 10001)
                {
                    obj.tousername = "Admin";
                }
                else if (obj.to_user_id == 0)
                {
                    var sst = int.Parse(Session["admin"].ToString());

                    if (sst == 10001)
                    {
                        obj.tousername = "dispatcher";
                    }
                    else
                    {

                        obj.tousername = dc.tblsubadmins.Find(sst).sub_admin_name;
                    }
                }
                else
                {
                    obj.tousername = dc.tblsubadmins.Find(obj.to_user_id).sub_admin_name;
                }

                obj.date = item.date;
                obj.time = item.time;
               
                listdata.Add(obj);
               
              

            }
            
           return Json(listdata, JsonRequestBehavior.AllowGet);

        }

        public ActionResult conversation(int id)
        {

            Session["ticketId"] = id;
            ViewBag.ticket_id = id;
       
            return View();
           
        }
        [HttpPost]
        public string UpdateStatus(int id)
        {
           
            var c = dc.tbltickets.Find(id);
            c.ticket_status = "Picked";
            c.ticket_picked_date = DateTime.Parse(DateTime.Now.ToString());
            c.ticket_picked_time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
            dc.SaveChanges();
            var fromid = int.Parse(Session["admin"].ToString());
            decimal toid;
            if (fromid == dc.tbltickets.Find(id).ticket_open_by_id)
            {
                toid = decimal.Parse(dc.tbltickets.Find(id).ticket_assigned_user_id.ToString());
            }
            else
            {
                toid = decimal.Parse(dc.tbltickets.Find(id).ticket_open_by_id.ToString());

            }
            tblticketconveration obj = new tblticketconveration();
            obj.ticket_id = id;
            obj.from_user_id = fromid;
            obj.to_user_id = toid;
            obj.ticket_conversation = "Ticket Recived..";
            obj.ticket_status = "Picked";
            obj.date = DateTime.Parse(DateTime.Now.ToString());
            obj.time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
            dc.tblticketconverations.Add(obj);
            dc.SaveChanges();
            return "Ticked Picked Successfully ..";
   
        }

        [HttpPost]
        public string  postconv(int ticket_id,string desc,string vname,string task)
        {
            var fromid = int.Parse(Session["admin"].ToString());
            if(vname!=null || vname!="")
            {
                var p = dc.tbltickets.Find(ticket_id);
                p.mac_address = vname;
                dc.SaveChanges();
                
            }
            decimal toid;
            if (fromid == dc.tbltickets.Find(ticket_id).ticket_open_by_id)
            {
                toid = decimal.Parse(dc.tbltickets.Find(ticket_id).ticket_assigned_user_id.ToString());
            }
            else
            {
                toid = decimal.Parse(dc.tbltickets.Find(ticket_id).ticket_open_by_id.ToString());

            }
           
            tblticketconveration obj = new tblticketconveration();
            obj.ticket_id = ticket_id;
            obj.from_user_id = fromid;
            obj.to_user_id = toid;
            obj.ticket_conversation = desc;

            obj.date = DateTime.Parse(DateTime.Now.ToString());
            obj.time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
            if (fromid == dc.tbltickets.Find(ticket_id).ticket_assigned_user_id)
            {
               

                var ss = dc.tbltickets.Find(ticket_id);
                ss.ticket_status = "Revert";
                if (task == "Yes")
                {
                    ss.ticket_status = "Done";

                }
                dc.SaveChanges();
                obj.ticket_status = "Revert";

            }
            

          else  if(fromid == dc.tbltickets.Find(ticket_id).ticket_open_by_id)
            {
                
                var ss = dc.tbltickets.Find(ticket_id);
                ss.ticket_status = "Replied";
                if (task == "Yes")
                {
                    ss.ticket_status = "Done";

                }
                dc.SaveChanges();
                obj.ticket_status = "Replied";
            }
          else  if(fromid == 10001)
            {
                var ss = dc.tbltickets.Find(ticket_id);
                ss.ticket_status = "Replied";
                if (task == "Yes")
                {
                    ss.ticket_status = " Done";

                }
                dc.SaveChanges();
                obj.ticket_status = "Replied";

            }
        else
            {
                var ss = dc.tbltickets.Find(ticket_id);
                ss.ticket_status = "Replied";
                if (task == "Yes")
                {
                    ss.ticket_status = "Done";

                }
                dc.SaveChanges();
                obj.ticket_status = "Replied";

            }

            if(task == "Yes")
            {
                obj.ticket_status = "Done";

                try
                {
                    taskdoneemail(ticket_id);
                }
                catch( Exception ex)
                {

                }
                
                
            }

            dc.tblticketconverations.Add(obj);
            dc.SaveChanges();

            return "successfully ...";
        }

        public void taskdoneemail( int id)
        {

            var obj = dc.tbltickets.Find(id);
            var subject = "Closed Ticket Status";
      
            var emailid = "manang@lastmiled.com";
            var body = "Hi manan, Task for the Ticket# { "+obj.ticket_id+"} is DONE. The ticket needs to be CLOSED <a href = 'http://cftclients.com/Main/Login'> Click Open </a>";
           // sendfinalemail(emailid, subject, body);


        }
        [HttpPost]
        public string ClosedStatus(int id)
        {
            var c = dc.tbltickets.Find(id);
            var status = c.ticket_status;
            if(status!="Closed")
            {
                c.ticket_status = "Closed";
                c.ticket_closed_date = DateTime.Now;
                c.ticket_closed_time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                dc.SaveChanges();
                tblticketconveration obj = new tblticketconveration();
                obj.date = DateTime.Parse(DateTime.Now.ToString());
                obj.time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                obj.ticket_id = id;
                obj.ticket_status = "Closed";
                obj.from_user_id = int.Parse(Session["admin"].ToString());
                obj.to_user_id = int.Parse(Session["admin"].ToString());
                obj.ticket_conversation = "Problem fixed";
                
                dc.tblticketconverations.Add(obj);


                dc.SaveChanges();
                return "Ticket Closed Successfully ..";

            }
            else
            {

                return "Ticket Already Closed ..";

            }
           
        }

        public string ReOpen(int id)
        {
            var c = dc.tbltickets.Find(id);
            var status = c.ticket_status;
            if (status != "ReOpen")
            {
                c.ticket_status = "ReOpen";
                c.ticket_closed_date = DateTime.Now;
                c.ticket_closed_time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                dc.SaveChanges();
                tblticketconveration obj = new tblticketconveration();
                obj.date = DateTime.Parse(DateTime.Now.ToString());
                obj.time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                obj.ticket_id = id;
                obj.ticket_status = "ReOpen";
                obj.from_user_id = int.Parse(Session["admin"].ToString());
                obj.to_user_id = int.Parse(Session["admin"].ToString());
                obj.ticket_conversation = "Problem fixed";

                dc.tblticketconverations.Add(obj);
                
                dc.SaveChanges();

                var obj1 = dc.tbltickets.Find(id);
                var openby = "";
                if (obj1.ticket_open_by_id == 10001)
                {

                    openby = "admin";
                    var subject = "Ticket  ReOpen!!!";
                     if(obj1.ticket_assigned_user_id == 0)
                    {

                        var ss = dc.tblsubadmins.Where(l => l.user_type == "Yes").ToList();
                        foreach (var item in ss)
                        {
                            var username = dc.tblsubadmins.Find(item.sub_admin_id).sub_admin_name;
                            var emailid = dc.tblsubadmins.Find(item.sub_admin_id).sub_admin_email_id;
                            var body = "Dear " + username + " There is Ticket  Open for " + obj1.ticket_type + " Please Check More Detail To Login LMLD and this is open by " + openby;
                         //   sendfinalemail(emailid, subject, body);


                        }


                    }
                    else
                    {



                        var username = dc.tblsubadmins.Find(obj1.ticket_assigned_user_id).sub_admin_name;

                        var emailid = dc.tblsubadmins.Find(obj1.ticket_assigned_user_id).sub_admin_email_id;

                        var body = "Dear " + username + " There is Ticket  Open for " + obj1.ticket_type + " Please Check More Detail To Login LMLD and this is open by " + openby;
                       // sendfinalemail(emailid, subject, body);


                    }



                }
                else
                {
                    var subject = "Ticket  ReOpen!!!";
                    if (obj1.ticket_assigned_user_id == 0)
                    {
                        var ss = dc.tblsubadmins.Where(l => l.user_type == "Yes").ToList();
                        foreach (var item in ss)
                        {
                            var username = dc.tblsubadmins.Find(item.sub_admin_id).sub_admin_name;
                            var emailid = dc.tblsubadmins.Find(item.sub_admin_id).sub_admin_email_id;
                            var body = "Dear " + username + " There is Ticket  Open for " + obj1.ticket_type + " Please Check More Detail To Login LMLD and this is open by " + openby;
                          //  sendfinalemail(emailid, subject, body);
                            
                        }

                    }
                    else
                    {
                        openby = dc.tblsubadmins.Find(obj1.ticket_open_by_id).sub_admin_name;
                        //var subject = "Ticket  ReOpen!!!";
                        var username = dc.tblsubadmins.Find(obj1.ticket_assigned_user_id).sub_admin_name;
                        var emailid = dc.tblsubadmins.Find(obj1.ticket_assigned_user_id).sub_admin_email_id;
                        var body = "Dear " + username + " There is Ticket  Open for " + obj1.ticket_type + " Please Check More Detail To Login LMLD and this is open by " + openby;
                       // sendfinalemail(emailid, subject, body);

                    }




                }
               
                return "Ticket Reopen Again Successfully ..";

            }
            else
            {

                return "Ticket Already ReOpen Ticket..";

            }


        }

        public ActionResult TicketReAssign(int id)
        {
            getusername();

            
            
            Session["tid"] = id;

            return View();

        }

        [HttpPost]
        public ActionResult TicketReAssign(FormCollection fc)
        {
            var tid = int.Parse(Session["tid"].ToString());
            var ss =  int.Parse(fc["subadminid"]);
            var sst = dc.tbltickets.Find(tid);
            sst.ticket_assigned_user_id = ss;
            dc.SaveChanges();
            return RedirectToAction("ViewOpenTicket");


        }

        public ActionResult ClosedCoversation(int id)
        {

            Session["ticketId"] = id;

            return View();



        }

        public ActionResult duplicateview(string id)
        {
            var userid = int.Parse(Session["admin"].ToString());

            if (id == null)
            {

                if (userid == 10001)
                {
                    var p = (from n in dc.tbltickets
                             where n.ticket_status == "Open" || n.ticket_status == "Picked"

                             select new
                             {
                                 n.ticket_id,
                                 n.ticket_type,
                                 n.ticket_open_date,
                                 n.ticket_open_time,
                                 n.ticket_assigned_user_id,
                                 n.cat_phone_number,
                                 n.gas_card_number,
                                 n.issue,
                                 n.vendor_name,
                                 n.van_type,
                                 n.van_number,
                                 n.supply_needed,
                                 n.Quantity,
                                 n.ticket_open_by_id,
                                 n.ticket_status,
                                 n.ticket_closed_date,
                                 n.ticket_closed_time,
                                 n.ticket_picked_date,
                                 n.ticket_picked_time,
                                 n.mac_address
                             }).ToList();
                    List<tblticket> objlist = new List<tblticket>();

                    foreach (var item in p)
                    {
                        tblticket obj = new tblticket();
                        obj.ticket_id = item.ticket_id;
                        obj.ticket_type = item.ticket_type;
                        obj.ticket_status = item.ticket_status;
                        obj.ticket_open_date = item.ticket_open_date;
                        obj.ticket_open_time = item.ticket_open_time;
                        obj.ticket_closed_date = item.ticket_closed_date;
                        obj.ticket_closed_time = item.ticket_closed_time;
                        // obj.assignedusername = item.sub_admin_name;
                        obj.ticket_open_by_id = item.ticket_open_by_id;
                        obj.supply_needed = item.supply_needed;
                        obj.Quantity = item.Quantity;
                        obj.issue = item.issue;
                        obj.vendor_name = item.vendor_name;
                        obj.van_type = item.van_type;
                        obj.van_number = item.van_number;
                        obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                        obj.ticket_picked_date = item.ticket_picked_date;
                        obj.ticket_picked_time = item.ticket_picked_time;
                        obj.mac_address = item.mac_address;
                        obj.cat_phone_number = item.cat_phone_number;
                        obj.gas_card_number = item.gas_card_number;
                        objlist.Add(obj);
                        dc.SaveChanges();

                    }

                    return View(objlist);

                }
                else
                {
                    bool isdisp = false;

                    var isdispacher = dc.tblsubadmins.Where(c => c.sub_admin_id == userid).FirstOrDefault().user_type;
                    var secondid = 1;
                    if (isdispacher == "Yes")
                    {
                        isdisp = true;
                        secondid = 0;

                        var p = (from n in dc.tbltickets
                                 where n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid && (n.ticket_status == "Open" || n.ticket_status == "Picked")
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.gas_card_number,
                                     n.cat_phone_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        foreach (var item in p)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            obj.gas_card_number = item.gas_card_number;
                            obj.cat_phone_number = item.cat_phone_number;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }


                        return View(objlist);
                    }
                    else
                    {
                        secondid = 1;
                        isdisp = false;
                        var p = (from n in dc.tbltickets

                                 where
                                 n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid && (n.ticket_status == "Open")
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.gas_card_number,
                                     n.cat_phone_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        foreach (var item in p)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            obj.gas_card_number = item.gas_card_number;
                            obj.cat_phone_number = item.cat_phone_number;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }

                        return View(objlist);
                    }






                }
            }

            else if (id == "All")
            {
                if (userid == 10001)
                {
                    var p = (from n in dc.tbltickets


                             select new
                             {
                                 n.ticket_id,
                                 n.ticket_type,
                                 n.ticket_open_date,
                                 n.ticket_open_time,
                                 n.ticket_assigned_user_id,
                                 n.gas_card_number,
                                 n.cat_phone_number,
                                 n.issue,
                                 n.vendor_name,
                                 n.van_type,
                                 n.van_number,
                                 n.supply_needed,
                                 n.Quantity,
                                 n.ticket_open_by_id,
                                 n.ticket_status,
                                 n.ticket_closed_date,
                                 n.ticket_closed_time,
                                 n.ticket_picked_date,
                                 n.ticket_picked_time,
                                 n.mac_address
                             }).ToList();
                    List<tblticket> objlist = new List<tblticket>();

                    foreach (var item in p)
                    {
                        tblticket obj = new tblticket();
                        obj.ticket_id = item.ticket_id;
                        obj.ticket_type = item.ticket_type;
                        obj.ticket_status = item.ticket_status;
                        obj.ticket_open_date = item.ticket_open_date;
                        obj.ticket_open_time = item.ticket_open_time;
                        obj.ticket_closed_date = item.ticket_closed_date;
                        obj.ticket_closed_time = item.ticket_closed_time;
                        // obj.assignedusername = item.sub_admin_name;
                        obj.ticket_open_by_id = item.ticket_open_by_id;
                        obj.supply_needed = item.supply_needed;
                        obj.Quantity = item.Quantity;
                        obj.issue = item.issue;
                        obj.vendor_name = item.vendor_name;
                        obj.van_type = item.van_type;
                        obj.van_number = item.van_number;
                        obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                        obj.ticket_picked_date = item.ticket_picked_date;
                        obj.ticket_picked_time = item.ticket_picked_time;
                        obj.mac_address = item.mac_address;
                        obj.gas_card_number = item.gas_card_number;
                        obj.cat_phone_number = item.cat_phone_number;
                        objlist.Add(obj);
                        dc.SaveChanges();

                    }

                    return View(objlist);

                }
                else
                {
                    bool isdisp = false;

                    var isdispacher = dc.tblsubadmins.Where(c => c.sub_admin_id == userid).FirstOrDefault().user_type;
                    var secondid = 1;
                    if (isdispacher == "Yes")
                    {
                        isdisp = true;
                        secondid = 0;

                        var p = (from n in dc.tbltickets
                                 where n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.cat_phone_number,
                                     n.gas_card_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        foreach (var item in p)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            obj.gas_card_number = item.gas_card_number;
                            obj.cat_phone_number = item.cat_phone_number;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }


                        return View(objlist);
                    }
                    else
                    {
                        secondid = 1;
                        isdisp = false;
                        var p = (from n in dc.tbltickets

                                 where
                                 n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.cat_phone_number,
                                     n.gas_card_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        foreach (var item in p)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            obj.gas_card_number = item.gas_card_number;
                            obj.cat_phone_number = item.cat_phone_number;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }

                        return View(objlist);
                    }






                }

            }
            else
            {
                if (userid == 10001)
                {
                    var p = (from n in dc.tbltickets
                             where n.ticket_status == id

                             select new
                             {
                                 n.ticket_id,
                                 n.ticket_type,
                                 n.ticket_open_date,
                                 n.ticket_open_time,
                                 n.ticket_assigned_user_id,
                                 n.cat_phone_number,
                                 n.gas_card_number,

                                 n.issue,
                                 n.vendor_name,
                                 n.van_type,
                                 n.van_number,
                                 n.supply_needed,
                                 n.Quantity,
                                 n.ticket_open_by_id,
                                 n.ticket_status,
                                 n.ticket_closed_date,
                                 n.ticket_closed_time,
                                 n.ticket_picked_date,
                                 n.ticket_picked_time,
                                 n.mac_address
                             }).ToList();
                    List<tblticket> objlist = new List<tblticket>();

                    foreach (var item in p)
                    {
                        tblticket obj = new tblticket();
                        obj.ticket_id = item.ticket_id;
                        obj.ticket_type = item.ticket_type;
                        obj.ticket_status = item.ticket_status;
                        obj.ticket_open_date = item.ticket_open_date;
                        obj.ticket_open_time = item.ticket_open_time;
                        obj.ticket_closed_date = item.ticket_closed_date;
                        obj.ticket_closed_time = item.ticket_closed_time;
                        // obj.assignedusername = item.sub_admin_name;
                        obj.ticket_open_by_id = item.ticket_open_by_id;
                        obj.supply_needed = item.supply_needed;
                        obj.Quantity = item.Quantity;
                        obj.issue = item.issue;
                        obj.vendor_name = item.vendor_name;
                        obj.van_type = item.van_type;
                        obj.van_number = item.van_number;
                        obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                        obj.ticket_picked_date = item.ticket_picked_date;
                        obj.ticket_picked_time = item.ticket_picked_time;
                        obj.mac_address = item.mac_address;
                        obj.gas_card_number = item.gas_card_number;
                        obj.cat_phone_number = item.cat_phone_number;
                        objlist.Add(obj);
                        dc.SaveChanges();

                    }

                    return View(objlist);

                }
                else
                {
                    bool isdisp = false;

                    var isdispacher = dc.tblsubadmins.Where(c => c.sub_admin_id == userid).FirstOrDefault().user_type;
                    var secondid = 1;
                    if (isdispacher == "Yes")
                    {
                        isdisp = true;
                        secondid = 0;

                        var p = (from n in dc.tbltickets
                                 where n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid && (n.ticket_status == id)
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.gas_card_number,
                                     n.cat_phone_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        foreach (var item in p)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            obj.cat_phone_number = item.cat_phone_number;
                            obj.gas_card_number = item.gas_card_number;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }


                        return View(objlist);
                    }
                    else
                    {
                        secondid = 1;
                        isdisp = false;
                        var p = (from n in dc.tbltickets

                                 where
                                 n.ticket_open_by_id == userid || n.ticket_assigned_user_id == userid || n.ticket_assigned_user_id == secondid && (n.ticket_status == id)
                                 select new
                                 {
                                     n.ticket_id,
                                     n.ticket_type,
                                     n.ticket_open_date,
                                     n.ticket_open_time,
                                     n.ticket_assigned_user_id,
                                     n.gas_card_number,
                                     n.cat_phone_number,
                                     n.issue,
                                     n.vendor_name,
                                     n.van_type,
                                     n.van_number,
                                     n.supply_needed,
                                     n.Quantity,
                                     n.ticket_open_by_id,
                                     n.ticket_status,
                                     n.ticket_closed_date,
                                     n.ticket_closed_time,
                                     n.ticket_picked_date,
                                     n.ticket_picked_time,
                                     n.mac_address
                                 }).ToList();


                        List<tblticket> objlist = new List<tblticket>();

                        foreach (var item in p)
                        {
                            tblticket obj = new tblticket();
                            obj.ticket_id = item.ticket_id;
                            obj.ticket_type = item.ticket_type;
                            obj.ticket_status = item.ticket_status;
                            obj.ticket_open_date = item.ticket_open_date;
                            obj.ticket_open_time = item.ticket_open_time;
                            obj.ticket_closed_date = item.ticket_closed_date;
                            obj.ticket_closed_time = item.ticket_closed_time;
                            // obj.assignedusername = item.sub_admin_name;
                            obj.ticket_open_by_id = item.ticket_open_by_id;
                            obj.supply_needed = item.supply_needed;
                            obj.Quantity = item.Quantity;
                            obj.issue = item.issue;
                            obj.vendor_name = item.vendor_name;
                            obj.van_type = item.van_type;
                            obj.van_number = item.van_number;
                            obj.ticket_assigned_user_id = item.ticket_assigned_user_id;
                            obj.ticket_picked_date = item.ticket_picked_date;
                            obj.ticket_picked_time = item.ticket_picked_time;
                            obj.mac_address = item.mac_address;
                            obj.gas_card_number = item.gas_card_number;
                            obj.cat_phone_number = item.cat_phone_number;
                            objlist.Add(obj);
                            dc.SaveChanges();

                        }

                        return View(objlist);
                    }






                }

            }





        }

        public ActionResult getvaninfo(int id)
        {

            var p = dc.tbltickets.Where(c => c.ticket_id == id).FirstOrDefault();


            return View(p);

        }

        public ActionResult getcatinfo(int id)
        {
            var p = dc.tbltickets.Where(c => c.ticket_id == id).FirstOrDefault();


            return View(p);

        }
        public ActionResult getgascard(int id)
        {
            var p = dc.tbltickets.Where(c => c.ticket_id == id).FirstOrDefault();


            return View(p);

        }
        public  ActionResult getbussnesssuplied(int id)
        {
            var p = dc.tbltickets.Where(c => c.ticket_id == id).FirstOrDefault();


            return View(p);
        }

        public string ticketclose(int ticket_id)
        {

            var c = dc.tbltickets.Find(ticket_id);
            var status = c.ticket_status;
            if (status != "Closed")
            {
                c.ticket_status = "Closed";
                c.ticket_closed_date = DateTime.Now;
                c.ticket_closed_time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                dc.SaveChanges();
                tblticketconveration obj = new tblticketconveration();
                obj.date = DateTime.Parse(DateTime.Now.ToString());
                obj.time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                obj.ticket_id = ticket_id;
                obj.ticket_status = "Closed";
                obj.from_user_id = int.Parse(Session["admin"].ToString());
                obj.to_user_id = int.Parse(Session["admin"].ToString());
                obj.ticket_conversation = "Problem fixed";

                dc.tblticketconverations.Add(obj);
           
                dc.SaveChanges();
                return "Ticket Closed Successfully ..";

            }
            else
            {

                return "Ticket Already Closed ..";

            }
        }

    }
}