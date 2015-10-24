using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ESO_Project.Controllers;
using System.Web.Hosting;

namespace ESO_Project.Logs
{
    public class Logger
    {
        public static void log(string text)
        {
            writeToFile(text);
        }

        static void writeToFile(string text)
        {
            FileInfo logFile;
            int day, month, year;
            day = DateTime.Now.Day;
            month = DateTime.Now.Month;
            year = DateTime.Now.Year;

            string fileName = "Logger_" + day + "_" + month + "_" + year;
            logFile = new FileInfo(HostingEnvironment.ApplicationPhysicalPath + @".\Logs\" + fileName);
            if (!logFile.Exists)
            {
                using(StreamWriter sw = logFile.CreateText())
                {
                    sw.WriteLine(DateTime.Now + " : " + text);
                }
            }
            else
            {
                using(StreamWriter sw = logFile.AppendText())
                {
                    sw.WriteLine(DateTime.Now + " : " + text);
                }
            }
            
        }


    }
}