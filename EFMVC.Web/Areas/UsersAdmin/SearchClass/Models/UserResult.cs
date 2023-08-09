using EFMVC.Model;
using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.UsersAdmin.Models
{
    public class UserResult
    {
        public int UserId { get; set; }
        public int Id {
            get
            {
                return UserId;
            }
        }
        public int RoleId { get; set; }
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int NoOfactivecampaign { get; set; }
        public int NoOfunapprovedadverts { get; set; }
        public double Creditlimit { get; set; }
        public double Outstandinginvoice { get; set; }
        public int Activated { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedDate
        {
            get
            {
                return DateCreated.Value.ToString("dd/MM/yyyy");
            }
        }

        public string Status
        {
            get
            {
                var str = string.Empty;
                if (Activated == 0)
                {
                    str = "Pending";
                }
                else if (Activated == 1)
                {
                    str = "Approved";
                }
                else if (Activated == 2)
                {
                    str = "Suspended";
                }
                else
                {
                    str = "Deleted";
                }
                return str;
            }
        }

        public string fstatus { get; set; }

        public string MSISDN { get; set; }

        public string RoleName { get; set; }
        public int OperatorId { get; set; }
        public int CountryId { get; set; }
    }


  
}