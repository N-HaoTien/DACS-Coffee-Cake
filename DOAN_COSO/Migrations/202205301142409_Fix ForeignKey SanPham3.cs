namespace DOAN_COSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixForeignKeySanPham3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SanPhams", "LoaiSanPhamId", "dbo.LoaiSanPhams");
            AddForeignKey("dbo.SanPhams", "LoaiSanPhamId", "dbo.LoaiSanPhams", "IDLoaiSP");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SanPhams", "LoaiSanPhamId", "dbo.LoaiSanPhams");
            AddForeignKey("dbo.SanPhams", "LoaiSanPhamId", "dbo.LoaiSanPhams", "IDLoaiSP", cascadeDelete: true);
        }
    }
}
