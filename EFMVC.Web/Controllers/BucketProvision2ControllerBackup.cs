
using EFMVC.Data;
using EFMVC.EF;
using EFMVC.Model;
using EFMVC.ProvisioningModel;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using EFMVC.Web.Common;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace EFMVC.Web.Controllers
{
    public class BucketProvision2Controller : Controller
    {
        
        EFMVCDataContex db = new EFMVCDataContex();

        public async Task<ActionResult> Index()
        {
            try
            {
                MediaTransfer.MediaFileTransfer();
                await db.Database.ExecuteSqlCommandAsync("CampaignUserMatchSp2");
                await db.Database.ExecuteSqlCommandAsync("BucketProvision2");
                
                var connection = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                ExecuteLinkedSP("TransferData2", connection);

                //ExecuteLinkedSP("UpdateBucketItemData2", conn);
                //ExecuteLinkedSP("ProcessBucketAudit2", conn);

                var connctionString = ConfigurationManager.AppSettings["localserverconnectionstring"];
                string file = "D:\\AdtoneLatest\\SQLFileBackup\\MySQL_Bucket_File2.sql";
                using (MySqlConnection conn = new MySqlConnection(connctionString)) // Current Server Connection
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            cmd.CommandTimeout = 18000;
                            conn.Open();
                            mb.ExportInfo.ExportTableStructure = false;
                            mb.ExportInfo.ExportProcedures = false;
                            mb.ExportInfo.ExportFunctions = false;
                            mb.ExportInfo.ExportViews = false;
                            mb.ExportInfo.TablesToBeExportedList = new List<string> {
                                    "bucket2",
                                    "bucket_batch2",
                                    "bucket_item2"
                                };
                            mb.ExportToFile(file);
                            conn.Close();
                            ViewBag.Error = "Success";
                        }

                    }
                }

                string externalServerConnString = ConfigurationManager.AppSettings["externalserverconnectionstring"];
                using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            cmd.CommandTimeout = 18000;
                            conn.Open();
                            mb.ImportFromFile(file);
                            conn.Close();
                        }
                    }
                }

                using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                {
                    mySQLEntities.uspa_delete_spent_records2();
                }

                ViewBag.Error = "Suceess";
                ViewBag.InnerMsg = "No InnerException";
                ViewBag.Time = DateTime.Now;
                ViewBag.Window = "True";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.InnerMsg = ex.InnerException.Message;
                ViewBag.ExecuteNonQuery = "No";
            }
           
            return View();
        }


        public ActionResult DeleteRecords()
        {
            try
            {

                var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                ExecuteLinkedSP("DeleteProcessRecords2", conn);
                ViewBag.Error = "Suceess";
                ViewBag.Time = DateTime.Now;

                ViewBag.Window = "True";

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.ExecuteNonQuery = "No";

                return Json("Failure", JsonRequestBehavior.AllowGet);
            }

            //return View();
        }

        public void ExecuteLinkedSP(string spname, string conn)
        {


            using (SqlConnection con = new SqlConnection(conn))
            {

                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ViewBag.ExecuteNonQuery = "Yes";
                    con.Close();
                }
            }
        }



    }
}