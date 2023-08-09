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
    /// <seealso cref="EFMVC.CommandProcessor.Command.ICommandHandler{EFMVC.Domain.Commands.UsersCredit.DeleteUsersCreditCommand}" />
    public class DeleteUsersCreditHandler : ICommandHandler<DeleteUsersCreditCommand>
    {  
        /// <summary>
       /// The _usercredit repository
       /// </summary>
        private readonly IUsersCreditRepository _usercreditRepository;


        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUsersCreditHandler"/> class.
        /// </summary>
        /// <param name="usercreditRepository">The usercredit repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteUsersCreditHandler(IUsersCreditRepository usercreditRepository, IUnitOfWork unitOfWork)
        {
            _usercreditRepository = usercreditRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(DeleteUsersCreditCommand command)
        {
            UsersCredit usercredit = _usercreditRepository.GetById(command.Id);
            _usercreditRepository.Delete(usercredit);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

    }
}
