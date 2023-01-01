using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YemekTarifleriWebProjesi.Models;

namespace YemekTarifleriWebProjesi.Controllers
{
    public class GirisController : Controller
    {
        public IActionResult GirisYap()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GirisYap(Kullanicilar kullanicilar, string ReturnUrl)
        {
            YemektarifleriDbContext db = new YemektarifleriDbContext();
            var kullanici = db.Kullanicilars.FirstOrDefault(k => k.Eposta == kullanicilar.Eposta && k.Parola == kullanicilar.Parola && k.Silindi == false && k.Aktif == true);
            if(kullanici != null)
            {

                string yetki = (bool)kullanici.Yetki ? "Yonetici" : "Uye";
                var talepler = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, kullanici.Eposta.ToString()),
                    new Claim(ClaimTypes.Role, yetki),
                    new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciId.ToString())

                };
                ClaimsIdentity kimlik = new ClaimsIdentity(talepler,"Login");
                ClaimsPrincipal kural = new ClaimsPrincipal(kimlik);
                await HttpContext.SignInAsync(kural);
                if(!String.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl); 
                }
                else
                {
                    if((bool)kullanici.Yetki)
                    {
                        return Redirect("/Yonetim/Index");
                    }
                    else
                    {
                        return Redirect("/Home/Index");
                    }
                }
            }

            return View();
        }

        public async Task<IActionResult> CikisYap()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }
    }
}
