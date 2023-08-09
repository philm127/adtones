using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
   public class CreateOrUpdateAdvertCategoryCommand : ICommand
    {
        public int AdvertCategoryId { get; set; }

        public string Name { get; set; }
        public int? CountryId { get; set; }

        public int? AdtoneServerAdvertCategoryId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
