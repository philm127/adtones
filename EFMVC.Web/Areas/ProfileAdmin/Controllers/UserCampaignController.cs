using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.Web.Areas.ProfileAdmin.Models;
using EFMVC.Web.Areas.UsersAdmin.Models;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Models;

namespace EFMVC.Web.Areas.ProfileAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "ProfileAdmin")]
    [RouteArea("ProfileAdmin")]
    [RoutePrefix("UserCampaign")]
    public class UserCampaignController : Controller
    {
        // GET: ProfileAdmin/UserCampaign
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _companydetails repository
        /// </summary>
        private readonly ICompanyDetailsRepository _companyDetailsRepository;

        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The _operator repository
        /// </summary>
        private readonly IOperatorRepository _operatorRepository;

        /// <summary>
        /// The _contact repository
        /// </summary>
        private readonly IContactsRepository _contactsRepository;

        /// <summary>
        /// The _campaign repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileRepository;

        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        /// <summary>
        /// The _campaignAdvert Repository
        /// </summary>
        private readonly ICampaignAdvertRepository _campaignAdvertRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public UserCampaignController(ICommandBus commandBus, IUserRepository userRepository, ICompanyDetailsRepository companyDetailsRepository, ICountryRepository countryRepository, IOperatorRepository operatorRepository, IContactsRepository contactsRepository, ICampaignProfileRepository profileRepository, IAdvertRepository advertRepository, ICampaignAdvertRepository campaignAdvertRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _companyDetailsRepository = companyDetailsRepository;
            _countryRepository = countryRepository;
            _operatorRepository = operatorRepository;
            _contactsRepository = contactsRepository;
            _profileRepository = profileRepository;
            _advertRepository = advertRepository;
            _campaignAdvertRepository = campaignAdvertRepository;
        }

        [Route("Index")]
        public ActionResult Index()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var operatorAdminId = efmvcUser.UserId;
            List<UserCampaignResult> _result = new List<UserCampaignResult>();
            UserCampaignResult campaignResult = new UserCampaignResult();
            UserCampaignFilter _filterCritearea = new UserCampaignFilter();
            FillCountryDropdown();
            FillOperatorDropdown(0);
            FillCampaignDropdown(0);
            FillAdvertDropdown(0);
            _result.Add(campaignResult);
            return View(Tuple.Create(_result, _filterCritearea));
        }

        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                bool searchValue = false;
                List<String> columnSearch = new List<string>();
                var cnt = 10;
                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                    if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null") searchValue = true;
                }
                List<CampaignProfile> campaignProfiles = null;
                List<UserCampaignResult> _result = new List<UserCampaignResult>();
                if (searchValue == true)
                {
                    int?[] CountryId = new int?[cnt];
                    int[] OperatorId = new int[cnt];
                    int[] CampaignId = new int[cnt];
                    int[] AdvertId = new int[cnt];

                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            CountryId = columnSearch[0].Split(',').Select(top => (int?)Convert.ToInt32(top)).ToArray();
                        }
                        else
                        {
                            columnSearch[0] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[1]))
                    {
                        if (columnSearch[1] != "null")
                        {
                            OperatorId = columnSearch[1].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[1] = null;

                        }
                    }
                    if (!String.IsNullOrEmpty(columnSearch[2]))
                    {
                        if (columnSearch[2] != "null")
                        {
                            CampaignId = columnSearch[2].Split(',').Select(int.Parse).ToArray();
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
                            AdvertId = columnSearch[3].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[3] = null;
                        }
                    }

                    if (columnSearch[0] != null)
                    {
                        campaignProfiles = _profileRepository.GetMany(top => CountryId.Contains(top.CountryId) && (top.Status != 5)).OrderByDescending(top => top.CampaignProfileId).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        int?[] countryId = new int?[cnt];
                        countryId = _operatorRepository.GetMany(top => OperatorId.Contains(top.OperatorId)).Select(top => top.CountryId).ToArray();
                        campaignProfiles = campaignProfiles.Where(top => countryId.Contains(top.CountryId)).OrderByDescending(top => top.CampaignProfileId).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        campaignProfiles = campaignProfiles.Where(top => CampaignId.Contains(top.CampaignProfileId)).OrderByDescending(top => top.CampaignProfileId).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        int[] campaignId = new int[cnt];
                        campaignId = _campaignAdvertRepository.GetMany(top => AdvertId.Contains(top.AdvertId)).Select(top => top.CampaignProfileId).ToArray();
                        campaignProfiles = campaignProfiles.Where(top => campaignId.Contains(top.CampaignProfileId)).OrderByDescending(top => top.CampaignProfileId).ToList();
                    }
                    else
                    {
                        campaignProfiles = _profileRepository.GetMany(top => top.Status != 5).OrderByDescending(top => top.CampaignProfileId).ToList();
                    }

                    cnt = campaignProfiles.Count();
                    campaignProfiles = campaignProfiles.Skip(param.Start).Take(param.Length).ToList();
                }
                else
                {
                    campaignProfiles = _profileRepository.GetMany(top => top.Status != 5).OrderByDescending(top => top.CampaignProfileId).ToList();
                    cnt = campaignProfiles.Count();
                    campaignProfiles = campaignProfiles.Skip(param.Start).Take(param.Length).ToList();
                }

                if (campaignProfiles != null)
                {
                    foreach (var item in campaignProfiles)
                    {
                        EFMVCDataContex db = new EFMVCDataContex();
                        IEnumerable<CampaignAuditFormModel> CampaignAuditData = null;
                        var play = CampaignAuditStatusExtensions.PlayedAsLowerCase;
                        CampaignAuditData = db.CampaignAudits.Where(top => top.Status.ToLower() == play && top.PlayLengthTicks > 6000 && top.CampaignProfileId == item.CampaignProfileId)
                                            .Select(s => new CampaignAuditFormModel { CampaignAuditId = s.CampaignAuditId, CampaignProfileId = s.CampaignProfileId, UserProfileId = s.UserProfileId });
                        var userProfileAdvertCount = CampaignAuditData.Select(s => s.UserProfileId).Distinct().Count();

                        var advertdetails = item.CampaignAdverts.Where(top => top.CampaignProfileId == item.CampaignProfileId).FirstOrDefault();
                        string advertname = string.Empty;
                        var advertId = 0;
                        string mediaFile = string.Empty;
                        if (advertdetails != null)
                        {
                            var advert = _advertRepository.GetAll().Where(top => top.AdvertId == advertdetails.AdvertId).FirstOrDefault();
                            advertname = advert.AdvertName;
                            advertId = advert.AdvertId;
                            mediaFile = advert.MediaFileLocation;
                        }
                        else
                        {
                            advertname = "-";
                            advertId = 0;
                        }
                        var operatorDetails = _operatorRepository.Get(top => top.CountryId == item.CountryId);
                        _result.Add(new UserCampaignResult
                        {
                            CampaignProfileId = item.CampaignProfileId,
                            CampaignName = item.CampaignName,
                            AdvertId = advertId,
                            AdvertName = advertname,
                            TotalPlayCount = userProfileAdvertCount,
                            CountryId = item.CountryId,
                            CountryName = item.Country.Name,
                            OperatorId = operatorDetails.OperatorId,
                            OperatorName = operatorDetails.OperatorName,
                            ProfileType = "-",
                            ProfileName = "-",
                            ExpectedResponse = "-",
                            Listen = mediaFile
                        });
                    }
                }
                _result = ApplySorting(param, _result);
                DTResult<UserCampaignResult> result = new DTResult<UserCampaignResult>
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

        // Function For Filter/Sorting Campaign Profile Data
        private static List<UserCampaignResult> ApplySorting(DTParameters param, List<UserCampaignResult> dtsource)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.CampaignName).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.CampaignName).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.AdvertName).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.AdvertName).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.TotalPlayCount).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.TotalPlayCount).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.CountryName).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.CountryName).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.OperatorName).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.OperatorName).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.ProfileType).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.ProfileType).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.ProfileName).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.ProfileName).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.ExpectedResponse).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.ExpectedResponse).ToList();
                }
            }
            return dtsource;
        }

        public void FillCountryDropdown()
        {
            var countrydetails = _countryRepository.GetMany(top => top.Status == 1).Select(top => new
            {
                Name = top.Name,
                Id = top.Id.ToString(),
            }).ToList();
            ViewBag.country = new MultiSelectList(countrydetails, "Id", "Name");
        }

        public void FillOperatorDropdown(int? countryId)
        {
            var operatordetails = _operatorRepository.GetMany(top => top.CountryId == countryId && top.IsActive == true).Select(top => new
            {
                Name = top.OperatorName,
                Id = top.OperatorId.ToString(),
            }).ToList();
            ViewBag.operators = new MultiSelectList(operatordetails, "Id", "Name");
        }

        public void FillCampaignDropdown(int? countryId)
        {
            if (countryId != 0)
            {
                var campaigndetails = _profileRepository.GetMany(top => top.CountryId == countryId).Select(top => new
                {
                    Name = top.CampaignName,
                    Id = top.CampaignProfileId.ToString(),
                }).ToList();
                ViewBag.campaigns = new MultiSelectList(campaigndetails, "Id", "Name");
            }
            else
            {
                List<CampaignProfile> campaignList = new List<CampaignProfile>();
                var campaigndetails = campaignList.Select(top => new
                {
                    Name = top.CampaignName,
                    Id = top.CampaignProfileId.ToString(),
                }).ToList();
                ViewBag.campaigns = new MultiSelectList(campaigndetails, "Id", "Name");
            }
        }

        public void FillAdvertDropdown(int? countryId)
        {
            if (countryId != 0)
            {
                var advertdetails = _advertRepository.GetMany(top => top.CountryId == countryId).Select(top => new
                {
                    Name = top.AdvertName,
                    Id = top.AdvertId.ToString(),
                }).ToList();
                ViewBag.adverts = new MultiSelectList(advertdetails, "Id", "Name");
            }
            else
            {
                List<Advert> advertList = new List<Advert>();
                var advertdetails = advertList.Select(top => new
                {
                    Name = top.AdvertName,
                    Id = top.AdvertId.ToString(),
                }).ToList();
                ViewBag.adverts = new MultiSelectList(advertdetails, "Id", "Name");
            }
        }

        [Route("getOperatorByCountryId")]
        [HttpPost]
        public ActionResult getOperatorByCountryId(int[] countryId)
        {
            try
            {
                if (countryId != null)
                {
                    var operatordetails = _operatorRepository.GetAll().Where(top => countryId.Contains((int)(top.CountryId)) && top.IsActive == true).Select(top => new
                    {
                        Name = top.OperatorName,
                        Id = top.OperatorId
                    }).ToList();
                    return Json(operatordetails);
                }
                else
                {
                    var operatordetails = _profileRepository.GetAll().Select(top => new
                    {
                        Name = top.CampaignName,
                        Id = top.CampaignProfileId
                    }).ToList();
                    return Json(null);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        [Route("GetCampaignsByUserId")]
        [HttpPost]
        public ActionResult GetCampaignsByCountryId(int[] countryId)
        {
            try
            {
                if (!countryId.Contains(0))
                {
                    var campaigndetails = _profileRepository.GetAll().Where(top => countryId.Contains((int)(top.CountryId)) && top.Status != 5).Select(top => new
                    {
                        Name = top.CampaignName,
                        Id = top.CampaignProfileId
                    }).ToList();
                    return Json(campaigndetails);
                }
                else
                {
                    var campaigndetails = _profileRepository.GetAll().Select(top => new
                    {
                        Name = top.CampaignName,
                        Id = top.CampaignProfileId
                    }).ToList();
                    return Json(null);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        [Route("getAdvertByCountryId")]
        [HttpPost]
        public ActionResult getAdvertByCountryId(int[] countryId, int[] operatorId, int[] campaignId)
        {
            try
            {
                if (!countryId.Contains(0))
                {
                    var advertdetails = _advertRepository.GetAll().Where(top => countryId.Contains((int)(top.CountryId))).Select(top => new
                    {
                        Name = top.AdvertName,
                        Id = top.AdvertId
                    }).ToList();
                    return Json(advertdetails);
                }
                else if (!operatorId.Contains(0))
                {
                    var advertdetails = _advertRepository.GetAll().Where(top => operatorId.Contains((int)(top.OperatorId))).Select(top => new
                    {
                        Name = top.AdvertName,
                        Id = top.AdvertId
                    }).ToList();
                    return Json(advertdetails);
                }
                else if (!campaignId.Contains(0))
                {
                    List<int> campaignAdvertId = _campaignAdvertRepository.GetMany(top => campaignId.Contains(top.CampaignProfileId)).Select(top => top.AdvertId).ToList();
                    var advertdetails = _advertRepository.GetAll().Where(top => campaignAdvertId.Contains((int)(top.AdvertId))).Select(top => new
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
                    return Json(null);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }
    }
}