using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers.Profile
{
  public  class CreateOrUpdateProfileDemographicsHandler : ICommandHandler<CreateOrUpdateUserProfileDemographicsCommand>
    { /// <summary>
      /// The _advert repository
      /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferencRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileAdvertHandler"/> class.
        /// </summary>
        /// <param name="advertRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateProfileDemographicsHandler(IUserProfilePreferenceRepository userProfilePreferencRepository,
                                                      IUnitOfWork unitOfWork)
        {
            _userProfilePreferencRepository = userProfilePreferencRepository;
            _unitOfWork = unitOfWork;
        }
        #region ICommandHandler<CreateOrUpdateUserProfileAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileDemographicsCommand command)
        {
            var userProfilePreference = new UserProfilePreference
            {
                Gender_Demographics = command.Gender_Demographics,
                IncomeBracket_Demographics = command.IncomeBracket_Demographics,
                WorkingStatus_Demographics = command.WorkingStatus_Demographics,
                RelationshipStatus_Demographics = command.RelationshipStatus_Demographics,
                Education_Demographics = command.Education_Demographics,
                HouseholdStatus_Demographics = command.HouseholdStatus_Demographics,
                Location_Demographics = command.Location_Demographics,
                Postcode = command.Postcode,
                UserProfileId = command.UserProfileId,
                Id = command.Id
            };

            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorId);
            if (userProfilePreference.Id == 0)
            {
                _userProfilePreferencRepository.Add(userProfilePreference);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(db, command.UserProfileId);
                        if(externalServerUserProfileId != 0)
                        {
                            var userProfilePreference2 = new UserProfilePreference
                            {
                                Gender_Demographics = command.Gender_Demographics,
                                IncomeBracket_Demographics = command.IncomeBracket_Demographics,
                                WorkingStatus_Demographics = command.WorkingStatus_Demographics,
                                RelationshipStatus_Demographics = command.RelationshipStatus_Demographics,
                                Education_Demographics = command.Education_Demographics,
                                HouseholdStatus_Demographics = command.HouseholdStatus_Demographics,
                                Location_Demographics = command.Location_Demographics,
                                Postcode = command.Postcode,
                                UserProfileId = externalServerUserProfileId,
                                Id = command.Id,
                                AdtoneServerUserProfilePrefId = userProfilePreference.Id
                            };
                            db.UserProfilePreference.Add(userProfilePreference2);
                            db.SaveChanges();
                        }                       

                    }                     
                }
            }
            else
            {               
                UserProfilePreference userprofile = _userProfilePreferencRepository.GetById(command.Id);
                userprofile.Gender_Demographics = command.Gender_Demographics;
                userprofile.IncomeBracket_Demographics = command.IncomeBracket_Demographics;
                userprofile.WorkingStatus_Demographics = command.WorkingStatus_Demographics;
                userprofile.RelationshipStatus_Demographics = command.RelationshipStatus_Demographics;
                userprofile.Education_Demographics = command.Education_Demographics;
                userprofile.HouseholdStatus_Demographics = command.HouseholdStatus_Demographics;
                userprofile.Location_Demographics = command.Location_Demographics;
                userprofile.Postcode = command.Postcode;
                userprofile.UserProfileId = command.UserProfileId;
                userprofile.Id = userprofile.Id;
                _userProfilePreferencRepository.Update(userprofile);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
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
                                userProfiePref.Gender_Demographics = command.Gender_Demographics;
                                userProfiePref.IncomeBracket_Demographics = command.IncomeBracket_Demographics;
                                userProfiePref.WorkingStatus_Demographics = command.WorkingStatus_Demographics;
                                userProfiePref.RelationshipStatus_Demographics = command.RelationshipStatus_Demographics;
                                userProfiePref.Education_Demographics = command.Education_Demographics;
                                userProfiePref.HouseholdStatus_Demographics = command.HouseholdStatus_Demographics;
                                userProfiePref.Location_Demographics = command.Location_Demographics;
                                userProfiePref.Postcode = command.Postcode;
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
