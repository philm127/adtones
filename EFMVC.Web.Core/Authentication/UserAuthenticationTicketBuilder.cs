
using System;
using System.Web.Security;
using EFMVC.Web.Core.Models;
using EFMVC.Model;


namespace EFMVC.Web.Core.Authentication
{
    public class UserAuthenticationTicketBuilder
    {
         public static FormsAuthenticationTicket CreateAuthenticationTicket(User user)
        {
            UserInfo userInfo = CreateUserContextFromUser(user);

            var ticket = new FormsAuthenticationTicket(
                1,
                user.FirstName,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                false,
                userInfo.ToString());

            return ticket;
        }

        public static UserInfo CreateUserContextFromUser(User user)
        {
            var userContext = new UserInfo
            {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                UserIdentifier = user.Email,
                RoleName=Enum.GetName(typeof(UserRoles),user.RoleId)
            };

            return userContext;
        }
    }
}