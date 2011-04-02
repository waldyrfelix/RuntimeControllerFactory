using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace RuntimeControllerTestApp.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Hello Mono on Master";
			
            return View();
        }
		
		public string RuntimeFunc()
		{
			return "added at runtime";
		}

        public ActionResult About()
        {
            return View();
        }
    }
}