namespace ELib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TokenInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FileModels", "Token", c => c.Int(nullable: false));
            AlterColumn("dbo.PublicationModels", "Token", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PublicationModels", "Token", c => c.String());
            AlterColumn("dbo.FileModels", "Token", c => c.String());
        }
    }
}
