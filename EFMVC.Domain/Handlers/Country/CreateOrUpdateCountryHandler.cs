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
    public class CreateOrUpdateCountryHandler : ICommandHandler<CreateOrUpdateCountryCommand>
    {

        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCountryHandler"/> class.
        /// </summary>
        /// <param name="countryRepository">The country repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCountryHandler(ICountryRepository countryRepository, IUnitOfWork unitOfWork)
        {
            _countryRepository = countryRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateCountryCommand command)
        {
            var country = new Model.Country
            {
                Id = command.Id,
                UserId = command.UserId,
                Name = command.Name,
                ShortName = command.ShortName,
                CountryCode = command.CountryCode,
                CreatedDate = command.CreatedDate,
                UpdatedDate = DateTime.Now,
                Status = 1,
                TermAndConditionFileName = command.TermAndConditionFileName
            };

            if (country.Id == 0)
            {
                _countryRepository.Add(country);
                _unitOfWork.Commit();

                var ConnString = ConnectionString.GetAllConnectionString();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var country2 = new Model.Country
                        {
                            Id = command.Id,
                            UserId = command.UserId,
                            Name = command.Name,
                            ShortName = command.ShortName,
                            CountryCode = command.CountryCode,
                            CreatedDate = command.CreatedDate,
                            UpdatedDate = DateTime.Now,
                            Status = 1,
                            TermAndConditionFileName = command.TermAndConditionFileName,
                            AdtoneServeCountryId = country.Id
                        };
                        db.Country.Add(country2);
                        db.SaveChanges();
                    }
                   
                }
              
            }                
            else
            {
                _countryRepository.Update(country);
                _unitOfWork.Commit();
                var ConnString = ConnectionString.GetAllConnectionString();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var countryData = db.Country.Where(s => s.Id == country.Id).FirstOrDefault();
                        if(countryData != null)
                        {
                            countryData.UserId = command.UserId;
                            countryData.Name = command.Name;
                            countryData.ShortName = command.ShortName;
                            countryData.CountryCode = command.CountryCode;
                            countryData.CreatedDate = command.CreatedDate;
                            countryData.UpdatedDate = DateTime.Now;
                            countryData.Status = 1;
                            countryData.TermAndConditionFileName = command.TermAndConditionFileName;
                            countryData.AdtoneServeCountryId = country.Id;
                            db.SaveChanges();
                        }
                    }
                }
            }
                

            return new CommandResult(true);
        }
    }
}
