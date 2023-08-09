using EFMVC.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Common
{
    public static class AdTransfer
    {
        public static string CopyAdToOpeartorServer(int advertId)
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                var advert = db.Adverts.Where(s => s.AdvertId == advertId).FirstOrDefault();
                var mediaFile = advert.MediaFileLocation;
                if (!string.IsNullOrEmpty(mediaFile) && advert.UploadedToMediaServer == false)
                {
                    var getFTPdetails = db.OperatorFTPDetails.Where(s => s.OperatorId == advert.OperatorId).FirstOrDefault();
                    if(getFTPdetails != null)
                    {
                        var host = getFTPdetails.Host;
                        var port = Convert.ToInt32(getFTPdetails.Port);
                        var username = getFTPdetails.UserName;
                        var password = getFTPdetails.Password;
                        var localRoot = System.Web.HttpContext.Current.Server.MapPath("~/Media");
                        var ftpRoot = getFTPdetails.FtpRoot;

                        // Test FTP Details
                        //var host = "138.68.177.47";
                        //var port = 22;
                        //var username = "provisioning";
                        //var password = "adtonespassword";
                        //var localRoot = System.Web.HttpContext.Current.Server.MapPath("~/Media");
                        //var ftpRoot = "/usr/local/arthar/adds";

                        //Live FTP Details
                        //var host = "sftp://10.5.46.46";
                        //var port = 22;
                        //var username = "usdpuser";
                        //var password = "Huawei_123";
                        //var localRoot = System.Web.HttpContext.Current.Server.MapPath("~/Media");
                        //var ftpRoot = "/mnt/Y:/share";

                        using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
                        {
                            client.Connect();
                            if (client.IsConnected)
                            {
                                var SourceFile = localRoot + @"\" + advert.UserId + @"\" + System.IO.Path.GetFileName(advert.MediaFileLocation);
                                var DestinationFile = ftpRoot + "/" + System.IO.Path.GetFileName(advert.MediaFileLocation);
                                var filestream = new FileStream(SourceFile, FileMode.Open);
                                //client.UploadFile(filestream, "/usr/local/arthar" + DestinationFile, null);
                                //client.UploadFile(filestream, DestinationFile);
                                client.UploadFile(filestream, DestinationFile, null);
                                filestream.Close();

                                if (advert.OperatorId == (int)OperatorTableId.Safaricom) // Second File Transfer
                                {
                                    var adName = System.IO.Path.GetFileName(advert.MediaFileLocation);
                                    var temp = adName.Split('.')[0];
                                    var secondAdname = Convert.ToInt64(temp) + 1;

                                    var SourceFile2 = localRoot + @"\" + advert.UserId + @"\SecondAudioFile\" + secondAdname + ".wav";
                                    var DestinationFile2 = ftpRoot + "/" + secondAdname + ".wav";
                                    var filestream2 = new FileStream(SourceFile2, FileMode.Open);
                                    client.UploadFile(filestream2, DestinationFile2, null);
                                    filestream2.Close();
                                }

                                advert.UploadedToMediaServer = true;
                                db.SaveChanges();
                            }
                            client.Disconnect();
                        }
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}