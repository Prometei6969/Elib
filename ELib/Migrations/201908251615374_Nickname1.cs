namespace ELib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nickname1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileModels", "Nickname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileModels", "Nickname");
        }
    }
}
