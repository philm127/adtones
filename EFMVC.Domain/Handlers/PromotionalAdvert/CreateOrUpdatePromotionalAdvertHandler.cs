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
    public class CreateOrUpdatePromotionalAdvertHandler : ICommandHandler<CreateOrUpdatePromotionalAdvertCommand>
    {

        /// <summary>
        /// The _promotionalAdvert Repository
        /// </summary>
        private readonly IPromotionalAdvertRepository _promotionalAdvertRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdatePromotionalAdvertHandler"/> class.
        /// </summary>
        /// <param name="promotionalAdvertRepository">The promotionalCampaign Repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdatePromotionalAdvertHandler(IPromotionalAdvertRepository promotionalAdvertRepository, IUnitOfWork unitOfWork)
        {
            _promotionalAdvertRepository = promotionalAdvertRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdatePromotionalAdvertCommand command)
        {
            var promotionalAdvert = new PromotionalAdvert
            {
                ID = command.ID,
                CampaignID = command.CampaignID,
                AdvertName = command.AdvertName,
                AdvertLocation = command.AdvertLocation
            };

            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorID);
            if (promotionalAdvert.ID == 0)
            {
                _promotionalAdvertRepository.Add(promotionalAdvert);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorID.Value);
                        var externalServerCampaignId = OperatorServer.GetPromotionalCampaignIdFromOperatorServer(db, command.CampaignID.Value);
                        if (externalServerOperatorId != 0 && externalServerCampaignId != 0)
                        {
                            var promotionalAdvert2 = new PromotionalAdvert
                            {
                                ID = command.ID,
                                CampaignID = externalServerCampaignId,
                                AdvertName = command.AdvertName,
                                AdvertLocation = command.AdvertLocation,
                                AdtoneServerPromotionalAdvertId = promotionalAdvert.ID
                            };
                            db.PromotionalAdverts.Add(promotionalAdvert2);
                            db.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                _promotionalAdvertRepository.Update(promotionalAdvert);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var promotionalAdvertData = db.PromotionalAdverts.Where(s => s.AdtoneServerPromotionalAdvertId == command.ID).FirstOrDefault();
                        if (promotionalAdvertData != null)
                        {
                            var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorID.Value);
                            var externalServerCampaignId = OperatorServer.GetPromotionalCampaignIdFromOperatorServer(db, command.CampaignID.Value);
                            if (externalServerOperatorId != 0 && externalServerCampaignId != 0)
                            {
                                promotionalAdvertData.AdvertLocation = command.AdvertLocation;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }

            return new CommandResult(true, promotionalAdvert.ID);
        }
    }
}
