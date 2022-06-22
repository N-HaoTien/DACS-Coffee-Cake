namespace DOAN_COSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddcolumnAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Address", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Address");
        }
    }
}
