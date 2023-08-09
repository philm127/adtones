using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Data.Infrastructure;
using EFMVC.Model;

namespace EFMVC.Data.Repositories
{
  public  class PaymentMethodRepository : RepositoryBase<PaymentMethod>, IPaymentMethodRepository
    { 
        /// <summary>
       /// Initializes a new instance of the <see cref="ClientRepository"/> class.
       /// </summary>
       /// <param name="databaseFactory">The database factory.</param>
        public PaymentMethodRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    /// <summary>
    /// Interface IUserRepository
    /// </summary>
    public interface IPaymentMethodRepository : IRepository<PaymentMethod>
    {
    }
}
