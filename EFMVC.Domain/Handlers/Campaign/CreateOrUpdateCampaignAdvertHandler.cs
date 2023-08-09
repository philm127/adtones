// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignAdvertHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using System.Linq;

/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateCampaignAdvertHandler.
    /// </summary>
    public class CreateOrUpdateCampaignAdvertHandler : ICommandHandler<CreateOrUpdateCampaignAdvertCommand>
    {
        /// <summary>
        /// The _campaign advert repository
        /// </summary>
        private readonly ICampaignAdvertRepository _campaignAdvertRepository;
        private readonly ICampaignProfileRepository _profileRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignAdvertHandler"/> class.
        /// </summary>
        /// <param name="campaignAdvertRepository">The campaign advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignAdvertHandler(ICampaignAdvertRepository campaignAdvertRepository, ICampaignProfileRepository profileRepository,
                                                   IUnitOfWork unitOfWork)
        {
            _campaignAdvertRepository = campaignAdvertRepository;
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignAdvertCommand command)
        {
            var advert = new CampaignAdvert
                             {
                                 CampaignAdvertId = command.CampaignAdvertId,
                                 AdvertId = command.AdvertId,
                                 CampaignProfileId = command.CampaignProfileId,
                                 NextStatus = command.NextStatus
                             };
            int countryId = 0;
            var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == command.CampaignProfileId);
            if (campaignProfile != null)
            {
                countryId = (int)campaignProfile.CountryId;
            }
            var ConnString = ConnectionString.GetConnectionStringByCountryId(campaignProfile.CountryId);

            if (advert.CampaignAdvertId == 0)
            {
                _campaignAdvertRepository.Add(advert);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerAdvertId = OperatorServer.GetAdvertIdFromOperatorServer(db, command.AdvertId);
                        var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, command.CampaignProfileId);
                        if(externalServerAdvertId != 0 && externalServerCampaignProfileId != 0)
                        {
                            var campaignAdvert = new CampaignAdvert
                            {
                                CampaignAdvertId = command.CampaignAdvertId,
                                AdvertId = externalServerAdvertId,
                                CampaignProfileId = externalServerCampaignProfileId,
                                NextStatus = command.NextStatus,
                                AdtoneServerCampaignAdvertId = advert.CampaignAdvertId
                            };
                            db.CampaignAdverts.Add(campaignAdvert);
                            db.SaveChanges();
                        }
                        
                    }
                }
            }
            else
            {
                _campaignAdvertRepository.Update(advert);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);

                        var campAdvert = db.CampaignAdverts.Where(s => s.AdtoneServerCampaignAdvertId == command.CampaignAdvertId).FirstOrDefault();
                        if(campAdvert != null)
                        {
                            var externalServerAdvertId = OperatorServer.GetAdvertIdFromOperatorServer(db, command.AdvertId);
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, command.CampaignProfileId);

                            if (externalServerAdvertId != 0 && externalServerCampaignProfileId != 0)
                            {
                                campAdvert.AdvertId = externalServerAdvertId;
                                campAdvert.CampaignProfileId = externalServerCampaignProfileId;
                                campAdvert.NextStatus = command.NextStatus;
                                db.SaveChanges();
                            }
                           
                        }                       
                    }
                }
            }

            

            return new CommandResult(true);
        }

        #endregion
    }
}