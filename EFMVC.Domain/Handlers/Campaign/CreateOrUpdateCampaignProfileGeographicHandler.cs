// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileAttitudeHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileGeographicHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileGeographicHandler :
        ICommandHandler<CreateOrUpdateCampaignProfileGeographicCommand>
    {
        /// <summary>
        /// The _attitude repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _attitudeRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileGeographicHandler"/> class.
        /// </summary>
        /// <param name="attitudeRepository">The attitude repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileGeographicHandler(ICampaignProfilePreferenceRepository attitudeRepository,
                                                            IUnitOfWork unitOfWork)
        {
            _attitudeRepository = attitudeRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileGeographicCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileGeographicCommand command)
        {
            var attitude = new CampaignProfilePreference
            {
                Postcode = command.PostCode,
                CountryId = command.CountryId,    
                Location_Demographics = command.Location_Demographics,          
                Id = command.CampaignProfileGeographicId
            };
            var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);
            if (attitude.Id == 0)
            {
                attitude.CampaignProfileId = command.CampaignProfileId;
                _attitudeRepository.Add(attitude);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                        var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, (int)command.CampaignProfileId);
                        if (externalServerCountryId != 0 && externalServerCampaignProfileId != 0)
                        {
                            var attitude2 = new CampaignProfilePreference
                            {
                                Postcode = command.PostCode,
                                CountryId = externalServerCountryId,
                                Location_Demographics = command.Location_Demographics,
                                CampaignProfileId = externalServerCampaignProfileId,
                                Id = command.CampaignProfileGeographicId,
                                AdtoneServerCampaignProfilePrefId = attitude.Id
                            };
                            db.CampaignProfilePreference.Add(attitude2);
                            db.SaveChanges();
                        }
                       
                    }                        
                }
            }
            else
            {
                CampaignProfilePreference campaignProfile = _attitudeRepository.GetById(command.CampaignProfileGeographicId);
                campaignProfile.Postcode = command.PostCode;
                campaignProfile.CountryId = command.CountryId;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.Location_Demographics = command.Location_Demographics;
                campaignProfile.Id = campaignProfile.Id;
                _attitudeRepository.Update(campaignProfile);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var campProfilePreference = db.CampaignProfilePreference.Where(s => s.AdtoneServerCampaignProfilePrefId == command.CampaignProfileId).FirstOrDefault();
                        if (campProfilePreference != null)
                        {
                            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, (int)command.CampaignProfileId);
                            if (externalServerCountryId != 0 && externalServerCampaignProfileId != 0)
                            {
                                campProfilePreference.Postcode = command.PostCode;
                                campProfilePreference.CountryId = externalServerCountryId;
                                campProfilePreference.CampaignProfileId = externalServerCampaignProfileId;
                                campProfilePreference.Location_Demographics = command.Location_Demographics;
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