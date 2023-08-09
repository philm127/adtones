using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using System.Linq;

namespace EFMVC.Domain.Handlers
{
    public class DeleteAdvertCategoryHandler : ICommandHandler<DeleteAdvertCategoryCommand>
    {
        /// <summary>
        /// The _advertCategory Repository
        /// </summary>
        private readonly IAdvertCategoryRepository _advertCategoryRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public DeleteAdvertCategoryHandler(IAdvertCategoryRepository advertCategoryRepository, IUnitOfWork unitOfWork)
        {
            _advertCategoryRepository = advertCategoryRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(DeleteAdvertCategoryCommand command)
        {
            Model.Entities.AdvertCategory advertCategoryInfo = _advertCategoryRepository.GetById(command.AdvertCategoryId);
            _advertCategoryRepository.Delete(advertCategoryInfo);
            var ConnString = ConnectionString.GetConnectionStringByOperatorId(advertCategoryInfo.CountryId);
            if (ConnString != null && ConnString.Count() > 0)
            {
                foreach (var item in ConnString)
                {
                    EFMVCDataContex db = new EFMVCDataContex(item);
                    var advertCategoryData = db.AdvertCategory.Where(s => s.AdtoneServerAdvertCategoryId == command.AdvertCategoryId).FirstOrDefault();
                    if(advertCategoryData != null)
                    {
                        db.AdvertCategory.Remove(advertCategoryData);
                        db.SaveChanges();
                    }
                }
            }

             _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
