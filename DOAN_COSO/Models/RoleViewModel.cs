using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOAN_COSO.Models
{
    public class RoleViewModel
    {
        public RoleViewModel() { }

        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Selected { get; set; }
        public IdentityUser User { get; set; }
        public SelectList Roles { get; set; }
    }
}