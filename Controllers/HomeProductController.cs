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


        /// <summary>
        /// Handles the Index action for displaying a paginated list of products.
        /// Supports filtering by category and searching by product name.
        /// </summary>
        /// <param name="filter">Filter options for category</param>
        /// <param name="searchTerm">Search term for product name</param>
        /// <param name="page">Current page number</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>A view displaying the list of products</returns>
        public async Task<IActionResult> Index([Bind("CategoryId")] ProductFilter filter, string searchTerm, int page = 1, int pageSize = 4)
        {
            // Step 1: Initialize query for products
            var productsQuery = _context.Products.AsQueryable();

            // Step 2: Apply filters
            if (filter.CategoryId != null)
            {
                // Filter products by category ID
                productsQuery = productsQuery.Where(p => p.categoryid == filter.CategoryId);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Filter products by search term in product name
                productsQuery = productsQuery.Where(p => p.productName.Contains(searchTerm));
            }

            // Step 3: Pagination logic
            var totalProducts = await productsQuery.CountAsync(); // Get total number of filtered products
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize); // Calculate total pages
            var products = await productsQuery
                .Skip((page - 1) * pageSize) // Skip items for previous pages
                .Take(pageSize)             // Take items for the current page
                .ToListAsync();

            // Step 4: Load categories for dropdown
            var categories = await _context.Categories.ToListAsync();

            // Step 5: Build view models for each product
            var productCardViewModels = products.Select(product =>
            {
                // Get a random image from the product's image directory
                var fullImagesDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", product.ImagesDirectory);
                var files = Directory.GetFiles(fullImagesDirectoryPath, "*.*").ToList();
                var randomFile = files.Any() ? Path.GetRelativePath(_webHostEnvironment.WebRootPath, files.First()) : null;

                // Find the product's category
                var category = categories.FirstOrDefault(c => c.CategoryID == product.categoryid);

                // Create a product view model
                return new ProductCardViewModel
                {
                    productID = product.productID,
                    productName = product.productName,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    MainImage = randomFile,
                    CategoryName = category?.CategoryName
                };
            }).ToList();

            // Step 6: Pass data to the view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");

            return View(productCardViewModels);
        }

    }
}
