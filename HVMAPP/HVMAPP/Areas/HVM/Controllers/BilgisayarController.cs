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
    public class BilgisayarController : Controller
    {
        private HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();

        // GET: HVM/Bilgisayar
        public ActionResult Listele()
        {
            var electronicStuffs = db.ElectronicStuffs.Include(e => e.LiveScreen).Include(e => e.Room);
            return View(electronicStuffs.ToList());
        }

        public ActionResult Detay(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ElectronicStuff electronicStuff = db.ElectronicStuffs.Include(e => e.LiveScreen).Include(x=>x.Images).Include(e => e.Room).Where(x => x.ID == ID).FirstOrDefault();
            if (electronicStuff == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(electronicStuff);
        }

        // GET: HVM/Bilgisayar/Create
        public ActionResult Ekle()
        {
            
            ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true && x.LiveScreenName=="Tablet"), "ID", "LiveScreenName");
            ViewBag.RoomID = new SelectList(db.Rooms.Where(x => x.IsActive == true), "ID", "Name");
            ElectronicStuff electronicStuff = new ElectronicStuff();
            var LstES = db.ElectronicStuffs.OrderByDescending(c => c.ID).FirstOrDefault();
            return View(electronicStuff);
        }

        // POST: HVM/Bilgisayar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle(ElectronicStuff electronicStuff)
        {
            var data = db.ElectronicStuffs.Where(x => x.Name == electronicStuff.Name ||  x.RoomID==electronicStuff.RoomID).FirstOrDefault();
            var RS = db.RoomSettings.ToList();
            if (ModelState.IsValid)
            {

                if (data != null)
                {
                    ViewBag.Mesaj = "Yeni oluşturmak istediğiniz ekran sistemde mevcut bu yüzden yeni ekleme yapamıyoruz.";
                    ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true), "ID", "LiveScreenName", electronicStuff.LiveScreenID);
                    ViewBag.RoomID = new SelectList(db.Rooms.Where(x => x.IsActive == true), "ID", "Name", electronicStuff.RoomID);
                    return View(electronicStuff);
                }
                else
                {
                    if (RS.FirstOrDefault().Company==electronicStuff.CompanyName)
                    {
                        if (db.ElectronicStuffs.Count() < RS.FirstOrDefault().TabletNumber)
                        {
                            electronicStuff.UpdateDate = DateTime.Now;
                            db.ElectronicStuffs.Add(electronicStuff);
                            db.SaveChanges();
                            return RedirectToAction("Listele");
                        }
                        else
                        {
                            ViewBag.Mesaj = "Maksimum Oda Ekleme Sınırına Ulaştınız";
                            ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true && x.LiveScreenName == "Tablet"), "ID", "LiveScreenName", electronicStuff.LiveScreenID);
                            ViewBag.RoomID = new SelectList(db.Rooms.Where(x => x.IsActive == true), "ID", "Name", electronicStuff.RoomID);
                            return View(electronicStuff);
                        }
                    }
                    else
                    {
                        ViewBag.Mesaj = "İşlem Sorunla Karşılaştı";
                        ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true), "ID", "LiveScreenName", electronicStuff.LiveScreenID);
                        ViewBag.RoomID = new SelectList(db.Rooms.Where(x => x.IsActive == true), "ID", "Name", electronicStuff.RoomID);
                        return View(electronicStuff);
                    }
                }
            }
            ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true && x.LiveScreenName == "Tablet"), "ID", "LiveScreenName", electronicStuff.LiveScreenID);
            ViewBag.RoomID = new SelectList(db.Rooms.Where(x => x.IsActive == true), "ID", "Name", electronicStuff.RoomID);
            return View(electronicStuff);
        }

        // GET: HVM/Bilgisayar/Edit/5
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ElectronicStuff electronicStuff = db.ElectronicStuffs.Find(id);
            if (electronicStuff == null)
            {
                return HttpNotFound();
            }
            ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true && x.LiveScreenName == "Tablet"), "ID", "LiveScreenName", electronicStuff.LiveScreenID);
            ViewBag.RoomID = new SelectList(db.Rooms.Where(x => x.IsActive == true), "ID", "Name", electronicStuff.RoomID);
            return View(electronicStuff);
        }

        // POST: HVM/Bilgisayar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(int Id,ElectronicStuff electronicStuff)
        {
            var ES = db.ElectronicStuffs.Include(x=>x.LiveScreen).Include(x=>x.Room).Where(x=>x.ID==Id).FirstOrDefault();
            var data = db.ElectronicStuffs.Where(x => x.Name == electronicStuff.Name  && x.RoomID == electronicStuff.RoomID && x.ID != Id).SingleOrDefault();
            var RS = db.RoomSettings.ToList();
            if (ModelState.IsValid)
            {
                if (data != null)
                {
                    ViewBag.Mesaj = "Güncellemek istediğiniz ekran bilgileri başka bir kayıtta mevcut olduğu için başka id ile üzerine yazma gerçekleştiremezsiniz.";
                    ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true && x.LiveScreenName == "Tablet"), "ID", "LiveScreenName", electronicStuff.LiveScreenID);
                    ViewBag.RoomID = new SelectList(db.Rooms.Where(x => x.IsActive == true), "ID", "Name", electronicStuff.RoomID);
                    return View(data);
                }
                else
                {
                    if (RS.FirstOrDefault().Company == electronicStuff.CompanyName)
                    {
                        if (db.ElectronicStuffs.Count() < RS.FirstOrDefault().TabletNumber || db.ElectronicStuffs.Count() < RS.FirstOrDefault().TV || db.ElectronicStuffs.Count() < RS.FirstOrDefault().VideoWallNumber)
                        {
                            ES.ID = electronicStuff.ID;
                            ES.CompanyName = electronicStuff.CompanyName;
                            ES.IdentityNumber = electronicStuff.IdentityNumber;
                            ES.IsActive = electronicStuff.IsActive;
                            ES.LiveScreenID = electronicStuff.LiveScreenID;
                            ES.Name = electronicStuff.Name;
                            ES.RoomID = electronicStuff.RoomID;
                            ES.UpdateDate = DateTime.Now;
                            ES.ZipCode = electronicStuff.ZipCode;
                            ES.IpAddress = electronicStuff.IpAddress;
                            db.SaveChanges();
                            return RedirectToAction("Listele");
                        }
                        else
                        {
                            ViewBag.Mesaj = "İşlemde Hata Oluştu";
                            ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true && x.LiveScreenName == "Tablet"), "ID", "LiveScreenName", electronicStuff.LiveScreenID);
                            ViewBag.RoomID = new SelectList(db.Rooms.Where(x => x.IsActive == true), "ID", "Name", electronicStuff.RoomID);
                            return View(electronicStuff);
                        }
                    }
                }

            }
            ViewBag.LiveScreenID = new SelectList(db.LiveScreens.Where(x => x.IsActive == true), "ID", "LiveScreenName", electronicStuff.LiveScreenID);
            ViewBag.RoomID = new SelectList(db.Rooms.Where(x => x.IsActive == true), "ID", "Name", electronicStuff.RoomID);
            return View(electronicStuff);
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
