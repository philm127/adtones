using System.Data.Entity.ModelConfiguration;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
    public class QuestionCommentImagesConfiguration : EntityTypeConfiguration<QuestionCommentImages>
    {
        public QuestionCommentImagesConfiguration()
        {
            ToTable("QuestionCommentImages");
            Property(b => b.Id);
            Property(b => b.QuestionCommentId);
            Property(b => b.UploadImages);
        }
    }
}
