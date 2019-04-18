using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary description for CatalogComparer.
	/// </summary>
	public class CatalogComparer : IComparer
	{
		CatalogCompareKey[] _keys;

		public CatalogComparer(params CatalogCompareKey[] keys)
		{
			_keys = keys;
		}

		int IComparer.Compare(object a, object b)
		{
			return Compare((DbContentCatalog) a, (DbContentCatalog) b);
		}

		public int Compare(DbContentCatalog a, DbContentCatalog b)
		{
			int result = 0;
			
			for(int i = 0; i <= _keys.GetUpperBound(0); i++)
			{
				result = 0;

				switch(_keys[i])
				{
					case CatalogCompareKey.Title:
						result = string.Compare(a.Title, b.Title);
						break;
					case CatalogCompareKey.MenuOrder:
						result = a.MenuOrder - b.MenuOrder;
						break;
					case CatalogCompareKey.MenuEnabled:
						if(a.MenuEnabled & !b.MenuEnabled)
							result = -1;
						else if(!a.MenuEnabled & b.MenuEnabled)
							result = 1;
						break;
					case CatalogCompareKey.SortOrder:
						result = a.SortOrder - b.SortOrder;
						break;
				}

				// cease compare processing if the result is not equal
				if(result != 0)
					return result;
			}

			return result;
		}
	}
}
