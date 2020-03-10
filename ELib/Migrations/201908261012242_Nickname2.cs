namespace ELib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nickname2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PublicationModels", "Nickname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PublicationModels", "Nickname");
        }
    }
}
