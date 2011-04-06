using System.Web.Routing;

namespace Fusonic.Web.Mvc.RuntimeController
{
	public interface IRuntimeControllerPathProvider
	{
		string GetPath(RequestContext requestContext, string controllerName);
	}
}
