using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using RuntimeControllerTestApp.Models;

namespace RuntimeControllerTestApp.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public IMessageProvider MessageProvider { get; private set; }

        public HomeController(IMessageProvider provider)
        {
            this.MessageProvider = provider;
        }

        public ActionResult Index()
        {
            ViewData["Message"] = this.MessageProvider.GetMessage("Waldyr");

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