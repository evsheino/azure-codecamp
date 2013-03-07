namespace AzureCodeCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.user_UserId, cascadeDelete: true)
                .Index(t => t.user_UserId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.JoukkoVideos", new[] { "user_UserId" });
            DropForeignKey("dbo.JoukkoVideos", "user_UserId", "dbo.UserProfile");
            DropTable("dbo.UserProfile");
            DropTable("dbo.JoukkoVideos");
        }
    }
}
