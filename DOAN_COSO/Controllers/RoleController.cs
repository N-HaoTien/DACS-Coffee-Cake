using DOAN_COSO.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;

namespace DOAN_COSO.Controllers
{
    //[Authorize(Roles = "Admin")]

    public class RoleController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();
        public RoleController()
        {
        }

        public RoleController(ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
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

        /*        [Authorize(Roles = "Khách Hàng")]*/
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            var role = new ApplicationRole() { Name = model.Role };

            await RoleManager.CreateAsync(role);


            return RedirectToAction("Index");

        }
        public ActionResult Index(RoleViewModel model ,int? page)
        {
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNum = page ?? 1;
            var usersWithRoles = (from user in db.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.Name,
                                      Email = user.Email,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in db.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new RoleViewModel()

                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames)
                                  }).ToList();
            return View(usersWithRoles.ToPagedList(pageNum, pageSize));
        }
        public async Task<ActionResult> ManageRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "abc";
                return View("NotFound");
            }
            var model = new List<RoleManage>();

            foreach (var item in db.Roles.ToList())
            {
                var role = new RoleManage
                {
                    RoleId = item.Id,
                    RoleName = item.Name,
                };
                if (await UserManager.IsInRoleAsync(user.Id, item.Name))
                {
                    role.Selected = true;
                }
                else
                {
                    role.Selected = false;
                }

                model.Add(role);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> ManageRoles(List<RoleManage>model,string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);;
            foreach (var item in model)
            {
                var role = RoleManager.FindById(item.RoleId);
                if (UserManager.IsInRole(user.Id, item.RoleName) && !item.Selected)
                {
                    var result = UserManager.RemoveFromRole(user.Id, role.Name);
                }
                else if (!UserManager.IsInRole(user.Id, item.RoleName) && item.Selected)
                {
                    var result = UserManager.AddToRole(user.Id, role.Name);
                }
            }
            return RedirectToAction("Index","Role");
        }
    }
}