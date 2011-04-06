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
using Microsoft.CSharp;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;

namespace Fusonic.Web.Mvc.RuntimeController
{
    public class RuntimeControllerFactory : DefaultControllerFactory
    {
        private readonly IRuntimeControllerPathProvider pathProvider;

        public RuntimeControllerFactory(IRuntimeControllerPathProvider pathProvider)
        {
            this.pathProvider = pathProvider;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            string controllerName = GetControllerName(requestContext, controllerType);
            string path = pathProvider.GetPath(requestContext, controllerName);
            CSharpControllerFile controllerFile = CSharpControllerFile.Parse(File.ReadAllText(path));

            var parameters = new CompilerParameters();
            addReferencedAssemblies(parameters);

            var codeProvider = new CSharpCodeProvider().CompileAssemblyFromSource(parameters, controllerFile.ClassSource);

            object controllerInstance = codeProvider.CompiledAssembly.CreateInstance(controllerFile.FullClassName);

            return (IController)controllerInstance ?? base.GetControllerInstance(requestContext, controllerType);
        }

        private void addReferencedAssemblies(CompilerParameters parameters)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    string location = assembly.Location;
                    if (!String.IsNullOrEmpty(location))
                    {
                        parameters.ReferencedAssemblies.Add(location);
                    }
                }
                catch (NotSupportedException) { }
            }
        }

        protected virtual string GetControllerName(RequestContext requestContext, Type controllerType)
        {
            string controllerName;
            if (controllerType != null)
            {
                controllerName = controllerType.Name;
            }
            else
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