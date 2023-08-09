// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="AdvertConfiguration.cs" company="">
//     Copyright (c) . All rights reserved.
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
    /// Class AdvertConfiguration.
    /// </summary>
    public class AdvertConfiguration : EntityTypeConfiguration<Advert>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertConfiguration"/> class.
        /// </summary>
        public AdvertConfiguration()
        {
            ToTable("Advert");
            Property(a => a.AdvertId).IsRequired();
            Property(a => a.UserId).IsRequired();
            Property(a => a.AdvertDescription).HasMaxLength(2000);
            Property(a => a.AdvertName).HasMaxLength(250);
            Property(a => a.Brand).HasMaxLength(250);
            Property(a => a.Script);
            Property(a => a.ScriptFileLocation);
            Property(a => a.MediaFileLocation).HasMaxLength(500);
            Property(a => a.UploadedToMediaServer);
            Property(a => a.CreatedDateTime);
            Property(a => a.UpdatedDateTime);
            HasRequired(p => p.Clients)
             .WithMany(c => c.Advert)
             .HasForeignKey(p => p.ClientId);
            Property(a => a.Status);
              
    }
    }
}