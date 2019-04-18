using System;

namespace Amns.GreyFox.Data
{
	/// <summary>
	/// Summary description for WeightedQueryColumn.
	/// </summary>
	public class WeightedQueryColumn
	{
		string columnName;
		string tableName;
		int weight;

		public string ColumnName
		{
			get
			{
				return columnName;
			}
			set
			{
				columnName = value;
			}
		}

		public string TableName
		{
			get
			{
				return tableName;
			}
			set
			{
				tableName = value;
			}
		}

		public int Weight
		{
			get
			{
				return weight;
			}
			set
			{
				weight = value;
			}
		}

		public WeightedQueryColumn(string columnName, string tableName, string weight)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
