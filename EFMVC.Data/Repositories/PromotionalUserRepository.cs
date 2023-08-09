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
    public interface IPromotionalUserRepository : IRepository<PromotionalUser>
    {
    }
    public class PromotionalUserRepository : RepositoryBase<PromotionalUser>, IPromotionalUserRepository
    {
        public PromotionalUserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
