using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers
{
    public class CreateOrUpdateRewardHandler : ICommandHandler<CreateOrUpdateRewardCommand>
    {

        /// <summary>
        /// The _reward repository
        /// </summary>
        private readonly IRewardRepository _rewardRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateRewardHandler"/> class.
        /// </summary>
        /// <param name="rewardRepository">The reward repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateRewardHandler(IRewardRepository rewardRepository, IUnitOfWork unitOfWork)
        {
            _rewardRepository = rewardRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateRewardCommand command)
        {
            var reward = new Model.Entities.Reward
            {
                RewardId = command.Id,
                RewardName = command.Name,
                RewardValue = command.Value,
                OperatorId = command.OperatorId,
                AddedDate = command.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorId);
            if (reward.RewardId == 0)
            {
                _rewardRepository.Add(reward);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);

                        var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);

                        if (externalServerOperatorId != 0)
                        {
                            var reward2 = new Model.Entities.Reward
                            {
                                RewardId = command.Id,
                                RewardName = command.Name,
                                RewardValue = command.Value,
                                OperatorId = externalServerOperatorId,
                                AddedDate = command.CreatedDate,
                                UpdatedDate = DateTime.Now,
                                AdtoneServerRewardId = reward.RewardId
                            };
                            db.Rewards.Add(reward2);
                            db.SaveChanges();
                        }

                    }
                }

            }
            else
            {
                _rewardRepository.Update(reward);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var rewardData = db.Rewards.Where(s => s.AdtoneServerRewardId == command.Id).FirstOrDefault();
                        if (rewardData != null)
                        {
                            var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);
                            if (externalServerOperatorId != 0)
                            {
                                rewardData.RewardName = command.Name;
                                rewardData.RewardValue = command.Value;
                                rewardData.OperatorId = externalServerOperatorId;
                                rewardData.UpdatedDate = DateTime.Now;
                                db.SaveChanges();
                            }
                        }

                    }
                }
            }




            return new CommandResult(true);
        }
    }
}
