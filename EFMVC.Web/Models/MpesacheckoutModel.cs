using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class MpesacheckoutModel
    {
        [Required]
        public int Amount { get; set; }

        public string Access_Token { get; set; }
        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>The response code.</value>
        [DeserializeAs(Name = "ResponseCode")]
        public int ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the checkout request identifier.
        /// </summary>
        /// <value>The checkout request identifier.</value>
        public string CheckoutRequestID { get; set; }

        /// <summary>
        /// Gets or sets the customer message.
        /// </summary>
        /// <value>The customer message.</value>
        public string CustomerMessage { get; set; }

        /// <summary>
        /// Gets or sets the merchant request identifier.
        /// </summary>
        /// <value>The merchant request identifier.</value>
        public string MerchantRequestID { get; set; }


        [DeserializeAs(Name = "ConversationID")]
        public string ConversationId { get; set; }

        /// <summary>
        /// Gets or sets the originator coversation identifier.
        /// </summary>
        /// <value>The originator coversation identifier.</value>
        [DeserializeAs(Name = "OriginatorCoversationID")]
        public string OriginatorCoversationId { get; set; }

        /// <summary>
        /// Gets or sets the response description.
        /// </summary>
        /// <value>The response description.</value>
        [DeserializeAs(Name = "ResponseDescription")]
        public string ResponseDescription { get; set; }

    }

    public class MpesaPaymentRequest
    {

        public string requestId { get; set; }

        public string errorCode { get; set; }

        public string errorMessage { get; set; }

        public string MerchantRequestID { get; set; }

        public string CheckoutRequestID { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseDescription { get; set; }
        public string CustomerMessage { get; set; }

      
    }

    public class MpesaPaymentResponse
    {
        public string receiptNo { get; set; }
        public string transactionType { get; set; }
        public string Description { get; set; }
        public string accountReference { get; set; }
        public decimal Amount { get; set; }
        public string phoneNumber { get; set; }
    }
}
