using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportStoreCore.Models
{
    public class LoginModel
    {
        [Required]
        public string login { get; set; }
        
        [Required]
        [UIHint("password")]
        public string password { get; set; }
        public string str { get; set; }
        public string ReturnUrl { get; set; } = "/";
    }
}
