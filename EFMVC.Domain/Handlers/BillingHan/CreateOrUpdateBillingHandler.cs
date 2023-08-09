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
using EFMVC.Domain.OperatorServerData;

namespace EFMVC.Domain.Handlers.BillingHan
{
    public class CreateOrUpdateBillingHandler : ICommandHandler<CreateOrUpdateBillingCommand>
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
        public CreateOrUpdateBillingHandler(IBillingRepository billingRepository, ICampaignProfileRepository profileRepository, IUnitOfWork unitOfWork)
        {
            _billingRepository = billingRepository;
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }
       
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateBillingCommand command)
        {
            int countryId = 0;
            var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == command.CampaignProfileId);
            if (campaignProfile != null)
            {
                countryId = (int)campaignProfile.CountryId;
            }
            var ConnString = ConnectionString.GetConnectionStringByCountryId(campaignProfile.CountryId);
            if (command.Id == 0)
            {
                if (command.ClientId == 0)
                {
                    command.ClientId = null;
                }

                var billing = new Billing
                {
                    Id = command.Id,
                    UserId = command.UserId,
                    ClientId = command.ClientId,
                    CampaignProfileId = command.CampaignProfileId,
                    PaymentMethodId = command.PaymentMethodId,
                    InvoiceNumber = command.InvoiceNumber,
                    PONumber = command.PONumber,
                    FundAmount = command.FundAmount,
                    TaxPercantage=command.TaxPercantage,
                    TotalAmount = command.TotalAmount,
                    PaymentDate = command.PaymentDate,
                    SettledDate = command.SettledDate,
                    Status = command.Status,
                    ErrorCode = command.ErrorCode,
                    ErrorDescription = command.ErrorDescription,
                    CurrencyCode = command.CurrencyCode
                };

                _billingRepository.Add(billing);                
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        if (command.ClientId == null)
                        {
                            command.ClientId = 0;
                        }

                        EFMVCDataContex db = new EFMVCDataContex(item);
                        int? externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, (int)command.ClientId);
                        var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, (int)command.CampaignProfileId);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, (int)command.UserId);

                        if (externalServerCampaignProfileId != 0 && externalServerUserId != 0)//externalServerClientId != 0 && 
                        {
                            int? operatorClientId;
                            if (externalServerClientId == 0)
                            {
                                operatorClientId = null;
                            }
                            else
                            {
                                operatorClientId = externalServerClientId;
                            }

                            var billing2 = new Billing
                            {
                                Id = command.Id,
                                UserId = externalServerUserId,
                                ClientId = operatorClientId,
                                CampaignProfileId = externalServerCampaignProfileId,
                                PaymentMethodId = command.PaymentMethodId,
                                InvoiceNumber = command.InvoiceNumber,
                                PONumber = command.PONumber,
                                FundAmount = command.FundAmount,
                                TaxPercantage = command.TaxPercantage,
                                TotalAmount = command.TotalAmount,
                                PaymentDate = command.PaymentDate,
                                SettledDate = command.SettledDate,
                                Status = command.Status,
                                ErrorCode = command.ErrorCode,
                                ErrorDescription = command.ErrorDescription,
                                AdtoneServerBillingId = billing.Id,
                                CurrencyCode = command.CurrencyCode

                            };
                            db.Billings.Add(billing2);
                            db.SaveChanges();
                        }

                    }
                }
                return new CommandResult(true,billing.Id);
            }
            else
            {
                if (command.ClientId == 0)
                {
                    command.ClientId = null;
                }

                Billing billing = _billingRepository.GetById(command.Id);
                billing.ClientId = command.ClientId;
                billing.UserId = command.UserId;
                billing.PaymentMethodId = command.PaymentMethodId;
                billing.CampaignProfileId = command.CampaignProfileId;
                billing.InvoiceNumber = command.InvoiceNumber;
                billing.PONumber = command.PONumber;
                billing.FundAmount = command.FundAmount;
                billing.TaxPercantage = command.TaxPercantage;
                billing.TotalAmount = command.TotalAmount;
                billing.PaymentDate = command.PaymentDate;
                billing.SettledDate = command.SettledDate;
                billing.Status = command.Status;
                billing.ErrorCode = command.ErrorCode;
                billing.ErrorDescription = command.ErrorDescription;
                billing.CurrencyCode = command.CurrencyCode;
                _billingRepository.Update(billing);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        if (command.ClientId == null)
                        {
                            command.ClientId = 0;
                        }

                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var billingDetail = db.Billings.Where(s => s.AdtoneServerBillingId == command.Id).FirstOrDefault();
                        if(billingDetail != null)
                        {
                            int? externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, (int)command.ClientId);
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, (int)command.CampaignProfileId);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, (int)command.UserId);

                            if (externalServerCampaignProfileId != 0 && externalServerUserId != 0)//externalServerClientId != 0 && 
                            {
                                int? operatorClientId;
                                if (externalServerClientId == 0)
                                {
                                    operatorClientId = null;
                                }
                                else
                                {
                                    operatorClientId = externalServerClientId;
                                }

                                billingDetail.ClientId = operatorClientId;
                                billingDetail.UserId = externalServerUserId;
                                billingDetail.PaymentMethodId = command.PaymentMethodId;
                                billingDetail.CampaignProfileId = externalServerCampaignProfileId;
                                billingDetail.InvoiceNumber = command.InvoiceNumber;
                                billingDetail.PONumber = command.PONumber;
                                billingDetail.FundAmount = command.FundAmount;
                                billingDetail.TaxPercantage = command.TaxPercantage;
                                billingDetail.TotalAmount = command.TotalAmount;
                                billingDetail.PaymentDate = command.PaymentDate;
                                billingDetail.SettledDate = command.SettledDate;
                                billingDetail.Status = command.Status;
                                billingDetail.ErrorCode = command.ErrorCode;
                                billingDetail.ErrorDescription = command.ErrorDescription;
                                billingDetail.CurrencyCode = command.CurrencyCode;
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
