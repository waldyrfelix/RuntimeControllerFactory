using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RuntimeControllerTestApp.Controllers
{
	public class TestAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted (ActionExecutedContext filterContext)
		{
			System.Console.WriteLine("OK");
		}
	}
}