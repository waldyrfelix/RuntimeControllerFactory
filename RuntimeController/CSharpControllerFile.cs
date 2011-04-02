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