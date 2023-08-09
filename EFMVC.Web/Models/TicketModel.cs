using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Models
{
    public class TicketModel
    {
        public string id { get; set; }
        public string owner_contactid { get; set; }
        public string departmentid { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public string date_created { get; set; }
        public string public_access_urlcode { get; set; }
        public string subject { get; set; }
        public List<object> custom_fields { get; set; }
    }

    public class AgentModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string avatar_url { get; set; }
        public string online_status { get; set; }
        public string status { get; set; }
        public string gender { get; set; }
    }

    public class CustomField
    {
        public string code { get; set; }
        public string value { get; set; }
    }

    public class TicketUpdatable
    {
        public string agentid { get; set; }
        public List<CustomField> custom_fields { get; set; }
        public string departmentid { get; set; }
        public string owner_contactid { get; set; }
        public string status { get; set; }
        public List<string> tags { get; set; }
    }
}