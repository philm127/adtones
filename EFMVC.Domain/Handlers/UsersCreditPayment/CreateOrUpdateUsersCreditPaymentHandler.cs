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

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.CommandProcessor.Command.ICommandHandler{EFMVC.Domain.Commands.CreateOrUpdateUsersCreditPaymentCommand}" />
    public class CreateOrUpdateUsersCreditPaymentHandler : ICommandHandler<CreateOrUpdateUsersCreditPaymentCommand>
    {
        /// <summary>
        /// The _usercreditpayment repository
        /// </summary>
        private readonly IUsersCreditPaymentRepository _usercreditpaymentRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUsersCreditPaymentHandler"/> class.
        /// </summary>
        /// <param name="usercreditpaymentRepository">The usercreditpayment repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUsersCreditPaymentHandler(IUsersCreditPaymentRepository usercreditpaymentRepository, IUnitOfWork unitOfWork)
        {
            _usercreditpaymentRepository = usercreditpaymentRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        /// ICommandResult.
        /// </returns>
        public ICommandResult Execute(CreateOrUpdateUsersCreditPaymentCommand command)
        {
            if (command.Id == 0)
            {
                var usercreditpayment = new UsersCreditPayment
                {
                    UserId = command.UserId,
                    BillingId=command.BillingId,
                   Amount=command.Amount,
                   Description=command.Description,
                   Status=command.Status,
                    CreatedDate = command.CreatedDate,
                    UpdatedDate = command.UpdatedDate,
                    CampaignProfileId = command.CampaignProfileId
                };

                _usercreditpaymentRepository.Add(usercreditpayment);
            }
            else
            {
                UsersCreditPayment userCreditpayment = _usercreditpaymentRepository.GetById(command.Id);
                userCreditpayment.UserId = command.UserId;
                userCreditpayment.BillingId = command.BillingId;
                userCreditpayment.Amount = command.Amount;
                userCreditpayment.Description = command.Description;
                userCreditpayment.Status = command.Status;
                userCreditpayment.UpdatedDate = command.UpdatedDate;
                userCreditpayment.CampaignProfileId = command.CampaignProfileId;

                _usercreditpaymentRepository.Update(userCreditpayment);
            }

            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
