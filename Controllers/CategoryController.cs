//using Demo_Code_First.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace Demo_Code_First.Controllers
//{
//    public class CategoryController : Controller
//    {
//        private readonly AppDbContext _context;

//        public CategoryController(AppDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult AddCategory()
//        {
//            try
//            {
//                var categories = new List<Category>
//        {
//            new Category { CategoryName = "Iphone", CategoryDescription = "Nice" },
//            new Category { CategoryName = "Màn hình", CategoryDescription = "Good" },
//            new Category { CategoryName = "Android", CategoryDescription = "Perfect" }
//        };

//                _context.Categories.AddRange(categories);
//                _context.SaveChanges();

//                return Content("Category added successfully!");
//            }
//            catch (Exception ex)
//            {
//                return Content($"Error: {ex.Message}");
//            }
//        }
//    }
//}
