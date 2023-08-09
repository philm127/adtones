using EFMVC.Data;
using EFMVC.ProvisioningModel;
using Microsoft.AspNet.SignalR;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Threading;

namespace EFMVC.Web.Hubs
{
    public class AdDetailHub : Hub
    {
        public void LetsChat(string Cl_Name, string Cl_Message)
        {
            Clients.All.NewMessage(Cl_Name, Cl_Message);
           // Clients.All.Testing(Cl_Name, Cl_Message);
        }

        public void GetAdsName(string requestFormat)
        {
           
        
            try
            {
                var header = requestFormat.Substring(0, 14);
                var userAdName = header;
                // var userAdName = "ad100000567890";
                
                #region SQLServer
                // EFMVCDataContex db = new EFMVCDataContex();
                //var userAdName = "Ad not Found";
                //var userProfile = db.Userprofiles.Where(s => s.MSISDN == mobile).FirstOrDefault();
                //if (userProfile != null)
                //{
                //    var userProfileId = userProfile.UserProfileId;
                //    var userprofileAdvert = db.UserProfileAdvertsReceived.Where(s => s.UserProfileId == userProfileId).OrderByDescending(s => s.AddedDate).FirstOrDefault();
                //    if(userprofileAdvert != null)
                //    {
                //         userAdName = userprofileAdvert.AdvertName == null? "Test Ad": userprofileAdvert.AdvertName;
                //        Clients.All.AdName(userAdName);
                //    }
                //    else
                //    {
                //        Clients.All.AdName(userAdName);
                //    }
                //}
                //else
                //{
                //    Clients.All.AdName(userAdName);
                //}
                #endregion

                #region Request Parameter
                //Request Parameter
                //------------
                //ad1000005678900000000044798000000200000000447980720251#
                //ad1 - IP ID
                //00000 - Space
                //56789 - Message ID
                //0 - CMD
                //00000000 - Space
                //447980720250 - Caller ID
                //00000000 - Space
                //447980720251 - Called ID
                //# - End Flag
                #endregion

                #region Response Parameter
                //Response Result
                //----------------
                //ad100000567890xxxxxxxxxxxxxxxxxabc0#
                //ad1 - IP ID
                //00000 - Space
                //56789 - Message ID
                //0 - CMD
                //xxxxxxxxxxxxxxxxxabc - AdName
                //0 - Result (O:Success, 1:Fail)
                //# - End Flag
                #endregion
                string externalServerConnString = ConfigurationManager.AppSettings["externalserverconnectionstring"];

                var count = getlookupCount(externalServerConnString);
                int cnt = Convert.ToInt32(count);

                long num = 447890000012;
                Thread thread1 = new Thread(() => func1(num,externalServerConnString, userAdName));
                thread1.Start();


                long num2 = 447900000022;
                Thread thread2 = new Thread(() => func2(num2, externalServerConnString, userAdName));
                thread2.Start();

                long num3 = 447910000032;
                Thread thread3 = new Thread(() => func3(num3, externalServerConnString, userAdName));
                thread3.Start();

                long num4 = 447920000042;
                Thread thread4 = new Thread(() => func4(num4, externalServerConnString, userAdName));
                thread4.Start();

                long num5 = 447930000052;
                Thread thread5 = new Thread(() => func5(num5, externalServerConnString, userAdName));
                thread5.Start();

                long num6 = 447940000062;
                Thread thread6 = new Thread(() => func6(num6, externalServerConnString, userAdName));
                thread6.Start();

                long num7 = 447950000072;
                Thread thread7 = new Thread(() => func7(num7, externalServerConnString, userAdName));
                thread7.Start();

                long num8 = 447960000082;
                Thread thread8 = new Thread(() => func8(num8, externalServerConnString, userAdName));
                thread8.Start();

                long num9 = 447970000092;
                Thread thread9 = new Thread(() => func9(num9, externalServerConnString, userAdName));
                thread9.Start();

                long num10 = 447980000002;
                Thread thread10 = new Thread(() => func10(num10, externalServerConnString, userAdName));
                thread10.Start();

                #region ForLoop
                //for (int i = 1; i < cnt; i++)
                //{
                //    var mobileNumber = getLookupNumber(temp, externalServerConnString);
                //    temp = temp + 1;

                //   // var mobileNumber = requestFormat.Substring(22, 12);

                //    bool isExist = IsLookUptable(mobileNumber, externalServerConnString);

                //    if (isExist)
                //    {
                //        using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                //        {
                //            using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                //            {
                //                cmd.CommandTimeout = 3600;
                //                cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                //                cmd.CommandType = CommandType.StoredProcedure;
                //                conn.Open();

                //                MySqlDataReader dr = cmd.ExecuteReader();
                //                while (dr.Read())
                //                {
                //                    var adDetail = Convert.ToString(dr["adname"]);
                //                    if (adDetail == "NoAd")
                //                    {
                //                        var noDetail = userAdName + adDetail + "1#";
                //                        Clients.All.AdName(noDetail);
                //                    }
                //                    else
                //                    {
                //                        var fileName = userAdName + adDetail + "0#";
                //                        Clients.All.AdName(fileName);
                //                    }
                //                }
                //                conn.Close();
                //            }
                //        }
                //    }
                //    else
                //    {
                //        var noDetail = userAdName + "NoAdd1#";
                //        Clients.All.AdName(noDetail);
                //    }
                //}
                #endregion


                #region FromLocalDB
                //var mysqldb = new arthar_addcache_provisioningEntities4();
                //var bucketData = mysqldb.BUCKETs.Where(s => s.MSISDN == mobileNumber).OrderByDescending(s => s.ID);
                //if (bucketData != null)
                //{
                //    var bucketId = bucketData.FirstOrDefault().ID;
                //    var bucketItemData = mysqldb.BUCKET_ITEM.Where(s => s.BUCKET_ID == bucketId && s.ADD_STATE_ID == 2).OrderByDescending(s => s.ID); // Add condition AddStateID = 2(Play Add) 
                //    if (bucketItemData != null)
                //    {
                //        var fileName = userAdName + bucketItemData.FirstOrDefault().MEDIA_URL + "0#";
                //        Clients.All.AdName(fileName);
                //    }
                //    else
                //    {
                //        userAdName = userAdName + "NoAdd1#";
                //        Clients.Caller.AdName(userAdName);
                //    }

                //}
                //else
                //{
                //    userAdName = userAdName + "NoAdd1#";
                //    Clients.All.AdName(userAdName);
                //}
                #endregion

            }
            catch (Exception ex)
            {
                var userAdName = "ad100000567890NoAdd1#";
                Clients.All.AdName(userAdName);
            }

        }       

        private string getlookupCount(string externalServerConnString)
        {
            string tableRecords = @"select count(*) as cnt FROM  44798table";
            MySqlConnection MyConn = new MySqlConnection(externalServerConnString);
            MySqlCommand MyCommand = new MySqlCommand(tableRecords, MyConn);
            MyConn.Open();
            MySqlDataReader MyReader = MyCommand.ExecuteReader();

            while (MyReader.Read())
            {
                var count = Convert.ToString(MyReader["cnt"]);
                return count;
            }

            return "0";
        }

        private string getLookupNumber(long mobileNumber, string externalServerConnString)
        {
            using (MySqlConnection connn = new MySqlConnection(externalServerConnString)) //External Server Connection
            {
                using (MySqlCommand cmdd = new MySqlCommand("get_mobilenumber_lookuptable", connn))
                {
                    cmdd.CommandTimeout = 3600;
                    cmdd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber.ToString()));
                    cmdd.CommandType = CommandType.StoredProcedure;
                    connn.Open();

                    MySqlDataReader drr = cmdd.ExecuteReader();
                    while (drr.Read())
                    {
                        var number = Convert.ToString(drr["MobileNumber"]);
                        return number;
                    }
                }
            }
            return "no";
        }

        private bool IsLookUptable(string mobileNumber, string externalServerConnString)
        {          
            using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
            {
                using (MySqlCommand cmd = new MySqlCommand("number_exist_on_lookup_table", conn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        conn.Close();
                        return true;                       
                    }                    
                    conn.Close();
                    return false;
                }
            }
        }

        #region Threading Function

        private void func1(long num, string externalServerConnString, string userAdName)
        {

            //for (int i = 1; i <= 10; i++)
            //{
                // var mobileNumber = getLookupNumber(num, externalServerConnString);

                var mobileNumber = num.ToString();
                // var mobileNumber = requestFormat.Substring(22, 12);

                bool isExist = IsLookUptable(mobileNumber, externalServerConnString);
                //num = num + 1;
                if (isExist)
                {
                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                    {
                        using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                            MySqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var adDetail = Convert.ToString(dr["adname"]);
                                if (adDetail == "NoAd")
                                {
                                    var noDetail =  userAdName + adDetail + "1#";
                                    Clients.All.AdName(noDetail);
                                }
                                else
                                {
                                    var fileName =  userAdName + adDetail + "0#";
                                    Clients.All.AdName(fileName);
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    var noDetail =  userAdName + "NoAdd1#";
                    Clients.All.AdName(noDetail);
                }
            //}
        }

        private void func2(long num, string externalServerConnString, string userAdName)
        {

            //for (int i = 1; i <= 10; i++)
            //{
                // var mobileNumber = getLookupNumber(num, externalServerConnString);

                var mobileNumber = num.ToString();

                // var mobileNumber = requestFormat.Substring(22, 12);

                bool isExist = IsLookUptable(mobileNumber, externalServerConnString);
               // num = num + 1;

                if (isExist)
                {
                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                    {
                        using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                            MySqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var adDetail = Convert.ToString(dr["adname"]);
                                if (adDetail == "NoAd")
                                {
                                    var noDetail =  userAdName + adDetail + "1#";
                                    Clients.All.AdName(noDetail);
                                }
                                else
                                {
                                    var fileName = userAdName + adDetail + "0#";
                                    Clients.All.AdName(fileName);
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    var noDetail =  userAdName + "NoAdd1#";
                    Clients.All.AdName(noDetail);
                }
            //}
        }

        private void func3(long num, string externalServerConnString, string userAdName)
        {

            //for (int i = 1; i <= 10; i++)
            //{
                // var mobileNumber = getLookupNumber(num, externalServerConnString);

                var mobileNumber = num.ToString();

                // var mobileNumber = requestFormat.Substring(22, 12);

                bool isExist = IsLookUptable(mobileNumber, externalServerConnString);
               // num = num + 1;

                if (isExist)
                {
                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                    {
                        using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                            MySqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var adDetail = Convert.ToString(dr["adname"]);
                                if (adDetail == "NoAd")
                                {
                                    var noDetail =  userAdName + adDetail + "1#";
                                    Clients.All.AdName(noDetail);
                                }
                                else
                                {
                                    var fileName =  userAdName + adDetail + "0#";
                                    Clients.All.AdName(fileName);
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    var noDetail =  userAdName + "NoAdd1#";
                    Clients.All.AdName(noDetail);
                }
            //}
        }

        private void func4(long num, string externalServerConnString, string userAdName)
        {

            //for (int i = 1; i <= 10; i++)
            //{
                // var mobileNumber = getLookupNumber(num, externalServerConnString);

                var mobileNumber = num.ToString();

                // var mobileNumber = requestFormat.Substring(22, 12);

                bool isExist = IsLookUptable(mobileNumber, externalServerConnString);
                //num = num + 1;

                if (isExist)
                {
                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                    {
                        using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                            MySqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var adDetail = Convert.ToString(dr["adname"]);
                                if (adDetail == "NoAd")
                                {
                                    var noDetail = userAdName + adDetail + "1#";
                                    Clients.All.AdName(noDetail);
                                }
                                else
                                {
                                    var fileName = userAdName + adDetail + "0#";
                                    Clients.All.AdName(fileName);
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    var noDetail = userAdName + "NoAdd1#";
                    Clients.All.AdName(noDetail);
                }
            //}
        }

        private void func5(long num, string externalServerConnString, string userAdName)
        {

            //for (int i = 1; i <= 10; i++)
            //{
                // var mobileNumber = getLookupNumber(num, externalServerConnString);

                var mobileNumber = num.ToString();

                // var mobileNumber = requestFormat.Substring(22, 12);

                bool isExist = IsLookUptable(mobileNumber, externalServerConnString);
               // num = num + 1;

                if (isExist)
                {
                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                    {
                        using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                            MySqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var adDetail = Convert.ToString(dr["adname"]);
                                if (adDetail == "NoAd")
                                {
                                    var noDetail = userAdName + adDetail + "1#";
                                    Clients.All.AdName(noDetail);
                                }
                                else
                                {
                                    var fileName = userAdName + adDetail + "0#";
                                    Clients.All.AdName(fileName);
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    var noDetail = userAdName + "NoAdd1#";
                    Clients.All.AdName(noDetail);
                }
            //}
        }

        private void func6(long num, string externalServerConnString, string userAdName)
        {

            //for (int i = 1; i <= 10; i++)
            //{
                // var mobileNumber = getLookupNumber(num, externalServerConnString);

                var mobileNumber = num.ToString();

                // var mobileNumber = requestFormat.Substring(22, 12);

                bool isExist = IsLookUptable(mobileNumber, externalServerConnString);
               // num = num + 1;

                if (isExist)
                {
                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                    {
                        using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                            MySqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var adDetail = Convert.ToString(dr["adname"]);
                                if (adDetail == "NoAd")
                                {
                                    var noDetail = userAdName + adDetail + "1#";
                                    Clients.All.AdName(noDetail);
                                }
                                else
                                {
                                    var fileName = userAdName + adDetail + "0#";
                                    Clients.All.AdName(fileName);
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    var noDetail = userAdName + "NoAdd1#";
                    Clients.All.AdName(noDetail);
                }
            //}
        }

        private void func7(long num, string externalServerConnString, string userAdName)
        {

            //for (int i = 1; i <= 10; i++)
            //{
                // var mobileNumber = getLookupNumber(num, externalServerConnString);

                var mobileNumber = num.ToString();

                // var mobileNumber = requestFormat.Substring(22, 12);

                bool isExist = IsLookUptable(mobileNumber, externalServerConnString);
               // num = num + 1;

                if (isExist)
                {
                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                    {
                        using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                            MySqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var adDetail = Convert.ToString(dr["adname"]);
                                if (adDetail == "NoAd")
                                {
                                    var noDetail = userAdName + adDetail + "1#";
                                    Clients.All.AdName(noDetail);
                                }
                                else
                                {
                                    var fileName = userAdName + adDetail + "0#";
                                    Clients.All.AdName(fileName);
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    var noDetail = userAdName + "NoAdd1#";
                    Clients.All.AdName(noDetail);
                }
            //}
        }

        private void func8(long num, string externalServerConnString, string userAdName)
        {

            //for (int i = 1; i <= 10; i++)
            //{
                // var mobileNumber = getLookupNumber(num, externalServerConnString);

                var mobileNumber = num.ToString();

                // var mobileNumber = requestFormat.Substring(22, 12);

                bool isExist = IsLookUptable(mobileNumber, externalServerConnString);
                //num = num + 1;

                if (isExist)
                {
                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                    {
                        using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                            MySqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var adDetail = Convert.ToString(dr["adname"]);
                                if (adDetail == "NoAd")
                                {
                                    var noDetail = userAdName + adDetail + "1#";
                                    Clients.All.AdName(noDetail);
                                }
                                else
                                {
                                    var fileName = userAdName + adDetail + "0#";
                                    Clients.All.AdName(fileName);
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    var noDetail = userAdName + "NoAdd1#";
                    Clients.All.AdName(noDetail);
                }
            //}
        }

        private void func9(long num, string externalServerConnString, string userAdName)
        {

            //for (int i = 1; i <= 10; i++)
            //{
                // var mobileNumber = getLookupNumber(num, externalServerConnString);

                var mobileNumber = num.ToString();

                // var mobileNumber = requestFormat.Substring(22, 12);

                bool isExist = IsLookUptable(mobileNumber, externalServerConnString);
                //num = num + 1;

                if (isExist)
                {
                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                    {
                        using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                            MySqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var adDetail = Convert.ToString(dr["adname"]);
                                if (adDetail == "NoAd")
                                {
                                    var noDetail = userAdName + adDetail + "1#";
                                    Clients.All.AdName(noDetail);
                                }
                                else
                                {
                                    var fileName = userAdName + adDetail + "0#";
                                    Clients.All.AdName(fileName);
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    var noDetail = userAdName + "NoAdd1#";
                    Clients.All.AdName(noDetail);
                }
            //}
        }

        private void func10(long num, string externalServerConnString, string userAdName)
        {

            //for (int i = 1; i <= 10; i++)
            //{
                // var mobileNumber = getLookupNumber(num, externalServerConnString);

                var mobileNumber = num.ToString();

                // var mobileNumber = requestFormat.Substring(22, 12);

                bool isExist = IsLookUptable(mobileNumber, externalServerConnString);
               // num = num + 1;

                if (isExist)
                {
                    using (MySqlConnection conn = new MySqlConnection(externalServerConnString)) //External Server Connection
                    {
                        using (MySqlCommand cmd = new MySqlCommand("get_ad_name", conn))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Parameters.Add(new MySqlParameter("phonenumber", mobileNumber));
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();

                            MySqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                var adDetail = Convert.ToString(dr["adname"]);
                                if (adDetail == "NoAd")
                                {
                                    var noDetail = userAdName + adDetail + "1#";
                                    Clients.All.AdName(noDetail);
                                }
                                else
                                {
                                    var fileName = userAdName + adDetail + "0#";
                                    Clients.All.AdName(fileName);
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    var noDetail = userAdName + "NoAdd1#";
                    Clients.All.AdName(noDetail);
                }
            //}
        }

        #endregion
    }
}