using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;

using EFMVC.Model;
using EFMVC.Model.Entities;

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.CommandProcessor.Command.ICommandHandler{EFMVC.Domain.Commands.CampaignCreditPeriod.DeleteCampaignCreditPeriodCommand}" />
    public class DeleteCampaignCreditPeriodHandler : ICommandHandler<DeleteCampaignCreditPeriodCommand>
    {  
        /// <summary>
       /// The _usercredit repository
       /// </summary>
        private readonly ICampaignCreditPeriodRepository _campaignCreditRepository;


        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignCreditPeriodHandler"/> class.
        /// </summary>
        /// <param name="usercreditRepository">The usercredit repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteCampaignCreditPeriodHandler(ICampaignCreditPeriodRepository campaignCreditRepository, IUnitOfWork unitOfWork)
        {
            _campaignCreditRepository = campaignCreditRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(DeleteCampaignCreditPeriodCommand command)
        {
            CampaignCreditPeriod campaignCredit = _campaignCreditRepository.GetById(command.Id);
            _campaignCreditRepository.Delete(campaignCredit);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

    }
}
