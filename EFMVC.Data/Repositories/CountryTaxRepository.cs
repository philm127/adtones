using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    public interface ICountryTaxRepository : IRepository<CountryTax>
    {
    }
    public class CountryTaxRepository : RepositoryBase<CountryTax>, ICountryTaxRepository
    {
        public CountryTaxRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
