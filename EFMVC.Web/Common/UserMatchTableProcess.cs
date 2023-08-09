using EFMVC.Data;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Common
{
    public class UserMatchTableProcess
    {
        //string connectionString = "Data Source=217.160.184.168;Initial Catalog=Arthar;User ID=sa;Password=z_nTJG@5TB";
        string connectionString = "Data Source=S20494255;Initial Catalog=Arthar;User ID=sa;Password=z_nTJG@5TB";

        public string GetUserMatchTableNumber(int operatorId)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            var userData = db.Users.Where(s => s.RoleId == 2 && s.OperatorId == operatorId).OrderByDescending(s => s.UserId).FirstOrDefault();

            if (operatorId == (int)OperatorTableId.Safaricom) // Ten User Match Table
            {
                if (userData != null)
                {
                    //if (userData.UserMatchTableName == null)
                    //    return "UserMatch1";
                    //else if (userData.UserMatchTableName == "UserMatch1")
                    //    return "UserMatch2";
                    //else if (userData.UserMatchTableName == "UserMatch2")
                    //    return "UserMatch3";
                    //else if (userData.UserMatchTableName == "UserMatch3")
                    //    return "UserMatch4";
                    //else if (userData.UserMatchTableName == "UserMatch4")
                    //    return "UserMatch5";
                    //else if (userData.UserMatchTableName == "UserMatch5")
                    //    return "UserMatch6";
                    //else if (userData.UserMatchTableName == "UserMatch6")
                    //    return "UserMatch7";
                    //else if (userData.UserMatchTableName == "UserMatch7")
                    //    return "UserMatch8";
                    //else if (userData.UserMatchTableName == "UserMatch8")
                    //    return "UserMatch9";
                    //else if (userData.UserMatchTableName == "UserMatch9")
                    //    return "UserMatch10";
                    //else if (userData.UserMatchTableName == "UserMatch10")
                        return "UserMatch1";
                }
            }
            else if (operatorId == (int)OperatorTableId.Expresso) // Ten User Match Table
            {
                //Code Uncomment When Senegal Ten Linux Server has been Setup
                //int UserMatch1 = 0, UserMatch2 = 0, UserMatch3 = 0, UserMatch4 = 0, UserMatch5 = 0, UserMatch6 = 0, UserMatch7 = 0, UserMatch8 = 0, UserMatch9 = 0, UserMatch10 = 0;
                //var getUserMatchCount = ExecuteSP("GetUserMatchCount", connectionString);
                //foreach (DataRow row in getUserMatchCount.Rows)
                //{
                //    UserMatch1 = Convert.ToInt32(row["UserMatch1"].ToString());
                //    UserMatch2 = Convert.ToInt32(row["UserMatch2"].ToString());
                //    UserMatch3 = Convert.ToInt32(row["UserMatch3"].ToString());
                //    UserMatch4 = Convert.ToInt32(row["UserMatch4"].ToString());
                //    UserMatch5 = Convert.ToInt32(row["UserMatch5"].ToString());
                //    UserMatch6 = Convert.ToInt32(row["UserMatch6"].ToString());
                //    UserMatch7 = Convert.ToInt32(row["UserMatch7"].ToString());
                //    UserMatch8 = Convert.ToInt32(row["UserMatch8"].ToString());
                //    UserMatch9 = Convert.ToInt32(row["UserMatch9"].ToString());
                //    UserMatch10 = Convert.ToInt32(row["UserMatch10"].ToString());
                //}
                if (userData != null)
                {
                    return "UserMatch1";
                    //Code Uncomment When Senegal Ten Linux Server has been Setup
                    //if (userData.UserMatchTableName == null && UserMatch1 < 50000)
                    //    return "UserMatch1";
                    //else if (userData.UserMatchTableName == "UserMatch1" && UserMatch2 < 50000)
                    //    return "UserMatch2";
                    //else if (userData.UserMatchTableName == "UserMatch2" && UserMatch3 < 50000)
                    //    return "UserMatch3";
                    //else if (userData.UserMatchTableName == "UserMatch3" && UserMatch4 < 50000)
                    //    return "UserMatch4";
                    //else if (userData.UserMatchTableName == "UserMatch4" && UserMatch5 < 50000)
                    //    return "UserMatch5";
                    //else if (userData.UserMatchTableName == "UserMatch5" && UserMatch6 < 50000)
                    //    return "UserMatch6";
                    //else if (userData.UserMatchTableName == "UserMatch6" && UserMatch7 < 50000)
                    //    return "UserMatch7";
                    //else if (userData.UserMatchTableName == "UserMatch7" && UserMatch8 < 50000)
                    //    return "UserMatch8";
                    //else if (userData.UserMatchTableName == "UserMatch8" && UserMatch9 < 50000)
                    //    return "UserMatch9";
                    //else if (userData.UserMatchTableName == "UserMatch9" && UserMatch10 < 50000)
                    //    return "UserMatch10";
                    //else if (userData.UserMatchTableName == "UserMatch10" && UserMatch1 < 50000)
                    //    return "UserMatch1";
                }
            }

            return "UserMatch1";
        }

        public void AddUserMatchData(string userMatchTableName, UserProfile profilePref, User userData, EFMVCDataContex db)
        {
            if (userMatchTableName == "UserMatch1")
            {
                #region UserMatch1

                UserMatch objUserMatch = new UserMatch();

                objUserMatch.Email = userData.Email;
                objUserMatch.MSISDN = profilePref.MSISDN;
                objUserMatch.MSUserProfileId = profilePref.UserProfileId;
                objUserMatch.UserId = userData.UserId;
                objUserMatch.Gender_Demographics = "C";
                objUserMatch.WorkingStatus_Demographics = "I";
                objUserMatch.RelationshipStatus_Demographics = "G";
                objUserMatch.Education_Demographics = "E";
                objUserMatch.HouseholdStatus_Demographics = "D";
                objUserMatch.Location_Demographics = "A";
                objUserMatch.Food_Advert = "B";
                objUserMatch.SweetSaltySnacks_Advert = "B";
                objUserMatch.AlcoholicDrinks_Advert = "B";
                objUserMatch.NonAlcoholicDrinks_Advert = "B";
                objUserMatch.Householdproducts_Advert = "B";
                objUserMatch.ToiletriesCosmetics_Advert = "B";
                objUserMatch.PharmaceuticalChemistsProducts_Advert = "B";
                objUserMatch.TobaccoProducts_Advert = "B";
                objUserMatch.PetsPetFood_Advert = "B";
                objUserMatch.ShoppingRetailClothing_Advert = "B";
                objUserMatch.DIYGardening_Advert = "B";
                objUserMatch.ElectronicsOtherPersonalItems_Advert = "B";
                objUserMatch.CommunicationsInternet_Advert = "B";
                objUserMatch.FinancialServices_Advert = "B";
                objUserMatch.HolidaysTravel_Advert = "B";
                objUserMatch.SportsLeisure_Advert = "B";
                objUserMatch.Motoring_Advert = "B";
                objUserMatch.Newspapers_Advert = "B";
                objUserMatch.TV_Advert = "B";
                objUserMatch.Cinema_Advert = "B";
                objUserMatch.SocialNetworking_Advert = "B";
                objUserMatch.Shopping_Advert = "B";
                objUserMatch.Fitness_Advert = "B";
                objUserMatch.Environment_Advert = "B";
                objUserMatch.GoingOut_Advert = "B";
                objUserMatch.Religion_Advert = "B";
                objUserMatch.Music_Advert = "B";
                objUserMatch.BusinessOrOpportunities_AdType = "B";
                objUserMatch.Gambling_AdType = "B";
                objUserMatch.Restaurants_AdType = "B";
                objUserMatch.Insurance_AdType = "B";
                objUserMatch.Furniture_AdType = "B";
                objUserMatch.InformationTechnology_AdType = "B";
                objUserMatch.Energy_AdType = "B";
                objUserMatch.Supermarkets_AdType = "B";
                objUserMatch.Healthcare_AdType = "B";
                objUserMatch.JobsAndEducation_AdType = "B";
                objUserMatch.Gifts_AdType = "B";
                objUserMatch.AdvocacyOrLegal_AdType = "B";
                objUserMatch.DatingAndPersonal_AdType = "B";
                objUserMatch.RealEstate_AdType = "B";
                objUserMatch.Games_AdType = "B";
                objUserMatch.Hustlers_AdType = "A";
                objUserMatch.Youth_AdType = "A";
                objUserMatch.DiscerningProfessionals_AdType = "A";
                objUserMatch.Mass_AdType = "A";
                objUserMatch.ContractType_Mobile = "A";
                objUserMatch.Spend_Mobile = "A";

                db.UserMatch.Add(objUserMatch);
                db.SaveChanges();
                #endregion
            }
            else if (userMatchTableName == "UserMatch2")
            {
                #region UserMatch2
                UserMatch2 objUserMatch = new UserMatch2();

                objUserMatch.Email = userData.Email;
                objUserMatch.MSISDN = profilePref.MSISDN;
                objUserMatch.MSUserProfileId = profilePref.UserProfileId;
                objUserMatch.UserId = userData.UserId;
                objUserMatch.Gender_Demographics = "C";
                objUserMatch.WorkingStatus_Demographics = "I";
                objUserMatch.RelationshipStatus_Demographics = "G";
                objUserMatch.Education_Demographics = "E";
                objUserMatch.HouseholdStatus_Demographics = "D";
                objUserMatch.Location_Demographics = "A";
                objUserMatch.Food_Advert = "B";
                objUserMatch.SweetSaltySnacks_Advert = "B";
                objUserMatch.AlcoholicDrinks_Advert = "B";
                objUserMatch.NonAlcoholicDrinks_Advert = "B";
                objUserMatch.Householdproducts_Advert = "B";
                objUserMatch.ToiletriesCosmetics_Advert = "B";
                objUserMatch.PharmaceuticalChemistsProducts_Advert = "B";
                objUserMatch.TobaccoProducts_Advert = "B";
                objUserMatch.PetsPetFood_Advert = "B";
                objUserMatch.ShoppingRetailClothing_Advert = "B";
                objUserMatch.DIYGardening_Advert = "B";
                objUserMatch.ElectronicsOtherPersonalItems_Advert = "B";
                objUserMatch.CommunicationsInternet_Advert = "B";
                objUserMatch.FinancialServices_Advert = "B";
                objUserMatch.HolidaysTravel_Advert = "B";
                objUserMatch.SportsLeisure_Advert = "B";
                objUserMatch.Motoring_Advert = "B";
                objUserMatch.Newspapers_Advert = "B";
                objUserMatch.TV_Advert = "B";
                objUserMatch.Cinema_Advert = "B";
                objUserMatch.SocialNetworking_Advert = "B";
                objUserMatch.Shopping_Advert = "B";
                objUserMatch.Fitness_Advert = "B";
                objUserMatch.Environment_Advert = "B";
                objUserMatch.GoingOut_Advert = "B";
                objUserMatch.Religion_Advert = "B";
                objUserMatch.Music_Advert = "B";
                objUserMatch.BusinessOrOpportunities_AdType = "B";
                objUserMatch.Gambling_AdType = "B";
                objUserMatch.Restaurants_AdType = "B";
                objUserMatch.Insurance_AdType = "B";
                objUserMatch.Furniture_AdType = "B";
                objUserMatch.InformationTechnology_AdType = "B";
                objUserMatch.Energy_AdType = "B";
                objUserMatch.Supermarkets_AdType = "B";
                objUserMatch.Healthcare_AdType = "B";
                objUserMatch.JobsAndEducation_AdType = "B";
                objUserMatch.Gifts_AdType = "B";
                objUserMatch.AdvocacyOrLegal_AdType = "B";
                objUserMatch.DatingAndPersonal_AdType = "B";
                objUserMatch.RealEstate_AdType = "B";
                objUserMatch.Games_AdType = "B";
                objUserMatch.Hustlers_AdType = "A";
                objUserMatch.Youth_AdType = "A";
                objUserMatch.DiscerningProfessionals_AdType = "A";
                objUserMatch.Mass_AdType = "A";
                objUserMatch.ContractType_Mobile = "A";
                objUserMatch.Spend_Mobile = "A";

                db.UserMatch2.Add(objUserMatch);
                db.SaveChanges();



                #endregion
            }
            else if (userMatchTableName == "UserMatch3")
            {
                #region UserMatch3
                UserMatch3 objUserMatch = new UserMatch3();

                objUserMatch.Email = userData.Email;
                objUserMatch.MSISDN = profilePref.MSISDN;
                objUserMatch.MSUserProfileId = profilePref.UserProfileId;
                objUserMatch.UserId = userData.UserId;
                objUserMatch.Gender_Demographics = "C";
                objUserMatch.WorkingStatus_Demographics = "I";
                objUserMatch.RelationshipStatus_Demographics = "G";
                objUserMatch.Education_Demographics = "E";
                objUserMatch.HouseholdStatus_Demographics = "D";
                objUserMatch.Location_Demographics = "A";
                objUserMatch.Food_Advert = "B";
                objUserMatch.SweetSaltySnacks_Advert = "B";
                objUserMatch.AlcoholicDrinks_Advert = "B";
                objUserMatch.NonAlcoholicDrinks_Advert = "B";
                objUserMatch.Householdproducts_Advert = "B";
                objUserMatch.ToiletriesCosmetics_Advert = "B";
                objUserMatch.PharmaceuticalChemistsProducts_Advert = "B";
                objUserMatch.TobaccoProducts_Advert = "B";
                objUserMatch.PetsPetFood_Advert = "B";
                objUserMatch.ShoppingRetailClothing_Advert = "B";
                objUserMatch.DIYGardening_Advert = "B";
                objUserMatch.ElectronicsOtherPersonalItems_Advert = "B";
                objUserMatch.CommunicationsInternet_Advert = "B";
                objUserMatch.FinancialServices_Advert = "B";
                objUserMatch.HolidaysTravel_Advert = "B";
                objUserMatch.SportsLeisure_Advert = "B";
                objUserMatch.Motoring_Advert = "B";
                objUserMatch.Newspapers_Advert = "B";
                objUserMatch.TV_Advert = "B";
                objUserMatch.Cinema_Advert = "B";
                objUserMatch.SocialNetworking_Advert = "B";
                objUserMatch.Shopping_Advert = "B";
                objUserMatch.Fitness_Advert = "B";
                objUserMatch.Environment_Advert = "B";
                objUserMatch.GoingOut_Advert = "B";
                objUserMatch.Religion_Advert = "B";
                objUserMatch.Music_Advert = "B";
                objUserMatch.BusinessOrOpportunities_AdType = "B";
                objUserMatch.Gambling_AdType = "B";
                objUserMatch.Restaurants_AdType = "B";
                objUserMatch.Insurance_AdType = "B";
                objUserMatch.Furniture_AdType = "B";
                objUserMatch.InformationTechnology_AdType = "B";
                objUserMatch.Energy_AdType = "B";
                objUserMatch.Supermarkets_AdType = "B";
                objUserMatch.Healthcare_AdType = "B";
                objUserMatch.JobsAndEducation_AdType = "B";
                objUserMatch.Gifts_AdType = "B";
                objUserMatch.AdvocacyOrLegal_AdType = "B";
                objUserMatch.DatingAndPersonal_AdType = "B";
                objUserMatch.RealEstate_AdType = "B";
                objUserMatch.Games_AdType = "B";
                objUserMatch.Hustlers_AdType = "A";
                objUserMatch.Youth_AdType = "A";
                objUserMatch.DiscerningProfessionals_AdType = "A";
                objUserMatch.Mass_AdType = "A";
                objUserMatch.ContractType_Mobile = "A";
                objUserMatch.Spend_Mobile = "A";

                db.UserMatch3.Add(objUserMatch);
                db.SaveChanges();
                #endregion
            }
            else if (userMatchTableName == "UserMatch4")
            {
                #region UserMatch4
                UserMatch4 objUserMatch = new UserMatch4();

                objUserMatch.Email = userData.Email;
                objUserMatch.MSISDN = profilePref.MSISDN;
                objUserMatch.MSUserProfileId = profilePref.UserProfileId;
                objUserMatch.UserId = userData.UserId;
                objUserMatch.Gender_Demographics = "C";
                objUserMatch.WorkingStatus_Demographics = "I";
                objUserMatch.RelationshipStatus_Demographics = "G";
                objUserMatch.Education_Demographics = "E";
                objUserMatch.HouseholdStatus_Demographics = "D";
                objUserMatch.Location_Demographics = "A";
                objUserMatch.Food_Advert = "B";
                objUserMatch.SweetSaltySnacks_Advert = "B";
                objUserMatch.AlcoholicDrinks_Advert = "B";
                objUserMatch.NonAlcoholicDrinks_Advert = "B";
                objUserMatch.Householdproducts_Advert = "B";
                objUserMatch.ToiletriesCosmetics_Advert = "B";
                objUserMatch.PharmaceuticalChemistsProducts_Advert = "B";
                objUserMatch.TobaccoProducts_Advert = "B";
                objUserMatch.PetsPetFood_Advert = "B";
                objUserMatch.ShoppingRetailClothing_Advert = "B";
                objUserMatch.DIYGardening_Advert = "B";
                objUserMatch.ElectronicsOtherPersonalItems_Advert = "B";
                objUserMatch.CommunicationsInternet_Advert = "B";
                objUserMatch.FinancialServices_Advert = "B";
                objUserMatch.HolidaysTravel_Advert = "B";
                objUserMatch.SportsLeisure_Advert = "B";
                objUserMatch.Motoring_Advert = "B";
                objUserMatch.Newspapers_Advert = "B";
                objUserMatch.TV_Advert = "B";
                objUserMatch.Cinema_Advert = "B";
                objUserMatch.SocialNetworking_Advert = "B";
                objUserMatch.Shopping_Advert = "B";
                objUserMatch.Fitness_Advert = "B";
                objUserMatch.Environment_Advert = "B";
                objUserMatch.GoingOut_Advert = "B";
                objUserMatch.Religion_Advert = "B";
                objUserMatch.Music_Advert = "B";
                objUserMatch.BusinessOrOpportunities_AdType = "B";
                objUserMatch.Gambling_AdType = "B";
                objUserMatch.Restaurants_AdType = "B";
                objUserMatch.Insurance_AdType = "B";
                objUserMatch.Furniture_AdType = "B";
                objUserMatch.InformationTechnology_AdType = "B";
                objUserMatch.Energy_AdType = "B";
                objUserMatch.Supermarkets_AdType = "B";
                objUserMatch.Healthcare_AdType = "B";
                objUserMatch.JobsAndEducation_AdType = "B";
                objUserMatch.Gifts_AdType = "B";
                objUserMatch.AdvocacyOrLegal_AdType = "B";
                objUserMatch.DatingAndPersonal_AdType = "B";
                objUserMatch.RealEstate_AdType = "B";
                objUserMatch.Games_AdType = "B";
                objUserMatch.Hustlers_AdType = "A";
                objUserMatch.Youth_AdType = "A";
                objUserMatch.DiscerningProfessionals_AdType = "A";
                objUserMatch.Mass_AdType = "A";
                objUserMatch.ContractType_Mobile = "A";
                objUserMatch.Spend_Mobile = "A";

                db.UserMatch4.Add(objUserMatch);
                db.SaveChanges();
                #endregion
            }
            else if (userMatchTableName == "UserMatch5")
            {
                #region UserMatch5
                UserMatch5 objUserMatch = new UserMatch5();

                objUserMatch.Email = userData.Email;
                objUserMatch.MSISDN = profilePref.MSISDN;
                objUserMatch.MSUserProfileId = profilePref.UserProfileId;
                objUserMatch.UserId = userData.UserId;
                objUserMatch.Gender_Demographics = "C";
                objUserMatch.WorkingStatus_Demographics = "I";
                objUserMatch.RelationshipStatus_Demographics = "G";
                objUserMatch.Education_Demographics = "E";
                objUserMatch.HouseholdStatus_Demographics = "D";
                objUserMatch.Location_Demographics = "A";
                objUserMatch.Food_Advert = "B";
                objUserMatch.SweetSaltySnacks_Advert = "B";
                objUserMatch.AlcoholicDrinks_Advert = "B";
                objUserMatch.NonAlcoholicDrinks_Advert = "B";
                objUserMatch.Householdproducts_Advert = "B";
                objUserMatch.ToiletriesCosmetics_Advert = "B";
                objUserMatch.PharmaceuticalChemistsProducts_Advert = "B";
                objUserMatch.TobaccoProducts_Advert = "B";
                objUserMatch.PetsPetFood_Advert = "B";
                objUserMatch.ShoppingRetailClothing_Advert = "B";
                objUserMatch.DIYGardening_Advert = "B";
                objUserMatch.ElectronicsOtherPersonalItems_Advert = "B";
                objUserMatch.CommunicationsInternet_Advert = "B";
                objUserMatch.FinancialServices_Advert = "B";
                objUserMatch.HolidaysTravel_Advert = "B";
                objUserMatch.SportsLeisure_Advert = "B";
                objUserMatch.Motoring_Advert = "B";
                objUserMatch.Newspapers_Advert = "B";
                objUserMatch.TV_Advert = "B";
                objUserMatch.Cinema_Advert = "B";
                objUserMatch.SocialNetworking_Advert = "B";
                objUserMatch.Shopping_Advert = "B";
                objUserMatch.Fitness_Advert = "B";
                objUserMatch.Environment_Advert = "B";
                objUserMatch.GoingOut_Advert = "B";
                objUserMatch.Religion_Advert = "B";
                objUserMatch.Music_Advert = "B";
                objUserMatch.BusinessOrOpportunities_AdType = "B";
                objUserMatch.Gambling_AdType = "B";
                objUserMatch.Restaurants_AdType = "B";
                objUserMatch.Insurance_AdType = "B";
                objUserMatch.Furniture_AdType = "B";
                objUserMatch.InformationTechnology_AdType = "B";
                objUserMatch.Energy_AdType = "B";
                objUserMatch.Supermarkets_AdType = "B";
                objUserMatch.Healthcare_AdType = "B";
                objUserMatch.JobsAndEducation_AdType = "B";
                objUserMatch.Gifts_AdType = "B";
                objUserMatch.AdvocacyOrLegal_AdType = "B";
                objUserMatch.DatingAndPersonal_AdType = "B";
                objUserMatch.RealEstate_AdType = "B";
                objUserMatch.Games_AdType = "B";
                objUserMatch.Hustlers_AdType = "A";
                objUserMatch.Youth_AdType = "A";
                objUserMatch.DiscerningProfessionals_AdType = "A";
                objUserMatch.Mass_AdType = "A";
                objUserMatch.ContractType_Mobile = "A";
                objUserMatch.Spend_Mobile = "A";

                db.UserMatch5.Add(objUserMatch);
                db.SaveChanges();
                #endregion
            }
            else if (userMatchTableName == "UserMatch6")
            {
                #region UserMatch6
                UserMatch6 objUserMatch = new UserMatch6();

                objUserMatch.Email = userData.Email;
                objUserMatch.MSISDN = profilePref.MSISDN;
                objUserMatch.MSUserProfileId = profilePref.UserProfileId;
                objUserMatch.UserId = userData.UserId;
                objUserMatch.Gender_Demographics = "C";
                objUserMatch.WorkingStatus_Demographics = "I";
                objUserMatch.RelationshipStatus_Demographics = "G";
                objUserMatch.Education_Demographics = "E";
                objUserMatch.HouseholdStatus_Demographics = "D";
                objUserMatch.Location_Demographics = "A";
                objUserMatch.Food_Advert = "B";
                objUserMatch.SweetSaltySnacks_Advert = "B";
                objUserMatch.AlcoholicDrinks_Advert = "B";
                objUserMatch.NonAlcoholicDrinks_Advert = "B";
                objUserMatch.Householdproducts_Advert = "B";
                objUserMatch.ToiletriesCosmetics_Advert = "B";
                objUserMatch.PharmaceuticalChemistsProducts_Advert = "B";
                objUserMatch.TobaccoProducts_Advert = "B";
                objUserMatch.PetsPetFood_Advert = "B";
                objUserMatch.ShoppingRetailClothing_Advert = "B";
                objUserMatch.DIYGardening_Advert = "B";
                objUserMatch.ElectronicsOtherPersonalItems_Advert = "B";
                objUserMatch.CommunicationsInternet_Advert = "B";
                objUserMatch.FinancialServices_Advert = "B";
                objUserMatch.HolidaysTravel_Advert = "B";
                objUserMatch.SportsLeisure_Advert = "B";
                objUserMatch.Motoring_Advert = "B";
                objUserMatch.Newspapers_Advert = "B";
                objUserMatch.TV_Advert = "B";
                objUserMatch.Cinema_Advert = "B";
                objUserMatch.SocialNetworking_Advert = "B";
                objUserMatch.Shopping_Advert = "B";
                objUserMatch.Fitness_Advert = "B";
                objUserMatch.Environment_Advert = "B";
                objUserMatch.GoingOut_Advert = "B";
                objUserMatch.Religion_Advert = "B";
                objUserMatch.Music_Advert = "B";
                objUserMatch.BusinessOrOpportunities_AdType = "B";
                objUserMatch.Gambling_AdType = "B";
                objUserMatch.Restaurants_AdType = "B";
                objUserMatch.Insurance_AdType = "B";
                objUserMatch.Furniture_AdType = "B";
                objUserMatch.InformationTechnology_AdType = "B";
                objUserMatch.Energy_AdType = "B";
                objUserMatch.Supermarkets_AdType = "B";
                objUserMatch.Healthcare_AdType = "B";
                objUserMatch.JobsAndEducation_AdType = "B";
                objUserMatch.Gifts_AdType = "B";
                objUserMatch.AdvocacyOrLegal_AdType = "B";
                objUserMatch.DatingAndPersonal_AdType = "B";
                objUserMatch.RealEstate_AdType = "B";
                objUserMatch.Games_AdType = "B";
                objUserMatch.Hustlers_AdType = "A";
                objUserMatch.Youth_AdType = "A";
                objUserMatch.DiscerningProfessionals_AdType = "A";
                objUserMatch.Mass_AdType = "A";
                objUserMatch.ContractType_Mobile = "A";
                objUserMatch.Spend_Mobile = "A";

                db.UserMatch6.Add(objUserMatch);
                db.SaveChanges();
                #endregion
            }
            else if (userMatchTableName == "UserMatch7")
            {
                #region UserMatch7
                UserMatch7 objUserMatch = new UserMatch7();

                objUserMatch.Email = userData.Email;
                objUserMatch.MSISDN = profilePref.MSISDN;
                objUserMatch.MSUserProfileId = profilePref.UserProfileId;
                objUserMatch.UserId = userData.UserId;
                objUserMatch.Gender_Demographics = "C";
                objUserMatch.WorkingStatus_Demographics = "I";
                objUserMatch.RelationshipStatus_Demographics = "G";
                objUserMatch.Education_Demographics = "E";
                objUserMatch.HouseholdStatus_Demographics = "D";
                objUserMatch.Location_Demographics = "A";
                objUserMatch.Food_Advert = "B";
                objUserMatch.SweetSaltySnacks_Advert = "B";
                objUserMatch.AlcoholicDrinks_Advert = "B";
                objUserMatch.NonAlcoholicDrinks_Advert = "B";
                objUserMatch.Householdproducts_Advert = "B";
                objUserMatch.ToiletriesCosmetics_Advert = "B";
                objUserMatch.PharmaceuticalChemistsProducts_Advert = "B";
                objUserMatch.TobaccoProducts_Advert = "B";
                objUserMatch.PetsPetFood_Advert = "B";
                objUserMatch.ShoppingRetailClothing_Advert = "B";
                objUserMatch.DIYGardening_Advert = "B";
                objUserMatch.ElectronicsOtherPersonalItems_Advert = "B";
                objUserMatch.CommunicationsInternet_Advert = "B";
                objUserMatch.FinancialServices_Advert = "B";
                objUserMatch.HolidaysTravel_Advert = "B";
                objUserMatch.SportsLeisure_Advert = "B";
                objUserMatch.Motoring_Advert = "B";
                objUserMatch.Newspapers_Advert = "B";
                objUserMatch.TV_Advert = "B";
                objUserMatch.Cinema_Advert = "B";
                objUserMatch.SocialNetworking_Advert = "B";
                objUserMatch.Shopping_Advert = "B";
                objUserMatch.Fitness_Advert = "B";
                objUserMatch.Environment_Advert = "B";
                objUserMatch.GoingOut_Advert = "B";
                objUserMatch.Religion_Advert = "B";
                objUserMatch.Music_Advert = "B";
                objUserMatch.BusinessOrOpportunities_AdType = "B";
                objUserMatch.Gambling_AdType = "B";
                objUserMatch.Restaurants_AdType = "B";
                objUserMatch.Insurance_AdType = "B";
                objUserMatch.Furniture_AdType = "B";
                objUserMatch.InformationTechnology_AdType = "B";
                objUserMatch.Energy_AdType = "B";
                objUserMatch.Supermarkets_AdType = "B";
                objUserMatch.Healthcare_AdType = "B";
                objUserMatch.JobsAndEducation_AdType = "B";
                objUserMatch.Gifts_AdType = "B";
                objUserMatch.AdvocacyOrLegal_AdType = "B";
                objUserMatch.DatingAndPersonal_AdType = "B";
                objUserMatch.RealEstate_AdType = "B";
                objUserMatch.Games_AdType = "B";
                objUserMatch.Hustlers_AdType = "A";
                objUserMatch.Youth_AdType = "A";
                objUserMatch.DiscerningProfessionals_AdType = "A";
                objUserMatch.Mass_AdType = "A";
                objUserMatch.ContractType_Mobile = "A";
                objUserMatch.Spend_Mobile = "A";

                db.UserMatch7.Add(objUserMatch);
                db.SaveChanges();
                #endregion
            }
            else if (userMatchTableName == "UserMatch8")
            {
                #region UserMatch8
                UserMatch8 objUserMatch = new UserMatch8();

                objUserMatch.Email = userData.Email;
                objUserMatch.MSISDN = profilePref.MSISDN;
                objUserMatch.MSUserProfileId = profilePref.UserProfileId;
                objUserMatch.UserId = userData.UserId;
                objUserMatch.Gender_Demographics = "C";
                objUserMatch.WorkingStatus_Demographics = "I";
                objUserMatch.RelationshipStatus_Demographics = "G";
                objUserMatch.Education_Demographics = "E";
                objUserMatch.HouseholdStatus_Demographics = "D";
                objUserMatch.Location_Demographics = "A";
                objUserMatch.Food_Advert = "B";
                objUserMatch.SweetSaltySnacks_Advert = "B";
                objUserMatch.AlcoholicDrinks_Advert = "B";
                objUserMatch.NonAlcoholicDrinks_Advert = "B";
                objUserMatch.Householdproducts_Advert = "B";
                objUserMatch.ToiletriesCosmetics_Advert = "B";
                objUserMatch.PharmaceuticalChemistsProducts_Advert = "B";
                objUserMatch.TobaccoProducts_Advert = "B";
                objUserMatch.PetsPetFood_Advert = "B";
                objUserMatch.ShoppingRetailClothing_Advert = "B";
                objUserMatch.DIYGardening_Advert = "B";
                objUserMatch.ElectronicsOtherPersonalItems_Advert = "B";
                objUserMatch.CommunicationsInternet_Advert = "B";
                objUserMatch.FinancialServices_Advert = "B";
                objUserMatch.HolidaysTravel_Advert = "B";
                objUserMatch.SportsLeisure_Advert = "B";
                objUserMatch.Motoring_Advert = "B";
                objUserMatch.Newspapers_Advert = "B";
                objUserMatch.TV_Advert = "B";
                objUserMatch.Cinema_Advert = "B";
                objUserMatch.SocialNetworking_Advert = "B";
                objUserMatch.Shopping_Advert = "B";
                objUserMatch.Fitness_Advert = "B";
                objUserMatch.Environment_Advert = "B";
                objUserMatch.GoingOut_Advert = "B";
                objUserMatch.Religion_Advert = "B";
                objUserMatch.Music_Advert = "B";
                objUserMatch.BusinessOrOpportunities_AdType = "B";
                objUserMatch.Gambling_AdType = "B";
                objUserMatch.Restaurants_AdType = "B";
                objUserMatch.Insurance_AdType = "B";
                objUserMatch.Furniture_AdType = "B";
                objUserMatch.InformationTechnology_AdType = "B";
                objUserMatch.Energy_AdType = "B";
                objUserMatch.Supermarkets_AdType = "B";
                objUserMatch.Healthcare_AdType = "B";
                objUserMatch.JobsAndEducation_AdType = "B";
                objUserMatch.Gifts_AdType = "B";
                objUserMatch.AdvocacyOrLegal_AdType = "B";
                objUserMatch.DatingAndPersonal_AdType = "B";
                objUserMatch.RealEstate_AdType = "B";
                objUserMatch.Games_AdType = "B";
                objUserMatch.Hustlers_AdType = "A";
                objUserMatch.Youth_AdType = "A";
                objUserMatch.DiscerningProfessionals_AdType = "A";
                objUserMatch.Mass_AdType = "A";
                objUserMatch.ContractType_Mobile = "A";
                objUserMatch.Spend_Mobile = "A";

                db.UserMatch8.Add(objUserMatch);
                db.SaveChanges();
                #endregion
            }
            else if (userMatchTableName == "UserMatch9")
            {
                #region UserMatch9
                UserMatch9 objUserMatch = new UserMatch9();

                objUserMatch.Email = userData.Email;
                objUserMatch.MSISDN = profilePref.MSISDN;
                objUserMatch.MSUserProfileId = profilePref.UserProfileId;
                objUserMatch.UserId = userData.UserId;
                objUserMatch.Gender_Demographics = "C";
                objUserMatch.WorkingStatus_Demographics = "I";
                objUserMatch.RelationshipStatus_Demographics = "G";
                objUserMatch.Education_Demographics = "E";
                objUserMatch.HouseholdStatus_Demographics = "D";
                objUserMatch.Location_Demographics = "A";
                objUserMatch.Food_Advert = "B";
                objUserMatch.SweetSaltySnacks_Advert = "B";
                objUserMatch.AlcoholicDrinks_Advert = "B";
                objUserMatch.NonAlcoholicDrinks_Advert = "B";
                objUserMatch.Householdproducts_Advert = "B";
                objUserMatch.ToiletriesCosmetics_Advert = "B";
                objUserMatch.PharmaceuticalChemistsProducts_Advert = "B";
                objUserMatch.TobaccoProducts_Advert = "B";
                objUserMatch.PetsPetFood_Advert = "B";
                objUserMatch.ShoppingRetailClothing_Advert = "B";
                objUserMatch.DIYGardening_Advert = "B";
                objUserMatch.ElectronicsOtherPersonalItems_Advert = "B";
                objUserMatch.CommunicationsInternet_Advert = "B";
                objUserMatch.FinancialServices_Advert = "B";
                objUserMatch.HolidaysTravel_Advert = "B";
                objUserMatch.SportsLeisure_Advert = "B";
                objUserMatch.Motoring_Advert = "B";
                objUserMatch.Newspapers_Advert = "B";
                objUserMatch.TV_Advert = "B";
                objUserMatch.Cinema_Advert = "B";
                objUserMatch.SocialNetworking_Advert = "B";
                objUserMatch.Shopping_Advert = "B";
                objUserMatch.Fitness_Advert = "B";
                objUserMatch.Environment_Advert = "B";
                objUserMatch.GoingOut_Advert = "B";
                objUserMatch.Religion_Advert = "B";
                objUserMatch.Music_Advert = "B";
                objUserMatch.BusinessOrOpportunities_AdType = "B";
                objUserMatch.Gambling_AdType = "B";
                objUserMatch.Restaurants_AdType = "B";
                objUserMatch.Insurance_AdType = "B";
                objUserMatch.Furniture_AdType = "B";
                objUserMatch.InformationTechnology_AdType = "B";
                objUserMatch.Energy_AdType = "B";
                objUserMatch.Supermarkets_AdType = "B";
                objUserMatch.Healthcare_AdType = "B";
                objUserMatch.JobsAndEducation_AdType = "B";
                objUserMatch.Gifts_AdType = "B";
                objUserMatch.AdvocacyOrLegal_AdType = "B";
                objUserMatch.DatingAndPersonal_AdType = "B";
                objUserMatch.RealEstate_AdType = "B";
                objUserMatch.Games_AdType = "B";
                objUserMatch.Hustlers_AdType = "A";
                objUserMatch.Youth_AdType = "A";
                objUserMatch.DiscerningProfessionals_AdType = "A";
                objUserMatch.Mass_AdType = "A";
                objUserMatch.ContractType_Mobile = "A";
                objUserMatch.Spend_Mobile = "A";

                db.UserMatch9.Add(objUserMatch);
                db.SaveChanges();
                #endregion
            }
            else if (userMatchTableName == "UserMatch10")
            {
                #region UserMatch10
                UserMatch10 objUserMatch = new UserMatch10();

                objUserMatch.Email = userData.Email;
                objUserMatch.MSISDN = profilePref.MSISDN;
                objUserMatch.MSUserProfileId = profilePref.UserProfileId;
                objUserMatch.UserId = userData.UserId;
                objUserMatch.Gender_Demographics = "C";
                objUserMatch.WorkingStatus_Demographics = "I";
                objUserMatch.RelationshipStatus_Demographics = "G";
                objUserMatch.Education_Demographics = "E";
                objUserMatch.HouseholdStatus_Demographics = "D";
                objUserMatch.Location_Demographics = "A";
                objUserMatch.Food_Advert = "B";
                objUserMatch.SweetSaltySnacks_Advert = "B";
                objUserMatch.AlcoholicDrinks_Advert = "B";
                objUserMatch.NonAlcoholicDrinks_Advert = "B";
                objUserMatch.Householdproducts_Advert = "B";
                objUserMatch.ToiletriesCosmetics_Advert = "B";
                objUserMatch.PharmaceuticalChemistsProducts_Advert = "B";
                objUserMatch.TobaccoProducts_Advert = "B";
                objUserMatch.PetsPetFood_Advert = "B";
                objUserMatch.ShoppingRetailClothing_Advert = "B";
                objUserMatch.DIYGardening_Advert = "B";
                objUserMatch.ElectronicsOtherPersonalItems_Advert = "B";
                objUserMatch.CommunicationsInternet_Advert = "B";
                objUserMatch.FinancialServices_Advert = "B";
                objUserMatch.HolidaysTravel_Advert = "B";
                objUserMatch.SportsLeisure_Advert = "B";
                objUserMatch.Motoring_Advert = "B";
                objUserMatch.Newspapers_Advert = "B";
                objUserMatch.TV_Advert = "B";
                objUserMatch.Cinema_Advert = "B";
                objUserMatch.SocialNetworking_Advert = "B";
                objUserMatch.Shopping_Advert = "B";
                objUserMatch.Fitness_Advert = "B";
                objUserMatch.Environment_Advert = "B";
                objUserMatch.GoingOut_Advert = "B";
                objUserMatch.Religion_Advert = "B";
                objUserMatch.Music_Advert = "B";
                objUserMatch.BusinessOrOpportunities_AdType = "B";
                objUserMatch.Gambling_AdType = "B";
                objUserMatch.Restaurants_AdType = "B";
                objUserMatch.Insurance_AdType = "B";
                objUserMatch.Furniture_AdType = "B";
                objUserMatch.InformationTechnology_AdType = "B";
                objUserMatch.Energy_AdType = "B";
                objUserMatch.Supermarkets_AdType = "B";
                objUserMatch.Healthcare_AdType = "B";
                objUserMatch.JobsAndEducation_AdType = "B";
                objUserMatch.Gifts_AdType = "B";
                objUserMatch.AdvocacyOrLegal_AdType = "B";
                objUserMatch.DatingAndPersonal_AdType = "B";
                objUserMatch.RealEstate_AdType = "B";
                objUserMatch.Games_AdType = "B";
                objUserMatch.Hustlers_AdType = "A";
                objUserMatch.Youth_AdType = "A";
                objUserMatch.DiscerningProfessionals_AdType = "A";
                objUserMatch.Mass_AdType = "A";
                objUserMatch.ContractType_Mobile = "A";
                objUserMatch.Spend_Mobile = "A";

                db.UserMatch10.Add(objUserMatch);
                db.SaveChanges();
                #endregion
            }          
        }

        public void UpdateMobileData(UserProfileMobileFormModel model, User user,EFMVCDataContex SQLServerEntities)
        {
            if (user.UserMatchTableName == "UserMatch1")
            {
                #region UserMatch1
                var GetUserProfileForMobile = SQLServerEntities.UserMatch.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForMobile != null)
                {
                    GetUserProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                    GetUserProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                    GetUserProfileForMobile.UserId = user.UserId;
                    GetUserProfileForMobile.Email = user.Email;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch();
                    usermatch.ContractType_Mobile = model.ContractType_Mobile;
                    usermatch.Spend_Mobile = model.Spend_Mobile;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    SQLServerEntities.UserMatch.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch2")
            {
                #region UserMatch2
                var GetUserProfileForMobile = SQLServerEntities.UserMatch2.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForMobile != null)
                {
                    GetUserProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                    GetUserProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                    GetUserProfileForMobile.UserId = user.UserId;
                    GetUserProfileForMobile.Email = user.Email;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch2();
                    usermatch.ContractType_Mobile = model.ContractType_Mobile;
                    usermatch.Spend_Mobile = model.Spend_Mobile;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    SQLServerEntities.UserMatch2.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }

                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch3")
            {
                #region UserMatch3
                var GetUserProfileForMobile = SQLServerEntities.UserMatch3.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForMobile != null)
                {
                    GetUserProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                    GetUserProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                    GetUserProfileForMobile.UserId = user.UserId;
                    GetUserProfileForMobile.Email = user.Email;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch3();
                    usermatch.ContractType_Mobile = model.ContractType_Mobile;
                    usermatch.Spend_Mobile = model.Spend_Mobile;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    SQLServerEntities.UserMatch3.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch4")
            {
                #region UserMatch4
                var GetUserProfileForMobile = SQLServerEntities.UserMatch4.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForMobile != null)
                {
                    GetUserProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                    GetUserProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                    GetUserProfileForMobile.UserId = user.UserId;
                    GetUserProfileForMobile.Email = user.Email;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch4();
                    usermatch.ContractType_Mobile = model.ContractType_Mobile;
                    usermatch.Spend_Mobile = model.Spend_Mobile;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    SQLServerEntities.UserMatch4.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch5")
            {
                #region UserMatch5
                var GetUserProfileForMobile = SQLServerEntities.UserMatch5.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForMobile != null)
                {
                    GetUserProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                    GetUserProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                    GetUserProfileForMobile.UserId = user.UserId;
                    GetUserProfileForMobile.Email = user.Email;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch5();
                    usermatch.ContractType_Mobile = model.ContractType_Mobile;
                    usermatch.Spend_Mobile = model.Spend_Mobile;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    SQLServerEntities.UserMatch5.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch6")
            {
                #region UserMatch6
                var GetUserProfileForMobile = SQLServerEntities.UserMatch6.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForMobile != null)
                {
                    GetUserProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                    GetUserProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                    GetUserProfileForMobile.UserId = user.UserId;
                    GetUserProfileForMobile.Email = user.Email;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch6();
                    usermatch.ContractType_Mobile = model.ContractType_Mobile;
                    usermatch.Spend_Mobile = model.Spend_Mobile;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    SQLServerEntities.UserMatch6.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch7")
            {
                #region UserMatch7
                var GetUserProfileForMobile = SQLServerEntities.UserMatch7.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForMobile != null)
                {
                    GetUserProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                    GetUserProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                    GetUserProfileForMobile.UserId = user.UserId;
                    GetUserProfileForMobile.Email = user.Email;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch7();
                    usermatch.ContractType_Mobile = model.ContractType_Mobile;
                    usermatch.Spend_Mobile = model.Spend_Mobile;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    SQLServerEntities.UserMatch7.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch8")
            {
                #region UserMatch8
                var GetUserProfileForMobile = SQLServerEntities.UserMatch8.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForMobile != null)
                {
                    GetUserProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                    GetUserProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                    GetUserProfileForMobile.UserId = user.UserId;
                    GetUserProfileForMobile.Email = user.Email;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch8();
                    usermatch.ContractType_Mobile = model.ContractType_Mobile;
                    usermatch.Spend_Mobile = model.Spend_Mobile;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    SQLServerEntities.UserMatch8.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch9")
            {
                #region UserMatch9
                var GetUserProfileForMobile = SQLServerEntities.UserMatch9.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForMobile != null)
                {
                    GetUserProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                    GetUserProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                    GetUserProfileForMobile.UserId = user.UserId;
                    GetUserProfileForMobile.Email = user.Email;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch9();
                    usermatch.ContractType_Mobile = model.ContractType_Mobile;
                    usermatch.Spend_Mobile = model.Spend_Mobile;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    SQLServerEntities.UserMatch9.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch10")
            {
                #region UserMatch10
                var GetUserProfileForMobile = SQLServerEntities.UserMatch10.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForMobile != null)
                {
                    GetUserProfileForMobile.ContractType_Mobile = model.ContractType_Mobile;
                    GetUserProfileForMobile.Spend_Mobile = model.Spend_Mobile;
                    GetUserProfileForMobile.UserId = user.UserId;
                    GetUserProfileForMobile.Email = user.Email;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch10();
                    usermatch.ContractType_Mobile = model.ContractType_Mobile;
                    usermatch.Spend_Mobile = model.Spend_Mobile;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    SQLServerEntities.UserMatch10.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
        }

        public void UpdateDemographicData(UserProfileDemographicFormModel model, User user, EFMVCDataContex SQLServerEntities)
        {
            if (user.UserMatchTableName == "UserMatch1")
            {
                #region UserMatch1
                var Getusermatch = SQLServerEntities.UserMatch.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = model.Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = model.UserProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = model.Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch2")
            {
                #region UserMatch2
                var Getusermatch = SQLServerEntities.UserMatch2.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = model.Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = model.UserProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch2();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = model.Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch2.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }

                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch3")
            {
                #region UserMatch3
                var Getusermatch = SQLServerEntities.UserMatch3.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = model.Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = model.UserProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch3();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = model.Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch3.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch4")
            {
                #region UserMatch4
                var Getusermatch = SQLServerEntities.UserMatch4.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = model.Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = model.UserProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch4();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = model.Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch4.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch5")
            {
                #region UserMatch5
                var Getusermatch = SQLServerEntities.UserMatch5.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = model.Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = model.UserProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch5();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = model.Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch5.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch6")
            {
                #region UserMatch6
                var Getusermatch = SQLServerEntities.UserMatch6.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = model.Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = model.UserProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch6();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = model.Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch6.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch7")
            {
                #region UserMatch7
                var Getusermatch = SQLServerEntities.UserMatch7.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = model.Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = model.UserProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch7();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = model.Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch7.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch8")
            {
                #region UserMatch8
                var Getusermatch = SQLServerEntities.UserMatch8.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = model.Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = model.UserProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch8();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = model.Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch8.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch9")
            {
                #region UserMatch9
                var Getusermatch = SQLServerEntities.UserMatch9.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = model.Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = model.UserProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch9();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = model.Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch9.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch10")
            {
                #region UserMatch10
                var Getusermatch = SQLServerEntities.UserMatch10.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = model.Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = model.UserProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch10();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = model.Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch10.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
        }

        public void UpdatePersonalInfoData(UserProfileFormModel model, User user, int userProfileId, EFMVCDataContex SQLServerEntities)
        {
            string Age = null;
            if (model.DOB != null)
            {
                DateTime now = DateTime.Today;
                int age = now.Year - model.DOB.Value.Year;
                if (age < 18)
                    Age = "A";
                else if (age >= 18 && age <= 24)
                    Age = "B";
                else if (age >= 25 && age <= 34)
                    Age = "C";
                else if (age >= 35 && age <= 44)
                    Age = "D";
                else if (age >= 45 && age <= 54)
                    Age = "E";
                else if (age >= 55 && age <= 64)
                    Age = "F";
                else if (age >= 65)
                    Age = "G";
                else
                    Age = "H";
            }

            if (user.UserMatchTableName == "UserMatch1")
            {
                #region UserMatch1
                var Getusermatch = SQLServerEntities.UserMatch.Where(s => s.MSUserProfileId == userProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = userProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = userProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch2")
            {
                #region UserMatch2
                var Getusermatch = SQLServerEntities.UserMatch2.Where(s => s.MSUserProfileId == userProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = userProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch2();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = userProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch2.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch3")
            {
                #region UserMatch3
                var Getusermatch = SQLServerEntities.UserMatch3.Where(s => s.MSUserProfileId == userProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = userProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch3();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = userProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch3.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch4")
            {
                #region UserMatch4
                var Getusermatch = SQLServerEntities.UserMatch4.Where(s => s.MSUserProfileId == userProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = userProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch4();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = userProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch4.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch5")
            {
                #region UserMatch5
                var Getusermatch = SQLServerEntities.UserMatch5.Where(s => s.MSUserProfileId == userProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = userProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch5();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = userProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch5.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch6")
            {
                #region UserMatch6
                var Getusermatch = SQLServerEntities.UserMatch6.Where(s => s.MSUserProfileId == userProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = userProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch6();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = userProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch6.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch7")
            {
                #region UserMatch7
                var Getusermatch = SQLServerEntities.UserMatch7.Where(s => s.MSUserProfileId == userProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = userProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch7();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = userProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch7.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch8")
            {
                #region UserMatch8
                var Getusermatch = SQLServerEntities.UserMatch8.Where(s => s.MSUserProfileId == userProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = userProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch8();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = userProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch8.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch9")
            {
                #region UserMatch9
                var Getusermatch = SQLServerEntities.UserMatch9.Where(s => s.MSUserProfileId == userProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = userProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch9();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = userProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch9.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch10")
            {
                #region UserMatch10
                var Getusermatch = SQLServerEntities.UserMatch10.Where(s => s.MSUserProfileId == userProfileId).FirstOrDefault();
                if (Getusermatch != null)
                {
                    Getusermatch.MSISDN = model.MSISDN;
                    Getusermatch.DOB = model.DOB;
                    Getusermatch.Age_Demographics = Age;
                    Getusermatch.Education_Demographics = model.Education;
                    Getusermatch.Gender_Demographics = model.Gender;
                    Getusermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    Getusermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    Getusermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    Getusermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    Getusermatch.UserId = user.UserId;
                    Getusermatch.MSUserProfileId = userProfileId;
                    Getusermatch.Email = user.Email;
                    Getusermatch.Location_Demographics = model.Location;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch10();
                    usermatch.MSISDN = model.MSISDN;
                    usermatch.DOB = model.DOB;
                    usermatch.Age_Demographics = Age;
                    usermatch.Education_Demographics = model.Education;
                    usermatch.Gender_Demographics = model.Gender;
                    usermatch.HouseholdStatus_Demographics = model.HouseholdStatus;
                    usermatch.IncomeBracket_Demographics = model.IncomeBracket;
                    usermatch.RelationshipStatus_Demographics = model.RelationshipStatus;
                    usermatch.WorkingStatus_Demographics = model.WorkingStatus;
                    usermatch.UserId = user.UserId;
                    usermatch.MSUserProfileId = userProfileId;
                    usermatch.Email = user.Email;
                    usermatch.Location_Demographics = model.Location;
                    SQLServerEntities.UserMatch10.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
        }

        public void UpdateAdTypes(UserProfileAdvertFormModel model, User user, EFMVCDataContex SQLServerEntities)
        {
            if (user.UserMatchTableName == "UserMatch1")
            {
                #region UserMatch1
                var GetUserProfileForAdverts = SQLServerEntities.UserMatch.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForAdverts != null)
                {                    
                    GetUserProfileForAdverts.Food_Advert = model.Food_Advert;
                    GetUserProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    GetUserProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                    GetUserProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    GetUserProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    GetUserProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    GetUserProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    GetUserProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    GetUserProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                    GetUserProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    GetUserProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    GetUserProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                    GetUserProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    GetUserProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    GetUserProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                    GetUserProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                    GetUserProfileForAdverts.TV_Advert = model.TV_Advert;
                    GetUserProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                    GetUserProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    GetUserProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                    GetUserProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                    GetUserProfileForAdverts.Environment_Advert = model.Environment_Advert;
                    GetUserProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                    GetUserProfileForAdverts.Religion_Advert = model.Religion_Advert;
                    GetUserProfileForAdverts.Music_Advert = model.Music_Advert;
                    GetUserProfileForAdverts.UserId = user.UserId;
                    GetUserProfileForAdverts.Email = user.Email;
                    GetUserProfileForAdverts.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    GetUserProfileForAdverts.Gambling_AdType = model.Gambling_AdType;
                    GetUserProfileForAdverts.Restaurants_AdType = model.Restaurants_AdType;
                    GetUserProfileForAdverts.Insurance_AdType = model.Insurance_AdType;
                    GetUserProfileForAdverts.Furniture_AdType = model.Furniture_AdType;
                    GetUserProfileForAdverts.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    GetUserProfileForAdverts.Energy_AdType = model.Energy_AdType;
                    GetUserProfileForAdverts.Supermarkets_AdType = model.Supermarkets_AdType;
                    GetUserProfileForAdverts.Healthcare_AdType = model.Healthcare_AdType;
                    GetUserProfileForAdverts.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    GetUserProfileForAdverts.Gifts_AdType = model.Gifts_AdType;
                    GetUserProfileForAdverts.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    GetUserProfileForAdverts.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    GetUserProfileForAdverts.RealEstate_AdType = model.RealEstate_AdType;
                    GetUserProfileForAdverts.Games_AdType = model.Games_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch();
                    usermatch.Food_Advert = model.Food_Advert;
                    usermatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    usermatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    usermatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    usermatch.Householdproducts_Advert = model.Householdproducts_Advert;
                    usermatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    usermatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    usermatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    usermatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    usermatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    usermatch.DIYGardening_Advert = model.DIYGardening_Advert;
                    usermatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    usermatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    usermatch.FinancialServices_Advert = model.FinancialServices_Advert;
                    usermatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    usermatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    usermatch.Motoring_Advert = model.Motoring_Advert;
                    usermatch.Newspapers_Advert = model.Newspapers_Advert;
                    usermatch.TV_Advert = model.TV_Advert;
                    usermatch.Cinema_Advert = model.Cinema_Advert;
                    usermatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    usermatch.Shopping_Advert = model.Shopping_Advert;
                    usermatch.Fitness_Advert = model.Fitness_Advert;
                    usermatch.Environment_Advert = model.Environment_Advert;
                    usermatch.GoingOut_Advert = model.GoingOut_Advert;
                    usermatch.Religion_Advert = model.Religion_Advert;
                    usermatch.Music_Advert = model.Music_Advert;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    usermatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    usermatch.Gambling_AdType = model.Gambling_AdType;
                    usermatch.Restaurants_AdType = model.Restaurants_AdType;
                    usermatch.Insurance_AdType = model.Insurance_AdType;
                    usermatch.Furniture_AdType = model.Furniture_AdType;
                    usermatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    usermatch.Energy_AdType = model.Energy_AdType;
                    usermatch.Supermarkets_AdType = model.Supermarkets_AdType;
                    usermatch.Healthcare_AdType = model.Healthcare_AdType;
                    usermatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    usermatch.Gifts_AdType = model.Gifts_AdType;
                    usermatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    usermatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    usermatch.RealEstate_AdType = model.RealEstate_AdType;
                    usermatch.Games_AdType = model.Games_AdType;
                    SQLServerEntities.UserMatch.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch2")
            {
                #region UserMatch2
                var GetUserProfileForAdverts = SQLServerEntities.UserMatch2.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForAdverts != null)
                {
                    GetUserProfileForAdverts.Food_Advert = model.Food_Advert;
                    GetUserProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    GetUserProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                    GetUserProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    GetUserProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    GetUserProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    GetUserProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    GetUserProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    GetUserProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                    GetUserProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    GetUserProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    GetUserProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                    GetUserProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    GetUserProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    GetUserProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                    GetUserProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                    GetUserProfileForAdverts.TV_Advert = model.TV_Advert;
                    GetUserProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                    GetUserProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    GetUserProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                    GetUserProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                    GetUserProfileForAdverts.Environment_Advert = model.Environment_Advert;
                    GetUserProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                    GetUserProfileForAdverts.Religion_Advert = model.Religion_Advert;
                    GetUserProfileForAdverts.Music_Advert = model.Music_Advert;
                    GetUserProfileForAdverts.UserId = user.UserId;
                    GetUserProfileForAdverts.Email = user.Email;
                    GetUserProfileForAdverts.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    GetUserProfileForAdverts.Gambling_AdType = model.Gambling_AdType;
                    GetUserProfileForAdverts.Restaurants_AdType = model.Restaurants_AdType;
                    GetUserProfileForAdverts.Insurance_AdType = model.Insurance_AdType;
                    GetUserProfileForAdverts.Furniture_AdType = model.Furniture_AdType;
                    GetUserProfileForAdverts.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    GetUserProfileForAdverts.Energy_AdType = model.Energy_AdType;
                    GetUserProfileForAdverts.Supermarkets_AdType = model.Supermarkets_AdType;
                    GetUserProfileForAdverts.Healthcare_AdType = model.Healthcare_AdType;
                    GetUserProfileForAdverts.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    GetUserProfileForAdverts.Gifts_AdType = model.Gifts_AdType;
                    GetUserProfileForAdverts.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    GetUserProfileForAdverts.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    GetUserProfileForAdverts.RealEstate_AdType = model.RealEstate_AdType;
                    GetUserProfileForAdverts.Games_AdType = model.Games_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch2();
                    usermatch.Food_Advert = model.Food_Advert;
                    usermatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    usermatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    usermatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    usermatch.Householdproducts_Advert = model.Householdproducts_Advert;
                    usermatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    usermatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    usermatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    usermatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    usermatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    usermatch.DIYGardening_Advert = model.DIYGardening_Advert;
                    usermatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    usermatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    usermatch.FinancialServices_Advert = model.FinancialServices_Advert;
                    usermatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    usermatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    usermatch.Motoring_Advert = model.Motoring_Advert;
                    usermatch.Newspapers_Advert = model.Newspapers_Advert;
                    usermatch.TV_Advert = model.TV_Advert;
                    usermatch.Cinema_Advert = model.Cinema_Advert;
                    usermatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    usermatch.Shopping_Advert = model.Shopping_Advert;
                    usermatch.Fitness_Advert = model.Fitness_Advert;
                    usermatch.Environment_Advert = model.Environment_Advert;
                    usermatch.GoingOut_Advert = model.GoingOut_Advert;
                    usermatch.Religion_Advert = model.Religion_Advert;
                    usermatch.Music_Advert = model.Music_Advert;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    usermatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    usermatch.Gambling_AdType = model.Gambling_AdType;
                    usermatch.Restaurants_AdType = model.Restaurants_AdType;
                    usermatch.Insurance_AdType = model.Insurance_AdType;
                    usermatch.Furniture_AdType = model.Furniture_AdType;
                    usermatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    usermatch.Energy_AdType = model.Energy_AdType;
                    usermatch.Supermarkets_AdType = model.Supermarkets_AdType;
                    usermatch.Healthcare_AdType = model.Healthcare_AdType;
                    usermatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    usermatch.Gifts_AdType = model.Gifts_AdType;
                    usermatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    usermatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    usermatch.RealEstate_AdType = model.RealEstate_AdType;
                    usermatch.Games_AdType = model.Games_AdType;
                    SQLServerEntities.UserMatch2.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch3")
            {
                #region UserMatch3
                var GetUserProfileForAdverts = SQLServerEntities.UserMatch3.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForAdverts != null)
                {
                    GetUserProfileForAdverts.Food_Advert = model.Food_Advert;
                    GetUserProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    GetUserProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                    GetUserProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    GetUserProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    GetUserProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    GetUserProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    GetUserProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    GetUserProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                    GetUserProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    GetUserProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    GetUserProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                    GetUserProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    GetUserProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    GetUserProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                    GetUserProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                    GetUserProfileForAdverts.TV_Advert = model.TV_Advert;
                    GetUserProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                    GetUserProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    GetUserProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                    GetUserProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                    GetUserProfileForAdverts.Environment_Advert = model.Environment_Advert;
                    GetUserProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                    GetUserProfileForAdverts.Religion_Advert = model.Religion_Advert;
                    GetUserProfileForAdverts.Music_Advert = model.Music_Advert;
                    GetUserProfileForAdverts.UserId = user.UserId;
                    GetUserProfileForAdverts.Email = user.Email;
                    GetUserProfileForAdverts.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    GetUserProfileForAdverts.Gambling_AdType = model.Gambling_AdType;
                    GetUserProfileForAdverts.Restaurants_AdType = model.Restaurants_AdType;
                    GetUserProfileForAdverts.Insurance_AdType = model.Insurance_AdType;
                    GetUserProfileForAdverts.Furniture_AdType = model.Furniture_AdType;
                    GetUserProfileForAdverts.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    GetUserProfileForAdverts.Energy_AdType = model.Energy_AdType;
                    GetUserProfileForAdverts.Supermarkets_AdType = model.Supermarkets_AdType;
                    GetUserProfileForAdverts.Healthcare_AdType = model.Healthcare_AdType;
                    GetUserProfileForAdverts.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    GetUserProfileForAdverts.Gifts_AdType = model.Gifts_AdType;
                    GetUserProfileForAdverts.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    GetUserProfileForAdverts.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    GetUserProfileForAdverts.RealEstate_AdType = model.RealEstate_AdType;
                    GetUserProfileForAdverts.Games_AdType = model.Games_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch3();
                    usermatch.Food_Advert = model.Food_Advert;
                    usermatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    usermatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    usermatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    usermatch.Householdproducts_Advert = model.Householdproducts_Advert;
                    usermatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    usermatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    usermatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    usermatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    usermatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    usermatch.DIYGardening_Advert = model.DIYGardening_Advert;
                    usermatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    usermatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    usermatch.FinancialServices_Advert = model.FinancialServices_Advert;
                    usermatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    usermatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    usermatch.Motoring_Advert = model.Motoring_Advert;
                    usermatch.Newspapers_Advert = model.Newspapers_Advert;
                    usermatch.TV_Advert = model.TV_Advert;
                    usermatch.Cinema_Advert = model.Cinema_Advert;
                    usermatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    usermatch.Shopping_Advert = model.Shopping_Advert;
                    usermatch.Fitness_Advert = model.Fitness_Advert;
                    usermatch.Environment_Advert = model.Environment_Advert;
                    usermatch.GoingOut_Advert = model.GoingOut_Advert;
                    usermatch.Religion_Advert = model.Religion_Advert;
                    usermatch.Music_Advert = model.Music_Advert;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    usermatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    usermatch.Gambling_AdType = model.Gambling_AdType;
                    usermatch.Restaurants_AdType = model.Restaurants_AdType;
                    usermatch.Insurance_AdType = model.Insurance_AdType;
                    usermatch.Furniture_AdType = model.Furniture_AdType;
                    usermatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    usermatch.Energy_AdType = model.Energy_AdType;
                    usermatch.Supermarkets_AdType = model.Supermarkets_AdType;
                    usermatch.Healthcare_AdType = model.Healthcare_AdType;
                    usermatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    usermatch.Gifts_AdType = model.Gifts_AdType;
                    usermatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    usermatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    usermatch.RealEstate_AdType = model.RealEstate_AdType;
                    usermatch.Games_AdType = model.Games_AdType;
                    SQLServerEntities.UserMatch3.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch4")
            {
                #region UserMatch4
                var GetUserProfileForAdverts = SQLServerEntities.UserMatch4.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForAdverts != null)
                {
                    GetUserProfileForAdverts.Food_Advert = model.Food_Advert;
                    GetUserProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    GetUserProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                    GetUserProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    GetUserProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    GetUserProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    GetUserProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    GetUserProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    GetUserProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                    GetUserProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    GetUserProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    GetUserProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                    GetUserProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    GetUserProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    GetUserProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                    GetUserProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                    GetUserProfileForAdverts.TV_Advert = model.TV_Advert;
                    GetUserProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                    GetUserProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    GetUserProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                    GetUserProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                    GetUserProfileForAdverts.Environment_Advert = model.Environment_Advert;
                    GetUserProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                    GetUserProfileForAdverts.Religion_Advert = model.Religion_Advert;
                    GetUserProfileForAdverts.Music_Advert = model.Music_Advert;
                    GetUserProfileForAdverts.UserId = user.UserId;
                    GetUserProfileForAdverts.Email = user.Email;
                    GetUserProfileForAdverts.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    GetUserProfileForAdverts.Gambling_AdType = model.Gambling_AdType;
                    GetUserProfileForAdverts.Restaurants_AdType = model.Restaurants_AdType;
                    GetUserProfileForAdverts.Insurance_AdType = model.Insurance_AdType;
                    GetUserProfileForAdverts.Furniture_AdType = model.Furniture_AdType;
                    GetUserProfileForAdverts.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    GetUserProfileForAdverts.Energy_AdType = model.Energy_AdType;
                    GetUserProfileForAdverts.Supermarkets_AdType = model.Supermarkets_AdType;
                    GetUserProfileForAdverts.Healthcare_AdType = model.Healthcare_AdType;
                    GetUserProfileForAdverts.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    GetUserProfileForAdverts.Gifts_AdType = model.Gifts_AdType;
                    GetUserProfileForAdverts.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    GetUserProfileForAdverts.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    GetUserProfileForAdverts.RealEstate_AdType = model.RealEstate_AdType;
                    GetUserProfileForAdverts.Games_AdType = model.Games_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch4();
                    usermatch.Food_Advert = model.Food_Advert;
                    usermatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    usermatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    usermatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    usermatch.Householdproducts_Advert = model.Householdproducts_Advert;
                    usermatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    usermatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    usermatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    usermatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    usermatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    usermatch.DIYGardening_Advert = model.DIYGardening_Advert;
                    usermatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    usermatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    usermatch.FinancialServices_Advert = model.FinancialServices_Advert;
                    usermatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    usermatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    usermatch.Motoring_Advert = model.Motoring_Advert;
                    usermatch.Newspapers_Advert = model.Newspapers_Advert;
                    usermatch.TV_Advert = model.TV_Advert;
                    usermatch.Cinema_Advert = model.Cinema_Advert;
                    usermatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    usermatch.Shopping_Advert = model.Shopping_Advert;
                    usermatch.Fitness_Advert = model.Fitness_Advert;
                    usermatch.Environment_Advert = model.Environment_Advert;
                    usermatch.GoingOut_Advert = model.GoingOut_Advert;
                    usermatch.Religion_Advert = model.Religion_Advert;
                    usermatch.Music_Advert = model.Music_Advert;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    usermatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    usermatch.Gambling_AdType = model.Gambling_AdType;
                    usermatch.Restaurants_AdType = model.Restaurants_AdType;
                    usermatch.Insurance_AdType = model.Insurance_AdType;
                    usermatch.Furniture_AdType = model.Furniture_AdType;
                    usermatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    usermatch.Energy_AdType = model.Energy_AdType;
                    usermatch.Supermarkets_AdType = model.Supermarkets_AdType;
                    usermatch.Healthcare_AdType = model.Healthcare_AdType;
                    usermatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    usermatch.Gifts_AdType = model.Gifts_AdType;
                    usermatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    usermatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    usermatch.RealEstate_AdType = model.RealEstate_AdType;
                    usermatch.Games_AdType = model.Games_AdType;
                    SQLServerEntities.UserMatch4.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch5")
            {
                #region UserMatch5
                var GetUserProfileForAdverts = SQLServerEntities.UserMatch5.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForAdverts != null)
                {
                    GetUserProfileForAdverts.Food_Advert = model.Food_Advert;
                    GetUserProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    GetUserProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                    GetUserProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    GetUserProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    GetUserProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    GetUserProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    GetUserProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    GetUserProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                    GetUserProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    GetUserProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    GetUserProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                    GetUserProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    GetUserProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    GetUserProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                    GetUserProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                    GetUserProfileForAdverts.TV_Advert = model.TV_Advert;
                    GetUserProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                    GetUserProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    GetUserProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                    GetUserProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                    GetUserProfileForAdverts.Environment_Advert = model.Environment_Advert;
                    GetUserProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                    GetUserProfileForAdverts.Religion_Advert = model.Religion_Advert;
                    GetUserProfileForAdverts.Music_Advert = model.Music_Advert;
                    GetUserProfileForAdverts.UserId = user.UserId;
                    GetUserProfileForAdverts.Email = user.Email;
                    GetUserProfileForAdverts.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    GetUserProfileForAdverts.Gambling_AdType = model.Gambling_AdType;
                    GetUserProfileForAdverts.Restaurants_AdType = model.Restaurants_AdType;
                    GetUserProfileForAdverts.Insurance_AdType = model.Insurance_AdType;
                    GetUserProfileForAdverts.Furniture_AdType = model.Furniture_AdType;
                    GetUserProfileForAdverts.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    GetUserProfileForAdverts.Energy_AdType = model.Energy_AdType;
                    GetUserProfileForAdverts.Supermarkets_AdType = model.Supermarkets_AdType;
                    GetUserProfileForAdverts.Healthcare_AdType = model.Healthcare_AdType;
                    GetUserProfileForAdverts.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    GetUserProfileForAdverts.Gifts_AdType = model.Gifts_AdType;
                    GetUserProfileForAdverts.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    GetUserProfileForAdverts.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    GetUserProfileForAdverts.RealEstate_AdType = model.RealEstate_AdType;
                    GetUserProfileForAdverts.Games_AdType = model.Games_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch5();
                    usermatch.Food_Advert = model.Food_Advert;
                    usermatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    usermatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    usermatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    usermatch.Householdproducts_Advert = model.Householdproducts_Advert;
                    usermatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    usermatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    usermatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    usermatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    usermatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    usermatch.DIYGardening_Advert = model.DIYGardening_Advert;
                    usermatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    usermatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    usermatch.FinancialServices_Advert = model.FinancialServices_Advert;
                    usermatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    usermatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    usermatch.Motoring_Advert = model.Motoring_Advert;
                    usermatch.Newspapers_Advert = model.Newspapers_Advert;
                    usermatch.TV_Advert = model.TV_Advert;
                    usermatch.Cinema_Advert = model.Cinema_Advert;
                    usermatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    usermatch.Shopping_Advert = model.Shopping_Advert;
                    usermatch.Fitness_Advert = model.Fitness_Advert;
                    usermatch.Environment_Advert = model.Environment_Advert;
                    usermatch.GoingOut_Advert = model.GoingOut_Advert;
                    usermatch.Religion_Advert = model.Religion_Advert;
                    usermatch.Music_Advert = model.Music_Advert;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    usermatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    usermatch.Gambling_AdType = model.Gambling_AdType;
                    usermatch.Restaurants_AdType = model.Restaurants_AdType;
                    usermatch.Insurance_AdType = model.Insurance_AdType;
                    usermatch.Furniture_AdType = model.Furniture_AdType;
                    usermatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    usermatch.Energy_AdType = model.Energy_AdType;
                    usermatch.Supermarkets_AdType = model.Supermarkets_AdType;
                    usermatch.Healthcare_AdType = model.Healthcare_AdType;
                    usermatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    usermatch.Gifts_AdType = model.Gifts_AdType;
                    usermatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    usermatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    usermatch.RealEstate_AdType = model.RealEstate_AdType;
                    usermatch.Games_AdType = model.Games_AdType;
                    SQLServerEntities.UserMatch5.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch6")
            {
                #region UserMatch6
                var GetUserProfileForAdverts = SQLServerEntities.UserMatch6.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForAdverts != null)
                {
                    GetUserProfileForAdverts.Food_Advert = model.Food_Advert;
                    GetUserProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    GetUserProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                    GetUserProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    GetUserProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    GetUserProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    GetUserProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    GetUserProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    GetUserProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                    GetUserProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    GetUserProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    GetUserProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                    GetUserProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    GetUserProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    GetUserProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                    GetUserProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                    GetUserProfileForAdverts.TV_Advert = model.TV_Advert;
                    GetUserProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                    GetUserProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    GetUserProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                    GetUserProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                    GetUserProfileForAdverts.Environment_Advert = model.Environment_Advert;
                    GetUserProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                    GetUserProfileForAdverts.Religion_Advert = model.Religion_Advert;
                    GetUserProfileForAdverts.Music_Advert = model.Music_Advert;
                    GetUserProfileForAdverts.UserId = user.UserId;
                    GetUserProfileForAdverts.Email = user.Email;
                    GetUserProfileForAdverts.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    GetUserProfileForAdverts.Gambling_AdType = model.Gambling_AdType;
                    GetUserProfileForAdverts.Restaurants_AdType = model.Restaurants_AdType;
                    GetUserProfileForAdverts.Insurance_AdType = model.Insurance_AdType;
                    GetUserProfileForAdverts.Furniture_AdType = model.Furniture_AdType;
                    GetUserProfileForAdverts.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    GetUserProfileForAdverts.Energy_AdType = model.Energy_AdType;
                    GetUserProfileForAdverts.Supermarkets_AdType = model.Supermarkets_AdType;
                    GetUserProfileForAdverts.Healthcare_AdType = model.Healthcare_AdType;
                    GetUserProfileForAdverts.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    GetUserProfileForAdverts.Gifts_AdType = model.Gifts_AdType;
                    GetUserProfileForAdverts.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    GetUserProfileForAdverts.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    GetUserProfileForAdverts.RealEstate_AdType = model.RealEstate_AdType;
                    GetUserProfileForAdverts.Games_AdType = model.Games_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch6();
                    usermatch.Food_Advert = model.Food_Advert;
                    usermatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    usermatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    usermatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    usermatch.Householdproducts_Advert = model.Householdproducts_Advert;
                    usermatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    usermatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    usermatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    usermatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    usermatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    usermatch.DIYGardening_Advert = model.DIYGardening_Advert;
                    usermatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    usermatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    usermatch.FinancialServices_Advert = model.FinancialServices_Advert;
                    usermatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    usermatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    usermatch.Motoring_Advert = model.Motoring_Advert;
                    usermatch.Newspapers_Advert = model.Newspapers_Advert;
                    usermatch.TV_Advert = model.TV_Advert;
                    usermatch.Cinema_Advert = model.Cinema_Advert;
                    usermatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    usermatch.Shopping_Advert = model.Shopping_Advert;
                    usermatch.Fitness_Advert = model.Fitness_Advert;
                    usermatch.Environment_Advert = model.Environment_Advert;
                    usermatch.GoingOut_Advert = model.GoingOut_Advert;
                    usermatch.Religion_Advert = model.Religion_Advert;
                    usermatch.Music_Advert = model.Music_Advert;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    usermatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    usermatch.Gambling_AdType = model.Gambling_AdType;
                    usermatch.Restaurants_AdType = model.Restaurants_AdType;
                    usermatch.Insurance_AdType = model.Insurance_AdType;
                    usermatch.Furniture_AdType = model.Furniture_AdType;
                    usermatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    usermatch.Energy_AdType = model.Energy_AdType;
                    usermatch.Supermarkets_AdType = model.Supermarkets_AdType;
                    usermatch.Healthcare_AdType = model.Healthcare_AdType;
                    usermatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    usermatch.Gifts_AdType = model.Gifts_AdType;
                    usermatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    usermatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    usermatch.RealEstate_AdType = model.RealEstate_AdType;
                    usermatch.Games_AdType = model.Games_AdType;
                    SQLServerEntities.UserMatch6.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch7")
            {
                #region UserMatch7
                var GetUserProfileForAdverts = SQLServerEntities.UserMatch7.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForAdverts != null)
                {
                    GetUserProfileForAdverts.Food_Advert = model.Food_Advert;
                    GetUserProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    GetUserProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                    GetUserProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    GetUserProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    GetUserProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    GetUserProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    GetUserProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    GetUserProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                    GetUserProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    GetUserProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    GetUserProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                    GetUserProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    GetUserProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    GetUserProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                    GetUserProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                    GetUserProfileForAdverts.TV_Advert = model.TV_Advert;
                    GetUserProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                    GetUserProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    GetUserProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                    GetUserProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                    GetUserProfileForAdverts.Environment_Advert = model.Environment_Advert;
                    GetUserProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                    GetUserProfileForAdverts.Religion_Advert = model.Religion_Advert;
                    GetUserProfileForAdverts.Music_Advert = model.Music_Advert;
                    GetUserProfileForAdverts.UserId = user.UserId;
                    GetUserProfileForAdverts.Email = user.Email;
                    GetUserProfileForAdverts.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    GetUserProfileForAdverts.Gambling_AdType = model.Gambling_AdType;
                    GetUserProfileForAdverts.Restaurants_AdType = model.Restaurants_AdType;
                    GetUserProfileForAdverts.Insurance_AdType = model.Insurance_AdType;
                    GetUserProfileForAdverts.Furniture_AdType = model.Furniture_AdType;
                    GetUserProfileForAdverts.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    GetUserProfileForAdverts.Energy_AdType = model.Energy_AdType;
                    GetUserProfileForAdverts.Supermarkets_AdType = model.Supermarkets_AdType;
                    GetUserProfileForAdverts.Healthcare_AdType = model.Healthcare_AdType;
                    GetUserProfileForAdverts.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    GetUserProfileForAdverts.Gifts_AdType = model.Gifts_AdType;
                    GetUserProfileForAdverts.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    GetUserProfileForAdverts.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    GetUserProfileForAdverts.RealEstate_AdType = model.RealEstate_AdType;
                    GetUserProfileForAdverts.Games_AdType = model.Games_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch7();
                    usermatch.Food_Advert = model.Food_Advert;
                    usermatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    usermatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    usermatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    usermatch.Householdproducts_Advert = model.Householdproducts_Advert;
                    usermatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    usermatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    usermatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    usermatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    usermatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    usermatch.DIYGardening_Advert = model.DIYGardening_Advert;
                    usermatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    usermatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    usermatch.FinancialServices_Advert = model.FinancialServices_Advert;
                    usermatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    usermatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    usermatch.Motoring_Advert = model.Motoring_Advert;
                    usermatch.Newspapers_Advert = model.Newspapers_Advert;
                    usermatch.TV_Advert = model.TV_Advert;
                    usermatch.Cinema_Advert = model.Cinema_Advert;
                    usermatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    usermatch.Shopping_Advert = model.Shopping_Advert;
                    usermatch.Fitness_Advert = model.Fitness_Advert;
                    usermatch.Environment_Advert = model.Environment_Advert;
                    usermatch.GoingOut_Advert = model.GoingOut_Advert;
                    usermatch.Religion_Advert = model.Religion_Advert;
                    usermatch.Music_Advert = model.Music_Advert;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    usermatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    usermatch.Gambling_AdType = model.Gambling_AdType;
                    usermatch.Restaurants_AdType = model.Restaurants_AdType;
                    usermatch.Insurance_AdType = model.Insurance_AdType;
                    usermatch.Furniture_AdType = model.Furniture_AdType;
                    usermatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    usermatch.Energy_AdType = model.Energy_AdType;
                    usermatch.Supermarkets_AdType = model.Supermarkets_AdType;
                    usermatch.Healthcare_AdType = model.Healthcare_AdType;
                    usermatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    usermatch.Gifts_AdType = model.Gifts_AdType;
                    usermatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    usermatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    usermatch.RealEstate_AdType = model.RealEstate_AdType;
                    usermatch.Games_AdType = model.Games_AdType;
                    SQLServerEntities.UserMatch7.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch8")
            {
                #region UserMatch8
                var GetUserProfileForAdverts = SQLServerEntities.UserMatch8.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForAdverts != null)
                {
                    GetUserProfileForAdverts.Food_Advert = model.Food_Advert;
                    GetUserProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    GetUserProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                    GetUserProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    GetUserProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    GetUserProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    GetUserProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    GetUserProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    GetUserProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                    GetUserProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    GetUserProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    GetUserProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                    GetUserProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    GetUserProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    GetUserProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                    GetUserProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                    GetUserProfileForAdverts.TV_Advert = model.TV_Advert;
                    GetUserProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                    GetUserProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    GetUserProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                    GetUserProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                    GetUserProfileForAdverts.Environment_Advert = model.Environment_Advert;
                    GetUserProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                    GetUserProfileForAdverts.Religion_Advert = model.Religion_Advert;
                    GetUserProfileForAdverts.Music_Advert = model.Music_Advert;
                    GetUserProfileForAdverts.UserId = user.UserId;
                    GetUserProfileForAdverts.Email = user.Email;
                    GetUserProfileForAdverts.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    GetUserProfileForAdverts.Gambling_AdType = model.Gambling_AdType;
                    GetUserProfileForAdverts.Restaurants_AdType = model.Restaurants_AdType;
                    GetUserProfileForAdverts.Insurance_AdType = model.Insurance_AdType;
                    GetUserProfileForAdverts.Furniture_AdType = model.Furniture_AdType;
                    GetUserProfileForAdverts.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    GetUserProfileForAdverts.Energy_AdType = model.Energy_AdType;
                    GetUserProfileForAdverts.Supermarkets_AdType = model.Supermarkets_AdType;
                    GetUserProfileForAdverts.Healthcare_AdType = model.Healthcare_AdType;
                    GetUserProfileForAdverts.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    GetUserProfileForAdverts.Gifts_AdType = model.Gifts_AdType;
                    GetUserProfileForAdverts.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    GetUserProfileForAdverts.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    GetUserProfileForAdverts.RealEstate_AdType = model.RealEstate_AdType;
                    GetUserProfileForAdverts.Games_AdType = model.Games_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch8();
                    usermatch.Food_Advert = model.Food_Advert;
                    usermatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    usermatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    usermatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    usermatch.Householdproducts_Advert = model.Householdproducts_Advert;
                    usermatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    usermatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    usermatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    usermatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    usermatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    usermatch.DIYGardening_Advert = model.DIYGardening_Advert;
                    usermatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    usermatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    usermatch.FinancialServices_Advert = model.FinancialServices_Advert;
                    usermatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    usermatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    usermatch.Motoring_Advert = model.Motoring_Advert;
                    usermatch.Newspapers_Advert = model.Newspapers_Advert;
                    usermatch.TV_Advert = model.TV_Advert;
                    usermatch.Cinema_Advert = model.Cinema_Advert;
                    usermatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    usermatch.Shopping_Advert = model.Shopping_Advert;
                    usermatch.Fitness_Advert = model.Fitness_Advert;
                    usermatch.Environment_Advert = model.Environment_Advert;
                    usermatch.GoingOut_Advert = model.GoingOut_Advert;
                    usermatch.Religion_Advert = model.Religion_Advert;
                    usermatch.Music_Advert = model.Music_Advert;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    usermatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    usermatch.Gambling_AdType = model.Gambling_AdType;
                    usermatch.Restaurants_AdType = model.Restaurants_AdType;
                    usermatch.Insurance_AdType = model.Insurance_AdType;
                    usermatch.Furniture_AdType = model.Furniture_AdType;
                    usermatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    usermatch.Energy_AdType = model.Energy_AdType;
                    usermatch.Supermarkets_AdType = model.Supermarkets_AdType;
                    usermatch.Healthcare_AdType = model.Healthcare_AdType;
                    usermatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    usermatch.Gifts_AdType = model.Gifts_AdType;
                    usermatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    usermatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    usermatch.RealEstate_AdType = model.RealEstate_AdType;
                    usermatch.Games_AdType = model.Games_AdType;
                    SQLServerEntities.UserMatch8.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch9")
            {
                #region UserMatch9
                var GetUserProfileForAdverts = SQLServerEntities.UserMatch9.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForAdverts != null)
                {
                    GetUserProfileForAdverts.Food_Advert = model.Food_Advert;
                    GetUserProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    GetUserProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                    GetUserProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    GetUserProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    GetUserProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    GetUserProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    GetUserProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    GetUserProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                    GetUserProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    GetUserProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    GetUserProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                    GetUserProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    GetUserProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    GetUserProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                    GetUserProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                    GetUserProfileForAdverts.TV_Advert = model.TV_Advert;
                    GetUserProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                    GetUserProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    GetUserProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                    GetUserProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                    GetUserProfileForAdverts.Environment_Advert = model.Environment_Advert;
                    GetUserProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                    GetUserProfileForAdverts.Religion_Advert = model.Religion_Advert;
                    GetUserProfileForAdverts.Music_Advert = model.Music_Advert;
                    GetUserProfileForAdverts.UserId = user.UserId;
                    GetUserProfileForAdverts.Email = user.Email;
                    GetUserProfileForAdverts.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    GetUserProfileForAdverts.Gambling_AdType = model.Gambling_AdType;
                    GetUserProfileForAdverts.Restaurants_AdType = model.Restaurants_AdType;
                    GetUserProfileForAdverts.Insurance_AdType = model.Insurance_AdType;
                    GetUserProfileForAdverts.Furniture_AdType = model.Furniture_AdType;
                    GetUserProfileForAdverts.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    GetUserProfileForAdverts.Energy_AdType = model.Energy_AdType;
                    GetUserProfileForAdverts.Supermarkets_AdType = model.Supermarkets_AdType;
                    GetUserProfileForAdverts.Healthcare_AdType = model.Healthcare_AdType;
                    GetUserProfileForAdverts.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    GetUserProfileForAdverts.Gifts_AdType = model.Gifts_AdType;
                    GetUserProfileForAdverts.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    GetUserProfileForAdverts.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    GetUserProfileForAdverts.RealEstate_AdType = model.RealEstate_AdType;
                    GetUserProfileForAdverts.Games_AdType = model.Games_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch9();
                    usermatch.Food_Advert = model.Food_Advert;
                    usermatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    usermatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    usermatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    usermatch.Householdproducts_Advert = model.Householdproducts_Advert;
                    usermatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    usermatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    usermatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    usermatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    usermatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    usermatch.DIYGardening_Advert = model.DIYGardening_Advert;
                    usermatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    usermatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    usermatch.FinancialServices_Advert = model.FinancialServices_Advert;
                    usermatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    usermatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    usermatch.Motoring_Advert = model.Motoring_Advert;
                    usermatch.Newspapers_Advert = model.Newspapers_Advert;
                    usermatch.TV_Advert = model.TV_Advert;
                    usermatch.Cinema_Advert = model.Cinema_Advert;
                    usermatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    usermatch.Shopping_Advert = model.Shopping_Advert;
                    usermatch.Fitness_Advert = model.Fitness_Advert;
                    usermatch.Environment_Advert = model.Environment_Advert;
                    usermatch.GoingOut_Advert = model.GoingOut_Advert;
                    usermatch.Religion_Advert = model.Religion_Advert;
                    usermatch.Music_Advert = model.Music_Advert;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    usermatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    usermatch.Gambling_AdType = model.Gambling_AdType;
                    usermatch.Restaurants_AdType = model.Restaurants_AdType;
                    usermatch.Insurance_AdType = model.Insurance_AdType;
                    usermatch.Furniture_AdType = model.Furniture_AdType;
                    usermatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    usermatch.Energy_AdType = model.Energy_AdType;
                    usermatch.Supermarkets_AdType = model.Supermarkets_AdType;
                    usermatch.Healthcare_AdType = model.Healthcare_AdType;
                    usermatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    usermatch.Gifts_AdType = model.Gifts_AdType;
                    usermatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    usermatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    usermatch.RealEstate_AdType = model.RealEstate_AdType;
                    usermatch.Games_AdType = model.Games_AdType;
                    SQLServerEntities.UserMatch9.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch10")
            {
                #region UserMatch10
                var GetUserProfileForAdverts = SQLServerEntities.UserMatch10.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForAdverts != null)
                {
                    GetUserProfileForAdverts.Food_Advert = model.Food_Advert;
                    GetUserProfileForAdverts.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    GetUserProfileForAdverts.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    GetUserProfileForAdverts.Householdproducts_Advert = model.Householdproducts_Advert;
                    GetUserProfileForAdverts.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    GetUserProfileForAdverts.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    GetUserProfileForAdverts.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    GetUserProfileForAdverts.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    GetUserProfileForAdverts.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    GetUserProfileForAdverts.DIYGardening_Advert = model.DIYGardening_Advert;
                    GetUserProfileForAdverts.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    GetUserProfileForAdverts.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    GetUserProfileForAdverts.FinancialServices_Advert = model.FinancialServices_Advert;
                    GetUserProfileForAdverts.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    GetUserProfileForAdverts.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    GetUserProfileForAdverts.Motoring_Advert = model.Motoring_Advert;
                    GetUserProfileForAdverts.Newspapers_Advert = model.Newspapers_Advert;
                    GetUserProfileForAdverts.TV_Advert = model.TV_Advert;
                    GetUserProfileForAdverts.Cinema_Advert = model.Cinema_Advert;
                    GetUserProfileForAdverts.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    GetUserProfileForAdverts.Shopping_Advert = model.Shopping_Advert;
                    GetUserProfileForAdverts.Fitness_Advert = model.Fitness_Advert;
                    GetUserProfileForAdverts.Environment_Advert = model.Environment_Advert;
                    GetUserProfileForAdverts.GoingOut_Advert = model.GoingOut_Advert;
                    GetUserProfileForAdverts.Religion_Advert = model.Religion_Advert;
                    GetUserProfileForAdverts.Music_Advert = model.Music_Advert;
                    GetUserProfileForAdverts.UserId = user.UserId;
                    GetUserProfileForAdverts.Email = user.Email;
                    GetUserProfileForAdverts.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    GetUserProfileForAdverts.Gambling_AdType = model.Gambling_AdType;
                    GetUserProfileForAdverts.Restaurants_AdType = model.Restaurants_AdType;
                    GetUserProfileForAdverts.Insurance_AdType = model.Insurance_AdType;
                    GetUserProfileForAdverts.Furniture_AdType = model.Furniture_AdType;
                    GetUserProfileForAdverts.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    GetUserProfileForAdverts.Energy_AdType = model.Energy_AdType;
                    GetUserProfileForAdverts.Supermarkets_AdType = model.Supermarkets_AdType;
                    GetUserProfileForAdverts.Healthcare_AdType = model.Healthcare_AdType;
                    GetUserProfileForAdverts.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    GetUserProfileForAdverts.Gifts_AdType = model.Gifts_AdType;
                    GetUserProfileForAdverts.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    GetUserProfileForAdverts.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    GetUserProfileForAdverts.RealEstate_AdType = model.RealEstate_AdType;
                    GetUserProfileForAdverts.Games_AdType = model.Games_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch10();
                    usermatch.Food_Advert = model.Food_Advert;
                    usermatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
                    usermatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
                    usermatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
                    usermatch.Householdproducts_Advert = model.Householdproducts_Advert;
                    usermatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
                    usermatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
                    usermatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
                    usermatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
                    usermatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
                    usermatch.DIYGardening_Advert = model.DIYGardening_Advert;
                    usermatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
                    usermatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
                    usermatch.FinancialServices_Advert = model.FinancialServices_Advert;
                    usermatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
                    usermatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
                    usermatch.Motoring_Advert = model.Motoring_Advert;
                    usermatch.Newspapers_Advert = model.Newspapers_Advert;
                    usermatch.TV_Advert = model.TV_Advert;
                    usermatch.Cinema_Advert = model.Cinema_Advert;
                    usermatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
                    usermatch.Shopping_Advert = model.Shopping_Advert;
                    usermatch.Fitness_Advert = model.Fitness_Advert;
                    usermatch.Environment_Advert = model.Environment_Advert;
                    usermatch.GoingOut_Advert = model.GoingOut_Advert;
                    usermatch.Religion_Advert = model.Religion_Advert;
                    usermatch.Music_Advert = model.Music_Advert;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    usermatch.UserId = user.UserId;
                    usermatch.Email = user.Email;
                    usermatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
                    usermatch.Gambling_AdType = model.Gambling_AdType;
                    usermatch.Restaurants_AdType = model.Restaurants_AdType;
                    usermatch.Insurance_AdType = model.Insurance_AdType;
                    usermatch.Furniture_AdType = model.Furniture_AdType;
                    usermatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
                    usermatch.Energy_AdType = model.Energy_AdType;
                    usermatch.Supermarkets_AdType = model.Supermarkets_AdType;
                    usermatch.Healthcare_AdType = model.Healthcare_AdType;
                    usermatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
                    usermatch.Gifts_AdType = model.Gifts_AdType;
                    usermatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
                    usermatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
                    usermatch.RealEstate_AdType = model.RealEstate_AdType;
                    usermatch.Games_AdType = model.Games_AdType;
                    SQLServerEntities.UserMatch10.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
        }

        public void UpdateSkizaProfile(SkizaProfileFormModel model, User user, EFMVCDataContex SQLServerEntities)
        {
            if (user.UserMatchTableName == "UserMatch1")
            {
                #region UserMatch1
                var GetUserProfileForSkiza = SQLServerEntities.UserMatch.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForSkiza != null)
                {
                    GetUserProfileForSkiza.Hustlers_AdType = model.Hustlers_AdType;
                    GetUserProfileForSkiza.Youth_AdType = model.Youth_AdType;
                    GetUserProfileForSkiza.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    GetUserProfileForSkiza.Mass_AdType = model.Mass_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch();
                    usermatch.Hustlers_AdType = model.Hustlers_AdType;
                    usermatch.Youth_AdType = model.Youth_AdType;
                    usermatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    usermatch.Mass_AdType = model.Mass_AdType;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    SQLServerEntities.UserMatch.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch2")
            {
                #region UserMatch2
                var GetUserProfileForSkiza = SQLServerEntities.UserMatch2.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForSkiza != null)
                {
                    GetUserProfileForSkiza.Hustlers_AdType = model.Hustlers_AdType;
                    GetUserProfileForSkiza.Youth_AdType = model.Youth_AdType;
                    GetUserProfileForSkiza.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    GetUserProfileForSkiza.Mass_AdType = model.Mass_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch2();
                    usermatch.Hustlers_AdType = model.Hustlers_AdType;
                    usermatch.Youth_AdType = model.Youth_AdType;
                    usermatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    usermatch.Mass_AdType = model.Mass_AdType;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    SQLServerEntities.UserMatch2.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch3")
            {
                #region UserMatch3
                var GetUserProfileForSkiza = SQLServerEntities.UserMatch3.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForSkiza != null)
                {
                    GetUserProfileForSkiza.Hustlers_AdType = model.Hustlers_AdType;
                    GetUserProfileForSkiza.Youth_AdType = model.Youth_AdType;
                    GetUserProfileForSkiza.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    GetUserProfileForSkiza.Mass_AdType = model.Mass_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch3();
                    usermatch.Hustlers_AdType = model.Hustlers_AdType;
                    usermatch.Youth_AdType = model.Youth_AdType;
                    usermatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    usermatch.Mass_AdType = model.Mass_AdType;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    SQLServerEntities.UserMatch3.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch4")
            {
                #region UserMatch4
                var GetUserProfileForSkiza = SQLServerEntities.UserMatch4.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForSkiza != null)
                {
                    GetUserProfileForSkiza.Hustlers_AdType = model.Hustlers_AdType;
                    GetUserProfileForSkiza.Youth_AdType = model.Youth_AdType;
                    GetUserProfileForSkiza.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    GetUserProfileForSkiza.Mass_AdType = model.Mass_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch4();
                    usermatch.Hustlers_AdType = model.Hustlers_AdType;
                    usermatch.Youth_AdType = model.Youth_AdType;
                    usermatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    usermatch.Mass_AdType = model.Mass_AdType;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    SQLServerEntities.UserMatch4.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch5")
            {
                #region UserMatch5
                var GetUserProfileForSkiza = SQLServerEntities.UserMatch5.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForSkiza != null)
                {
                    GetUserProfileForSkiza.Hustlers_AdType = model.Hustlers_AdType;
                    GetUserProfileForSkiza.Youth_AdType = model.Youth_AdType;
                    GetUserProfileForSkiza.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    GetUserProfileForSkiza.Mass_AdType = model.Mass_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch5();
                    usermatch.Hustlers_AdType = model.Hustlers_AdType;
                    usermatch.Youth_AdType = model.Youth_AdType;
                    usermatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    usermatch.Mass_AdType = model.Mass_AdType;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    SQLServerEntities.UserMatch5.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch6")
            {
                #region UserMatch6
                var GetUserProfileForSkiza = SQLServerEntities.UserMatch6.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForSkiza != null)
                {
                    GetUserProfileForSkiza.Hustlers_AdType = model.Hustlers_AdType;
                    GetUserProfileForSkiza.Youth_AdType = model.Youth_AdType;
                    GetUserProfileForSkiza.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    GetUserProfileForSkiza.Mass_AdType = model.Mass_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch6();
                    usermatch.Hustlers_AdType = model.Hustlers_AdType;
                    usermatch.Youth_AdType = model.Youth_AdType;
                    usermatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    usermatch.Mass_AdType = model.Mass_AdType;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    SQLServerEntities.UserMatch6.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch7")
            {
                #region UserMatch7
                var GetUserProfileForSkiza = SQLServerEntities.UserMatch7.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForSkiza != null)
                {
                    GetUserProfileForSkiza.Hustlers_AdType = model.Hustlers_AdType;
                    GetUserProfileForSkiza.Youth_AdType = model.Youth_AdType;
                    GetUserProfileForSkiza.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    GetUserProfileForSkiza.Mass_AdType = model.Mass_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch7();
                    usermatch.Hustlers_AdType = model.Hustlers_AdType;
                    usermatch.Youth_AdType = model.Youth_AdType;
                    usermatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    usermatch.Mass_AdType = model.Mass_AdType;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    SQLServerEntities.UserMatch7.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch8")
            {
                #region UserMatch8
                var GetUserProfileForSkiza = SQLServerEntities.UserMatch8.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForSkiza != null)
                {
                    GetUserProfileForSkiza.Hustlers_AdType = model.Hustlers_AdType;
                    GetUserProfileForSkiza.Youth_AdType = model.Youth_AdType;
                    GetUserProfileForSkiza.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    GetUserProfileForSkiza.Mass_AdType = model.Mass_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch8();
                    usermatch.Hustlers_AdType = model.Hustlers_AdType;
                    usermatch.Youth_AdType = model.Youth_AdType;
                    usermatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    usermatch.Mass_AdType = model.Mass_AdType;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    SQLServerEntities.UserMatch8.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch9")
            {
                #region UserMatch9
                var GetUserProfileForSkiza = SQLServerEntities.UserMatch9.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForSkiza != null)
                {
                    GetUserProfileForSkiza.Hustlers_AdType = model.Hustlers_AdType;
                    GetUserProfileForSkiza.Youth_AdType = model.Youth_AdType;
                    GetUserProfileForSkiza.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    GetUserProfileForSkiza.Mass_AdType = model.Mass_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch9();
                    usermatch.Hustlers_AdType = model.Hustlers_AdType;
                    usermatch.Youth_AdType = model.Youth_AdType;
                    usermatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    usermatch.Mass_AdType = model.Mass_AdType;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    SQLServerEntities.UserMatch9.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }
            else if (user.UserMatchTableName == "UserMatch10")
            {
                #region UserMatch10
                var GetUserProfileForSkiza = SQLServerEntities.UserMatch10.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault();
                if (GetUserProfileForSkiza != null)
                {
                    GetUserProfileForSkiza.Hustlers_AdType = model.Hustlers_AdType;
                    GetUserProfileForSkiza.Youth_AdType = model.Youth_AdType;
                    GetUserProfileForSkiza.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    GetUserProfileForSkiza.Mass_AdType = model.Mass_AdType;
                    SQLServerEntities.SaveChanges();
                }
                else
                {
                    var usermatch = new UserMatch10();
                    usermatch.Hustlers_AdType = model.Hustlers_AdType;
                    usermatch.Youth_AdType = model.Youth_AdType;
                    usermatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
                    usermatch.Mass_AdType = model.Mass_AdType;
                    usermatch.MSUserProfileId = model.UserProfileId;
                    SQLServerEntities.UserMatch10.Add(usermatch);
                    SQLServerEntities.SaveChanges();
                }
                #endregion
            }

        }

        public void AddCampaignData(CampaignProfile campProfile, EFMVCDataContex SQLServerEntities)
        {
            var campaignMatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campProfile.CampaignProfileId).FirstOrDefault();
            bool isAdd = false;
            if (campaignMatch == null)
            {
                campaignMatch = new CampaignMatch();
                isAdd = true;
            }        
            campaignMatch.UserId = campProfile.UserId;
            campaignMatch.ClientId = campProfile.ClientId;
            campaignMatch.CampaignName = campProfile.CampaignName;
            campaignMatch.UserId = campProfile.UserId;
            campaignMatch.CampaignDescription = campProfile.CampaignDescription;
            campaignMatch.TotalBudget = (decimal)campProfile.TotalBudget;
            campaignMatch.MaxDailyBudget = (decimal)campProfile.MaxDailyBudget;
            campaignMatch.MaxBid = (int)campProfile.MaxBid;
            campaignMatch.MaxMonthBudget = (decimal)campProfile.MaxMonthBudget;
            campaignMatch.MaxWeeklyBudget = (decimal)campProfile.MaxWeeklyBudget;
            campaignMatch.MaxHourlyBudget = (decimal)campProfile.MaxHourlyBudget;
            campaignMatch.TotalCredit = (decimal)campProfile.TotalCredit;
            campaignMatch.AvailableCredit = campProfile.AvailableCredit.ToString();
            campaignMatch.PlaysToDate = campProfile.PlaysToDate;
            campaignMatch.PlaysLastMonth = campProfile.PlaysLastMonth;
            campaignMatch.PlaysCurrentMonth = campProfile.PlaysCurrentMonth;
            campaignMatch.CancelledToDate = campProfile.CancelledToDate;
            campaignMatch.CancelledLastMonth = campProfile.CancelledLastMonth;
            campaignMatch.CancelledCurrentMonth = campProfile.CancelledCurrentMonth;
            campaignMatch.SmsToDate = campProfile.SmsToDate;
            campaignMatch.SmsLastMonth = campProfile.SmsLastMonth;
            campaignMatch.SmsCurrentMonth = campProfile.SmsCurrentMonth;
            campaignMatch.EmailToDate = campProfile.EmailToDate;
            campaignMatch.EmailsLastMonth = campProfile.EmailsLastMonth;
            campaignMatch.EmailsCurrentMonth = campProfile.EmailsCurrentMonth;
            campaignMatch.EmailFileLocation = campProfile.EmailFileLocation;
            campaignMatch.Active = campProfile.Active;
            campaignMatch.NumberOfPlays = campProfile.NumberOfPlays;
            campaignMatch.AverageDailyPlays = campProfile.AverageDailyPlays;
            campaignMatch.SmsRequests = campProfile.SmsRequests;
            campaignMatch.EmailsDelievered = campProfile.EmailsDelievered;
            campaignMatch.EmailSubject = campProfile.EmailSubject;
            campaignMatch.EmailBody = campProfile.EmailBody;
            campaignMatch.EmailSenderAddress = campProfile.EmailSenderAddress;
            campaignMatch.SmsOriginator = campProfile.SmsOriginator;
            campaignMatch.SmsBody = campProfile.SmsBody;
            campaignMatch.SMSFileLocation = campProfile.SMSFileLocation;
            campaignMatch.CreatedDateTime = campProfile.CreatedDateTime;
            campaignMatch.UpdatedDateTime = campProfile.UpdatedDateTime;
            campaignMatch.Status = campProfile.Status;
            campaignMatch.StartDate = campProfile.StartDate;
            campaignMatch.EndDate = campProfile.EndDate;
            campaignMatch.NumberInBatch = campProfile.NumberInBatch;
            campaignMatch.CountryId = campProfile.CountryId;
            campaignMatch.MSCampaignProfileId = campProfile.CampaignProfileId;
            campaignMatch.NextStatus = campProfile.NextStatus;

            if (isAdd)
            {
                SQLServerEntities.CampaignMatch.Add(campaignMatch);
                SQLServerEntities.SaveChanges();
            }
            else
            {
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateCampaignGeographic(CampaignProfileGeographicFormModel model,CampaignProfile campProfile, EFMVCDataContex SQLServerEntities)
        {
            var campaignMatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campProfile.CampaignProfileId).FirstOrDefault();
            bool isAdd = false;
            if (campaignMatch == null)
            {
                campaignMatch = new CampaignMatch();
                isAdd = true;                
            }

            campaignMatch.Location_Demographics = model.Location_Demographics;
            campaignMatch.MSCampaignProfileId = campProfile.CampaignProfileId;
            if (isAdd)
            {
                SQLServerEntities.CampaignMatch.Add(campaignMatch);
                SQLServerEntities.SaveChanges();
            }
            else
            {
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateCampaignDemographics(CampaignProfileDemographicsFormModel model, CampaignProfile campProfile, EFMVCDataContex SQLServerEntities)
        {
            var campaignMatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campProfile.CampaignProfileId).FirstOrDefault();
            bool isAdd = false;
            if (campaignMatch == null)
            {
                campaignMatch = new CampaignMatch();
                isAdd = true;
            }

            campaignMatch.Age_Demographics = model.Age_Demographics;
            campaignMatch.Gender_Demographics = model.Gender_Demographics;
            campaignMatch.IncomeBracket_Demographics = model.IncomeBracket_Demographics;
            campaignMatch.WorkingStatus_Demographics = model.WorkingStatus_Demographics;
            campaignMatch.RelationshipStatus_Demographics = model.RelationshipStatus_Demographics;
            campaignMatch.Education_Demographics = model.Education_Demographics;
            campaignMatch.HouseholdStatus_Demographics = model.HouseholdStatus_Demographics;
            campaignMatch.MSCampaignProfileId = campProfile.CampaignProfileId;

            if (isAdd)
            {
                SQLServerEntities.CampaignMatch.Add(campaignMatch);
                SQLServerEntities.SaveChanges();
            }
            else
            {
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateCampaignAdtypes(CampaignProfileAdvertFormModel model, CampaignProfile campProfile, EFMVCDataContex SQLServerEntities)
        {
            var campaignMatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campProfile.CampaignProfileId).FirstOrDefault();
            bool isAdd = false;
            if (campaignMatch == null)
            {
                campaignMatch = new CampaignMatch();
                isAdd = true;
            }

            campaignMatch.Food_Advert = model.Food_Advert;
            campaignMatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
            campaignMatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
            campaignMatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
            campaignMatch.Householdproducts_Advert = model.Householdproducts_Advert;
            campaignMatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
            campaignMatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
            campaignMatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
            campaignMatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
            campaignMatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
            campaignMatch.DIYGardening_Advert = model.DIYGardening_Advert;
            campaignMatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
            campaignMatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
            campaignMatch.FinancialServices_Advert = model.FinancialServices_Advert;
            campaignMatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
            campaignMatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
            campaignMatch.Motoring_Advert = model.Motoring_Advert;
            campaignMatch.Newspapers_Advert = model.Newspapers_Advert;
            campaignMatch.TV_Advert = model.TV_Advert;
            campaignMatch.Cinema_Advert = model.Cinema_Advert;
            campaignMatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
            campaignMatch.Shopping_Advert = model.Shopping_Advert;
            campaignMatch.Fitness_Advert = model.Fitness_Advert;
            campaignMatch.Environment_Advert = model.Environment_Advert;
            campaignMatch.GoingOut_Advert = model.GoingOut_Advert;
            campaignMatch.Religion_Advert = model.Religion_Advert;
            campaignMatch.Music_Advert = model.Music_Advert;
            campaignMatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
            campaignMatch.Gambling_AdType = model.Gambling_AdType;
            campaignMatch.Restaurants_AdType = model.Restaurants_AdType;
            campaignMatch.Insurance_AdType = model.Insurance_AdType;
            campaignMatch.Furniture_AdType = model.Furniture_AdType;
            campaignMatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
            campaignMatch.Energy_AdType = model.Energy_AdType;
            campaignMatch.Supermarkets_AdType = model.Supermarkets_AdType;
            campaignMatch.Healthcare_AdType = model.Healthcare_AdType;
            campaignMatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
            campaignMatch.Gifts_AdType = model.Gifts_AdType;
            campaignMatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
            campaignMatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
            campaignMatch.RealEstate_AdType = model.RealEstate_AdType;
            campaignMatch.Games_AdType = model.Games_AdType;
            campaignMatch.MSCampaignProfileId = campProfile.CampaignProfileId;

            if (isAdd)
            {
                SQLServerEntities.CampaignMatch.Add(campaignMatch);
                SQLServerEntities.SaveChanges();
            }
            else
            {
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateSkizaProfile(CampaignProfileSkizaFormModel model, CampaignProfile campProfile, EFMVCDataContex SQLServerEntities)
        {
            var campaignMatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campProfile.CampaignProfileId).FirstOrDefault();
            bool isAdd = false;
            if (campaignMatch == null)
            {
                campaignMatch = new CampaignMatch();
                isAdd = true;
            }

            campaignMatch.Hustlers_AdType = model.Hustlers_AdType;
            campaignMatch.Youth_AdType = model.Youth_AdType;
            campaignMatch.DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType;
            campaignMatch.Mass_AdType = model.Mass_AdType;
            campaignMatch.MSCampaignProfileId = campProfile.CampaignProfileId;

            if (isAdd)
            {
                SQLServerEntities.CampaignMatch.Add(campaignMatch);
                SQLServerEntities.SaveChanges();
            }
            else
            {
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateUsage(CampaignProfileMobileFormModel model, CampaignProfile campProfile, EFMVCDataContex SQLServerEntities)
        {
            var campaignMatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campProfile.CampaignProfileId).FirstOrDefault();
            bool isAdd = false;
            if (campaignMatch == null)
            {
                campaignMatch = new CampaignMatch();
                isAdd = true;
            }
            campaignMatch.ContractType_Mobile = model.ContractType_Mobile;
            campaignMatch.Spend_Mobile = model.Spend_Mobile;
            campaignMatch.MSCampaignProfileId = campProfile.CampaignProfileId;

            if (isAdd)
            {
                SQLServerEntities.CampaignMatch.Add(campaignMatch);
                SQLServerEntities.SaveChanges();
            }
            else
            {
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateCampaignInfo(CampaignProfileFormModel campaignProfileFormModel,CampaignProfile campaigndetails,int userId, EFMVCDataContex SQLServerEntities)
        {
            var campaignmatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campaigndetails.CampaignProfileId).FirstOrDefault();
            if (campaignmatch != null)
            {
                var clientId = campaignProfileFormModel.ClientId == null ? 0 : campaignProfileFormModel.ClientId;
                var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(SQLServerEntities, (int)clientId);
               // var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(SQLServerEntities, campaignProfileFormModel.ClientId.Value);
                var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(SQLServerEntities, campaignProfileFormModel.CountryId);

                int? operatorClientId;
                if (externalServerClientId == 0)
                {
                    operatorClientId = null;
                }
                else
                {
                    operatorClientId = externalServerClientId;
                }

                campaignmatch.CampaignName = campaignProfileFormModel.CampaignName;
                campaignmatch.CampaignDescription = campaignProfileFormModel.CampaignDescription;
                campaignmatch.ClientId = operatorClientId;
                campaignmatch.StartDate = campaignProfileFormModel.StartDate;
                campaignmatch.EndDate = campaignProfileFormModel.EndDate;
                campaignmatch.NumberInBatch = campaignProfileFormModel.NumberInBatch;
                campaignmatch.CountryId = externalServerCountryId;
                campaignmatch.MSCampaignProfileId = campaigndetails.CampaignProfileId;
                campaignmatch.Status = campaigndetails.Status;
                campaignmatch.Active = campaigndetails.Active;
                campaignmatch.AvailableCredit = campaigndetails.AvailableCredit.ToString();
                campaignmatch.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                campaignmatch.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                campaignmatch.CancelledToDate = campaigndetails.CancelledToDate;
                campaignmatch.CreatedDateTime = campaigndetails.CreatedDateTime;
                campaignmatch.EmailToDate = campaigndetails.EmailToDate;
                campaignmatch.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                campaignmatch.EmailsLastMonth = campaigndetails.EmailsLastMonth;
                campaignmatch.MaxBid = Convert.ToInt32(campaigndetails.MaxBid);
                campaignmatch.MaxMonthBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                campaignmatch.MaxWeeklyBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                campaignmatch.MaxHourlyBudget = Convert.ToInt64(campaigndetails.MaxHourlyBudget);
                campaignmatch.TotalCredit = Convert.ToInt64(campaigndetails.TotalCredit);
                campaignmatch.SpentToDate = campaigndetails.SpendToDate.ToString();
                campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                campaignmatch.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                campaignmatch.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                campaignmatch.PlaysToDate = campaigndetails.PlaysToDate;
                campaignmatch.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                campaignmatch.SmsLastMonth = campaigndetails.SmsLastMonth;
                campaignmatch.SmsToDate = campaigndetails.SmsToDate;
                campaignmatch.TotalBudget = Convert.ToInt64(campaigndetails.TotalBudget);
                campaignmatch.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                campaignmatch.UserId = campaigndetails.UserId;
                campaignmatch.EmailBody = campaigndetails.EmailBody;
                campaignmatch.EmailSenderAddress = campaigndetails.EmailSenderAddress;
                campaignmatch.EmailSubject = campaigndetails.EmailSubject;
                campaignmatch.SmsBody = campaigndetails.SmsBody;
                campaignmatch.SmsOriginator = campaigndetails.SmsOriginator;
                campaignmatch.EMAIL_MESSAGE = campaigndetails.EmailBody;
                campaignmatch.SMS_MESSAGE = campaigndetails.SmsBody;
                campaignmatch.ORIGINATOR = campaigndetails.SmsOriginator;
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateCampaignBudgetInfo(CampaignProfileFormModel campaignProfileFormModel, CampaignProfile campaigndetails, int userId, EFMVCDataContex SQLServerEntities)
        {
            var campaignmatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campaigndetails.CampaignProfileId).FirstOrDefault();
            if (campaignmatch != null)
            {
                campaignmatch.MaxBid = Convert.ToInt32(campaignProfileFormModel.MaxBid);
                campaignmatch.MaxMonthBudget = Convert.ToInt64(campaignProfileFormModel.MaxMonthBudget);
                campaignmatch.MaxWeeklyBudget = Convert.ToInt64(campaignProfileFormModel.MaxWeeklyBudget);
                campaignmatch.MaxHourlyBudget = Convert.ToInt64(campaignProfileFormModel.MaxHourlyBudget);
                campaignmatch.MaxDailyBudget = Convert.ToInt64(campaignProfileFormModel.MaxDailyBudget);
                campaignmatch.MSCampaignProfileId = campaigndetails.CampaignProfileId;
                campaignmatch.StartDate = campaigndetails.StartDate;
                campaignmatch.EndDate = campaigndetails.EndDate;
                campaignmatch.CampaignDescription = campaigndetails.CampaignDescription;
                campaignmatch.CampaignName = campaigndetails.CampaignName;
                campaignmatch.ClientId = campaigndetails.ClientId;
                campaignmatch.Status = campaigndetails.Status;
                campaignmatch.Active = campaigndetails.Active;
                campaignmatch.AvailableCredit = campaigndetails.AvailableCredit.ToString();
                campaignmatch.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                campaignmatch.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                campaignmatch.CancelledToDate = campaigndetails.CancelledToDate;
                campaignmatch.CreatedDateTime = campaigndetails.CreatedDateTime;
                campaignmatch.EmailToDate = campaigndetails.EmailToDate;
                campaignmatch.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                campaignmatch.EmailsLastMonth = campaigndetails.EmailsLastMonth;
                campaignmatch.TotalCredit = Convert.ToInt64(campaigndetails.TotalCredit);
                campaignmatch.SpentToDate = campaigndetails.SpendToDate.ToString();
                campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                campaignmatch.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                campaignmatch.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                campaignmatch.PlaysToDate = campaigndetails.PlaysToDate;
                campaignmatch.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                campaignmatch.SmsLastMonth = campaigndetails.SmsLastMonth;
                campaignmatch.SmsToDate = campaigndetails.SmsToDate;
                campaignmatch.TotalBudget = Convert.ToInt64(campaigndetails.TotalBudget);
                campaignmatch.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                campaignmatch.UserId = campaigndetails.UserId;
                campaignmatch.EmailBody = campaigndetails.EmailBody;
                campaignmatch.EmailSenderAddress = campaigndetails.EmailSenderAddress;
                campaignmatch.EmailSubject = campaigndetails.EmailSubject;
                campaignmatch.SmsBody = campaigndetails.SmsBody;
                campaignmatch.SmsOriginator = campaigndetails.SmsOriginator;
                campaignmatch.EMAIL_MESSAGE = campaigndetails.EmailBody;
                campaignmatch.SMS_MESSAGE = campaigndetails.SmsBody;
                campaignmatch.ORIGINATOR = campaigndetails.SmsOriginator;
                campaignmatch.NumberInBatch = campaigndetails.NumberInBatch;
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateCampaignSmsAndEmail(CampaignProfileFormModel campaignProfileFormModel, CampaignProfile campaigndetails,string emailFileLocation, string smsFileLocation, EFMVCDataContex SQLServerEntities)
        {
            var campaignmatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campaigndetails.CampaignProfileId).FirstOrDefault();
            if (campaignmatch != null)
            {
                campaignmatch.EMAIL_MESSAGE = campaignProfileFormModel.EmailBody;
                campaignmatch.SMS_MESSAGE = campaignProfileFormModel.SmsBody;
                campaignmatch.ORIGINATOR = campaignProfileFormModel.SmsOriginator;
                campaignmatch.EmailBody = campaignProfileFormModel.EmailBody;
                campaignmatch.EmailSenderAddress = campaignProfileFormModel.EmailSenderAddress;
                campaignmatch.EmailSubject = campaignProfileFormModel.EmailSubject;
                campaignmatch.SmsBody = campaignProfileFormModel.SmsBody;
                campaignmatch.SmsOriginator = campaignProfileFormModel.SmsOriginator;
                campaignmatch.EmailFileLocation = emailFileLocation;
                campaignmatch.SMSFileLocation = smsFileLocation;
                campaignmatch.StartDate = campaigndetails.StartDate;
                campaignmatch.EndDate = campaigndetails.EndDate;
                campaignmatch.MaxBid = Convert.ToInt32(campaigndetails.MaxBid);
                campaignmatch.MaxMonthBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                campaignmatch.MaxWeeklyBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                campaignmatch.MaxHourlyBudget = Convert.ToInt64(campaigndetails.MaxHourlyBudget);
                campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                campaignmatch.CampaignDescription = campaigndetails.CampaignDescription;
                campaignmatch.CampaignName = campaigndetails.CampaignName;
                campaignmatch.ClientId = campaigndetails.ClientId;
                campaignmatch.Status = campaigndetails.Status;
                campaignmatch.Active = campaigndetails.Active;
                campaignmatch.AvailableCredit = campaigndetails.AvailableCredit.ToString();
                campaignmatch.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                campaignmatch.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                campaignmatch.CancelledToDate = campaigndetails.CancelledToDate;
                campaignmatch.CreatedDateTime = campaigndetails.CreatedDateTime;
                campaignmatch.EmailToDate = campaigndetails.EmailToDate;
                campaignmatch.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                campaignmatch.EmailsLastMonth = campaigndetails.EmailsLastMonth;
                campaignmatch.TotalCredit = Convert.ToInt64(campaigndetails.TotalCredit);
                campaignmatch.SpentToDate = campaigndetails.SpendToDate.ToString();
                campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                campaignmatch.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                campaignmatch.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                campaignmatch.PlaysToDate = campaigndetails.PlaysToDate;
                campaignmatch.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                campaignmatch.SmsLastMonth = campaigndetails.SmsLastMonth;
                campaignmatch.SmsToDate = campaigndetails.SmsToDate;
                campaignmatch.TotalBudget = Convert.ToInt64(campaigndetails.TotalBudget);
                campaignmatch.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                campaignmatch.UserId = campaigndetails.UserId;
                campaignmatch.NumberInBatch = campaigndetails.NumberInBatch;
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateCampaignAd(int campaignProfileId, string adName, EFMVCDataContex SQLServerEntities)
        {
            //var CampaignAuditData = SQLServerEntities.CampaignAudits.Where(s => s.CampaignProfileId == campaignProfileId).ToList();
            //if (CampaignAuditData.Count() > 0)
            //{
            //    SQLServerEntities.CampaignAudits.RemoveRange(CampaignAuditData);
            //    SQLServerEntities.SaveChanges();
            //}

            var GetCampaignMatchProfile = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campaignProfileId).FirstOrDefault();
            if (GetCampaignMatchProfile != null)
            {
                GetCampaignMatchProfile.MEDIA_URL = adName;
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateCampaignStatus(int campaignProfileId, int status, EFMVCDataContex SQLServerEntities)
        {
            var campaignmatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campaignProfileId).FirstOrDefault();
            if(campaignmatch != null)
            {
                campaignmatch.Status = status;
                SQLServerEntities.SaveChanges();
            }
        }

        public void UpdateMediaFileLocation(int campaignProfileId, string mediaUrl, EFMVCDataContex SQLServerEntities)
        {
            var campaignmatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campaignProfileId).FirstOrDefault();
            if (campaignmatch != null)
            {
                campaignmatch.MEDIA_URL = mediaUrl;
                SQLServerEntities.SaveChanges();
            }
        }

        //public void UpdateCampaignGeographicWizard(CampaignProfileGeographicFormModel model, EFMVCDataContex SQLServerEntities)
        //{
        //    var campaignMatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
        //    bool isAdd = false;
        //    if (campaignMatch == null)
        //    {
        //        campaignMatch = new CampaignMatch();
        //        isAdd = true;
        //    }

        //    campaignMatch.Location_Demographics = model.Location_Demographics;
        //    campaignMatch.MSCampaignProfileId = model.CampaignProfileId;
        //    if (isAdd)
        //    {
        //        SQLServerEntities.CampaignMatch.Add(campaignMatch);
        //        SQLServerEntities.SaveChanges();
        //    }
        //    else
        //    {
        //        SQLServerEntities.SaveChanges();
        //    }
        //}

        //public int UpdateCampaignDemographicsWizard(CampaignProfileDemographicsFormModel model, EFMVCDataContex SQLServerEntities)
        //{
        //    var campaignMatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
        //    bool isAdd = false;
        //    if (campaignMatch == null)
        //    {
        //        campaignMatch = new CampaignMatch();
        //        isAdd = true;
        //    }

        //    campaignMatch.Age_Demographics = model.Age_Demographics;
        //    campaignMatch.Gender_Demographics = model.Gender_Demographics;
        //    campaignMatch.IncomeBracket_Demographics = model.IncomeBracket_Demographics;
        //    campaignMatch.WorkingStatus_Demographics = model.WorkingStatus_Demographics;
        //    campaignMatch.RelationshipStatus_Demographics = model.RelationshipStatus_Demographics;
        //    campaignMatch.Education_Demographics = model.Education_Demographics;
        //    campaignMatch.HouseholdStatus_Demographics = model.HouseholdStatus_Demographics;
        //    campaignMatch.MSCampaignProfileId = model.CampaignProfileId;
        //    campaignMatch.NextStatus = true;

        //    if (isAdd)
        //    {
        //        SQLServerEntities.CampaignMatch.Add(campaignMatch);
        //        SQLServerEntities.SaveChanges();
        //        return campaignMatch.CampaignProfileId;
        //    }
        //    else
        //    {
        //        SQLServerEntities.SaveChanges();
        //        return campaignMatch.CampaignProfileId;
        //    }
        //}

        //public int UpdateCampaignAdtypesWizard(CampaignProfileAdvertFormModel model, EFMVCDataContex SQLServerEntities)
        //{
        //    var campaignMatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
        //    bool isAdd = false;
        //    if (campaignMatch == null)
        //    {
        //        campaignMatch = new CampaignMatch();
        //        isAdd = true;
        //    }

        //    campaignMatch.Food_Advert = model.Food_Advert;
        //    campaignMatch.SweetSaltySnacks_Advert = model.SweetSaltySnacks_Advert;
        //    campaignMatch.AlcoholicDrinks_Advert = model.AlcoholicDrinks_Advert;
        //    campaignMatch.NonAlcoholicDrinks_Advert = model.NonAlcoholicDrinks_Advert;
        //    campaignMatch.Householdproducts_Advert = model.Householdproducts_Advert;
        //    campaignMatch.ToiletriesCosmetics_Advert = model.ToiletriesCosmetics_Advert;
        //    campaignMatch.PharmaceuticalChemistsProducts_Advert = model.PharmaceuticalChemistsProducts_Advert;
        //    campaignMatch.TobaccoProducts_Advert = model.TobaccoProducts_Advert;
        //    campaignMatch.PetsPetFood_Advert = model.PetsPetFood_Advert;
        //    campaignMatch.ShoppingRetailClothing_Advert = model.ShoppingRetailClothing_Advert;
        //    campaignMatch.DIYGardening_Advert = model.DIYGardening_Advert;
        //    campaignMatch.ElectronicsOtherPersonalItems_Advert = model.ElectronicsOtherPersonalItems_Advert;
        //    campaignMatch.CommunicationsInternet_Advert = model.CommunicationsInternet_Advert;
        //    campaignMatch.FinancialServices_Advert = model.FinancialServices_Advert;
        //    campaignMatch.HolidaysTravel_Advert = model.HolidaysTravel_Advert;
        //    campaignMatch.SportsLeisure_Advert = model.SportsLeisure_Advert;
        //    campaignMatch.Motoring_Advert = model.Motoring_Advert;
        //    campaignMatch.Newspapers_Advert = model.Newspapers_Advert;
        //    campaignMatch.TV_Advert = model.TV_Advert;
        //    campaignMatch.Cinema_Advert = model.Cinema_Advert;
        //    campaignMatch.SocialNetworking_Advert = model.SocialNetworking_Advert;
        //    campaignMatch.Shopping_Advert = model.Shopping_Advert;
        //    campaignMatch.Fitness_Advert = model.Fitness_Advert;
        //    campaignMatch.Environment_Advert = model.Environment_Advert;
        //    campaignMatch.GoingOut_Advert = model.GoingOut_Advert;
        //    campaignMatch.Religion_Advert = model.Religion_Advert;
        //    campaignMatch.Music_Advert = model.Music_Advert;
        //    campaignMatch.BusinessOrOpportunities_AdType = model.BusinessOrOpportunities_AdType;
        //    campaignMatch.Gambling_AdType = model.Gambling_AdType;
        //    campaignMatch.Restaurants_AdType = model.Restaurants_AdType;
        //    campaignMatch.Insurance_AdType = model.Insurance_AdType;
        //    campaignMatch.Furniture_AdType = model.Furniture_AdType;
        //    campaignMatch.InformationTechnology_AdType = model.InformationTechnology_AdType;
        //    campaignMatch.Energy_AdType = model.Energy_AdType;
        //    campaignMatch.Supermarkets_AdType = model.Supermarkets_AdType;
        //    campaignMatch.Healthcare_AdType = model.Healthcare_AdType;
        //    campaignMatch.JobsAndEducation_AdType = model.JobsAndEducation_AdType;
        //    campaignMatch.Gifts_AdType = model.Gifts_AdType;
        //    campaignMatch.AdvocacyOrLegal_AdType = model.AdvocacyOrLegal_AdType;
        //    campaignMatch.DatingAndPersonal_AdType = model.DatingAndPersonal_AdType;
        //    campaignMatch.RealEstate_AdType = model.RealEstate_AdType;
        //    campaignMatch.Games_AdType = model.Games_AdType;
        //    campaignMatch.MSCampaignProfileId = model.CampaignProfileId;
        //    campaignMatch.NextStatus = true;

        //    if (isAdd)
        //    {
        //        SQLServerEntities.CampaignMatch.Add(campaignMatch);
        //        SQLServerEntities.SaveChanges();
        //        return campaignMatch.CampaignProfileId;
        //    }
        //    else
        //    {
        //        SQLServerEntities.SaveChanges();
        //        return campaignMatch.CampaignProfileId;
        //    }
        //}

        //public int UpdateUsageWizard(CampaignProfileMobileFormModel model, EFMVCDataContex SQLServerEntities)
        //{
        //    var campaignMatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == model.CampaignProfileId).FirstOrDefault();
        //    bool isAdd = false;
        //    if (campaignMatch == null)
        //    {
        //        campaignMatch = new CampaignMatch();
        //        isAdd = true;
        //    }
        //    campaignMatch.ContractType_Mobile = model.ContractType_Mobile;
        //    campaignMatch.Spend_Mobile = model.Spend_Mobile;
        //    campaignMatch.MSCampaignProfileId = model.CampaignProfileId;
        //    campaignMatch.NextStatus = true;

        //    if (isAdd)
        //    {
        //        SQLServerEntities.CampaignMatch.Add(campaignMatch);
        //        SQLServerEntities.SaveChanges();
        //        return campaignMatch.CampaignProfileId;
        //    }
        //    else
        //    {
        //        SQLServerEntities.SaveChanges();
        //        return campaignMatch.CampaignProfileId;
        //    }
        //}

        public void UpdateCampaignSmsAndEmailWizard(CampaignProfileFormModel campaignProfileFormModel, CampaignProfile campaigndetails, string emailFileLocation, string smsFileLocation, EFMVCDataContex SQLServerEntities)
        {
            var campaignmatch = SQLServerEntities.CampaignMatch.Where(s => s.MSCampaignProfileId == campaigndetails.CampaignProfileId).FirstOrDefault();
            if (campaignmatch != null)
            {
                campaignmatch.EMAIL_MESSAGE = campaignProfileFormModel.EmailBody;
                campaignmatch.SMS_MESSAGE = campaignProfileFormModel.SmsBody;
                campaignmatch.ORIGINATOR = campaignProfileFormModel.SmsOriginator;
                campaignmatch.EmailBody = campaignProfileFormModel.EmailBody;
                campaignmatch.EmailSenderAddress = campaignProfileFormModel.EmailSenderAddress;
                campaignmatch.EmailSubject = campaignProfileFormModel.EmailSubject;
                campaignmatch.SmsBody = campaignProfileFormModel.SmsBody;
                campaignmatch.SmsOriginator = campaignProfileFormModel.SmsOriginator;
                campaignmatch.EmailFileLocation = emailFileLocation;
                campaignmatch.SMSFileLocation = smsFileLocation;
                campaignmatch.StartDate = campaignProfileFormModel.StartDate;
                campaignmatch.EndDate = campaignProfileFormModel.EndDate;
                campaignmatch.MaxBid = Convert.ToInt32(campaigndetails.MaxBid);
                campaignmatch.MaxMonthBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                campaignmatch.MaxWeeklyBudget = Convert.ToInt64(campaigndetails.MaxWeeklyBudget);
                campaignmatch.MaxHourlyBudget = Convert.ToInt64(campaigndetails.MaxHourlyBudget);
                campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                campaignmatch.CampaignDescription = campaigndetails.CampaignDescription;
                campaignmatch.CampaignName = campaigndetails.CampaignName;
                campaignmatch.ClientId = campaigndetails.ClientId;
                campaignmatch.Status = campaigndetails.Status;
                campaignmatch.Active = campaigndetails.Active;
                campaignmatch.AvailableCredit = campaigndetails.AvailableCredit.ToString();
                campaignmatch.CancelledCurrentMonth = campaigndetails.CancelledCurrentMonth;
                campaignmatch.CancelledLastMonth = campaigndetails.CancelledLastMonth;
                campaignmatch.CancelledToDate = campaigndetails.CancelledToDate;
                campaignmatch.CreatedDateTime = campaigndetails.CreatedDateTime;
                campaignmatch.EmailToDate = campaigndetails.EmailToDate;
                campaignmatch.EmailsCurrentMonth = campaigndetails.EmailsCurrentMonth;
                campaignmatch.EmailsLastMonth = campaigndetails.EmailsLastMonth;
                campaignmatch.TotalCredit = Convert.ToInt64(campaigndetails.TotalCredit);
                campaignmatch.SpentToDate = campaigndetails.SpendToDate.ToString();
                campaignmatch.MaxDailyBudget = Convert.ToInt64(campaigndetails.MaxDailyBudget);
                campaignmatch.PlaysCurrentMonth = campaigndetails.PlaysCurrentMonth;
                campaignmatch.PlaysLastMonth = campaigndetails.PlaysLastMonth;
                campaignmatch.PlaysToDate = campaigndetails.PlaysToDate;
                campaignmatch.SmsCurrentMonth = campaigndetails.SmsCurrentMonth;
                campaignmatch.SmsLastMonth = campaigndetails.SmsLastMonth;
                campaignmatch.SmsToDate = campaigndetails.SmsToDate;
                campaignmatch.TotalBudget = Convert.ToInt64(campaigndetails.TotalBudget);
                campaignmatch.UpdatedDateTime = campaigndetails.UpdatedDateTime;
                campaignmatch.UserId = campaigndetails.UserId;
                campaignmatch.NumberInBatch = campaigndetails.NumberInBatch;
                SQLServerEntities.SaveChanges();
            }
        }

        public DataTable ExecuteSP(string spname, string conn)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable dt = new DataTable();
                        con.Open();
                        da.Fill(dt);
                        con.Close();
                        return dt;
                    }
                }
            }
        }
    }
}