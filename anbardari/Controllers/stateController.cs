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
    public class stateController : Controller
    {
        private Database db = new Database();

        // GET: state
        public ActionResult Index()
        {
            return View(db.tbl_state.ToList());
        }

        // GET: state/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_state tbl_state = db.tbl_state.Find(id);
            if (tbl_state == null)
            {
                return HttpNotFound();
            }
            return View(tbl_state);
        }

        // GET: state/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: state/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,subject,active")] tbl_state tbl_state)
        {
            if (ModelState.IsValid)
            {
                db.tbl_state.Add(tbl_state);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_state);
        }

        // GET: state/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_state tbl_state = db.tbl_state.Find(id);
            if (tbl_state == null)
            {
                return HttpNotFound();
            }
            return View(tbl_state);
        }

        // POST: state/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,subject,active")] tbl_state tbl_state)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_state).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_state);
        }

        // GET: state/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_state tbl_state = db.tbl_state.Find(id);
            if (tbl_state == null)
            {
                return HttpNotFound();
            }
            return View(tbl_state);
        }

        // POST: state/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_state tbl_state = db.tbl_state.Find(id);
            db.tbl_state.Remove(tbl_state);
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
