// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateBlockedNumberHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.BlockedNumber;
using EFMVC.Domain.CountryConnectionString;
using System.Linq;

/// <summary>
/// The BlockedNumber namespace.
/// </summary>

namespace EFMVC.Domain.Handlers.BlockedNumber
{
    /// <summary>
    /// Class CreateOrUpdateProfileHandler.
    /// </summary>
    public class CreateOrUpdateProfileHandler : ICommandHandler<CreateOrUpdateBlockedNumberCommand>
    {
        /// <summary>
        /// The _blocked number repository
        /// </summary>
        private readonly IBlockedNumberRepository _blockedNumberRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateProfileHandler"/> class.
        /// </summary>
        /// <param name="blockedNumberRepository">The blocked number repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateProfileHandler(IBlockedNumberRepository blockedNumberRepository, IUnitOfWork unitOfWork)
        {
            _blockedNumberRepository = blockedNumberRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateBlockedNumberCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateBlockedNumberCommand command)
        {
            var blockedNumber = new Model.BlockedNumber
                                    {
                                        Id = command.Id,
                                        UserId = command.UserId,
                                        TelephoneNumber = command.TelephoneNumber,
                                        Name = command.Name,
                                        Active = command.Active
                                    };

            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorId);
            if (blockedNumber.Id == 0)
            {
                _blockedNumberRepository.Add(blockedNumber);

                if (ConnString != null && ConnString.Count > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);                     
                        db.BlockedNumbers.Add(blockedNumber);
                        db.SaveChanges();
                    }

                }
            }
            else
            {
                _blockedNumberRepository.Update(blockedNumber);
                if (ConnString != null && ConnString.Count > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var blockNumber = db.BlockedNumbers.Where(s => s.Id == command.Id).FirstOrDefault();
                        if (blockNumber != null)
                        {
                            blockNumber.UserId = command.UserId;
                            blockNumber.TelephoneNumber = command.TelephoneNumber;
                            blockNumber.Name = command.Name;
                            blockNumber.Active = command.Active;
                            db.SaveChanges();
                        }
                    }

                }
            }

            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}