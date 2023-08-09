using System.ComponentModel.DataAnnotations;

namespace EFMVC.Model {
    public class CampaignProfileAdvert {
        [Key]
        public int CampaignProfileAdvertsId { get; set; }
        public int CampaignProfileId { get; set; }
        
        [StringLength(50)]
        public string Food { get; set; }

        [StringLength(50)]
        public string SweetSaltySnacks { get; set; }

        [StringLength(50)]
        public string AlcoholicDrinks { get; set; }

        [StringLength(50)]
        public string NonAlcoholicDrinks { get; set; }

        [StringLength(50)]
        public string Householdproducts { get; set; }

        [StringLength(50)]
        public string ToiletriesCosmetics { get; set; }

        [StringLength(50)]
        public string PharmaceuticalChemistsProducts { get; set; }

        [StringLength(50)]
        public string TobaccoProducts { get; set; }

        [StringLength(50)]
        public string PetsPetFood { get; set; }

        [StringLength(50)]
        public string ShoppingRetailClothing { get; set; }

        [StringLength(50)]
        public string DIYGardening { get; set; }

        [StringLength(50)]
        public string AppliancesOtherHouseholdDurables { get; set; }

        [StringLength(50)]
        public string ElectronicsOtherPersonalItems { get; set; }

        [StringLength(50)]
        public string CommunicationsInternet { get; set; }

        [StringLength(50)]
        public string FinancialServices { get; set; }

        [StringLength(50)]
        public string HolidaysTravel { get; set; }

        [StringLength(50)]
        public string SportsLeisure { get; set; }

        [StringLength(50)]
        public string Motoring { get; set; }

        [StringLength(50)]
        public string Newspapers { get; set; }

        [StringLength(50)]
        public string Magazines { get; set; }

        [StringLength(50)]
        public string TV { get; set; }

        [StringLength(50)]
        public string Radio { get; set; }

        [StringLength(50)]
        public string Cinema { get; set; }

        [StringLength(50)]
        public string SocialNetworking { get; set; }

        [StringLength(50)]
        public string GeneralUse { get; set; }

        [StringLength(50)]
        public string Shopping { get; set; }

        [StringLength(50)]
        public string Fitness { get; set; }

        [StringLength(50)]
        public string Holidays { get; set; }

        [StringLength(50)]
        public string Environment { get; set; }

        [StringLength(50)]
        public string GoingOut { get; set; }

        [StringLength(50)]
        public string FinancialProducts { get; set; }

        [StringLength(50)]
        public string Religion { get; set; }

        [StringLength(50)]
        public string Fashion { get; set; }

        [StringLength(50)]
        public string Music { get; set; }
    }
}
