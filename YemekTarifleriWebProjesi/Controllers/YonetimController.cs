using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;

using YemekTarifleriWebProjesi.Models;

namespace YemekTarifleriWebProjesi.Controllers
{
    [Authorize(Roles = "Yonetici")]
    public class YonetimController : Controller
    {
        YemektarifleriDbContext db = new YemektarifleriDbContext();


        public IActionResult Index()
        {
            return View();
        }
        
      
        public IActionResult Bilgilerim()
        {
            int kulId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var girisYapanKullanici = db.Kullanicilars.Find(kulId);
            girisYapanKullanici.Parola = "";
            return View(girisYapanKullanici);
        }
        public IActionResult BilgilerimiGuncelle(Kullanicilar kllnc)
        {


            var kullanici = db.Kullanicilars.Where(t => t.Silindi == false && t.KullaniciId == kllnc.KullaniciId).FirstOrDefault();

            kullanici.Adi = kllnc.Adi;
            kullanici.Soyadi = kllnc.Soyadi;
            kullanici.Eposta = kllnc.Eposta;
            kullanici.Telefon = kllnc.Telefon;

            try
            {
                if (kllnc.Parola.Trim().Length != 0)
                {
                    kullanici.Parola = kllnc.Parola;
                }
            }
            catch
            {


            }

            kullanici.Yetki = kllnc.Yetki;
            db.Kullanicilars.Update(kullanici);
            db.SaveChanges();

            return RedirectToAction("Bilgilerim");



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

            return View("SayfaGuncelle", sayfa);
        }

        public IActionResult SayfaGuncelle(Sayfalar syf)
        {
            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == syf.SayfaId).FirstOrDefault();
            sayfa.Baslik = syf.Baslik;
            sayfa.Icerik = syf.Icerik;
            sayfa.Aktif = syf.Aktif;
            db.Sayfalars.Update(sayfa);
            db.SaveChanges();

            return RedirectToAction("Sayfalar");
        }
        public IActionResult SayfaSil(int id)
        {
            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == id).FirstOrDefault();
            sayfa.Silindi = true;
            db.Sayfalars.Update(sayfa);
            db.SaveChanges();
            return RedirectToAction("Sayfalar");
        }


        public IActionResult Kategoriler()
        {
            var kategoriler = db.Kategorilers.Where(k => k.Silindi == false).OrderBy(k => k.Kategoriadi).ToList();

            return View(kategoriler);
        }

        public IActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult KategoriEkle(Kategoriler kategori)
        {
            kategori.Silindi = false;
            db.Kategorilers.Add(kategori);
            db.SaveChanges();

            return RedirectToAction("Kategoriler");
        }

        public IActionResult KategoriGetir(int id)
        {
            var kategori = db.Kategorilers.Where(k => k.Silindi == false && k.KategoriId == id).FirstOrDefault();

            return View("KategoriGuncelle", kategori);
        }

        public IActionResult KategoriYemekler(int id)
        {
            var yemekler = db.YemekTarifleris.Include(k => k.Kategori).Where(y => y.Silindi == false && y.KategoriId == id).ToList();
            return View("Tarifler", yemekler);
        }

        public IActionResult KategoriGuncelle(Kategoriler ktgr)
        {
            var kategori = db.Kategorilers.Where(k => k.Silindi == false && k.KategoriId == ktgr.KategoriId).FirstOrDefault();
            kategori.Kategoriadi = ktgr.Kategoriadi;
            kategori.Aktif = ktgr.Aktif;

            db.Kategorilers.Update(kategori);
            db.SaveChanges();

            return RedirectToAction("Kategoriler");
        }
        public IActionResult KategoriSil(int id)
        {
            var kategori = db.Kategorilers.Where(k => k.Silindi == false && k.KategoriId == id).FirstOrDefault();
            kategori.Silindi = true;
            db.Kategorilers.Update(kategori);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }


        public IActionResult Tarifler()
        {
            var tarifler = db.YemekTarifleris.Include(k => k.Kategori).Where(t => t.Silindi == false).OrderBy(t => t.Yemekadi).ToList();

            return View(tarifler);
        }

        public IActionResult TarifEkle()
        {
            var kategoriler = (from k in db.Kategorilers.Where(k => k.Silindi == false && k.Aktif == true).ToList()
            select new SelectListItem
            {
                Text = k.Kategoriadi,
                Value = k.KategoriId.ToString()
            }
            );
            ViewBag.KategoriId = kategoriler;
            return View();
        }

        [HttpPost]
        public IActionResult TarifEkle(YemekTarifleri yemekTarifleri)
        {
            yemekTarifleri.Silindi = false;
            yemekTarifleri.EklemeTarihi = DateTime.Now;
            db.YemekTarifleris.Add(yemekTarifleri);
            db.SaveChanges();

            return RedirectToAction("Tarifler");
        }

        public IActionResult TarifGetir(int id)
        {
            var tarif = db.YemekTarifleris.Include(k => k.Kategori).Where(t => t.Silindi == false && t.TarifId == id).FirstOrDefault();
            var kategoriler = (from k in db.Kategorilers.Where(k => k.Silindi == false && k.Aktif == true).ToList()
                               select new SelectListItem
                               {
                                   Text = k.Kategoriadi,
                                   Value = k.KategoriId.ToString()
                               }
           );
            ViewBag.KategoriId = kategoriler;
            return View("TarifGuncelle", tarif);
        }

        public IActionResult TarifYorumlari(int id)
        {
            var yorumlar = db.Yorumlars.Where(y => y.Silindi == false && y.TarifId == id).ToList();
            return View("Yorumlar", yorumlar);
        }

        public IActionResult TarifGuncelle(YemekTarifleri trf)
        {
            var tarif = db.YemekTarifleris.Where(t => t.Silindi == false && t.TarifId == trf.TarifId).FirstOrDefault();
            tarif.Yemekadi = trf.Yemekadi;
            tarif.Tarif = trf.Tarif;
            tarif.Sira = trf.Sira;
            tarif.KategoriId = trf.KategoriId;
            tarif.Aktif = trf.Aktif;
            
            
            db.YemekTarifleris.Update(tarif);
            db.SaveChanges();

            return RedirectToAction("Tarifler");
        }
        public IActionResult TarifSil(int id)
        {
            var tarif = db.YemekTarifleris.Where(t => t.Silindi == false && t.TarifId == id).FirstOrDefault();
            tarif.Silindi = true;
            db.YemekTarifleris.Update(tarif);
            db.SaveChanges();
            return RedirectToAction("Tarifler");
        }

        public IActionResult CikisYap()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Yorumlar()
        {
            var yorumlar = db.Yorumlars.Include(t => t.TarifNavigation).Include(u => u.Uye).Where(y => y.Silindi  == false).OrderByDescending(y => y.EklemeTarihi).ToList();

            return View(yorumlar);

 

        }
        [HttpPost]
        public IActionResult Yorumlar(string listelemeturu)
        {
            var yorumlar = db.Yorumlars.Include(t => t.TarifNavigation).Include(u => u.Uye).Where(y => y.Silindi == false).OrderByDescending(y => y.EklemeTarihi).ToList();
            switch (listelemeturu)
            {
                case "Onayli": yorumlar = db.Yorumlars.Include(t => t.TarifNavigation).Include(u => u.Uye).Where(y => y.Silindi == false && y.Aktif == true).OrderByDescending(y => y.EklemeTarihi).ToList(); break;

                case "Onaysiz": yorumlar = db.Yorumlars.Include(t => t.TarifNavigation).Include(u => u.Uye).Where(y => y.Silindi == false && y.Aktif == false).OrderByDescending(y => y.EklemeTarihi).ToList(); break;
               
            }
            return View(yorumlar);



        }



        public IActionResult Onayla(int id)
        {
            var yorum = db.Yorumlars.Where(y => y.Silindi == false && y.YorumId == id).FirstOrDefault();
            yorum.Aktif = Convert.ToBoolean((-1*Convert.ToInt32(yorum.Aktif))+1);
            db.Yorumlars.Update(yorum);
            db.SaveChanges();
            return RedirectToAction("Yorumlar");
        }
        public IActionResult YorumSil(int id)
        {
            var yorum = db.Yorumlars.Where(y => y.Silindi == false && y.YorumId == id).FirstOrDefault();
            yorum.Aktif = true;
            db.Yorumlars.Update(yorum);
            db.SaveChanges();
            return RedirectToAction("Yorumlar");
        }




        public IActionResult Kullanicilar()
        {
            var kullanicilar = db.Kullanicilars.Where(t => t.Silindi == false).OrderBy(t => t.Yetki).OrderBy(t => t.Adi).OrderBy(t => t.Soyadi).ToList();

            return View(kullanicilar);
        }

        public IActionResult KullaniciEkle()
        {
            
                              
            return View();
        }

        [HttpPost]
        public IActionResult KullaniciEkle(Kullanicilar kullanicilar)
        {
            kullanicilar.Silindi = false;

            db.Kullanicilars.Add(kullanicilar);
            db.SaveChanges();

            return RedirectToAction("Kullanicilar");




        }

        public IActionResult KullaniciGetir(int id)
        {
            var kullanici = db.Kullanicilars.Where(t => t.Silindi == false && t.KullaniciId == id).FirstOrDefault();
            
            return View("KullaniciGüncelle", kullanici);
        }

        public IActionResult KullaniciGuncelle(Kullanicilar kllnc)
        {
            var kullanici = db.Kullanicilars.Where(t => t.Silindi == false && t.KullaniciId == kllnc.KullaniciId).FirstOrDefault();

            kullanici.Aktif = kllnc.Aktif;
            kullanici.Adi = kllnc.Adi;
            kullanici.Soyadi= kllnc.Soyadi;
            kullanici.Eposta = kllnc.Eposta;
            kullanici.Telefon = kllnc.Telefon;

            try
            {
                if (kllnc.Parola.Trim().Length != 0)
                {
                    kullanici.Parola = kllnc.Parola;
                }
            }
            catch 
            {

               
            }
            
            kullanici.Yetki = kllnc.Yetki;
            db.Kullanicilars.Update(kullanici);
            db.SaveChanges();

            return RedirectToAction("Kullanicilar");
        }
        public IActionResult KullaniciSil(int id)
        {
            var kullanici = db.Kullanicilars.Where(t => t.Silindi == false && t.KullaniciId == id).FirstOrDefault();
            kullanici.Silindi = true;
            db.Kullanicilars.Update(kullanici);
            db.SaveChanges();
            return RedirectToAction("Kullanicilar");
        }

      

    }
}

