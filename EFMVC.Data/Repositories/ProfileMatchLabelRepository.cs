using EFMVC.Data.Infrastructure;
using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Interface IProfileMatchInformationRepository
    /// </summary>
    public interface IProfileMatchLabelRepository : IRepository<ProfileMatchLabel>
    {
    }
    public class ProfileMatchLabelRepository : RepositoryBase<ProfileMatchLabel>, IProfileMatchLabelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileMatchLabelRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public ProfileMatchLabelRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
