using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.Model.Entities;
using EFMVC.Web.Areas.Admin.ViewModel;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ClosedXML.Excel;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("ManagementReport")]
    public class ManagementReportController : Controller
    {

        private readonly IImportFileTracksRepository _ImportFileTrackRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IOperatorRepository _OperatorRepository;
        private readonly IUserRepository _UserRepository;
        private readonly ICampaignAuditRepository _CampaignAuditRepository;
        private readonly IImportsRepository _ImportsRepository;
        private readonly ICampaignAdvertRepository _CampaignAdvertRepository;
        private readonly IAdvertRepository _AdvertRepository;
        private readonly ICampaignProfileRepository _CampaignProfileRepository;
        private readonly IUserProfileAdvertsReceivedRepository _UserProfileAdvertsReceivedRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly CurrencyConversion _currencyConversion;
        public ManagementReportController(IImportFileTracksRepository ImportFileTrackRepository, ICurrencyRepository currencyRepository, IOperatorRepository OperatorRepository, IUserRepository UserRepository, ICampaignAuditRepository CampaignAuditRepository, IImportsRepository ImportsRepository, ICampaignAdvertRepository CampaignAdvertRepository, IAdvertRepository AdvertRepository, ICampaignProfileRepository CampaignProfileRepository, IUserProfileAdvertsReceivedRepository UserProfileAdvertsReceivedRepository, ICountryRepository countryRepository)
        {
            _ImportFileTrackRepository = ImportFileTrackRepository;
            _currencyRepository = currencyRepository;
            _OperatorRepository = OperatorRepository;
            _UserRepository = UserRepository;
            _CampaignAuditRepository = CampaignAuditRepository;
            _ImportsRepository = ImportsRepository;
            _CampaignAdvertRepository = CampaignAdvertRepository;
            _AdvertRepository = AdvertRepository;
            _CampaignProfileRepository = CampaignProfileRepository;
            _UserProfileAdvertsReceivedRepository = UserProfileAdvertsReceivedRepository;
            _countryRepository = countryRepository;
            _currencyConversion = CurrencyConversion.CreateForCurrentUser(this, _currencyRepository);
        }

        private readonly Dictionary<string, CurrencyModel> _currencyModelCache = new Dictionary<string, CurrencyModel>();

        private CurrencyModel GetCurrencyRateModel(string from, string to, CurrencyConversion currencyConversion)
        {
            string compositeKey = $"{from}~{to}";
            CurrencyModel result;
            if (!_currencyModelCache.TryGetValue(compositeKey, out result))
            {
                result = currencyConversion.ForeignCurrencyConversion("1", from, to);
                _currencyModelCache.Add(compositeKey, result);
            }

            return result;
        }

        [Route("Index")]
        public async Task<ActionResult> Index()
        {
            int[] operatorId = _OperatorRepository.GetAll().Select(top => top.OperatorId).ToArray();
            HashSet<int?> countryId = new HashSet<int?>(await _OperatorRepository.AsQueryable().Where(top => operatorId.Contains(top.OperatorId)).Select(top => top.CountryId).ToListAsync());
            var play = CampaignAuditStatusExtensions.PlayedAsLowerCase;
            var cancel = CampaignAuditStatusExtensions.CancelledAsLowerCase;
            campaignAudit campaignAudit = new campaignAudit();
            await FillOperator();
            ManagementReportModel model = new ManagementReportModel();
            model.NumOfTotalUser = await _UserRepository.AsQueryable().CountAsync(s => s.Activated == 1 && s.RoleId == 2);
            //model.NumOfRemovedUser = _UserRepository.GetMany(s => s.Activated == 0 && s.RoleId == 2).Count();
            model.NumOfRemovedUser = await _UserRepository.AsQueryable().CountAsync(s => s.Activated == 3 && s.RoleId == 2);
            List<ImportFileTrack> importFileTrack = await _ImportFileTrackRepository.AsQueryable().ToListAsync();
            if (importFileTrack.Count() > 0)
            {
                model.NumOfTextFile = await importFileTrack.AsQueryable().SumAsync(s => s.NumOfTextFile);
                model.NumOfTextLine = await importFileTrack.AsQueryable().SumAsync(s => s.NumOfTextLine);
            }
            else
            {
                model.NumOfTextFile = 0;
                model.NumOfTextLine = 0;
            }
            model.NumOfUpdateToAudit = _ImportsRepository.GetMany(s => s.Proceed == true).Count();
            var campaignAuditData = await _CampaignAuditRepository.AsQueryable()
                .Include(t=>t.CampaignProfile)
                .Where(top => top.Status.ToLower() == play && top.PlayLengthTicks >= 6000)
                .ToListAsync();
            
            model.NumOfPlay = campaignAuditData.Count();
            model.NumOfPlayUnder6secs = await _CampaignAuditRepository.AsQueryable().Where(top => top.Status.ToLower() == play && top.PlayLengthTicks < 6000).CountAsync();
            model.NumOfSMS = campaignAuditData.Count(s => s.SMSCost != 0);
            model.NumOfEmail = campaignAuditData.Count(s => s.EmailCost != 0);
            model.NumOfCancel = await _CampaignAuditRepository.AsQueryable().CountAsync(top => top.Status.ToLower() == cancel);
            //Currency Conversion
            if (campaignAuditData.Count() > 0)
            {
                if (model.NumOfTotalUser != 0)
                {
                    model.AveragePlaysPerUser = (campaignAuditData.Count() / model.NumOfTotalUser);
                }
                else
                {
                    model.AveragePlaysPerUser = campaignAuditData.Count();
                }

                CurrencyModel currencyModel;
                //foreach (var campaignAuditItem in campaignAuditData)
                //{
                //    if (campaignAuditItem.CampaignProfileId != 0)
                //    {
                //        var campaignProfileCurrencyCode = campaignAuditItem.CampaignProfile.CurrencyCode;
                //        string fromCurrencyCode = campaignProfileCurrencyCode;
                //        string toCurrencyCode = "GBP";
                //        if (toCurrencyCode == fromCurrencyCode) model.TotalSpend = model.TotalSpend + campaignAuditItem.TotalCost;
                //        else
                //        {
                //            currencyModel = GetCurrencyRateModel(fromCurrencyCode, toCurrencyCode, _currencyConversion);
                //            var currencyRate = currencyModel.Amount;
                //            if (currencyModel.Code == "OK") model.TotalSpend = model.TotalSpend + (Convert.ToDouble(Convert.ToDecimal(campaignAuditItem.TotalCost) * currencyRate));
                //        }
                //    }
                //}
                var campaignProfileList = campaignAuditData
                    .GroupBy(ca => ca.CampaignProfileId, ca => ca.CampaignProfile, (key, g) => g.FirstOrDefault())
                    .ToList();
                //foreach (var campaignProfileData in campaignProfileList)
                //{
                //    string fromCurrencyCode = campaignProfileData.CurrencyCode;
                //    string toCurrencyCode = "GBP";
                //    if (toCurrencyCode == fromCurrencyCode) model.TotalCredit = model.TotalCredit + (double)campaignProfileData.TotalCredit;
                //    else
                //    {
                //        currencyModel = GetCurrencyRateModel(fromCurrencyCode, toCurrencyCode, _currencyConversion);
                //        var currencyRate = currencyModel.Amount;
                //        if (currencyModel.Code == "OK") model.TotalCredit = model.TotalCredit + (Convert.ToDouble(Convert.ToDecimal(campaignProfileData.TotalCredit) * currencyRate));
                //    }
                //}

                campaignAudit = CalculateTotalSpendTotalCredit(campaignAuditData, null, null, operatorId, countryId);
                if (campaignAudit.TotalSpend != 0) model.TotalSpend = campaignAudit.TotalSpend;
                else model.TotalSpend = campaignAuditData.Sum(s => s.TotalCost);
                if (campaignAudit.TotalCredit != 0) model.TotalCredit = campaignAudit.TotalCredit;
                else model.TotalCredit = Convert.ToDouble(campaignProfileList.Sum(s => s.TotalCredit));

                model.NumOfLiveCampaign = campaignProfileList.Count();
                model.NumberOfAdsProvisioned = await _AdvertRepository.AsQueryable().CountAsync();
                TempData["NumOfTotalUser"] = model.NumOfTotalUser;
                TempData["NumOfRemovedUser"] = model.NumOfRemovedUser;
                TempData["NumOfTextFile"] = model.NumOfTextFile;
                TempData["NumOfTextLine"] = model.NumOfTextLine;
                TempData["NumOfUpdateToAudit"] = model.NumOfUpdateToAudit;
                TempData["NumOfPlay"] = model.NumOfPlay;
                TempData["NumOfPlayUnder6secs"] = model.NumOfPlayUnder6secs;
                TempData["NumOfSMS"] = model.NumOfSMS;
                TempData["NumOfEmail"] = model.NumOfEmail;
                TempData["NumOfCancel"] = model.NumOfCancel;
                TempData["TotalSpend"] = model.TotalSpend;
                TempData["TotalCredit"] = model.TotalCredit;
                TempData["NumOfLiveCampaign"] = model.NumOfLiveCampaign;
                TempData["NumberOfAdsProvisioned"] = model.NumberOfAdsProvisioned;
                TempData["AveragePlaysPerUser"] = model.AveragePlaysPerUser;
                TempData["OperatorName"] = "All Operators";
                TempData["FromDate"] = "";
                TempData["ToDate"] = "";
            }
            return View(model);
        }

        public async Task FillOperator()
        {
            var operatordetails = await _OperatorRepository.AsQueryable().Select(top => new { Name = top.OperatorName, Id = top.OperatorId }).ToListAsync();
            ViewBag.OperatorList = new MultiSelectList(operatordetails, "Id", "Name");
        }

        [Route("SearchImportFileTracks")]
        public async Task<ActionResult> SearchImportFileTracks(int[] OperatorId, string Fromdate, string Todate)
        {
            ManagementReportModel model = new ManagementReportModel();
            campaignAudit campaignAudit = new campaignAudit();
            int NumOfLiveCampaign = 0;
            var NumberOfAdsProvisioned = 0;
            var play = CampaignAuditStatusExtensions.PlayedAsLowerCase;
            var cancel = CampaignAuditStatusExtensions.CancelledAsLowerCase;
            var NumOfTotalUser = await _UserRepository.AsQueryable().Where(s => s.Activated == 1 && s.RoleId == 2).ToListAsync();
            //var NumOfRemovedUser = _UserRepository.GetMany(s => s.Activated == 0 && s.RoleId == 2).ToList();
            var NumOfRemovedUser = await _UserRepository.AsQueryable().Where(s => s.Activated == 3 && s.RoleId == 2).ToListAsync();
            var NumOfTextFile = await _ImportFileTrackRepository.AsQueryable().ToListAsync();
            var NumOfTextLine = new List<ImportFileTrack>(NumOfTextFile);
            var NumOfUpdateToAudit = await _ImportsRepository.AsQueryable().Where(s => s.Proceed == true).ToListAsync();
            var campaignAuditData = await _CampaignAuditRepository.AsQueryable()
                .Include(ca=>ca.CampaignProfile)
                .Where(top => top.Status == play && top.PlayLengthTicks >= 6000).ToListAsync();
            var NumOfPlayUnder6secs = await _CampaignAuditRepository.AsQueryable()
                .Include(ca => ca.CampaignProfile).Where(top => top.Status.ToLower() == play && top.PlayLengthTicks < 6000).ToListAsync();
            var NumOfPlay = campaignAuditData.ToList();
            var NumOfSMS = campaignAuditData.Where(s => s.SMSCost != 0).ToList();
            var NumOfEmail = campaignAuditData.Where(s => s.EmailCost != 0).ToList();
            var NumOfCancel = await _CampaignAuditRepository.AsQueryable().Where(top => top.Status == cancel).ToListAsync();
            var TotalSpendData = await _CampaignAuditRepository.AsQueryable().Include(ca=>ca.CampaignProfile).Where(top => top.Status == play && top.PlayLengthTicks >= 6000).ToListAsync();
            var campaignProfileList = TotalSpendData.GroupBy(ca=>ca.CampaignProfileId, ca=>ca.CampaignProfile, (i,ca)=>ca.FirstOrDefault()).ToList();
            var TotalCredit = new List<CampaignProfile>(campaignProfileList);
            var NumOfBucket = new List<ImportFileTrack>(NumOfTextFile);

            if (!string.IsNullOrEmpty(Fromdate) && !string.IsNullOrEmpty(Todate) && OperatorId != null)
            {
                string strTodate = Todate;
                DateTime Todatet = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strFromdate = Fromdate;
                DateTime Fromdatet = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                NumOfTotalUser = NumOfTotalUser.Where(s => s.DateCreated.Date >= Fromdatet && s.DateCreated.Date <= Todatet && OperatorId.Contains(s.OperatorId)).ToList();
                NumOfRemovedUser = NumOfRemovedUser.Where(s => s.DateCreated.Date >= Fromdatet && s.DateCreated.Date <= Todatet && OperatorId.Contains(s.OperatorId)).ToList();
                NumOfTextFile = NumOfTextFile.Where(s => s.AddedDate.Date >= Fromdatet && s.AddedDate.Date <= Todatet && OperatorId.Contains((int)s.OperatorId)).ToList();
                NumOfTextLine = NumOfTextLine.Where(s => s.AddedDate.Date >= Fromdatet && s.AddedDate.Date <= Todatet && OperatorId.Contains((int)s.OperatorId)).ToList();
                if (NumOfUpdateToAudit.Count() > 0)
                {
                    NumOfUpdateToAudit = NumOfUpdateToAudit.Where(s => s.AddedDate.Date >= Fromdatet && s.AddedDate.Date <= Todatet).ToList();
                }
                HashSet<int?> countryId = new HashSet<int?>(await _OperatorRepository.AsQueryable().Where(top => OperatorId.Contains(top.OperatorId)).Select(top => top.CountryId).ToListAsync());
                if (countryId.Count > 0)
                {
                    NumOfSMS = NumOfSMS.Where(s => s.AddedDate.Value.Date >= Fromdatet && s.AddedDate.Value.Date <= Todatet && countryId.Contains(s.CampaignProfile.CountryId)).ToList();
                    NumOfEmail = NumOfEmail.Where(s => s.AddedDate.Value.Date >= Fromdatet && s.AddedDate.Value.Date <= Todatet && countryId.Contains(s.CampaignProfile.CountryId)).ToList();
                    campaignAudit = CalculateTotalSpendTotalCredit(TotalSpendData, Fromdatet, Todatet, OperatorId, countryId);
                    TotalSpendData = TotalSpendData.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet && s.AddedDate.GetValueOrDefault().Date <= Todatet && countryId.Contains(s.CampaignProfile.CountryId)).ToList();
                    TotalCredit = TotalCredit.Where(s => s.CreatedDateTime.Date >= Fromdatet && s.CreatedDateTime.Date <= Todatet && countryId.Contains(s.CountryId)).ToList();
                    if (campaignAudit.TotalSpend != 0) model.TotalSpend = campaignAudit.TotalSpend;
                    else model.TotalSpend = TotalSpendData.Sum(s => s.TotalCost);
                    if (campaignAudit.TotalCredit != 0) model.TotalCredit = campaignAudit.TotalCredit;
                    else model.TotalCredit = Convert.ToDouble(TotalCredit.Sum(s => s.TotalCredit));
                    NumOfBucket = NumOfBucket.Where(s => s.AddedDate.Date >= Fromdatet && s.AddedDate.Date <= Todatet).ToList();
                    NumOfCancel = NumOfCancel.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet && s.AddedDate.GetValueOrDefault().Date <= Todatet && s.CampaignProfile != null && countryId.Contains(s.CampaignProfile.CountryId)).ToList();
                    NumOfPlay = NumOfPlay.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet && s.AddedDate.GetValueOrDefault().Date <= Todatet && s.CampaignProfile != null && countryId.Contains(s.CampaignProfile.CountryId)).ToList();
                    NumOfPlayUnder6secs = NumOfPlayUnder6secs.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet && s.AddedDate.GetValueOrDefault().Date <= Todatet && s.CampaignProfile != null && countryId.Contains(s.CampaignProfile.CountryId)).ToList();
                    NumOfLiveCampaign = TotalSpendData.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet && s.AddedDate.GetValueOrDefault().Date <= Todatet && s.CampaignProfile != null && countryId.Contains(s.CampaignProfile.CountryId)).Select(s => s.CampaignProfileId).Distinct().Count();
                    NumberOfAdsProvisioned = await _AdvertRepository.AsQueryable().CountAsync(s => s.CreatedDateTime >= Fromdatet && s.CreatedDateTime <= Todatet && s.OperatorId != null && OperatorId.Contains(s.OperatorId.Value));

                    //if (TotalSpendData.Count() > 0 && NumOfTotalUser.Count() > 0)
                    //{
                    //    model.AveragePlaysPerUser = TotalSpendData.Count() / NumOfTotalUser.Count();
                    //}
                    //else
                    //{
                    //    var UserProfileId = TotalSpendData.Select(s => s.UserProfileId);
                    //    var UsersData = await _UserRepository.AsQueryable().CountAsync(s => UserProfileId.Contains(s.UserProfiles.FirstOrDefault().UserProfileId));
                    //    model.AveragePlaysPerUser = TotalSpendData.Count / UsersData;
                    //}

                    if (TotalSpendData.Count() > 0 && NumOfTotalUser.Count() > 0)
                    {
                        model.AveragePlaysPerUser = (TotalSpendData.Count() / NumOfTotalUser.Count());
                    }
                    else
                    {
                        model.AveragePlaysPerUser = TotalSpendData.Count();
                    }
                }
            }
            else if (!string.IsNullOrEmpty(Fromdate) && !string.IsNullOrEmpty(Todate) && OperatorId == null)
            {
                string strTodate = Todate;
                DateTime Todatet = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string strFromdate = Fromdate;
                DateTime Fromdatet = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                NumOfTotalUser = NumOfTotalUser.Where(s => s.DateCreated.Date >= Fromdatet && s.DateCreated.Date <= Todatet).ToList();
                NumOfRemovedUser = NumOfRemovedUser.Where(s => s.DateCreated.Date >= Fromdatet && s.DateCreated.Date <= Todatet).ToList();
                NumOfTextFile = NumOfTextFile.Where(s => s.AddedDate.Date >= Fromdatet && s.AddedDate.Date <= Todatet).ToList();
                NumOfTextLine = NumOfTextLine.Where(s => s.AddedDate.Date >= Fromdatet && s.AddedDate.Date <= Todatet).ToList();
                if (NumOfUpdateToAudit.Count() > 0)
                {
                    NumOfUpdateToAudit = NumOfUpdateToAudit.Where(s => s.AddedDate.Date >= Fromdatet && s.AddedDate.Date <= Todatet).ToList();
                }
                NumOfSMS = NumOfSMS.Where(s => s.AddedDate.Value.Date >= Fromdatet && s.AddedDate.Value.Date <= Todatet).ToList();
                NumOfEmail = NumOfEmail.Where(s => s.AddedDate.Value.Date >= Fromdatet && s.AddedDate.Value.Date <= Todatet).ToList();
                campaignAudit = CalculateTotalSpendTotalCredit(TotalSpendData, Fromdatet, Todatet, null, null);
                TotalSpendData = TotalSpendData.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet && s.AddedDate.GetValueOrDefault().Date <= Todatet).ToList();
                TotalCredit = TotalCredit.Where(s => s.CreatedDateTime.Date >= Fromdatet && s.CreatedDateTime.Date <= Todatet).ToList();
                if (campaignAudit.TotalSpend != 0) model.TotalSpend = campaignAudit.TotalSpend;
                else model.TotalSpend = TotalSpendData.Sum(s => s.TotalCost);
                if (campaignAudit.TotalCredit != 0) model.TotalCredit = campaignAudit.TotalCredit;
                else model.TotalCredit = Convert.ToDouble(TotalCredit.Sum(s => s.TotalCredit));
                NumOfBucket = NumOfBucket.Where(s => s.AddedDate.Date >= Fromdatet && s.AddedDate.Date <= Todatet).ToList();
                NumOfCancel = NumOfCancel.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet && s.AddedDate.GetValueOrDefault().Date <= Todatet).ToList();
                NumOfPlay = NumOfPlay.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet && s.AddedDate.GetValueOrDefault().Date <= Todatet).ToList();
                NumOfPlayUnder6secs = NumOfPlayUnder6secs.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet && s.AddedDate.GetValueOrDefault().Date <= Todatet).ToList();
                NumOfLiveCampaign = TotalSpendData.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet && s.AddedDate.GetValueOrDefault().Date <= Todatet).Select(s => s.CampaignProfileId).Distinct().Count();
                NumberOfAdsProvisioned = _AdvertRepository.GetMany(s => s.CreatedDateTime >= Fromdatet && s.CreatedDateTime <= Todatet).Count();

                //if (TotalSpendData.Count() > 0 && NumOfTotalUser.Count() > 0)
                //{
                //    model.AveragePlaysPerUser = TotalSpendData.Count() / NumOfTotalUser.Count();
                //}
                //else
                //{
                //    var UserProfileId = TotalSpendData.Select(s => s.UserProfileId);
                //    var UsersData = await _UserRepository.AsQueryable().CountAsync(s => UserProfileId.Contains(s.UserProfiles.FirstOrDefault().UserProfileId));
                //    model.AveragePlaysPerUser = TotalSpendData.Count / UsersData;
                //}
                if (TotalSpendData.Count() > 0 && NumOfTotalUser.Count() > 0)
                {
                    model.AveragePlaysPerUser = (TotalSpendData.Count() / NumOfTotalUser.Count());
                }
                else
                {
                    model.AveragePlaysPerUser = TotalSpendData.Count();
                }
            }
            else if (OperatorId != null && string.IsNullOrEmpty(Fromdate) && string.IsNullOrEmpty(Todate))
            {
                NumOfTotalUser = NumOfTotalUser.Where(s => OperatorId.Contains(s.OperatorId)).ToList();
                NumOfRemovedUser = NumOfRemovedUser.Where(s => OperatorId.Contains(s.OperatorId)).ToList();
                NumOfTextFile = NumOfTextFile.Where(s => OperatorId.Contains((int)s.OperatorId)).ToList();
                NumOfTextLine = NumOfTextLine.Where(s => OperatorId.Contains((int)s.OperatorId)).ToList();
                HashSet<int?> countryId = new HashSet<int?>(await _OperatorRepository.AsQueryable().Where(top => OperatorId.Contains(top.OperatorId)).Select(top => top.CountryId).ToListAsync());
                if (countryId.Count > 0)
                {
                    NumOfPlay = NumOfPlay.Where(top => top.CampaignProfile != null && countryId.Contains(top.CampaignProfile.CountryId)).ToList();
                    NumOfPlayUnder6secs = NumOfPlayUnder6secs.Where(top => top.CampaignProfile != null && countryId.Contains(top.CampaignProfile.CountryId)).ToList();
                    NumOfSMS = NumOfSMS.Where(top => countryId.Contains(top.CampaignProfile.CountryId)).ToList();
                    NumOfEmail = NumOfEmail.Where(top => countryId.Contains(top.CampaignProfile.CountryId)).ToList();
                    NumOfCancel = NumOfCancel.Where(top => countryId.Contains(top.CampaignProfile.CountryId)).ToList();
                    campaignAudit = CalculateTotalSpendTotalCredit(TotalSpendData, null, null, OperatorId, countryId);
                    TotalSpendData = TotalSpendData.Where(top => countryId.Contains(top.CampaignProfile.CountryId)).ToList();
                    TotalCredit = TotalCredit.Where(s => countryId.Contains(s.CountryId)).ToList();
                    campaignProfileList = TotalSpendData.GroupBy(ca => ca.CampaignProfileId, ca => ca.CampaignProfile, (i, ca) => ca.FirstOrDefault()).ToList();
                    if (campaignAudit.TotalSpend != 0) model.TotalSpend = campaignAudit.TotalSpend;
                    else model.TotalSpend = TotalSpendData.Sum(s => s.TotalCost);
                    if (campaignAudit.TotalCredit != 0) model.TotalCredit = campaignAudit.TotalCredit;
                    else model.TotalCredit = Convert.ToDouble(TotalCredit.Sum(s => s.TotalCredit));
                    TotalCredit = new List<CampaignProfile>(campaignProfileList);
                    NumOfLiveCampaign = campaignProfileList.Count(top => top != null);
                    NumberOfAdsProvisioned = await _AdvertRepository.AsQueryable().CountAsync(s => s.OperatorId != null && OperatorId.Contains(s.OperatorId.Value));

                    //if (TotalSpendData.Count() > 0 && NumOfTotalUser.Count() > 0)
                    //{
                    //    model.AveragePlaysPerUser = TotalSpendData.Count() / NumOfTotalUser.Count();
                    //}
                    //else
                    //{
                    //    var UserProfileId = TotalSpendData.Select(s => s.UserProfileId);
                    //    var userdataCount = await _UserRepository.AsQueryable().CountAsync(s => UserProfileId.Contains(s.UserProfiles.FirstOrDefault().UserProfileId));
                    //    model.AveragePlaysPerUser = TotalSpendData.Count / userdataCount;
                    //}
                    if (TotalSpendData.Count() > 0 && NumOfTotalUser.Count() > 0)
                    {
                        model.AveragePlaysPerUser = (TotalSpendData.Count() / NumOfTotalUser.Count());
                    }
                    else
                    {
                        model.AveragePlaysPerUser = TotalSpendData.Count();
                    }
                }
            }
            model.NumOfPlay = NumOfPlay.Count();
            model.NumOfPlayUnder6secs = NumOfPlayUnder6secs.Count();
            model.NumOfTotalUser = NumOfTotalUser.Count();
            model.NumOfRemovedUser = NumOfRemovedUser.Count();
            model.NumOfTextFile = NumOfTextFile.Sum(s => s.NumOfTextFile);
            model.NumOfTextLine = NumOfTextLine.Sum(s => s.NumOfTextLine);
            model.NumOfUpdateToAudit = NumOfUpdateToAudit.Count();
            model.NumOfSMS = NumOfSMS.Count();
            model.NumOfEmail = NumOfEmail.Count();
            model.NumOfLiveCampaign = NumOfLiveCampaign;
            model.NumberOfAdsProvisioned = NumberOfAdsProvisioned;
            model.NumOfCancel = NumOfCancel.Count();
            

            TempData["NumOfTotalUser"] = model.NumOfTotalUser;
            TempData["NumOfRemovedUser"] = model.NumOfRemovedUser;
            TempData["NumOfTextFile"] = model.NumOfTextFile;
            TempData["NumOfTextLine"] = model.NumOfTextLine;
            TempData["NumOfUpdateToAudit"] = model.NumOfUpdateToAudit;
            TempData["NumOfPlay"] = model.NumOfPlay;
            TempData["NumOfPlayUnder6secs"] = model.NumOfPlayUnder6secs;
            TempData["NumOfSMS"] = model.NumOfSMS;
            TempData["NumOfEmail"] = model.NumOfEmail;
            TempData["NumOfCancel"] = model.NumOfCancel;
            TempData["TotalSpend"] = model.TotalSpend;
            TempData["TotalCredit"] = model.TotalCredit;
            TempData["NumOfLiveCampaign"] = model.NumOfLiveCampaign;
            TempData["NumberOfAdsProvisioned"] = model.NumberOfAdsProvisioned;
            TempData["AveragePlaysPerUser"] = model.AveragePlaysPerUser;
            TempData["OperatorName"] = "All Operators";
            if (OperatorId != null)
            {
                foreach (var item in OperatorId)
                {
                    var OperatorName = _OperatorRepository.GetById(item).OperatorName;
                    TempData["OperatorName"] = OperatorName;
                }
            }
            TempData["FromDate"] = Fromdate;
            TempData["ToDate"] = Todate;

            return PartialView("_ManagementReport", model);
        }

        [Route("ResetData")]
        public async Task<ActionResult> ResetData()
        {
            int[] operatorId = _OperatorRepository.GetAll().Select(top => top.OperatorId).ToArray();
            HashSet<int?> countryId = new HashSet<int?>(await _OperatorRepository.AsQueryable().Where(top => operatorId.Contains(top.OperatorId)).Select(top => top.CountryId).ToListAsync());
            var play = CampaignAuditStatusExtensions.PlayedAsLowerCase;
            var cancel = CampaignAuditStatusExtensions.CancelledAsLowerCase;
            campaignAudit campaignAudit = new campaignAudit();
            await FillOperator();
            ManagementReportModel model = new ManagementReportModel();
            model.NumOfTotalUser = await _UserRepository.AsQueryable().CountAsync(s => s.Activated == 1 && s.RoleId == 2);
            model.NumOfRemovedUser = await _UserRepository.AsQueryable().CountAsync(s => s.Activated == 3 && s.RoleId == 2);
            List<ImportFileTrack> importFileTrack = await _ImportFileTrackRepository.AsQueryable().ToListAsync();
            if (importFileTrack.Count() > 0)
            {
                model.NumOfTextFile = await importFileTrack.AsQueryable().SumAsync(s => s.NumOfTextFile);
                model.NumOfTextLine = await importFileTrack.AsQueryable().SumAsync(s => s.NumOfTextLine);
            }
            else
            {
                model.NumOfTextFile = 0;
                model.NumOfTextLine = 0;
            }
            model.NumOfUpdateToAudit = await _ImportsRepository.AsQueryable().CountAsync(s => s.Proceed == true);
            var campaignAuditData = await _CampaignAuditRepository.AsQueryable()
                .Include(ca=>ca.CampaignProfile)
                .Where(top => top.Status.ToLower() == play && top.PlayLengthTicks >= 6000)
                .ToListAsync();
            model.NumOfPlay = campaignAuditData.Count();
            model.NumOfPlayUnder6secs = await _CampaignAuditRepository.AsQueryable().Where(top => top.Status.ToLower() == play && top.PlayLengthTicks < 6000).CountAsync();
            model.NumOfSMS = campaignAuditData.Count(s => s.SMSCost != 0);
            model.NumOfEmail = campaignAuditData.Count(s => s.EmailCost != 0);
            model.NumOfCancel = await _CampaignAuditRepository.AsQueryable().CountAsync(top => top.Status.ToLower() == cancel);
            //Currency Conversion
            if (campaignAuditData.Count() > 0)
            {
                if (model.NumOfTotalUser != 0)
                {
                    model.AveragePlaysPerUser = campaignAuditData.Count() / model.NumOfTotalUser;
                }
                else
                {
                    model.AveragePlaysPerUser = campaignAuditData.Count();
                }
                //foreach (var campaignAuditItem in campaignAuditData)
                //{
                //    if (campaignAuditItem.CampaignProfileId != 0)
                //    {
                //        var campaignProfileCurrencyCode = _CampaignProfileRepository.GetById(campaignAuditItem.CampaignProfileId).CurrencyCode;
                //        string fromCurrencyCode = campaignProfileCurrencyCode;
                //        string toCurrencyCode = "GBP";
                //        if (toCurrencyCode == fromCurrencyCode) model.TotalSpend = model.TotalSpend + campaignAuditItem.TotalCost;
                //        else
                //        {
                //            var currencyModel = GetCurrencyRateModel(fromCurrencyCode, toCurrencyCode, _currencyConversion);
                //            decimal currencyRate = currencyModel.Amount;
                //            if (currencyModel.Code == "OK") model.TotalSpend = model.TotalSpend + (Convert.ToDouble(Convert.ToDecimal(campaignAuditItem.TotalCost) * currencyRate));
                //        }
                //    }
                //}
                var campaignProfileList = campaignAuditData
                    .GroupBy(ca => ca.CampaignProfileId, ca => ca.CampaignProfile, (key, g) => g.FirstOrDefault())
                    .ToList();
                //foreach (var campaignProfileData in campaignProfileList)
                //{
                //    string fromCurrencyCode = campaignProfileData.CurrencyCode;
                //    string toCurrencyCode = "GBP";
                //    if (toCurrencyCode == fromCurrencyCode) model.TotalCredit = model.TotalCredit + (double)campaignProfileData.TotalCredit;
                //    else
                //    {
                //        var currencyModel = GetCurrencyRateModel(fromCurrencyCode, toCurrencyCode, _currencyConversion);
                //        var currencyRate = currencyModel.Amount;
                //        if (currencyModel.Code == "OK") model.TotalCredit = model.TotalCredit + (Convert.ToDouble(Convert.ToDecimal(campaignProfileData.TotalCredit) * currencyRate));
                //    }
                //}

                campaignAudit = CalculateTotalSpendTotalCredit(campaignAuditData, null, null, operatorId, countryId);
                if (campaignAudit.TotalSpend != 0) model.TotalSpend = campaignAudit.TotalSpend;
                else model.TotalSpend = campaignAuditData.Sum(s => s.TotalCost);
                if (campaignAudit.TotalCredit != 0) model.TotalCredit = campaignAudit.TotalCredit;
                else model.TotalCredit = Convert.ToDouble(campaignProfileList.Sum(s => s.TotalCredit));

                model.NumOfLiveCampaign = campaignProfileList.Count();
                model.NumberOfAdsProvisioned = await _AdvertRepository.AsQueryable().CountAsync();

                TempData["NumOfTotalUser"] = model.NumOfTotalUser;
                TempData["NumOfRemovedUser"] = model.NumOfRemovedUser;
                TempData["NumOfTextFile"] = model.NumOfTextFile;
                TempData["NumOfTextLine"] = model.NumOfTextLine;
                TempData["NumOfUpdateToAudit"] = model.NumOfUpdateToAudit;
                TempData["NumOfPlay"] = model.NumOfPlay;
                TempData["NumOfPlayUnder6secs"] = model.NumOfPlayUnder6secs;
                TempData["NumOfSMS"] = model.NumOfSMS;
                TempData["NumOfEmail"] = model.NumOfEmail;
                TempData["NumOfCancel"] = model.NumOfCancel;
                TempData["TotalSpend"] = model.TotalSpend;
                TempData["TotalCredit"] = model.TotalCredit;
                TempData["NumOfLiveCampaign"] = model.NumOfLiveCampaign;
                TempData["NumberOfAdsProvisioned"] = model.NumberOfAdsProvisioned;
                TempData["AveragePlaysPerUser"] = model.AveragePlaysPerUser;
                TempData["OperatorName"] = "All Operators";
                TempData["FromDate"] = "";
                TempData["ToDate"] = "";
            }
            return PartialView("_ManagementReport", model);
        }

        public class campaignAudit
        {
            public double TotalSpend { get; set; }
            public double TotalCredit { get; set; }
        }

        //Calculate TotalSpend TotalCredit with Currency Conversion
        public campaignAudit CalculateTotalSpendTotalCredit(List<CampaignAudit> campaignAuditData, DateTime? Fromdatet, DateTime? Todatet, int[] OperatorId, HashSet<int?> countryId)
        {
            campaignAudit campaignAudit = new campaignAudit();
            if (!string.IsNullOrEmpty(Fromdatet.ToString()) && !string.IsNullOrEmpty(Todatet.ToString()) && OperatorId != null)
            {
                if (campaignAuditData.Count > 0)
                {
                    campaignAuditData = campaignAuditData.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet.GetValueOrDefault().Date && s.AddedDate.GetValueOrDefault().Date <= Todatet.GetValueOrDefault().Date && countryId.Contains(s.CampaignProfile.CountryId)).ToList();
                    campaignAudit = CalculateSearchData(campaignAuditData);
                }
            }
            else if (!string.IsNullOrEmpty(Fromdatet.ToString()) && !string.IsNullOrEmpty(Todatet.ToString()) && OperatorId == null)
            {
                if (campaignAuditData.Count > 0)
                {
                    campaignAuditData = campaignAuditData.Where(s => s.AddedDate.GetValueOrDefault().Date >= Fromdatet.GetValueOrDefault().Date && s.AddedDate.GetValueOrDefault().Date <= Todatet.GetValueOrDefault().Date).ToList();
                    campaignAudit = CalculateSearchData(campaignAuditData);
                }
            }
            else if (OperatorId != null && string.IsNullOrEmpty(Fromdatet.ToString()) && string.IsNullOrEmpty(Todatet.ToString()))
            {
                if (campaignAuditData.Count > 0)
                {
                    campaignAuditData = campaignAuditData.Where(top => countryId.Contains(top.CampaignProfile.CountryId)).ToList();
                    campaignAudit = CalculateSearchData(campaignAuditData);
                }
            }
            return campaignAudit;
        }

        //Calculate Campaign Audit Data with Currency Conversion
        public campaignAudit CalculateSearchData(List<CampaignAudit> campaignAuditData)
        {
            campaignAudit campaignAudit = new campaignAudit();
            if (campaignAuditData.Count > 0)
            {
                foreach (var campaignAuditItem in campaignAuditData)
                {
                    if (campaignAuditItem.CampaignProfileId != 0)
                    {
                        var campaignProfileCurrencyCode = _CampaignProfileRepository.GetById(campaignAuditItem.CampaignProfileId).CurrencyCode;
                        string fromCurrencyCode = campaignProfileCurrencyCode;
                        string toCurrencyCode = "GBP";
                        if (toCurrencyCode == fromCurrencyCode) campaignAudit.TotalSpend = campaignAudit.TotalSpend + campaignAuditItem.TotalCost;
                        else
                        {
                            var currencyModel = GetCurrencyRateModel(fromCurrencyCode, toCurrencyCode, _currencyConversion);
                            var currencyRate = currencyModel.Amount;
                            if (currencyModel.Code == "OK") campaignAudit.TotalSpend = campaignAudit.TotalSpend + (Convert.ToDouble(Convert.ToDecimal(campaignAuditItem.TotalCost) * currencyRate));
                        }
                    }
                }
                var campaignIdList = campaignAuditData.Select(s => s.CampaignProfileId).Distinct();
                foreach (var campaignId in campaignIdList)
                {
                    var campaignProfileData = _CampaignProfileRepository.GetById(campaignId);
                    string fromCurrencyCode = campaignProfileData.CurrencyCode;
                    string toCurrencyCode = "GBP";
                    if (toCurrencyCode == fromCurrencyCode) campaignAudit.TotalCredit = campaignAudit.TotalCredit + Convert.ToDouble(campaignProfileData.TotalCredit);
                    else
                    {
                        var currencyModel = GetCurrencyRateModel(fromCurrencyCode, toCurrencyCode, _currencyConversion);
                        var currencyRate = currencyModel.Amount;
                        if (currencyModel.Code == "OK") campaignAudit.TotalCredit = campaignAudit.TotalCredit + (Convert.ToDouble(Convert.ToDecimal(campaignProfileData.TotalCredit) * currencyRate));
                    }
                }
            }
            return campaignAudit;
        }

        [Route("GenerateReport")]
        public ActionResult GenerateReport()
        {
            var wb = new XLWorkbook();

            ManagementReportModel model = new ManagementReportModel();

            model.NumOfTotalUser = Convert.ToInt32(TempData["NumOfTotalUser"].ToString());
            model.NumOfRemovedUser = Convert.ToInt32(TempData["NumOfRemovedUser"].ToString());
            model.NumOfTextFile = Convert.ToInt32(TempData["NumOfTextFile"].ToString());
            model.NumOfTextLine = Convert.ToInt32(TempData["NumOfTextLine"].ToString());
            model.NumOfUpdateToAudit = Convert.ToInt32(TempData["NumOfUpdateToAudit"].ToString());
            model.NumOfPlay = Convert.ToInt32(TempData["NumOfPlay"].ToString());
            model.NumOfPlayUnder6secs = Convert.ToInt32(TempData["NumOfPlayUnder6secs"].ToString());
            model.NumOfSMS = Convert.ToInt32(TempData["NumOfSMS"].ToString());
            model.NumOfEmail = Convert.ToInt32(TempData["NumOfEmail"].ToString());
            model.NumOfCancel = Convert.ToInt32(TempData["NumOfCancel"].ToString());
            model.TotalSpend = Convert.ToDouble(TempData["TotalSpend"].ToString());
            model.TotalCredit = Convert.ToDouble(TempData["TotalCredit"].ToString());
            model.NumOfLiveCampaign = Convert.ToInt32(TempData["NumOfLiveCampaign"].ToString());
            model.NumberOfAdsProvisioned = Convert.ToInt32(TempData["NumberOfAdsProvisioned"].ToString());
            model.AveragePlaysPerUser = Convert.ToInt32(TempData["AveragePlaysPerUser"].ToString());

            List<ManagementReportModel> mappingResult = new List<ManagementReportModel>();
            mappingResult.Add(model);

            string fromDate = "", toDate = "", operatorName = "";
            fromDate = TempData["FromDate"].ToString();
            toDate = TempData["ToDate"].ToString();
            operatorName = TempData["OperatorName"].ToString();


            var ws = wb.Worksheets.Add("Management Report");
            ws.Style.Font.FontSize = 9;
            ws.Range("A1" + ":" + "N1").Merge().Value = "Management Report Data";
            ws.Range("A1" + ":" + "N1").Style.Font.FontSize = 14;
            ws.Range("A1" + ":" + "N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A1" + ":" + "N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A1" + ":" + "N1").Style.Font.Bold = true;
            ws.Columns("A:M").Width = 25;

            ws.Range("A2" + ":" + "B2").Merge().Value = "Operator";
            ws.Range("A2" + ":" + "B2").Style.Font.FontSize = 12;
            ws.Range("A2" + ":" + "B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A2" + ":" + "B2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A2" + ":" + "B2").Style.Font.Bold = true;
            ws.Columns("A:B").Width = 10;

            ws.Range("C2" + ":" + "D2").Merge().Value = operatorName;
            ws.Range("C2" + ":" + "D2").Style.Font.FontSize = 10;
            ws.Range("C2" + ":" + "D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("C2" + ":" + "D2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Columns("C:D").Width = 10;

            ws.Range("A3" + ":" + "B3").Merge().Value = "Date";
            ws.Range("A3" + ":" + "B3").Style.Font.FontSize = 12;
            ws.Range("A3" + ":" + "B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A3" + ":" + "B3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A3" + ":" + "B3").Style.Font.Bold = true;
            ws.Columns("A:B").Width = 10;

            ws.Range("C3" + ":" + "D3").Merge().Value = fromDate + " - " + toDate;
            ws.Range("C3" + ":" + "D3").Style.Font.FontSize = 10;
            ws.Range("C3" + ":" + "C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("C3" + ":" + "C3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Columns("C:D").Width = 10;

            ws.Range("A4" + ":" + "A5").Merge().Value = "Total Users";
            ws.Range("A4" + ":" + "A5").Style.Font.FontSize = 12;
            ws.Range("A4" + ":" + "A5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A4" + ":" + "A5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A4" + ":" + "A5").Style.Font.Bold = true;
            ws.Column("A").Width = 15;

            ws.Range("B4" + ":" + "B5").Merge().Value = "Removed Users";
            ws.Range("B4" + ":" + "B5").Style.Font.FontSize = 12;
            ws.Range("B4" + ":" + "B5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("B4" + ":" + "B5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("B4" + ":" + "B5").Style.Font.Bold = true;
            ws.Column("B").Width = 18;

            ws.Range("C4" + ":" + "C5").Merge().Value = "Plays";
            ws.Range("C4" + ":" + "C5").Style.Font.FontSize = 12;
            ws.Range("C4" + ":" + "C5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("C4" + ":" + "C5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("C4" + ":" + "C5").Style.Font.Bold = true;
            ws.Column("C").Width = 10;

            ws.Range("D4" + ":" + "D5").Merge().Value = "Plays (Under 6sec)";
            ws.Range("D4" + ":" + "D5").Style.Font.FontSize = 12;
            ws.Range("D4" + ":" + "D5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("D4" + ":" + "D5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("D4" + ":" + "D5").Style.Font.Bold = true;
            ws.Column("D").Width = 20;

            ws.Range("E4" + ":" + "E5").Merge().Value = "SMS";
            ws.Range("E4" + ":" + "E5").Style.Font.FontSize = 12;
            ws.Range("E4" + ":" + "E5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("E4" + ":" + "E5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("E4" + ":" + "E5").Style.Font.Bold = true;
            ws.Column("E").Width = 10;

            ws.Range("F4" + ":" + "F5").Merge().Value = "Email";
            ws.Range("F4" + ":" + "F5").Style.Font.FontSize = 12;
            ws.Range("F4" + ":" + "F5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("F4" + ":" + "F5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("F4" + ":" + "F5").Style.Font.Bold = true;
            ws.Column("F").Width = 10;

            ws.Range("G4" + ":" + "G5").Merge().Value = "Live Campaign";
            ws.Range("G4" + ":" + "G5").Style.Font.FontSize = 12;
            ws.Range("G4" + ":" + "G5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("G4" + ":" + "G5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("G4" + ":" + "G5").Style.Font.Bold = true;
            ws.Column("G").Width = 18;

            ws.Range("H4" + ":" + "H5").Merge().Value = "Ads provisioned";
            ws.Range("H4" + ":" + "H5").Style.Font.FontSize = 12;
            ws.Range("H4" + ":" + "H5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("H4" + ":" + "H5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("H4" + ":" + "H5").Style.Font.Bold = true;
            ws.Column("H").Width = 20;

            ws.Range("I4" + ":" + "I5").Merge().Value = "Total Spend (in GBP)";
            ws.Range("I4" + ":" + "I5").Style.Font.FontSize = 12;
            ws.Range("I4" + ":" + "I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("I4" + ":" + "I5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("I4" + ":" + "I5").Style.Font.Bold = true;
            ws.Column("I").Width = 25;

            ws.Range("J4" + ":" + "J5").Merge().Value = "Total Credit (in GBP)";
            ws.Range("J4" + ":" + "J5").Style.Font.FontSize = 12;
            ws.Range("J4" + ":" + "J5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("J4" + ":" + "J5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("J4" + ":" + "J5").Style.Font.Bold = true;
            ws.Column("J").Width = 25;

            ws.Range("K4" + ":" + "K5").Merge().Value = "Total Cancel";
            ws.Range("K4" + ":" + "K5").Style.Font.FontSize = 12;
            ws.Range("K4" + ":" + "K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("K4" + ":" + "K5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("K4" + ":" + "K5").Style.Font.Bold = true;
            ws.Column("K").Width = 15;

            ws.Range("L4" + ":" + "L5").Merge().Value = "Average Plays Per User";
            ws.Range("L4" + ":" + "L5").Style.Font.FontSize = 12;
            ws.Range("L4" + ":" + "L5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("L4" + ":" + "L5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("L4" + ":" + "L5").Style.Font.Bold = true;
            ws.Column("L").Width = 25;

            ws.Range("M4" + ":" + "M5").Merge().Value = "Text Files Processed";
            ws.Range("M4" + ":" + "M5").Style.Font.FontSize = 12;
            ws.Range("M4" + ":" + "M5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("M4" + ":" + "M5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("M4" + ":" + "M5").Style.Font.Bold = true;
            ws.Column("M").Width = 25;

            ws.Range("N4" + ":" + "N5").Merge().Value = "Text Lines Processed";
            ws.Range("N4" + ":" + "N5").Style.Font.FontSize = 12;
            ws.Range("N4" + ":" + "N5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("N4" + ":" + "N5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("N4" + ":" + "N5").Style.Font.Bold = true;
            ws.Column("N").Width = 25;



            int first = 5;
            int last = first;
            int excelrowno = first;
            if (mappingResult.Count() > 0)
            {
                for (int i = 0; i < mappingResult.Count(); i++)
                {
                    excelrowno += 1;
                    int j = excelrowno;

                    ws.Cell("A" + j.ToString()).Value = mappingResult[i].NumOfTotalUser.ToString();
                    ws.Cell("B" + j.ToString()).Value = mappingResult[i].NumOfRemovedUser.ToString();
                    ws.Cell("C" + j.ToString()).Value = mappingResult[i].NumOfPlay.ToString();
                    ws.Cell("D" + j.ToString()).Value = mappingResult[i].NumOfPlayUnder6secs.ToString();
                    ws.Cell("E" + j.ToString()).Value = mappingResult[i].NumOfSMS.ToString();
                    ws.Cell("F" + j.ToString()).Value = mappingResult[i].NumOfEmail.ToString();
                    ws.Cell("G" + j.ToString()).Value = mappingResult[i].NumOfLiveCampaign.ToString();
                    ws.Cell("H" + j.ToString()).Value = mappingResult[i].NumberOfAdsProvisioned.ToString();
                    ws.Cell("I" + j.ToString()).Value = mappingResult[i].TotalSpend.ToString("N");
                    ws.Cell("J" + j.ToString()).Value = mappingResult[i].TotalCredit.ToString("N");
                    ws.Cell("K" + j.ToString()).Value = mappingResult[i].NumOfCancel.ToString();
                    ws.Cell("L" + j.ToString()).Value = mappingResult[i].AveragePlaysPerUser.ToString();
                    ws.Cell("M" + j.ToString()).Value = mappingResult[i].NumOfTextFile.ToString();
                    ws.Cell("N" + j.ToString()).Value = mappingResult[i].NumOfUpdateToAudit.ToString();
                }
            }

            string fileName = "Management Report.csv";
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                //Return xlsx/csv Excel File  
                return File(MyMemoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}