using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Model
{
    public class Contacts
    {
        public Contacts()
        {

        }
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }


        public string MobileNumber { get; set; }


        public string FixedLine { get; set; }


        public string Email { get; set; }


        public string PhoneNumber { get; set; }


        public string Address { get; set; }

        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        public int? CurrencyId { get; set; }
        public int?  AdtoneServerContactId { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
