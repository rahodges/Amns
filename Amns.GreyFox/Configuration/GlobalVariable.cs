using System;

namespace Amns.GreyFox.Configuration
{
	/// <summary>
	/// Summary description for GlobalVariable.
	/// </summary>
	public struct GlobalVariable
	{
		private string _Name;
		private string _Value;
//		private string _ReadRole;
//		private string _WriteRole;

		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
			}
		}

		public string Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value = value;
			}
		}

		public GlobalVariable(string Name, string Value)
		{
			_Name = Name;
			_Value = Value;
		}
	}
}