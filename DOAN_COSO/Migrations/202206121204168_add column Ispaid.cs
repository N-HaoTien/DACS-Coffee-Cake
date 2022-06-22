namespace DOAN_COSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumnIspaid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "IsPaid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "IsPaid");
        }
    }
}
