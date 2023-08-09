// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="AutoMapperConfiguration.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using AutoMapper;

/// <summary>
/// The Mappers namespace.
/// </summary>

namespace EFMVC.Web.Mappers
{
    /// <summary>
    /// Class AutoMapperConfiguration.
    /// </summary>
    public class AutoMapperConfiguration
    {
        /// <summary>
        /// Configures this instance.
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(x =>
                                  {
                                      x.AddProfile<DomainToViewModelMappingProfile>();
                                      x.AddProfile<ViewModelToDomainMappingProfile>();
                                  });
        }
    }
}