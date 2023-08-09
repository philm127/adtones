// ***********************************************************************
// Assembly         : EFMVC.MediaTransfer
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="MediaTransferHelper.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tamir.SharpSsh;
using log4net;


using System.Web;
using Renci.SshNet;
/// <summary>
/// The MediaTransfer namespace.
/// </summary>
namespace EFMVC.MediaTransfer
{
    /// <summary>
    /// Class MediaTransferHelper.
    /// </summary>
    public class MediaTransferHelper
    {
        /// <summary>
        /// The log
        /// </summary>
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Copies the specified advert.
        /// </summary>
        /// <param name="advert">The advert.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Copy(EF.Advert advert)
        {
            try
            {
                //Code commented on 19/04/2017
                //Log.Info("Attempting to ftp file:" + advert.MediaFileLocation);
                //var sftp = new Sftp(Settings1.Default.ftpHost, Settings1.Default.ftpUserName,
                //                    Settings1.Default.ftpPassword);


                //sftp.Connect(21);

                //Log.Info("Local path: " + Settings1.Default.localRoot + @"\" + advert.UserId + @"\" + Path.GetFileName(advert.MediaFileLocation));
                //Log.Info("FTP path: " + Settings1.Default.ftpRoot + "/"+Path.GetFileName(advert.MediaFileLocation));

                //sftp.Put(Settings1.Default.localRoot + @"\" + advert.UserId + @"\" + Path.GetFileName(advert.MediaFileLocation), Settings1.Default.ftpRoot + "/" + Path.GetFileName(advert.MediaFileLocation));

                ////sftp.Get(getFilePath);

               // var test = ConfigurationManager.AppSettings["localRoot"].ToString();

                var host = Settings1.Default.ftpHost;
                var port = 22;
                var username = Settings1.Default.ftpUserName;
                var password = Settings1.Default.ftpPassword;

                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {                       
                        var SourceFile = Settings1.Default.localRoot + @"\" + advert.UserId + @"\" + Path.GetFileName(advert.MediaFileLocation);
                        var DestinationFile = Settings1.Default.ftpRoot + "/" + Path.GetFileName(advert.MediaFileLocation);
                        var filestream = new FileStream(SourceFile, FileMode.Open);                       
                        client.UploadFile(filestream, DestinationFile);
                    }
                  
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Error("FTP failed for file:" + advert.MediaFileLocation + " - " + exception.Message, exception);
                return false;
                throw;
            }

        }
    }
}