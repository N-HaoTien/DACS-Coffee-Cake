using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DOAN_COSO.Models;
namespace DOAN_COSO.Models
{
    public class LoaiSanPham : IValidatableObject
    {

        [Key]
        public int IDLoaiSP { get; set; }
        [Required]

        [StringLength(30)]
        [Index(IsUnique = true)]
        public string tenLoaiSP { get; set; }
        [StringLength(50)]
        public string Hinh { get; set; }

        public ICollection<SanPham> SanPhams { get; set; }
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var validateName = db.LoaiSanPhams.FirstOrDefault(x => x.tenLoaiSP == tenLoaiSP && x.IDLoaiSP != IDLoaiSP);
            if (validateName != null)
            {
                ValidationResult errorMessage = new ValidationResult("Category Product name already exists.", new[] { "tenLoaiSP" });
                yield return errorMessage;
            }
            else
            {
                yield return ValidationResult.Success;
            }

        }
    }
}