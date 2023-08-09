using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Clients;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers.Clients
{
    public class CreateOrUpdateCopyClientHandler : ICommandHandler<CreateOrUpdateCopyClientCommand>
    {
        /// <summary>
        /// The client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCopyClientHandler"/> class.
        /// </summary>
        /// <param name="clientRepository">The client repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCopyClientHandler(IClientRepository clientRepository,
                                                    IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateCopyClientCommand command)
        {
            var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);

            Model.Client client = null;
            client = _clientRepository.GetById(command.ClientId);
            if (client != null)
            {
                client.UserId = command.UserId;
                client.Name = command.ClientName;
                client.Description = command.ClientDescription;
                client.ContactInfo = command.ClientContactInfo;
                client.Budget = command.ClientBudget;
                client.CreatedDate = System.DateTime.Now;
                client.UpdatedDate = System.DateTime.Now;
                client.Status = command.ClientStatus;
                client.Email = command.ClientEmail;
                client.PhoneticAlphabet = command.ClientPhoneticAlphabet;
                client.NextStatus = command.NextStatus;
                client.ContactPhone = command.ClientContactPhone;
                client.CountryId = command.CountryId;
                client.AdtoneServerClientId = command.AdtoneServerClientId;
            }
            else
            {
                client = new Model.Client
                {
                    Id = command.ClientId,
                    UserId = command.UserId,
                    Name = command.ClientName,
                    Description = command.ClientDescription,
                    ContactInfo = command.ClientContactInfo,
                    Budget = command.ClientBudget,
                    CreatedDate = System.DateTime.Now,
                    UpdatedDate = System.DateTime.Now,
                    Status = command.ClientStatus,
                    Email = command.ClientEmail,
                    PhoneticAlphabet = command.ClientPhoneticAlphabet,
                    NextStatus = command.NextStatus,
                    ContactPhone = command.ClientContactPhone,
                    CountryId = command.CountryId,
                    AdtoneServerClientId = command.AdtoneServerClientId
                };
            }

            if (client.Id == 0)
            {
                _clientRepository.Add(client);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                        if(externalServerUserId != 0 && externalServerCountryId != 0)
                        {
                            var client2 = new Model.Client
                            {
                                Id = command.ClientId,
                                UserId = externalServerUserId,
                                Name = command.ClientName,
                                Description = command.ClientDescription,
                                ContactInfo = command.ClientContactInfo,
                                Budget = command.ClientBudget,
                                CreatedDate = System.DateTime.Now,
                                UpdatedDate = System.DateTime.Now,
                                Status = command.ClientStatus,
                                Email = command.ClientEmail,
                                PhoneticAlphabet = command.ClientPhoneticAlphabet,
                                NextStatus = command.NextStatus,
                                ContactPhone = command.ClientContactPhone,
                                CountryId = externalServerCountryId,
                                AdtoneServerClientId = client.Id
                            };
                            db.Clients.Add(client2);
                            db.SaveChanges();
                        }

                       
                    }
                }
            }
            else
            {
                _clientRepository.Update(client);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var clientDetails = db.Clients.Where(s => s.AdtoneServerClientId == command.ClientId).FirstOrDefault();
                        if (clientDetails != null)
                        {
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                            if (externalServerUserId != 0 && externalServerCountryId != 0)
                            {
                                clientDetails.UserId = externalServerUserId;
                                clientDetails.Name = command.ClientName;
                                clientDetails.Description = command.ClientDescription;
                                clientDetails.ContactInfo = command.ClientContactInfo;
                                clientDetails.Budget = command.ClientBudget;
                                clientDetails.CreatedDate = System.DateTime.Now;
                                clientDetails.UpdatedDate = System.DateTime.Now;
                                clientDetails.Status = command.ClientStatus;
                                clientDetails.Email = command.ClientEmail;
                                clientDetails.PhoneticAlphabet = command.ClientPhoneticAlphabet;
                                clientDetails.NextStatus = command.NextStatus;
                                clientDetails.ContactPhone = command.ClientContactPhone;
                                clientDetails.CountryId = externalServerCountryId;
                                db.SaveChanges();
                            }
                                
                        }
                    }
                }
            }




            return new CommandResult(true, client.Id);
        }
    }
}
