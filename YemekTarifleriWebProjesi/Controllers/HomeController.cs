using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YemekTarifleriWebProjesi.Models;

namespace YemekTarifleriWebProjesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int id)
        {
            YemektarifleriDbContext db = new YemektarifleriDbContext();
            var sayfa = db.Sayfalars.Where(a => a.Silindi ==false && a.Aktif ==true && a.SayfaId ==id).FirstOrDefault();
            return View(sayfa);
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
    }
}