

using System.ComponentModel.DataAnnotations;


namespace EFMVC.Model
{
   public class CompanyDetails
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public string CompanyName { get; set; }
        public string Address { get; set; }

        public string AdditionalAddress { get; set; }

        public string Town { get; set; }
        public string PostCode { get; set; }
        public int? CountryId { get; set; }
        public int? AdtoneServerCompanyDetailId { get; set; }
        public virtual Country Country { get; set; }
    }
}
