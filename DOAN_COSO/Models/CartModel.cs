using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN_COSO.Models
{
    public class CartModel
    {
        public SanPham SanPham { get; set; }

        public int Quantity { get; set; }

        public double ThanhTien => SanPham.GiaBan * Quantity ;
    }
}