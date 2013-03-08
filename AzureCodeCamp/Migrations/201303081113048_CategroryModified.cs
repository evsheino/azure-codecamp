namespace AzureCodeCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategroryModified : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JoukkoVideos", "Category_ID", "dbo.Categories");
            DropIndex("dbo.JoukkoVideos", new[] { "Category_ID" });
            AlterColumn("dbo.JoukkoVideos", "category_ID", c => c.Int(nullable: false));
            AddForeignKey("dbo.JoukkoVideos", "category_ID", "dbo.Categories", "ID", cascadeDelete: true);
            CreateIndex("dbo.JoukkoVideos", "category_ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JoukkoVideos", new[] { "category_ID" });
            DropForeignKey("dbo.JoukkoVideos", "category_ID", "dbo.Categories");
            AlterColumn("dbo.JoukkoVideos", "Category_ID", c => c.Int());
            CreateIndex("dbo.JoukkoVideos", "Category_ID");
            AddForeignKey("dbo.JoukkoVideos", "Category_ID", "dbo.Categories", "ID");
        }
    }
}
