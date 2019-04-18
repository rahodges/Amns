using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary description for DbSort.
	/// </summary>
	public class HitComparer : IComparer
	{
		private HitCompareKey[] _keys;

		public HitComparer(params HitCompareKey[] keys)
		{
			_keys = keys;
		}

		int IComparer.Compare(object a, object b)
		{
			if(a is IContentObject && b is DbContentHit)
				return Compare((DbContentHit) a, (DbContentHit) b);
			throw(new Exception("Apply expected DbContentHit."));
		}

		public int Compare(DbContentHit a, DbContentHit b)
		{
			int result = 0;

			for(int i = 0; i <= _keys.GetUpperBound(0); i++)
			{
				switch(_keys[i])
				{
					case HitCompareKey.RequestDate:
						result = a.RequestDate.CompareTo(b.RequestDate);
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
