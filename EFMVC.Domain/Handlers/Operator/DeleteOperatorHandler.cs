using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using System.Linq;

namespace EFMVC.Domain.Handlers
{
    public class DeleteOperatorHandler : ICommandHandler<DeleteOperatorCommand>
    {
        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly IOperatorRepository _operatorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public DeleteOperatorHandler(IOperatorRepository operatorRepository, IUserRepository userRepository, IUserProfileRepository userProfileRepository, IUserProfilePreferenceRepository userProfilePreferenceRepository, IUnitOfWork unitOfWork)
        {
            _operatorRepository = operatorRepository;
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _userProfilePreferenceRepository = userProfilePreferenceRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(DeleteOperatorCommand command)
        {
            var GetUserData = _userRepository.GetMany(s => s.OperatorId == command.OperatorId).ToList();
            if(GetUserData.Count() > 0)
            {
                foreach(var userrecord in GetUserData)
                {
                    var UserProfile = _userProfileRepository.GetMany(s => s.UserId == userrecord.UserId).FirstOrDefault();
                    if(UserProfile != null)
                    {
                        var UserProfilePreference = _userProfilePreferenceRepository.GetMany(s => s.UserProfileId == UserProfile.UserProfileId).FirstOrDefault();
                        if(UserProfilePreference != null)
                        {
                            _userProfilePreferenceRepository.Delete(UserProfilePreference);
                        }
                        _userProfileRepository.Delete(UserProfile);
                    }
                    var UserInfo = _userRepository.GetById(userrecord.UserId);
                    _userRepository.Delete(UserInfo);
                }
            }

            var operatorInfo = _operatorRepository.GetById(command.OperatorId);
            _operatorRepository.Delete(operatorInfo);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
