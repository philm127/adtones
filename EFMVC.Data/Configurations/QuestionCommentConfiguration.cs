using System.Data.Entity.ModelConfiguration;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
    public class QuestionCommentConfiguration : EntityTypeConfiguration<QuestionComment>
    {
        public QuestionCommentConfiguration()
        {
            ToTable("QuestionComment");
            Property(b => b.Id);
            Property(b => b.UserId);
            Property(b => b.QuestionId);
            Property(b => b.Title);
            Property(b => b.Description);
            Property(b => b.ResponseDatetime);
        }
    }
}
