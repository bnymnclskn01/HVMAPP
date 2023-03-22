using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HVMAPP.DAL;
using HVMAPP.Entity;

namespace HVMAPP.Areas.HVM.Controllers
{
    public class TabletNewsController : Controller
    {
        private HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();

        // GET: HVM/TabletNews
        public ActionResult Listele()
        {
            var tabletNews = db.TabletNews.Include(t => t.ElectronicStuff);
            return View(tabletNews.ToList());
        }

        // GET: HVM/TabletNews/Create
        public ActionResult Ekle()
        {
            ViewBag.ElectronicStuffID = db.ElectronicStuffs.ToList().OrderBy(x=>x.Name);
            return View();
        }

        // POST: HVM/TabletNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle(int[] EsId, TabletNews tabletNews)
        {
            if (ModelState.IsValid)
            {
                if (EsId.Length > 0)
                {
                    foreach (var id in EsId)
                    {
                        tabletNews.ElectronicStuffId = id;
                        tabletNews.Company = "Avrasya Hospital";
                        tabletNews.UpdateDate = DateTime.Now;
                        db.TabletNews.Add(tabletNews);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Listele", "TabletNews");
                }
                else
                {
                    ViewBag.Mesaj = "En az bir adet seçim yapmak zorundasınız";
                    ViewBag.ElectronicStuffId = new SelectList(db.ElectronicStuffs, "ID", "Name", tabletNews.ElectronicStuffId);
                    return View(tabletNews);
                }
            }
            ViewBag.ElectronicStuffId = new SelectList(db.ElectronicStuffs, "ID", "Name", tabletNews.ElectronicStuffId);
            return View(tabletNews);
        }

        // GET: HVM/TabletNews/Edit/5
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabletNews tabletNews = db.TabletNews.Find(id);
            if (tabletNews == null)
            {
                return HttpNotFound();
            }
            ViewBag.ElectronicStuffId = new SelectList(db.ElectronicStuffs, "ID", "Name", tabletNews.ElectronicStuffId);
            return View(tabletNews);
        }

        // POST: HVM/TabletNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(int Id, TabletNews tabletNews)
        {
            var TN = db.TabletNews.Find(Id);
            if (ModelState.IsValid)
            {
                TN.ID = tabletNews.ID;
                TN.Company = "Avrasya Hospital";
                TN.IsActive = tabletNews.IsActive;
                TN.Note = tabletNews.Note;
                TN.Title = tabletNews.Title;
                TN.UpdateDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Listele");
            }
            ViewBag.ElectronicStuffId = new SelectList(db.ElectronicStuffs, "ID", "Name", tabletNews.ElectronicStuffId);
            return View(tabletNews);
        }

        // GET: HVM/TabletNews/Delete/5
        public ActionResult Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabletNews tabletNews = db.TabletNews.Include(x => x.ElectronicStuff).Where(x => x.ID == id).FirstOrDefault();
            if (tabletNews == null)
            {
                return HttpNotFound();
            }
            return View(tabletNews);
        }

        // POST: HVM/TabletNews/Delete/5
        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TabletNews tabletNews = db.TabletNews.Include(x=>x.ElectronicStuff).Where(x=>x.ID==id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.TabletNews.Remove(tabletNews);
                db.SaveChanges();
                return RedirectToAction("Listele");
            }
            return View(tabletNews);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
