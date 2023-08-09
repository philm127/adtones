using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.CompanyDetails
{
    public class CreateOrUpdateCompanyDetailsCommand : ICommand
    {
        public int Id { get; set; }
        public int UserId { get; set; }
     
        public string CompanyName { get; set; }
        public string Address { get; set; }

        public string AdditionalAddress { get; set; }

        public string Town { get; set; }
        public string PostCode { get; set; }
        public int CountryId { get; set; }
    }
}
