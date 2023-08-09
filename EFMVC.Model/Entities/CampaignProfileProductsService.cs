using System.ComponentModel.DataAnnotations;


namespace EFMVC.Model {
    
    public class CampaignProfileProductsService {
        [Key]
        public int CampaignProfileProductsServicesId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }

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
    }
}
