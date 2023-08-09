// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileAdvertConfiguration.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;
using EFMVC.Model;

/// <summary>
/// The Configurations namespace.
/// </summary>

namespace EFMVC.Data.Configurations
{
    /// <summary>
    /// Class CampaignProfileAdvertConfiguration.
    /// </summary>
    public class CampaignProfileAdvertConfiguration : EntityTypeConfiguration<CampaignProfileAdvert>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileAdvertConfiguration"/> class.
        /// </summary>
        public CampaignProfileAdvertConfiguration()
        {
            ToTable("CampaignProfileAdvert");
            Property(u => u.CampaignProfileId).IsRequired();
            Property(u => u.Food);
            Property(u => u.SweetSaltySnacks);
            Property(u => u.AlcoholicDrinks);
            Property(u => u.NonAlcoholicDrinks);
            Property(u => u.Householdproducts);
            Property(u => u.ToiletriesCosmetics);
            Property(u => u.PharmaceuticalChemistsProducts);
            Property(u => u.TobaccoProducts);
            Property(u => u.PetsPetFood);
            Property(u => u.ShoppingRetailClothing);
            Property(u => u.DIYGardening);
            Property(u => u.AppliancesOtherHouseholdDurables);
            Property(u => u.ElectronicsOtherPersonalItems);
            Property(u => u.CommunicationsInternet);
            Property(u => u.FinancialServices);
            Property(u => u.HolidaysTravel);
            Property(u => u.SportsLeisure);
            Property(u => u.Motoring);
            Property(u => u.Newspapers);
            Property(u => u.Magazines);
            Property(u => u.TV);
            Property(u => u.Radio);
            Property(u => u.Cinema);
            Property(u => u.SocialNetworking);
            Property(u => u.GeneralUse);
            Property(u => u.Shopping);
            Property(u => u.Fitness);
            Property(u => u.Holidays);
            Property(u => u.Environment);
            Property(u => u.GoingOut);
            Property(u => u.FinancialProducts);
            Property(u => u.Religion);
            Property(u => u.Fashion);
            Property(u => u.Music);
        }
    }
}