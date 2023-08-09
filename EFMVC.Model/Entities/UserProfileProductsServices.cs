using System.ComponentModel.DataAnnotations;


namespace EFMVC.Model {
    
    public class UserProfileProductsService {
        [Key]
        public int UserProfileProductsServicesId { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
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
    }
}
