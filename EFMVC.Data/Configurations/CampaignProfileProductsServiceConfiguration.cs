// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileProductsServiceConfiguration.cs" company="Noat">
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
    /// Class CampaignProfileProductsServiceConfiguration.
    /// </summary>
    public class CampaignProfileProductsServiceConfiguration : EntityTypeConfiguration<CampaignProfileProductsService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileProductsServiceConfiguration"/> class.
        /// </summary>
        public CampaignProfileProductsServiceConfiguration()
        {
            ToTable("CampaignProfileProductsService");
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
        }
    }
}