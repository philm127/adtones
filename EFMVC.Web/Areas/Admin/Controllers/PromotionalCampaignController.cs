using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using EFMVC.Model.Entities;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Areas.Admin.ViewModel;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using EFMVC.Web.Models;
using MadScripterWrappers;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("PromotionalCampaign")]
    public class PromotionalCampaignController : Controller
    {
        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The _operator Repository
        /// </summary>
        private readonly IOperatorRepository _operatorRepository;

        /// <summary>
        /// The _operatorFTPDetails Repository
        /// </summary>
        private readonly IOperatorFTPDetailsRepository _operatorFTPDetailsRepository;

        /// <summary>
        /// The _promotionalUser Repository
        /// </summary>
        private readonly IPromotionalUserRepository _promotionalUserRepository;

        /// <summary>
        /// The _promotionalCampaign Repository
        /// </summary>
        private readonly IPromotionalCampaignRepository _promotionalCampaignRepository;

        /// <summary>
        /// The _promotionalAdvert Repository
        /// </summary>
        private readonly IPromotionalAdvertRepository _promotionalAdvertRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;


        public PromotionalCampaignController(ICommandBus commandBus, ICountryRepository countryRepository, IOperatorRepository operatorRepository, IOperatorFTPDetailsRepository operatorFTPDetailsRepository, IPromotionalUserRepository promotionalUserRepository, IPromotionalCampaignRepository promotionalCampaignRepository, IPromotionalAdvertRepository promotionalAdvertRepository)
        {
            _commandBus = commandBus;
            _countryRepository = countryRepository;
            _operatorRepository = operatorRepository;
            _operatorFTPDetailsRepository = operatorFTPDetailsRepository;
            _promotionalUserRepository = promotionalUserRepository;
            _promotionalCampaignRepository = promotionalCampaignRepository;
            _promotionalAdvertRepository = promotionalAdvertRepository;
        }

        // GET: Admin/PromotionalCampaign
        [Route("Index")]
        public async Task<ActionResult> Index()
        {
            List<PromotionalCampaignResult> _result = await FillPromotionalCampaignResult(null,null);
            SearchClass.OperatorFilter _filterCritearea = new SearchClass.OperatorFilter();
            await FillOperator(0, 0);
            return View(_result);
        }

        // List Promotional Campaign
        private async Task<List<PromotionalCampaignResult>> FillPromotionalCampaignResult(int?[] OperatorId, int?[] BatchId)
        {
            var promotionalCampaignResult = _promotionalCampaignRepository.AsQueryable().Include(nameof(PromotionalCampaign.Operator));
            if (OperatorId != null && OperatorId.Length > 0 && OperatorId[0] != 0)
                promotionalCampaignResult = promotionalCampaignResult.Where(pc => OperatorId.Contains(pc.OperatorID));
            if (BatchId != null && BatchId.Length > 0 && BatchId[0] != 0)
                promotionalCampaignResult = promotionalCampaignResult.Where(pc => BatchId.Contains(pc.BatchID));
            var joined = promotionalCampaignResult.Join(_promotionalAdvertRepository.AsQueryable(),
                a => a.ID, c => c.CampaignID, (c, a) => new {Campaign = c, Advert = a});
            return await joined.Select(j => new PromotionalCampaignResult
            {
                OperatorId = j.Campaign.OperatorID,
                OperatorName = j.Campaign.Operator.OperatorName,
                CampaignId = j.Campaign.ID,
                CampaignName = j.Campaign.CampaignName,
                BatchID = j.Campaign.BatchID,
                MaxDaily = j.Campaign.MaxDaily,
                MaxWeekly = j.Campaign.MaxWeekly,
                AdvertName = j.Advert.AdvertName,
                AdvertLocation = j.Campaign.AdvertLocation,
                Status = j.Campaign.Status,
                rStatus = j.Campaign.Status == 1 ? "Play" : "Stop"
            }).ToListAsync();
        }

        [Route("AddPromotionalCampaign")]
        public async Task<ActionResult> AddPromotionalCampaign()
        {
            await FillCountry(0, 0);
            PromotionalCampaignFormModel _promotionalCampaignModel = new PromotionalCampaignFormModel();
            _promotionalCampaignModel.Status = (int)PromotionalCampaignStatus.Play;
            return View(_promotionalCampaignModel);
        }

        //To Add Promotional Campaign Data in DB Server
        [Route("AddPromotionalCampaign")]
        [HttpPost]
        public async Task<ActionResult> AddPromotionalCampaign(PromotionalCampaignFormModel model, HttpPostedFileBase mediaFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (mediaFile != null)
                    {
                        string operatorName = _operatorRepository.GetById(model.OperatorId).OperatorName.ToString();

                        var operatorConnectionString = ConnectionString.GetSingleConnectionStringByOperatorId(model.OperatorId);

                        if (!string.IsNullOrEmpty(operatorConnectionString))
                        {
                            EFMVCDataContex db = new EFMVCDataContex(operatorConnectionString);
                            var isBatchIDExist = await _promotionalCampaignRepository.AsQueryable().Where(top => top.BatchID.Equals(model.BatchID)).ToListAsync();
                            if (isBatchIDExist.Count > 0)
                            {
                                await FillCountry(model.CountryId, model.OperatorId);
                                TempData["Error"] = "BatchID exists to " + operatorName + " operator.";
                                return View(model);
                            }
                            else
                            {
                                if (model.OperatorId == (int)OperatorTableId.Expresso)
                                {
                                    #region Media
                                    if (mediaFile.ContentLength != 0)
                                    {
                                        var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();
                                        string fileName = firstAudioName;

                                        string extension = Path.GetExtension(mediaFile.FileName);
                                        var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                                        //string outputFormat = model.OperatorId == 1 ? "wav" : model.OperatorId == 2 ? "mp3" : "wav";
                                        string outputFormat = "wav";
                                        var audioFormatExtension = "." + outputFormat;

                                        if (extension != audioFormatExtension)
                                        {
                                            string tempDirectoryName = Server.MapPath("~/PromotionalMedia/Temp/");
                                            string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
                                            mediaFile.SaveAs(tempPath);

                                            SaveConvertedFile(tempPath, extension, model.OperatorId.ToString(), fileName, outputFormat);

                                            model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorId.ToString(),
                                                                                fileName + "." + outputFormat);
                                        }
                                        else
                                        {
                                            string directoryName = Server.MapPath("~/PromotionalMedia/");
                                            directoryName = Path.Combine(directoryName, model.OperatorId.ToString());

                                            if (!Directory.Exists(directoryName))
                                                Directory.CreateDirectory(directoryName);

                                            string path = Path.Combine(directoryName, fileName + extension);
                                            mediaFile.SaveAs(path);

                                            string archiveDirectoryName = Server.MapPath("~/PromotionalMedia/Archive/");

                                            if (!Directory.Exists(archiveDirectoryName))
                                                Directory.CreateDirectory(archiveDirectoryName);

                                            string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                            mediaFile.SaveAs(archivePath);

                                            model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorId.ToString(),
                                                                                    fileName + extension);
                                        }

                                        //Add Promotional Campaign Data on DB Server
                                        CreateOrUpdatePromotionalCampaignCommand promotionalCampaignCommand =
                                            Mapper.Map<PromotionalCampaignFormModel, CreateOrUpdatePromotionalCampaignCommand>(model);
                                        ICommandResult promotionalCampaignResult = _commandBus.Submit(promotionalCampaignCommand);
                                        if (promotionalCampaignResult.Success)
                                        {
                                            int promotionalCampaignId = promotionalCampaignResult.Id;
                                            string adName = "";
                                            if (string.IsNullOrEmpty(model.AdvertLocation))
                                            {
                                                adName = "";
                                            }
                                            else
                                            {
                                                if (model.OperatorId == (int)OperatorTableId.Expresso)
                                                {
                                                    var operatorFTPDetails = _operatorFTPDetailsRepository.Get(top => top.OperatorId == model.OperatorId);

                                                    //Transfer Advert File From Operator Server to Linux Server
                                                    var returnValue = CopyAdToOpeartorServer(model.AdvertLocation, model.OperatorId, operatorFTPDetails);
                                                    if (returnValue == "Success")
                                                    {
                                                        if (operatorFTPDetails != null) adName = operatorFTPDetails.FtpRoot + "/" + model.AdvertLocation.Split('/')[3];

                                                        //Get and Update Promotional Campaign Data on DB Server
                                                        var promotionalCampaignData = await db.PromotionalCampaigns.FirstOrDefaultAsync(top => top.AdtoneServerPromotionalCampaignId == promotionalCampaignId && top.BatchID == model.BatchID && top.CampaignName.ToLower().Equals(model.CampaignName.ToLower()));
                                                        if (promotionalCampaignData != null)
                                                        {
                                                            promotionalCampaignData.AdvertLocation = adName;
                                                            db.SaveChanges();
                                                        }
                                                    }
                                                }
                                            }

                                            //Add Promotional Advert Data on DB Server
                                            PromotionalAdvertFormModel promotionalAdvertModel = new PromotionalAdvertFormModel();
                                            promotionalAdvertModel.ID = model.ID;
                                            promotionalAdvertModel.CampaignID = promotionalCampaignId;
                                            promotionalAdvertModel.OperatorID = model.OperatorId;
                                            promotionalAdvertModel.AdvertName = model.AdvertName;
                                            promotionalAdvertModel.AdvertLocation = model.AdvertLocation;

                                            CreateOrUpdatePromotionalAdvertCommand promotionalAdvertCommand =
                                                Mapper.Map<PromotionalAdvertFormModel, CreateOrUpdatePromotionalAdvertCommand>(promotionalAdvertModel);
                                            ICommandResult promotionalAdvertResult = _commandBus.Submit(promotionalAdvertCommand);
                                            if (promotionalAdvertResult.Success)
                                            {
                                                int promotionalAdvertId = promotionalAdvertResult.Id;
                                                //Get and Update Promotional Advert Data on DB Server
                                                var promotionalAdvertData = await db.PromotionalAdverts.FirstOrDefaultAsync(top => top.AdtoneServerPromotionalAdvertId == promotionalAdvertId);
                                                if (promotionalAdvertData != null)
                                                {
                                                    promotionalAdvertData.AdvertLocation = adName;
                                                    await db.SaveChangesAsync();
                                                }
                                            }

                                            await OnCampaignCreated(model.BatchID);
                                        }
                                    }
                                    #endregion

                                    TempData["success"] = "Campaign and Advert added successfully for operator " + operatorName + ".";
                                    return RedirectToAction("Index");
                                }
                                else if (model.OperatorId == (int)OperatorTableId.Safaricom)
                                {
                                    #region Media
                                    if (mediaFile.ContentLength != 0)
                                    {
                                        var firstAudioName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Second.ToString();
                                        string fileName = firstAudioName;

                                        string extension = Path.GetExtension(mediaFile.FileName);
                                        var onlyFileName = Path.GetFileNameWithoutExtension(mediaFile.FileName);
                                        string outputFormat = model.OperatorId == 1 ? "wav" : model.OperatorId == 2 ? "mp3" : "wav";
                                        var audioFormatExtension = "." + outputFormat;

                                        if (extension != audioFormatExtension)
                                        {
                                            string tempDirectoryName = Server.MapPath("~/PromotionalMedia/Temp/");
                                            if (!Directory.Exists(tempDirectoryName))
                                                Directory.CreateDirectory(tempDirectoryName);
                                            string tempPath = Path.Combine(tempDirectoryName, fileName + extension);
                                            mediaFile.SaveAs(tempPath);

                                            SaveConvertedFile(tempPath, extension, model.OperatorId.ToString(), fileName, outputFormat);

                                            model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorId.ToString(),
                                                                                fileName + "." + outputFormat);
                                        }
                                        else
                                        {
                                            string directoryName = Server.MapPath("~/PromotionalMedia/");
                                            directoryName = Path.Combine(directoryName, model.OperatorId.ToString());

                                            if (!Directory.Exists(directoryName))
                                                Directory.CreateDirectory(directoryName);

                                            string path = Path.Combine(directoryName, fileName + extension);
                                            mediaFile.SaveAs(path);

                                            string archiveDirectoryName = Server.MapPath("~/PromotionalMedia/Archive/");

                                            if (!Directory.Exists(archiveDirectoryName))
                                                Directory.CreateDirectory(archiveDirectoryName);

                                            string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                                            mediaFile.SaveAs(archivePath);

                                            model.AdvertLocation = string.Format("/PromotionalMedia/{0}/{1}", model.OperatorId.ToString(),
                                                                                    fileName + extension);
                                        }

                                        //Add Promotional Campaign Data on DB Server
                                        CreateOrUpdatePromotionalCampaignCommand promotionalCampaignCommand =
                                            Mapper.Map<PromotionalCampaignFormModel, CreateOrUpdatePromotionalCampaignCommand>(model);
                                        ICommandResult promotionalCampaignResult = _commandBus.Submit(promotionalCampaignCommand);
                                        if (promotionalCampaignResult.Success)
                                        {
                                            int promotionalCampaignId = promotionalCampaignResult.Id;
                                            string adName = "";
                                            if (string.IsNullOrEmpty(model.AdvertLocation))
                                            {
                                                adName = "";
                                            }
                                            else
                                            {
                                                if (model.OperatorId == (int)OperatorTableId.Safaricom)
                                                {
                                                    var operatorFTPDetails = await _operatorFTPDetailsRepository.AsQueryable().FirstOrDefaultAsync(top => top.OperatorId == model.OperatorId);

                                                    //Transfer Advert File From Operator Server to Linux Server
                                                    var returnValue = CopyAdToOpeartorServer(model.AdvertLocation, model.OperatorId, operatorFTPDetails);
                                                    if (returnValue == "Success")
                                                    {
                                                        if (operatorFTPDetails != null) adName = operatorFTPDetails.FtpRoot + "/" + model.AdvertLocation.Split('/')[3];

                                                        //Get and Update Promotional Campaign Data on DB Server
                                                        var promotionalCampaignData = await db.PromotionalCampaigns.FirstOrDefaultAsync(top => top.AdtoneServerPromotionalCampaignId == promotionalCampaignId && top.BatchID == model.BatchID && top.CampaignName.ToLower().Equals(model.CampaignName.ToLower()));
                                                        if (promotionalCampaignData != null)
                                                        {
                                                            promotionalCampaignData.AdvertLocation = adName;
                                                            db.SaveChanges();
                                                        }
                                                    }
                                                }
                                            }

                                            //Add Promotional Advert Data on DB Server
                                            PromotionalAdvertFormModel promotionalAdvertModel = new PromotionalAdvertFormModel();
                                            promotionalAdvertModel.ID = model.ID;
                                            promotionalAdvertModel.CampaignID = promotionalCampaignId;
                                            promotionalAdvertModel.OperatorID = model.OperatorId;
                                            promotionalAdvertModel.AdvertName = model.AdvertName;
                                            promotionalAdvertModel.AdvertLocation = model.AdvertLocation;

                                            CreateOrUpdatePromotionalAdvertCommand promotionalAdvertCommand =
                                                Mapper.Map<PromotionalAdvertFormModel, CreateOrUpdatePromotionalAdvertCommand>(promotionalAdvertModel);
                                            ICommandResult promotionalAdvertResult = _commandBus.Submit(promotionalAdvertCommand);
                                            if (promotionalAdvertResult.Success)
                                            {
                                                int promotionalAdvertId = promotionalAdvertResult.Id;
                                                //Get and Update Promotional Advert Data on DB Server
                                                var promotionalAdvertData = await db.PromotionalAdverts.FirstOrDefaultAsync(top => top.AdtoneServerPromotionalAdvertId == promotionalAdvertId);
                                                if (promotionalAdvertData != null)
                                                {
                                                    promotionalAdvertData.AdvertLocation = adName;
                                                    await db.SaveChangesAsync();
                                                }
                                            }

                                            await OnCampaignCreated(model.BatchID);
                                        }
                                    }
                                    #endregion

                                    TempData["success"] = "Campaign and Advert added successfully for operator " + operatorName + ".";
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    TempData["Error"] = operatorName + " operator implementation is under process.";
                                }
                            }
                        }
                        else TempData["Error"] = operatorName + " operator implementation is under process.";
                    }
                }
                await FillCountry(model.CountryId, model.OperatorId);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Index");
            }
        }

        private async Task OnCampaignCreated(int batchId)
        {
            //await ProvisionBucketsForBatch(batchId);
        }

        //To Update Promotional Campaign Data in DB Server
        [Route("UpdatePromotionalCampaign")]
        [HttpPost]
        public async Task<ActionResult> UpdatePromotionalCampaign(int id, int status)
        {
            try
            {
                var promotionalCampaign = _promotionalCampaignRepository.GetById(id);
                //Update Promotional Campaign Status Data on DB Server
                ChangePromotionalCampaignStatusCommand promotionalCampaignCommand =
                    Mapper.Map<PromotionalCampaign, ChangePromotionalCampaignStatusCommand>(promotionalCampaign);
                promotionalCampaignCommand.Status = status;
                ICommandResult promotionalCampaignResult = _commandBus.Submit(promotionalCampaignCommand);
                if (promotionalCampaignResult.Success)
                {
                    if (status == 1)
                    {
                        TempData["success"] = promotionalCampaign.CampaignName + " campaign status successfully changed to Play";
                    }
                    else
                    {
                        TempData["success"] = promotionalCampaign.CampaignName + " campaign status successfully changed to Stop";
                    }
                    return Json("success");
                }
                else
                {
                    TempData["Error"] = "Something went wrong.";
                    return Json("fail");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return Json("fail");
            }
        }

        //Convert File
        private void SaveConvertedFile(string audioFilePath, string extension, string operatorId, string fileName, string outputFormat)
        {
            var id = Convert.ToInt32(operatorId);
            string inputFormat = extension.Replace(".", "");

            CloudConvert api = new CloudConvert("WNdHFlLrT9GdETzjTJC4BoUsjE6tXbRi8sZX5aokQbua3D2hbJITOTylPs7Nre1A");
            var url = api.GetProcessURL(inputFormat, outputFormat);

            Dictionary<string, object> options = new Dictionary<string, object>()
            {
                { "audio_codec", "PCM MU-LAW" },
                { "audio_bitrate", "64 kbps" },
                { "audio_frequency", "8000" },
                { "audio_channels", "1" }
            };

            var convertedFile = api.UploadFileTemp(url, audioFilePath, outputFormat, null, options);
            var convertedMediaData = new JavaScriptSerializer().Deserialize<CloudConvertModel.RootObject>(convertedFile);

            using (WebClient webClient = new WebClient())
            {
                var downloadUrl = "http:" + convertedMediaData.output.url;
                string directoryName = Server.MapPath("~/PromotionalMedia/");
                directoryName = Path.Combine(directoryName, operatorId);

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                string savePath = Path.Combine(directoryName, fileName + "." + outputFormat);

                webClient.DownloadFile(downloadUrl, savePath);

                string archiveDirectoryName = Server.MapPath("~/PromotionalMedia/Archive/");

                if (!Directory.Exists(archiveDirectoryName))
                    Directory.CreateDirectory(archiveDirectoryName);

                string archivePath = Path.Combine(archiveDirectoryName, fileName + "." + outputFormat);

                System.IO.File.Copy(savePath, archivePath, true);

                System.IO.File.Delete(audioFilePath);
            }
        }

        //Transfer Advert File From Operator Server to Linux Server
        public static string CopyAdToOpeartorServer(string advertLocation, int operatorId, OperatorFTPDetail operatorFTPDetails)
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                var mediaFile = advertLocation;
                if (!string.IsNullOrEmpty(mediaFile))
                {
                    var getFTPdetails = operatorFTPDetails;
                    if (getFTPdetails != null)
                    {
                        var host = getFTPdetails.Host;
                        var port = Convert.ToInt32(getFTPdetails.Port);
                        var username = getFTPdetails.UserName;
                        var password = getFTPdetails.Password;
                        var localRoot = System.Web.HttpContext.Current.Server.MapPath("~/PromotionalMedia");
                        var ftpRoot = getFTPdetails.FtpRoot;

                        using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
                        {
                            client.Connect();
                            if (client.IsConnected)
                            {
                                var SourceFile = localRoot + @"\" + operatorId + @"\" + System.IO.Path.GetFileName(mediaFile);
                                var DestinationFile = ftpRoot + "/" + System.IO.Path.GetFileName(mediaFile);
                                var filestream = new FileStream(SourceFile, FileMode.Open);
                                client.UploadFile(filestream, DestinationFile, null);
                                filestream.Close();

                                //if (operatorId == (int)OperatorTableId.Safaricom) // Second File Transfer
                                //{
                                //    var adName = System.IO.Path.GetFileName(mediaFile);
                                //    var temp = adName.Split('.')[0];
                                //    var secondAdname = Convert.ToInt64(temp) + 1;

                                //    var SourceFile2 = localRoot + @"\" + @"\SecondAudioFile\" + secondAdname + ".wav";
                                //    var DestinationFile2 = ftpRoot + "/" + secondAdname + ".wav";
                                //    var filestream2 = new FileStream(SourceFile2, FileMode.Open);
                                //    client.UploadFile(filestream2, DestinationFile2, null);
                                //    filestream2.Close();
                                //}
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

        // Search Promotional Campaign
        [Route("SearchPromotionalCampaign")]
        public async Task<ActionResult> SearchPromotionalCampaign(int?[] OperatorId, int?[] BatchId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = await FillPromotionalCampaignResult(OperatorId, BatchId);
                return PartialView("_PromotionalCampaignDetails", result);
            }
            else
            {
                return PartialView("_PromotionalCampaignDetails", "notauthorise");
            }
        }

        //Fill County Drop Down List
        public async Task FillCountry(int countryId, int operatorId)
        {
            var clientdetails = await _countryRepository.AsQueryable().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToListAsync();
            ViewBag.countrydetails = new MultiSelectList(clientdetails, "Id", "Name");
            await FillOperator(countryId, operatorId);
        }

        //Fill Operator Drop Down List
        [HttpPost]
        [Route("FillOperator")]
        public async Task<ActionResult> FillOperator(int countryId, int operatorId)
        {
            if (countryId == 0)
            {
                var operatordetails = await _operatorRepository.AsQueryable().Where(top => top.IsActive == true).Select(top => new
                {
                    Name = top.OperatorName,
                    Id = top.OperatorId
                }).ToListAsync();
                ViewBag.operatordetails = new MultiSelectList(operatordetails, "Id", "Name");
            }
            else
            {
                var operatordetails = await _operatorRepository.AsQueryable().Where(top => top.CountryId == countryId && top.IsActive == true).Select(top => new
                {
                    Name = top.OperatorName,
                    Id = top.OperatorId
                }).ToListAsync();
                ViewBag.operatordetails = new MultiSelectList(operatordetails, "Id", "Name");
            }
            await FillBatchID(operatorId);
            return Json(ViewBag.operatordetails);
        }

        //Fill Batch ID Drop Down List
        [HttpPost]
        [Route("FillBatchID")]
        public async Task<ActionResult> FillBatchID(int operatorId)
        {
            var emptyResult = Enumerable.Range(0, 0).Select(i => new {Id = i, Name = i}).ToList();
            ViewBag.batchiddetails = new MultiSelectList(emptyResult, "Id", "Name");

            if (operatorId == 0)
                return Json(ViewBag.batchiddetails);

            var operatorConnString = ConnectionString.GetSingleConnectionStringByOperatorId(operatorId);

            if (string.IsNullOrEmpty(operatorConnString))
                return Json(ViewBag.batchiddetails);

            using (EFMVCDataContex db = new EFMVCDataContex(operatorConnString))
            {
                var promotionalUserBaychIDDetails = await db.PromotionalUsers.Where(top => top.Status == 1)
                    .Select(top => top.BatchID).Distinct().Select(top => new
                    {
                        Name = top,
                        Id = top
                    }).ToListAsync();

                ViewBag.batchiddetails = new MultiSelectList(promotionalUserBaychIDDetails, "Id", "Name");
                return Json(ViewBag.batchiddetails);
            }
        }
    }
}