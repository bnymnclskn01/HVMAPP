using HVMAPP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HVMAPP.Controllers
{
    public class GorunumController : Controller
    {
        HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();
        // GET: Gorunum
        public ActionResult Detay(int? Id)
        {
            //Response.AddHeader("Refresh", "3600");
            //if (Id==null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            //var ES = db.ElectronicStuffs.Include(x => x.Room).Include(x => x.LiveScreen).Include(x => x.Images).Where(x => x.ID == Id).FirstOrDefault();
            //if (ES == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}

            ViewBag.ID = Id;

            return View();
        }

        public PartialViewResult _DetailPartial(int? Id)
        {

            var ES = db.ElectronicStuffs.Include(x => x.Room).Include(x => x.LiveScreen).Include(x => x.Images).Where(x => x.ID == Id).FirstOrDefault();
          
            return PartialView(ES);
        }
    }
}