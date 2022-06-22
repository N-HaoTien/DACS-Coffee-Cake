using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DOAN_COSO.Models
{
    public class RoleManage
    {
        public string RoleId { get; set; }
        public bool Selected { get; set; }
        public string RoleName { get; set; }
    }

}