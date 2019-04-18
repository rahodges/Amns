using System;

namespace Amns.GreyFox.Serialization
{
	/// <summary>
	/// Summary description for ICsvRow.
	/// </summary>
	public interface ICsvConverter
	{
		string[] ColumnHeaders {get;}
		string[] ColumnData {get;}
	}
}