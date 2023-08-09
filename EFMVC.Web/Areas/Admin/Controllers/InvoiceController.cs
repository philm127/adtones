using AutoMapper;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Areas.Admin.SearchClass;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.EFWebPDF;
using EFMVC.Web.Helpers;
using EFMVC.Web.Mailer;
using EFMVC.Web.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("Invoice")]
    public class InvoiceController : Controller
    {
        // GET: Admin/Invoice

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;
        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileRepository;

        /// <summary>
        /// The _user credit repository
        /// </summary>
        private readonly IUsersCreditRepository _userCreditRepository;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _companydetails repository
        /// </summary>
        private readonly ICompanyDetailsRepository _companydetailsRepository;

        /// <summary>
        /// The _contact repository
        /// </summary>
        private readonly IContactsRepository _contactRepository;

        /// <summary>
        /// The _send email mailer
        /// </summary>
        private ISendEmailMailer _sendEmailMailer = new SendEmailMailer();

        /// <summary>
        /// The _country tax repository
        /// </summary>
        private readonly ICountryTaxRepository _countryTaxRepository;
        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUsersCreditPaymentRepository _usersCreditPaymentRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        public ISendEmailMailer sendEmailMailer
        {
            get { return _sendEmailMailer; }
            set { _sendEmailMailer = value; }
        }
        /// <summary>
        /// The _billing repository
        /// </summary>
        private readonly IBillingRepository _billingRepository;
        public InvoiceController(ICommandBus commandBus, IClientRepository clientRepository, ICampaignProfileRepository profileRepository, IBillingRepository billingRepository, IPaymentMethodRepository paymentMethodRepository, IUsersCreditRepository userCreditRepository, IUserRepository userRepository, ICompanyDetailsRepository companydetailsRepository, IContactsRepository contactRepository, ICountryTaxRepository countryTaxRepository, ICountryRepository countryRepository, IUsersCreditPaymentRepository usersCreditPaymentRepository, ICurrencyRepository currencyRepository)
        {
            _commandBus = commandBus;
            _clientRepository = clientRepository;
            _profileRepository = profileRepository;
            _billingRepository = billingRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _userCreditRepository = userCreditRepository;
            _userRepository = userRepository;
            _companydetailsRepository = companydetailsRepository;
            _contactRepository = contactRepository;
            _currencyRepository = currencyRepository;
            _countryTaxRepository = countryTaxRepository;
            _countryRepository = countryRepository;
            _usersCreditPaymentRepository = usersCreditPaymentRepository;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            //List<InvoiceResult> _result = GetInvoiceResult();
            List<InvoiceResult> _result = new List<InvoiceResult>();
            InvoiceFilter _filterCritearea = new InvoiceFilter();

            var _clientdetails = _clientRepository.GetAll();
            IEnumerable<ClientModel> clientModels =
                Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            FillClient(clientModels);
            FillStatus();
            FillPaymentDropdown();
            FillUserDropdown();

            return View(Tuple.Create(_result, _filterCritearea));
        }

        //Add 01-07-2019
        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<InvoiceResult> _result = new List<InvoiceResult>();
                IEnumerable<UsersCreditPayment> usersCreditPayment = null;
                DateTimeFormat dateTimeConvert = new DateTimeFormat();
                string status = string.Empty;
                ViewBag.SearchResult = false;
                var cnt = 10;
                int userId = 0;

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
                    int?[] ClientId = new int?[cnt];
                    int[] StatusId = new int[cnt];
                    int[] ModeofPaymentId = new int[cnt];
                    string InvoiceNo = string.Empty;
                    string PoNo = string.Empty;
                    DateTime InvoiceDatefromdate = new DateTime();
                    DateTime InvoiceDatetodate = new DateTime();
                    DateTime SettledDatefromdate = new DateTime();
                    DateTime SettledDatetodate = new DateTime();
                    decimal InvoiceTotalFrom = 0.00M;
                    decimal InvoiceTotalTo = 0.00M;

                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            InvoiceNo = columnSearch[0].ToString();
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
                            PoNo = columnSearch[1].ToString();
                        }
                        else
                        {
                            columnSearch[1] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[4]))
                    {
                        if (columnSearch[4] != "null")
                        {
                            UserId = columnSearch[4].Split(',').Select(a => (int)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[4] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[5]))
                    {
                        if (columnSearch[5] != "null")
                        {
                            ClientId = columnSearch[5].Split(',').Select(a => (int?)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[5] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[7]))
                    {
                        if (columnSearch[7] != "null")
                        {
                            var data = columnSearch[7].Split(',').ToArray();
                            InvoiceDatefromdate = Convert.ToDateTime(data[0]);
                            InvoiceDatetodate = Convert.ToDateTime(data[1]);
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
                            InvoiceTotalFrom = Convert.ToDecimal(data[0]);
                            InvoiceTotalTo = Convert.ToDecimal(data[1]);
                        }
                        else
                        {
                            columnSearch[8] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[9]))
                    {
                        if (columnSearch[9] != "null")
                        {
                            StatusId = columnSearch[9].Split(',').Select(int.Parse).ToArray();
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
                            SettledDatefromdate = Convert.ToDateTime(data[0]);
                            SettledDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[10] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[11]))
                    {
                        if (columnSearch[11] != "null")
                        {
                            ModeofPaymentId = columnSearch[11].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[11] = null;
                        }
                    }

                    usersCreditPayment = _usersCreditPaymentRepository.GetAll().OrderByDescending(top => top.Id).ToList();

                    string fstatus = string.Empty;
                    if (usersCreditPayment.Count() > 0)
                    {
                        foreach (var item in usersCreditPayment)
                        {
                            BillingStatus billingStatus = (BillingStatus)item.Billing.Status;
                            fstatus = billingStatus.ToString();
                            _result.Add(new InvoiceResult
                            {
                                ID = item.Billing.Id,
                                InvoiceNO = item.Billing.InvoiceNumber,
                                PONumber = item.Billing.PONumber == null ? "-" : item.Billing.PONumber,
                                ClienId = item.Billing.ClientId == null ? 0 : (int)item.Billing.ClientId,
                                ClientName = item.Billing.ClientId == null ? "-" : item.Billing.Client.Name,
                                CampaignId = (int)item.CampaignProfileId,
                                CampaignName = item.CampaignProfile.CampaignName,
                                InvoiceDate = item.Billing.PaymentDate.ToString("dd/MM/yyyy"),
                                InvoiceDateSort = Convert.ToDateTime(item.Billing.PaymentDate),
                                //InvoiceTotal = item.Billing.TotalAmount,
                                InvoiceTotal = item.Amount,
                                status = fstatus,
                                SettledDate = item.Billing.SettledDate.ToString("dd/MM/yyyy"),
                                SettledDateSort = item.Billing.SettledDate,
                                MethodOfPayment = item.Billing.PaymentMethod.Description,
                                PaymentMethodId = (int)item.Billing.PaymentMethodId,
                                fstatus = item.Billing.Status,
                                UserId = item.UserId,
                                UserName = item.User.FirstName + " " + item.User.LastName,
                                Emailaddress = item.User.Email,
                                Organisation = item.User.Organisation == null ? "-" : item.User.Organisation,
                                UsersCreditPaymentID = item.Id
                            });
                        }
                    }
                    if (columnSearch[0] != null)
                    {
                        _result = _result.Where(top => top.InvoiceNO == InvoiceNo).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        _result = _result.Where(top => top.PONumber == PoNo).ToList();
                    }
                    if (columnSearch[4] != null)
                    {
                        _result = _result.Where(top => (UserId.Contains(Convert.ToInt32(top.UserId)))).ToList();
                    }
                    if (columnSearch[5] != null)
                    {
                        _result = _result.Where(top => (ClientId.Contains(Convert.ToInt32(top.ClienId)))).ToList();
                    }
                    if (columnSearch[7] != null)
                    {
                        _result = _result.Where(top => (top.InvoiceDateSort >= InvoiceDatefromdate && top.InvoiceDateSort <= InvoiceDatetodate)).ToList();
                    }
                    if (columnSearch[8] != null)
                    {
                        _result = _result.Where(top => (top.InvoiceTotal >= InvoiceTotalFrom && top.InvoiceTotal <= InvoiceTotalTo)).ToList();
                    }
                    if (columnSearch[9] != null)
                    {
                        _result = _result.Where(top => (StatusId.Contains((int)top.fstatus))).ToList();
                    }
                    if (columnSearch[10] != null)
                    {
                        _result = _result.Where(top => (top.SettledDateSort >= SettledDatefromdate && top.SettledDateSort <= SettledDatetodate)).ToList();
                    }
                    if (columnSearch[11] != null)
                    {
                        _result = _result.Where(top => (ModeofPaymentId.Contains((int)top.PaymentMethodId))).ToList();
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();

                    #endregion
                }
                else
                {
                    usersCreditPayment = _usersCreditPaymentRepository.GetAll().OrderByDescending(top => top.Id).ToList();

                    string fstatus = string.Empty;
                    if (usersCreditPayment.Count() > 0)
                    {
                        foreach (var item in usersCreditPayment)
                        {
                            BillingStatus billingStatus = (BillingStatus)item.Billing.Status;
                            fstatus = billingStatus.ToString();
                            _result.Add(new InvoiceResult
                            {
                                ID = item.Billing.Id,
                                InvoiceNO = item.Billing.InvoiceNumber,
                                PONumber = item.Billing.PONumber == null ? "-" : item.Billing.PONumber,
                                ClienId = item.Billing.ClientId == null ? 0 : (int)item.Billing.ClientId,
                                ClientName = item.Billing.ClientId == null ? "-" : item.Billing.Client.Name,
                                CampaignId = (int)item.CampaignProfileId,
                                CampaignName = item.CampaignProfile.CampaignName,
                                InvoiceDate = item.Billing.PaymentDate.ToString("dd/MM/yyyy"),
                                InvoiceDateSort = Convert.ToDateTime(item.Billing.PaymentDate),
                                //InvoiceTotal = item.Billing.TotalAmount,
                                InvoiceTotal = item.Amount,
                                // status = fstatus,
                                status = item.Billing.Status == 3 ? fstatus : "Paid",
                                SettledDate = item.Billing.SettledDate.ToString("dd/MM/yyyy"),
                                SettledDateSort = item.Billing.SettledDate,
                                MethodOfPayment = item.Billing.PaymentMethodId == 1 ? "Cheque" : item.Billing.PaymentMethod.Description,
                                PaymentMethodId = (int)item.Billing.PaymentMethodId,
                                fstatus = item.Billing.Status,
                                UserId = item.UserId,
                                UserName = item.User.FirstName + " " + item.User.LastName,
                                Emailaddress = item.User.Email,
                                Organisation = item.User.Organisation == null ? "-" : item.User.Organisation,
                                UsersCreditPaymentID = item.Id
                            });
                        }
                    }
                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<InvoiceResult> result = new DTResult<InvoiceResult>
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

        //Add 01-07-2019
        // Function For Filter/Sorting Invoice Data
        private static List<InvoiceResult> ApplySorting(DTParameters param, List<InvoiceResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.InvoiceNO).ToList();
                    else
                        result = result.OrderByDescending(top => top.InvoiceNO).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.PONumber).ToList();
                    else
                        result = result.OrderByDescending(top => top.PONumber).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.UserName).ToList();
                    else
                        result = result.OrderByDescending(top => top.UserName).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Organisation).ToList();
                    else
                        result = result.OrderByDescending(top => top.Organisation).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Emailaddress).ToList();
                    else
                        result = result.OrderByDescending(top => top.Emailaddress).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.ClientName).ToList();
                    else
                        result = result.OrderByDescending(top => top.ClientName).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CampaignName).ToList();
                    else
                        result = result.OrderByDescending(top => top.CampaignName).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.InvoiceDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.InvoiceDateSort).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.InvoiceTotal).ToList();
                    else
                        result = result.OrderByDescending(top => top.InvoiceTotal).ToList();
                }
                else if (paramOrderDetails.Column == 9)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.fstatus).ToList();
                    else
                        result = result.OrderByDescending(top => top.fstatus).ToList();
                }
                else if (paramOrderDetails.Column == 10)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.SettledDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.SettledDateSort).ToList();
                }
                else if (paramOrderDetails.Column == 11)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.MethodOfPayment).ToList();
                    else
                        result = result.OrderByDescending(top => top.MethodOfPayment).ToList();
                }
            }
            return result;
        }

        #region Old
        //private List<InvoiceResult> GetInvoiceResult()
        //{

        //    List<InvoiceResult> _result = new List<InvoiceResult>();


        //    var _clientdetails = _clientRepository.GetAll();
        //    IEnumerable<ClientModel> clientModels =
        //        Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);


        //    // IEnumerable<Billing> billing = _billingRepository.GetAll().OrderByDescending(top=>top.PaymentDate);

        //    //invoice section we'll not add the "credit" payment
        //    IEnumerable<Billing> billing = _billingRepository.GetMany(s=>s.PaymentMethodId != 1).OrderByDescending(top => top.PaymentDate);
        //    IEnumerable<BillingFormModel> billingFormModels =
        //        Mapper.Map<IEnumerable<Billing>, IEnumerable<BillingFormModel>>(billing);

        //    FillClient(clientModels);
        //    FillStatus();
        //    FillPaymentDropdown();
        //    FillUserDropdown();
        //    string status = string.Empty;
        //    if (billingFormModels.Count() > 0)
        //    {

        //        foreach (var item in billingFormModels)
        //        {

        //            BillingStatus billingStatus = (BillingStatus)item.Status;
        //            status = billingStatus.ToString();
        //            _result.Add(new InvoiceResult { ID = item.Id, InvoiceNO = item.InvoiceNumber, PONumber = item.PONumber == null ? "-" : item.PONumber, ClienId = item.ClientId == 0 ? 0 : item.ClientId, ClientName = item.ClientId == 0 ? "-" : item.Client.Name, CampaignId = item.CampaignProfileId, CampaignName = item.CampaignProfile.CampaignName, InvoiceDate = Convert.ToDateTime(item.PaymentDate), InvoiceTotal = item.TotalAmount, status = status, SettledDate = item.SettledDate, MethodOfPayment = item.PaymentMethod.Description, PaymentMethodId = item.PaymentMethodId, fstatus = item.Status, UserId = item.UserId, UserName = item.User.FirstName + " " + item.User.LastName, Emailaddress = item.User.Email, Organisation = item.User.Organisation == null ? "-" : item.User.Organisation });
        //        }
        //    }

        //    return _result;

        //}
        #endregion

        private List<InvoiceResult> GetInvoiceResult()
        {

            List<InvoiceResult> _result = new List<InvoiceResult>();


            var _clientdetails = _clientRepository.GetAll();
            IEnumerable<ClientModel> clientModels =
                Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);


            // IEnumerable<Billing> billing = _billingRepository.GetAll().OrderByDescending(top=>top.PaymentDate);

            //invoice section we'll not add the "credit" payment
            IEnumerable<UsersCreditPayment> usersCreditPayment = _usersCreditPaymentRepository.GetAll().OrderByDescending(top => top.Id);

            FillClient(clientModels);
            FillStatus();
            FillPaymentDropdown();
            FillUserDropdown();
            string status = string.Empty;
            if (usersCreditPayment.Count() > 0)
            {
                foreach (var item in usersCreditPayment)
                {
                    BillingStatus billingStatus = (BillingStatus)item.Billing.Status;
                    status = billingStatus.ToString();
                    _result.Add(new InvoiceResult
                    {
                        ID = item.Billing.Id,
                        InvoiceNO = item.Billing.InvoiceNumber,
                        PONumber = item.Billing.PONumber == null ? "-" : item.Billing.PONumber,
                        ClienId = item.Billing.ClientId == null ? 0 : (int)item.Billing.ClientId,
                        ClientName = item.Billing.ClientId == null ? "-" : item.Billing.Client.Name,
                        CampaignId = (int)item.CampaignProfileId,
                        CampaignName = item.CampaignProfile.CampaignName,
                        InvoiceDate = item.Billing.PaymentDate.ToString("dd/MM/yyyy"),
                        InvoiceDateSort = Convert.ToDateTime(item.Billing.PaymentDate),
                        //InvoiceTotal = item.Billing.TotalAmount,
                        InvoiceTotal = item.Amount,
                        status = item.Billing.Status == 3 ? status : "Paid",
                        SettledDate = item.Billing.SettledDate.ToString("dd/MM/yyyy"),
                        SettledDateSort = item.Billing.SettledDate,
						MethodOfPayment = item.Billing.PaymentMethodId == 1 ? "Cheque" : item.Billing.PaymentMethod.Description,
                        PaymentMethodId = (int)item.Billing.PaymentMethodId,
                        fstatus = item.Billing.Status,
                        UserId = item.UserId,
                        UserName = item.User.FirstName + " " + item.User.LastName,
                        Emailaddress = item.User.Email,
                        Organisation = item.User.Organisation == null ? "-" : item.User.Organisation,
                        UsersCreditPaymentID = item.Id
                    });
                }
            }

            return _result;

        }
        
       
        public void FillUserDropdown()
        {
            //var userdetails = _userRepository.GetAll().Select(top => new
            //{
            //    Name = top.FirstName+" "+top.LastName+"("+top.Email+")",
            //    Id = top.UserId,
            //});
            udd userdetails = new udd
            {
                Name = "FirstName LastName (Email)",
                Id = 88,
            };
            uddL lst = new uddL();
            lst.Add(userdetails);
            //var userdetails = _userRepository.GetAll().Select(top => new
            //{
            //    Name = top.FirstName + " " + top.LastName + "(" + top.Email + ")",
            //    Id = top.UserId,
            //}).Take(1000).ToList();
            ViewBag.userdetails = new MultiSelectList(lst.udL, "Id", "Name");
        }

        //Add 26-02-2019
        [Route("FillUserDropdownAJAX")]
        [HttpPost]
        public ActionResult FillUserDropdownAJAX()
        {
            try
            {
                var userdetails = _userRepository.GetAll().Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName,
                    UserId = top.UserId,
                }).ToList();
                ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
                var data = ViewBag.userdetails;
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }

        public void FillPaymentDropdown()
        {

            var paymentMethoddetails = _paymentMethodRepository.GetAll().Select(top => new
            {
                Name = top.Id == 1 ? "Cheque" :  top.Description,
                Id = top.Id.ToString(),
            });
            ViewBag.paymentMethod = new MultiSelectList(paymentMethoddetails, "Id", "Name");

        }
        private void FillClient(IEnumerable<ClientModel> clientModels)
        {

            var clientdetails = clientModels.Select(top => new
            {
                Name = top.Name,
                Id = top.Id.ToString(),
            }).ToList();
            ViewBag.client = new MultiSelectList(clientdetails, "Id", "Name");

        }
        public void FillStatus()
        {

            //billing status
            IEnumerable<Common.BillingStatus> billingTypes = Enum.GetValues(typeof(Common.BillingStatus))
                                                     .Cast<Common.BillingStatus>();
            //var billingstatus = (from action in billingTypes
            //                     select new SelectListItem
            //                     {
            //                         Text = action.ToString(),
            //                         Value = ((int)action).ToString()
            //                     }).ToList();

            var billingstatus = Enum.GetValues(typeof(BillingStatus))
                        .Cast<BillingStatus>()
                        .Where(e => e != BillingStatus.Credited)
                        .Select(e => new SelectListItem
                        {
                            Value = ((int)e).ToString(),
                            Text = e.ToString()
                        });
            ViewBag.billingStatus = new MultiSelectList(billingstatus, "Value", "Text");


        }
        [Route("SearchInvoice")]
        public ActionResult SearchInvoice([Bind(Prefix = "Item2")]InvoiceFilter _filterCritearea, int[] InvoiceClientId, int[] InvoicestatusId, int[] InvoicemethodId, int[] UserId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<InvoiceResult> _result = new List<InvoiceResult>();
                var finalresult = new List<InvoiceResult>();
                if (_filterCritearea != null)
                {
                    _result = GetInvoiceResult();
                    finalresult = getinvoicefilterResult(_result, _filterCritearea, InvoiceClientId, InvoicestatusId, InvoicemethodId, UserId);
                }
                else
                {
                    _result = GetInvoiceResult();
                    finalresult = getinvoicefilterResult(_result, _filterCritearea, InvoiceClientId, InvoicestatusId, InvoicemethodId, UserId);
                }
                return PartialView("_InvoiceList", finalresult);
            }
            else
            {
                return PartialView("_InvoiceList", "notauthorise");
            }
        }
        public List<InvoiceResult> getinvoicefilterResult(List<InvoiceResult> invoiceresult, InvoiceFilter _filterCritearea, int[] InvoiceClientId, int[] InvoicestatusId, int[] InvoicemethodId, int[] UserId)
        {
            if (invoiceresult != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.PONumber))
                {
                    invoiceresult = invoiceresult.Where(top => !string.IsNullOrEmpty(top.PONumber) && top.PONumber.Trim().ToLower() == _filterCritearea.PONumber.Trim()).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.Id))
                {
                    int id = Convert.ToInt32(_filterCritearea.Id);
                    invoiceresult = invoiceresult.Where(top => top.ID == id).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.InvoiceNO))
                {

                    invoiceresult = invoiceresult.Where(top => top.InvoiceNO.Trim().ToLower() == _filterCritearea.InvoiceNO.Trim().ToLower()).ToList();

                }
                if (UserId != null)
                {
                    invoiceresult = invoiceresult.Where(top => UserId.Contains(top.UserId)).ToList();
                }
                if (InvoiceClientId != null)
                {
                    invoiceresult = invoiceresult.Where(top => InvoiceClientId.Contains(top.ClienId)).ToList();
                }
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {

                    //invoiceresult = invoiceresult.Where(top => top.InvoiceDate.Date >= _filterCritearea.Fromdate.Value.Date && top.InvoiceDate.Date <= _filterCritearea.Todate.Value.Date).ToList();

                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    invoiceresult = invoiceresult.Where(top => top.InvoiceDateSort.Date >= Fromdate && top.InvoiceDateSort.Date <= Todate).ToList();
                }
                if ((_filterCritearea.SettedFromdate != null && _filterCritearea.SettedTodate != null))
                {
                    //invoiceresult = invoiceresult.Where(top => top.SettledDate.Date >= _filterCritearea.SettedFromdate.Value.Date && top.SettledDate.Date <= _filterCritearea.SettedTodate.Value.Date).ToList();

                    string strTodate = _filterCritearea.SettedTodate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.SettedFromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    invoiceresult = invoiceresult.Where(top => top.SettledDateSort.Date >= Fromdate && top.SettledDateSort.Date <= Todate).ToList();
                }
                if (InvoicestatusId != null)
                {
                    int[] status = null;
                    if(InvoicestatusId.Contains(1) && !InvoicestatusId.Contains(3))
                    {
                        status = new int[] { 1, 2 };
                    }
                    else if(InvoicestatusId.Contains(1) && InvoicestatusId.Contains(3))
                    {
                        status = new int[] { 1, 2, 3 };
                    }
                    else
                    {
                        status = InvoicestatusId;
                    }
                    invoiceresult = invoiceresult.Where(top => status.Contains(top.fstatus)).ToList();
                }
                if (!String.IsNullOrEmpty(_filterCritearea.InvoiceFromTotal) && !String.IsNullOrEmpty(_filterCritearea.InvoiceToTotal))
                {

                    invoiceresult = invoiceresult.Where(top => top.InvoiceTotal >= Convert.ToDecimal(_filterCritearea.InvoiceFromTotal) && top.InvoiceTotal <= Convert.ToDecimal(_filterCritearea.InvoiceToTotal)).ToList();


                }
                if (InvoicemethodId != null)
                {
                    invoiceresult = invoiceresult.Where(top => InvoicemethodId.Contains(top.PaymentMethodId)).ToList();
                }
            }
            return invoiceresult;
        }
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
        [Route("SendInvoice")]
        [Route("{billingId}/{userId}/{UsersCreditPaymentID}")]
        public ActionResult SendInvoice(int billingId, int userId, int UsersCreditPaymentID)
        {
            CreatePDF(billingId, userId, UsersCreditPaymentID);
            TempData["success"] = "Email sent successfully.";
            return RedirectToAction("Index");
        }
        public void CreatePDF(int billingId, int userId, int UsersCreditPaymentID)
        {
            //string invoiceno = string.Empty;
            //string customername = string.Empty;
            //string companyname = string.Empty;
            //string address = string.Empty;
            //string addaddress = string.Empty;
            //string town = string.Empty;
            //string postcode = string.Empty;
            //string country = string.Empty;
            //string finaladdress = string.Empty;
            //string itemdetails = string.Empty;
            string methodofPayment = string.Empty;
            //string country_Tax = string.Empty;

            //get billing data
            var billingdetails = _billingRepository.Get(top => top.Id == billingId);
            //invoiceno = billingdetails.InvoiceNumber;
            //itemdetails = billingdetails.CampaignProfile.CampaignName;
            methodofPayment = _paymentMethodRepository.Get(top => top.Id == billingdetails.PaymentMethodId).Description;
            ////get userinfo
            var userdetails = _userRepository.Get(top => top.UserId == userId);
            //customername = userdetails.FirstName + " " + userdetails.LastName;
            ////get client company info
            //var clientcompany = _companydetailsRepository.Get(top => top.UserId == userId);
            //companyname = clientcompany.CompanyName;
            //address = clientcompany.Address;
            //addaddress = clientcompany.AdditionalAddress;
            //town = clientcompany.Town;
            ////postcode = clientcompany.Town;
            //postcode = clientcompany.PostCode;
            //country = clientcompany.Country.Name;

            //get country invocie tax
            var countryId = _companydetailsRepository.Get(top => top.UserId == userdetails.UserId).CountryId;
            //get customer contact info
            var customercontactinfo = _contactRepository.Get(top => top.UserId == userId);

            //get UsersCreditPayment data
            var Amount = _usersCreditPaymentRepository.GetById(UsersCreditPaymentID).Amount;

            //EFMVC.Web.EFWebPDF.Item item1 = new EFMVC.Web.EFWebPDF.Item();
            //item1.Description = itemdetails;
            ////item1.Price = billingdetails.FundAmount;
            //item1.Price = Amount;
            //item1.Quantity = 1;
            //item1.Organisation = clientcompany.CompanyName;
            //Customer customer = new Customer();
            //customer.FullName = customername;
            //customer.AddressLine1 = address;
            //customer.AddressLine2 = addaddress;
            //customer.City = town;
            //customer.Country = country;
            //customer.Postcode = postcode;
            //customer.PhoneNumber = customercontactinfo.PhoneNumber;
            ////customer.PhoneNumber = "+" + customercontactinfo.MobileNumber;
            //customer.Email = userdetails.Email;
            //Invoice invoice = new Invoice();
            //invoice = new Invoice(item1);
            //invoice.InvoiceNumber = billingdetails.InvoiceNumber;
            //invoice.vat = (_countryTaxRepository.Get(top => top.CountryId == countryId).TaxPercantage) / 100;
            //invoice.InvoiceTax = _countryTaxRepository.Get(top => top.CountryId == countryId).TaxPercantage.ToString();
            //invoice.InvoiceCountry = _countryRepository.Get(top => top.Id == countryId).ShortName;
            //invoice.MethodOfPayment = methodofPayment;
            //invoice.PONumber = billingdetails.PONumber;
            //invoice.SettledDate = billingdetails.SettledDate;
            //invoice.Imagepath = Server.MapPath("~/Images/5acf06fc.png");
            //invoice.Customer = customer;
            //invoice.Items = 1;
            //invoice.CountryId = countryId;

            //CurrencySymbol currencySymbol = new CurrencySymbol();
            //invoice.CurrencySymbol = currencySymbol.GetCurrencySymbolusingCountryId(countryId);            

            //GeneratePDF pdf = new GeneratePDF();
            //pdf.Invoice = invoice;

           // string path = pdf.CreatePDF(Server.MapPath("~/Invoice"),"","");
            string[] mailto = new string[1];
            mailto[0] = userdetails.Email;
            string[] attachment = new string[1];
            attachment[0] = Server.MapPath("~/Invoice/Adtones_invoice_" + billingdetails.InvoiceNumber + ".pdf");

            // sendemailtoclient(userdetails.Email, userdetails.FirstName, userdetails.LastName, "Invoice Details", 2, mailto, null, null, attachment, true, DateTime.Now.ToString());

            //Comment 13-08-2019
            //sendemailtoclient(userdetails.Email, userdetails.FirstName, userdetails.LastName, "Adtones Invoice (" + invoice.InvoiceNumber + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Date + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Month + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Year + ")", 2, mailto, null, null, attachment, true, DateTime.Now.ToString(), methodofPayment, invoice.SettledDate, invoice.InvoiceNumber);

            //Add 13-08-2019
            //sendemailtoclient(userdetails.Email, userdetails.FirstName, userdetails.LastName, "Adtones Invoice (" + invoice.InvoiceNumber + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Day + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Month + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Year + ")", 2, mailto, null, null, attachment, true, DateTime.Now.ToString(), methodofPayment, invoice.SettledDate, invoice.InvoiceNumber);

            string[] ParamNames = new string[] { "dotnetesting@gmail.com" };
            sendemailtoclient(userdetails.Email, userdetails.FirstName, userdetails.LastName, "Adtones Invoice (" + billingdetails.InvoiceNumber + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Day + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Month + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Year + ")", 2, ParamNames, null, null, attachment, true, DateTime.Now.ToString(), methodofPayment, billingdetails.SettledDate, billingdetails.InvoiceNumber);
        }
        public void sendemailtoclient(string to, string fname, string lname, string subject, int formatId, string[] mailTo, string[] mailCC, string[] mailBcc, string[] attachment, bool isBodyHTML, string completedDatetime, string paymentMethod, DateTime? dueDate, string InvoiceNumber)
        {
            //var EmailContent = new SendEmailModel();
            //if (mailTo != null)
            //{
            //    EmailContent.To = mailTo;
            //}
            //if (mailCC != null)
            //{
            //    EmailContent.CC = mailCC;
            //}
            //if (mailBcc != null)
            //{
            //    EmailContent.Bcc = mailBcc;
            //}
            //if (attachment != null)
            //{
            //    EmailContent.attachment = attachment;
            //}
            //EmailContent.Link = ConfigurationManager.AppSettings["siteAddress"].ToString() + "Admin/UserManagement/Index";
            //EmailContent.isBodyHTML = isBodyHTML;
            //EmailContent.Fname = fname;
            //EmailContent.Lname = lname;
            //EmailContent.Subject = subject;
            //EmailContent.FormatId = formatId;
            //EmailContent.CompletedDatetime = completedDatetime;
            //sendEmailMailer.SendEmail(EmailContent).SendAsync();

             var EmailContent = new SendEmailModel();
            if (mailTo != null)
            {
                EmailContent.To = mailTo;
            }
            if (mailCC != null)
            {
                EmailContent.CC = mailCC;
            }
            if (mailBcc != null)
            {
                EmailContent.Bcc = mailBcc;
            }
            if (attachment != null)
            {
                EmailContent.attachment = attachment;
            }
            EmailContent.Link = ConfigurationManager.AppSettings["siteAddress"].ToString() + "Admin/UserManagement/Index";
            EmailContent.isBodyHTML = isBodyHTML;
            EmailContent.Fname = fname;
            EmailContent.Lname = lname;
            EmailContent.Subject = subject;
            EmailContent.FormatId = formatId;
            EmailContent.InvoiceNumber = InvoiceNumber;
            EmailContent.CompletedDatetime = completedDatetime;

            EmailContent.PaymentMethod = paymentMethod;
            if (paymentMethod == "Card")
            {
                EmailContent.PaymentMethod = "Instantpayment";
            }
            if (EmailContent.PaymentMethod == "Instantpayment")
            {
                EmailContent.DueDate = null;
            }
            else
            {
                EmailContent.DueDate = dueDate;
                EmailContent.PaymentLink = ConfigurationManager.AppSettings["siteAddress"].ToString() + "Billing/buy_credit";
            }
            //sendEmailMailer.SendEmail(EmailContent).SendAsync();
            sendEmailMailer.SendEmail(EmailContent);
        }
    }

    public class udd
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class uddL
    {
        public List<udd> udL { get; set; }

        public uddL()
        {
            // Initialize the list in the constructor
            udL = new List<udd>();
        }

        // Optionally, you can add a method to add user details to the list
        public void Add(udd userDetails)
        {
            udL.Add(userDetails);
        }
    }
}