using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Security;
using EFMVC.Model;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;

namespace EFMVC.Domain.Handlers.Security
{
   public class ChangeUserProfileInfoHandler : ICommandHandler<ChangeUserProfileInfoCommand>
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContactsRepository _contactsRepository;
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmailHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public ChangeUserProfileInfoHandler(IUserRepository userRepository, IContactsRepository contactsRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _contactsRepository = contactsRepository;
            _unitOfWork = unitOfWork;
        }
        #region ICommandHandler<ChangeEmailCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>

        public ICommandResult Execute(ChangeUserProfileInfoCommand command)
        {
            User user = _userRepository.GetById(command.UserId);
            user.Email = command.Email;
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.Organisation = command.Organisation;
            _unitOfWork.Commit();

            if (user.Operator != null && user.RoleId == 2)
            {
                var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);

                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach(var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);

                        var userResult = db.Users.Where(s => s.AdtoneServerUserId == command.UserId).FirstOrDefault();
                        if (userResult != null)
                        {
                            userResult.Email = command.Email;
                            userResult.FirstName = command.FirstName;
                            userResult.LastName = command.LastName;
                            userResult.Organisation = command.Organisation;
                            userResult.VerificationStatus = command.VerificationStatus;
                            db.SaveChanges();
                        }
                    }
                    
                }
            }
            else if(user.RoleId == 3)
            {
                
                var contactData = _contactsRepository.GetMany(s => s.UserId == command.UserId).FirstOrDefault();
                if(contactData != null && contactData.CountryId != null)
                {
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(contactData.CountryId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex db = new EFMVCDataContex(item);
                            var userResult = db.Users.Where(s => s.AdtoneServerUserId == command.UserId).FirstOrDefault();
                            if (userResult != null)
                            {
                                userResult.Email = command.Email;
                                userResult.FirstName = command.FirstName;
                                userResult.LastName = command.LastName;
                                userResult.Organisation = command.Organisation;
                                userResult.VerificationStatus = command.VerificationStatus;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            return new CommandResult(true);
        }

        //Commented 19-02-2019
        //New Add 15-02-2019
        //public ICommandResult Execute(ChangeUserProfileInfoCommand command)
        //{
        //    User user = _userRepository.GetById(command.UserId);
        //    user.Email = command.Email;
        //    user.FirstName = command.FirstName;
        //    user.LastName = command.LastName;
        //    user.Organisation = command.Organisation;
        //    user.RewardId = command.RewardId;
        //    _unitOfWork.Commit();

        //    if (user.Operator != null)
        //    {
        //        var ConnString = ConnectionString.GetConnectionString(user.Operator.CountryId);

        //        if (!string.IsNullOrEmpty(ConnString))
        //        {
        //            EFMVCDataContex db = new EFMVCDataContex(ConnString);

        //            var userResult = db.Users.Where(s => s.UserId == command.UserId).FirstOrDefault();
        //            if (userResult != null)
        //            {
        //                userResult.Email = command.Email;
        //                userResult.FirstName = command.FirstName;
        //                userResult.LastName = command.LastName;
        //                userResult.Organisation = command.Organisation;
        //                userResult.VerificationStatus = command.VerificationStatus;
        //                userResult.RewardId = command.RewardId;
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    return new CommandResult(true);
        //}

        #endregion
    }
}
