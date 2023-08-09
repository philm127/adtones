// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Administrator
// Created          : 05-09-2014
//
// Last Modified By : Administrator
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="Bootstrapper.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Web.Common;
using EFMVC.Web.Core;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Mappers;
using Adtones.Rollups.Data.Services;

/// <summary>
/// The Web namespace.
/// </summary>

namespace EFMVC.Web
{
    /// <summary>
    /// Class Bootstrapper.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Runs this instance.
        /// </summary>
        public static void Run()
        {
            SetAutofacContainer();
            AutoMapperConfiguration.Configure();
        }

        /// <summary>
        /// Sets the autofac container.
        /// </summary>
        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<DefaultCommandBus>().As<ICommandBus>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof (ProfileRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            Assembly services = Assembly.Load("EFMVC.Domain");
            builder.RegisterAssemblyTypes(services)
                .AsClosedTypesOf(typeof (ICommandHandler<>)).InstancePerRequest();
            builder.RegisterAssemblyTypes(services)
                .AsClosedTypesOf(typeof (IValidationHandler<>)).InstancePerRequest();
            builder.RegisterType<DefaultFormsAuthentication>().As<IFormsAuthentication>().InstancePerRequest();
            builder.RegisterFilterProvider();
            builder.RegisterType<InMemoryCache>().As<ICacheService>().InstancePerRequest();
            builder.RegisterType<CampaignDashboardSummariesProvider>().AsSelf().InstancePerRequest();
            builder.Register(((c,p)=>
            {
                return new StatsProviderConfiguration
                {
                    StatsConnectionString = ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString,
                };
            })).AsSelf().SingleInstance();
            builder.RegisterType<StatsProvider>().AsSelf().InstancePerRequest();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}