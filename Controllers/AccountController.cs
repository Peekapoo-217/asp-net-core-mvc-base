using Microsoft.AspNetCore.Mvc;
using Demo_Code_First.Models;

namespace Demo_Code_First.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            return View(user);
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.userName);

                return RedirectToAction("Index", "User");
            }

            ViewBag.ErrorMessage = "Email hoặc mật khẩu không đúng ";
            return View();
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
