using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportStoreCore.Models;

namespace SportStoreCore.Components
{
    public class CartSummaryViewComponent:ViewComponent
    {
        private Cart cart;

        public CartSummaryViewComponent(Cart cartservice)
        {
            cart = cartservice;
        }

        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}
