
using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Clients;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using System.Linq;

namespace EFMVC.Domain.Handlers.Clients
{
    public class CreateOrUpdateClientHandler : ICommandHandler<CreateOrUpdateClientCommand>
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
        /// Initializes a new instance of the <see cref="CreateOrUpdateClientHandler" /> class.
        /// </summary>
        /// <param name="clientRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateClientHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateClientCommand command)
        {
            var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);
            if (command.Id == 0)
            {
                var client = new Client
                {
                    Id=command.Id,
                    UserId=command.UserId,
                    Name = command.Name,
                    Budget = command.Budget,
                    ContactInfo = command.ContactInfo,
                    Description = command.Description,
                    Status = command.Status,
                    CreatedDate = command.CreatedDate,
                    UpdatedDate = command.UpdatedDate,
                    NextStatus = false,                   
                    CountryId = command.CountryId

                };

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
                            var client2 = new Client
                            {
                                Id = command.Id,
                                UserId = externalServerUserId,
                                Name = command.Name,
                                Budget = command.Budget,
                                ContactInfo = command.ContactInfo,
                                Description = command.Description,
                                Status = command.Status,
                                CreatedDate = command.CreatedDate,
                                UpdatedDate = command.UpdatedDate,
                                NextStatus = false,
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
                Client client = _clientRepository.GetById(command.Id);
                client.Name = command.Name;
                client.UserId = command.UserId;
                client.Budget = command.Budget;
                client.ContactInfo = command.ContactInfo;
                client.Description = command.Description;
                client.Status = command.Status;
                client.CreatedDate = client.CreatedDate;
                client.UpdatedDate = command.UpdatedDate;
                client.NextStatus = false;
                client.CountryId = command.CountryId;
                _clientRepository.Update(client);
                _unitOfWork.Commit();

                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var clientDetails = db.Clients.Where(s => s.AdtoneServerClientId == command.Id).FirstOrDefault();
                        if(clientDetails != null)
                        {
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);

                            if (externalServerUserId != 0 && externalServerCountryId != 0)
                            {
                                clientDetails.Name = command.Name;
                                clientDetails.UserId = externalServerUserId;
                                clientDetails.Budget = command.Budget;
                                clientDetails.ContactInfo = command.ContactInfo;
                                clientDetails.Description = command.Description;
                                clientDetails.Status = command.Status;
                                clientDetails.CreatedDate = client.CreatedDate;
                                clientDetails.UpdatedDate = command.UpdatedDate;
                                clientDetails.NextStatus = false;
                                clientDetails.CountryId = externalServerCountryId;
                                db.SaveChanges();
                            }
                             
                        }
                        
                    }

                }
            }

           

            return new CommandResult(true);
        }

        #endregion
    }
}
