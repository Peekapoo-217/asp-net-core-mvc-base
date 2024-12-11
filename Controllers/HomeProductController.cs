using Demo_Code_First.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Demo_Code_First.Controllers
{
    public class HomeProductController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index([Bind("CategoryId")] ProductFilter filter)
        {
            var products = await _context.Products.ToListAsync();

            products = products.Where(p => (filter.CategoryId != null) ? p.categoryid == filter.CategoryId : 1 == 1).ToList(); 

            var categories = await _context.Categories.ToListAsync();
            List<ProductCardViewModel> productCardViewModels = new List<ProductCardViewModel>();
            foreach (var product in products)
            {
                var fullImagesDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", product.ImagesDirectory);
                var files = Directory.GetFiles(fullImagesDirectoryPath, "*.*").ToList();
                var randomFile = files.First();
                randomFile = Path.GetRelativePath(_webHostEnvironment.WebRootPath, randomFile);

                var category = categories.FirstOrDefault(c => c.CategoryID == product.categoryid);


                productCardViewModels.Add(new ProductCardViewModel
                {
                    productID = product.productID,
                    productName = product.productName,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    MainImage = randomFile,
                    CategoryName = category?.CategoryName
                });
            }

            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");
            return View(productCardViewModels);
        }
    }
}
