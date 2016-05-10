using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TAClassifieds.Models.DAL;

namespace TAClassifieds.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private TAC_Team4Entities db = new TAC_Team4Entities();

        // GET: Admin
        public ActionResult Index()
        {
            var tAC_User = db.TAC_User.Include(t => t.TAC_Country).Include(t => t.TAC_Country1);
            return View(tAC_User.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAC_User tAC_User = db.TAC_User.Find(id);
            if (tAC_User == null)
            {
                return HttpNotFound();
            }
            return View(tAC_User);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.Country = new SelectList(db.TAC_Country, "CountryId", "CountryName");
            ViewBag.Country = new SelectList(db.TAC_Country, "CountryId", "CountryName");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Email,UPassword,First_Name,Last_Name,Gender,DOB,Address1,Address2,City,State,Country,Phone,IsVerified,IsLocked,IsActive,CreatedDate")] TAC_User tAC_User)
        {
            if (ModelState.IsValid)
            {
                tAC_User.UserId = Guid.NewGuid();
                db.TAC_User.Add(tAC_User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Country = new SelectList(db.TAC_Country, "CountryId", "CountryName", tAC_User.Country);
            ViewBag.Country = new SelectList(db.TAC_Country, "CountryId", "CountryName", tAC_User.Country);
            return View(tAC_User);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAC_User tAC_User = db.TAC_User.Find(id);
            if (tAC_User == null)
            {
                return HttpNotFound();
            }
            ViewBag.Country = new SelectList(db.TAC_Country, "CountryId", "CountryName", tAC_User.Country);
            ViewBag.Country = new SelectList(db.TAC_Country, "CountryId", "CountryName", tAC_User.Country);
            return View(tAC_User);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Email,UPassword,First_Name,Last_Name,Gender,DOB,Address1,Address2,City,State,Country,Phone,IsVerified,IsLocked,IsActive,CreatedDate")] TAC_User tAC_User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAC_User).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Country = new SelectList(db.TAC_Country, "CountryId", "CountryName", tAC_User.Country);
            ViewBag.Country = new SelectList(db.TAC_Country, "CountryId", "CountryName", tAC_User.Country);
            return View(tAC_User);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAC_User tAC_User = db.TAC_User.Find(id);
            if (tAC_User == null)
            {
                return HttpNotFound();
            }
            return View(tAC_User);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TAC_User tAC_User = db.TAC_User.Find(id);
            db.TAC_User.Remove(tAC_User);
            db.SaveChanges();
            return RedirectToAction("Index");
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
