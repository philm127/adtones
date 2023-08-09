using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.Campaign;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using EFMVC.Web.Areas.OperatorAdmin.Models;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Models;
using EFMVC.Web.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace EFMVC.Web.Areas.OperatorAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "OperatorAdmin")]
    [RouteArea("OperatorAdmin")]
    [RoutePrefix("UserAdvert")]
    public class UserAdvertController : Controller
    {
        private readonly IAdvertRepository _advertRepository;

        private readonly ICampaignAdvertRepository _campaignadvertRepository;

        private readonly ICampaignProfileRepository _campaignprofileRepository;
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        private readonly ISoapApiResponseCodeRepository _soapApiResponseCodeRepository;
        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly IAdvertRejectionRepository _advertRejectionRepository;

        /// <summary>
        /// The _billing repository
        /// </summary>
        private readonly IBillingRepository _billingRepository;
        private readonly IContactsRepository _contactsRepository;

        private readonly ICommandBus _commandBus;

        public UserAdvertController(ICommandBus commandBus, IAdvertRepository advertRepository, IUserRepository userRepository, IClientRepository clientRepository, ICampaignAdvertRepository campaignadvertRepository, ICampaignProfileRepository campaignprofileRepository, IAdvertRejectionRepository advertRejectionRepository, ISoapApiResponseCodeRepository soapApiResponseCodeRepository, IBillingRepository billingRepository, IContactsRepository _contactsRepository)
        {
            _commandBus = commandBus;
            _advertRepository = advertRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _campaignadvertRepository = campaignadvertRepository;
            _campaignprofileRepository = campaignprofileRepository;
            _advertRejectionRepository = advertRejectionRepository;
            _soapApiResponseCodeRepository = soapApiResponseCodeRepository;
            _billingRepository = billingRepository;
            _contactsRepository = _contactsRepository;
        }
        // GET: OperatorAdmin/UserAdvert

        [Route("Index")]
        [Route("{userId}")]
        public ActionResult Index(int? userId, int? id)
        {
            //Add 16-04-2019
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var operatorAdminId = efmvcUser.UserId;
            ViewBag.UserId = efmvcUser.UserId;
            //List<UserAdvertResult> _result = GetAdvertResult(userId, id, Convert.ToInt32(operatorAdminId));
            List<UserAdvertResult> _result = new List<UserAdvertResult>();

            //Comment 16-04-2019
            //List<UserAdvertResult> _result = GetAdvertResult(userId, id);
            //EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            FillUserDropdown(userId, Convert.ToInt32(operatorAdminId));
            FillClientDropdown(Convert.ToInt32(operatorAdminId));
            FillAdvertDropdown(id, Convert.ToInt32(operatorAdminId));
            FillAdvertStatus(userId);
            TempData["UserId"] = userId;
            TempData["Id"] = id;
            UserAdvertFilter _filterCritearea = new UserAdvertFilter();
            return View(Tuple.Create(_result, _filterCritearea));
        }

        //Add 02-07-2019
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
                int userId = 0;
                int id = 0;

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

                var operatorAdminId = efmvcUser.UserId;
                var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;
                var userIdData = _userRepository.GetMany(top => top.OperatorId == operatorId && top.RoleId == 3).Select(top => top.UserId).ToList();

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
                    int[] UserId = new int[cnt];
                    int[] CountryId = new int[cnt];
                    int[] OperatorId = new int[cnt];
                    int?[] ClientId = new int?[cnt];
                    int[] AdvertId = new int[cnt];
                    int[] StatusId = new int[cnt];
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

                    if (!String.IsNullOrEmpty(columnSearch[9]))
                    {
                        if (columnSearch[9] != "null")
                        {
                            StatusId = columnSearch[4].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[9] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[10]))
                    {
                        if (columnSearch[10] != "null")
                        {
                            var data = columnSearch[10].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[10] = null;
                        }
                    }

                    if (userId != 0)
                    {
                        advert = _advertRepository.GetAll().Where(top => top.UserId == userId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
                    }
                    else
                    {
                        advert = _advertRepository.GetAll().Where(top => top.OperatorId == operatorId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
                    }
                    if (id != 0)
                    {
                        advert = _advertRepository.GetAll().Where(top => top.OperatorId == operatorId && top.AdvertId == id && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
                    }
                    foreach (var item in advert)
                    {
                        var fstatus = string.Empty;
                        if (item.Status == 8)
                        {
                            fstatus = "Campaign Paused Due To Insufficient Funds";
                        }
                        //else if (item.Status == (int)AdvertStatus.Pending)
                        //{
                        //    fstatus = "Waitingforapproval";
                        //}
                        else
                        {
                            fstatus = ((AdvertStatus)item.Status).ToString();
                        }

                        var campaignAdvert = _campaignadvertRepository.Get(top => top.AdvertId == item.AdvertId);
                        if (campaignAdvert != null)
                        {
                            var campaignProfile = _campaignprofileRepository.Get(top => top.CampaignProfileId == campaignAdvert.CampaignProfileId);
                            _result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.ClientId == null ? "-" : item.Clients == null ? "-" : item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, fstatus = fstatus, CreatedDate = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation, SMSbody = campaignProfile.SmsBody == null ? "-" : campaignProfile.SmsBody, Emailbody = campaignProfile.EmailBody == null ? "-" : campaignProfile.EmailBody, UpdatedBy = item.UpdatedBy });
                        }
                        else
                        {
                            _result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.ClientId == null ? "-" : item.Clients == null ? "-" : item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, fstatus = fstatus, CreatedDate = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation, SMSbody = "-", Emailbody = "-", UpdatedBy = item.UpdatedBy });
                        }
                    }

                    if (columnSearch[0] != null)
                    {
                        _result = _result.Where(top => (UserId.Contains(Convert.ToInt32(top.userId)))).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        _result = _result.Where(top => (ClientId.Contains(Convert.ToInt32(top.ClientId)))).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        _result = _result.Where(top => (AdvertId.Contains(Convert.ToInt32(top.AdvertId)))).ToList();
                    }
                    if (columnSearch[9] != null)
                    {
                        _result = _result.Where(top => (StatusId.Contains((int)top.status))).ToList();
                    }
                    if (columnSearch[10] != null)
                    {
                        _result = _result.Where(top => (top.CreatedDateSort >= CreatedDatefromdate && top.CreatedDateSort <= CreatedDatetodate)).ToList();
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();

                    #endregion
                }
                else
                {
                    if (userId != 0)
                    {
                        advert = _advertRepository.GetAll().Where(top => top.UserId == userId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
                    }
                    else
                    {
                        advert = _advertRepository.GetAll().Where(top => top.OperatorId == operatorId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
                    }
                    if (id != 0)
                    {
                        advert = _advertRepository.GetAll().Where(top => top.OperatorId == operatorId && top.AdvertId == id && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
                    }
                    foreach (var item in advert)
                    {
                        var fstatus = string.Empty;
                        if (item.Status == 8)
                        {
                            fstatus = "Campaign Paused Due To Insufficient Funds";
                        }
                        //else if (item.Status == (int)AdvertStatus.Pending)
                        //{
                        //    fstatus = "Waitingforapproval";
                        //}
                        else
                        {
                            fstatus = ((AdvertStatus)item.Status).ToString();
                        }

                        var campaignAdvert = _campaignadvertRepository.Get(top => top.AdvertId == item.AdvertId);
                        if (campaignAdvert != null)
                        {
                            var campaignProfile = _campaignprofileRepository.Get(top => top.CampaignProfileId == campaignAdvert.CampaignProfileId);
                            _result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.ClientId == null ? "-" : item.Clients == null ? "-" : item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, fstatus = fstatus, CreatedDate = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation, SMSbody = campaignProfile.SmsBody == null ? "-" : campaignProfile.SmsBody, Emailbody = campaignProfile.EmailBody == null ? "-" : campaignProfile.EmailBody, UpdatedBy = item.UpdatedBy });
                        }
                        else
                        {
                            _result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.ClientId == null ? "-" : item.Clients == null ? "-" : item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, fstatus = fstatus, CreatedDate = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation, SMSbody = "-", Emailbody = "-", UpdatedBy = item.UpdatedBy });
                        }
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

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

        //Add 02-07-2019
        // Function For Filter/Sorting Advert Data
        private static List<UserAdvertResult> ApplySorting(DTParameters param, List<UserAdvertResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.UserName).ToList();
                    else
                        result = result.OrderByDescending(top => top.UserName).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Name).ToList();
                    else
                        result = result.OrderByDescending(top => top.Name).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.status).ToList();
                    else
                        result = result.OrderByDescending(top => top.status).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.SMSbody).ToList();
                    else
                        result = result.OrderByDescending(top => top.SMSbody).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.ClientName).ToList();
                    else
                        result = result.OrderByDescending(top => top.ClientName).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Brand).ToList();
                    else
                        result = result.OrderByDescending(top => top.Brand).ToList();
                }
                else if (paramOrderDetails.Column == 9)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Emailbody).ToList();
                    else
                        result = result.OrderByDescending(top => top.Emailbody).ToList();
                }
                else if (paramOrderDetails.Column == 10)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Email).ToList();
                    else
                        result = result.OrderByDescending(top => top.Email).ToList();
                }
                else if (paramOrderDetails.Column == 11)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateSort).ToList();
                }
            }
            return result;
        }

        public void FillUserDropdown(int? userId, int operatorAdminId)
        {
            //Add 16-04-2019
            var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;

            if (userId != null)
            {
                var advertUser = _advertRepository.GetAll().Where(s => s.UserId == userId).Select(s => s.UserId).ToList();
                var userdetails = _userRepository.GetAll().Where(s => advertUser.Contains(s.UserId) && s.OperatorId == operatorId && (s.RoleId == 2 || s.RoleId == 3)).Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName,
                    UserId = top.UserId,
                }).ToList();
                ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
            }
            else
            {
                var advertUser = _advertRepository.GetAll().Select(s => s.UserId).ToList();
                var userdetails = _userRepository.GetAll().Where(s => advertUser.Contains(s.UserId) && s.OperatorId == operatorId && (s.RoleId == 2 || s.RoleId == 3)).Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName,
                    UserId = top.UserId,
                }).Take(1).ToList();
                ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
            }

        }

        public void FillClientDropdown(int operatorAdminId)
        {
            //Add 17-04-2019
            var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;
            var userIdData = _userRepository.GetAll().Where(top => top.OperatorId == operatorId).Select(top => top.UserId).ToList();

            var clientdetails = _clientRepository.GetAll().Where(top => userIdData.Contains((int)top.UserId)).Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.client = new MultiSelectList(clientdetails, "Id", "Name");
        }
        public void FillAdvertDropdown(int? id, int operatorAdminId)
        {
            //Add 16-04-2019
            var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;

            if (id != null)
            {
                var advert_id = new[] { id };
                //var userIdDetails = _userRepository.GetMany(s => s.OperatorId == operatorId && s.RoleId == 3).Select(top => top.UserId).ToList();
                var advertdetails = _advertRepository.GetAll().Where(top => top.AdvertId == id && top.OperatorId == operatorId && top.Status != (int)AdvertStatus.Draft).Select(top => new
                {
                    AdvertName = top.AdvertName,
                    AdvertId = top.AdvertId
                }).ToList();
                ViewBag.adverts = new MultiSelectList(advertdetails, "AdvertId", "AdvertName", advert_id);
            }
            else
            {
                //var userIdDetails = _userRepository.GetMany(s => s.OperatorId == operatorId && s.RoleId == 3).Select(top => top.UserId).ToList();
                var advertdetails = _advertRepository.GetAll().Where(top => top.OperatorId == operatorId && top.Status != (int)AdvertStatus.Draft).Select(top => new
                {
                    AdvertName = top.AdvertName,
                    AdvertId = top.AdvertId
                }).ToList();
                ViewBag.adverts = new MultiSelectList(advertdetails, "AdvertId", "AdvertName");
            }

        }

        [Route("GetUsersAdvert")]
        [HttpPost]
        public ActionResult GetUsersAdvert(int[] userId)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var operatorAdminId = efmvcUser.UserId;
                var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;
                if (userId != null)
                {
                    var advertdetails = _advertRepository.GetAll().Where(top => top.OperatorId == operatorId && userId.Contains((int)(top.UserId))).Select(top => new
                    {
                        Name = top.AdvertName,
                        Id = top.AdvertId
                    }).ToList();
                    return Json(advertdetails);
                }
                else
                {
                    var advertdetails = _advertRepository.GetAll().Where(top => top.OperatorId == operatorId).Select(top => new
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
                ViewBag.advertstatus = new MultiSelectList(advertstatus, "Value", "Text", new int[] { 4 });
            }
            else
            {
                ViewBag.advertstatus = new MultiSelectList(advertstatus, "Value", "Text");
            }
        }

        //Comment 05-03-2019
        //private List<UserAdvertResult> GetAdvertResult(int? userId, int? id)
        //{
        //    List<UserAdvertResult> _result = new List<UserAdvertResult>();
        //    List<Advert> adverts = new List<Advert>();
        //    if (userId != null)
        //    {
        //        adverts = _advertRepository.GetAll().Where(top => top.UserId == userId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
        //    }
        //    else
        //    {
        //        adverts = _advertRepository.GetAll().Where(top => top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
        //    }
        //    if (id != null)
        //    {
        //        adverts = _advertRepository.GetAll().Where(top => top.AdvertId == id && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
        //    }
        //    foreach (var item in adverts)
        //    {
        //        _result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, CreatedDate = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation });
        //    }
        //    return _result;
        //}

        //Add 05-03-2019
        private List<UserAdvertResult> GetAdvertResult(int? userId, int? id, int operatorAdminId)
        {
            List<UserAdvertResult> _result = new List<UserAdvertResult>();
            List<Advert> adverts = new List<Advert>();

            //Add 16-04-2019
            var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;
            var userIdData = _userRepository.GetMany(top => top.OperatorId == operatorId && top.RoleId == 3).Select(top => top.UserId).ToList();

            if (userId != null)
            {
                adverts = _advertRepository.GetAll().Where(top => top.UserId == userId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
            }
            else
            {
                //adverts = _advertRepository.GetMany(top=> top.OperatorId == operatorId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
                adverts = _advertRepository.GetAll().Where(top => top.OperatorId == operatorId && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
            }
            if (id != null)
            {
                adverts = _advertRepository.GetAll().Where(top => top.OperatorId == operatorId && top.AdvertId == id && top.Status != (int)AdvertStatus.Draft).OrderByDescending(top => top.CreatedDateTime).ThenByDescending(top => top.Status).ToList();
            }
            foreach (var item in adverts)
            {
                var fstatus = string.Empty;
                if (item.Status == 8)
                {
                    fstatus = "Campaign Paused Due To Insufficient Funds";
                }
                //else if (item.Status == (int)AdvertStatus.Pending)
                //{
                //    fstatus = "Waitingforapproval";
                //}
                else
                {
                    fstatus = ((AdvertStatus)item.Status).ToString();
                }

                var campaignAdvert = _campaignadvertRepository.Get(top => top.AdvertId == item.AdvertId);
                if (campaignAdvert != null)
                {
                    var campaignProfile = _campaignprofileRepository.Get(top => top.CampaignProfileId == campaignAdvert.CampaignProfileId);
                    //_result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, CreatedDate = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation, SMSbody = campaignProfile.SmsBody == null ? "-" : campaignProfile.SmsBody, Emailbody = campaignProfile.EmailBody == null ? "-" : campaignProfile.EmailBody });
                    _result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.ClientId == null ? "-" : item.Clients == null ? "-" : item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, fstatus = fstatus, CreatedDate = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation, SMSbody = campaignProfile.SmsBody == null ? "-" : campaignProfile.SmsBody, Emailbody = campaignProfile.EmailBody == null ? "-" : campaignProfile.EmailBody, UpdatedBy = item.UpdatedBy });
                }
                else
                {
                    //_result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, CreatedDate = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation, SMSbody = "-", Emailbody = "-" });
                    _result.Add(new UserAdvertResult { Brand = item.Brand, MediaFileLocation = item.MediaFileLocation, AdvertId = item.AdvertId, Name = item.AdvertName, ClientId = item.ClientId, ClientName = item.ClientId == null ? "-" : item.Clients == null ? "-" : item.Clients.Name, userId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Email = item.User.Email, status = item.Status, fstatus = fstatus, CreatedDate = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDateTime, Scripts = item.Script, ScriptsPath = String.IsNullOrEmpty(item.ScriptFileLocation) ? item.ScriptFileLocation : ConfigurationManager.AppSettings["siteAddress"] + item.ScriptFileLocation, SMSbody = "-", Emailbody = "-", UpdatedBy = item.UpdatedBy });
                }
            }
            return _result;
        }

        [Route("SearchAdverts")]
        public ActionResult SearchAdverts([Bind(Prefix = "Item2")]UserAdvertFilter _filterCritearea, int[] UserId, int?[] ClientId, int[] AdvertId, int[] AdvertStatusId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<UserAdvertResult> _result = new List<UserAdvertResult>();
                var finalresult = new List<UserAdvertResult>();

                //Add 16-04-2019
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var operatorAdminId = efmvcUser.UserId;

                if (_filterCritearea != null)
                {
                    _result = GetAdvertResult(null, null, Convert.ToInt32(operatorAdminId));
                    finalresult = getadvertResult(_result, _filterCritearea, UserId, ClientId, AdvertId, AdvertStatusId);
                }
                else
                {
                    AdvertStatusId = new int[] { 1 };
                    _result = GetAdvertResult(null, null, Convert.ToInt32(operatorAdminId));
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
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            ChangeAdvertStatusCommand command = new ChangeAdvertStatusCommand();
            command.AdvertId = advertId;
            command.UpdatedBy = efmvcUser.UserId;
            command.Status = 5; // Rejection Status
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
               
                CreateOrUpdateAdvertRejectionCommand command2 = new CreateOrUpdateAdvertRejectionCommand();
                command2.AdvertId = advertId;
                command2.UserId = efmvcUser.UserId;
                command2.RejectionReason = rejectionReason;
                command2.CreatedDate = DateTime.Now;
                ICommandResult result2 = _commandBus.Submit(command2);

                var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == advertId);
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


                //TempData["status"] = "Advert is rejected successfully.";
                var advertName = _advertRepository.GetById(advertId).AdvertName;
                TempData["status"] = "Advert " + advertName + " is rejected successfully.";
                return Json("success");
            }
            return Json("error");
        }

        [Route("ApproveORRejectAdvert")]
        [HttpPost]
        public ActionResult ApproveORRejectAdvert(int id, int status, int oldstatus)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            ChangeAdvertStatusCommand command = new ChangeAdvertStatusCommand();
            //if (result.Success)
            //{
            if (status == (int)AdvertStatus.Live && oldstatus != (int)AdvertStatus.Pending)
            {
                command.AdvertId = id;
                command.Status = (int)AdvertStatus.Pending;
                command.UpdatedBy = efmvcUser.UserId;
                ICommandResult result = _commandBus.Submit(command);

                var advertData = _advertRepository.GetById(id);
                var mediaUrl = advertData.MediaFileLocation;
                var operatordId = advertData.OperatorId;
                var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == id);

                if (oldstatus == 4 && campaignadvertId != null)
                {
                    var camstatus = Changecampaignstatus(campaignadvertId.CampaignProfileId);
                }

                if (campaignadvertId != null)
                {
                    var CampaignData = _campaignprofileRepository.GetById(campaignadvertId.CampaignProfileId);
                    var countryId = CampaignData.CountryId == null ? 0 : CampaignData.CountryId;

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
                                //Add 08-08-2019
                                string adName = "";
                                if (mediaUrl == null || mediaUrl == "")
                                {
                                    adName = "";
                                }
                                else
                                {
                                    if (operatordId != (int)OperatorTableId.Safaricom)
                                    {
                                        var advertOperatorId = _advertRepository.GetById(id).OperatorId;
                                        //var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.Where(top => top.OperatorId == (int)advertOperatorId).FirstOrDefault();
                                        var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.FirstOrDefault();
                                        if (operatorFTPDetails != null)  adName = operatorFTPDetails.FtpRoot + "/" + mediaUrl.Split('/')[3];
                                    }
                                }
                                obj.UpdateMediaFileLocation(campaigndetails.CampaignProfileId, adName, SQLServerEntities);

                                //Comment 08-08-2019
                                //obj.UpdateMediaFileLocation(campaigndetails.CampaignProfileId, mediaUrl, SQLServerEntities);

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

                //TempData["status"] = "Advert is approved successfully.";
                var advertName = _advertRepository.GetById(id).AdvertName;
                TempData["status"] = "Advert " + advertName + " is approved successfully.";
            }
            else if (status == (int)AdvertStatus.Live && oldstatus == (int)AdvertStatus.Pending) // Live
            {

                
                EFMVCDataContex db = new EFMVCDataContex();
                var operatordId = db.Adverts.Where(s => s.AdvertId == id).FirstOrDefault().OperatorId;

                if (operatordId == (int)OperatorTableId.Safaricom)
                {

                    var returnValue = AdTransfer.CopyAdToOpeartorServer(id);
                    if (returnValue != "Success")
                    {
                        GenerateTicket objTicket = new GenerateTicket(_commandBus, _userRepository);
                        string message = returnValue;
                        int subjectId = (int)QuestionSubjectStatus.AdvertError;
                        objTicket.CreateAdTicket(efmvcUser.UserId, "Advert Error", message, subjectId, 0);
                    }
                    else
                    {
                        var crbtResponseValue = SoapApiProcess.UploadToneOnCRBTServer(id);
                        //var crbtResponseValue = "Success";
                        if (crbtResponseValue != "Success")
                        {
                            GenerateTicket objTicket = new GenerateTicket(_commandBus, _userRepository);
                            string message = crbtResponseValue;
                            int subjectId = (int)QuestionSubjectStatus.AdvertError;
                            objTicket.CreateAdTicket(efmvcUser.UserId, "Advert Error", message, subjectId, 0);
                        }
                        else
                        {
                            var responseCode = SoapApiProcess.UploadSoapTone(id);
                            //var responseCode = "000000";
                            if (responseCode == "000000")
                            {
                                ApproveAd(id, command, efmvcUser.UserId, status, oldstatus);       
                                //var advertData = _advertRepository.GetById(id);
                                //GenerateTicket objTicket = new GenerateTicket(_commandBus, _userRepository);
                                //int subjectId = (int)QuestionSubjectStatus.AdvertError;
                                //objTicket.CreateAdTicket(advertData.UserId, "Advert Error", responseCode, subjectId, 0);
                            }
                            else
                            {
                                var advertData = _advertRepository.GetById(id);
                                string message = "";
                                if (responseCode == "0" || responseCode.Contains("?"))
                                {
                                    message = responseCode + " - Unable to connect to the remote server";
                                }
                                else
                                {
                                    var responseCodeDetail = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == responseCode).FirstOrDefault();
                                    if (responseCodeDetail != null)
                                    {
                                        message = responseCode + " - " + responseCodeDetail.Description;
                                    }
                                    else
                                    {
                                        message = responseCode + " - please add this response code";
                                    }
                                }

                                GenerateTicket objTicket = new GenerateTicket(_commandBus, _userRepository);
                                int subjectId = (int)QuestionSubjectStatus.AdvertError;
                                objTicket.CreateAdTicket(advertData.UserId, "Advert Error", message, subjectId, 0);
                            }
                        }

                    }


                }
                else if (operatordId == (int)OperatorTableId.Expresso)
                {
                    var returnValue = AdTransfer.CopyAdToOpeartorServer(id);
                    if (returnValue != "Success")
                    {
                        GenerateTicket objTicket = new GenerateTicket(_commandBus, _userRepository);
                        string message = returnValue;
                        int subjectId = (int)QuestionSubjectStatus.AdvertError;
                        objTicket.CreateAdTicket(efmvcUser.UserId, "Advert Error", message, subjectId, 0);
                    }

                    ApproveAd(id, command, efmvcUser.UserId, status, oldstatus);
                }


            }
            else if (status == (int)AdvertStatus.Suspended) // suspended
            {
                command.AdvertId = id;
                command.Status = status;
                command.UpdatedBy = efmvcUser.UserId;
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

                //TempData["status"] = "Advert is suspended successfully.";
                var advertName = _advertRepository.GetById(id).AdvertName;
                TempData["status"] = "Advert " + advertName + " is suspended successfully.";
            }
            else if (status == (int)AdvertStatus.Archived) // Archived(Deleted)
            {
                command.AdvertId = id;
                command.Status = status;
                command.UpdatedBy = efmvcUser.UserId;
                ICommandResult result = _commandBus.Submit(command);
                EFMVCDataContex db = new EFMVCDataContex();
                var advertData = db.Adverts.Where(s => s.AdvertId == id).FirstOrDefault();

                if (advertData.OperatorId == (int)OperatorTableId.Safaricom)
                {
                    
                    //if (advertData.SoapToneId != null)
                    //{
                        //var responseCode = SoapApiProcess.DeleteToneSoapApi(id);
                        //271191
                        var responseCode = SoapApiProcess.DeleteSoapTone(id);
                        if (responseCode != "000000")
                        {
                            string message = "";
                            if (responseCode == "0" || responseCode.Contains("?"))
                            {
                                message = responseCode + " - Unable to connect to the remote server";
                            }
                            else
                            {
                                var responseCodeDetail = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == responseCode).FirstOrDefault();
                                if (responseCodeDetail != null)
                                {
                                    message = responseCode + " - " + responseCodeDetail.Description;
                                }
                                else
                                {
                                    message = responseCode + " - please add this response code";
                                }
                            }
                            GenerateTicket objTicket = new GenerateTicket(_commandBus, _userRepository);
                            int subjectId = (int)QuestionSubjectStatus.AdvertError;
                            objTicket.CreateAdTicket(advertData.UserId, "Advert Error", message, subjectId,0);
                        }
                   // }
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

                //TempData["status"] = "Advert is archived successfully.";
                var advertName = _advertRepository.GetById(id).AdvertName;
                TempData["status"] = "Advert " + advertName + " is archived successfully.";
            }
            else if (status == (int)AdvertStatus.Rejected)
            {
                command.AdvertId = id;
                command.Status = status;
                command.UpdatedBy = efmvcUser.UserId;
                ICommandResult result = _commandBus.Submit(command);
                //TempData["status"] = "Advert is rejected successfully.";
                var advertName = _advertRepository.GetById(id).AdvertName;
                TempData["status"] = "Advert " + advertName + " is rejected successfully.";
            }
            return Json("success");
            //}
            //return Json("error");
        }

        private void ApproveAd(int id, ChangeAdvertStatusCommand command, int userId, int status, int oldstatus)
        {
            command.Status = status;
            command.AdvertId = id;
            command.UpdatedBy = userId;
            ICommandResult result = _commandBus.Submit(command);
            var advertData = _advertRepository.GetById(id);
            var campaignadvertId = _campaignadvertRepository.Get(top => top.AdvertId == id);

            var operatordId = advertData.OperatorId;

            //var returnValue = AdTransfer.CopyAdToOpeartorServer(id);
            //if (returnValue != "Success")
            //{
            //    GenerateTicket objTicket = new GenerateTicket(_commandBus, _userRepository);
            //    string message = returnValue;
            //    int subjectId = (int)QuestionSubjectStatus.AdvertError;
            //    objTicket.CreateAdTicket(advertData.UserId, "Advert Error", message, subjectId,0);
            //}

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
                            //Add 08-08-2019
                            string adName = "";
                            if (advertData.MediaFileLocation == null || advertData.MediaFileLocation == "")
                            {
                                adName = "";
                            }
                            else
                            {
                                if (operatordId != (int)OperatorTableId.Safaricom)
                                {
                                    var advertOperatorId = _advertRepository.GetById(id).OperatorId;
                                    // var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.Where(top => top.OperatorId == (int)advertOperatorId).FirstOrDefault();
                                    var operatorFTPDetails = SQLServerEntities.OperatorFTPDetails.FirstOrDefault();
                                    if (operatorFTPDetails != null)  adName = operatorFTPDetails.FtpRoot + "/" + advertData.MediaFileLocation.Split('/')[3];

                                }
                            }
                            obj.UpdateMediaFileLocation(campaigndetails.CampaignProfileId, adName, SQLServerEntities);

                            //Comment 08-08-2019
                            //obj.UpdateMediaFileLocation(campaigndetails.CampaignProfileId, advertData.MediaFileLocation, SQLServerEntities);

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

            //TempData["status"] = "Advert is approved successfully.";
            var advertName = _advertRepository.GetById(id).AdvertName;
            TempData["status"] = "Advert " + advertName + " is approved successfully.";
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
                var campaignBilling = _billingRepository.Get(top => top.CampaignProfileId == campaigndetails.CampaignProfileId);
                if (campaignBilling != null)
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
                else
                {
                    _campaignstatus.Status = (int)CampaignStatus.CampaignPausedDueToInsufficientFunds;
                }
            }
            ICommandResult campaignstatuscommandResult = _commandBus.Submit(_campaignstatus);
            if (campaignstatuscommandResult.Success)
            {
                status = true;

            }
            return status;
        }

        public List<UserAdvertResult> getadvertResult(List<UserAdvertResult> advertresult, UserAdvertFilter _filterCritearea, int[] UserId, int?[] ClientId, int[] AdvertId, int[] AdvertStatusId)
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

                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    advertresult = advertresult.Where(top => top.CreatedDateSort.Value.Date >= Fromdate && top.CreatedDateSort.Value.Date <= Todate).ToList();
                    // advertresult = advertresult.Where(top => top.CreatedDate.Value.Date >= _filterCritearea.Fromdate.Value.Date && top.CreatedDate.Value.Date <= _filterCritearea.Todate.Value.Date).ToList();
                }
            }
            else
            {
                if (UserId != null)
                {
                    advertresult = advertresult.Where(top => UserId.Contains(top.userId)).ToList();
                }
                if (AdvertStatusId != null)
                {
                    //advertresult = advertresult.Where(top => AdvertStatusId.Contains(top.status)).ToList();
                    advertresult = advertresult.ToList();
                }
            }
            return advertresult;
        }
        [Route("GetClientsUser")]
        [HttpPost]
        public ActionResult GetClientsUser(int[] userId)
        {
            try
            {
                //Add 17-04-2019
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var operatorAdminId = efmvcUser.UserId;
                var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;
                var userIdData = _userRepository.GetMany(top => top.OperatorId == operatorId).Select(top => top.UserId).ToList();

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
                    var clientdetails = _clientRepository.GetMany(top => userIdData.Contains((int)top.UserId)).Select(top => new
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

                    var advertdetails = _advertRepository.GetAll().Where(top => clientId.Contains((int)(top.ClientId))).Select(top => new
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
                            TempData["msgsuccess"] = "Record updated successfully.";
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



        private void CopyAdToOpeartorServer(int advertId)
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                var advert = db.Adverts.Where(s => s.AdvertId == advertId).FirstOrDefault();
                var mediaFile = advert.MediaFileLocation;
                if (!string.IsNullOrEmpty(mediaFile) && advert.UploadedToMediaServer == false)
                {
                    var host = "138.68.177.47";
                    var port = 22;
                    var username = "provisioning";
                    var password = "adtonespassword";
                    var localRoot = Server.MapPath("~/Media");
                    var ftpRoot = "/usr/local/arthar/adds";
                    using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
                    {
                        client.Connect();
                        if (client.IsConnected)
                        {
                            var SourceFile = localRoot + @"\" + advert.UserId + @"\" + System.IO.Path.GetFileName(advert.MediaFileLocation);
                            var DestinationFile = ftpRoot + "/" + System.IO.Path.GetFileName(advert.MediaFileLocation);
                            var filestream = new FileStream(SourceFile, FileMode.Open);
                            client.UploadFile(filestream, DestinationFile);
                            advert.UploadedToMediaServer = true;
                            db.SaveChanges();
                        }
                    }
                }

            }
            catch
            {
                throw;
            }
        }

        [Route("FillUserDropdownAJAX")]
        [HttpPost]
        public ActionResult FillUserDropdownAJAX(string UserName)
        {
            try
            {
                //Add 16-04-2019
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var operatorAdminId = efmvcUser.UserId;
                var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;

                var userData = _userRepository.GetById(operatorAdminId);
                var userIdData = _userRepository.GetMany(top => top.OperatorId == operatorId && top.RoleId == 3).Select(top => top.UserId).ToList();

                if (!string.IsNullOrEmpty(UserName))
                {
                    //var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName) && userIdData.Contains(top.UserId)).Select(top => new
                    //var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName) && top.OperatorId == operatorId && (top.RoleId == 2 || top.RoleId == 3)).Select(top => new
                    var userdetails = _userRepository.GetMany(top => top.FirstName.Contains(UserName) || top.LastName.Contains(UserName) && top.OperatorId == operatorId && userIdData.Contains(top.UserId)).Select(top => new
                    // var userdetails = _userRepository.GetMany(top => top.FirstName.Contains(UserName) || top.LastName.Contains(UserName)).Select(top => new
                    {
                        Name = top.FirstName + " " + top.LastName,
                        UserId = top.UserId,
                    }).ToList();
                    ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
                    var data = userdetails.Select(x => new { id = x.UserId, name = x.Name }).ToArray();
                    return Json(data);
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }
    }
}