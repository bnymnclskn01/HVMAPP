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
    public class Ekran2Controller : Controller
    {
        private HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();

        // GET: HVM/Ekran2
        public ActionResult Listele()
        {
            var tabletTwos = db.TabletTwos.Include(t => t.Doctor).Include(t => t.LiveScreen);
            return View(tabletTwos.ToList());
        }

        // GET: HVM/Ekran2/Create
        public ActionResult Ekle()
        {
            ViewBag.DoctorID = new SelectList(db.Doctors.Where(x=>x.IsActive==true), "ID", "NameSurname");
            ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x=>x.IsActive==true && x.LiveScreenName== "Tablet-2"), "ID", "LiveScreenName");
            return View();
        }

        // POST: HVM/Ekran2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle(TabletTwo tabletTwo)
        {
            var TT = db.TabletTwos.Where(x => x.DepartmentName == tabletTwo.DepartmentName || x.DoctorID == tabletTwo.DoctorID).FirstOrDefault();
            var RS = db.RoomSettings.ToList();
            if (ModelState.IsValid)
            {
                if (TT != null)
                {
                    ViewBag.Mesaj = "Yeni oluşturmak istediğiniz ekran sistemde mevcut bu yüzden yeni ekleme yapamıyoruz.";
                    ViewBag.DoctorID = new SelectList(db.Doctors.Where(x => x.IsActive == true), "ID", "NameSurname", tabletTwo.DoctorID);
                    ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true && x.LiveScreenName == "Tablet-2"), "ID", "LiveScreenName", tabletTwo.LiveScreenID);
                    return View(tabletTwo);
                }
                else
                {
                    if (RS.FirstOrDefault().Company == tabletTwo.CompanyName)
                    {
                        if (db.TabletTwos.Count() < RS.FirstOrDefault().TabletNumber)
                        {
                            tabletTwo.CompanyName = "Avrasya Hospital";
                            tabletTwo.UpdateDate = DateTime.Now;
                            db.TabletTwos.Add(tabletTwo);
                            db.SaveChanges();
                            return RedirectToAction("Listele");
                        }
                        else
                        {
                            ViewBag.Mesaj = "Maksimum Oda Ekleme Sınırına Ulaştınız";
                            ViewBag.DoctorID = new SelectList(db.Doctors.Where(x => x.IsActive == true), "ID", "NameSurname", tabletTwo.DoctorID);
                            ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true && x.LiveScreenName == "Tablet-2"), "ID", "LiveScreenName", tabletTwo.LiveScreenID);
                            return View(tabletTwo);
                        }
                    }
                    else
                    {
                        ViewBag.Mesaj = "İşlem Sorunla Karşılaştı";
                        ViewBag.DoctorID = new SelectList(db.Doctors.Where(x => x.IsActive == true), "ID", "NameSurname", tabletTwo.DoctorID);
                        ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true && x.LiveScreenName == "Tablet-2"), "ID", "LiveScreenName", tabletTwo.LiveScreenID);
                        return View(tabletTwo);
                    }

                }

            }

            ViewBag.DoctorID = new SelectList(db.Doctors.Where(x => x.IsActive == true), "ID", "NameSurname", tabletTwo.DoctorID);
            ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true && x.LiveScreenName == "Tablet-2"), "ID", "LiveScreenName", tabletTwo.LiveScreenID);
            return View(tabletTwo);
        }

        // GET: HVM/Ekran2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabletTwo tabletTwo = db.TabletTwos.Find(id);
            if (tabletTwo == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameSurname", tabletTwo.DoctorID);
            ViewBag.LiveScreenID = new SelectList(db.LiveScreens, "ID", "LiveScreenName", tabletTwo.LiveScreenID);
            return View(tabletTwo);
        }

        // POST: HVM/Ekran2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TabletTwo tabletTwo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tabletTwo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "ID", "NameSurname", tabletTwo.DoctorID);
            ViewBag.LiveScreenID = new SelectList(db.LiveScreens, "ID", "LiveScreenName", tabletTwo.LiveScreenID);
            return View(tabletTwo);
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
