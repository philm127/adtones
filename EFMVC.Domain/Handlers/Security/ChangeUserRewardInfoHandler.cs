using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Security;
using EFMVC.Model;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;
using EFMVC.Domain.OperatorServerData;

namespace EFMVC.Domain.Handlers.Security
{
    public class ChangeUserRewardInfoHandler : ICommandHandler<ChangeUserRewardInfoCommand>
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The _userReward repository
        /// </summary>
        private readonly IUserRewardRepository _userRewardRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserRewardInfoHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public ChangeUserRewardInfoHandler(IUserRewardRepository userRewardRepository, IUnitOfWork unitOfWork)
        {
            _userRewardRepository = userRewardRepository;
            _unitOfWork = unitOfWork;
        }
        #region ICommandHandler<ChangeEmailCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>

        public ICommandResult Execute(ChangeUserRewardInfoCommand command)
        {
            Model.Entities.UserReward userReward = null;
            if (command.UserRewardId != 0)
            {
                userReward = _userRewardRepository.GetById(command.UserRewardId);
                if (userReward != null)
                {
                    userReward.UserId = command.UserId;
                    userReward.RewardId = command.RewardId;
                }
            }
            else
            {
                userReward = new Model.Entities.UserReward
                {
                    UserRewardId = command.UserRewardId,
                    UserId = command.UserId,
                    RewardId = command.RewardId
                };
            }
            
            //Add 22-02-2019
            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorId);

            if (userReward.UserRewardId == 0)
            {
                _userRewardRepository.Add(userReward);
                _unitOfWork.Commit();

                if (ConnString != null && ConnString.Count > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                        var externalServerRewardId = OperatorServer.GetRewardIdFromOperatorServer(db, command.RewardId);
                        if(externalServerUserId != 0 && externalServerRewardId != 0)
                        {
                            var userReward1 = new Model.Entities.UserReward
                            {
                                UserId = externalServerUserId,
                                RewardId = externalServerRewardId,
                                AdtoneServerUserRewardId = userReward.UserRewardId
                            };

                            db.UserRewards.Add(userReward1);
                            db.SaveChanges();
                        }
                   
                    }

                }
            }
            else
            {
                _userRewardRepository.Update(userReward);
                _unitOfWork.Commit();

                if (ConnString != null && ConnString.Count > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var userReward2 = db.UserRewards.Where(s => s.AdtoneServerUserRewardId == command.UserRewardId).FirstOrDefault();
                        if (userReward2 != null)
                        {
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                            var externalServerRewardId = OperatorServer.GetRewardIdFromOperatorServer(db, command.RewardId);
                            if (externalServerUserId != 0 && externalServerRewardId != 0)
                            {
                                userReward2.UserId = externalServerUserId;
                                userReward2.RewardId = externalServerRewardId;
                                db.SaveChanges();
                            }                              
                            
                        }
                    }
                }
            }

            //_unitOfWork.Commit();
            return new CommandResult(true);
        }

        #endregion
    }
}
