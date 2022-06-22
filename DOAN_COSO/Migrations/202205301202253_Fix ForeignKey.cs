namespace DOAN_COSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChiTietHoaDons", "IdHoaDon", "dbo.HoaDons");
            DropForeignKey("dbo.ChiTietHoaDons", "IdSanPham", "dbo.SanPhams");
            AddForeignKey("dbo.ChiTietHoaDons", "IdHoaDon", "dbo.HoaDons", "IDHD");
            AddForeignKey("dbo.ChiTietHoaDons", "IdSanPham", "dbo.SanPhams", "IDSP");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChiTietHoaDons", "IdSanPham", "dbo.SanPhams");
            DropForeignKey("dbo.ChiTietHoaDons", "IdHoaDon", "dbo.HoaDons");
            AddForeignKey("dbo.ChiTietHoaDons", "IdSanPham", "dbo.SanPhams", "IDSP", cascadeDelete: true);
            AddForeignKey("dbo.ChiTietHoaDons", "IdHoaDon", "dbo.HoaDons", "IDHD", cascadeDelete: true);
        }
    }
}
