using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deliverymangementsystem.EDM;
using System.Data.Entity;
using deliverymangementsystem.Models;
using System.Net.Http;
using System.Globalization;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.ComponentModel;
using System.Data.Entity;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Web.Services;
using System.Net.Mime;

namespace deliverymangementsystem.Controllers
{

    public class AdminController : Controller
    {
        //dsdatabaseEntities dc = new dsdatabaseEntities();
        dsdatabaseEntities dc = new dsdatabaseEntities();
        public string conString = ConfigurationManager.ConnectionStrings["dsdatabaseEntities"].ConnectionString;

        //export to excel interviewlist

        //create source form


        public ActionResult scheduleTraningListPopup(int id)
        {

            var c = dc.tbldrivers.Find(id).emp_id;

            var k = dc.tblonbordingmasters.Find(c);

            return View(k);
        }

        public void getdriverinfo()
        {

            var p = dc.tblfinaldrivermasters.Where(c => c.driver_status == "active").OrderBy(c => c.final_driver_name).ToList();

            List<SelectListItem> objlist = new List<SelectListItem>();

            foreach (var item in p)
            {
                SelectListItem ss = new SelectListItem();
                ss.Text = item.final_driver_name;
                ss.Value = item.final_driver_id.ToString();
                objlist.Add(ss);
            }

            ViewData["dlist"] = objlist;
        }

        public ActionResult updateTransportID()
        {

            getdriverinfo();
            var pp = dc.tblfinaldrivermasters.Where(c => c.driver_status == "active").OrderBy(c => c.final_driver_name).ToList();
            ViewData["dipdata"] = pp;

            return View();

        }

        [HttpPost]
        public ActionResult updateTransportID(FormCollection fc)
        {
            var d_id = int.Parse(fc["d_id"]);

            var tranport = fc["tranportID"];
            var p = dc.tblfinaldrivermasters.Where(c => c.final_driver_id == d_id).FirstOrDefault();

            p.transport_id = tranport;

            dc.SaveChanges();
            return RedirectToAction("updateTransportID");

        }

        public string Rollback(int id)
        {
            var dd = dc.tbldrivers.Find(id);
            var emp_id = dd.emp_id;
            dc.tbldrivers.Remove(dd);
            dc.SaveChanges();
            var stp = dc.tblonbordingmasters.Find(emp_id);
            stp.Entry_status = "ApplicationSent";
            dc.SaveChanges();
            AddRemarks(emp_id.ToString(), "Applicant moved back to application submitted list", "Job Offer List");
            return "Rollback Successfully Done";


        }

        [HttpPost]
        public ActionResult scheduleTraningListPopup(FormCollection fc, HttpPostedFileBase linc_file)
        {
            var emp_id = int.Parse(fc["emp_id1"]);
            if (fc["generatePDF"] == null)
            {
                //var S = fc["SourceOfInquiry"];
                //var SOI = dc.tblsources.Where(s => s.source_name == S).ToList();


                var obj = dc.tblonbordingmasters.Find(emp_id);

                //if (SOI.Count == 0)
                //{
                //    tblsource objtblsource = new tblsource();
                //    objtblsource.source_name = fc["SourceOfInquiry"];

                //    dc.tblsources.Add(objtblsource);
                //    dc.SaveChanges();

                //    var SOI_id = dc.tblsources.Where(e => e.source_name == S).ToList();
                //    obj.source_id = Convert.ToInt32(SOI_id.FirstOrDefault().source_id);
                //}

                ////var obj = dc.tblonbordingmasters.Find(emp_id);
                ////obj.Entry_status = "ApplicationSent";
                ////obj.emp_inteview_status = "ApplicationSent";
                //obj.emp_name = fc["fname"];
                //obj.emp_phone = fc["mobile"];
                //obj.emp_personal_email = fc["email"];
                //// obj.degination_name = fc["dob"];
                ////obj.emp_dob = DateTime.Parse(fc["dob"]);
                //obj.emp_add = fc["addr"];
                //obj.city = fc["city"];
                //obj.state = fc["statetemp"];
                //obj.zip = fc["zip"];
                //obj.are_you_us_city = fc["iscitizenus"];
                ////obj.are_you_legal_us_city = fc["islegallyallowed"];
                ////obj.drug_test = fc["submitdrugtest"];
                ////obj.Convicted_felony = fc["isconvicted"];
                ////obj.Convicted_felony_explain = fc["convictiondetails"];
                //obj.work_sat = fc["availablesaturday"];
                //obj.work_sat_explain = fc["saturdaydetails"];
                //obj.work_sun = fc["availablesunday"];
                //obj.work_sun_explain = fc["sundaydetails"];
                ////obj.ticket_point = fc["isdrivingticket"];
                ////obj.count = fc["ticketcount"];
                ////obj.driver_linc = fc["dl"];
                ////obj.applied_pos = fc["position"];
                //obj.available_startdate = DateTime.Parse(fc["startdate"]);
                //obj.desired_Pay = fc["desiredpay"];
                //obj.employment_desired = fc["employmentdesired"];
                //obj.school_name1 = fc["schoolname1"];
                //obj.school_name2 = fc["schoolname2"];
                //obj.location1 = fc["location1"];
                //obj.location2 = fc["location2"];
                //obj.year_attended1 = fc["yearsattended1"];
                //obj.year_attended2 = fc["yearsattended2"];
                //obj.major1 = fc["major1"];
                //obj.major2 = fc["major2"];
                //obj.employer1 = fc["employer1"];
                //obj.employer2 = fc["employer2"];
                //obj.jobtitle1 = fc["jobtitle1"];
                //obj.jobtitle2 = fc["jobtitle2"];
                //obj.state1 = fc["state1"];
                //obj.state2 = fc["state2"];
                //obj.work_phone1 = fc["workphone1"];
                //obj.work_phone2 = fc["workphone2"];
                //obj.zip1 = fc["zip1"];
                //obj.zip2 = fc["zip2"];
                //obj.address1 = fc["address1"];
                //obj.address2 = fc["address2"];
                //obj.remarks1 = fc["remak1"];
                //obj.remark2 = fc["remak2"];

                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);


                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);


                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);



                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);

                //if (fc["IsLeagalyAuthorized"].ToString().Trim() != "")
                //{
                //    string[] s = fc["IsLeagalyAuthorized"].Split(',');
                //    obj.IsLeagalyAuthorized = s[1].ToString();
                //}
                //else
                //{
                //    obj.IsLeagalyAuthorized = fc["IsLeagalyAuthorized"];
                //}

                //if (fc["IsSponShipRequired"].ToString().Trim() != "")
                //{
                //    string[] s = fc["IsSponShipRequired"].Split(',');
                //    obj.IsSponShipRequired = s[1].ToString();
                //}
                //else
                //{
                //    obj.IsSponShipRequired = fc["IsSponShipRequired"];
                //}


                //obj.WorkPerformed1 = fc["WorkPerformed1"];
                //obj.WorkPerformed2 = fc["WorkPerformed2"];
                //obj.employer3 = fc["employer3"];
                //obj.jobtitle3 = fc["jobtitle3"];
                //obj.work_phone3 = fc["workphone3"];
                //obj.starting_pay_rate3 = fc["startpayingrate3"];
                //obj.ending_pay_rate3 = fc["endpayrate3"];
                //obj.address3 = fc["address3"];
                //obj.city3 = fc["city3"];
                //obj.state3 = fc["state3"];
                //obj.zip3 = fc["zip3"];
                //obj.remarks3 = fc["remak3"];
                //obj.WorkPerformed3 = fc["WorkPerformed3"];
                //obj.degree_rec1 = fc["degreereceived1"];
                //obj.degree_rec2 = fc["degreereceived2"];

                ////obj.FinalChk1 = fc["FinalChk1"];
                ////obj.FinalChk2 = fc["FinalChk2"];
                ////obj.FinalChk3 = fc["FinalChk3"];

                //obj.empstartdate1 = fc["empstartdate1"];
                //obj.empstartdate2 = fc["empstartdate2"];
                //obj.empstartdate3 = fc["empstartdate3"];

                //obj.empenddate1 = fc["empenddate1"];
                //obj.empenddate2 = fc["empenddate2"];
                //obj.empenddate3 = fc["empenddate3"];



                if (linc_file != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filePath = path + Path.GetFileName(linc_file.FileName);
                    string fileName = Path.GetFileName(linc_file.FileName).Split('.')[0];
                    string extension = Path.GetExtension(linc_file.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        linc_file.SaveAs(filePath);
                        obj.upload_linces_photo = linc_file.FileName;
                        dc.SaveChanges();
                    }
                    else
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            int index = 1;

                            string finalFileName = fileName + "_" + index + extension;

                            while (System.IO.File.Exists(path + finalFileName))
                            {
                                finalFileName = fileName + "_" + ++index + extension;
                            }

                            string finalFilePath = path + finalFileName;

                            linc_file.SaveAs(finalFilePath);
                            obj.upload_linces_photo = finalFileName;
                            dc.SaveChanges();
                        }
                    }
                }

                var rem = fc["ApplicantRemarks"];

                if (rem.ToString().Trim() != "")
                {
                    AddRemarks(emp_id.ToString(), rem, "Scheduled Driver Training List");
                }


            }
            return RedirectToAction("ScheduleTraningList");


        }

        public ActionResult applicationdata(int id)
        {
            var p = dc.tblonbordingmasters.Find(id);


            return View(p);
        }

        [HttpGet]
        public ActionResult JobofferListdataPopup(int id)
        {
            var p = dc.tbldrivers.Find(id).emp_id;

            var k = dc.tblonbordingmasters.Find(p);

            return View(k);

        }

        [HttpPost]
        public ActionResult JobofferListdataPopup(FormCollection fc, HttpPostedFileBase linc_file)
        {

            var emp_id = int.Parse(fc["emp_id1"]);

            if (fc["generatePDF"] == null)
            {

                //var S = fc["SourceOfInquiry"];
                //var SOI = dc.tblsources.Where(s => s.source_name == S).ToList();


                var obj = dc.tblonbordingmasters.Find(emp_id);

                //if (SOI.Count == 0)
                //{
                //    tblsource objtblsource = new tblsource();
                //    objtblsource.source_name = fc["SourceOfInquiry"];

                //    dc.tblsources.Add(objtblsource);
                //    dc.SaveChanges();

                //    var SOI_id = dc.tblsources.Where(e => e.source_name == S).ToList();
                //    obj.source_id = Convert.ToInt32(SOI_id.FirstOrDefault().source_id);
                //}

                ////var obj = dc.tblonbordingmasters.Find(emp_id);
                ////obj.Entry_status = "ApplicationSent";
                ////obj.emp_inteview_status = "ApplicationSent";
                //obj.emp_name = fc["fname"];
                //obj.emp_phone = fc["mobile"];
                //obj.emp_personal_email = fc["email"];
                //// obj.degination_name = fc["dob"];
                ////obj.emp_dob = DateTime.Parse(fc["dob"]);
                //obj.emp_add = fc["addr"];
                //obj.city = fc["city"];
                //obj.state = fc["statetemp"];
                //obj.zip = fc["zip"];
                //obj.are_you_us_city = fc["iscitizenus"];
                ////obj.are_you_legal_us_city = fc["islegallyallowed"];
                ////obj.drug_test = fc["submitdrugtest"];
                ////obj.Convicted_felony = fc["isconvicted"];
                ////obj.Convicted_felony_explain = fc["convictiondetails"];
                //obj.work_sat = fc["availablesaturday"];
                //obj.work_sat_explain = fc["saturdaydetails"];
                //obj.work_sun = fc["availablesunday"];
                //obj.work_sun_explain = fc["sundaydetails"];
                ////obj.ticket_point = fc["isdrivingticket"];
                ////obj.count = fc["ticketcount"];
                ////obj.driver_linc = fc["dl"];
                ////obj.applied_pos = fc["position"];
                //obj.available_startdate = DateTime.Parse(fc["startdate"]);
                //obj.desired_Pay = fc["desiredpay"];
                //obj.employment_desired = fc["employmentdesired"];
                //obj.school_name1 = fc["schoolname1"];
                //obj.school_name2 = fc["schoolname2"];
                //obj.location1 = fc["location1"];
                //obj.location2 = fc["location2"];
                //obj.year_attended1 = fc["yearsattended1"];
                //obj.year_attended2 = fc["yearsattended2"];
                //obj.major1 = fc["major1"];
                //obj.major2 = fc["major2"];
                //obj.employer1 = fc["employer1"];
                //obj.employer2 = fc["employer2"];
                //obj.jobtitle1 = fc["jobtitle1"];
                //obj.jobtitle2 = fc["jobtitle2"];
                //obj.state1 = fc["state1"];
                //obj.state2 = fc["state2"];
                //obj.work_phone1 = fc["workphone1"];
                //obj.work_phone2 = fc["workphone2"];
                //obj.zip1 = fc["zip1"];
                //obj.zip2 = fc["zip2"];
                //obj.address1 = fc["address1"];
                //obj.address2 = fc["address2"];
                //obj.remarks1 = fc["remak1"];
                //obj.remark2 = fc["remak2"];

                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);


                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);



                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);


                //if (fc["IsLeagalyAuthorized"].ToString().Trim() != "")
                //{
                //    string[] s = fc["IsLeagalyAuthorized"].Split(',');
                //    obj.IsLeagalyAuthorized = s[1].ToString();
                //}
                //else
                //{
                //    obj.IsLeagalyAuthorized = fc["IsLeagalyAuthorized"];
                //}

                //if (fc["IsSponShipRequired"].ToString().Trim() != "")
                //{
                //    string[] s = fc["IsSponShipRequired"].Split(',');
                //    obj.IsSponShipRequired = s[1].ToString();
                //}
                //else
                //{
                //    obj.IsSponShipRequired = fc["IsSponShipRequired"];
                //}

                //obj.WorkPerformed1 = fc["WorkPerformed1"];
                //obj.WorkPerformed2 = fc["WorkPerformed2"];
                //obj.employer3 = fc["employer3"];
                //obj.jobtitle3 = fc["jobtitle3"];
                //obj.work_phone3 = fc["workphone3"];
                //obj.starting_pay_rate3 = fc["startpayingrate3"];
                //obj.ending_pay_rate3 = fc["endpayrate3"];
                //obj.address3 = fc["address3"];
                //obj.city3 = fc["city3"];
                //obj.state3 = fc["state3"];
                //obj.zip3 = fc["zip3"];
                //obj.remarks3 = fc["remak3"];
                //obj.WorkPerformed3 = fc["WorkPerformed3"];
                //obj.degree_rec1 = fc["degreereceived1"];
                //obj.degree_rec2 = fc["degreereceived2"];

                ////obj.FinalChk1 = fc["FinalChk1"];
                ////obj.FinalChk2 = fc["FinalChk2"];
                ////obj.FinalChk3 = fc["FinalChk3"];

                //obj.empstartdate1 = fc["empstartdate1"];
                //obj.empstartdate2 = fc["empstartdate2"];
                //obj.empstartdate3 = fc["empstartdate3"];

                //obj.empenddate1 = fc["empenddate1"];
                //obj.empenddate2 = fc["empenddate2"];
                //obj.empenddate3 = fc["empenddate3"];


                if (linc_file != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filePath = path + Path.GetFileName(linc_file.FileName);
                    string fileName = Path.GetFileName(linc_file.FileName).Split('.')[0];
                    string extension = Path.GetExtension(linc_file.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        linc_file.SaveAs(filePath);
                        obj.upload_linces_photo = linc_file.FileName;
                        dc.SaveChanges();
                    }
                    else
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            int index = 1;

                            string finalFileName = fileName + "_" + index + extension;

                            while (System.IO.File.Exists(path + finalFileName))
                            {
                                finalFileName = fileName + "_" + ++index + extension;
                            }

                            string finalFilePath = path + finalFileName;

                            linc_file.SaveAs(finalFilePath);
                            obj.upload_linces_photo = finalFileName;
                            dc.SaveChanges();
                        }
                    }
                }

                var rem = fc["ApplicantRemarks"];

                if (rem.ToString().Trim() != "")
                {
                    AddRemarks(emp_id.ToString(), rem, "Job Offer List");
                }

            }
            return RedirectToAction("JobOfferList");


        }

        public ActionResult addnewsource()
        {
            return View();


        }

        [HttpPost]
        public ActionResult addnewsource(tblsource obj)
        {
            dc.tblsources.Add(obj);

            dc.SaveChanges();

            return RedirectToAction("AddNewEntry");

        }

        public ActionResult displaysource()
        {


            return View(dc.tblsources.ToList());

        }

        public ActionResult editsource(int id)
        {
            var k = dc.tblsources.Find(id);
            return View(k);

        }

        [HttpPost]
        public ActionResult editsource(tblsource obj)
        {
            dc.Entry(obj).State = EntityState.Modified;
            dc.SaveChanges();
            return RedirectToAction("AddNewEntry");

        }

        public string deletesource(int id)
        {
            var c = dc.tblsources.Find(id);
            dc.tblsources.Remove(c);
            dc.SaveChanges();
            return "success";


        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Home()
        {
            Session["chklist"] = "";

            return View();

        }

        public void getlistdata()
        {
            List<SelectListItem> objlist = new List<SelectListItem> { new SelectListItem { Text = "Driver", Value = "Driver", Selected = true }, new SelectListItem { Text = "HR", Value = "HR", Disabled = true }, new SelectListItem { Text = "Other", Value = "Other", Disabled = true } };

            ViewBag.data = objlist;

        }

        public void getsources()
        {
            //var p = dc.tblsources.ToList();
            //var p = dc.tblsources.Where(w => w.source_name == "Indeed" || w.source_name == "Talentify" || w.source_name == "Driver Recommendation" || w.source_name == "Amazon Event").ToList();
            var p = dc.tblsources.Where(w => w.source_name == "Indeed" || w.source_name == "Talentify" || w.source_name == "Driver Recommendation" || w.source_name == "Amazon Event" || w.source_name == "Other").ToList();

            List<SelectListItem> objlist = new List<SelectListItem>();
            foreach (var item in p)
            {

                SelectListItem obj = new SelectListItem();
                obj.Text = item.source_name;
                obj.Value = item.source_id.ToString();
                objlist.Add(obj);

            }

            ViewData["source"] = objlist;

        }

        public void editgetlistdata(string strname)
        {
            List<SelectListItem> objlist = new List<SelectListItem> { new SelectListItem { Text = "Driver", Value = "Driver" }, new SelectListItem { Text = "HR", Value = "HR" }, new SelectListItem { Text = "Other", Value = "Other" } };

            List<SelectListItem> test = new List<SelectListItem>();

            foreach (var item in objlist)
            {

                if (item.Text == strname)
                {
                    item.Selected = true;
                    test.Add(item);

                }
                else
                {
                    // item.Selected = true;
                    test.Add(item);


                }

            }
            ViewBag.data = test;

        }

        public void AddRemarks(string EmpID, string Entry_Remark, string PageName)
        {

            tblDriverRemark objtblDrvRem = new tblDriverRemark();
            objtblDrvRem.EmpID = Convert.ToInt32(EmpID);
            objtblDrvRem.RemarkPageName = PageName;
            objtblDrvRem.Remarks = Entry_Remark;

            //var RemarkDate = DateTime.Now.Date;
            //var RemarkTime = DateTime.Now.ToShortTimeString();

            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

            objtblDrvRem.RemarkDate = easternTime.Date;
            objtblDrvRem.RemarkTime = easternTime.ToShortTimeString();
            var username = Session["username"];
            objtblDrvRem.RemarkBy = username == null ? "Applicant" : username.ToString();
            objtblDrvRem.RemarkBy_ID = Session["admin"] != null ? Convert.ToInt32(Session["admin"].ToString()) : 0;

            dc.tblDriverRemarks.Add(objtblDrvRem);
            dc.SaveChanges();

        }

        public ActionResult AddNewEntry()
        {
            getlistdata();
            getsources();
            // alerfun();
            //ViewBag.msgbulk = Session["msgbulk"];
            //Session["msgbulk"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult AddNewEntry(tblonbordingmaster obj, FormCollection fc)
        {
            var DAname = fc["emp_name"].ToString();
            var DAnumber = fc["emp_phone"].ToString();

            var S = fc["SourceOfInquiry"];
            var SOI = dc.tblsources.Where(s => s.source_name == S).ToList();

            if (SOI.Count == 0)
            {
                tblsource objtblsource = new tblsource();
                objtblsource.source_name = fc["SourceOfInquiry"];

                dc.tblsources.Add(objtblsource);
                dc.SaveChanges();

                var SOI_id = dc.tblsources.Where(e => e.source_name == S).ToList();
                obj.source_id = Convert.ToInt32(SOI_id.FirstOrDefault().source_id);
            }

            var p = dc.tblonbordingmasters.Where(w => w.emp_name == DAname && w.emp_phone == DAnumber).ToList();

            if (p.Count() == 0)
            {
                var EmpID = obj.emp_id;
                var Entry_Remark = fc["Entry_Remark"];
                if (obj.emp_inteview_status == "No")
                {
                    obj.entry_date = DateTime.Now.Date;
                    obj.role_id = int.Parse(Session["admin"].ToString());
                    obj.Entry_status = "Pending";
                    obj.Entry_Remark = Entry_Remark;
                    obj.amazon_description = Entry_Remark;
                    obj.RecomDriver = fc["RecomDriver"] != "" ? fc["RecomDriver"].ToString() : "";
                    // obj.emp_interview_date = DateTime.Parse(fc["emp_interview_date"]);
                    dc.tblonbordingmasters.Add(obj);
                    dc.SaveChanges();
                    ViewBag.msg = "Candidate Successfully Added In your System";
                    ModelState.Clear();
                }
                else
                {
                    var time = DateTime.Parse(fc["emp_interview_time"]);
                    var dd = time.ToString("HH:mm");
                    var ss = TimeSpan.Parse(dd.ToString());
                    obj.emp_interview_time = TimeSpan.Parse(dd.ToString());
                    obj.entry_date = DateTime.Now.Date;
                    //  var ss = fc["emp_interview_date"];
                    //obj.emp_interview_date = DateTime.ParseExact(ss, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    obj.role_id = int.Parse(Session["admin"].ToString());
                    // obj.emp_interview_date = DateTime.Parse(fc["emp_interview_date"]);
                    obj.Entry_status = "Schedule";
                    obj.Entry_Remark = Entry_Remark;
                    obj.amazon_description = Entry_Remark;
                    obj.RecomDriver = fc["RecomDriver"] != "" ? fc["RecomDriver"].ToString() : "";
                    dc.tblonbordingmasters.Add(obj);
                    dc.SaveChanges();

                    var uploadedApplicant = dc.tblonbordingmasters.Where(w => w.emp_name == DAname && w.emp_phone == DAnumber).ToList();
                    var emp_id = uploadedApplicant.FirstOrDefault().emp_id;
                    var Host = Request.Url.Host.ToString();
                    if (obj.emp_personal_email != null)
                    {
                        var applicationformLink = "Application Form link - " + Host + "/Admin/ApplicatinForm/" + emp_id;
                        var subline = "Last Mile Deliveries LLC (LMDL) - Interview Confirmation -  " + obj.emp_interview_date.Value.ToShortDateString() + " at " + fc["emp_interview_time"];
                        var body = "Hello " + obj.emp_name + ",<br/><br/>" +
                                   "Thank you for your interest in Delivery Associate position with Last Mile Deliveries LLC (LMDL), an Amazon Delivery Service Partner (DSP). <br/><br/>" +
                                   "This email confirms your interview on " + obj.emp_interview_date.Value.ToShortDateString() + " at " + fc["emp_interview_time"] + ".<br/>" +
                                   "Please click on below link to fill an application form and receive further instructions for the interview. <br/>" +
                                   applicationformLink + "<br/><br/>" +
                                   "Please contact HR @ 732-648-4674 or hr@lastmiled.com for any questions.<br/>" +
                                   "We look forward to meeting with you.<br/><br/>" +
                                   //"Thank you,<br/>" +
                                   "Last Mile Deliveries LLC (LMDL)  <br/>" +

                        //"<img alt=\"LMDL icon\" src=\"data:image/png;base64,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\" width=\"70\" height=\"70\" />";
                        "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";
                        //"Please do not reply to this email.<br/>" +
                        //"It was sent from an address that cannot accept incoming messages.<br/>";
                        string ccMail = ConfigurationManager.AppSettings["CCEmailIDHR"].ToString();
                        SendEmail_Fixed(obj.emp_personal_email.ToString(), ccMail, subline.ToString(), body);

                        AddRemarks(emp_id.ToString(), Host + "/Admin/ApplicatinForm/" + emp_id, "AddNewEntry");

                    }
                    if (obj.emp_phone != null)
                    {
                        var smsm = "Your Interview Scheduled on date " + obj.emp_interview_date.Value.ToShortDateString() + " and " + fc["emp_interview_time"] + " for the position of Delivery Associate at location reply with Yes to confirm";
                        //sendsms(obj.emp_phone, smsm);
                    }
                    ViewBag.msg = "Interview scheduled successfully..";
                    ModelState.Clear();
                    AddRemarks(obj.emp_id.ToString(), Entry_Remark, "AddNewEntry");
                }
                

            }
            else
            {
                ViewBag.msg = "Duplicate driver name found !!!";
            }

            getlistdata();
            getsources();
            // alerfun();
            return View();

        }

        public void alerfun()
        {
            ViewData["msg"] = "<script> $.alert({ icon: 'fa fa-spinner fa-spin',title: 'Working!',content: 'Sit back, we are processing your request. <br>The animated icon is provided by Font-Awesome!'});<script>>";
        }

        public ActionResult ScheduleIntrviewList(string id)
        {
            ViewBag.Status = "";
            if (id == "All")
            {
                var p = dc.tblonbordingmasters.Where(c => c.Entry_status == "Schedule" || c.Entry_status == "Reschedule" || c.Entry_status == "Notshow").OrderBy(c => c.emp_interview_date).ToList();
                return View(p);
            }
            else if (id == "Pending")
            {
                ViewBag.Status = id;
                //var p = dc.tblonbordingmasters.Where(c => c.emp_inteview_status == "No").OrderByDescending(c => c.emp_interview_date).ToList();
                var p = dc.tblonbordingmasters.Where(c => c.emp_inteview_status == "No" || c.Entry_status == "Waitlist").OrderByDescending(c => c.emp_interview_date).ToList();
                return View(p);
            }
            else if (id == "Rejected")
            {
                ViewBag.Status = "Rejected";
                var p = dc.tblonbordingmasters.Where(c => c.Entry_status == "Rejected" || c.Entry_status == "NoShow" || c.Entry_status == "WithdrawApplication").OrderByDescending(c => c.emp_interview_date).ToList();
                return View(p);
            }
            else if (id == null)
            {

                var p = dc.tblonbordingmasters.Where(c => c.Entry_status == "Schedule" || c.Entry_status == "Reschedule").OrderByDescending(c => c.emp_interview_date).ToList();
                return View(p);
            }
            else
            {
                var p = dc.tblonbordingmasters.Where(c => c.Entry_status == id).OrderBy(c => c.emp_interview_date).ToList();

                return View(p);
            }

        }

        public ActionResult applicationSubmitedList()
        {
            var p = dc.tblonbordingmasters.Where(c => c.Entry_status == "ApplicationSent" || c.Entry_status == "ApplicationPending").OrderByDescending(o => o.entry_date).ToList();
            return View(p);
        }

        public ActionResult PendingIntrviewList()
        {
            var p = dc.tblonbordingmasters.Where(c => c.emp_inteview_status == "No").OrderByDescending(c => c.emp_interview_date).ToList();

            return View(p);

        }

        //job offer rejected after selection
        public string jobofferrejected(int id)
        {
            var p = dc.tbldrivers.Find(id).emp_id;
            var delobj = dc.tbldrivers.Find(id);
            dc.tbldrivers.Remove(delobj);
            dc.SaveChanges();
            var k = dc.tblonbordingmasters.Find(p);
            k.Entry_status = "Rejected";
            dc.SaveChanges();
            return "success";

        }

        public ActionResult scheduleInterview(int id)
        {
            var p = dc.tblonbordingmasters.Find(id);
            Session["chklist"] = 1;
            return View(p);

        }

        [HttpPost]
        public ActionResult scheduleInterview(tblonbordingmaster obj, FormCollection fc)
        {

            var time = DateTime.Parse(fc["emp_interview_time"]);
            var dd = time.ToString("HH:mm");
            var ss = TimeSpan.Parse(dd.ToString());
            var p = dc.tblonbordingmasters.Find(obj.emp_id);
            p.emp_inteview_status = "Yes";
            p.Entry_status = "Schedule";
            p.emp_interview_date = obj.emp_interview_date;
            p.emp_interview_time = ss;
            dc.SaveChanges();

            if (obj.emp_personal_email != null)
            {
                var subline = "Your Interview  Scheduled on date" + obj.emp_interview_date.Value.ToShortDateString() + "  for the position of" + obj.degination_name;
                var body = "Dear " + obj.emp_name + " Thank you for applying for the position of Delivery Associate and we are pleased to inform you that you have been shortlisted for the said post and your interview is scheduled on " + obj.emp_interview_date.Value.ToShortDateString() + " and " + fc["emp_interview_time"] + " at location. <p> Please confirm by replying this email. Thanks";
                // sendemail(body, obj.emp_personal_email, subline);

                // duplicatefinalemail(obj.emp_personal_email, subline, body);

            }
            if (obj.emp_phone != null)
            {

                var smsm = "Your Interview Scheduled on date " + obj.emp_interview_date.Value.ToShortDateString() + " and " + fc["emp_interview_time"] + " for the position of " + obj.degination_name + "at location reply with Yes to confirm";

                sendsms(obj.emp_phone, smsm);
            }

            return RedirectToAction("JobOfferList");

        }

        //reschdule interview
        public ActionResult rescheule(int id)
        {

            var details = dc.tblonbordingmasters.Find(id);

            return View(details);

        }

        [HttpPost]
        public ActionResult rescheule(tblonbordingmaster obj, FormCollection fc)
        {
            var time = DateTime.Parse(fc["emp_interview_time"]);
            var dd = time.ToString("HH:mm");
            var ss = TimeSpan.Parse(dd.ToString());

            var finalobj = dc.tblonbordingmasters.Find(obj.emp_id);
            finalobj.emp_interview_date = obj.emp_interview_date;
            finalobj.emp_interview_time = ss;
            finalobj.emp_inteview_status = obj.emp_inteview_status;
            finalobj.emp_name = obj.emp_name;
            finalobj.emp_personal_email = obj.emp_personal_email;
            finalobj.emp_phone = obj.emp_phone;
            finalobj.emp_interview_date = obj.emp_interview_date;
            finalobj.Entry_status = "Reschedule";
            finalobj.RescheduleReason = obj.RescheduleReason;
            dc.SaveChanges();

            AddRemarks(obj.emp_id.ToString(), obj.RescheduleReason, "Reschedule Interview");

            tblreschedule reobj = new tblreschedule();
            reobj.reschedule_date = obj.emp_interview_date;
            reobj.reschedule_time = ss;
            reobj.reschedule_status = "rescheule";
            reobj.user_id = int.Parse(Session["admin"].ToString());
            reobj.entry_date = DateTime.Now.Date;
            reobj.emp_id = obj.emp_id;
            dc.tblreschedules.Add(reobj);
            dc.SaveChanges();

            if (obj.emp_personal_email != null)
            {
                //var subline = "Your Interview  Rescheduled on date" + obj.emp_interview_date.Value.ToShortDateString() + "  for the " + obj.degination_name;
                //var body = "Dear " + obj.emp_name + " Thank you for applying for the position of Driver and we are pleased to inform you that you have been selected for the said post and your interview is scheduled on " + obj.emp_interview_date.Value.ToShortDateString() + " and " + obj.emp_interview_time + " at location. <p> Please confirm by replying this email. Thanks";
                ////  sendemail(body, obj.emp_personal_email, subline);
                ////  duplicatefinalemail(obj.emp_personal_email, subline, body);

                if (obj.ApplicationSubmittedDate != null)
                {
                    var subline = "Last Mile Deliveries LLC (LMDL)  - Rescheduled Interview Confirmation -  " + obj.emp_interview_date.Value.ToShortDateString() + " at " + fc["emp_interview_time"] + ".";
                    
                    var body = "Hello " + obj.emp_name + ",<br/><br/>" +
                                "This email confirms your rescheduled interview on " + obj.emp_interview_date.Value.ToShortDateString() + " at " + fc["emp_interview_time"] + ".<br/>" +                                
                                "Please contact HR @ 732-648-4674 or hr@lastmiled.com for any questions.<br/>" +
                                "We look forward to meeting with you.<br/><br/>" +
                                "Last Mile Deliveries LLC (LMDL)    <br/>" +
                                "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";
                    string ccMail = ConfigurationManager.AppSettings["CCEmailIDHR"].ToString();
                    SendEmail_Fixed(obj.emp_personal_email.ToString(), ccMail, subline.ToString(), body);
                }
                else
                {

                    var subline = "Last Mile Deliveries LLC (LMDL)  - Rescheduled Interview Confirmation -  " + obj.emp_interview_date.Value.ToShortDateString() + " at " + fc["emp_interview_time"] + ".";
                    var Host = Request.Url.Host.ToString();
                    var applicationformLink = "Application Form link - " + Host + "/Admin/ApplicatinForm/" + obj.emp_id;

                    var body = "Hello " + obj.emp_name + ",<br/><br/>" +
                                "This email confirms your rescheduled interview on " + obj.emp_interview_date.Value.ToShortDateString() + " at " + fc["emp_interview_time"] + ".<br/>" +
                                "Please click on below link to fill an application form and receive further instructions for the interview. <br/>" +
                                applicationformLink + "<br/><br/>" +
                                "Please contact HR @ 732-648-4674 or hr@lastmiled.com for any questions.<br/>" +
                                "We look forward to meeting with you.<br/><br/>" +
                                "Last Mile Deliveries LLC (LMDL)    <br/>" +
                                "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";
                    string ccMail = ConfigurationManager.AppSettings["CCEmailIDHR"].ToString();
                    SendEmail_Fixed(obj.emp_personal_email.ToString(), ccMail, subline.ToString(), body);
                }
                
            }
            if (obj.emp_phone != null)
            {
                var smsm = "Your Interview Rescheduled on date " + obj.emp_interview_date.Value.ToShortDateString() + " and " + obj.emp_interview_time + " for the position of " + obj.degination_name + " at location reply with Yes to confirm";

                //sendsms(obj.emp_phone, smsm);

            }
            return RedirectToAction("ScheduleIntrviewList");
        }

        public ActionResult pendingrescheule(int id)
        {

            Session["chklist"] = 2;
            var details = dc.tblonbordingmasters.Find(id);

            return View(details);

        }

        [HttpPost]
        public ActionResult pendingrescheule(tblonbordingmaster obj, FormCollection fc)
        {

            var time = DateTime.Parse(fc["emp_interview_time"]);
            var dd = time.ToString("HH:mm");
            var ss = TimeSpan.Parse(dd.ToString());
            var p = dc.tblonbordingmasters.Find(obj.emp_id);
            p.emp_inteview_status = "Yes";
            p.Entry_status = "Schedule";
            p.emp_interview_date = obj.emp_interview_date;
            p.emp_interview_time = ss;
            p.emp_personal_email = obj.emp_personal_email;
            p.emp_phone = obj.emp_phone;

            dc.SaveChanges();

            if (obj.emp_personal_email != null)
            {
                var subline = "Your Interview  Scheduled on date" + obj.emp_interview_date.Value.ToShortDateString() + "  for the position of" + obj.degination_name;
                var body = "Dear " + obj.emp_name + " Thank you for applying for the position of Driver and we are pleased to inform you that you have been selected for the said post and your interview is scheduled on " + obj.emp_interview_date.Value.ToShortDateString() + " and " + fc["emp_interview_time"] + " at location. <p> Please confirm by replying this email. Thanks";
                // sendemail(body, obj.emp_personal_email, subline);

                //  duplicatefinalemail(obj.emp_personal_email, subline, body);

            }
            if (obj.emp_phone != null)
            {

                var smsm = "Your Interview Scheduled on date " + obj.emp_interview_date.Value.ToShortDateString() + " and " + fc["emp_interview_time"] + " for the position of " + obj.degination_name + "at location reply with Yes to confirm";

                sendsms(obj.emp_phone, smsm);
            }

            return RedirectToAction("JobOfferList");

        }

        // rescheule histrory
        public ActionResult reschedulehistory(int id)
        {
            var p = dc.tblreschedules.Where(c => c.emp_id == id).ToList();

            return View(p);

        }

        public string Interviewstatusupdate(int id, string status, string email, string PageName)
        {
            if (status == "SendApplication")
            {
                var p = dc.tblonbordingmasters.Find(id);
                p.emp_personal_email = email;
                p.ApplicationSubmittedDate = null;
                dc.SaveChanges();


                if (p.emp_personal_email != null)
                {
                    var Host = Request.Url.Host.ToString();
                    var subline = "Application Form";
                    var applicationformLink = "Application Form link - " + Host + "/Admin/ApplicatinForm/" + p.emp_id;

                    var body = "Hello " + p.emp_name + ",<br/><br/>" +
                                "Thank you for your interest in Delivery Associate position with Last Mile Deliveries LLC (LMDL)  , an Amazon Delivery Service Partner (DSP). <br/><br/>" +
                                //"This email confirms your interview on " + interview_date.Value.ToShortDateString() + " at " + interview_time + ".<br/>" +
                                "Please click on below link to fill an application form and receive further instructions for the interview. <br/>" +
                                applicationformLink + "<br/><br/>" +
                                "Please contact HR @ 732-648-4674 or hr@lastmiled.com for any questions.<br/>" +
                                "We look forward to meeting with you.<br/><br/>" +
                                //"Thank you,<br/>" +
                                "Last Mile Deliveries LLC (LMDL)    <br/>" +
                                "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";



                    string ccMail = ConfigurationManager.AppSettings["CCEmailIDHR"].ToString();
                    bool result = SendEmail_Fixed(p.emp_personal_email, ccMail, subline.ToString(), body.ToString());

                    string ShowErrorAndCreds = ConfigurationManager.AppSettings["ShowErrorAndCreds"].ToString();

                    if (result)
                    {
                        Session["LinkModel"] = "Mail sent successfully !!!+" + Host + "/Admin/ApplicatinForm/" + p.emp_id;

                        var IsNotes = dc.tblDriverRemarks.Where(w => w.EmpID == id && w.RemarkPageName == "Application Form link").ToList();

                        if (IsNotes.Count == 0)
                        {
                            AddRemarks(id.ToString(), Host + "/Admin/ApplicatinForm/" + p.emp_id, "Application Form link");
                        }

                        return "Mail sent successfully !!!" + "\n" + Host + "/Admin/ApplicatinForm/" + p.emp_id;
                    }
                    else
                    {
                        if (ShowErrorAndCreds == "Show")
                        {
                            Session["LinkModel"] = applicationformLink + "\nError - " + Session["Creds"] + "\nError - " + Session["Error"].ToString();
                        }
                        return applicationformLink;
                    }

                }
                else
                {
                    return "Email ID is Missing !! ";
                }






            }
            else
            {
                var p = dc.tblonbordingmasters.Find(id);
                p.Entry_status = status;
                p.Entry_Remark = email;
                dc.SaveChanges();

                AddRemarks(id.ToString(), status + " - " + email, PageName);

                return "success";

            }


        }

        public ActionResult ApplicatinForm(int id)
        {
            ViewBag.emp_id = id;
            var ss = dc.tblonbordingmasters.Find(id);
            ViewBag.applicationSubmittedDate = ss.ApplicationSubmittedDate;
            return View(ss);
        }

        [HttpPost]
        public ActionResult ApplicatinForm(FormCollection fc, HttpPostedFileBase singdata, HttpPostedFileBase linc_file)
        {

            var emp_id = int.Parse(fc["emp_id"]);
            var S = fc["SourceOfInquiry"];
            var SOI = dc.tblsources.Where(s => s.source_name == S).ToList();


            var obj = dc.tblonbordingmasters.Find(emp_id);

            if (SOI.Count == 0)
            {
                tblsource objtblsource = new tblsource();
                objtblsource.source_name = fc["SourceOfInquiry"];

                dc.tblsources.Add(objtblsource);
                dc.SaveChanges();

                var SOI_id = dc.tblsources.Where(e => e.source_name == S).ToList();
                obj.source_id = Convert.ToInt32(SOI_id.FirstOrDefault().source_id);
            }





            obj.Entry_status = "ApplicationSent";
            //obj.emp_inteview_status = "ApplicationSent";
            obj.emp_name = fc["fname"];
            obj.emp_phone = fc["mobile"];
            obj.emp_personal_email = fc["email"];
            // obj.degination_name = fc["dob"];
            //obj.emp_dob = DateTime.Parse(fc["dob"]);
            obj.emp_add = fc["addr"];
            obj.city = fc["city"];
            obj.state = fc["state"];
            obj.zip = fc["zip"];
            //obj.are_you_us_city = fc["iscitizenus"];
            //obj.are_you_legal_us_city = fc["islegallyallowed"];
            //obj.drug_test = fc["submitdrugtest"];
            // obj.Convicted_felony = fc["isconvicted"];
            //   obj.Convicted_felony_explain = fc["convictiondetails"];
            obj.work_sat = fc["availablesaturday"];
            obj.work_sat_explain = fc["saturdaydetails"];
            obj.work_sun = fc["availablesunday"];
            obj.work_sun_explain = fc["sundaydetails"];
            //obj.ticket_point = fc["isdrivingticket"];
            //obj.count = fc["ticketcount"];
            //obj.driver_linc = fc["dl"];
            obj.applied_pos = fc["position"];
            obj.available_startdate = DateTime.Parse(fc["startdate"]);
            obj.desired_Pay = fc["desiredpay"];
            obj.employment_desired = fc["employmentdesired"];
            obj.school_name1 = fc["schoolname1"];
            obj.school_name2 = fc["schoolname2"];
            obj.location1 = fc["location1"];
            obj.location2 = fc["location2"];
            obj.year_attended1 = fc["yearsattended1"];
            obj.year_attended2 = fc["yearsattended2"];
            obj.major1 = fc["major1"];
            obj.major2 = fc["major2"];
            obj.employer1 = fc["employer1"];
            obj.employer2 = fc["employer2"];
            obj.jobtitle1 = fc["jobtitle1"];
            obj.jobtitle2 = fc["jobtitle2"];
            obj.state1 = fc["state1"];
            obj.state2 = fc["state2"];
            obj.work_phone1 = fc["workphone1"];
            obj.work_phone2 = fc["workphone2"];
            obj.zip1 = fc["zip1"];
            obj.zip2 = fc["zip2"];
            obj.address1 = fc["address1"];
            obj.address2 = fc["address2"];
            obj.remarks1 = fc["remak1"];
            obj.remark2 = fc["remak2"];


            obj.starting_pay_rate1 = fc["startpayingrate1"];
            obj.starting_pay_rate2 = fc["startpayingrate2"];
            obj.ending_pay_rate1 = fc["endpayrate1"];
            obj.ending_pay_rate2 = fc["endpayrate2"];
            obj.city1 = fc["city1"];
            obj.city2 = fc["city2"];
            obj.sig_name = fc["sig_name"];
            obj.sig_date = DateTime.Parse(fc["sig_date"]);


            obj.IsLeagalyAuthorized = fc["IsLeagalyAuthorized"];
            obj.IsSponShipRequired = fc["IsSponShipRequired"];
            obj.WorkPerformed1 = fc["WorkPerformed1"];
            obj.WorkPerformed2 = fc["WorkPerformed2"];
            obj.employer3 = fc["employer3"];
            obj.jobtitle3 = fc["jobtitle3"];
            obj.work_phone3 = fc["workphone3"];
            obj.starting_pay_rate3 = fc["startpayingrate3"];
            obj.ending_pay_rate3 = fc["endpayrate3"];
            obj.address3 = fc["address3"];
            obj.city3 = fc["city3"];
            obj.state3 = fc["state3"];
            obj.zip3 = fc["zip3"];
            obj.remarks3 = fc["remak3"];
            obj.WorkPerformed3 = fc["WorkPerformed3"];
            obj.degree_rec1 = fc["degreereceived1"];
            obj.degree_rec2 = fc["degreereceived2"];

            obj.FinalChk1 = fc["FinalChk1"];
            obj.FinalChk2 = fc["FinalChk2"];
            obj.FinalChk3 = fc["FinalChk3"];
            obj.FinalChk4 = fc["FinalChk4"];

            obj.empstartdate1 = fc["empstartdate1"];
            obj.empstartdate2 = fc["empstartdate2"];
            obj.empstartdate3 = fc["empstartdate3"];

            obj.empenddate1 = fc["empenddate1"];
            obj.empenddate2 = fc["empenddate2"];
            obj.empenddate3 = fc["empenddate3"];
            obj.ApplicationSubmittedDate = DateTime.Now;

            if (singdata != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var filePath = path + Path.GetFileName(singdata.FileName);
                string extension = Path.GetExtension(singdata.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    singdata.SaveAs(filePath);
                    obj.sig_name = singdata.FileName;

                }
                else
                {
                    ViewBag.msg = "File already uploaded";
                }
            }
            else
            {
            }

            if (linc_file != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var filePath = path + Path.GetFileName(linc_file.FileName);
                string fileName = Path.GetFileName(linc_file.FileName).Split('.')[0];
                string extension = Path.GetExtension(linc_file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    linc_file.SaveAs(filePath);
                    obj.upload_linces_photo = linc_file.FileName;
                }
                else
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        int index = 1;

                        string finalFileName = fileName + "_" + index + extension;

                        while (System.IO.File.Exists(path + finalFileName))
                        {
                            finalFileName = fileName + "_" + ++index + extension;
                        }

                        string finalFilePath = path + finalFileName;

                        linc_file.SaveAs(finalFilePath);
                        obj.upload_linces_photo = finalFileName;
                    }
                }
            }
            else
            {
                ViewBag.msg = "Please upload driving license.";
                return View();
            }


            dc.SaveChanges();

            AddRemarks(emp_id.ToString(), "Application submitted on - " + DateTime.Now.ToShortDateString(), "Application Form");

            ViewBag.emp_id = emp_id;
            var ss = dc.tblonbordingmasters.Find(emp_id);
            ViewBag.msg = "Form Submited Successfully..";
            return RedirectToAction("FinalformSubmtion");
        }

        [HttpGet]
        public ActionResult FinalformSubmtion()
        {
            return View();

        }

        public string PendingInterviewstatusupdate(int id, string status)
        {
            var p = dc.tblonbordingmasters.Find(id);
            p.Entry_status = status;
            p.emp_interview_date = DateTime.Now;
            p.emp_inteview_status = "Yes";
            p.emp_interview_time = new TimeSpan(1, 0, 0);
            dc.SaveChanges();
            return "success";



        }

        public string InterviewTakeFinal(int id)
        {

            var p = dc.tblonbordingmasters.Find(id);

            var d_name = p.emp_name;
            var d_phone = p.emp_phone;
            var d_email = p.emp_personal_email;
            var d_dob = p.emp_dob;
            var d_lin_no = p.driver_linc;
            var d_joboffer = "Yes";
            var emp_id = p.emp_id;
            var user_id = int.Parse(Session["admin"].ToString());
            var entry_date = DateTime.Now.Date;
            var phonevaerifiyed = "yes";
            var remakes = "Ok";
            var d_state = p.state;
            // var d_ssn = fc["d_ssn"];
            var d_address = p.emp_add;

            tbldriver dobj = new tbldriver();
            dobj.d_name = d_name;
            dobj.d_phone = d_phone;
            dobj.d_email = d_email;
            dobj.emp_id = int.Parse(emp_id.ToString());
            dobj.d_dob = d_dob;
            dobj.d_license_number = d_lin_no;
            dobj.joboffer = d_joboffer;
            dobj.Remakes = remakes;
            dobj.entry_date = entry_date;
            dobj.user_id = user_id;
            dobj.phone_no_verifiedy_status = phonevaerifiyed;

            dobj.onbordingstatus = "Pending";

            dobj.d_document_status = "Pending";

            dobj.d_address = d_address;

            dobj.d_license_state = d_state;

            // dobj.d_ssn = d_ssn;

            dc.tbldrivers.Add(dobj);
            dc.SaveChanges();
            var empid = int.Parse(emp_id.ToString());

            var p1 = dc.tblonbordingmasters.Find(empid);
            p1.emp_inteview_status = "taken";

            if (d_joboffer == "Yes")
            {
                p.Entry_status = "StartOnbording";

            }
            else
            {
                p1.rejection_resion = remakes;
                p1.Entry_status = "Rejected";

            }

            dc.SaveChanges();

            return "successfully";


        }

        //interiew taken code
        [HttpGet]
        public ActionResult InterviewTake(int id)
        {
            var p = dc.tblonbordingmasters.Find(id);

            var d_name = p.emp_name;
            var d_phone = p.emp_phone;
            var d_email = p.emp_personal_email;
            var d_dob = p.emp_dob;
            var d_lin_no = p.driver_linc;
            var d_joboffer = "Yes";
            var emp_id = p.emp_id;
            var user_id = int.Parse(Session["admin"].ToString());
            var entry_date = DateTime.Now.Date;
            var phonevaerifiyed = "yes";
            var remakes = "Ok";
            var d_state = p.state;
            // var d_ssn = fc["d_ssn"];
            var d_address = p.emp_add;

            tbldriver dobj = new tbldriver();
            dobj.d_name = d_name;
            dobj.d_phone = d_phone;
            dobj.d_email = d_email;
            dobj.emp_id = int.Parse(emp_id.ToString());
            dobj.d_dob = d_dob;
            dobj.d_license_number = d_lin_no;
            dobj.joboffer = d_joboffer;
            dobj.Remakes = remakes;
            dobj.entry_date = entry_date;
            dobj.user_id = user_id;
            dobj.phone_no_verifiedy_status = phonevaerifiyed;

            dobj.onbordingstatus = "Pending";

            dobj.d_document_status = "Pending";

            dobj.d_address = d_address;

            dobj.d_license_state = d_state;

            // dobj.d_ssn = d_ssn;

            dc.tbldrivers.Add(dobj);
            dc.SaveChanges();
            var empid = int.Parse(emp_id.ToString());

            var p1 = dc.tblonbordingmasters.Find(empid);
            p1.emp_inteview_status = "taken";

            if (d_joboffer == "Yes")
            {
                p.Entry_status = "StartOnbording";

            }
            else
            {
                p1.rejection_resion = remakes;
                p1.Entry_status = "Rejected";

            }

            dc.SaveChanges();





            return View(p);

        }

        [HttpPost]
        public ActionResult InterviewTake(FormCollection fc)
        {

            var d_name = fc["emp_name"].ToString();
            var d_phone = fc["emp_phone"];
            var d_email = fc["emp_personal_email"];
            var d_dob = DateTime.Parse(fc["d_dob"]);
            var d_lin_no = fc["d_license_number"];
            var d_joboffer = fc["joboffer"];
            var emp_id = fc["emp_id"];
            var user_id = int.Parse(Session["admin"].ToString());
            var entry_date = DateTime.Now.Date;
            var phonevaerifiyed = "yes";
            var remakes = fc["Remakes"];
            var d_state = fc["d_license_state"];
            // var d_ssn = fc["d_ssn"];
            var d_address = fc["d_address"];

            tbldriver dobj = new tbldriver();
            dobj.d_name = d_name;
            dobj.d_phone = d_phone;
            dobj.d_email = d_email;
            dobj.emp_id = int.Parse(emp_id);
            dobj.d_dob = d_dob;
            dobj.d_license_number = d_lin_no;
            dobj.joboffer = d_joboffer;
            dobj.Remakes = remakes;
            dobj.entry_date = entry_date;
            dobj.user_id = user_id;
            dobj.phone_no_verifiedy_status = phonevaerifiyed;

            dobj.onbordingstatus = "Pending";

            dobj.d_document_status = "Pending";

            dobj.d_address = d_address;

            dobj.d_license_state = d_state;

            // dobj.d_ssn = d_ssn;

            dc.tbldrivers.Add(dobj);
            dc.SaveChanges();
            var empid = int.Parse(emp_id);

            var p = dc.tblonbordingmasters.Find(empid);
            p.emp_inteview_status = "taken";

            if (d_joboffer == "Yes")
            {
                p.Entry_status = "StartOnbording";

            }
            else
            {
                p.rejection_resion = remakes;
                p.Entry_status = "Rejected";

            }

            dc.SaveChanges();
            return RedirectToAction("applicationSubmitedList");
        }

        //send welcome message and email to dirver 
        public string welcomemsgsend(int id)
        {
            var empid = dc.tbldrivers.Find(id).emp_id;
            var p = dc.tblonbordingmasters.Find(empid);

            if (p.emp_personal_email != null)
            {
                var obj = dc.tbldrivertranings.Where(c => c.d_id == id).FirstOrDefault();

                var sub = "Welcome To LMDL !!! -" + dc.tbldrivers.Find(obj.d_id).d_name;

                var subdesc = "<div style='margin: 10px auto;'><img width ='80' height ='90' src='http://lastmiled.com/wp-content/uploads/brizy/93/assets/images/iW=78&iH=78&oX=0&oY=0&cW=78&cH=78/lmdl.png' /></div>" +
              "<p> Hello " + dc.tbldrivers.Find(obj.d_id).d_name + ",</ p >" +
                  "<p> I am very exited to welcome you to the team.Please see below details for your first 3 days of training.</ p >" +

                    " <p> " + obj.inclassdateone.Value.ToShortDateString() + " , " + obj.inclassdatetwo.Value.ToShortDateString() + "  Amazon in-class training - " + obj.inclasstime + " " + obj.amazonaddress + " </p>" +
"<p>Plan to reach the facility at 7:45am and park at the guest parking.training will be held on the second floor conference room, please follow the signs on the 2nd floor, at the end of your class report to the front desk on the lobby to have your parking pass validated for free parking.</p>" +

"<p>" + obj.ridealongdate.Value.ToShortDateString() + " LMDL ride-along - " + obj.training_time + " -  " + obj.ridealongaddress + " https://goo.gl/maps/9YHqhJyUk9ZkXKat5" +

"Driver Trainer - " + obj.ridealongpersonname + "</p>" +

"<p>Call " + obj.dispecher_name + " when you get to the Parking lot and he can direct you.Your Car has to be parked in our parking lot.Please reach at 8:45am sharp<br />" +
"Manan G. (Me) - 732-804-4423<br />" +
"Hiral S. (HR) - 732-648-4674</p>" +

"<p>Hilton Newark Airport</p>" +
"<div> <img width = '400' height = '570' src ='https://lastmiled.com/wp-content/uploads/map.jpg'/> </div>";

                //   duplicatefinalemail(p.emp_personal_email, sub, subdesc);



            }




            return "successfully";

        }

        public ActionResult CreateSubAdmin()
        {

            return View();

        }

        [HttpGet]
        public ActionResult CreateRole(int id)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                var p = dc.tblroles.Find(id);
                return View(p);


            }
        }

        [HttpPost]
        public ActionResult CreateRole(tblrole obj)
        {

            if (ModelState.IsValid)
            {

                if (obj.role_id == 0)
                {
                    dc.tblroles.Add(obj);
                    obj.entry_date = DateTime.Now.Date;
                    dc.SaveChanges();
                }
                else
                {
                    dc.Entry(obj).State = EntityState.Modified;
                    dc.SaveChanges();

                }

                return RedirectToAction("ViewRole");
            }
            else
            {
                return RedirectToAction("ViewRole");


            }
        }

        public ActionResult ViewRole()
        {

            var listdata = dc.tblroles.ToList();
            return View(listdata);
        }

        [HttpGet]
        public string DeleteRole(int id)
        {

            dc.tblroles.Remove(dc.tblroles.Find(id));
            dc.SaveChanges();

            return "success";
        }

        //createsubadmintask
        public ActionResult ViewSubadmin()
        {


            return View(dc.tblsubadmins.ToList());
        }

        [HttpGet]
        public ActionResult CreateSubAdmin(int id)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                var p = dc.tblsubadmins.Find(id);
                return View(p);


            }
        }

        [HttpPost]
        public ActionResult CreateSubAdmin(tblsubadmin obj)
        {

            if (obj.sub_admin_id == 0)
            {
                dc.tblsubadmins.Add(obj);
                obj.entry_user_id = int.Parse(Session["admin"].ToString());
                obj.entry_date = DateTime.Now.Date;
                dc.SaveChanges();
            }
            else
            {

                dc.Entry(obj).State = EntityState.Modified;

                dc.SaveChanges();

            }

            return RedirectToAction("ViewSubadmin");

        }

        [HttpGet]
        public string DeleteSubAdmin(int id)
        {

            dc.tblsubadmins.Remove(dc.tblsubadmins.Find(id));
            dc.SaveChanges();

            return "success";
        }

        [HttpGet]
        public ActionResult viewModule()
        {



            return View(dc.tblmodules.ToList());

        }

        public ActionResult createModule(int id)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                var p = dc.tblmodules.Find(id);
                return View(p);


            }
        }

        [HttpPost]
        public ActionResult createModule(tblmodule obj)
        {

            if (obj.module_id == 0)
            {
                dc.tblmodules.Add(obj);
                obj.entry_user_id = int.Parse(Session["admin"].ToString());
                obj.entry_date = DateTime.Now.Date;
                dc.SaveChanges();
            }
            else
            {

                dc.Entry(obj).State = EntityState.Modified;

                dc.SaveChanges();

            }

            return RedirectToAction("viewModule");

        }

        [HttpGet]
        public string deleteModule(int id)
        {

            dc.tblmodules.Remove(dc.tblmodules.Find(id));
            dc.SaveChanges();

            return "success";
        }

        public void getalladmin()
        {
            var list = new List<SelectListItem>();
            var objlist = dc.tblsubadmins.ToList();

            foreach (var item in objlist)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.sub_admin_name;
                obj.Value = item.sub_admin_id.ToString();
                list.Add(obj);

            }
            ViewData["temp"] = list;
        }

        public Assignauth getmodulename()
        {
            Assignauth objfinal = new Assignauth();
            var list = new List<ModuleCheck>();
            var objlist = dc.tblmodules.ToList();

            foreach (var item in objlist)
            {
                ModuleCheck obj = new ModuleCheck();
                obj.id = item.module_id;
                obj.name = item.module_name;
                obj.check = false;

                list.Add(obj);
            }

            objfinal.listmodule = list;
            //   ViewData["moudle"] = list;

            return objfinal;
        }

        public ActionResult createAssign()
        {

            getalladmin();
            // getmodulename();

            return View(getmodulename());

        }

        [HttpPost]
        public ActionResult createAssign(Assignauth obj)
        {

            var p = obj.listmodule.Where(c => c.check == true).ToList();

            var strmodule = "";
            foreach (var item in p)
            {

                if (item.check == true)
                {
                    strmodule += item.name + ",";
                }

            }

            tblassign tblobj = new tblassign();
            tblobj.entry_date = DateTime.Now.Date;
            tblobj.sub_admin_id = obj.sub_admin_id;

            tblobj.module_id = strmodule;
            tblobj.entry_user_id = int.Parse(Session["admin"].ToString());
            dc.tblassigns.Add(tblobj);
            dc.SaveChanges();
            ViewBag.msg = "Assigned Successfully ...!";
            getalladmin();
            return View(getmodulename());

        }

        public ActionResult viewAssigned()
        {


            /* var p = (from s in dc.tblassigns
                     from l in dc.tblsubadmins
                     where s.sub_admin_id == l.sub_admin_id select s,l)

              */

            return View();

        }

        // edit interviewlist
        public ActionResult editinterview(int id, string flg)
        {

            Session["flg"] = flg;

            var p = dc.tblonbordingmasters.Find(id);

            editgetlistdata(p.degination_name);

            return View(p);

        }

        [HttpPost]
        public ActionResult editinterview(tblonbordingmaster obj, FormCollection fc)
        {
            var flg = Session["flg"].ToString();


            var p = dc.tblonbordingmasters.Find(obj.emp_id);

            p.emp_name = obj.emp_name;
            p.emp_phone = obj.emp_phone;
            p.emp_personal_email = obj.emp_personal_email;
            p.emp_inteview_status = "Yes";

            //p.emp_interview_time = obj.emp_interview_time;            
            //p.emp_interview_date = obj.emp_interview_date;

            //var time = DateTime.Parse(fc["emp_interview_time"]);
            //var dd = time.ToString("HH:mm");
            //var ss = TimeSpan.Parse(dd.ToString());

            //p.emp_interview_time = ss;            
            //p.degination_name = obj.degination_name;

            p.emp_inteview_status = obj.emp_inteview_status;

            dc.SaveChanges();

            if (flg == "app")
            {
                return RedirectToAction("applicationSubmitedList");

            }
            else
            {
                return RedirectToAction("ScheduleIntrviewList");

            }

        }

        //delete inteview from list
        [HttpGet]
        public string deleteInterview(int id)
        {

            dc.tblonbordingmasters.Remove(dc.tblonbordingmasters.Find(id));
            dc.SaveChanges();
            return "Success";

        }

        [HttpGet]
        public ActionResult JobOfferList()
        {
            if (Session["chklist"] == null)
            {
                return RedirectToAction("Home");

            }

            var p = dc.tbldrivers.Where(c => c.joboffer == "Yes" && c.d_document_status == "Pending").OrderByDescending(o => o.entry_date).ToList();
            List<jobofferlistModel> objlistmodel = new List<jobofferlistModel>();

            foreach (var item in p)
            {
                jobofferlistModel obj = new jobofferlistModel();
                obj.d_id = int.Parse(item.d_id.ToString());
                obj.d_name = item.d_name;
                obj.d_email = item.d_email;
                obj.d_phone = item.d_phone;
                obj.emp_id = int.Parse(item.emp_id.ToString());

                bool existsA = dc.tbldrivertests.FirstOrDefault(c => c.d_id == item.d_id) != null;
                if (existsA)
                {
                    var sps = dc.tbldrivertests.Where(c => c.d_id == item.d_id).FirstOrDefault();


                    if (sps.isADPMVR == "Yes")
                    {
                        obj.isADPMVR = true;
                    }
                    else
                    {
                        obj.isADPMVR = false;
                    }
                }
                else
                {
                    obj.isADPMVR = false;
                }

                bool exists = dc.tbldrivertests.FirstOrDefault(c => c.d_id == item.d_id) != null;
                if (exists)
                {
                    var sps = dc.tbldrivertests.Where(c => c.d_id == item.d_id).FirstOrDefault();


                    if (sps.isemailcreated == "Yes")
                    {
                        obj.isemailid = true;
                    }
                    else
                    {
                        obj.isemailid = false;
                    }
                }
                else
                {
                    obj.isemailid = false;
                }

                bool exists1 = dc.tbldrivertests.FirstOrDefault(c => c.d_id == item.d_id) != null;
                if (exists1)
                {
                    var sps = dc.tbldrivertests.Where(c => c.d_id == item.d_id).FirstOrDefault();

                    if (sps.isbackgrundcheckclear == "Meets Req.")
                    {
                        obj.isbackgroundcheck = true;
                    }
                    else
                    {
                        obj.isbackgroundcheck = false;
                    }

                }
                else
                {
                    obj.isbackgroundcheck = false;

                }

                bool exits2 = dc.tbldrivertests.FirstOrDefault(c => c.d_id == item.d_id) != null;
                if (exits2)
                {

                    var sps = dc.tbldrivertests.Where(c => c.d_id == item.d_id).FirstOrDefault();

                    if (sps.isonbordingLinkCreated == "Accepted")
                    {
                        obj.isonbordinglink = true;
                    }
                    else
                    {
                        obj.isonbordinglink = false;
                    }

                }
                else
                {
                    obj.isonbordinglink = false;

                }

                bool exits3 = dc.tbldrivertests.FirstOrDefault(c => c.d_id == item.d_id) != null;
                if (exits3)
                {

                    var sps = dc.tbldrivertests.Where(c => c.d_id == item.d_id).FirstOrDefault();

                    if (sps.isdragtestclear == "Yes")
                    {
                        obj.isdrugtest = true;
                    }
                    else
                    {
                        obj.isdrugtest = false;
                    }


                }
                else
                {
                    obj.isdrugtest = false;

                }

                objlistmodel.Add(obj);
            }


            ViewData["jobofferlist"] = objlistmodel;

            var p1 = dc.tblonbordingmasters.Where(c => c.emp_inteview_status == "No").OrderByDescending(c => c.emp_interview_date).ToList();

            ViewData["pendinglist"] = p1;

            var p2 = dc.tblonbordingmasters.Where(c => c.Entry_status == "Rejected" || c.Entry_status == "NoShow" || c.Entry_status == "WithdrawApplication").ToList();

            ViewData["rejectedlist"] = p2;

            var p3 = (from n in dc.tbldrivers
                      from l in dc.tbldrivertests

                      where n.d_id == l.d_id && n.d_document_status == "Done" && n.onbordingstatus == "Pending"
                      select n).ToList();

            ViewData["scheduletraning"] = p3;

            return View();

        }

        //driver Documentation part
        public ActionResult testcheck(int id)
        {
            Session["d_id"] = id;
            Session["chklist"] = 0;
            var pp = dc.tbldrivertests.Where(c => c.d_id == id).FirstOrDefault();


            return View(pp);

        }

        public ActionResult isdrugtest(int id)
        {
            Session["chklist"] = 0;
            ViewBag.id = id;

            var p = dc.tbldrivertests.Where(c => c.d_id == id).ToList();
            if (p.Count > 0)
            {
                var s = p.FirstOrDefault().isdragtestclear;

                if (s != null)
                {
                    ViewBag.status = s;
                }
            }

            return View();

        }

        [HttpPost]
        public ActionResult isdrugtest(FormCollection fc)
        {
            var d_id = int.Parse(fc["did"].ToString());

            var exits = dc.tbldrivertests.FirstOrDefault(c => c.d_id == d_id) != null;
            if (exits)
            {
                var ss = dc.tbldrivertests.Where(c => c.d_id == d_id).FirstOrDefault();
                var status = fc["isdragtestclear"];
                ss.isdragtestclear = status;
                dc.SaveChanges();

                if (status != "NoShow" && status != "Rejected")
                {
                    updatedocumentstatus(d_id);

                    var empID = dc.tbldrivers.Find(d_id).emp_id;
                    AddRemarks(empID.ToString(), "Drug status changed to - " + status, "Joboffer List");

                }
                else
                {
                    var p = dc.tbldrivers.Find(d_id);
                    if (p != null)
                    {
                        p.joboffer = "No";
                        //p.onbordingstatus = status;
                        dc.SaveChanges();

                        var p1 = dc.tblonbordingmasters.Find(p.emp_id);
                        p1.Entry_status = status;
                        dc.SaveChanges();

                        AddRemarks(p.emp_id.ToString(), "Drug test failed.", "Job Offer List");

                    }
                }
            }
            else
            {
                var did = int.Parse(fc["did"].ToString());
                var isdragtestclear = fc["isdragtestclear"];

                tbldrivertest objtest = new tbldrivertest();
                objtest.d_id = did;
                objtest.isdragtestclear = isdragtestclear;

                dc.tbldrivertests.Add(objtest);
                dc.SaveChanges();

                updatedocumentstatus(d_id);

            }

            return RedirectToAction("JobOfferList");



        }

        public ActionResult isbackgroundcheck(int id)
        {
            Session["chklist"] = 0;
            ViewBag.id = id;


            var p = dc.tbldrivertests.Where(c => c.d_id == id).ToList();
            if (p.Count > 0)
            {
                var s = p.FirstOrDefault().isbackgrundcheckclear;

                if (s != null)
                {
                    ViewBag.status = s;
                }
            }


            return View();
        }

        [HttpPost]
        public ActionResult isbackgroundcheck(FormCollection fc)
        {
            var d_id = int.Parse(fc["did"].ToString());

            var exits = dc.tbldrivertests.FirstOrDefault(c => c.d_id == d_id) != null;
            if (exits)
            {
                var ss = dc.tbldrivertests.Where(c => c.d_id == d_id).FirstOrDefault();
                var status = fc["isbackgrundcheckclear"];

                if (status != "Rejected")
                {
                    ss.isbackgrundcheckclear = status;
                    dc.SaveChanges();
                    updatedocumentstatus(d_id);

                    var empID = dc.tbldrivers.Find(d_id).emp_id;
                    AddRemarks(empID.ToString(), "Accurate background status changed to - " + status, "Joboffer List");
                }
                else
                {
                    var p = dc.tbldrivers.Find(d_id);
                    if (p != null)
                    {
                        p.joboffer = "No";
                        //p.onbordingstatus = status;
                        dc.SaveChanges();

                        var p1 = dc.tblonbordingmasters.Find(p.emp_id);
                        p1.Entry_status = status;
                        dc.SaveChanges();

                        AddRemarks(p.emp_id.ToString(), "Background check failed.", "Job Offer List");

                    }
                }
            }
            else
            {
                var did = int.Parse(fc["did"].ToString());
                var isbackgrundcheckclear = fc["isbackgrundcheckclear"];

                tbldrivertest objtest = new tbldrivertest();
                objtest.d_id = did;
                objtest.isbackgrundcheckclear = isbackgrundcheckclear;

                dc.tbldrivertests.Add(objtest);
                dc.SaveChanges();
                updatedocumentstatus(d_id);

            }

            return RedirectToAction("JobOfferList");
        }

        public ActionResult isonbordinglink(int id)
        {
            Session["chklist"] = 0;
            ViewBag.id = id;

            var p = dc.tbldrivertests.Where(c => c.d_id == id).ToList();
            if (p.Count > 0)
            {
                var s = p.FirstOrDefault().isonbordingLinkCreated;

                if (s != null)
                {
                    ViewBag.status = s;
                }
            }

            return View();


        }

        [HttpPost]
        public ActionResult isonbordinglink(FormCollection fc)
        {
            var d_id = int.Parse(fc["did"].ToString());

            var exits = dc.tbldrivertests.FirstOrDefault(c => c.d_id == d_id) != null;
            if (exits)
            {
                var status = fc["isonbordingLinkCreated"];
                var ss = dc.tbldrivertests.Where(c => c.d_id == d_id).FirstOrDefault();

                ss.isonbordingLinkCreated = status;
                dc.SaveChanges();

                if (status != "NoShow")
                {
                    updatedocumentstatus(d_id);
                    var empID = dc.tbldrivers.Find(d_id).emp_id;
                    AddRemarks(empID.ToString(), "OnBording Link status changed to - " + status, "Joboffer List");
                }
                else
                {
                    var p = dc.tbldrivers.Find(d_id);
                    if (p != null)
                    {
                        p.joboffer = "No";
                        //p.onbordingstatus = status;
                        dc.SaveChanges();

                        var p1 = dc.tblonbordingmasters.Find(p.emp_id);
                        p1.Entry_status = status;
                        dc.SaveChanges();

                        AddRemarks(p.emp_id.ToString(), "Onboarding link failed.", "Job Offer List");

                    }
                }
            }
            else
            {
                var did = int.Parse(fc["did"].ToString());
                var onboardinglink = fc["isonbordingLinkCreated"];

                tbldrivertest objtest = new tbldrivertest();
                objtest.d_id = did;
                objtest.isonbordingLinkCreated = onboardinglink;

                dc.tbldrivertests.Add(objtest);
                dc.SaveChanges();
                updatedocumentstatus(d_id);

            }

            return RedirectToAction("JobOfferList");
        }

        public ActionResult isemailcheck(int id)
        {

            Session["chklist"] = 0;
            ViewBag.id = id;

            return View();

        }

        public void updatedocumentstatus(int id)
        {
            var ss = dc.tbldrivertests.Where(c => c.d_id == id).FirstOrDefault();
            if (ss.isemailcreated == "Yes" && (ss.isbackgrundcheckclear == "MeetsRequirement") && ss.isdragtestclear == "Negative" && (ss.isonbordingLinkCreated == "Sent" || ss.isonbordingLinkCreated == "Accepted") && ss.isADPMVR == "Yes" && ss.isOnBordingVideo == "Completed")
            {
                var stp = dc.tbldrivers.Find(id);
                stp.d_document_status = "Done";
                dc.SaveChanges();
            }
            TransferToScheduledTraining(id);
            //UpdateSecondform(id);
        }

        public void TransferToScheduledTraining(int id)
        {
            var ss = dc.tbldrivertests.Where(c => c.d_id == id).FirstOrDefault();
            if (ss.isdragtestclear != null && ss.isOnBordingVideo != null)
            {
                if (ss.isdragtestclear == "Negative" && ss.isOnBordingVideo == "Completed")
                {
                    var stp = dc.tbldrivers.Find(id);
                    stp.d_document_status = "Done";
                    stp.onbordingstatus = "Pending";
                    dc.SaveChanges();
                }
            }
        }

        public void UpdateSecondform(int id)
        {
            //var ss = dc.tbldrivertests.Where(c => c.d_id == id).FirstOrDefault();
            //if (ss.isbackgrundcheckclear != null && ss.isdragtestclear != null)
            //{
            //    if (ss.isbackgrundcheckclear == "No" || ss.isdragtestclear == "No")
            //    {
            //        var stp = dc.tbldrivers.Find(id);
            //        stp.d_document_status = "not";
            //        dc.SaveChanges();
            //        var pp = stp.emp_id;
            //        var sss = dc.tblonbordingmasters.Where(c => c.emp_id == pp).FirstOrDefault();
            //        sss.Entry_status = "Rejected";
            //        dc.SaveChanges();
            //    }
            //}
            //else
            //{

            //}



        }

        [HttpPost]
        public ActionResult isemailcheck(FormCollection fc)
        {
            var d_id = int.Parse(fc["did"].ToString());

            var exits = dc.tbldrivertests.FirstOrDefault(c => c.d_id == d_id) != null;
            if (exits)
            {
                var ss = dc.tbldrivertests.Where(c => c.d_id == d_id).FirstOrDefault();
                ss.isemailcreated = fc["isemailcreated"];
                ss.company_email = fc["company_email"];
                dc.SaveChanges();
                updatedocumentstatus(d_id);
            }
            else
            {
                var did = int.Parse(fc["did"].ToString());
                var company_email = fc["company_email"];
                var compyesno = fc["isemailcreated"];
                tbldrivertest objtest = new tbldrivertest();
                objtest.d_id = did;
                objtest.company_email = company_email;
                objtest.isemailcreated = compyesno;
                dc.tbldrivertests.Add(objtest);
                dc.SaveChanges();
                updatedocumentstatus(d_id);
            }

            if (fc["isemailcreated"].ToString() == "Yes")
            {
                var empID = dc.tbldrivers.Find(d_id).emp_id;
                AddRemarks(empID.ToString(), "Email created", "Joboffer List");
            }

            return RedirectToAction("JobOfferList");
        }

        [HttpPost]
        public ActionResult testcheck(tbldrivertest obj)
        {

            if (obj.d_id == null)
            {
                obj.d_id = int.Parse(Session["d_id"].ToString());

                if (obj.isbackgrundcheckclear == "Yes" && obj.isonbordingLinkCreated == "Yes" && obj.isemailcreated == "Yes" && obj.isdragtestclear == "Yes")
                {
                    obj.teststatus = "Yes";
                    var ll = dc.tbldrivers.Find(obj.d_id);
                    ll.d_document_status = "Done";
                    dc.SaveChanges();

                }
                else
                {
                    obj.teststatus = "No";
                }
                obj.entry_date = DateTime.Now.Date;
                obj.user_id = int.Parse(Session["admin"].ToString());
                dc.tbldrivertests.Add(obj);
                dc.SaveChanges();
            }
            else
            {

                if (obj.isbackgrundcheckclear == "Yes" && obj.isonbordingLinkCreated == "Yes" && obj.isemailcreated == "Yes" && obj.isdragtestclear == "Yes")
                {
                    obj.teststatus = "Yes";
                    var ll = dc.tbldrivers.Find(obj.d_id);
                    ll.d_document_status = "Done";
                    dc.SaveChanges();
                }
                else
                {
                    obj.teststatus = "No";
                }

                var p = dc.tbldrivertests.Find(obj.test_id);

                p.isbackgrundcheckclear = obj.isbackgrundcheckclear;
                p.isdragtestclear = obj.isdragtestclear;
                p.isemailcreated = obj.isemailcreated;
                p.isonbordingLinkCreated = obj.isonbordingLinkCreated;
                p.user_id = int.Parse(Session["admin"].ToString());
                p.entry_date = DateTime.Now.Date;
                dc.SaveChanges();



            }

            return RedirectToAction("JobOfferList");

        }

        public void getdriverlist()
        {

            var p = (from n in dc.tbldrivers
                     from l in dc.tbldrivertests

                     where n.d_id == l.d_id && n.d_document_status == "Done" && n.onbordingstatus == "Pending"
                     select n).ToList();

            List<SelectListItem> objlist = new List<SelectListItem>();
            foreach (var item in p)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.d_name;
                obj.Value = item.d_id.ToString();
                objlist.Add(obj);
            }

            ViewData["dlist"] = objlist;

        }

        public ActionResult ScheduleTraning(int id)
        {
            var p = dc.tbldrivers.Find(id);
            ViewBag.driver_name = p.d_name;
            ViewBag.d_id = p.d_id;
            //getdriverlist();
            return View();
        }

        //Schedule Traning 
        [HttpPost]
        public ActionResult ScheduleTraning(tbldrivertraning obj, FormCollection fc, HttpPostedFileBase Image1, HttpPostedFileBase Image2)
        {
            obj.user_id = int.Parse(Session["admin"].ToString());
            obj.status = "Traning Schedule";
            obj.entry_date = DateTime.Now.Date;
            //var time = DateTime.Parse(fc["training_time"]);

            // var dd = time.ToString("HH:mm");
            //var ss = TimeSpan.Parse(dd.ToString());
            // obj.training_time = TimeSpan.Parse(dd.ToString());
            obj.d_id = int.Parse(fc["d_id"]);
            var cnt = dc.tbldrivertranings.Where(c => c.d_id == obj.d_id);
            if (cnt.ToList().Count == 0)
            {

                var officeemailid = dc.tbldrivertests.Where(c => c.d_id == obj.d_id).FirstOrDefault();
                var empid = dc.tbldrivers.Find(obj.d_id).emp_id;
                var personalemail = dc.tblonbordingmasters.Find(empid);
                dc.tbldrivertranings.Add(obj);
                dc.SaveChanges();
                var training_day = DateTime.Parse(fc["VirtualDateOne"]).DayOfWeek.ToString();
                var training_date = fc["VirtualDateOne"];
                var ride_along_day = DateTime.Parse(fc["ridealongdate"]).DayOfWeek.ToString();
                var ride_along_date = fc["ridealongdate"];
                var subject = "Welcome to LMDL - " + personalemail.emp_name;
                var body = "Hi " + personalemail.emp_name + "<br>" +
                           "We are excited to welcome you to the team. Please see below details for your 1-day Virtual training.<br><br>" +
                           "<span style=\"font-weight:bold; background-color:yellow;font-size:22;\">" + training_day + ", " + training_date + " - 7.45am </span> <br><br>" +
                           "<span style=\"font-size:18;\">Virtual Training - 10 hours - Recommended to use a Tablet or Computer with Internet Access (Wifi - if possible)</span><br><br>" +
                           "We will give you more information with Login details one day before class.<br>" +
                           "I will need a Screen shot of the final Score at the end of your Training.<br><br>" +
                           "Important Points to remember:<br><br>" +
                           "1.	The training will last upto 10 hours. Make sure that the <span style=\"background-color:#33FF33;font-size:18;\">Computer / Tablet / Phone you are going to use for the training session has a Charging cable.</span> <br>" +
                           "2.	You will be given Tests periodically throughout the day and a FINAL test at the end of the session. You need to score <span style=\"background-color:#33FF33;font-size:18;\">80% or higher on the FINAL TEST.</span><br>" +
                           "3.	You will be using <span style=\"background-color:#33FF33;font-size:18;\">Chime and KNET for the entire session.</span> Details and Log in Information for these will be <span style=\"background-color:#33FF33;font-size:18;\">sent one day Prior to the Training Session.</span><br>" +
                           "4.	Make sure you <span style=\"background-color:#33FF33;font-size:18;font-weight:bold;\">Log in by 7.45 am.</span> They will do a ROLL CALL  and anyone that is not present at the time, will be moved to the next available DATE.<br>" +
                           "5.	You will get one 30-Lunch Break and two 15 min rest breaks throughout the day. <br>" +
                           "6.	Make sure you do not take extended break times. If they see inactivity for a certain period, they automatically consider it FAILED.<br><br>" +
                           "You will also be required to come in on <span style=\"background-color:yellow;font-weight:bold;font-size:18;\">" + ride_along_day + ", " + ride_along_date + " for about 2 hours in the Morning at 7.45am at the below address.</span><br><br>" +
                           "<span style=\"background-color:yellow;font-size:18;font-weight:bold;\">271-277 Doremus Ave, Newark, NJ 07105<br>" +
                           "https://goo.gl/maps/9YHqhJyUk9ZkXKat5 </span><br><br>" +
                           "This is the address you will be reporting to every day.<br>" +
                           "Thank you.<br><br>" +
                           "Last Mile Deliveries LLC (LMDL) <br>";
                string attachmentPath1 = "";
                string attachmentPath2 = "";
                string path = Server.MapPath("~/ScheduledTrainingAttatchments/");

                if (Image1 != null)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filePath = path + Path.GetFileName(Image1.FileName);
                    string fileName = Path.GetFileName(Image1.FileName).Split('.')[0];
                    string extension = Path.GetExtension(Image1.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        Image1.SaveAs(filePath);
                        attachmentPath1 = filePath;
                    }
                    else
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            int index = 1;
                            string finalFileName = fileName + "_" + index + extension;

                            while (System.IO.File.Exists(path + finalFileName))
                            {
                                finalFileName = fileName + "_" + ++index + extension;
                            }
                            string finalFilePath = path + finalFileName;
                            Image1.SaveAs(finalFilePath);
                            attachmentPath1 = finalFilePath;
                        }
                    }

                }

                if (Image2 != null)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filePath = path + Path.GetFileName(Image2.FileName);
                    string fileName = Path.GetFileName(Image2.FileName).Split('.')[0];
                    string extension = Path.GetExtension(Image2.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        Image2.SaveAs(filePath);
                        attachmentPath2 = filePath;
                    }
                    else
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            int index = 1;
                            string finalFileName = fileName + "_" + index + extension;

                            while (System.IO.File.Exists(path + finalFileName))
                            {
                                finalFileName = fileName + "_" + ++index + extension;
                            }
                            string finalFilePath = path + finalFileName;
                            Image2.SaveAs(finalFilePath);
                            attachmentPath2 = finalFilePath;
                        }
                    }
                }
                string ccMail = ConfigurationManager.AppSettings["CCEmailIDHR"].ToString();
                SendEmail_Fixed(officeemailid.company_email, ccMail, subject, body, attachmentPath1, attachmentPath2);

                AddRemarks(empid.ToString(), "Virtual training - " + training_date, "Schedule Driver Training");

                getdriverlist();
                ViewBag.msg = "Traning Schedule successfully";
                ModelState.Clear();

            }
            else
            {
                getdriverlist();

                ViewBag.msg = "Traning Already Reschdule";
                ModelState.Clear();

            }

            // getdriverlist();
            return RedirectToAction("ScheduleTraningList");
        }

        public ActionResult ScheduleTraningList()
        {
            //var p = (from n in dc.tbldrivertranings
            //         from l in dc.tbldrivers
            //         where n.d_id == l.d_id && n.status == "Traning Schedule"
            //         select new { l.emp_id, l.d_name, l.d_phone, n.training_date, n.training_time, n.user_id, n.entry_date, n.d_id, n.traning_id, n.inclassdateone, n.inclassdatetwo, n.ridealongdate, n.ridealongpersonname }).ToList();

            //var p = (from n in dc.tbldrivertranings
            //         from l in dc.tbldrivers
            //         from tdts in dc.tbldrivertests
            //         where l.d_id == tdts.d_id && n.d_id == l.d_id && l.d_document_status == "Done" && l.onbordingstatus == "Pending"
            //         select new
            //         {
            //             l.emp_id,
            //             l.d_name,
            //             l.d_phone,

            //             n.training_date,
            //             n.training_time,
            //             n.user_id,
            //             n.entry_date,
            //             n.d_id,
            //             n.traning_id,
            //             n.inclassdateone,
            //             n.inclassdatetwo,
            //             n.ridealongdate,
            //             n.ridealongpersonname
            //         }).ToList();

            var p = (from n in dc.tbldrivers
                     from l in dc.tbldrivertests

                     where n.d_id == l.d_id && n.d_document_status == "Done" && n.onbordingstatus == "Pending"
                     select n).ToList();

            List<ScheduleTraningModel> objlist = new List<ScheduleTraningModel>();

            foreach (var item in p)
            {
                var drvtraining = dc.tbldrivertranings.Where(w => w.d_id == item.d_id).ToList();

                ScheduleTraningModel obj = new ScheduleTraningModel();

                obj.d_name = item.d_name;
                obj.d_id = item.d_id;
                obj.d_number = item.d_phone;
                obj.entry_date = item.entry_date;
                if (drvtraining.Count > 0)
                {
                    obj.user_id = item.user_id;
                    obj.training_date = drvtraining.FirstOrDefault().training_date;
                    obj.training_time = drvtraining.FirstOrDefault().training_time;
                    obj.traning_id = drvtraining.FirstOrDefault().traning_id;
                    obj.inclassdateone = drvtraining.FirstOrDefault().inclassdateone;
                    obj.inclassdatetwo = drvtraining.FirstOrDefault().inclassdatetwo;
                    obj.inclassridedate = drvtraining.FirstOrDefault().ridealongdate;
                    obj.ridealongname = drvtraining.FirstOrDefault().ridealongpersonname;
                    obj.training_type = drvtraining.FirstOrDefault().training_type;
                    obj.ridealongdate = drvtraining.FirstOrDefault().ridealongdate;
                    obj.VirtualDateOne = drvtraining.FirstOrDefault().VirtualDateOne;
                    obj.RideAlongTrainee = drvtraining.FirstOrDefault().RideAlongTrainee;
                }
                else
                {
                    //obj.training_date = null;
                    //obj.training_time = null;
                    //obj.traning_id = drvtraining.FirstOrDefault().traning_id;
                    //obj.inclassdateone = drvtraining.FirstOrDefault().inclassdateone;
                    //obj.inclassdatetwo = drvtraining.FirstOrDefault().inclassdatetwo;
                    //obj.inclassridedate = drvtraining.FirstOrDefault().ridealongdate;
                    //obj.ridealongname = drvtraining.FirstOrDefault().ridealongpersonname;
                }

                obj.emp_id = Convert.ToInt32(item.emp_id.ToString());
                objlist.Add(obj);
            }
            return View(objlist);

        }

        public ActionResult EditRescheduleTraning(int id)
        {

            var p = (from n in dc.tbldrivertranings
                     from l in dc.tbldrivers
                     where n.d_id == l.d_id && n.traning_id == id
                     select new
                     {
                         l.d_name,
                         l.d_phone,
                         n.training_date,
                         n.training_time,
                         n.user_id,
                         n.entry_date,
                         n.d_id,
                         n.traning_id,
                         n.inclassdateone,
                         n.inclassdatetwo,
                         n.ridealongdate,
                         n.ridealongpersonname,

                         n.training_type,
                         n.inClassTimeTwo,
                         n.VirtualDateOne,
                         n.VirtualTime,
                         n.RideAlongTrainee
                     }).ToList();

            List<ScheduleTraningModel> objlist = new List<ScheduleTraningModel>();

            foreach (var item in p)
            {
                ScheduleTraningModel obj = new ScheduleTraningModel();
                obj.d_name = item.d_name;
                obj.d_id = item.d_id;
                obj.d_number = item.d_phone;
                obj.training_date = item.training_date;
                obj.training_time = item.training_time;
                obj.user_id = item.user_id;
                obj.entry_date = item.entry_date;
                obj.traning_id = item.traning_id;
                obj.inclassdateone = item.inclassdateone;
                obj.inclassdatetwo = item.inclassdatetwo;
                obj.inclassridedate = item.ridealongdate;
                obj.ridealongname = item.ridealongpersonname;

                obj.training_type = item.training_type;
                obj.inClassTimeTwo = item.inClassTimeTwo;
                obj.VirtualDateOne = item.VirtualDateOne;
                obj.VirtualTime = item.VirtualTime;
                obj.RideAlongTrainee = item.RideAlongTrainee;

                objlist.Add(obj);
            }




            var p1 = objlist.FirstOrDefault();
            getdriverlist();
            return View(p1);


        }

        [HttpPost]
        public ActionResult EditRescheduleTraning(ScheduleTraningModel obj, FormCollection fc, HttpPostedFileBase Image1)
        {
            tbldrivertraning finalobj = new tbldrivertraning();

            var p = dc.tbldrivertranings.Find(obj.traning_id);

            var d_id = p.d_id;

            p.training_date = obj.training_date;
            // p.training_time = obj.training_time;
            p.inclassdateone = obj.inclassdateone;
            p.inclassdatetwo = obj.inclassdatetwo;
            p.ridealongdate = obj.inclassridedate;
            // p.ridealongpersonname = obj.ridealongname;

            p.training_type = obj.training_type;
            p.VirtualDateOne = obj.VirtualDateOne;
            p.RideAlongTrainee = obj.RideAlongTrainee;

            p.user_id = int.Parse(Session["admin"].ToString());
            dc.SaveChanges();

            var training_day = DateTime.Parse(fc["VirtualDateOne"]).DayOfWeek.ToString();
            var training_date = fc["VirtualDateOne"];
            var ride_along_day = DateTime.Parse(fc["inclassridedate"]).DayOfWeek.ToString();
            var ride_along_date = fc["inclassridedate"];

            var tbldriver = dc.tbldrivers.Find(d_id);
            var tbldrivertst = dc.tbldrivertests.Where(w => w.d_id == d_id).FirstOrDefault();

            var subject = "Rescheduled Driver Training - " + tbldriver.d_name;
            var body = "Hi " + tbldriver.d_name + "<br>" +
                       "We are excited to welcome you to the team. Please see below details for your 1-day Virtual training.<br><br>" +
                       "<span style=\"font-weight:bold; background-color:yellow;font-size:18;\">" + training_day + ", " + training_date + " - 7.45am </span> <br><br>" +
                       "<span style=\"font-size:18;\">Virtual Training - 10 hours - Recommended to use a Tablet or Computer with Internet Access (Wifi - if possible)</span><br><br>" +
                       "We will give you more information with Login details one day before class.<br>" +
                       "I will need a Screen shot of the final Score at the end of your Training.<br><br>" +
                       "Important Points to remember:<br><br>" +
                       "1.	The training will last upto 10 hours. Make sure that the <span style=\"background-color:#33FF33;\">Computer / Tablet / Phone you are going to use for the training session has a Charging cable.</span> <br>" +
                       "2.	You will be given Tests periodically throughout the day and a FINAL test at the end of the session. You need to score <span style=\"background-color:#33FF33;\">80% or higher on the FINAL TEST.</span><br>" +
                       "3.	You will be using <span style=\"background-color:#33FF33;\">Chime and KNET for the entire session.</span> Details and Log in Information for these will be <span style=\"background-color:#33FF33;\">sent one day Prior to the Training Session.</span><br>" +
                       "4.	Make sure you <span style=\"background-color:#33FF33;font-weight:bold;\">Log in by 7.45 am.</span> They will do a ROLL CALL  and anyone that is not present at the time, will be moved to the next available DATE.<br>" +
                       "5.	You will get one 30-Lunch Break and two 15 min rest breaks throughout the day. <br>" +
                       "6.	Make sure you do not take extended break times. If they see inactivity for a certain period, they automatically consider it FAILED.<br><br>" +
                       "You will also be required to come in on <span style=\"background-color:yellow;font-weight:bold;\">" + ride_along_day + ", " + ride_along_date + " for about 2 hours in the Morning at 7.45am at the below address.</span><br><br>" +
                       "<span style=\"background-color:yellow;font-size:18;\">271-277 Doremus Ave, Newark, NJ 07105<br>" +
                       "https://goo.gl/maps/9YHqhJyUk9ZkXKat5 </span><br><br>" +
                       "This is the address you will be reporting to every day.<br>" +
                       "Thank you.<br><br>" +
                       "Last Mile Deliveries LLC (LMDL)  <br>";
            string attachmentPath1 = "";
            string path = Server.MapPath("~/ScheduledTrainingAttatchments/");

            if (Image1 != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var filePath = path + Path.GetFileName(Image1.FileName);
                string fileName = Path.GetFileName(Image1.FileName).Split('.')[0];
                string extension = Path.GetExtension(Image1.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    Image1.SaveAs(filePath);
                    attachmentPath1 = filePath;
                }
                else
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        int index = 1;
                        string finalFileName = fileName + "_" + index + extension;

                        while (System.IO.File.Exists(path + finalFileName))
                        {
                            finalFileName = fileName + "_" + ++index + extension;
                        }
                        string finalFilePath = path + finalFileName;
                        Image1.SaveAs(finalFilePath);
                        attachmentPath1 = finalFilePath;
                    }
                }

            }

            string ccMail = ConfigurationManager.AppSettings["CCEmailIDHR"].ToString();
            SendEmail_Fixed(tbldrivertst.company_email, ccMail, subject, body, attachmentPath1, "");

            AddRemarks(tbldriver.emp_id.ToString(), "Virtual training - " + training_date, "Reschedule Driver");

            return RedirectToAction("ScheduleTraningList");
        }

        public ActionResult rescheduleDriverTraning(int id)
        {
            ViewBag.d_id = id;
            ViewBag.d_name = dc.tbldrivers.Find(id).d_name;
            ViewBag.phone = dc.tbldrivers.Find(id).d_phone;
            ViewBag.personalemail = dc.tbldrivers.Find(id).d_email;
            ViewBag.workemail = dc.tbldrivertests.Where(w => w.d_id == id).FirstOrDefault().company_email;

            var p = dc.tbldrivertranings.Where(c => c.d_id == id).FirstOrDefault();
            return View(p);

        }

        [HttpPost]
        public ActionResult rescheduleDriverTraning(FormCollection fc)
        {
            var d_id = Convert.ToInt32(fc["d_id"]);
            var name = fc["drivername"];
            var phone = fc["phone"];
            var pemail = fc["personalemail"];
            var wemail = fc["workemail"];

            var p1 = dc.tbldrivers.Find(d_id);
            p1.d_name = name;
            p1.d_phone = phone;
            p1.d_email = pemail;
            dc.SaveChanges();

            var p2 = dc.tbldrivertests.Where(w => w.d_id == d_id).ToList();
            p2.FirstOrDefault().company_email = wemail;
            dc.SaveChanges();

            var p3 = dc.tblonbordingmasters.Find(p1.emp_id);
            p3.emp_name = name;
            p3.emp_phone = phone;
            p3.emp_personal_email = pemail;
            dc.SaveChanges();


            return RedirectToAction("ScheduleTraningList");

        }

        public ActionResult driverridesetup(int id)
        {
            Session["d_id"] = id;

            return View();
        }

        [HttpPost]
        public ActionResult driverridesetup(tblridesetup obj)
        {

            int driverid = int.Parse(Session["d_id"].ToString());
            obj.d_id = int.Parse(Session["d_id"].ToString());
            obj.entry_date = DateTime.Now.Date;
            obj.user_id = int.Parse(Session["admin"].ToString());

            dc.tblridesetups.Add(obj);

            var pp = dc.tbldrivertranings.Where(c => c.d_id == driverid).FirstOrDefault();
            pp.status = "done";
            dc.SaveChanges();
            var ss = dc.tbldrivers.Find(driverid);
            ss.onbordingstatus = "done";
            dc.SaveChanges();

            // setup final entry details;
            var details = dc.tbldrivers.Find(driverid);
            var testing = dc.tbldrivertests.Where(c => c.d_id == driverid).FirstOrDefault();
            var tbldrier = dc.tbldrivertranings.Where(c => c.d_id == driverid).FirstOrDefault();
            tblfinaldrivermaster finalobjmaster = new tblfinaldrivermaster();
            finalobjmaster.final_driver_name = details.d_name;
            finalobjmaster.final_driver_personal_email = details.d_email;
            finalobjmaster.final_driver_company_email = testing.company_email;
            finalobjmaster.personal_mobile_no = details.d_phone;
            finalobjmaster.dob = details.d_dob;
            finalobjmaster.driver_license_no = details.d_license_number;
            finalobjmaster.driver_license_state = details.d_license_state;
            finalobjmaster.driver_full_address = details.d_address;
            finalobjmaster.driver_ssn = details.d_ssn;
            finalobjmaster.driver_status = "active";
            finalobjmaster.ride_along_driver_name = tbldrier.ridealongpersonname;
            finalobjmaster.ride_along_date = tbldrier.ridealongdate;
            finalobjmaster.traning_date_one = tbldrier.inclassdateone;
            finalobjmaster.traning_date_two = tbldrier.inclassdatetwo;
            finalobjmaster.d_id = driverid;

            var ss12 = dc.tbldrivers.Find(driverid).emp_id;

            var driver_linc_photo = dc.tblonbordingmasters.Find(ss12).upload_linces_photo;

            finalobjmaster.driver_license_photo = driver_linc_photo;

            dc.tblfinaldrivermasters.Add(finalobjmaster);



            dc.SaveChanges();
            return RedirectToAction("ScheduleTraningList");

        }

        public ActionResult RejectedList()
        {

            var p = dc.tblonbordingmasters.Where(c => c.Entry_status == "Rejected").ToList();

            return View(p);

        }

        public ActionResult viewprofile()
        {

            return View();


        }

        public ActionResult rejectionfromjobofferlist(int id)
        {
            Session["reject_id"] = id;
            Session["chklist"] = 0;
            ViewBag.DAname = dc.tblonbordingmasters.Find(id).emp_name;

            return View();
        }

        [HttpPost]
        public ActionResult rejectionfromjobofferlist(FormCollection fc)
        {
            var backgroundcheck = Convert.ToBoolean(fc["backgroundcheckfail"].Split(',')[0]);
            //bool MyBoolValue = Convert.ToBoolean(collection["showAll"].Split(',')[0]);
            var drugtestfailed = Convert.ToBoolean(fc["drugtestfailed"].Split(',')[0]);
            var noshowonfordrugtesting = Convert.ToBoolean(fc["noshowonfordrugtesting"].Split(',')[0]);
            var withdrewapplication = Convert.ToBoolean(fc["withdrewapplication"].Split(',')[0]);
            var other = Convert.ToBoolean(fc["other"].Split(',')[0]);
            var resdesc = "";
            if (backgroundcheck == true)
            {
                resdesc += "Background Check Fail" + ";";

            }
            if (drugtestfailed == true)
            {
                resdesc += "Drug Test Failed" + ";";

            }
            if (noshowonfordrugtesting == true)
            {
                resdesc += "No Show On For drug Testing" + ";";
            }
            if (withdrewapplication == true)
            {
                resdesc += "Withdrew Application" + ";";
            }
            if (other == true)
            {
                resdesc += fc["rejection_resion"] + ";";

            }


            int id = int.Parse(Session["reject_id"].ToString());
            var p = dc.tblonbordingmasters.Find(id);
            p.rejection_resion = resdesc.ToString();
            p.Entry_status = "Rejected";
            dc.SaveChanges();

            AddRemarks(id.ToString(), resdesc.ToString(), "Job Offer List");

            var ss = dc.tbldrivers.Where(c => c.emp_id == id).FirstOrDefault();
            dc.tbldrivers.Remove(ss);
            dc.SaveChanges();


            return RedirectToAction("jobOfferList");



        }

        public ActionResult rejectionpopup(int id, string rejflg)
        {
            Session["rejflg"] = rejflg;

            Session["reject_id"] = id;

            Session["chklist"] = 1;

            return View();
        }

        [HttpPost]
        public ActionResult rejectionpopup(FormCollection fc)
        {

            var backgroundcheck = Convert.ToBoolean(fc["backgroundcheckfail"].Split(',')[0]);
            //bool MyBoolValue = Convert.ToBoolean(collection["showAll"].Split(',')[0]);
            var drugtestfailed = Convert.ToBoolean(fc["drugtestfailed"].Split(',')[0]);
            var noshowonfordrugtesting = Convert.ToBoolean(fc["noshowonfordrugtesting"].Split(',')[0]);
            var withdrewapplication = Convert.ToBoolean(fc["withdrewapplication"].Split(',')[0]);
            var other = Convert.ToBoolean(fc["other"].Split(',')[0]);
            var resdesc = "";
            if (backgroundcheck == true)
            {
                resdesc += "Background Check Fail" + ";";

            }
            if (drugtestfailed == true)
            {
                resdesc += "Drug Test Failed" + ";";

            }
            if (noshowonfordrugtesting == true)
            {
                resdesc += "No Show On For drug Testing" + ";";
            }
            if (withdrewapplication == true)
            {
                resdesc += "Withdrew Application" + ";";
            }
            if (other == true)
            {
                resdesc += fc["rejection_resion"] + ";";

            }

            int id = int.Parse(Session["reject_id"].ToString());
            var p = dc.tblonbordingmasters.Find(id);
            p.rejection_resion = resdesc.ToString();
            p.Entry_status = "Rejected";

            dc.SaveChanges();

            var sst = Session["rejflg"].ToString();

            if (sst == "app")
            {

                return RedirectToAction("applicationSubmitedList");
            }
            else
            {
                return RedirectToAction("JobOfferList");

            }
        }

        public ActionResult Logout()
        {

            Session.Abandon();
            Session["admin"] = null;
            return RedirectToAction("Login", "Main");
        }

        //set message setup
        public void sendsms(string mob, string message)
        {
            var finalmob = "91" + mob;
            string strulr = "http://smsc.smsconnexion.com/api/gateway.aspx?action=send&username=Tejas&passphrase=123456&message=" + message + "&phone=" + finalmob + "&from=WEBVEC";


            // List<Employee> list = new List<Employee>();
            HttpClient client = new HttpClient();
            var result = client.GetAsync(strulr).Result;
            if (result.IsSuccessStatusCode)
            {
                // list = result.Content.ReadAsAsync<List<Employee>>().Result;
            }

        }

        public void sendemail(string msg, string to, string sub)
        {


            string strulr = "http://diary.wvclients.in/app_api/emailtest.php?message=" + msg + "&to=" + to + "&sub=" + sub;
            // List<Employee> list = new List<Employee>();
            HttpClient client = new HttpClient();

            var result = client.GetAsync(strulr).Result;
            if (result.IsSuccessStatusCode)
            {
                // list = result.Content.ReadAsAsync<List<Employee>>().Result;
            }


        }

        public bool SendEmail_Fixed(string toEmail, string ccMail, string mailSubject, string mailBody)
        {
            Session["Error"] = "";
            Session["Creds"] = "";
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["smtpUser"].ToString();
                string fromPassword = ConfigurationManager.AppSettings["smtpPass"].ToString();
                string smtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
                string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
                string IsEnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString();
                bool EnableSsl = IsEnableSSL == "true" ? true : false;
                Session["Creds"] = "SMTP server - " + smtpServer + ", SMTP port - " + smtpPort + ", FromEmail - " + fromEmail + ", Password - " + fromPassword;

                // add company logo to email
                string path = Server.MapPath("~/Logimages/");
                string fullPath = path + "companyEmailIcon.png";
                var linkedResource = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                //var htmlBody = $"<img src=\"cid:siteLogo\" width=\"70\" height=\"70\"/>";
                linkedResource.ContentId = "siteLogo";
                var alternateView = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateView.LinkedResources.Add(linkedResource);

                #region mail send with zoho api...
                // mail send with zoho api...
                //const string WEBSERVICE_URL = "https://mail.zoho.in/api/accounts/714785090/messages";

                //var webRequest = System.Net.WebRequest.Create(WEBSERVICE_URL);
                //if (webRequest != null)
                //{
                //    webRequest.Method = "POST";
                //    webRequest.Headers.Add("Authorization", "{token}}");
                //    webRequest.ContentType = "application/json";

                //    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                //    {
                //        string json = "{" +
                //                            "\"fromAddress\": \"" + fromEmail + "\"," +
                //                            "\"toAddress\": \"" + toEmail + "\"," +
                //                            "\"subject\": \"" + mailSubject + "\"" +
                //                        "}";



                //        streamWriter.Write(json);
                //        streamWriter.Flush();
                //        streamWriter.Close();
                //    }

                //    var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //    {
                //        var result = streamReader.ReadToEnd();
                //        Console.WriteLine(String.Format("Response: {0}", result));
                //    }
                //}
                //return true;

                #endregion

                //Working code
                //SmtpClient smtp = new SmtpClient(smtpServer,Convert.ToInt32(smtpPort));
                SmtpClient smtp = new SmtpClient(smtpServer, Convert.ToInt32(smtpPort));
                smtp.EnableSsl = EnableSsl;
                smtp.Timeout = 100000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);

                MailMessage ms = new MailMessage(fromEmail, toEmail, mailSubject, mailBody);
                ms.IsBodyHtml = true;
                ms.BodyEncoding = UTF8Encoding.UTF8;
                //ms.CC.Add(ccMail);
                ms.AlternateViews.Add(alternateView);

                smtp.Send(ms);

                return true;
            }
            catch (Exception ex)
            {
                Session["Error"] = ex.ToString();
                return false;
            }
        }

        public bool SendEmail_HRchecklist(string toEmail, string ccMail, string mailSubject, string mailBody)
        {
            Session["Error"] = "";
            Session["Creds"] = "";
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["smtpUser"].ToString();
                string fromPassword = ConfigurationManager.AppSettings["smtpPass"].ToString();
                string smtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
                string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
                string IsEnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString();
                bool EnableSsl = IsEnableSSL == "true" ? true : false;
                Session["Creds"] = "SMTP server - " + smtpServer + ", SMTP port - " + smtpPort + ", FromEmail - " + fromEmail + ", Password - " + fromPassword;

                // add company logo to email
                string path = Server.MapPath("~/Logimages/");
                string fullPath = path + "companyEmailIcon.png";
                var linkedResource = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResource.ContentId = "siteLogo";
                var alternateView = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateView.LinkedResources.Add(linkedResource);

                //Working code
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
                ms.AlternateViews.Add(alternateView);

                smtp.Send(ms);

                return true;
            }
            catch (Exception ex)
            {
                Session["Error"] = ex.ToString();
                return false;
            }
        }

        public bool SendEmail_HRchecklistI9DocGas(string toEmail, string ccMail, string mailSubject, string mailBody, string attachmentPath1)
        {
            Session["Error"] = "";
            Session["Creds"] = "";
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["smtpUser"].ToString();
                string fromPassword = ConfigurationManager.AppSettings["smtpPass"].ToString();
                string smtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
                string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
                string IsEnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString();
                bool EnableSsl = IsEnableSSL == "true" ? true : false;
                Session["Creds"] = "SMTP server - " + smtpServer + ", SMTP port - " + smtpPort + ", FromEmail - " + fromEmail + ", Password - " + fromPassword;
                Attachment attachment = null;
                
                // add company logo to email
                string path = Server.MapPath("~/Logimages/");
                string fullPath = path + "companyEmailIcon.png";
                var linkedResource = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResource.ContentId = "siteLogo";
                var alternateView = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateView.LinkedResources.Add(linkedResource);

                //Working code
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
                ms.AlternateViews.Add(alternateView);

                if (attachmentPath1.Trim() != "")
                {
                    attachment = new Attachment(attachmentPath1);
                    ms.Attachments.Add(attachment);
                }

                smtp.Send(ms);
                attachment.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Session["Error"] = ex.ToString();
                return false;
            }
        }

        public bool SendEmail_HRchecklistADPInstruction(string toEmail, string ccMail, string mailSubject, string mailBody)
        {
            Session["Error"] = "";
            Session["Creds"] = "";
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["smtpUser"].ToString();
                string fromPassword = ConfigurationManager.AppSettings["smtpPass"].ToString();
                string smtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
                string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
                string IsEnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString();
                bool EnableSsl = IsEnableSSL == "true" ? true : false;
                Session["Creds"] = "SMTP server - " + smtpServer + ", SMTP port - " + smtpPort + ", FromEmail - " + fromEmail + ", Password - " + fromPassword;
                string path = "";
                string fullPath = "";


                // add application screen shots one
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "1.png";
                var linkedResourceone = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourceone.ContentId = "one";
                var alternateViewOne = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewOne.LinkedResources.Add(linkedResourceone);

                // add application screen shots two
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "2.png";
                var linkedResourcetwo = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourcetwo.ContentId = "two";
                var alternateViewTwo = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewTwo.LinkedResources.Add(linkedResourcetwo);

                // add application screen shots three
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "3.png";
                var linkedResourcethree = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourcethree.ContentId = "three";
                var alternateViewThree = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewThree.LinkedResources.Add(linkedResourcethree);

                // add application screen shots four
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "4.png";
                var linkedResourcefour = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourcefour.ContentId = "four";
                var alternateViewFour = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewFour.LinkedResources.Add(linkedResourcefour);

                // add application screen shots five
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "5.png";
                var linkedResourcefive = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourcefive.ContentId = "five";
                var alternateViewFive = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewFive.LinkedResources.Add(linkedResourcefive);

                // add application screen shots six
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "6.png";
                var linkedResourcesix = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourcesix.ContentId = "six";
                var alternateViewSix = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewSix.LinkedResources.Add(linkedResourcesix);

                // add application screen shots seven
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "7.png";
                var linkedResourceseven = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourceseven.ContentId = "seven";
                var alternateViewSeven = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewSeven.LinkedResources.Add(linkedResourceseven);

                // add application screen shots eight
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "8.png";
                var linkedResourceeight = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourceeight.ContentId = "eight";
                var alternateViewEight = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewEight.LinkedResources.Add(linkedResourceeight);

                // add application screen shots nine
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "9.png";
                var linkedResourcenine = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourcenine.ContentId = "nine";
                var alternateViewNine = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewNine.LinkedResources.Add(linkedResourcenine);

                // add application screen shots ten
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "10.png";
                var linkedResourceten = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourceten.ContentId = "ten";
                var alternateViewTen = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewTen.LinkedResources.Add(linkedResourceten);

                // add application screen shots eleven
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "11.png";
                var linkedResourceeleven = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourceeleven.ContentId = "eleven";
                var alternateViewEleven = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewEleven.LinkedResources.Add(linkedResourceeleven);

                // add application screen shots twleve
                path = Server.MapPath("~/HRemailCommonFiles/");
                fullPath = path + "12.png";
                var linkedResourcetwleve = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResourcetwleve.ContentId = "twelve";
                var alternateViewTwelve = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewTwelve.LinkedResources.Add(linkedResourcetwleve);


                // add company logo to email
                path = Server.MapPath("~/Logimages/");
                fullPath = path + "companyEmailIcon.png";
                var linkedResource = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResource.ContentId = "siteLogo";
                var alternateViewCompIcon = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateViewCompIcon.LinkedResources.Add(linkedResource);

                //Working code
                SmtpClient smtp = new SmtpClient(smtpServer, Convert.ToInt32(smtpPort));
                smtp.EnableSsl = EnableSsl;
                smtp.Timeout = 200000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);

                MailMessage ms = new MailMessage(fromEmail, toEmail, mailSubject, mailBody);
                ms.IsBodyHtml = true;
                ms.BodyEncoding = UTF8Encoding.UTF8;
                ms.CC.Add(ccMail);
                ms.AlternateViews.Add(alternateViewOne);
                ms.AlternateViews.Add(alternateViewTwo);
                ms.AlternateViews.Add(alternateViewThree);
                ms.AlternateViews.Add(alternateViewFour);
                ms.AlternateViews.Add(alternateViewFive);
                ms.AlternateViews.Add(alternateViewSix);
                ms.AlternateViews.Add(alternateViewSeven);
                ms.AlternateViews.Add(alternateViewEight);
                ms.AlternateViews.Add(alternateViewNine);
                ms.AlternateViews.Add(alternateViewTen);
                ms.AlternateViews.Add(alternateViewEleven);
                ms.AlternateViews.Add(alternateViewTwelve);
                ms.AlternateViews.Add(alternateViewCompIcon);

                smtp.Send(ms);

                return true;
            }
            catch (Exception ex)
            {
                Session["Error"] = ex.ToString();
                return false;
            }
        }

        public bool SendEmail_Fixed(string toEmail, string ccEmail, string mailSubject, string mailBody, string attachmentPath1, string attachmentPath2)
        {
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["smtpUser"].ToString();
                string fromPassword = ConfigurationManager.AppSettings["smtpPass"].ToString();
                string smtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
                string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
                string IsEnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString();
                bool EnableSsl = IsEnableSSL == "true" ? true : false;
                Attachment attachment = null;

                SmtpClient smtp = new SmtpClient(smtpServer, Convert.ToInt32(smtpPort));
                smtp.EnableSsl = EnableSsl;
                smtp.Timeout = 100000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);

                MailMessage ms = new MailMessage(fromEmail, toEmail, mailSubject, mailBody);
                ms.IsBodyHtml = true;
                ms.BodyEncoding = UTF8Encoding.UTF8;
                ms.CC.Add(ccEmail);

                if (attachmentPath1.Trim() != "")
                {
                    attachment = new Attachment(attachmentPath1);
                    ms.Attachments.Add(attachment);
                }

                if (attachmentPath2.Trim() != "")
                {
                    attachment = new Attachment(attachmentPath2);
                    ms.Attachments.Add(attachment);
                }

                smtp.Send(ms);
                attachment.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendEmail_HRchecklistInsurance(string toEmail, string ccMail, string mailSubject, string mailBody)
        {
            Session["Error"] = "";
            Session["Creds"] = "";
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["smtpUser"].ToString();
                string fromPassword = ConfigurationManager.AppSettings["smtpPass"].ToString();
                string smtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
                string smtpPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
                string IsEnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString();
                bool EnableSsl = IsEnableSSL == "true" ? true : false;
                Session["Creds"] = "SMTP server - " + smtpServer + ", SMTP port - " + smtpPort + ", FromEmail - " + fromEmail + ", Password - " + fromPassword;
                Attachment attachment = null;
                
                string pathCommonFiles = Server.MapPath("~/HRemailCommonFiles/");

                string attachmentPath1 = "";
                attachmentPath1 = pathCommonFiles + "PBE_Dental_Advantage.pdf";

                string attachmentPath2 = "";
                attachmentPath2 = pathCommonFiles + "PBE_Vision_Select.pdf";

                string attachmentPath3 = "";
                attachmentPath3 = pathCommonFiles + "Plan_Options_2020_Last_Mile_Deliveries.pdf";


                // add company logo to email
                string path = Server.MapPath("~/Logimages/");
                string fullPath = path + "companyEmailIcon.png";
                var linkedResource = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg);
                linkedResource.ContentId = "siteLogo";
                var alternateView = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
                alternateView.LinkedResources.Add(linkedResource);

                //Working code
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
                ms.AlternateViews.Add(alternateView);

                if (attachmentPath1.Trim() != "")
                {
                    attachment = new Attachment(attachmentPath1);
                    ms.Attachments.Add(attachment);
                }

                if (attachmentPath2.Trim() != "")
                {
                    attachment = new Attachment(attachmentPath2);
                    ms.Attachments.Add(attachment);
                }

                if (attachmentPath3.Trim() != "")
                {
                    attachment = new Attachment(attachmentPath3);
                    ms.Attachments.Add(attachment);
                }

                smtp.Send(ms);
                attachment.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Session["Error"] = ex.ToString();
                return false;
            }
        }

        public void SendEmail_Fixed()
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("test@lastmiled.com");

            message.To.Add(new MailAddress("vaibhavmawale123@gmail.com"));

            message.Subject = "your subject";
            message.Body = "content of your email";

            SmtpClient client = new SmtpClient();
            client.Send(message);
        }

        //[Obsolete("Do not use this in Production code!!!", true)]
        //static void NEVER_EAT_POISON_Disable_CertificateValidation()
        //{
        //    // Disabling certificate validation can expose you to a man-in-the-middle attack
        //    // which may allow your encrypted message to be read by an attacker
        //    // https://stackoverflow.com/a/14907718/740639
        //    ServicePointManager.ServerCertificateValidationCallback =
        //        delegate (
        //            object s,
        //            X509Certificate certificate,
        //            X509Chain chain,
        //            SslPolicyErrors sslPolicyErrors
        //        ) {
        //            return true;
        //        };
        //}

        public ActionResult uploadCsvfile()
        {

            return View();
        }

        [HttpPost]
        public ActionResult uploadCsvfile(HttpPostedFileBase postedFile)
        {

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
                        return View();
                    }
                    //Create a DataTable.
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[19]
                    {
                        new DataColumn("driver_name", typeof(string)),
                        new DataColumn("driver_personal_email", typeof(string)),
                        new DataColumn("driver_company_email",typeof(string)),
                        new DataColumn("personal_mobile_no",typeof(string)),
                        new DataColumn("company_mobile_no",typeof(string)),
                        new DataColumn("dob",typeof(string)),
                        new DataColumn("driver_linese_no",typeof(string)),
                        new DataColumn("driver_lincense_state",typeof(string)),
                        new DataColumn("driver_full_address",typeof(string)),
                        new DataColumn("driver_ssn",typeof(string)),
                        new DataColumn("job_offer_date",typeof(string)),
                        new DataColumn("join_date",typeof(string)),
                        new DataColumn("driver_adp_id",typeof(string)),
                        new DataColumn("total_exp",typeof(string)),
                        new DataColumn("traning_date_one",typeof(string)),
                        new DataColumn("traning_date_two",typeof(string)),
                        new DataColumn("ride_along_date",typeof(string)),
                        new DataColumn("ride_along_driver_name",typeof(string)),
                        new DataColumn("driver_status",typeof(string))

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

                            ViewBag.msg = "File format is not proper according to system";
                            return View();
                        }

                    }


                    if (dt.Rows.Count > 0)
                    {

                    }

                    string conString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(conString))
                    {

                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            try
                            {
                                var dob = dt.Rows[i]["dob"].ToString();
                                if (dob == "")
                                {
                                    dob = "1990-09-05";

                                }

                                var job_offer_date = dt.Rows[i]["job_offer_date"].ToString();

                                if (job_offer_date == "")
                                {
                                    job_offer_date = "1990-09-05";

                                }
                                var join_date = dt.Rows[i]["join_date"].ToString();
                                if (join_date == "")
                                {
                                    join_date = "1990-09-05";
                                }
                                var traning_date_one = dt.Rows[i]["traning_date_one"].ToString();

                                if (traning_date_one == "")
                                {
                                    traning_date_one = "1990-09-05";
                                }

                                var traning_date_two = dt.Rows[i]["traning_date_two"].ToString();
                                if (traning_date_two == "")
                                {
                                    traning_date_two = "1990-09-05";
                                }

                                var ride_along_date = dt.Rows[i]["ride_along_date"].ToString();
                                if (ride_along_date == "")
                                {
                                    ride_along_date = "1990-09-05";
                                }
                                var driver_status = dt.Rows[i]["driver_status"].ToString().Trim();
                                if (driver_status == "")
                                {
                                    driver_status = "active";
                                }
                                else
                                {

                                }
                                // ViBes 11 May 2020
                                string query = " DECLARE " +
                                               "     @IsDriverPresent INT = 0, " +
                                               "     @DriverName VARCHAR(200) = '" + dt.Rows[i]["driver_name"] + "';" +
                                               "    SELECT @IsDriverPresent = COUNT(*) FROM tblfinaldrivermaster WHERE final_driver_name = @DriverName " +
                                               " IF(@IsDriverPresent > 0) " +
                                               " BEGIN " +
                                               "     SELECT @IsDriverPresent = COUNT(*) FROM tblfinaldrivermaster WHERE ISNULL(driver_status,'') = 'active' AND final_driver_name = @DriverName " +
                                               "     IF(@IsDriverPresent > 0) " +
                                               "     BEGIN " +
                                               "         SELECT 2 " + //-- UPDATE
                                               "    END " +
                                               "    ELSE " +
                                               "     BEGIN " +
                                               "         SELECT 0 " + // Not to do anything
                                               "     END" +
                                               " END " +
                                               " ELSE " +
                                               " BEGIN " +
                                               "     SELECT 1 " + //-- INSERT
                                               " END";

                                SqlCommand cmd1 = new SqlCommand();
                                cmd1.CommandText = query;
                                cmd1.Connection = con;
                                con.Open();
                                int IfExists = Convert.ToInt32(cmd1.ExecuteScalar().ToString());
                                con.Close();

                                if (IfExists == 1) // Insert
                                {
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.CommandText = "insert into tblfinaldrivermaster(final_driver_name,final_driver_personal_email,final_driver_company_email,personal_mobile_no,company_mobile_no,dob,driver_license_no,driver_license_state,driver_full_address,driver_ssn,job_offer_date,join_date,driver_adp_id,total_exp,traning_date_one,traning_date_two,ride_along_date,ride_along_driver_name,driver_status)values('" + dt.Rows[i]["driver_name"] + "','" + dt.Rows[i]["driver_personal_email"] + "','" + dt.Rows[i]["driver_company_email"] + "','" + dt.Rows[i]["personal_mobile_no"] + "','" + dt.Rows[i]["company_mobile_no"] + "','" + DateTime.Parse(dob) + "','" + dt.Rows[i]["driver_linese_no"] + "','" + dt.Rows[i]["driver_lincense_state"] + "','" + dt.Rows[i]["driver_full_address"] + "','" + dt.Rows[i]["driver_ssn"] + "','" + DateTime.Parse(job_offer_date) + "','" + DateTime.Parse(join_date) + "','" + dt.Rows[i]["driver_adp_id"] + "','" + dt.Rows[i]["total_exp"] + "','" + DateTime.Parse(traning_date_one) + "','" + DateTime.Parse(traning_date_two) + "','" + DateTime.Parse(ride_along_date) + "','" + dt.Rows[i]["ride_along_driver_name"] + "','" + driver_status + "')";
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                else if (IfExists == 2) // Update
                                {
                                    query = "";
                                    query = " UPDATE tblfinaldrivermaster " +
                                            "   SET " +
                                            "       final_driver_personal_email = '" + dt.Rows[i]["driver_personal_email"] + "'" +
                                            " WHERE " +
                                            "   driver_status = 'active' AND " +
                                            "   final_driver_name = '" + dt.Rows[i]["driver_name"] + "'";

                                    SqlCommand cmd = new SqlCommand();
                                    cmd.CommandText = query;
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                ViewBag.msg = "File format is not proper according to system";
                                con.Close();
                                return View();
                            }
                        }

                    }
                    ViewBag.msg = "CSV file Uploaded Successfully..";
                    return RedirectToAction("finalDriverList");
                }
            }
            else
            {
                ViewBag.msg = "Only uplaod csv file";
            }
            //}
            //else
            //    {

            //        ViewBag.msg = "Please Uplaod csv file only";
            //    }


            return View();

        }

        public JsonResult UploadProfilepic(FormCollection fc)
        {
            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);

                    _imgname = Guid.NewGuid().ToString();
                    var _comPath = Server.MapPath("~/Upload") + _imgname + _ext;
                    _imgname = "MVC_" + _imgname + _ext;

                    //  ViewBag.Msg = _comPath;
                    var path = _comPath;

                    // Saving Image in Original Mode
                    pic.SaveAs(path);

                    // resizing image

                    // end resize
                }
            }
            return Json(Convert.ToString(_imgname), JsonRequestBehavior.AllowGet);

        }

        public ActionResult finalDriverList(string id)
        {
            if (id == null)
            {
                var p = dc.tblfinaldrivermasters.Where(c => c.driver_status == "active").ToList();
                return View(p);
            }
            else
            {
                var p = dc.tblfinaldrivermasters.Where(c => c.driver_status == id).ToList();
                return View(p);
            }

        }

        [HttpGet]
        public ActionResult ViewMoreEmployee(int id)
        {

            var p = dc.tblfinaldrivermasters.Find(id);
            var imgname = "";
            try
            {

                var emp_id = dc.tbldrivers.Find(p.d_id).emp_id;
                imgname = dc.tblonbordingmasters.Find(emp_id).upload_linces_photo;

                if (p.d_id != null)
                {
                    var did = p.d_id;
                    if (did != null)
                    {
                        did = Convert.ToInt32(did);
                        var p1 = dc.tbldrivertranings.Where(w => w.d_id == did).ToList();
                        if(p1.Count > 0)
                        {
                            ViewBag.trainingType = p1.FirstOrDefault().training_type;
                            ViewBag.virtualDate = p1.FirstOrDefault().VirtualDateOne;
                        }

                    }
                }


            }
            catch
            {

            }
            ViewBag.url = imgname;

            return View(p);
        }

        [HttpPost]
        public ActionResult ViewMoreEmployee(FormCollection fc, tblfinaldrivermaster obj)
        {
            var p = dc.tblfinaldrivermasters.Find(obj.final_driver_id);
            p.final_driver_name = obj.final_driver_name;
            p.final_driver_personal_email = obj.final_driver_personal_email;
            p.final_driver_company_email = obj.final_driver_company_email;
            p.driver_license_state = obj.driver_license_state;
            p.company_mobile_no = obj.company_mobile_no;

            p.personal_mobile_no = obj.personal_mobile_no;

            dc.SaveChanges();
            return RedirectToAction("finalDriverList");



            return View();

        }

        /*  public ActionResult assigneroledisplay()
          {
              var p = from n in dc.tblassigns
                      from l in dc.tblsubadmins
                      where n.sub_admin_id == l.sub_admin_id
                      select new { n, l };
              List<Assignauth> finallist = new List<Assignauth>();
              foreach (var item in p)
              {
                  Assignauth obj = new Assignauth();
                  obj.s

              }

              return View();
          }

         */

        public string duplicatefinalemail(string toemail, string subject, string emailbody)
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
                return "no";
            }
        }

        public string sendfinalemail(string toemail, string subject, string emailbody)
        {
            try
            {
                string senderemail = "support@lastmiled.com";
                string senderpassword = "Lastmiled@2019";

                //  SmtpClient client = new SmtpClient("relay-hosting.secureserver.net", 25);
                SmtpClient client = new SmtpClient("smtp.zoho.com", 465);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(senderemail, senderpassword);
                MailMessage mailmessage = new MailMessage(senderemail, toemail, subject, emailbody);
                mailmessage.IsBodyHtml = true;
                mailmessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailmessage);

                return "Yes";
            }
            catch (Exception ex)
            {

                return ex.InnerException.Message;

            }

        }

        public ActionResult Terminate(int id)
        {
            Session["stp"] = id;

            var ss = dc.tblfinaldrivermasters.Find(id).final_driver_name;
            ViewBag.driver_name = ss;

            return View();
        }

        [HttpPost]
        public ActionResult Terminate(FormCollection fc)
        {
            tblterminate obj = new tblterminate();
            int drvier_id = int.Parse(Session["stp"].ToString());
            obj.final_driver_id = drvier_id;
            obj.termination_date = DateTime.Parse(fc["tcdb"]);
            obj.last_day_wrok = DateTime.Parse(fc["lastdd"]);
            obj.last_day_terminate = DateTime.Parse(fc["terminatedd"]);
            obj.eligibal_for_hire = fc["paymentMethod"];
            obj.isvalountry = fc["paymentMethod1"];
            obj.resion = fc["paymentMethod2"];

            var ADP = Convert.ToBoolean(fc["ADP"].Split(',')[0]);

            bool Mentor = Convert.ToBoolean(fc["Mentor"].Split(',')[0]);
            bool Zoho = Convert.ToBoolean(fc["Zoho"].Split(',')[0]);
            bool gaspin = Convert.ToBoolean(fc["gaspin"].Split(',')[0]);
            var sstp = "";
            if (ADP == true)
            {
                sstp += "APD" + ",";

            }

            if (Mentor == true)
            {
                sstp += "Mentor" + ",";

            }
            if (Zoho == true)
            {
                sstp += "Zoho" + ",";


            }
            if (gaspin == true)
            {
                sstp += "gaspin" + ",";

            }

            obj.account_deactive = sstp;
            obj.comment = fc["comment"];
            dc.tblterminates.Add(obj);
            dc.SaveChanges();
            var pp = dc.tblfinaldrivermasters.Find(drvier_id);
            pp.driver_status = "dactive";
            dc.SaveChanges();

            return RedirectToAction("finalDriverList");

        }

        public JsonResult UploadLincApp(FormCollection fromcollection)
        {
            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                var empid = int.Parse(fromcollection["empid"]);

                if (pic.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);

                    _imgname = Guid.NewGuid().ToString();
                    var _comPath = Server.MapPath("/Uploads/") + _imgname + _ext;
                    _imgname = _imgname + _ext;


                    ViewBag.Msg = _comPath;
                    var path = _comPath;
                    pic.SaveAs(path);

                    var ss = dc.tblonbordingmasters.Find(empid);
                    ss.upload_linces_photo = _imgname;
                    dc.SaveChanges();


                    // resizing image

                    // end resize
                }
            }
            return Json(Convert.ToString(_imgname), JsonRequestBehavior.AllowGet);

        }

        public JsonResult UploadLinc(FormCollection fromcollection)
        {
            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                var finaldriverid = int.Parse(fromcollection["finaldriverid"]);

                if (pic.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);

                    _imgname = Guid.NewGuid().ToString();
                    var _comPath = Server.MapPath("/Uploads/") + _imgname + _ext;
                    _imgname = _imgname + _ext;


                    ViewBag.Msg = _comPath;
                    var path = _comPath;
                    pic.SaveAs(path);

                    var ss = dc.tblfinaldrivermasters.Find(finaldriverid);
                    ss.driver_license_photo = _imgname;
                    dc.SaveChanges();



                    // resizing image

                    // end resize
                }
            }
            return Json(Convert.ToString(_imgname), JsonRequestBehavior.AllowGet);

        }

        public ActionResult Hrchecklist(int id)
        {

            var d = dc.tblridesetups.FirstOrDefault(c => c.d_id == id) != null;

            if (d)
            {
                var ss = dc.tblridesetups.Where(c => c.d_id == id).FirstOrDefault();
                ViewBag.d_name = dc.tbldrivers.Find(id).d_name;
                return View(ss);
            }
            else
            {
                tblridesetup obj = new tblridesetup();
                obj.d_id = id;
                ViewBag.d_name = dc.tbldrivers.Find(id).d_name;

                return View(obj);
            }





        }

        public void setupfinaldriver(int id)
        {

            var pp = dc.tbldrivertranings.Where(c => c.d_id == id).FirstOrDefault();
            pp.status = "done";
            dc.SaveChanges();
            var ss = dc.tbldrivers.Find(id);
            ss.onbordingstatus = "done";
            dc.SaveChanges();

            // setup final entry details;
            var details = dc.tbldrivers.Find(id);
            var testing = dc.tbldrivertests.Where(c => c.d_id == id).FirstOrDefault();
            var tbldrier = dc.tbldrivertranings.Where(c => c.d_id == id).FirstOrDefault();
            tblfinaldrivermaster finalobjmaster = new tblfinaldrivermaster();
            finalobjmaster.final_driver_name = details.d_name;
            finalobjmaster.final_driver_personal_email = details.d_email;
            finalobjmaster.final_driver_company_email = testing.company_email;
            finalobjmaster.personal_mobile_no = details.d_phone;
            finalobjmaster.dob = details.d_dob;
            finalobjmaster.driver_license_no = details.d_license_number;
            finalobjmaster.driver_license_state = details.d_license_state;
            finalobjmaster.driver_full_address = details.d_address;
            finalobjmaster.driver_ssn = details.d_ssn;
            finalobjmaster.driver_status = "active";
            finalobjmaster.ride_along_driver_name = tbldrier.ridealongpersonname;
            finalobjmaster.ride_along_date = tbldrier.ridealongdate;
            finalobjmaster.traning_date_one = tbldrier.inclassdateone;
            finalobjmaster.traning_date_two = tbldrier.inclassdatetwo;
            finalobjmaster.d_id = id;

            var ss12 = dc.tbldrivers.Find(id).emp_id;

            var driver_linc_photo = dc.tblonbordingmasters.Find(ss12).upload_linces_photo;

            finalobjmaster.driver_license_photo = driver_linc_photo;

            dc.tblfinaldrivermasters.Add(finalobjmaster);

            dc.SaveChanges();



        }

        public bool checkUpdatedata(int id)
        {

            var ss = dc.tblridesetups.Where(c => c.d_id == id).FirstOrDefault();

            if (ss.welcome_mail == "Yes" && ss.i9_document_uploded == "Yes" && ss.imp_company_contact == "Yes" && ss.setup_menter == "Yes" && ss.gas_pin_menter_activation_code == "Yes" && ss.place_zoho_email_group == "Yes" && ss.download_i9_document == "Yes" && ss.setup_adp == "Yes" && ss.health_ins_email == "Yes" && ss.missing_puch_email == "Yes" && ss.send_adp_time_off_ins == "Yes")
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        [HttpPost]
        public ActionResult Hrchecklist(tblridesetup obj)
        {
            var ss = dc.tblridesetups.FirstOrDefault(c => c.d_id == obj.d_id) != null;
            if (ss)
            {
                var stp = dc.tblridesetups.Where(c => c.d_id == obj.d_id).FirstOrDefault();
                stp.welcome_mail = obj.welcome_mail;
                stp.i9_document_uploded = obj.i9_document_uploded;
                stp.imp_company_contact = obj.imp_company_contact;
                // stp.sent_ever_sign = obj.sent_ever_sign;
                stp.setup_menter = obj.setup_menter;
                stp.gas_pin_menter_activation_code = obj.gas_pin_menter_activation_code;
                stp.place_zoho_email_group = obj.place_zoho_email_group;
                stp.download_i9_document = obj.download_i9_document;
                //  stp.download_ever_sign_document = obj.download_ever_sign_document;
                //  stp.imp_document = obj.imp_document;
                /// stp.w4 = obj.w4;
                stp.setup_adp = obj.setup_adp;
                // stp.place_in_pto_policy = obj.place_in_pto_policy;
                stp.health_ins_email = obj.health_ins_email;
                stp.missing_puch_email = obj.missing_puch_email;
                stp.send_adp_time_off_ins = obj.send_adp_time_off_ins;

                dc.SaveChanges();
                var status = checkUpdatedata(int.Parse(obj.d_id.ToString())); ;

                if (status == true)
                {
                    setupfinaldriver(int.Parse(obj.d_id.ToString()));

                }


            }
            else
            {
                dc.tblridesetups.Add(obj);
                dc.SaveChanges();

                var status = checkUpdatedata(int.Parse(obj.d_id.ToString())); ;

                if (status == true)
                {
                    setupfinaldriver(int.Parse(obj.d_id.ToString()));

                }

            }

            return RedirectToAction("ScheduleTraningList");

        }

        [HttpPost]
        public ActionResult applicationdata(FormCollection fc, HttpPostedFileBase linc_file)
        {

            var emp_id = int.Parse(fc["emp_id1"]);

            if (fc["generatePDF"] == null)
            {

                //var S = fc["SourceOfInquiry"];
                //var SOI = dc.tblsources.Where(s => s.source_name == S).ToList();
                var obj = dc.tblonbordingmasters.Find(emp_id);

                //if (SOI.Count == 0)
                //{
                //    tblsource objtblsource = new tblsource();
                //    objtblsource.source_name = fc["SourceOfInquiry"];

                //    dc.tblsources.Add(objtblsource);
                //    dc.SaveChanges();

                //    var SOI_id = dc.tblsources.Where(e => e.source_name == S).ToList();
                //    obj.source_id = Convert.ToInt32(SOI_id.FirstOrDefault().source_id);
                //}

                ////var obj = dc.tblonbordingmasters.Find(emp_id);
                ////obj.Entry_status = "ApplicationSent";
                ////obj.emp_inteview_status = "ApplicationSent";
                //obj.emp_name = fc["fname"];
                //obj.emp_phone = fc["mobile"];
                //obj.emp_personal_email = fc["email"];
                //// obj.degination_name = fc["dob"];
                ////obj.emp_dob = DateTime.Parse(fc["dob"]);
                //obj.emp_add = fc["addr"];
                //obj.city = fc["city"];
                //obj.state = fc["statetemp"];
                //obj.zip = fc["zip"];
                //obj.are_you_us_city = fc["iscitizenus"];
                ////obj.are_you_legal_us_city = fc["islegallyallowed"];
                ////obj.drug_test = fc["submitdrugtest"];SS
                ////obj.Convicted_felony = fc["isconvicted"];
                ////obj.Convicted_felony_explain = fc["convictiondetails"];

                //obj.work_sat = fc["availablesaturday"];
                //obj.work_sat_explain = fc["saturdaydetails"];
                //obj.work_sun = fc["availablesunday"];
                //obj.work_sun_explain = fc["sundaydetails"];

                ////obj.ticket_point = fc["isdrivingticket"];
                ////obj.count = fc["ticketcount"];
                ////obj.driver_linc = fc["dl"];
                ////obj.applied_pos = fc["position"];
                //obj.available_startdate = DateTime.Parse(fc["startdate"]);
                //obj.desired_Pay = fc["desiredpay"];
                //obj.employment_desired = fc["employmentdesired"];
                //obj.school_name1 = fc["schoolname1"];
                //obj.school_name2 = fc["schoolname2"];
                //obj.location1 = fc["location1"];
                //obj.location2 = fc["location2"];
                //obj.year_attended1 = fc["yearsattended1"];
                //obj.year_attended2 = fc["yearsattended2"];
                //obj.major1 = fc["major1"];
                //obj.major2 = fc["major2"];
                //obj.employer1 = fc["employer1"];
                //obj.employer2 = fc["employer2"];
                //obj.jobtitle1 = fc["jobtitle1"];
                //obj.jobtitle2 = fc["jobtitle2"];
                //obj.state1 = fc["state1"];
                //obj.state2 = fc["state2"];
                //obj.work_phone1 = fc["workphone1"];
                //obj.work_phone2 = fc["workphone2"];
                //obj.zip1 = fc["zip1"];
                //obj.zip2 = fc["zip2"];
                //obj.address1 = fc["address1"];
                //obj.address2 = fc["address2"];
                //obj.remarks1 = fc["remak1"];
                //obj.remark2 = fc["remak2"];

                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);



                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);



                //if (fc["IsLeagalyAuthorized"].ToString().Trim() != "")
                //{
                //    string[] s = fc["IsLeagalyAuthorized"].Split(',');
                //    obj.IsLeagalyAuthorized = s[1].ToString();
                //}
                //else
                //{
                //    obj.IsLeagalyAuthorized = fc["IsLeagalyAuthorized"];
                //}

                //if (fc["IsSponShipRequired"].ToString().Trim() != "")
                //{
                //    string[] s = fc["IsSponShipRequired"].Split(',');
                //    obj.IsSponShipRequired = s[1].ToString();
                //}
                //else
                //{
                //    obj.IsSponShipRequired = fc["IsSponShipRequired"];
                //}

                //obj.WorkPerformed1 = fc["WorkPerformed1"];
                //obj.WorkPerformed2 = fc["WorkPerformed2"];
                //obj.employer3 = fc["employer3"];
                //obj.jobtitle3 = fc["jobtitle3"];
                //obj.work_phone3 = fc["workphone3"];
                //obj.starting_pay_rate3 = fc["startpayingrate3"];
                //obj.ending_pay_rate3 = fc["endpayrate3"];
                //obj.address3 = fc["address3"];
                //obj.city3 = fc["city3"];
                //obj.state3 = fc["state3"];
                //obj.zip3 = fc["zip3"];
                //obj.remarks3 = fc["remak3"];
                //obj.WorkPerformed3 = fc["WorkPerformed3"];
                //obj.degree_rec1 = fc["degreereceived1"];
                //obj.degree_rec2 = fc["degreereceived2"];

                ////obj.FinalChk1 = fc["FinalChk1"];
                ////obj.FinalChk2 = fc["FinalChk2"];
                ////obj.FinalChk3 = fc["FinalChk3"];

                //obj.empstartdate1 = fc["empstartdate1"];
                //obj.empstartdate2 = fc["empstartdate2"];
                //obj.empstartdate3 = fc["empstartdate3"];

                //obj.empenddate1 = fc["empenddate1"];
                //obj.empenddate2 = fc["empenddate2"];
                //obj.empenddate3 = fc["empenddate3"];


                if (linc_file != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filePath = path + Path.GetFileName(linc_file.FileName);
                    string fileName = Path.GetFileName(linc_file.FileName).Split('.')[0];
                    string extension = Path.GetExtension(linc_file.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        linc_file.SaveAs(filePath);
                        obj.upload_linces_photo = linc_file.FileName;
                        dc.SaveChanges();
                    }
                    else
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            int index = 1;

                            string finalFileName = fileName + "_" + index + extension;

                            while (System.IO.File.Exists(path + finalFileName))
                            {
                                finalFileName = fileName + "_" + ++index + extension;
                            }

                            string finalFilePath = path + finalFileName;

                            linc_file.SaveAs(finalFilePath);
                            obj.upload_linces_photo = finalFileName;
                            dc.SaveChanges();
                        }
                    }
                }

                var rem = fc["ApplicantRemarks"];

                if (rem.ToString().Trim() != "")
                {
                    AddRemarks(emp_id.ToString(), rem, "Application submitted list");
                }

            }
            return RedirectToAction("applicationSubmitedList");

        }

        public JsonResult UploadFile(FormCollection formCollection)
        {
            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                var finaldriverid = int.Parse(formCollection["finaldriverid"]);

                if (pic.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);

                    _imgname = Guid.NewGuid().ToString();
                    var _comPath = Server.MapPath("/Uploads/") + _imgname + _ext;
                    _imgname = _imgname + _ext;


                    ViewBag.Msg = _comPath;
                    var path = _comPath;
                    pic.SaveAs(path);

                    var ss = dc.tblfinaldrivermasters.Find(finaldriverid);
                    ss.driver_profile_pic = _imgname;
                    dc.SaveChanges();



                    // resizing image

                    // end resize
                }
            }
            return Json(Convert.ToString(_imgname), JsonRequestBehavior.AllowGet);
        }

        // Added By ViBeS on 03/04/2020 for adding No Show Functionality as per discussion and requirement
        public string ScheduledTrainingStatusUpdate(int id, string status, string notes, string PageName)
        {
            if (status == "NoShow" || status == "WithdrawApplication")
            {
                //var p = dc.tbldrivertranings.Find(id);

                // Working Code
                List<tbldrivertraning> objtbldrivertraning = new List<tbldrivertraning>();
                objtbldrivertraning = dc.tbldrivertranings.Where(e => e.d_id == id).ToList();
                var tbldriver = dc.tbldrivers.Find(id);

                if (objtbldrivertraning.Count > 0)
                {

                    var p = dc.tbldrivertranings.Find(objtbldrivertraning[0].traning_id);
                    p.status = status;
                    dc.SaveChanges();

                    var objtbldriver = dc.tbldrivers.Find(id);
                    objtbldriver.d_document_status = status;
                    dc.SaveChanges();

                    var p1 = dc.tblonbordingmasters.Find(objtbldriver.emp_id);
                    p1.Entry_status = status;
                    dc.SaveChanges();
                    AddRemarks(objtbldriver.emp_id.ToString(), status + " - " + notes, PageName);
                    return "success";


                    //var p = dc.tbldrivertranings.Find(objtbldrivertraning[0].traning_id);
                    //p.status = status;
                    //dc.SaveChanges();
                    //List<tbldriver> objtbldriver = new List<tbldriver>();
                    //objtbldriver = dc.tbldrivers.Where(e => e.d_id == p.d_id).ToList();
                    //var p1 = dc.tblonbordingmasters.Find(objtbldriver[0].emp_id);
                    //p1.Entry_status = status;
                    //dc.SaveChanges();
                    //AddRemarks(objtbldriver[0].emp_id.ToString(), notes, PageName);
                    //return "success";

                }
                else if (tbldriver != null)
                {
                    tbldriver.d_document_status = status;
                    dc.SaveChanges();

                    var p1 = dc.tblonbordingmasters.Find(tbldriver.emp_id);
                    p1.Entry_status = status;
                    dc.SaveChanges();
                    AddRemarks(tbldriver.emp_id.ToString(), status + " - " + notes, PageName);
                    return "success";


                    //var p = dc.tbldrivertranings.Find(objtbldrivertraning[0].traning_id);
                    //p.status = status;
                    //dc.SaveChanges();
                    //List<tbldriver> objtbldriver = new List<tbldriver>();
                    //objtbldriver = dc.tbldrivers.Where(e => e.d_id == p.d_id).ToList();
                    //var p1 = dc.tblonbordingmasters.Find(objtbldriver[0].emp_id);
                    //p1.Entry_status = status;
                    //dc.SaveChanges();
                    //AddRemarks(objtbldriver[0].emp_id.ToString(), notes, PageName);
                    //return "success";

                }
                else
                {
                    return "error";
                }


                //using (SqlConnection con = new SqlConnection(conString))
                //{
                //    try
                //    {
                //        SqlCommand cmd = new SqlCommand();
                //        cmd.CommandText = "prcUpdateDriverStatus";
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Connection = con;
                //        con.Open();
                //        cmd.ExecuteNonQuery();
                //        con.Close();

                //        return "success";
                //    }
                //    catch (Exception ex)
                //    {
                //        con.Close();
                //        return "Error";
                //    }

                //}

            }
            else
            {
                return "error";
            }
        }

        // Added By ViBeS on 03/04/2020 for adding No Show, Withdraw Application Functionality as per discussion and requirement
        public string UpdateDriverJobOfferStatus(int id, string status, string notes)
        {
            if (status.Trim() != "")
            {
                // Update by ViBeS on 22/04/2020 for adding the required code for two new Actions on Rejected list.
                if (status == "ApplicationSent")
                {
                    var p1 = dc.tblonbordingmasters.Find(id);

                    if (p1.upload_linces_photo == null || p1.city == null && p1.emp_add == null)
                    {
                        return "DataNotFound";
                    }


                    p1.Entry_status = status;
                    dc.SaveChanges();

                    return "success";
                }
                else if (status == "JobOffer")
                {
                    var p1 = dc.tblonbordingmasters.Find(id);

                    if (p1.emp_dob == null && p1.emp_add == null && p1.city == null)
                    {
                        return "DataNotFound";
                    }

                    p1.Entry_status = "StartOnbording";
                    dc.SaveChanges();

                    var IsInserted = dc.tbldrivers.Where(e => e.emp_id == id).ToList();

                    #region working code for insert driver

                    if (IsInserted.Count() == 0)
                    {
                        var p = dc.tblonbordingmasters.Find(id);

                        var d_name = p.emp_name;
                        var d_phone = p.emp_phone;
                        var d_email = p.emp_personal_email;
                        var d_dob = p.emp_dob;
                        var d_lin_no = p.driver_linc;
                        var d_joboffer = "Yes";
                        var emp_id = p.emp_id;
                        var user_id = int.Parse(Session["admin"].ToString());
                        var entry_date = DateTime.Now.Date;
                        var phone_verified = "yes";
                        var remarks = "job offered again.";
                        var d_state = p.state;
                        var d_address = p.emp_add;

                        tbldriver dobj = new tbldriver();
                        dobj.d_name = d_name;
                        dobj.d_phone = d_phone;
                        dobj.d_email = d_email;
                        dobj.emp_id = int.Parse(emp_id.ToString());
                        dobj.d_dob = d_dob;
                        dobj.d_license_number = d_lin_no;
                        dobj.joboffer = d_joboffer;
                        dobj.Remakes = remarks;
                        dobj.entry_date = entry_date;
                        dobj.user_id = user_id;
                        dobj.phone_no_verifiedy_status = phone_verified;
                        dobj.onbordingstatus = "Pending";
                        dobj.d_document_status = "Pending";
                        dobj.d_address = d_address;
                        dobj.d_license_state = d_state;

                        dc.tbldrivers.Add(dobj);
                        dc.SaveChanges();
                        return "success";
                    }
                    else
                    {
                        var d_id = dc.tbldrivers.SingleOrDefault(e => e.emp_id == id).d_id;
                        var p = dc.tbldrivers.Find(d_id);
                        p.joboffer = "Yes";
                        p.onbordingstatus = "Pending";
                        p.d_document_status = "Pending";

                        dc.SaveChanges();
                        return "success";
                    }

                    #endregion

                }
                else
                {
                    var p = dc.tbldrivers.Find(id);
                    if (p != null)
                    {
                        p.joboffer = "No";
                        //p.onbordingstatus = status;
                        dc.SaveChanges();

                        var p1 = dc.tblonbordingmasters.Find(p.emp_id);
                        p1.Entry_status = status;
                        dc.SaveChanges();

                        AddRemarks(p.emp_id.ToString(), status + " - " + notes, "Job Offer List");

                        //var p2 = dc.tbldrivertranings.Find(p.d_id);
                        //if (p2 != null)
                        //{
                        //    p2.status = status;
                        //    dc.SaveChanges();
                        //}

                        return "success";
                    }
                    else
                    {
                        return "error";
                    }
                }
            }
            else
            {
                return "error";
            }
        }

        // Added By ViBeS on 13/04/2020 for adding pop up to show/update the data of the driver.
        public ActionResult ApplicationSubmittedDataPopup(int id, string form)
        {
            if (form == "FinalDriverList")
            {
                var tblFinalDrivers = dc.tblfinaldrivermasters.Find(id);

                if (tblFinalDrivers.d_id == null)
                {
                    string msg = "Application submitted data not found for " + tblFinalDrivers.final_driver_name + ". \n Driver details uploaded from CSV.";
                    return RedirectToAction("DriverDataNotFound", new { message = msg });
                }

                var tblDrivers = dc.tbldrivers.Find(tblFinalDrivers.d_id);

                var tblOnBardingMaster = dc.tblonbordingmasters.Find(tblDrivers.emp_id);

                return View(tblOnBardingMaster);
            }
            else if (form == "RejectedList")
            {
                var tbl = dc.tblonbordingmasters.Find(id);
                if (tbl.upload_linces_photo == null || tbl.city == null && tbl.emp_add == null)
                {
                    string msg = "Application submitted data not found for " + tbl.emp_name + ".";
                    return RedirectToAction("DriverDataNotFound", new { message = msg });
                }
                var tblOnBardingMaster = dc.tblonbordingmasters.Find(id);
                return View(tblOnBardingMaster);
            }
            else if (form == "ApplicantList")
            {
                ViewBag.formName = "ApplicantList";
                var tbl = dc.tblonbordingmasters.Find(id);
                if (tbl.upload_linces_photo == null || tbl.city == null && tbl.emp_add == null)
                {
                    string msg = "Application submitted data not found for " + tbl.emp_name + ".";
                    return RedirectToAction("DriverDataNotFound", new { message = msg });
                }
                var tblOnBardingMaster = dc.tblonbordingmasters.Find(id);
                return View(tblOnBardingMaster);
            }
            return View();
        }

        public ActionResult DriverDataNotFound(string message)
        {
            ViewBag.DriverNotFoundmsg = message;
            return View();
        }

        [HttpGet]
        public ActionResult UpdateDriverStatusFromRejected(int id, string status)
        {
            Session["emp_id"] = id;
            Session["status"] = status;
            return View();
        }

        public string UpdateDriverStatusFromRejected()
        {
            string result = "";

            var id = Convert.ToInt32(Session["emp_id"].ToString());
            var status = Session["status"].ToString();

            var p1 = dc.tblonbordingmasters.Find(id);
            if (p1 != null)
            {
                p1.Entry_status = status;
                dc.SaveChanges();
                result = "success";
            }
            else
            {
                result = "error";
            }


            return result;
        }

        // Added By ViBeS on 14/04/2020 for adding No Show Functionality as per discussion and requirement
        public string UpdateDriverStatus(int id, string status)
        {
            if (id > 0)
            {
                var p1 = dc.tblonbordingmasters.Find(id);
                p1.Entry_status = status;
                dc.SaveChanges();

                return "success";
            }
            else
            {
                return "error";
            }
        }

        [HttpPost]
        public ActionResult ApplicationSubmittedDataPopup(FormCollection fc, string formName, HttpPostedFileBase linc_file)
        {
            var emp_id = int.Parse(fc["emp_id1"]);
            if (fc["generatePDF"] == null)
            {
                //var S = fc["SourceOfInquiry"];
                //var SOI = dc.tblsources.Where(s => s.source_name == S).ToList();

                ////var frmName = fc["formName"];

                var obj = dc.tblonbordingmasters.Find(emp_id);

                //if (SOI.Count == 0)
                //{
                //    tblsource objtblsource = new tblsource();
                //    objtblsource.source_name = fc["SourceOfInquiry"];

                //    dc.tblsources.Add(objtblsource);
                //    dc.SaveChanges();

                //    var SOI_id = dc.tblsources.Where(e => e.source_name == S).ToList();
                //    obj.source_id = Convert.ToInt32(SOI_id.FirstOrDefault().source_id);
                //}
                ////var obj = dc.tblonbordingmasters.Find(emp_id);
                //obj.emp_name = fc["fname"];
                //obj.emp_phone = fc["mobile"];
                //obj.emp_personal_email = fc["email"];
                //// obj.degination_name = fc["dob"];
                ////obj.emp_dob = DateTime.Parse(fc["dob"]);
                //obj.emp_add = fc["addr"];
                //obj.city = fc["city"];
                //obj.state = fc["statetemp"];
                //obj.zip = fc["zip"];
                //obj.are_you_us_city = fc["iscitizenus"];
                ////obj.are_you_legal_us_city = fc["islegallyallowed"];
                ////obj.drug_test = fc["submitdrugtest"];
                ////obj.Convicted_felony = fc["isconvicted"];
                ////obj.Convicted_felony_explain = fc["convictiondetails"];
                //obj.work_sat = fc["availablesaturday"];
                //obj.work_sat_explain = fc["saturdaydetails"];
                //obj.work_sun = fc["availablesunday"];
                //obj.work_sun_explain = fc["sundaydetails"];
                ////obj.ticket_point = fc["isdrivingticket"];
                ////obj.count = fc["ticketcount"];
                ////obj.driver_linc = fc["dl"];
                ////obj.applied_pos = fc["position"];
                //obj.available_startdate = DateTime.Parse(fc["startdate"]);
                //obj.desired_Pay = fc["desiredpay"];
                //obj.employment_desired = fc["employmentdesired"];
                //obj.school_name1 = fc["schoolname1"];
                //obj.school_name2 = fc["schoolname2"];
                //obj.location1 = fc["location1"];
                //obj.location2 = fc["location2"];
                //obj.year_attended1 = fc["yearsattended1"];
                //obj.year_attended2 = fc["yearsattended2"];
                //obj.major1 = fc["major1"];
                //obj.major2 = fc["major2"];
                //obj.employer1 = fc["employer1"];
                //obj.employer2 = fc["employer2"];
                //obj.jobtitle1 = fc["jobtitle1"];
                //obj.jobtitle2 = fc["jobtitle2"];
                //obj.state1 = fc["state1"];
                //obj.state2 = fc["state2"];
                //obj.work_phone1 = fc["workphone1"];
                //obj.work_phone2 = fc["workphone2"];
                //obj.zip1 = fc["zip1"];
                //obj.zip2 = fc["zip2"];
                //obj.address1 = fc["address1"];
                //obj.address2 = fc["address2"];
                //obj.remarks1 = fc["remak1"];
                //obj.remark2 = fc["remak2"];

                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);


                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);



                //obj.starting_pay_rate1 = fc["startpayingrate1"];
                //obj.starting_pay_rate2 = fc["startpayingrate2"];
                //obj.ending_pay_rate1 = fc["endpayrate1"];
                //obj.ending_pay_rate2 = fc["endpayrate2"];
                //obj.city1 = fc["city1"];
                //obj.city2 = fc["city2"];
                //obj.sig_name = fc["sig_name"];
                //obj.sig_date = DateTime.Parse(fc["sig_date"]);



                //if (fc["IsLeagalyAuthorized"].ToString().Trim() != "")
                //{
                //    string[] s = fc["IsLeagalyAuthorized"].Split(',');
                //    obj.IsLeagalyAuthorized = s[1].ToString();
                //}
                //else
                //{
                //    obj.IsLeagalyAuthorized = fc["IsLeagalyAuthorized"];
                //}

                //if (fc["IsSponShipRequired"].ToString().Trim() != "")
                //{
                //    string[] s = fc["IsSponShipRequired"].Split(',');
                //    obj.IsSponShipRequired = s[1].ToString();
                //}
                //else
                //{
                //    obj.IsSponShipRequired = fc["IsSponShipRequired"];
                //}


                //obj.WorkPerformed1 = fc["WorkPerformed1"];
                //obj.WorkPerformed2 = fc["WorkPerformed2"];
                //obj.employer3 = fc["employer3"];
                //obj.jobtitle3 = fc["jobtitle3"];
                //obj.work_phone3 = fc["workphone3"];
                //obj.starting_pay_rate3 = fc["startpayingrate3"];
                //obj.ending_pay_rate3 = fc["endpayrate3"];
                //obj.address3 = fc["address3"];
                //obj.city3 = fc["city3"];
                //obj.state3 = fc["state3"];
                //obj.zip3 = fc["zip3"];
                //obj.remarks3 = fc["remak3"];
                //obj.WorkPerformed3 = fc["WorkPerformed3"];
                //obj.degree_rec1 = fc["degreereceived1"];
                //obj.degree_rec2 = fc["degreereceived2"];

                ////obj.FinalChk1 = fc["FinalChk1"];
                ////obj.FinalChk2 = fc["FinalChk2"];
                ////obj.FinalChk3 = fc["FinalChk3"];

                //obj.empstartdate1 = fc["empstartdate1"];
                //obj.empstartdate2 = fc["empstartdate2"];
                //obj.empstartdate3 = fc["empstartdate3"];

                //obj.empenddate1 = fc["empenddate1"];
                //obj.empenddate2 = fc["empenddate2"];
                //obj.empenddate3 = fc["empenddate3"];

                if (linc_file != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filePath = path + Path.GetFileName(linc_file.FileName);
                    string fileName = Path.GetFileName(linc_file.FileName).Split('.')[0];
                    string extension = Path.GetExtension(linc_file.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        linc_file.SaveAs(filePath);
                        obj.upload_linces_photo = linc_file.FileName;
                        dc.SaveChanges();
                    }
                    else
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            int index = 1;

                            string finalFileName = fileName + "_" + index + extension;

                            while (System.IO.File.Exists(path + finalFileName))
                            {
                                finalFileName = fileName + "_" + ++index + extension;
                            }

                            string finalFilePath = path + finalFileName;

                            linc_file.SaveAs(finalFilePath);
                            obj.upload_linces_photo = finalFileName;
                            dc.SaveChanges();
                        }
                    }
                }

                var rem = fc["ApplicantRemarks"];

                if (rem.ToString().Trim() != "")
                {
                    formName = formName.Contains("ScheduleIntrviewList") ? "Schedule Interview List" : "Final Driver List";
                    AddRemarks(emp_id.ToString(), rem, formName);
                }

                if (formName.Contains("ScheduleIntrviewList"))
                {
                    return RedirectToAction("ScheduleIntrviewList");
                }
            }

            if (fc["formName"].Contains("ScheduleIntrviewList/Rejected"))
            {
                return RedirectToAction("ScheduleIntrviewList", new { id = "Rejected"});
            }
            else if (fc["formName"].Contains("ScheduleIntrviewList/Pending"))
            {
                return RedirectToAction("ScheduleIntrviewList", new { id = "Pending" });
            }

            return RedirectToAction("finalDriverList");

            //if (formName.Contains("finalDriverList"))
            //{
            //    return RedirectToAction("finalDriverList");
            //}
            //else //if(formName == "JobOfferList")
            //{
            //    return RedirectToAction("JobOfferList");
            //}

        }

        // Commented on 12/06/2020 for new requirements
        //public ActionResult isADPMVR(int id)
        //{
        //    Session["chklist"] = 0;
        //    ViewBag.id = id;

        //    return View();

        //}
        //[HttpPost]
        //public ActionResult isADPMVR(FormCollection fc)
        //{
        //    var d_id = int.Parse(fc["did"].ToString());
        //    var exits = dc.tbldrivertests.FirstOrDefault(c => c.d_id == d_id) != null;
        //    if (exits)
        //    {
        //        var ss = dc.tbldrivertests.Where(c => c.d_id == d_id).FirstOrDefault();
        //        ss.isADPMVR = fc["isADPMVR"];
        //        dc.SaveChanges();
        //        updatedocumentstatus(d_id);
        //    }
        //    else
        //    {
        //        var did = int.Parse(fc["did"].ToString());
        //        var isADPMVR = fc["isADPMVR"];
        //        tbldrivertest objtest = new tbldrivertest();
        //        objtest.d_id = did;
        //        objtest.isADPMVR = isADPMVR;
        //        dc.tbldrivertests.Add(objtest);
        //        dc.SaveChanges();
        //        updatedocumentstatus(d_id);
        //    }
        //    return RedirectToAction("JobOfferList");
        //}

        public string isADPMVR(int id)
        {
            string result = "";

            var IsExistsDriver = dc.tbldrivers.Where(w => w.emp_id == id).ToList();
            if (IsExistsDriver.Count() == 0)
            {

                // Insert to tbldriver
                var p = dc.tblonbordingmasters.Find(id);

                var d_name = p.emp_name;
                var d_phone = p.emp_phone;
                var d_email = p.emp_personal_email;
                var d_dob = p.emp_dob;
                var d_lin_no = p.driver_linc;
                // not adding job offer status as yes because we dont want to move driver to JobOfferList If ADP Initiated
                //var d_joboffer = "Yes";
                var emp_id = p.emp_id;
                var user_id = int.Parse(Session["admin"].ToString());
                var entry_date = DateTime.Now.Date;
                var phonevaerifiyed = "yes";
                var remakes = "Ok";
                var d_state = p.state;
                // var d_ssn = fc["d_ssn"];
                var d_address = p.emp_add;

                tbldriver dobj = new tbldriver();
                dobj.d_name = d_name;
                dobj.d_phone = d_phone;
                dobj.d_email = d_email;
                dobj.emp_id = int.Parse(emp_id.ToString());
                dobj.d_dob = d_dob;
                dobj.d_license_number = d_lin_no;
                //dobj.joboffer = d_joboffer;
                dobj.Remakes = remakes;
                dobj.entry_date = entry_date;
                dobj.user_id = user_id;
                dobj.phone_no_verifiedy_status = phonevaerifiyed;
                dobj.onbordingstatus = "Pending";
                dobj.d_document_status = "Pending";
                dobj.d_address = d_address;
                dobj.d_license_state = d_state;

                dc.tbldrivers.Add(dobj);
                dc.SaveChanges();
                var empid = int.Parse(emp_id.ToString());

                //var p1 = dc.tblonbordingmasters.Find(empid);
                //p1.emp_inteview_status = "taken";

                //if (d_joboffer == "Yes")
                //{
                //    p.Entry_status = "StartOnbording";

                //}
                //else
                //{
                //    p1.rejection_resion = remakes;
                //    p1.Entry_status = "Rejected";

                //}

                dc.SaveChanges();

                // Update ADP status
                var p2 = dc.tbldrivers.Where(w => w.emp_id == id).ToList();
                var d_id = Convert.ToInt32(p2.FirstOrDefault().d_id.ToString());

                AddRemarks(p2.FirstOrDefault().emp_id.ToString(), "Initiate ADP-MVR", "Application Submitted List");

                var exits = dc.tbldrivertests.FirstOrDefault(c => c.d_id == d_id) != null;

                if (exits)
                {
                    var ss = dc.tbldrivertests.Where(c => c.d_id == d_id).FirstOrDefault();
                    ss.isADPMVR = "Yes";
                    dc.SaveChanges();
                    //updatedocumentstatus(d_id);

                }
                else
                {
                    tbldrivertest objtest = new tbldrivertest();
                    objtest.d_id = d_id;
                    objtest.isADPMVR = "Yes";

                    dc.tbldrivertests.Add(objtest);
                    dc.SaveChanges();

                    //updatedocumentstatus(d_id);

                }
            }
            result = "success";
            return result;



        }

        public ActionResult Linkmodel()
        {
            string msg = Session["LinkModel"].ToString();
            if (msg.Contains("+"))
            {
                string[] s = msg.Split('+');
                ViewBag.msg = s[0].ToString();
                ViewBag.Link = s[1].ToString();
            }
            else
            {
                ViewBag.msg = "Email report";
                ViewBag.Link = Session["LinkModel"].ToString();
            }
            return View();
        }

        [HttpGet]
        public JsonResult CheckDuplicateDrivers(string DAname, string DAnumber)
        {
            var p = dc.tblonbordingmasters.Where(w => w.emp_name == DAname || w.emp_phone == DAnumber).ToList();
            return Json(p, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDuplicateDA(string DAname, string DAstatus, int emp_id, string emp_number)
        {
            ViewBag.DAname = DAname;
            ViewBag.DAstatus = DAstatus;
            ViewBag.emp_id = emp_id;
            ViewBag.emp_number = emp_number;


            return View();
        }

        [HttpGet]
        public ActionResult AddNotesPopup(int emp_id, string formName)
        {
            if (formName == "FinalDriverList")
            {
                if (emp_id > 0)
                {
                    var p = dc.tbldrivers.Find(emp_id);
                    ViewBag.emp_id = p.emp_id;
                    ViewBag.ApplicantName = p.d_name;
                    Session["emp_id"] = p.emp_id;
                    Session["formName"] = "";
                    Session["formName"] = formName;
                    var DriverRemarksData = dc.tblDriverRemarks.Where(w => w.EmpID == p.emp_id).OrderByDescending(o => o.ID).ToList();
                    return View(DriverRemarksData);
                }
                else
                {
                    string msg = "Application submitted data not found.\nDriver details uploaded from CSV.";
                    return RedirectToAction("DriverDataNotFound", new { message = msg });
                }
            }
            else
            {
                var p = dc.tblonbordingmasters.Find(emp_id);
                ViewBag.emp_id = emp_id;
                ViewBag.ApplicantName = p.emp_name;
                ViewBag.Entry_Remark = p.Entry_Remark;
                Session["emp_id"] = emp_id;
                Session["formName"] = "";
                Session["formName"] = formName;
                var DriverRemarksData = dc.tblDriverRemarks.Where(w => w.EmpID == emp_id).OrderByDescending(o => o.ID).ToList();
                return View(DriverRemarksData);
            }
        }

        [HttpPost]
        public ActionResult AddNotesPopup(FormCollection fc)
        {
            var remark = fc["Entry_Remark"];
            var emp_id = Convert.ToInt32(Session["emp_id"].ToString());
            var PageName = Session["formName"];
            //var p = dc.tblonbordingmasters.Find(emp_id);
            //p.Entry_Remark = remark;
            //dc.SaveChanges();

            AddRemarks(emp_id.ToString(), remark, PageName.ToString());

            if (Session["formName"].ToString() == "ScheduleTrainingList")
            {
                Session["formName"] = "";
                return RedirectToAction("ScheduleTraningList");
            }
            else if (Session["formName"].ToString() == "Job Offer List")
            {
                Session["formName"] = "";
                return RedirectToAction("JobOfferList");
            }
            else if (Session["formName"].ToString() == "Scheduled Interview List")
            {
                Session["formName"] = "";
                return RedirectToAction("ScheduleIntrviewList");
            }
            if (Session["formName"].ToString() == "FinalDriverList")
            {
                Session["formName"] = "";
                return RedirectToAction("finalDriverList");
            }
            else
            {
                Session["formName"] = "";
                return RedirectToAction("applicationSubmitedList");
            }
        }

        [HttpGet]
        public ActionResult SetSubADPMVR(int d_id)
        {
            ViewBag.id = d_id;


            var p = dc.tbldrivertests.Where(c => c.d_id == d_id).ToList();
            if (p.Count > 0)
            {
                var s = p.FirstOrDefault().ADPMVRstatus1;

                if (s != null)
                {
                    ViewBag.status = s;
                }
            }


            return View();
        }

        [HttpPost]
        public ActionResult SetSubADPMVR(FormCollection fc)
        {
            var d_id = Convert.ToInt32(fc["did"].ToString());
            var status = fc["isADPMVR"];

            var p = dc.tbldrivertests.Where(w => w.d_id == d_id);
            p.FirstOrDefault().ADPMVRstatus1 = status;
            dc.SaveChanges();

            if (status == "Waived")
            {
                var p1 = dc.tbldrivers.Find(d_id);
                p1.joboffer = "Yes";
                dc.SaveChanges();

                var p2 = dc.tblonbordingmasters.Find(p1.emp_id);
                p2.Entry_status = "StartOnbording";
                p2.emp_inteview_status = "taken";
                dc.SaveChanges();
            }

            var empID = dc.tbldrivers.Find(d_id).emp_id;
            AddRemarks(empID.ToString(), "ADP-MVR status changed to - " + status, "Application Submitted List");

            return RedirectToAction("applicationSubmitedList");
        }

        [HttpGet]
        public ActionResult SetSubADPMVR2(int d_id)
        {
            ViewBag.id = d_id;

            var p = dc.tbldrivertests.Where(c => c.d_id == d_id).ToList();
            if (p.Count > 0)
            {
                var s = p.FirstOrDefault().ADPMVRstatus2;

                if (s != null)
                {
                    ViewBag.status = s;
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult SetSubADPMVR2(FormCollection fc)
        {
            var d_id = Convert.ToInt32(fc["did"].ToString());
            var status = fc["isADPMVR"];

            var p = dc.tbldrivertests.Where(w => w.d_id == d_id);
            p.FirstOrDefault().ADPMVRstatus2 = status;
            dc.SaveChanges();

            if (status == "Proceed")
            {
                var p1 = dc.tbldrivers.Find(d_id);
                p1.joboffer = "Yes";
                dc.SaveChanges();

                var p2 = dc.tblonbordingmasters.Find(p1.emp_id);
                p2.Entry_status = "StartOnbording";
                p2.emp_inteview_status = "taken";
                dc.SaveChanges();
            }

            if (status == "Adverse-Sent")
            {
                var p1 = dc.tbldrivers.Find(d_id);
                p1.joboffer = "No";
                dc.SaveChanges();

                var p2 = dc.tblonbordingmasters.Find(p1.emp_id);
                p2.Entry_status = "Rejected";
                //p2.emp_inteview_status = "taken";
                dc.SaveChanges();
                //AddRemarks(p1.emp_id.ToString(), "Adverse-Sent", "Application Submitted List");
            }

            var empID = dc.tbldrivers.Find(d_id).emp_id;
            AddRemarks(empID.ToString(), "ADP-MVR status changed to - " + status, "Application Submitted List");

            return RedirectToAction("applicationSubmitedList");
        }

        [HttpGet]
        public string SendToApplicationSubmitted(int id)
        {
            string result = "";
            var status = "";
            var tblOnBordingData = dc.tblonbordingmasters.Find(id);

            if (tblOnBordingData.upload_linces_photo == null || tblOnBordingData.sig_date == null && tblOnBordingData.sig_name == null)
            {
                status = "ApplicationPending";
            }
            else
            {
                status = "ApplicationSent";
            }

            tblOnBordingData.Entry_status = status;
            tblOnBordingData.emp_inteview_status = "Yes";
            dc.SaveChanges();
            result = "success";
            AddRemarks(id.ToString(), "Applicant moved to application submitted list.", "Applicant List");
            return result;

        }

        public ActionResult isOnBordingVideo(int id)
        {
            ViewBag.id = id;

            var p = dc.tbldrivertests.Where(c => c.d_id == id).ToList();
            if (p.Count > 0)
            {
                var s = p.FirstOrDefault().isOnBordingVideo;

                if (s != null)
                {
                    ViewBag.status = s;
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult isOnBordingVideo(FormCollection fc)
        {
            var d_id = int.Parse(fc["did"].ToString());

            var exits = dc.tbldrivertests.FirstOrDefault(c => c.d_id == d_id) != null;
            if (exits)
            {
                var ss = dc.tbldrivertests.Where(c => c.d_id == d_id).FirstOrDefault();
                var status = fc["isOnBordingVideo"];
                var notes = fc["OnBordinVideoRemark"];

                ss.isOnBordingVideo = status;
                dc.SaveChanges();
                updatedocumentstatus(d_id);

                if (notes.ToString().Trim() != "")
                {
                    var p = dc.tbldrivers.Find(d_id);
                    if (p != null)
                    {
                        AddRemarks(p.emp_id.ToString(), notes, "JobOffer List - OnBoarding Video");
                    }
                }
                else
                {
                    var empID = dc.tbldrivers.Find(d_id).emp_id;
                    AddRemarks(empID.ToString(), "Onboarding video status changed to - " + status, "Joboffer List");
                }

                // Working COde
                //if (status != "NoShow" || status != "Rejected")
                //{
                //    ss.isOnBordingVideo = status;
                //    dc.SaveChanges();
                //    updatedocumentstatus(d_id);
                //}
                //else
                //{
                //    var p = dc.tbldrivers.Find(d_id);
                //    if (p != null)
                //    {
                //        p.joboffer = "No";
                //        //p.onbordingstatus = status;
                //        dc.SaveChanges();

                //        var p1 = dc.tblonbordingmasters.Find(p.emp_id);
                //        p1.Entry_status = status;
                //        dc.SaveChanges();

                //        AddRemarks(p.emp_id.ToString(), "Drug test failed.", "Job Offer List");

                //    }
                //}
            }
            else
            {
                var did = int.Parse(fc["did"].ToString());
                var isOnBordingVideo = fc["isOnBordingVideo"];

                tbldrivertest objtest = new tbldrivertest();
                objtest.d_id = did;
                objtest.isOnBordingVideo = isOnBordingVideo;

                dc.tbldrivertests.Add(objtest);
                dc.SaveChanges();

                updatedocumentstatus(d_id);

            }

            return RedirectToAction("JobOfferList");



        }

        public ActionResult AddHRcheckList(int id)
        {
            setData(id);
            return View();
        }

        [HttpPost]
        public ActionResult AddHRcheckList(FormCollection fc)
        {
            var d_id = Convert.ToInt32(fc["d_id"].ToString());
            if (fc["submitddl"] != null)
            {
                var I9_doc_one = fc["I9one"];
                var I9_one_PRC_front = fc["chkonefront"];
                var I9_one_PRC_back = fc["chkoneBack"];
                var one_Expiry_date = fc["expone"];

                var I9_doc_two = fc["I9two"];
                var I9_two_PRC_front = fc["chktwofront"];
                var I9_two_PRC_back = fc["chktwoback"];
                var two_Expiry_date = fc["exptwo"];

                var I9_doc_three = fc["I9three"];
                var I9_three_PRC_front = fc["chkthreefront"];
                var I9_three_PRC_back = fc["chkthreeback"];
                var three_Expiry_date = fc["expthree"];

                //var I9_other = fc[""];


                var IsExists = dc.tblridesetups.Where(w => w.d_id == d_id).ToList();

                if (IsExists.Count == 0)
                {
                    tblridesetup obj = new tblridesetup();
                    obj.d_id = d_id;
                    obj.user_id = Convert.ToInt32(Session["admin"]);
                    obj.entry_date = DateTime.Now.Date;

                    dc.tblridesetups.Add(obj);
                    dc.SaveChanges();
                }

                var p = dc.tblridesetups.Where(w => w.d_id == d_id).ToList();

                var rideID = p.FirstOrDefault().ride_id;

                var o = dc.tblridesetups.Find(rideID);

                if (I9_doc_one != null)
                {
                    o.I9_doc_one = I9_doc_one;
                    o.I9_one_PRC_front = I9_one_PRC_front;
                    o.I9_one_PRC_back = I9_one_PRC_back;
                    if (one_Expiry_date != "") { o.one_Expiry_date = DateTime.Parse(one_Expiry_date); }
                }

                if (I9_doc_two != null)
                {
                    o.I9_doc_two = I9_doc_two;
                    o.I9_two_PRC_front = I9_two_PRC_front;
                    o.I9_two_PRC_back = I9_two_PRC_back;
                    if (two_Expiry_date != "") { o.two_Expiry_date = DateTime.Parse(two_Expiry_date); }
                }

                if (I9_doc_three != null)
                {
                    o.I9_doc_three = I9_doc_three;
                    o.I9_three_PRC_front = I9_three_PRC_front;
                    o.I9_three_PRC_back = I9_three_PRC_back;
                    if (three_Expiry_date != "") { o.three_Expiry_date = DateTime.Parse(three_Expiry_date); }
                    //o.I9_other = I9_other;
                }

                var tblDriver = dc.tbldrivers.Find(d_id);

                AddRemarks(tblDriver.emp_id.ToString(), "I9 documents updated.", "HR-CHeck List");

                dc.SaveChanges();

            }
            else if (fc["btnCompContact"] != null)
            {
                //var dt = dc.tbldrivertests.Where(w => w.d_id == d_id).ToList();
                //var workemail = dt.FirstOrDefault().company_email;


                //var subject = "Important Contacts – Save on your PHONE";
                //var body = "Good Morning,<br><br>" +
                //           "We are very excited to have you on the LMDL team. Below are the Important Contacts that you <mark style='font-weight:600;'>SHOULD SAVE ON YOUR PHONE.</mark><br><br>" +
                //           "Manan, is the Owner, and can be reached anytime for any questions or concerns. You may not always see him, but he is always around.<br><br>" +
                //           "On your scheduled work day, please reach out to one of the dispatchers below if you are running Late OR not able to make it to work.<br><br>" +
                //           "I will be your main point of contact for any HR related questions.<br><br>" +
                //           "<b>Manan (Owner) - (732)804-4423</b><br>" +
                //           "<b>Hiral (Myself) - (732)648-4674</b><br><br>" +
                //           "<table style='border-collapse: collapse;'>" +
                //           "<tr style='border: 1px solid black;'>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>Joan Hernandez<br>(Dispatcher)</b></td>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>(732)688-6158 </b></td>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'>Sunday<br/>" +
                //           "Monday<br>Tuesday<br>Wednesday<br>Thursday</td>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'>" +
                //           "6.00 am - 3.00pm<br>6.00 am - 3.00pm<br> 6.00 am - 3.00pm<br> 6.00 am - 3.00pm<br> 6.00 am - 3.00pm </td>" +
                //           "</tr>" +
                //           "<tr style='border: 1px solid black;'>" +
                //           "<td style='border: 1px solid black;'><br></td>" +
                //           "<td style='border: 1px solid black;'><br></td>" +
                //           "<td style='border: 1px solid black;'><br></td>" +
                //           "<td style='border: 1px solid black;'><br></td>" +
                //           "</tr>" +
                //           "<tr style='border: 1px solid black;'>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>Jermaine Coombs<br>(Dispatcher)</b></td>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>(732)688-6158 </b></td>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'>Sunday<br/>" +
                //           "Wednesday<br>Thursday<br>Friday<br>Saturday</td>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'>" +
                //           "7.00am - 6.00pm<br>7.00am - 6.00pm<br>6.00am - 6.00pm<br>6.00am - 6.00pm</tr>" +
                //           "<tr style='border: solid 1px black;'>" +
                //           "<td style='border: solid 1px black;'><br></td>" +
                //           "<td style='border: solid 1px black;'><br></td>" +
                //           "<td style='border: solid 1px black;'><br></td>" +
                //           "<td style='border: solid 1px black;'><br></td>" +
                //           "</tr>" +
                //           "<tr style='border: 1px solid black;'>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>Chris Vasquez<br>(Dispatcher)</b></td>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>(732)688-6158 </b></td>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'>Sunday<br/>" +
                //           "Monday<br>Tuesday<br>Wednesday<br>Thursday</td>" +
                //           "<td align=\"center\" style='border: 1px solid black; padding:10px;'>" +
                //           "2.00pm - 9.00pm<br>2.00pm - 9.00pm<br>2.00pm - 9.00pm<br>2.00pm - 9.00pm<br>2.00pm - 9.00pm</td>" +
                //           "</tr>" +
                //           "</table><br>" +
                //           "Have a great day!<br><br>" +
                //           "Last Mile Deliveries LLC (LMDL)    <br><br>" +
                //           "<img src='~/Logimages/lmdl.png' alt='LMDL Logo' style='width:100px; height:auto;'>";
                //SendEmail_Fixed("vaibhavmawale123@gmail.com", subject, body);

            }
            else if (fc["btnI9Upd"] != null)
            {

            }
            else if (fc["btngaspin"] != null)
            {

            }
            else if (fc["btnadp"] != null)
            {

            }
            else if (fc["btnmissingpunch"] != null)
            {

            }
            else if (fc["btnInsuranceEmail"] != null)
            {
                var IsExists = dc.tblridesetups.Where(w => w.d_id == d_id).ToList();
                var tblDriver = dc.tbldrivers.Find(d_id);
                if (IsExists.Count == 0)
                {
                    tblridesetup obj = new tblridesetup();
                    obj.d_id = d_id;
                    obj.user_id = Convert.ToInt32(Session["admin"]);
                    obj.entry_date = DateTime.Now.Date;
                    dc.tblridesetups.Add(obj);
                    dc.SaveChanges();
                }
                var p = dc.tblridesetups.Where(w => w.d_id == d_id).ToList();
                var rideID = p.FirstOrDefault().ride_id;

                var tblDriverTest = dc.tbldrivertests.Where(w => w.d_id == d_id).ToList().FirstOrDefault();
                var workEmail = tblDriverTest.company_email;
                string ccMail = ConfigurationManager.AppSettings["CCEmailIDHRs"].ToString();
                var sDate = fc["enrollStartDate"];
                var eDate = fc["enrollEndDate"];

                var subject = "***Health Insurance Coverage - ACTION NEEDED***";
                var body = "Good Afternoon,<br><br>" +
                           "First, I would like to congratulate you on completing 30 days of employment with Last Mile Deliveries, LLC. This is the first milestone among many more to come.<br><br> " +
                           "On completing 30 days of employment, you also become eligible for Health Insurance Coverage through Last Mile Deliveries LLC, <mark style='background:#FFFF99; font-size:16px; font-weight:600;'>if you choose to opt in.</mark><br>" +
                           "Your Benefit Enrollment period begins, on <mark style='font-size:16px; font-weight:600;'> " + sDate + " and continues through " + eDate + " </mark>. This is your only chance to enroll in benefits, until next open enrollment (April 2021).<br><br>" +
                           "Attached is the overview of the Medical, Dental, and Vision Plan Offered. LMDL pays a portion of the enrollment cost, and you would be responsible for the balance. Based on the plan you select, and your age, I can let you know the total cost of the plan, and Employee portion that will be deducted from the payroll every week.<br><br>" +
                           "<mark style='background:#FFFF99; font-size:16px; font-weight:600;'>If you do not wish to enroll, please reply back to the email declining your coverage option.</mark><br><br>" +
                           "Should you have any questions please reach out to me directly via email or phone.<br><br>" +
                           "Thank you.<br><br>" +
                           "<b>Last Mile Deliveries LLC (LMDL)</b><br><br>" +
                           "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";

                if (SendEmail_HRchecklistInsurance(workEmail, ccMail, subject.ToString(), body))
                {
                    var o = dc.tblridesetups.Find(rideID);
                    o.health_ins_email = "Sent";
                    dc.SaveChanges();
                    AddRemarks(tblDriver.emp_id.ToString(), "Benefits email sent", "HR-CHeck List");
                }

            }
            else if (fc["btnBack"] != null)
            {
                return RedirectToAction("ScheduleTraningList");
            }


            setData(d_id);
            return View();
        }

        public void RollbackDA(int id, string status, string notes, string PageName)
        {

            var dd = dc.tbldrivers.Find(id);
            dd.d_document_status = "Pending";
            dc.SaveChanges();

            var t = dc.tbldrivertests.Where(w => w.d_id == id);
            t.FirstOrDefault().isOnBordingVideo = "Pending";
            dc.SaveChanges();

            AddRemarks(dd.emp_id.ToString(), notes, PageName);
        }

        private void setData(int id)
        {
            var DAdetails = dc.tbldrivers.Find(id);
            ViewBag.DAname = DAdetails.d_name;
            ViewBag.d_id = DAdetails.d_id;
            var p = dc.tblridesetups.Where(w => w.d_id == id).ToList();
            if (p.Count > 0)
            {
                ViewBag.chkOffEvrSent = p.FirstOrDefault().off_letter_evr_sent;
                ViewBag.off_letter_evr_signed = p.FirstOrDefault().off_letter_evr_signed;
                ViewBag.gaspin = p.FirstOrDefault().gas_pin;
                ViewBag.mentoracc = p.FirstOrDefault().setup_menter;

                ViewBag.onBoardingComplete = p.FirstOrDefault().onbording_doc_download;
                ViewBag.I9download = p.FirstOrDefault().download_i9_document;
                ViewBag.questRes = p.FirstOrDefault().quest_res_download;
                ViewBag.bgCheck = p.FirstOrDefault().bg_check_download;
                ViewBag.offLetterdown = p.FirstOrDefault().offerletter_download;

                ViewBag.hidI9one = p.FirstOrDefault().I9_doc_one;
                ViewBag.hidI9two = p.FirstOrDefault().I9_doc_two;
                ViewBag.hidI9three = p.FirstOrDefault().I9_doc_three;

                ViewBag.I9_one_PRC_front = p.FirstOrDefault().I9_one_PRC_front;
                ViewBag.I9_one_PRC_back = p.FirstOrDefault().I9_one_PRC_back;

                ViewBag.I9_two_PRC_front = p.FirstOrDefault().I9_two_PRC_front;
                ViewBag.I9_two_PRC_back = p.FirstOrDefault().I9_two_PRC_back;

                ViewBag.I9_three_PRC_front = p.FirstOrDefault().I9_three_PRC_front;
                ViewBag.I9_three_PRC_back = p.FirstOrDefault().I9_three_PRC_back;

                ViewBag.one_Expiry_date = p.FirstOrDefault().one_Expiry_date;
                ViewBag.two_Expiry_date = p.FirstOrDefault().two_Expiry_date;
                ViewBag.three_Expiry_date = p.FirstOrDefault().three_Expiry_date;

                ViewBag.imp_company_contact = p.FirstOrDefault().imp_company_contact;
                ViewBag.i9_document_uploded = p.FirstOrDefault().i9_document_uploded;
                ViewBag.gas_pin_menter_activation_code = p.FirstOrDefault().gas_pin_menter_activation_code;
                ViewBag.setup_adp = p.FirstOrDefault().setup_adp;
                ViewBag.missing_puch_email = p.FirstOrDefault().missing_puch_email;
                ViewBag.health_ins_email = p.FirstOrDefault().health_ins_email;


            }
        }

        public ActionResult AddNewEntryBulk()
        {
            ViewBag.msgbulk = Session["msgbulk"];
            Session["msgbulk"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult AddNewEntryBulk(HttpPostedFileBase fileBulk)
        {
            if (fileBulk != null)
            {
                string filePath = string.Empty;
                if (fileBulk != null)
                {
                    string path = Server.MapPath("~/BulkNewEntry/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //filePath = path + Path.GetFileName(fileBulk.FileName);
                    string fileName = Path.GetFileName(fileBulk.FileName).Split('.')[0];
                    string extension = Path.GetExtension(fileBulk.FileName);

                    string dateStamp = DateTime.Now.Date.ToString().Split(' ')[0].Replace("/", "_");
                    string timeStamp = DateTime.Now.ToLongTimeString().ToString().Replace(" ", "_").Replace(":", "_");

                    filePath = path + fileName + dateStamp + "_" + timeStamp + extension;
                    fileBulk.SaveAs(filePath);

                    //if (!System.IO.File.Exists(filePath))
                    //{
                    //    filePath = filePath + DateTime.Now.ToLongTimeString().ToString();
                    //    fileBulk.SaveAs(filePath);
                    //}
                    //else
                    //{
                    //    Session["msgbulk"] = "File already uploaded";
                    //    return RedirectToAction("AddNewEntry");
                    //}



                    //Create a DataTable.
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[10]
                    {
                        new DataColumn("driver_name", typeof(string)),
                        new DataColumn("designation", typeof(string)),
                        new DataColumn("emp_phone",typeof(string)),
                        new DataColumn("driver_personal_email",typeof(string)),
                        new DataColumn("interview_status",typeof(string)),
                        new DataColumn("interview_date",typeof(string)),
                        new DataColumn("interview_time",typeof(string)),
                        new DataColumn("is_DSP",typeof(string)),
                        new DataColumn("inquiry_source",typeof(string)),
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

                            Session["msgbulk"] = "File format is not proper according to system";
                            return RedirectToAction("AddNewEntryBulk");
                        }

                    }

                    string conString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
                    List<modelDuplucateDrivers> obj = new List<modelDuplucateDrivers>();
                    int uploadCount = 0;
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            var driver_name = dt.Rows[i]["driver_name"].ToString();
                            var designation = dt.Rows[i]["designation"].ToString();
                            var emp_phone = dt.Rows[i]["emp_phone"].ToString();
                            var driver_personal_email = dt.Rows[i]["driver_personal_email"].ToString();
                            var interview_status = dt.Rows[i]["interview_status"].ToString();
                            try
                            {
                                DateTime? interview_date = null;
                                TimeSpan? interview_time = null;

                                if (interview_status.ToLower() == "yes")
                                {
                                    interview_date = DateTime.Parse(dt.Rows[i]["interview_date"].ToString());
                                    var interview_times = dt.Rows[i]["interview_time"].ToString().Trim();

                                    var time = DateTime.Parse(interview_times);
                                    var dd = time.ToString("HH:mm");
                                    var ss = TimeSpan.Parse(dd.ToString());
                                    interview_time = TimeSpan.Parse(dd.ToString());
                                }
                                var amazon_worked = dt.Rows[i]["is_DSP"].ToString().Trim();
                                var inquiry_source = dt.Rows[i]["inquiry_source"].ToString().Trim();
                                var Entry_Remark = dt.Rows[i]["notes"].ToString().Trim();

                                int source_id = 0;

                                var SOI = dc.tblsources.Where(s => s.source_name == inquiry_source).ToList();
                                if (SOI.Count == 0)
                                {
                                    tblsource objtblsource = new tblsource();
                                    objtblsource.source_name = inquiry_source;
                                    dc.tblsources.Add(objtblsource);
                                    dc.SaveChanges();
                                    var SOI_id = dc.tblsources.Where(e => e.source_name == inquiry_source).ToList();
                                    source_id = Convert.ToInt32(SOI_id.FirstOrDefault().source_id);
                                }
                                else
                                {
                                    var SOI_id = dc.tblsources.Where(e => e.source_name == inquiry_source).ToList();
                                    source_id = Convert.ToInt32(SOI_id.FirstOrDefault().source_id);
                                }


                                var entry_date = DateTime.Now.Date;
                                int role_id = int.Parse(Session["admin"].ToString());
                                var Entry_status = interview_status == "Yes" ? "Schedule" : "Pending";
                                var amazon_description = amazon_worked == "Yes" ? Entry_Remark : " ";

                                var IsExist = dc.tblonbordingmasters.Where(w => w.emp_name == driver_name || w.emp_phone == emp_phone).ToList();
                                //var IsExistNumber = dc.tblonbordingmasters.Where(w => w.emp_phone == emp_phone).ToList();

                                if (IsExist.Count == 0)
                                {
                                    string qry = " INSERT INTO tblonbordingmaster " +
                                                 " (emp_name,degination_name,emp_phone,emp_personal_email,emp_inteview_status,emp_interview_date," +
                                                 " emp_interview_time,amazon_worked,source_id,Entry_Remark,entry_date,role_id,Entry_status,amazon_description)" +
                                                 " VALUES ('" + driver_name + "','" + designation + "','" + emp_phone + "','" + driver_personal_email + "','" + interview_status + "','" + interview_date + "'," +
                                                 "  '" + interview_time + "','" + amazon_worked + "'," + source_id + ",'" + Entry_Remark + "','" + entry_date + "','" + role_id + "','" + Entry_status + "','" + amazon_description + "')";

                                    SqlCommand cmd = new SqlCommand();
                                    cmd.CommandText = qry;
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    uploadCount++;
                                    var p = dc.tblonbordingmasters.Where(w => w.emp_name == driver_name).ToList();
                                    var emp_id = p.FirstOrDefault().emp_id;

                                    var Host = Request.Url.Host.ToString();
                                    if (interview_status == "Yes")
                                    {
                                        var applicationformLink = "Application Form link - " + Host + "/Admin/ApplicatinForm/" + emp_id;
                                        var subline = "Last Mile Deliveries LLC (LMDL)   - Interview Confirmation -  " + interview_date.Value.ToShortDateString() + " at " + interview_time;
                                        var body = "Hello " + driver_name + ",<br/><br/>" +
                                                   "Thank you for your interest in Delivery Associate position with Last Mile Deliveries LLC (LMDL)  , an Amazon Delivery Service Partner (DSP). <br/><br/>" +
                                                   "This email confirms your interview on " + interview_date.Value.ToShortDateString() + " at " + interview_time + ".<br/>" +
                                                   "Please click on below link to fill an application form and receive further instructions for the interview. <br/>" +
                                                   applicationformLink + "<br/><br/>" +
                                                   "Please contact HR @ 732-648-4674 or hr@lastmiled.com for any questions.<br/>" +
                                                   "We look forward to meeting with you.<br/><br/>" +
                                                   //"Thank you,<br/>" +
                                                   "Last Mile Deliveries LLC (LMDL)    <br/>" +
                                                   "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";
                                        //"Please do not reply to this email.<br/>" +
                                        //"It was sent from an address that cannot accept incoming messages.<br/>";
                                        string ccMail = ConfigurationManager.AppSettings["CCEmailIDHR"].ToString();
                                        SendEmail_Fixed(driver_personal_email, ccMail, subline.ToString(), body);
                                    }
                                    AddRemarks(emp_id.ToString(), Entry_Remark, "Bulk Entry");
                                    AddRemarks(emp_id.ToString(), Host + "/Admin/ApplicatinForm/" + emp_id, "Bulk Entry");
                                }
                                else
                                {
                                    obj.Add(new modelDuplucateDrivers { ApplicantName = driver_name, ApplicantNumber = emp_phone, ApplicantStatus = "Please check name/contact. We already have applicant with status - " + IsExist.FirstOrDefault().Entry_status });

                                    Session["DuplicateApplicants"] = obj;
                                }


                            }
                            catch (Exception ex)
                            {
                                Session["msgbulk"] = "File format is not proper according to system";
                                con.Close();
                                obj.Add(new modelDuplucateDrivers { ApplicantName = driver_name, ApplicantNumber = emp_phone, ApplicantStatus = "Please make sure that field value should not have special characters, values should be correct according to master CSV file." });

                                Session["DuplicateApplicants"] = obj;
                            }
                        }

                    }
                    Session["msgbulk"] = uploadCount > 0 && Session["DuplicateApplicants"] == null ? "CSV file uploaded successfully." : uploadCount == 0 && Session["DuplicateApplicants"] != null ? "CSV file not uploaded." : "CSV file uploaded successfully with some applicants..";

                    if (uploadCount == 0 && Session["DuplicateApplicants"] != null)
                    {
                        System.IO.File.Delete(filePath);
                    }

                    return RedirectToAction("AddNewEntryBulk");
                }
            }
            else
            {
                Session["msgbulk"] = "Only uplaod csv file";
            }

            return View();

        }

        public bool checkHRstatus(int id)
        {
            var o = dc.tblridesetups.Where(c => c.d_id == id).FirstOrDefault();
            if (o.off_letter_evr_sent == "Yes" && o.off_letter_evr_signed == "Yes" && o.gas_pin != null && o.setup_menter == "Yes" && o.onbording_doc_download == "Yes" && o.download_i9_document == "Yes" && o.quest_res_download == "Yes" && o.bg_check_download == "Yes" && o.offerletter_download == "Yes" && o.imp_company_contact == "Sent" && o.i9_document_uploded == "Sent" && o.gas_pin_menter_activation_code == "Sent" && o.setup_adp == "Sent" && o.missing_puch_email == "Sent" && o.health_ins_email == "Sent")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string updateHRcheck(int d_id, string ActionType, int? gaspin, string sDate, string eDate)
        {
            try
            {
                // this folder contains common files to send to applicants in their emails.
                string path = Server.MapPath("~/HRemailCommonFiles/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var IsExists = dc.tblridesetups.Where(w => w.d_id == d_id).ToList();
                var tblDriver = dc.tbldrivers.Find(d_id);
                if (IsExists.Count == 0)
                {
                    tblridesetup obj = new tblridesetup();
                    obj.d_id = d_id;
                    obj.user_id = Convert.ToInt32(Session["admin"]);
                    obj.entry_date = DateTime.Now.Date;
                    dc.tblridesetups.Add(obj);
                    dc.SaveChanges();
                }
                var p = dc.tblridesetups.Where(w => w.d_id == d_id).ToList();
                var rideID = p.FirstOrDefault().ride_id;

                var tblDriverTest = dc.tbldrivertests.Where(w => w.d_id == d_id).ToList().FirstOrDefault();
                var workEmail = tblDriverTest.company_email;
                string ccMail = ConfigurationManager.AppSettings["CCEmailIDHRs"].ToString();

                if (ActionType == "offerLetterEvrSent")
                {
                    var o = dc.tblridesetups.Find(rideID);
                    o.off_letter_evr_sent = "Yes";
                    dc.SaveChanges();

                    AddRemarks(tblDriver.emp_id.ToString(), "Offer Letter - Eversign sent", "HR-CHeck List");

                }
                else if (ActionType == "offerLetterEvrSigned")
                {
                    var o = dc.tblridesetups.Find(rideID);
                    o.off_letter_evr_signed = "Yes";
                    dc.SaveChanges();

                    AddRemarks(tblDriver.emp_id.ToString(), "Offer Letter - Eversign signed", "HR-CHeck List");

                }
                else if (ActionType == "gaspin")
                {
                    var o = dc.tblridesetups.Find(rideID);
                    o.gas_pin = gaspin;
                    dc.SaveChanges();
                    AddRemarks(tblDriver.emp_id.ToString(), "Gas pin created (" + gaspin + ")", "HR-CHeck List");

                }
                else if (ActionType == "mentoracc")
                {
                    var o = dc.tblridesetups.Find(rideID);
                    o.setup_menter = "Yes";
                    dc.SaveChanges();

                    AddRemarks(tblDriver.emp_id.ToString(), "Mentor Account Created.", "HR-CHeck List");

                }
                else if (ActionType == "onBoardingComplete")
                {
                    var o = dc.tblridesetups.Find(rideID);
                    o.onbording_doc_download = "Yes";
                    dc.SaveChanges();

                    AddRemarks(tblDriver.emp_id.ToString(), "OnBording complete/Document download.", "HR-CHeck List");

                }
                else if (ActionType == "I9download")
                {
                    var o = dc.tblridesetups.Find(rideID);
                    o.download_i9_document = "Yes";
                    dc.SaveChanges();

                    AddRemarks(tblDriver.emp_id.ToString(), "I9 download", "HR-CHeck List");

                }
                else if (ActionType == "questRes")
                {
                    var o = dc.tblridesetups.Find(rideID);
                    o.quest_res_download = "Yes";
                    dc.SaveChanges();

                    AddRemarks(tblDriver.emp_id.ToString(), "Quest result download", "HR-CHeck List");

                }
                else if (ActionType == "bgCheck")
                {
                    var o = dc.tblridesetups.Find(rideID);
                    o.bg_check_download = "Yes";
                    dc.SaveChanges();

                    AddRemarks(tblDriver.emp_id.ToString(), "Accurate background check download", "HR-CHeck List");

                }
                else if (ActionType == "offLetterdown")
                {
                    var o = dc.tblridesetups.Find(rideID);
                    o.offerletter_download = "Yes";
                    dc.SaveChanges();

                    AddRemarks(tblDriver.emp_id.ToString(), "Offer letter download", "HR-CHeck List");

                }
                else if (ActionType == "imp_company_contact")
                {
                    var subject = "Important Contacts – Save on your PHONE";
                    var body = "Good Morning,<br><br>" +
                               "We are very excited to have you on the LMDL team. Below are the Important Contacts that you <mark style='font-weight:600; font-size:16px;'>SHOULD SAVE ON YOUR PHONE.</mark><br><br>" +
                               "Manan, is the Owner, and can be reached anytime for any questions or concerns. You may not always see him, but he is always around.<br><br>" +
                               "On your scheduled work day, please reach out to one of the dispatchers below if you are running Late OR not able to make it to work.<br><br>" +
                               "I will be your main point of contact for any HR related questions.<br><br>" +
                               "<b>Manan (Owner) - (732)804-4423</b><br>" +
                               "<b>Hiral (Myself) - (732)648-4674</b><br><br>" +
                               "<table style='border-collapse: collapse;'>" +
                               "<tr style='border: 1px solid black;'>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>Joan Hernandez<br>(Dispatcher)</b></td>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>(732)688-6158 </b></td>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'>Sunday<br/>" +
                               "Monday<br>Tuesday<br>Wednesday<br>Thursday</td>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'>" +
                               "6.00 am - 3.00pm<br>6.00 am - 3.00pm<br> 6.00 am - 3.00pm<br> 6.00 am - 3.00pm<br> 6.00 am - 3.00pm </td>" +
                               "</tr>" +
                               "<tr style='border: 1px solid black;'>" +
                               "<td style='border: 1px solid black;'><br></td>" +
                               "<td style='border: 1px solid black;'><br></td>" +
                               "<td style='border: 1px solid black;'><br></td>" +
                               "<td style='border: 1px solid black;'><br></td>" +
                               "</tr>" +
                               "<tr style='border: 1px solid black;'>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>Jermaine Coombs<br>(Dispatcher)</b></td>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>(732)688-6158 </b></td>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'>" +
                               "Wednesday<br>Thursday<br>Friday<br>Saturday</td>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'>" +
                               "7.00am - 6.00pm<br>7.00am - 6.00pm<br>6.00am - 6.00pm<br>6.00am - 6.00pm</tr>" +
                               "<tr style='border: solid 1px black;'>" +
                               "<td style='border: solid 1px black;'><br></td>" +
                               "<td style='border: solid 1px black;'><br></td>" +
                               "<td style='border: solid 1px black;'><br></td>" +
                               "<td style='border: solid 1px black;'><br></td>" +
                               "</tr>" +
                               "<tr style='border: 1px solid black;'>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>Chris Vasquez<br>(Dispatcher)</b></td>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'><b>(732)688-6158 </b></td>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'>Sunday<br/>" +
                               "Monday<br>Tuesday<br>Wednesday<br>Thursday</td>" +
                               "<td align=\"center\" style='border: 1px solid black; padding:10px;'>" +
                               "2.00pm - 9.00pm<br>2.00pm - 9.00pm<br>2.00pm - 9.00pm<br>2.00pm - 9.00pm<br>2.00pm - 9.00pm</td>" +
                               "</tr>" +
                               "</table><br>" +
                               "Have a great day!<br><br>" +
                               "<b>Last Mile Deliveries LLC (LMDL)</b><br><br>" +
                               "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";

                    if (SendEmail_HRchecklist(workEmail, ccMail, subject.ToString(), body))
                    {
                        var o = dc.tblridesetups.Find(rideID);
                        o.imp_company_contact = "Sent";
                        dc.SaveChanges();
                        AddRemarks(tblDriver.emp_id.ToString(), "Company contacts email sent", "HR-CHeck List");
                    }
                    else
                    {
                        return "error";
                    }
                    //var o = dc.tblridesetups.Find(rideID);
                    //o.imp_company_contact = "Sent";
                    //dc.SaveChanges();
                    //AddRemarks(tblDriver.emp_id.ToString(), "Company contacts email sent", "HR-CHeck List");
                }
                else if (ActionType == "i9_document_uploded")
                {
                    var subject = "I9 – Document Upload";
                    var body = "Good Morning,<br><br>" +
                                "Please see below a link to submit the I9 documents. These documents are required for employment verification. 3 documents are required to submit on this form:<br><br>" +
                                "1) Valid Drivers License - Required (No Substitution)<br>" +
                                "2) Social Security Card - Required (No Exception)<br>" +
                                "3) US Birth Certificate or USA Passport, are the 2 most common for this Category. Other Common documents are:<br>" +
                                "a. EAD<br>" +
                                "b. Green Card (Front & Back)<br><br>" +
                                "If you do not have either, please look at the chart below for other acceptable documents and submit any valid document.<br><br>" +
                                "<a href='https://forms.gle/E7RmKGgQMkNDYFxi9' target='_blank'>https://forms.gle/E7RmKGgQMkNDYFxi9</a><br><br>" +
                                "Please complete these by end of day today, so we can set you up correctly and quickly.<br><br>" +
                                "Thank you.<br><br>" +
                                "<b>Last Mile Deliveries LLC (LMDL)</b><br><br>" +
                               "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";


                    string pathCommonFiles = Server.MapPath("~/HRemailCommonFiles/");
                    string attachmentPath1 = "";
                    attachmentPath1 = pathCommonFiles + "LMDL_I9_List_of_Acceptable_Documents.pdf";


                    if (SendEmail_HRchecklistI9DocGas(workEmail, ccMail, subject.ToString(), body, attachmentPath1))
                    {
                        var o = dc.tblridesetups.Find(rideID);
                        o.i9_document_uploded = "Sent";
                        dc.SaveChanges();
                        AddRemarks(tblDriver.emp_id.ToString(), "I9 upload email sent", "HR-CHeck List");
                    }
                    else
                    {
                        return "error";
                    }
                    //var o = dc.tblridesetups.Find(rideID);
                    //o.i9_document_uploded = "Sent";
                    //dc.SaveChanges();
                    //AddRemarks(tblDriver.emp_id.ToString(), "I9 upload email sent", "HR-CHeck List");

                }
                else if (ActionType == "gas_pin_menter_activation_code")
                {
                    var tb = dc.tblridesetups.Find(rideID);
                    if (tb.gas_pin != null)
                    {
                        var subject = "GAS PIN & MENTOR";
                        var body = "Good Afternoon <br><br>" +
                                   "Below is your GAS PIN. Please save this PIN and keep it secure. You will need this PIN everyday to fill up GAS.<br><br>" +
                                   "<ins style='background:#FF7F50; font-size:16px; font-weight:600;'><b> &nbsp;" + tb.gas_pin + "&nbsp; </b></ins><br><br>" +
                                   "Preferred Gas station Address:<br><br>" +
                                   "<b>600 Bond St, Elizabeth, NJ 07206</b><br><br>" +
                                   "<a href='https://goo.gl/maps/oAYDA4iNWn6XHK9P6' target='_blank'>https://goo.gl/maps/oAYDA4iNWn6XHK9P6</a><br><br>" +
                                   "Also, you should have received an email <mark style='font-size:16px; font-weight:600;'>from Mentor with an Activation Link. Please make sure you activate your account today.</mark><br><br>" +
                                   "<mark style='font-size:16px; font-weight:600;'>There are on-boarding videos on Mentor as well. These need to be watched before your First Route.</mark><br><br>" +
                                   "Below is everything you use Mentor for." +
                                   "<ul>" +
                                   "<li>Pre-trip every morning (report any damages to the van)</li>" +
                                   "<li>Post-trip every evening (report damages to the van, view your FICO score & see how well you have driven)</li>" +
                                   "<li>This also gives us access to your driving skills, and rank each driver on their safe driving habbit.</li>" +
                                   "</ul>" +
                                   "Attached is PDF document that shows you step-by-step procedure on how to use the Mentor App.<br><br>" +
                                   "Thank you!<br><br>" +
                                   "<b>Last Mile Deliveries LLC (LMDL)</b><br><br>" +
                                   "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";

                        string pathCommonFiles = Server.MapPath("~/HRemailCommonFiles/");
                        string attachmentPath1 = "";
                        attachmentPath1 = pathCommonFiles + "Amazon_DSP_Driver_Guide.pdf";

                        if (SendEmail_HRchecklistI9DocGas(workEmail, ccMail, subject.ToString(), body, attachmentPath1))
                        {
                            var o = dc.tblridesetups.Find(rideID);
                            o.gas_pin_menter_activation_code = "Sent";
                            dc.SaveChanges();
                            AddRemarks(tblDriver.emp_id.ToString(), "Gas pin/Mentor activation email sent", "HR-CHeck List");
                        }
                        else
                        {
                            return "error";
                        }
                    }
                    else
                    {
                        return "error";
                    }
                    
                }
                else if (ActionType == "setup_adp")
                {

                    var subject = "ADP - Uses and Instructions";
                    var body = "Good Afternoon,<br><br>" +
                               "You should have received an email from <mark style='font-weight:600; font-size:16px;'>ADP with Activation link, please make sure to activate ADP account asap.</mark><br><br>" +
                               "ADP Mobile Solutions <i><b>(MUST BE DOWNLOADED ON YOUR PERSONAL CELL)</b></i><br><br>" +
                               "<mark style='font-weight:600; font-size:16px;'>Once the APP is downloaded and you have logged in, We need you to complete ADP ON-BOARDING.</mark> This includes various tasks and documents, including Direct Deposit Set up.<br><br>" +
                               "SECOND,<br><br>" +
                               "<ol>" +
                               "<li><mark style='background:#00FF00; font-size:16px; font-weight:600;'>You will use ADP everyday to punch in & punch out from parking lot. (on ADP dashboard you will see the punch in/punch out option). The Punch In and Out can only be done at the Parking Lot. It is Geo Fenced, and will not work outside the Parking Lot.</mark><br><br>" +
                               "<img style='width:200px;' src =\"cid:one\" alt='img1'>" +
                               "</li><br>" +
                               "<li><mark style='background:#00FF00; font-size:16px; font-weight:600;'>Report Lunch Break on ADP using Meal Out and Meal Return.</mark><br><br>" +
                               "<img style='width:200px;' src =\"cid:two\" alt='img2'>" +
                               "</li><br>" +
                               "<li><mark style='background:#00FF00; font-weight:600; font-size:16px;'>View your Weekly schedule. This is the only place you will be able to see your schedule.</mark><br><br>" +
                               "<ul><li>(Go To - MENU ->  Myself -> Schedule) Make sure you are looking at the correct week and dates.</li></ul>" +
                               "</li><br>" +
                               "<li><mark style='background:#00FF00; font-weight:600; font-size:16px;'>Complete all ON-BOARDING tasks. This will include, Emergency contact, Tax information, Direct Deposit, I9, and Documents. This needs to be completed within the next few days.</mark><br><br>" +
                               "There are several other uses for ADP." +
                               "<ul><br>" +
                               "<li>View your Weekly Time-card</li>" +
                               "<li>View your pay stub</li>" +
                               "<li>View your PTO Balance</li>" +
                               "<li>Request Time Off Using ADP.</li>" +
                               "</ul></li>" +
                               "</ol><br><br>" +
                               "Please note all Time Off Requests, and PTO requests require management approval.<br><br>" +
                               "<b>PTO</b> - Paid Time Off. You Start Accruing your PTO from the first day of Employment. However, you are not Eligible to use the PTO until you have completed 90 days of employment.<br><br>" +
                               "For your reference, I have listed below a step-by-step procedure to request this time off.<br><br>" +
                               "<b>Step 1:</b><br>" +
                               "Once you sign in to the ADP application on your cell phone - Click on the <font color='green'><b>Burger sign</b></font> on the top left corner, circled in red Below.<br><br>" +
                               "<img style='width:200px;'src =\"cid:three\" alt='img3'><br><br>" +
                               "<b>Step 2:</b><br>" +
                               "Click on <font color='green'><b>Myself</b></font><br><br>" +
                               "<img style='width:200px;' src =\"cid:four\" alt='img4'><br><br>" +
                               "<b>Step 3:</b><br>" +
                               "Click on <font color='green'><b>Time Off.</b></font><br><br>" +
                               "<img style='width:200px;' src =\"cid:five\" alt='img5'><br><br>" +
                               "<b>Step 4:</b><br>" +
                               "The Next screen gives you the synopsis of PTO used and unused. Time off used, and scheduled. On this screen you would click on <font color='green'>CREATE REQUEST</b></font>, to create / submit a new time off request.<br><br>" +
                               "<img style='width:200px;' src =\"cid:six\" alt='img6'><br><br>" +
                               "<b>Step 5:</b><br>" +
                               "Approve by, section needs to be left <font color='green'><b>BLANK.</b></font><br><br>" +
                               "<img style='width:200px;' src =\"cid:seven\" alt='img7'><br><br>" +
                               "<b>Step 6:</b><br>" +
                               "Select the type off Request you want to Submit. If you want to use your PTO towards the dates, or regular Time Off.<br><br>" +
                               "<img style='width:200px;' src =\"cid:eight\" alt='img8'><br><br>" +
                               "<b>Step 7:</b><br>" +
                               "Next, Select the appropriate <font color='green'><b>Start and End Dates</b></font> and Click on <font color='green'><b>Create!</b></font><br><br>" +
                               "<img style='width:200px;' src =\"cid:nine\" alt='img9'><br><br>" +
                               "<b>Step 8:</b><br>" +
                               "Once you click on Create, below is the screen that will show. Here, Start Time can be left <font color='green'><b>AS IS</b></font>. We are Working with ADP to get that updated to 9.00am as Default start time. However, Daily Time, needs to be changed to <font color='green'><b>10 hours</b></font><br><br>" +
                               "<img style='width:200px;'src =\"cid:ten\" alt='img10'><br><br>" +
                               "<b>Step 9:</b><br>" +
                               "On the next screen, confirm all the information, and click on <font color='green'><b>UPDATE CHANGES</b></font>.<br><br>" +
                               "<img style='width:200px;' src =\"cid:eleven\" alt='img11'><br><br>" +
                               "<b>Step 10:</b><br>" +
                               "Once you see the below screen, it means your request was submitted to us.<br><br>" +
                               "<img style='width:200px;' src =\"cid:twelve\" alt='img12'><br><br>" +
                               "Your Request will then show PENDING, until approved under the Time Off Page (Step 4).<br><br>" +
                               "Thank you.<br><br>" +
                               "Hiral Savla<br>" +
                               "<b>Last Mile Deliveries LLC (LMDL)</b><br>" +
                               "732-648-4674<br><br>" +
                               "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";


                    if (SendEmail_HRchecklistADPInstruction(workEmail, ccMail, subject.ToString(), body))
                    {
                        var o = dc.tblridesetups.Find(rideID);
                        o.setup_adp = "Sent";
                        dc.SaveChanges();
                        AddRemarks(tblDriver.emp_id.ToString(), "ADP instructions email sent", "HR-CHeck List");
                    }
                    else
                    {
                        return "error";
                    }
                }
                else if (ActionType == "missing_puch_email")
                {
                    var subject = "Missing Punch Link";
                    var body = "Good Morning,<br><br>" +
                           "You are required to use ADP to Punch in and Punch out EVERY DAY. However, there may be times that you forget to do either.<br><br>" +
                           "For these instances ONLY, please use the below link to report your punched times.<br><br>" +
                           "<a href='http://lastmiled.com/adp-missed-punch-form/' target='_blank'><mark style='color:blue; font-size:16px; font-weight:600;'>http://lastmiled.com/adp-missed-punch-form/</mark></a><br><br>" +
                           "If you miss a morning Punch, time will be processed as 8.10am, unless a later time is reported on the Link.<br><br>" +
                           "Any missed punch needs to be submitted within 24 hours.<br><br>" +
                           "Thank you!<br><br>" +
                           "<b>Last Mile Deliveries LLC (LMDL)</b><br><br>" +
                            "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";

                    if (SendEmail_HRchecklist(workEmail, ccMail, subject.ToString(), body))
                    {
                        var o = dc.tblridesetups.Find(rideID);
                        o.missing_puch_email = "Sent";
                        dc.SaveChanges();
                        AddRemarks(tblDriver.emp_id.ToString(), "Missing punch link email sent", "HR-CHeck List");
                    }
                    else
                    {
                        return "error";
                    }
                    //var o = dc.tblridesetups.Find(rideID);
                    //o.missing_puch_email = "Sent";
                    //dc.SaveChanges();
                    //AddRemarks(tblDriver.emp_id.ToString(), "Missing punch link email sent", "HR-CHeck List");

                }
                else if (ActionType == "health_ins_email")
                {
                    //var o = dc.tblridesetups.Find(rideID);
                    //o.health_ins_email = "Sent";
                    //dc.SaveChanges();

                    //AddRemarks(tblDriver.emp_id.ToString(), "Benefits email sent", "HR-CHeck List");

                }


                var status = checkHRstatus(d_id); ;

                if (status == true)
                {
                    setupfinaldriver(d_id);

                    AddRemarks(tblDriver.emp_id.ToString(), "Added to final driver list.", "HR-CHeck List");

                }


                return "success";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }


        public ActionResult AddHRcheckListPopUp(int id)
        {
            setData(id);
            return View();
        }


        // edit interviewlist
        public ActionResult editApplicantData(int id, string page)
        {
            Session["page"] = page;
            var p = dc.tblonbordingmasters.Find(id);

            ViewBag.emp_id = id;
            ViewBag.empName = p.emp_name;
            ViewBag.empContact = p.emp_phone;
            ViewBag.empPersonalEmail = p.emp_personal_email;

            var tblDrv = dc.tbldrivers.Where(w => w.emp_id == id).ToList();
            if (tblDrv.Count() > 0)
            {
                var d_id = Convert.ToInt32(tblDrv.FirstOrDefault().d_id);
                var tblDrvTst = dc.tbldrivertests.Where(w => w.d_id == d_id && w.isemailcreated == "Yes").ToList();
                if (tblDrvTst.Count() > 0)
                {
                    ViewBag.empWorkEmail = tblDrvTst.FirstOrDefault().company_email;
                }

            }


            return View(p);

        }

        [HttpPost]
        public ActionResult editApplicantData(FormCollection fc)
        {
            var page = Session["page"].ToString();
            var emp_id = Convert.ToInt32(fc["hidEmpID"]);
            var empName = fc["empName"];
            var empContact = fc["empContact"];
            var empPersonalEmail = fc["empPersonalEmail"];
            var empWorkEmail = fc["empWorkEmail"];


            if (page == "JobOfferList")
            {
                var p = dc.tblonbordingmasters.Find(emp_id);
                p.emp_name = empName;
                p.emp_phone = empContact;
                p.emp_personal_email = empPersonalEmail;
                dc.SaveChanges();

                var tblDrv = dc.tbldrivers.Where(w => w.emp_id == emp_id).ToList();
                if (tblDrv.Count() > 0)
                {
                    tblDrv.FirstOrDefault().d_name = empName;
                    tblDrv.FirstOrDefault().d_phone = empContact;
                    tblDrv.FirstOrDefault().d_email = empPersonalEmail;
                    dc.SaveChanges();
                    var d_id = Convert.ToInt32(tblDrv.FirstOrDefault().d_id);
                    var tblDrvTst = dc.tbldrivertests.Where(w => w.d_id == d_id && w.isemailcreated == "Yes").ToList();
                    if (tblDrvTst.Count() > 0)
                    {
                        tblDrvTst.FirstOrDefault().company_email = empWorkEmail;
                        dc.SaveChanges();
                    }

                }


                return RedirectToAction("jobofferlist");
            }
            else
            {
                return RedirectToAction("jobofferlist");
            }

        }

        public ActionResult setHealthInsuranceDate (int d_id)
        {
            ViewBag.d_id = d_id;
            return View();
        }

        //[HttpPost]
        //public ActionResult setHealthInsuranceDate(FormCollection fc)
        //{
        //    //var d_id = Convert.ToInt32(fc["hidDid"]);
        //    //var IsExists = dc.tblridesetups.Where(w => w.d_id == d_id).ToList();
        //    //var tblDriver = dc.tbldrivers.Find(d_id);
        //    //if (IsExists.Count == 0)
        //    //{
        //    //    tblridesetup obj = new tblridesetup();
        //    //    obj.d_id = d_id;
        //    //    obj.user_id = Convert.ToInt32(Session["admin"]);
        //    //    obj.entry_date = DateTime.Now.Date;
        //    //    dc.tblridesetups.Add(obj);
        //    //    dc.SaveChanges();
        //    //}
        //    //var p = dc.tblridesetups.Where(w => w.d_id == d_id).ToList();
        //    //var rideID = p.FirstOrDefault().ride_id;

        //    //var tblDriverTest = dc.tbldrivertests.Where(w => w.d_id == d_id).ToList().FirstOrDefault();
        //    //var workEmail = tblDriverTest.company_email;
        //    //string ccMail = ConfigurationManager.AppSettings["CCEmailIDHRs"].ToString();
            
        //    //var subject = "***Health Insurance Coverage - ACTION NEEDED***";
        //    //var body = "Good Afternoon Rene,<br><br>" +
        //    //           "First, I would like to congratulate you on completing 30 days of employment with Last Mile Deliveries, LLC. This is the first milestone among many more to come.<br><br> " +
        //    //           "On completing 30 days of employment, you also become eligible for Health Insurance Coverage through Last Mile Deliveries LLC, <mark style='background:#FFFF99; font-size:16px; font-weight:600;'>if you choose to opt in.</mark><br>" +
        //    //           "Your Benefit Enrollment period begins, on <mark style='font-size:16px; font-weight:600;'> "+ sDate + " and continues through "+ eDate + " </mark>. This is your only chance to enroll in benefits, until next open enrollment (April 2021).<br><br>" +
        //    //           "Attached is the overview of the Medical, Dental, and Vision Plan Offered. LMDL pays a portion of the enrollment cost, and you would be responsible for the balance. Based on the plan you select, and your age, I can let you know the total cost of the plan, and Employee portion that will be deducted from the payroll every week.<br><br>" +
        //    //           "<mark style='background:#FFFF99; font-size:16px; font-weight:600;'>If you do not wish to enroll, please reply back to the email declining your coverage option.</mark><br><br>" +
        //    //           "Should you have any questions please reach out to me directly via email or phone.<br><br>" +
        //    //           "Thank you.<br><br>"+
        //    //           "<img src =\"cid:siteLogo\" width=\"70\" height=\"70\"/>";

        //    //if (SendEmail_HRchecklist(workEmail, ccMail, subject.ToString(), body))
        //    //{
        //    //    var o = dc.tblridesetups.Find(rideID);
        //    //    o.health_ins_email = "Sent";
        //    //    dc.SaveChanges();
        //    //    AddRemarks(tblDriver.emp_id.ToString(), "Benefits email sent", "HR-CHeck List");
        //    //}


        //    return RedirectToAction("AddHRcheckList", new { id = d_id });
        //}

    }


}