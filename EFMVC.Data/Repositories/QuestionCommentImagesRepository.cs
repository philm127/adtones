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
    /// IQuestionCommentImagesRepository
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.QuestionCommentImages}" />
    public interface IQuestionCommentImagesRepository : IRepository<QuestionCommentImages>
    {
    }
    /// <summary>
    /// QuestionCommentImagesRepository
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.RepositoryBase{EFMVC.Model.QuestionCommentImages}" />
    /// <seealso cref="EFMVC.Data.Repositories.IQuestionCommentImagesRepository" />
    public class QuestionCommentImagesRepository : RepositoryBase<QuestionCommentImages>, IQuestionCommentImagesRepository
    {
        public QuestionCommentImagesRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
