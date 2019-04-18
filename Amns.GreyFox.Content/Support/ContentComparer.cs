using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary description for DbSort.
	/// </summary>
	public class ContentComparer : IComparer
	{
		private ContentCompareKey[] _keys;

		public ContentComparer(params ContentCompareKey[] keys)
		{
			_keys = keys;
		}

		int IComparer.Compare(object a, object b)
		{
			if(a is IContentObject && b is IContentObject)
				return Compare((IContentObject) a, (IContentObject) b);
			throw(new Exception("Apply expected IContentObject."));
		}

		public int Compare(IContentObject a, IContentObject b)
		{
			int result = 0;

			for(int i = 0; i <= _keys.GetUpperBound(0); i++)
			{
				switch(_keys[i])
				{
					case ContentCompareKey.Author:
						if(a.Authors.Count > 0 &
							b.Authors.Count > 0)
							result = a.Authors[0].Contact.LastName.CompareTo(b.Authors[0].Contact.LastName);
						else if(a.Authors.Count == 0)
							result = -1;
						else if(b.Authors.Count == 0)
							result = 1;
						else
							result = 0;
						break;
					case ContentCompareKey.ExpireDate:
						result = a.ExpirationDate.CompareTo(b.ExpirationDate);
						break;
					case ContentCompareKey.ID:
						result = a.ID - b.ID;
						break;
					case ContentCompareKey.MenuEnabled:
						if(a.MenuEnabled & !b.MenuEnabled)
							result = -1;
						else if(!a.MenuEnabled & b.MenuEnabled)
							result = 1;
						break;
					case ContentCompareKey.MenuOrder:
						result = a.MenuOrder - b.MenuOrder;			// AHHH! THIS HAS TO BE REVERSED!
						break;
					case ContentCompareKey.SortOrder:
						result = a.SortOrder - b.SortOrder;
						break;
					case ContentCompareKey.PublishDate:
						result = a.PublishDate.CompareTo(b.PublishDate);
						break;
					case ContentCompareKey.Title:
						result = a.Title.CompareTo(b.Title);
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
