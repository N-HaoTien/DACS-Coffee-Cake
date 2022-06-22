using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DOAN_COSO.Models
{
    public class HoaDon
    {
        [Key]
        public int IDHD { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string UserID { get; set; }

        public DateTime NgayDat { get; set; }
        public DateTime NgayGiao { get; set; }

        public double TongTien { get; set; }

        public bool IsPaid { get; set; }

        public bool TinhTrangDH { get; set; }

        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    }
}