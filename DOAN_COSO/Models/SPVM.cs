using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN_COSO.Models
{
    public class SPVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hinh { get; set; }
        public double GiaBan { get; set; }
        public bool TinhTrang { get; set; }
        public string NameLoaiSanPham { get; set; }
        public int LoaiSanPhamId { get; set; }
        public bool Selected { get; set; }
    }
}