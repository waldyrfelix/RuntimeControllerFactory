using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Mono.CSharp;
using System.IO;

namespace Fusonic.Web.Mvc.RuntimeController
{
	public class DefaultPathProvider : IRuntimeControllerPathProvider
	{
		public string GetPath (RequestContext requestContext, string controllerName)
		{
			return controllerName != null ? AppDomain.CurrentDomain.BaseDirectory +
				Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + controllerName + ".cs" : null;
		}
	}
}