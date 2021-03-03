using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using anbardari;
using anbardari.Models.Repository;
using anbardari.Models.Tools;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace anbardari.Controllers
{
    public class ordersController : Controller
    {
        private Database db = new Database();

        // GET: orders
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
            var currentAnbar = db.tbl_anbar.Where(an => an.Id == currentUser.anbarId).FirstOrDefault();

            int date1=0, date2=0, b=0;
            try
            {
                if (string.IsNullOrEmpty(Request.QueryString["date1"]))
                {
                    date1 = int.Parse(persianDateClass.getPersianDateTime("yyyymmdd"));
                }
                else
                {
                    var d = tools.toEnglishNumber(Request.QueryString["date1"].Replace("/", ""));
                    date1 = int.Parse(d);
                }

                if (string.IsNullOrEmpty(Request.QueryString["date2"]))
                {
                    date2 = int.Parse(persianDateClass.getPersianDateTime("yyyymmdd"));
                }
                else
                {
                    var d = tools.toEnglishNumber(Request.QueryString["date2"].Replace("/", ""));
                    date2 = int.Parse(d);
                }
                
                b = (string.IsNullOrEmpty(Request.QueryString["b"])?0:int.Parse(Request.QueryString["b"]));
            }
            catch (Exception ex)
            {
                date1 = int.Parse(persianDateClass.getPersianDateTime("yyyymmdd"));
                date2 = int.Parse(persianDateClass.getPersianDateTime("yyyymmdd"));
            }
            
            var mod = db.tbl_order
                            .Where(o => 
                                    (!currentUser.admin && !currentUser.anbardar ? 
                                        o.anbar2 == currentAnbar.Id && o.date >= date1 && o.date <= date2 :
                                        (!currentUser.admin && currentUser.anbardar?o.state>1:true) && o.date >= date1 && o.date <= date2 && (b > 0 ? o.anbar2 == b : true))
                            ).OrderByDescending(x => x.Id).ToList();

            ViewBag.currentUser = currentUser;
            ViewBag.currentAnbar = currentAnbar;
            return View(mod);
        }

        // GET: orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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


            tbl_order tbl_order = db.tbl_order.Find(id);

            if (tbl_order == null)
            {
                return HttpNotFound();
            }
            ViewBag.currentUser = currentUser;
            return View(tbl_order);
        }

        // GET: orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,orderId,date,time,anbar1,anbar2,price,state,barcode")] tbl_order tbl_order)
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
            if (!currentUser.admin && !currentUser.anbardar)
            {
                tbl_order.anbar1 = 1;
                if (currentUser.anbarId==null || currentUser.anbarId==0)
                {
                    return View();
                }
                tbl_order.anbar2 = currentUser.anbarId;
                if (string.IsNullOrEmpty(currentUser.pic))
                {
                    Session["alert"] = "danger";
                    Session["message"] = "شما هنوز امضای خود را بارگزاری نکرده اید.برای بارگزاری امضا <a href='/home/profile/"+currentUser.Id+"'>اینجا</a> را کلیک کنید";
                    return View();
                }
            }
            else
            {
                return View();
            }
            var date = Request.Form["date"].ToString();
            var time = Request.Form["time"].ToString();

            ModelState.Remove("date");
            ModelState.Remove("time");
            ModelState.Remove("barcode");
            ModelState.Remove("orderId");

            if (date != "")
                tbl_order.date = int.Parse(tools.toEnglishNumber(date).Replace("/", ""));
            if (time != "")
                tbl_order.time = int.Parse(tools.toEnglishNumber(time).Replace(":", ""));

            tbl_order.barcode = date.Replace("/","") + new Random().Next(10000, 99999).ToString() + time.Replace(":", "");

            int order = 1;
            if (db.tbl_order.ToList().Count > 0)
            {
                order = (int)db.tbl_order.OrderByDescending(x => x.orderId).FirstOrDefault().Id;
            }
            tbl_order.orderId = db.tbl_order.ToList().Count > 0 ? order + 1 : 1;

            if (ModelState.IsValid)
            {

                db.tbl_order.Add(tbl_order);
                var n = db.SaveChanges();

                if(n>0)
                {
                    var orderId = db.tbl_order.Where(x => x.barcode == tbl_order.barcode).First().Id;
                    var productIds = Request.Form["productId"];
                    var productNumbers = Request.Form["productNumber"];

                    var productArray = productIds.Split(',');
                    var numberArray = productNumbers.Split(',');
                    for (int i = 0; i < productArray.Length; i++)
                    {
                        try
                        {
                            var pid = int.Parse(productArray[i]);
                            var product = db.tbl_product.Where(x => x.Id == pid).First();
                            var tblOProduct = new tbl_orderProduct();
                            tblOProduct.description = "";
                            tblOProduct.numberDarkhasti = int.Parse(numberArray[i]);
                            tblOProduct.number = 0;
                            tblOProduct.orderId = orderId;
                            tblOProduct.personId = currentUser.Id;
                            tblOProduct.price = 0;
                            tblOProduct.productId = product.Id;
                            tblOProduct.state = false;

                            db.tbl_orderProduct.Add(tblOProduct);
                            var y = db.SaveChanges();
                            if (y>0)
                            {
                                var isLimited = db.tbl_productAnbar.Where(x => x.productId == product.Id && x.anbarId == currentUser.anbarId).FirstOrDefault();
                                if (isLimited==null)
                                {
                                    var tpa = new tbl_productAnbar();
                                    tpa.anbarId = (int)currentUser.anbarId;
                                    tpa.days = 2;
                                    tpa.lastMonthOrder = int.Parse(persianDateClass.getPersianDateTime("mm"));
                                    tpa.lastYearOrder = int.Parse(persianDateClass.getPersianDateTime("yyyy"));
                                    tpa.maxOrder = product.maxOrder;
                                    tpa.productId = product.Id;

                                    db.tbl_productAnbar.Add(tpa);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    isLimited.lastMonthOrder = int.Parse(persianDateClass.getPersianDateTime("mm"));
                                    isLimited.lastYearOrder = int.Parse(persianDateClass.getPersianDateTime("yyyy"));

                                    db.Entry(isLimited).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                
                return RedirectToAction("Index");
            }

            return View(tbl_order);
        }

        // GET: orders/Edit/5
        public ActionResult printOption()
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

            return View();
        }

        // GET: orders/Edit/5
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_order tbl_order = db.tbl_order.Find(id);
            if (tbl_order == null)
            {
                return HttpNotFound();
            }
            ViewBag.currentUser = currentUser;
            return View(tbl_order);
        }

        // POST: orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,orderId,date,time,anbar1,anbar2,price,state,barcode")] tbl_order tbl_order)
        {
            try
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
                var order = db.tbl_order.Where(o => o.orderId == tbl_order.orderId).FirstOrDefault();
                if (true)
                {
                    order.anbar2 = (currentUser.admin&&tbl_order.anbar2 != null && tbl_order.anbar2 > 0 ? tbl_order.anbar2 : order.anbar2);
                    order.state = tbl_order.state;
                    if (currentUser.anbardar && !currentUser.admin)
                    {
                        order.state = 3;
                    }
                    db.Entry(order).State = EntityState.Modified;
                    var n = db.SaveChanges();

                    if (n > 0)
                    {
                        var productIds = Request.Form["productId"];
                        var productNumbers = Request.Form["productNumber"];
                        var pNumbers = Request.Form["pNumber"];

                        var productArray = productIds.Split(',');
                        var numberDarkhstiArray = productNumbers.Split(',');
                        string[] numberArray=null;
                        if (currentUser.admin || currentUser.anbardar)
                        {
                            numberArray = pNumbers.Split(',');
                        }

                        foreach (var item in (db.tbl_orderProduct.Where(x => x.orderId == tbl_order.orderId).ToList()))
                        {
                            db.Entry(item).State = EntityState.Deleted;
                            var y = db.SaveChanges();
                        } 

                        for (int i = 0; i < productArray.Length; i++)
                        {
                            try
                            {
                                var pid = int.Parse(productArray[i]);
                                var tblOProduct = new tbl_orderProduct();
                                tblOProduct.numberDarkhasti = int.Parse(numberDarkhstiArray[i]);
                                tblOProduct.number = numberArray!=null ? int.Parse(numberArray[i]) : 0;
                                tblOProduct.description = "";
                                tblOProduct.orderId = (int)tbl_order.orderId;
                                tblOProduct.personId = currentUser.Id;
                                tblOProduct.price = 0;
                                tblOProduct.productId = pid;
                                tblOProduct.state = currentUser.admin || currentUser.anbardar ? true : false;

                                db.tbl_orderProduct.Add(tblOProduct);
                                var y = db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    ViewBag.currentUser = currentUser;
                    return Redirect("~/orders/");
                    //return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                return Redirect("~/Home/");
            }
            
        }

        // GET: orders/Delete/5
        public ActionResult Delete(int? id)
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_order tbl_order = db.tbl_order.Find(id);
            if (tbl_order == null)
            {
                return HttpNotFound();
            }
            return View(tbl_order);
        }

        // POST: orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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

            tbl_order tbl_order = db.tbl_order.Find(id);
            db.tbl_order.Remove(tbl_order);
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

        [HttpPost]
        public JsonResult getNumberOfProduct(int productid)
        {
            Database db = new Database();
            var userid = 0;
            var maxOrder = 0;

            try
            {
                userid = int.Parse(Session["userid"].ToString());
                var user = db.tbl_users.Where(u => u.Id == userid).FirstOrDefault();
                var anbarProduct = db.tbl_productAnbar.Where(x => x.anbarId == user.anbarId && 
                                                            x.productId==productid
                                                        ).FirstOrDefault();
                if (anbarProduct==null)
                {
                    var product = db.tbl_product.Where(p => p.Id == productid).FirstOrDefault();
                    maxOrder = (int)product.maxOrder;
                }
                else
                {
                    var days = (int)db.tbl_typeDays.Where(x => x.Id == anbarProduct.days).FirstOrDefault().days;
                    maxOrder = (int)anbarProduct.maxOrder;

                    var currentDate = int.Parse(persianDateClass.getPersianDateTime("yyyymmdd"));
                    var date1 = persianDateClass.dateDecInc(currentDate, days);

                    //var numberOfOrdered = db.tbl_order.Where(x => x.date >= date1 && x.date <= currentDate).Join(
                    //                db.tbl_orderProduct.Where(p => p.Id == productid),
                    //                o => o.Id,
                    //                op => op.orderId,
                    //                (o, op) => new { tbl_order = o, tbl_orderProduct = op }
                    //                ).Select(
                    //                    x=>new {
                    //                        orderid = x.tbl_order.Id,
                    //                        date = x.tbl_order.date,
                    //                        productId = x.tbl_orderProduct.productId,
                    //                        number = x.tbl_orderProduct.number
                    //                    });

                }
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
            return Json(maxOrder);
        }

        [HttpPost]
        public JsonResult saveMaxNumberOfProduct(int productid,int number)
        {
            int res = -1;
            try
            {
                Database db = new Database();
                var userid = int.Parse(Session["userid"].ToString());
                var user = db.tbl_users.Where(u => u.Id == userid).FirstOrDefault();
                var anbarProduct = db.tbl_productAnbar.Where(x => x.anbarId == user.anbarId &&
                                                            x.productId == productid
                                                        ).FirstOrDefault();
                if (anbarProduct==null)
                {
                    var ap = new tbl_productAnbar();
                    ap.days = 2;
                    ap.anbarId = (int)user.anbarId;
                    ap.productId = productid;
                    ap.maxOrder = number;
                    ap.lastYearOrder = int.Parse(persianDateClass.getPersianDateTime("yyyy"));
                    ap.lastMonthOrder = int.Parse(persianDateClass.getPersianDateTime("mm"));
                    db.tbl_productAnbar.Add(ap);
                    res = db.SaveChanges();
                }
                else
                {
                    anbarProduct.maxOrder = number;
                    db.Entry(anbarProduct).State = EntityState.Modified;
                    res = db.SaveChanges();
                }
                return Json(res);
            }
            catch (Exception ex)
            {
                return Json(res);
            }
        }

        [HttpGet]
        public ActionResult productReport()
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

            int date1=0,date2=0,productId=0,anbar2=0;
            try
            {
                date1 = int.Parse(Request.QueryString["date1"].Replace("/",""));
            }
            catch (Exception ex)
            {
            }

            try
            {
                date2 = int.Parse(Request.QueryString["date2"].Replace("/",""));
            }
            catch (Exception ex)
            {
            }
            try
            {
                productId = int.Parse(Request.QueryString["p"]);
            }
            catch (Exception ex)
            {
            }
            try
            {
                if (currentUser.admin || currentUser.anbardar)
                {
                    anbar2 = int.Parse(Request.QueryString["a"]);
                }
                else
                {
                    anbar2 = (int)currentUser.anbarId;
                }
            }
            catch (Exception ex )
            {
            }

            var oc = new orderClass();
            ViewBag.currentModel = oc.getProductInOrders(productId,anbar2,date1,date2);
            return View();
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

                var oid = int.Parse(Request.QueryString["o"]);
                var order = db.tbl_order.Where(x => x.Id == oid).FirstOrDefault();
                if (!currentUser.admin && !currentUser.anbardar && order.anbar2 != currentUser.anbarId)
                {
                    return null;
                }
                var anbardarUser = db.tbl_users.Where(x => x.admin == true).FirstOrDefault();
                var bakhshUser = db.tbl_users.Where(x => x.anbarId==order.anbar2).FirstOrDefault();

                var report = new StiReport();
                var path = Server.MapPath("~/reports/darkhastAnbar.mrt");
                try
                {
                    var size = Request.QueryString["size"].ToString();
                    if (size=="4")
                    {
                        path = Server.MapPath("~/reports/darkhastAnbar_A4.mrt");
                    }
                }
                catch (Exception)
                {
                }
                report.Load(path);

                report.Dictionary.Variables.Add("date", persianDateClass.strToDate(order.date.ToString()));
                report.Dictionary.Variables.Add("orderId", order.Id);

                report.Dictionary.Variables.Add("bakhsh", db.tbl_anbar.Where(x=>x.Id==order.anbar2).FirstOrDefault().subject);
                report.Dictionary.Variables.Add("person1", bakhshUser.name + " " + bakhshUser.family);
                report.Dictionary.Variables.Add("semat1", db.tbl_semat.Where(x=>x.Id== bakhshUser.sematId).FirstOrDefault().subject);
                if(bakhshUser.pic != null && bakhshUser.pic != "" )
                {
                    report.Dictionary.Variables.Add("emza1", getImage(bakhshUser.pic));
                }
                else
                {
                    report.Dictionary.Variables.Add("emza1", getImage("no.jpg"));
                }
                report.Dictionary.Variables.Add("bakhsh2", db.tbl_anbar.Where(x => x.Id == order.anbar1).FirstOrDefault().subject);
                report.Dictionary.Variables.Add("person2", anbardarUser.name + " " + anbardarUser.family);
                report.Dictionary.Variables.Add("semat2", db.tbl_anbar.Where(x => x.Id == anbardarUser.anbarId).FirstOrDefault().subject);
                if(anbardarUser.pic != null && anbardarUser.pic != "" && order.state > 1)
                {
                    report.Dictionary.Variables.Add("emza2", getImage(anbardarUser.pic));
                }
                else
                {
                    report.Dictionary.Variables.Add("emza2", getImage("no.jpg"));
                }
                var dt = new DataTable("DataSource1");

                dt.Columns.Add("row");
                dt.Columns.Add("subject");
                dt.Columns.Add("number");
                dt.Columns.Add("numberF");
                dt.Columns.Add("description");

                var products = db.tbl_orderProduct.Where(x => x.orderId == order.Id).ToList();
                for (int i = 0; i < products.Count; i++)
                {
                    var pid = (int)products[i].productId;
                    var product = db.tbl_product.Where(x => x.Id == pid).FirstOrDefault();
                    dt.Rows.Add(i + 1, product.name, products[i].numberDarkhasti, products[i].number, products[i].description);
                }

                var dataSet = new DataSet("DataSource1");
                dataSet.Tables.Add(dt);
                report.RegData(dataSet);
                return StiMvcViewer.GetReportSnapshotResult(report);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public System.Drawing.Image getImage(string picName)
        {
            var imageAddress = Server.MapPath("~/images/users/") + picName;
            var img = System.Drawing.Bitmap.FromFile(imageAddress);
            MemoryStream ms1,ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            var imgArray = ms.ToArray();

            ms1 = new MemoryStream(imgArray);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms1);

            return image;
        }

        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
    }
}
