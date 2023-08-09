using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands
{
    public class CreateOrUpdateUsersCreditCommand : ICommand
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CurrencyId { get; set; }

        public decimal AssignCredit { get; set; }

        public decimal AvailableCredit { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
