namespace ELib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Token : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileModels", "Token", c => c.String());
            AddColumn("dbo.PublicationModels", "Token", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PublicationModels", "Token");
            DropColumn("dbo.FileModels", "Token");
        }
    }
}
