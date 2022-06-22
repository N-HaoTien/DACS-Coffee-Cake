using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DOAN_COSO.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
namespace DOAN_COSO.Controllers
{
    public class GioHangController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: GioHang
        public List<CartModel> LayGioHang()
        {
            List<CartModel> list = Session["cart"] as List<CartModel>;
            if (list == null)
            {
                list = new List<CartModel>();
                Session["cart"] = list;
            }
            return list;
        }
        public ActionResult GioHanggetJson()
        {
            List<CartModel> list = LayGioHang();
            return Json(new
            {
                SL = list.Count(),
                TongTien = list.Sum(c => c.ThanhTien),
                Messeage = "Thành công",
                Sum = list.Sum(c => c.Quantity),
                JsonRequestBehavior.AllowGet
            }); ;
        }
        public ActionResult GioHang()
        {
            List<CartModel> list = LayGioHang();
            ViewBag.TongSLSP = list.Sum(n => n.Quantity);
            ViewBag.TongSL = list.Count();
            ViewBag.TongTien = list.Sum(c => c.ThanhTien);
            return View(list);
        }

        public ActionResult AddToCart(int IDSP, int quantity)
        {
            List<CartModel> cart;
            if (Session["cart"] == null)
            {
                cart = new List<CartModel>();
                cart.Add(new CartModel { SanPham = db.SanPhams.Find(IDSP), Quantity = quantity });
                Session["cart"] = cart;
                Session["count"] = 1;
            }
            else
            {
                cart = (List<CartModel>)Session["cart"];
                //kiểm tra sản phẩm có tồn tại trong giỏ hàng chưa???
                int index = isExist(IDSP);
                if (index != -1)
                {
                    //nếu sp tồn tại trong giỏ hàng thì cộng thêm số lượng
                    cart[index].Quantity += quantity;
                }
                else
                {
                    //nếu không tồn tại thì thêm sản phẩm vào giỏ hàng
                    cart.Add(new CartModel { SanPham = db.SanPhams.Find(IDSP), Quantity = quantity });
                    //Tính lại số sản phẩm trong giỏ hàng
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }
                Session["cart"] = cart;
            }
            return Json(new
            {
                Count = cart.Count(),
                Sum = cart.Sum(c => c.Quantity)
            });
        }
        public ActionResult CapNhatGioHang(int IDSP, int Quantity)
        {
            List<CartModel> list = LayGioHang();
            CartModel sp = list.SingleOrDefault(n => n.SanPham.IDSP == IDSP);
            if (sp != null)
            {
                sp.Quantity = Quantity;
                return Json(new
                {
                    TongTien = list.Sum(c => c.ThanhTien),
                    Messeage = "Thành công",
                    ThanhTien = sp.ThanhTien,
                    Sum = list.Sum(c => c.Quantity),
                    JsonRequestBehavior.AllowGet
                });
            }
            return Json(new
            {
                Messeage = "Thất Bại"
            });

        }
        private int isExist(int IDSP)
        {
            List<CartModel> cart = (List<CartModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].SanPham.IDSP.Equals(IDSP))
                    return i;
            return -1;
        }

        //xóa sản phẩm khỏi giỏ hàng theo id

        public ActionResult Remove(int IDSP)
        {
            List<CartModel> list = (List<CartModel>)Session["cart"];
            list.RemoveAll(x => x.SanPham.IDSP == IDSP);
            Session["cart"] = list;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return Json(new
            {
                Count = list.Count(),
                TongTien = list.Sum(c => c.ThanhTien),
                Messeage = "Thành công",
                Sum = list.Sum(c => c.Quantity),
                JsonRequestBehavior.AllowGet
            });
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<CartModel> list = LayGioHang();
            list.Clear();
            return Json(new
            {
                Count = list.Count(),
                Sum = list.Sum(c => c.Quantity),
                Message = "Thành công",
                JsonRequestBehavior.AllowGet
            });
        }
        public ActionResult Success(string mess = "")
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(c => c.Id == userId);
            if (string.IsNullOrEmpty(mess))
            {

            }
            else if (mess == "Lỗi")
            {

            }    
            else
            {
                HoaDon hd = new HoaDon();
                List<CartModel> gh = LayGioHang();

                hd.UserID = userId;

                hd.NgayDat = DateTime.Now;

                hd.NgayGiao = DateTime.Now.AddMinutes(20);
                hd.IsPaid = true;
                hd.TinhTrangDH = false;

                hd.TongTien = gh.Sum(c => c.ThanhTien);

                db.HoaDons.Add(hd);
                db.SaveChanges();
                foreach (var item in gh)
                {
                    ChiTietHoaDon ct = new ChiTietHoaDon();

                    ct.IdSanPham = item.SanPham.IDSP;
                    ct.IdHoaDon = hd.IDHD;
                    ct.SoLuong = item.Quantity;

                    db.ChiTietHoaDons.Add(ct);
                    db.SaveChanges();
                }
                SentMail("Đặt hàng thành công", user.Email.ToString(), "Cảm ơn bạn đã ghé qua,Nhấn để quay về trang chủ: <a href=\"" + Url.Action("Home", "TrangChu") + "\">link</a><br/> Mã Đơn Hàng của bạn là \"" + hd.IDHD + "\" ");
                Session["cart"] = null;
                Session["count"] = 0;
            }
           
            return View();
        }
        [Authorize]
        public ActionResult Checkout()
        {
            List<CartModel> cart = LayGioHang();
            if (cart.Count() > 0)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(c => c.Id == userId);

                CheckoutViewModels model = null;
                if (user != null)
                {
                    model = new CheckoutViewModels()
                    {
                        Email = user.Email,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        NgayDat = DateTime.Now,
                        NgayGiao = DateTime.Now.AddMinutes(20)
                    };
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
                ViewBag.TongTien = cart.Sum(c => c.ThanhTien).ToString("#,##0");
                ViewBag.Count = cart.Count();
                ViewBag.SLSP = cart.Sum(c => c.Quantity);
                ViewBag.GioHang = cart;
                return View(model);
            }
            else
            {
                return RedirectToAction("TrangChu", "Home");
            }

        }
        [Authorize]
        [HttpPost]
        public ActionResult Checkout(CheckoutViewModels model, string payment = "")
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(c => c.Id == userId);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    db.SaveChanges();
                    IdentityResult result = UserManager.Update(user);
                    if (result.Succeeded)
                    {
                        Session["FullName"] = user.Name;
                    }
                }
            }
            if (payment == "Trả tiền qua momo")
            {
                return RedirectToAction("PaymentWithMomo", "Payment");
            }
            else if (payment == "Trả tiền qua Paypal")
            {
                return RedirectToAction("PaymentWithPaypal", "Payment");
            }
            HoaDon hd = new HoaDon();
            List<CartModel> gh = LayGioHang();

            hd.UserID = userId;

            hd.NgayDat = DateTime.Now;

            hd.NgayGiao = DateTime.Now.AddMinutes(20);
            hd.IsPaid = false;

            hd.TinhTrangDH = false;

            hd.TongTien = gh.Sum(c => c.ThanhTien);

            db.HoaDons.Add(hd);
            db.SaveChanges();
            foreach (var item in gh)
            {
                ChiTietHoaDon ct = new ChiTietHoaDon();

                ct.IdSanPham = item.SanPham.IDSP;
                ct.IdHoaDon = hd.IDHD;
                ct.SoLuong = item.Quantity;

                db.ChiTietHoaDons.Add(ct);
                db.SaveChanges();
            }
            SentMail("Đặt hàng thành công", user.Email.ToString(), "Cảm ơn bạn đã ghé qua,Nhấn để quay về trang chủ: <a href=\"" + Url.Action("Home", "TrangChu") + "\">link</a><br/> Mã Đơn Hàng của bạn là \"" + hd.IDHD + "\" ");
            Session["cart"] = null;
            Session["count"] = 0;
            return View("Success" , new { mess = ""});

        }
        public void SentMail(string Title, string ToEmail, string Content)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("nht.it19@gmail.com");
            msg.To.Add(new MailAddress(ToEmail));
            msg.Subject = Title;
            msg.IsBodyHtml = true;
            msg.Body = "<html><body> " + Content + " </body></html>";
            msg.IsBodyHtml = true;
            //smtp-mail.outlook.com
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("nht.it19@gmail.com", "anhyeuem3");
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
    }
}