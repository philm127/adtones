using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands.Security
{
   public class ChangeUserProfileInfoCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the organisation.
        /// </summary>
        /// <value>
        /// The organisation.
        /// </value>
        public string Organisation { get; set; }

        /// <summary>
        /// Gets or sets the organisation.
        /// </summary>
        /// <value>
        /// The organisation.
        /// </value>
        public bool VerificationStatus { get; set; }

        ////Commented 19-02-2019
        ////Add 15-02-2019
        ///// <summary>
        ///// Gets or sets the organisation.
        ///// </summary>
        ///// <value>
        ///// The organisation.
        ///// </value>
        //public int? RewardId { get; set; }
    }
}
