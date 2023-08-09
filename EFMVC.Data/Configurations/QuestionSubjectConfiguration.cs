using System.Data.Entity.ModelConfiguration;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
    public class QuestionSubjectConfiguration : EntityTypeConfiguration<QuestionSubject>
    {
        public QuestionSubjectConfiguration()
        {
            ToTable("QuestionSubject");
            Property(a => a.SubjectId).IsRequired();
            Property(a => a.Name);
            Property(a => a.CreatedDate);
            Property(a => a.UpdatedDate);
        }
    }
}
