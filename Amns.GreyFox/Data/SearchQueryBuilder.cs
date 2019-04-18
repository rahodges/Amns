using System;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace Amns.GreyFox.Data
{
	/// <summary>
	/// Summary description for SearchQueryBuilder.
	/// </summary>
	public class SearchQueryBuilder
	{
		const string OPERATORS_REQUIRED = "+";
		const string OPERATORS_ABSOLUTE = "=";

		public SearchQueryBuilder()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Generates a string representing fields for search hit counting. This should be
		/// used with the StandardSearch method.
		/// </summary>
		/// <param name="fields">Database fields to search for hits against.</param>
		/// <param name="searchKeys">Search keys to search against fields for.</param>
		/// <param name="absoluteWeight">The absolute weight multiplier. If zero, turns off absolute weight hits.</param>
		/// <param name="fuzzyWeight">The fuzzy weight multiplier. If zero, turns off fuzzy weight hits.</param>
		/// <returns></returns>
		public static string WeightedQueryAddOn(string[] fields, string[] searchKeys,
			int absoluteWeight, int fuzzyWeight, string asField)
		{
			if(absoluteWeight == 0 & fuzzyWeight == 0)
				throw(new Exception("No weights specified."));

			StringBuilder s = new StringBuilder(" (");
			string key;
			bool andKey;
			bool absoluteKey;

			SqlFilter.FilterArray(searchKeys);

			for(int findex = 0; findex <= fields.GetUpperBound(0); findex++)
			{
				for(int kindex = 0; kindex <= searchKeys.GetUpperBound(0); kindex++)
				{
					key = searchKeys[kindex];

					if(key == string.Empty || key == OPERATORS_REQUIRED)
						continue;

					andKey = key.StartsWith(OPERATORS_REQUIRED);
					if(andKey)
						key = key.Substring(1, key.Length-1);

					absoluteKey = key.StartsWith(OPERATORS_ABSOLUTE);
					if(absoluteKey)
						key = key.Substring(1, key.Length-1);					

					// Absolute value search hits and multiply by weight.
					if(absoluteWeight != 0)
					{
						s.Append("ABS(");
						s.Append(fields[findex]);
						s.Append("='");
						s.Append(key);
						s.Append("')*");
						s.Append(absoluteWeight);
						s.Append("+");
					}

					if(!absoluteKey & !andKey & fuzzyWeight != 0)
					{
						s.Append("ABS(");
						s.Append(fields[findex]);
						s.Append(" LIKE '%");
						s.Append(key);
						s.Append("%')*");
						s.Append(fuzzyWeight);
						s.Append("+");
					}
				}
			}

			if(s.Length > 0)
				s.Length--;

			s.Append(") AS ");
			s.Append(asField);
			s.Append(' ');

			return s.ToString();
		}

		/// <summary>
		/// Generates a string representing fields for search hit counting. This should be
		/// used with the StandardSearch method.
		/// </summary>
		/// <param name="fields">Database fields to search for hits against.</param>
		/// <param name="searchKeys">Search keys to search against fields for.</param>
		/// <param name="absoluteWeight">The absolute weight multiplier. If zero, turns off absolute weight hits.</param>
		/// <param name="fuzzyWeight">The fuzzy weight multiplier. If zero, turns off fuzzy weight hits.</param>
		/// <returns></returns>
		public static string ShiftedWeightQueryAddOn(string[] fields, string[] searchKeys,
			int absoluteWeight, int fuzzyWeight, string asField, params int[] weightCurve)
		{
			if(absoluteWeight == 0 & fuzzyWeight == 0)
				throw(new Exception("No weights specified."));
			if(weightCurve == null)
				throw(new Exception("No weight curve specified; null weight array."));
			if(weightCurve.GetUpperBound(0) == 0)
				throw(new Exception("No weight curve specified; zero weights in array."));

			StringBuilder s = new StringBuilder(" (");
			string key;
			bool andKey;
			bool absoluteKey;

			SqlFilter.FilterArray(searchKeys);

			for(int findex = 0; findex <= fields.GetUpperBound(0); findex++)
			{
				for(int kindex = 0; kindex <= searchKeys.GetUpperBound(0); kindex++)
				{
					key = searchKeys[kindex];

					if(key == string.Empty || key == OPERATORS_REQUIRED)
						continue;

					andKey = key.StartsWith(OPERATORS_REQUIRED);
					if(andKey)
						key = key.Substring(1, key.Length-1);

					absoluteKey = key.StartsWith(OPERATORS_ABSOLUTE);
					if(absoluteKey)
						key = key.Substring(1, key.Length-1);

					// Absolute value search hits and multiply by weight.
					if(absoluteWeight != 0)
					{
						s.Append("ABS(");
						s.Append(fields[findex]);
						s.Append("='");
						s.Append(key);
						s.Append("')*");
						s.Append(absoluteWeight);
						s.Append("+");
						if(kindex > weightCurve.GetUpperBound(0))
							s.Append(weightCurve[weightCurve.GetUpperBound(0)]);
						else
							s.Append(weightCurve[kindex]);
						s.Append("+");
					}

					if(!absoluteKey & !andKey & fuzzyWeight != 0)
					{
						s.Append("ABS(");
						s.Append(fields[findex]);
						s.Append(" LIKE '%");
						s.Append(key);
						s.Append("%')*");
						s.Append(fuzzyWeight);
						s.Append("+");
						if(kindex > weightCurve.GetUpperBound(0))
							s.Append(weightCurve[weightCurve.GetUpperBound(0)]);
						else
							s.Append(weightCurve[kindex]);
						s.Append("+");
					}
				}
			}

			if(s.Length > 0)
				s.Length--;

			s.Append(") AS ");
			s.Append(asField);
			s.Append(' ');

			return s.ToString();
		}

		public static string StandardSearch(string[] fields, string[] searchKeys)
		{
			StringBuilder orQuery = new StringBuilder();
			StringBuilder andQuery = new StringBuilder();
			bool firstOrKey = true;
			bool firstAndKey = true;
			string key;
			bool andKey;
			bool absoluteKey;

			SqlFilter.FilterArray(searchKeys);

			foreach(string searchKey in searchKeys)
			{
				key = searchKey;

				// skip empty keys
				if(key == string.Empty || key == OPERATORS_REQUIRED)
					continue;

				andKey = key.StartsWith(OPERATORS_REQUIRED);
				if(andKey)
					key = key.Substring(1, key.Length-1);

				absoluteKey = key.StartsWith(OPERATORS_ABSOLUTE);
				if(absoluteKey)
					key = key.Substring(1, key.Length-1);

				// If this is the first "OR" key...
				if(!andKey && firstOrKey)
				{
					if(absoluteKey)
					{
						//orQuery.AppendFormat("({0}='{1}' ", fields[0], key);

						orQuery.Append("(");
						orQuery.Append(fields[0]);
						orQuery.Append("='");
						orQuery.Append(key);
						orQuery.Append("'");
					}
					else
					{
						//orQuery.AppendFormat("({0} LIKE '%{1}%' ", fields[0], key);

						orQuery.Append("(");
						orQuery.Append(fields[0]);
						orQuery.Append(" LIKE '%");
						orQuery.Append(key);
						orQuery.Append("%'");
					}

					for(int x = 1; x <= fields.GetUpperBound(0); x++)
					{
						if(absoluteKey)
							orQuery.AppendFormat("OR {0}='{1}' ", fields[x], key);
						else
							orQuery.AppendFormat("OR {0} LIKE '%{1}%' ", fields[x], key);
					}

					orQuery.Append(")");
					firstOrKey = false;
					continue;
				}

				if(andKey && firstAndKey)
				{
					if(absoluteKey)
						andQuery.AppendFormat("({0}='{1}' ", fields[0], key);
					else
						andQuery.AppendFormat("({0} LIKE '%{1}%' ", fields[0], key);

					for(int x = 1; x <= fields.GetUpperBound(0); x++)
					{
						if(absoluteKey)
							andQuery.AppendFormat("OR {0}='{1}' ", fields[x], key);
						else
							andQuery.AppendFormat("OR {0} LIKE '%{1}%' ", fields[x], key);
					}

					andQuery.Append(")");
					firstAndKey = false;
					continue;
				}

				if(andKey)
				{
					if(absoluteKey)					
						andQuery.AppendFormat("AND ({0}='{1}' ", fields[0], key);
					else
						andQuery.AppendFormat("AND ({0} LIKE '%{1}%' ", fields[0], key);

					for(int x = 1; x <= fields.GetUpperBound(0); x++)
					{
						if(absoluteKey)
                            andQuery.AppendFormat("OR {0}='{1}' ", fields[x], key);
						else
							andQuery.AppendFormat("OR {0} LIKE '%{1}%' ", fields[x], key);
					}
					andQuery.Append(")");
				}
				else
				{
					if(absoluteKey)
						orQuery.AppendFormat("OR ({0}='{1}' ", fields[0], key);
					else
                        orQuery.AppendFormat("OR ({0} LIKE '%{1}%' ", fields[0], key);

					for(int x = 1; x <= fields.GetUpperBound(0); x++)
					{
						if(absoluteKey)
							orQuery.AppendFormat("OR {0}='{1}' ", fields[x], key);
						else
							orQuery.AppendFormat("OR {0} LIKE '%{1}%' ", fields[x], key);
					}
					orQuery.Append(")");
				}
			}
			
			// REPLACE THE "AND" WITH "WHERE" IF THE QUERY IS TO SEARCH ALL
			// DIPLOMATES!

			if(orQuery.Length == 0 && andQuery.Length != 0)
			{
				// This will take care of queries that have no keys specified.
				// Queries without an orQuery and andQuery are ignored. This is
				// optimized code.
				return andQuery.ToString();
			}
			else if(andQuery.Length == 0)
			{
				return orQuery.ToString();
			}

			orQuery.Insert(0, "(");
			orQuery.Append(") AND ");
			orQuery.Append(andQuery.ToString());
			return orQuery.ToString();
		}
	}
}
