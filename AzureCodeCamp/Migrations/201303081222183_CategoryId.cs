namespace AzureCodeCamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.JoukkoVideos", name: "user_UserId", newName: "userId");
            RenameColumn(table: "dbo.JoukkoVideos", name: "category_ID", newName: "categoryId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.JoukkoVideos", name: "categoryId", newName: "category_ID");
            RenameColumn(table: "dbo.JoukkoVideos", name: "userId", newName: "user_UserId");
        }
    }
}
