using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.ProfileMatchInfo;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers.ProfileMatchInfo
{
   public  class CreateOrUpdateProfileMatchInfoHandler : ICommandHandler<CreateOrUpdateProfileMatchInfoCommand>
    {
        private readonly IProfileMatchInformationRepository _profileMatchInformationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrUpdateProfileMatchInfoHandler(IProfileMatchInformationRepository profileMatchInformationRepository,
                                                      IUnitOfWork unitOfWork)
        {
            _profileMatchInformationRepository = profileMatchInformationRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateProfileMatchInfoCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateProfileMatchInfoCommand command)
        {
            var ProfileMatchInfo = new ProfileMatchInformation
            {
                ProfileName = command.ProfileName,
                CountryId = command.CountryId,
                IsActive = command.IsActive,   
                CreatedDate = DateTime.Now,            
                UpdatedDate = DateTime.Now,
                Id = command.Id,
                ProfileType = command.ProfileType
            };

            var ConnString = ConnectionString.GetConnectionString(command.CountryId);
            if (ProfileMatchInfo.Id == 0)
            {
                _profileMatchInformationRepository.Add(ProfileMatchInfo);
                if (!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    var ProfileMatchInfo2 = new ProfileMatchInformation
                    {
                        ProfileName = command.ProfileName,
                        CountryId = command.CountryId,
                        IsActive = command.IsActive,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        Id = command.Id,
                        ProfileType = command.ProfileType
                    };
                    db.ProfileMatchInformation.Add(ProfileMatchInfo2);
                    db.SaveChanges();
                }
            }
            else
            {
                ProfileMatchInformation profilematch = _profileMatchInformationRepository.GetById(command.Id);
                profilematch.ProfileName = command.ProfileName;
                profilematch.CountryId = command.CountryId;
                profilematch.IsActive = command.IsActive;
                profilematch.Id = profilematch.Id;
                profilematch.UpdatedDate = DateTime.Now;
                profilematch.ProfileType = command.ProfileType;
                _profileMatchInformationRepository.Update(profilematch);

                if(!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    var profileMatch2 = db.ProfileMatchInformation.Where(s => s.Id == command.Id).FirstOrDefault();
                    if(profileMatch2 != null)
                    {
                        profileMatch2.ProfileName = command.ProfileName;
                        profileMatch2.CountryId = command.CountryId;
                        profileMatch2.IsActive = command.IsActive;
                        profileMatch2.Id = profilematch.Id;
                        profileMatch2.UpdatedDate = DateTime.Now;
                        profileMatch2.ProfileType = command.ProfileType;
                        db.SaveChanges();
                    }
                }

            }
            _unitOfWork.Commit();

            return new CommandResult(true, ProfileMatchInfo.Id);
        }

        #endregion
    }
}
