using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands.Clients
{
    public class CreateOrUpdateCopyClientCommand : ICommand
    {
        public int ClientId { get; set; }

        public int UserId { get; set; }

        public string ClientName { get; set; }

        public string ClientDescription { get; set; }

        public string ClientContactInfo { get; set; }

        public decimal ClientBudget { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int ClientStatus { get; set; }

        public string ClientEmail { get; set; }

        public string ClientPhoneticAlphabet { get; set; }

        public bool NextStatus { get; set; }

        public int CountryId { get; set; }

        public string ClientContactPhone { get; set; }

        public int? AdtoneServerClientId { get; set; }
    }
}