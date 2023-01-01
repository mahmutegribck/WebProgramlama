using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using YemekTarifleriWebProjesi.Models;

namespace YemekTarifleriWebProjesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        YemektarifleriDbContext db = new YemektarifleriDbContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int id)
        {
            
            var sayfa = db.Sayfalars.Where(a => a.Silindi ==false && a.Aktif ==true && a.SayfaId ==id).FirstOrDefault();
            return View(sayfa);
        }

        public IActionResult TumTarifler()
        {
           
            var tarifler = db.YemekTarifleris.Include(k => k.Kategori).Where(a => a.Silindi == false && a.Aktif == true).OrderBy(s => s.Sira).ToList();
            return View(tarifler);
        }
        public IActionResult Tarif(int id)
        {
            
            var tarifler = db.YemekTarifleris.Include(k => k.Kategori).Where(a => a.Silindi == false && a.Aktif == true && a.TarifId == id).FirstOrDefault();
            return View(tarifler);
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