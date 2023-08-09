
using System.Web;
using System.Web.Security;

namespace EFMVC.Web.Core.Authentication
{
    public interface IFormsAuthentication
    {       
        void Signout();
        string getRedirectionURL(string username, bool persistent);
        void SetAuthCookie(HttpContextBase httpContext, FormsAuthenticationTicket authenticationTicket);        
        void SetAuthCookie(HttpContext httpContext, FormsAuthenticationTicket authenticationTicket);
        FormsAuthenticationTicket Decrypt(string encryptedTicket);
    }
}