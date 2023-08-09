using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands.ProfileMatchInfo
{
    public class CreateOrUpdateProfileMatchInfoCommand : ICommand
    {
        public int Id { get; set; }
       
        public string ProfileName { get; set; }

        public bool IsActive { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string ProfileType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
