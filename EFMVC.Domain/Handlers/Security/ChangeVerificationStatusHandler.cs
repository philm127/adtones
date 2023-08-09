// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="ChangePasswordHandler.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using System.Linq;

/// <summary>
/// The Security namespace.
/// </summary>

namespace EFMVC.Domain.Handlers.Security
{
    /// <summary>
    /// Class ChangePasswordHandler.
    /// </summary>
    public class ChangeVerificationStatusHandler : ICommandHandler<ChangeVerificationStatusCommand>
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;
        private readonly IContactsRepository contactsRepository;
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public ChangeVerificationStatusHandler(IUserRepository userRepository, IContactsRepository contactsRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.contactsRepository = contactsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ICommandHandler<ChangeVerificationStatusCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(ChangeVerificationStatusCommand command)
        {
            User user = userRepository.GetById(command.UserId);
            user.VerificationStatus = command.VerificationStatus;
            user.IsEmailVerfication = command.IsEmailVerfication;
            userRepository.Update(user);
            unitOfWork.Commit();

            if (user.RoleId == 2)
            {
                var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);

                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var userResult = db.Users.Where(s => s.AdtoneServerUserId == command.UserId).FirstOrDefault();
                        if (userResult != null)
                        {
                            userResult.VerificationStatus = command.VerificationStatus;
                            userResult.IsEmailVerfication = command.IsEmailVerfication;
                            db.SaveChanges();
                        }
                    }
                }
            }
            else if(user.RoleId == 3)
            {
                var contactsData = contactsRepository.GetMany(s => s.UserId == command.UserId).FirstOrDefault();
                if(contactsData != null)
                {
                    var countryId = contactsData.CountryId == null ? 0 : contactsData.CountryId;
                    var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);

                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex db = new EFMVCDataContex(item);
                            var userResult = db.Users.Where(s => s.AdtoneServerUserId == command.UserId).FirstOrDefault();
                            if (userResult != null)
                            {
                                userResult.VerificationStatus = command.VerificationStatus;
                                userResult.IsEmailVerfication = command.IsEmailVerfication;
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