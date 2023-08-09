using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using EFMVC.Data.Repositories;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Areas.Admin.SearchClass;
using EFMVC.Model;
using EFMVC.Domain.Commands;
using EFMVC.CommandProcessor.Command;
using EFMVC.Domain.Commands.Campaign;
using System.Configuration;
using EFMVC.Web.ViewModels;
using EFMVC.Web.Core.Models;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing;
using System.IO;
using EFMVC.Web.Common;
using EFMVC.Data;
using EFMVC.Web.Core.Extensions;
using RestSharp;
using System.Xml;
using EFMVC.Domain.CountryConnectionString;
using System.Globalization;
using EFMVC.Web.Models;

namespace EFMVC.Web.Areas.Admin.Controllers
{

    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("UserAdvert")]
    public class UserAdvertController : Controller
    {
        //
        // GET: /Admin/Advert/

        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        private readonly ICampaignAdvertRepository _campaignadvertRepository;

        private readonly ICampaignProfileRepository _campaignprofileRepository;
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;
        private readonly ICountryRepository _countryRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly IAdvertRejectionRepository _advertRejectionRepository;
        private readonly ICommandBus _commandBus;

        public UserAdvertController(ICommandBus commandBus, IAdvertRepository advertRepository, IUserRepository userRepository, IClientRepository clientRepository, ICampaignAdvertRepository campaignadvertRepository, ICampaignProfileRepository campaignprofileRepository, IAdvertRejectionRepository advertRejectionRepository, ICountryRepository countryRepository)
        {
            _commandBus = commandBus;
            _advertRepository = advertRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _campaignadvertRepository = campaignadvertRepository;
            _campaignprofileRepository = campaignprofileRepository;
            _advertRejectionRepository = advertRejectionRepository;
            _countryRepository = countryRepository;
        }

        [Route("Index")]
        [Route("{userId}")]
        public ActionResult Index(int? userId, int? id, int? status)
        {
            TempData["UserId"] = userId;
            TempData["Id"] = id;
            TempData["AdvertStatus"] = status;
            //List<UserAdvertResult> _result = GetAdvertResult(userId, id);
            List<UserAdvertResult> _result = new List<UserAdvertResult>();
            FillUserDropdown(userId);
            FillClientDropdown();
            FillAdvertDropdown(id);
            FillAdvertStatus(userId);
            UserAdvertFilter _filterCritearea = new UserAdvertFilter();
            return View(Tuple.Create(_result, _filterCritearea));
        }

        //Add 28-06-2019
        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<UserAdvertResult> _result = new List<UserAdvertResult>();
                IEnumerable<Advert> advert = null;
                DateTimeFormat dateTimeConvert = new DateTimeFormat();
                string status = string.Empty;
                ViewBag.SearchResult = false;
                var cnt = 10;
                int? userId = 0;
                int? id = 0;
                int? advertstatus = 0;
                if (TempData["UserId"] == null)
                {
                    userId = 0;
                }
                else
                {
                    userId = Convert.ToInt32(TempData["UserId"].ToString());
                }
                if (TempData["Id"] == null)
                {
                    id = 0;
                }
                else
                {
                    id = Convert.ToInt32(TempData["Id"].ToString());
                }
                if (TempData["AdvertStatus"] == null)
                {
                    advertstatus = 0;
                }
                else
                {
                    advertstatus = Convert.ToInt32(TempData["AdvertStatus"].ToString());
                }

                bool searchValue = false;
                List<String> columnSearch = new List<string>();

                if (param.Columns != null)
                {
                    foreach (var col in param.Columns)
                    {
                        columnSearch.Add(col.Search.Value);
                        if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                            searchValue = true;
                    }
                }

                if (searchValue == true)
                {
                    #region Search Functionality
                    int?[] ClientId = new int?[cnt];
                    int[] StatusId = new int[cnt];
                    int[] UserId = new int[cnt];
                    int[] AdvertId = new int[cnt];
                    DateTime CreatedDatefromdate = new DateTime();
                    DateTime CreatedDatetodate = new DateTime();

                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            UserId = columnSearch[0].Split(',').Select(a => (int)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[0] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[2]))
                    {
                        if (columnSearch[2] != "null")
                        {
                            ClientId = columnSearch[2].Split(',').Select(a => (int?)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[2] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[3]))
                    {
                        if (columnSearch[3] != "null")
                        {
                            AdvertId = columnSearch[3].Split(',').Select(a => (int)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[3] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[7]))
                    {
                        if (columnSearch[7] != "null")
                        {
                            StatusId = columnSearch[7].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[7] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[8]))
                    {
                        if (columnSearch[8] != "null")
                        {
                            var data = columnSearch[8].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[2] = null;
                        }
                    }

                    if (userId != null || userId != 0)
                    {
                        advert = _advertRepository.GetAll().Where(top => top.UserId == userId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.AdvertId).ToList();
                    }
                    else
                    {
                        advert = _advertRepository.GetAll().Where(top => top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.AdvertId).ToList();
                    }
                    if (id != null || id != 0)
                    {
                        advert = _advertRepository.GetAll().Where(top => top.AdvertId == id && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.AdvertId).ToList();
                    }
                    if (advertstatus != 0)
                    {
                        advert = _advertRepository.GetAll().Where(top => top.UserId == userId && top.Status == (int)AdvertStatus.Waitingforapproval).OrderByDescending(top => top.AdvertId).ToList();
                    }

                    if (columnSearch[0] != null)
                    {
                        advert = advert.Where(top => (UserId.Contains(Convert.ToInt32(top.UserId)))).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        advert = advert.Where(top => (ClientId.Contains(Convert.ToInt32(top.ClientId)))).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        advert = advert.Where(top => (AdvertId.Contains(Convert.ToInt32(top.AdvertId)))).ToList();
                    }
                    if (columnSearch[7] != null)
                    {
                        advert = advert.Where(top => (StatusId.Contains((int)top.Status))).ToList();
                    }
                    if (columnSearch[8] != null)
                    {
                        advert = advert.Where(top => (top.CreatedDateTime >= CreatedDatefromdate && top.CreatedDateTime <= CreatedDatetodate)).ToList();
                    }

                    cnt = advert.Count();
                    advert = advert.Skip(param.Start).Take(param.Length);

                    #endregion
                }
                else
                {
                    if (userId != 0)
                    {
                        advert = _advertRepository.GetAll().Where(top => top.UserId == userId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.AdvertId).ToList();
                    }
                    else
                    {
                        advert = _advertRepository.GetAll().Where(top => top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.AdvertId).ToList();
                    }
                    if (id != 0)
                    {
                        advert = _advertRepository.GetAll().Where(top => top.AdvertId == id && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.AdvertId).ToList();
                    }
                    if (advertstatus != 0)
                    {
                        advert = _advertRepository.GetAll().Where(top => top.UserId == userId && top.Status == (int)AdvertStatus.Waitingforapproval).OrderByDescending(top => top.AdvertId).ToList();
                    }
                    cnt = advert.Count();
                    advert = advert.Skip(param.Start).Take(param.Length);
                }

                foreach (var item in advert)
                {
                    // 16-10-2019
                    //var fstatus = "";
                    //if (item.Status != 1 && item.Status != 2 && item.Status != 3 && item.Status != 4)
                    //{
                    //    fstatus = "Rejected";
                    //    item.Status = (int)AdvertStatus.Rejected;
                    //}
                    //else
                    //{
                    //    var astatus = (AdvertStatus)item.Status;
                    //    fstatus = astatus.ToString();
                    //}
                    var fstatus = "";                   
                    var astatus = (AdvertStatus)item.Status;
                    fstatus = astatus.ToString();

                    _result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.ClientId == null ? "-" : item.Clients == null ? "-" : item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, fstatus = fstatus.ToString(), CreatedDate = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation });
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                TempData.Keep("UserId");
                TempData.Keep("Id");
                TempData.Keep("AdvertStatus");

                DTResult<UserAdvertResult> result = new DTResult<UserAdvertResult>
                {
                    draw = param.Draw,
                    data = _result,
                    recordsFiltered = cnt,
                    recordsTotal = cnt
                };

                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        //Add 28-06-2019
        // Function For Filter/Sorting Advert Data
        private static List<UserAdvertResult> ApplySorting(DTParameters param, List<UserAdvertResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Email).ToList();
                    else
                        result = result.OrderByDescending(top => top.Email).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.UserName).ToList();
                    else
                        result = result.OrderByDescending(top => top.UserName).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.ClientName).ToList();
                    else
                        result = result.OrderByDescending(top => top.ClientName).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Name).ToList();
                    else
                        result = result.OrderByDescending(top => top.Name).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Brand).ToList();
                    else
                        result = result.OrderByDescending(top => top.Brand).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.fstatus).ToList();
                    else
                        result = result.OrderByDescending(top => top.fstatus).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateSort).ToList();
                }
            }
            return result;
        }

        public void FillUserDropdown(int? userId)
        {
            if (userId != null)
            {
                var advertUser = _advertRepository.GetMany(s => s.UserId == userId).Select(s => s.UserId).ToList();
                var userdetails = _userRepository.GetMany(s => advertUser.Contains(s.UserId)).Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName,
                    UserId = top.UserId,
                }).ToList();
                ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
            }
            else
            {
                var advertUser = _advertRepository.GetAll().Select(s => s.UserId).ToList();
                var userdetails = _userRepository.GetMany(s => advertUser.Contains(s.UserId)).Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName,
                    UserId = top.UserId,
                }).ToList();
                ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
            }

        }

        //Code Commented on 10/09/2018
        //public void FillUserDropdown(int? userId)
        //{
        //    if (userId != null)
        //    {
        //        var userdetails = _userRepository.GetAll().Where(top => top.UserId == userId).Select(top => new
        //        {
        //            Name = top.FirstName + " " + top.LastName,
        //            UserId = top.UserId,
        //        }).ToList();
        //        ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
        //    }
        //    else
        //    {
        //        var userdetails = _userRepository.GetAll().Select(top => new
        //        {
        //            Name = top.FirstName + " " + top.LastName,
        //            UserId = top.UserId,
        //        }).ToList();
        //        ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
        //    }

        //}


        public void FillClientDropdown()
        {

            var clientdetails = _clientRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.client = new MultiSelectList(clientdetails, "Id", "Name");
        }
        public void FillAdvertDropdown(int? id)
        {
            if (id != null)
            {
                var advert_id = new[] { id };

                var advertdetails = _advertRepository.GetAll().Where(top => top.AdvertId == id).Select(top => new
                {
                    AdvertName = top.AdvertName,
                    AdvertId = top.AdvertId
                }).ToList();
                ViewBag.adverts = new MultiSelectList(advertdetails, "AdvertId", "AdvertName", advert_id);
            }
            else
            {
                var advertdetails = _advertRepository.GetAll().Select(top => new
                {
                    AdvertName = top.AdvertName,
                    AdvertId = top.AdvertId
                }).ToList();
                ViewBag.adverts = new MultiSelectList(advertdetails, "AdvertId", "AdvertName");
            }

        }
        public void FillAdvertStatus(int? userId)
        {
            IEnumerable<Common.AdvertStatus> advertTypes = Enum.GetValues(typeof(Common.AdvertStatus))
                                                     .Cast<Common.AdvertStatus>();
            var advertstatus = (from action in advertTypes
                                where action != AdvertStatus.Draft
                                select new
                                {
                                    Text = action.ToString(),
                                    Value = ((int)action).ToString()
                                }).ToList();
            if (userId != null)
            {
                //ViewBag.advertstatus = new MultiSelectList(advertstatus, "Value", "Text", new int[] { 4 });
                ViewBag.advertstatus = new MultiSelectList(advertstatus, "Value", "Text");
            }
            else
            {
                ViewBag.advertstatus = new MultiSelectList(advertstatus, "Value", "Text");
            }
        }
        private List<UserAdvertResult> GetAdvertResult(int? userId, int? id)
        {
            List<UserAdvertResult> _result = new List<UserAdvertResult>();
            List<Advert> adverts = new List<Advert>();
            if (userId != null)
            {
                adverts = _advertRepository.GetAll().Where(top => top.UserId == userId.Value && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.AdvertId).ToList();
                //adverts = _advertRepository.GetAll().Where(top => top.UserId == userId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
            }
            else
            {
                adverts = _advertRepository.GetAll().Where(top => top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.AdvertId).ToList();
            }
            if (id != null)
            {
                adverts = _advertRepository.GetAll().Where(top => top.AdvertId == id.Value && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.AdvertId).ToList();
            }
            foreach (var item in adverts)
            {
                var fstatus = "";
                // 16-10-2019

                //if (item.Status != 1 && item.Status != 2 && item.Status != 3 && item.Status != 4)
                //{
                //    fstatus = "Rejected";
                //    item.Status = (int)AdvertStatus.Rejected;
                //}
                //else
                //{
                //    var astatus = (AdvertStatus)item.Status;
                //    fstatus = astatus.ToString();
                //}

                var astatus = (AdvertStatus)item.Status;
                   fstatus = astatus.ToString();
                _result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.ClientId == null ? "-" : item.Clients == null ? "-" : item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, fstatus = fstatus.ToString(), CreatedDate = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation });
            }
            return _result;
        }
        [Route("SearchAdverts")]
        public ActionResult SearchAdverts([Bind(Prefix = "Item2")]SearchClass.UserAdvertFilter _filterCritearea, int[] UserId, int?[] ClientId, int[] AdvertId, int[] AdvertStatusId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<UserAdvertResult> _result = new List<UserAdvertResult>();
                var finalresult = new List<UserAdvertResult>();
                if (_filterCritearea != null)
                {
                    _result = GetAdvertResult(null, null);
                    finalresult = getadvertResult(_result, _filterCritearea, UserId, ClientId, AdvertId, AdvertStatusId);
                }
                else
                {
                    AdvertStatusId = new int[] { 1 };
                    _result = GetAdvertResult(null, null);
                    finalresult = getadvertResult(_result, _filterCritearea, UserId, ClientId, AdvertId, AdvertStatusId);
                }

                return PartialView("_UserAdvertDetails", finalresult);
            }
            else
            {
                return PartialView("_UserAdvertDetails", "notauthorise");
            }
        }

        [Route("RejectAdvert")]
        [HttpPost]
        public ActionResult RejectAdvert(int advertId, string rejectionReason)
        {
            ChangeAdvertStatusCommand command = new ChangeAdvertStatusCommand();
            command.AdvertId = advertId;

            command.Status = 5; // Rejection Status
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                CreateOrUpdateAdvertRejectionCommand command2 = new CreateOrUpdateAdvertRejectionCommand();
                command2.AdvertId = advertId;
                command2.UserId = efmvcUser.UserId;
                command2.RejectionReason = rejectionReason;
                command2.CreatedDate = DateTime.Now;
                ICommandResult result2 = _commandBus.Submit(command2);

                var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == advertId);
                var campaigndetails = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);


                EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                UserMatchTableProcess obj = new UserMatchTableProcess();
                obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, null, SQLServerEntities);
                PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, conn);

                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaigndetails.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        SQLServerEntities = new EFMVCDataContex(item);
                        obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, null, SQLServerEntities);
                        PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, item);
                    }
                }


                TempData["status"] = "Advert is rejected successfully.";
                return Json("success");
            }
            return Json("error");
        }

        [Route("ApproveORRejectAdvert")]
        [HttpPost]
        public ActionResult ApproveORRejectAdvert(int id, int status, int oldstatus)
        {

            ChangeAdvertStatusCommand command = new ChangeAdvertStatusCommand();
            //if (result.Success)
            //{
            if (status == 1) // Live
            {
                AdTransfer.CopyAdToOpeartorServer(id);
                //271191                
                var returnCode = SoapApiProcess.UploadSoapTone(id);
                //var returnCode = SoapApiProcess.UpdateToneSoapApi(id);
                if (returnCode == "000000")
                {
                    command.AdvertId = id;
                    command.Status = status;
                    ICommandResult result = _commandBus.Submit(command);
                    var mediaUrl = _advertRepository.GetById(id).MediaFileLocation;
                    var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == id);

                    if (oldstatus == 4 && campaignadvertId != null)
                    {
                        var camstatus = Changecampaignstatus(campaignadvertId.CampaignProfileId);
                        //if (camstatus)
                        //{
                        //    TempData["status"] = "Advert is approved successfully.";
                        //}                         

                    }

                    if (campaignadvertId != null)
                    {
                        var CampaignData = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);
                        var countryId = CampaignData.CountryId == null ? 0 : CampaignData.CountryId;

                        //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                        //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                        //UserMatchTableProcess obj = new UserMatchTableProcess();
                        //obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, mediaUrl, SQLServerEntities);
                        //PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, conn);

                        var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                        if (ConnString != null && ConnString.Count() > 0)
                        {
                            UserMatchTableProcess obj = new UserMatchTableProcess();
                            foreach (var item in ConnString)
                            {
                                EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                                var campaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignadvertId.CampaignProfileId).FirstOrDefault();
                                if (campaigndetails != null)
                                {
                                    obj.UpdateMediaFileLocation(campaigndetails.CampaignProfileId, mediaUrl, SQLServerEntities);
                                    PreMatchProcess.PrematchProcessForCampaign(campaigndetails.CampaignProfileId, item);
                                }

                            }
                        }
                    }

                    var rejectionList = _advertRejectionRepository.GetMany(s => s.AdvertId == id).ToList();
                    foreach (var item in rejectionList)
                    {
                        var delteAdReasoncommand = new DeleteAdvertRejectionCommand { Id = item.AdvertRejectionId };
                        ICommandResult commandResult = _commandBus.Submit(delteAdReasoncommand);
                    }

                    TempData["status"] = "Advert is approved successfully.";
                }
                else
                {
                    //Generate Ticket
                }


            }
            else if (status == 2) // suspended
            {
                command.AdvertId = id;
                command.Status = status;
                ICommandResult result = _commandBus.Submit(command);

                var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == id);
                var campaigndetails = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);


                //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                //UserMatchTableProcess obj = new UserMatchTableProcess();
                //obj.UpdateMediaFileLocation(campaignadvertId.CampaignProfileId, null, SQLServerEntities);
                //PreMatchProcess.PrematchProcessForCampaign(campaignadvertId.CampaignProfileId, conn);

                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaigndetails.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    UserMatchTableProcess obj = new UserMatchTableProcess();
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                        var OperatorCampaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignadvertId.CampaignProfileId).FirstOrDefault();
                        if (OperatorCampaigndetails != null)
                        {
                            obj.UpdateMediaFileLocation(OperatorCampaigndetails.CampaignProfileId, null, SQLServerEntities);
                            PreMatchProcess.PrematchProcessForCampaign(OperatorCampaigndetails.CampaignProfileId, item);
                        }

                    }
                }

                TempData["status"] = "Advert is suspended successfully.";
            }
            else if (status == 3) // Archived(Deleted)
            {
                command.AdvertId = id;
                command.Status = status;
                ICommandResult result = _commandBus.Submit(command);
                EFMVCDataContex db = new EFMVCDataContex();
                var toneId = db.Adverts.Where(s => s.AdvertId == id).FirstOrDefault().SoapToneId;

                //271191
                //var returnCode = SoapApiProcess.DeleteSoapTone(toneId);
                if (toneId != null)
                {
                    var returnCode = SoapApiProcess.DeleteToneSoapApi(id);
                }
                var campaignAdvertData = db.CampaignAdverts.Where(s => s.AdvertId == id).ToList();
                if (campaignAdvertData.Count() > 0)
                {
                    var campProfileId = campaignAdvertData.FirstOrDefault().CampaignProfileId;
                    var CampaignData = _campaignprofileRepository.GetById(campProfileId);
                    var countryId = CampaignData.CountryId == null ? 0 : CampaignData.CountryId;

                    //EFMVCDataContex SQLServerEntities = new EFMVCDataContex();
                    //var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
                    //UserMatchTableProcess obj = new UserMatchTableProcess();
                    //obj.UpdateMediaFileLocation(campProfileId, null, SQLServerEntities);
                    //PreMatchProcess.PrematchProcessForCampaign(campProfileId, conn);

                    var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        UserMatchTableProcess obj = new UserMatchTableProcess();
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                            var OperatorCampaigndetails = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campProfileId).FirstOrDefault();
                            if (OperatorCampaigndetails != null)
                            {
                                obj.UpdateMediaFileLocation(OperatorCampaigndetails.CampaignProfileId, null, SQLServerEntities);
                                PreMatchProcess.PrematchProcessForCampaign(OperatorCampaigndetails.CampaignProfileId, item);
                            }

                        }
                    }

                }

                TempData["status"] = "Advert is archived successfully.";
            }
            else if (status == 5)
            {
                command.AdvertId = id;
                command.Status = status;
                ICommandResult result = _commandBus.Submit(command);
                TempData["status"] = "Advert is rejected successfully.";
            }
            return Json("success");
            //}
            //return Json("error");
        }

        public bool Changecampaignstatus(int campaignId)
        {
            bool status = false;
            //update campaign status from ad approval to the planned
            ChangeCampaignStatusCommand _campaignstatus = new ChangeCampaignStatusCommand();
            _campaignstatus.CampaignProfileId = campaignId;
            //check campaign details
            var campaigndetails = _campaignprofileRepository.Get(top => top.CampaignProfileId == campaignId);
            if (campaigndetails != null)
            {
                if (campaigndetails.StartDate == null && campaigndetails.EndDate == null)
                {
                    _campaignstatus.Status = (int)CampaignStatus.Play;
                }
                else
                {
                    if (campaigndetails.StartDate != null)
                    {
                        if (campaigndetails.StartDate.Value.Date == DateTime.Now.Date)
                        {
                            _campaignstatus.Status = (int)CampaignStatus.Play;
                        }
                        else
                        {
                            _campaignstatus.Status = (int)CampaignStatus.Planned;
                        }
                    }
                    else
                    {
                        _campaignstatus.Status = (int)CampaignStatus.Planned;
                    }
                }
            }
            ICommandResult campaignstatuscommandResult = _commandBus.Submit(_campaignstatus);
            if (campaignstatuscommandResult.Success)
            {
                status = true;

            }
            return status;
        }

        public List<UserAdvertResult> getadvertResult(List<UserAdvertResult> advertresult, SearchClass.UserAdvertFilter _filterCritearea, int[] UserId, int?[] ClientId, int[] AdvertId, int[] AdvertStatusId)
        {
            if (advertresult != null && _filterCritearea != null)
            {

                if (UserId != null)
                {
                    advertresult = advertresult.Where(top => UserId.Contains(top.userId)).ToList();
                }
                if (ClientId != null)
                {
                    advertresult = advertresult.Where(top => ClientId.Contains(top.ClientId)).ToList();
                }
                if (AdvertId != null)
                {
                    advertresult = advertresult.Where(top => AdvertId.Contains(top.AdvertId)).ToList();
                }
                if (AdvertStatusId != null)
                {
                    advertresult = advertresult.Where(top => AdvertStatusId.Contains(top.status)).ToList();
                }
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    //advertresult = advertresult.Where(top => top.CreatedDate.Value.Date >= _filterCritearea.Fromdate.Value.Date && top.CreatedDate.Value.Date <= _filterCritearea.Todate.Value.Date).ToList();
                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    advertresult = advertresult.Where(top => top.CreatedDateSort.Value.Date >= Fromdate && top.CreatedDateSort.Value.Date <= Todate).ToList();
                }
            }
            else
            {
                if (UserId != null)
                {
                    advertresult = advertresult.Where(top => UserId.Contains(top.userId)).ToList();
                }
                //if (AdvertStatusId != null)
                //{
                //    //advertresult = advertresult.Where(top => AdvertStatusId.Contains(top.status)).ToList();
                //    advertresult = advertresult.ToList();
                //}
            }
            return advertresult;
        }
        [Route("GetClientsUser")]
        [HttpPost]
        public ActionResult GetClientsUser(int[] userId)
        {
            try
            {
                if (userId != null && userId.Any(s => s != 0))
                {

                    var clientdetails = _clientRepository.GetAll().Where(top => userId.Contains((int)(top.UserId))).Select(top => new
                    {
                        Name = top.Name,
                        Id = top.Id
                    }).ToList();
                    return Json(clientdetails);

                }
                else
                {
                    var clientdetails = _clientRepository.GetAll().Select(top => new
                    {
                        Name = top.Name,
                        Id = top.Id
                    }).ToList();
                    return Json(clientdetails);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }
        [Route("GetClientsAdvert")]
        [HttpPost]
        public ActionResult GetClientsAdvert(int[] clientId)
        {
            try
            {
                if (clientId != null)
                {
                    var advertdetails = _advertRepository.GetAll().Where(top => clientId.Contains((int)(top.ClientId == null ? 0 : top.ClientId))).Select(top => new
                    {
                        Name = top.AdvertName,
                        Id = top.AdvertId
                    }).ToList();
                    return Json(advertdetails);
                }
                else
                {
                    var advertdetails = _advertRepository.GetAll().Select(top => new
                    {
                        Name = top.AdvertName,
                        Id = top.AdvertId
                    }).ToList();
                    return Json(advertdetails);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        //Add 05-02-2019 For Fill Advert DropDown
        [Route("GetAdvert")]
        [HttpPost]
        public ActionResult GetAdvert()
        {
            try
            {
                var advertdetails = _advertRepository.GetAll().Select(top => new
                {
                    Name = top.AdvertName,
                    Id = top.AdvertId
                }).ToList();
                return Json(advertdetails);
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        [Route("AdvertDetails")]
        [Route("{id}/{userid}")]
        [HttpGet]
        public ActionResult AdvertDetails(int id, int userid)
        {

            User user = _userRepository.GetById(userid);
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == userid);
            IEnumerable<ClientModel> clientModels =
                Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillAddClient(clientModels);
            FillCountry();
            if (_advertRepository.Count(x => x.AdvertId == id && x.UserId == userid) == 0)
                return RedirectToAction("Index");

            Advert advert = _advertRepository.GetById(id);

            if (advert != null)
            {
                FillAddStatus(id, advert.Status);
                //AdvertFormModel model = Mapper.Map<Advert, AdvertFormModel>(advert);
                AdminAdvertFormModel model = Mapper.Map<Advert, AdminAdvertFormModel>(advert);
                ViewBag.advert = model.AdvertName;
                ViewBag.medialocation = @"~" + model.MediaFileLocation;
                if (!string.IsNullOrEmpty(model.ScriptFileLocation))
                {
                    ViewBag.scriptlocation = System.Configuration.ConfigurationManager.AppSettings["siteAddress"] + model.ScriptFileLocation;
                }
                else
                {
                    ViewBag.scriptlocation = String.Empty;
                }
                ViewBag.selectedadvertstatus = advert.Status;
                return View("AdvertDetails", model);
            }
            return RedirectToAction("Index");

        }
        public void FillCountry()
        {
            var clientdetails = _countryRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.country = new MultiSelectList(clientdetails, "Id", "Name");

        }
        private void FillAddClient(IEnumerable<ClientModel> clientModels)
        {
            var clientdetails = clientModels.Select(top => new SelectListItem
            {
                Text = top.Name,
                Value = top.Id.ToString(),
            });
            var client = clientdetails.ToList();
            ViewBag.client = client;
        }
        public void FillAddStatus(int? id, int? status)
        {

            if (id != null)
            {
                if (status != 4)
                {
                    IEnumerable<Common.AdvertStatus> advertstatusTypes = Enum.GetValues(typeof(Common.AdvertStatus))
                                                       .Cast<Common.AdvertStatus>();
                    var advertStatus = (from action in advertstatusTypes
                                        where (int)action != 4 && (int)action != 5
                                        select new SelectListItem
                                        {
                                            Text = action.ToString(),
                                            Value = ((int)action).ToString()
                                        }).ToList();
                    ViewBag.advertStatus = advertStatus;
                }
                else
                {
                    IEnumerable<Common.AdvertStatus> advertstatusTypes = Enum.GetValues(typeof(Common.AdvertStatus))
                                                       .Cast<Common.AdvertStatus>();
                    var advertStatus = (from action in advertstatusTypes
                                        select new SelectListItem
                                        {
                                            Text = action.ToString(),
                                            Value = ((int)action).ToString()
                                        }).ToList();
                    ViewBag.advertStatus = advertStatus;

                }

            }
            else
            {
                IEnumerable<Common.AdvertStatus> advertstatusTypes = Enum.GetValues(typeof(Common.AdvertStatus))
                                                        .Cast<Common.AdvertStatus>();
                var advertStatus = (from action in advertstatusTypes
                                    select new SelectListItem
                                    {
                                        Text = action.ToString(),
                                        Value = ((int)action).ToString()
                                    }).ToList();
                ViewBag.advertStatus = advertStatus;

            }


        }
        [Route("UpdateAdvert")]
        [HttpPost]
        public ActionResult UpdateAdvert(AdminAdvertFormModel _model)//AdvertFormModel 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.Files.Count != 0)
                    {
                        HttpPostedFileBase file = Request.Files[0];

                        if (file.ContentLength != 0)
                        {
                            string fileName = Guid.NewGuid().ToString();
                            string extension = System.IO.Path.GetExtension(file.FileName);

                            string directoryName = Server.MapPath("~/Media/");
                            directoryName = System.IO.Path.Combine(directoryName, _model.UserId.ToString());

                            if (!Directory.Exists(directoryName))
                                Directory.CreateDirectory(directoryName);

                            string path = System.IO.Path.Combine(directoryName, fileName + extension);
                            file.SaveAs(path);

                            string archiveDirectoryName = Server.MapPath("~/Media/Archive/");

                            if (!Directory.Exists(archiveDirectoryName))
                                Directory.CreateDirectory(archiveDirectoryName);

                            string archivePath = System.IO.Path.Combine(archiveDirectoryName, fileName + extension);
                            file.SaveAs(archivePath);

                            _model.MediaFileLocation = string.Format("/Media/{0}/{1}", _model.UserId.ToString(),
                                                                    fileName + extension);
                        }
                        HttpPostedFileBase scriptfile = Request.Files[1];

                        if (scriptfile.ContentLength != 0)
                        {
                            string fileName = Guid.NewGuid().ToString();
                            string extension = System.IO.Path.GetExtension(scriptfile.FileName);

                            string directoryName = Server.MapPath("~/Script/");
                            directoryName = System.IO.Path.Combine(directoryName, _model.UserId.ToString());

                            if (!Directory.Exists(directoryName))
                                Directory.CreateDirectory(directoryName);

                            string path = System.IO.Path.Combine(directoryName, fileName + extension);
                            scriptfile.SaveAs(path);

                            string archiveDirectoryName = Server.MapPath("~/Script/Archive/");

                            if (!Directory.Exists(archiveDirectoryName))
                                Directory.CreateDirectory(archiveDirectoryName);

                            string archivePath = System.IO.Path.Combine(archiveDirectoryName, fileName + extension);
                            scriptfile.SaveAs(archivePath);

                            _model.ScriptFileLocation = string.Format("/Script/{0}/{1}", _model.UserId.ToString(),
                                                                    fileName + extension);
                        }
                    }


                    if (_model.AdvertId == 0)
                        _model.CreatedDateTime = DateTime.Now;

                    _model.UpdatedDateTime = DateTime.Now;
                    _model.UserId = _model.UserId;

                    //CreateOrUpdateAdvertCommand command = Mapper.Map<AdvertFormModel, CreateOrUpdateAdvertCommand>(_model);
                    CreateOrUpdateAdvertCommand command = Mapper.Map<AdminAdvertFormModel, CreateOrUpdateAdvertCommand>(_model);

                    if (ModelState.IsValid)
                    {
                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            //TempData["msgsuccess"] = "Record updated successfully.";
                            TempData["msgsuccess"] = "Advert " + _model.AdvertName + " updated successfully.";
                            //return RedirectToAction("Index");
                            return RedirectToAction("AdvertDetails", new { @id = Convert.ToInt32(_model.AdvertId), @userid = Convert.ToInt32(_model.UserId) });
                        }

                    }
                    return View("Index");

                }
                //return View(_model);
                return RedirectToAction("AdvertDetails", new { @id = Convert.ToInt32(_model.AdvertId), @userid = Convert.ToInt32(_model.UserId) });
            }
            catch (Exception ex)
            {
                return RedirectToAction("AdvertDetails", new { @id = Convert.ToInt32(_model.AdvertId), @userid = Convert.ToInt32(_model.UserId) });
            }
        }

        [Route("GetRejectedReasonList")]
        public ActionResult GetRejectedReasonList(int advertId)
        {
            var rejectionList = _advertRejectionRepository.GetMany(s => s.AdvertId == advertId).ToList();
            if (rejectionList.Count() > 0)
            {
                return PartialView("_AdvertRejectionList", rejectionList);
            }
            return Json("False");
        }


        //private void CopyAdToOpeartorServer(int advertId)
        //{
        //    try
        //    {
        //        EFMVCDataContex db = new EFMVCDataContex();
        //        var advert = db.Adverts.Where(s => s.AdvertId == advertId).FirstOrDefault();
        //        var mediaFile = advert.MediaFileLocation;
        //        if (!string.IsNullOrEmpty(mediaFile) && advert.UploadedToMediaServer == false)
        //        {
        //            // Test FTP Details
        //            //var host = "138.68.177.47";
        //            //var port = 22;
        //            //var username = "provisioning";
        //            //var password = "adtonespassword";
        //            //var localRoot = Server.MapPath("~/Media");
        //            //var ftpRoot = "/usr/local/arthar/adds";

        //            //Live FTP Details
        //            var host = "sftp://10.5.46.46";
        //            var port = 22;
        //            var username = "usdpuser";
        //            var password = "Huawei_123";
        //            var localRoot = Server.MapPath("~/Media");
        //            var ftpRoot = "/mnt/Y:/share";
        //            using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
        //            {
        //                client.Connect();
        //                if (client.IsConnected)
        //                {
        //                    var SourceFile = localRoot + @"\" + advert.UserId + @"\" + System.IO.Path.GetFileName(advert.MediaFileLocation);
        //                    var DestinationFile = ftpRoot + "/" + System.IO.Path.GetFileName(advert.MediaFileLocation);
        //                    var filestream = new FileStream(SourceFile, FileMode.Open);
        //                    client.UploadFile(filestream, DestinationFile);
        //                    advert.UploadedToMediaServer = true;
        //                    db.SaveChanges();
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //throw;
        //    }


        //}

    }
}
