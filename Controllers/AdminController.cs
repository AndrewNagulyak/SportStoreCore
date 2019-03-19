using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportStoreCore.Models;

namespace SportStoreCore.Controllers
{
    [Authorize (policy:"Admins")]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repository)
        {
            this.repository = repository;
        }
        public IActionResult Index()
        {
            return View(repository.Products);
        }

        public IActionResult Edit(int ProductID)    
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == ProductID);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product editproduct)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(editproduct);
                TempData["message"] = $"{editproduct.Name} has been saved";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(editproduct);
            }
        }
        public IActionResult Delete(int ProductID)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == ProductID);
            repository.RemoveProduct(product);
            TempData["message"] = $"{product.Name} has been deleted";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View("Edit",new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
           repository.SaveProduct(product);
           return RedirectToAction(nameof(Index));
        }
    }
}