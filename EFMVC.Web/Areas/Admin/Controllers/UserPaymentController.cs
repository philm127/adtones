using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Data.Repositories;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Web.Areas.Admin.ViewModel;
using EFMVC.Domain.Commands;
using EFMVC.Web.ViewModels;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using EFMVC.Web.Areas.UsersAdmin.Models;
using EFMVC.Model;
using System.Globalization;
using EFMVC.Web.Models;
using Microsoft.Ajax.Utilities;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("UserPayment")]
    public class UserPaymentController : Controller
    {
        //
        // GET: /Admin/UserPayment/

        //

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileRepository;
        /// <summary>
        /// The _usercredit repository
        /// </summary>
        private readonly IUsersCreditRepository _usercreditRepository;
        /// <summary>
        /// The _usercreditpayment repository
        /// </summary>
        private readonly IUsersCreditPaymentRepository _usercreditpaymentRepository;

        /// <summary>
        /// The _billing repository
        /// </summary>
        private readonly IBillingRepository _billingRepository;

        /// <summary>
        /// The _companydetails repository
        /// </summary>
        private readonly ICompanyDetailsRepository _companydetailsRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public UserPaymentController(ICommandBus commandBus, IUserRepository userRepository, IUsersCreditRepository usercreditRepository, IUsersCreditPaymentRepository usercreditpaymentRepository, IBillingRepository billingRepository, IClientRepository clientRepository, ICampaignProfileRepository profileRepository, ICompanyDetailsRepository companydetailsRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _usercreditpaymentRepository = usercreditpaymentRepository;
            _billingRepository = billingRepository;
            _usercreditRepository = usercreditRepository;
            _clientRepository = clientRepository;
            _profileRepository = profileRepository;
            _companydetailsRepository = companydetailsRepository;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            //List<UserCreditPaymentResult> _result = FillUserCreditPaymentResult();
            List<UserCreditPaymentResult> _result = new List<UserCreditPaymentResult>();
            FillUserDropdown(null);
            FillClientDropdown();
            FillCampaignDropdown();
            SearchClass.UserCreditPaymentFilter _filterCritearea = new SearchClass.UserCreditPaymentFilter();
            return View(Tuple.Create(_result, _filterCritearea));

        }

        //Add 26-02-2019
        [Route("FillUserDropdownAJAX")]
        [HttpPost]
        public ActionResult FillUserDropdownAJAX(string UserName)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName) && top.RoleId == 3 && top.Activated == 1).Select(top => new
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

        public void FillClientDropdown()
        {
            var clientdetails = _clientRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id,
            }).ToList();
            ViewBag.clientdetails = new MultiSelectList(clientdetails, "Id", "Name");

        }
        public void FillCampaignDropdown()
        {
            var campaigndetails = _profileRepository.GetAll().Select(top => new
            {
                Name = top.CampaignName,
                Id = top.CampaignProfileId,
            }).ToList();
            ViewBag.campaigns = new MultiSelectList(campaigndetails, "Id", "Name");

        }
        public void FillUserDropdown(int? userId)
        {
            if (userId != null)
            {
                //var userdetails = _userRepository.GetAll().Where(top => top.UserId == userId).Select(top => new
                //{
                //    Name = top.FirstName + " " + top.LastName + "(" + top.Email + ")",
                //    UserId = top.UserId,
                //}).ToList();
                var userdetails = _userRepository.GetMany(top => top.UserId == userId && top.Activated == 1 && top.RoleId == 3).Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName + "(" + top.Email + ")",
                    UserId = top.UserId,
                }).ToList();
                ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
            }
            else
            {
                //var userdetails = _userRepository.GetAll().Select(top => new
                //{
                //    Name = top.FirstName + " " + top.LastName + "(" + top.Email + ")",
                //    UserId = top.UserId,
                //}).ToList();
                var userdetails = _userRepository.GetMany(top =>  top.Activated == 1 && top.RoleId == 3).Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName + "(" + top.Email + ")",
                    UserId = top.UserId,
                }).ToList();
                ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
            }

        }
        //public List<UserCreditPaymentResult> FillUserCreditPaymentResult()
        //{
        //    List<UserCreditPaymentResult> _usercreditResult = new List<UserCreditPaymentResult>();
        //    var result = _usercreditpaymentRepository.GetAll().OrderByDescending(top => top.CreatedDate);
        //    foreach (var item in result)
        //    {
        //        //var billingamount = _billingRepository.GetAll().Where(top => top.Id == item.BillingId).Sum(top => top.FundAmount);
        //        //var clientId = item.Billing.ClientId == null ? 0 : item.Billing.ClientId;
        //        //var outstandingAmount = billingamount - item.Amount;
        //        //if (outstandingAmount != 0)
        //        //{
        //        //    _usercreditResult.Add(new UserCreditPaymentResult { Id = item.Id, UserId = item.UserId, Email = item.User.Email, Name = item.User.FirstName + " " + item.User.LastName, CreatedDate = item.CreatedDate, Amount = item.Amount, Description = item.Description, Status = item.Status, CampaignName = item.Billing.CampaignProfile.CampaignName, CampaignProfileId = (int)item.Billing.CampaignProfileId, ClientId = clientId, ClientName = clientId == 0 ? "-" : item.Billing.Client.Name, InvoiceNumber = item.Billing.InvoiceNumber, TotalAmount = billingamount, OutstandingAmount = outstandingAmount, Organisation = item.User.Organisation });
        //        //}
        //        //New
        //        //PaymentMethodId = 1 Credit
        //        var totalAmount = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item.CampaignProfileId).Sum(s => s.Amount);
        //        var billingamount = _billingRepository.GetMany(s => s.CampaignProfileId == item.CampaignProfileId && s.PaymentMethodId == 1).Sum(top => top.FundAmount);
        //        var clientId = item.Billing.ClientId == null ? 0 : item.Billing.ClientId;
        //        var outstandingAmount = billingamount - totalAmount;
        //        if (outstandingAmount > 0)
        //        {
        //            _usercreditResult.Add(new UserCreditPaymentResult { Id = item.Id, UserId = item.UserId, Email = item.User.Email, Name = item.User.FirstName + " " + item.User.LastName, CreatedDate = item.CreatedDate, Amount = item.Amount, Description = item.Description, Status = item.Status, CampaignName = item.Billing.CampaignProfile.CampaignName, CampaignProfileId = (int)item.Billing.CampaignProfileId, ClientId = clientId, ClientName = clientId == 0 ? "-" : item.Billing.Client.Name, InvoiceNumber = item.Billing.InvoiceNumber, TotalAmount = billingamount, OutstandingAmount = outstandingAmount, Organisation = item.User.Organisation });
        //        }
        //    }
        //    return _usercreditResult;
        //}


        //[HttpPost]
        //public JsonResult LoadData(DTParameters param)
        //{
        //    try
        //    {
        //        List<UserCreditPaymentResult> _resultFinal = new List<UserCreditPaymentResult>();


        //        IEnumerable<UsersCreditPayment> usersCreditPayments = null;

        //        ViewBag.SearchResult = false;
        //        var cnt = 10;
        //      //  int userId = 0;

        //        bool searchValue = false;
        //        List<String> columnSearch = new List<string>();

        //        foreach (var col in param.Columns)
        //        {
        //            columnSearch.Add(col.Search.Value);
        //            if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
        //                searchValue = true;
        //        }

        //        //if (param.Columns.First().Search.Value == "0")
        //        //{
        //        //    usersCreditPayments = _profileRepository.GetAll().OrderByDescending(top => top.CreatedDateTime).ToList();
        //        //    cnt = usersCreditPayments.Count();
        //        //    usersCreditPayments = usersCreditPayments.Skip(param.Start).Take(param.Length);
        //        //}

        //        if (searchValue == true)
        //        {
        //            #region Search Functionality
        //            int[] UserId = new int[cnt];
        //            int[] ClientId = new int[cnt];
        //            int[] CampaignId = new int[cnt];
        //            string Invoice = "";
        //            int FromAmt = 0;
        //            int ToAmt = 0;
        //            DateTime fromdate = new DateTime();
        //            DateTime todate = new DateTime();
        //            if (!String.IsNullOrEmpty(columnSearch[0]))
        //            {
        //                if (columnSearch[0] != "null")
        //                {
        //                    UserId = columnSearch[0].Split(',').Select(int.Parse).ToArray();
        //                }
        //                else
        //                {
        //                    columnSearch[0] = null;
        //                }
        //            }

        //            if (!String.IsNullOrEmpty(columnSearch[1]))
        //            {
        //                if (columnSearch[1] != "null")
        //                {
        //                    ClientId = columnSearch[1].Split(',').Select(int.Parse).ToArray();
        //                }
        //                else
        //                {
        //                    columnSearch[1] = null;
        //                }
        //            }

        //            if (!String.IsNullOrEmpty(columnSearch[2]))
        //            {
        //                if (columnSearch[2] != "null")
        //                {
        //                    CampaignId = columnSearch[2].Split(',').Select(int.Parse).ToArray();
        //                }
        //                else
        //                {
        //                    columnSearch[2] = null;
        //                }
        //            }

        //            if (!String.IsNullOrEmpty(columnSearch[3]))
        //            {
        //                if (columnSearch[3] != "null")
        //                {
        //                    Invoice = columnSearch[3].ToString();
        //                }
        //                else
        //                {
        //                    columnSearch[3] = null;
        //                }
        //            }

        //            if (!String.IsNullOrEmpty(columnSearch[4]))
        //            {
        //                if (columnSearch[4] != "null")
        //                {
        //                    var data = columnSearch[4].Split(',').ToArray();
        //                    FromAmt = Convert.ToInt32(data[0]);
        //                    ToAmt = Convert.ToInt32(data[1]);
        //                }
        //                else
        //                {
        //                    columnSearch[4] = null;
        //                }
        //            }

        //            if (!String.IsNullOrEmpty(columnSearch[5]))
        //            {
        //                if (columnSearch[5] != "null")
        //                {
        //                    var data = columnSearch[5].Split(',').ToArray();

        //                    //fromdate = Convert.ToDateTime(data[0]);
        //                    //todate = Convert.ToDateTime(data[1]);

        //                    string strTodate = data[0];
        //                    fromdate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                    string strFromdate = data[1];
        //                    todate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                }
        //                else
        //                {
        //                    columnSearch[5] = null;

        //                }
        //            }

        //             usersCreditPayments = _usercreditpaymentRepository.GetAll().OrderByDescending(top => top.CreatedDate);
        //            if (columnSearch[0] != null)
        //            {
        //                usersCreditPayments = usersCreditPayments.Where(top => (UserId.Contains(top.UserId))).ToList();
        //            }
        //            if (columnSearch[1] != null)
        //            {
        //                //usersCreditPayments = usersCreditPayments.Where(top => (ClientId.Contains((int)top.Billing.ClientId))).ToList();
        //                usersCreditPayments = usersCreditPayments.Where(top => (ClientId.Contains((int)(top.Billing.ClientId == null ? 0 : top.Billing.ClientId)))).ToList();
        //            }
        //            if (columnSearch[2] != null)
        //            {
        //                usersCreditPayments = usersCreditPayments.Where(top => (CampaignId.Contains((int)top.Billing.CampaignProfileId))).ToList();
        //            }
        //            if (columnSearch[3] != null)
        //            {
        //                usersCreditPayments = usersCreditPayments.Where(top => (Invoice.Contains(top.Billing.InvoiceNumber))).ToList();
        //            }
        //            if (columnSearch[4] != null)
        //            {
        //                usersCreditPayments = usersCreditPayments.Where(top => (top.Amount >= FromAmt && top.Amount <= ToAmt)).ToList();
        //            }
        //            if (columnSearch[5] != null)
        //            {
        //                usersCreditPayments = usersCreditPayments.Where(top => (top.CreatedDate >= fromdate && top.CreatedDate <= todate)).ToList();
        //            }
        //            //usersCreditPayments = _usercreditpaymentRepository.GetMany(top => (UserId.Contains(top.UserId)) && (ClientId.Contains((int)top.Billing.ClientId)) && (Invoice.Contains(top.Billing.InvoiceNumber)) && (CampaignId.Contains((int)top.Billing.CampaignProfileId)) && (top.Amount >= FromAmt && top.Amount <= ToAmt) && (top.CreatedDate >= fromdate && top.CreatedDate <= todate));

        //            cnt = usersCreditPayments.Count();

        //            usersCreditPayments = usersCreditPayments.Skip(param.Start).Take(param.Length);

        //            //campaignProfileFormModels =
        //            //    Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(usersCreditPayments);
        //            #endregion

        //        }
        //        else
        //        {
        //            //usersCreditPayments = _profileRepository.GetAll().Skip(param.Start).Take(param.Length);
        //            //if (usersCreditPayments.Count() >= 10)
        //            //    cnt = _profileRepository.GetAll().Count(); 

        //            usersCreditPayments = _usercreditpaymentRepository.GetAll().OrderByDescending(top => top.CreatedDate);
        //            cnt = usersCreditPayments.Count();
        //            usersCreditPayments = usersCreditPayments.Skip(param.Start).Take(param.Length);
        //        }

        //        if (Session["UserID"] != null)
        //        {
        //            int uId = (int)Session["UserID"];
        //            usersCreditPayments = _usercreditpaymentRepository.GetMany(top => top.UserId == uId);
        //            if (usersCreditPayments.Count() > 10)
        //                cnt = usersCreditPayments.Count();
        //        }
        //        if (Session["CampaignID"] != null)
        //        {
        //            int campId = (int)Session["CampaignID"];
        //            usersCreditPayments = _usercreditpaymentRepository.GetMany(top => top.Billing.CampaignProfileId == campId);
        //            if (usersCreditPayments.Count() > 10)
        //                cnt = usersCreditPayments.Count();
        //        }
        //        foreach (var item in usersCreditPayments)
        //        {
        //            var billingamount = _billingRepository.GetAll().Where(top => top.Id == item.BillingId).Sum(top => top.FundAmount);
        //            var clientId = item.Billing.ClientId == null ? 0 : item.Billing.ClientId;
        //            _resultFinal.Add(new UserCreditPaymentResult { Id = item.Id, UserId = item.UserId, Email = item.User.Email, Name = item.User.FirstName + " " + item.User.LastName, CreatedDate = item.CreatedDate, Amount = item.Amount, Description = item.Description, Status = item.Status, CampaignName = item.Billing.CampaignProfile.CampaignName, CampaignProfileId = (int)item.Billing.CampaignProfileId, ClientId = clientId, ClientName = clientId == 0 ? "-" : item.Billing.Client.Name, InvoiceNumber = item.Billing.InvoiceNumber, TotalAmount = billingamount, OutstandingAmount = billingamount - item.Amount, Organisation = item.User.Organisation });
        //        }
        //        Session["UserID"] = null;
        //        Session["CampaignID"] = null;


        //        _resultFinal = ApplySorting(param, _resultFinal);

        //        DTResult<UserCreditPaymentResult> result = new DTResult<UserCreditPaymentResult>
        //        {
        //            draw = param.Draw,
        //            data = _resultFinal,
        //            recordsFiltered = cnt,
        //            recordsTotal = cnt
        //        };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { error = ex.Message });
        //    }
        //}

        public List<UserCreditPaymentResult> FillUserCreditPaymentResult()
        {
            List<UserCreditPaymentResult> _usercreditResult = new List<UserCreditPaymentResult>();

            var result = _billingRepository.GetMany(s => s.PaymentMethodId == 1).OrderByDescending(s => s.Id).DistinctBy(s => s.CampaignProfileId).ToList();
            foreach (var item in result)
            {
                var totalAmount = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.CampaignProfileId == item.CampaignProfileId).Sum(top => top.FundAmount);
                var paidAmount = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item.CampaignProfileId).Sum(s => s.Amount);
                var outStandingAmount = totalAmount - paidAmount;
                var clientId = item.ClientId == null ? 0 : item.ClientId;
                var description = "-";
                var userCreditPaymentData = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item.CampaignProfileId).OrderByDescending(s => s.Id).FirstOrDefault();
                if (userCreditPaymentData != null)
                {
                    description = userCreditPaymentData.Description;
                }
                int status = 0;
                if (paidAmount > 0)
                {
                    status = 1;
                }
                if (outStandingAmount > 0)
                {
                    _usercreditResult.Add(new UserCreditPaymentResult
                    {
                        Id = item.Id,
                        UserId = (int)item.UserId,
                        Email = item.User.Email,
                        Name = item.User.FirstName + " " + item.User.LastName,
                        CreatedDate = item.PaymentDate.ToString("dd/MM/yyyy"),
                        CreatedDateSort = item.PaymentDate,
                        Amount = paidAmount,
                        Description = description,
                        Status = status,
                        CampaignName = item.CampaignProfile.CampaignName,
                        CampaignProfileId = (int)item.CampaignProfileId,
                        ClientId = clientId,
                        ClientName = clientId == 0 ? "-" : item.Client.Name,
                        InvoiceNumber = item.InvoiceNumber,
                        TotalAmount = totalAmount,
                        OutstandingAmount = outStandingAmount,
                        Organisation = item.User.Organisation
                    });
                }
            }

            return _usercreditResult;
        }

        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                List<UserCreditPaymentResult> _resultFinal = new List<UserCreditPaymentResult>();


                // IEnumerable<UsersCreditPayment> usersCreditPayments = null;
                IEnumerable<Billing> billingDetails = null;

                ViewBag.SearchResult = false;
                var cnt = 10;

                bool searchValue = false;
                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                    if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                        searchValue = true;
                }


                if (searchValue == true)
                {
                    #region Search Functionality
                    //int[] UserId = new int[cnt];
                    //int[] ClientId = new int[cnt];
                    //int[] CampaignId = new int[cnt];
                    //string Invoice = "";
                    //int FromAmt = 0;
                    //int ToAmt = 0;
                    //DateTime fromdate = new DateTime();
                    //DateTime todate = new DateTime();
                    //if (!String.IsNullOrEmpty(columnSearch[0]))
                    //{
                    //    if (columnSearch[0] != "null")
                    //    {
                    //        UserId = columnSearch[0].Split(',').Select(int.Parse).ToArray();
                    //    }
                    //    else
                    //    {
                    //        columnSearch[0] = null;
                    //    }
                    //}

                    //if (!String.IsNullOrEmpty(columnSearch[1]))
                    //{
                    //    if (columnSearch[1] != "null")
                    //    {
                    //        ClientId = columnSearch[1].Split(',').Select(int.Parse).ToArray();
                    //    }
                    //    else
                    //    {
                    //        columnSearch[1] = null;
                    //    }
                    //}

                    //if (!String.IsNullOrEmpty(columnSearch[2]))
                    //{
                    //    if (columnSearch[2] != "null")
                    //    {
                    //        CampaignId = columnSearch[2].Split(',').Select(int.Parse).ToArray();
                    //    }
                    //    else
                    //    {
                    //        columnSearch[2] = null;
                    //    }
                    //}

                    //if (!String.IsNullOrEmpty(columnSearch[3]))
                    //{
                    //    if (columnSearch[3] != "null")
                    //    {
                    //        Invoice = columnSearch[3].ToString();
                    //    }
                    //    else
                    //    {
                    //        columnSearch[3] = null;
                    //    }
                    //}

                    //if (!String.IsNullOrEmpty(columnSearch[4]))
                    //{
                    //    if (columnSearch[4] != "null")
                    //    {
                    //        var data = columnSearch[4].Split(',').ToArray();
                    //        FromAmt = Convert.ToInt32(data[0]);
                    //        ToAmt = Convert.ToInt32(data[1]);
                    //    }
                    //    else
                    //    {
                    //        columnSearch[4] = null;
                    //    }
                    //}

                    //if (!String.IsNullOrEmpty(columnSearch[5]))
                    //{
                    //    if (columnSearch[5] != "null")
                    //    {
                    //        var data = columnSearch[5].Split(',').ToArray();

                    //        //fromdate = Convert.ToDateTime(data[0]);
                    //        //todate = Convert.ToDateTime(data[1]);

                    //        string strTodate = data[0];
                    //        fromdate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //        string strFromdate = data[1];
                    //        todate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //    }
                    //    else
                    //    {
                    //        columnSearch[5] = null;

                    //    }
                    //}

                    //usersCreditPayments = _usercreditpaymentRepository.GetAll().OrderByDescending(top => top.CreatedDate);
                    //if (columnSearch[0] != null)
                    //{
                    //    usersCreditPayments = usersCreditPayments.Where(top => (UserId.Contains(top.UserId))).ToList();
                    //}
                    //if (columnSearch[1] != null)
                    //{
                    //    //usersCreditPayments = usersCreditPayments.Where(top => (ClientId.Contains((int)top.Billing.ClientId))).ToList();
                    //    usersCreditPayments = usersCreditPayments.Where(top => (ClientId.Contains((int)(top.Billing.ClientId == null ? 0 : top.Billing.ClientId)))).ToList();
                    //}
                    //if (columnSearch[2] != null)
                    //{
                    //    usersCreditPayments = usersCreditPayments.Where(top => (CampaignId.Contains((int)top.Billing.CampaignProfileId))).ToList();
                    //}
                    //if (columnSearch[3] != null)
                    //{
                    //    usersCreditPayments = usersCreditPayments.Where(top => (Invoice.Contains(top.Billing.InvoiceNumber))).ToList();
                    //}
                    //if (columnSearch[4] != null)
                    //{
                    //    usersCreditPayments = usersCreditPayments.Where(top => (top.Amount >= FromAmt && top.Amount <= ToAmt)).ToList();
                    //}
                    //if (columnSearch[5] != null)
                    //{
                    //    usersCreditPayments = usersCreditPayments.Where(top => (top.CreatedDate >= fromdate && top.CreatedDate <= todate)).ToList();
                    //}
                    ////usersCreditPayments = _usercreditpaymentRepository.GetMany(top => (UserId.Contains(top.UserId)) && (ClientId.Contains((int)top.Billing.ClientId)) && (Invoice.Contains(top.Billing.InvoiceNumber)) && (CampaignId.Contains((int)top.Billing.CampaignProfileId)) && (top.Amount >= FromAmt && top.Amount <= ToAmt) && (top.CreatedDate >= fromdate && top.CreatedDate <= todate));

                    //cnt = usersCreditPayments.Count();

                    //usersCreditPayments = usersCreditPayments.Skip(param.Start).Take(param.Length);

                    ////campaignProfileFormModels =
                    ////    Mapper.Map<IEnumerable<CampaignProfile>, IEnumerable<CampaignProfileFormModel>>(usersCreditPayments);
                    #endregion                    

                }
                else
                {

                    //usersCreditPayments = _usercreditpaymentRepository.GetAll().OrderByDescending(top => top.CreatedDate);
                    //cnt = usersCreditPayments.Count();
                    //usersCreditPayments = usersCreditPayments.Skip(param.Start).Take(param.Length);

                    billingDetails = _billingRepository.GetMany(s => s.PaymentMethodId == 1).OrderByDescending(s=>s.Id).DistinctBy(s => s.CampaignProfileId).ToList();
                    //cnt = billingDetails.Count();
                    //billingDetails = billingDetails.Skip(param.Start).Take(param.Length);
                }

                if (Session["UserID"] != null)
                {
                    int uId = (int)Session["UserID"];
                    //usersCreditPayments = _usercreditpaymentRepository.GetMany(top => top.UserId == uId);
                    //if (usersCreditPayments.Count() > 10)
                    //    cnt = usersCreditPayments.Count();

                    billingDetails = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.UserId == uId).OrderByDescending(s => s.Id).DistinctBy(s => s.CampaignProfileId).ToList();
                    if (billingDetails.Count() > 10)
                        cnt = billingDetails.Count();
                }
                if (Session["CampaignID"] != null)
                {
                    int campId = (int)Session["CampaignID"];
                    //usersCreditPayments = _usercreditpaymentRepository.GetMany(top => top.Billing.CampaignProfileId == campId);
                    //if (usersCreditPayments.Count() > 10)
                    //    cnt = usersCreditPayments.Count();

                    billingDetails = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.CampaignProfileId == campId).OrderByDescending(s => s.Id).DistinctBy(s => s.CampaignProfileId).ToList();
                    if (billingDetails.Count() > 10)
                        cnt = billingDetails.Count();
                }

                foreach (var item in billingDetails)
                {
                    var totalAmount = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.CampaignProfileId == item.CampaignProfileId).Sum(top => top.FundAmount);
                    var paidAmount = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item.CampaignProfileId).Sum(s => s.Amount);
                    var outStandingAmount = totalAmount - paidAmount;
                    var clientId = item.ClientId == null ? 0 : item.ClientId;
                    var description = "-";
                    var userCreditPaymentData = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == item.CampaignProfileId).OrderByDescending(s => s.Id).FirstOrDefault();
                    if (userCreditPaymentData != null)
                    {
                        description = userCreditPaymentData.Description;
                    }
                    int status = 0;
                    if (paidAmount > 0)
                    {
                        status = 1;
                    }
                    if (outStandingAmount > 0)
                    {
                        _resultFinal.Add(new UserCreditPaymentResult { Id = item.Id, UserId = (int)item.UserId, Email = item.User.Email, Name = item.User.FirstName + " " + item.User.LastName, CreatedDate = item.PaymentDate.ToString("dd/MM/yyyy"),
                            CreatedDateSort = item.PaymentDate, Amount = paidAmount, Description = description, Status = status, CampaignName = item.CampaignProfile.CampaignName, CampaignProfileId = (int)item.CampaignProfileId, ClientId = clientId,
                            ClientName = clientId == 0 ? "-" : item.Client.Name, InvoiceNumber = item.InvoiceNumber, TotalAmount = totalAmount, OutstandingAmount = outStandingAmount, Organisation = item.User.Organisation });
                    }
                }
                cnt = _resultFinal.Count();
                _resultFinal = _resultFinal.Skip(param.Start).Take(param.Length).ToList();

                Session["UserID"] = null;
                Session["CampaignID"] = null;


                _resultFinal = ApplySorting(param, _resultFinal);
                //_resultFinal = _resultFinal.Skip(param.Start).Take(param.Length).ToList();

                DTResult<UserCreditPaymentResult> result = new DTResult<UserCreditPaymentResult>
                {
                    draw = param.Draw,
                    data = _resultFinal,
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
        private static List<UserCreditPaymentResult> ApplySorting(DTParameters param, List<UserCreditPaymentResult> result)
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
                        result = result.OrderBy(top => top.Name).ToList();
                    else
                        result = result.OrderByDescending(top => top.Name).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Organisation).ToList();
                    else
                        result = result.OrderByDescending(top => top.Organisation).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.ClientName).ToList();
                    else
                        result = result.OrderByDescending(top => top.ClientName).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CampaignName).ToList();
                    else
                        result = result.OrderByDescending(top => top.CampaignName).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.InvoiceNumber).ToList();
                    else
                        result = result.OrderByDescending(top => top.InvoiceNumber).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.TotalAmount).ToList();
                    else
                        result = result.OrderByDescending(top => top.TotalAmount).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Amount).ToList();
                    else
                        result = result.OrderByDescending(top => top.Amount).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.OutstandingAmount).ToList();
                    else
                        result = result.OrderByDescending(top => top.OutstandingAmount).ToList();
                }
                else if (paramOrderDetails.Column == 9)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Description).ToList();
                    else
                        result = result.OrderByDescending(top => top.Description).ToList();
                }
                else if (paramOrderDetails.Column == 10)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDate).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDate).ToList();
                }
                else if (paramOrderDetails.Column == 11)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Status).ToList();
                    else
                        result = result.OrderByDescending(top => top.Status).ToList();
                }
            }
            return result;
        }

        [Route("ReceivePayment")]
        public ActionResult ReceivePayment()
        {
            UserCreditPaymentFormModel _paymentModel = new UserCreditPaymentFormModel();
            FillUserPaymentDropdown();
            return View(_paymentModel);
        }

        [Route("ReceivePayment")]
        [HttpPost]
        public ActionResult ReceivePayment(UserCreditPaymentFormModel _model)
        {
            if (ModelState.IsValid)
            {
                UsersCreditPaymentFormModel _payment = new UsersCreditPaymentFormModel();
                _payment.UserId = _model.UserId;
                _payment.BillingId = _model.BillingId;
                _payment.Amount = _model.Amount;
                _payment.Description = _model.Description;
                _payment.CreatedDate = DateTime.Now;
                _payment.UpdatedDate = DateTime.Now;
                _payment.Status = 1;
                _payment.CampaignProfileId = _model.CampaignProfileId;


                CreateOrUpdateUsersCreditPaymentCommand command =
              Mapper.Map<UsersCreditPaymentFormModel, CreateOrUpdateUsersCreditPaymentCommand>(_payment);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    updateusercredit(_model.UserId, _model.Amount);
                    //TempData["status"] = "Payment received successfully.";
                    var invoiceNo = _billingRepository.GetById(_model.BillingId).InvoiceNumber;
                    var userName = _userRepository.GetById(_model.UserId);
                    TempData["status"] = "Payment received successfully for " + invoiceNo + " from " + userName.FirstName + " " + userName.LastName;
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        public bool updateusercredit(int userid, decimal receivedamount)
        {
            bool status = false;
            var usercredit = _usercreditRepository.Get(top => top.UserId == userid);
            if (usercredit != null)
            {
                var AvailableCredit = usercredit.AvailableCredit;
                AvailableCredit = AvailableCredit + receivedamount;
                UpdateUserCreditCommand command = new UpdateUserCreditCommand();
                command.UserId = userid;
                command.AvailableCredit = AvailableCredit;
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    status = true;
                }
            }
            return status;
        }
        public void FillUserPaymentDropdown()
        {
            var userdetails = _userRepository.GetAll().Where(top => top.RoleId == 3 && top.VerificationStatus == true && top.Activated == 1).Select(top => new SelectListItem
            {
                Text = top.FirstName + " " + top.LastName + "(" + top.Email + ")",
                Value = top.UserId.ToString(),
            }).ToList();
            ViewBag.userdetails = userdetails;
            List<SelectListItem> _billingdetails = new List<SelectListItem>();
            _billingdetails.Add(new SelectListItem { Text = "--Select Invoice--", Value = "" });
            ViewBag.billingdetails = _billingdetails;
            List<SelectListItem> campaignDetails = new List<SelectListItem>();
            campaignDetails.Add(new SelectListItem { Text = "--Select Campaign--", Value = "" });
            ViewBag.CampaignDetails = campaignDetails;
        }
        //[Route("GetInvoiceDetails")]
        //[HttpPost]
        //public ActionResult GetInvoiceDetails(int? userId)
        //{
        //    List<SelectListItem> _billingdetails = new List<SelectListItem>();
        //    _billingdetails.Add(new SelectListItem { Text = "--Select Invoice--", Value = "" });
        //    if (userId != null)
        //    {
        //        //var billingdetails = _billingRepository.GetAll().Where(top => top.PaymentMethodId == 1 && top.UserId == userId).ToList();
        //        //var billingdetailsId = _billingRepository.GetAll().Where(top => top.PaymentMethodId == 1 && top.UserId == userId).Select(top => top.Id).ToList();
        //        //var userCredit = _usercreditpaymentRepository.GetAll().Where(top => billingdetailsId.Contains(top.BillingId));
        //        //foreach (var item in billingdetails.ToList())
        //        //{
        //        //    var sumAmount = userCredit.Where(top => top.BillingId == item.Id).Sum(top => top.Amount);
        //        //    if(sumAmount == item.FundAmount)
        //        //    {
        //        //        billingdetails.Remove(billingdetails.Where(top => top.Id == item.Id).FirstOrDefault());
        //        //    }
        //        //}

        //        //foreach (var item in billingdetails)
        //        //{    
        //        //    _billingdetails.Add(new SelectListItem { Text = item.InvoiceNumber, Value = item.Id.ToString() });
        //        //}

        //        var billingdetails = _billingRepository.GetAll().Where(top => top.PaymentMethodId == 1 && top.UserId == userId).ToList();
        //        var campaignProfileIdList = _billingRepository.GetAll().Where(top => top.PaymentMethodId == 1 && top.UserId == userId).Select(top => top.CampaignProfileId).ToList();
        //        var totalAmount = _usercreditpaymentRepository.GetMany(s => campaignProfileIdList.Contains(s.CampaignProfileId)).Sum(s => s.Amount);
        //        foreach (var item in billingdetails.ToList())
        //        {                
        //            var billingamount = item.FundAmount;
        //            var outstandingAmount = billingamount - totalAmount;
        //            if (outstandingAmount > 0)
        //            {
        //                _billingdetails.Add(new SelectListItem { Text = item.InvoiceNumber, Value = item.Id.ToString() });
        //            }
        //        }              

        //    }
        //    return Json(_billingdetails);
        //}


        [Route("GetInvoiceDetails")]
        [HttpPost]
        public ActionResult GetInvoiceDetails(int? CampaignProfileId)
        {
            List<SelectListItem> _billingdetails = new List<SelectListItem>();
            _billingdetails.Add(new SelectListItem { Text = "--Select Invoice--", Value = "" });
            if (CampaignProfileId != null)
            {
                var totalAmount = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.CampaignProfileId == CampaignProfileId).Sum(top => top.FundAmount);
                var paidAmount = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == CampaignProfileId).Sum(s => s.Amount);
                var outStandingAmount = totalAmount - paidAmount;
                if (outStandingAmount > 0)
                {
                    var billingdetails = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.CampaignProfileId == CampaignProfileId).OrderByDescending(s => s.Id).Select(s => new SelectListItem { Text = s.InvoiceNumber, Value = s.Id.ToString() }).FirstOrDefault();
                    _billingdetails.Add(new SelectListItem { Text = billingdetails.Text, Value = billingdetails.Value.ToString() });
                }

            }
            return Json(_billingdetails);
        }

        [Route("GetCampaignDetails")]
        [HttpPost]
        public ActionResult GetCampaignDetails(int? userId)
        {
            List<SelectListItem> campaignDetails = new List<SelectListItem>();
            campaignDetails.Add(new SelectListItem { Text = "--Select Campaign--", Value = "" });
            if (userId != null)
            {
                var campDetails = _billingRepository.GetMany(s => s.UserId == userId).DistinctBy(s => s.CampaignProfileId).Select(s => new SelectListItem { Text = s.CampaignProfile.CampaignName, Value = s.CampaignProfileId.ToString() }).ToList();
                campaignDetails.AddRange(campDetails);
                return Json(campaignDetails);
            }
            return Json(campaignDetails);
        }

        [Route("GetInvoiceAmount")]
        [HttpPost]
        public ActionResult GetInvoiceAmount(int? CampaignProfileId)
        {
            try
            {
                var totalAmount = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.CampaignProfileId == CampaignProfileId).Sum(top => top.FundAmount);
                var paidAmount = _usercreditpaymentRepository.GetMany(s => s.CampaignProfileId == CampaignProfileId).Sum(s => s.Amount);
                var outStandingAmount = totalAmount - paidAmount;
                return Json(outStandingAmount);
            }
            catch(Exception ex)
            {
                return Json(0);
            }            
        }

        //[Route("GetInvoiceAmount")]
        //[HttpPost]
        //public ActionResult GetInvoiceAmount(int? billingId)
        //{
        //    decimal billingamount = 0;
        //    decimal payablebillingamount = 0;
        //    decimal totalamount = 0;
        //    //var billingdetails = _billingRepository.Get(top => top.Id == billingId);
        //    //if (billingdetails != null)
        //    //{
        //    //    billingamount = billingdetails.FundAmount;
        //    //}
        //    //payablebillingamount = _usercreditpaymentRepository.GetAll().Where(top => top.BillingId == billingId).Sum(top => top.Amount);
        //    //decimal totalamount = billingamount - payablebillingamount;

        //    List <int> bilingIdList = new List<int>();
        //    var billingdetails = _billingRepository.Get(top => top.Id == billingId);           
        //    if (billingdetails != null)
        //    {
        //        billingamount = billingdetails.FundAmount;
        //        // bilingIdList = _billingRepository.GetMany(s => s.UserId == billingdetails.UserId &&  s.CampaignProfileId == billingdetails.CampaignProfileId).Select(s => s.Id).ToList();
        //        //payablebillingamount = _usercreditpaymentRepository.GetAll().Where(top => bilingIdList.Contains(top.BillingId)).Sum(top => top.Amount);
        //        payablebillingamount = _usercreditpaymentRepository.GetMany(top => top.CampaignProfileId == billingdetails.CampaignProfileId).Sum(top => top.Amount);
        //        totalamount = billingamount - payablebillingamount;
        //    }

        //    return Json(totalamount);
        //}

        [Route("GetClientsUser")]
        [HttpPost]
        public ActionResult GetClientsUser(int[] userId)
        {
            try
            {


                if (userId != null)
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
        [Route("GetUsersCampaign")]
        [HttpPost]
        public ActionResult GetUsersCampaign(int[] userId)
        {
            try
            {


                if (userId != null)
                {

                    var campaigndetails = _profileRepository.GetAll().Where(top => userId.Contains((int)(top.UserId))).Select(top => new
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
                    return Json(campaigndetails);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }
        [Route("GetClientsCampaign")]
        [HttpPost]
        public ActionResult GetClientsCampaign(int[] clientId, int[] userId)
        {
            try
            {

                if (clientId != null)
                {
                    if (clientId != null)
                    {

                        var campaigndetails = _profileRepository.GetAll().Where(top => clientId.Contains((int)(top.ClientId == null ? 0 : top.ClientId))).Select(top => new
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
                        return Json(campaigndetails);
                    }
                }
                else
                {
                    if (userId != null)
                    {

                        var campaigndetails = _profileRepository.GetAll().Where(top => userId.Contains((int)(top.UserId))).Select(top => new
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
                        return Json(campaigndetails);
                    }
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }
        [Route("SearchUsersCreditPayment")]
        public ActionResult SearchUsersCreditPayment([Bind(Prefix = "Item2")]SearchClass.UserCreditPaymentFilter _filterCritearea, int[] UserId, int?[] ClientId, int[] CampaignId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<UserCreditPaymentResult> _result = new List<UserCreditPaymentResult>();
                var finalresult = new List<UserCreditPaymentResult>();
                if (_filterCritearea != null)
                {
                    _result = FillUserCreditPaymentResult();
                    finalresult = getusercreditResult(_result, _filterCritearea, UserId, ClientId, CampaignId);
                }
                else
                {

                    _result = FillUserCreditPaymentResult();
                    finalresult = getusercreditResult(_result, _filterCritearea, UserId, ClientId, CampaignId);
                }

                return PartialView("_UserCreditPaymentDetails", finalresult);
            }
            else
            {
                return PartialView("_UserCreditPaymentDetails", "notauthorise");
            }
        }

        public List<UserCreditPaymentResult> getusercreditResult(List<UserCreditPaymentResult> usercreditresult, SearchClass.UserCreditPaymentFilter _filterCritearea, int[] UserId, int?[] ClientId, int[] CampaignId)
        {
            if (usercreditresult != null && _filterCritearea != null)
            {

                if (UserId != null)
                {
                    usercreditresult = usercreditresult.Where(top => UserId.Contains(top.UserId)).ToList();
                }
                if (ClientId != null)
                {
                    usercreditresult = usercreditresult.Where(top => ClientId.Contains(top.ClientId)).ToList();
                }

                if (CampaignId != null)
                {
                    usercreditresult = usercreditresult.Where(top => CampaignId.Contains(top.CampaignProfileId)).ToList();
                }
                if (!string.IsNullOrEmpty(_filterCritearea.InvoiceNumber))
                {
                    usercreditresult = usercreditresult.Where(top => top.InvoiceNumber.Contains(_filterCritearea.InvoiceNumber)).ToList();
                }
                if ((!String.IsNullOrEmpty(_filterCritearea.Fromamount) && !String.IsNullOrEmpty(_filterCritearea.Toamount)))
                {
                    decimal fromaount = Convert.ToDecimal(_filterCritearea.Fromamount);
                    decimal toamount = Convert.ToDecimal(_filterCritearea.Toamount);
                    usercreditresult = usercreditresult.Where(top => top.Amount >= fromaount && top.Amount <= toamount).ToList();
                }
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    //usercreditresult = usercreditresult.Where(top => top.CreatedDate.Date >= _filterCritearea.Fromdate.Value.Date && top.CreatedDate.Date <= _filterCritearea.Todate.Value.Date).ToList();

                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    usercreditresult = usercreditresult.Where(top => top.CreatedDateSort.Date >= Fromdate && top.CreatedDateSort.Date <= Todate).ToList();
                }

            }
            else
            {
                if (UserId != null)
                {
                    usercreditresult = usercreditresult.Where(top => UserId.Contains(top.UserId)).ToList();
                }

            }
            return usercreditresult;
        }


    }
}
