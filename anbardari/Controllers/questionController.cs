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
    public class questionController : Controller
    {
        private Database db = new Database();

        // GET: question
        public ActionResult Index()
        {
            return View(db.tbl_question.ToList());
        }

        // GET: question/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_question tbl_question = db.tbl_question.Find(id);
            if (tbl_question == null)
            {
                return HttpNotFound();
            }
            return View(tbl_question);
        }

        // GET: question/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,question,maxRate,personTypeId,active")] tbl_question tbl_question)
        {
            if (ModelState.IsValid)
            {
                db.tbl_question.Add(tbl_question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_question);
        }

        // GET: question/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_question tbl_question = db.tbl_question.Find(id);
            if (tbl_question == null)
            {
                return HttpNotFound();
            }
            return View(tbl_question);
        }

        // POST: question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,question,maxRate,personTypeId,active")] tbl_question tbl_question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_question);
        }

        // GET: question/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_question tbl_question = db.tbl_question.Find(id);
            if (tbl_question == null)
            {
                return HttpNotFound();
            }
            return View(tbl_question);
        }

        // POST: question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_question tbl_question = db.tbl_question.Find(id);
            db.tbl_question.Remove(tbl_question);
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
