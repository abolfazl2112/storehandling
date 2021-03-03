using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using anbardari;
using OfficeOpenXml;

namespace anbardari.Controllers
{
    public class productController : Controller
    {
        private Database db = new Database();

        // GET: product
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
            ViewBag.currentUser = db.tbl_users.Where(x => x.Id == userid).FirstOrDefault();
            return View(db.tbl_product.ToList());
        }

        // GET: product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_product tbl_product = db.tbl_product.Find(id);
            if (tbl_product == null)
            {
                return HttpNotFound();
            }

            return View(tbl_product);
        }

        // GET: product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,anbarId,category,codeKala,name,date1,date2,serialNumber,barcode,temperetureMin,temperetureMax,minOrder,maxOrder,active")] tbl_product tbl_product)
        {

            if (ModelState.IsValid)
            {
                db.tbl_product.Add(tbl_product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_product);
        }

        // GET: product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_product tbl_product = db.tbl_product.Find(id);
            if (tbl_product == null)
            {
                return HttpNotFound();
            }
            return View(tbl_product);
        }

        // POST: product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,anbarId,category,codeKala,name,date1,date2,serialNumber,barcode,temperetureMin,temperetureMax,minOrder,maxOrder,active")] tbl_product tbl_product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_product);
        }

        // GET: product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_product tbl_product = db.tbl_product.Find(id);
            if (tbl_product == null)
            {
                return HttpNotFound();
            }
            return View(tbl_product);
        }

        // POST: product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_product tbl_product = db.tbl_product.Find(id);
            db.tbl_product.Remove(tbl_product);
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

        // GET: product/import
        public ActionResult import()
        {
            return View();
        }

        // POST: product/import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult import(HttpPostedFileBase file)
        {
            string rootFolder = Server.MapPath("~/Uploads");
            string pathFile = "";
            string fileName = "";
            try
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var arr = new string[] { ".xlsx", ".xls" };
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName) && arr.Contains(fileExtension.ToLower()))
                {
                    fileName = file.FileName;
                    pathFile = Path.Combine(rootFolder, fileName);
                    file.SaveAs(pathFile);
                    
                    FileInfo fileInfo = new FileInfo(pathFile);
                    using (ExcelPackage package = new ExcelPackage(fileInfo))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets["products"];
                        int totalRows = workSheet.Dimension.Rows;

                        var nproduct = 0;
                        List<tbl_product> productList = new List<tbl_product>();
                        for (int i = 2; i <= totalRows; i++)
                        {
                            try
                            {
                                //productList.Add(new tbl_product
                                //{
                                //    name = workSheet.Cells[i, 2].Value.ToString(),
                                //    codeKala = workSheet.Cells[i, 3].Value.ToString(),
                                //    active = true,
                                //    maxOrder = 10000,
                                //    anbarId = 0
                                //});
                                var codekala = workSheet.Cells[i, 3].Value.ToString();
                                var namekala = workSheet.Cells[i, 2].Value.ToString();
                                var op = db.tbl_product.Where(x => x.codeKala == codekala).FirstOrDefault();
                                if(op!=null)
                                {
                                    continue;
                                }
                                var p = new tbl_product
                                {
                                    name = namekala,
                                    codeKala = codekala,
                                    active = true,
                                    maxOrder = 10000,
                                    anbarId = 0
                                };
                                db.tbl_product.Add(p);
                                nproduct += db.SaveChanges();
                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var ValidationErrors in e.EntityValidationErrors)
                                {
                                    foreach (var it in ValidationErrors.ValidationErrors)
                                    {
                                        var error = it.ErrorMessage;
                                    }
                                }
                            }
                            catch(Exception ex)
                            {

                            }
                            
                        }

                        //db.tbl_product.AddRange(productList);
                        ViewBag.np = nproduct;
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            ViewBag.np = 0;
            return View();
        }
    }
}
