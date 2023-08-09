using EFMVC.Data;
using EFMVC.Model;
using EFMVC.Model.Entities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace EFMVC.Web.Controllers
{
    public class ImportUserCSVController : Controller
    {
        EFMVCDataContex db = new EFMVCDataContex();
       
        public ActionResult Index()
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/Safaricom/"));
            var allFiles = directory.GetFiles().ToList();
            foreach (var item in allFiles)
            {

                var fileName = System.IO.Path.GetFileName(item.FullName);
                var path = System.IO.Path.Combine(Server.MapPath("~/Safaricom/"), fileName);

                StreamReader sr = new StreamReader(path);

                //DataTable userDt = new DataTable();
                //userDt.Columns.Add("Email");
                //userDt.Columns.Add("FirstName");
                //userDt.Columns.Add("LastName");
                //userDt.Columns.Add("PasswordHash");
                //userDt.Columns.Add("DateCreated");
                //userDt.Columns.Add("Organisation");
                //userDt.Columns.Add("LastLoginTime");
                //userDt.Columns.Add("Activated");
                //userDt.Columns.Add("RoleId");
                //userDt.Columns.Add("VerificationStatus");
                //userDt.Columns.Add("Outstandingdays");
                //userDt.Columns.Add("OperatorId");


                //DataTable userProfileDt = new DataTable();
                //userProfileDt.Columns.Add("UserId");
                //userProfileDt.Columns.Add("DOB");
                //userProfileDt.Columns.Add("Gender");
                //userProfileDt.Columns.Add("IncomeBracket");
                //userProfileDt.Columns.Add("WorkingStatus");
                //userProfileDt.Columns.Add("RelationshipStatus");
                //userProfileDt.Columns.Add("Education");
                //userProfileDt.Columns.Add("HouseholdStatus");
                //userProfileDt.Columns.Add("Location");
                //userProfileDt.Columns.Add("MSISDN");

                DataTable userProfilePrefeDt = new DataTable();
                userProfilePrefeDt.Columns.Add("UserProfileId");
                userProfilePrefeDt.Columns.Add("Gender_Demographics");
                userProfilePrefeDt.Columns.Add("IncomeBracket_Demographics");
                userProfilePrefeDt.Columns.Add("WorkingStatus_Demographics");
                userProfilePrefeDt.Columns.Add("RelationshipStatus_Demographics");
                userProfilePrefeDt.Columns.Add("Education_Demographics");
                userProfilePrefeDt.Columns.Add("HouseholdStatus_Demographics");
                userProfilePrefeDt.Columns.Add("Location_Demographics");
                userProfilePrefeDt.Columns.Add("Food_Advert");
                userProfilePrefeDt.Columns.Add("SweetSaltySnacks_Advert");
                userProfilePrefeDt.Columns.Add("AlcoholicDrinks_Advert");
                userProfilePrefeDt.Columns.Add("NonAlcoholicDrinks_Advert");
                userProfilePrefeDt.Columns.Add("Householdproducts_Advert");
                userProfilePrefeDt.Columns.Add("ToiletriesCosmetics_Advert");
                userProfilePrefeDt.Columns.Add("PharmaceuticalChemistsProducts_Advert");
                userProfilePrefeDt.Columns.Add("TobaccoProducts_Advert");
                userProfilePrefeDt.Columns.Add("PetsPetFood_Advert");
                userProfilePrefeDt.Columns.Add("ShoppingRetailClothing_Advert");
                userProfilePrefeDt.Columns.Add("DIYGardening_Advert");
                userProfilePrefeDt.Columns.Add("AppliancesOtherHouseholdDurables_Advert");
                userProfilePrefeDt.Columns.Add("ElectronicsOtherPersonalItems_Advert");
                userProfilePrefeDt.Columns.Add("CommunicationsInternet_Advert");
                userProfilePrefeDt.Columns.Add("FinancialServices_Advert");
                userProfilePrefeDt.Columns.Add("HolidaysTravel_Advert");
                userProfilePrefeDt.Columns.Add("SportsLeisure_Advert");
                userProfilePrefeDt.Columns.Add("Motoring_Advert");
                userProfilePrefeDt.Columns.Add("Newspapers_Advert");
                userProfilePrefeDt.Columns.Add("Magazines_Advert");
                userProfilePrefeDt.Columns.Add("TV_Advert");
                userProfilePrefeDt.Columns.Add("Radio_Advert");
                userProfilePrefeDt.Columns.Add("Cinema_Advert");
                userProfilePrefeDt.Columns.Add("SocialNetworking_Advert");
                userProfilePrefeDt.Columns.Add("GeneralUse_Advert");
                userProfilePrefeDt.Columns.Add("Shopping_Advert");
                userProfilePrefeDt.Columns.Add("Fitness_Advert");
                userProfilePrefeDt.Columns.Add("Holidays_Advert");
                userProfilePrefeDt.Columns.Add("Environment_Advert");
                userProfilePrefeDt.Columns.Add("GoingOut_Advert");
                userProfilePrefeDt.Columns.Add("FinancialProducts_Advert");
                userProfilePrefeDt.Columns.Add("Religion_Advert");
                userProfilePrefeDt.Columns.Add("Fashion_Advert");
                userProfilePrefeDt.Columns.Add("Music_Advert");
                userProfilePrefeDt.Columns.Add("Fitness_Attitude");
                userProfilePrefeDt.Columns.Add("Holidays_Attitude");
                userProfilePrefeDt.Columns.Add("Environment_Attitude");
                userProfilePrefeDt.Columns.Add("GoingOut_Attitude");
                userProfilePrefeDt.Columns.Add("FinancialStabiity_Attitude");
                userProfilePrefeDt.Columns.Add("Religion_Attitude");
                userProfilePrefeDt.Columns.Add("Fashion_Attitude");
                userProfilePrefeDt.Columns.Add("Music_Attitude");
                userProfilePrefeDt.Columns.Add("Cinema_Cinema");
                userProfilePrefeDt.Columns.Add("SocialNetworking_Internet");
                userProfilePrefeDt.Columns.Add("Video_Internet");
                userProfilePrefeDt.Columns.Add("Research_Internet");
                userProfilePrefeDt.Columns.Add("Auctions_Internet");
                userProfilePrefeDt.Columns.Add("Shopping_Internet");
                userProfilePrefeDt.Columns.Add("ContractType_Mobile");
                userProfilePrefeDt.Columns.Add("Spend_Mobile");
                userProfilePrefeDt.Columns.Add("Local_Press");
                userProfilePrefeDt.Columns.Add("National_Press");
                userProfilePrefeDt.Columns.Add("FreeNewpapers_Press");
                userProfilePrefeDt.Columns.Add("Magazines_Press");
                userProfilePrefeDt.Columns.Add("Food_ProductsService");
                userProfilePrefeDt.Columns.Add("SweetSaltySnacks_ProductsService");
                userProfilePrefeDt.Columns.Add("AlcoholicDrinks_ProductsService");
                userProfilePrefeDt.Columns.Add("NonAlcoholicDrinks_ProductsService");
                userProfilePrefeDt.Columns.Add("Householdproducts_ProductsService");
                userProfilePrefeDt.Columns.Add("ToiletriesCosmetics_ProductsService");
                userProfilePrefeDt.Columns.Add("PharmaceuticalChemistsProducts_ProductsService");
                userProfilePrefeDt.Columns.Add("TobaccoProducts_ProductsService");
                userProfilePrefeDt.Columns.Add("PetsPetFood_ProductsService");
                userProfilePrefeDt.Columns.Add("ShoppingRetailClothing_ProductsService");
                userProfilePrefeDt.Columns.Add("DIYGardening_ProductsService");
                userProfilePrefeDt.Columns.Add("AppliancesOtherHouseholdDurables_ProductsService");
                userProfilePrefeDt.Columns.Add("ElectronicsOtherPersonalItems_ProductsService");
                userProfilePrefeDt.Columns.Add("CommunicationsInternet_ProductsService");
                userProfilePrefeDt.Columns.Add("FinancialServices_ProductsService");
                userProfilePrefeDt.Columns.Add("HolidaysTravel_ProductsService");
                userProfilePrefeDt.Columns.Add("SportsLeisure_ProductsService");
                userProfilePrefeDt.Columns.Add("Motoring_ProductsService");
                userProfilePrefeDt.Columns.Add("National_Radio");
                userProfilePrefeDt.Columns.Add("Local_Radio");
                userProfilePrefeDt.Columns.Add("Music_Radio");
                userProfilePrefeDt.Columns.Add("Sport_Radio");
                userProfilePrefeDt.Columns.Add("Talk_Radio");
                userProfilePrefeDt.Columns.Add("Satallite_TV");
                userProfilePrefeDt.Columns.Add("Cable_TV");
                userProfilePrefeDt.Columns.Add("Terrestrial_TV");
                userProfilePrefeDt.Columns.Add("Internet_TV");

                userProfilePrefeDt.Columns.Add("Postcode");
                userProfilePrefeDt.Columns.Add("BusinessOrOpportunities_AdType");
                userProfilePrefeDt.Columns.Add("Gambling_AdType");
                userProfilePrefeDt.Columns.Add("Restaurants_AdType");
                userProfilePrefeDt.Columns.Add("Insurance_AdType");
                userProfilePrefeDt.Columns.Add("Furniture_AdType");
                userProfilePrefeDt.Columns.Add("InformationTechnology_AdType");
                userProfilePrefeDt.Columns.Add("Energy_AdType");
                userProfilePrefeDt.Columns.Add("Supermarkets_AdType");
                userProfilePrefeDt.Columns.Add("Healthcare_AdType");
                userProfilePrefeDt.Columns.Add("JobsAndEducation_AdType");
                userProfilePrefeDt.Columns.Add("Gifts_AdType");
                userProfilePrefeDt.Columns.Add("AdvocacyOrLegal_AdType");
                userProfilePrefeDt.Columns.Add("DatingAndPersonal_AdType");
                userProfilePrefeDt.Columns.Add("RealEstate_AdType");
                userProfilePrefeDt.Columns.Add("Games_AdType");
                userProfilePrefeDt.Columns.Add("Hustlers_AdType");
                userProfilePrefeDt.Columns.Add("Youth_AdType");
                userProfilePrefeDt.Columns.Add("DiscerningProfessionals_AdType");
                userProfilePrefeDt.Columns.Add("Mass_AdType");

                #region UserMatch Column
                DataTable userMatchDt = new DataTable();
                userMatchDt.Columns.Add("Gender_Demographics");
                userMatchDt.Columns.Add("IncomeBracket_Demographics");
                userMatchDt.Columns.Add("WorkingStatus_Demographics");
                userMatchDt.Columns.Add("RelationshipStatus_Demographics");
                userMatchDt.Columns.Add("Education_Demographics");
                userMatchDt.Columns.Add("HouseholdStatus_Demographics");
                userMatchDt.Columns.Add("Location_Demographics");
                userMatchDt.Columns.Add("Food_Advert");
                userMatchDt.Columns.Add("SweetSaltySnacks_Advert");
                userMatchDt.Columns.Add("AlcoholicDrinks_Advert");
                userMatchDt.Columns.Add("NonAlcoholicDrinks_Advert");
                userMatchDt.Columns.Add("Householdproducts_Advert");
                userMatchDt.Columns.Add("ToiletriesCosmetics_Advert");
                userMatchDt.Columns.Add("PharmaceuticalChemistsProducts_Advert");
                userMatchDt.Columns.Add("TobaccoProducts_Advert");
                userMatchDt.Columns.Add("PetsPetFood_Advert");
                userMatchDt.Columns.Add("ShoppingRetailClothing_Advert");
                userMatchDt.Columns.Add("DIYGardening_Advert");
                userMatchDt.Columns.Add("AppliancesOtherHouseholdDurables_Advert");
                userMatchDt.Columns.Add("ElectronicsOtherPersonalItems_Advert");
                userMatchDt.Columns.Add("CommunicationsInternet_Advert");
                userMatchDt.Columns.Add("FinancialServices_Advert");
                userMatchDt.Columns.Add("HolidaysTravel_Advert");
                userMatchDt.Columns.Add("SportsLeisure_Advert");
                userMatchDt.Columns.Add("Motoring_Advert");
                userMatchDt.Columns.Add("Newspapers_Advert");
                userMatchDt.Columns.Add("Magazines_Advert");
                userMatchDt.Columns.Add("TV_Advert");
                userMatchDt.Columns.Add("Radio_Advert");
                userMatchDt.Columns.Add("Cinema_Advert");
                userMatchDt.Columns.Add("SocialNetworking_Advert");
                userMatchDt.Columns.Add("GeneralUse_Advert");
                userMatchDt.Columns.Add("Shopping_Advert");
                userMatchDt.Columns.Add("Fitness_Advert");
                userMatchDt.Columns.Add("Holidays_Advert");
                userMatchDt.Columns.Add("Environment_Advert");
                userMatchDt.Columns.Add("GoingOut_Advert");
                userMatchDt.Columns.Add("FinancialProducts_Advert");
                userMatchDt.Columns.Add("Religion_Advert");
                userMatchDt.Columns.Add("Fashion_Advert");
                userMatchDt.Columns.Add("Music_Advert");
                userMatchDt.Columns.Add("Fitness_Attitude");
                userMatchDt.Columns.Add("Holidays_Attitude");
                userMatchDt.Columns.Add("Environment_Attitude");
                userMatchDt.Columns.Add("GoingOut_Attitude");
                userMatchDt.Columns.Add("FinancialStabiity_Attitude");
                userMatchDt.Columns.Add("Religion_Attitude");
                userMatchDt.Columns.Add("Fashion_Attitude");
                userMatchDt.Columns.Add("Music_Attitude");
                userMatchDt.Columns.Add("Cinema_Cinema");
                userMatchDt.Columns.Add("SocialNetworking_Internet");
                userMatchDt.Columns.Add("Video_Internet");
                userMatchDt.Columns.Add("Research_Internet");
                userMatchDt.Columns.Add("Auctions_Internet");
                userMatchDt.Columns.Add("Shopping_Internet");
                userMatchDt.Columns.Add("ContractType_Mobile");
                userMatchDt.Columns.Add("Spend_Mobile");
                userMatchDt.Columns.Add("Local_Press");
                userMatchDt.Columns.Add("National_Press");
                userMatchDt.Columns.Add("FreeNewpapers_Press");
                userMatchDt.Columns.Add("Magazines_Press");
                userMatchDt.Columns.Add("Food_ProductsService");
                userMatchDt.Columns.Add("SweetSaltySnacks_ProductsService");
                userMatchDt.Columns.Add("AlcoholicDrinks_ProductsService");
                userMatchDt.Columns.Add("NonAlcoholicDrinks_ProductsService");
                userMatchDt.Columns.Add("Householdproducts_ProductsService");
                userMatchDt.Columns.Add("ToiletriesCosmetics_ProductsService");
                userMatchDt.Columns.Add("PharmaceuticalChemistsProducts_ProductsService");
                userMatchDt.Columns.Add("TobaccoProducts_ProductsService");
                userMatchDt.Columns.Add("PetsPetFood_ProductsService");
                userMatchDt.Columns.Add("ShoppingRetailClothing_ProductsService");
                userMatchDt.Columns.Add("DIYGardening_ProductsService");
                userMatchDt.Columns.Add("AppliancesOtherHouseholdDurables_ProductsService");
                userMatchDt.Columns.Add("ElectronicsOtherPersonalItems_ProductsService");
                userMatchDt.Columns.Add("CommunicationsInternet_ProductsService");
                userMatchDt.Columns.Add("FinancialServices_ProductsService");
                userMatchDt.Columns.Add("HolidaysTravel_ProductsService");
                userMatchDt.Columns.Add("SportsLeisure_ProductsService");
                userMatchDt.Columns.Add("Motoring_ProductsService");
                userMatchDt.Columns.Add("National_Radio");
                userMatchDt.Columns.Add("Local_Radio");
                userMatchDt.Columns.Add("Music_Radio");
                userMatchDt.Columns.Add("Sport_Radio");
                userMatchDt.Columns.Add("Talk_Radio");
                userMatchDt.Columns.Add("Satallite_TV");
                userMatchDt.Columns.Add("Cable_TV");
                userMatchDt.Columns.Add("Terrestrial_TV");
                userMatchDt.Columns.Add("Internet_TV");
                userMatchDt.Columns.Add("Email");
                userMatchDt.Columns.Add("MSISDN");
                userMatchDt.Columns.Add("MSUserProfileId");
                userMatchDt.Columns.Add("UserId");
                userMatchDt.Columns.Add("DOB");
                userMatchDt.Columns.Add("Gender");
                userMatchDt.Columns.Add("IncomeBracket");
                userMatchDt.Columns.Add("WorkingStatus");
                userMatchDt.Columns.Add("RelationshipStatus");
                userMatchDt.Columns.Add("Education");
                userMatchDt.Columns.Add("HouseholdStatus");
                userMatchDt.Columns.Add("Location");
                userMatchDt.Columns.Add("Age_Demographics");
                userMatchDt.Columns.Add("BusinessOrOpportunities_AdType");
                userMatchDt.Columns.Add("Gambling_AdType");
                userMatchDt.Columns.Add("Restaurants_AdType");
                userMatchDt.Columns.Add("Insurance_AdType");
                userMatchDt.Columns.Add("Furniture_AdType");
                userMatchDt.Columns.Add("InformationTechnology_AdType");
                userMatchDt.Columns.Add("Energy_AdType");
                userMatchDt.Columns.Add("Supermarkets_AdType");
                userMatchDt.Columns.Add("Healthcare_AdType");
                userMatchDt.Columns.Add("JobsAndEducation_AdType");
                userMatchDt.Columns.Add("Gifts_AdType");
                userMatchDt.Columns.Add("AdvocacyOrLegal_AdType");
                userMatchDt.Columns.Add("DatingAndPersonal_AdType");
                userMatchDt.Columns.Add("RealEstate_AdType");
                userMatchDt.Columns.Add("Games_AdType");
                userMatchDt.Columns.Add("Hustlers_AdType");
                userMatchDt.Columns.Add("Youth_AdType");
                userMatchDt.Columns.Add("DiscerningProfessionals_AdType");
                userMatchDt.Columns.Add("Mass_AdType");
                #endregion

                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    try
                    {

                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] rowsArray = line.Split(';');
                            string FirstValue = rowsArray[0];
                            if (FirstValue.ToLower() != "header" && FirstValue.ToLower() != "trailer")
                            {

                                if (rowsArray[1].ToUpper() == "E")
                                {

                                    CSVRecordsOperation(rowsArray, "Add", userProfilePrefeDt, userMatchDt);
                                    TempData["success"] = "Record inserted successfully.";
                                }
                                else if (rowsArray[1].ToUpper() == "D")
                                {
                                    CSVRecordsOperation(rowsArray, "Delete", userProfilePrefeDt, userMatchDt);
                                    TempData["success"] = "Record deleted successfully.";
                                }
                                else if (rowsArray[1].ToUpper() == "P")
                                {
                                    CSVRecordsOperation(rowsArray, "Update", userProfilePrefeDt, userMatchDt);
                                    TempData["success"] = "Record updated successfully.";
                                }

                            }
                        }


                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg);
                    }

                }

                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = "dbo.UserProfilePreference";

                        sqlBulkCopy.ColumnMappings.Add("UserProfileId", "UserProfileId");
                        sqlBulkCopy.ColumnMappings.Add("Gender_Demographics", "Gender_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("IncomeBracket_Demographics", "IncomeBracket_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("WorkingStatus_Demographics", "WorkingStatus_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("RelationshipStatus_Demographics", "RelationshipStatus_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("Education_Demographics", "Education_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("HouseholdStatus_Demographics", "HouseholdStatus_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("Location_Demographics", "Location_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("Food_Advert", "Food_Advert");
                        sqlBulkCopy.ColumnMappings.Add("SweetSaltySnacks_Advert", "SweetSaltySnacks_Advert");
                        sqlBulkCopy.ColumnMappings.Add("AlcoholicDrinks_Advert", "AlcoholicDrinks_Advert");
                        sqlBulkCopy.ColumnMappings.Add("NonAlcoholicDrinks_Advert", "NonAlcoholicDrinks_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Householdproducts_Advert", "Householdproducts_Advert");
                        sqlBulkCopy.ColumnMappings.Add("ToiletriesCosmetics_Advert", "ToiletriesCosmetics_Advert");
                        sqlBulkCopy.ColumnMappings.Add("PharmaceuticalChemistsProducts_Advert", "PharmaceuticalChemistsProducts_Advert");
                        sqlBulkCopy.ColumnMappings.Add("TobaccoProducts_Advert", "TobaccoProducts_Advert");
                        sqlBulkCopy.ColumnMappings.Add("PetsPetFood_Advert", "PetsPetFood_Advert");
                        sqlBulkCopy.ColumnMappings.Add("ShoppingRetailClothing_Advert", "ShoppingRetailClothing_Advert");
                        sqlBulkCopy.ColumnMappings.Add("DIYGardening_Advert", "DIYGardening_Advert");
                        sqlBulkCopy.ColumnMappings.Add("AppliancesOtherHouseholdDurables_Advert", "AppliancesOtherHouseholdDurables_Advert");
                        sqlBulkCopy.ColumnMappings.Add("ElectronicsOtherPersonalItems_Advert", "ElectronicsOtherPersonalItems_Advert");
                        sqlBulkCopy.ColumnMappings.Add("CommunicationsInternet_Advert", "CommunicationsInternet_Advert");
                        sqlBulkCopy.ColumnMappings.Add("FinancialServices_Advert", "FinancialServices_Advert");
                        sqlBulkCopy.ColumnMappings.Add("HolidaysTravel_Advert", "HolidaysTravel_Advert");
                        sqlBulkCopy.ColumnMappings.Add("SportsLeisure_Advert", "SportsLeisure_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Motoring_Advert", "Motoring_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Newspapers_Advert", "Newspapers_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Magazines_Advert", "Magazines_Advert");
                        sqlBulkCopy.ColumnMappings.Add("TV_Advert", "TV_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Radio_Advert", "Radio_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Cinema_Advert", "Cinema_Advert");
                        sqlBulkCopy.ColumnMappings.Add("SocialNetworking_Advert", "SocialNetworking_Advert");
                        sqlBulkCopy.ColumnMappings.Add("GeneralUse_Advert", "GeneralUse_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Shopping_Advert", "Shopping_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Fitness_Advert", "Fitness_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Holidays_Advert", "Holidays_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Environment_Advert", "Environment_Advert");
                        sqlBulkCopy.ColumnMappings.Add("GoingOut_Advert", "GoingOut_Advert");
                        sqlBulkCopy.ColumnMappings.Add("FinancialProducts_Advert", "FinancialProducts_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Religion_Advert", "Religion_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Fashion_Advert", "Fashion_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Music_Advert", "Music_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Fitness_Attitude", "Fitness_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Holidays_Attitude", "Holidays_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Environment_Attitude", "Environment_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("GoingOut_Attitude", "GoingOut_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("FinancialStabiity_Attitude", "FinancialStabiity_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Religion_Attitude", "Religion_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Fashion_Attitude", "Fashion_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Music_Attitude", "Music_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Cinema_Cinema", "Cinema_Cinema");
                        sqlBulkCopy.ColumnMappings.Add("SocialNetworking_Internet", "SocialNetworking_Internet");
                        sqlBulkCopy.ColumnMappings.Add("Video_Internet", "Video_Internet");
                        sqlBulkCopy.ColumnMappings.Add("Research_Internet", "Research_Internet");
                        sqlBulkCopy.ColumnMappings.Add("Auctions_Internet", "Auctions_Internet");
                        sqlBulkCopy.ColumnMappings.Add("Shopping_Internet", "Shopping_Internet");
                        sqlBulkCopy.ColumnMappings.Add("ContractType_Mobile", "ContractType_Mobile");
                        sqlBulkCopy.ColumnMappings.Add("Spend_Mobile", "Spend_Mobile");
                        sqlBulkCopy.ColumnMappings.Add("Local_Press", "Local_Press");
                        sqlBulkCopy.ColumnMappings.Add("National_Press", "National_Press");
                        sqlBulkCopy.ColumnMappings.Add("FreeNewpapers_Press", "FreeNewpapers_Press");
                        sqlBulkCopy.ColumnMappings.Add("Magazines_Press", "Magazines_Press");
                        sqlBulkCopy.ColumnMappings.Add("Food_ProductsService", "Food_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("SweetSaltySnacks_ProductsService", "SweetSaltySnacks_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("AlcoholicDrinks_ProductsService", "AlcoholicDrinks_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("NonAlcoholicDrinks_ProductsService", "NonAlcoholicDrinks_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("Householdproducts_ProductsService", "Householdproducts_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("ToiletriesCosmetics_ProductsService", "ToiletriesCosmetics_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("PharmaceuticalChemistsProducts_ProductsService", "PharmaceuticalChemistsProducts_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("TobaccoProducts_ProductsService", "TobaccoProducts_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("PetsPetFood_ProductsService", "PetsPetFood_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("ShoppingRetailClothing_ProductsService", "ShoppingRetailClothing_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("DIYGardening_ProductsService", "DIYGardening_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("AppliancesOtherHouseholdDurables_ProductsService", "AppliancesOtherHouseholdDurables_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("ElectronicsOtherPersonalItems_ProductsService", "ElectronicsOtherPersonalItems_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("CommunicationsInternet_ProductsService", "CommunicationsInternet_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("FinancialServices_ProductsService", "FinancialServices_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("HolidaysTravel_ProductsService", "HolidaysTravel_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("SportsLeisure_ProductsService", "SportsLeisure_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("Motoring_ProductsService", "Motoring_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("National_Radio", "National_Radio");
                        sqlBulkCopy.ColumnMappings.Add("Local_Radio", "Local_Radio");
                        sqlBulkCopy.ColumnMappings.Add("Music_Radio", "Music_Radio");
                        sqlBulkCopy.ColumnMappings.Add("Sport_Radio", "Sport_Radio");
                        sqlBulkCopy.ColumnMappings.Add("Talk_Radio", "Talk_Radio");
                        sqlBulkCopy.ColumnMappings.Add("Satallite_TV", "Satallite_TV");
                        sqlBulkCopy.ColumnMappings.Add("Cable_TV", "Cable_TV");
                        sqlBulkCopy.ColumnMappings.Add("Terrestrial_TV", "Terrestrial_TV");
                        sqlBulkCopy.ColumnMappings.Add("Internet_TV", "Internet_TV");

                        sqlBulkCopy.ColumnMappings.Add("Postcode", "Postcode");
                        sqlBulkCopy.ColumnMappings.Add("BusinessOrOpportunities_AdType", "BusinessOrOpportunities_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Gambling_AdType", "Gambling_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Restaurants_AdType", "Restaurants_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Insurance_AdType", "Insurance_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Furniture_AdType", "Furniture_AdType");
                        sqlBulkCopy.ColumnMappings.Add("InformationTechnology_AdType", "InformationTechnology_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Energy_AdType", "Energy_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Supermarkets_AdType", "Supermarkets_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Healthcare_AdType", "Healthcare_AdType");
                        sqlBulkCopy.ColumnMappings.Add("JobsAndEducation_AdType", "JobsAndEducation_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Gifts_AdType", "Gifts_AdType");
                        sqlBulkCopy.ColumnMappings.Add("AdvocacyOrLegal_AdType", "AdvocacyOrLegal_AdType");
                        sqlBulkCopy.ColumnMappings.Add("DatingAndPersonal_AdType", "DatingAndPersonal_AdType");
                        sqlBulkCopy.ColumnMappings.Add("RealEstate_AdType", "RealEstate_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Games_AdType", "Games_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Hustlers_AdType", "Hustlers_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Youth_AdType", "Youth_AdType");
                        sqlBulkCopy.ColumnMappings.Add("DiscerningProfessionals_AdType", "DiscerningProfessionals_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Mass_AdType", "Mass_AdType");

                        con.Open();
                        sqlBulkCopy.WriteToServer(userProfilePrefeDt);
                        con.Close();
                    }

                    #region UserMatch Add

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = "dbo.UserMatches";
                        // sqlBulkCopy.ColumnMappings.Add("Internet_TV", "Internet_TV");

                        sqlBulkCopy.ColumnMappings.Add("Gender_Demographics", "Gender_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("IncomeBracket_Demographics", "IncomeBracket_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("WorkingStatus_Demographics", "WorkingStatus_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("RelationshipStatus_Demographics", "RelationshipStatus_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("Education_Demographics", "Education_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("HouseholdStatus_Demographics", "HouseholdStatus_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("Location_Demographics", "Location_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("Food_Advert", "Food_Advert");
                        sqlBulkCopy.ColumnMappings.Add("SweetSaltySnacks_Advert", "SweetSaltySnacks_Advert");
                        sqlBulkCopy.ColumnMappings.Add("AlcoholicDrinks_Advert", "AlcoholicDrinks_Advert");
                        sqlBulkCopy.ColumnMappings.Add("NonAlcoholicDrinks_Advert", "NonAlcoholicDrinks_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Householdproducts_Advert", "Householdproducts_Advert");
                        sqlBulkCopy.ColumnMappings.Add("ToiletriesCosmetics_Advert", "ToiletriesCosmetics_Advert");
                        sqlBulkCopy.ColumnMappings.Add("PharmaceuticalChemistsProducts_Advert", "PharmaceuticalChemistsProducts_Advert");
                        sqlBulkCopy.ColumnMappings.Add("TobaccoProducts_Advert", "TobaccoProducts_Advert");
                        sqlBulkCopy.ColumnMappings.Add("PetsPetFood_Advert", "PetsPetFood_Advert");
                        sqlBulkCopy.ColumnMappings.Add("ShoppingRetailClothing_Advert", "ShoppingRetailClothing_Advert");
                        sqlBulkCopy.ColumnMappings.Add("DIYGardening_Advert", "DIYGardening_Advert");
                        sqlBulkCopy.ColumnMappings.Add("AppliancesOtherHouseholdDurables_Advert", "AppliancesOtherHouseholdDurables_Advert");
                        sqlBulkCopy.ColumnMappings.Add("ElectronicsOtherPersonalItems_Advert", "ElectronicsOtherPersonalItems_Advert");
                        sqlBulkCopy.ColumnMappings.Add("CommunicationsInternet_Advert", "CommunicationsInternet_Advert");
                        sqlBulkCopy.ColumnMappings.Add("FinancialServices_Advert", "FinancialServices_Advert");
                        sqlBulkCopy.ColumnMappings.Add("HolidaysTravel_Advert", "HolidaysTravel_Advert");
                        sqlBulkCopy.ColumnMappings.Add("SportsLeisure_Advert", "SportsLeisure_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Motoring_Advert", "Motoring_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Newspapers_Advert", "Newspapers_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Magazines_Advert", "Magazines_Advert");
                        sqlBulkCopy.ColumnMappings.Add("TV_Advert", "TV_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Radio_Advert", "Radio_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Cinema_Advert", "Cinema_Advert");
                        sqlBulkCopy.ColumnMappings.Add("SocialNetworking_Advert", "SocialNetworking_Advert");
                        sqlBulkCopy.ColumnMappings.Add("GeneralUse_Advert", "GeneralUse_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Shopping_Advert", "Shopping_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Fitness_Advert", "Fitness_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Holidays_Advert", "Holidays_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Environment_Advert", "Environment_Advert");
                        sqlBulkCopy.ColumnMappings.Add("GoingOut_Advert", "GoingOut_Advert");
                        sqlBulkCopy.ColumnMappings.Add("FinancialProducts_Advert", "FinancialProducts_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Religion_Advert", "Religion_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Fashion_Advert", "Fashion_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Music_Advert", "Music_Advert");
                        sqlBulkCopy.ColumnMappings.Add("Fitness_Attitude", "Fitness_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Holidays_Attitude", "Holidays_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Environment_Attitude", "Environment_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("GoingOut_Attitude", "GoingOut_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("FinancialStabiity_Attitude", "FinancialStabiity_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Religion_Attitude", "Religion_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Fashion_Attitude", "Fashion_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Music_Attitude", "Music_Attitude");
                        sqlBulkCopy.ColumnMappings.Add("Cinema_Cinema", "Cinema_Cinema");
                        sqlBulkCopy.ColumnMappings.Add("SocialNetworking_Internet", "SocialNetworking_Internet");
                        sqlBulkCopy.ColumnMappings.Add("Video_Internet", "Video_Internet");
                        sqlBulkCopy.ColumnMappings.Add("Research_Internet", "Research_Internet");
                        sqlBulkCopy.ColumnMappings.Add("Auctions_Internet", "Auctions_Internet");
                        sqlBulkCopy.ColumnMappings.Add("Shopping_Internet", "Shopping_Internet");
                        sqlBulkCopy.ColumnMappings.Add("ContractType_Mobile", "ContractType_Mobile");
                        sqlBulkCopy.ColumnMappings.Add("Spend_Mobile", "Spend_Mobile");
                        sqlBulkCopy.ColumnMappings.Add("Local_Press", "Local_Press");
                        sqlBulkCopy.ColumnMappings.Add("National_Press", "National_Press");
                        sqlBulkCopy.ColumnMappings.Add("FreeNewpapers_Press", "FreeNewpapers_Press");
                        sqlBulkCopy.ColumnMappings.Add("Magazines_Press", "Magazines_Press");
                        sqlBulkCopy.ColumnMappings.Add("Food_ProductsService", "Food_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("SweetSaltySnacks_ProductsService", "SweetSaltySnacks_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("AlcoholicDrinks_ProductsService", "AlcoholicDrinks_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("NonAlcoholicDrinks_ProductsService", "NonAlcoholicDrinks_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("Householdproducts_ProductsService", "Householdproducts_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("ToiletriesCosmetics_ProductsService", "ToiletriesCosmetics_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("PharmaceuticalChemistsProducts_ProductsService", "PharmaceuticalChemistsProducts_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("TobaccoProducts_ProductsService", "TobaccoProducts_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("PetsPetFood_ProductsService", "PetsPetFood_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("ShoppingRetailClothing_ProductsService", "ShoppingRetailClothing_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("DIYGardening_ProductsService", "DIYGardening_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("AppliancesOtherHouseholdDurables_ProductsService", "AppliancesOtherHouseholdDurables_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("ElectronicsOtherPersonalItems_ProductsService", "ElectronicsOtherPersonalItems_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("CommunicationsInternet_ProductsService", "CommunicationsInternet_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("FinancialServices_ProductsService", "FinancialServices_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("HolidaysTravel_ProductsService", "HolidaysTravel_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("SportsLeisure_ProductsService", "SportsLeisure_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("Motoring_ProductsService", "Motoring_ProductsService");
                        sqlBulkCopy.ColumnMappings.Add("National_Radio", "National_Radio");
                        sqlBulkCopy.ColumnMappings.Add("Local_Radio", "Local_Radio");
                        sqlBulkCopy.ColumnMappings.Add("Music_Radio", "Music_Radio");
                        sqlBulkCopy.ColumnMappings.Add("Sport_Radio", "Sport_Radio");
                        sqlBulkCopy.ColumnMappings.Add("Talk_Radio", "Talk_Radio");
                        sqlBulkCopy.ColumnMappings.Add("Satallite_TV", "Satallite_TV");
                        sqlBulkCopy.ColumnMappings.Add("Cable_TV", "Cable_TV");
                        sqlBulkCopy.ColumnMappings.Add("Terrestrial_TV", "Terrestrial_TV");
                        sqlBulkCopy.ColumnMappings.Add("Internet_TV", "Internet_TV");
                        sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                        sqlBulkCopy.ColumnMappings.Add("MSISDN", "MSISDN");
                        sqlBulkCopy.ColumnMappings.Add("MSUserProfileId", "MSUserProfileId");
                        sqlBulkCopy.ColumnMappings.Add("UserId", "UserId");
                        sqlBulkCopy.ColumnMappings.Add("DOB", "DOB");
                        sqlBulkCopy.ColumnMappings.Add("Gender", "Gender");
                        sqlBulkCopy.ColumnMappings.Add("IncomeBracket", "IncomeBracket");
                        sqlBulkCopy.ColumnMappings.Add("WorkingStatus", "WorkingStatus");
                        sqlBulkCopy.ColumnMappings.Add("RelationshipStatus", "RelationshipStatus");
                        sqlBulkCopy.ColumnMappings.Add("Education", "Education");
                        sqlBulkCopy.ColumnMappings.Add("HouseholdStatus", "HouseholdStatus");
                        sqlBulkCopy.ColumnMappings.Add("Location", "Location");
                        sqlBulkCopy.ColumnMappings.Add("Age_Demographics", "Age_Demographics");
                        sqlBulkCopy.ColumnMappings.Add("BusinessOrOpportunities_AdType", "BusinessOrOpportunities_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Gambling_AdType", "Gambling_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Restaurants_AdType", "Restaurants_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Insurance_AdType", "Insurance_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Furniture_AdType", "Furniture_AdType");
                        sqlBulkCopy.ColumnMappings.Add("InformationTechnology_AdType", "InformationTechnology_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Energy_AdType", "Energy_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Supermarkets_AdType", "Supermarkets_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Healthcare_AdType", "Healthcare_AdType");
                        sqlBulkCopy.ColumnMappings.Add("JobsAndEducation_AdType", "JobsAndEducation_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Gifts_AdType", "Gifts_AdType");
                        sqlBulkCopy.ColumnMappings.Add("AdvocacyOrLegal_AdType", "AdvocacyOrLegal_AdType");
                        sqlBulkCopy.ColumnMappings.Add("DatingAndPersonal_AdType", "DatingAndPersonal_AdType");
                        sqlBulkCopy.ColumnMappings.Add("RealEstate_AdType", "RealEstate_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Games_AdType", "Games_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Hustlers_AdType", "Hustlers_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Youth_AdType", "Youth_AdType");
                        sqlBulkCopy.ColumnMappings.Add("DiscerningProfessionals_AdType", "DiscerningProfessionals_AdType");
                        sqlBulkCopy.ColumnMappings.Add("Mass_AdType", "Mass_AdType");

                        con.Open();
                        sqlBulkCopy.WriteToServer(userMatchDt);
                        con.Close();
                    }
                    #endregion
                }

                sr.Close();
                // System.IO.File.Delete(path);

            }

            ViewBag.Window = "True";
            return View();
        }


        //public ActionResult InsertRecordTest()
        //{
        //    var connection = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
        //    DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/Safaricom/"));
        //    var allFiles = directory.GetFiles().ToList();
        //    foreach (var item in allFiles)
        //    {

        //        var fileName = System.IO.Path.GetFileName(item.FullName);
        //        var path = System.IO.Path.Combine(Server.MapPath("~/Safaricom/"), fileName);

        //        StreamReader sr = new StreamReader(path);



        //        DataTable userProfilePrefeDt = new DataTable();
        //        DataTable userMatchDt = new DataTable();


        //        while (sr.Peek() >= 0)
        //        {
        //            string line = sr.ReadLine();
        //            try
        //            {

        //                if (!string.IsNullOrEmpty(line))
        //                {
        //                    string[] rowsArray = line.Split(';');
        //                    string FirstValue = rowsArray[0];
        //                    if (FirstValue.ToLower() != "header" && FirstValue.ToLower() != "trailer")
        //                    {

        //                        if (rowsArray[1].ToUpper() == "E")
        //                        {

        //                            // CSVRecordsOperation(rowsArray, "Add", userProfilePrefeDt, userMatchDt);
        //                            AddRecords(rowsArray);
        //                            TempData["success"] = "Record inserted successfully.";
        //                        }
        //                        else if (rowsArray[1].ToUpper() == "D")
        //                        {
        //                            CSVRecordsOperation(rowsArray, "Delete", userProfilePrefeDt, userMatchDt);
        //                            TempData["success"] = "Record deleted successfully.";
        //                        }
        //                        else if (rowsArray[1].ToUpper() == "P")
        //                        {
        //                            CSVRecordsOperation(rowsArray, "Update", userProfilePrefeDt, userMatchDt);
        //                            TempData["success"] = "Record updated successfully.";
        //                        }

        //                    }
        //                }


        //            }
        //            catch (Exception msg)
        //            {
        //                Console.WriteLine(msg);
        //            }

        //        }



        //        sr.Close();
        //        // System.IO.File.Delete(path);

        //    }

        //    ViewBag.Window = "True";
        //    return View();
        //}





        private void AddRecords(string[] rowsArray)
        {
            string Email = rowsArray[2];

            var Date = rowsArray[4];
            var year = Date.Substring(0, 4);
            var month = Date.Substring(4, 2);
            var day = Date.Substring(6, 2);
            var hour = Date.Substring(8, 2);
            var min = Date.Substring(10, 2);
            var sec = Date.Substring(12, 2);

            var strDate = year + "-" + month + "-" + day + " " + hour + ":" + min + ":" + sec;

            DateTime dateFormat = Convert.ToDateTime(strDate);
            var DateCreated = dateFormat;
            bool IsMsisdnMatch;
            bool isValid;
            var OperatorId = 1;
            var Activated = 1;
            var phoneNumber = rowsArray[0];
            if (OperatorId == 1)
            {
                //271191
                isValid = CheckUserExistSoapApi(phoneNumber);
                if (isValid)
                {
                    Activated = 1;
                    IsMsisdnMatch = true;
                }
                else
                {
                    Activated = 2;
                    IsMsisdnMatch = false;
                }
            }
            else
            {
                isValid = true;
                Activated = 1;
                IsMsisdnMatch = true;
            }

            var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;

            using (SqlConnection con = new SqlConnection(conn))
            {

                using (SqlCommand cmd = new SqlCommand("BulkImport", con))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@addedDate", DateCreated);
                    cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@isMsisdnMatch", IsMsisdnMatch);
                    cmd.Parameters.AddWithValue("@activated", Activated);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ViewBag.ExecuteNonQuery = "Yes";
                    con.Close();
                }
            }
        }

        private void CSVRecordsOperation(string[] rowsArray, string operation, DataTable userProfilePrefeDt, DataTable userMatchDt)
        {
            if (operation == "Add")
            {
                

                #region Bulk Import

                User userrecord = new User();
                // userrecord.Email = rowsArray[0] + "@email.com";
                userrecord.Email = rowsArray[2];
                userrecord.FirstName = "Tjeep";
                userrecord.LastName = "user";
                userrecord.PasswordHash = "뫙g���98���2X"; //Pa55w0rd!


                var Date = rowsArray[4];
                var year = Date.Substring(0, 4);
                var month = Date.Substring(4, 2);
                var day = Date.Substring(6, 2);
                var hour = Date.Substring(8, 2);
                var min = Date.Substring(10, 2);
                var sec = Date.Substring(12, 2);

                var strDate = year + "-" + month + "-" + day + " " + hour + ":" + min + ":" + sec;

                DateTime dateFormat = Convert.ToDateTime(strDate);
                userrecord.DateCreated = dateFormat;
                userrecord.Organisation = null;
                userrecord.LastLoginTime = DateTime.Now;               
                userrecord.RoleId = 2;
                userrecord.Outstandingdays = 0;
                userrecord.OperatorId = 1;
                userrecord.VerificationStatus = true;
                bool isValid = false;
                if (userrecord.OperatorId == 1)
                {
                    //271191
                    //var phoneNumber = rowsArray[0];
                    //isValid = CheckUserExistSoapApi(phoneNumber);
                    //if(isValid)
                    //{
                        userrecord.Activated = 1;
                        userrecord.IsMsisdnMatch = true;                    
                    //}
                    //else
                    //{
                    //    userrecord.Activated = 2;
                    //}
                }
                else
                {
                    isValid = true;
                    userrecord.Activated = 1;
                }
                db.Users.Add(userrecord);
                db.SaveChanges();

                UserProfile obj = new UserProfile();
                obj.UserId = userrecord.UserId;
                obj.DOB = null;
                obj.Gender = "C";
                obj.IncomeBracket = "G";
                obj.WorkingStatus = "I";
                obj.RelationshipStatus = "G";
                obj.Education = "E";
                obj.HouseholdStatus = "D";
                obj.Location = null;
                obj.MSISDN = "+" + rowsArray[0];
                db.Userprofiles.Add(obj);
                db.SaveChanges();

                // UserProfilePreference obj2 = new UserProfilePreference();
                if (isValid)
                {
                    DataRow rowUserProfile = userProfilePrefeDt.NewRow();
                    rowUserProfile["UserProfileId"] = obj.UserProfileId;
                    rowUserProfile["Gender_Demographics"] = "C";
                    rowUserProfile["IncomeBracket_Demographics"] = "G";
                    rowUserProfile["WorkingStatus_Demographics"] = "I";
                    rowUserProfile["RelationshipStatus_Demographics"] = "G";
                    rowUserProfile["Education_Demographics"] = "E";
                    rowUserProfile["HouseholdStatus_Demographics"] = "D";
                    rowUserProfile["Location_Demographics"] = null;
                    rowUserProfile["Food_Advert"] = "B";
                    rowUserProfile["SweetSaltySnacks_Advert"] = "B";
                    rowUserProfile["AlcoholicDrinks_Advert"] = "B";
                    rowUserProfile["NonAlcoholicDrinks_Advert"] = "B";
                    rowUserProfile["Householdproducts_Advert"] = "B";
                    rowUserProfile["ToiletriesCosmetics_Advert"] = "B";
                    rowUserProfile["PharmaceuticalChemistsProducts_Advert"] = "B";
                    rowUserProfile["TobaccoProducts_Advert"] = "B";
                    rowUserProfile["PetsPetFood_Advert"] = "B";
                    rowUserProfile["ShoppingRetailClothing_Advert"] = "B";
                    rowUserProfile["DIYGardening_Advert"] = "B";
                    rowUserProfile["AppliancesOtherHouseholdDurables_Advert"] = "B";
                    rowUserProfile["ElectronicsOtherPersonalItems_Advert"] = "B";
                    rowUserProfile["CommunicationsInternet_Advert"] = "B";
                    rowUserProfile["FinancialServices_Advert"] = "B";
                    rowUserProfile["HolidaysTravel_Advert"] = "B";
                    rowUserProfile["SportsLeisure_Advert"] = "B";
                    rowUserProfile["Motoring_Advert"] = "B";
                    rowUserProfile["Newspapers_Advert"] = "B";
                    rowUserProfile["Magazines_Advert"] = "B";
                    rowUserProfile["TV_Advert"] = "B";
                    rowUserProfile["Radio_Advert"] = "B";
                    rowUserProfile["Cinema_Advert"] = "B";
                    rowUserProfile["SocialNetworking_Advert"] = "B";
                    rowUserProfile["GeneralUse_Advert"] = "B";
                    rowUserProfile["Shopping_Advert"] = "B";
                    rowUserProfile["Fitness_Advert"] = "B";
                    rowUserProfile["Holidays_Advert"] = "B";
                    rowUserProfile["Environment_Advert"] = "B";
                    rowUserProfile["GoingOut_Advert"] = "B";
                    rowUserProfile["FinancialProducts_Advert"] = "B";
                    rowUserProfile["Religion_Advert"] = "B";
                    rowUserProfile["Fashion_Advert"] = "B";
                    rowUserProfile["Music_Advert"] = "B";
                    rowUserProfile["Fitness_Attitude"] = "B";
                    rowUserProfile["Holidays_Attitude"] = "B";
                    rowUserProfile["Environment_Attitude"] = "B";
                    rowUserProfile["GoingOut_Attitude"] = "B";
                    rowUserProfile["FinancialStabiity_Attitude"] = "B";
                    rowUserProfile["Religion_Attitude"] = "B";
                    rowUserProfile["Fashion_Attitude"] = "B";
                    rowUserProfile["Music_Attitude"] = "B";
                    rowUserProfile["Cinema_Cinema"] = "A";
                    rowUserProfile["SocialNetworking_Internet"] = "A";
                    rowUserProfile["Video_Internet"] = "A";
                    rowUserProfile["Research_Internet"] = "A";
                    rowUserProfile["Auctions_Internet"] = "A";
                    rowUserProfile["Shopping_Internet"] = "A";
                    rowUserProfile["ContractType_Mobile"] = "A";
                    rowUserProfile["Spend_Mobile"] = "A";
                    rowUserProfile["Local_Press"] = "A";
                    rowUserProfile["National_Press"] = "A";
                    rowUserProfile["FreeNewpapers_Press"] = "A";
                    rowUserProfile["Magazines_Press"] = "A";
                    rowUserProfile["Food_ProductsService"] = "A";
                    rowUserProfile["SweetSaltySnacks_ProductsService"] = "A";
                    rowUserProfile["AlcoholicDrinks_ProductsService"] = "A";
                    rowUserProfile["NonAlcoholicDrinks_ProductsService"] = "A";
                    rowUserProfile["Householdproducts_ProductsService"] = "A";
                    rowUserProfile["ToiletriesCosmetics_ProductsService"] = "A";
                    rowUserProfile["PharmaceuticalChemistsProducts_ProductsService"] = "A";
                    rowUserProfile["TobaccoProducts_ProductsService"] = "A";
                    rowUserProfile["PetsPetFood_ProductsService"] = "A";
                    rowUserProfile["ShoppingRetailClothing_ProductsService"] = "A";
                    rowUserProfile["DIYGardening_ProductsService"] = "A";
                    rowUserProfile["AppliancesOtherHouseholdDurables_ProductsService"] = "A";
                    rowUserProfile["ElectronicsOtherPersonalItems_ProductsService"] = "A";
                    rowUserProfile["CommunicationsInternet_ProductsService"] = "A";
                    rowUserProfile["FinancialServices_ProductsService"] = "A";
                    rowUserProfile["HolidaysTravel_ProductsService"] = "A";
                    rowUserProfile["SportsLeisure_ProductsService"] = "A";
                    rowUserProfile["Motoring_ProductsService"] = "A";
                    rowUserProfile["National_Radio"] = "A";
                    rowUserProfile["Local_Radio"] = "A";
                    rowUserProfile["Music_Radio"] = "A";
                    rowUserProfile["Sport_Radio"] = "A";
                    rowUserProfile["Talk_Radio"] = "A";
                    rowUserProfile["Satallite_TV"] = "A";
                    rowUserProfile["Cable_TV"] = "A";
                    rowUserProfile["Terrestrial_TV"] = "A";
                    rowUserProfile["Internet_TV"] = "A";

                    rowUserProfile["Postcode"] = null;
                    rowUserProfile["BusinessOrOpportunities_AdType"] = "B";
                    rowUserProfile["Gambling_AdType"] = "B";
                    rowUserProfile["Restaurants_AdType"] = "B";
                    rowUserProfile["Insurance_AdType"] = "B";
                    rowUserProfile["Furniture_AdType"] = "B";
                    rowUserProfile["InformationTechnology_AdType"] = "B";
                    rowUserProfile["Energy_AdType"] = "B";
                    rowUserProfile["Supermarkets_AdType"] = "B";
                    rowUserProfile["Healthcare_AdType"] = "B";
                    rowUserProfile["JobsAndEducation_AdType"] = "B";
                    rowUserProfile["Gifts_AdType"] = "B";
                    rowUserProfile["AdvocacyOrLegal_AdType"] = "B";
                    rowUserProfile["DatingAndPersonal_AdType"] = "B";
                    rowUserProfile["RealEstate_AdType"] = "B";
                    rowUserProfile["Games_AdType"] = "B";
                    rowUserProfile["Hustlers_AdType"] = "A";
                    rowUserProfile["Youth_AdType"] = "A";
                    rowUserProfile["DiscerningProfessionals_AdType"] = "A";
                    rowUserProfile["Mass_AdType"] = "A";
                    userProfilePrefeDt.Rows.Add(rowUserProfile);

                    //db.UserProfilePreference.Add(obj2);
                    //db.SaveChanges();

                    //  UserMatch userMatchData = new UserMatch();

                    #region UserMatchRow
                    DataRow rowUserMatch = userMatchDt.NewRow();

                    rowUserMatch["Gender_Demographics"] = "A";
                    rowUserMatch["IncomeBracket_Demographics"] = "B";
                    rowUserMatch["WorkingStatus_Demographics"] = "A";
                    rowUserMatch["RelationshipStatus_Demographics"] = "E";
                    rowUserMatch["Education_Demographics"] = "A";
                    rowUserMatch["HouseholdStatus_Demographics"] = "A";
                    rowUserMatch["Location_Demographics"] = "A";
                    rowUserMatch["Food_Advert"] = "C";
                    rowUserMatch["SweetSaltySnacks_Advert"] = "B";
                    rowUserMatch["AlcoholicDrinks_Advert"] = "B";
                    rowUserMatch["NonAlcoholicDrinks_Advert"] = "B";
                    rowUserMatch["Householdproducts_Advert"] = "B";
                    rowUserMatch["ToiletriesCosmetics_Advert"] = "B";
                    rowUserMatch["PharmaceuticalChemistsProducts_Advert"] = "B";
                    rowUserMatch["TobaccoProducts_Advert"] = "B";
                    rowUserMatch["PetsPetFood_Advert"] = "B";
                    rowUserMatch["ShoppingRetailClothing_Advert"] = "B";
                    rowUserMatch["DIYGardening_Advert"] = "B";
                    rowUserMatch["AppliancesOtherHouseholdDurables_Advert"] = "B";
                    rowUserMatch["ElectronicsOtherPersonalItems_Advert"] = "B";
                    rowUserMatch["CommunicationsInternet_Advert"] = "B";
                    rowUserMatch["FinancialServices_Advert"] = "B";
                    rowUserMatch["HolidaysTravel_Advert"] = "B";
                    rowUserMatch["SportsLeisure_Advert"] = "B";
                    rowUserMatch["Motoring_Advert"] = "B";
                    rowUserMatch["Newspapers_Advert"] = "B";
                    rowUserMatch["Magazines_Advert"] = "B";
                    rowUserMatch["TV_Advert"] = "B";


                    rowUserMatch["Radio_Advert"] = "B";
                    rowUserMatch["Cinema_Advert"] = "B";
                    rowUserMatch["SocialNetworking_Advert"] = "B";
                    rowUserMatch["GeneralUse_Advert"] = "B";
                    rowUserMatch["Shopping_Advert"] = "B";
                    rowUserMatch["Fitness_Advert"] = "B";
                    rowUserMatch["Holidays_Advert"] = "B";
                    rowUserMatch["Environment_Advert"] = "B";
                    rowUserMatch["GoingOut_Advert"] = "B";
                    rowUserMatch["FinancialProducts_Advert"] = "B";
                    rowUserMatch["Religion_Advert"] = "B";
                    rowUserMatch["Fashion_Advert"] = "B";
                    rowUserMatch["Music_Advert"] = "B";
                    rowUserMatch["Fitness_Attitude"] = "B";
                    rowUserMatch["Holidays_Attitude"] = "B";
                    rowUserMatch["Environment_Attitude"] = "B";
                    rowUserMatch["GoingOut_Attitude"] = "B";
                    rowUserMatch["FinancialStabiity_Attitude"] = "B";
                    rowUserMatch["Religion_Attitude"] = "B";
                    rowUserMatch["Fashion_Attitude"] = "B";
                    rowUserMatch["Music_Attitude"] = "B";

                    rowUserMatch["Cinema_Cinema"] = "A";
                    rowUserMatch["SocialNetworking_Internet"] = "A";
                    rowUserMatch["Video_Internet"] = "A";
                    rowUserMatch["Research_Internet"] = "A";
                    rowUserMatch["Auctions_Internet"] = "A";
                    rowUserMatch["Shopping_Internet"] = "A";
                    rowUserMatch["ContractType_Mobile"] = "B";
                    rowUserMatch["Spend_Mobile"] = "C";

                    rowUserMatch["Local_Press"] = "A";
                    rowUserMatch["National_Press"] = "A";
                    rowUserMatch["FreeNewpapers_Press"] = "A";
                    rowUserMatch["Magazines_Press"] = "A";
                    rowUserMatch["Food_ProductsService"] = "A";
                    rowUserMatch["SweetSaltySnacks_ProductsService"] = "A";
                    rowUserMatch["AlcoholicDrinks_ProductsService"] = "A";
                    rowUserMatch["NonAlcoholicDrinks_ProductsService"] = "A";
                    rowUserMatch["Householdproducts_ProductsService"] = "A";
                    rowUserMatch["ToiletriesCosmetics_ProductsService"] = "A";
                    rowUserMatch["PharmaceuticalChemistsProducts_ProductsService"] = "A";
                    rowUserMatch["TobaccoProducts_ProductsService"] = "A";
                    rowUserMatch["PetsPetFood_ProductsService"] = "A";
                    rowUserMatch["ShoppingRetailClothing_ProductsService"] = "A";
                    rowUserMatch["DIYGardening_ProductsService"] = "A";
                    rowUserMatch["AppliancesOtherHouseholdDurables_ProductsService"] = "A";
                    rowUserMatch["ElectronicsOtherPersonalItems_ProductsService"] = "A";
                    rowUserMatch["CommunicationsInternet_ProductsService"] = "A";
                    rowUserMatch["FinancialServices_ProductsService"] = "A";
                    rowUserMatch["HolidaysTravel_ProductsService"] = "A";
                    rowUserMatch["SportsLeisure_ProductsService"] = "A";
                    rowUserMatch["Motoring_ProductsService"] = "A";
                    rowUserMatch["National_Radio"] = "A";
                    rowUserMatch["Local_Radio"] = "A";
                    rowUserMatch["Music_Radio"] = "A";
                    rowUserMatch["Sport_Radio"] = "A";
                    rowUserMatch["Talk_Radio"] = "A";
                    rowUserMatch["Satallite_TV"] = "A";
                    rowUserMatch["Cable_TV"] = "A";
                    rowUserMatch["Terrestrial_TV"] = "A";
                    rowUserMatch["Internet_TV"] = "A";

                    rowUserMatch["Email"] = rowsArray[2];
                    rowUserMatch["MSISDN"] = rowsArray[0];
                    rowUserMatch["MSUserProfileId"] = obj.UserProfileId;
                    rowUserMatch["UserId"] = userrecord.UserId;
                    rowUserMatch["DOB"] = null;
                    rowUserMatch["Gender"] = "A";
                    rowUserMatch["IncomeBracket"] = "F";
                    rowUserMatch["WorkingStatus"] = "A";
                    rowUserMatch["RelationshipStatus"] = "E";
                    rowUserMatch["Education"] = "D";
                    rowUserMatch["HouseholdStatus"] = "B";
                    rowUserMatch["Location"] = "A";
                    rowUserMatch["Age_Demographics"] = null;
                    rowUserMatch["BusinessOrOpportunities_AdType"] = "B";
                    rowUserMatch["Gambling_AdType"] = "B";
                    rowUserMatch["Restaurants_AdType"] = "B";
                    rowUserMatch["Insurance_AdType"] = "B";
                    rowUserMatch["Furniture_AdType"] = "B";
                    rowUserMatch["InformationTechnology_AdType"] = "B";
                    rowUserMatch["Energy_AdType"] = "B";
                    rowUserMatch["Supermarkets_AdType"] = "B";
                    rowUserMatch["Healthcare_AdType"] = "B";
                    rowUserMatch["JobsAndEducation_AdType"] = "B";
                    rowUserMatch["Gifts_AdType"] = "B";
                    rowUserMatch["AdvocacyOrLegal_AdType"] = "B";
                    rowUserMatch["DatingAndPersonal_AdType"] = "B";
                    rowUserMatch["RealEstate_AdType"] = "B";
                    rowUserMatch["Games_AdType"] = "B";
                    rowUserMatch["Hustlers_AdType"] = "A";
                    rowUserMatch["Youth_AdType"] = "A";
                    rowUserMatch["DiscerningProfessionals_AdType"] = "A";
                    rowUserMatch["Mass_AdType"] = "A";

                    userMatchDt.Rows.Add(rowUserMatch);
                    #endregion
                }
                //db.UserMatch.Add(userMatchData);
                //db.SaveChanges();


                //UserProfileTimeSetting userprofiletime = new UserProfileTimeSetting();
                //userprofiletime.UserProfileId = obj.UserProfileId;
                //userprofiletime.Monday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userprofiletime.Tuesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userprofiletime.Wednesday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userprofiletime.Thursday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userprofiletime.Friday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userprofiletime.Saturday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //userprofiletime.Sunday = "01:00,02:00,03:00,04:00,05:00,06:00,07:00,08:00,09:00,10:00,11:00,12:00,13:00,14:00,15:00,16:00,17:00,18:00,19:00,20:00,21:00,22:00,23:00,24:00";
                //db.UserProfileTimeSetting.Add(userprofiletime);
                //db.SaveChanges();

                // CSVRecordsOperationOnMySql(rowsArray, obj.UserProfileId, obj.UserId, "AddRecord");


                #endregion
            }
            else if (operation == "Delete")
            {
                #region Delete User
                var Msisdn = "+" + rowsArray[0];
                var GetUserProfileData = db.Userprofiles.Where(s => s.MSISDN == Msisdn).FirstOrDefault();
                if (GetUserProfileData != null)
                {
                    var GetUserProfilePreferenceData = db.UserProfilePreference.Where(s => s.UserProfileId == GetUserProfileData.UserProfileId).FirstOrDefault();
                    if (GetUserProfilePreferenceData != null)
                    {
                        db.UserProfilePreference.Remove(GetUserProfilePreferenceData);
                    }
                    db.Userprofiles.Remove(GetUserProfileData);
                    var GetUserData = db.Users.Where(s => s.UserId == GetUserProfileData.UserId).FirstOrDefault();
                    if (GetUserData != null)
                    {
                        db.Users.Remove(GetUserData);
                    }
                    db.SaveChanges();
                    //CSVRecordsDeleteOnMySql(GetUserProfileData.UserProfileId);


                }
                #endregion
            }
            else
            {
                #region UpdateUser
                var Msisdn = "+" + rowsArray[0];
                var GetUserProfileData = db.Userprofiles.Where(s => s.MSISDN == Msisdn).FirstOrDefault();
                if (GetUserProfileData != null)
                {
                    GetUserProfileData.MSISDN = "+" + rowsArray[0];
                    db.SaveChanges();
                    var GetUserProfilePreferenceData = db.UserProfilePreference.Where(s => s.UserProfileId == GetUserProfileData.UserProfileId).FirstOrDefault();
                    if (GetUserProfilePreferenceData != null)
                    {
                        // db.UserProfilePreference.Remove(GetUserProfilePreferenceData);
                    }

                    var GetUserData = db.Users.Where(s => s.UserId == GetUserProfileData.UserId).FirstOrDefault();
                    if (GetUserData != null)
                    {
                        // GetUserData.DateCreated = Convert.ToDateTime(rowsArray[3]); //DateTime.Now;
                        // DateTime date = DateTime.ParseExact(rowsArray[4], "yyyy-MM-dd hh:mm:ss", null);


                        var Date = rowsArray[4];
                        var year = Date.Substring(0, 4);
                        var month = Date.Substring(4, 2);
                        var day = Date.Substring(6, 2);
                        var hour = Date.Substring(8, 2);
                        var min = Date.Substring(10, 2);
                        var sec = Date.Substring(12, 2);

                        var strDate = year + "-" + month + "-" + day + " " + hour + ":" + min + ":" + sec;

                        DateTime dateFormat = Convert.ToDateTime(strDate);
                        GetUserData.Email = rowsArray[2];
                        GetUserData.DateCreated = dateFormat;
                        db.SaveChanges();
                    }
                    // CSVRecordsOperationOnMySql(rowsArray, GetUserProfileData.UserProfileId, GetUserProfileData.UserId, "UpdateRecord");


                }
                #endregion
            }
        }

        private bool CheckUserExistSoapApi(string phoneNum)
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                
                    var portalAccount = "adtones";
                    var portalPassword = "TBD";
                    var portalType = "1";
                    var role = "4";
                    var roleCode = "100";
                    var corpId = "440114";
                    // var phoneNumber = "254000000000";
                    var phoneNumber = phoneNum;

                    string soapUIUrl = ConfigurationManager.AppSettings["SoapUIUrl"];
                    var client = new RestClient(soapUIUrl);
                    var request = new RestRequest(Method.POST);

                    request.AddParameter("undefined",
                        "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:cor=\"http://corpusermanage.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <cor:addCorpUser soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n" +
                        "<event xsi:type=\"even:AddCorpUserEvt\" xmlns:even=\"http://event.corpusermanage.ivas.huawei.com\">\r\n           \t" +
                            "<portalAccount>" + portalAccount + "</portalAccount>\r\n\t\t\t<portalPwd>" + portalPassword + "</portalPwd>\r\n\t\t\t" +
                            "<portalType>" + portalType + "</portalType>\r\n\t\t\t<role>" + role + "</role>\r\n\t\t\t" +
                            "<roleCode>" + roleCode + "</roleCode>\r\n\t\t\t" +
                            "<corpID>" + corpId + "</corpID>\r\n\t\t\t<phoneNumber>" + phoneNumber + "</phoneNumber>\r\n" +
                        "</event>\r\n      </cor:addCorpUser>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var responseContent = response.Content;

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(responseContent);
                    XmlNodeList nodeList = xmldoc.GetElementsByTagName("addCorpUserReturn");
                    if (nodeList.Count > 0)
                    {
                        foreach (XmlNode node in nodeList)
                        {
                            var returnCode = node.SelectSingleNode("returnCode").InnerXml;
                            if (returnCode == "000000")
                            {
                                return true;
                            }
                            else if (returnCode == "100002")
                            {
                                return false;
                                //SYSTEM_BUSY
                            }
                            else if (returnCode == "100003")
                            {
                                return false;
                                //OPERATION_OVERTIME
                            }
                            else if (returnCode == "100004")
                            {
                                return false;
                                //NETWORK_EXCEPTION
                            }
                            else if (returnCode == "100006")
                            {
                                return false;
                                //DATABASE_OPERATION_FAILED
                            }
                            else if (returnCode == "100007")
                            {
                                return false;
                                //HAS_NOT_SERVICE
                            }
                            //var temp2 = node.SelectSingleNode("operationID").InnerXml;
                        }
                    }
                
            }
            catch (Exception ex)
            {

            }
            return false;

        }
    }
}