using System;
using System.Linq;
using System.Web.Mvc;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Models;
using EFMVC.Web.Paypal;
using com.paypal.sdk.profiles;
using com.paypal.sdk.services;
using System.Configuration;
using EFMVC.Domain.Commands.Billing;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.Web.ViewModels;
using EFMVC.Domain.Commands.BillingDetails;
using EFMVC.Model;
using EFMVC.Web.Core.ActionFilters;
using System.Collections.Generic;
using EFMVC.Web.Common;
using EFMVC.Web.SearchClass;
using EFMVC.Domain.Commands;
using System.IO;
using EFMVC.Web.EFWebPDF;
using EFMVC.Web.Mailer;
using EFMVC.ProvisioningModel;
using EFMVC.Data;
using EFMVC.Domain.CountryConnectionString;
using System.Data.SqlClient;
using System.Data;
using RestSharp;
using RestSharp.Deserializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EFMVC.Domain.OperatorServerData;
using System.Globalization;
using System.Data.Entity.Core.Objects;
using Minuco.MPLS.Common.Encryption;
using RestSharp.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using EFMVC.Model.Entities;
using Microsoft.Ajax.Utilities;
using System.CodeDom;
using System.Net.Http;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    //[Authorize(Roles = "Advertiser")]
    public class BillingController : Controller
    {
        public string pendpointurl = ConfigurationManager.AppSettings["paypalendpoint"].ToString();

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;
        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        private ISendEmailMailer _sendEmailMailer = new SendEmailMailer();
        /// <summary>
        /// The _billing repository
        /// </summary>
        private readonly IBillingRepository _billingRepository;

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

        private readonly IContactsRepository _contactRepository;

        private readonly ICountryTaxRepository _countryTaxRepository;
        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        private readonly ICurrencyRepository _currencyRepository;

        private readonly ICurrencyRateRepository _currencyRateRepository;

        private readonly IAdvertRepository _advertRepository;

        private readonly ICampaignAdvertRepository _campaignAdvertRepository;

        private readonly ICampaignCreditPeriodRepository _campaignCreditPeriodRepository;
        private CurrencyConversion _currencyConversion;
        /// <summary>
        /// The _command bus
        /// </summary>
        /// 
        private readonly ICommandBus _commandBus;
        public ISendEmailMailer sendEmailMailer
        {
            get { return _sendEmailMailer; }
            set { _sendEmailMailer = value; }
        }
        public BillingController(ICommandBus commandBus, IClientRepository clientRepository, ICampaignProfileRepository profileRepository, IBillingRepository billingRepository, IPaymentMethodRepository paymentMethodRepository, IUsersCreditRepository userCreditRepository, IUserRepository userRepository, ICompanyDetailsRepository companydetailsRepository, IContactsRepository contactRepository, ICountryTaxRepository countryTaxRepository, ICountryRepository countryRepository, ICurrencyRepository currencyRepository, ICurrencyRateRepository currencyRateRepository, IAdvertRepository advertRepository, ICampaignAdvertRepository campaignAdvertRepository, ICampaignCreditPeriodRepository campaignCreditPeriodRepository)
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
            _countryTaxRepository = countryTaxRepository;
            _countryRepository = countryRepository;
            _currencyRepository = currencyRepository;
            _currencyRateRepository = currencyRateRepository;
            _advertRepository = advertRepository;
            _campaignAdvertRepository = campaignAdvertRepository;
            _campaignCreditPeriodRepository = campaignCreditPeriodRepository;
            _currencyConversion = EFMVC.Web.Common.CurrencyConversion.CreateForCurrentUser(this, _currencyRepository);
        }

        public static int sUserId { get; set; }
        public static int? sClientId { get; set; }
        public static int sCampaignProfileId { get; set; }
        public static string sPONumber { get; set; }
        public static string sInvoiceNumber { get; set; }
        public static decimal sFundAmount { get; set; }
        public static decimal sTotalAmount { get; set; }
        public static decimal sTaxPercantage { get; set; }
        public static int scurrencyId { get; set; }
        public static string scurrencySymbol { get; set; }

        public static int scurrencyID { get; set; }

        public void GetCampaignFundAvailable()
        {
            var finalCampaignFundsAvailable = 0;
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var CountryID = _profileRepository.GetMany(top => top.UserId == efmvcUser.UserId).Select(a => a.CountryId).FirstOrDefault();
            var ConnString = ConnectionString.GetConnectionString(CountryID);
            IEnumerable<CampaignAuditFormModel> playedresult = null;
            SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString);
            SqlCommand cmd = new SqlCommand("GetCampaignFundAvailable", myConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            myConnection.Open();
            cmd.Parameters.AddWithValue("@UserId", efmvcUser.UserId);
            using (IDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ViewBag.CampaignFundsAvailable = reader["totalpurchasedfund"];
                }
            }
        }
        public double CurrencyConversion(decimal amount, string currency)
        {
            CampaignDashboardChartResult _CampaignDashboardChartResult = new CampaignDashboardChartResult();
            var exchangeRate = _currencyRateRepository.Get(top => top.CurrencyCode == currency);
            double rate = Convert.ToDouble(amount) * Convert.ToDouble(exchangeRate.CurrencyRateAmount);
            return rate;
        }
        public void GetUserCreditDetails()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            CurrencySymbol currencySymbol = new CurrencySymbol();
            CurrencyModel currencyModel = new CurrencyModel();
            var userCreditdetails = _userCreditRepository.Get(top => top.UserId == efmvcUser.UserId);

            if (userCreditdetails != null)
            {
                decimal currencyRate = 0.00M;
                var userContactData = _contactRepository.Get(top => top.UserId == efmvcUser.UserId);
                var userCurrencyId = userContactData.CurrencyId;
                var userCountryId = userContactData.CountryId;
                var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == userCountryId).TaxPercantage;
                decimal taxConversion = Convert.ToDecimal((1 + (taxpercantage / 100)).ToString("F2"));
                var currencyCode = _currencyRepository.Get(top => top.CurrencyId == userCurrencyId).CurrencyCode;
                string fromCurrencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId).CurrencyCode;
                string userCreditCurrencyCode = _currencyRepository.Get(top => top.CurrencyId == userCreditdetails.CurrencyId).CurrencyCode;
                string toCurrencyCode = currencyCode;
                if (userCreditCurrencyCode == toCurrencyCode)
                {
                    ViewBag.CreditAvailable = Convert.ToDecimal((userCreditdetails.AvailableCredit / taxConversion).ToString("F2"));
                    ViewBag.Maximumamountofcredit = Convert.ToDecimal(userCreditdetails.AssignCredit.ToString("F2"));
                }
                else
                {
                    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", userCreditCurrencyCode, toCurrencyCode);
                    currencyRate = currencyModel.Amount;
                    if (currencyModel.Code == "OK")
                    {
                        ViewBag.CreditAvailable = Convert.ToDecimal(Convert.ToDouble(userCreditdetails.AvailableCredit / taxConversion * currencyRate).ToString("F2"));
                        ViewBag.Maximumamountofcredit = Convert.ToDecimal(Convert.ToDouble(userCreditdetails.AssignCredit * currencyRate).ToString("F2"));
                    }
                }
                if (toCurrencyCode == fromCurrencyCode)
                {


                    TempData["CreditAvailable"] = Convert.ToDecimal((userCreditdetails.AvailableCredit / taxConversion).ToString("F2"));
                    var currencySymbole = currencySymbol.GetCurrencySymbolusingCurrencyId(userCurrencyId, _currencyRepository);
                    ViewBag.CurrencySymbole = currencySymbole;
                }
                else
                {
                    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                    currencyRate = currencyModel.Amount;
                    if (currencyModel.Code == "OK")
                    {


                        TempData["CreditAvailable"] = Convert.ToDecimal((userCreditdetails.AvailableCredit / taxConversion).ToString("F2"));
                        var currencySymbole = currencySymbol.GetCurrencySymbolusingCurrencyId(userCurrencyId, _currencyRepository);
                        ViewBag.CurrencySymbole = currencySymbole;
                    }
                }
            }
            else
            {
                ViewBag.Maximumamountofcredit = 0;
                ViewBag.CreditAvailable = 0;
                TempData["CreditAvailable"] = 0;
            }
        }
        public void FillClientDropdown()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var clientdetails = _clientRepository.GetAll().Where(top => top.UserId == efmvcUser.UserId && (top.Status == 1 || top.Status == 2)).Select(top => new SelectListItem { Text = top.Name, Value = top.Id.ToString() });
            var client = clientdetails.ToList();
            client.Insert(0, new SelectListItem { Text = "--Select Client--", Value = "0" });
            ViewBag.client = client;
        }
        public void FillPaymentDropdown()
        {
            var paymentMethoddetails = _paymentMethodRepository.GetAll().Select(top => new { Name = top.Description, Id = top.Id.ToString() });
            ViewBag.paymentMethod = new MultiSelectList(paymentMethoddetails, "Id", "Name");
        }
        public bool UpdateCampaignCredit(int campaignId, decimal fundcredit,decimal totalcredit)
        {
            CampaignProfileFormModel CampaignProfileFormModel = new CampaignProfileFormModel();
            CreateOrUpdateCampaignProfileCommand command = Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(CampaignProfileFormModel);
            var campaigndetails = _profileRepository.GetById(campaignId);
            if (campaigndetails != null)
            {
                //var totalcampaignbudget = Convert.ToDouble(campaigndetails.TotalBudget) + (float)totalcredit;
                var totalcampaignbudget = campaigndetails.TotalBudget + fundcredit;
                var final_credit = campaigndetails.TotalCredit + totalcredit;
                command.CampaignProfileId = campaigndetails.CampaignProfileId;
                command.TotalCredit = final_credit;
                command.TotalBudget = totalcampaignbudget;
                command.Status = (int)CampaignStatus.Play;
                command.UserId = campaigndetails.UserId;
                command.CampaignDescription = campaigndetails.CampaignDescription;
                command.CampaignName = campaigndetails.CampaignName;
                command.ClientId = campaigndetails.ClientId;
                command.Active = campaigndetails.Active;
                command.AvailableCredit = (campaigndetails.AvailableCredit + command.TotalCredit);
                command.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                command.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                command.CancelledToDate = campaigndetails.CancelledToDate;
                command.CreatedDateTime = campaigndetails.CreatedDateTime;
                command.EmailToDate = campaigndetails.EmailToDate;
                command.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                command.EmailsLastMonth = campaigndetails.EmailsLastMonth;
                command.MaxBid = campaigndetails.MaxBid;
                command.MaxMonthBudget = campaigndetails.MaxWeeklyBudget;
                command.MaxWeeklyBudget = campaigndetails.MaxWeeklyBudget;
                command.MaxHourlyBudget = campaigndetails.MaxHourlyBudget;
                command.SpendToDate = campaigndetails.SpendToDate;
                command.MaxDailyBudget = campaigndetails.MaxDailyBudget;
                command.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                command.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                command.PlaysToDate = campaigndetails.PlaysToDate;
                command.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                command.SmsLastMonth = campaigndetails.SmsLastMonth;
                command.SmsToDate = campaigndetails.SmsToDate;
                command.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                command.UserId = campaigndetails.UserId;
                command.EmailBody = campaigndetails.EmailBody;
                command.EmailSenderAddress = campaigndetails.EmailSenderAddress;
                command.EmailSubject = campaigndetails.EmailSubject;
                command.SmsBody = campaigndetails.SmsBody;
                command.SmsOriginator = campaigndetails.SmsOriginator;
                command.IsAdminApproval = campaigndetails.IsAdminApproval;
                command.CountryId = (int)campaigndetails.CountryId;
                command.CurrencyCode = campaigndetails.CurrencyCode;
                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaigndetails.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex SQLServerEntities = new EFMVCDataContex(item);
                        var campaigndetailFromOP = SQLServerEntities.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignId).FirstOrDefault();
                        if (campaigndetailFromOP != null)
                        {
                            var campaignmatch2 = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campaigndetailFromOP.CampaignProfileId).FirstOrDefault();
                            if (campaignmatch2 != null)
                            {
                                campaignmatch2.TotalCredit = Convert.ToDecimal(final_credit);
                                campaignmatch2.TotalBudget = Convert.ToDecimal(totalcampaignbudget);
                                var available = campaigndetails.AvailableCredit + command.TotalCredit;
                                campaignmatch2.AvailableCredit = available.ToString();
                                campaignmatch2.Status = (int)CampaignStatus.Play;
                                SQLServerEntities.SaveChanges();
                            }
                        }
                    }
                }
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    return true;
                }
            }
            return false;
        }
        public void FillCreditCardType()
        {
            IEnumerable<Common.CreditCardType> creditcardTypes = Enum.GetValues(typeof(Common.CreditCardType)).Cast<Common.CreditCardType>();
            var creditcard = (from action in creditcardTypes select new SelectListItem { Text = action.ToString(), Value = ((int)action).ToString() }).ToList();
            ViewBag.creditcard = creditcard;
        }
        [Authorize(Roles = "Advertiser")]
        public ActionResult PaymentProcess()
        {
            String payerid = Request.QueryString["PAYERID"];
            String token = Request.QueryString["TOKEN"];
            NVPCallerServices caller2 = new NVPCallerServices();
            IAPIProfile profile2 = ProfileFactory.createSignatureAPIProfile();
            SetCredentials(caller2, profile2);
            Paypal.NVPCodec encoder2 = new Paypal.NVPCodec();
            encoder2["VERSION"] = "109.0";
            encoder2["METHOD"] = "GetExpressCheckoutDetails";
            encoder2["PAYERID"] = payerid;
            encoder2["TOKEN"] = token;
            string DoECStr = encoder2.Encode();
            string DoECResp = caller2.Call(DoECStr);
            Paypal.NVPCodec decoder2 = new Paypal.NVPCodec();
            decoder2.Decode(DoECResp);
            string[] resp = DoECResp.Split('&');
            int iresp = resp.Length - 1;
            foreach (string sresp in resp)
            {
                string[] aresp = sresp.Split('=');
                if (aresp.Length > 1)
                {
                    string sKey = aresp[0];
                    string sVal = aresp[1];
                    if (sKey == "ACK")
                    {
                        if (sVal == "Success" || sVal == "SuccessWithWarning")
                        {
                            caller2 = new NVPCallerServices();
                            profile2 = ProfileFactory.createSignatureAPIProfile();
                            SetCredentials(caller2, profile2);
                            encoder2 = DoExpressCheckout(payerid, token, TempData["Fundamount"].ToString(), TempData["emailaddress"].ToString(), TempData["ponumber"].ToString());
                            // Execute the API operation and obtain the response.
                            DoECStr = encoder2.Encode();
                            DoECResp = caller2.Call(DoECStr);
                            decoder2 = new Paypal.NVPCodec();
                            decoder2.Decode(DoECResp);
                            resp = DoECResp.Split('&');
                            string transactionid = "";
                            foreach (string srespo in resp)
                            {
                                string[] arespo = srespo.Split('=');
                                if (arespo.Length > 1)
                                {
                                    sKey = arespo[0];
                                    sVal = arespo[1];
                                    if (sKey == "ACK")
                                    {
                                        if (sVal.Trim().ToLower() == "failure")
                                        {
                                            TempData["error"] = decoder2.GetValues("L_LONGMESSAGE0")[0].ToString();
                                            break;
                                        }
                                    }
                                    else if (sKey == "PAYMENTINFO_0_TRANSACTIONID")
                                    {
                                        var currencySymbol = Session["CurrencySymbol"];
                                        CreatePDF(Convert.ToInt32(TempData["BilllingID"]), Convert.ToInt32(TempData["UserId"]), 2, "Instantpayment", currencySymbol.ToString(), "", "");
                                        transactionid = sVal;
                                        //update campaign credit available of user
                                        bool campaigncreditstatus = UpdateCampaignCredit(Convert.ToInt32(TempData["CampaingId"]), Convert.ToDecimal(TempData["Fundamount"].ToString()), Convert.ToDecimal(TempData["Fundamount"].ToString()));
                                        if (campaigncreditstatus)
                                        {
                                            AddBillingDetails(transactionid);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            TempData["error"] = decoder2.GetValues("L_LONGMESSAGE0")[0].ToString();
                            break;
                        }
                    }
                }
            }
            return RedirectToAction("buy_credit");
        }
        [Authorize(Roles = "Advertiser")]
        public ActionResult AddBillingDetails(string transId)
        {
            string paypalId = TempData["emailaddress"].ToString();
            BillingDetailsFormModel _billingdetails = new BillingDetailsFormModel();
            _billingdetails.BillingId = Convert.ToInt32(TempData["BilllingID"]);
            _billingdetails.PaypalTranID = transId;
            _billingdetails.PaypalEmail = paypalId;
            _billingdetails.CardNumber = String.Empty;
            _billingdetails.ExpiryMonth = String.Empty;
            _billingdetails.ExpiryYear = String.Empty;
            _billingdetails.SecurityCode = String.Empty;
            _billingdetails.BillingAddress = String.Empty;
            _billingdetails.BillingTown = String.Empty;
            _billingdetails.BillingPostcode = String.Empty;
            CreateOrUpdateBillingDetailsCommand commands = Mapper.Map<BillingDetailsFormModel, CreateOrUpdateBillingDetailsCommand>(_billingdetails);
            ICommandResult result = _commandBus.Submit(commands);
            if (result.Success)
            {
                int billingId = Convert.ToInt32(TempData["BilllingID"]);
                var _updatebillingdetails = _billingRepository.Get(x => x.Id == billingId);
                if (_updatebillingdetails != null)
                {
                    _updatebillingdetails.Status = 1;
                    BillingFormModel billing = Mapper.Map<Billing, BillingFormModel>(_updatebillingdetails);
                    CreateOrUpdateBillingCommand billingcommand = Mapper.Map<BillingFormModel, CreateOrUpdateBillingCommand>(billing);
                    ICommandResult billingresult = _commandBus.Submit(billingcommand);
                    if (billingresult.Success)
                    {
                        TempData["success"] = "Payment received successfully for " + _updatebillingdetails.InvoiceNumber;
                        return RedirectToAction("buy_credit");
                    }
                }
            }
            TempData["error"] = "Internal Server Error. Please try again.";
            return RedirectToAction("buy_credit");
        }
        public bool AddBillingDetails(string transId, BillingPaymentInfoDetails _model, int billingID)
        {
            BillingDetailsFormModel _billingdetails = new BillingDetailsFormModel();
            _billingdetails.BillingId = billingID;
            _billingdetails.PaypalTranID = transId;
            _billingdetails.PaypalEmail = String.Empty;
            _billingdetails.CardType = _model.CardType;
            var cardNumber = _model.CardNumber.Substring(_model.CardNumber.Length - 4); ;
            _billingdetails.CardNumber = "XXXX-XXXX-XXXX-" + cardNumber;
            _billingdetails.NameOfCard = _model.NameOfCard;
            _billingdetails.ExpiryMonth = _model.ExpiryMonth;
            _billingdetails.ExpiryYear = _model.ExpiryYear;
            _billingdetails.SecurityCode = String.Empty;
            _billingdetails.BillingAddress = _model.BillingAddress;
            _billingdetails.BillingTown = _model.BillingTown;
            _billingdetails.BillingPostcode = _model.BillingPostcode;
            CreateOrUpdateBillingDetailsCommand commands = Mapper.Map<BillingDetailsFormModel, CreateOrUpdateBillingDetailsCommand>(_billingdetails);
            ICommandResult result = _commandBus.Submit(commands);
            if (result.Success)
            {
                var _updatebillingdetails = _billingRepository.Get(x => x.Id == billingID);
                if (_updatebillingdetails != null)
                {
                    _updatebillingdetails.Status = 1;
                    BillingFormModel billing = Mapper.Map<Billing, BillingFormModel>(_updatebillingdetails);
                    CreateOrUpdateBillingCommand billingcommand = Mapper.Map<BillingFormModel, CreateOrUpdateBillingCommand>(billing);
                    ICommandResult billingresult = _commandBus.Submit(billingcommand);
                    if (billingresult.Success)
                    {
                        TempData["success"] = "Payment received successfully for " + _model.InvoiceNumber;
                        return true;
                    }
                }
            }
            return false;
        }
        public bool AddSageBillingDetails(BillingPaymentInfoDetails _model, int billingID)
        {
            BillingDetailsFormModel _billingdetails = new BillingDetailsFormModel();
            _billingdetails.BillingId = billingID;
            _billingdetails.PaypalTranID = TempData["TranID"].ToString();
            _billingdetails.PaypalEmail = String.Empty;
            _billingdetails.CardType = _model.CardType;
            var cardNumber = _model.CardNumber.Substring(_model.CardNumber.Length - 4); ;
            _billingdetails.CardNumber = "XXXX-XXXX-XXXX-" + cardNumber;
            _billingdetails.NameOfCard = _model.NameOfCard;
            _billingdetails.ExpiryMonth = _model.ExpiryMonth;
            _billingdetails.ExpiryYear = _model.ExpiryYear;
            _billingdetails.SecurityCode = String.Empty;
            _billingdetails.BillingAddress = _model.BillingAddress;
            _billingdetails.BillingAddress2 = _model.BillingAddress2;
            _billingdetails.BillingTown = _model.BillingTown;
            _billingdetails.BillingPostcode = _model.BillingPostcode;
            CreateOrUpdateBillingDetailsCommand commands = Mapper.Map<BillingDetailsFormModel, CreateOrUpdateBillingDetailsCommand>(_billingdetails);
            ICommandResult result = _commandBus.Submit(commands);
            if (result.Success)
            {
                var _updatebillingdetails = _billingRepository.Get(x => x.Id == billingID);
                if (_updatebillingdetails != null)
                {
                    _updatebillingdetails.Status = 1;
                    BillingFormModel billing = Mapper.Map<Billing, BillingFormModel>(_updatebillingdetails);
                    CreateOrUpdateBillingCommand billingcommand = Mapper.Map<BillingFormModel, CreateOrUpdateBillingCommand>(billing);
                    ICommandResult billingresult = _commandBus.Submit(billingcommand);
                    if (billingresult.Success)
                    {
                        TempData["success"] = "Payment received successfully";
                        return true;
                    }
                }
            }
            return false;
        }
        [Authorize(Roles = "Advertiser")]
        public ActionResult CancelPaymentProcess()
        {
            var billingId = Convert.ToInt32(TempData["BilllingID"].ToString());
            var status = DeleteTempBilling(billingId);
            if (status)
            {
                TempData["error"] = "transaction is abored by the user. so please try again";
            }
            else
            {
                TempData["error"] = "Internal Server error. so please try again";
            }
            return RedirectToAction("buy_credit");
        }
        private static Paypal.NVPCodec DoExpressCheckout(string payerid, string token, string amount, string email, string ponumber)
        {
            Paypal.NVPCodec encoder2 = new Paypal.NVPCodec();
            encoder2["VERSION"] = "109.0";
            encoder2["METHOD"] = "DoExpressCheckoutPayment";
            // Add request-specific fields to the request.
            encoder2["PAYERID"] = payerid;
            encoder2["TOKEN"] = token;
            encoder2["L_PAYMENTREQUEST_0_NAME0"] = "Adtones";
            encoder2["L_PAYMENTREQUEST_0_NUMBER0"] = ponumber;
            encoder2["L_PAYMENTREQUEST_0_DESC0"] = "campaign fund amount";
            encoder2["L_PAYMENTREQUEST_0_AMT0"] = amount;
            encoder2["L_PAYMENTREQUEST_0_QTY0"] = "1";
            encoder2["PAYMENTREQUEST_0_AMT"] = amount;
            encoder2["PAYMENTREQUEST_0_CURRENCYCODE"] = "EUR";
            encoder2["PAYMENTREQUEST_0_ITEMAMT"] = amount;
            encoder2["ALLOWNOTE"] = "1";
            encoder2["EMAIL"] = email;
            encoder2["LANDINGPAGE"] = "Billing";
            encoder2["ADDROVERRIDE"] = "1";
            return encoder2;
        }
        private static void SetCredentials(NVPCallerServices caller2, IAPIProfile profile2)
        {
            profile2.APIUsername = ConfigurationManager.AppSettings["paypalmerchantemail"].ToString();
            profile2.APIPassword = ConfigurationManager.AppSettings["paypalmerchantpassword"].ToString();
            profile2.APISignature = ConfigurationManager.AppSettings["paypalsecurity"].ToString();
            profile2.Environment = ConfigurationManager.AppSettings["paypalmode"].ToString();
            caller2.APIProfile = profile2;
        }
        [Authorize(Roles = "Advertiser")]
        public ActionResult Index()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            CurrencySymbol currencySymbol = new CurrencySymbol();
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            IEnumerable<ClientModel> clientModels = Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            var userCountryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
            string currencysymbol = "";
            string currencyCode = "";
            var userCurrencyId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
            if (userCurrencyId != null || userCurrencyId != 0)
            {
                currencysymbol = currencySymbol.GetCurrencySymbolusingCurrencyId(userCurrencyId, _currencyRepository);
                currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
            }
            else
            {
                currencysymbol = currencySymbol.GetCurrencySymbolusingCountryId(userCountryId, _currencyRepository);
                currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
            }
            List<BillingResult> _result = new List<BillingResult>();
            _result.Add(new BillingResult { CurrencySymbol = currencysymbol, CurrencyCode = currencyCode });
            BillingFilter _filterCritearea = new BillingFilter();
            FillClient(clientModels);
            FillStatus();
            FillPaymentDropdown();
            ViewBag.userCountryId = userCountryId;
            ViewBag.userCurrencyId = userCurrencyId;
            return View(Tuple.Create(_result, _filterCritearea));
        }
        [Authorize(Roles = "Advertiser")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                CurrencySymbol currencySymbol = new CurrencySymbol();
                List<BillingResult> _result = new List<BillingResult>();

                CurrencyModel currencyModel = new CurrencyModel();
                IEnumerable<Billing> billings = null;
                string status = string.Empty;
                ViewBag.SearchResult = false;
                var cnt = 10;
                int userId = 0;
                bool searchValue = false;
                List<String> columnSearch = new List<string>();
                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                    if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null") searchValue = true;
                }

                if (searchValue == true)
                {
                    #region Search Functionality
                    string InvoiceNo = "";
                    string PONo = "";
                    int?[] ClientId = new int?[cnt];
                    int[] BillingStatusId = new int[cnt];
                    int[] ModeofPaymentId = new int[cnt];
                    DateTime Invoicefromdate = new DateTime();
                    DateTime Invoicetodate = new DateTime();
                    DateTime Settledfromdate = new DateTime();
                    DateTime Settledtodate = new DateTime();
                    string invoicetotalfrom = "";
                    string invoicetotalto = "";
                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null") InvoiceNo = columnSearch[0].ToString();
                        else columnSearch[0] = null;
                    }
                    if (!String.IsNullOrEmpty(columnSearch[1]))
                    {
                        if (columnSearch[1] != "null") PONo = columnSearch[1].ToString();
                        else columnSearch[1] = null;
                    }
                    if (!String.IsNullOrEmpty(columnSearch[2]))
                    {
                        if (columnSearch[2] != "null") ClientId = columnSearch[2].Split(',').Select(a => (int?)Convert.ToInt32(a)).ToArray();
                        else columnSearch[2] = null;
                    }
                    if (!String.IsNullOrEmpty(columnSearch[3]))
                    {
                        if (columnSearch[3] != "null")
                        {
                            var data = columnSearch[3].Split(',').ToArray();
                            Invoicefromdate = Convert.ToDateTime(data[0]);
                            Invoicetodate = Convert.ToDateTime(data[1]);
                        }
                        else columnSearch[3] = null;
                    }
                    if (!String.IsNullOrEmpty(columnSearch[4]))
                    {
                        if (columnSearch[4] != "null")
                        {
                            var data = columnSearch[4].Split(',').ToArray();
                            Settledfromdate = Convert.ToDateTime(data[0]);
                            Settledtodate = Convert.ToDateTime(data[1]);
                        }
                        else columnSearch[4] = null;
                    }
                    if (!String.IsNullOrEmpty(columnSearch[5]))
                    {
                        if (columnSearch[5] != "null")
                        {
                            var data = columnSearch[5].Split(',').ToArray();
                            invoicetotalfrom = data[0].ToString();
                            invoicetotalto = data[1].ToString();
                        }
                        else columnSearch[5] = null;
                    }
                    if (!String.IsNullOrEmpty(columnSearch[6]))
                    {
                        if (columnSearch[6] != "null") BillingStatusId = columnSearch[6].Split(',').Select(int.Parse).ToArray();
                        else columnSearch[6] = null;
                    }
                    if (!String.IsNullOrEmpty(columnSearch[7]))
                    {
                        if (columnSearch[7] != "null") ModeofPaymentId = columnSearch[7].Split(',').Select(int.Parse).ToArray();
                        else columnSearch[7] = null;
                    }
                    billings = _billingRepository.GetMany(x => x.UserId == efmvcUser.UserId).OrderByDescending(x => x.Id).ToList();
                    if (columnSearch[0] != null)
                    {
                        billings = billings.Where(top => top.InvoiceNumber == InvoiceNo).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        billings = billings.Where(top => top.PONumber == PONo).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        billings = billings.Where(top => (ClientId.Contains(Convert.ToInt32(top.ClientId)))).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        billings = billings.Where(top => (top.PaymentDate >= Invoicefromdate && top.PaymentDate <= Invoicetodate)).ToList();
                    }
                    if (columnSearch[4] != null)
                    {
                        billings = billings.Where(top => (top.SettledDate >= Settledfromdate && top.SettledDate <= Settledtodate)).ToList();
                    }
                    if (columnSearch[5] != null)
                    {
                        billings = billings.Where(top => (top.TotalAmount >= Convert.ToDecimal(invoicetotalfrom) && top.TotalAmount <= Convert.ToDecimal(invoicetotalto))).ToList();
                    }
                    if (columnSearch[6] != null)
                    {
                        billings = billings.Where(top => (BillingStatusId.Contains((int)top.Status))).ToList();
                    }
                    if (columnSearch[7] != null)
                    {
                        billings = billings.Where(top => (ModeofPaymentId.Contains((int)top.PaymentMethodId))).ToList();
                    }
                    cnt = billings.Count();
                    billings = billings.Skip(param.Start).Take(param.Length);
                    #endregion
                }
                else
                {
                    billings = _billingRepository.GetMany(x => x.UserId == efmvcUser.UserId).OrderByDescending(x => x.Id).ToList();
                    cnt = billings.Count();
                    billings = billings.Skip(param.Start).Take(param.Length);
                }
                var userCountryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
                string currencysymbol = "";
                string currencyCode = "";
                var userCurrencyId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
                if (userCurrencyId != null || userCurrencyId != 0)
                {
                    currencysymbol = currencySymbol.GetCurrencySymbolusingCurrencyId(userCurrencyId, _currencyRepository);
                    currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
                }
                else
                {
                    currencysymbol = currencySymbol.GetCurrencySymbolusingCountryId(userCountryId, _currencyRepository);
                    currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
                }
                if (billings.Count() > 0)
                {
                    foreach (var item in billings)
                    {
                        double invoiceTotal = 0.00;
                        decimal currencyRate = 0.00M;
                        string fromCurrencyCode = item.CurrencyCode;
                        string toCurrencyCode = currencyCode;
                        if (toCurrencyCode == fromCurrencyCode)
                        {
                            invoiceTotal = Convert.ToDouble(item.FundAmount.ToString());
                        }
                        else
                        {
                            currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                            currencyRate = currencyModel.Amount;
                            if (currencyModel.Code == "OK")
                            {
                                invoiceTotal = Convert.ToDouble(item.FundAmount * currencyRate);
                            }
                        }
                        BillingStatus billingStatus = (BillingStatus)item.Status;
                        status = billingStatus.ToString();
                        _result.Add(new BillingResult { ID = item.Id, InvoiceNO = item.InvoiceNumber, PONumber = item.PONumber, ClienId = item.ClientId == null ? 0 : item.ClientId.Value, ClientName = item.ClientId == null ? "-" : item.Client.Name, CampaignId = item.CampaignProfileId == null ? 0 : item.CampaignProfileId.Value, CampaignName = item.CampaignProfileId == null ? null : item.CampaignProfile.CampaignName, InvoiceDate = Convert.ToDateTime(item.PaymentDate), InvoiceTotal = Convert.ToDecimal(invoiceTotal.ToString("F2")), status = status, SettledDate = item.SettledDate, MethodOfPayment = item.PaymentMethod.Description, PaymentMethodId = item.PaymentMethodId == null ? 0 : item.PaymentMethodId.Value, fstatus = item.Status, CurrencySymbol = currencysymbol, CurrencyCode = currencyCode });
                    }
                }
                ViewBag.userCountryId = userCountryId;
                ViewBag.userCurrencyId = userCurrencyId;
                _result = ApplySorting(param, _result);
                DTResult<BillingResult> result = new DTResult<BillingResult>
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
        private static List<BillingResult> ApplySorting(DTParameters param, List<BillingResult> result)
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
                        result = result.OrderBy(top => top.ClientName).ToList();
                    else
                        result = result.OrderByDescending(top => top.ClientName).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CampaignName).ToList();
                    else
                        result = result.OrderByDescending(top => top.CampaignName).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.InvoiceDate).ToList();
                    else
                        result = result.OrderByDescending(top => top.InvoiceDate).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.InvoiceTotal).ToList();
                    else
                        result = result.OrderByDescending(top => top.InvoiceTotal).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.status).ToList();
                    else
                        result = result.OrderByDescending(top => top.status).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.SettledDate).ToList();
                    else
                        result = result.OrderByDescending(top => top.SettledDate).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.MethodOfPayment).ToList();
                    else
                        result = result.OrderByDescending(top => top.MethodOfPayment).ToList();
                }
            }
            return result;
        }
        private static Paypal.NVPCodec SetExpressCheckout(NVPCallerServices caller, IAPIProfile profile, string amount, string email, string invoiceno, string currencyCode)
        {
            buildPaypalCredentials(profile);
            caller.APIProfile = profile;
            Paypal.NVPCodec encoder = new Paypal.NVPCodec();
            buildNVPCredentials(encoder);
            encoder["L_PAYMENTREQUEST_0_NAME0"] = "Adtones";
            encoder["L_PAYMENTREQUEST_0_NUMBER0"] = invoiceno;
            encoder["L_PAYMENTREQUEST_0_DESC0"] = "campaign fund amount";
            encoder["L_PAYMENTREQUEST_0_AMT0"] = amount;
            encoder["L_PAYMENTREQUEST_0_QTY0"] = "1";
            encoder["PAYMENTREQUEST_0_AMT"] = amount;
            encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = currencyCode;
            encoder["PAYMENTREQUEST_0_ITEMAMT"] = amount;
            encoder["ALLOWNOTE"] = "1";
            encoder["EMAIL"] = email;
            encoder["LANDINGPAGE"] = "Billing";
            encoder["ADDROVERRIDE"] = "1";
            encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "sale";
            encoder["RETURNURL"] = ConfigurationManager.AppSettings["siteAddress"] + "Billing/PaymentProcess";
            encoder["CANCELURL"] = ConfigurationManager.AppSettings["siteAddress"] + "Billing/CancelPaymentProcess";
            encoder["CallbackURL"] = ConfigurationManager.AppSettings["siteAddress"] + "Billing/PaymentProcess";
            return encoder;
        }
        private static void buildNVPCredentials(Paypal.NVPCodec encoder)
        {
            encoder["VERSION"] = "109.0";
            encoder["METHOD"] = "SetExpressCheckout";
            encoder["USER"] = ConfigurationManager.AppSettings["paypalmerchantemail"].ToString();
            encoder["PWD"] = ConfigurationManager.AppSettings["paypalmerchantpassword"].ToString();
            encoder["SIGNATURE"] = ConfigurationManager.AppSettings["paypalsecurity"].ToString();
        }
        private static void buildPaypalCredentials(IAPIProfile profile)
        {
            profile.APIUsername = ConfigurationManager.AppSettings["paypalmerchantemail"].ToString();
            profile.APIPassword = ConfigurationManager.AppSettings["paypalmerchantpassword"].ToString();
            profile.APISignature = ConfigurationManager.AppSettings["paypalsecurity"].ToString();
            profile.Environment = ConfigurationManager.AppSettings["paypalmode"].ToString();
        }
        private void SetUserNotification(string notification)
        {
            TempData["ErrorMessage"] = notification;
        }
        [Authorize(Roles = "Advertiser")]
        public ActionResult buy_credit()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            PaymentModel _payment = new PaymentModel();
            BillingInfoDetails _billing = new BillingInfoDetails();
            BillingPaymentInfoDetails _billingpayment = new BillingPaymentInfoDetails();
            BillingPaymentInfoDetailsWithPaypal _billingpaymentpaypal = new BillingPaymentInfoDetailsWithPaypal();
            BillingPaymentInfoDetailsWithMpesa _billingpaymentmpesa = new BillingPaymentInfoDetailsWithMpesa();
            CurrencySymbol currencySymbol = new CurrencySymbol();
            _billingpaymentpaypal.PONumber = _billingpayment.PONumber;
            _billing.PaymentDate = DateTime.Now;
            _payment.BillingInfoDetails = _billing;
            _payment.BillingPaymentInfoDetailswithPaypalCreditCard = _billingpayment;
            _payment.BillingPaymentInfoDetailswithPaypal = _billingpaymentpaypal;
            _payment.MpesaBillingDetails = _billingpaymentmpesa;
            FillClientDropdown();
            FillCreditCardType();
            GetUserCreditDetails();
            FillCurrencyList();
            var userCountryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
            var userCountryCode = _currencyRepository.Get(c => c.CountryId == userCountryId).CurrencyCode;
            ViewBag.userCountryCode = userCountryCode;
            int userCountryID = 0;
            if (userCountryId == 12 || userCountryId == 13 || userCountryId == 14) userCountryID = 12;
            else if (userCountryId == 11) userCountryID = 8;
            else userCountryID = Convert.ToInt32(userCountryId);
            string countrySymbol = currencySymbol.GetCurrencySymbolusingCountryId(userCountryID, _currencyRepository);
            ViewBag.countrySymbole = countrySymbol;
            var currencyCode = _currencyRepository.Get(c => c.CountryId == userCountryID).CurrencyCode;
            if (Request.QueryString["clientId"] != null) _billing.ClientId = Convert.ToInt16(Request.QueryString["clientId"]);
            if (Request.QueryString["campaignId"] != null)
            {
                var countryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
                int CountryId = 0;
                if (countryId == 12 || countryId == 13 || countryId == 14) CountryId = 12;
                else if (countryId == 11) CountryId = 8;
                else CountryId = Convert.ToInt32(countryId);
            }
            ViewBag.currencyCode = "(" + currencyCode + ")";
            return View(_payment);
        }
        [Authorize(Roles = "Advertiser")]
        [HttpPost]
        public ActionResult PaywithUserCredit(BillingPaymentInfoDetails _model)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                // CurrencySymbol currencySymbol = new CurrencySymbol();
                CurrencyModel currencyModel = new CurrencyModel();
                var companydetails = _companydetailsRepository.Get(top => top.UserId == efmvcUser.UserId);
                if (Convert.ToDecimal(TempData["Fundamount"].ToString()) <= 0)
                {
                    TempData["error"] = "Please enter a fundAmount bigger than {0}.";
                    return RedirectToAction("buy_credit");
                }
                if (companydetails.Country == null)
                {
                    TempData["error"] = "Please update your country details before payment.";
                    return RedirectToAction("buy_credit");
                }
                if (companydetails == null)
                {
                    TempData["error"] = "Please update your company details before payment.";
                    return RedirectToAction("buy_credit");
                }
                var contactdetails = _contactRepository.Get(top => top.UserId == efmvcUser.UserId);
                if (contactdetails == null)
                {
                    TempData["error"] = "Please update your contact details before payment.";
                    return RedirectToAction("buy_credit");
                }
                int errorstatus = 0;
                errorstatus = ValidateBilling(errorstatus);
                if (errorstatus == 1) return RedirectToAction("buy_credit");

                int campaignCountryId = Convert.ToInt32(Session["CountryId"].ToString());
                int currencyId = Convert.ToInt32(Session["currencyId"].ToString());

                var currencyCountryData = _currencyRepository.Get(c => c.CurrencyId == currencyId);
                var currencyCountryId = currencyCountryData.Country.Id;
                var currencyCountryCode = currencyCountryData.CurrencyCode;
                var currencyCode = _currencyRepository.Get(c => c.CountryId == campaignCountryId).CurrencyCode;
                decimal currencyRate = 0.00M;
                string fromCurrencyCode = currencyCountryCode;
                string toCurrencyCode = currencyCode;
                int CountryId = 0;
                if (TempData["CampaingId"] != null)
                {
                    var countryId = _profileRepository.GetById(Convert.ToInt32(TempData["CampaingId"].ToString())).CountryId;
                    if (countryId == 12 || countryId == 13 || countryId == 14) CountryId = 12;
                    else if (countryId == 11) CountryId = 8;
                    else CountryId = Convert.ToInt32(countryId);
                }
                if (currencyCountryId == campaignCountryId)
                {
                    //check credit available
                    var CreditAvailable = Convert.ToDecimal(TempData["CreditAvailable"].ToString());
                    var userfundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                    if (CreditAvailable == 0)
                    {
                        TempData["error"] = "Credit is not available so please try again.";//Credit is not available.so please try again.
                        return RedirectToAction("buy_credit");
                    }
                    else if (userfundamount > CreditAvailable)
                    {
                        TempData["error"] = "Fund amount is more than credit available";
                        return RedirectToAction("buy_credit");
                    }
                    var finalamount = CreditAvailable - userfundamount;
                    var fundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                    _model.Fundamount = fundamount;
                    var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage;
                    var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
                    var final_amount = _model.Fundamount + totaltaxamount;
                    _model.TaxPercantage = taxpercantage;
                    _model.TotalAmount = final_amount;
                }
                else
                {
                    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                    currencyRate = currencyModel.Amount;
                    if (currencyModel.Code == "OK")
                    {
                        // double userCreditAvailable = Convert.ToDouble(TempData["CreditAvailable"].ToString());
                        double CreditAvailable = Convert.ToDouble(TempData["CreditAvailable"].ToString());
                        double FundAmount = Convert.ToDouble(Convert.ToDecimal(TempData["Fundamount"].ToString()) * currencyRate);
                        if (CreditAvailable == 0)
                        {
                            TempData["error"] = "Credit is not available so please try again.";//Credit is not available.so please try again.
                            return RedirectToAction("buy_credit");
                        }
                        else if (FundAmount > CreditAvailable)
                        {
                            TempData["error"] = "Fund amount is more than credit available";
                            return RedirectToAction("buy_credit");
                        }
                        var finalamount = CreditAvailable - FundAmount;
                        _model.Fundamount = Convert.ToDecimal(FundAmount.ToString());
                        var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage;
                        var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
                        //var final_amount = _model.Fundamount + totaltaxamount;
                        var final_amount = Convert.ToDecimal(FundAmount.ToString()) + totaltaxamount;
                        _model.TaxPercantage = taxpercantage;
                        _model.TotalAmount = final_amount;
                    }
                }
                _model.InvoiceNumber = "A" + RandomString(6) + DateTime.Now.ToString("yy");
                BillingFormModel _billingdetails = new BillingFormModel();
                _billingdetails.PaymentMethodId = 1;
                _billingdetails.Status = 2;
                _billingdetails.CurrencyCode = currencyCode;
                _billingdetails.SettledDate = DateTime.Now.AddDays(_userRepository.Get(top => top.UserId == efmvcUser.UserId).Outstandingdays);
                InsertBillingInfo(_model, efmvcUser, _billingdetails);
                CreateOrUpdateBillingCommand command = Mapper.Map<BillingFormModel, CreateOrUpdateBillingCommand>(_billingdetails);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //Update Advert And CampaignAdvert
                    UpdateAdvertAndCampaignAdvertDetails(command.CampaignProfileId, CountryId);
                    bool creditStatus = false;

                    var userCreditCurrencyId = _userCreditRepository.Get(top => top.UserId == efmvcUser.UserId).CurrencyId;
                    var userCreditCurrencyCode = _currencyRepository.Get(c => c.CurrencyId == userCreditCurrencyId).CurrencyCode;
                    var selectedCurrencyCode = _currencyRepository.Get(c => c.CurrencyCode == fromCurrencyCode).CurrencyId;

                    var CreditAvailableTemp = Convert.ToDecimal(TempData["CreditAvailable"].ToString());
                    // var userfundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                    var finalamount = CreditAvailableTemp - _model.TotalAmount;

                    if (userCreditCurrencyId != selectedCurrencyCode)
                    {
                        currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, userCreditCurrencyCode);
                        currencyRate = currencyModel.Amount;
                        if (currencyModel.Code == "OK")
                        {
                            CreditAvailableTemp = Convert.ToDecimal(TempData["CreditAvailable"].ToString());
                            var userfundamount = Convert.ToDecimal(_model.TotalAmount * currencyRate);
                            finalamount = CreditAvailableTemp - userfundamount;
                        }
                    }

                    // 12-12-2019 333
                    // if (currencyCountryId == campaignCountryId)

                    // var fundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());

                    var campaignCurrencyCode = _profileRepository.GetById(Convert.ToInt32(TempData["CampaingId"].ToString())).CurrencyCode;
                    var selectedCurrencyCurrency = _currencyRepository.Get(c => c.CurrencyId == selectedCurrencyCode).CurrencyCode;
                    if (campaignCurrencyCode == selectedCurrencyCurrency)
                    {
                        bool campaigncreditstatus = UpdateCampaignCredit(Convert.ToInt32(TempData["CampaingId"]),_model.Fundamount, _model.TotalAmount);
                        if (campaigncreditstatus)
                        {
                            //update credit available of user
                            creditStatus = UpdateUserCredit(efmvcUser.UserId, finalamount);
                        }
                        else TempData["error"] = "Internal Server error. Please try again.";
                    }
                    else
                    {
                        currencyModel = _currencyConversion.ForeignCurrencyConversion("1", selectedCurrencyCurrency, campaignCurrencyCode);
                        currencyRate = currencyModel.Amount;
                        if (currencyModel.Code == "OK")
                        {
                            double FundAmount = Convert.ToDouble(Convert.ToDecimal(TempData["Fundamount"].ToString()) * currencyRate);
                            bool campaigncreditstatus = UpdateCampaignCredit(Convert.ToInt32(TempData["CampaingId"]), _model.Fundamount, _model.TotalAmount);
                            if (campaigncreditstatus)
                            {
                                creditStatus = UpdateUserCredit(efmvcUser.UserId, decimal.Parse(finalamount.ToString()));
                            }
                            else
                            {
                                TempData["error"] = "Internal Server error. Please try again.";
                            }
                        }
                    }

                    if (creditStatus)
                    {
                        TempData["success"] = "Payment received successfully for " + _model.InvoiceNumber;
                        int a = result.Id;
                        int b = efmvcUser.UserId;
                        //var currencySymbol1 = currencySymbol.GetCurrencySymbolusingCountryId(campaignCountryId);
                        var currencySymbol1 = Session["CurrencySymbol"].ToString();
                        var pdfStatus = CreatePDF(result.Id, efmvcUser.UserId, 1, "CreditPayment", currencySymbol1, fromCurrencyCode, toCurrencyCode);
                        if (pdfStatus) return RedirectToAction("Index", "Billing");
                    }
                    else TempData["error"] = "Internal Server error. Please try again.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message.ToString();
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Advertiser")]
        [HttpPost]
        public ActionResult PaywithPaypal(BillingPaymentInfoDetailsWithPaypal _model)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                CurrencySymbol currencySymbol = new CurrencySymbol();
                CurrencyModel currencyModel = new CurrencyModel();
                var companydetails = _companydetailsRepository.Get(top => top.UserId == efmvcUser.UserId);
                if (Convert.ToDecimal(TempData["Fundamount"].ToString()) <= 0)
                {
                    TempData["error"] = "Please enter a fundAmount bigger than {0}.";
                    return RedirectToAction("buy_credit");
                }
                if (companydetails.Country == null)
                {
                    TempData["error"] = "Please update your country details before payment.";
                    return RedirectToAction("buy_credit");
                }
                if (companydetails == null)
                {
                    TempData["error"] = "Please update your company details before payment.";
                    return RedirectToAction("buy_credit");
                }
                var contactdetails = _contactRepository.Get(top => top.UserId == efmvcUser.UserId);
                if (contactdetails == null)
                {
                    TempData["error"] = "Please update your contact details before payment.";
                    return RedirectToAction("buy_credit");
                }
                int errorstatus = 0;
                if (ModelState.IsValid)
                {
                    errorstatus = ValidateBilling(errorstatus);
                    if (errorstatus == 1) return RedirectToAction("buy_credit");
                    int campaignCountryId = Convert.ToInt32(Session["CountryId"].ToString());
                    int currencyId = Convert.ToInt32(Session["currencyId"].ToString());
                    var currencyCountryData = _currencyRepository.Get(c => c.CurrencyId == currencyId);
                    var currencyCountryId = currencyCountryData.Country.Id;
                    var currencyCountryCode = currencyCountryData.CurrencyCode;
                    var currencyCode = _currencyRepository.Get(c => c.CountryId == campaignCountryId).CurrencyCode;
                    decimal currencyRate = 0.00M;
                    string fromCurrencyCode = currencyCountryCode;
                    string toCurrencyCode = currencyCode;
                    int CountryId = 0;
                    if (TempData["CampaingId"] != null)
                    {
                        var countryId = _profileRepository.GetById(Convert.ToInt32(TempData["CampaingId"].ToString())).CountryId;
                        if (countryId == 12 || countryId == 13 || countryId == 14) CountryId = 12;
                        else if (countryId == 11) CountryId = 8;
                        else CountryId = Convert.ToInt32(countryId);
                    }
                    if (currencyCountryId == campaignCountryId)
                    {
                        var fundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                        _model.Fundamount = fundamount;
                        var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage;
                        var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
                        var final_amount = _model.Fundamount + totaltaxamount;
                        _model.TaxPercantage = taxpercantage;
                        _model.TotalAmount = final_amount;
                    }
                    else
                    {
                        currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                        currencyRate = currencyModel.Amount;
                        if (currencyModel.Code == "OK")
                        {
                            double fundamount = Convert.ToDouble(Convert.ToDecimal(TempData["Fundamount"].ToString()) * currencyRate);
                            _model.Fundamount = Convert.ToDecimal(fundamount.ToString());
                            var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage;
                            var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
                            var final_amount = _model.Fundamount + totaltaxamount;
                            _model.TaxPercantage = taxpercantage;
                            _model.TotalAmount = final_amount;
                        }
                    }
                    _model.InvoiceNumber = "A" + RandomString(6) + DateTime.Now.ToString("yy");
                    TempData["ponumber"] = "IT" + DateTime.Now.ToString("ddMMyyyymmss") + efmvcUser.UserId; ;
                    BillingFormModel _billingdetails = new BillingFormModel();
                    _billingdetails.CurrencyCode = currencyCode;
                    InsertBillingInfo(_model, efmvcUser, _billingdetails);
                    TempData["emailaddress"] = _model.PaypalEmail;
                    CreateOrUpdateBillingCommand command = Mapper.Map<BillingFormModel, CreateOrUpdateBillingCommand>(_billingdetails);
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //Update Advert And CampaignAdvert
                        UpdateAdvertAndCampaignAdvertDetails(command.CampaignProfileId, CountryId);
                        var currencyCode1 = _currencyRepository.GetById(Convert.ToInt32(currencyId)).CurrencyCode;
                        TempData["BilllingID"] = result.Id;
                        TempData["UserId"] = efmvcUser.UserId;
                        NVPCallerServices caller = new NVPCallerServices();
                        IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
                        Paypal.NVPCodec encoder = SetExpressCheckout(caller, profile, _model.TotalAmount.ToString(), _model.PaypalEmail, TempData["ponumber"].ToString(), currencyCode1);
                        string pStrrequestforNvp = encoder.Encode();
                        string pStresponsenvp = caller.Call(pStrrequestforNvp);
                        Paypal.NVPCodec decoder = new Paypal.NVPCodec();
                        decoder.Decode(pStresponsenvp);
                        var current = result.Id;
                        if (decoder != null)
                        {
                            if (decoder.Keys.Count > 0)
                            {
                                string status = "", token = "";
                                string errormsg = "";
                                for (int i = 0; i < decoder.Keys.Count; i++)
                                {
                                    if (decoder.Keys[i].ToString() == "ACK")
                                    {
                                        status = decoder.GetValues("ACK")[0].ToString();
                                        if (status.Trim().ToLower() == "failure")
                                        {
                                            errormsg = decoder.GetValues("L_LONGMESSAGE0")[0].ToString();
                                            break;
                                        }
                                    }
                                    else if (decoder.Keys[i].ToString() == "TOKEN")
                                    {
                                        token = decoder.GetValues("TOKEN")[0].ToString();
                                        break;
                                    }
                                }
                                bool flag = false;
                                if (token != "")
                                {
                                    flag = true;
                                    return Redirect("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + token);
                                }
                                if (flag == true)
                                {
                                    //update credit available of campaign
                                    bool campaigncreditstatus = false;
                                    var payment = ReceivePayment(result.Id, _model.TotalAmount, "Card", Convert.ToInt32(TempData["CampaingId"]));
                                    if (payment != 0)
                                    {
                                        if (currencyCountryId == campaignCountryId)
                                        {
                                            campaigncreditstatus = UpdateCampaignCredit(Convert.ToInt32(TempData["CampaingId"]),_model.Fundamount, payment);
                                        }
                                        else
                                        {
                                            currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                                            currencyRate = currencyModel.Amount;
                                            if (currencyModel.Code == "OK")
                                            {
                                                decimal convertedFundAmount = Convert.ToDecimal(payment * currencyRate);
                                                campaigncreditstatus = UpdateCampaignCredit(Convert.ToInt32(TempData["CampaingId"]),_model.Fundamount, convertedFundAmount);
                                            }
                                        }
                                    }
                                    //var currencySymbol1 = currencySymbol.GetCurrencySymbolusingCountryId(campaignCountryId);
                                    var currencySymbol1 = Session["CurrencySymbol"].ToString();
                                    var pdfStatus = CreatePDF(result.Id, efmvcUser.UserId, 1, "InstantPayment", currencySymbol1, fromCurrencyCode, toCurrencyCode);
                                    TempData["success"] = "Payment received successfully for " + _model.InvoiceNumber;
                                    return RedirectToAction("Index");
                                }
                                if (status.ToLower().Trim() == "failure")
                                {
                                    TempData["error"] = errormsg;
                                    DeleteTempBilling(result.Id);
                                    return RedirectToAction("buy_credit");
                                }
                            }
                            else TempData["error"] = "Internal Server error. Please try again.";
                        }
                        else TempData["error"] = "Internal Server error. Please try again.";
                    }
                    else TempData["error"] = "Internal Server error. Please try again.";
                }
                return RedirectToAction("buy_credit");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message.ToString();
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Advertiser")]
        [HttpPost]
        public ActionResult PaywithCoinBase(BillingPaymentInfoDetailsWithPaypal _model)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var companydetails = _companydetailsRepository.Get(top => top.UserId == efmvcUser.UserId);
            if (companydetails == null)
            {
                TempData["error"] = "Please update your company details before payment.";
                return RedirectToAction("buy_credit");
            }
            var contactdetails = _contactRepository.Get(top => top.UserId == efmvcUser.UserId);
            if (contactdetails == null)
            {
                TempData["error"] = "Please update your contact details before payment.";
                return RedirectToAction("buy_credit");
            }
            int errorstatus = 0;
            errorstatus = ValidateBilling(errorstatus);
            if (errorstatus == 1) return RedirectToAction("buy_credit");
            var CreditAvailable = Convert.ToDecimal(TempData["CreditAvailable"].ToString());
            var userfundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
            if (CreditAvailable == 0)
            {
                TempData["error"] = "Credit is not available so please try again.";//Credit is not available.so please try again.
                return RedirectToAction("buy_credit");
            }
            else if (userfundamount > CreditAvailable)
            {
                TempData["error"] = "Fund amount is not greater than the credit available.";
                return RedirectToAction("buy_credit");
            }
            var finalamount = CreditAvailable - userfundamount;
            _model.InvoiceNumber = "A" + RandomString(6) + DateTime.Now.ToString("yy");
            BillingFormModel _billingdetails = new BillingFormModel();
            _billingdetails.PaymentMethodId = 5;
            _billingdetails.Status = 2;
            var fundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
            _model.Fundamount = fundamount;
            var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == companydetails.CountryId).TaxPercantage;
            var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
            var final_amount = _model.Fundamount + totaltaxamount;
            _model.TaxPercantage = taxpercantage;
            _model.TotalAmount = final_amount;
            _billingdetails.SettledDate = DateTime.Now.AddDays(_userRepository.Get(top => top.UserId == efmvcUser.UserId).Outstandingdays);
            InsertBillingInfo(_model, efmvcUser, _billingdetails);
            CreateOrUpdateBillingCommand command = Mapper.Map<BillingFormModel, CreateOrUpdateBillingCommand>(_billingdetails);
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                var sagePayCurrency = ConfigurationManager.AppSettings["sagepaycurrency"].ToString();
                var sagePayCountry = ConfigurationManager.AppSettings["sagepaycountry"].ToString();
                var transaction = "";
                var msg = DoDirectCoinBasePaymentCode(_model, companydetails, contactdetails);
                if (msg != "error") return Redirect(msg);
            }
            return RedirectToAction("buy_credit");
        }
        [Authorize(Roles = "Advertiser")]
        [HttpPost]
        public ActionResult PaywithPaypalCreditCard(BillingPaymentInfoDetails _model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var companydetails = _companydetailsRepository.Get(top => top.UserId == efmvcUser.UserId);
                if (companydetails == null)
                {
                    TempData["error"] = "Please update your company details before payment.";
                    return RedirectToAction("buy_credit");
                }
                var contactdetails = _contactRepository.Get(top => top.UserId == efmvcUser.UserId);
                if (contactdetails == null)
                {
                    TempData["error"] = "Please update your contact details before payment.";
                    return RedirectToAction("buy_credit");
                }
                int errorstatus = 0;
                errorstatus = ValidateBilling(errorstatus);
                if (errorstatus == 0)
                {
                    _model.InvoiceNumber = "A" + RandomString(6) + DateTime.Now.ToString("yy");
                    var fundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                    _model.Fundamount = fundamount;
                    var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == companydetails.CountryId).TaxPercantage;
                    var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
                    var final_amount = _model.Fundamount + totaltaxamount;
                    _model.TaxPercantage = taxpercantage;
                    _model.TotalAmount = final_amount;
                    BillingFormModel _billingdetails = new BillingFormModel();
                    _billingdetails.PaymentMethodId = 2;
                    _billingdetails.Status = 3;
                    InsertBillingInfo(_model, efmvcUser, _billingdetails);
                    CreateOrUpdateBillingCommand command = Mapper.Map<BillingFormModel, CreateOrUpdateBillingCommand>(_billingdetails);
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        var paypalcurrency = ConfigurationManager.AppSettings["paypalcurrency"].ToString();
                        var paypalcountry = ConfigurationManager.AppSettings["paypalcountry"].ToString();
                        Paypal.NVPCodec decoder = new Paypal.NVPCodec();
                        DoDirectPayment directpayment = new DoDirectPayment();
                        Common.CreditCardType cardTy = (Common.CreditCardType)Convert.ToInt32(_model.CardType);
                        string cardType = cardTy.ToString();
                        decoder = directpayment.DoDirectPaymentCode("Sale", _billingdetails.TotalAmount.ToString(), cardType, _model.CardNumber, _model.ExpiryMonth + _model.ExpiryYear, _model.SecurityCode, _model.NameOfCard, _model.NameOfCard, paypalcountry, paypalcurrency);
                        if (decoder != null)
                        {
                            if (decoder.Keys.Count > 0)
                            {
                                string status = "", transaction = "", LONGMESSAGE = "";
                                string errormsg = "";
                                for (int i = 0; i < decoder.Keys.Count; i++)
                                {
                                    if (decoder.Keys[i].ToString() == "ACK")
                                    {
                                        status = decoder.GetValues("ACK")[0].ToString();
                                        if (status.Trim().ToLower() == "success")
                                        {
                                            transaction = decoder.GetValues("TRANSACTIONID")[0].ToString();
                                            break;
                                        }
                                        else
                                        {
                                            LONGMESSAGE = decoder.GetValues("L_LONGMESSAGE0")[0].ToString();
                                            errormsg = decoder.GetValues("L_ERRORCODE0")[0].ToString();
                                            break;
                                        }
                                    }
                                }
                                if (status.Trim().ToLower() == "failure")
                                {
                                    DeleteTempBilling(result.Id);
                                    TempData["error"] = LONGMESSAGE;
                                    return RedirectToAction("buy_credit");
                                }
                                else
                                {
                                    //update campaign credit available of user
                                    bool campaigncreditstatus = UpdateCampaignCredit(Convert.ToInt32(TempData["CampaingId"]), _model.Fundamount, _model.TotalAmount);
                                    if (campaigncreditstatus)
                                    {
                                        bool tranStatus = AddBillingDetails(transaction, _model, result.Id);
                                        if (tranStatus)
                                        {
                                            var currencySymbol = Session["CurrencySymbol"];
                                            CreatePDF(result.Id, efmvcUser.UserId, 3, "Instantpayment", currencySymbol.ToString(), "", "");
                                            TempData["success"] = "Payment received successfully for " + _model.InvoiceNumber;
                                            return RedirectToAction("Index");
                                        }
                                        else TempData["error"] = "Internal Server error. Please try again.";
                                    }
                                    else TempData["error"] = "Internal Server error. Please try again.";
                                    return RedirectToAction("buy_credit");
                                }
                            }
                        }
                    }
                }
            }
            return View();
        }
        [Authorize(Roles = "Advertiser")]
        [HttpPost]
        public ActionResult PaywithSagePayCreditCard(BillingPaymentInfoDetails _model)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                CurrencySymbol currencySymbol = new CurrencySymbol();
                CurrencyModel currencyModel = new CurrencyModel();
                var companydetails = _companydetailsRepository.Get(top => top.UserId == efmvcUser.UserId);
                if (Convert.ToDecimal(TempData["Fundamount"].ToString()) <= 0)
                {
                    TempData["error"] = "Please enter a fundAmount bigger than {0}.";
                    return RedirectToAction("buy_credit");
                }
                if (companydetails.Country == null)
                {
                    TempData["error"] = "Please update your country details before payment.";
                    return RedirectToAction("buy_credit");
                }
                if (companydetails == null)
                {
                    TempData["error"] = "Please update your company details before payment.";
                    return RedirectToAction("buy_credit");
                }
                var contactdetails = _contactRepository.Get(top => top.UserId == efmvcUser.UserId);
                if (contactdetails == null)
                {
                    TempData["error"] = "Please update your contact details before payment.";
                    return RedirectToAction("buy_credit");
                }
                if (contactdetails.PhoneNumber == null)
                {
                    TempData["error"] = "Please update your contact details before payment Phone number is missing.";
                    return RedirectToAction("buy_credit");
                }
                if (_model.CardNumber.Length > 16)
                {
                    TempData["error"] = "Not a Valid Card Number.";
                    return RedirectToAction("buy_credit");
                }
                int errorstatus = 0;
                errorstatus = ValidateBilling(errorstatus);
                if (errorstatus == 1)
                {
                    return RedirectToAction("buy_credit");
                }
                int campaignCountryId = Convert.ToInt32(Session["CountryId"].ToString());
                int currencyId = Convert.ToInt32(Session["currencyId"].ToString());
                var currencyCountryData = _currencyRepository.Get(c => c.CurrencyId == currencyId);
                var currencyCountryId = currencyCountryData.Country.Id;
                var currencyCountryCode = currencyCountryData.CurrencyCode;
                var currencyCode = _currencyRepository.Get(c => c.CountryId == campaignCountryId).CurrencyCode;
                decimal currencyRate = 0.00M;
                string fromCurrencyCode = currencyCountryCode;
                string toCurrencyCode = currencyCode;
                int CountryId = 0;
                if (TempData["CampaingId"] != null)
                {
                    var countryId = _profileRepository.GetById(Convert.ToInt32(TempData["CampaingId"].ToString())).CountryId;
                    if (countryId == 12 || countryId == 13 || countryId == 14) CountryId = 12;
                    else if (countryId == 11) CountryId = 8;
                    else CountryId = Convert.ToInt32(countryId);
                }
                if (currencyCountryId == campaignCountryId)
                {
                    //check credit available
                    var CreditAvailable = Convert.ToDecimal(TempData["CreditAvailable"].ToString());
                    var userfundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                    var finalamount = CreditAvailable - userfundamount;
                    var fundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                    _model.Fundamount = fundamount;
                    var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage;
                    var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
                    var final_amount = _model.Fundamount + totaltaxamount;
                    _model.TaxPercantage = taxpercantage;
                    _model.TotalAmount = final_amount;
                }
                else
                {
                    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                    currencyRate = currencyModel.Amount;
                    if (currencyModel.Code == "OK")
                    {
                        var FundAmount = Convert.ToDouble(Convert.ToDecimal(TempData["Fundamount"].ToString()) * currencyRate);
                        _model.Fundamount = Convert.ToDecimal(FundAmount.ToString());
                        var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage;
                        var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
                        var final_amount = Convert.ToDecimal(FundAmount.ToString()) + totaltaxamount;
                        _model.TaxPercantage = taxpercantage;
                        _model.TotalAmount = final_amount;
                    }
                }

                _model.InvoiceNumber = "A" + RandomString(6) + DateTime.Now.ToString("yy");
                SagepayFormModel _billingdetails = new SagepayFormModel();
                _billingdetails.PaymentMethodId = 2;
                _billingdetails.Status = 2;
                _billingdetails.CurrencyCode = currencyCode;
                _billingdetails.SettledDate = DateTime.Now.AddDays(_userRepository.Get(top => top.UserId == efmvcUser.UserId).Outstandingdays);
                InsertBillingInfoSagePay(_model, efmvcUser, _billingdetails);
                CreateOrUpdateBillingCommand command =
                Mapper.Map<SagepayFormModel, CreateOrUpdateBillingCommand>(_billingdetails);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //Update Advert And CampaignAdvert
                    UpdateAdvertAndCampaignAdvertDetails(command.CampaignProfileId, CountryId);
                    var sagePayCurrency = ConfigurationManager.AppSettings["sagepaycurrency"].ToString();
                    var sagePayCountry = ConfigurationManager.AppSettings["sagepaycountry"].ToString();
                    var msg = DoDirectSagePaymentCode(_model, companydetails, contactdetails, currencyCountryCode, currencyCode, Convert.ToDecimal(TempData["Fundamount"].ToString()));
                    if (msg != "Created")
                    {
                        DeleteTempBilling(result.Id);
                        GenerateTicket objTicket = new GenerateTicket(_commandBus, _userRepository);
                        string message = TempData["error"].ToString();
                        int subjectId = (int)QuestionSubjectStatus.Billing;
                        objTicket.CreateAdTicketForBilling(efmvcUser.UserId, "Card payment error", message, subjectId, Convert.ToInt32(Session["poClientId"]), _billingdetails.CampaignProfileId, _billingdetails.PaymentMethodId);
                        return RedirectToAction("buy_credit");
                    }
                    else
                    {
                        bool campaigncreditstatus = false;
                        var userCreditCurrencyId = _userCreditRepository.Get(top => top.UserId == efmvcUser.UserId).CurrencyId;
                        var userCreditCurrencyCode = _currencyRepository.Get(c => c.CurrencyId == userCreditCurrencyId).CurrencyCode;
                        var fundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                        var selectedCurrencyCode = _currencyRepository.Get(c => c.CurrencyCode == fromCurrencyCode).CurrencyId;
                        var selectedCurrencyCurrency = _currencyRepository.Get(c => c.CurrencyId == selectedCurrencyCode).CurrencyCode;

                        if (userCreditCurrencyId != selectedCurrencyCode)
                        {
                            currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, userCreditCurrencyCode);
                            currencyRate = currencyModel.Amount;
                            fundamount = Convert.ToDecimal(_model.TotalAmount * currencyRate);
                        }
                        var campaignCurrencyCode = _profileRepository.GetById(Convert.ToInt32(TempData["CampaingId"].ToString())).CurrencyCode;
                        var payment = ReceivePayment(result.Id, fundamount, "Card", Convert.ToInt32(TempData["CampaingId"]));
                        if (payment != 0)
                        {
                            //10-12-2019 333
                            ///  if (currencyCountryId == campaignCountryId)
                            if (campaignCurrencyCode == selectedCurrencyCurrency)
                            {
                                campaigncreditstatus = UpdateCampaignCredit(Convert.ToInt32(TempData["CampaingId"]), _model.Fundamount, _model.TotalAmount);
                            }
                            else
                            {
                                currencyModel = _currencyConversion.ForeignCurrencyConversion("1", selectedCurrencyCurrency, campaignCurrencyCode);
                                currencyRate = currencyModel.Amount;
                                if (currencyModel.Code == "OK")
                                {
                                    double convertedFundAmount = Convert.ToDouble(payment * currencyRate);
                                    campaigncreditstatus = UpdateCampaignCredit(Convert.ToInt32(TempData["CampaingId"]),_model.Fundamount, Convert.ToDecimal(convertedFundAmount));
                                }
                            }
                        }
                        //if (campaigncreditstatus)
                        //{
                        bool tranStatus = AddSageBillingDetails(_model, result.Id);
                        //}
                        //var currencySymbol1 = currencySymbol.GetCurrencySymbolusingCountryId(campaignCountryId);
                        var currencySymbol1 = Session["CurrencySymbol"].ToString();
                        var pdfStatus = CreatePDF(result.Id, efmvcUser.UserId, 2, "Instantpayment", currencySymbol1, fromCurrencyCode, toCurrencyCode);
                        TempData["success"] = "Payment received successfully for " + _model.InvoiceNumber;
                        return RedirectToAction("buy_credit");
                    }
                }
                else return RedirectToAction("buy_credit");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message.ToString();
                return RedirectToAction("buy_credit");
            }
        }
        public decimal ReceivePayment(int billingId, decimal amount, string paymentMethod, int CampaignProfileId)
        {

            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            UsersCreditPaymentFormModel _payment = new UsersCreditPaymentFormModel();
            _payment.UserId = efmvcUser.UserId;
            if (sUserId > 0)
            {
                _payment.UserId = sUserId;
            }
            _payment.BillingId = billingId;
            _payment.Amount = amount;
            _payment.Description = paymentMethod;
            _payment.CreatedDate = DateTime.Now;
            _payment.UpdatedDate = DateTime.Now;

            _payment.Status = 1;
            _payment.CampaignProfileId = CampaignProfileId;
            CreateOrUpdateUsersCreditPaymentCommand command = Mapper.Map<UsersCreditPaymentFormModel, CreateOrUpdateUsersCreditPaymentCommand>(_payment);
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                var creditResult = updateusercredit(_payment.UserId, amount);
                var isCreditAmount = _billingRepository.GetMany(s => s.PaymentMethodId == 1 && s.CampaignProfileId == CampaignProfileId).Any();
                if (isCreditAmount == false)
                    return amount;
                return creditResult;
            }
            return 0;
        }
        public decimal updateusercredit(int userid, decimal receivedamount)
        {
            int status = 0;
            decimal extraCredit = 0;
            var usercredit = _userCreditRepository.Get(top => top.UserId == userid);
            if (usercredit != null)
            {
                var AvailableCredit = usercredit.AvailableCredit;
                AvailableCredit = AvailableCredit + receivedamount;
                if (AvailableCredit > usercredit.AssignCredit)
                {
                    extraCredit = AvailableCredit - usercredit.AssignCredit;
                    AvailableCredit = AvailableCredit - extraCredit;
                }
                UpdateUserCreditCommand command = new UpdateUserCreditCommand();
                command.UserId = userid;
                command.CurrencyId = usercredit.CurrencyId;
                command.AvailableCredit = AvailableCredit;
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    if (extraCredit != 0) return extraCredit;
                    status = 0;
                }
            }
            return status;
        }
        public string DoDirectCoinBasePaymentCode(BillingPaymentInfoDetailsWithPaypal model, CompanyDetails companydetails, Contacts contactdetails)
        {
            var deserial = new JsonDeserializer();
            var coinbaseAPIKey = ConfigurationManager.AppSettings["coinbaseAPIKey"].ToString();
            var siteAddress = ConfigurationManager.AppSettings["siteAddress"].ToString();
            var redirect_url = new Uri("https://adtonss.com/Billing/buy_credit");
            IRestResponse checkoutresponse;
            var clientcharge = new RestClient("https://api.commerce.coinbase.com/charges");
            var requestcharge = new RestRequest(Method.POST);
            requestcharge.AddHeader("cache-control", "no-cache");
            requestcharge.AddHeader("content-type", "application/json");
            requestcharge.AddHeader("x-cc-version", "2018-03-22");
            requestcharge.AddHeader("x-cc-api-key", coinbaseAPIKey);
            requestcharge.AddParameter("application/json", "{\r\n\"name\": \"" + contactdetails.User.FirstName + ' ' + contactdetails.User.LastName + "is pay to adtons\",\r\n       \"description\": \"Mastering the Transition to the Information Age\",\r\n       \"local_price\": {\r\n         \"amount\": \"" + model.TotalAmount + "\",\r\n         \"currency\": \"GBP\"\r\n       },\r\n       \"pricing_type\": \"fixed_price\",\r\n       \"metadata\": {\r\n         \"customer_id\": \"" + model.InvoiceNumber + "\",\r\n         \"customer_name\": \"" + contactdetails.User.FirstName + ' ' + contactdetails.User.LastName + "\"\r\n       },\r\n       \"redirect_url\":\"" + redirect_url + "\"\r\n,\r\n       \"cancel_url\":\"" + redirect_url + "\"\r\n}", ParameterType.RequestBody);
            IRestResponse chargeresponse = clientcharge.Execute(requestcharge);
            dynamic jsonResponse = JsonConvert.DeserializeObject(chargeresponse.Content);
            var chargefinalresult = deserial.Deserialize<List<RootObject1>>(chargeresponse);
            if (chargefinalresult[0].data.code != "" && chargefinalresult[0].data.code != null)
            {
                var clientcheckout = new RestClient("https://api.commerce.coinbase.com/checkouts");
                var requestcheckout = new RestRequest(Method.POST);
                requestcheckout.AddHeader("X-CC-Version", "2018-03-22");
                requestcheckout.AddHeader("X-CC-Api-Key", "d7b369cf-0f84-405f-99b8-638cb297b6aa");
                requestcheckout.AddHeader("Content-Type", "application/json");
                requestcheckout.AddParameter("application/json", "{\r\n         \"name\": \"" + contactdetails.User.FirstName + ' ' + contactdetails.User.LastName + "\",\r\n         \"description\": \"Mastering the Transition to the Information Age\",\r\n         \"local_price\": {\r\n             \"amount\": \"" + model.TotalAmount + "\",\r\n             \"currency\": \"GBP\"\r\n         },\r\n       \"pricing_type\": \"fixed_price\",\r\n         \"requested_info\": [\"email\"]\r\n     }", ParameterType.RequestBody);
                checkoutresponse = clientcheckout.Execute(requestcheckout);
                var checkoutfinalresult = deserial.Deserialize<List<RootObjectCheckOut>>(checkoutresponse);
                if (checkoutfinalresult[0].data.id != "" && checkoutfinalresult[0].data.id != null) return chargefinalresult[0].data.hosted_url;
                else return "error";
            }
            else return "error";
        }
        [Authorize(Roles = "Advertiser")]
        public ActionResult CoinbaseConfirm(BillingPaymentInfoDetails _model)
        {
            return View("Index");
        }


        [Authorize(Roles = "Advertiser")]
        [HttpPost]
        public ActionResult PaywithMpesa(BillingPaymentInfoDetailsWithMpesa _model)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            CurrencySymbol currencySymbol = new CurrencySymbol();
            CurrencyModel currencyModel = new CurrencyModel();
            var companydetails = _companydetailsRepository.Get(top => top.UserId == efmvcUser.UserId);
            var advertiserFullName = companydetails.User.FirstName + " " + companydetails.User.LastName;

            if (Convert.ToDecimal(TempData["Fundamount"].ToString()) <= 0)
            {
                TempData["error"] = "Please enter a fundAmount bigger than {0}.";
                return RedirectToAction("buy_credit");
            }
            if (companydetails.Country == null)
            {
                TempData["error"] = "Please update your country details before payment.";
                return RedirectToAction("buy_credit");
            }
            if (companydetails == null)
            {
                TempData["error"] = "Please update your company details before payment.";
                return RedirectToAction("buy_credit");
            }
            var contactdetails = _contactRepository.Get(top => top.UserId == efmvcUser.UserId);
            var advertiserPhone = _model.PhoneNumber;
            if (contactdetails == null)
            {
                TempData["error"] = "Please update your contact details before payment.";
                return RedirectToAction("buy_credit");
            }
            int errorstatus = 0;
            errorstatus = ValidateBilling(errorstatus);
            if (errorstatus == 1) return RedirectToAction("buy_credit");
            int campaignCountryId = Convert.ToInt32(Session["CountryId"].ToString());
            int currencyId = Convert.ToInt32(Session["currencyId"].ToString());
            var currencyCountryData = _currencyRepository.Get(c => c.CurrencyId == currencyId);
            var currencyCountryId = currencyCountryData.Country.Id;
            var currencyCountryCode = currencyCountryData.CurrencyCode;
            var currencyCode = _currencyRepository.Get(c => c.CountryId == campaignCountryId).CurrencyCode;
            decimal currencyRate = 0.00M;
            string fromCurrencyCode = currencyCountryCode;
            string toCurrencyCode = currencyCode;
            var final_amount = 0.0M;
            int CountryId = 0;
            var CreditAvailable = Convert.ToDecimal(TempData["CreditAvailable"].ToString());
            var userfundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
            if (TempData["CampaingId"] != null)
            {
                var countryId = _profileRepository.GetById(Convert.ToInt32(TempData["CampaingId"].ToString())).CountryId;
                if (countryId == 12 || countryId == 13 || countryId == 14) CountryId = 12;
                else if (countryId == 11) CountryId = 8;
                else CountryId = Convert.ToInt32(countryId);
            }
            var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage;

            _model.InvoiceNumber = "A" + RandomString(6) + DateTime.Now.ToString("yy");
            var totalAmount = Convert.ToDecimal(TempData["Fundamount"].ToString()) + ((Convert.ToDecimal(TempData["Fundamount"].ToString())) * (taxpercantage / 100));
            if (currencyCountryId == campaignCountryId)
            {
                //check credit available
                CreditAvailable = Convert.ToDecimal(TempData["CreditAvailable"].ToString());
                userfundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                var finalamount = CreditAvailable - userfundamount;
                var fundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                _model.Fundamount = fundamount;

                var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
                final_amount = _model.Fundamount + totaltaxamount;
                _model.TaxPercantage = taxpercantage;
                _model.TotalAmount = final_amount;
            }
            else
            {

                currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                currencyRate = currencyModel.Amount;
                if (currencyModel.Code == "OK")
                {
                    var FundAmount = Convert.ToDouble(Convert.ToDecimal(TempData["Fundamount"].ToString()) * currencyRate);
                    _model.Fundamount = Convert.ToDecimal(FundAmount.ToString());
                    var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
                    final_amount = Convert.ToDecimal(FundAmount.ToString()) + totaltaxamount;
                    _model.TaxPercantage = taxpercantage;
                    _model.TotalAmount = final_amount;
                }
            }

            // var msg = "200";
            var msg = DoDirectMpesaCode(totalAmount, advertiserFullName, advertiserPhone, currencyCountryCode);
            if (msg != "200")
            {
                TempData["error"] = msg;
                return RedirectToAction("buy_credit");
            }
            else
            {
                sCampaignProfileId = Convert.ToInt32(TempData["CampaingId"].ToString());
                sUserId = efmvcUser.UserId;
                sClientId = Convert.ToInt32(TempData["ClientId"].ToString());
                sPONumber = _model.PONumber;
                sInvoiceNumber = _model.InvoiceNumber;
                sFundAmount = _model.Fundamount;
                sTotalAmount = _model.TotalAmount;
                sTaxPercantage = _model.TaxPercantage;
                scurrencyId = Convert.ToInt32(Session["currencyId"].ToString());
                scurrencyID = Convert.ToInt32(Session["CountryId"].ToString());
                scurrencySymbol = Session["CurrencySymbol"].ToString();
                TempData["success"] = "Please check your phone and enter your Mpesa PIN to approve the transaction.";
                return RedirectToAction("buy_credit");
            }
        }

        [HttpPost]
        public JsonResult MpesaCallBackUrl(MpesaPaymentResponse model)
        {
            try
            {
                int currencyId = scurrencyId;
                var currencyCountryData = _currencyRepository.Get(c => c.CurrencyId == currencyId);
                var currencyCountryId = currencyCountryData.Country.Id;
                var currencyCountryCode = currencyCountryData.CurrencyCode;
                int campaignCountryId = scurrencyID;
                var currencyCode = _currencyRepository.Get(c => c.CountryId == campaignCountryId).CurrencyCode;
                decimal currencyRate = 0.00M;
                string fromCurrencyCode = currencyCountryCode;
                string toCurrencyCode = currencyCode;
                MpesaFormModel _billingdetails = new MpesaFormModel();

                CurrencyModel currencyModel = new CurrencyModel();
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                _billingdetails.CurrencyCode = currencyCode;
                InsertBillingInfoMpesa(efmvcUser, _billingdetails);
                CreateOrUpdateBillingCommand command = Mapper.Map<MpesaFormModel, CreateOrUpdateBillingCommand>(_billingdetails);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {

                    EFMVCDataContex db = new EFMVCDataContex();
                    MpesaHistory mpesaHistory = new MpesaHistory();
                    mpesaHistory.TransactionType = model.transactionType;
                    mpesaHistory.ReceiptNo = model.receiptNo;
                    mpesaHistory.Description = model.Description;
                    mpesaHistory.AccountReference = model.accountReference;
                    mpesaHistory.Amount = model.Amount;
                    mpesaHistory.PhoneNumber = model.phoneNumber;
                    mpesaHistory.BillingId = result.Id;
                    db.MpesaHistory.Add(mpesaHistory);
                    db.SaveChanges();

                    int CountryId = 0;
                    if (sCampaignProfileId > 0)
                    {
                        var countryId = _profileRepository.GetById(sCampaignProfileId).CountryId;
                        if (countryId == 12 || countryId == 13 || countryId == 14) CountryId = 12;
                        else if (countryId == 11) CountryId = 8;
                        else CountryId = Convert.ToInt32(countryId);
                    }

                    var advertInfo = _advertRepository.GetMany(top => top.CampProfileId == command.CampaignProfileId).ToList();
                    var campaignAdvertInfo = _campaignAdvertRepository.GetMany(top => top.CampaignProfileId == command.CampaignProfileId).ToList();
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);
                    var userfundamount = Convert.ToDecimal(CurrencyConversion(sFundAmount, currencyCode));
                    if (advertInfo.Count() > 0)
                    {
                        foreach (var item in advertInfo)
                        {
                            var advert = db.Adverts.Where(top => top.AdvertId == item.AdvertId).FirstOrDefault();
                            advert.Status = (int)AdvertStatus.Live;
                            db.SaveChanges();
                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var itemConnString in ConnString)
                                {
                                    EFMVCDataContex db1 = new EFMVCDataContex(itemConnString);
                                    var advertDetails = db1.Adverts.Where(s => s.AdtoneServerAdvertId == advert.AdvertId).FirstOrDefault();
                                    if (advertDetails != null)
                                    {
                                        advertDetails.UpdatedDateTime = DateTime.Now;
                                        advertDetails.Status = (int)AdvertStatus.Live;
                                        advertDetails.IsAdminApproval = true;
                                        advertDetails.NextStatus = false;
                                        db1.SaveChanges();
                                    }
                                    var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db1, sCampaignProfileId);
                                    var campaignMatchDetails = db1.CampaignMatch.Where(s => s.MSCampaignProfileId == externalServerCampaignProfileId).FirstOrDefault();
                                    if (campaignMatchDetails != null)
                                    {
                                        campaignMatchDetails.UpdatedDateTime = DateTime.Now;
                                        campaignMatchDetails.Status = (int)CampaignStatus.Play;
                                        campaignMatchDetails.NextStatus = false;
                                        db1.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    if (campaignAdvertInfo.Count() > 0)
                    {
                        foreach (var item in campaignAdvertInfo)
                        {
                            var advert = db.Adverts.Where(top => top.AdvertId == item.AdvertId).FirstOrDefault();
                            advert.Status = (int)AdvertStatus.Live;
                            db.SaveChanges();
                            if (ConnString != null && ConnString.Count() > 0)
                            {
                                foreach (var itemConnString in ConnString)
                                {
                                    EFMVCDataContex db2 = new EFMVCDataContex(itemConnString);
                                    var advertDetails = db2.Adverts.Where(s => s.AdtoneServerAdvertId == advert.AdvertId).FirstOrDefault();
                                    if (advertDetails != null)
                                    {
                                        advertDetails.UpdatedDateTime = DateTime.Now;
                                        advertDetails.Status = (int)AdvertStatus.Live;
                                        advertDetails.IsAdminApproval = true;
                                        advertDetails.NextStatus = false;
                                        db2.SaveChanges();
                                    }
                                    var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db2, (int)Convert.ToInt32(sCampaignProfileId));
                                    var campaignMatchDetails = db2.CampaignMatch.Where(s => s.MSCampaignProfileId == externalServerCampaignProfileId).FirstOrDefault();
                                    if (campaignMatchDetails != null)
                                    {
                                        campaignMatchDetails.UpdatedDateTime = DateTime.Now;
                                        campaignMatchDetails.Status = (int)CampaignStatus.Play;
                                        campaignMatchDetails.NextStatus = false;
                                        db2.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                    bool campaigncreditstatus = false;
                    var userCreditCurrencyId = _userCreditRepository.Get(top => top.UserId == sUserId).CurrencyId;
                    var userCreditCurrencyCode = _currencyRepository.Get(c => c.CurrencyId == userCreditCurrencyId).CurrencyCode;
                    var fundamount = sFundAmount;
                    var selectedCurrencyCode = _currencyRepository.Get(c => c.CurrencyCode == fromCurrencyCode).CurrencyId;
                    var selectedCurrencyCurrency = _currencyRepository.Get(c => c.CurrencyId == selectedCurrencyCode).CurrencyCode;

                    if (userCreditCurrencyId != selectedCurrencyCode)
                    {
                        currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, userCreditCurrencyCode);
                        currencyRate = currencyModel.Amount;
                        fundamount = (sFundAmount * currencyRate);
                    }
                    var campaignCurrencyCode = _profileRepository.GetById(sCampaignProfileId).CurrencyCode;
                    var payment = ReceivePayment(result.Id, fundamount, "Card", sCampaignProfileId);
                    if (payment != 0)
                    {
                        if (campaignCurrencyCode == selectedCurrencyCurrency)
                        {
                            campaigncreditstatus = UpdateCampaignCredit(sCampaignProfileId,model.Amount,fundamount);
                        }
                        else
                        {
                            currencyModel = _currencyConversion.ForeignCurrencyConversion("1", selectedCurrencyCurrency, campaignCurrencyCode);
                            currencyRate = currencyModel.Amount;
                            if (currencyModel.Code == "OK")
                            {
                                double convertedFundAmount = Convert.ToDouble(payment * currencyRate);
                                campaigncreditstatus = UpdateCampaignCredit((int)sCampaignProfileId, sFundAmount, Convert.ToDecimal(convertedFundAmount));
                            }
                        }
                    }

                    var currencySymbol1 = scurrencySymbol;
                    var pdfStatus = CreatePDF(result.Id, sUserId, 6, "Instantpayment", currencySymbol1, fromCurrencyCode, toCurrencyCode);

            

                    sCampaignProfileId = 0;
                    sUserId = 0;
                    sClientId = 0;
                    sPONumber = null;
                    sInvoiceNumber = null;
                    sFundAmount = 0.0M;
                    sTotalAmount = 0.0M;
                    sTaxPercantage = 0.0M;
                    scurrencyId = 0;
                    scurrencySymbol = null;
                }

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(ex.Message))
                {
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(ex.InnerException.Message, JsonRequestBehavior.AllowGet);
                }

            }

        }
        private int ValidateBilling(int errorstatus)
        {
            if (TempData["CampaingId"] == null)
            {
                TempData["error"] = "Please select atleast one campaign";
                errorstatus = 1;
                return errorstatus;
            }
            else
            {
                if (TempData["CampaingId"].ToString() == "0")
                {
                    TempData["error"] = "Please select atleast one campaign";
                    errorstatus = 1;
                    return errorstatus;
                }
            }
            if (TempData["Fundamount"].ToString() == "")
            {
                TempData["error"] = "Please enter fund amount.";
                errorstatus = 1;
                return errorstatus;
            }
            else if (TempData["Fundamount"] == null)
            {
                TempData["error"] = "Please enter valid fund amount.";
                errorstatus = 1;
                return errorstatus;
            }
            else
            {
                if (TempData["Fundamount"].ToString() == "0")
                {
                    TempData["error"] = "Please enter valid fund amount.";
                    errorstatus = 1;
                    return errorstatus;
                }
            }
            return errorstatus;
        }
        private bool UpdateUserCredit(int userId, decimal availablecredit)
        {
            var _usercreditDetails = _userCreditRepository.Get(top => top.UserId == userId);
            if (_usercreditDetails != null)
            {
                UsersCreditFormModel _userCreditModel = new UsersCreditFormModel();
                _userCreditModel.Id = _usercreditDetails.Id;
                _userCreditModel.UserId = _usercreditDetails.UserId;
                _userCreditModel.AssignCredit = _usercreditDetails.AssignCredit;
                _userCreditModel.AvailableCredit = availablecredit;
                _userCreditModel.UpdatedDate = DateTime.Now;
                _userCreditModel.CreatedDate = _usercreditDetails.CreatedDate;
                _userCreditModel.CurrencyId = _usercreditDetails.CurrencyId;
                CreateOrUpdateUsersCreditCommand command =
                  Mapper.Map<UsersCreditFormModel, CreateOrUpdateUsersCreditCommand>(_userCreditModel);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    return true;
                }
            }
            return false;
        }
        private void InsertBillingInfo(BillingPaymentInfoDetailsWithPaypal _model, EFMVCUser efmvcUser, BillingFormModel _billingdetails)
        {
            _billingdetails.PaymentMethodId = 3;
            _billingdetails.UserId = efmvcUser.UserId;
            _billingdetails.ClientId = Convert.ToInt32(TempData["ClientId"]);
            _billingdetails.CampaignProfileId = Convert.ToInt32(TempData["CampaingId"]);
            _billingdetails.PONumber = _model.PONumber;
            _billingdetails.InvoiceNumber = _model.InvoiceNumber;
            _billingdetails.FundAmount = _model.Fundamount;
            _billingdetails.TotalAmount = _model.TotalAmount;
            _billingdetails.PaymentDate = DateTime.Now;
            _billingdetails.SettledDate = DateTime.Now;
            _billingdetails.Status = 1;
            _billingdetails.TaxPercantage = _model.TaxPercantage;
        }
        private void InsertBillingInfoSagePay(BillingPaymentInfoDetails _model, EFMVCUser efmvcUser, SagepayFormModel _billingdetails)
        {
            _billingdetails.PaymentMethodId = 2;
            _billingdetails.UserId = efmvcUser.UserId;
            _billingdetails.ClientId = Convert.ToInt32(TempData["ClientId"]);
            _billingdetails.CampaignProfileId = Convert.ToInt32(TempData["CampaingId"]);
            _billingdetails.PONumber = _model.PONumber;
            _billingdetails.InvoiceNumber = _model.InvoiceNumber;
            _billingdetails.FundAmount = _model.Fundamount;
            _billingdetails.TotalAmount = _model.TotalAmount;
            _billingdetails.PaymentDate = DateTime.Now;
            _billingdetails.SettledDate = DateTime.Now;
            _billingdetails.Status = 1;
            _billingdetails.TaxPercantage = _model.TaxPercantage;
        }
        private void InsertBillingInfoMpesa(EFMVCUser efmvcUser, MpesaFormModel _billingdetails)
        {
            _billingdetails.PaymentMethodId = 6;
            _billingdetails.UserId = sUserId;
            _billingdetails.ClientId = sClientId;
            _billingdetails.CampaignProfileId = sCampaignProfileId;
            _billingdetails.PONumber = sPONumber;
            _billingdetails.InvoiceNumber = sInvoiceNumber;
            _billingdetails.FundAmount = sFundAmount;
            _billingdetails.TotalAmount = sTotalAmount;
            _billingdetails.PaymentDate = DateTime.Now;
            _billingdetails.SettledDate = DateTime.Now;
            _billingdetails.Status = 1;
            _billingdetails.TaxPercantage = sTaxPercantage;
        }
        private void InsertBillingInfo(BillingPaymentInfoDetails _model, EFMVCUser efmvcUser, BillingFormModel _billingdetails)
        {
            _billingdetails.UserId = efmvcUser.UserId;
            _billingdetails.ClientId = Convert.ToInt32(TempData["ClientId"]);
            _billingdetails.CampaignProfileId = Convert.ToInt32(TempData["CampaingId"]);
            _billingdetails.PONumber = _model.PONumber;
            _billingdetails.InvoiceNumber = _model.InvoiceNumber;
            _billingdetails.FundAmount = _model.Fundamount;
            _billingdetails.TotalAmount = _model.TotalAmount;
            _billingdetails.PaymentDate = DateTime.Now;
            _billingdetails.SettledDate = DateTime.Now;
            _billingdetails.TaxPercantage = _model.TaxPercantage;
        }
        public bool DeleteTempBilling(int id)
        {
            int campaignId = Convert.ToInt32(TempData["CampaingId"].ToString());
            int clientId = Convert.ToInt32(TempData["ClientId"].ToString());
            var command = new DeleteBillingCommand { Id = id, CampaignId = campaignId, ClientId = clientId };
            if (_commandBus != null)
            {
                _commandBus.Submit(command);
                return true;
            }
            return false;
        }
        [Authorize(Roles = "Advertiser")]
        public ActionResult SetCampaign(int? CampaingId, string Fundamount, string DropdownChange)
        {
            TempData["Fundamount"] = Fundamount;
            TempData["CampaingId"] = CampaingId;
            if (DropdownChange == "True")
            {
                string campaignFundAvailable = GetCampaignFundAvailable(CampaingId);
                return Json(new { data = "success", value = campaignFundAvailable });
            }
            else
            {
                return Json("success");
            }
        }
        public string GetCampaignFundAvailable(int? campaingId)
        {
            string campaignFundAvailable = "0";
            SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString);
            SqlCommand cmd = new SqlCommand("GetCampaignFundAvailableByCampaignId", myConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            myConnection.Open();
            cmd.Parameters.AddWithValue("@campaignId", campaingId);
            using (IDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read()) campaignFundAvailable = reader["CampaignFundAvailable"].ToString();
            }
            return campaignFundAvailable;
        }
        [Authorize(Roles = "Advertiser")]
        [HttpPost]
        public JsonResult GetCampaign(int ClientId)
        {
            TempData["ClientId"] = ClientId;
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            if (ClientId == 0)
            {
                var campaigndetails = _profileRepository.GetAll().Where(top => top.UserId == efmvcUser.UserId && (top.Status != 5 && top.Status != 6 && top.Status != 7)).Select(top => new SelectListItem { Text = top.CampaignName, Value = top.CampaignProfileId.ToString() });
                var campaigns = campaigndetails.ToList();
                campaigns.Insert(0, new SelectListItem { Text = "--Select Campaign--", Value = "0" });
                return Json(campaigns);
            }
            else
            {
                var campaigndetails = _profileRepository.GetAll().Where(top => top.ClientId == ClientId && top.UserId == efmvcUser.UserId && (top.Status != 5 && top.Status != 6 && top.Status != 7)).Select(top => new SelectListItem { Text = top.CampaignName, Value = top.CampaignProfileId.ToString() });
                var campaigns = campaigndetails.ToList();
                campaigns.Insert(0, new SelectListItem { Text = "--Select Campaign--", Value = "0" });
                return Json(campaigns);
            }
        }
        [Authorize(Roles = "Advertiser")]
        public JsonResult GetCreditcardMonth(string year)
        {
            int currentyear = DateTime.Now.Year;
            int selectedYear = Convert.ToInt32(year);
            if (selectedYear == currentyear)
            {
                int[] selectedmonth = Enumerable.Range(3, 10).ToArray();
                var month = selectedmonth.Select(top => new SelectListItem { Text = top.ToString().Length == 1 ? "0" + top.ToString() : top.ToString(), Value = top.ToString().Length == 1 ? "0" + top.ToString() : top.ToString() });
                var months = month.ToList();
                months.Insert(0, new SelectListItem { Text = "Select Month", Value = "" });
                return Json(months);
            }
            else
            {
                int[] selectedmonth = Enumerable.Range(1, 12).ToArray();
                var month = selectedmonth.Select(top => new SelectListItem { Text = top.ToString().Length == 1 ? "0" + top.ToString() : top.ToString(), Value = top.ToString().Length == 1 ? "0" + top.ToString() : top.ToString() });
                var months = month.ToList();
                months.Insert(0, new SelectListItem { Text = "Select Month", Value = "" });
                return Json(months);
            }
        }
        private void FillClient(IEnumerable<ClientModel> clientModels)
        {
            var clientdetails = clientModels.Select(top => new { Name = top.Name, Id = top.Id.ToString() }).ToList();
            ViewBag.client = new MultiSelectList(clientdetails, "Id", "Name");
        }
        public void FillStatus()
        {
            IEnumerable<Common.BillingStatus> billingTypes = Enum.GetValues(typeof(Common.BillingStatus)).Cast<Common.BillingStatus>();
            var billingstatus = (from action in billingTypes select new SelectListItem { Text = action.ToString(), Value = ((int)action).ToString() }).ToList();
            ViewBag.billingStatus = new MultiSelectList(billingstatus, "Value", "Text");
        }
        private List<BillingResult> GetBillingResult()
        {
            List<BillingResult> _result = new List<BillingResult>();
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            CurrencySymbol currencySymbol = new CurrencySymbol();
            CurrencyModel currencyModel = new CurrencyModel();
            var _clientdetails = _clientRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            IEnumerable<ClientModel> clientModels = Mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(_clientdetails);
            IEnumerable<Billing> billing = _billingRepository.GetMany(x => x.UserId == efmvcUser.UserId).OrderByDescending(top => top.Id).ToList();
            IEnumerable<BillingFormModel> billingFormModels = Mapper.Map<IEnumerable<Billing>, IEnumerable<BillingFormModel>>(billing);
            FillClient(clientModels);
            FillStatus();
            FillPaymentDropdown();
            string status = string.Empty;
            var userCountryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
            string currencysymbol = "";
            string currencyCode = "";
            var userCurrencyId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
            if (userCurrencyId != null || userCurrencyId != 0)
            {
                currencysymbol = currencySymbol.GetCurrencySymbolusingCurrencyId(userCurrencyId, _currencyRepository);
                currencyCode = _currencyRepository.GetById(userCurrencyId.Value).CurrencyCode;
            }
            else
            {
                currencysymbol = currencySymbol.GetCurrencySymbolusingCountryId(userCountryId, _currencyRepository);
                currencyCode = _currencyRepository.Get(top => top.CountryId == userCountryId.Value).CurrencyCode;
            }
            if (billingFormModels.Count() > 0)
            {
                foreach (var item in billingFormModels)
                {
                    double invoiceTotal = 0.00;
                    decimal currencyRate = 0.00M;
                    string fromCurrencyCode = item.CurrencyCode;
                    string toCurrencyCode = currencyCode;
                    if (toCurrencyCode == fromCurrencyCode)
                    {
                        invoiceTotal = Convert.ToDouble(item.FundAmount.ToString());
                    }
                    else
                    {
                        currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                        currencyRate = currencyModel.Amount;
                        if (currencyModel.Code == "OK")
                        {
                            invoiceTotal = Convert.ToDouble(item.FundAmount * currencyRate);
                        }
                    }
                    BillingStatus billingStatus = (BillingStatus)item.Status;
                    status = billingStatus.ToString();
                    _result.Add(new BillingResult { ID = item.Id, InvoiceNO = item.InvoiceNumber, PONumber = item.PONumber, ClienId = item.ClientId, ClientName = item.ClientId == 0 ? "-" : item.Client.Name, CampaignId = item.CampaignProfileId, CampaignName = item.CampaignProfile.CampaignName, InvoiceDate = Convert.ToDateTime(item.PaymentDate), InvoiceTotal = Convert.ToDecimal(invoiceTotal.ToString("F2")), status = status, SettledDate = item.SettledDate, MethodOfPayment = item.PaymentMethod.Description, PaymentMethodId = item.PaymentMethodId, fstatus = item.Status, CurrencySymbol = currencysymbol, CurrencyCode = currencyCode });
                }
            }
            ViewBag.userCountryId = userCountryId;
            ViewBag.userCurrencyId = userCurrencyId;
            return _result;
        }
        [Authorize(Roles = "Advertiser")]
        public ActionResult SearchBilling([Bind(Prefix = "Item2")]BillingFilter _filterCritearea, int[] BillingClientId, int[] BillingstatusId, int[] BillingmethodId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<BillingResult> _result = new List<BillingResult>();
                var finalresult = new List<BillingResult>();
                if (_filterCritearea != null)
                {
                    _result = GetBillingResult();
                    finalresult = getbillingfilterResult(_result, _filterCritearea, BillingClientId, BillingstatusId, BillingmethodId);
                }
                else
                {
                    _result = GetBillingResult();
                    finalresult = getbillingfilterResult(_result, _filterCritearea, BillingClientId, BillingstatusId, BillingmethodId);
                }
                TempData["result"] = finalresult;
                return PartialView("_BillingList", finalresult);
            }
            else
            {
                return PartialView("_BillingList", "notauthorise");
            }
        }
        public List<BillingResult> getbillingfilterResult(List<BillingResult> billingresult, BillingFilter _filterCritearea, int[] BillingClientId, int[] BillingstatusId, int[] BillingmethodId)
        {
            if (billingresult != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.PONumber))
                {
                    billingresult = billingresult.Where(top => !string.IsNullOrEmpty(top.PONumber) && top.PONumber.Trim().ToLower() == _filterCritearea.PONumber.Trim().ToLower()).ToList();
                }
                if (!String.IsNullOrEmpty(_filterCritearea.Id))
                {
                    int id = Convert.ToInt32(_filterCritearea.Id);
                    billingresult = billingresult.Where(top => top.ID == id).ToList();
                }
                if (!String.IsNullOrEmpty(_filterCritearea.InvoiceNO))
                {
                    billingresult = billingresult.Where(top => top.InvoiceNO.Trim().ToLower() == _filterCritearea.InvoiceNO.Trim().ToLower()).ToList();
                }
                if (BillingClientId != null)
                {
                    billingresult = billingresult.Where(top => BillingClientId.Contains(top.ClienId)).ToList();
                }
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    billingresult = billingresult.Where(top => top.InvoiceDate.Date >= Fromdate.Date && top.InvoiceDate.Date <= Todate.Date).ToList();
                }
                if ((_filterCritearea.SettedFromdate != null && _filterCritearea.SettedTodate != null))
                {
                    string strTodate = _filterCritearea.SettedTodate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.SettedFromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    billingresult = billingresult.Where(top => top.SettledDate.Date >= Fromdate && top.SettledDate.Date <= Todate).ToList();
                }
                if (BillingstatusId != null)
                {
                    billingresult = billingresult.Where(top => BillingstatusId.Contains(top.fstatus)).ToList();
                }
                if (!String.IsNullOrEmpty(_filterCritearea.InvoiceFromTotal) && !String.IsNullOrEmpty(_filterCritearea.InvoiceToTotal))
                {
                    billingresult = billingresult.Where(top => top.InvoiceTotal >= Convert.ToDecimal(_filterCritearea.InvoiceFromTotal) && top.InvoiceTotal <= Convert.ToDecimal(_filterCritearea.InvoiceToTotal)).ToList();
                }
                if (BillingmethodId != null)
                {
                    billingresult = billingresult.Where(top => BillingmethodId.Contains(top.PaymentMethodId)).ToList();
                }
            }
            return billingresult;
        }
        public bool CreatePDF(int billingId, int userId, int type, string paymentMethod, string currencySymbol1, string fromCurrencyCode, string toCurrencyCode)
        {
            try
            {
                CurrencySymbol currencySymbol = new CurrencySymbol();
                string invoiceno = string.Empty;
                string customername = string.Empty;
                string companyname = string.Empty;
                string address = string.Empty;
                string addaddress = string.Empty;
                string town = string.Empty;
                string postcode = string.Empty;
                string country = string.Empty;
                string finaladdress = string.Empty;
                string itemdetails = string.Empty;
                string methodofPayment = string.Empty;
                int? typeofPayment;
                string country_Tax = string.Empty;
                var billingdetails = _billingRepository.Get(top => top.Id == billingId);
                invoiceno = billingdetails.InvoiceNumber;
                itemdetails = billingdetails.CampaignProfile.CampaignName;
                methodofPayment = _paymentMethodRepository.Get(top => top.Id == billingdetails.PaymentMethodId).Description;
                typeofPayment = billingdetails.PaymentMethodId;
                var userdetails = _userRepository.Get(top => top.UserId == userId);
                customername = userdetails.FirstName + " " + userdetails.LastName;
                var clientcompany = _companydetailsRepository.Get(top => top.UserId == userId);
                companyname = clientcompany.CompanyName;
                address = clientcompany.Address;
                addaddress = clientcompany.AdditionalAddress;
                town = clientcompany.Town;
                postcode = clientcompany.PostCode;
                country = clientcompany.Country.Name;
                var campaignCreditDetails = _campaignCreditPeriodRepository.Get(top => top.UserId == userId && top.CampaignProfileId == billingdetails.CampaignProfile.CampaignProfileId);
                int campaignId = billingdetails.CampaignProfileId.Value;
                // 03-12-2019  
                //var countryId = _profileRepository.GetById(campaignId).CountryId;
                // 19-12-2019 333
                int? countryId = 0;
                if (sCampaignProfileId > 0)
                {
                    countryId = _profileRepository.GetById(sCampaignProfileId).CountryId;
                }
                else
                {
                    countryId = _profileRepository.GetById(Convert.ToInt32(TempData["CampaingId"].ToString())).CountryId;
                }
                //var countryId = _currencyRepository.GetById(Convert.ToInt32(Session["currencyId"].ToString())).CountryId;
                int CountryId = 0;
                if (countryId == 12 || countryId == 13 || countryId == 14)
                    CountryId = 12;
                else if (countryId == 11)
                    CountryId = 8;
                else
                    CountryId = Convert.ToInt32(countryId);

                var customercontactinfo = _contactRepository.Get(top => top.UserId == userId);
                var currencyCode = "";
                currencyCode = currencySymbol1;
                EFMVC.Web.EFWebPDF.Item item1 = new EFMVC.Web.EFWebPDF.Item();
                item1.Description = itemdetails;
                item1.Price = billingdetails.FundAmount;
                item1.Quantity = 1;
                item1.Organisation = clientcompany.CompanyName;
                Customer customer = new Customer();
                customer.FullName = customername;
                customer.AddressLine1 = address;
                customer.AddressLine2 = addaddress;
                customer.City = town;
                customer.Country = country;
                customer.Postcode = postcode;
                customer.PhoneNumber = customercontactinfo.PhoneNumber;
                customer.Email = customercontactinfo.Email;
                Invoice invoice = new Invoice();
                invoice = new Invoice(item1);
                invoice.InvoiceNumber = billingdetails.InvoiceNumber;
                invoice.vat = (_countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage) / 100;
                invoice.InvoiceTax = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage.ToString();
                invoice.InvoiceCountry = _countryRepository.Get(top => top.Id == CountryId).ShortName;
                invoice.MethodOfPayment = methodofPayment;
                invoice.typeOfPayment = typeofPayment;
                invoice.PONumber = billingdetails.PONumber;
                if (paymentMethod == "Instantpayment") invoice.SettledDate = null;
                else
                {
                    if (paymentMethod == "CreditPayment")
                    {
                        if (campaignCreditDetails == null) invoice.SettledDate = billingdetails.SettledDate.AddDays(7);
                        else invoice.SettledDate = billingdetails.SettledDate.AddDays(campaignCreditDetails.CreditPeriod);
                    }
                    else invoice.SettledDate = billingdetails.SettledDate.AddDays(45);
                }
                invoice.Imagepath = Server.MapPath("~/Images/5acf06fc.png");
                invoice.Customer = customer;
                invoice.Items = 1;
                invoice.CountryId = countryId;
                invoice.CurrencySymbol = currencyCode;

                GeneratePDF pdf = new GeneratePDF(_currencyConversion);
                pdf.Invoice = invoice;
                string path = pdf.CreatePDF(Server.MapPath("~/Invoice"), fromCurrencyCode, toCurrencyCode);
                string[] mailto = new string[1];
                mailto[0] = userdetails.Email;
                string[] attachment = new string[1];
                attachment[0] = path;
                // sendemailtoclient(userdetails.Email, userdetails.FirstName, userdetails.LastName, "Adtones Invoice (" + invoice.InvoiceNumber + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Day + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Month + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Year + ")", 2, mailto, null, null, attachment, true, DateTime.Now.ToString(), paymentMethod, invoice.SettledDate, invoice.InvoiceNumber);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void sendemailtoclient(string to, string fname, string lname, string subject, int formatId, string[] mailTo, string[] mailCC, string[] mailBcc, string[] attachment, bool isBodyHTML, string completedDatetime, string paymentMethod, DateTime? dueDate, string InvoiceNumber)
        {
            var EmailContent = new SendEmailModel();
            if (mailTo != null) EmailContent.To = mailTo;
            if (mailCC != null) EmailContent.CC = mailCC;
            if (mailBcc != null) EmailContent.Bcc = mailBcc;
            if (attachment != null) EmailContent.attachment = attachment;
            EmailContent.Link = ConfigurationManager.AppSettings["siteAddress"].ToString() + "Admin/UserManagement/Index";
            EmailContent.isBodyHTML = isBodyHTML;
            EmailContent.Fname = fname;
            EmailContent.Lname = lname;
            EmailContent.Subject = subject;
            EmailContent.FormatId = formatId;
            EmailContent.InvoiceNumber = InvoiceNumber;
            EmailContent.CompletedDatetime = completedDatetime;
            EmailContent.PaymentMethod = paymentMethod;
            if (paymentMethod == "Instantpayment") EmailContent.DueDate = null;
            else
            {
                EmailContent.DueDate = dueDate;
                EmailContent.PaymentLink = ConfigurationManager.AppSettings["siteAddress"].ToString() + "Billing/buy_credit";
            }
            sendEmailMailer.SendEmail(EmailContent);
        }

        [Authorize(Roles = "Advertiser")]
        [HttpPost]
        public JsonResult CalculateAmount(string amount, int campaingId, string currencyId)
        {
            if (!String.IsNullOrEmpty(amount) && amount != "0")
            {
                try
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    var AdvertCountryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
                    CurrencySymbol currencySymbol = new CurrencySymbol();
                    // 19-12-2019 333
                    var countryId = _profileRepository.GetMany(a => a.CampaignProfileId == campaingId).FirstOrDefault().CountryId;
                    // var countryId = _currencyRepository.GetById(Convert.ToInt32(Session["currencyId"].ToString())).CountryId;
                    int CountryId = 0;
                    if (countryId == 12 || countryId == 13 || countryId == 14)
                        CountryId = 12;
                    else if (countryId == 11)
                        CountryId = 8;
                    else
                        CountryId = Convert.ToInt32(countryId);
                    var famount = Convert.ToDecimal(amount);
                    var companydetails = _companydetailsRepository.Get(top => top.UserId == efmvcUser.UserId);
                    if (companydetails == null) return Json("-1");
                    else
                    {
                        var currencyCode = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyCode;
                        var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage;
                        var totaltaxamount = (famount) * (taxpercantage / 100);
                        var finalamount = famount;
                        string finalamount1 = finalamount.ToString();
                        var loginAdvertCountryId = string.IsNullOrEmpty(AdvertCountryId.ToString()) ? 0 : Convert.ToInt32(AdvertCountryId);
                        finalamount = famount + totaltaxamount;
                        finalamount1 = finalamount.ToString();
                        var currencySymbol1 = _currencyRepository.GetById(Convert.ToInt32(currencyId)).CurrencyCode;
                        return Json(new { success = "success", value = finalamount1, value1 = "(" + currencySymbol1 + ")" });
                    }
                }
                catch (Exception ex)
                {
                    return Json("-2");
                }
            }
            else
            {
                return Json("-3");
            }
        }
        [Authorize(Roles = "Advertiser")]
        [HttpPost]
        public JsonResult CalculateModelAmount(decimal amount, int campaingId)
        {
            List<ModelFundamount> _modelFundamount = new List<ModelFundamount>();
            if (amount != 0)
            {
                try
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    // 19-12-2019 333
                    var countryId = _profileRepository.GetMany(a => a.CampaignProfileId == campaingId).FirstOrDefault().CountryId;
                    //var countryId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CountryId;
                    var advertCountryId = _currencyRepository.GetById(Convert.ToInt32(Session["currencyId"].ToString())).CountryId;
                    int CountryId = 0;
                    if (countryId == 12 || countryId == 13 || countryId == 14) CountryId = 12;
                    else if (countryId == 11) CountryId = 8;
                    else CountryId = Convert.ToInt32(countryId);
                    var companydetails = _companydetailsRepository.Get(top => top.UserId == efmvcUser.UserId);
                    if (companydetails == null) return Json("-1");
                    else
                    {
                        decimal taxpercantage = 0;
                        decimal totaltaxamount = 0;
                        decimal finalamount = amount;

                        var country = _countryRepository.Get(top => top.Id == countryId).ShortName;
                        taxpercantage = _countryTaxRepository.Get(top => top.CountryId == countryId).TaxPercantage;
                        //  var country = _countryRepository.Get(top => top.Id == advertCountryId).ShortName;
                        // taxpercantage = _countryTaxRepository.Get(top => top.CountryId == advertCountryId).TaxPercantage;

                        totaltaxamount = (amount) * (taxpercantage / 100);
                        finalamount = amount + totaltaxamount;
                        var taxamount = totaltaxamount + "(" + taxpercantage + "% of " + country + ")";
                        _modelFundamount.Add(new ModelFundamount { FundAmount = amount, TaxAmount = taxamount, TotaAmount = finalamount.ToString("F") });
                        return Json(_modelFundamount);
                    }
                }
                catch (Exception)
                {
                    return Json("-2");
                }
            }
            else
            {
                return Json("-2");
            }
        }
        public class ModelFundamount
        {
            public decimal FundAmount { get; set; }
            public string TaxAmount { get; set; }
            public string TotaAmount { get; set; }
        }
        public string DoDirectSagePaymentCode(BillingPaymentInfoDetails model, CompanyDetails companydetails, Contacts contactdetails, string currencyCountryCode, string currencyCode, decimal fundamount)
        {
            CurrencyModel currencyModel = new CurrencyModel();
            var authorization = ConfigurationManager.AppSettings["authorization"].ToString();
            var vendorName = ConfigurationManager.AppSettings["vendorName"].ToString();
            var sagePayCurrency = ConfigurationManager.AppSettings["sagepaycurrency"].ToString();
            var sagepaycountry = ConfigurationManager.AppSettings["sagepaycountry"].ToString();
            var transactionType = ConfigurationManager.AppSettings["transactionType"].ToString();
            var merchantSessionKeys = ConfigurationManager.AppSettings["MerchantSessionKeys"].ToString();
            var cardIdentifiers = ConfigurationManager.AppSettings["CardIdentifiers"].ToString();
            var transactions = ConfigurationManager.AppSettings["Transactions"].ToString();
            decimal finalAmount = 0.00M;
            decimal currencyRate = 0.00M;
            //string fromCurrencyCode = currencyCode;
            string toCurrencyCode = sagePayCurrency;
            string fromCurrencyCode = currencyCountryCode;
            //string toCurrencyCode = currencyCode;
            if (toCurrencyCode == fromCurrencyCode)
            {
                finalAmount = model.TotalFundAmount * 100;
            }
            else
            {
                currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                currencyRate = currencyModel.Amount;
                if (currencyModel.Code == "OK")
                {
                    finalAmount = Math.Round((model.TotalFundAmount * currencyRate), 2);
                    if (Convert.ToDecimal(0.01) >= finalAmount)
                    {
                        TempData["error"] = "Please enter amount that is greater than 0.01 pence.";
                        return "error";
                    }
                    finalAmount = finalAmount * 100;
                }
            }
            var deserial = new JsonDeserializer();
            IRestResponse response1;
            IRestResponse response2;
            var client = new RestClient(merchantSessionKeys);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", authorization);
            request.AddParameter("application/json", "{\r\n\t\"vendorName\":\"" + vendorName + "\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
            var OfferJ = deserial.Deserialize<List<Card>>(response);
            if (response.StatusCode.ToString() == "Created")
            {
                client = new RestClient(cardIdentifiers);
                request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Bearer " + OfferJ[0].MerchantSessionKey.ToString());
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", "{\r\n  \"cardDetails\": {\r\n    \"cardholderName\": \"" + model.FirstName + ' ' + model.LastName + "\",\r\n    \"cardNumber\": \"" + model.CardNumber + "\",\r\n    \"expiryDate\": \"" + model.ExpiryMonth + model.ExpiryYear.Substring(2, 2) + "\",\r\n    \"securityCode\": \"" + model.SecurityCode + "\"\r\n  }\r\n}", ParameterType.RequestBody);
                response1 = client.Execute(request);
                dynamic jsonResponse1 = JsonConvert.DeserializeObject(response1.Content);
                var OfferJ1 = deserial.Deserialize<List<CardIdentifiers>>(response1);
                if (response1.StatusCode.ToString() == "Created")
                {
                    Random randNo = new Random();
                    long randnum2 = (long)(randNo.NextDouble() * 9000000000) + 1000000000;
                    client = new RestClient(transactions);
                    request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("authorization", authorization);
                    request.AddHeader("content-type", "application/json");
                    var billingAddress = model.BillingAddress.Contains("\r\n") == true ? model.BillingAddress.Replace("\r\n", " ") : model.BillingAddress;
                    request.AddParameter("application/json", "{\r\n    \"paymentMethod\": {\r\n     \"card\": {\r\n      \"merchantSessionKey\":\"" + OfferJ[0].MerchantSessionKey.ToString() + "\",\r\n      \"cardIdentifier\":\"" + OfferJ1[0].CardIdentifier.ToString() + "\"\r\n     }\r\n    },\r\n    \"transactionType\":\"" + transactionType + "\",\r\n    \"vendorTxCode\":\"adtoneslimited-" + randnum2.ToString() + "\",\r\n    \"amount\":" + Convert.ToInt32(finalAmount) + ",\r\n    \"currency\":\"" + sagePayCurrency + "\",\r\n    \"customerFirstName\":\"" + model.FirstName + "\",\r\n    \"customerLastName\":\"" + model.LastName + "\",\r\n    \"billingAddress\":{\r\n        \"address1\":\"" + billingAddress + "\",\r\n        \"city\":\"" + model.BillingTown + "\",\r\n        \"postalCode\":\"" + model.BillingPostcode + "\",\r\n        \"country\":\"" + sagepaycountry + "\"\r\n    },\r\n    \"entryMethod\":\"Ecommerce\",\r\n    \"apply3DSecure\":\"Disable\",\r\n    \"applyAvsCvcCheck\":\"Disable\",\r\n    \"description\":\"Testing\",\r\n    \"customerEmail\":\"" + contactdetails.Email + "\",\r\n    \"customerPhone\":\"" + contactdetails.PhoneNumber + "\",\r\n    \"shippingDetails\":{\r\n        \"recipientFirstName\":\"" + model.FirstName + "\",\r\n        \"recipientLastName\":\"" + model.LastName + "\",\r\n        \"shippingAddress1\":\"" + billingAddress + "\",\r\n        \"shippingCity\":\"" + model.BillingTown + "\",\r\n        \"shippingPostalCode\":\"" + model.BillingPostcode + "\",\r\n        \"shippingCountry\":\"GB\"\r\n    }\r\n}\r\n", ParameterType.RequestBody);
                    response2 = client.Execute(request);
                    var finalstatus = deserial.Deserialize<List<Output>>(response2);
                    if (response2.StatusCode.ToString() == "Created")
                    {
                        TempData["TranID"] = finalstatus[0].TransactionId;
                        TempData["success"] = "Payment received successfully for " + model.InvoiceNumber;
                        return response.StatusCode.ToString();
                    }
                    else
                    {
                        TempData["error"] = finalstatus[0].StatusDetail;
                        return "error";
                    }
                }
                else
                {
                    TempData["error"] = jsonResponse1.errors[0].clientMessage;
                    return "error";
                }
            }
            else
            {
                TempData["error"] = jsonResponse.errors[0].clientMessage;
                return jsonResponse.code.ToString();
            }
        }
        public string DoDirectMpesaCode(decimal TotalAmount, string Name, string advertiserPhone, string currencyCountryCode)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            CurrencyModel currencyModel = new CurrencyModel();
            var deserial = new JsonDeserializer();
            decimal finalAmount = TotalAmount;
            decimal currencyRate = 0.00M;
            //string fromCurrencyCode = currencyCode;
            string toCurrencyCode = "KES";
            string fromCurrencyCode = currencyCountryCode;


            //string toCurrencyCode = currencyCode;
            if (toCurrencyCode != fromCurrencyCode)
            {
                currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                currencyRate = currencyModel.Amount;
                if (currencyModel.Code == "OK")
                {
                    finalAmount = Math.Round((TotalAmount * currencyRate), 2);
                    if (1 >= finalAmount)
                    {
                        TempData["error"] = "Please enter amount that is greater than 0.01 pence.";
                        return "error";
                    }
                }
            }

            var finalPaymentAmt = Round((double)finalAmount);
            var mpesaapiurl = ConfigurationManager.AppSettings["MpesaAPIUrl"].ToString();
            var client = new RestClient(mpesaapiurl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\n\"businessshortcode\": \"288520\",\n  \"msisdn\": \"" + advertiserPhone + "\",\n  \"amount\": \"" + finalAmount + "\",\n  \"accountreference\": \"test\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
                return response.ErrorException.InnerException.Message.ToString();
            else
            {
                dynamic jsonresponse = JsonConvert.DeserializeObject(response.Content);
                var offerj = deserial.Deserialize<List<MPesaResponse>>(response);
                if (offerj[0].Status == "200")
                {
                    return offerj[0].Status;
                }
                else
                {
                    TempData["error"] = offerj[0].Description;
                    return offerj[0].Status;
                }
            }
        }

        #region Currency Code For Initalize
        private void FillCurrencyList()
        {
            var currency = (from action in _currencyRepository.GetAll().OrderBy(c => c.CurrencyCode).Skip(1) select new SelectListItem { Text = action.CurrencyCode, Value = action.CurrencyId.ToString() }).ToList();
            ViewBag.currencyList = currency;
        }
        [Authorize(Roles = "Advertiser")]
        public ActionResult GetCurrencyCode(int id, int campaignid, string label, int poClientId)
        {
            CurrencySymbol currencySymbol = new CurrencySymbol();
            CurrencyModel currencyModel = new CurrencyModel();
            var currencyCode = "";
            double Maximumamountofcredit = 0.00;
            double CreditAvailable = 0;
            double CampaignFundAvailable = 0.00;
            decimal currencyRate = 0.00M;
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var userCreditdetails = _userCreditRepository.Get(top => top.UserId == efmvcUser.UserId);
            if (label == "campaign")
            {
                var campaignDetails = _profileRepository.GetById(id);
                int CountryId = 0;
                CountryId = campaignDetails.CountryId.Value;
                if (CountryId == 12 || CountryId == 13 || CountryId == 14) CountryId = 12;
                else if (CountryId == 11) CountryId = 8;
                else CountryId = Convert.ToInt32(CountryId);
                var campaignCurrencyId = _currencyRepository.Get(c => c.CountryId == CountryId).CurrencyId;
                var currencyId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
                //03-12-2019 
                //  var currencySymbol1 = currencySymbol.GetCurrencySymbolusingCurrencyId(currencyId);
                var currencySymbol1 = currencySymbol.GetCurrencySymbolusingCurrencyId(campaignCurrencyId, _currencyRepository);
                //currencyCode = _currencyRepository.Get(c => c.CurrencyId == currencyId).CurrencyCode;
                currencyCode = _currencyRepository.Get(c => c.CurrencyId == campaignCurrencyId).CurrencyCode;
                if (campaignDetails != null)
                {
                    string fromCurrencyCode = campaignDetails.CurrencyCode;
                    string toCurrencyCode = currencyCode;
                    if (campaignDetails.CurrencyCode == currencyCode)
                    {
                        CampaignFundAvailable = GetCampaignAvailableFund(campaignDetails.CampaignProfileId, "True");
                        CampaignFundAvailable = Convert.ToDouble(CampaignFundAvailable.ToString("F2"));
                        //CampaignFundAvailable = Convert.ToDouble(campaignDetails.TotalBudget.ToString("F2"));
                    }
                    else
                    {
                        currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                        currencyRate = currencyModel.Amount;
                        if (currencyModel.Code == "OK")
                        {
                            CampaignFundAvailable = GetCampaignAvailableFund(campaignDetails.CampaignProfileId, "True");
                            CampaignFundAvailable = Convert.ToDouble(Convert.ToDecimal(CampaignFundAvailable.ToString()) * currencyRate);
                            CampaignFundAvailable = Convert.ToDouble(CampaignFundAvailable.ToString("F2"));
                        }
                    }
                }
                Session["CurrencySymbol"] = currencySymbol1;
                Session["CountryId"] = CountryId;
                Session["currencyId"] = campaignCurrencyId;
                Session["poClientId"] = poClientId;
                return Json(new { data = "success", value = "(" + currencyCode + ")", value1 = campaignCurrencyId, value2 = currencySymbol1, value3 = Maximumamountofcredit, value4 = CreditAvailable, value5 = CampaignFundAvailable });
            }
            else if (label == "currency")
            {
                var campaignDetails = _profileRepository.GetById(campaignid);
                var currencySymbol1 = "";
                currencySymbol1 = currencySymbol.GetCurrencySymbolusingCurrencyId(id, _currencyRepository);
                var currencyCode1 = "";
                if (id != 0)
                {
                    currencyCode1 = _currencyRepository.Get(c => c.CurrencyId == id).CurrencyCode;
                }
                var currencyId = _contactRepository.Get(c => c.UserId == efmvcUser.UserId).CurrencyId;
                var currencySymbol2 = currencySymbol.GetCurrencySymbolusingCurrencyId(currencyId, _currencyRepository);
                //currencyCode = _currencyRepository.Get(c => c.CurrencyId == currencyId).CurrencyCode;
                currencyCode = _currencyRepository.Get(c => c.CurrencyId == currencyId).CurrencyCode;
                Session["CurrencySymbol"] = currencySymbol1;
                Session["currencyId"] = id;
                if (campaignDetails != null)
                {
                    string fromCurrencyCode = campaignDetails.CurrencyCode;
                    string toCurrencyCode = currencyCode;
                    if (campaignDetails.CurrencyCode == currencyCode)
                    {
                        CampaignFundAvailable = GetCampaignAvailableFund(campaignDetails.CampaignProfileId, "True");
                        CampaignFundAvailable = Convert.ToDouble(CampaignFundAvailable.ToString("F2"));
                        //CampaignFundAvailable = Convert.ToDouble(campaignDetails.TotalBudget.ToString("F2"));
                    }
                    else
                    {
                        currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                        currencyRate = currencyModel.Amount;
                        if (currencyModel.Code == "OK")
                        {
                            CampaignFundAvailable = GetCampaignAvailableFund(campaignDetails.CampaignProfileId, "True");
                            CampaignFundAvailable = Convert.ToDouble(Convert.ToDecimal(CampaignFundAvailable.ToString()) * currencyRate);
                            CampaignFundAvailable = Convert.ToDouble(CampaignFundAvailable.ToString("F2"));
                        }
                    }
                }
                return Json(new { data = "success", value = currencySymbol2, value1 = currencyCode1, value2 = id, value3 = Maximumamountofcredit, value4 = CreditAvailable, value5 = CampaignFundAvailable });
            }
            else
            {
                return Json(new { data = "fail" });
            }
        }
        public double GetCampaignAvailableFund(int? CampaingId, string DropdownChange)
        {
            if (DropdownChange == "True")
            {
                string campaignFundAvailable = GetCampaignFundAvailable(CampaingId);
                return Convert.ToDouble(campaignFundAvailable.ToString());
            }
            else
            {
                return 0.00;
            }
        }
        #endregion
        #region Code for Update Advert And CampaignAdvert Details
        public void UpdateAdvertAndCampaignAdvertDetails(int? CampaignProfileId, int CountryId)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            var advertInfo = _advertRepository.GetMany(top => top.CampProfileId == CampaignProfileId).ToList();
            var campaignAdvertInfo = _campaignAdvertRepository.GetMany(top => top.CampaignProfileId == CampaignProfileId).ToList();
            var ConnString = ConnectionString.GetConnectionStringByCountryId(CountryId);
            if (advertInfo.Count() > 0)
            {
                foreach (var item in advertInfo)
                {
                    var advert = db.Adverts.Where(top => top.AdvertId == item.AdvertId).FirstOrDefault();
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var itemConnString in ConnString)
                        {
                            EFMVCDataContex db1 = new EFMVCDataContex(itemConnString);
                            var advertDetails = db1.Adverts.Where(s => s.AdtoneServerAdvertId == advert.AdvertId).FirstOrDefault();
                            if (advertDetails != null)
                            {
                                advertDetails.UpdatedDateTime = DateTime.Now;
                                advertDetails.IsAdminApproval = true;
                                advertDetails.NextStatus = false;
                                db1.SaveChanges();
                            }
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db1, (int)Convert.ToInt32(TempData["CampaingId"].ToString()));
                            var campaignMatchDetails = db1.CampaignMatch.Where(s => s.MSCampaignProfileId == externalServerCampaignProfileId).FirstOrDefault();
                            if (campaignMatchDetails != null)
                            {
                                campaignMatchDetails.UpdatedDateTime = DateTime.Now;
                                campaignMatchDetails.Status = (int)CampaignStatus.Play;
                                campaignMatchDetails.NextStatus = false;
                                db1.SaveChanges();
                            }
                        }
                    }
                }
            }
            if (campaignAdvertInfo.Count() > 0)
            {
                foreach (var item in campaignAdvertInfo)
                {
                    var advert = db.Adverts.Where(top => top.AdvertId == item.AdvertId).FirstOrDefault();
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var itemConnString in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(itemConnString);
                            var advertDetails = db2.Adverts.Where(s => s.AdtoneServerAdvertId == advert.AdvertId).FirstOrDefault();
                            if (advertDetails != null)
                            {
                                advertDetails.UpdatedDateTime = DateTime.Now;
                                advertDetails.IsAdminApproval = true;
                                advertDetails.NextStatus = false;
                                db2.SaveChanges();
                            }
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db2, (int)Convert.ToInt32(TempData["CampaingId"].ToString()));
                            var campaignMatchDetails = db2.CampaignMatch.Where(s => s.MSCampaignProfileId == externalServerCampaignProfileId).FirstOrDefault();
                            if (campaignMatchDetails != null)
                            {
                                campaignMatchDetails.UpdatedDateTime = DateTime.Now;
                                campaignMatchDetails.Status = (int)CampaignStatus.Play;
                                campaignMatchDetails.NextStatus = false;
                                db2.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
        #endregion
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public int Round(double value)
        {
            double decimalpoints = Math.Abs(value - Math.Floor(value));
            if (decimalpoints > 0.5)
                return (int)Math.Round(value);
            else
                return (int)Math.Floor(value);
        }
    }
}

