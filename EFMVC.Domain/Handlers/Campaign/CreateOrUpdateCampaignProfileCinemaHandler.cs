// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileCinemaHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileCinemaHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileCinemaHandler :
        ICommandHandler<CreateOrUpdateCampaignProfileCinemaCommand>
    {
        /// <summary>
        /// The _cinema repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _cinemaRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileCinemaHandler"/> class.
        /// </summary>
        /// <param name="cinemaRepository">The cinema repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileCinemaHandler(ICampaignProfilePreferenceRepository cinemaRepository,
                                                          IUnitOfWork unitOfWork)
        {
            _cinemaRepository = cinemaRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileCinemaCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileCinemaCommand command)
        {
            var cinema = new CampaignProfilePreference
            {
                Cinema_Cinema = command.Cinema_Cinema,
                CampaignProfileId = command.CampaignProfileId,
                Id = command.CampaignProfileCinemaId
            };

            if (cinema.Id == 0)
            {
                _cinemaRepository.Add(cinema);
            }
            else
            {
                CampaignProfilePreference campaignProfile = _cinemaRepository.GetById(command.CampaignProfileCinemaId);
                campaignProfile.Cinema_Cinema = command.Cinema_Cinema;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.Id = campaignProfile.Id;
                _cinemaRepository.Update(campaignProfile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}