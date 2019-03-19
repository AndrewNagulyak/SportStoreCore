using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStoreCore.Models;

namespace SportStoreCore.Components
{
    public class LoginViewComponent : ViewComponent
    {
        

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
