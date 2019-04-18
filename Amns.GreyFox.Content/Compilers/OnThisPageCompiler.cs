using System;
using System.Text.RegularExpressions;

namespace Amns.GreyFox.Content.Compilers
{
	/// <summary>
	/// Summary description for OnThisPageParser.
	/// </summary>
	public class OnThisPageCompiler : IContentCompiler
	{
		public readonly string Title = "On This Page Compiler";
		public readonly string Description = "Compiles an 'On This Page' list using header tags.";
		public readonly string Tag = "OnThisPage";

		private const int MAXCOUNT = 25;
		private const string HEADERTAG = "H3";
		
		private DbContentClip _clip;
		private int _cursorIndex = 0;
		private bool _isCompiling = false;

		public DbContentClip Clip
		{
			get { return _clip; }
			set 
			{ 
				if(_isCompiling)
					throw(new Exception("Cannot set a new clip during compilation."));
				_clip = value;
			}
		}
		
		public void Compile(DbContentClip clip)
		{
			_isCompiling = true;

			Regex tagFinder = new Regex("<h1((.|\n)*?)</h1>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			MatchCollection matches = tagFinder.Matches(clip.Body);
		
			matches.Count
		}
	}
}