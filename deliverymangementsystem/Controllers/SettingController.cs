using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using deliverymangementsystem.EDM;
using System.IO;
using deliverymangementsystem.Models;
using System.Configuration;


namespace deliverymangementsystem.Controllers
{
    public class SettingController : Controller
    {
        dsdatabaseEntities dc = new dsdatabaseEntities();
        public string conString = ConfigurationManager.ConnectionStrings["dsdatabaseEntities"].ConnectionString;


        #region ActionResults

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Settings()
        {
            modelSettings obj = new modelSettings();
            var CompanyID = 10001;
            var p = dc.tblCompanyDetailsMasters.Find(CompanyID);
            if (p != null)
            {
                obj.SiteTitle = p.SiteTitle;
                obj.TagLine = p.TagLine;
                obj.AdministrativeEmailID = p.AdministrativeEmailID;
                obj.TimeZone = p.TimeZone;
                obj.DateFromat = p.DateFromat;
                obj.TimeFormat = p.TimeFormat;
                obj.WeekStartOn = p.WeekStartOn;
                obj.CompLogo = p.CompLogo;
                obj.SiteIcon = p.SiteIcon;
                obj.CompanyAddress = p.CompanyAddress;
            }
            else
            {
                obj.SiteTitle = "";
                obj.TagLine = "";
                obj.AdministrativeEmailID = "";
                obj.TimeZone = "";
                obj.DateFromat = "";
                obj.TimeFormat = "";
                obj.WeekStartOn = "";
                obj.CompLogo = "";
                obj.SiteIcon = "";
                obj.CompanyAddress = "";
            }

            var p1 = dc.tblLoginSettingMasters.Find(CompanyID);
            if (p1 != null)
            {
                obj.LoginPageLogo = p1.LoginPageLogo;
                obj.LogoWidth = p1.LogoWidth;
                obj.LogoHeight = p1.LogoHeight;
                obj.LogoBottomMargin = p1.LogoBottomMargin;
                obj.BackgroundImage = p1.BackgroundImage;
                obj.Position = p1.Position;
                obj.ForgotPassLink = p1.ForgotPassLink;
                obj.BackToHomeLink = p1.BackToHomeLink;

            }
            else
            {
                obj.LoginPageLogo = "";
                obj.LogoWidth = "";
                obj.LogoHeight = "";
                obj.LogoBottomMargin = "";
                obj.BackgroundImage = "";
                obj.Position = "";
                obj.ForgotPassLink = "";
                obj.BackToHomeLink = "";
            }

            var p2 = dc.tblSocialMediaSettingMasters.Find(CompanyID);
            if (p2 != null)
            {
                obj.WhatsApp = p2.WhatsApp;
                obj.FaceBook = p2.FaceBook;
                obj.InstaGram = p2.InstaGram;
                obj.LinkedIn = p2.LinkedIn;
                obj.Twitter = p2.Twitter;
                obj.Twitter = p2.Twitter;
                obj.Youtube = p2.Youtube;
            }
            else
            {
                obj.WhatsApp = "";
                obj.FaceBook = "";
                obj.InstaGram = "";
                obj.LinkedIn = "";
                obj.Twitter = "";
                obj.Youtube = "";
            }

            var p3 = dc.tblContactSettingMasters.Find(CompanyID);
            if (p3 != null)
            {
                obj.StreetAddress1 = p3.StreetAddress1;
                obj.StreetAddress2 = p3.StreetAddress2;
                obj.City = p3.City;
                obj.State = p3.State;
                obj.Zip = Convert.ToInt32(p3.Zip);
                obj.WorkPhone = p3.WorkPhone;
                obj.MobileNo = p3.MobileNo;
                obj.Location = p3.Location;

                obj.StreetAddress1_1 = p3.StreetAddress1_1;
                obj.StreetAddress2_1 = p3.StreetAddress2_1;
                obj.City_1 = p3.City_1;
                obj.State_1 = p3.State_1;
                obj.Zip_1 = Convert.ToInt32(p3.Zip_1);
                obj.WorkPhone_1 = p3.WorkPhone_1;
                obj.MobileNo_1 = p3.MobileNo_1;
                obj.Location_1 = p3.Location_1;

                obj.StreetAddress1_2 = p3.StreetAddress1_2;
                obj.StreetAddress2_2 = p3.StreetAddress2_2;
                obj.City_2 = p3.City_2;
                obj.State_2 = p3.State_2;
                obj.Zip_2 = Convert.ToInt32(p3.Zip_2);
                obj.WorkPhone_2 = p3.WorkPhone_2;
                obj.MobileNo_2 = p3.MobileNo_2;
                obj.Location_2 = p3.Location_2;

            }
            else
            {
                obj.StreetAddress1 = "";
                obj.StreetAddress2 = "";
                obj.City = "";
                obj.State = "";
                obj.Zip = 0;
                obj.WorkPhone = "";
                obj.MobileNo = "";
                obj.Location = "";

                obj.StreetAddress1_1 = "";
                obj.StreetAddress2_1 = "";
                obj.City_1 = "";
                obj.State_1 = "";
                obj.Zip_1 = 0;
                obj.WorkPhone_1 = "";
                obj.MobileNo_1 = "";
                obj.Location_1 = "";

                obj.StreetAddress1_2 = "";
                obj.StreetAddress2_2 = "";
                obj.City_2 = "";
                obj.State_2 = "";
                obj.Zip_2 = 0;
                obj.WorkPhone_2 = "";
                obj.MobileNo_2 = "";
                obj.Location_2 = "";
            }

            GetTimeZone();
            GetDateTimeFormat();
            GetWeekDays();
            return View(obj);
        }


        public ActionResult AccessMaster()
        {
            modelSettings obj = new modelSettings();

            ViewBag.Role = Session["msgRole"];
            Session["msgRole"] = null;

            GetRoleName();

            return View();

        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            return View();
        }

        [HttpPost]
        public ActionResult GeneralSetup(FormCollection fc, HttpPostedFileBase CompLogo, HttpPostedFileBase SiteIcon)
        {
            var CheckIfAvailable = dc.tblCompanyDetailsMasters.ToList();

            var CompanyID = 10001;
            var SiteTitle = fc["SiteTitle"];
            var TagLine = fc["TagLine"];
            var AdministrativeEmailID = fc["AdmEmailID"];
            var TimeZone = fc["TimeZone"];
            var DateFromat = fc["DateFormat"];
            var TimeFormat = fc["TimeFormat"];
            var WeekStartOn = fc["WeekStart"];
            var CompanyAddress = fc["CompAddress"];
            var CmpLogo = "";
            var CmpLogoName = "";
            var StIcon = "";
            var StIconName = "";
            var extension = "";
            var FolderPath = Server.MapPath("~/CompanyDetails/");

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            if (CompLogo != null)
            {
                var FilePath = FolderPath + Path.GetFileName(CompLogo.FileName);
                CmpLogo = CompLogo.FileName;
                CmpLogoName = CompLogo.FileName.Split('.')[0].ToString();
                extension = "." + CompLogo.FileName.Split('.')[1].ToString();
                if (!System.IO.File.Exists(FilePath))
                {
                    CompLogo.SaveAs(FilePath);
                }
                else
                {
                    if (System.IO.File.Exists(FilePath))
                    {
                        int index = 1;
                        string finalFileName = CmpLogoName + "_" + index + extension;

                        while (System.IO.File.Exists(FolderPath + finalFileName))
                        {
                            finalFileName = CmpLogoName + "_" + ++index + extension;
                        }

                        string finalFilePath = FolderPath + finalFileName;
                        CompLogo.SaveAs(finalFilePath);
                        CmpLogo = finalFileName;
                    }
                }
            }
            else if (fc["hdnCompLogo"] == "")
            {
                CmpLogo = "";
                if (Directory.Exists(FolderPath))
                {
                    var p = dc.tblCompanyDetailsMasters.Find(10001);
                    string filePath = FolderPath + Path.GetFileName(p.CompLogo);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        p.CompLogo = "";
                        dc.SaveChanges();
                    }
                }
            }


            if (SiteIcon != null)
            {
                var FilePath = FolderPath + Path.GetFileName(SiteIcon.FileName);
                StIcon = SiteIcon.FileName;
                StIconName = SiteIcon.FileName.Split('.')[0].ToString();
                extension = "." + SiteIcon.FileName.Split('.')[1].ToString();
                if (!System.IO.File.Exists(FilePath))
                {
                    SiteIcon.SaveAs(FilePath);
                }
                else
                {
                    if (System.IO.File.Exists(FilePath))
                    {
                        int index = 1;

                        string finalFileName = StIconName + "_" + index + extension;

                        while (System.IO.File.Exists(FolderPath + finalFileName))
                        {
                            finalFileName = StIconName + "_" + ++index + extension;
                        }

                        string finalFilePath = FolderPath + finalFileName;
                        SiteIcon.SaveAs(finalFilePath);
                        StIcon = finalFileName;
                    }
                }
            }
            else if (fc["hdnSiteIcon"] == "")
            {
                StIcon = "";
                if (Directory.Exists(FolderPath))
                {
                    var p = dc.tblCompanyDetailsMasters.Find(10001);
                    string filePath = FolderPath + Path.GetFileName(p.SiteIcon);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        p.SiteIcon = "";
                        dc.SaveChanges();
                    }
                }
            }

            if (CheckIfAvailable.Count() == 0)
            {
                tblCompanyDetailsMaster obj = new tblCompanyDetailsMaster();
                obj.CompanyID = CompanyID;
                obj.SiteTitle = SiteTitle;
                obj.TagLine = TagLine;
                obj.AdministrativeEmailID = AdministrativeEmailID;
                obj.TimeZone = TimeZone;
                obj.DateFromat = DateFromat;
                obj.TimeFormat = TimeFormat;
                obj.CompLogo = CmpLogo;
                obj.SiteIcon = StIcon;
                obj.WeekStartOn = WeekStartOn;
                obj.CompanyAddress = CompanyAddress;

                dc.tblCompanyDetailsMasters.Add(obj);
                dc.SaveChanges();
            }
            else
            {
                var p = dc.tblCompanyDetailsMasters.Find(CompanyID);
                p.SiteTitle = SiteTitle;
                p.TagLine = TagLine;
                p.AdministrativeEmailID = AdministrativeEmailID;
                p.TimeZone = TimeZone;
                p.DateFromat = DateFromat;
                p.TimeFormat = TimeFormat;
                p.CompLogo = CmpLogo.Trim() == "" ? p.CompLogo : CmpLogo;
                p.SiteIcon = StIcon.Trim() == "" ? p.SiteIcon : StIcon;
                p.WeekStartOn = WeekStartOn;
                p.CompanyAddress = CompanyAddress;
                dc.SaveChanges();
            }

            GetTimeZone();
            GetDateTimeFormat();
            GetWeekDays();
            return RedirectToAction("Settings");
        }
        [HttpPost]
        public ActionResult ContactSetup(FormCollection fc)
        {
            var CompanyID = 10001;
            var StreetAddress1 = fc["StreetAddress1"];
            var StreetAddress2 = fc["StreetAddress2"];
            var City = fc["City"];
            var State = fc["State"];
            var Zip = fc["Zip"];
            var WorkPhone = fc["WorkPhone"];
            var MobileNo = fc["MobileNo"];
            var Location = fc["Location"];

            var StreetAddress1_1 = fc["StreetAddress1_1"];
            var StreetAddress2_1 = fc["StreetAddress2_1"];
            var City_1 = fc["City_1"];
            var State_1 = fc["State_1"];
            var Zip_1 = fc["Zip_1"];
            var WorkPhone_1 = fc["WorkPhone_1"];
            var MobileNo_1 = fc["MobileNo_1"];
            var Location_1 = fc["Location_1"];

            var StreetAddress1_2 = fc["StreetAddress1_2"];
            var StreetAddress2_2 = fc["StreetAddress2_2"];
            var City_2 = fc["City_2"];
            var State_2 = fc["State_2"];
            var Zip_2 = fc["Zip_2"];
            var WorkPhone_2 = fc["WorkPhone_2"];
            var MobileNo_2 = fc["MobileNo_2"];
            var Location_2 = fc["Location_2"];

            var CheckIfAvailable = dc.tblContactSettingMasters.Where(w => w.CompID == CompanyID).ToList();
            if (CheckIfAvailable.Count() == 0)
            {
                tblContactSettingMaster obj = new tblContactSettingMaster();
                obj.CompID = CompanyID;
                obj.StreetAddress1 = StreetAddress1;
                obj.StreetAddress2 = StreetAddress2;
                obj.City = City;
                obj.State = State;
                obj.Zip = Convert.ToInt32(Zip);
                obj.WorkPhone = WorkPhone;
                obj.MobileNo = MobileNo;
                obj.Location = Location;

                obj.StreetAddress1_1 = StreetAddress1_1;
                obj.StreetAddress2_1 = StreetAddress2_1;
                obj.City_1 = City_1;
                obj.State = State;
                obj.Zip_1 = Convert.ToInt32(Zip_1);
                obj.WorkPhone_1 = WorkPhone_1;
                obj.MobileNo_1 = MobileNo_1;
                obj.Location_1 = Location_1;

                obj.StreetAddress1_2 = StreetAddress1_2;
                obj.StreetAddress2_2 = StreetAddress2_2;
                obj.City_2 = City_2;
                obj.State_2 = State_2;
                obj.Zip_2 = Convert.ToInt32(Zip_2);
                obj.WorkPhone_2 = WorkPhone_2;
                obj.MobileNo_2 = MobileNo_2;
                obj.Location_2 = Location_2;

                dc.tblContactSettingMasters.Add(obj);
                dc.SaveChanges();
            }
            else
            {
                var p3 = dc.tblContactSettingMasters.Find(CompanyID);
                p3.StreetAddress1 = StreetAddress1;
                p3.StreetAddress2 = StreetAddress2;
                p3.City = City;
                p3.State = State;
                p3.Zip = Convert.ToInt32(Zip);
                p3.WorkPhone = WorkPhone;
                p3.MobileNo = MobileNo;
                p3.Location = Location;

                p3.StreetAddress1_1 = StreetAddress1_1;
                p3.StreetAddress2_1 = StreetAddress2_1;
                p3.City_1 = City_1;
                p3.State_1 = State_1;
                p3.Zip_1 = Convert.ToInt32(Zip_1);
                p3.WorkPhone_1 = WorkPhone_1;
                p3.MobileNo_1 = MobileNo_1;
                p3.Location_1 = Location_1;

                p3.StreetAddress1_2 = StreetAddress1_2;
                p3.StreetAddress2_2 = StreetAddress2_2;
                p3.City_2 = City_2;
                p3.State_2 = State_2;
                p3.Zip_2 = Convert.ToInt32(Zip_2);
                p3.WorkPhone_2 = WorkPhone_2;
                p3.MobileNo_2 = MobileNo_2;
                p3.Location_2 = Location_2;
                dc.SaveChanges();
            }
           

            //GetTimeZone();
            //GetDateTimeFormat();
            //GetWeekDays();
            return RedirectToAction("Settings");
        }



        [HttpPost]
        public ActionResult LoginSetup(FormCollection fc, HttpPostedFileBase LoginLogo, HttpPostedFileBase BgImage)
        {
            var CompanyID = 10001;
            var LogoBottomMargin = fc["LogobtmMargin"];
            var LogoWidth = fc["LogoWidth"];
            var LogoHeight = fc["LogoHeight"];
            var Position = fc["Position"];
            var ForgotPassLink = fc["ForgotPassLink"];
            var BackToHomeLink = fc["BackToHomeLink"];
            var LogLogo = "";
            var LogLogoName = "";
            var BgIcon = "";
            var BgIconName = "";
            var extension = "";
            var FolderPath = Server.MapPath("~/CompanyDetails/");

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            if (LoginLogo != null)
            {
                var FilePath = FolderPath + Path.GetFileName(LoginLogo.FileName);
                LogLogo = LoginLogo.FileName;
                LogLogoName = LoginLogo.FileName.Split('.')[0].ToString();
                extension = "." + LoginLogo.FileName.Split('.')[1].ToString();
                if (!System.IO.File.Exists(FilePath))
                {
                    LoginLogo.SaveAs(FilePath);
                }
                else
                {
                    if (System.IO.File.Exists(FilePath))
                    {
                        int index = 1;
                        string finalFileName = LogLogoName + "_" + index + extension;

                        while (System.IO.File.Exists(FolderPath + finalFileName))
                        {
                            finalFileName = LogLogoName + "_" + ++index + extension;
                        }

                        string finalFilePath = FolderPath + finalFileName;
                        LoginLogo.SaveAs(finalFilePath);
                        LogLogo = finalFileName;
                    }
                }
            }
            else if (fc["hdnLoginLogo"] == "")
            {
                LogLogo = "";
                if (Directory.Exists(FolderPath))
                {
                    var p = dc.tblLoginSettingMasters.Find(10001);
                    string filePath = FolderPath + Path.GetFileName(p.LoginPageLogo);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        p.LoginPageLogo = "";
                        dc.SaveChanges();
                    }
                }
            }


            if (BgImage != null)
            {
                var FilePath = FolderPath + Path.GetFileName(BgImage.FileName);
                BgIcon = BgImage.FileName;
                BgIconName = BgImage.FileName.Split('.')[0].ToString();
                extension = "." + BgImage.FileName.Split('.')[1].ToString();
                if (!System.IO.File.Exists(FilePath))
                {
                    BgImage.SaveAs(FilePath);
                }
                else
                {
                    if (System.IO.File.Exists(FilePath))
                    {
                        int index = 1;

                        string finalFileName = BgIconName + "_" + index + extension;

                        while (System.IO.File.Exists(FolderPath + finalFileName))
                        {
                            finalFileName = BgIconName + "_" + ++index + extension;
                        }

                        string finalFilePath = FolderPath + finalFileName;
                        BgImage.SaveAs(finalFilePath);
                        BgIcon = finalFileName;
                    }
                }
            }
            else if (fc["hdnBgImage"] == "")
            {
                BgIcon = "";
                if (Directory.Exists(FolderPath))
                {
                    var p = dc.tblLoginSettingMasters.Find(10001);
                    string filePath = FolderPath + Path.GetFileName(p.BackgroundImage);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        p.BackgroundImage = "";
                        dc.SaveChanges();
                    }
                }
            }
            var CheckIfAvailable = dc.tblLoginSettingMasters.ToList();
            if (CheckIfAvailable.Count() == 0)
            {
                tblLoginSettingMaster obj = new tblLoginSettingMaster();
                obj.CompanyID = CompanyID;
                obj.LoginPageLogo = LogLogo;
                obj.LogoWidth = LogoWidth;
                obj.LogoHeight = LogoHeight;
                obj.LogoBottomMargin = LogoBottomMargin;
                obj.BackgroundImage = BgIcon;
                obj.Position = Position;
                obj.ForgotPassLink = ForgotPassLink;
                obj.BackToHomeLink = BackToHomeLink;
                dc.tblLoginSettingMasters.Add(obj);
                dc.SaveChanges();
            }
            else
            {
                var p = dc.tblLoginSettingMasters.Find(CompanyID);
                //p.CompanyID = CompanyID;
                p.LoginPageLogo = LogLogo.Trim() == "" ? p.LoginPageLogo : LogLogo;
                p.LogoWidth = LogoWidth;
                p.LogoHeight = LogoHeight;
                p.LogoBottomMargin = LogoBottomMargin;
                p.BackgroundImage = BgIcon.Trim() == "" ? p.BackgroundImage : BgIcon;
                p.Position = Position;
                p.ForgotPassLink = ForgotPassLink;
                p.BackToHomeLink = BackToHomeLink;

                dc.SaveChanges();
            }

            //GetTimeZone();
            //GetDateTimeFormat();
            //GetWeekDays();
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public ActionResult SocialSetup(FormCollection fc)
        {
            var CompanyID = 10001;

            var whatsapp = fc["whatsapp"];
            var facebook = fc["facebook"];
            var instagram = fc["instagram"];
            var linkedin = fc["linkedin"];
            var twitter = fc["twitter"];
            var youtube = fc["youtube"];

            var CheckIfAvailable = dc.tblSocialMediaSettingMasters.ToList();
            if (CheckIfAvailable.Count() == 0)
            {
                tblSocialMediaSettingMaster obj = new tblSocialMediaSettingMaster();
                obj.CompID = CompanyID;
                obj.WhatsApp = whatsapp;
                obj.FaceBook = facebook;
                obj.InstaGram = instagram;
                obj.LinkedIn = linkedin;
                obj.Twitter = twitter;
                obj.Youtube = youtube;

                dc.tblSocialMediaSettingMasters.Add(obj);
                dc.SaveChanges();
            }
            else
            {
                var p = dc.tblSocialMediaSettingMasters.Find(CompanyID);
                p.WhatsApp = whatsapp;
                p.FaceBook = facebook;
                p.InstaGram = instagram;
                p.LinkedIn = linkedin;
                p.Twitter = twitter;
                p.Youtube = youtube;
                dc.SaveChanges();
            }

            return RedirectToAction("Settings");
        }


        [HttpPost]
        public ActionResult RoleSetting(FormCollection fc)
        {
           
            var RoleName = fc["RoleName"];
         
            var CheckIfAvailable = dc.tblRoleMasters.Where(w=> w.RoleName == RoleName).ToList();
            if (CheckIfAvailable.Count() == 0)
            {
                tblRoleMaster obj = new tblRoleMaster();
                obj.RoleName = RoleName;
                dc.tblRoleMasters.Add(obj);
                dc.SaveChanges();
                Session["msgRole"] = "Role Added Successfully";
            }
            else
            {
                Session["msgRole"] = "Role already exist.";
            }


            //GetTimeZone();
            //GetDateTimeFormat();
            //GetWeekDays();
            return RedirectToAction("AccessMaster");
        }

        #endregion

        [HttpPost]
        public ActionResult ModuleSetting(FormCollection fc)
        {
            return View();
        }


        #region UserDefinedFunctions

        public void GetTimeZone()
        {
            var tblTimeZoneMaster = dc.tblTimeZoneMasters.Where(w => w.Status == "active").ToList();
            List<SelectListItem> objTimeZone = new List<SelectListItem>();
            foreach (var TimeZone in tblTimeZoneMaster)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = TimeZone.CountryName + " ( " + TimeZone.TimeZone + " " + TimeZone.GMToffset + " ) ";
                obj.Value = TimeZone.TimeZone;
                objTimeZone.Add(obj);
            }

            ViewBag.TimeZone = objTimeZone;
        }

        public void GetDateTimeFormat()
        {
            var tblDateFormat = dc.tblDateTimeFormatMasters.Where(w => w.DateTimeFormat == "DateFormat" && w.Status == "active").ToList();
            List<SelectListItem> objDateFormat = new List<SelectListItem>();
            foreach (var DateFormat in tblDateFormat)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = DateFormat.FormatValue;
                obj.Value = DateFormat.FormatValue;
                objDateFormat.Add(obj);
            }
            ViewBag.DateFormat = objDateFormat;

            var tblTimeFormat = dc.tblDateTimeFormatMasters.Where(w => w.DateTimeFormat == "TimeFormat" && w.Status == "active").ToList();
            List<SelectListItem> objTimeFormat = new List<SelectListItem>();
            foreach (var TimeFormat in tblTimeFormat)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = TimeFormat.FormatValue;
                obj.Value = TimeFormat.FormatValue;
                objTimeFormat.Add(obj);
            }
            ViewBag.TimeFormat = objTimeFormat;
        }

        public void GetWeekDays()
        {
            var tblWeekDays = dc.tblWeekDaysMasters.Where(w => w.Status == "active").OrderBy(o => o.Sequence).ToList();
            List<SelectListItem> objWeekDays = new List<SelectListItem>();
            foreach (var Day in tblWeekDays)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = Day.Day;
                obj.Value = Day.ID.ToString();
                objWeekDays.Add(obj);
            }
            ViewBag.WeekDays = objWeekDays;
        }

        public void GetRoleName()
        {
            var tblRoleMaster = dc.tblRoleMasters.OrderBy(o => o.RoleName).ToList();
            List<SelectListItem> objRole = new List<SelectListItem>();
            foreach (var role in tblRoleMaster)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = role.RoleName;
                obj.Value = role.RoleID.ToString();
                objRole.Add(obj);
            }
            ViewBag.RoleName = objRole;
        }

        #endregion
    }
}