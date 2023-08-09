using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Security;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers.Security
{
    public class UserPasswordHistoryHandler : ICommandHandler<UserPasswordHistoryCommand>
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The user password history repository
        /// </summary>
        private readonly IUserPasswordHistoryRepository userPasswordHistoryRepository;

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository userRepository;

        public UserPasswordHistoryHandler(IUserPasswordHistoryRepository userPasswordHistoryRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userPasswordHistoryRepository = userPasswordHistoryRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(UserPasswordHistoryCommand command)
        {
            UserPasswordHistory userPasswordHistory = null;

            userPasswordHistory = new UserPasswordHistory
            {
                UserId = command.UserId,
                PasswordHash = command.PasswordHash,
                DateCreated = command.DateCreated
            };

            userPasswordHistoryRepository.Add(userPasswordHistory);
            unitOfWork.Commit();
            var operatorId = userRepository.GetById(command.UserId).OperatorId;
            var ConnString = ConnectionString.GetConnectionStringByOperatorId(operatorId);
            if (ConnString != null && ConnString.Count > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, operatorId);
                    var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                    if (externalServerOperatorId != 0 && externalServerUserId != 0)
                    {
                        UserPasswordHistory userPasswordHistory2 = null;
                        userPasswordHistory2 = new UserPasswordHistory
                        {
                            UserId = externalServerUserId,
                            PasswordHash = command.PasswordHash,
                            DateCreated = command.DateCreated,
                            AdtoneServerUserPasswordHistoryId = userPasswordHistory.UserPasswordHistoryId
                        };
                        db.UserPasswordHistories.Add(userPasswordHistory2);
                        db.SaveChanges();
                    }
                }
            }
            return new CommandResult(true);
        }
    }
}
