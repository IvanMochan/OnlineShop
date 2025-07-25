﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Db;

namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly OnlineShopContext _context;
        public ProductsController(OnlineShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Product> products = _context.Products.OrderByDescending(x => x.Id).ToList();
            return View(products);
        }

        public IActionResult SearchProducts(string SearchText)
        {
            var products = _context.Products
                .Where(x=>
                EF.Functions.Like(x.Title, "%" + SearchText + "%") || 
                EF.Functions.Like(x.Tags, "%" + SearchText + "%")
                )
                .OrderBy(x=>x.Title)
                .ToList();
            return View("Index", products);
        }
        public IActionResult ProductDetails(int id)
        {
            Product? product = _context.Products.FirstOrDefault(x=> x.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            ViewData["gallery"] = _context.ProductGalleries.Where(x=> x.ProductId == id).ToList();

            ViewData["comments"] = _context.Comments.Where(x => x.ProductId == id).
                OrderByDescending(x=> x.CreateDate).ToList();
            return View(product);
        } 
    }
}
