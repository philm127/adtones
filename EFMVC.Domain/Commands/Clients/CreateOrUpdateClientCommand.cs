using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.Clients
{
    public class CreateOrUpdateClientCommand : ICommand
    {
        
        /// </summary>
        /// <value>The client identifier.</value>
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description of the client.
        /// </summary>
        /// <value>The Description of the client.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the client ContactInfo.
        /// </summary>
        /// <value>The client ContactInfo.</value>
        public string ContactInfo { get; set; }

        /// Gets or sets the client CreatedDate.
        /// </summary>
        /// <value>The client CreatedDate.</value>
        /// 
        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        /// Gets or sets the client Status.
        /// </summary>
        /// <value>The client Status.</value>
        /// 
        public int Status { get; set; }


        /// <summary>
        /// Gets or sets the Budget.
        /// </summary>
        /// <value>The Budget.</value>
        public decimal Budget { get; set; }
        public int CountryId { get; set; }

    }
}
