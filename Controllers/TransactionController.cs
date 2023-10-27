using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using bankaccounts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace bankaccounts.Controllers;

public class TransactionController : Controller
{
    private readonly ILogger<HomeController> _logger;
private MyContext _context; 
    public TransactionController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;  
    }
[Route("accounts")]
    public IActionResult Account()
    {
        return View("Index");
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

     [HttpPost("/login")]
    public IActionResult Login(LoginUser userSubmission) //parameter to pass in, the user submission of their email and password
    {
        //EMAIL CHECK
        //Check if submission is valid according to our model
        if (!ModelState.IsValid) //If submission is not valid according to our model
        {
            return View("Index"); //return them back to the index view to register/login again
        }

        //Check if email submitted is in our database
        User? userInDb = _context.Users.FirstOrDefault(e => e.Email == userSubmission.Email);

        //If nothing comes back, add an error and return the same view to render the validation
        if (userInDb == null)
        {
            ModelState.AddModelError("Email", "Invalid Email Address/Password");
            return View("Index");
        }

        //PASSWORD CHECK
        //Invoke password hasher
        PasswordHasher<LoginUser> hashbrowns = new PasswordHasher<LoginUser>();
        //LoginUser, hashed password, password submitted by user
        var result = hashbrowns.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
        

        if (result == 0)
        {
            ModelState.AddModelError("Email", "Invalid Email Address/Password"); //string key, error message parameters
            return View("Index");
        }

        //PASSES ALL CHECKS, HANDLE SUCCESS NOW
        //We want to set the session key:value pair to id:UserId and we'll use this to keep track if our user is logged in
        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
        HttpContext.Session.SetString("UserName", userInDb.Email);
        List<Transaction> _transact = _context.Transactions.ToList();
        UserTransaction userTransaction = new UserTransaction
        {
            User = userInDb
        };
        
        userTransaction.User.Alltransaction = _transact;
               

        
        return View("Index",userTransaction);
    }

   public IActionResult Amount(decimal amount)
   {  UserTransaction _userTransaction = new UserTransaction();
        _userTransaction.Transaction = new Transaction(); 
        
     int id  = (int)HttpContext.Session.GetInt32("UserId");
     
        User? userInDb = _context.Users.FirstOrDefault(e => e.UserId == id);
        List<Transaction> _transact = _context.Transactions.Where(t => t.UserId == id).ToList();
        decimal _amount = decimal.Parse(amount.ToString());
        _userTransaction.User = userInDb;
        _userTransaction.Transaction.UserId = _userTransaction.User.UserId;
        _userTransaction.Transaction.Amount = _amount;
        _userTransaction.User = userInDb;
        _userTransaction.User.Alltransaction = _transact;
        decimal suma= _userTransaction.User.Alltransaction.Sum( c => c.Amount);
        Console.WriteLine ("-------------" + (suma-amount));
    if( suma + amount < 0){
         ModelState.AddModelError(string.Empty, "Error: No puede retirar un monto mayor que su cuenta permite.");
          return View("Index",_userTransaction);
        
    }else{
        Console.WriteLine(_userTransaction.Transaction.Amount);
        _context.Add(_userTransaction.Transaction);
        _context.SaveChanges();
        return View("Index",_userTransaction);
    }     
        
    }
}
