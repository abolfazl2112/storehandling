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
    public class orderDetailsController : Controller
    {
        private Database db = new Database();

        // GET: orderDetails
        public ActionResult Index()
        {
            return View(db.tbl_orderProduct.ToList());
        }

        // GET: orderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_orderProduct tbl_orderProduct = db.tbl_orderProduct.Find(id);
            if (tbl_orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(tbl_orderProduct);
        }

        // GET: orderDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: orderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,productId,personId,number,price,description,state")] tbl_orderProduct tbl_orderProduct)
        {
            if (ModelState.IsValid)
            {
                db.tbl_orderProduct.Add(tbl_orderProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_orderProduct);
        }

        // GET: orderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_orderProduct tbl_orderProduct = db.tbl_orderProduct.Find(id);
            if (tbl_orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(tbl_orderProduct);
        }

        // POST: orderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,productId,personId,number,price,description,state")] tbl_orderProduct tbl_orderProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_orderProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_orderProduct);
        }

        // GET: orderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_orderProduct tbl_orderProduct = db.tbl_orderProduct.Find(id);
            if (tbl_orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(tbl_orderProduct);
        }

        // POST: orderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_orderProduct tbl_orderProduct = db.tbl_orderProduct.Find(id);
            db.tbl_orderProduct.Remove(tbl_orderProduct);
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
