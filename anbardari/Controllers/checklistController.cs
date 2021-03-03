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
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace anbardari.Controllers
{
    public class checklistController : Controller
    {
        private Database db = new Database();

        // GET: checklist
        public ActionResult Index()
        {
            int userid = 0;
            try
            {
                userid = int.Parse(Session["userid"].ToString());
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/");
            }
            var currentUser = db.tbl_users.Where(x => x.Id == userid).FirstOrDefault();
            ViewBag.currentUser = currentUser;
            return View(db.tbl_checklist.ToList());
        }

        // GET: checklist/Details/5
        public ActionResult Details()
        {
            int userid = 0;
            try
            {
                userid = int.Parse(Session["userid"].ToString());
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/");
            }
            var currentUser = db.tbl_users.Where(x => x.Id == userid).FirstOrDefault();
            ViewBag.currentUser = currentUser;
            try
            {
                var year = int.Parse(Request.QueryString["y"]);
                var month = int.Parse(Request.QueryString["m"]);
                var bakhshId = int.Parse(Request.QueryString["a"]);
                var sematId = int.Parse(Request.QueryString["p"]);

                ViewBag.currentBakhshId = bakhshId;
                ViewBag.currentSematId = sematId;
                ViewBag.currentMonth = month;
                ViewBag.currentYear = year;

                var model = db.tbl_checklist.Where(x =>
                                        x.bakhshId == bakhshId &&
                                        x.month == month &&
                                        x.sematId == sematId &&
                                        x.year == year
                                      ).ToList();
                //ViewBag.personTypeList;
                //ViewBag.bakhshList;
                //ViewBag.questions;
                //ViewBag.personList;

                return View(model);
            }
            catch (Exception ex)
            {
                return Redirect("~/checklist/");

            }
        }

        // GET: checklist/Create
        public ActionResult Create()
        {
            int userid = 0;
            try
            {
                userid = int.Parse(Session["userid"].ToString());
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/");
            }
            var currentUser = db.tbl_users.Where(x => x.Id == userid).FirstOrDefault();
            ViewBag.currentUser = currentUser;

            int year = string.IsNullOrEmpty(Request.QueryString["y"]) ?
                                        int.Parse(Models.Tools.persianDateClass.getPersianDateTime("yyyy")) :
                                        int.Parse(Request.QueryString["y"]);

            int month = string.IsNullOrEmpty(Request.QueryString["m"]) ?
                                        int.Parse(Models.Tools.persianDateClass.getPersianDateTime("mm")) :
                                        int.Parse(Request.QueryString["m"]);

            int sematId = string.IsNullOrEmpty(Request.QueryString["p"]) ?
                                        0 : int.Parse(Request.QueryString["p"]);

            int bakhshId = currentUser.admin ? (string.IsNullOrEmpty(Request.QueryString["a"]) ?
                                        0 : int.Parse(Request.QueryString["a"])) : (int)currentUser.anbarId;

            ViewBag.currentYear = year;
            ViewBag.currentMonth = month;
            ViewBag.currentBakhshId = bakhshId;
            ViewBag.currentSematId = sematId;

            List<tbl_semat> personTypeList = db.tbl_semat.Where(x => x.active == true && (sematId > 0 && !currentUser.admin ? x.Id == sematId : true)).ToList();
            List<tbl_anbar> bakhshList = db.tbl_anbar.Where(x => x.active == true && (bakhshId > 0 && !currentUser.admin ? x.Id == bakhshId : true)).ToList();
            ViewBag.personTypeList = personTypeList;
            ViewBag.bakhshList = bakhshList;

            var qClass = new Models.Repository.questionClass();
            var questions = qClass.getQuestions(sematId > 0 ? sematId : 0);
            ViewBag.questions = questions;

            ViewBag.personList = db.tbl_personel.Where(x => (sematId!=6? x.bakhshId == bakhshId && x.sematId == sematId:x.bakhshId==0)).ToList();

            return View();
        }

        // POST: checklist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection dataPosetd)
        {
            int userid = 0;
            try
            {
                userid = int.Parse(Session["userid"].ToString());
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/");
            }
            var currentUser = db.tbl_users.Where(x => x.Id == userid).FirstOrDefault();
            ViewBag.currentUser = currentUser;

            //if (ModelState.IsValid)
            //{
            //    db.tbl_checklist.Add(tbl_checklist);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            var inputPerson = Request.Form["inputPerson"];
            var inputRate = Request.Form["inputRate"];
            var inputQuestion = Request.Form["inputQuestion"];

            var personArray = inputPerson.Split(',');
            var rateArray = inputRate.Split(',');
            var questionArray = inputQuestion.Split(',');

            for (int i = 0; i < personArray.Length; i++)
            {
                for (int j = 0; j < questionArray.Length; j++)
                {
                    var newChecklist = new tbl_checklist();
                    newChecklist.questionId = int.Parse(questionArray[j]);
                    newChecklist.rate = int.Parse(rateArray[j]);
                    newChecklist.personId = int.Parse(personArray[i]);

                    newChecklist.month = int.Parse(dataPosetd["month"]);
                    newChecklist.year = int.Parse(dataPosetd["year"]);
                    newChecklist.sematId = int.Parse(dataPosetd["sematId"]);
                    newChecklist.bakhshId = int.Parse(dataPosetd["bakhshId"]);

                    db.tbl_checklist.Add(newChecklist);
                    db.SaveChanges();
                }
            }

            return Create();
        }

        // GET: checklist/Edit/5
        public ActionResult Edit(int? id)
        {
            int userid = 0;
            try
            {
                userid = int.Parse(Session["userid"].ToString());
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/");
            }
            var currentUser = db.tbl_users.Where(x => x.Id == userid).FirstOrDefault();
            ViewBag.currentUser = currentUser;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_checklist tbl_checklist = db.tbl_checklist.Find(id);
            if (tbl_checklist == null)
            {
                return HttpNotFound();
            }
            return View(tbl_checklist);
        }

        // POST: checklist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,bakhshId,sematId,personId,questionId,rate,year,month")] tbl_checklist tbl_checklist)
        {
            int userid = 0;
            try
            {
                userid = int.Parse(Session["userid"].ToString());
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/");
            }
            var currentUser = db.tbl_users.Where(x => x.Id == userid).FirstOrDefault();
            ViewBag.currentUser = currentUser;

            if (ModelState.IsValid)
            {
                db.Entry(tbl_checklist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_checklist);
        }

        // GET: checklist/Delete/5
        //public ActionResult Delete()
        //{
        //    int userid = 0;
        //    try
        //    {
        //        userid = int.Parse(Session["userid"].ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Redirect("~/Home/");
        //    }
        //    var currentUser = db.tbl_users.Where(x => x.Id == userid).FirstOrDefault();
        //    if(!currentUser.admin)
        //    {
        //        return Redirect("~/Home/");
        //    }
        //    ViewBag.currentUser = currentUser;

        //    var year = int.Parse(Request.QueryString["y"]);
        //    var month = int.Parse(Request.QueryString["m"]);
        //    var sematId = int.Parse(Request.QueryString["p"]);
        //    var bakhshId = int.Parse(Request.QueryString["a"]);

        //    ViewBag.year = year;
        //    ViewBag.month = month;
        //    ViewBag.sematId = sematId;
        //    ViewBag.bakhshId = bakhshId;

        //    return View();
        //}

        // POST: checklist/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete()
        {
            int userid = 0;
            try
            {
                userid = int.Parse(Session["userid"].ToString());
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/");
            }
            try
            {
                var currentUser = db.tbl_users.Where(x => x.Id == userid).FirstOrDefault();
                if(!currentUser.admin)
                { return Redirect("~/Home/"); }
                //ViewBag.currentUser = currentUser;

                var year = int.Parse(Request.QueryString["y"]);
                var month = int.Parse(Request.QueryString["m"]);
                var sematId = int.Parse(Request.QueryString["p"]);
                var bakhshId = int.Parse(Request.QueryString["a"]);

                var checklist = db.tbl_checklist.Where(x => x.bakhshId == bakhshId && x.month == month && x.sematId == sematId && x.year == year).ToList();
                if (checklist == null)
                {
                    return HttpNotFound();
                }
                foreach (var item in checklist)
                {
                    db.tbl_checklist.Remove(item);
                    db.SaveChanges();
                }
            
            }
            catch (Exception ex)
            {
            }
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

        public ActionResult print()
        {
            return View();
        }

        public ActionResult GetReport()
        {
            try
            {
                Database db = new Database();
                var userid = int.Parse(Session["userid"].ToString());
                var currentUser = db.tbl_users.Where(u => u.Id == userid).FirstOrDefault();

                var year = int.Parse(Request.QueryString["y"]);
                var month = int.Parse(Request.QueryString["m"]);
                var bakhshId = int.Parse(Request.QueryString["a"]);
                var sematId = int.Parse(Request.QueryString["p"]);

                var checklist = db.tbl_checklist.Where(x => 
                                                        x.year == year&&
                                                        x.month==month&&
                                                        x.bakhshId==bakhshId&&
                                                        x.sematId==sematId
                                                      ).ToList();

                if (!currentUser.admin && currentUser.anbarId != currentUser.anbarId)
                {
                    return null;
                }

                var report = new StiReport();
                var path = Server.MapPath("~/reports/arzyabi.mrt");
                report.Load(path);

                report.Dictionary.Variables.Add("date", tools.toShamsiMonthName(month) + " " + year.ToString());

                var dt = new DataTable("DataSource1");

                dt.Columns.Add("row");
                dt.Columns.Add("question");
                dt.Columns.Add("name");
                dt.Columns.Add("rate");

                for (int i = 0; i < checklist.Count; i++)
                {
                    var rate = (int)checklist[i].rate;
                    var pid = checklist[i].personId;
                    var qid = checklist[i].questionId;
                    var person = db.tbl_personel.Where(x => x.id == pid).FirstOrDefault();
                    var question = db.tbl_question.Where(x=>x.id==qid).FirstOrDefault();
                    dt.Rows.Add(i + 1, question.question, person.name+" "+person.family, checklist[i].rate);
                }

                var dataSet = new DataSet("DataSource1");
                dataSet.Tables.Add(dt);
                report.RegData(dataSet);
                
                return StiMvcViewer.GetReportSnapshotResult(report);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
    }
}
