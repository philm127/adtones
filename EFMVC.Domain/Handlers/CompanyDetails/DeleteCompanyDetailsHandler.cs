using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.CompanyDetails;

namespace EFMVC.Domain.Handlers.CompanyDetails
{
    public class DeleteCompanyDetailsHandler : ICommandHandler<DeleteCampanyDetailsCommand>
    {
        /// <summary>
        /// The companydetails repository
        /// </summary>
        private readonly ICompanyDetailsRepository _companydetailsRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBlockedNumberHandler"/> class.
        /// </summary>
        /// <param name="contactRepository">The blocked number repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteCompanyDetailsHandler(ICompanyDetailsRepository companydetailsRepository, IUnitOfWork unitOfWork)
        {
            _companydetailsRepository = companydetailsRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteCampanyDetailsCommand command)
        {
            Model.CompanyDetails companyinfo = _companydetailsRepository.GetById(command.Id);
            _companydetailsRepository.Delete(companyinfo);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
