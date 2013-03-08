namespace AzureCodeCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Category : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JoukkoVideos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false),
                        path = c.String(nullable: false),
                        timestamp = c.DateTime(nullable: false),
                        user_UserId = c.Int(nullable: false),
                        Category_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.user_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_ID)
                .Index(t => t.user_UserId)
                .Index(t => t.Category_ID);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.JoukkoVideos", new[] { "Category_ID" });
            DropIndex("dbo.JoukkoVideos", new[] { "user_UserId" });
            DropForeignKey("dbo.JoukkoVideos", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.JoukkoVideos", "user_UserId", "dbo.UserProfile");
            DropTable("dbo.Categories");
            DropTable("dbo.UserProfile");
            DropTable("dbo.JoukkoVideos");
        }
    }
}
