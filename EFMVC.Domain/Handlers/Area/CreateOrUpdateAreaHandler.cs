using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers
{
    public class CreateOrUpdateAreaHandler : ICommandHandler<CreateOrUpdateAreaCommand>
    {

        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly IAreaRepository _areaRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateAreaHandler"/> class.
        /// </summary>
        /// <param name="AreaRepository">The country repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateAreaHandler(IAreaRepository areaRepository, IUnitOfWork unitOfWork)
        {
            _areaRepository = areaRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateAreaCommand command)
        {
            var Areas = new Model.Entities.Area
            {
                AreaId = command.AreaId,
                AreaName = command.AreaName,   
                CountryId = command.CountryId,
                IsActive = command.IsActive
            };

            var ConnString = ConnectionString.GetConnectionString(command.CountryId);

            if (Areas.AreaId == 0)
            {
                _areaRepository.Add(Areas);

                if (!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    var Areas2 = new Model.Entities.Area
                    {
                        AreaId = command.AreaId,
                        AreaName = command.AreaName,
                        CountryId = command.CountryId,
                        IsActive = command.IsActive
                    };
                    db.Area.Add(Areas2);
                    db.SaveChanges();
                }
            }
            else
            {
                _areaRepository.Update(Areas);

                if(!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    var areaInfo = db.Area.Where(s => s.AreaId == command.AreaId).FirstOrDefault();
                    if(areaInfo != null)
                    {
                        areaInfo.AreaName = command.AreaName;
                        areaInfo.CountryId = command.CountryId;
                        areaInfo.IsActive = command.IsActive;
                        db.SaveChanges();
                    }
                }
            }

            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
