using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Areas.Users.Models
{
    public class SocialLoginModel
    {
        public class Status
        {
            public string flag { get; set; }
            public int code { get; set; }
            public string info { get; set; }
        }

        public class Request
        {
            public string date { get; set; }
            public string resource { get; set; }
            public Status status { get; set; }
        }

        public class Status2
        {
            public string flag { get; set; }
            public int code { get; set; }
            public string info { get; set; }
        }

        public class Data2
        {
            public string action { get; set; }
            public string operation { get; set; }
            public string reason { get; set; }
            public string status { get; set; }
        }

        public class Plugin
        {
            public string key { get; set; }
            public Data2 data { get; set; }
        }

        public class Connection
        {
            public string connection_token { get; set; }
            public string date_creation { get; set; }
            public string callback_uri { get; set; }
            public string status { get; set; }
        }

        public class AccessToken
        {
            public string key { get; set; }
            public string date_expiration { get; set; }
        }

        public class RefreshToken
        {
            public string key { get; set; }
        }

        public class Source
        {
            public string name { get; set; }
            public string key { get; set; }
            public AccessToken access_token { get; set; }
            public RefreshToken refresh_token { get; set; }
        }

        public class Name
        {
            public string formatted { get; set; }
            public string givenName { get; set; }
            public string familyName { get; set; }
        }

        public class Email
        {
            public string value { get; set; }
            public bool is_verified { get; set; }
        }

        public class Url
        {
            public string value { get; set; }
            public string type { get; set; }
        }

        public class Account
        {
            public string domain { get; set; }
            public string userid { get; set; }
        }

        public class Locale
        {
            public string value { get; set; }
            public string description { get; set; }
        }

        public class Version
        {
            public string major { get; set; }
            public string full { get; set; }
        }

        public class Platform
        {
            public string name { get; set; }
            public string type { get; set; }
        }

        public class Browser
        {
            public string agent { get; set; }
            public string type { get; set; }
            public Version version { get; set; }
            public Platform platform { get; set; }
        }

        public class Identity
        {
            public string identity_token { get; set; }
            public string date_creation { get; set; }
            public string date_last_update { get; set; }
            public string provider { get; set; }
            public string provider_identity_uid { get; set; }
            public Source source { get; set; }
            public string id { get; set; }
            public string displayName { get; set; }
            public Name name { get; set; }
            public string preferredUsername { get; set; }
            public string profileUrl { get; set; }
            public string gender { get; set; }
            public List<Email> emails { get; set; }
            public List<Url> urls { get; set; }
            public List<Account> accounts { get; set; }
            public List<Locale> locales { get; set; }
            public Browser browser { get; set; }
        }

        public class Identity2
        {
            public string identity_token { get; set; }
            public string provider { get; set; }
        }

        public class User
        {
            public string uuid { get; set; }
            public string user_token { get; set; }
            public Identity identity { get; set; }
            public List<Identity2> identities { get; set; }
        }

        public class Data
        {
            public string action { get; set; }
            public Plugin plugin { get; set; }
            public Connection connection { get; set; }
            public User user { get; set; }
        }

        public class Result
        {
            public Status2 status { get; set; }
            public Data data { get; set; }
        }

        public class Response
        {
            public Request request { get; set; }
            public Result result { get; set; }
        }

        public class RootObject
        {
            public Response response { get; set; }
        }
    }
}