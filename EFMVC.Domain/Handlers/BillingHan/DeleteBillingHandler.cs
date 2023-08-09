using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Billing;
using EFMVC.Model;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;

namespace EFMVC.Domain.Handlers.BillingHan
{
    public class DeleteBillingHandler : ICommandHandler<DeleteBillingCommand>
    {
        /// <summary>
        /// The billing repository
        /// </summary>
        private readonly IBillingRepository _billingRepository;
        private readonly ICampaignProfileRepository _profileRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateClientHandler" /> class.
        /// </summary>
        /// <param name="clientRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteBillingHandler(IBillingRepository billingRepository, ICampaignProfileRepository profileRepository, IUnitOfWork unitOfWork)
        {
            _billingRepository = billingRepository;
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }
        #region ICommandHandler<DeleteAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteBillingCommand command)
        {
            Billing billing = _billingRepository.GetById(command.Id);
            _billingRepository.Delete(billing);
            _unitOfWork.Commit();

            int countryId = 0;
            var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == command.CampaignId);
            if (campaignProfile != null)
            {
                countryId = (int)campaignProfile.CountryId;
            }

            var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
            if (ConnString != null && ConnString.Count() > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var billingDetails = db.Billings.Where(s => s.AdtoneServerBillingId == command.Id).FirstOrDefault();
                    if (billingDetails != null)
                    {
                        db.Billings.Remove(billingDetails);
                        db.SaveChanges();
                    }
                }
            }

            return new CommandResult(true);
        }

        #endregion
    }
}
