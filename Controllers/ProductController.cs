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

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            var categories = _context.Categories.ToList();

            List<ProductViewModel> productViewModels = (from category in categories
                                                              join product in products on category.CategoryID equals product.categoryid
                                                              select new ProductViewModel
                                                              {
                                                                  productID = product.productID,
                                                                  productName = product.productName,
                                                                  Price = product.Price,
                                                                  Quantity = product.Quantity,
                                                                  Description = product.Description,
                                                                  CategoryName = category.CategoryName,
                                                                  //ImagesDirectory = product.ImagesDirectory
                                                              }).ToList();  
            return View(productViewModels);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, List<IFormFile> images)
        {
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName");
                return View(product);
            }

            var imagesDirectory = $"{Guid.NewGuid()}";
            var imagesDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imagesDirectory);
            Directory.CreateDirectory(imagesDirectoryPath);

            foreach (var image in images)
            {
                var imageFilePath = Path.Combine(imagesDirectoryPath, Path.GetFileName(image.FileName));
                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }

            var newProduct = new Product
            {
                productName = product.productName,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                categoryid = product.categoryid,
                ImagesDirectory = imagesDirectory
            };

                _context.Products.Add(newProduct);
                _context.SaveChanges();

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Edit(int id)
        {
            var productEdit = _context.Products.Include(p => p.category).FirstOrDefault(p => p.productID == id);
            if (productEdit == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName");

            return View(productEdit);
        }


        [HttpPost]
        public IActionResult Edit(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName");
                return View(product);
            }

            var productEdit = _context.Products.FirstOrDefault(p => p.productID == id);

            if (productEdit == null)
            {
                return NotFound();
            }

            productEdit.productName = product.productName;
            productEdit.Price = product.Price;
            productEdit.Quantity = product.Quantity;
            productEdit.Description = product.Description;
            productEdit.categoryid = product.categoryid;
            _context.SaveChanges(); 

            return RedirectToAction("Index");
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
