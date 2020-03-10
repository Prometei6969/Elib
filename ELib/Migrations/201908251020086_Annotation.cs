namespace ELib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Annotation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PublicationModels", "Annotation", c => c.String(nullable: false));
            AlterColumn("dbo.PublicationModels", "Theme", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PublicationModels", "Theme", c => c.Int(nullable: false));
            DropColumn("dbo.PublicationModels", "Annotation");
        }
    }
}
