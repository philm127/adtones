using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.BillingDetails;
using EFMVC.Model;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;
using EFMVC.Domain.OperatorServerData;

namespace EFMVC.Domain.Handlers.BillingDetailsHan
{
   public class CreateOrUpdateBillingDetailsHandler : ICommandHandler<CreateOrUpdateBillingDetailsCommand>
    {
        /// <summary>
        /// The billing repository
        /// </summary>
        /// 
        private readonly IBillingRepository _billingRepository;
        private readonly IBillingDetailsRepository _billingDetailsRepository;
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
        public CreateOrUpdateBillingDetailsHandler(IBillingDetailsRepository billingDetailsRepository, ICampaignProfileRepository profileRepository, IBillingRepository billingRepository, IUnitOfWork unitOfWork)
        {
            _billingDetailsRepository = billingDetailsRepository;
            _profileRepository = profileRepository;
            _billingRepository = billingRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateBillingDetailsCommand command)
        {
            var campaignProfileId = _billingRepository.GetById((long)command.BillingId).CampaignProfileId;

            int countryId = 0;
            var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == campaignProfileId);
            if (campaignProfile != null)
            {
                countryId = (int)campaignProfile.CountryId;
            }

            var ConnString = ConnectionString.GetConnectionStringByCountryId(campaignProfile.CountryId);
            if (command.Id == 0)
            {
                var billing = new BillingDetails
                {
                    Id = command.Id,
                   BillingId=command.BillingId,
                   BillingAddress=command.BillingAddress,
                   BillingAddress2 = command.BillingAddress2,
                   BillingTown =command.BillingTown,
                   BillingPostcode=command.BillingTown,
                   CardType=command.CardType,
                   CardNumber=command.CardNumber,
                   ExpiryMonth=command.ExpiryMonth,
                   ExpiryYear=command.ExpiryYear,
                   NameOfCard=command.NameOfCard,
                   PaypalEmail=command.PaypalEmail,
                   PaypalTranID=command.PaypalTranID,
                   SecurityCode=command.SecurityCode                 
                };

                _billingDetailsRepository.Add(billing);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerBillingId = OperatorServer.GetBillingIdFromOperatorServer(db, (int)command.BillingId);
                        if(externalServerBillingId != 0)
                        {
                            var billing2 = new BillingDetails
                            {
                                Id = command.Id,
                                BillingId = externalServerBillingId,
                                BillingAddress = command.BillingAddress,
                                BillingAddress2 = command.BillingAddress2,
                                BillingTown = command.BillingTown,
                                BillingPostcode = command.BillingTown,
                                CardType = command.CardType,
                                CardNumber = command.CardNumber,
                                ExpiryMonth = command.ExpiryMonth,
                                ExpiryYear = command.ExpiryYear,
                                NameOfCard = command.NameOfCard,
                                PaypalEmail = command.PaypalEmail,
                                PaypalTranID = command.PaypalTranID,
                                SecurityCode = command.SecurityCode,
                                AdtoneServerBillingDetailId = billing.Id
                            };
                            db.BillingDetails.Add(billing2);
                            db.SaveChanges();
                        }
                   
                    }
                }
               

                return new CommandResult(true, billing.Id);

            }
            else
            {
                BillingDetails billingdetails = _billingDetailsRepository.GetById(command.Id);
                billingdetails.BillingId = command.BillingId;
                billingdetails.BillingAddress = command.BillingAddress;
                billingdetails.BillingAddress2 = command.BillingAddress2;
                billingdetails.BillingPostcode = command.BillingPostcode;
                billingdetails.BillingTown = command.BillingTown;
                billingdetails.CardType = command.CardType;
                billingdetails.CardNumber = command.CardNumber;
                billingdetails.ExpiryMonth = command.ExpiryMonth;
                billingdetails.ExpiryYear = command.ExpiryYear;
                billingdetails.NameOfCard = command.NameOfCard;
                billingdetails.SecurityCode = command.SecurityCode;
                billingdetails.PaypalEmail = command.PaypalEmail;
                billingdetails.PaypalTranID = command.PaypalTranID;
                _billingDetailsRepository.Update(billingdetails);
                _unitOfWork.Commit();

                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var billingDetailsData = db.BillingDetails.Where(s=>s.AdtoneServerBillingDetailId == command.Id).FirstOrDefault();
                        if(billingDetailsData != null)
                        {
                            var externalServerBillingId = OperatorServer.GetBillingIdFromOperatorServer(db, (int)command.BillingId);

                            if(externalServerBillingId != 0)
                            {
                                billingDetailsData.BillingId = externalServerBillingId;
                                billingDetailsData.BillingAddress = command.BillingAddress;
                                billingDetailsData.BillingAddress2 = command.BillingAddress2;
                                billingDetailsData.BillingPostcode = command.BillingPostcode;
                                billingDetailsData.BillingTown = command.BillingTown;
                                billingDetailsData.CardType = command.CardType;
                                billingDetailsData.CardNumber = command.CardNumber;
                                billingDetailsData.ExpiryMonth = command.ExpiryMonth;
                                billingDetailsData.ExpiryYear = command.ExpiryYear;
                                billingDetailsData.NameOfCard = command.NameOfCard;
                                billingDetailsData.SecurityCode = command.SecurityCode;
                                billingDetailsData.PaypalEmail = command.PaypalEmail;
                                billingDetailsData.PaypalTranID = command.PaypalTranID;
                                db.SaveChanges();
                            }
                         
                        }                        
                    }
                }

               
                return new CommandResult(true);
            }

          

          
        }
    }
}
