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
    public interface ICopyRightRepository : IRepository<CopyRight>
    {
    }
    public class CopyRightRepository : RepositoryBase<CopyRight>, ICopyRightRepository
    {
        public CopyRightRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
