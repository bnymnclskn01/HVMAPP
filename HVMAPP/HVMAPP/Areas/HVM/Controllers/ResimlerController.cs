using HVMAPP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HVMAPP.Entity;
using System.IO;
using System.Web.Helpers;
using System.Net;
using System.Web.Razor.Generator;

namespace HVMAPP.Areas.HVM.Controllers
{
    public class ResimlerController : Controller
    {
        HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();
        // GET: HVM/Resimler
        public ActionResult Listele()
        {
            return View(db.Images.Include(x=>x.ElectronicStuff).ToList());
        }
        public ActionResult Ekle()
        {
            ViewBag.ElectronicStuffID = new SelectList(db.ElectronicStuffs.Where(x => x.IsActive == true), "ID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle(IEnumerable<HttpPostedFileBase> ImageUrl,Image img)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in ImageUrl)
                {
                    string photoName = Path.GetFileName(item.FileName);
                    var url = Path.Combine(Server.MapPath("~/Resimler/" + item.FileName));
                    item.SaveAs(url);
                    img.CompanyName = "Avrasya Hospital";
                    img.ImageUrl = photoName;
                    db.Images.Add(img);
                    db.SaveChanges();
                }
                return RedirectToAction("Listele", "Resimler");
            }
            ViewBag.ElectronicStuffID = new SelectList(db.ElectronicStuffs.Where(x => x.IsActive == true), "ID", "Name", img.ElectronicStuffID);
            return View(img);
        }
        public ActionResult CokluEkle()
        {
            ViewBag.ElectronicStuffID = db.ElectronicStuffs.Include(x=>x.Room).ToList().OrderBy(x=>x.Name);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CokluEkle(int[] EsId, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                foreach (int id in EsId)
                {
                    Image electronicStuffImage = new Image();
                    electronicStuffImage.ElectronicStuffID = id;
                    string photoName = Path.GetFileName(Guid.NewGuid().ToString()+ImageUrl.FileName);
                    var url = Path.Combine(Server.MapPath("~/Resimler/" + photoName));
                    ImageUrl.SaveAs(url);
                    electronicStuffImage.CompanyName = "Avrasya Hospital";
                    electronicStuffImage.ImageUrl = photoName;
                    db.Images.Add(electronicStuffImage);
                    db.SaveChanges();
                }
                return RedirectToAction("Listele", "Resimler");
            }
            ViewBag.ElectronicStuffID = db.ElectronicStuffs.ToList();
            return View();
        }
        // GET: HVM/Images/Edit/5
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            ViewBag.ElectronicStuffID = new SelectList(db.ElectronicStuffs, "ID", "Name", image.ElectronicStuffID);
            return View(image);
        }

        // POST: HVM/Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(int ID, Image image,HttpPostedFileBase ImageUrl)
        {
            var images = db.Images.Find(ID);
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Resimler/" + images.ImageUrl)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Resimler/" + images.ImageUrl));
                    }
                    string photoName = Path.GetFileName(Guid.NewGuid().ToString() + ImageUrl.FileName);
                    var url = Path.Combine(Server.MapPath("~/Resimler/" + photoName));
                    ImageUrl.SaveAs(url);
                    images.ImageUrl = photoName;
                    images.ID = image.ID;
                    images.ElectronicStuffID = image.ElectronicStuffID;
                    image.CompanyName = image.CompanyName;
                    db.SaveChanges();
                    return RedirectToAction("Listele");
                }

            }
            ViewBag.ElectronicStuffID = new SelectList(db.ElectronicStuffs, "ID", "Name", image.ElectronicStuffID);
            return View(image);
        }
        public ActionResult Sil(int Id)
        {
            if (ModelState.IsValid)
            {
                var Images = db.Images.Include(x => x.ElectronicStuff).Where(x => x.ID == Id).FirstOrDefault();
                if (System.IO.File.Exists(Server.MapPath("~/Resimler/" + Images.ImageUrl)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Resimler/" + Images.ImageUrl));
                }
                db.Images.Remove(Images);
                db.SaveChanges();
                return RedirectToAction("Listele", "Resimler");
            }
            return View();
        }
    }
}