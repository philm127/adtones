using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers
{
    public class CreateOrUpdateAdvertCategoryHandler : ICommandHandler<CreateOrUpdateAdvertCategoryCommand>
    {

        /// <summary>
        /// The _advertCategory Repository
        /// </summary>
        private readonly IAdvertCategoryRepository _advertCategoryRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateAdvertCategoryHandler"/> class.
        /// </summary>
        /// <param name="advertCategoryRepository">The advertCategory Repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateAdvertCategoryHandler(IAdvertCategoryRepository advertCategoryRepository, IUnitOfWork unitOfWork)
        {
            _advertCategoryRepository = advertCategoryRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateAdvertCategoryCommand command)
        {
            var advertCategory = new Model.Entities.AdvertCategory
            {
                AdvertCategoryId = command.AdvertCategoryId,
                Name = command.Name,
                CountryId = command.CountryId,
                CreatedDate = command.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);
            if (advertCategory.AdvertCategoryId == 0)
            {
                _advertCategoryRepository.Add(advertCategory);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);

                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId.Value);

                        if (externalServerCountryId != 0)
                        {
                            var advertCategory2 = new Model.Entities.AdvertCategory
                            {
                                AdvertCategoryId = command.AdvertCategoryId,
                                Name = command.Name,
                                CountryId = externalServerCountryId,
                                CreatedDate = command.CreatedDate,
                                UpdatedDate = DateTime.Now,
                                AdtoneServerAdvertCategoryId = advertCategory.AdvertCategoryId
                            };
                            db.AdvertCategory.Add(advertCategory2);
                            db.SaveChanges();
                        }

                    }
                }

            }
            else
            {
                _advertCategoryRepository.Update(advertCategory);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var advertCategoryData = db.AdvertCategory.Where(s => s.AdtoneServerAdvertCategoryId == command.AdvertCategoryId).FirstOrDefault();
                        if (advertCategoryData != null)
                        {
                            var externalServerCountryId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.CountryId.Value);
                            if (externalServerCountryId != 0)
                            {
                                advertCategoryData.AdvertCategoryId = command.AdvertCategoryId;
                                advertCategoryData.Name = command.Name;
                                advertCategoryData.UpdatedDate = DateTime.Now;
                                advertCategoryData.CountryId = externalServerCountryId;
                                db.SaveChanges();
                            }
                        }

                    }
                }
            }




            return new CommandResult(true);
        }
    }
}
