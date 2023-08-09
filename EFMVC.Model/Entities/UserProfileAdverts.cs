using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model {
    public class UserProfileAdvert {
        [Key]
        public int UserProfileAdvertsId { get; set; }
        public int UserProfileId { get; set; }
        public int? Food { get; set; }
        public int? SweetSaltySnacks { get; set; }
        public int? AlcoholicDrinks { get; set; }
        public int? NonAlcoholicDrinks { get; set; }
        public int? Householdproducts { get; set; }
        public int? ToiletriesCosmetics { get; set; }
        public int? PharmaceuticalChemistsProducts { get; set; }
        public int? TobaccoProducts { get; set; }
        public int? PetsPetFood { get; set; }
        public int? ShoppingRetailClothing { get; set; }
        public int? DIYGardening { get; set; }
        public int? AppliancesOtherHouseholdDurables { get; set; }
        public int? ElectronicsOtherPersonalItems { get; set; }
        public int? CommunicationsInternet { get; set; }
        public int? FinancialServices { get; set; }
        public int? HolidaysTravel { get; set; }
        public int? SportsLeisure { get; set; }
        public int? Motoring { get; set; }
        public int? Newspapers { get; set; }
        public int? Magazines { get; set; }
        public int? TV { get; set; }
        public int? Radio { get; set; }
        public int? Cinema { get; set; }
        public int? SocialNetworking { get; set; }
        public int? GeneralUse { get; set; }
        public int? Shopping { get; set; }
        public int? Fitness { get; set; }
        public int? Holidays { get; set; }
        public int? Environment { get; set; }
        public int? GoingOut { get; set; }
        public int? FinancialProducts { get; set; }
        public int? Religion { get; set; }
        public int? Fashion { get; set; }
        public int? Music { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
