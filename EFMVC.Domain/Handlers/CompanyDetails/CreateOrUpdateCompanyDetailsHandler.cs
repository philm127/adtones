using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.CompanyDetails;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;
using EFMVC.Domain.OperatorServerData;

namespace EFMVC.Domain.Handlers.CompanyDetails
{
    public class CreateOrUpdateCompanyDetailsHandler : ICommandHandler<CreateOrUpdateCompanyDetailsCommand>
    {
        /// <summary>
        /// The companydetails repository
        /// </summary>
        private readonly ICompanyDetailsRepository _companydetailsRepository;
        private readonly IContactsRepository _contactsRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateProfileHandler"/> class.
        /// </summary>
        /// <param name="companydetailsRepository">The blocked number repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>

        public CreateOrUpdateCompanyDetailsHandler(ICompanyDetailsRepository companydetailsRepository, IContactsRepository contactsRepository, IUnitOfWork unitOfWork)
        {
            _companydetailsRepository = companydetailsRepository;
            _contactsRepository = contactsRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCompanyDetailsCommand command)
        {
            var companyinfo = new Model.CompanyDetails
            {
                Id = command.Id,
                UserId = command.UserId,
                Address = command.Address,
                AdditionalAddress = command.AdditionalAddress,
                CompanyName = command.CompanyName,
                CountryId = command.CountryId,
                PostCode = command.PostCode,
                Town = command.Town
            };

            var contactData = _contactsRepository.GetMany(s => s.UserId == command.UserId).FirstOrDefault();
            if (contactData != null && contactData.CountryId != null)
            {
                var ConnString = ConnectionString.GetConnectionStringByCountryId(contactData.CountryId);
                if (companyinfo.Id == 0)
                {
                    _companydetailsRepository.Add(companyinfo);
                    _unitOfWork.Commit();
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex db = new EFMVCDataContex(item);
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                            if(externalServerUserId != 0 && externalServerCountryId != 0)
                            {
                                var companyinfo2 = new Model.CompanyDetails
                                {
                                    Id = command.Id,
                                    UserId = externalServerUserId,
                                    Address = command.Address,
                                    AdditionalAddress = command.AdditionalAddress,
                                    CompanyName = command.CompanyName,
                                    CountryId = externalServerCountryId,
                                    PostCode = command.PostCode,
                                    Town = command.Town,
                                    AdtoneServerCompanyDetailId = companyinfo.Id
                                };
                                db.CompanyDetails.Add(companyinfo2);
                                db.SaveChanges();
                            }
                            
                        }
                    }
                }
                else
                {
                    _companydetailsRepository.Update(companyinfo);
                    _unitOfWork.Commit();
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex db = new EFMVCDataContex(item);
                            var companyDetails = db.CompanyDetails.Where(s => s.AdtoneServerCompanyDetailId == command.Id).FirstOrDefault();
                            if (companyDetails != null)
                            {
                                var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                                var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                                if (externalServerUserId != 0 && externalServerCountryId != 0)
                                {
                                    companyDetails.UserId = externalServerUserId;
                                    companyDetails.Address = command.Address;
                                    companyDetails.AdditionalAddress = command.AdditionalAddress;
                                    companyDetails.CompanyName = command.CompanyName;
                                    companyDetails.CountryId = externalServerCountryId;
                                    companyDetails.PostCode = command.PostCode;
                                    companyDetails.Town = command.Town;
                                    db.SaveChanges();
                                }
                                   
                            }
                        }
                    }
                }
            }
         

            

            return new CommandResult(true);
        }
    }
}
