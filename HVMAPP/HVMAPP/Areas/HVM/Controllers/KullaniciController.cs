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
    public class KullaniciController : Controller
    {
        private HVMAPPDBCONTEXT db = new HVMAPPDBCONTEXT();

        // GET: HVM/Kullanici
        public ActionResult Listele()
        {
            var userMembers = db.UserMembers.Include(u => u.RoleMember);
            return View(userMembers.ToList());
        }

        // GET: HVM/Kullanici/Create
        public ActionResult Ekle()
        {
            ViewBag.RoleMemberID = new SelectList(db.RoleMembers.Where(x=>x.IsActive==true), "ID", "RoleName");
            return View();
        }

        // POST: HVM/Kullanici/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle(UserMember userMember)
        {
            var data = db.UserMembers.Where(x => x.Username.ToUpper().ToLower() == userMember.Username.ToLower().ToUpper()).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (data!=null)
                {
                    ViewBag.Mesaj = "Böyle bir kullanıcı sistemde mevcut olduğu için yeni ekleme yapamıyoruz.";
                    ViewBag.RoleMemberID = new SelectList(db.RoleMembers.Where(x => x.IsActive == true), "ID", "RoleName", userMember.RoleMemberID);
                    return View(userMember);
                }
                else
                {
                    userMember.UpdateDate = DateTime.Now;
                    db.UserMembers.Add(userMember);
                    db.SaveChanges();
                    return RedirectToAction("Listele");
                }
            }

            ViewBag.RoleMemberID = new SelectList(db.RoleMembers.Where(x => x.IsActive == true), "ID", "RoleName", userMember.RoleMemberID);
            return View(userMember);
        }

        // GET: HVM/Kullanici/Edit/5
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMember userMember = db.UserMembers.Find(id);
            if (userMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleMemberID = new SelectList(db.RoleMembers.Where(x => x.IsActive == true), "ID", "RoleName", userMember.RoleMemberID);
            return View(userMember);
        }

        // POST: HVM/Kullanici/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(int Id, UserMember userMember)
        {
            var UM = db.UserMembers.Find(Id);
            var data = db.UserMembers.Where(x => x.Username.ToUpper().ToLower() == userMember.Username.ToLower().ToUpper() && x.ID != Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (data != null)
                {
                    ViewBag.Mesaj = "Böyle bir kullanıcı sistemde mevcut olduğu için aynı kullanıcı üstüne id'ler tutmadığı için güncelleme yapamıyoruz.";
                    ViewBag.RoleMemberID = new SelectList(db.RoleMembers.Where(x => x.IsActive == true), "ID", "RoleName", userMember.RoleMemberID);
                    return View(userMember);
                }
                else
                {
                    UM.ID = userMember.ID;
                    UM.CompanyName = userMember.CompanyName;
                    UM.IsActive = userMember.IsActive;
                    UM.Name = userMember.Name;
                    UM.Password = userMember.Password;
                    UM.RoleMemberID = userMember.RoleMemberID;
                    UM.Surname = userMember.Surname;
                    UM.UpdateDate = DateTime.Now;
                    UM.Username = userMember.Username;
                    db.SaveChanges();
                    return RedirectToAction("Listele");
                }

            }
            ViewBag.RoleMemberID = new SelectList(db.RoleMembers.Where(x => x.IsActive == true), "ID", "RoleName", userMember.RoleMemberID);
            return View(userMember);
        }

        // GET: HVM/Kullanici/Delete/5
        public ActionResult Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMember userMember = db.UserMembers.Include(x => x.RoleMember).Where(x => x.ID == id).FirstOrDefault();
            if (userMember == null)
            {
                return HttpNotFound();
            }
            return View(userMember);
        }

        // POST: HVM/Kullanici/Delete/5
        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserMember userMember = db.UserMembers.Include(x => x.RoleMember).Where(x => x.ID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.UserMembers.Remove(userMember);
                db.SaveChanges();
                return RedirectToAction("Listele");
            }
            return View(userMember);
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
