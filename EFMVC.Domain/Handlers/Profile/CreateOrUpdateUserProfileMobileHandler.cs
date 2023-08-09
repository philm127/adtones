// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileMobileHandler.cs" company="Noat">
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
/// The Mobile namespace.
/// </summary>

namespace EFMVC.Domain.Mobile
{
    /// <summary>
    /// Class CreateOrUpdateUserProfileMobileHandler.
    /// </summary>
    public class CreateOrUpdateUserProfileMobileHandler : ICommandHandler<CreateOrUpdateUserProfileMobileCommand>
    {
        /// <summary>
        /// The _mobile repository
        /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileMobileHandler"/> class.
        /// </summary>
        /// <param name="mobileRepository">The mobile repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfileMobileHandler(IUserProfilePreferenceRepository userProfilePreferenceRepository, IUnitOfWork unitOfWork)
        {
            _userProfilePreferenceRepository = userProfilePreferenceRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileMobileCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileMobileCommand command)
        {
            var Mobile = new UserProfilePreference
            {
                ContractType_Mobile = command.ContractType_Mobile,
                Spend_Mobile = command.Spend_Mobile,
                                 UserProfileId = command.UserProfileId,
                                 Id = command.Id
                             };

            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorId);
            if (Mobile.Id == 0)
            {
                _userProfilePreferenceRepository.Add(Mobile);
                _unitOfWork.Commit();

                if (ConnString != null && ConnString.Count > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(db, command.UserProfileId);
                        if(externalServerUserProfileId != 0)
                        {
                            var Mobile2 = new UserProfilePreference
                            {
                                ContractType_Mobile = command.ContractType_Mobile,
                                Spend_Mobile = command.Spend_Mobile,
                                UserProfileId = externalServerUserProfileId,
                                Id = command.Id,
                                AdtoneServerUserProfilePrefId = Mobile.Id
                            };

                            db.UserProfilePreference.Add(Mobile2);
                            db.SaveChanges();
                        }
                       
                    }
                       
                }
            }
            else
            {
                UserProfilePreference userprofile = _userProfilePreferenceRepository.GetById(command.Id);
                userprofile.ContractType_Mobile = command.ContractType_Mobile;
                userprofile.Spend_Mobile = command.Spend_Mobile;
                userprofile.UserProfileId = command.UserProfileId;
                userprofile.Id = userprofile.Id;
                _userProfilePreferenceRepository.Update(userprofile);
                _unitOfWork.Commit();

                if (ConnString != null && ConnString.Count > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var userProfiePref = db.UserProfilePreference.Where(s => s.AdtoneServerUserProfilePrefId == command.Id).FirstOrDefault();
                        if (userProfiePref != null)
                        {
                            var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(db, command.UserProfileId);
                            if (externalServerUserProfileId != 0)
                            {
                                userProfiePref.ContractType_Mobile = command.ContractType_Mobile;
                                userProfiePref.Spend_Mobile = command.Spend_Mobile;
                                userProfiePref.UserProfileId = externalServerUserProfileId;
                                userProfiePref.Id = userprofile.Id;
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