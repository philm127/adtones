// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileAdvertsReceivedHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Domain.Commands;
using EFMVC.Model;

namespace EFMVC.Domain.Handlers
{
    public class CreateOrUpdateUserProfileAdvertsReceivedHandler :
        ICommandHandler<CreateOrUpdateUserProfileAdvertsReceivedCommand>
    {
        private readonly IRepository<UserProfileAdvertsReceived> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrUpdateUserProfileAdvertsReceivedHandler(IRepository<UserProfileAdvertsReceived> repository,
                                                               IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileAdvertsReceivedCommand> Members

        public ICommandResult Execute(CreateOrUpdateUserProfileAdvertsReceivedCommand command)
        {
            var profile = new UserProfileAdvertsReceived
                              {
                                  AdvertName = command.AdvertName,
                                  AdvertRef = command.AdvertRef,
                                  Brand = command.Brand,
                                  FileName = command.FileName,
                                  CreditsReceived = command.CreditsReceived,
                                  DateTimePlayed = command.DateTimePlayed,
                                  Id = command.Id,
                                  UserProfileId = command.UserProfileId
                              };

            if (profile.UserProfileId == 0)
                _repository.Add(profile);
            else
                _repository.Update(profile);

            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}