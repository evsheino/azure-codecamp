namespace AzureCodeCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JoukkoVideos", "title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JoukkoVideos", "title");
        }
    }
}
