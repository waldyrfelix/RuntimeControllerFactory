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
	public interface IRuntimeControllerPathProvider
	{
		string GetPath(RequestContext requestContext, string controllerName);
	}
}
