// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileMobileHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileMobileHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileMobileHandler :
        ICommandHandler<CreateOrUpdateCampaignProfileMobileCommand>
    {
        /// <summary>
        /// The _mobile repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _mobileRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICampaignProfileRepository _profileRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileMobileHandler"/> class.
        /// </summary>
        /// <param name="mobileRepository">The mobile repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileMobileHandler(ICampaignProfilePreferenceRepository mobileRepository, ICampaignProfileRepository profileRepository,
                                                          IUnitOfWork unitOfWork)
        {
            _mobileRepository = mobileRepository;
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileMobileCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileMobileCommand command)
        {
            var Mobile = new CampaignProfilePreference
                             {
                                 ContractType_Mobile = command.ContractType_Mobile,
                                 Spend_Mobile = command.Spend_Mobile,
                                 CampaignProfileId = command.CampaignProfileId,
                                 Id = command.CampaignProfileMobileId,
                                 NextStatus = command.NextStatus
                             };

            if (Mobile.Id == 0)
            {
                _mobileRepository.Add(Mobile);
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
                        var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, (int)command.CampaignProfileId);
                        if(externalServerCampaignProfileId != 0)
                        {
                            var Mobile2 = new CampaignProfilePreference
                            {
                                ContractType_Mobile = command.ContractType_Mobile,
                                Spend_Mobile = command.Spend_Mobile,
                                CampaignProfileId = externalServerCampaignProfileId,
                                Id = command.CampaignProfileMobileId,
                                NextStatus = command.NextStatus,
                                AdtoneServerCampaignProfilePrefId = Mobile.Id
                            };
                            db.CampaignProfilePreference.Add(Mobile2);
                            db.SaveChanges();
                        }
                       
                    }
                        
                }
            }
            else
            {
                CampaignProfilePreference campaignProfile = _mobileRepository.GetById(command.CampaignProfileMobileId);
                campaignProfile.ContractType_Mobile = command.ContractType_Mobile;
                campaignProfile.Spend_Mobile = command.Spend_Mobile;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.Id = command.CampaignProfileMobileId;
                _mobileRepository.Update(campaignProfile);
                _unitOfWork.Commit();

                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaignProfile.CountryId);

                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var campProfilePreference = db.CampaignProfilePreference.Where(s => s.AdtoneServerCampaignProfilePrefId == command.CampaignProfileMobileId).FirstOrDefault();
                        if (campProfilePreference != null)
                        {
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, command.CampaignProfileId);
                            if(externalServerCampaignProfileId != 0)
                            {
                                campProfilePreference.ContractType_Mobile = command.ContractType_Mobile;
                                campProfilePreference.Spend_Mobile = command.Spend_Mobile;
                                campProfilePreference.CampaignProfileId = externalServerCampaignProfileId;
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