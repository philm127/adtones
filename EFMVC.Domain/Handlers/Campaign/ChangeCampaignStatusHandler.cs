using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Campaign;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers
{
   public class ChangeCampaignStatusHandler : ICommandHandler<ChangeCampaignStatusCommand>
    {

        /// <summary>
        /// The _profile campaign repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileCampaignRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public ChangeCampaignStatusHandler(ICampaignProfileRepository profileCampaignRepository,
                                                  IUnitOfWork unitOfWork)
        {
            _profileCampaignRepository = profileCampaignRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(ChangeCampaignStatusCommand command)
        {
            CampaignProfile campaign = _profileCampaignRepository.GetById(command.CampaignProfileId);
            campaign.Status = command.Status;
            _profileCampaignRepository.Update(campaign);
            _unitOfWork.Commit();

            var ConnString = ConnectionString.GetConnectionStringByCountryId(campaign.CountryId);
            if (ConnString != null && ConnString.Count() > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var campaignData = db.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == command.CampaignProfileId).FirstOrDefault();
                    if(campaignData != null)
                    {
                        campaignData.Status = command.Status;
                        db.SaveChanges();
                    }                    
                }
            }

            return new CommandResult(true);
        }
    }
}
