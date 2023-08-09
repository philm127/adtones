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
    public class CreateOrUpdateAdvertHandler : ICommandHandler<CreateOrUpdateAdvertCommand>
    {
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateAdvertHandler" /> class.
        /// </summary>
        /// <param name="advertRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateAdvertHandler(IAdvertRepository advertRepository, IUnitOfWork unitOfWork)
        {
            _advertRepository = advertRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateAdvertCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateAdvertCommand command)
        {
            var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);
            
            if (command.AdvertId == 0)
            {
                var advert = new Model.Advert
                                 {
                                     AdvertId = command.AdvertId,
                                     ClientId = command.ClientId,
                                     Script=command.Script,
                                     ScriptFileLocation=command.ScriptFileLocation,
                                     AdvertDescription = command.AdvertDescription,
                                     AdvertName = command.AdvertName,
                                     Brand = command.Brand,
                                     CreatedDateTime = command.CreatedDateTime,
                                     MediaFileLocation = command.MediaFileLocation,
                                     UploadedToMediaServer = command.UploadedToMediaServer,
                                     UpdatedDateTime = command.UpdatedDateTime,
                                     UserId = command.UserId,
                                     Status=command.Status,
                                     AdvertCategoryId = command.AdvertCategoryId,
                                     CountryId = command.CountryId,
                                     NextStatus = false,
                                     CampProfileId = command.CampaignProfileId,
                                     OperatorId = command.OperatorId
                                 };

                _advertRepository.Add(advert);
                _unitOfWork.Commit();
              
                if (ConnString != null && ConnString.Count() > 0)
                {                   
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);

                        int clientId = 0;
                        if (command.ClientId == null)
                        {
                            clientId = 0;
                        }
                        else
                        {
                            clientId = command.ClientId.Value;
                        }

                        var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, clientId);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                        var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, (int)command.CampaignProfileId);
                        var externalOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, (int)command.OperatorId);
                        var externalAdvertCategoryId = OperatorServer.GetAdvertCategoryIdFromOperatorServer(db, (int)command.AdvertCategoryId);
                        if (externalServerUserId != 0 && externalServerCountryId != 0 && externalServerCampaignProfileId != 0 && externalOperatorId != 0 && externalAdvertCategoryId != 0)//externalServerClientId != 0 && 
                        {
                            int? operatorClientId;
                            if (externalServerClientId == 0)
                            {
                                operatorClientId = null;
                            }
                            else
                            {
                                operatorClientId = externalServerClientId;
                            }
                            var advert2 = new Model.Advert
                            {
                                AdvertId = command.AdvertId,
                                ClientId = operatorClientId,
                                Script = command.Script,
                                ScriptFileLocation = command.ScriptFileLocation,
                                AdvertDescription = command.AdvertDescription,
                                AdvertName = command.AdvertName,
                                Brand = command.Brand,
                                CreatedDateTime = command.CreatedDateTime,
                                MediaFileLocation = command.MediaFileLocation,
                                UploadedToMediaServer = command.UploadedToMediaServer,
                                UpdatedDateTime = command.UpdatedDateTime,
                                UserId = externalServerUserId,
                                Status = command.Status,
                                AdvertCategoryId = externalAdvertCategoryId,
                                CountryId = externalServerCountryId,
                                NextStatus = false,
                                CampProfileId = externalServerCampaignProfileId,
                                OperatorId = externalOperatorId,
                                AdtoneServerAdvertId = advert.AdvertId
                            };

                            db.Adverts.Add(advert2);
                            db.SaveChanges();
                        }                      
                                            
                       
                    }

                }
                
                return new CommandResult(true,advert.AdvertId);
            }
            else
            {
                //Commented 22-02-2019
                //Model.Advert advert = _advertRepository.GetById(command.AdvertId);
                //advert.AdvertDescription = command.AdvertDescription;
                //advert.AdvertName = command.AdvertName;
                //advert.Brand = command.Brand;
                //advert.Script = command.Script;
                //advert.ScriptFileLocation = command.ScriptFileLocation;
                //advert.CreatedDateTime = advert.CreatedDateTime;
                //advert.UploadedToMediaServer = command.UploadedToMediaServer;
                //advert.UpdatedDateTime = command.UpdatedDateTime;
                //advert.UserId = command.UserId;
                //advert.ClientId = command.ClientId;
                //advert.Status = command.Status;
                //advert.AdvertCategoryId = command.AdvertCategoryId;
                //if (!string.IsNullOrEmpty(command.MediaFileLocation))
                //{
                //    advert.MediaFileLocation = command.MediaFileLocation;
                //}
                ////advert.CountryId = command.CountryId;
                //advert.CountryId = advert.CountryId;
                //advert.NextStatus = false;
                //_advertRepository.Update(advert);
                //_unitOfWork.Commit();
                //return new CommandResult(true);

                //Add 22-02-2019
                Model.Advert advert = _advertRepository.GetById(command.AdvertId);
                advert.AdvertDescription = command.AdvertDescription;
                advert.AdvertName = command.AdvertName;
                advert.Brand = command.Brand;
                advert.Script = command.Script;
                if (!string.IsNullOrEmpty(command.ScriptFileLocation))
                {
                    advert.ScriptFileLocation = command.ScriptFileLocation;
                }
                advert.CreatedDateTime = advert.CreatedDateTime;
                advert.UploadedToMediaServer = command.UploadedToMediaServer;
                advert.UpdatedDateTime = command.UpdatedDateTime;
                advert.UserId = command.UserId;
                advert.ClientId = command.ClientId;
                advert.Status = command.Status;
                advert.AdvertCategoryId = command.AdvertCategoryId;
                if (!string.IsNullOrEmpty(command.MediaFileLocation))
                {
                    advert.MediaFileLocation = command.MediaFileLocation;
                }
                advert.CountryId = command.CountryId;
                //advert.CountryId = advert.CountryId;
                advert.NextStatus = false;
                advert.CampProfileId = command.CampaignProfileId;
                advert.OperatorId = command.OperatorId;
                _advertRepository.Update(advert);
                _unitOfWork.Commit();

                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var advertData = db.Adverts.Where(s => s.AdtoneServerAdvertId == command.AdvertId).FirstOrDefault();
                        
                        if (advertData != null)
                        {
                            int clientId = 0;
                            if (command.ClientId == null)
                            {
                                clientId = 0;
                            }
                            else
                            {
                                clientId = (int)command.ClientId;
                            }

                            var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, clientId);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, (int)advert.CampaignAdverts.FirstOrDefault().CampaignProfileId);
                            var externalOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, (int)command.OperatorId);
                            var externalAdvertCategoryId = OperatorServer.GetAdvertCategoryIdFromOperatorServer(db, (int)command.AdvertCategoryId);
                            if (externalServerUserId != 0 && externalServerCountryId != 0 && externalServerCampaignProfileId != 0 && externalOperatorId != 0 && externalAdvertCategoryId != 0)//externalServerClientId != 0 && 
                            {
                                int? operatorClientId;
                                if (externalServerClientId == 0)
                                {
                                    operatorClientId = null;
                                }
                                else
                                {
                                    operatorClientId = externalServerClientId;
                                }
                                advertData.AdvertDescription = command.AdvertDescription;
                                advertData.AdvertName = command.AdvertName;
                                advertData.Brand = command.Brand;
                                advertData.Script = command.Script;                              
                                advertData.ScriptFileLocation = advert.ScriptFileLocation;
                                advertData.CreatedDateTime = advert.CreatedDateTime;
                                advertData.UploadedToMediaServer = advert.UploadedToMediaServer;
                                advertData.UpdatedDateTime = command.UpdatedDateTime;
                                advertData.UserId = externalServerUserId;
                                advertData.UpdatedBy = externalServerUserId;
                                advertData.ClientId = operatorClientId;
                                advertData.Status = command.Status;
                                advertData.AdvertCategoryId = externalAdvertCategoryId;                               
                                advertData.MediaFileLocation = advert.MediaFileLocation;                                
                                advertData.CountryId = externalServerCountryId;
                                advertData.OperatorId = externalOperatorId;
                                advertData.NextStatus = false;
                                advertData.CampProfileId = externalServerCampaignProfileId;
                                db.SaveChanges();
                            }
                            
                        }
                    }
                }
                return new CommandResult(true);

            }

        
            
        }

        #endregion
    }
}