using HVMAPP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HVMAPP.Areas.HVM.Controllers
{
    public class GostergePaneliController : Controller
    {
        HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();
        // GET: HVM/GostergePaneli
        public ActionResult Index()
        {
            ViewBag.ToplamEkran = db.RoomSettings.Where(x => x.Company == "Avrasya Hospital").FirstOrDefault();
            ViewBag.KullanilanEkran = db.ElectronicStuffs.Where(x => x.CompanyName == "Avrasya Hospital" && x.IsActive==true).Count();
            ViewBag.KullanilmayanEkran = ViewBag.ToplamEkran.TabletNumber - ViewBag.KullanilanEkran;
            return View();
        }
    }
}