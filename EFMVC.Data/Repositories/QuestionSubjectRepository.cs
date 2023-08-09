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
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.QuestionSubject}" />
    public interface IQuestionSubjectRepository : IRepository<QuestionSubject>
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.RepositoryBase{EFMVC.Model.QuestionSubject}" />
    /// <seealso cref="EFMVC.Data.Repositories.IQuestionSubjectRepository" />
    public class QuestionSubjectRepository : RepositoryBase<QuestionSubject>, IQuestionSubjectRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionSubjectRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public QuestionSubjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
