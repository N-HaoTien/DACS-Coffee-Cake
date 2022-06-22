using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DOAN_COSO.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [StringLength(30)]
        [Required]
        public string Name { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {

        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SanPham>()
            .HasRequired<LoaiSanPham>(s => s.LoaiSanPham)
            .WithMany(g => g.SanPhams)
            .HasForeignKey<int>(s => s.LoaiSanPhamId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<SanPham>()
                   .HasMany(e => e.ChiTietHoaDons)
                   .WithRequired(e => e.SanPham)
                   .WillCascadeOnDelete(false);
            modelBuilder.Entity<HoaDon>()
               .HasMany(e => e.ChiTietHoaDons)
               .WithRequired(e => e.HoaDon)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<HoaDon>()
    .HasRequired<ApplicationUser>(s => s.User)
    .WithMany(g => g.HoaDons)
    .WillCascadeOnDelete(false);
            

            base.OnModelCreating(modelBuilder);

           
        }
    }
}
