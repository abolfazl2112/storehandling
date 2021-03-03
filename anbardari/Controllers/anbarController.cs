using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using anbardari;

namespace anbardari.Controllers
{
    public class anbarController : Controller
    {
        private Database db = new Database();

        // GET: anbar
        public ActionResult Index()
        {
            return View(db.tbl_anbar.ToList());
        }

        // GET: anbar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_anbar tbl_anbar = db.tbl_anbar.Find(id);
            if (tbl_anbar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_anbar);
        }

        // GET: anbar/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: anbar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,subject,active")] tbl_anbar tbl_anbar)
        {
            if (ModelState.IsValid)
            {
                db.tbl_anbar.Add(tbl_anbar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_anbar);
        }

        // GET: anbar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_anbar tbl_anbar = db.tbl_anbar.Find(id);
            if (tbl_anbar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_anbar);
        }

        // POST: anbar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,subject,active")] tbl_anbar tbl_anbar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_anbar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_anbar);
        }

        // GET: anbar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_anbar tbl_anbar = db.tbl_anbar.Find(id);
            if (tbl_anbar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_anbar);
        }

        // POST: anbar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_anbar tbl_anbar = db.tbl_anbar.Find(id);
            db.tbl_anbar.Remove(tbl_anbar);
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
