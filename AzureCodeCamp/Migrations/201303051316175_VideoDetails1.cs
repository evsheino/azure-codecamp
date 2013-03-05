namespace AzureCodeCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoDetails1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            AddColumn("dbo.JoukkoVideos", "timestamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.JoukkoVideos", "user_UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.JoukkoVideos", "path", c => c.String(nullable: false));
            AddForeignKey("dbo.JoukkoVideos", "user_UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            CreateIndex("dbo.JoukkoVideos", "user_UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JoukkoVideos", new[] { "user_UserId" });
            DropForeignKey("dbo.JoukkoVideos", "user_UserId", "dbo.UserProfile");
            AlterColumn("dbo.JoukkoVideos", "path", c => c.String());
            DropColumn("dbo.JoukkoVideos", "user_UserId");
            DropColumn("dbo.JoukkoVideos", "timestamp");
            DropTable("dbo.UserProfile");
        }
    }
}
