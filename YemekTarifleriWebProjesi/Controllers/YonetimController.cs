using Microsoft.AspNetCore.Mvc;

namespace YemekTarifleriWebProjesi.Controllers
{
    public class YonetimController : Controller
    {
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
    }
}
