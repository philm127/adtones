// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="DeleteAdvertHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
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
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class DeleteAdvertHandler.
    /// </summary>
    public class DeleteUserRewardHandler : ICommandHandler<DeleteUserRewardCommand>
    {
        /// <summary>
        /// The _userReward repository
        /// </summary>
        private readonly IUserRewardRepository _userRewardRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAdvertHandler" /> class.
        /// </summary>
        /// <param name="userRewardRepository">The userReward repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteUserRewardHandler(IUserRewardRepository userRewardRepository, IUnitOfWork unitOfWork)
        {
            _userRewardRepository = userRewardRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<DeleteUserRewardCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteUserRewardCommand command)
        {
            Model.Entities.UserReward userReward = _userRewardRepository.GetById(command.UserRewardId);

            //Add 22-02-2019
            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorId);

            _userRewardRepository.Delete(userReward);


            if (ConnString != null && ConnString.Count > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var userReward2 = db.UserRewards.Where(s => s.AdtoneServerUserRewardId == command.UserRewardId).FirstOrDefault();
                    if (userReward2 != null)
                    {
                        db.UserRewards.Remove(userReward2);
                        db.SaveChanges();
                    }
                }
            }

            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}