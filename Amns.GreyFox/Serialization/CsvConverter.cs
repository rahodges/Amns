using System;
using System.Text;

namespace Amns.GreyFox.Serialization
{
	/// <summary>
	/// Summary description for CsvConverter.
	/// </summary>
	public class CsvConverter
	{
		public CsvConverter()
		{
			
		}

		public string ConvertData(object[] csvRows)
		{
			if(csvRows.Length == 0)
				return string.Empty;

			ICsvConverter row = (ICsvConverter) csvRows[0];
			StringBuilder s = new StringBuilder();

			s.Append(string.Join("\",\"", row.ColumnHeaders));
			s.Append("\r\n");

			for(int x = 0; x <= csvRows.GetUpperBound(0); x++)
			{
				row = (ICsvConverter) csvRows[x];
				s.Append(string.Join("\",\"", row.ColumnData));
				s.Append("\r\n");
			}

			return s.ToString();
		}
	}
}
