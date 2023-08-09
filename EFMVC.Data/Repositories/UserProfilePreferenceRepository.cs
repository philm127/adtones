using EFMVC.Data.Infrastructure;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Data.Repositories
{
    public interface IUserProfilePreferenceRepository : IRepository<UserProfilePreference>
    {
    }
    public class UserProfilePreferenceRepository : RepositoryBase<UserProfilePreference>, IUserProfilePreferenceRepository
    {
        public UserProfilePreferenceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
