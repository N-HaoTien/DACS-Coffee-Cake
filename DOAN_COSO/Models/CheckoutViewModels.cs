using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DOAN_COSO.Models
{
    public class CheckoutViewModels
    {
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime NgayGiao { get; set; }


    }
}