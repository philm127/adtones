using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Contacts;

namespace EFMVC.Domain.Handlers.Contacts
{
   public class DeleteContactsHandler : ICommandHandler<DeleteContactsCommand>
    {
        /// <summary>
        /// The _blocked number repository
        /// </summary>
        private readonly IContactsRepository _contactRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBlockedNumberHandler"/> class.
        /// </summary>
        /// <param name="contactRepository">The blocked number repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteContactsHandler(IContactsRepository contactRepository, IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteContactsCommand command)
        {
            Model.Contacts contactinfo = _contactRepository.GetById(command.Id);
            _contactRepository.Delete(contactinfo);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
