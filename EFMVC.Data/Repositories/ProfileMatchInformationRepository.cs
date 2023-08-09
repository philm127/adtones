using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using EFMVC.Model.Entities;

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Interface IProfileMatchInformationRepository
    /// </summary>
    public interface IProfileMatchInformationRepository : IRepository<ProfileMatchInformation>
    {
    }
    public  class ProfileMatchInformationRepository : RepositoryBase<ProfileMatchInformation>, IProfileMatchInformationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileMatchInformationRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public ProfileMatchInformationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
