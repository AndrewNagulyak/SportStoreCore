using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportStoreCore.Models
{
    public class RegistrationModel:LoginModel
    {
        [Required]
        [UIHint("mail")]
        public string email { get; set; }
    }
}
