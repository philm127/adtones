using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    public interface IPromotionalAdvertRepository : IRepository<PromotionalAdvert>
    {
    }
    public class PromotionalAdvertRepository : RepositoryBase<PromotionalAdvert>, IPromotionalAdvertRepository
    {
        public PromotionalAdvertRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
