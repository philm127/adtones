// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateAdvertHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;

/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateAdvertHandler.
    /// </summary>
    public class CreateOrUpdateSystemConfigHandler : ICommandHandler<CreateOrUpdateSystemConfigCommand>
    {
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly ISystemConfigRepository _systemConfigRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateAdvertHandler" /> class.
        /// </summary>
        /// <param name="advertRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateSystemConfigHandler(ISystemConfigRepository systemConfigRepository, IUnitOfWork unitOfWork)
        {
            _systemConfigRepository = systemConfigRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateSystemConfigCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateSystemConfigCommand command)
        {
            if (command.SystemConfigId == 0)
            {
                var systemConfig = new SystemConfig
                                 {
                                     SystemConfigId = command.SystemConfigId,
                                     SystemConfigKey = command.SystemConfigKey,
                                     SystemConfigValue = command.SystemConfigValue,
                                     SystemConfigType = command.SystemConfigType,
                                     CreatedDateTime = command.CreatedDateTime,
                                     UpdatedDateTime = command.UpdatedDateTime,
                                 };

                _systemConfigRepository.Add(systemConfig);
            }
            else
            {
                SystemConfig systemConfig = _systemConfigRepository.GetById(command.SystemConfigId);
                systemConfig.SystemConfigKey = command.SystemConfigKey;
                systemConfig.SystemConfigValue = command.SystemConfigValue;
                systemConfig.SystemConfigType = command.SystemConfigType;
                systemConfig.CreatedDateTime = command.CreatedDateTime;
                systemConfig.UpdatedDateTime = command.UpdatedDateTime;

                _systemConfigRepository.Update(systemConfig);
            }

            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}