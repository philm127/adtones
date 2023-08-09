using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;

namespace EFMVC.Web.Mailer
{
    public interface ISendEmailMailer
    {
        MvcMailMessage SendEmail(SendEmailModel model);
    }
}