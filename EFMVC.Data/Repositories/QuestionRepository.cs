using EFMVC.Data.Infrastructure;
using EFMVC.Model;

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.Question}" />
    public interface IQuestionRepository : IRepository<Question>
    {
    }
    /// <summary>
    /// QuestionRepository
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.RepositoryBase{EFMVC.Model.Question}" />
    /// <seealso cref="EFMVC.Data.Repositories.IQuestionRepository" />
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">The database factory.</param>
        public QuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
