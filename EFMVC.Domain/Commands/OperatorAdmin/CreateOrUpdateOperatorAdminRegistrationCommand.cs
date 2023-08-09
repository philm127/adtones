using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands.OperatorAdmin
{
    public class CreateOrUpdateOperatorAdminRegistrationCommand : ICommand
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Organisation { get; set; }

        public string Password { get; set; }

        public int OperatorId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastLoginTime { get; set; }

        public int Activated { get; set; }

        public int RoleId { get; set; }

        public bool VerificationStatus { get; set; }

        public int Outstandingdays { get; set; }

        public bool IsMsisdnMatch { get; set; }

        public bool IsEmailVerfication { get; set; }

        public string PhoneticAlphabet { get; set; }

        public bool IsMobileVerfication { get; set; }

        public int? OrganisationTypeId { get; set; }

        public string UserMatchTableName { get; set; }

        public int CountryId { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
    }
}
