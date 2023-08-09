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

namespace EFMVC.Domain.Handlers
{
    public class ChangePromotionalCampaignStatusHandler : ICommandHandler<ChangePromotionalCampaignStatusCommand>
    {
        /// <summary>
        /// The _promotionalCampaign Repository
        /// </summary>
        private readonly IPromotionalCampaignRepository _promotionalCampaignRepository;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        public ChangePromotionalCampaignStatusHandler(IPromotionalCampaignRepository promotionalCampaignRepository, IUnitOfWork unitOfWork)
        {
            _promotionalCampaignRepository = promotionalCampaignRepository;
            this.unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(ChangePromotionalCampaignStatusCommand command)
        {
            var promotionalCampaignDetail = _promotionalCampaignRepository.GetById(command.ID);
            promotionalCampaignDetail.Status = command.Status;
            _promotionalCampaignRepository.Update(promotionalCampaignDetail);
            var ConnString = ConnectionString.GetConnectionStringByOperatorId(promotionalCampaignDetail.OperatorID);
            if (ConnString != null && ConnString.Count() > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var advertData = db.PromotionalCampaigns.Where(s => s.AdtoneServerPromotionalCampaignId == command.ID).FirstOrDefault();
                    if (advertData != null)
                    {
                        advertData.Status = command.Status;
                        db.SaveChanges();
                    }
                }
            }
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
