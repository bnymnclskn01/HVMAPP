using HVMAPP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HVMAPP.Areas.HVM.Controllers
{
    public class HVMGirisController : Controller
    {
        HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();
        // GET: HVM/HVMGiris
        public ActionResult GirisYap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GirisYap(string Username, string Password)
        {
            var data = db.UserMembers.Include(x=>x.RoleMember).Where(x => x.Username.ToLower().ToUpper() == Username.ToLower().ToUpper() && x.Password.ToLower().ToUpper() == Password.ToLower().ToUpper() && x.IsActive == true).ToList();
            if (data.Count == 1)
            {
                Session["HvmGiris"] = data.FirstOrDefault();
                return RedirectToAction("Index", "GostergePaneli");
            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı Adı Şifre Hatalı veya Aktif Durumunuz Onaylanmamıştır.";
                return View(data);
            }
        }
        public ActionResult CikisYap()
        {
            Session["HvmGiris"] = null;
            Session.Abandon();
            return RedirectToAction("GirisYap", "HVMGiris");
        }
    }
}