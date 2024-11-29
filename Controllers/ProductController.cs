using Demo_Code_First.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Demo_Code_First.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(productDto);

            //}return RedirectToAction("Index", "Products");
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName");
                return View(productDto);
            }
            var product = new Product
            {
                productName = productDto.productName,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                Description = productDto.Description,
                categoryid = productDto.categoryid
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }

        public IActionResult Delete(int id)
        {
            var products = _context.Products.FirstOrDefault(p => p.productID == id);
            if (products != null)
            {
                return NotFound();
            }
            return View(products);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var products = _context.Products.FirstOrDefault(x => x.productID == id);
            if (products != null)
            {
                _context.Products.Remove(products);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
