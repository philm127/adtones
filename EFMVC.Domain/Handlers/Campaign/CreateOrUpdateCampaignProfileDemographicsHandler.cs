// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-22-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-22-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileDemographicsHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileDemographicsHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileDemographicsHandler :
        ICommandHandler<CreateOrUpdateCampaignProfileDemographicsCommand>
    {
        /// <summary>
        /// The _repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _repository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICampaignProfileRepository _profileRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileDemographicsHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileDemographicsHandler(ICampaignProfilePreferenceRepository repository, ICampaignProfileRepository profileRepository,
                                                                IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileDemographicsCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileDemographicsCommand command)
        {
            var demographics = new CampaignProfilePreference
            {
                Id = command.Id,
                CampaignProfileId = command.CampaignProfileId,
                DOBEnd_Demographics = command.DOBEnd_Demographics,
                DOBStart_Demographics = command.DOBStart_Demographics,
                Age_Demographics = command.Age_Demographics,
                Education_Demographics = command.Education_Demographics,
                Gender_Demographics = command.Gender_Demographics,
                HouseholdStatus_Demographics = command.HouseholdStatus_Demographics,
                IncomeBracket_Demographics = command.IncomeBracket_Demographics,
                Location_Demographics = command.Location_Demographics,
                RelationshipStatus_Demographics = command.RelationshipStatus_Demographics,
                WorkingStatus_Demographics = command.WorkingStatus_Demographics,
                NextStatus = command.NextStatus
            };

            if (demographics.Id == 0)
            {
                _repository.Add(demographics);
                _unitOfWork.Commit();

                int countryId = 0;
                var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == command.CampaignProfileId);
                if (campaignProfile != null)
                {
                     countryId = (int)campaignProfile.CountryId;
                }
                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaignProfile.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, command.CampaignProfileId);
                        if(externalServerCampaignProfileId != 0)
                        {
                            var demographics2 = new CampaignProfilePreference
                            {
                                Id = command.Id,
                                CampaignProfileId = externalServerCampaignProfileId,
                                DOBEnd_Demographics = command.DOBEnd_Demographics,
                                DOBStart_Demographics = command.DOBStart_Demographics,
                                Age_Demographics = command.Age_Demographics,
                                Education_Demographics = command.Education_Demographics,
                                Gender_Demographics = command.Gender_Demographics,
                                HouseholdStatus_Demographics = command.HouseholdStatus_Demographics,
                                IncomeBracket_Demographics = command.IncomeBracket_Demographics,
                                Location_Demographics = command.Location_Demographics,
                                RelationshipStatus_Demographics = command.RelationshipStatus_Demographics,
                                WorkingStatus_Demographics = command.WorkingStatus_Demographics,
                                AdtoneServerCampaignProfilePrefId = demographics.Id
                            };
                            db.CampaignProfilePreference.Add(demographics2);
                            db.SaveChanges();
                        }
                    
                    }                        
                }
            }
            else
            {
                CampaignProfilePreference campaignProfile = _repository.GetById(command.Id);
                campaignProfile.Id = campaignProfile.Id;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.DOBEnd_Demographics = command.DOBEnd_Demographics;
                campaignProfile.DOBStart_Demographics = command.DOBStart_Demographics;
                campaignProfile.Age_Demographics = command.Age_Demographics;
                campaignProfile.Education_Demographics = command.Education_Demographics;
                campaignProfile.Gender_Demographics = command.Gender_Demographics;
                campaignProfile.HouseholdStatus_Demographics = command.HouseholdStatus_Demographics;
                campaignProfile.IncomeBracket_Demographics = command.IncomeBracket_Demographics;
                campaignProfile.RelationshipStatus_Demographics = command.RelationshipStatus_Demographics;
                campaignProfile.WorkingStatus_Demographics = command.WorkingStatus_Demographics;
                _repository.Update(campaignProfile);
                _unitOfWork.Commit();

                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaignProfile.CountryId);

                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var campProfilePreference = db.CampaignProfilePreference.Where(s => s.AdtoneServerCampaignProfilePrefId == command.Id).FirstOrDefault();
                        if (campProfilePreference != null)
                        {
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, command.CampaignProfileId);
                            if (externalServerCampaignProfileId != 0)
                            {
                                campProfilePreference.CampaignProfileId = externalServerCampaignProfileId;
                                campProfilePreference.DOBEnd_Demographics = command.DOBEnd_Demographics;
                                campProfilePreference.DOBStart_Demographics = command.DOBStart_Demographics;
                                campProfilePreference.Age_Demographics = command.Age_Demographics;
                                campProfilePreference.Education_Demographics = command.Education_Demographics;
                                campProfilePreference.Gender_Demographics = command.Gender_Demographics;
                                campProfilePreference.HouseholdStatus_Demographics = command.HouseholdStatus_Demographics;
                                campProfilePreference.IncomeBracket_Demographics = command.IncomeBracket_Demographics;
                                campProfilePreference.RelationshipStatus_Demographics = command.RelationshipStatus_Demographics;
                                campProfilePreference.WorkingStatus_Demographics = command.WorkingStatus_Demographics;
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