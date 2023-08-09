using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.Model;

namespace EFMVC.Data.Configurations
{
   public class QuestionImagesConfiguration : EntityTypeConfiguration<QuestionImages>
    {
        public QuestionImagesConfiguration()
        {
            ToTable("QuestionImages");
            Property(b => b.Id);
            Property(b => b.QuestionId);
            Property(b => b.UploadImage);
        }
    }
}
