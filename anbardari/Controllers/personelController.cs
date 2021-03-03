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
    public class personelController : Controller
    {
        private Database db = new Database();

        // GET: personel
        public ActionResult Index()
        {
            return View(db.tbl_personel.ToList());
        }

        // GET: personel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_personel tbl_personel = db.tbl_personel.Find(id);
            if (tbl_personel == null)
            {
                return HttpNotFound();
            }
            return View(tbl_personel);
        }

        // GET: personel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: personel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,family,father,meli,bdate,pic,emza,sematId,bakhshId,active")] tbl_personel tbl_personel)
        {
            if (ModelState.IsValid)
            {
                db.tbl_personel.Add(tbl_personel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_personel);
        }

        // GET: personel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_personel tbl_personel = db.tbl_personel.Find(id);
            if (tbl_personel == null)
            {
                return HttpNotFound();
            }
            return View(tbl_personel);
        }

        // POST: personel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,family,father,meli,bdate,pic,emza,sematId,bakhshId,active")] tbl_personel tbl_personel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_personel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_personel);
        }

        // GET: personel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_personel tbl_personel = db.tbl_personel.Find(id);
            if (tbl_personel == null)
            {
                return HttpNotFound();
            }
            return View(tbl_personel);
        }

        // POST: personel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_personel tbl_personel = db.tbl_personel.Find(id);
            db.tbl_personel.Remove(tbl_personel);
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
