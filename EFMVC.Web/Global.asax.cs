// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Administrator
// Created          : 05-09-2014
//
// Last Modified By : Administrator
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="Global.asax.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using EFMVC.Web.App_Start;
using EFMVC.Web.Controllers;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Areas.Admin.Controllers;
using System.Globalization;
using System.Threading;
using System.Configuration;
using System.Web.Http;
using Newtonsoft.Json;
using EFMVC.Core.Common;

/// <summary>
/// The Web namespace.
/// </summary>

namespace EFMVC.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// Class MvcApplication.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserFilter());
        }

        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { area = "", controller = "Landing", action = "Index", id = UrlParameter.Optional }

                );
            routes.MapRoute(
               name: "ClientCampaign",
               url: "{controller}/{action}/{id}",
               defaults: new { area = "", controller = "Dashboard", action = "Index", clientid = RouteParameter.Optional, advertid = RouteParameter.Optional }
               );
            routes.MapRoute(
             name: "AssignAdvertCampaign",
             url: "{controller}/{action}/{id}",
             defaults: new { area = "", controller = "Advert", action = "AddAdvert", campaignId = RouteParameter.Optional }
             );
            routes.MapRoute(
            name: "CampaignBudget",
            url: "{controller}/{action}/{id}",
            defaults: new { area = "", controller = "Billing", action = "RaisePo", clientId = RouteParameter.Optional, campaignId = RouteParameter.Optional }
            );

            routes.MapRoute(
                    name: "AdminError",
                    url: "Admin/{*url}",
                    defaults: new { controller = "Error", action = "NotFound", area = "Admin" }
            );

            routes.MapRoute(
                    name: "AdvertAdminError",
                    url: "AdvertAdmin/{*url}",
                    defaults: new { controller = "Error", action = "NotFound", area = "AdvertAdmin" }
            );

            routes.MapRoute(
                    name: "OperatorAdminError",
                    url: "OperatorAdmin/{*url}",
                    defaults: new { controller = "Error", action = "NotFound", area = "OperatorAdmin" }
            );

            routes.MapRoute(
                    name: "UserAdminError",
                    url: "UserAdmin/{*url}",
                    defaults: new { controller = "Error", action = "NotFound", area = "UserAdmin" }
            );

            routes.MapRoute(
                    name: "ProfileAdminError",
                    url: "ProfileAdmin/{*url}",
                    defaults: new { controller = "Error", action = "NotFound", area = "ProfileAdmin" }
            );

            routes.MapRoute(
                    name: "UsersError",
                    url: "Users/{*url}",
                    defaults: new { controller = "Error", action = "NotFound", area = "Users" }
            );

            routes.MapRoute(
                    name: "AdvertiserError",
                    url: "{*url}",
                    defaults: new { area = "", controller = "Error", action = "NotFound" }
            );

            //routes.MapRoute(
            //    name: "404-PageNotFound",
            //    url: "{*url}",
            //  defaults: new { area = "", controller = "Error", action = "Http404" }
            //);
        }

        /// <summary>
        /// Executes custom initialization code after all event handler modules have been added.
        /// </summary>
        public override void Init()
        {
            PostAuthenticateRequest += PostAuthenticateRequestHandler;
            base.Init();
        }

        /// <summary>
        /// Post authenticate request handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PostAuthenticateRequestHandler(object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (IsValidAuthCookie(authCookie))
            {
                var formsAuthentication = DependencyResolver.Current.GetService<IFormsAuthentication>();

                FormsAuthenticationTicket ticket = formsAuthentication.Decrypt(authCookie.Value);
                var efmvcUser = new EFMVCUser(ticket);
                string[] userRoles = { efmvcUser.RoleName };
                Context.User = new GenericPrincipal(efmvcUser, userRoles);
                formsAuthentication.SetAuthCookie(Context, ticket);
            }
        }

        /// <summary>
        /// Determines whether [is valid authentication cookie] [the specified authentication cookie].
        /// </summary>
        /// <param name="authCookie">The authentication cookie.</param>
        /// <returns><c>true</c> if [is valid authentication cookie] [the specified authentication cookie]; otherwise, <c>false</c>.</returns>
        private static bool IsValidAuthCookie(HttpCookie authCookie)
        {
            return authCookie != null && !String.IsNullOrEmpty(authCookie.Value);
        }


        /// <summary>
        /// Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            RouteTable.Routes.MapHubs();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();
            BootstrapBundleConfig.RegisterBundles();

            Bootstrapper.Run();

            //            Database.SetInitializer(new CreateDatabaseIfNotExists<EFMVCDataContex>());
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {

            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception exc = HttpContext.Current.Server.GetLastError();
                var httpContext = ((MvcApplication)sender).Context;
                var currentController = " ";
                var currentAction = " ";
                var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
                if (currentRouteData != null)
                {
                    if(httpContext.Request.Path.ToLower().StartsWith("/admin/") || httpContext.Request.Path.ToLower().StartsWith("/advertadmin/") || httpContext.Request.Path.ToLower().StartsWith("/operatoradmin/") || httpContext.Request.Path.ToLower().StartsWith("/usersadmin/") || httpContext.Request.Path.ToLower().StartsWith("/profileadmin/") || httpContext.Request.Path.ToLower().StartsWith("/users/"))
                    {
                        AdminRouting(httpContext, ref currentController, ref currentAction, currentRouteData);
                    }
                    else
                    {
                        FrontRouting(httpContext, ref currentController, ref currentAction, currentRouteData);
                    }
                }
                else
                {
                    var urlStr = httpContext.Request.Url.ToString() ?? "";
                    string reqStr = JsonConvert.SerializeObject(httpContext.Request, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, ContractResolver = new IgnoreErrorPropertiesResolver(), PreserveReferencesHandling = PreserveReferencesHandling.None, NullValueHandling = NullValueHandling.Ignore }) ?? "";
                    string respStr = JsonConvert.SerializeObject(httpContext.Request, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, ContractResolver = new IgnoreErrorPropertiesResolver(), PreserveReferencesHandling = PreserveReferencesHandling.None, NullValueHandling = NullValueHandling.Ignore }) ?? "";
                    string exStr = JsonConvert.SerializeObject(exc, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, ContractResolver = new IgnoreErrorPropertiesResolver(), PreserveReferencesHandling = PreserveReferencesHandling.None, NullValueHandling = NullValueHandling.Ignore }) ?? "";

                    bool isAjaxCall = string.Equals("XMLHttpRequest", httpContext.Request.Headers["x-requested-with"], StringComparison.OrdinalIgnoreCase);

                    if (isAjaxCall)
                    {
                        httpContext.ClearError();
                        string ajaxExStr = JsonConvert.SerializeObject(exc, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, ContractResolver = new IgnoreErrorPropertiesResolver(), PreserveReferencesHandling = PreserveReferencesHandling.None, NullValueHandling = NullValueHandling.Ignore }) ?? "";
                        httpContext.Response.ContentType = "application/json";
                        httpContext.Response.StatusCode = 200;
                        httpContext.Response.Write(exc.Message);
                        httpContext.Response.End();
                        return;
                    }

                    if (currentRouteData != null)
                    {
                        var routeData = new RouteData();
                        var action = "Index";
                        if (exc is HttpException)
                        {
                            var httpEx = exc as HttpException;
                            switch (httpEx.GetHttpCode())
                            {
                                case 404:
                                    action = "NotFound";
                                    break;
                                case 500:
                                    action = "ServerError";
                                    break;
                                default:
                                    action = "ServerError";
                                    break;
                            }
                        }

                        httpContext.ClearError();
                        httpContext.Response.Clear();
                        httpContext.Response.StatusCode = exc is HttpException ? ((HttpException)exc).GetHttpCode() : 500;
                        httpContext.Response.TrySkipIisCustomErrors = true;
                        Response.Redirect("/Error/" + action);
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                var httpContext = HttpContext.Current;
                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = 500;
                httpContext.Response.TrySkipIisCustomErrors = true;
                Response.Redirect("/Error/ServerError");
            }
            //TODO: Handle Exception
        }

        private void FrontRouting(HttpContext httpContext, ref string currentController, ref string currentAction, RouteData currentRouteData)
        {
            if (currentRouteData != null)
            {
                var ex = Server.GetLastError();
                var controller = new Controllers.ErrorController();
                var routeData = new RouteData();
                var action = "ServerError";
                if (ex is HttpException)
                {
                    var httpEx = ex as HttpException;
                    switch (httpEx.GetHttpCode())
                    {
                        case 404:
                            action = "NotFound";
                            break;
                        case 500:
                            action = "ServerError";
                            break;
                        default:
                            action = "ServerError";
                            break;
                    }
                }
                //pending
                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
                httpContext.Response.TrySkipIisCustomErrors = true;

                Response.Redirect("/Error/" + action);
            }
        }

        private void AdminRouting(HttpContext httpContext, ref string currentController, ref string currentAction, RouteData currentRouteData)
        {
            var ex = Server.GetLastError();

            var controller = new Areas.Admin.Controllers.ErrorController();
            var routeData = new RouteData();
            var action = "ServerError";

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;

                switch (httpEx.GetHttpCode())
                {
                    case 404:
                        action = "NotFound";
                        break;
                    case 500:
                        action = "ServerError";
                        break;
                    default:
                        action = "ServerError";
                        break;
                }
            }
            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;

            if (httpContext.Request.Path.ToLower().StartsWith("/admin/"))
            {
                Response.Redirect("/Admin/Error/" + action);
            }
            else if (httpContext.Request.Path.ToLower().StartsWith("/advertadmin/"))
            {
                Response.Redirect("/AdvertAdmin/Error/" + action);
            }
            else if (httpContext.Request.Path.ToLower().StartsWith("/operatoradmin/"))
            {
                Response.Redirect("/OperatorAdmin/Error/" + action);
            }
            else if (httpContext.Request.Path.ToLower().StartsWith("/usersadmin/"))
            {
                Response.Redirect("/UsersAdmin/Error/" + action);
            }
            else if (httpContext.Request.Path.ToLower().StartsWith("/profileadmin/"))
            {
                Response.Redirect("/ProfileAdmin/Error/" + action);
            }
            else if (httpContext.Request.Path.ToLower().StartsWith("/users/"))
            {
                Response.Redirect("/Users/Error/" + action);
            }
            else
            {
                Response.Redirect("/Error/" + action);
            }
        }
    }
}