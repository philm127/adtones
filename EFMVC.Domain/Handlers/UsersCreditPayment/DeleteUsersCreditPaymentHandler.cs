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
   public class DeleteUsersCreditPaymentHandler : ICommandHandler<DeleteUsersCreditPaymentCommand>
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
        public DeleteUsersCreditPaymentHandler(IUsersCreditPaymentRepository usercreditpaymentRepository, IUnitOfWork unitOfWork)
        {
            _usercreditpaymentRepository = usercreditpaymentRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(DeleteUsersCreditPaymentCommand command)
        {
            UsersCreditPayment userCreditpayment = _usercreditpaymentRepository.GetById(command.Id);
            _usercreditpaymentRepository.Delete(userCreditpayment);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
