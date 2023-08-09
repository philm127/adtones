using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Data.Infrastructure;
using EFMVC.Model;

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// Interface IUsersCreditPaymentRepository
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.UsersCreditPayment}" />
    public interface IUsersCreditPaymentRepository : IRepository<UsersCreditPayment>
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.RepositoryBase{EFMVC.Model.UsersCreditPayment}" />
    /// <seealso cref="EFMVC.Data.Repositories.IUsersCreditPaymentRepository" />
    public class UsersCreditPaymentRepository : RepositoryBase<UsersCreditPayment>, IUsersCreditPaymentRepository
    {
        public UsersCreditPaymentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
