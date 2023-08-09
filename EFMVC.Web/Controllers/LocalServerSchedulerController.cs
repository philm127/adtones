using EFMVC.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    public class LocalServerSchedulerController : Controller
    {
        //EFMVCDataContex db = new EFMVCDataContex();
        // GET: SocketConnectionProvisionProcess
        public async Task<ActionResult> Index()
        {
            try
            {
                // Add the records on bucket and bucket item table on adtone server from PreMatche table 
                //await db.Database.ExecuteSqlCommandAsync("BucketProvision");

                string adtoneServerconnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                string connectionString = @"Data Source=S20494255\SQLEXPRESS2014;Initial Catalog=Arthar;User ID=sa;Password=z_nTJG@5TB";
              
                //SqlCommand comm;
                // Transfer Data to other sever sql server db
                //string destinationconnectionString = WebConfigurationManager.AppSettings["KenyaConnectionString"];
                //string destinationconnectionString = @"Data Source=S20494255\SQLEXPRESS2014;Initial Catalog=ArtharCountry;User ID=sa;Password=z_nTJG@5TB";
                string destinationconnectionString = @"Data Source=116.203.23.56;Initial Catalog=Arthar;User ID=sa;Password=!Qwerty123qaz";
                using (SqlConnection sourceConnection =
                           new SqlConnection(connectionString))
                {


                    sourceConnection.Open();

                    // Add the records on bucket and bucket item table on adtone server from PreMatche table 
                    ExecuteStoreProcedure("BucketProvision", connectionString);

                    //Get generated spent bucket data from operator server and insert records on SocketSpentBuckets table on adtone server. 
                    var spentBucketData = ExecuteSP("GetSpentBucketData", destinationconnectionString);
                    InsertRecordsOnTable("SocketSpentBuckets", spentBucketData, adtoneServerconnectionString);

                    //Get generated spent bucket item data from operator server and insert records on SocketSpentBucketItems table on adtone server.
                    var spentBucketItemData = ExecuteSP("GetSpentBucketItemData", destinationconnectionString);
                    InsertRecordsOnTable("SocketSpentBucketItems", spentBucketItemData, adtoneServerconnectionString);

                    //Delete spent recods and their reference from operator server.
                    ExecuteStoreProcedure("DeleteSpentRecords", destinationconnectionString);

                    // Change the bucket status on operator server.
                    // Status will be added with pending status(2).(First time hourly process)
                    // Pending status will be chnaged to live status(1).(Second time hourly process)
                    // Live status will be chnaged to spent status(3).(Third time hourly process)
                    ExecuteStoreProcedure("ChangeBucketStatus", destinationconnectionString);

                    //Get bucket batch data from adtone server and add this bucket batch data on operator server.
                    var bucketBatchData = ExecuteSP("GetBucketBatchData", connectionString);
                    InsertRecordsOnTable("BucketBatches", bucketBatchData, destinationconnectionString);

                    //Get bucket data from adtone server and add this bucket data on operator server.
                    var bucketData = ExecuteSP("GetBucketData", connectionString);
                    InsertRecordsOnTable("Buckets", bucketData, destinationconnectionString);

                    //Get bucket item data from adtone server and add this bucket item data on operator server.
                    var bucketItemData = ExecuteSP("GetBucketItemData", connectionString);
                    InsertRecordsOnTable("BucketItems", bucketItemData, destinationconnectionString);

                    //Add records on campaign audit table and User Profile Advert received table from spent bucket and  spent bucket item table on adtone server. Calculate the calculation for campaign profile. 
                    ExecuteStoreProcedure("ProcessBucketAuditForSocketConnection", adtoneServerconnectionString);

                    ViewBag.Suceess = "Suceess";
                    ViewBag.Time = DateTime.Now;


                    sourceConnection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Suceess = ex.Message;
            }
            return View();
        }

        public void InsertRecordsOnTable(string tableName, DataTable data, string destinationconnectionString)
        {
            using (SqlConnection destinationConnection =
                        new SqlConnection(destinationconnectionString))
            {
                destinationConnection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationconnectionString, SqlBulkCopyOptions.KeepIdentity))

                {
                    bulkCopy.DestinationTableName =
                        tableName;
                    bulkCopy.BulkCopyTimeout = 3600;
                    try
                    {
                        bulkCopy.WriteToServer(data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


            }
        }

        public void ExecuteStoreProcedure(string spname, string connctionString)
        {
            using (SqlConnection conn = new SqlConnection(connctionString))
            {
                using (SqlCommand cmd = new SqlCommand(spname, conn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public DataTable ExecuteSP(string spname, string conn)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable dt = new DataTable();
                        con.Open();
                        //cmd.ExecuteNonQuery();
                        da.Fill(dt);
                        con.Close();
                        return dt;
                    }
                }
            }
        }
    }
}