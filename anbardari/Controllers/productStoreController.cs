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
    public class productStoreController : Controller
    {
        private Database db = new Database();

        // GET: productStore
        public ActionResult Index()
        {
            int a = 0, p = 0;
            if (Request.QueryString.AllKeys.Contains("a"))
            {
                a = int.Parse(Request.QueryString["a"]);
            }
            if (Request.QueryString.AllKeys.Contains("p"))
            {
                p = int.Parse(Request.QueryString["p"]);
            }

            return View(db.tbl_productAnbar.Where(
                x => (a == 0 ? true : x.anbarId == a) && (p == 0 ? true : x.productId == p)
                ).ToList());
        }

        // GET: productStore/Details/5
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

        // GET: productStore/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: productStore/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,productId,anbarId,number,minOrder,maxOrder,ghafase,location,days")] tbl_productAnbar tbl_productAnbar)
        {
            if (ModelState.IsValid)
            {
                db.tbl_productAnbar.Add(tbl_productAnbar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_productAnbar);
        }

        // GET: productStore/Edit/5
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

        // POST: productStore/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,productId,anbarId,number,minOrder,maxOrder,ghafase,location,days")] tbl_productAnbar tbl_productAnbar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_productAnbar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_productAnbar);
        }

        // GET: productStore/Delete/5
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

        // POST: productStore/Delete/5
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
