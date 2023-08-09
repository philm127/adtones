using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Data.Infrastructure;
using EFMVC.Model.Entities;

namespace EFMVC.Data.Repositories
{
    public interface ICurrencyRateRepository : IRepository<CurrencyRate>
    {
    }
    public class CurrencyRateRepository : RepositoryBase<CurrencyRate>, ICurrencyRateRepository
    {
            public CurrencyRateRepository(IDatabaseFactory databaseFactory)
                : base(databaseFactory)
            {
            }
        }
    }
