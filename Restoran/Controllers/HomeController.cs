using Microsoft.AspNetCore.Mvc;
using Restoran.DAL;
using System.Diagnostics;

namespace Restoran.Controllers
{
    public class HomeController : Controller
    {
     private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Menus.OrderByDescending(x=>x.Id).Take(4).ToList());
        }

      
    }
}