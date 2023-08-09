using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers
{
   public class ChangeAdvertStatusHandler : ICommandHandler<ChangeAdvertStatusCommand>
    {/// <summary>
     /// The unit of work
     /// </summary>
        private readonly IUnitOfWork unitOfWork;


        /// <summary>
        /// The advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        public ChangeAdvertStatusHandler(IAdvertRepository advertRepository, IUnitOfWork unitOfWork)
        {
            this._advertRepository = advertRepository;
            this.unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(ChangeAdvertStatusCommand command)
        {
            //Model.Advert advert = _advertRepository.GetById(command.AdvertId);
            var advertDetail = _advertRepository.GetById(command.AdvertId);
            advertDetail.Status = command.Status;
            advertDetail.UpdatedBy = command.UpdatedBy;
            _advertRepository.Update(advertDetail);
            var ConnString = ConnectionString.GetConnectionStringByCountryId(advertDetail.CountryId);
            if (ConnString != null && ConnString.Count() > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, (int)command.UpdatedBy);
                    var advertData = db.Adverts.Where(s => s.AdtoneServerAdvertId == command.AdvertId).FirstOrDefault();
                    if (advertData != null)
                    {
                        advertData.Status = command.Status;
                        if(externalServerUserId != 0)
                        {
                            advertData.UpdatedBy = command.UpdatedBy;
                        }
                        else
                        {
                            advertData.UpdatedBy = null;
                        }
                       
                        db.SaveChanges();
                    }
                }
            }
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
