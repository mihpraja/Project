using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrometheusWebApplication.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Method for logout.
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session["UserId"] = null;
            return RedirectToAction("Index");
        }
    }
}