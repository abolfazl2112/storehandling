using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace anbardari.Controllers
{
    public class dashbordController : Controller
    {
        // GET: dashbord
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult dashbord()
        {
            return View();
        }
    }
}