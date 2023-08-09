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
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using System.Linq;
/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateAdvertHandler.
    /// </summary>
    public class CreateOrUpdateAdvertRejectionHandler : ICommandHandler<CreateOrUpdateAdvertRejectionCommand>
    {
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRejectionRepository _advertRejectionRepository;
        private readonly IAdvertRepository _advertRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateAdvertRejectionHandler" /> class.
        /// </summary>
        /// <param name="advertRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateAdvertRejectionHandler(IAdvertRejectionRepository advertRejectionRepository, IAdvertRepository advertRepository, IUnitOfWork unitOfWork)
        {
            _advertRejectionRepository = advertRejectionRepository;
            _advertRepository = advertRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateAdvertRejectionHandler> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateAdvertRejectionCommand command)
        {
            var countryId = _advertRepository.GetById((long)command.AdvertId).CountryId;
            var ConnString = ConnectionString.GetConnectionStringByCountryId(countryId);
            if (command.AdvertRejectionId == 0)
            {
                var advertRejection = new AdvertRejection
                                 {
                                     AdvertId = command.AdvertId,
                                     UserId = command.UserId,
                                     CreatedDate = command.CreatedDate,
                                     RejectionReason = command.RejectionReason
                                };

                _advertRejectionRepository.Add(advertRejection);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerAdvertId = OperatorServer.GetAdvertIdFromOperatorServer(db, (int)command.AdvertId);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, (int)command.UserId);

                        if(externalServerAdvertId != 0 && externalServerUserId != 0)
                        {
                            var advertRejection2 = new AdvertRejection
                            {
                                AdvertId = externalServerAdvertId,
                                UserId = externalServerUserId,
                                CreatedDate = command.CreatedDate,
                                RejectionReason = command.RejectionReason,
                                AdtoneServerAdvertRejectionId = advertRejection.AdvertRejectionId

                            };
                            db.AdvertRejection.Add(advertRejection2);
                            db.SaveChanges();
                        }
                        
                    }
                }

                return new CommandResult(true);
            }
            else
            {
                AdvertRejection advertRejection = _advertRejectionRepository.GetById(command.AdvertRejectionId);
                advertRejection.AdvertId = command.AdvertId;
                advertRejection.UserId = command.UserId;
                advertRejection.CreatedDate = command.CreatedDate;
                advertRejection.RejectionReason = command.RejectionReason;
                _advertRejectionRepository.Update(advertRejection);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);

                        var externalServerAdvertId = OperatorServer.GetAdvertIdFromOperatorServer(db, (int)command.AdvertId);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, (int)command.UserId);


                        var advertRejectionDetails = db.AdvertRejection.Where(s=>s.AdtoneServerAdvertRejectionId == command.AdvertRejectionId).FirstOrDefault();
                        if(advertRejectionDetails != null)
                        {
                            advertRejectionDetails.AdvertId = externalServerAdvertId;
                            advertRejectionDetails.UserId = externalServerUserId;
                            advertRejectionDetails.CreatedDate = command.CreatedDate;
                            advertRejectionDetails.RejectionReason = command.RejectionReason;
                            db.SaveChanges();
                        }                        
                    }
                }
                return new CommandResult(true);
            }       
            
        }

        #endregion
    }
}