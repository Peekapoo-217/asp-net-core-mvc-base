using Demo_Code_First.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Code_First.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult DeleteAll()
        {
            var user = _context.Users.ToList();
            if(user.Any())
            {
                _context.Users.RemoveRange(user);
                _context.SaveChanges();
            }return RedirectToAction("Index");
        }
        
        public IActionResult Index()
        {
            var user = _context.Users.ToList();
            return View(user);
        }
    }
}
