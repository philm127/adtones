using EFMVC.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Commands.Security
{
   public class ChangeUserRoleCommand : ICommand
    {   /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }


        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the outstandingdays.
        /// </summary>
        /// <value>
        /// The outstandingdays.
        /// </value>
        public int Outstandingdays { get; set; }

        /// <summary>
        /// Gets or sets the fname.
        /// </summary>
        /// <value>
        /// The fname.
        /// </value>
        public string Fname { get; set; }
        /// <summary>
        /// Gets or sets the lname.
        /// </summary>
        /// <value>
        /// The lname.
        /// </value>
        public string Lname { get; set; }
        /// <summary>
        /// Gets or sets the organisation.
        /// </summary>
        /// <value>
        /// The organisation.
        /// </value>
        public string Organisation { get; set; }
    }
}
