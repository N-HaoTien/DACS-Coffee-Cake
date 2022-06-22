using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using DOAN_COSO.Models;
using System.Data.Entity;
using System.Net;

namespace DOAN_COSO.Controllers
{
    [Authorize(Roles = "Admin,Clerk")]
    public class AdminController : Controller
    {
        ApplicationDbContext db;
        public AdminController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Admin
        public ActionResult ThongKe()
        {
            return View();
        }
        public ActionResult TongTien()
        {
            var query = db.HoaDons
                                        .GroupBy(n => new
                                        {
                                            a = n.NgayDat.Month
                                        })
                                     .Select(s => new { months = s.Key.a, count = s.Sum(w => w.TongTien) });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TongSL()
        {
            var query = db.ChiTietHoaDons.Include(n => n.SanPham)
                .GroupBy(p => p.SanPham.tenSP)
                .Select(g => new { name = g.Key, count = g.Sum(w => w.SoLuong) }).OrderBy(p => p.count).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string searchstring)
        {
            ViewBag.KeyWord = searchstring;
            ViewBag.searchstring = searchstring;
            var books = db.SanPhams.Include(n => n.LoaiSanPham);
            if (!String.IsNullOrEmpty(searchstring))
            {
                searchstring = searchstring.ToLower();
                books = books.Where(b => b.tenSP.ToLower().Contains(searchstring));
            }

            return View(books.ToList());
        }
        IEnumerable<SanPham> GetAll()
        {
            return db.SanPhams.ToList();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ListSanPham(int? page, string searchString, int? LoaiSanPhamId)
        {
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNum = page ?? 1;
            ViewBag.LoaiSanPhamId = new SelectList(db.LoaiSanPhams, "IDLoaiSP", "tenLoaiSP");
            ViewBag.Keyword = searchString;
            if (String.IsNullOrEmpty(searchString) && LoaiSanPhamId == null)
            {
                var all_loai = db.SanPhams.Include(p => p.LoaiSanPham).OrderBy(p => p.LoaiSanPhamId).ToList();
                return View(all_loai.ToPagedList(pageNum, pageSize));
            }
            else if (!String.IsNullOrEmpty(searchString) && LoaiSanPhamId == null)
            {
                searchString = searchString.ToLower();
                var all_loai = db.SanPhams.Include(p => p.LoaiSanPham).Where(n => n.tenSP.Contains(searchString) || n.LoaiSanPham.tenLoaiSP.Contains(searchString)).OrderBy(p => p.LoaiSanPhamId).ToList();
                return View(all_loai.ToPagedList(pageNum, pageSize));
            }
            else if (String.IsNullOrEmpty(searchString) && LoaiSanPhamId != null)
            {
                var all_loai = db.SanPhams.Include(p => p.LoaiSanPham).Where(n => n.LoaiSanPhamId == LoaiSanPhamId).OrderBy(p => p.LoaiSanPhamId).ToList();
                return View(all_loai.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var all_loai = db.SanPhams.Include(p => p.LoaiSanPham).Where(n => n.LoaiSanPhamId == LoaiSanPhamId && n.tenSP.Contains(searchString)).OrderBy(p => p.LoaiSanPhamId).ToList();
                return View(all_loai.ToPagedList(pageNum, pageSize));
            }

        }
        [Authorize(Roles = "Admin")]
        public ActionResult CreateSanPham()
        {
            SanPhamViewModel model = new SanPhamViewModel
            {
                LoaiSanPhams = db.LoaiSanPhams.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateSanPham(SanPhamViewModel model)
        {
            bool IsProduct = db.SanPhams.Any(x => x.tenSP == model.tenSP && x.IDSP != model.IDSP);
            if (IsProduct == true)
            {
                ModelState.AddModelError("tenSP", "Sản Phẩm Đã Tồn Tại ");
            }
            if (ModelState.IsValid)
            {
                var loai = new SanPham
                {
                    tenSP = model.tenSP,
                    Hinh = model.Hinh,
                    GiaBan = model.GiaBan,
                    TinhTrang = true,
                    LoaiSanPhamId = model.LoaiSanPhamID
                };
                db.SanPhams.Add(loai);
                db.SaveChanges();
                TempData["ThongBao"] = new XMessage("success", "Thêm Sản Phẩm Thành Công");
                return RedirectToAction("ListSanPham");
            }
            return this.CreateSanPham();
        }
        [Authorize(Roles = "Admin,Clerk")]
        public ActionResult ManageDonHang(DateTime? From, DateTime? To, int? page, string searchString, bool? Duyet)
        {
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNum = page ?? 1;
            List<SelectListItem> isDuyet = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = "true",
                    Text = "Đã Duyệt"
                },new SelectListItem
                {
                    Value = "false",
                    Text = "Chưa Duyệt"
                }

            };
            ViewBag.List = isDuyet;
            ViewData["From"] = From;
            ViewData["To"] = To;
            ViewBag.KeyWord = searchString;


            if (string.IsNullOrEmpty(searchString) && From == null && To == null && Duyet == null)
            {
                var list = db.HoaDons.Include(p => p.User).ToList();
                return View(list.ToPagedList(pageNum, pageSize));

            }
            else if (string.IsNullOrEmpty(searchString) && Duyet != null && (From == null || To == null))
            {
                var list = db.HoaDons.Include(p => p.User).Where(p => p.TinhTrangDH == Duyet).ToList();
                return View(list.ToPagedList(pageNum, pageSize));

            }
            else if (From != null && To != null && string.IsNullOrEmpty(searchString) && Duyet == null)
            {
                var list = db.HoaDons.Include(p => p.User).Where(p => p.NgayDat >= From && p.NgayDat <= To).ToList();
                return View(list.ToPagedList(pageNum, pageSize));
            }
            else if (From != null && To != null && string.IsNullOrEmpty(searchString) && Duyet != null)
            {
                var list = db.HoaDons.Include(p => p.User).Where(p => p.NgayDat >= From && p.NgayDat <= To && p.TinhTrangDH == Duyet).ToList();
                return View(list.ToPagedList(pageNum, pageSize));
            }
            else if (From != null && To != null && !string.IsNullOrEmpty(searchString) && Duyet == null)
            {
                var list = db.HoaDons.Include(p => p.User).Where(p => p.NgayDat >= From && p.NgayDat <= To && p.IDHD.ToString() == searchString).ToList();
                return View(list.ToPagedList(pageNum, pageSize));
            }

            else if (From == null && To == null && !string.IsNullOrEmpty(searchString) && Duyet == null)
            {
                var list = db.HoaDons.Include(p => p.User).Where(p => p.IDHD.ToString() == searchString).ToList();
                return View(list.ToPagedList(pageNum, pageSize));
            }
            else if (From == null && To == null && !string.IsNullOrEmpty(searchString) && Duyet != null)
            {
                var list = db.HoaDons.Include(p => p.User).Where(p => p.IDHD.ToString() == searchString && p.TinhTrangDH == Duyet).ToList();
                return View(list.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var list = db.HoaDons.Include(p => p.User).Where(p => p.NgayDat >= From && p.NgayDat <= To && p.IDHD.ToString() == searchString && p.TinhTrangDH == Duyet).ToList();
                return View(list.ToPagedList(pageNum, pageSize));
            }
        }
        public bool ChangeHoaDon(int id)
        {
            var a = db.HoaDons.Find(id);
            a.TinhTrangDH = !a.TinhTrangDH;
            db.SaveChanges();
            return a.TinhTrangDH;
        }
        public JsonResult StatusHD(int id)
        {
            var result = ChangeHoaDon(id);
            if (result == true)
            {
                TempData["Type"] = "pd";
                TempData["ThongBaoX"] = new XMessage("success", "Thay Đổi Trạng Thái Thành Công");
            }
            else
            {
                TempData["Type"] = "ds";
                TempData["ThongBaoX"] = new XMessage("danger", "Thay Đổi Trạng Thái Thành Công");
            }


            return Json(new
            {
                message = "Thay Đổi Trạng Thái Thành Công",
                abc = result,

            }, JsonRequestBehavior.AllowGet); ;

        }
        [Authorize(Roles = "Admin")]
        public ActionResult EditSanPham(int? id)
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
            ViewBag.LoaiSanPhamId = new SelectList(db.LoaiSanPhams, "IDLoaiSP", "tenLoaiSP", sanPham.LoaiSanPhamId);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSanPham([Bind(Include = "IDSP,tenSP,Hinh,GiaBan,TinhTrang,LoaiSanPhamId")] SanPham sanPham)
        {
            bool IsProduct = db.SanPhams.Any(x => x.tenSP == sanPham.tenSP && x.IDSP != sanPham.IDSP);
            if (IsProduct == true)
            {
                ModelState.AddModelError("tenSP", "Sản Phẩm Đã Tồn Tại ");
            }
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                sanPham.TinhTrang = true;
                db.SaveChanges();
                TempData["ThongBao"] = new XMessage("success", "Sửa Sản Phẩm Thành Công");
                return RedirectToAction("ListSanPham");
            }
            ViewBag.LoaiSanPhamId = new SelectList(db.LoaiSanPhams, "IDLoaiSP", "tenLoaiSP", sanPham.LoaiSanPhamId);
            return View(sanPham);
        }
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham != null)
            {
                db.SanPhams.Remove(sanPham);
                try
                {
                    db.SaveChanges();
                    TempData["ThongBao"] = new XMessage("success", "Xóa Sản Phẩm Thành Công");
                    return Json(new
                    {
                        result = true
                        ,
                        Message = "Thành công"
                    }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    TempData["ThongBao"] = new XMessage("success", "Xóa Sản Phẩm Thất Bại");
                    return Json(new
                    {
                        result = false
                        ,
                        Message = "Thất bại"
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("ListSanPham");
        }
        public ActionResult DeleteConfirmedLOAISP(int id)
        {
            LoaiSanPham sanPham = db.LoaiSanPhams.Find(id);
            if (sanPham != null)
            {
                db.LoaiSanPhams.Remove(sanPham);
                try
                {
                    db.SaveChanges();
                    TempData["ThongBao"] = new XMessage("success", "Xóa Loại Sản Phẩm Thành Công");
                    return Json(new
                    {
                        result = true
                        ,
                        Message = "Thành công",
                    }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    TempData["ThongBao"] = new XMessage("Error", "Xóa Loại Sản Phẩm Thất Bại");
                    return Json(new
                    {
                        result = false
                        ,
                        Message = "Thất bại"
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("ListLoaiSanPham");
        }
        [HttpGet]
        public bool Change(int IDSP)
        {
            var a = db.SanPhams.Find(IDSP);
            a.TinhTrang = !a.TinhTrang;
            db.SaveChanges();
            return a.TinhTrang;
        }
        public JsonResult Status(int IDSP)
        {
            var result = Change(IDSP);
            if (result == true)
            {
                TempData["Type"] = "pd";
                TempData["ThongBaoX"] = new XMessage("success", "Thay Đổi Trạng Thái Thành Công");
            }
            else
            {
                TempData["Type"] = "ds";
                TempData["ThongBaoX"] = new XMessage("danger", "Thay Đổi Trạng Thái Thành Công");
            }


            return Json(new
            {
                message = "Thay Đổi Trạng Thái Thành Công",
                abc = result,

            }, JsonRequestBehavior.AllowGet); ;

        }
        [Authorize(Roles = "Admin,Clerk")]
        public ActionResult ListLoaiSanPham(int? page)
        {
            if (page == null) page = 1;
            var all_loai = (from s in db.LoaiSanPhams select s).OrderBy(p => p.IDLoaiSP);
            int pageSize = 2;
            int pageNum = page ?? 1;
            return View(all_loai.ToPagedList(pageNum, pageSize));
        }
        public ActionResult CreateLoaiSanPham()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateLoaiSanPham(LoaiSanPham loai, FormCollection collection)
        {
            var ten = collection["tenLoaiSP"];
            if (ModelState.IsValid)
            {
                loai.tenLoaiSP = ten;
                db.LoaiSanPhams.Add(loai);
                db.SaveChanges();
                TempData["ThongBao"] = new XMessage("success", "Thêm Loại Sản Phẩm Thành Công");
                return RedirectToAction("ListLoaiSanPham");
            }
            return this.CreateLoaiSanPham();
        }
        public ActionResult EditLoaiSanPham(int id)
        {
            var sl = db.LoaiSanPhams.First(n => n.IDLoaiSP == id);
            if (sl == null)
            {
                Response.SubStatusCode = 404;
                return null;
            }
            return View(sl);
        }
        [HttpPost]
        public ActionResult EditLoaiSanPham(int id, FormCollection collection)
        {
            var loai = db.LoaiSanPhams.First(n => n.IDLoaiSP == id);

            var ten = collection["tenLoaiSP"];


            if (string.IsNullOrEmpty(ten))
            {
                ViewData["Loi1"] = "Tên loại sản phẩm không được để trống";
            }
            else
            {
                loai.tenLoaiSP = ten;
                db.SaveChanges();
                TempData["ThongBao"] = new XMessage("success", "Sửa Loại Sản Phẩm Thành Công");
                return RedirectToAction("ListLoaiSanPham");
            }
            return this.EditLoaiSanPham(id);
        }
        public ActionResult ListDonHang()
        {
            return View();
        }
        public ActionResult EditDonHang()
        {
            return View();
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/Image/" + file.FileName));
            return "/Content/Image/" + file.FileName;
        }

    }
}