
namespace ELib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Surname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Surname");
        }
    }
}
