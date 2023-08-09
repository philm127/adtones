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
    public class DeleteCampaignProfilePreferenceHandler : ICommandHandler<DeleteCampaignProfilePreferenceCommand>
    {
        /// <summary>
        /// The _campaign profile preference repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _campaignProfilePreferenceRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCampaignProfilePreferenceHandler"/> class.
        /// </summary>
        /// <param name="campaignProfilePreferenceRepository">The campaign profile preference repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteCampaignProfilePreferenceHandler(ICampaignProfilePreferenceRepository campaignProfilePreferenceRepository, IUnitOfWork unitOfWork)
        {
            _campaignProfilePreferenceRepository = campaignProfilePreferenceRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<DeleteCampaignProfilePreferenceHandler> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteCampaignProfilePreferenceCommand command)
        {
            Model.CampaignProfilePreference campaignProfilePreference = _campaignProfilePreferenceRepository.GetById(command.CampaignProfilePreferenceId);
            _campaignProfilePreferenceRepository.Delete(campaignProfilePreference);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}
