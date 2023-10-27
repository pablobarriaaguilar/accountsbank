using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using bankaccounts.Models;
using Microsoft.AspNetCore.Identity;

namespace bankaccounts.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
private MyContext _context; 
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;  
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

[HttpPost("user/create")]
    public IActionResult CreateUser(User _user){
        if (ModelState.IsValid){
            PasswordHasher<User> Hasher = new PasswordHasher<User>();   
            _user.Password = Hasher.HashPassword(_user, _user.Password); 
            _context.Add(_user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }else{
            return RedirectToAction("Index");
        }
    }


 
}
