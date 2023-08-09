using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
  public  class CreateOrUpdateUserProfileDemographicsCommand : ICommand
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public string Gender_Demographics { get; set; }
        public string IncomeBracket_Demographics { get; set; }
        public string WorkingStatus_Demographics { get; set; }
        public string RelationshipStatus_Demographics { get; set; }
        public string Education_Demographics { get; set; }

        public string HouseholdStatus_Demographics { get; set; }
        public string Location_Demographics { get; set; }
        public string Postcode { get; set; }
        public int? CountryId { get; set; }
        public int OperatorId { get; set; }
    }
}
