using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Data.Infrastructure;
using EFMVC.Model;


namespace EFMVC.Data.Repositories
{
    public interface ICompanyDetailsRepository : IRepository<CompanyDetails>
    {
    }
    public class CompanyDetailsRepository : RepositoryBase<CompanyDetails>, ICompanyDetailsRepository
    {
        public CompanyDetailsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
