using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands
{
   public class CreateOrUpdateOperatorMaxAdvertCommand : ICommand
    {
        public int OperatorMaxAdvertId { get; set; }
        public int OperatorId { get; set; }
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public DateTime Addeddate { get; set; }
        public Nullable<DateTime> Updateddate { get; set; }
        public Nullable<int> AdtoneServerOperatorMaxAdvertId { get; set; }
    }
}