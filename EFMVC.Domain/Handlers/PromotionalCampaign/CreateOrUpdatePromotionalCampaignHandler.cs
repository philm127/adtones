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
    public class CreateOrUpdatePromotionalCampaignHandler : ICommandHandler<CreateOrUpdatePromotionalCampaignCommand>
    {

        /// <summary>
        /// The _promotionalCampaign Repository
        /// </summary>
        private readonly IPromotionalCampaignRepository _promotionalCampaignRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdatePromotionalCampaignHandler"/> class.
        /// </summary>
        /// <param name="promotionalCampaignRepository">The promotionalCampaign Repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdatePromotionalCampaignHandler(IPromotionalCampaignRepository promotionalCampaignRepository, IUnitOfWork unitOfWork)
        {
            _promotionalCampaignRepository = promotionalCampaignRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdatePromotionalCampaignCommand command)
        {
            PromotionalCampaign promotionalCampaign = null;
            if (command.ID != 0)
            {
                promotionalCampaign = _promotionalCampaignRepository.GetById(command.ID);
                if (promotionalCampaign != null)
                {
                    promotionalCampaign.ID = command.ID;
                    promotionalCampaign.OperatorID = command.OperatorID;
                    promotionalCampaign.CampaignName = command.CampaignName;
                    promotionalCampaign.BatchID = command.BatchID;
                    promotionalCampaign.MaxDaily = command.MaxDaily;
                    promotionalCampaign.MaxWeekly = command.MaxWeekly;
                    promotionalCampaign.AdvertLocation = command.AdvertLocation;
                    promotionalCampaign.Status = command.Status;
                }
            }
            else
            {
                promotionalCampaign = new PromotionalCampaign
                {
                    ID = command.ID,
                    OperatorID = command.OperatorID,
                    CampaignName = command.CampaignName,
                    BatchID = command.BatchID,
                    MaxDaily = command.MaxDaily,
                    MaxWeekly = command.MaxWeekly,
                    AdvertLocation = command.AdvertLocation,
                    Status = command.Status
                };
            }

            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorID);
            if (promotionalCampaign.ID == 0)
            {
                _promotionalCampaignRepository.Add(promotionalCampaign);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorID.Value);
                        if (externalServerOperatorId != 0)
                        {
                            var promotionalCampaign2 = new PromotionalCampaign
                            {
                                ID = command.ID,
                                OperatorID = externalServerOperatorId,
                                CampaignName = command.CampaignName,
                                BatchID = command.BatchID,
                                MaxDaily = command.MaxDaily,
                                MaxWeekly = command.MaxWeekly,
                                AdvertLocation = command.AdvertLocation,
                                Status = command.Status,
                                AdtoneServerPromotionalCampaignId = promotionalCampaign.ID
                            };
                            db.PromotionalCampaigns.Add(promotionalCampaign2);
                            db.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                _promotionalCampaignRepository.Update(promotionalCampaign);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var promotionalCampaignData = db.PromotionalCampaigns.Where(s => s.AdtoneServerPromotionalCampaignId == command.ID).FirstOrDefault();
                        if (promotionalCampaignData != null)
                        {
                            var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorID.Value);
                            if (externalServerOperatorId != 0)
                            {
                                promotionalCampaignData.AdvertLocation = command.AdvertLocation;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }

            return new CommandResult(true, promotionalCampaign.ID);
        }
    }
}
