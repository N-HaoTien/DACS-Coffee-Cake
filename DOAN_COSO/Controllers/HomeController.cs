using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOAN_COSO.Models;
using Microsoft.AspNet.Identity;

namespace DOAN_COSO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        // Get Top Product by Count 
        public ActionResult BestSeller()
        {
            var query = db.ChiTietHoaDons.Include(n => n.SanPham)
                .GroupBy(p => p.SanPham)
                .Select(g => new SanPhamViewModel() { 
                    tenSP = g.Key.tenSP,
                    count = g.Sum(w => w.SoLuong),
                    Hinh = g.Key.Hinh,
                    GiaBan = g.Key.GiaBan,
                    LoaiSanPhamID = g.Key.LoaiSanPhamId,
                    TinhTrang = g.Key.TinhTrang
                })
                .OrderByDescending(p => p.count).ToList();
            return View(query);
        }
        [HttpGet]
        public ActionResult TrangChu(int? IdLoai,string searchString)
        {
            ViewBag.Keyword = searchString;
            ViewBag.listLoaiSP = db.LoaiSanPhams.ToList();
            ViewBag.IdLoai = IdLoai;
            if (string.IsNullOrEmpty(searchString) && IdLoai == null)
            {
                var a = db.SanPhams.ToList().Where(p => p.TinhTrang == true);

                return View(a);
            }
            else if(IdLoai == null && !string.IsNullOrEmpty(searchString))
            {
                var a = db.SanPhams.Where(p => p.TinhTrang == true && p.tenSP.Contains(searchString)).ToList();

                return View(a);
            }
            else if(IdLoai != null && string.IsNullOrEmpty(searchString))
            {
                var a = db.SanPhams.Where(p => p.LoaiSanPhamId == IdLoai && p.TinhTrang == true).ToList();
                return View(a);
            }
            else 
            {
                var a = db.SanPhams.Where(p => p.LoaiSanPhamId == IdLoai && p.tenSP.Contains(searchString) && p.TinhTrang == true).ToList();
                return View(a);
            }
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }
        [Authorize]
        public ActionResult DonHangKH(string UserId)
        {
            UserId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(c => c.Id == UserId);

            if(user != null)
            {
                var hd = db.HoaDons.Where(p => p.UserID == UserId).ToList();
                return View(hd);
            }

            return View();
        }
        [HttpGet]
        public ActionResult DetailsHD(int id)
        {
            var ct = db.ChiTietHoaDons.Include(p => p.SanPham).ToList().Where(p => p.IdHoaDon == id);
            return PartialView("DetailsHD", ct);
        }
    }
}