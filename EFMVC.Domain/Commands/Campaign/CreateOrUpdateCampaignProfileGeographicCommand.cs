// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileAttitudeCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateCampaignProfileAttitudeCommand.
    /// </summary>
    public class CreateOrUpdateCampaignProfileGeographicCommand : ICommand
    {       
        public int CampaignProfileGeographicId { get; set; }
        public int CampaignProfileId { get; set; }
        public string PostCode { get; set; }
        public int CountryId { get; set; }
        public string Location_Demographics { get; set; }
        //public string CountryName { get; set; }
    }
}