using EFMVC.Data;
using EFMVC.Web.Common;
using Renci.SshNet;
using RestSharp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    public class TempController : Controller
    {
        // GET: Temp
        //public ActionResult Index()
        //{
        //    try
        //    {

        //        //Service Code| Calling number| Called number| RBT code| SPCode |Start time for playing Advertising RBT| End time for playing Advertising RBT| CallScheme|DTMF Key Pressed|
        //        string externalServerConnString = ConfigurationManager.AppSettings["externalserverconnectionstring"];
        //        var host = "188.166.171.194";
        //        var port = 22;
        //        var username = "provisioning";
        //        var password = "adtonespassword";
        //        var directoryPath = "/usr/local/arthar/adds/ImportFile";
        //        using (var client = new SftpClient(host, port, username, password))
        //        {
        //            client.Connect();

        //            if (client.IsConnected)
        //            {

        //                //var filePaths = client.ListDirectory(client.WorkingDirectory);
        //                var filePaths = client.ListDirectory(directoryPath);
        //                var extensionList = new List<string>() { ".txt" };
        //                var allData = filePaths.Where(s => extensionList.Contains(Path.GetExtension(s.Name))).ToList().OrderBy(s => s.LastWriteTime);
        //                foreach (var records in allData)
        //                {

        //                    //var SourceFile = "/usr/local/arthar/adds/ImportFile/bill.txt";

        //                    #region Operation
        //                    if (client.IsConnected == false)
        //                    {
        //                        client.Connect();
        //                    }
        //                    client.OperationTimeout = new TimeSpan(5, 0, 0);
        //                    var SourceFile = directoryPath + "/" + records.Name;
        //                    var ss = client.ReadAllText(SourceFile);
        //                    var allRecords = ss.Split(',');
        //                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString))
        //                    {
        //                        conn.Open();

        //                        foreach (var item in allRecords)
        //                        {
        //                            if (client.IsConnected == false)
        //                            {
        //                                client.Connect();
        //                            }
        //                            var singleRecord = item.Split('|');
        //                            var serviceCode = singleRecord[0];
        //                            var callingNumber = singleRecord[1];
        //                            var calledNumber = singleRecord[2];
        //                            var RBTCode = singleRecord[3];
        //                            var SPCode = singleRecord[4];

        //                            var StartTime = singleRecord[5]; // 20180523143240 YYYYMMDDhhmmss
        //                            var year = StartTime.Substring(0, 4);
        //                            var month = StartTime.Substring(4, 2);
        //                            var day = StartTime.Substring(6, 2);
        //                            var hour = StartTime.Substring(8, 2);
        //                            var minute = StartTime.Substring(10, 2);
        //                            var second = StartTime.Substring(12, 2);
        //                            var startDate = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;

        //                            var EndTime = singleRecord[6];
        //                            var year2 = EndTime.Substring(0, 4);
        //                            var month2 = EndTime.Substring(4, 2);
        //                            var day2 = EndTime.Substring(6, 2);
        //                            var hour2 = EndTime.Substring(8, 2);
        //                            var minute2 = EndTime.Substring(10, 2);
        //                            var second2 = EndTime.Substring(12, 2);
        //                            var endDate = year2 + "-" + month2 + "-" + day2 + " " + hour2 + ":" + minute2 + ":" + second2;

        //                            DateTime date1 = Convert.ToDateTime(startDate);
        //                            DateTime date2 = Convert.ToDateTime(endDate);

        //                            var playLength = date2.Subtract(date1).TotalMilliseconds;

        //                            var CallScheme = singleRecord[7];
        //                            var DTMF = singleRecord[8];

        //                            using (MySqlCommand cmd = new MySqlCommand("uspa_update_bucket_item_data10", conn))
        //                            {
        //                                cmd.CommandTimeout = int.MaxValue;
        //                                cmd.CommandType = CommandType.StoredProcedure;
        //                                cmd.Parameters.AddWithValue("@callingNumber", callingNumber);
        //                                cmd.Parameters.AddWithValue("@RBTCode", RBTCode);
        //                                cmd.Parameters.AddWithValue("@StartTime", StartTime);
        //                                cmd.Parameters.AddWithValue("@EndTime", EndTime);
        //                                cmd.Parameters.AddWithValue("@DTMF", DTMF);
        //                                cmd.Parameters.AddWithValue("@PlayLengthTicks", playLength);
        //                                cmd.ExecuteNonQuery();
        //                                ViewBag.ExecuteNonQuery = "Yes";
        //                            }

        //                            //using (MySqlCommand cmd = new MySqlCommand("", conn))
        //                            //{
        //                            //    cmd.CommandTimeout = int.MaxValue;
        //                            //    cmd.CommandText = "UPDATE bucket_batch10 SET BATCH_STATUS_ID = 3 WHERE BATCH_STATUS_ID = 1;";
        //                            //    cmd.ExecuteNonQuery();
        //                            //    ViewBag.ExecuteNonQuery = "Yes";
        //                            //}

        //                            //using (MySqlCommand cmd = new MySqlCommand("", conn))
        //                            //{
        //                            //    cmd.CommandTimeout = int.MaxValue;
        //                            //    cmd.CommandText = "UPDATE bucket_item10 SET  ADD_START=" +  StartTime + ",ADD_END=" +  EndTime + ",DTMF_EVENT=" +  DTMF + ",PlayLengthTicks=" + playLength + ",ADD_STATE_ID = 2, SMS_MESSAGE = 'SMS', SMSCost = 5,EMAIL_MESSAGE = 'Email', EmailCost = 5, TotalCost = BID_VALUE + SMSCost+ EmailCost WHERE BUCKET_ID IN(SELECT ID FROM bucket10 WHERE MSISDN=" +  callingNumber + " AND BUCKET_BATCH_ID=(SELECT ID FROM bucket_batch10 WHERE BATCH_STATUS_ID = 3));";
        //                            //    cmd.ExecuteNonQuery();
        //                            //    ViewBag.ExecuteNonQuery = "Yes";
        //                            //}

        //                        }

        //                        conn.Close();
        //                    }
        //                    client.Delete(SourceFile);
        //                    client.Disconnect();
        //                    #endregion
        //                }


        //                ViewBag.Error = "Suceess";

        //                ViewBag.Time = DateTime.Now;



        //            }
        //            client.Disconnect();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = ex.InnerException.Message.ToString();
        //    }

        //    //using (MySqlConnection conn = new MySqlConnection(externalServerConnString))
        //    //{

        //    //    using (MySqlCommand cmd = new MySqlCommand("uspa_test", conn))
        //    //    {
        //    //        cmd.CommandTimeout = 3600;
        //    //        cmd.CommandType = CommandType.StoredProcedure;
        //    //        conn.Open();
        //    //        cmd.Parameters.AddWithValue("@callingNumber", callingNumber);
        //    //        cmd.Parameters.AddWithValue("@RBTCode", RBTCode);
        //    //        cmd.Parameters.AddWithValue("@StartTime", StartTime);
        //    //        cmd.Parameters.AddWithValue("@EndTime", EndTime);
        //    //        cmd.Parameters.AddWithValue("@DTMF", DTMF);
        //    //        cmd.Parameters.AddWithValue("@PlayLengthTicks", playLength);
        //    //        cmd.ExecuteNonQuery();
        //    //        ViewBag.ExecuteNonQuery = "Yes";
        //    //        conn.Close();
        //    //    }
        //    //}


        //    return View();
        //}

        public ActionResult Testing()
        {
            try
            {
                var url = "http://172.29.128.103/AdTransfer/Index?advertId=1332";
                // var url = safricomUrl + "AdvertTransfer?advertId=" + advertId;
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);

                var valuess =  response.Content.Replace("\"", "");
                ViewBag.Value = valuess;
            }
            catch(Exception ex)
            {
                ViewBag.Value = ex.Message.ToString();
            }
            return View();
        }

        public ActionResult FTPTesting()
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                var host = "172.29.128.103";
                var port = 22;
                var username = "adtones";
                var password = "Cha#geD54321!@~";
                var directoryPath = @"/E:/Adverts";
                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();

                    if (client.IsConnected)
                    {
                        AdTransfer.CopyAdToOpeartorServer(83);
                        ViewBag.Value = "True";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Value = ex.Message.ToString();
            }
            return View();
        }


        public async Task<string> Process()
        {
            //var lookupServerDBConnectionString = ConfigurationManager.AppSettings["lookupServerConnectionString"];
            //var getLinuxServerConnectionString = await ExecuteSP("GetLinuxServerConnectionString", lookupServerDBConnectionString, request.CallerID);
            //string toneCode = "1";
            //foreach (DataRow row in getLinuxServerConnectionString.Rows)
            //{
            //    var connectionString = row["LinuxServerConnectionSting"].ToString();
            //    if (!string.IsNullOrEmpty(connectionString))
            //    {
            //        var getToneCode = await ExecuteSP("TempAd", connectionString, "254799873575");
            //        foreach (DataRow row2 in getToneCode.Rows)
            //        {
            //            toneCode = row2["MEDIA_URL"].ToString();
            //        }
            //    }
            //    break;
            //}

            string toneCode = "";

            var connectionString = @"Data Source=134.209.1.123;Initial Catalog=Arthar;User ID=sa;Password=!Qwerty123qaz";
            if (!string.IsNullOrEmpty(connectionString))
            {
                var getToneCode = await ExecuteSP("TempAd", connectionString, "254799873575");
                //foreach (DataRow row2 in getToneCode.Rows)
                //{
                //    toneCode = row2["MEDIA_URL"].ToString();
                //}
            }
           
            return toneCode;
        }

        public async Task<string> ExecuteSP(string spname, string conn, string mobileNumber)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                       
                        //while (reader.Read())
                        //{
                            //Console.WriteLine("{0}\t{1}", reader.GetInt32(0),
                            //    reader.GetString(1));
                            string str = reader["MEDIA_URL"].ToString();
                            reader.GetString(1);
                            
                        //}
                    }
                    con.Close();
                    return "";
                    //using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    //{
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    DataTable dt = new DataTable();
                    //    con.Open();
                    //    //cmd.ExecuteNonQuery();
                    //    cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                    //    da.Fill(dt);
                    //    con.Close();
                    //    return dt;
                    //}
                }
            }
        }

        public ActionResult TestingActiveExpertSMS()
        {
            try
            {
                //string test = ActivExpertSMS.SendSMS("221706077071", "Test Message1");
                //ViewBag.Value = test;
            }
            catch (Exception ex)
            {
                ViewBag.Value = ex.Message.ToString();
            }
            return View();
        }

        public ActionResult test()
        {
            int UserMatch1 = 0, UserMatch2 = 0, UserMatch3 = 0, UserMatch4 = 0, UserMatch5 = 0, UserMatch6 = 0, UserMatch7 = 0, UserMatch8 = 0, UserMatch9 = 0, UserMatch10 = 0;
            var getUserMatchCount = ExecuteSP("GetUserMatchCount", "Data Source=ZWT054\\SQLEXPRESS17;Initial Catalog=Arthar;User ID=sa;Password=this.admin");
            foreach(DataRow row in getUserMatchCount.Rows)
            {
                UserMatch1 = Convert.ToInt32(row["UserMatch1"].ToString());
                UserMatch2 = Convert.ToInt32(row["UserMatch2"].ToString());
                UserMatch3 = Convert.ToInt32(row["UserMatch3"].ToString());
                UserMatch4 = Convert.ToInt32(row["UserMatch4"].ToString());
                UserMatch5 = Convert.ToInt32(row["UserMatch5"].ToString());
                UserMatch6 = Convert.ToInt32(row["UserMatch6"].ToString());
                UserMatch7 = Convert.ToInt32(row["UserMatch7"].ToString());
                UserMatch8 = Convert.ToInt32(row["UserMatch8"].ToString());
                UserMatch9 = Convert.ToInt32(row["UserMatch9"].ToString());
                UserMatch10 = Convert.ToInt32(row["UserMatch10"].ToString());
            }
            UserMatch1 = 51000;
            var userData = "UserMatch1";
            var result = "";
            if (userData != null)
            {
                if (userData == "UserMatch1" && UserMatch1 < 50000)
                    result = "UserMatch1";
                else if (userData == "UserMatch1" && UserMatch2 < 50000)
                    result = "UserMatch2";
                else if (userData == "UserMatch2" && UserMatch3 < 50000)
                    result = "UserMatch3";
                else if (userData == "UserMatch3" && UserMatch4 < 50000)
                    result = "UserMatch4";
                else if (userData == "UserMatch4" && UserMatch5 < 50000)
                    result = "UserMatch5";
                else if (userData == "UserMatch5" && UserMatch6 < 50000)
                    result = "UserMatch6";
                else if (userData == "UserMatch6" && UserMatch7 < 50000)
                    result = "UserMatch7";
                else if (userData == "UserMatch7" && UserMatch8 < 50000)
                    result = "UserMatch8";
                else if (userData == "UserMatch8" && UserMatch9 < 50000)
                    result = "UserMatch9";
                else if (userData == "UserMatch9" && UserMatch10 < 50000)
                    result = "UserMatch10";
                else if (userData == "UserMatch10" && UserMatch1 < 50000)
                    result = "UserMatch1";
            }
            var temp = result;
            return View();
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
                        da.Fill(dt);
                        con.Close();
                        return dt;
                    }
                }
            }
        }
    }
}