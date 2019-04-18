using System;

namespace Amns.GreyFox.Content.Aluminum
{
	/// <summary>
	/// Summary description for AluminumConstructor.
	/// </summary>
	public class AluminumConstructor
	{
		private DbContentClip				_clip;
		private string						_output = string.Empty;
		private bool						_isConstructed = false;
		private string[]					_workingClip;
		private AluminumCommandCollection	_commands;
		
		public AluminumConstructor(DbContentClip clip)
		{
			this._clip = clip;
		}

		public EventHandler Parse;
		public void OnParse(System.EventArgs e)
		{
			// read clip's body into the working clip array.
			// commands are loading into the working clip array.
		}

		public EventHandler Construct;
		public void OnConstruct(System.EventArgs e)
		{
			// loop through clip body and read AluminumCommands
			// construct aluminum commands and execute them with threads			
			// complete construction when all threads are finished.

			isConstructed = true;

			if(Construct != null)
				Construct(this, e);
		}
	}
}
