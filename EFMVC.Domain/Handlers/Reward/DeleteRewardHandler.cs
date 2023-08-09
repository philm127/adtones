using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using System.Linq;

namespace EFMVC.Domain.Handlers
{
    public class DeleteRewardHandler : ICommandHandler<DeleteRewardCommand>
    {
        /// <summary>
        /// The _reward repository
        /// </summary>
        private readonly IRewardRepository _rewardRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRewardHandler(IRewardRepository rewardRepository, IUnitOfWork unitOfWork)
        {
            _rewardRepository = rewardRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(DeleteRewardCommand command)
        {
            Model.Entities.Reward rewardInfo = _rewardRepository.GetById(command.Id);
            _rewardRepository.Delete(rewardInfo);
            var ConnString = ConnectionString.GetConnectionStringByOperatorId(rewardInfo.OperatorId);
            if (ConnString != null && ConnString.Count() > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var rewardData = db.Rewards.Where(s => s.AdtoneServerRewardId == command.Id).FirstOrDefault();
                    if(rewardData != null)
                    {
                        db.Rewards.Remove(rewardData);
                        db.SaveChanges();
                    }
                }
            }

             _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
