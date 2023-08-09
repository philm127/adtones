using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
   public class CreateOrUpdateOperatorConfigurationCommand : ICommand
    {
        public int OperatorConfigurationId { get; set; }
        public int OperatorId { get; set; }
        public int Days { get; set; }
        public bool IsActive { get; set; }
        public DateTime Addeddate { get; set; }
        public Nullable<DateTime> Updateddate { get; set; }
        public Nullable<int> AdtoneServerOperatorConfigurationId { get; set; }
    }
}