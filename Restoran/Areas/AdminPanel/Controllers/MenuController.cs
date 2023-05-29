using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restoran.DAL;
using Restoran.Models;
using Restoran.Utilities.Extensions;

namespace Restoran.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class MenuController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MenuController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Menus.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Menu menu)
        {
            if(!ModelState.IsValid)return View();
            if (!menu.ImageFile.CheckType("Image/"))
            {
                ModelState.AddModelError("", "Image Type Invalid");
                return View();
            }
            if (menu.ImageFile.CheckSize(500))
            {
                ModelState.AddModelError("", "Image size large");
                return View();
            }
            Menu exists = new Menu()
            { 
                Name= menu.Name,
                Tag=menu.Tag,
            };
            exists.Image =await menu.ImageFile.SaveFileAsync(_env.WebRootPath, "assets/img/menu");
            await _context.AddRangeAsync(exists);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _context.Menus.FirstOrDefaultAsync(x=>x.Id==id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Menu menu)
        {
            if (!ModelState.IsValid) return View();
            if (menu.ImageFile != null)
            {
                if (!ModelState.IsValid) return View();
            }
            Menu? exists=await _context.Menus.FirstOrDefaultAsync(x=>x.Id==menu.Id);
            if(exists==null)
            {
                ModelState.AddModelError("", "Menu is null");
                return View();
            }
            if (menu.ImageFile != null)
            {
                if (!menu.ImageFile.CheckType("Image/"))
                {
                    ModelState.AddModelError("", "Image Type Invalid");
                    return View();
                }
                if (menu.ImageFile.CheckSize(500))
                {
                    ModelState.AddModelError("", "Image size large");
                    return View();
                }
                string path = Path.Combine(_env.WebRootPath, "assets/img/menu",exists.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                exists.Image = await menu.ImageFile.SaveFileAsync(_env.WebRootPath, "assets/img/menu");
            }
            exists.Name = menu.Name;
            exists.Tag=menu.Tag;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            Menu? exists = _context.Menus.FirstOrDefault(x => x.Id == id);
            string path = Path.Combine(_env.WebRootPath, "assets/img/menu",exists.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
           _context.Remove(exists);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
