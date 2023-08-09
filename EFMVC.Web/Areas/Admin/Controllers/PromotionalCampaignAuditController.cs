using AutoMapper;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.Web.Areas.Admin.SearchClass;
using EFMVC.Web.Areas.Admin.SearchClass.Models;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using EFMVC.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("PromotionalCampaignAudit")]
    public class PromotionalCampaignAuditController : Controller
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
        /// The _promotionalCampaignAudit Repository
        /// </summary>
        private readonly IPromotionalCampaignAuditRepository _promotionalCampaignAuditRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        //string connectionString = @"Data Source=ZWT048\SQLEXPRESS2017;Initial Catalog=Arthar;User ID=sa;Password=this.admin";
        string connectionString = @"Data Source=S20494255;Initial Catalog=Arthar;User ID=sa;Password=z_nTJG@5TB";

        public PromotionalCampaignAuditController(ICommandBus commandBus, ICountryRepository countryRepository, IOperatorRepository operatorRepository, IPromotionalUserRepository promotionalUserRepository, IPromotionalCampaignRepository promotionalCampaignRepository, IPromotionalAdvertRepository promotionalAdvertRepository, IPromotionalCampaignAuditRepository promotionalCampaignAuditRepository)
        {
            _commandBus = commandBus;
            _countryRepository = countryRepository;
            _operatorRepository = operatorRepository;
            _promotionalUserRepository = promotionalUserRepository;
            _promotionalCampaignRepository = promotionalCampaignRepository;
            _promotionalAdvertRepository = promotionalAdvertRepository;
            _promotionalCampaignAuditRepository = promotionalCampaignAuditRepository;
        }

        // GET: Admin/PromotionalCampaignAudit
        [Route("Index")]
        [Route("{campaignId}")]
        public ActionResult Index(int campaignId)
        {
            PromotionalCampaignAuditDashboardResult promotionalCampaignAuditDashboardResult = new PromotionalCampaignAuditDashboardResult();
            PromotionalCampaignAuditFilter promotionalCampaignAuditFilter = new PromotionalCampaignAuditFilter();
            List<PromotionalCampaignAuditResult> promotionalCampaignAuditResult = new List<PromotionalCampaignAuditResult>();
            PromotionalCampaignAuditMapping _mapping = new PromotionalCampaignAuditMapping();

            PromotionalCampaign promotionalCampaign = _promotionalCampaignRepository.Get(x => x.ID == campaignId);
            TempData["CampaignId"] = campaignId.ToString();
            ViewBag.campaignName = promotionalCampaign.CampaignName;

            int totalReach = 0;
            int totalPlayCount;
            List<MaxLengthGroup> _playgroup = new List<MaxLengthGroup>();
            List<PromotionalCampaignAudit> promotionalCampaignAudit = _promotionalCampaignAuditRepository.GetMany(top => top.PromotionalCampaignId == campaignId).ToList();
            if (promotionalCampaignAudit.Count() > 0)
            {
                totalPlayCount = promotionalCampaignAudit.Count();
                totalReach = promotionalCampaignAudit.Where(top => top.DTMFKey != "0" && top.DTMFKey != null).Select(top => top.MSISDN).Distinct().Count();

                promotionalCampaignAuditDashboardResult.PlaystoDate = totalPlayCount;
                promotionalCampaignAuditDashboardResult.TotalReach = totalReach;

                var playLengthTicksSum = promotionalCampaignAudit.Select(s => s.PlayLengthTicks).Sum();
                var sumOfSecond = Convert.ToDouble(playLengthTicksSum) / 1000;
                double AveragePlayTime = (sumOfSecond / totalPlayCount);

                promotionalCampaignAuditDashboardResult.AveragePlayTime = AveragePlayTime;

                List<long> _maxplaylength = new List<long>();
                _maxplaylength.Add(promotionalCampaignAudit.Max(top => top.PlayLengthTicks));
                if (_maxplaylength.Count > 0)
                {
                    var maxPlayLength = (_maxplaylength.Max()) / 1000;
                    promotionalCampaignAuditDashboardResult.MaxPlayLength = maxPlayLength;
                    promotionalCampaignAuditDashboardResult.MaxPlayLengthPercantage = RoundUp(((maxPlayLength / AveragePlayTime)) * 100, 2);
                    var playtimeDecimal = Convert.ToDecimal(promotionalCampaignAuditDashboardResult.AveragePlayTime);
                    promotionalCampaignAuditDashboardResult.MaxPlayLength = playtimeDecimal;
                }
                else
                {
                    promotionalCampaignAuditDashboardResult.MaxPlayLength = 0;
                    promotionalCampaignAuditDashboardResult.MaxPlayLengthPercantage = 0;
                }

                if (promotionalCampaignAudit.Count() > 0)
                {
                    _playgroup = (from sl in promotionalCampaignAudit select new MaxLengthGroup() { second = sl.PlayLengthTicks }).ToList();
                    int[] _getbarChartdata = _getbarChartData(_playgroup);
                    if (_getbarChartdata == null) ViewBag.getbarChartdata = _getbarChartdata;
                    else ViewBag.getbarChartdata = _getbarChartdata.ToArray();
                }
                else
                {
                    _playgroup.Add(new MaxLengthGroup { second = 0 });
                    int[] _getbarChartdata = _getbarChartData(_playgroup);
                    if (_getbarChartdata == null) ViewBag.getbarChartdata = _getbarChartdata;
                    else ViewBag.getbarChartdata = _getbarChartdata.ToArray();
                }
            }
            else
            {
                promotionalCampaignAuditDashboardResult.PlaystoDate = 0;
                promotionalCampaignAuditDashboardResult.TotalReach = 0;
                promotionalCampaignAuditDashboardResult.AveragePlayTime = 0;
                promotionalCampaignAuditDashboardResult.MaxPlayLength = 0;
                promotionalCampaignAuditDashboardResult.MaxPlayLengthPercantage = 0;
                _playgroup.Add(new MaxLengthGroup { second = 0 });
                int[] _getbarChartdata2 = _getbarChartData(_playgroup);
                ViewBag.getbarChartdata = _getbarChartdata2.ToArray();
            }

            _mapping.promotionalCampaignAuditDashboardResult = promotionalCampaignAuditDashboardResult;
            _mapping.promotionalCampaignAuditFilter = promotionalCampaignAuditFilter;
            _mapping.promotionalCampaignAuditResult = promotionalCampaignAuditResult;
            ViewBag.TotalReach = RoundUp(totalReach == 0 ? 0 : totalReach, 2);
            return View(_mapping);
        }

        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                string CampaignId = TempData["CampaignId"].ToString();
                bool searchValue = false;
                List<String> columnSearch = new List<string>();
                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                    if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                        searchValue = true;
                }
                int cnt = 10;
                int id = 0;
                if (!string.IsNullOrEmpty(CampaignId)) id = Convert.ToInt32(CampaignId);
                List<PromotionalCampaignAuditResult> promotionalCampaignAuditResult = new List<PromotionalCampaignAuditResult>();
                List<PromotionalCampaignAudit> promotionalCampaignAudit = new List<PromotionalCampaignAudit>();
                if (_promotionalCampaignAuditRepository.Count(x => x.PromotionalCampaignId == id) > 0)
                {
                    #region Search And Load
                    if (searchValue == true)
                    {
                        #region Searching Functionality
                        string MSISDN = "", DTMF = ""; double fromLengthOfPlay = 0, toLengthOfPlay = 0;
                        DateTime fromStartTime = new DateTime();
                        DateTime toStartTime = new DateTime();
                        DateTime fromEndTime = new DateTime();
                        DateTime toEndTime = new DateTime();
                        if (!String.IsNullOrEmpty(columnSearch[0]))
                        {
                            if (columnSearch[0] != "null") MSISDN = columnSearch[0].ToString();
                            else columnSearch[0] = null;
                        }
                        if (!String.IsNullOrEmpty(columnSearch[1]))
                        {
                            if (columnSearch[1] != "null")
                            {
                                var data = columnSearch[1].Split(',').ToArray();
                                fromLengthOfPlay = Convert.ToDouble(data[0]);
                                toLengthOfPlay = Convert.ToDouble(data[1]);
                            }
                            else columnSearch[1] = null;
                        }
                        if (!String.IsNullOrEmpty(columnSearch[2]))
                        {
                            if (columnSearch[2] != "null") DTMF = columnSearch[2].ToString();
                            else columnSearch[2] = null;
                        }

                        if (!String.IsNullOrEmpty(columnSearch[3]))
                        {
                            if (columnSearch[3] != "null")
                            {
                                var data = columnSearch[3].Split(',').ToArray();

                                DateTime from = DateTime.Parse(data[0].Substring(11, 8));
                                string fromDateTime = data[0].Substring(0, 11) + Convert.ToString(from.ToString("HH:mm:ss"));
                                fromStartTime = DateTime.ParseExact(fromDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                                DateTime to = DateTime.Parse(data[1].Substring(11, 8));
                                string toDateTime = data[1].Substring(0, 11) + Convert.ToString(to.ToString("HH:mm:ss"));
                                toStartTime = DateTime.ParseExact(toDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            }
                            else columnSearch[3] = null;
                        }

                        if (!String.IsNullOrEmpty(columnSearch[4]))
                        {
                            if (columnSearch[4] != "null")
                            {
                                var data = columnSearch[4].Split(',').ToArray();

                                DateTime from = DateTime.Parse(data[0].Substring(11, 8));
                                string fromDateTime = data[0].Substring(0, 11) + Convert.ToString(from.ToString("HH:mm:ss"));
                                fromEndTime = DateTime.ParseExact(fromDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                                DateTime to = DateTime.Parse(data[1].Substring(11, 8));
                                string toDateTime = data[1].Substring(0, 11) + Convert.ToString(to.ToString("HH:mm:ss"));
                                toEndTime = DateTime.ParseExact(toDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            }
                            else columnSearch[4] = null;
                        }

                        DataTable dt1 = ExecuteSPForGetData("GetPromotionalCampaignAuditData", id, connectionString);
                        promotionalCampaignAudit = CommonMethod.ConvertToList<PromotionalCampaignAudit>(dt1);
                        if (columnSearch[0] != null)
                        {
                            promotionalCampaignAudit = promotionalCampaignAudit.Where(top => MSISDN.Equals(top.MSISDN)).ToList();
                        }
                        if (columnSearch[1] != null)
                        {
                            promotionalCampaignAudit = promotionalCampaignAudit.Where(top => ((RoundUp(Convert.ToDouble(top.PlayLengthTicks) / 1000, 2)) >= fromLengthOfPlay && (RoundUp(Convert.ToDouble(top.PlayLengthTicks) / 1000, 2)) <= toLengthOfPlay)).ToList();
                        }
                        if (columnSearch[2] != null)
                        {
                            promotionalCampaignAudit = promotionalCampaignAudit.Where(top => DTMF.Equals(top.DTMFKey)).ToList();
                        }
                        if (columnSearch[3] != null)
                        {
                            promotionalCampaignAudit = promotionalCampaignAudit.Where(top => top.StartTime >= fromStartTime && top.StartTime <= toStartTime).ToList();
                        }
                        if (columnSearch[4] != null)
                        {
                            promotionalCampaignAudit = promotionalCampaignAudit.Where(top => top.EndTime >= fromEndTime && top.EndTime <= toEndTime).ToList();
                        }
                        cnt = promotionalCampaignAudit.Count();
                        promotionalCampaignAudit = promotionalCampaignAudit.Skip(param.Start).Take(param.Length).ToList();
                        #endregion
                    }
                    else
                    {
                        DataTable dt1 = ExecuteSPForGetData("GetPromotionalCampaignAuditData", id, connectionString);
                        promotionalCampaignAudit = CommonMethod.ConvertToList<PromotionalCampaignAudit>(dt1);
                        cnt = promotionalCampaignAudit.Count();
                        promotionalCampaignAudit = promotionalCampaignAudit.Skip(param.Start).Take(param.Length).ToList();
                    }
                    #endregion

                    foreach (var item in promotionalCampaignAudit)
                    {
                        var lengthOfPlay = Convert.ToDouble(item.PlayLengthTicks) / 1000;
                        string campaignName = _promotionalCampaignRepository.GetById(item.PromotionalCampaignId).CampaignName.ToString();
                        string advertName = _promotionalAdvertRepository.Get(top => top.CampaignID == item.PromotionalCampaignId).AdvertName.ToString();
                        string operatorName = _promotionalCampaignRepository.Get(top => top.ID == item.PromotionalCampaignId).Operator.OperatorName.ToString();
                        promotionalCampaignAuditResult.Add(
                            new PromotionalCampaignAuditResult
                            {
                                PromotionalCampaignAuditId = item.PromotionalCampaignAuditId,
                                OperatorName = operatorName,
                                PromotionalCampaignName = campaignName,
                                PromotionalAdvertName = advertName,
                                MSISDN = item.MSISDN,
                                StartTime = item.StartTime,
                                EndTime = item.EndTime,
                                PlayLengthTicks = RoundUp(lengthOfPlay, 2),
                                DTMFKey = item.DTMFKey == null ? "-" : item.DTMFKey,
                                DisplayStartTime = item.StartTime.ToString("dd/MM/yyyy hh:mm:ss tt"),
                                DisplayEndTime = item.EndTime.ToString("dd/MM/yyyy hh:mm:ss tt")
                            }
                            );
                    }
                }
                TempData["CampaignId"] = CampaignId.ToString();
                promotionalCampaignAuditResult = ApplySortingCampaigm(param, promotionalCampaignAuditResult);
                DTResult<PromotionalCampaignAuditResult> result = new DTResult<PromotionalCampaignAuditResult>
                {
                    draw = param.Draw,
                    data = promotionalCampaignAuditResult,
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
        private static List<PromotionalCampaignAuditResult> ApplySortingCampaigm(DTParameters param, List<PromotionalCampaignAuditResult> promotionalCampaignAuditResult)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderBy(top => top.OperatorName).ToList();
                    else
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderByDescending(top => top.OperatorName).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderBy(top => top.PromotionalCampaignName).ToList();
                    else
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderByDescending(top => top.PromotionalCampaignName).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderBy(top => top.PromotionalAdvertName).ToList();
                    else
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderByDescending(top => top.PromotionalAdvertName).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderBy(top => top.MSISDN).ToList();
                    else
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderByDescending(top => top.MSISDN).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderBy(top => top.DisplayStartTime).ToList();
                    else
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderByDescending(top => top.DisplayStartTime).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderBy(top => top.DisplayEndTime).ToList();
                    else
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderByDescending(top => top.DisplayEndTime).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderBy(top => top.PlayLengthTicks).ToList();
                    else
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderByDescending(top => top.PlayLengthTicks).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderBy(top => top.DTMFKey).ToList();
                    else
                        promotionalCampaignAuditResult = promotionalCampaignAuditResult.OrderByDescending(top => top.DTMFKey).ToList();
                }
            }
            return promotionalCampaignAuditResult;
        }

        public int[] _getbarChartData(List<MaxLengthGroup> _data)
        {
            int[] _barData = new int[9];
            if (_data.Count > 0)
            {
                _barData[0] = _data.Where(top => top.second >= 6 && top.second <= 50).Count();
                _barData[1] = _data.Where(top => top.second >= 9 && top.second <= 100).Count();
                _barData[2] = _data.Where(top => top.second >= 12 && top.second <= 200).Count();
                _barData[3] = _data.Where(top => top.second >= 15 && top.second <= 300).Count();
                _barData[4] = _data.Where(top => top.second >= 18 && top.second <= 400).Count();
                _barData[5] = _data.Where(top => top.second >= 21 && top.second <= 500).Count();
                _barData[6] = _data.Where(top => top.second >= 24 && top.second <= 600).Count();
                _barData[7] = _data.Where(top => top.second >= 27 && top.second <= 700).Count();
                _barData[8] = _data.Where(top => top.second >= 30 && top.second <= 999).Count();
            }
            else
            {
                return null;
            }
            return _barData;
        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        public DataTable ExecuteSPForGetData(string spname, int paramname, string conn)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PromotionalCampaignId", paramname);
                        DataTable dt = new DataTable();
                        con.Open();
                        da.Fill(dt);
                        con.Close();
                        return dt;
                    }
                }
            }
        }

        public static class CommonMethod
        {
            public static List<T> ConvertToList<T>(DataTable dt)
            {
                var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
                var properties = typeof(T).GetProperties();
                return dt.AsEnumerable().Select(row =>
                {
                    var objT = Activator.CreateInstance<T>();
                    foreach (var pro in properties)
                    {
                        if (columnNames.Contains(pro.Name.ToLower()))
                        {
                            try
                            {
                                pro.SetValue(objT, row[pro.Name]);
                            }
                            catch (Exception ex) { }
                        }
                    }
                    return objT;
                }).ToList();
            }
        }

        [HttpGet]
        [Route("Test")]
        public ActionResult Test()
        {
            var localRoot = @"E:/Projects Sample/file_example_JPG_1MB.jpg";
            if (!System.IO.File.Exists(localRoot))
            {
                string test = "test";
            }
            //System.Web.HttpContext.Current.Server.MapPath("~/Media");
            return View(localRoot);
        }
    }
}