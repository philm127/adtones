using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Common
{
    public class UpdateSessionTimeOut
    {
        /// <summary>
        /// The form authentication
        /// </summary>
        private readonly IFormsAuthentication formAuthentication;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        public UpdateSessionTimeOut(IFormsAuthentication formAuthentication, IUserRepository userRepository)
        {
            this.formAuthentication = formAuthentication;
            _userRepository = userRepository;
        }

        public void SessionTimeout(EFMVCUser efmvcUser)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            User user = _userRepository.GetById(efmvcUser.UserId);
            formAuthentication.SetAuthCookie(HttpContext.Current,
                                UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                    user));
        }
    }
}