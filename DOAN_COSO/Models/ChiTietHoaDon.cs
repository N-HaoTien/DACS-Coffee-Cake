using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DOAN_COSO.Models
{
    public class ChiTietHoaDon
    {
        [Key]
        [Column(Order = 1)]
        public int IdSanPham { get; set; }
        [ForeignKey(nameof(IdSanPham))]
        public virtual SanPham SanPham { get; set; }
        [Key]
        [Column(Order = 2)]
        public int IdHoaDon { get; set; }
        [ForeignKey(nameof(IdHoaDon))]
        public virtual HoaDon HoaDon { get; set; }

        public int SoLuong { get; set; }

    }
}