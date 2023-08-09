using EFMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.OperatorServerData
{
    public class OperatorServer
    {
        public static int GetAdvertIdFromOperatorServer(EFMVCDataContex db, int AdvertId)
        {
            var advertData = db.Adverts.Where(s => s.AdtoneServerAdvertId == AdvertId).FirstOrDefault();
            return advertData != null ? advertData.AdvertId : 0;
        }

        public static int GetAdvertRejectionIdFromOperatorServer(EFMVCDataContex db, int advertRejectionId)
        {
            var advertRejectionData = db.AdvertRejection.Where(s => s.AdtoneServerAdvertRejectionId == advertRejectionId).FirstOrDefault();
            return advertRejectionData != null ? advertRejectionData.AdvertRejectionId : 0;
        }

        public static int GetBillingDetailIdFromOperatorServer(EFMVCDataContex db, int billingDetailId)
        {
            var billingDetailsData = db.BillingDetails.Where(s => s.AdtoneServerBillingDetailId == billingDetailId).FirstOrDefault();
            return billingDetailsData != null ? billingDetailsData.Id : 0;
        }

        public static int GetBillingIdFromOperatorServer(EFMVCDataContex db, int billingId)
        {
            var billingData = db.Billings.Where(s => s.AdtoneServerBillingId == billingId).FirstOrDefault();
            return billingData != null ? billingData.Id : 0;
        }

        public static int GetCampaignProfileIdFromOperatorServer(EFMVCDataContex db, int campaignProfileId)
        {
            var campaignProfileData = db.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == campaignProfileId).FirstOrDefault();
            return campaignProfileData != null ? campaignProfileData.CampaignProfileId : 0;
        }

        public static int GetCampaignAdvertIdFromOperatorServer(EFMVCDataContex db, int campaignAdvertId)
        {
            var campaignAdvertData = db.CampaignAdverts.Where(s => s.AdtoneServerCampaignAdvertId == campaignAdvertId).FirstOrDefault();
            return campaignAdvertData != null ? campaignAdvertData.CampaignAdvertId : 0;
        }

        public static int GetCampaignProfilePrefIdFromOperatorServer(EFMVCDataContex db, int campaignProfilePrefId)
        {
            var campaignProfilePrefData = db.CampaignProfilePreference.Where(s => s.AdtoneServerCampaignProfilePrefId == campaignProfilePrefId).FirstOrDefault();
            return campaignProfilePrefData != null ? campaignProfilePrefData.Id : 0;
        }

        public static int GetCampaignProfileTimeIdFromOperatorServer(EFMVCDataContex db, int campaignProfileTimeId)
        {
            var campaignProfileTimeData = db.CampaignProfileTimeSettings.Where(s => s.AdtoneServerCampaignProfileTimeId == campaignProfileTimeId).FirstOrDefault();
            return campaignProfileTimeData != null ? campaignProfileTimeData.CampaignProfileTimeSettingsId : 0;
        }

        public static int GetClientIdFromOperatorServer(EFMVCDataContex db, int clientId)
        {
            var clientData = db.Clients.Where(s => s.AdtoneServerClientId == clientId).FirstOrDefault();
            return clientData != null ? clientData.Id : 0;
        }

        public static int GetCompanyDetailsIdFromOperatorServer(EFMVCDataContex db, int companyDetailsId)
        {
            var companyDetailData = db.CompanyDetails.Where(s => s.AdtoneServerCompanyDetailId == companyDetailsId).FirstOrDefault();
            return companyDetailData != null ? companyDetailData.Id : 0;
        }

        public static int GetContactIdFromOperatorServer(EFMVCDataContex db, int contactId)
        {
            var contactData = db.Contacts.Where(s => s.AdtoneServerContactId == contactId).FirstOrDefault();
            return contactData != null ? contactData.Id : 0;
        }

        public static int GetUserIdFromOperatorServer(EFMVCDataContex db, int userId)
        {
            var userData = db.Users.Where(s => s.AdtoneServerUserId == userId).FirstOrDefault();
            return userData != null ? userData.UserId : 0;
        }

        public static int GetUserProfilePrefIdFromOperatorServer(EFMVCDataContex db, int userProfilePrefId)
        {
            var userProfilePrefData = db.UserProfilePreference.Where(s => s.AdtoneServerUserProfilePrefId == userProfilePrefId).FirstOrDefault();
            return userProfilePrefData != null ? userProfilePrefData.Id : 0;
        }

        public static int GetCountryIdFromOperatorServer(EFMVCDataContex db, int countryId)
        {
            var countryData = db.Country.Where(s => s.AdtoneServeCountryId == countryId).FirstOrDefault();
            return countryData != null ? countryData.Id : 0;
        }

        public static int GetUserProfileIdFromOperatorServer(EFMVCDataContex db, int userProfileId)
        {
            var userprofileData = db.Userprofiles.Where(s => s.AdtoneServerUserProfileId == userProfileId).FirstOrDefault();
            return userprofileData != null ? userprofileData.UserProfileId : 0;
        }

        public static int GetOperatorIdFromOperatorServer(EFMVCDataContex db, int operatorId)
        {
            var operatorData = db.Operator.Where(s => s.AdtoneServerOperatorId == operatorId).FirstOrDefault();
            return operatorData != null ? operatorData.OperatorId : 0;
        }

        public static int GetRewardIdFromOperatorServer(EFMVCDataContex db, int rewardId)
        {
            var rewardData = db.Rewards.Where(s => s.AdtoneServerRewardId == rewardId).FirstOrDefault();
            return rewardData != null ? rewardData.RewardId : 0;
        }


        public static int GetAdvertCategoryIdFromOperatorServer(EFMVCDataContex db, int advertCategoryId)
        {
            var advertCategoryData = db.AdvertCategory.Where(s => s.AdtoneServerAdvertCategoryId == advertCategoryId).FirstOrDefault();
            return advertCategoryData != null ? advertCategoryData.AdvertCategoryId : 0;
        }

        public static int GetPromotionalCampaignIdFromOperatorServer(EFMVCDataContex db, int campaignId)
        {
            var promotionalCampaignData = db.PromotionalCampaigns.Where(s => s.AdtoneServerPromotionalCampaignId == campaignId).FirstOrDefault();
            return promotionalCampaignData != null ? promotionalCampaignData.ID : 0;
        }

    }
}
