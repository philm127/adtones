
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Configurations
{
    public class UserProfilePreferenceConfiguration : EntityTypeConfiguration<UserProfilePreference>
    {
        public UserProfilePreferenceConfiguration()
        {
            ToTable("UserProfilePreference");
            Property(u => u.UserProfileId).IsRequired();
            Property(u => u.Gender_Demographics);
            Property(u => u.IncomeBracket_Demographics);
            Property(u => u.WorkingStatus_Demographics);
            Property(u => u.RelationshipStatus_Demographics);
            Property(u => u.Education_Demographics);
            Property(u => u.HouseholdStatus_Demographics);
            Property(u => u.Location_Demographics);

            Property(u => u.Food_Advert);
            Property(u => u.SweetSaltySnacks_Advert);
            Property(u => u.AlcoholicDrinks_Advert);
            Property(u => u.NonAlcoholicDrinks_Advert);
            Property(u => u.Householdproducts_Advert);
            Property(u => u.ToiletriesCosmetics_Advert);
            Property(u => u.PharmaceuticalChemistsProducts_Advert);
            Property(u => u.TobaccoProducts_Advert);
            Property(u => u.PetsPetFood_Advert);
            Property(u => u.ShoppingRetailClothing_Advert);
            Property(u => u.DIYGardening_Advert);
            Property(u => u.AppliancesOtherHouseholdDurables_Advert);
            Property(u => u.ElectronicsOtherPersonalItems_Advert);
            Property(u => u.CommunicationsInternet_Advert);
            Property(u => u.FinancialServices_Advert);
            Property(u => u.HolidaysTravel_Advert);
            Property(u => u.SportsLeisure_Advert);
            Property(u => u.Motoring_Advert);
            Property(u => u.Newspapers_Advert);
            Property(u => u.Magazines_Advert);
            Property(u => u.TV_Advert);
            Property(u => u.Radio_Advert);
            Property(u => u.Cinema_Advert);
            Property(u => u.SocialNetworking_Advert);
            Property(u => u.GeneralUse_Advert);
            Property(u => u.Shopping_Advert);
            Property(u => u.Fitness_Advert);
            Property(u => u.Holidays_Advert);
            Property(u => u.Environment_Advert);
            Property(u => u.GoingOut_Advert);
            Property(u => u.FinancialProducts_Advert);
            Property(u => u.Religion_Advert);
            Property(u => u.Fashion_Advert);
            Property(u => u.Music_Advert);

            Property(u => u.Fitness_Attitude);
            Property(u => u.Holidays_Attitude);
            Property(u => u.Environment_Attitude);
            Property(u => u.GoingOut_Attitude);
            Property(u => u.FinancialStabiity_Attitude);
            Property(u => u.Religion_Attitude);
            Property(u => u.Fashion_Attitude);
            Property(u => u.Music_Attitude);

            Property(u => u.Cinema_Cinema);

            Property(u => u.SocialNetworking_Internet);
            Property(u => u.Video_Internet);
            Property(u => u.Research_Internet);
            Property(u => u.Auctions_Internet);
            Property(u => u.Shopping_Internet);

            Property(u => u.ContractType_Mobile);
            Property(u => u.Spend_Mobile);

            Property(u => u.Local_Press);
            Property(u => u.National_Press);
            Property(u => u.FreeNewpapers_Press);
            Property(u => u.Magazines_Press);

            Property(u => u.Food_ProductsService);
            Property(u => u.SweetSaltySnacks_ProductsService);
            Property(u => u.AlcoholicDrinks_ProductsService);
            Property(u => u.NonAlcoholicDrinks_ProductsService);
            Property(u => u.Householdproducts_ProductsService);
            Property(u => u.ToiletriesCosmetics_ProductsService);
            Property(u => u.PharmaceuticalChemistsProducts_ProductsService);
            Property(u => u.TobaccoProducts_ProductsService);
            Property(u => u.PetsPetFood_ProductsService);
            Property(u => u.ShoppingRetailClothing_ProductsService);
            Property(u => u.DIYGardening_ProductsService);
            Property(u => u.AppliancesOtherHouseholdDurables_ProductsService);
            Property(u => u.ElectronicsOtherPersonalItems_ProductsService);
            Property(u => u.CommunicationsInternet_ProductsService);
            Property(u => u.FinancialServices_ProductsService);
            Property(u => u.HolidaysTravel_ProductsService);
            Property(u => u.SportsLeisure_ProductsService);
            Property(u => u.Motoring_ProductsService);

            Property(u => u.National_Radio);
            Property(u => u.Local_Radio);
            Property(u => u.Music_Radio);
            Property(u => u.Sport_Radio);
            Property(u => u.Talk_Radio);


            Property(u => u.Satallite_TV);
            Property(u => u.Cable_TV);
            Property(u => u.Terrestrial_TV);
            Property(u => u.Internet_TV);
            Property(u => u.Postcode).HasMaxLength(100);

            Property(u => u.BusinessOrOpportunities_AdType).HasMaxLength(50);
            Property(u => u.Gambling_AdType).HasMaxLength(50);
            Property(u => u.Restaurants_AdType).HasMaxLength(50);
            Property(u => u.Insurance_AdType).HasMaxLength(50);
            Property(u => u.Furniture_AdType).HasMaxLength(50);
            Property(u => u.InformationTechnology_AdType).HasMaxLength(50);
            Property(u => u.Energy_AdType).HasMaxLength(50);
            Property(u => u.Supermarkets_AdType).HasMaxLength(50);
            Property(u => u.Healthcare_AdType).HasMaxLength(50);
            Property(u => u.JobsAndEducation_AdType).HasMaxLength(50);
            Property(u => u.Gifts_AdType).HasMaxLength(50);
            Property(u => u.AdvocacyOrLegal_AdType).HasMaxLength(50);
            Property(u => u.DatingAndPersonal_AdType).HasMaxLength(50);
            Property(u => u.RealEstate_AdType).HasMaxLength(50);
            Property(u => u.Games_AdType).HasMaxLength(50);
            //Property(u => u.SkizaProfile_AdType).HasMaxLength(50);

            Property(u => u.Hustlers_AdType).HasMaxLength(50);
            Property(u => u.Youth_AdType).HasMaxLength(50);
            Property(u => u.DiscerningProfessionals_AdType).HasMaxLength(50);
            Property(u => u.Mass_AdType).HasMaxLength(50);

        }
    }
}
