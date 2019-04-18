using System;

namespace Amns.GreyFox.Content.Aluminum
{
	/// <summary>
	/// Summary description for AluminumCommand.
	/// </summary>
	public abstract class AluminumCommand
	{
		private bool		_complete;		// Flag to signal completion of command
		private string		_workingText;
		private string[]	_parameters;

		public AluminumCommand()
		{
		}

		public EventHandler Parse;
		public void OnParse(System.EventArgs e)
		{
			if(Parse != null)
				Parse(this, e);
		}

		public EventHandler PreExecute;
		public void OnPreExecute(System.EventArgs e)
		{
			if(PreExecute != null)
				PreExecute(this, e);
		}

		public EventHandler Execute;
		public void OnExecute(System.EventArgs e)
		{
			if(EndExecute != null)
				EndExecute(this, e);
		}
	}
}
