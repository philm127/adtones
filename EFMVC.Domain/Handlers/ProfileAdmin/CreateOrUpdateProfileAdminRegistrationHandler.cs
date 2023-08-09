using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.ProfileAdmin;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers.ProfileAdmin
{
    public class CreateOrUpdateProfileAdminRegistrationHandler : ICommandHandler<CreateOrUpdateProfileAdminRegistrationCommand>
    {
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateOperatorAdminRegistrationHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateProfileAdminRegistrationHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateProfileAdminRegistrationCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        ///
        public ICommandResult Execute(CreateOrUpdateProfileAdminRegistrationCommand command)
        {
            User user = null;
            if (command.UserId != 0)
            {
                user = _userRepository.GetById(command.UserId);
                if (user != null)
                {
                    //User
                    user.Email = command.Email;
                    user.FirstName = command.FirstName;
                    user.LastName = command.LastName;
                    user.PasswordHash = command.Password;
                    user.DateCreated = command.DateCreated;
                    user.Organisation = command.Organisation;
                    user.LastLoginTime = command.LastLoginTime;
                    user.Activated = command.Activated;
                    user.RoleId = command.RoleId;
                    user.VerificationStatus = command.VerificationStatus;
                    user.Outstandingdays = command.Outstandingdays;
                    user.OperatorId = command.OperatorId;
                    user.IsMsisdnMatch = command.IsMsisdnMatch;
                    user.IsEmailVerfication = command.IsEmailVerfication;
                    user.PhoneticAlphabet = command.PhoneticAlphabet;
                    user.IsMobileVerfication = command.IsMobileVerfication;
                    user.OrganisationTypeId = command.OrganisationTypeId;
                    user.UserMatchTableName = command.UserMatchTableName;
                }
            }
            else
            {
                user = new User
                {
                    Email = command.Email,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    PasswordHash = command.Password,
                    DateCreated = command.DateCreated,
                    Organisation = command.Organisation,
                    LastLoginTime = command.LastLoginTime,
                    Activated = command.Activated,
                    RoleId = command.RoleId,
                    VerificationStatus = command.VerificationStatus,
                    Outstandingdays = command.Outstandingdays,
                    OperatorId = command.OperatorId,
                    IsMsisdnMatch = command.IsMsisdnMatch,
                    IsEmailVerfication = command.IsEmailVerfication,
                    PhoneticAlphabet = command.PhoneticAlphabet,
                    IsMobileVerfication = command.IsMobileVerfication,
                    OrganisationTypeId = command.OrganisationTypeId,
                    UserMatchTableName = command.UserMatchTableName
                };
            }

            //var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);
            if (user.UserId == 0)
            {
                _userRepository.Add(user);
                _unitOfWork.Commit();
                //if (ConnString != null && ConnString.Count() > 0)
                //{
                //    foreach (var item in ConnString)
                //    {
                //        EFMVCDataContex db = new EFMVCDataContex(item);
                //        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                //        var externalOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);

                //        if (externalServerCountryId != 0 && externalOperatorId != 0)
                //        {
                //            var user2 = new User
                //            {
                //                Email = command.Email,
                //                FirstName = command.FirstName,
                //                LastName = command.LastName,
                //                PasswordHash = command.Password,
                //                DateCreated = command.DateCreated,
                //                Organisation = command.Organisation,
                //                LastLoginTime = command.LastLoginTime,
                //                Activated = command.Activated,
                //                RoleId = command.RoleId,
                //                VerificationStatus = command.VerificationStatus,
                //                Outstandingdays = command.Outstandingdays,
                //                OperatorId = externalOperatorId,
                //                IsMsisdnMatch = command.IsMsisdnMatch,
                //                IsEmailVerfication = command.IsEmailVerfication,
                //                PhoneticAlphabet = command.PhoneticAlphabet,
                //                IsMobileVerfication = command.IsMobileVerfication,
                //                OrganisationTypeId = command.OrganisationTypeId,
                //                UserMatchTableName = command.UserMatchTableName,
                //                AdtoneServerUserId = user.UserId
                //            };
                //            db.Users.Add(user2);
                //            db.SaveChanges();
                //        }

                //    }
                //}
            }
            else
            {
                _userRepository.Update(user);
                _unitOfWork.Commit();
                //if (ConnString != null && ConnString.Count() > 0)
                //{
                //    foreach (var item in ConnString)
                //    {
                //        EFMVCDataContex db = new EFMVCDataContex(item);
                //        var user3 = db.Users.Where(s => s.AdtoneServerUserId == command.UserId).FirstOrDefault();
                //        if (user3 != null)
                //        {
                //            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                //            var externalOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);

                //            if (externalServerCountryId != 0 && externalOperatorId != 0)
                //            {
                //                user3.Email = command.Email;
                //                user3.FirstName = command.FirstName;
                //                user3.LastName = command.LastName;
                //                user3.PasswordHash = command.Password;
                //                user3.DateCreated = command.DateCreated;
                //                user3.Organisation = command.Organisation;
                //                user3.LastLoginTime = command.LastLoginTime;
                //                user3.Activated = command.Activated;
                //                user3.RoleId = command.RoleId;
                //                user3.VerificationStatus = command.VerificationStatus;
                //                user3.Outstandingdays = command.Outstandingdays;
                //                user3.OperatorId = externalOperatorId;
                //                user3.IsMsisdnMatch = command.IsMsisdnMatch;
                //                user3.IsEmailVerfication = command.IsEmailVerfication;
                //                user3.PhoneticAlphabet = command.PhoneticAlphabet;
                //                user3.IsMobileVerfication = command.IsMobileVerfication;
                //                user3.OrganisationTypeId = command.OrganisationTypeId;
                //                user3.UserMatchTableName = command.UserMatchTableName;
                //                user3.AdtoneServerUserId = user.UserId;
                //                db.SaveChanges();
                //            }

                //        }
                //    }
                //}
            }

            return new CommandResult(true, user.UserId);
        }

        #endregion
    }
}
