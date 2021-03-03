using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace anbardari.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var username = form["username"];
            var password = form["password"];

            var db = new Database();

            tbl_users user;
            try
            {
                user = db.tbl_users.Where(x => x.username == username && x.password == password && x.active == true).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return View();
            }
            if (user != null)
            {
                Session["userid"] = user.Id;
                return Redirect("~/dashbord");
            }
            return View();
        }

        public ActionResult profile(int? id)
        {
            try
            {
                var userid = int.Parse(Session["userid"].ToString());
                if(id==userid)
                {
                    var q = new Database().tbl_users.Where(x => x.Id == userid).FirstOrDefault();
                    return View(q);
                }
                else
                {
                    return Redirect("~/Home/");
                }
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/");
            }

        }

        [HttpPost]
        public ActionResult profile(FormCollection form)
        {
            string rootFolder = Server.MapPath("~/images/users");
            string pathFile = "";
            string fileName = "";
            try
            {
                var userid = int.Parse(Session["userid"].ToString());
                try
                {
                    HttpPostedFileBase file = Request.Files["pic"];
                    var fileExtension = Path.GetExtension(file.FileName);
                    var arr = new string[] { ".jpg",".png",".gif",".svg"};
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName) && arr.Contains(fileExtension.ToLower()))
                    {
                        fileName = Models.Tools.persianDateClass.getPersianDateTime("yyyymmdd")+"_"+ new Random().Next(1000, 9999) + fileExtension;
                        pathFile = Path.Combine(rootFolder, fileName);
                        file.SaveAs(pathFile);
                    }
                }
                catch (Exception ex)
                {
                }
                
                
                if (form["Id"] == userid.ToString())
                {
                    var db = new Database();
                    var tblUser = db.tbl_users.Where(p => p.Id == userid).FirstOrDefault();
                    tblUser.Id = int.Parse(form["Id"]);
                    tblUser.name = form["name"];
                    tblUser.family = form["family"];
                    tblUser.father = form["father"];
                    tblUser.codemeli = form["codemeli"];
                    tblUser.password = form["password"];
                    tblUser.pic = fileName;

                    db.Entry(tblUser).State = System.Data.Entity.EntityState.Modified;
                    var r = db.SaveChanges();

                    var q = db.tbl_users.Where(x => x.Id == userid).FirstOrDefault();
                    return View(q);
                }
                else
                {
                    return Redirect("~/Home/");
                }
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/");
            }

        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return Redirect("~/Home/");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}