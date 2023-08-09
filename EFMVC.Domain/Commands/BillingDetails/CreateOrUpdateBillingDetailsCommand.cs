using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.BillingDetails
{
   public class CreateOrUpdateBillingDetailsCommand : ICommand
    {
        [Key]
        public int Id { get; set; }

        public int? BillingId { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public string NameOfCard { get; set; }

        public string SecurityCode { get; set; }

        public string BillingAddress { get; set; }

        public string BillingAddress2 { get; set; }

        public string BillingTown { get; set; }


        public string BillingPostcode { get; set; }

        public string PaypalEmail { get; set; }

        public string PaypalTranID { get; set; }

        

    }
}
