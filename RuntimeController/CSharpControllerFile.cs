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
		private CSharpControllerFile ()
		{}
		
		public static CSharpControllerFile Parse(string source)
		{
			Regex regex = new Regex("namespace(.*)\n{");
			Match match = regex.Match(source);
			
			if(match.Success)
			{
				source = source.TrimEnd(new char[] {' ', '\n'});
				source = source.Substring(0, source.Length - 1);
				source = "using " + match.Groups[1] + ";\n" 
					+ source.Replace(match.Value, string.Empty);
			}
			
			return new CSharpControllerFile()
			{
				ClassSource = source
			};
		}
		
		public string ClassSource
		{
			get;
			private set;
		}
	}
}