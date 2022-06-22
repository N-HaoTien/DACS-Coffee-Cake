using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DOAN_COSO.Models
{
    public class SanPham
    {
        [Key]
        public int IDSP { get; set; }
        [StringLength(30)]
        [Required]
        [Index(IsUnique = true)]
        public string tenSP { get; set; }
        [StringLength(50)]

        public string Hinh { get; set; }
        public double GiaBan { get; set; }
        public bool TinhTrang { get; set; }
        public LoaiSanPham LoaiSanPham { get; set; }
        [Required]
        public int LoaiSanPhamId { get; set; }

        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }


    }
}