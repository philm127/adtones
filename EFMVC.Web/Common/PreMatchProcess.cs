using EFMVC.Data;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Common
{
    public class PreMatchProcess
    {        

        public static void PreCampaignUsermatchProcess(int userId, string userMatchTableNumber, string conn)
        {

            if (userMatchTableNumber == "UserMatch1") 
            {
                ExecuteSP("CampaignUserMatchSpByUserId", conn, userId);
            }
            else if (userMatchTableNumber == "UserMatch2") 
            {
                ExecuteSP("CampaignUserMatchSp2ByUserId", conn, userId);
            }
            else if (userMatchTableNumber == "UserMatch3") 
            {
                ExecuteSP("CampaignUserMatchSp3ByUserId", conn, userId);
            }
            else if (userMatchTableNumber == "UserMatch4") 
            {
                ExecuteSP("CampaignUserMatchSp4ByUserId", conn, userId);
            }
            else if (userMatchTableNumber == "UserMatch5") 
            {
                ExecuteSP("CampaignUserMatchSp5ByUserId", conn, userId);
            }
            else if (userMatchTableNumber == "UserMatch6") 
            {
                ExecuteSP("CampaignUserMatchSp6ByUserId", conn, userId);
            }
            else if (userMatchTableNumber == "UserMatch7") 
            {
                ExecuteSP("CampaignUserMatchSp7ByUserId", conn, userId);
            }
            else if (userMatchTableNumber == "UserMatch8") 
            {
                ExecuteSP("CampaignUserMatchSp8ByUserId", conn, userId);
            }
            else if (userMatchTableNumber == "UserMatch9") 
            {
                ExecuteSP("CampaignUserMatchSp9ByUserId", conn, userId);
            }
            else if (userMatchTableNumber == "UserMatch10") 
            {
                ExecuteSP("CampaignUserMatchSp10ByUserId", conn, userId);
            }

            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(conn);
            using (SQLServerEntities)
            {
                var userProfileData = SQLServerEntities.Userprofiles.Where(s => s.UserId == userId).FirstOrDefault();
                if (userProfileData != null)
                {
                    var userProfileId = userProfileData.UserProfileId;
                    var campaignIdList = SQLServerEntities.PreMatch.Where(s => s.MsUserProfileId == userProfileId.ToString()).Select(s => s.MSCampaignProfileId).ToList();
                    if (campaignIdList.Count() > 0)
                    {
                        foreach (var item in campaignIdList)
                        {
                            var campId = Convert.ToInt32(item);
                            UpdateCampaignBudget(SQLServerEntities, campId);
                        }
                    }
                }
            }
        }

        
        public static void PrematchProcessForCampaign(int campaignId, string conn)
        {          
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("CampaignUserMatchSpByCampaignId", con))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CampaignProfileId", campaignId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            EFMVCDataContex SQLServerEntities = new EFMVCDataContex(conn);
            UpdateCampaignBudget(SQLServerEntities, campaignId);
        }

        private static void UpdateCampaignBudget(EFMVCDataContex SQLServerEntities, int campaignId)
        {
            //var campaignBudgetData = SQLServerEntities.CampaignBudget.Where(s => s.CampaignProfileId == campaignId).FirstOrDefault();
            //if (campaignBudgetData != null)
            //{
                var campaignData = SQLServerEntities.CampaignProfiles.Where(s => s.CampaignProfileId == campaignId).FirstOrDefault();

            if (campaignData != null)
            {
                //var remainingMaxMonthBudget = campaignData.RemainingMaxMonthBudget;
                //var remainingMaxDailyBudget = campaignData.RemainingMaxDailyBudget;
                //var remainingMaxWeeklyBudget = campaignData.RemainingMaxWeeklyBudget;
                //var remainingMaxHourlyBudget = campaignData.RemainingMaxHourlyBudget;

                //ArrayList sortArrayList = new ArrayList();
                //sortArrayList.Add(remainingMaxMonthBudget);
                //sortArrayList.Add(remainingMaxDailyBudget);
                //sortArrayList.Add(remainingMaxWeeklyBudget);
                //sortArrayList.Add(remainingMaxHourlyBudget);
                //sortArrayList.Sort();

                //var minimumValue =  sortArrayList[0];
                //var bucketCount = campaignData.AvailableCredit / campaignData.MaxBid;
                //campaignBudgetData.BucketCount = (int)bucketCount;

                //campaignBudgetData.AvailableBudget = Convert.ToDecimal(campaignData.TotalCredit) + Convert.ToDecimal(minimumValue);
                // var bucketCount =  (float)campaignBudgetData.AvailableBudget / campaignData.MaxBid;
                float bucketCount = (float)campaignData.MaxHourlyBudget / campaignData.MaxBid;
                if (bucketCount > 0)
                {
                    campaignData.BucketCount = (int)bucketCount;
                }
                else
                {
                    campaignData.BucketCount = 0;
                }
                SQLServerEntities.SaveChanges();
            }
           // }

        }


        public static void ExecuteSP(string spname, string conn, int userId)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand(spname, con))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }


    }
}