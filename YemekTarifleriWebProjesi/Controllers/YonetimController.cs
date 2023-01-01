using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using YemekTarifleriWebProjesi.Models;

namespace YemekTarifleriWebProjesi.Controllers
{
    public class YonetimController : Controller
    {
        YemektarifleriDbContext db = new YemektarifleriDbContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Tarifler()
        {
            return View();
        }
        public IActionResult Yorumlar()
        {
            return View();
        }
        public IActionResult Kullanicilar()
        {
            return View();
        }
        public IActionResult Bilgilerim()
        {
            return View();
        }
        public IActionResult Sayfalar()
        {
            var sayfalar = db.Sayfalars.Where(s => s.Silindi == false).OrderBy(s => s.Baslik).ToList();

            return View (sayfalar);

        }
        public IActionResult SayfaEkle()
        {
            

            return View();

        }
        [HttpPost]
        public IActionResult SayfaEkle(Sayfalar sayfa)
        {
            sayfa.Silindi = false;
            db.Sayfalars.Add(sayfa);
            db.SaveChanges();

            return RedirectToAction("Sayfalar");

        }

        public IActionResult SayfaGetir(int id)
        {
            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == id).FirstOrDefault();

            return View("SayfaGuncelle",sayfa);

        }

        public IActionResult CikisYap()
        {
            return View();
        }
    }
}
