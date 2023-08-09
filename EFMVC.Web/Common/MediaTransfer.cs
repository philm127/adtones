using EFMVC.Data;
using System;
using System.IO;
using System.Linq;
using Renci.SshNet;

namespace EFMVC.Web.Common
{
   
    public static class MediaTransfer
    {

        public static void MediaFileTransfer()
        {
            UploadNewAdverts();
        }
        public static void UploadNewAdverts()
        {
            try
            {
                using (var entities = new EFMVCDataContex())
                {
                    var results = from cAdvert in entities.Adverts
                                  where cAdvert.UploadedToMediaServer == false && cAdvert.MediaFileLocation != null
                                  select cAdvert;
                    var list = results.ToList();
                    if (results.Any())
                    {

                        for (int i = 0; i < list.Count(); i++)
                        {
                            if (Copy(list[i]))
                            {
                                list[i].UploadedToMediaServer = true;
                                entities.SaveChanges();
                            }
                        }

                    }
                    else
                        return;
                }
            }
            catch (Exception exception)
            {

            }

        }

        public static bool Copy(Model.Advert advert)
        {
            try
            {


                var host = "188.166.171.194";
                var port = 22;
                var username = "provisioning";
                var password = "adtonespassword";
                var localRoot = @"C:\inetpub\wwwroot\Media";
                var ftpRoot = "/usr/local/arthar/adds";
                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        var SourceFile = localRoot + @"\" + advert.UserId + @"\" + Path.GetFileName(advert.MediaFileLocation);
                        var DestinationFile = ftpRoot + " / " + Path.GetFileName(advert.MediaFileLocation);
                        var filestream = new FileStream(SourceFile, FileMode.Open);
                        client.UploadFile(filestream, DestinationFile);
                    }

                }

                return true;
            }
            catch (Exception exception)
            {
                return false;
                throw;
            }

        }


    }
}