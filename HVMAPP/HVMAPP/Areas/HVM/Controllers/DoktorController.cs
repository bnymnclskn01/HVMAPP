using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using HVMAPP.DAL;
using HVMAPP.Entity;

namespace HVMAPP.Areas.HVM.Controllers
{
    public class DoktorController : Controller
    {
        private HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();

        // GET: HVM/Doktor
        public ActionResult Listele()
        {
            return View(db.Doctors.ToList());
        }

        // GET: HVM/Doktor/Create
        public ActionResult Ekle()
        {
            return View();
        }

        // POST: HVM/Doktor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle(Doctor doctor, HttpPostedFileBase ImageUrl)
        {
            var data = db.Doctors.Where(x => x.NameSurname.ToLower().ToUpper() == doctor.NameSurname.ToLower().ToUpper()).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (data != null)
                {
                    ViewBag.Mesaj = "Lütfen yeni kayıt oluştururken var olan bir kayıtı tekrar eklemek için çalışmayınız.";
                    return View(doctor);
                }
                else
                {
                    if (ImageUrl!=null)
                    {
                        WebImage img = new WebImage(ImageUrl.InputStream);
                        FileInfo imgdoctor = new FileInfo(ImageUrl.FileName);
                        string doctorimg = Guid.NewGuid().ToString() + imgdoctor.Extension;
                        img.Resize(300, 300, false, false);
                        img.Save("~/Resimler/Doctor/" + doctorimg);
                        doctor.Image = "/Resimler/Doctor/" + doctorimg;
                    }
                    doctor.UpdateDate = DateTime.Now;
                    db.Doctors.Add(doctor);
                    db.SaveChanges();
                    return RedirectToAction("Listele");
                }

            }

            return View(doctor);
        }

        // GET: HVM/Doktor/Edit/5
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: HVM/Doktor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(int Id, Doctor doctor,HttpPostedFileBase ImageUrl)
        {
            var D = db.Doctors.Find(Id);
            var data = db.Doctors.Where(x => x.NameSurname.ToLower().ToUpper() == doctor.NameSurname.ToLower().ToUpper() && x.ID!=Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (data != null)
                {
                    ViewBag.Mesaj = "Güncelleme yapmak istediğiniz veri başka kayıtta eşleşiyor eşleştiği için verinizi güncelleyemiyoruz.";
                }
                else
                {
                    if (ImageUrl != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath(D.Image)))
                        {
                            System.IO.File.Delete(Server.MapPath(D.Image));
                        }
                        WebImage img = new WebImage(ImageUrl.InputStream);
                        FileInfo imgdoctor = new FileInfo(ImageUrl.FileName);
                        string doctorimg = Guid.NewGuid().ToString() + imgdoctor.Extension;
                        img.Resize(300, 300, false, false);
                        img.Save("~/Resimler/Doctor/" + doctorimg);
                        D.Image = "/Resimler/Doctor/" + doctorimg;
                    }
                    D.CompanyName = doctor.CompanyName;
                    D.ID = doctor.ID;
                    D.IsActive = doctor.IsActive;
                    D.NameSurname = doctor.NameSurname;
                    D.Profession = doctor.Profession;
                    db.SaveChanges();
                    return RedirectToAction("Listele");
                }
            }
            return View(doctor);
        }

        // GET: HVM/Doktor/Delete/5
        public ActionResult Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: HVM/Doktor/Delete/5
        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var ES = db.ElectronicStuffs.ToList();
            Doctor doctor = db.Doctors.Find(id);
            if (ModelState.IsValid)
            {
                if (System.IO.File.Exists(Server.MapPath(doctor.Image)))
                {
                    System.IO.File.Delete(Server.MapPath(doctor.Image));
                }
                db.Doctors.Remove(doctor);
                db.SaveChanges();
                return RedirectToAction("Listele");
            }
            return View(doctor);
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
