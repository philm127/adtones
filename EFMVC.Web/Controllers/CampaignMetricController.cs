using EFMVC.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    public class CampaignMetricController : Controller
    {
        EFMVCDataContex db = new EFMVCDataContex();
        // GET: CampaignMetric
        public ActionResult Index()
        {
            try
            {
                var connection = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                ExecuteLinkedSP("UserDashboard", connection);
                ExecuteLinkedSP("CampaignAvgBidforUserDashboard", connection);
                ExecuteLinkedSP("PlayGroupforUserDashboard", connection);
                ViewBag.Error = "Suceess";
                ViewBag.Window = "True";
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.ExecuteNonQuery = "No";
            }

           
            return View();
        }

        public void ExecuteLinkedSP(string spname, string conn)
        {

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@FirstName", txtfirstname);
                    //cmd.Parameters.AddWithValue("@LastName", txtlastname);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ViewBag.ExecuteNonQuery = "Yes";
                    con.Close();
                }
            }
        }

    }
}