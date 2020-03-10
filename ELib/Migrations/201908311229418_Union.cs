namespace ELib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Union : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PublicationModels", "Path", c => c.String());
            AddColumn("dbo.PublicationModels", "FileName", c => c.String());
            DropTable("dbo.FileModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FileModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                        Nickname = c.String(),
                        Token = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.PublicationModels", "FileName");
            DropColumn("dbo.PublicationModels", "Path");
        }
    }
}
