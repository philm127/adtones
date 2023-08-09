using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.Contacts
{
    public class CreateOrUpdateContactsCommand : ICommand
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string MobileNumber { get; set; }


        public string FixedLine { get; set; }


        public string Email { get; set; }


        public string PhoneNumber { get; set; }


        public string Address { get; set; }

        public int? CountryId { get; set; }

        public int? CurrencyId { get; set; }

    }

}
