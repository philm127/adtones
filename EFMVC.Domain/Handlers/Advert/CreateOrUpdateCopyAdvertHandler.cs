using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Advert;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers.Advert
{
    public class CreateOrUpdateCopyAdvertHandler : ICommandHandler<CreateOrUpdateCopyAdvertCommand>
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
        /// Initializes a new instance of the <see cref="CreateOrUpdateCopyAdvertHandler" /> class.
        /// </summary>
        /// <param name="advertRepository">The advert repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCopyAdvertHandler(IAdvertRepository advertRepository, IUnitOfWork unitOfWork)
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
        public ICommandResult Execute(CreateOrUpdateCopyAdvertCommand command)
        {
            var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);

            Model.Advert advert = null;
            advert = _advertRepository.GetById(command.AdvertId);
            if (advert != null)
            {
                advert.UserId = command.UserId;
                advert.ClientId = command.AdvertClientId;
                advert.AdvertName = command.AdvertName;
                advert.AdvertDescription = command.AdvertDescription;
                advert.Brand = command.BrandName;
                advert.MediaFileLocation = command.MediaFileLocation;
                advert.UploadedToMediaServer = command.UploadedToMediaServer;
                advert.CreatedDateTime = command.CreatedDateTime;
                advert.UpdatedDateTime = command.UpdatedDateTime;
                advert.Status = command.Status;
                advert.Script = command.Script;
                advert.ScriptFileLocation = command.ScriptFileLocation;
                advert.IsAdminApproval = command.IsAdminApproval;
                advert.AdvertCategoryId = command.AdvertCategoryId;
                advert.CountryId = command.CountryId;
                advert.SoapToneId = command.SoapToneId;
                advert.PhoneticAlphabet = command.PhoneticAlphabet;
                advert.NextStatus = command.NextStatus;
                advert.OperatorId = command.OperatorId;
            }
            else
            {
                advert = new Model.Advert
                {
                    AdvertId = command.AdvertId,
                    UserId = command.UserId,
                    ClientId = command.AdvertClientId,
                    AdvertName = command.AdvertName,
                    AdvertDescription = command.AdvertDescription,
                    Brand = command.BrandName,
                    MediaFileLocation = command.MediaFileLocation,
                    UploadedToMediaServer = command.UploadedToMediaServer,
                    CreatedDateTime = command.CreatedDateTime,
                    UpdatedDateTime = command.UpdatedDateTime,
                    Status = command.Status,
                    Script = command.Script,
                    ScriptFileLocation = command.ScriptFileLocation,
                    IsAdminApproval = command.IsAdminApproval,
                    AdvertCategoryId = command.AdvertCategoryId,
                    CountryId = command.CountryId,
                    SoapToneId = command.SoapToneId,
                    PhoneticAlphabet = command.PhoneticAlphabet,
                    NextStatus = command.NextStatus,
                    OperatorId = command.OperatorId
                };
            }

            if (advert.AdvertId == 0)
            {
                _advertRepository.Add(advert);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);

                        int clientId = 0;
                        if (command.AdvertClientId == null)
                        {
                            clientId = 0;
                        }
                        else
                        {
                            clientId = command.AdvertClientId.Value;
                        }

                        var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, clientId);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                        var externalOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);
                        var externalAdvertCategoryId = OperatorServer.GetAdvertCategoryIdFromOperatorServer(db, command.AdvertCategoryId);
                        if (externalServerUserId != 0 && externalServerCountryId != 0 && externalOperatorId != 0 && externalAdvertCategoryId != 0)
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
                                UserId = externalServerUserId,
                                ClientId = operatorClientId,
                                AdvertName = command.AdvertName,
                                AdvertDescription = command.AdvertDescription,
                                Brand = command.BrandName,
                                MediaFileLocation = command.MediaFileLocation,
                                UploadedToMediaServer = command.UploadedToMediaServer,
                                CreatedDateTime = command.CreatedDateTime,
                                UpdatedDateTime = command.UpdatedDateTime,
                                Status = command.Status,
                                Script = command.Script,
                                ScriptFileLocation = command.ScriptFileLocation,
                                IsAdminApproval = command.IsAdminApproval,
                                AdvertCategoryId = externalAdvertCategoryId,
                                CountryId = externalServerCountryId,
                                SoapToneId = command.SoapToneId,
                                PhoneticAlphabet = command.PhoneticAlphabet,
                                NextStatus = command.NextStatus,
                                AdtoneServerAdvertId = advert.AdvertId,
                                OperatorId = externalOperatorId
                            };
                            db.Adverts.Add(advert2);
                            db.SaveChanges();
                        }

                        
                    }

                }
            }                
            else
            {
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
                            if (command.AdvertClientId == null)
                            {
                                clientId = 0;
                            }
                            else
                            {
                                clientId = command.AdvertClientId.Value;
                            }

                            var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, clientId);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                            var externalOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);
                            var externalAdvertCategoryId = OperatorServer.GetAdvertCategoryIdFromOperatorServer(db, command.AdvertCategoryId);
                            if (externalServerUserId != 0 && externalServerCountryId != 0 && externalOperatorId != 0 && externalAdvertCategoryId != 0) 
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

                                advertData.AdvertId = command.AdvertId;
                                advertData.UserId = externalServerUserId;
                                advertData.ClientId = operatorClientId;
                                advertData.AdvertName = command.AdvertName;
                                advertData.AdvertDescription = command.AdvertDescription;
                                advertData.Brand = command.BrandName;
                                advertData.MediaFileLocation = command.MediaFileLocation;
                                advertData.UploadedToMediaServer = command.UploadedToMediaServer;
                                advertData.CreatedDateTime = command.CreatedDateTime;
                                advertData.UpdatedDateTime = command.UpdatedDateTime;
                                advertData.Status = command.Status;
                                advertData.Script = command.Script;
                                advertData.ScriptFileLocation = command.ScriptFileLocation;
                                advertData.IsAdminApproval = command.IsAdminApproval;
                                advertData.AdvertCategoryId = externalAdvertCategoryId;
                                advertData.CountryId = externalServerCountryId;
                                advertData.SoapToneId = command.SoapToneId;
                                advertData.PhoneticAlphabet = command.PhoneticAlphabet;
                                advertData.NextStatus = command.NextStatus;
                                advertData.OperatorId = externalOperatorId;
                                db.SaveChanges();
                            }
                           
                        }
                    }
                }
            }

           

            return new CommandResult(true, advert.AdvertId);
        }

        #endregion
    }
}
