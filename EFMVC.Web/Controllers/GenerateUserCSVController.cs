using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    public class GenerateUserCSVController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {            
            string csv = "HEADER;0;1;20170331215000;\n";
            var number = 4600000000;
            for (int i = 1; i <= 10000; i++)
            {
                number = number + 1;
                var dateTime = DateTime.Now;
                var date = dateTime.Year.ToString() + dateTime.Month.ToString("d2") + dateTime.Day.ToString("d2") + dateTime.Hour.ToString("d2") + dateTime.Minute.ToString("d2") + dateTime.Second.ToString("d2");
                csv += number + ";E;" + number + "@email.com;(00-0);" + date + "\n";
            }            
            csv += "TRAILER;Z;40;20170331215000;";
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "UserCsv.csv");         
        }
    }
}