using HVMAPP.DAL;
using HVMAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace HVMAPP.Controllers
{
    public class AnasayfaController : Controller
    {
        HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();
        [Route("")]
        [Obsolete]
        // GET: Anasayfa
        public ActionResult Index()
        {
            var ipAdress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAdress))
            {
                ipAdress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            ViewBag.İP = ipAdress;
            var veri = db.ElectronicStuffs.Where(x => x.IpAddress == ipAdress).FirstOrDefault();
            if (ModelState.IsValid)
            {

                if (veri.IpAddress == ipAdress)
                {
                    string URL = "~/Gorunum/Detay/" + veri.ID;
                    Response.Redirect(URL);
                    return View(URL);
                }
                else
                {
                    return RedirectToAction("Index", "Anasayfa");
                }
            }
            return View(veri);
        }
    }
}


#region Test Denemeler
//string bilgisayarAdi = Dns.GetHostName();
//string ipAdresi = Dns.GetHostByName(bilgisayarAdi).AddressList[0].ToString();
//var veri = db.ElectronicStuffs.Where(x=>x.IpAddress==ipAdresi).FirstOrDefault();
//if (veri.IpAddress == ipAdresi)
//{
//    string URL = "~/Gorunum/Detay/" + veri.ID;
//    Response.Redirect(URL);
//    return View(URL);
//}
//else
//{
//    return RedirectToAction("Login", "Home");
//}

//if (ModelState.IsValid)
//{
//    string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
//    string hostName = Dns.GetHostName();
//    if (!hostName.EndsWith(domainName))
//    {
//        hostName += domainName;
//    }
//    var veri = db.ElectronicStuffs.Where(x => x.IpAddress == hostName).FirstOrDefault();
//    if (veri.IpAddress == hostName)
//    {
//        string URL = "~/Gorunum/Detay/" + veri.ID;
//        Response.Redirect(URL);
//        return View(URL);
//    }
//    else
//    {
//        return RedirectToAction("Login", "Home");
//    }
//}
#endregion