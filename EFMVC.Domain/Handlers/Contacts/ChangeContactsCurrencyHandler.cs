using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Contacts;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers.Contacts
{
    public class ChangeContactsCurrencyHandler : ICommandHandler<ChangeContactsCurrencyCommand>
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The contact repository
        /// </summary>
        private readonly IContactsRepository _contactsRepository;

        public ChangeContactsCurrencyHandler(IContactsRepository contactsRepository, IUnitOfWork unitOfWork)
        {
            this._contactsRepository = contactsRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(ChangeContactsCurrencyCommand command)
        {
            Model.Contacts contacts = _contactsRepository.GetById(command.Id);
            contacts.CurrencyId = command.CurrencyId;
            _contactsRepository.Update(contacts);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
