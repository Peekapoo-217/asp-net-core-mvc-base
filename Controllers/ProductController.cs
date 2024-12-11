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

                // Thêm sản phẩm vào cơ sở dữ liệu
                _context.Products.Add(newProduct);
                _context.SaveChanges();

            // Điều hướng về trang Index sau khi lưu thành công
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Edit(int id, Product product)
        {
            var productEdit = _context.Products.Find(id);
            if (productEdit == null)
            {
                return RedirectToAction("Index", "Products");
            }

                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = product.categoryid;
                return View(product);
            }
            productEdit.productName = product.productName;
            productEdit.Price = product.Price;
            productEdit.Quantity = product.Quantity;
            productEdit.Description = product.Description;
            productEdit.category = product.category;

            _context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }


        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.productID == id);

            if (product == null)
            {
                return NotFound();
            }

            var category = _context.Categories.FirstOrDefault(c => c.CategoryID == product.categoryid);

            var ImagesDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", product.ImagesDirectory);
            var files = Directory.Exists(ImagesDirectoryPath) ? Directory.GetFiles(ImagesDirectoryPath, "*.*").ToList() : new List<string>();
            var imagePath = files.Select(file =>
                "/" + Path.GetRelativePath(_webHostEnvironment.WebRootPath, file).Replace("\\", "/")
            ).ToList();

            var productDetailsViewModel = new ProductDetailsViewModel
            {
                productID = product.productID,
                productName = product.productName,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                CategoryName = product.category?.CategoryName,
                Images = imagePath
            };

            return View(productDetailsViewModel);
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
