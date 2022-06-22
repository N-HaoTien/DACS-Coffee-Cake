namespace DOAN_COSO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LamLaiDBdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiTietHoaDons",
                c => new
                    {
                        IdSanPham = c.Int(nullable: false),
                        IdHoaDon = c.Int(nullable: false),
                        SoLuong = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdSanPham, t.IdHoaDon })
                .ForeignKey("dbo.HoaDons", t => t.IdHoaDon, cascadeDelete: true)
                .ForeignKey("dbo.SanPhams", t => t.IdSanPham, cascadeDelete: true)
                .Index(t => t.IdSanPham)
                .Index(t => t.IdHoaDon);
            
            CreateTable(
                "dbo.HoaDons",
                c => new
                    {
                        IDHD = c.Int(nullable: false, identity: true),
                        UserID = c.String(nullable: false, maxLength: 128),
                        NgayDat = c.DateTime(nullable: false),
                        NgayGiao = c.DateTime(nullable: false),
                        TongTien = c.Double(nullable: false),
                        TinhTrangDH = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDHD)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 30),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SanPhams",
                c => new
                    {
                        IDSP = c.Int(nullable: false, identity: true),
                        tenSP = c.String(nullable: false, maxLength: 30),
                        Hinh = c.String(maxLength: 50),
                        GiaBan = c.Double(nullable: false),
                        TinhTrang = c.Boolean(nullable: false),
                        LoaiSanPhamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDSP)
                .ForeignKey("dbo.LoaiSanPhams", t => t.LoaiSanPhamId, cascadeDelete: true)
                .Index(t => t.tenSP, unique: true)
                .Index(t => t.LoaiSanPhamId);
            
            CreateTable(
                "dbo.LoaiSanPhams",
                c => new
                    {
                        IDLoaiSP = c.Int(nullable: false, identity: true),
                        tenLoaiSP = c.String(nullable: false, maxLength: 30),
                        Hinh = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.IDLoaiSP)
                .Index(t => t.tenLoaiSP, unique: true);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.SanPhams", "LoaiSanPhamId", "dbo.LoaiSanPhams");
            DropForeignKey("dbo.ChiTietHoaDons", "IdSanPham", "dbo.SanPhams");
            DropForeignKey("dbo.HoaDons", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChiTietHoaDons", "IdHoaDon", "dbo.HoaDons");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.LoaiSanPhams", new[] { "tenLoaiSP" });
            DropIndex("dbo.SanPhams", new[] { "LoaiSanPhamId" });
            DropIndex("dbo.SanPhams", new[] { "tenSP" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.HoaDons", new[] { "UserID" });
            DropIndex("dbo.ChiTietHoaDons", new[] { "IdHoaDon" });
            DropIndex("dbo.ChiTietHoaDons", new[] { "IdSanPham" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.LoaiSanPhams");
            DropTable("dbo.SanPhams");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.HoaDons");
            DropTable("dbo.ChiTietHoaDons");
        }
    }
}
