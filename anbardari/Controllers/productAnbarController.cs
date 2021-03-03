using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using anbardari;
using anbardari.Models.Tools;

namespace anbardari.Controllers
{
    public class productAnbarController : Controller
    {
        private Database db = new Database();

        // GET: productAnbar
        public ActionResult Index()
        {
            return View(db.tbl_productAnbar.ToList());
        }

        // GET: productAnbar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_productAnbar tbl_productAnbar = db.tbl_productAnbar.Find(id);
            if (tbl_productAnbar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_productAnbar);
        }

        // GET: productAnbar/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: productAnbar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,productId,anbarId,number,minOrder,maxOrder,lastYearOrder,lastMonthOrder")] tbl_productAnbar tbl_productAnbar)
        {
            if (ModelState.IsValid)
            {
                db.tbl_productAnbar.Add(tbl_productAnbar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_productAnbar);
        }

        // GET: productAnbar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_productAnbar tbl_productAnbar = db.tbl_productAnbar.Find(id);
            if (tbl_productAnbar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_productAnbar);
        }

        // POST: productAnbar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,productId,anbarId,number,minOrder,maxOrder,lastYearOrder,lastMonthOrder")] tbl_productAnbar tbl_productAnbar)
        {
            if (ModelState.IsValid)
            {
                tbl_productAnbar.lastYearOrder = int.Parse(persianDateClass.getPersianDateTime("yyyy"));
                tbl_productAnbar.lastMonthOrder = int.Parse(persianDateClass.getPersianDateTime("mm"));
                db.Entry(tbl_productAnbar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_productAnbar);
        }

        // GET: productAnbar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_productAnbar tbl_productAnbar = db.tbl_productAnbar.Find(id);
            if (tbl_productAnbar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_productAnbar);
        }

        // POST: productAnbar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_productAnbar tbl_productAnbar = db.tbl_productAnbar.Find(id);
            db.tbl_productAnbar.Remove(tbl_productAnbar);
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
