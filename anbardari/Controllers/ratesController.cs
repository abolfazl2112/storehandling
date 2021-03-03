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
    public class ratesController : Controller
    {
        private Database db = new Database();

        // GET: rates
        public ActionResult Index()
        {

            return View(db.tbl_rates.ToList());
        }

        // GET: rates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_rates tbl_rates = db.tbl_rates.Find(id);
            if (tbl_rates == null)
            {
                return HttpNotFound();
            }
            return View(tbl_rates);
        }

        // GET: rates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: rates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,subject,rate,active")] tbl_rates tbl_rates)
        {
            if (ModelState.IsValid)
            {
                db.tbl_rates.Add(tbl_rates);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_rates);
        }

        // GET: rates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_rates tbl_rates = db.tbl_rates.Find(id);
            if (tbl_rates == null)
            {
                return HttpNotFound();
            }
            return View(tbl_rates);
        }

        // POST: rates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,subject,rate,active")] tbl_rates tbl_rates)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_rates).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_rates);
        }

        // GET: rates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_rates tbl_rates = db.tbl_rates.Find(id);
            if (tbl_rates == null)
            {
                return HttpNotFound();
            }
            return View(tbl_rates);
        }

        // POST: rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_rates tbl_rates = db.tbl_rates.Find(id);
            db.tbl_rates.Remove(tbl_rates);
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
