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

        public async Task<IActionResult> Index([Bind("CategoryId")] ProductFilter filter, string SearchTerm, int page = 1, int pageSize = 4)
        {
            {
                // Lấy danh sách sản phẩm từ cơ sở dữ liệu
                var productsQuery = _context.Products.AsQueryable();

                // Lọc sản phẩm theo CategoryId nếu có
                if (filter.CategoryId != null)
                {
                    productsQuery = productsQuery.Where(p => p.categoryid == filter.CategoryId);
                }

                // Lọc theo SearchTerm nếu có
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    productsQuery = productsQuery.Where(p => p.productName.Contains(SearchTerm));
                }

                // Tính toán số sản phẩm trên mỗi trang
                var totalProducts = await productsQuery.CountAsync();
                var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

                // Lấy danh sách sản phẩm cho trang hiện tại
                var products = await productsQuery
                    .Skip((page - 1) * pageSize) // Bỏ qua số sản phẩm đã có trên các trang trước
                    .Take(pageSize)             // Lấy số sản phẩm theo pageSize
                    .ToListAsync();

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

                // Gửi các dữ liệu cần thiết vào View
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");
                return View(productCardViewModels);
            }
        }
    }
}
