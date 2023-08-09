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
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;
using EFMVC.Domain.OperatorServerData;

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateCampaignCreditPeriodHandler.
    /// </summary>
    /// <seealso cref="ICommandHandler{EFMVC.Domain.Commands.CampaignCreditPeriod.CreateOrUpdateCampaignCreditPeriodCommand}" />
    public class CreateOrUpdateCampaignCreditPeriodHandler : ICommandHandler<CreateOrUpdateCampaignCreditPeriodCommand>
    {
        /// <summary>
        /// The _usercredit repository
        /// </summary>
        private readonly ICampaignCreditPeriodRepository _campaignCreditRepository;

        private readonly ICampaignProfileRepository _profileRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignCreditPeriodHandler"/> class.
        /// </summary>
        /// <param name="usercreditRepository">The usercredit repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignCreditPeriodHandler(ICampaignCreditPeriodRepository campaignCreditRepository, ICampaignProfileRepository profileRepository, IUnitOfWork unitOfWork)
        {
            _campaignCreditRepository = campaignCreditRepository;
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        /// ICommandResult.
        /// </returns>
        public ICommandResult Execute(CreateOrUpdateCampaignCreditPeriodCommand command)
        {
            int countryId = 0;
            var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == command.CampaignProfileId);
            if (campaignProfile != null)
            {
                countryId = (int)campaignProfile.CountryId;
            }
            var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);

            if (command.CampaignCreditPeriodId == 0)
            {

                var campaignCredit = new CampaignCreditPeriod
                {
                    UserId = command.UserId,
                    CampaignProfileId = command.CampaignProfileId,
                    CreditPeriod = command.CreditPeriod,
                    CreatedDate=command.CreatedDate,
                    UpdatedDate=command.UpdatedDate
                };

                _campaignCreditRepository.Add(campaignCredit);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, command.CampaignProfileId);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                        if(externalServerCampaignProfileId != 0 && externalServerUserId != 0)
                        {
                            var operatorCampaignCredit = new CampaignCreditPeriod
                            {
                                UserId = externalServerUserId,
                                CampaignProfileId = externalServerCampaignProfileId,
                                CreditPeriod = command.CreditPeriod,
                                CreatedDate = command.CreatedDate,
                                UpdatedDate = command.UpdatedDate
                            };
                            db.CampaignCreditPeriod.Add(operatorCampaignCredit);
                            db.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                CampaignCreditPeriod campaignCredit = _campaignCreditRepository.GetById(command.CampaignCreditPeriodId);
                campaignCredit.UserId = command.UserId;
                campaignCredit.CampaignProfileId = command.CampaignProfileId;
                campaignCredit.CreditPeriod = command.CreditPeriod;
                campaignCredit.UpdatedDate = command.UpdatedDate;

                _campaignCreditRepository.Update(campaignCredit);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var operatorCampaignCredit = db.CampaignCreditPeriod.Where(s=>s.CampaignCreditPeriodId == command.CampaignCreditPeriodId).FirstOrDefault();
                        if(operatorCampaignCredit != null)
                        {
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, command.CampaignProfileId);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                            if (externalServerCampaignProfileId != 0 && externalServerUserId != 0)
                            {
                                operatorCampaignCredit.UserId = externalServerUserId;
                                operatorCampaignCredit.CampaignProfileId = externalServerCampaignProfileId;
                                operatorCampaignCredit.CreditPeriod = command.CreditPeriod;
                                operatorCampaignCredit.UpdatedDate = command.UpdatedDate;
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
