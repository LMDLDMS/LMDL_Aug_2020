using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverymangementsystem.Models
{
    public class modelSettings
    {
        // General setting
        public string SiteTitle { get; set; }
        public string TagLine { get; set; }
        public string AdministrativeEmailID { get; set; }
        public string TimeZone { get; set; }
        public string DateFromat { get; set; }
        public string TimeFormat { get; set; }
        public string WeekStartOn { get; set; }
        public string CompLogo { get; set; }
        public string SiteIcon { get; set; }
        public string CompanyAddress { get; set; }

        // Login Setting
        public string LoginPageLogo { get; set; }
        public string LogoWidth { get; set; }
        public string LogoHeight { get; set; }
        public string LogoBottomMargin { get; set; }
        public string BackgroundImage { get; set; }
        public string Position { get; set; }
        public string ForgotPassLink { get; set; }
        public string BackToHomeLink { get; set; }

        // Social Setting
        public string WhatsApp { get; set; }
        public string FaceBook { get; set; }
        public string InstaGram { get; set; }
        public string LinkedIn { get; set; }
        public string Twitter { get; set; }
        public string Youtube { get; set; }


        // Contact Setting
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public string WorkPhone { get; set; }
        public string MobileNo { get; set; }
        public string Location { get; set; }

        // Contact Setting
        public string StreetAddress1_1 { get; set; }
        public string StreetAddress2_1 { get; set; }
        public string City_1 { get; set; }
        public string State_1 { get; set; }
        public int Zip_1 { get; set; }
        public string WorkPhone_1 { get; set; }
        public string MobileNo_1 { get; set; }
        public string Location_1 { get; set; }

        // Contact Setting
        public string StreetAddress1_2 { get; set; }
        public string StreetAddress2_2 { get; set; }
        public string City_2 { get; set; }
        public string State_2 { get; set; }
        public int Zip_2 { get; set; }
        public string WorkPhone_2 { get; set; }
        public string MobileNo_2 { get; set; }
        public string Location_2 { get; set; }


        //Role Setting

        public string RoleName { get; set; }
    }
}