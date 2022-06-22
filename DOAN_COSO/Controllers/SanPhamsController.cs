using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOAN_COSO.Models;

namespace DOAN_COSO.Controllers
{
    public class SanPhamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ViewSearchCheckBox()
        {

            var a = db.SanPhams.Include(p => p.ChiTietHoaDons).Include(p => p.LoaiSanPham).GroupBy(p => p.ChiTietHoaDons).Select(p => new SanPhamViewModel() { tenSP = p.Key.Count().ToString() ,SoLuong = p.Key.Sum(d => d.SoLuong) }).ElementAt(3) ;

            //var b = db.SanPhams.Include(p => p.ChiTietHoaDons).Include(p => p.LoaiSanPham).GroupBy(p => p.j).Select(p => );

            return View(a);
        }
        
        // GET: SanPhams
        public ActionResult Index(int? LoaiSanPhamId)
        {

            ViewBag.KeyWord = LoaiSanPhamId;
            ViewBag.LoaiSanPhamId = new SelectList(db.LoaiSanPhams, "IDLoaiSP", "tenLoaiSP");
            if (LoaiSanPhamId == null)
            {
                var a = db.SanPhams.Include(c => c.LoaiSanPham).ToList();
                return View(a);
            }
            else
            {
                var a = db.SanPhams.Include(c => c.LoaiSanPham).Where(p => p.LoaiSanPhamId == LoaiSanPhamId).ToList();
                return View(a);
            }

        }

        /*[HttpPost]
        public ActionResult Index(SanPham sanPham)
        {
            int num = Convert.ToInt32(Session["row"]) + 5;
            var a = db.SanPhams.Include(c => c.LoaiSanPham).ToList().Take(num);

            Session["row"] = num;

            return PartialView("_LoadMore", a);
        }
*/
        // GET: SanPhams/Details/5
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

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.LoaiSanPhamId = new SelectList(db.LoaiSanPhams, "IDLoaiSP", "tenLoaiSP");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDSP,tenSP,Hinh,GiaBan,TinhTrang,LoaiSanPhamId")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoaiSanPhamId = new SelectList(db.LoaiSanPhams, "IDLoaiSP", "tenLoaiSP", sanPham.LoaiSanPhamId);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
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
        public ActionResult Edit([Bind(Include = "IDSP,tenSP,Hinh,GiaBan,TinhTrang,LoaiSanPhamId")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoaiSanPhamId = new SelectList(db.LoaiSanPhams, "IDLoaiSP", "tenLoaiSP", sanPham.LoaiSanPhamId);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
