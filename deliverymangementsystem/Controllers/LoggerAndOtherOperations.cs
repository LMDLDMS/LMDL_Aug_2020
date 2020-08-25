using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Net;

namespace deliverymangementsystem.Controllers
{
    public class LoggerAndOtherOperations
    {

        static string date;
        static string Logformat;
        public static string DeleveredEmailsFile = "EmailDeleveryDetails";

        public static void WriteFile(string DriverName, string DriverEmail)
        {
            try
            {

                Logformat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                string filePath = DeleveredEmailsFile + "\\";
                string Year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string Day = DateTime.Now.Day.ToString();
                date = Day + "_" + month + "_" + Year;
                string extension = ".log";
                string firstPath = Path.GetFullPath(filePath);
                string FolderName = System.Web.HttpContext.Current.Server.MapPath("~/EmailDeleveryDetails/");
                if (!Directory.Exists(@"D:/" + FolderName))
                {
                    Directory.CreateDirectory(@"D:/" + FolderName);
                }
                string MainPath = "D:\\LogFiles\\" + date + extension;
                if (!File.Exists(MainPath))
                {
                    FileStream path = new FileStream(MainPath, FileMode.Create, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(path);
                    writer.WriteLine("\n");
                    writer.WriteLine("Start file Writing---------------------------------------------------------");
                    writer.WriteLine("\n");
                    writer.WriteLine("End file writing-----------------------------------------------------------");
                    writer.WriteLine("\n");
                    writer.Close();
                    path.Close();
                }
                else
                {
                    FileStream path = new FileStream(MainPath, FileMode.Append, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(path);
                    writer.WriteLine("\n");
                    writer.WriteLine("Start file Writing-----------------------------------------------------------");
                    writer.WriteLine("\n");
                    writer.WriteLine("End file writing-----------------------------------------------------------");
                    writer.WriteLine("\n");
                    writer.Close();
                    path.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}