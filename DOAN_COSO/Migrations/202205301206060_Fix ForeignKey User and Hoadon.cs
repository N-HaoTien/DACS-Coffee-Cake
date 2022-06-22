namespace DOAN_COSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixForeignKeyUserandHoadon : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HoaDons", "UserID", "dbo.AspNetUsers");
            AddForeignKey("dbo.HoaDons", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HoaDons", "UserID", "dbo.AspNetUsers");
            AddForeignKey("dbo.HoaDons", "UserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
