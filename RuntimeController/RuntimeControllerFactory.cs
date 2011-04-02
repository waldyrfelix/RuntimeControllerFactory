//
// RuntimeControllerFactory.cs
//
// Author:
// David Roth <david.roth@fusonic.net>
//
// Copyright (c) 2011 Fusonic GmbH (http://www.fusonic.net)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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

		public RuntimeControllerFactory(IRuntimeControllerPathProvider pathProvider)
		{
			this.pathProvider = pathProvider;
		}

		public void ReferenceAssembly(Assembly assembly)
		{
			assemblies.Add(assembly);
		}

		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			CompilerSettings settings = new CompilerSettings();
			Report report = new Report(new ConsoleReportPrinter());
			Evaluator eval = new Evaluator(settings, report);
			
			object instance = null;
			bool instanceCreated = false;
			eval.ReferenceAssembly(typeof(Controller).Assembly);
			
			foreach (Assembly assembly in assemblies)
			{
				eval.ReferenceAssembly(assembly);
			}
			
			string controllerName = GetControllerName(requestContext, controllerType);
			string path = pathProvider.GetPath(requestContext, controllerName);
			CSharpControllerFile controllerFile = CSharpControllerFile.Parse(File.ReadAllText(path));
			
			eval.Run(controllerFile.ClassSource);
			eval.Evaluate("new " + controllerName + "();", out instance, out instanceCreated);
			
			return (IController)instance ?? base.GetControllerInstance(requestContext, controllerType);
		}

		protected virtual string GetControllerName(RequestContext requestContext, Type controllerType)
		{
			string controllerName;
			if (controllerType != null)
			{
				controllerName = controllerType.Name;
			} else
			{
				controllerName = requestContext.RouteData.Values["controller"] as string;
				if (controllerName != null)
				{
					controllerName += "Controller";
				}
			}
			
			return controllerName;
		}
	}
}