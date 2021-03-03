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
    public class sematController : Controller
    {
        private Database db = new Database();

        // GET: semat
        public ActionResult Index()
        {
            return View(db.tbl_semat.ToList());
        }

        // GET: semat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_semat tbl_semat = db.tbl_semat.Find(id);
            if (tbl_semat == null)
            {
                return HttpNotFound();
            }
            return View(tbl_semat);
        }

        // GET: semat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: semat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,subject,active")] tbl_semat tbl_semat)
        {
            if (ModelState.IsValid)
            {
                db.tbl_semat.Add(tbl_semat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_semat);
        }

        // GET: semat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_semat tbl_semat = db.tbl_semat.Find(id);
            if (tbl_semat == null)
            {
                return HttpNotFound();
            }
            return View(tbl_semat);
        }

        // POST: semat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,subject,active")] tbl_semat tbl_semat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_semat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_semat);
        }

        // GET: semat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_semat tbl_semat = db.tbl_semat.Find(id);
            if (tbl_semat == null)
            {
                return HttpNotFound();
            }
            return View(tbl_semat);
        }

        // POST: semat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_semat tbl_semat = db.tbl_semat.Find(id);
            db.tbl_semat.Remove(tbl_semat);
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
