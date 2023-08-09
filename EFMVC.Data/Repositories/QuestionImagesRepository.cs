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
    /// IQuestionImagesRepository
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.IRepository{EFMVC.Model.QuestionImages}" />
    public interface IQuestionImagesRepository : IRepository<QuestionImages>
    {
    }
    /// <summary>
    /// QuestionImagesRepository
    /// </summary>
    /// <seealso cref="EFMVC.Data.Infrastructure.RepositoryBase{EFMVC.Model.QuestionImages}" />
    /// <seealso cref="EFMVC.Data.Repositories.IQuestionImagesRepository" />
    public class QuestionImagesRepository : RepositoryBase<QuestionImages>, IQuestionImagesRepository
    {
        public QuestionImagesRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
