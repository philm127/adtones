// <auto-generated />
namespace EFMVC.Data.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.2-31219")]
    public sealed partial class RemoveBucketBatchRelationFromAllBucketTables : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(RemoveBucketBatchRelationFromAllBucketTables));
        
        string IMigrationMetadata.Id
        {
            get { return "201810250935089_Remove Bucket Batch Relation From All Bucket Tables"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
