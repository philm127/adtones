using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Core.Common;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Helpers;
using EFMVC.Web.Mailer;
using EFMVC.Web.ViewModels;
using Minuco.MPLS.Common.Encryption;
using EFMVC.Domain.Commands.Security;
using Renci.SshNet.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using static EFMVC.Web.Models.GravityForm;
using EFMVC.Domain.Commands.Clients;
using EFMVC.Data;
using EFMVC.Web.Common;
using System.Collections.ObjectModel;
using EFMVC.Model.Entities;

namespace EFMVC.Web.Controllers
{
    public class GravityFormsController : Controller
    {
        // GET: GravityForms
        EFMVCDataContex db = new EFMVCDataContex();
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The _send email mailer
        /// </summary>
        private ISendEmailMailer _sendEmailMailer = new SendEmailMailer();
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The form authentication
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="userRepository">The user repository.</param>
        //
        public ISendEmailMailer sendEmailMailer
        {
            get { return _sendEmailMailer; }
            set { _sendEmailMailer = value; }
        }
        public GravityFormsController(ICommandBus commandBus, IUserRepository userRepository, ICountryRepository countryRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
        }

        public class Sample
        {
            public static string GenerateSignature()
            {
                string publicKey = "97f02ff335";
                string privateKey = "b79f155f63be18b";
                string method = "GET";
                string route = "forms/1/entries";
                // string expires = Security.UtcTimestamp(new TimeSpan(0, 1, 0));

                int expires = Security.UtcTimestamp(new TimeSpan(1, 0, 0));


                string stringToSign = string.Format("{0}:{1}:{2}:{3}", publicKey, method, route, expires);



                var sig = Security.Sign(stringToSign, privateKey);
                return sig + "adtones" + expires;
                //Console.WriteLine(sig);
            }
        }

        public class Security
        {

            public static string UrlEncodeTo64(byte[] bytesToEncode)
            {
                string returnValue
                    = System.Convert.ToBase64String(bytesToEncode);

                return HttpUtility.UrlEncode(returnValue);
            }

            public static string Sign(string value, string key)
            {
                using (var hmac = new HMACSHA1(Encoding.ASCII.GetBytes(key)))
                {
                    return UrlEncodeTo64(hmac.ComputeHash(Encoding.ASCII.GetBytes(value)));
                }
            }

            public static int UtcTimestamp(TimeSpan timeSpanToAdd)
            {
                TimeSpan ts = (DateTime.UtcNow.Add(timeSpanToAdd) - new DateTime(1970, 1, 1, 0, 0, 0));
                int expires_int = (int)ts.TotalSeconds;
                return expires_int;
            }
        }
        //async Task<IHttpActionResult>
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            var data = Sample.GenerateSignature();
         

            string signature = data.Split(new string[] { "adtones" }, StringSplitOptions.None).First();
            string expires = data.Split(new string[] { "adtones" }, StringSplitOptions.None).Last();

            var path = "http://order.adtones.xyz/gravityformsapi/forms/1/entries/?api_key=97f02ff335&signature=" + signature + "&expires=" + expires + "&paging[page_size]=" + 20;
            var response = client.GetAsync(path).Result;

           

            string responseString = await response.Content.ReadAsStringAsync();
            //ViewBag.Response = responseString;
            var jsonData = JsonConvert.DeserializeObject<Welcome>(responseString);

            var entriesList = jsonData.Response.Entries.ToList();

            var CountryId = _countryRepository.Get(s => s.Name.ToLower() == "kenya").Id;


            #region Gravity Api Operation
            if (entriesList.Count() > 0)
            {
                foreach (var item in entriesList)
                {



                    //if (item.The7.ToLower() == "ben@arthar.com")
                    //{

                        try
                        {
                            int GravityFormsId = Convert.ToInt32(item.Id);
                            var GetGravityFormData = db.GravityFormsTrack.Where(s => s.GravityFormsId == GravityFormsId).FirstOrDefault();

                            if (GetGravityFormData == null)
                            {
                                string loc1 = string.IsNullOrEmpty(item.The841) ? "" : "A";
                                string loc2 = string.IsNullOrEmpty(item.The842) ? "" : "B";
                                string loc3 = string.IsNullOrEmpty(item.The843) ? "" : "C";
                                string loc4 = string.IsNullOrEmpty(item.The844) ? "" : "D";
                                string loc5 = string.IsNullOrEmpty(item.The845) ? "" : "E";
                                string loc6 = string.IsNullOrEmpty(item.The846) ? "" : "F";
                                var LocationValue = loc1 + loc2 + loc3 + loc4 + loc5 + loc6;
                                LocationValue = string.IsNullOrEmpty(LocationValue) ? null : LocationValue;

                                string Hustlers1 = string.IsNullOrEmpty(item.The461) ? "" : "A";
                                string Hustlers2 = string.IsNullOrEmpty(item.The491) ? "" : "B";
                                string Hustlers3 = string.IsNullOrEmpty(item.The511) ? "" : "C";
                                var HustlersValue = Hustlers1 + Hustlers2 + Hustlers3;
                                HustlersValue = string.IsNullOrEmpty(HustlersValue) ? null : HustlersValue;

                                string Youth1 = string.IsNullOrEmpty(item.The691) ? "" : "A";
                                string Youth2 = string.IsNullOrEmpty(item.The711) ? "" : "B";
                                string Youth3 = string.IsNullOrEmpty(item.The731) ? "" : "C";
                                var YouthValue = Youth1 + Youth2 + Youth3;
                                YouthValue = string.IsNullOrEmpty(YouthValue) ? null : YouthValue;

                                string DiscerningProfessionals1 = string.IsNullOrEmpty(item.The871) ? "" : "A";
                                string DiscerningProfessionals2 = string.IsNullOrEmpty(item.The891) ? "" : "B";
                                string DiscerningProfessionals3 = string.IsNullOrEmpty(item.The911) ? "" : "C";
                                string DiscerningProfessionals4 = string.IsNullOrEmpty(item.The931) ? "" : "D";
                                var DiscerningProfessionalsValue = DiscerningProfessionals1 + DiscerningProfessionals2 + DiscerningProfessionals3 + DiscerningProfessionals4;
                                DiscerningProfessionalsValue = string.IsNullOrEmpty(DiscerningProfessionalsValue) ? null : DiscerningProfessionalsValue;

                                string Mass1 = string.IsNullOrEmpty(item.The961) ? "" : "A";
                                string Mass2 = string.IsNullOrEmpty(item.The981) ? "" : "B";
                                string Mass3 = string.IsNullOrEmpty(item.The1001) ? "" : "C";
                                string Mass4 = string.IsNullOrEmpty(item.The1021) ? "" : "D";
                                string Mass5 = string.IsNullOrEmpty(item.The1041) ? "" : "E";
                                var MassValue = Mass1 + Mass2 + Mass3 + Mass4 + Mass5;
                                MassValue = string.IsNullOrEmpty(MassValue) ? null : MassValue;

                                UserFormModel form = new UserFormModel();
                                form.Email = item.The7;
                                form.FirstName = item.The25;
                                form.LastName = item.The78;
                                form.Organisation = item.The1;
                                form.Password = RandomString(8);

                                //if(form.Email == "testing.demi@gmail.com")
                                //{ 
                                var checkexistingemail = _userRepository.Get(u => u.Email.Trim().ToLower() == form.Email.Trim().ToLower() && u.Activated == 1 && u.RoleId == 3);

                                #region AddGravityTrackRecord
                                AddGravityTrackRecord(item.Id, item.The6, item.The7);
                                #endregion

                                if (checkexistingemail == null)
                                {
                                    #region Register Advertiser
                                    UserRegisterCommand command = Mapper.Map<UserFormModel, UserRegisterCommand>(form);
                                    command.Activated = 1;
                                    command.RoleId = (Int32)UserRoles.Advertiser;
                                    command.Outstandingdays = 0;

                                    ICommandResult result = _commandBus.Submit(command);
                                    if (result.Success)
                                    {
                                        User user = _userRepository.Get(u => u.Email == form.Email);
                                        string email = EncryptionHelper.EncryptSingleValue(user.Email);
                                        string url = string.Format("{0}?activationCode={1}",
                                                                   ConfigurationManager.AppSettings["AdvertiserVerificationUrl"], email);

                                        var reader =
                                            new StreamReader(
                                                Server.MapPath(ConfigurationManager.AppSettings["GravityAdvertiserVerificationEmailTemplate"]));
                                        string emailContent = reader.ReadToEnd();


                                        emailContent = string.Format(emailContent, url);
                                        emailContent = emailContent.Replace("randompass", form.Password);

                                        MailMessage mail = new MailMessage();
                                        mail.To.Add(user.Email);
                                        //mail.To.Add("xxx@gmail.com");
                                        mail.From = new MailAddress(ConfigurationManager.AppSettings["SiteEmailAddress"]);
                                        mail.Subject = "Email Verification";

                                        mail.Body = emailContent.Replace("\n", "<br/>");

                                        mail.IsBodyHtml = true;
                                        SmtpClient smtp = new SmtpClient();
                                        smtp.Host = ConfigurationManager.AppSettings["SmtpServerAddress"]; //Or Your SMTP Server Address
                                        smtp.Credentials = new System.Net.NetworkCredential
                                             (ConfigurationManager.AppSettings["SMTPEmail"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString()); // ***use valid credentials***
                                        smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);

                                        //Or your Smtp Email ID and Password
                                        smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailSending"].ToString());
                                        smtp.Send(mail); //REMOVE COMMENT

                                        int clientId = AddClient(user.UserId, item.The14, item.The1);
                                        AddCampaign(user.UserId, item.The14, item.The1, clientId, item.The66, item.The25, item.The78, item.The76, LocationValue, HustlersValue, YouthValue,DiscerningProfessionalsValue,MassValue, CountryId);
                                        AddCompanyDetails(user.UserId, item.The1, item.The79, CountryId);
                                        AddContactInfo(user.UserId, item.The6, form.Email, item.The79);

                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region AddCampaign
                                    int clientId = AddClient(checkexistingemail.UserId, item.The14, item.The1);
                                    AddCampaign(checkexistingemail.UserId, item.The14, item.The1, clientId, item.The66, item.The25, item.The78, item.The76, LocationValue, HustlersValue, YouthValue, DiscerningProfessionalsValue, MassValue, CountryId);
                                    #endregion
                                }
                                //}



                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    //}


                }
            }

            #endregion

            ViewBag.Window = "True";

            return View();
        }

        private void AddContactInfo(int userId, string phone, string email, string address)
        {
            Contacts con = new Contacts();

            con.UserId = userId;
            con.MobileNumber = phone;
            con.FixedLine = null;
            con.Email = email;
            con.PhoneNumber = phone;
            con.Address = address;

            db.Contacts.Add(con);
            db.SaveChanges();

        }

        private void AddCompanyDetails(int userId, string CompanyName, string CompanyAddress,int CountryId)
        {
            CompanyDetails cmp = new CompanyDetails();
            cmp.UserId = userId;
            cmp.CompanyName = CompanyName;
            cmp.Address = CompanyAddress;
            cmp.AdditionalAddress = null;
            cmp.Town = null;
            cmp.PostCode = null;
            cmp.CountryId = CountryId;

            db.CompanyDetails.Add(cmp);
            db.SaveChanges();
        }

        private void AddGravityTrackRecord(string id, string MSISDN, string Email)
        {
            GravityFormsTrack obj = new GravityFormsTrack();
            obj.GravityFormsId = Convert.ToInt32(id);
            obj.MSISDN = MSISDN;
            obj.Email = Email;
            db.GravityFormsTrack.Add(obj);
            db.SaveChanges();
        }

        private void AddCampaign(int userId, string budget, string organisation,int ClientId,string datetime,string firstname, string lastname,string bidPrice,string location,string Hustlers, string Youth, string DiscerningProfessionals, string Mass, int CountryId)
        {
            CampaignProfileFormModel model = new CampaignProfileFormModel();
            //EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            model.Status = (int)CampaignStatus.Play;
        
            model.Active = true;
            if (string.IsNullOrEmpty(budget))
            {
                model.TotalBudget = 0;
                model.TotalCredit = 0;
                model.AvailableCredit = 0;
            }
            else
            {
                model.TotalBudget = decimal.Parse(budget);
                model.TotalCredit = float.Parse(budget);
                model.AvailableCredit = float.Parse(budget);
            }

            //if (string.IsNullOrEmpty(datetime))
            //{
                model.CreatedDateTime = DateTime.Now;
                model.UpdatedDateTime = DateTime.Now;
            //}
            //else
            //{
            //    model.CreatedDateTime = Convert.ToDateTime(datetime);
            //    model.UpdatedDateTime = Convert.ToDateTime(datetime);
            //}

            DateTime dt = Convert.ToDateTime(datetime);
            var GetDate = dt.ToString("dd/MM/yyyy");

            model.CampaignName = GetDate + " Campaign";

            if(string.IsNullOrEmpty(bidPrice))
                model.MaxBid = 0;
            else
                model.MaxBid = float.Parse(bidPrice);

            var GetCampaign = db.CampaignProfiles.Where(s => s.CampaignName.Trim().ToLower() == model.CampaignName.Trim().ToLower()).FirstOrDefault();

            if (GetCampaign == null)
            {
                model.CampaignDescription = "Campaign booked for " + organisation + " by " + firstname + " " + lastname + " on " + GetDate;
                model.ClientId = ClientId;
                model.NumberInBatch = 1;

            

                model.CampaignProfileAttitudes = new Collection<CampaignProfileAttitudeFormModel>
                                                     {new CampaignProfileAttitudeFormModel()};

                CreateOrUpdateCampaignProfileCommand command =
                    Mapper.Map<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>(model);





                command.CampaignProfileAdverts =
                    Mapper.Map
                        <ICollection<CampaignProfileAdvertFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileAdvertCommand>>(model.CampaignProfileAdverts ??
                                                                                     new Collection
                                                                                         <CampaignProfileAdvertFormModel
                                                                                         >
                                                                                         {
                                                                                             new CampaignProfileAdvertFormModel
                                                                                                 ()
                                                                                         });
                //command.CampaignProfileAdverts =
                //    Mapper.Map
                //        <ICollection<CampaignProfileAdvertFormModel>,
                //            ICollection<CreateOrUpdateCampaignProfileAdvertCommand>>(model.CampaignProfileAdverts ??
                //                                                                     new Collection
                //                                                                         <CampaignProfileAdvertFormModel
                //                                                                         >
                //                                                                         {
                //                                                                             new CampaignProfileAdvertFormModel
                //                                                                                 (CountryId)
                //                                                                         });
                command.CampaignProfileAttitudes =
                    Mapper.Map
                        <ICollection<CampaignProfileAttitudeFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileAttitudeCommand>>(model.CampaignProfileAttitudes);
                command.CampaignProfileCinemas =
                    Mapper.Map
                        <ICollection<CampaignProfileCinemaFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileCinemaCommand>>(model.CampaignProfileCinemas);
                command.CampaignProfileInternets =
                    Mapper.Map
                        <ICollection<CampaignProfileInternetFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileInternetCommand>>(model.CampaignProfileInternets);
                command.CampaignProfileMobiles =
                    Mapper.Map
                        <ICollection<CampaignProfileMobileFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileMobileCommand>>(model.CampaignProfileMobiles);
                command.CampaignProfilePresses =
                    Mapper.Map
                        <ICollection<CampaignProfilePressFormModel>,
                            ICollection<CreateOrUpdateCampaignProfilePressCommand>>(model.CampaignProfilePresses);
                command.CampaignProfileProductsServices =
                    Mapper.Map
                        <ICollection<CampaignProfileProductsServiceFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileProductsServiceCommand>>(
                                model.CampaignProfileProductsServices);
                command.CampaignProfileRadios =
                    Mapper.Map
                        <ICollection<CampaignProfileRadioFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileRadioCommand>>(model.CampaignProfileRadios);
                command.CampaignProfileTimeSettings =
                    Mapper.Map
                        <ICollection<CampaignProfileTimeSettingFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileTimeSettingCommand>>(
                                model.CampaignProfileTimeSettings);
                command.CampaignProfileTvs =
                    Mapper.Map
                        <ICollection<CampaignProfileTvFormModel>, ICollection<CreateOrUpdateCampaignProfileTvCommand>>(
                            model.CampaignProfileTvs);
                //command.CampaignProfileDemographics =
                //    Mapper.Map
                //        <ICollection<CampaignProfileDemographicsFormModel>,
                //            ICollection<CreateOrUpdateCampaignProfileDemographicsCommand>>(
                //                model.CampaignProfileDemographicsFormModels ??
                //                new Collection<CampaignProfileDemographicsFormModel>
                //                    {new CampaignProfileDemographicsFormModel()});
                command.CampaignProfileDemographics =
                    Mapper.Map
                        <ICollection<CampaignProfileDemographicsFormModel>,
                            ICollection<CreateOrUpdateCampaignProfileDemographicsCommand>>(
                                model.CampaignProfileDemographicsFormModels ??
                                new Collection<CampaignProfileDemographicsFormModel>
                                    {new CampaignProfileDemographicsFormModel(CountryId)});

                command.UserId = userId;
                command.NumberInBatch = model.NumberInBatch;
                command.CountryId = CountryId;
                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        AddCampaignProfilePreference(result.Id, location, Hustlers,Youth,DiscerningProfessionals,Mass);

                        using (var SQLServerEntities = new EFMVCDataContex())
                        {
                            #region Add campaignmatch
                            var campaignmatch = new CampaignMatch();

                            campaignmatch.Budget = null;
                            campaignmatch.MaxBid = Convert.ToInt32(model.MaxBid);
                            campaignmatch.SpentToDate = "0";
                            campaignmatch.AvailableCredit = model.TotalCredit.ToString();

                            campaignmatch.Food_Advert = "ABC";
                            campaignmatch.SweetSaltySnacks_Advert = "ABC";
                            campaignmatch.AlcoholicDrinks_Advert = "ABC";
                            campaignmatch.NonAlcoholicDrinks_Advert = "ABC";
                            campaignmatch.Householdproducts_Advert = "ABC";
                            campaignmatch.ToiletriesCosmetics_Advert = "ABC";
                            campaignmatch.PharmaceuticalChemistsProducts_Advert = "ABC";
                            campaignmatch.TobaccoProducts_Advert = "ABC";
                            campaignmatch.PetsPetFood_Advert = "ABC";
                            campaignmatch.ShoppingRetailClothing_Advert = "ABC";
                            campaignmatch.DIYGardening_Advert = "ABC";
                           // campaignmatch.AppliancesOtherHouseholdDurables_Advert = "ABC";
                            campaignmatch.ElectronicsOtherPersonalItems_Advert = "ABC";
                            campaignmatch.CommunicationsInternet_Advert = "ABC";
                            campaignmatch.FinancialServices_Advert = "ABC";
                            campaignmatch.HolidaysTravel_Advert = "ABC";
                            campaignmatch.SportsLeisure_Advert = "ABC";
                            campaignmatch.Motoring_Advert = "ABC";
                            campaignmatch.Newspapers_Advert = "ABC";
                            //campaignmatch.Magazines_Advert = "ABC";
                            campaignmatch.TV_Advert = "ABC";
                            //campaignmatch.Radio_Advert = "ABC";
                            campaignmatch.Cinema_Advert = "ABC";
                            campaignmatch.SocialNetworking_Advert = "ABC";
                            //campaignmatch.GeneralUse_Advert = "ABC";
                            campaignmatch.Shopping_Advert = "ABC";
                            campaignmatch.Fitness_Advert = "ABC";
                           // campaignmatch.Holidays_Advert = "ABC";
                            campaignmatch.Environment_Advert = "ABC";
                            campaignmatch.GoingOut_Advert = "ABC";
                           // campaignmatch.FinancialProducts_Advert = "ABC";
                            campaignmatch.Religion_Advert = "ABC";
                           // campaignmatch.Fashion_Advert = "ABC";
                            campaignmatch.Music_Advert = "ABC";

                            //campaignmatch.Fitness_Attitude = "ABC";
                            //campaignmatch.Holidays_Attitude = "ABC";
                            //campaignmatch.Environment_Attitude = "ABC";
                            //campaignmatch.GoingOut_Attitude = "ABC";
                            //campaignmatch.FinancialStabiity_Attitude = "ABC";
                            //campaignmatch.Religion_Attitude = "ABC";
                            //campaignmatch.Fashion_Attitude = "ABC";
                            //campaignmatch.Music_Attitude = "ABC";

                            //campaignmatch.Cinema_Cinema = "ABCD";

                            campaignmatch.DOBStart_Demographics = null;
                            campaignmatch.DOBEnd_Demographics = null;
                            campaignmatch.Gender_Demographics = "ABC";
                            campaignmatch.IncomeBracket_Demographics = "ABCDEFG";
                            campaignmatch.WorkingStatus_Demographics = "ABCDEFGHI";
                            campaignmatch.RelationshipStatus_Demographics = "ABCDEFG";
                            campaignmatch.Education_Demographics = "ABCDE";
                            campaignmatch.HouseholdStatus_Demographics = "ABCD";

                            campaignmatch.Age_Demographics = "ABCDEFGH";
                            campaignmatch.ContractType_Mobile = "ABC";
                            campaignmatch.Spend_Mobile = "ABCDEFG";

                            //campaignmatch.SocialNetworking_Internet = "ABCD";
                            //campaignmatch.Video_Internet = "ABCD";
                            //campaignmatch.Research_Internet = "ABCD";
                            //campaignmatch.Auctions_Internet = "ABCD";
                            //campaignmatch.Shopping_Internet = "ABCD";

                            //campaignmatch.Local_Press = "ABCD";
                            //campaignmatch.National_Press = "ABCD";
                            //campaignmatch.FreeNewpapers_Press = "ABCD";
                            //campaignmatch.Magazines_Press = "ABCD";
                            //campaignmatch.Food_ProductsService = "ABCD";
                            //campaignmatch.SweetSaltySnacks_ProductsService = "ABCD";
                            //campaignmatch.AlcoholicDrinks_ProductsService = "ABCD";
                            //campaignmatch.NonAlcoholicDrinks_ProductsService = "ABCD";
                            //campaignmatch.Householdproducts_ProductsService = "ABCD";
                            //campaignmatch.ToiletriesCosmetics_ProductsService = "ABCD";
                            //campaignmatch.PharmaceuticalChemistsProducts_ProductsService = "ABCD";
                            //campaignmatch.TobaccoProducts_ProductsService = "ABCD";
                            //campaignmatch.PetsPetFood_ProductsService = "ABCD";
                            //campaignmatch.ShoppingRetailClothing_ProductsService = "ABCD";
                            //campaignmatch.DIYGardening_ProductsService = "ABCD";
                            //campaignmatch.AppliancesOtherHouseholdDurables_ProductsService = "ABCD";
                            //campaignmatch.ElectronicsOtherPersonalItems_ProductsService = "ABCD";
                            //campaignmatch.CommunicationsInternet_ProductsService = "ABCD";
                            //campaignmatch.FinancialServices_ProductsService = "ABCD";
                            //campaignmatch.HolidaysTravel_ProductsService = "ABCD";
                            //campaignmatch.SportsLeisure_ProductsService = "ABCD";
                            //campaignmatch.Motoring_ProductsService = "ABCD";
                            //campaignmatch.National_Radio = "ABCD";
                            //campaignmatch.Local_Radio = "ABCD";
                            //campaignmatch.Music_Radio = "ABCD";
                            //campaignmatch.Sport_Radio = "ABCD";
                            //campaignmatch.Talk_Radio = "ABCD";
                            //campaignmatch.Satallite_TV = "ABCD";
                            //campaignmatch.Cable_TV = "ABCD";
                            //campaignmatch.Terrestrial_TV = "ABCD";
                            //campaignmatch.Internet_TV = "ABCD";


                            //campaignmatch.Location_Demographics = "ABCDEF";
                            campaignmatch.Location_Demographics = location;//"ABCDEFGHIJKLMNO";
                            campaignmatch.BusinessOrOpportunities_AdType = "ABC";
                            campaignmatch.Gambling_AdType = "ABC";
                            campaignmatch.Restaurants_AdType = "ABC";
                            campaignmatch.Insurance_AdType = "ABC";
                            campaignmatch.Furniture_AdType = "ABC";
                            campaignmatch.InformationTechnology_AdType = "ABC";
                            campaignmatch.Energy_AdType = "ABC";
                            campaignmatch.Supermarkets_AdType = "ABC";
                            campaignmatch.Healthcare_AdType = "ABC";
                            campaignmatch.JobsAndEducation_AdType = "ABC";
                            campaignmatch.Gifts_AdType = "ABC";
                            campaignmatch.AdvocacyOrLegal_AdType = "ABC";
                            campaignmatch.DatingAndPersonal_AdType = "ABC";
                            campaignmatch.RealEstate_AdType = "ABC";
                            campaignmatch.Games_AdType = "ABC";
                            // campaignmatch.SkizaProfile_AdType = null; ;

                            campaignmatch.Hustlers_AdType = Hustlers;
                            campaignmatch.Youth_AdType = Youth;
                            campaignmatch.DiscerningProfessionals_AdType = DiscerningProfessionals;
                            campaignmatch.Mass_AdType = Mass;

                            campaignmatch.EMAIL_MESSAGE = null;
                            campaignmatch.MEDIA_URL = null;
                            campaignmatch.SMS_MESSAGE = null;
                            campaignmatch.ORIGINATOR = null;
                            campaignmatch.MSCampaignProfileId = result.Id;
                            campaignmatch.UserId = userId;

                            campaignmatch.ClientId = model.ClientId;
                            campaignmatch.CampaignName = model.CampaignName;
                            campaignmatch.CampaignDescription = model.CampaignDescription;
                            campaignmatch.TotalBudget = Convert.ToInt64(model.TotalBudget);
                            campaignmatch.MaxDailyBudget = Convert.ToInt64(model.MaxDailyBudget);

                            campaignmatch.MaxMonthBudget = 0;
                            campaignmatch.MaxWeeklyBudget = 0;
                            campaignmatch.MaxHourlyBudget = 0;
                            campaignmatch.TotalCredit = Convert.ToDecimal(model.TotalCredit);
                            campaignmatch.PlaysToDate = 0;
                            campaignmatch.PlaysLastMonth = 0;
                            campaignmatch.PlaysCurrentMonth = 0;
                            campaignmatch.CancelledToDate = 0;
                            campaignmatch.CancelledLastMonth = 0;
                            campaignmatch.CancelledCurrentMonth = 0;
                            campaignmatch.SmsToDate = 0;
                            campaignmatch.SmsLastMonth = 0;
                            campaignmatch.SmsCurrentMonth = 0;
                            campaignmatch.EmailToDate = 0;
                            campaignmatch.EmailsLastMonth = 0;
                            campaignmatch.EmailsCurrentMonth = 0;
                            campaignmatch.EmailFileLocation = null;
                            campaignmatch.Active = model.Active;
                            campaignmatch.NumberOfPlays = null;
                            campaignmatch.AverageDailyPlays = null;
                            campaignmatch.SmsRequests = null;
                            campaignmatch.EmailsDelievered = null;
                            campaignmatch.EmailSubject = null;
                            campaignmatch.EmailBody = null;
                            campaignmatch.EmailSenderAddress = null;
                            campaignmatch.SmsOriginator = null;
                            campaignmatch.SmsBody = null;
                            campaignmatch.SMSFileLocation = null;
                            campaignmatch.CreatedDateTime = model.CreatedDateTime;
                            campaignmatch.UpdatedDateTime = model.UpdatedDateTime;
                            campaignmatch.Status = model.Status;
                            campaignmatch.StartDate = model.StartDate;
                            campaignmatch.EndDate = model.EndDate;
                            campaignmatch.NumberInBatch = model.NumberInBatch;

                            SQLServerEntities.CampaignMatch.Add(campaignmatch);
                            SQLServerEntities.SaveChanges();
                            #endregion

                        }


                    }
                }
            }
        }


        private void AddCampaignProfilePreference(int CampaignProfileId,string location, string Hustlers, string Youth, string DiscerningProfessionals, string Mass)
        {
            CampaignProfilePreference obj = new CampaignProfilePreference();

            obj.CampaignProfileId = CampaignProfileId;
            obj.Food_Advert = "ABC";
            obj.SweetSaltySnacks_Advert = "ABC";
            obj.AlcoholicDrinks_Advert = "ABC";
            obj.NonAlcoholicDrinks_Advert = "ABC";
            obj.Householdproducts_Advert = "ABC";
            obj.ToiletriesCosmetics_Advert = "ABC";
            obj.PharmaceuticalChemistsProducts_Advert = "ABC";
            obj.TobaccoProducts_Advert = "ABC";
            obj.PetsPetFood_Advert = "ABC";
            obj.ShoppingRetailClothing_Advert = "ABC";
            obj.DIYGardening_Advert = "ABC";
            //obj.AppliancesOtherHouseholdDurables_Advert = "ABC";
            obj.ElectronicsOtherPersonalItems_Advert = "ABC";
            obj.CommunicationsInternet_Advert = "ABC";
            obj.FinancialServices_Advert = "ABC";
            obj.HolidaysTravel_Advert = "ABC";
            obj.SportsLeisure_Advert = "ABC";
            obj.Motoring_Advert = "ABC";
            obj.Newspapers_Advert = "ABC";
            //obj.Magazines_Advert = "ABC";
            obj.TV_Advert = "ABC";
           // obj.Radio_Advert = "ABC";
            obj.Cinema_Advert = "ABC";
            obj.SocialNetworking_Advert = "ABC";
            //obj.GeneralUse_Advert = "ABC";
            obj.Shopping_Advert = "ABC";
            obj.Fitness_Advert = "ABC";
            //obj.Holidays_Advert = "ABC";
            obj.Environment_Advert = "ABC";
            obj.GoingOut_Advert = "ABC";
           // obj.FinancialProducts_Advert = "ABC";
            obj.Religion_Advert = "ABC";
           // obj.Fashion_Advert = "ABC";
            obj.Music_Advert = "ABC";

            //obj.Fitness_Attitude = "ABC";
            //obj.Holidays_Attitude = "ABC";
            //obj.Environment_Attitude = "ABC";
            //obj.GoingOut_Attitude = "ABC";
            //obj.FinancialStabiity_Attitude = "ABC";
            //obj.Religion_Attitude = "ABC";
            //obj.Fashion_Attitude = "ABC";
            //obj.Music_Attitude = "ABC";
            //obj.Cinema_Cinema = "ABCD";

            obj.DOBStart_Demographics = null;
            obj.DOBEnd_Demographics = null;

            obj.Gender_Demographics = "ABC";
            obj.IncomeBracket_Demographics = "ABCDEFG";
            obj.WorkingStatus_Demographics = "ABCDEFGHI";
            obj.RelationshipStatus_Demographics = "ABCDEFG";
            obj.Education_Demographics = "ABCDE";
            obj.HouseholdStatus_Demographics = "ABCD";
            obj.Age_Demographics = "ABCDEFGH";

            //obj.SocialNetworking_Internet = "ABCD";
            //obj.Video_Internet = "ABCD";
            //obj.Research_Internet = "ABCD";
            //obj.Auctions_Internet = "ABCD";
            //obj.Shopping_Internet = "ABCD";

            obj.ContractType_Mobile = "ABC";
            obj.Spend_Mobile = "ABCDEFG";

            //obj.Local_Press = "ABCD";
            //obj.National_Press = "ABCD";
            //obj.FreeNewpapers_Press = "ABCD";
            //obj.Magazines_Press = "ABCD";
            //obj.Food_ProductsService = "ABCD";
            //obj.SweetSaltySnacks_ProductsService = "ABCD";
            //obj.AlcoholicDrinks_ProductsService = "ABCD";
            //obj.NonAlcoholicDrinks_ProductsService = "ABCD";
            //obj.Householdproducts_ProductsService = "ABCD";
            //obj.ToiletriesCosmetics_ProductsService = "ABCD";
            //obj.PharmaceuticalChemistsProducts_ProductsService = "ABCD";
            //obj.TobaccoProducts_ProductsService = "ABCD";
            //obj.PetsPetFood_ProductsService = "ABCD";
            //obj.ShoppingRetailClothing_ProductsService = "ABCD";
            //obj.DIYGardening_ProductsService = "ABCD";
            //obj.AppliancesOtherHouseholdDurables_ProductsService = "ABCD";
            //obj.ElectronicsOtherPersonalItems_ProductsService = "ABCD";
            //obj.CommunicationsInternet_ProductsService = "ABCD";
            //obj.FinancialServices_ProductsService = "ABCD";
            //obj.HolidaysTravel_ProductsService = "ABCD";
            //obj.SportsLeisure_ProductsService = "ABCD";
            //obj.Motoring_ProductsService = "ABCD";
            //obj.National_Radio = "ABCD";
            //obj.Local_Radio = "ABCD";
            //obj.Music_Radio = "ABCD";
            //obj.Sport_Radio = "ABCD";
            //obj.Talk_Radio = "ABCD";
            //obj.Satallite_TV = "ABCD";
            //obj.Cable_TV = "ABCD";
            //obj.Terrestrial_TV = "ABCD";
            //obj.Internet_TV = "ABCD";

            obj.Location_Demographics = location; //"ABCDEFGHIJKLMNO";//"ABCDEF";
            obj.BusinessOrOpportunities_AdType = "ABC";
            obj.Gambling_AdType = "ABC";
            obj.Restaurants_AdType = "ABC";
            obj.Insurance_AdType = "ABC";
            obj.Furniture_AdType = "ABC";
            obj.InformationTechnology_AdType = "ABC";
            obj.Energy_AdType = "ABC";
            obj.Supermarkets_AdType = "ABC";
            obj.Healthcare_AdType = "ABC";
            obj.JobsAndEducation_AdType = "ABC";
            obj.Gifts_AdType = "ABC";
            obj.AdvocacyOrLegal_AdType = "ABC";
            obj.DatingAndPersonal_AdType = "ABC";
            obj.RealEstate_AdType = "ABC";
            obj.Games_AdType = "ABC";
            //obj.SkizaProfile_AdType = null;
            obj.Hustlers_AdType = Hustlers;
            obj.Youth_AdType = Youth;
            obj.DiscerningProfessionals_AdType = DiscerningProfessionals;
            obj.Mass_AdType = Mass;

            obj.Postcode = null;
            obj.CountryId = 0;

            db.CampaignProfilePreference.Add(obj);
            db.SaveChanges();

            //CampaignProfileTimeSetting obj2 = new CampaignProfileTimeSetting();
            //obj2.CampaignProfileId = CampaignProfileId;
            //var hours = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
            //obj2.Monday = hours;
            //obj2.Tuesday = hours;
            //obj2.Wednesday = hours;
            //obj2.Thursday = hours;
            //obj2.Friday = hours;
            //obj2.Saturday = hours;
            //obj2.Sunday = hours;
            //db.CampaignProfileTimeSettings.Add(obj2);
            //db.SaveChanges();

        }

        private int AddClient(int userId, string budget, string clientName)
        {
            
            var GetClientData = db.Clients.Where(s => s.Name.Trim().ToLower() == clientName.Trim().ToLower()).FirstOrDefault();
            if (GetClientData != null)
            {
                return GetClientData.Id;
            }
            else
            {
                 Client _client = new Client();
                _client.UserId = userId;
                _client.Name = clientName;
                _client.Description = clientName + " Description";
                _client.ContactInfo = clientName + " ContactInfo";

                if (string.IsNullOrEmpty(budget))
                    _client.Budget = 10000;
                else
                    _client.Budget = Convert.ToDecimal(budget);

                _client.CreatedDate = DateTime.Now;
                _client.UpdatedDate = DateTime.Now;


                _client.Status = 1;

                db.Clients.Add(_client);
                db.SaveChanges();

                return _client.Id;
                
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}