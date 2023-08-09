using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers.Campaign
{
    public class DeleteCampaignAdvertHandler : ICommandHandler<DeleteCamapignAdvertCommand>
    {
        /// <summary>
        /// The _campaign repository
        /// </summary>
        private readonly ICampaignAdvertRepository _campaignAdvertRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCampaignAdvertHandler"/> class.
        /// </summary>
        /// <param name="campaignAdvertRepository">The campaign advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteCampaignAdvertHandler(ICampaignAdvertRepository campaignAdvertRepository, IUnitOfWork unitOfWork)
        {
            _campaignAdvertRepository = campaignAdvertRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<DeleteCamapignAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteCamapignAdvertCommand command)
        {
            Model.CampaignAdvert campaignAdvert = _campaignAdvertRepository.GetById(command.CampaignAdvertId);
            _campaignAdvertRepository.Delete(campaignAdvert);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}
