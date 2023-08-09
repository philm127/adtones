// ***********************************************************************
// Assembly         : EFMVC.MediaTransfer
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="Processor.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EFMVC.EF;
using log4net;


/// <summary>
/// The MediaTransfer namespace.
/// </summary>
namespace EFMVC.MediaTransfer
{
    /// <summary>
    /// Class Processor.
    /// </summary>
    public class Processor
    {
        /// <summary>
        /// The log
        /// </summary>
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Processes the new media.
        /// </summary>
        /// 

        public void ProcessNewMedia()
        {
            UploadNewAdverts();
        }

        /// <summary>
        /// Uploads the new adverts.
        /// </summary>
        private void UploadNewAdverts()
        {
            try
            {
                using (var entities = new ArtharEntities())
                {
                    IEnumerable<Advert> results = from cAdvert in entities.Adverts
                                                  where cAdvert.UploadedToMediaServer == false && cAdvert.MediaFileLocation != null
                                                  select cAdvert;
                    IList<Advert> list = results.ToList();
                    if (results.Any())
                    {
                        
                        MediaTransferHelper mediaTransferHelper = new MediaTransferHelper();
                        Log.Info("Attempting to ftp " + results.Count() + " files");
                        for (int i = 0; i < list.Count(); i++)
                        {
                            if (mediaTransferHelper.Copy(list[i]))
                            {
                                list[i].UploadedToMediaServer = true;
                                entities.SaveChanges();
                            }
                        }

                        Log.Info("FTP Complete for " + results.Count() + " files");
                    }
                    else
                        return;
                }
            }
            catch (Exception exception)
            {
                Log.Error("FTP Upload failed " + exception.Message, exception);

                //throw;
            }

        }
    }
}
