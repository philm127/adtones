using System.Data.Entity.ModelConfiguration;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
    public class QuestionConfiguration : EntityTypeConfiguration<Question>
    {
        public QuestionConfiguration()
        {
            ToTable("Question");
            Property(b => b.Id);
            Property(b => b.UserId);
            Property(b => b.QNumber);
            Property(b => b.SubjectId);
            Property(b => b.ClientId);
            Property(b => b.CampaignProfileId);
            Property(b => b.PaymentMethodId);
            Property(b => b.Title);
            Property(b => b.Description);
            Property(b => b.CreatedDate);
            Property(b => b.UpdatedDate);
            Property(b => b.LastResponseDateTime);
            Property(b => b.LastResponseDateTimeByUser);
            Property(b => b.Status);
        }
    }
}
