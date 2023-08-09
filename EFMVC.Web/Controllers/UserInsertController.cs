
using EFMVC.Data;
using EFMVC.Model;
using EFMVC.ProvisioningModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    public class UserInsertController : Controller
    {
        // GET: UserInsert
        EFMVCDataContex db = new EFMVCDataContex();
        arthar_addcache_provisioningEntities4 mySQLEntities = new arthar_addcache_provisioningEntities4();
        //Step 1
        public ActionResult Index()
        {
            //440000010000@email.com 440000013680@email.com
            //var number = 440000010000;
            //for (int i = 1; i <= 1000; i++)
            //{
            //    number = number + 1;
            //    User userrecord = new User();
            //    userrecord.Email = number + "@email.com";
            //    userrecord.FirstName = "Test-" + i;
            //    userrecord.LastName = "Last-" + i;
            //    userrecord.PasswordHash = "�r!nk�2|}�xO�";
            //    userrecord.DateCreated = DateTime.Now;
            //    userrecord.Organisation = "Adtones";
            //    userrecord.LastLoginTime = DateTime.Now;
            //    userrecord.Activated = 1;
            //    userrecord.RoleId = 2;
            //    userrecord.VerificationStatus = true;
            //    userrecord.Outstandingdays = 0;
            //    db.Users.Add(userrecord);
            //    db.SaveChanges();

            //    UserProfile obj = new UserProfile();
            //    obj.UserId = userrecord.UserId;
            //    obj.DOB = null;
            //    obj.Gender = "A";
            //    obj.IncomeBracket = "F";
            //    obj.WorkingStatus = "A";
            //    obj.RelationshipStatus = "E";
            //    obj.Education = "D";
            //    obj.HouseholdStatus = "B";
            //    obj.Location = "A";
            //    obj.MSISDN = number.ToString();
            //    db.Userprofiles.Add(obj);
            //    db.SaveChanges();



            //    UserProfilePreference obj2 = new UserProfilePreference();

            //    obj2.UserProfileId = obj.UserProfileId;
            //    obj2.Gender_Demographics = "A";
            //    obj2.IncomeBracket_Demographics = "B";
            //    obj2.WorkingStatus_Demographics = "A";
            //    obj2.RelationshipStatus_Demographics = "E";
            //    obj2.Education_Demographics = "A";
            //    obj2.HouseholdStatus_Demographics = "A";
            //    obj2.Location_Demographics = "A";
            //    obj2.Food_Advert = "B";
            //    obj2.SweetSaltySnacks_Advert = "B";
            //    obj2.AlcoholicDrinks_Advert = "B";
            //    obj2.NonAlcoholicDrinks_Advert = "B";
            //    obj2.Householdproducts_Advert = "C";

            //    obj2.ToiletriesCosmetics_Advert = "B";
            //    obj2.PharmaceuticalChemistsProducts_Advert = "B";
            //    obj2.TobaccoProducts_Advert = "B";
            //    obj2.PetsPetFood_Advert = "B";
            //    obj2.ShoppingRetailClothing_Advert = "B";
            //    obj2.DIYGardening_Advert = "B";
            //    obj2.AppliancesOtherHouseholdDurables_Advert = "B";
            //    obj2.ElectronicsOtherPersonalItems_Advert = "B";
            //    obj2.CommunicationsInternet_Advert = "B";
            //    obj2.FinancialServices_Advert = "B";
            //    obj2.HolidaysTravel_Advert = "B";
            //    obj2.SportsLeisure_Advert = "B";
            //    obj2.Motoring_Advert = "B";
            //    obj2.Newspapers_Advert = "B";
            //    obj2.Magazines_Advert = "B";
            //    obj2.TV_Advert = "B";
            //    obj2.Radio_Advert = "B";
            //    obj2.Cinema_Advert = "B";
            //    obj2.SocialNetworking_Advert = "B";
            //    obj2.GeneralUse_Advert = "B";
            //    obj2.Shopping_Advert = "B";
            //    obj2.Fitness_Advert = "B";
            //    obj2.Holidays_Advert = "B";
            //    obj2.Environment_Advert = "B";
            //    obj2.GoingOut_Advert = "B";
            //    obj2.FinancialProducts_Advert = "B";
            //    obj2.Religion_Advert = "B";
            //    obj2.Fashion_Advert = "B";
            //    obj2.Music_Advert = "B";
            //    obj2.Fitness_Attitude = "B";
            //    obj2.Holidays_Attitude = "B";
            //    obj2.Environment_Attitude = "B";
            //    obj2.GoingOut_Attitude = "B";
            //    obj2.FinancialStabiity_Attitude = "B";
            //    obj2.Religion_Attitude = "B";
            //    obj2.Fashion_Attitude = "B";
            //    obj2.Music_Attitude = "B";

            //    obj2.Cinema_Cinema = "A";
            //    obj2.SocialNetworking_Internet = "A";
            //    obj2.Video_Internet = "A";
            //    obj2.Research_Internet = "A";
            //    obj2.Auctions_Internet = "A";
            //    obj2.Shopping_Internet = "A";
            //    obj2.ContractType_Mobile = "B";
            //    obj2.Spend_Mobile = "C";
            //    obj2.Local_Press = "A";
            //    obj2.National_Press = "A";
            //    obj2.FreeNewpapers_Press = "A";
            //    obj2.Magazines_Press = "A";
            //    obj2.Food_ProductsService = "A";
            //    obj2.SweetSaltySnacks_ProductsService = "A";
            //    obj2.AlcoholicDrinks_ProductsService = "A";
            //    obj2.NonAlcoholicDrinks_ProductsService = "A";
            //    obj2.Householdproducts_ProductsService = "A";
            //    obj2.ToiletriesCosmetics_ProductsService = "A";
            //    obj2.PharmaceuticalChemistsProducts_ProductsService = "A";
            //    obj2.TobaccoProducts_ProductsService = "A";
            //    obj2.PetsPetFood_ProductsService = "A";
            //    obj2.ShoppingRetailClothing_ProductsService = "A";
            //    obj2.DIYGardening_ProductsService = "A";
            //    obj2.AppliancesOtherHouseholdDurables_ProductsService = "A";
            //    obj2.ElectronicsOtherPersonalItems_ProductsService = "A";
            //    obj2.CommunicationsInternet_ProductsService = "A";
            //    obj2.FinancialServices_ProductsService = "A";
            //    obj2.HolidaysTravel_ProductsService = "A";
            //    obj2.SportsLeisure_ProductsService = "A";
            //    obj2.Motoring_ProductsService = "A";
            //    obj2.National_Radio = "A";
            //    obj2.Local_Radio = "A";
            //    obj2.Music_Radio = "A";
            //    obj2.Sport_Radio = "A";
            //    obj2.Talk_Radio = "A";
            //    obj2.Satallite_TV = "A";
            //    obj2.Cable_TV = "A";
            //    obj2.Terrestrial_TV = "A";
            //    obj2.Internet_TV = "A";

            //    db.UserProfilePreference.Add(obj2);
            //    db.SaveChanges();

            //    UserProfileTimeSetting userTime = new UserProfileTimeSetting();
            //    userTime.UserProfileId = obj.UserProfileId;
            //    userTime.Monday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
            //    userTime.Tuesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
            //    userTime.Wednesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
            //    userTime.Thursday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
            //    userTime.Friday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
            //    userTime.Saturday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
            //    userTime.Sunday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
            //    db.UserProfileTimeSetting.Add(userTime);
            //    db.SaveChanges();


            //}
            return View();
        }
       
        //Step 1
        public ActionResult AddUser()
        {
            //440000010000@email.com
            //440000000001@email.com
            //440000010000@email.com 440000013680@email.com
            //var number = db.Userprofiles.OrderByDescending(s => s.UserProfileId).FirstOrDefault().MSISDN;
            var number = 440000010000;
            for (int i = 1; i <= 90000; i++)
            {
                number = number + 1;
                User userrecord = new User();
                userrecord.Email = number + "@email.com";
                userrecord.FirstName = "Test-" + i;
                userrecord.LastName = "Last-" + i;
                userrecord.PasswordHash = "�r!nk�2|}�xO�";
                userrecord.DateCreated = DateTime.Now;
                userrecord.Organisation = "Adtones";
                userrecord.LastLoginTime = DateTime.Now;
                userrecord.Activated = 1;
                userrecord.RoleId = 2;
                userrecord.VerificationStatus = true;
                userrecord.Outstandingdays = 0;
                db.Users.Add(userrecord);
                db.SaveChanges();

                UserProfile obj = new UserProfile();
                obj.UserId = userrecord.UserId;
                obj.DOB = null;
                obj.Gender = "A";
                obj.IncomeBracket = "F";
                obj.WorkingStatus = "A";
                obj.RelationshipStatus = "E";
                obj.Education = "D";
                obj.HouseholdStatus = "B";
                obj.Location = "A";
                obj.MSISDN = number.ToString();
                db.Userprofiles.Add(obj);
                db.SaveChanges();



                UserProfilePreference obj2 = new UserProfilePreference();

                obj2.UserProfileId = obj.UserProfileId;
                obj2.Gender_Demographics = "A";
                obj2.IncomeBracket_Demographics = "B";
                obj2.WorkingStatus_Demographics = "A";
                obj2.RelationshipStatus_Demographics = "E";
                obj2.Education_Demographics = "A";
                obj2.HouseholdStatus_Demographics = "A";
                obj2.Location_Demographics = "A";
                obj2.Food_Advert = "B";
                obj2.SweetSaltySnacks_Advert = "B";
                obj2.AlcoholicDrinks_Advert = "B";
                obj2.NonAlcoholicDrinks_Advert = "B";
                obj2.Householdproducts_Advert = "C";

                obj2.ToiletriesCosmetics_Advert = "B";
                obj2.PharmaceuticalChemistsProducts_Advert = "B";
                obj2.TobaccoProducts_Advert = "B";
                obj2.PetsPetFood_Advert = "B";
                obj2.ShoppingRetailClothing_Advert = "B";
                obj2.DIYGardening_Advert = "B";
                obj2.AppliancesOtherHouseholdDurables_Advert = "B";
                obj2.ElectronicsOtherPersonalItems_Advert = "B";
                obj2.CommunicationsInternet_Advert = "B";
                obj2.FinancialServices_Advert = "B";
                obj2.HolidaysTravel_Advert = "B";
                obj2.SportsLeisure_Advert = "B";
                obj2.Motoring_Advert = "B";
                obj2.Newspapers_Advert = "B";
                obj2.Magazines_Advert = "B";
                obj2.TV_Advert = "B";
                obj2.Radio_Advert = "B";
                obj2.Cinema_Advert = "B";
                obj2.SocialNetworking_Advert = "B";
                obj2.GeneralUse_Advert = "B";
                obj2.Shopping_Advert = "B";
                obj2.Fitness_Advert = "B";
                obj2.Holidays_Advert = "B";
                obj2.Environment_Advert = "B";
                obj2.GoingOut_Advert = "B";
                obj2.FinancialProducts_Advert = "B";
                obj2.Religion_Advert = "B";
                obj2.Fashion_Advert = "B";
                obj2.Music_Advert = "B";
                obj2.Fitness_Attitude = "B";
                obj2.Holidays_Attitude = "B";
                obj2.Environment_Attitude = "B";
                obj2.GoingOut_Attitude = "B";
                obj2.FinancialStabiity_Attitude = "B";
                obj2.Religion_Attitude = "B";
                obj2.Fashion_Attitude = "B";
                obj2.Music_Attitude = "B";

                obj2.Cinema_Cinema = "A";
                obj2.SocialNetworking_Internet = "A";
                obj2.Video_Internet = "A";
                obj2.Research_Internet = "A";
                obj2.Auctions_Internet = "A";
                obj2.Shopping_Internet = "A";
                obj2.ContractType_Mobile = "B";
                obj2.Spend_Mobile = "C";
                obj2.Local_Press = "A";
                obj2.National_Press = "A";
                obj2.FreeNewpapers_Press = "A";
                obj2.Magazines_Press = "A";
                obj2.Food_ProductsService = "A";
                obj2.SweetSaltySnacks_ProductsService = "A";
                obj2.AlcoholicDrinks_ProductsService = "A";
                obj2.NonAlcoholicDrinks_ProductsService = "A";
                obj2.Householdproducts_ProductsService = "A";
                obj2.ToiletriesCosmetics_ProductsService = "A";
                obj2.PharmaceuticalChemistsProducts_ProductsService = "A";
                obj2.TobaccoProducts_ProductsService = "A";
                obj2.PetsPetFood_ProductsService = "A";
                obj2.ShoppingRetailClothing_ProductsService = "A";
                obj2.DIYGardening_ProductsService = "A";
                obj2.AppliancesOtherHouseholdDurables_ProductsService = "A";
                obj2.ElectronicsOtherPersonalItems_ProductsService = "A";
                obj2.CommunicationsInternet_ProductsService = "A";
                obj2.FinancialServices_ProductsService = "A";
                obj2.HolidaysTravel_ProductsService = "A";
                obj2.SportsLeisure_ProductsService = "A";
                obj2.Motoring_ProductsService = "A";
                obj2.National_Radio = "A";
                obj2.Local_Radio = "A";
                obj2.Music_Radio = "A";
                obj2.Sport_Radio = "A";
                obj2.Talk_Radio = "A";
                obj2.Satallite_TV = "A";
                obj2.Cable_TV = "A";
                obj2.Terrestrial_TV = "A";
                obj2.Internet_TV = "A";

                db.UserProfilePreference.Add(obj2);
                db.SaveChanges();

                //UserProfileTimeSetting userTime = new UserProfileTimeSetting();
                //userTime.UserProfileId = obj.UserProfileId;
                //userTime.Monday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userTime.Tuesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userTime.Wednesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userTime.Thursday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userTime.Friday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userTime.Saturday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userTime.Sunday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //db.UserProfileTimeSetting.Add(userTime);
                //db.SaveChanges();


             
                var usermatch = new UserMatch();
                usermatch.Gender_Demographics = "A";
                usermatch.IncomeBracket_Demographics = "B";
                usermatch.WorkingStatus_Demographics = "A";
                usermatch.RelationshipStatus_Demographics = "E";
                usermatch.Education_Demographics = "A";
                usermatch.HouseholdStatus_Demographics = "A";
                usermatch.Location_Demographics = "A";
                usermatch.Food_Advert = "C";
                usermatch.SweetSaltySnacks_Advert = "B";
                usermatch.AlcoholicDrinks_Advert = "B";
                usermatch.NonAlcoholicDrinks_Advert = "B";
                usermatch.Householdproducts_Advert = "B";
                usermatch.ToiletriesCosmetics_Advert = "B";
                usermatch.PharmaceuticalChemistsProducts_Advert = "B";
                usermatch.TobaccoProducts_Advert = "B";
                usermatch.PetsPetFood_Advert = "B";
                usermatch.ShoppingRetailClothing_Advert = "B";
                usermatch.DIYGardening_Advert = "B";
                usermatch.AppliancesOtherHouseholdDurables_Advert = "B";
                usermatch.ElectronicsOtherPersonalItems_Advert = "B";
                usermatch.CommunicationsInternet_Advert = "B";
                usermatch.FinancialServices_Advert = "B";
                usermatch.HolidaysTravel_Advert = "B";
                usermatch.SportsLeisure_Advert = "B";
                usermatch.Motoring_Advert = "B";
                usermatch.Newspapers_Advert = "B";
                usermatch.Magazines_Advert = "B";
                usermatch.TV_Advert = "B";
                usermatch.Radio_Advert = "B";
                usermatch.Cinema_Advert = "B";
                usermatch.SocialNetworking_Advert = "B";
                usermatch.GeneralUse_Advert = "B";
                usermatch.Shopping_Advert = "B";
                usermatch.Fitness_Advert = "B";
                usermatch.Holidays_Advert = "B";
                usermatch.Environment_Advert = "B";
                usermatch.GoingOut_Advert = "B";
                usermatch.FinancialProducts_Advert = "B";
                usermatch.Religion_Advert = "B";
                usermatch.Fashion_Advert = "B";
                usermatch.Music_Advert = "B";
                usermatch.Fitness_Attitude = "B";
                usermatch.Holidays_Attitude = "B";
                usermatch.Environment_Attitude = "B";
                usermatch.GoingOut_Attitude = "B";
                usermatch.FinancialStabiity_Attitude = "B";
                usermatch.Religion_Attitude = "B";
                usermatch.Fashion_Attitude = "B";
                usermatch.Music_Attitude = "B";



                usermatch.Cinema_Cinema = "A";
                usermatch.SocialNetworking_Internet = "A";
                usermatch.Video_Internet = "A";
                usermatch.Research_Internet = "A";
                usermatch.Auctions_Internet = "A";
                usermatch.Shopping_Internet = "A";


                usermatch.ContractType_Mobile = "B";
                usermatch.Spend_Mobile = "C";

                usermatch.Local_Press = "A";
                usermatch.National_Press = "A";
                usermatch.FreeNewpapers_Press = "A";
                usermatch.Magazines_Press = "A";
                usermatch.Food_ProductsService = "A";
                usermatch.SweetSaltySnacks_ProductsService = "A";
                usermatch.AlcoholicDrinks_ProductsService = "A";
                usermatch.NonAlcoholicDrinks_ProductsService = "A";
                usermatch.Householdproducts_ProductsService = "A";
                usermatch.ToiletriesCosmetics_ProductsService = "A";
                usermatch.PharmaceuticalChemistsProducts_ProductsService = "A";
                usermatch.TobaccoProducts_ProductsService = "A";
                usermatch.PetsPetFood_ProductsService = "A";
                usermatch.ShoppingRetailClothing_ProductsService = "A";
                usermatch.DIYGardening_ProductsService = "A";
                usermatch.AppliancesOtherHouseholdDurables_ProductsService = "A";
                usermatch.ElectronicsOtherPersonalItems_ProductsService = "A";
                usermatch.CommunicationsInternet_ProductsService = "A";
                usermatch.FinancialServices_ProductsService = "A";
                usermatch.HolidaysTravel_ProductsService = "A";
                usermatch.SportsLeisure_ProductsService = "A";
                usermatch.Motoring_ProductsService = "A";
                usermatch.National_Radio = "A";
                usermatch.Local_Radio = "A";
                usermatch.Music_Radio = "A";
                usermatch.Sport_Radio = "A";
                usermatch.Talk_Radio = "A";
                usermatch.Satallite_TV = "A";
                usermatch.Cable_TV = "A";
                usermatch.Terrestrial_TV = "A";
                usermatch.Internet_TV = "A";

               

                usermatch.Email = number + "@email.com"; 
                usermatch.MSISDN = number.ToString();
                usermatch.MSUserProfileId = obj.UserProfileId;
                usermatch.UserId = userrecord.UserId;
                usermatch.DOB = null;
                usermatch.Gender = "A";
                usermatch.IncomeBracket ="F";
                usermatch.WorkingStatus = "A";
                usermatch.RelationshipStatus = "E";
                usermatch.Education = "D";
                usermatch.HouseholdStatus = "B";
                usermatch.Location ="A";

                usermatch.BusinessOrOpportunities_AdType = "B";
                usermatch.Gambling_AdType = "B";
                usermatch.Restaurants_AdType = "B";
                usermatch.Insurance_AdType = "B";
                usermatch.Furniture_AdType = "B";
                usermatch.InformationTechnology_AdType = "B";
                usermatch.Energy_AdType = "B";
                usermatch.Supermarkets_AdType = "B";
                usermatch.Healthcare_AdType = "B";
                usermatch.JobsAndEducation_AdType = "B";
                usermatch.Gifts_AdType = "B";
                usermatch.AdvocacyOrLegal_AdType = "B";
                usermatch.DatingAndPersonal_AdType = "B";
                usermatch.RealEstate_AdType = "B";
                usermatch.Games_AdType = "B";
                usermatch.Hustlers_AdType = "A";
                usermatch.Youth_AdType = "A";
                usermatch.DiscerningProfessionals_AdType = "A";
                usermatch.Mass_AdType = "A";

                db.UserMatch.Add(usermatch);
                db.SaveChanges();


                //var usertimesetting = new userprofiletimesetting();
                //usertimesetting.UserProfileId = usermatch.UserProfileId;
                //usertimesetting.Monday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //usertimesetting.Tuesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //usertimesetting.Wednesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //usertimesetting.Thursday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //usertimesetting.Friday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //usertimesetting.Saturday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //usertimesetting.Sunday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //mySQLEntities.userprofiletimesettings.Add(usertimesetting);
                //mySQLEntities.SaveChanges();


            }
            return View();
        }

        //Step 2
        // Add records in MySql 
        public ActionResult AddUserMatch()
        {
            using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
            {
                var usermatch = new usermatch();
                var UserList = db.Users.Where(s => s.RoleId == 2 && s.Activated == 1 && s.VerificationStatus == true && s.UserId > 62281).ToList();
                foreach (var item in UserList)
                {
                    usermatch.Gender_Demographics = "A";
                    usermatch.IncomeBracket_Demographics = "B";
                    usermatch.WorkingStatus_Demographics = "A";
                    usermatch.RelationshipStatus_Demographics = "E";
                    usermatch.Education_Demographics = "A";
                    usermatch.HouseholdStatus_Demographics = "A";
                    usermatch.Location_Demographics = "A";
                    usermatch.Food_Advert = "C";
                    usermatch.SweetSaltySnacks_Advert = "B";
                    usermatch.AlcoholicDrinks_Advert = "B";
                    usermatch.NonAlcoholicDrinks_Advert = "B";
                    usermatch.Householdproducts_Advert = "B";
                    usermatch.ToiletriesCosmetics_Advert = "B";
                    usermatch.PharmaceuticalChemistsProducts_Advert = "B";
                    usermatch.TobaccoProducts_Advert = "B";
                    usermatch.PetsPetFood_Advert = "B";
                    usermatch.ShoppingRetailClothing_Advert = "B";
                    usermatch.DIYGardening_Advert = "B";
                    usermatch.AppliancesOtherHouseholdDurables_Advert = "B";
                    usermatch.ElectronicsOtherPersonalItems_Advert = "B";
                    usermatch.CommunicationsInternet_Advert = "B";
                    usermatch.FinancialServices_Advert = "B";
                    usermatch.HolidaysTravel_Advert = "B";
                    usermatch.SportsLeisure_Advert = "B";
                    usermatch.Motoring_Advert = "B";
                    usermatch.Newspapers_Advert = "B";
                    usermatch.Magazines_Advert = "B";
                    usermatch.TV_Advert = "B";
                    usermatch.Radio_Advert = "B";
                    usermatch.Cinema_Advert = "B";
                    usermatch.SocialNetworking_Advert = "B";
                    usermatch.GeneralUse_Advert = "B";
                    usermatch.Shopping_Advert = "B";
                    usermatch.Fitness_Advert = "B";
                    usermatch.Holidays_Advert = "B";
                    usermatch.Environment_Advert = "B";
                    usermatch.GoingOut_Advert = "B";
                    usermatch.FinancialProducts_Advert = "B";
                    usermatch.Religion_Advert = "B";
                    usermatch.Fashion_Advert = "B";
                    usermatch.Music_Advert = "B";
                    usermatch.Fitness_Attitude = "B";
                    usermatch.Holidays_Attitude = "B";
                    usermatch.Environment_Attitude = "B";
                    usermatch.GoingOut_Attitude = "B";
                    usermatch.FinancialStabiity_Attitude = "B";
                    usermatch.Religion_Attitude = "B";
                    usermatch.Fashion_Attitude = "B";
                    usermatch.Music_Attitude = "B";



                    usermatch.Cinema_Cinema = "A";
                    usermatch.SocialNetworking_Internet = "A";
                    usermatch.Video_Internet = "A";
                    usermatch.Research_Internet = "A";
                    usermatch.Auctions_Internet = "A";
                    usermatch.Shopping_Internet = "A";


                    usermatch.ContractType_Mobile = "B";
                    usermatch.Spend_Mobile = "C";

                    usermatch.Local_Press = "A";
                    usermatch.National_Press = "A";
                    usermatch.FreeNewpapers_Press = "A";
                    usermatch.Magazines_Press = "A";
                    usermatch.Food_ProductsService = "A";
                    usermatch.SweetSaltySnacks_ProductsService = "A";
                    usermatch.AlcoholicDrinks_ProductsService = "A";
                    usermatch.NonAlcoholicDrinks_ProductsService = "A";
                    usermatch.Householdproducts_ProductsService = "A";
                    usermatch.ToiletriesCosmetics_ProductsService = "A";
                    usermatch.PharmaceuticalChemistsProducts_ProductsService = "A";
                    usermatch.TobaccoProducts_ProductsService = "A";
                    usermatch.PetsPetFood_ProductsService = "A";
                    usermatch.ShoppingRetailClothing_ProductsService = "A";
                    usermatch.DIYGardening_ProductsService = "A";
                    usermatch.AppliancesOtherHouseholdDurables_ProductsService = "A";
                    usermatch.ElectronicsOtherPersonalItems_ProductsService = "A";
                    usermatch.CommunicationsInternet_ProductsService = "A";
                    usermatch.FinancialServices_ProductsService = "A";
                    usermatch.HolidaysTravel_ProductsService = "A";
                    usermatch.SportsLeisure_ProductsService = "A";
                    usermatch.Motoring_ProductsService = "A";
                    usermatch.National_Radio = "A";
                    usermatch.Local_Radio = "A";
                    usermatch.Music_Radio = "A";
                    usermatch.Sport_Radio = "A";
                    usermatch.Talk_Radio = "A";
                    usermatch.Satallite_TV = "A";
                    usermatch.Cable_TV = "A";
                    usermatch.Terrestrial_TV = "A";
                    usermatch.Internet_TV = "A";

                    var UserProfileData = db.Userprofiles.Where(s => s.UserId == item.UserId).FirstOrDefault();

                    usermatch.Email = item.Email;
                    usermatch.MSISDN = UserProfileData.MSISDN;
                    usermatch.MSUserProfileId = UserProfileData.UserProfileId;
                    usermatch.UserId = item.UserId;
                    usermatch.DOB = UserProfileData.DOB;
                    usermatch.Gender = UserProfileData.Gender;
                    usermatch.IncomeBracket = UserProfileData.IncomeBracket;
                    usermatch.WorkingStatus = UserProfileData.WorkingStatus;
                    usermatch.RelationshipStatus = UserProfileData.RelationshipStatus;
                    usermatch.Education = UserProfileData.Education;
                    usermatch.HouseholdStatus = UserProfileData.HouseholdStatus;
                    usermatch.Location = UserProfileData.Location;

                    mySQLEntities.usermatches.Add(usermatch);
                    mySQLEntities.SaveChanges();


                    var usertimesetting = new userprofiletimesetting();
                    usertimesetting.UserProfileId = usermatch.UserProfileId;
                    usertimesetting.Monday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Tuesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Wednesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Thursday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Friday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Saturday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Sunday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    mySQLEntities.userprofiletimesettings.Add(usertimesetting);
                    mySQLEntities.SaveChanges();
                }


            }

            return View();
        }

        public ActionResult AddUserProfileTimeSetting()
        {
            using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
            {
                var usertimesetting = new userprofiletimesetting();
                var UserMatch = mySQLEntities.usermatches.ToList();
                foreach(var item in UserMatch)
                {
                    usertimesetting.UserProfileId = item.UserProfileId;
                    usertimesetting.Monday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Tuesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Wednesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Thursday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Friday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Saturday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    usertimesetting.Sunday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    mySQLEntities.userprofiletimesettings.Add(usertimesetting);
                    mySQLEntities.SaveChanges();
                }
            }
                return View();
        }



        public ActionResult AddCampaignTimeSetting()
        {
            using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
            {
                var campaigntimesetting = new campaignprofiletimesetting();
                var CamaignMatch = mySQLEntities.campaignmatches.ToList();
                foreach (var item in CamaignMatch)
                {
                    campaigntimesetting.MSCampaignProfileId = item.CampaignProfileId;
                    campaigntimesetting.Monday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Tuesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Wednesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Thursday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Friday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Saturday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Sunday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    mySQLEntities.campaignprofiletimesettings.Add(campaigntimesetting);
                    mySQLEntities.SaveChanges();
                }
            }
            return View();
        }


        public ActionResult AddCampaign()
        {
            var LatestCampaign = db.CampaignProfiles.OrderByDescending(s=>s.CampaignProfileId).FirstOrDefault();
            if(LatestCampaign != null)
            {
                for(int i = 7; i <= 30; i++)
                {
                    CampaignProfile obj = new CampaignProfile();
                    obj.UserId = LatestCampaign.UserId;
                    obj.ClientId = LatestCampaign.ClientId;
                    obj.CampaignName = "Tjeep" + i;

                    obj.CampaignDescription = "TP" + i;
                    obj.TotalBudget = LatestCampaign.TotalBudget;
                    obj.MaxDailyBudget = LatestCampaign.MaxDailyBudget;

                    obj.MaxBid = LatestCampaign.MaxBid;
                    obj.MaxMonthBudget = LatestCampaign.MaxMonthBudget;
                    obj.MaxWeeklyBudget = LatestCampaign.MaxWeeklyBudget;
                    obj.MaxHourlyBudget = LatestCampaign.MaxHourlyBudget;

                    obj.TotalCredit = LatestCampaign.TotalCredit;
                    obj.SpendToDate = LatestCampaign.SpendToDate;
                    obj.AvailableCredit = LatestCampaign.AvailableCredit;
                    obj.PlaysToDate = LatestCampaign.PlaysToDate;

                    obj.PlaysLastMonth = LatestCampaign.PlaysLastMonth;
                    obj.PlaysCurrentMonth = LatestCampaign.PlaysCurrentMonth;
                    obj.CancelledToDate = LatestCampaign.CancelledToDate;
                    obj.CancelledLastMonth = LatestCampaign.CancelledLastMonth;


                    obj.CancelledCurrentMonth = LatestCampaign.CancelledCurrentMonth;
                    obj.SmsToDate = LatestCampaign.SmsToDate;
                    obj.SmsLastMonth = LatestCampaign.SmsLastMonth;
                    obj.SmsCurrentMonth = LatestCampaign.SmsCurrentMonth;


                    obj.EmailToDate = LatestCampaign.EmailToDate;
                    obj.EmailsLastMonth = LatestCampaign.EmailsLastMonth;
                    obj.EmailsCurrentMonth = LatestCampaign.EmailsCurrentMonth;
                    obj.EmailFileLocation = LatestCampaign.EmailFileLocation;

                    obj.Active = LatestCampaign.Active;
                    obj.NumberOfPlays = LatestCampaign.NumberOfPlays;
                    obj.AverageDailyPlays = LatestCampaign.AverageDailyPlays;
                    obj.SmsRequests = LatestCampaign.SmsRequests;

                    obj.EmailsDelievered = LatestCampaign.EmailsDelievered;
                    obj.EmailSubject = LatestCampaign.EmailSubject;
                    obj.EmailBody = LatestCampaign.EmailBody;
                    obj.EmailSenderAddress = LatestCampaign.EmailSenderAddress;

                    obj.SmsOriginator = LatestCampaign.SmsOriginator;
                    obj.SmsBody = LatestCampaign.SmsBody;
                    obj.SMSFileLocation = LatestCampaign.SMSFileLocation;
                    obj.CreatedDateTime = LatestCampaign.CreatedDateTime;

                    obj.UpdatedDateTime = LatestCampaign.UpdatedDateTime;
                    obj.Status = 2;
                    obj.StartDate = LatestCampaign.StartDate;
                    obj.EndDate = LatestCampaign.EndDate;
                    obj.NumberInBatch = LatestCampaign.NumberInBatch;

                    db.CampaignProfiles.Add(obj);
                    db.SaveChanges();

                    var campaigntimesetting = new CampaignProfileTimeSetting();
                    campaigntimesetting.CampaignProfileId = obj.CampaignProfileId;
                    campaigntimesetting.Monday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Tuesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Wednesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Thursday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Friday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Saturday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Sunday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    db.CampaignProfileTimeSettings.Add(campaigntimesetting);
                    db.SaveChanges();
                }
            } 
            return View();
        }

        public ActionResult AddCampaignMySql()
        {
            using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
            {
                var AllCampaignList = db.CampaignProfiles.ToList();
                foreach(var item in AllCampaignList)
                {
                    var campaignmatch = new campaignmatch();
                    campaignmatch.CampaignName = item.CampaignName;
                    campaignmatch.CampaignDescription = item.CampaignDescription;
                    campaignmatch.ClientId = item.ClientId;
                    campaignmatch.StartDate = item.StartDate;
                    campaignmatch.EndDate = item.EndDate;
                    campaignmatch.UserId = item.UserId;
                    campaignmatch.NumberInBatch = item.NumberInBatch;
                    campaignmatch.MSCampaignProfileId = item.CampaignProfileId;
                    campaignmatch.Status = item.Status;
                    campaignmatch.Active = item.Active;
                    campaignmatch.AvailableCredit = item.AvailableCredit.ToString();
                    campaignmatch.CancelledCurrentMonth = item.CancelledCurrentMonth;
                    campaignmatch.CancelledLastMonth = item.CancelledLastMonth;
                    campaignmatch.CancelledToDate = item.CancelledToDate;
                    campaignmatch.CreatedDateTime = item.CreatedDateTime;
                    campaignmatch.EmailToDate = item.EmailToDate;
                    campaignmatch.EmailsCurrentMonth = item.EmailsCurrentMonth;
                    campaignmatch.EmailsLastMonth = item.EmailsLastMonth;
                    campaignmatch.MaxBid = Convert.ToInt32(item.MaxBid);
                    campaignmatch.MaxMonthBudget = Convert.ToInt64(item.MaxWeeklyBudget);
                    campaignmatch.MaxWeeklyBudget = Convert.ToInt64(item.MaxWeeklyBudget);
                    campaignmatch.MaxHourlyBudget = Convert.ToInt64(item.MaxHourlyBudget);
                    campaignmatch.TotalCredit = Convert.ToInt64(item.TotalCredit);
                    campaignmatch.SpentToDate = item.SpendToDate.ToString();
                    campaignmatch.MaxDailyBudget = Convert.ToInt64(item.MaxDailyBudget);
                    campaignmatch.PlaysCurrentMonth = item.PlaysCurrentMonth;
                    campaignmatch.PlaysLastMonth = item.PlaysLastMonth;
                    campaignmatch.PlaysToDate = item.PlaysToDate;
                    campaignmatch.SmsCurrentMonth = item.SmsCurrentMonth;
                    campaignmatch.SmsLastMonth = item.SmsLastMonth;
                    campaignmatch.SmsToDate = item.SmsToDate;
                    campaignmatch.TotalBudget = Convert.ToInt64(item.TotalBudget);
                    campaignmatch.UpdatedDateTime = item.UpdatedDateTime;
                    campaignmatch.UserId = item.UserId;
                    campaignmatch.EmailBody = item.EmailBody;
                    campaignmatch.EmailSenderAddress = item.EmailSenderAddress;
                    campaignmatch.EmailSubject = item.EmailSubject;
                    campaignmatch.SmsBody = item.SmsBody;
                    campaignmatch.SmsOriginator = item.SmsOriginator;
                    campaignmatch.EMAIL_MESSAGE = item.EmailBody;
                    campaignmatch.SMS_MESSAGE = item.SmsBody;
                    campaignmatch.ORIGINATOR = item.SmsOriginator;

                    var CampaignPreference = db.CampaignProfilePreference.Where(s => s.CampaignProfileId == item.CampaignProfileId).FirstOrDefault();
                    campaignmatch.Age_Demographics = CampaignPreference.Age_Demographics;
                    campaignmatch.Gender_Demographics = CampaignPreference.Gender_Demographics;
                    campaignmatch.IncomeBracket_Demographics = CampaignPreference.IncomeBracket_Demographics;
                    campaignmatch.WorkingStatus_Demographics = CampaignPreference.WorkingStatus_Demographics;
                    campaignmatch.RelationshipStatus_Demographics = CampaignPreference.RelationshipStatus_Demographics;
                    campaignmatch.Education_Demographics = CampaignPreference.Education_Demographics;
                    campaignmatch.HouseholdStatus_Demographics = CampaignPreference.HouseholdStatus_Demographics;

                    campaignmatch.Food_Advert = CampaignPreference.Food_Advert;
                    campaignmatch.SweetSaltySnacks_Advert = CampaignPreference.SweetSaltySnacks_Advert;
                    campaignmatch.AlcoholicDrinks_Advert = CampaignPreference.AlcoholicDrinks_Advert;
                    campaignmatch.NonAlcoholicDrinks_Advert = CampaignPreference.NonAlcoholicDrinks_Advert;
                    campaignmatch.Householdproducts_Advert = CampaignPreference.Householdproducts_Advert;
                    campaignmatch.ToiletriesCosmetics_Advert = CampaignPreference.ToiletriesCosmetics_Advert;
                    campaignmatch.PharmaceuticalChemistsProducts_Advert = CampaignPreference.PharmaceuticalChemistsProducts_Advert;
                    campaignmatch.TobaccoProducts_Advert = CampaignPreference.TobaccoProducts_Advert;
                    campaignmatch.PetsPetFood_Advert = CampaignPreference.PetsPetFood_Advert;
                    campaignmatch.ShoppingRetailClothing_Advert = CampaignPreference.ShoppingRetailClothing_Advert;
                    campaignmatch.DIYGardening_Advert = CampaignPreference.DIYGardening_Advert;
                    campaignmatch.AppliancesOtherHouseholdDurables_Advert = CampaignPreference.AppliancesOtherHouseholdDurables_Advert;
                    campaignmatch.ElectronicsOtherPersonalItems_Advert = CampaignPreference.ElectronicsOtherPersonalItems_Advert;
                    campaignmatch.CommunicationsInternet_Advert = CampaignPreference.CommunicationsInternet_Advert;
                    campaignmatch.FinancialServices_Advert = CampaignPreference.FinancialServices_Advert;
                    campaignmatch.HolidaysTravel_Advert = CampaignPreference.HolidaysTravel_Advert;
                    campaignmatch.SportsLeisure_Advert = CampaignPreference.SportsLeisure_Advert;
                    campaignmatch.Motoring_Advert = CampaignPreference.Motoring_Advert;
                    campaignmatch.Newspapers_Advert = CampaignPreference.Newspapers_Advert;
                    campaignmatch.Magazines_Advert = CampaignPreference.Magazines_Advert;
                    campaignmatch.TV_Advert = CampaignPreference.TV_Advert;
                    campaignmatch.Radio_Advert = CampaignPreference.Radio_Advert;
                    campaignmatch.Cinema_Advert = CampaignPreference.Cinema_Advert;
                    campaignmatch.SocialNetworking_Advert = CampaignPreference.SocialNetworking_Advert;
                    campaignmatch.GeneralUse_Advert = CampaignPreference.GeneralUse_Advert;
                    campaignmatch.Shopping_Advert = CampaignPreference.Shopping_Advert;
                    campaignmatch.Fitness_Advert = CampaignPreference.Fitness_Advert;
                    campaignmatch.Holidays_Advert = CampaignPreference.Holidays_Advert;
                    campaignmatch.Environment_Advert = CampaignPreference.Environment_Advert;
                    campaignmatch.GoingOut_Advert = CampaignPreference.GoingOut_Advert;
                    campaignmatch.FinancialProducts_Advert = CampaignPreference.FinancialProducts_Advert;
                    campaignmatch.Religion_Advert = CampaignPreference.Religion_Advert;
                    campaignmatch.Fashion_Advert = CampaignPreference.Fashion_Advert;
                    campaignmatch.Music_Advert = CampaignPreference.Music_Advert;

                    campaignmatch.Fitness_Attitude = CampaignPreference.Fitness_Attitude;
                    campaignmatch.Holidays_Attitude = CampaignPreference.Holidays_Attitude;
                    campaignmatch.Environment_Attitude = CampaignPreference.Environment_Attitude;
                    campaignmatch.GoingOut_Attitude = CampaignPreference.GoingOut_Attitude;
                    campaignmatch.FinancialStabiity_Attitude = CampaignPreference.FinancialStabiity_Attitude;
                    campaignmatch.Religion_Attitude = CampaignPreference.Religion_Attitude;
                    campaignmatch.Fashion_Attitude = CampaignPreference.Fashion_Attitude;
                    campaignmatch.Music_Attitude = CampaignPreference.Music_Attitude;
                    campaignmatch.MSCampaignProfileId = CampaignPreference.CampaignProfileId;

                    campaignmatch.Cinema_Cinema = CampaignPreference.Cinema_Cinema;

                    campaignmatch.SocialNetworking_Internet = CampaignPreference.SocialNetworking_Internet;
                    campaignmatch.Video_Internet = CampaignPreference.Video_Internet;
                    campaignmatch.Research_Internet = CampaignPreference.Research_Internet;
                    campaignmatch.Auctions_Internet = CampaignPreference.Auctions_Internet;
                    campaignmatch.Shopping_Internet = CampaignPreference.Shopping_Internet;

                    campaignmatch.Local_Press = CampaignPreference.Local_Press;
                    campaignmatch.National_Press = CampaignPreference.National_Press;
                    campaignmatch.FreeNewpapers_Press = CampaignPreference.FreeNewpapers_Press;
                    campaignmatch.Magazines_Press = CampaignPreference.Magazines_Press;


                    campaignmatch.Food_ProductsService = CampaignPreference.Food_ProductsService;
                    campaignmatch.SweetSaltySnacks_ProductsService = CampaignPreference.SweetSaltySnacks_ProductsService;
                    campaignmatch.AlcoholicDrinks_ProductsService = CampaignPreference.AlcoholicDrinks_ProductsService;
                    campaignmatch.NonAlcoholicDrinks_ProductsService = CampaignPreference.NonAlcoholicDrinks_ProductsService;
                    campaignmatch.Householdproducts_ProductsService = CampaignPreference.Householdproducts_ProductsService;
                    campaignmatch.ToiletriesCosmetics_ProductsService = CampaignPreference.ToiletriesCosmetics_ProductsService;
                    campaignmatch.PharmaceuticalChemistsProducts_ProductsService = CampaignPreference.PharmaceuticalChemistsProducts_ProductsService;
                    campaignmatch.TobaccoProducts_ProductsService = CampaignPreference.TobaccoProducts_ProductsService;
                    campaignmatch.PetsPetFood_ProductsService = CampaignPreference.PetsPetFood_ProductsService;
                    campaignmatch.ShoppingRetailClothing_ProductsService = CampaignPreference.ShoppingRetailClothing_ProductsService;
                    campaignmatch.DIYGardening_ProductsService = CampaignPreference.DIYGardening_ProductsService;
                    campaignmatch.AppliancesOtherHouseholdDurables_ProductsService = CampaignPreference.AppliancesOtherHouseholdDurables_ProductsService;
                    campaignmatch.ElectronicsOtherPersonalItems_ProductsService = CampaignPreference.ElectronicsOtherPersonalItems_ProductsService;
                    campaignmatch.CommunicationsInternet_ProductsService = CampaignPreference.CommunicationsInternet_ProductsService;
                    campaignmatch.FinancialServices_ProductsService = CampaignPreference.FinancialServices_ProductsService;
                    campaignmatch.HolidaysTravel_ProductsService = CampaignPreference.HolidaysTravel_ProductsService;
                    campaignmatch.SportsLeisure_ProductsService = CampaignPreference.SportsLeisure_ProductsService;
                    campaignmatch.Motoring_ProductsService = CampaignPreference.Motoring_ProductsService;

                    campaignmatch.National_Radio = CampaignPreference.National_Radio;
                    campaignmatch.Local_Radio = CampaignPreference.Local_Radio;
                    campaignmatch.Music_Radio = CampaignPreference.Music_Radio;
                    campaignmatch.Sport_Radio = CampaignPreference.Sport_Radio;
                    campaignmatch.Talk_Radio = CampaignPreference.Talk_Radio;

                    campaignmatch.Satallite_TV = CampaignPreference.Satallite_TV;
                    campaignmatch.Cable_TV = CampaignPreference.Cable_TV;
                    campaignmatch.Terrestrial_TV = CampaignPreference.Terrestrial_TV;
                    campaignmatch.Internet_TV = CampaignPreference.Internet_TV;

                    campaignmatch.ContractType_Mobile = CampaignPreference.ContractType_Mobile;
                    campaignmatch.Spend_Mobile = CampaignPreference.Spend_Mobile;

                    mySQLEntities.campaignmatches.Add(campaignmatch);
                    mySQLEntities.SaveChanges();

                    var campaigntimesetting = new campaignprofiletimesetting();
                    campaigntimesetting.MSCampaignProfileId = campaignmatch.CampaignProfileId;
                    campaigntimesetting.Monday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Tuesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Wednesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Thursday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Friday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Saturday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    campaigntimesetting.Sunday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                    mySQLEntities.campaignprofiletimesettings.Add(campaigntimesetting);
                    mySQLEntities.SaveChanges();
                } 
            }
                return View();
        }

        /* User Insert
         DECLARE @UsId int;
DECLARE @UsProfId int;
DECLARE @site_value INT;
SET @site_value = 0;

DECLARE @numbers bigint; 
SET @numbers = 440000001012;

DECLARE @Email varchar(max); 


WHILE @site_value <= 10
BEGIN
set @numbers = @numbers + 1;
set  @Email = CONVERT(varchar(max), @numbers) + '@email.com';

INSERT [dbo].[Users] ( [Email], [FirstName], [LastName], [PasswordHash], [DateCreated], [Organisation], [LastLoginTime], [Activated], [RoleId], [VerificationStatus], [Outstandingdays], [OperatorId]) VALUES ( @Email, 'Test-' + CONVERT(varchar(max), @numbers), 'Last-' + CONVERT(varchar(max), @numbers), N'�r!nk�2|}�xO�', CAST(N'2017-10-03 07:23:33.233' AS DateTime), N'Adtones', CAST(N'2017-10-03 07:23:33.233' AS DateTime), 1, 2, 1, 0, 0);

set @UsId = scope_identity();

INSERT [dbo].[UserProfile] ([UserId], [DOB], [Gender], [IncomeBracket], [WorkingStatus], [RelationshipStatus], [Education], [HouseholdStatus], [Location], [MSISDN]) VALUES ( @UsId, NULL, N'A', N'F', N'A', N'E', N'D', N'B', N'A', CONVERT(varchar(max), @numbers));
set @UsProfId = scope_identity();

INSERT [dbo].[UserProfilePreference] ([UserProfileId], [Gender_Demographics], [IncomeBracket_Demographics], [WorkingStatus_Demographics], [RelationshipStatus_Demographics], [Education_Demographics], [HouseholdStatus_Demographics], [Location_Demographics], [Food_Advert], [SweetSaltySnacks_Advert], [AlcoholicDrinks_Advert], [NonAlcoholicDrinks_Advert], [Householdproducts_Advert], [ToiletriesCosmetics_Advert], [PharmaceuticalChemistsProducts_Advert], [TobaccoProducts_Advert], [PetsPetFood_Advert], [ShoppingRetailClothing_Advert], [DIYGardening_Advert], [AppliancesOtherHouseholdDurables_Advert], [ElectronicsOtherPersonalItems_Advert], [CommunicationsInternet_Advert], [FinancialServices_Advert], [HolidaysTravel_Advert], [SportsLeisure_Advert], [Motoring_Advert], [Newspapers_Advert], [Magazines_Advert], [TV_Advert], [Radio_Advert], [Cinema_Advert], [SocialNetworking_Advert], [GeneralUse_Advert], [Shopping_Advert], [Fitness_Advert], [Holidays_Advert], [Environment_Advert], [GoingOut_Advert], [FinancialProducts_Advert], [Religion_Advert], [Fashion_Advert], [Music_Advert], [Fitness_Attitude], [Holidays_Attitude], [Environment_Attitude], [GoingOut_Attitude], [FinancialStabiity_Attitude], [Religion_Attitude], [Fashion_Attitude], [Music_Attitude], [Cinema_Cinema], [SocialNetworking_Internet], [Video_Internet], [Research_Internet], [Auctions_Internet], [Shopping_Internet], [ContractType_Mobile], [Spend_Mobile], [Local_Press], [National_Press], [FreeNewpapers_Press], [Magazines_Press], [Food_ProductsService], [SweetSaltySnacks_ProductsService], [AlcoholicDrinks_ProductsService], [NonAlcoholicDrinks_ProductsService], [Householdproducts_ProductsService], [ToiletriesCosmetics_ProductsService], [PharmaceuticalChemistsProducts_ProductsService], [TobaccoProducts_ProductsService], [PetsPetFood_ProductsService], [ShoppingRetailClothing_ProductsService], [DIYGardening_ProductsService], [AppliancesOtherHouseholdDurables_ProductsService], [ElectronicsOtherPersonalItems_ProductsService], [CommunicationsInternet_ProductsService], [FinancialServices_ProductsService], [HolidaysTravel_ProductsService], [SportsLeisure_ProductsService], [Motoring_ProductsService], [National_Radio], [Local_Radio], [Music_Radio], [Sport_Radio], [Talk_Radio], [Satallite_TV], [Cable_TV], [Terrestrial_TV], [Internet_TV], [Postcode]) VALUES ( @UsProfId, N'A', N'B', N'A', N'E', N'A', N'A', N'A', N'B', N'B', N'B', N'B', N'C', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'B', N'A', N'A', N'A', N'A', N'A', N'A', N'B', N'C', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', N'A', NULL);


INSERT [dbo].[UserProfileTimeSetting] ([UserProfileId], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Sunday]) VALUES (@UsProfId, N'01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00', N'01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00', N'01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00', N'01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00', N'01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00', N'01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00', N'01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00');

SET @site_value = @site_value + 1;
END;
         */
        /*
        ---- MySql---

INSERT INTO ADTONES...[usermatch]  ('Gender_Demographics','IncomeBracket_Demographics','WorkingStatus_Demographics','RelationshipStatus_Demographics','Education_Demographics','HouseholdStatus_Demographics','Location_Demographics','Food_Advert','SweetSaltySnacks_Advert','AlcoholicDrinks_Advert','NonAlcoholicDrinks_Advert','Householdproducts_Advert','ToiletriesCosmetics_Advert','PharmaceuticalChemistsProducts_Advert','TobaccoProducts_Advert','PetsPetFood_Advert','ShoppingRetailClothing_Advert','DIYGardening_Advert',  'AppliancesOtherHouseholdDurables_Advert','ElectronicsOtherPersonalItems_Advert','CommunicationsInternet_Advert','FinancialServices_Advert','HolidaysTravel_Advert',      'SportsLeisure_Advert','Motoring_Advert','Newspapers_Advert','Magazines_Advert','TV_Advert','Radio_Advert','Cinema_Advert','SocialNetworking_Advert','GeneralUse_Advert',             'Shopping_Advert','Fitness_Advert','Holidays_Advert','Environment_Advert','GoingOut_Advert','FinancialProducts_Advert','Religion_Advert','Fashion_Advert','Music_Advert',             'Fitness_Attitude','Holidays_Attitude','Environment_Attitude','GoingOut_Attitude','FinancialStabiity_Attitude','Religion_Attitude','Fashion_Attitude','Music_Attitude',             'Cinema_Cinema','SocialNetworking_Internet','Video_Internet','Research_Internet','Auctions_Internet','Shopping_Internet','ContractType_Mobile','Spend_Mobile','Local_Press',          'National_Press','FreeNewpapers_Press','Magazines_Press','Food_ProductsService','SweetSaltySnacks_ProductsService','AlcoholicDrinks_ProductsService',        'NonAlcoholicDrinks_ProductsService','Householdproducts_ProductsService','ToiletriesCosmetics_ProductsService','PharmaceuticalChemistsProducts_ProductsService',           'TobaccoProducts_ProductsService','PetsPetFood_ProductsService','ShoppingRetailClothing_ProductsService','DIYGardening_ProductsService',       'AppliancesOtherHouseholdDurables_ProductsService','ElectronicsOtherPersonalItems_ProductsService','CommunicationsInternet_ProductsService','FinancialServices_ProductsService',   'HolidaysTravel_ProductsService','SportsLeisure_ProductsService','Motoring_ProductsService','National_Radio','Local_Radio','Music_Radio','Sport_Radio','Talk_Radio',            'Satallite_TV','Cable_TV','Terrestrial_TV','Internet_TV','Email','MSISDN','MSUserProfileId','UserId','DOB','Gender','IncomeBracket','WorkingStatus','RelationshipStatus',            'Education','HouseholdStatus','Location','Age_Demographics')

SELECT 'A','B','A','E','A','A','A','C','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','B','A','A','A','A','A','A','B','C','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A','A',Email,MSISDN,UserProfileId,UserId,NULL,'A','F','A','E','D','B','A',NULL FROM
(
   SELECT u.UserId, u.Email,up.MSISDN, up.UserProfileId, up.DOB,  up.Gender, up.IncomeBracket, up.WorkingStatus, up.RelationshipStatus, up.Education, up.HouseholdStatus, up.Location
   FROM 
   [AdtoneTest].[dbo].[Users] u,
   [AdtoneTest].[dbo].[UserProfile] up
   where u.UserId = up.UserId and u.RoleId = 2 and u.Activated = 1 and u.VerificationStatus = 'true' and u.UserId > 64648	and u.UserId < 	64650
) as vs 




Msg 215, Level 16, State 1, Line 1
Parameters supplied for object 'ADTONES...usermatch' which is not a function. If the parameters are intended as a table hint, a WITH keyword is required.


insert into [LINKED_SERVER].DB.SCHEMA.TABLE
select * from TABLE  



        */

    }
}