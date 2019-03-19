using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStoreCore.Models;

namespace SportStoreCore.Controllers
{
    public class OrderController : Controller
    {
        public ViewResult Checkout() => View(new Order());
        private Cart cart;
        private IOrderRepository rep;
        public OrderController(IOrderRepository orderRepository,Cart cart)
        {
            rep = orderRepository;
            this.cart = cart;
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (!cart.Lines.Any())
            {
                ModelState.AddModelError("", "Cart is empty");
            }   

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                rep.SaveOrder(order);
               return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }




        }
        [Authorize(policy:"Moderator")]
        public IActionResult ModeratorList()
        {
            return View("List",rep.Orders.Where(x => !x.Shipped));
        }
        [Authorize]
        public IActionResult List()
        {
            return View(rep.Orders);
        }

        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderId)
        {
            Order order = rep.Orders.FirstOrDefault(x => x.OrderID == orderId);
            if (order != null)  
            {
                order.Shipped = true;
                rep.SaveOrder(order);
            }

            return RedirectToAction(nameof(List));
        }
        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}