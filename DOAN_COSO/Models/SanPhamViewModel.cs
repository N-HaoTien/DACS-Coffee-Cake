using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DOAN_COSO.Models
{
    public class SanPhamViewModel : IValidatableObject
    {
        public int IDSP { get; set; }
        [Required]
        public string tenSP { get; set; }
        public string Hinh { get; set; }
        public int SoLuong { get; set; }
        public string TenLoaiSP { get; set; }

        public double GiaBan { get; set; }
        public bool TinhTrang { get; set; }
        public int LoaiSanPhamID { get; set; }

        public int count { get; set; }

        public IEnumerable<LoaiSanPham> LoaiSanPhams { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var validateName = db.SanPhams.FirstOrDefault(x => x.tenSP == tenSP && x.IDSP != IDSP);
            if (validateName != null)
            {
                ValidationResult errorMessage = new ValidationResult("Product name already exists.", new[] { "tenSP" });
                yield return errorMessage;
            }
            else
            {
                yield return ValidationResult.Success;
            }

        }
    }
}