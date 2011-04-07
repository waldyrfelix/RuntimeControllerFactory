using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RuntimeControllerTestApp.Models;

namespace RuntimeControllerTestApp.Areas.Admin.Controllers
{
    public class PanelController : Controller
    {
        public IMessageProvider MessageProvider{ get; private set; }

        public PanelController(IMessageProvider messageProvider)
        {
            this.MessageProvider = messageProvider;
        }

        public ActionResult Index()
        {
            ViewData["Message"] = MessageProvider.GetMessage("Admin Area");
            return View();
        }

    }
}
