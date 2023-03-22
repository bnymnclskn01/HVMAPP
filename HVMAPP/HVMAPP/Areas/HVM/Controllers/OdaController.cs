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
    public class OdaController : Controller
    {
        private HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();

        // GET: HVM/Oda
        public ActionResult Listele()
        {
            return View(db.Rooms.ToList());
        }

        // GET: HVM/Oda/Create
        public ActionResult Ekle()
        {
            return View();
        }

        // POST: HVM/Oda/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle(Room room)
        {
            var data = db.Rooms.Where(x => x.Name.ToLower().ToUpper() == room.Name.ToLower().ToUpper()).FirstOrDefault();
            var RS = db.RoomSettings.ToList();
            if (ModelState.IsValid)
            {
                if (data != null)
                {
                    ViewBag.Mesaj = "Sistemde eklemek istediğiniz kayıt olduğu için aynı kayıt için yeni ekleme yapamazsınız.";
                    return View(room);
                }
                else
                {
                    if (db.Rooms.Count() < RS.FirstOrDefault().TabletNumber)
                    {
                        room.UpdateDate = DateTime.Now;
                        db.Rooms.Add(room);
                        db.SaveChanges();
                        return RedirectToAction("Listele");
                    }
                    else
                    {
                        ViewBag.Mesaj = "Maksimum Oda Ekleme Sayısına Ulaştınız";
                    }
                }

            }

            return View(room);
        }

        // GET: HVM/Oda/Edit/5
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: HVM/Oda/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(int Id, Room room)
        {
            var R = db.Rooms.Find(Id);
            var data = db.Rooms.Where(x => x.Name.ToLower().ToUpper() == room.Name.ToLower().ToUpper() && x.ID!=Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (data != null) 
                {
                    ViewBag.Mesaj = "Lütfen var olan bir kayıt listesindeki veriyle aynı veriyi eşlemeye çalışmayın";
                    return View(data);
                }
                else
                {
                    R.ID = room.ID;
                    R.CompanyName = room.CompanyName;
                    R.Description = room.Description;
                    R.IsActive = room.IsActive;
                    R.Name = room.Name;
                    R.UpdateDate = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Listele");
                }

            }
            return View(room);
        }

        // GET: HVM/Oda/Delete/5
        public ActionResult Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: HVM/Oda/Delete/5
        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            var ES = db.ElectronicStuffs.ToList();
            var data = db.Rooms.Where(x => x.ID == ES.FirstOrDefault().RoomID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (data != null) 
                {
                    ViewBag.Mesaj = "Bu Oda Kayıdı Başka Kerde İlişkili Olarak Kullanıldığı için silme işlemi yapamazsınız";
                    return View(data);
                }
                else
                {
                    db.Rooms.Remove(room);
                    db.SaveChanges();
                    return RedirectToAction("Listele");
                }

            }
            return View(room);
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
