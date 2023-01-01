using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            var yemekler = db.YemekTarifleris.Where(y => y.Silindi == false && y.KategoriId == id).ToList();
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
            return View();
        }

        [HttpPost]
        public IActionResult TarifEkle(YemekTarifleri yemekTarifleri)
        {
            yemekTarifleri.Silindi = false;
            db.YemekTarifleris.Add(yemekTarifleri);
            db.SaveChanges();

            return RedirectToAction("Tarifler");
        }

        public IActionResult TarifGetir(int id)
        {
            var tarif = db.YemekTarifleris.Where(t => t.Silindi == false && t.TarifId == id).FirstOrDefault();

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
            return RedirectToAction("Tarif");
        }

        public IActionResult CikisYap()
        {
            return View();
        }
    }
}
