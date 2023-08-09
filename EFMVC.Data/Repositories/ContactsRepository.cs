using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Data.Infrastructure;
using EFMVC.Model;


namespace EFMVC.Data.Repositories
{
    public interface IContactsRepository : IRepository<Contacts>
    {
    }
    public class ContactsRepository : RepositoryBase<Contacts>, IContactsRepository
    {
        public ContactsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
