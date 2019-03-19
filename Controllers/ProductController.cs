using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportStoreCore.Models;
using SportStoreCore.Models.ViewModels;

namespace SportStoreCore.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly IProductRepository repository;
        public int PageSize = 2;
        public ProductController( IProductRepository rep)
        {
            repository = rep;
        }
        public ViewResult List(string category,int productPage = 1)
            => View(new ProductsViewModal
            {
                CurrentCategory = category,
                Products = repository.Products
                    .OrderBy(p => p.ProductID)
                    .Where(p=>p.Category==category || category==null)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Count(p => p.Category == category || category == null)
                }
            });

        public ViewResult Index()
        {
            return View("ASf");
        }

    }
}