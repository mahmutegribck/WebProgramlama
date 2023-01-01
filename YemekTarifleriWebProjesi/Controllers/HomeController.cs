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
            TarifYorumlar t = new TarifYorumlar();
            var tarifler = db.YemekTarifleris.Include(k => k.Kategori).Where(a => a.Silindi == false && a.Aktif == true && a.TarifId == id).FirstOrDefault();
            t.Tarifler = tarifler;
            var yorumlar = db.Yorumlars.Include(u => u.Uye).Where(a => a.Silindi == false && a.Aktif == true && a.TarifId == id).OrderByDescending(y => y.EklemeTarihi).ToList();
            t.Yorumlar= yorumlar;
            return View(t);
        }

        public IActionResult YorumYap(Yorumlar yor)
        {
            yor.Silindi = false;
            yor.Aktif = false;
            yor.EklemeTarihi = DateTime.Now;
            db.Yorumlars.Add(yor);
            db.SaveChanges();
            TempData["mesaj"] = "Yorumunuz Alındı, Yönetici Onayından Sonra Görünecektir";
            return Redirect("/Home/Tarif" +yor.TarifId);
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