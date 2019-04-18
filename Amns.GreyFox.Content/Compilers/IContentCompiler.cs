using System;

namespace Amns.GreyFox.Content.Compilers
{
	/// <summary>
	/// ICompiler provides an interface for content comilation classes.
	/// </summary>
	public interface IContentCompiler
	{
		/// <summary>
		/// The Compile method is responsible for modifying the underlying clip's contents.
		/// </summary>
		/// <param name="clip"></param>
		void Compile(DbContentClip clip);
	}
}
