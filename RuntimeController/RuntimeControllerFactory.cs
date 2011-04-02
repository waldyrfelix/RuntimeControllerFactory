using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Mono.CSharp;
using System.IO;
using System.Reflection;

namespace Fusonic.Web.Mvc.RuntimeController
{	
	public class RuntimeControllerFactory : DefaultControllerFactory
	{
		private readonly IRuntimeControllerPathProvider pathProvider;
		private readonly List<Assembly> assemblies = new List<Assembly>();
		
		public RuntimeControllerFactory (IRuntimeControllerPathProvider pathProvider)
		{
			this.pathProvider = pathProvider;
		}
		
		public void ReferenceAssembly (Assembly assembly)
		{
			assemblies.Add(assembly);
		}
		
		public override IController CreateController (RequestContext requestContext, string controllerName)
		{
			return base.CreateController (requestContext, controllerName);
		}
		
		protected override IController GetControllerInstance (RequestContext requestContext, Type controllerType)
		{
			CompilerSettings settings = new CompilerSettings();
			Report report = new Report(new ConsoleReportPrinter());
			Evaluator eval = new Evaluator(settings, report);
			
			object instance = null;
			bool instanceCreated = false;
			eval.ReferenceAssembly(typeof(Controller).Assembly);
			
			foreach(Assembly assembly in assemblies)
			{
				eval.ReferenceAssembly(assembly);
			}
			
			string controllerName = GetControllerName(requestContext, controllerType);
			string path = pathProvider.GetPath(requestContext, controllerName);
			CSharpControllerFile controllerFile = CSharpControllerFile.Parse(File.ReadAllText(path));
			
			eval.Run(controllerFile.ClassSource);
			eval.Evaluate("new " + controllerName  + "();", out instance, out instanceCreated);			
			
			return (IController)instance ?? base.GetControllerInstance(requestContext, controllerType);
		}
		
		protected string GetControllerName(RequestContext requestContext, Type controllerType)
		{
			string controllerName;
			if(controllerType != null)
			{
				controllerName = controllerType.Name;
			}
			else
			{
				controllerName = requestContext.RouteData.Values["controller"] as string;
				if(controllerName != null)
				{
					controllerName += "Controller";
				}
			}
			
			return controllerName;
		}
		
		public override void ReleaseController (IController controller)
		{
			base.ReleaseController (controller);
		}
		
		protected override Type GetControllerType (RequestContext requestContext, string controllerName)
		{
			return base.GetControllerType (requestContext, controllerName);
		}
	}
}