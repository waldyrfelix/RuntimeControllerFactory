//
// CSharpControllerFile.cs
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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text.RegularExpressions;

namespace Fusonic.Web.Mvc.RuntimeController
{
    public class CSharpControllerFile
    {
        private CSharpControllerFile() { }

        public string ClassName { get; private set; }
        public string Namespace { get; private set; }
        public string ClassSource { get; private set; }
        public string FullClassName { get; private set; }

        #region Static Members

        private static Regex regexNamespace = new Regex(@"namespace\s+([\w\.]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex regexClassName = new Regex(@"class\s+([\w\.]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static CSharpControllerFile Parse(string source)
        {
            Match matchClassName = regexClassName.Match(source);
            Match matchNamespace = regexNamespace.Match(source);
            if (!matchClassName.Success || !matchNamespace.Success)
            {
                return null;
            }

            string className = matchClassName.Groups[1].Value.Trim();
            string @namespace = matchNamespace.Groups[1].Value.Trim();

            return new CSharpControllerFile()
            {
                ClassSource = source,
                ClassName = className,
                FullClassName = String.Format("{0}.{1}", @namespace, className),
                Namespace = @namespace
            };
        }

        #endregion

    }
}