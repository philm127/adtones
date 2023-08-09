using EFMVC.Data.Infrastructure;
using EFMVC.Model;

namespace EFMVC.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.QuestionComment}" />
    public interface IQuestionCommentRepository : IRepository<QuestionComment>
    {
    }
    /// <summary>
    /// QuestionCommentRepository
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.RepositoryBase{EFMVC.Model.QuestionComment}" />
    /// <seealso cref="EFMVC.Data.Repositories.IQuestionCommentRepository" />
    public class QuestionCommentRepository : RepositoryBase<QuestionComment>, IQuestionCommentRepository
    {
        public QuestionCommentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
