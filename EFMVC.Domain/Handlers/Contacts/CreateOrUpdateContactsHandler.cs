using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Contacts;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;
using EFMVC.Domain.OperatorServerData;

namespace EFMVC.Domain.Handlers.Contacts
{
    public class CreateOrUpdateContactsHandler : ICommandHandler<CreateOrUpdateContactsCommand>
    {
        /// <summary>
        /// The _blocked number repository
        /// </summary>
        private readonly IContactsRepository _contactsRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateProfileHandler"/> class.
        /// </summary>
        /// <param name="contactsRepository">The blocked number repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>

        public CreateOrUpdateContactsHandler(IContactsRepository contactsRepository, IUnitOfWork unitOfWork)
        {
            _contactsRepository = contactsRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateContactsCommand command)
        {
            Model.Contacts contactinfo = null;
            if (command.Id != 0)
            {
                contactinfo = _contactsRepository.GetById(command.Id);
                if (contactinfo != null)
                {
                    contactinfo.UserId = command.UserId;
                    contactinfo.Address = command.Address;
                    contactinfo.Email = command.Email;
                    contactinfo.FixedLine = command.FixedLine;
                    contactinfo.MobileNumber = command.MobileNumber;
                    contactinfo.PhoneNumber = command.PhoneNumber;
                    contactinfo.CountryId = command.CountryId;
                    contactinfo.CurrencyId = command.CurrencyId;
                }
            }
            else
            {
                contactinfo = new Model.Contacts
                {
                    UserId = command.UserId,
                    Address = command.Address,
                    Email = command.Email,
                    FixedLine = command.FixedLine,
                    MobileNumber = command.MobileNumber,
                    PhoneNumber = command.PhoneNumber,
                    CountryId = command.CountryId,
                    CurrencyId = command.CurrencyId
                };
            }

            var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);

            if (contactinfo.Id == 0)
            {
                _contactsRepository.Add(contactinfo);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, (int)command.CountryId);
                        if (externalServerUserId != 0 && externalServerCountryId != 0)
                        {
                            var contactinfo2 = new Model.Contacts
                            {
                                Id = command.Id,
                                UserId = externalServerUserId,
                                Address = command.Address,
                                Email = command.Email,
                                FixedLine = command.FixedLine,
                                MobileNumber = command.MobileNumber,
                                PhoneNumber = command.PhoneNumber,
                                CountryId = externalServerCountryId,
                                CurrencyId = command.CurrencyId,
                                AdtoneServerContactId = contactinfo.Id
                            };
                            db.Contacts.Add(contactinfo2);
                            db.SaveChanges();
                        }

                    }
                }
            }
            else
            {
                _contactsRepository.Update(contactinfo);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var contactData = db.Contacts.Where(s => s.AdtoneServerContactId == command.Id).FirstOrDefault();
                        if (contactData != null)
                        {
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, (int)command.CountryId);
                            if (externalServerUserId != 0 && externalServerCountryId != 0)
                            {
                                contactData.UserId = externalServerUserId;
                                contactData.Address = command.Address;
                                contactData.Email = command.Email;
                                contactData.FixedLine = command.FixedLine;
                                contactData.MobileNumber = command.MobileNumber;
                                contactData.PhoneNumber = command.PhoneNumber;
                                contactData.CountryId = externalServerCountryId;
                                contactData.CurrencyId = command.CurrencyId;
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
