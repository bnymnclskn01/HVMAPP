using HVMAPP.DAL;
using HVMAPP.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HVMAPP.Controllers
{
    public class HomeController : Controller
    {
        HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();
        public ActionResult Login()
        {
            HttpCookie cookie = Request.Cookies["LoginScreen"];
            if (cookie != null)
            {
                ViewBag.ZipCode = cookie["ZipCode"].ToString();
                ViewBag.IdentityNumber = cookie["IdentityNumber"].ToString();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(ElectronicStuff loginScreen)
        {
            HttpCookie cookie = new HttpCookie("LoginScreen");
            if (ModelState.IsValid == true)
            {
                if (loginScreen.IsActive == true)
                {
                    cookie["IdentityNumber"] = loginScreen.IdentityNumber;
                    cookie["ZipCode"] = loginScreen.ZipCode;
                    cookie.Expires = DateTime.Now.AddDays(100);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                var row = db.ElectronicStuffs.Where(x => loginScreen.ZipCode == x.ZipCode && loginScreen.IdentityNumber == x.IdentityNumber).FirstOrDefault();
                if (row != null)
                {
                    Session["ZipCode"] = loginScreen.ZipCode;
                    Session["IdentityNumber"] = loginScreen.IdentityNumber;
                    var ES = db.ElectronicStuffs.Where(x => x.ZipCode == loginScreen.ZipCode).FirstOrDefault();
                    string URL = "/Gorunum/Detay/" + ES.ID;
                    Response.Redirect(URL);
                }
                else
                {
                    ViewBag.Mesaj = "Lütfen Doğru Bilgileri Giriniz";
                    return View(loginScreen);
                }
            }
            return View(loginScreen);
        }
    }
}