using System;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Data
{
	/// <summary>
	/// Summary description for SqlFilter.
	/// </summary>
	public class SqlFilter
	{
		public SqlFilter()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void FilterArray(string[] fieldData)
		{
			for(int x = 0; x < fieldData.Length; x++)
				fieldData[x] = FilterString(fieldData[x]);
		}

		public static string FilterString(string fieldData)
		{
			return fieldData.Replace("'", "''");
		}

		public static string FilterString(string fieldData, int length)
		{
			if(length == 0)
				length = 255;

			fieldData = fieldData.Trim();
			
			if(fieldData.Length > length)
				fieldData = fieldData.Substring(0, length);
			fieldData = fieldData.Replace("'", "''");
			return fieldData;
		}

		public static string FilterString(TextBox textBox)
		{
			int length = textBox.MaxLength;
			if(length == 0)
				length = 255;

			string fieldData = textBox.Text.Trim();
			
			if(fieldData.Length > length)
				fieldData = fieldData.Substring(0, length);
			fieldData = fieldData.Replace("'", "''");
			return fieldData;
		}

		public static void ApplyFilter(params TextBox[] textBoxes)
		{
			int length;
			foreach(TextBox t in textBoxes)
			{
				length = t.MaxLength;
				if(length == 0)
					length = 255;

				t.Text = t.Text.Trim();
				if(t.Text.Length > length)
					t.Text = t.Text.Substring(0, length);		
				t.Text = t.Text.Replace("'", "''");
			}
		}
	}
}
