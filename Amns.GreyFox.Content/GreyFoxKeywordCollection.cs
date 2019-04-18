using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Keyword
	/// </summary>
	public class GreyFoxKeywordCollection : IList, ICloneable 
	{
		private int itemCount = 0;
		internal GreyFoxKeyword[] GreyFoxKeywordArray ;

		public GreyFoxKeywordCollection() : base()
		{
			GreyFoxKeywordArray = new GreyFoxKeyword[15];
		}

		public GreyFoxKeywordCollection(int capacity) : base()
		{
			GreyFoxKeywordArray = new GreyFoxKeyword[capacity];
		}

		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				GreyFoxKeywordArray[index] = (GreyFoxKeyword) value;
			}
		}

		public GreyFoxKeyword this[int index]
		{
			get
			{
				if(index > itemCount - 1)
					throw(new Exception("Index out of bounds."));
				return GreyFoxKeywordArray[index];
			}
			set
			{
				GreyFoxKeywordArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((GreyFoxKeyword) value);
		}

		public int Add(GreyFoxKeyword value)
		{
			itemCount++;
			if(itemCount > GreyFoxKeywordArray.GetUpperBound(0) + 1)
			{
				GreyFoxKeyword[] tempGreyFoxKeywordArray = new GreyFoxKeyword[itemCount * 2];
				for(int x = 0; x <= GreyFoxKeywordArray.GetUpperBound(0); x++)
					tempGreyFoxKeywordArray[x] = GreyFoxKeywordArray[x];
				GreyFoxKeywordArray = tempGreyFoxKeywordArray;
			}
			GreyFoxKeywordArray[itemCount - 1] = value;
			return itemCount -1;
		}

		public void Clear()
		{
			itemCount = 0;
			GreyFoxKeywordArray = new GreyFoxKeyword[15];
		}

		bool IList.Contains(object value)
		{
			return Contains((GreyFoxKeyword) value);
		}

		public bool Contains(GreyFoxKeyword value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((GreyFoxKeyword) value);
		}

		public int IndexOf(GreyFoxKeyword value)
		{
			for(int x = 0; x < itemCount; x++)
				if(GreyFoxKeywordArray[x].Equals(value))
					return x;
			return -1;
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (GreyFoxKeyword) value);
		}

		public void Insert(int index, GreyFoxKeyword value)
		{
			itemCount++;
			if(itemCount > GreyFoxKeywordArray.Length)
			for(int x = index + 1; x == itemCount - 2; x ++)
				GreyFoxKeywordArray[x] = GreyFoxKeywordArray[x - 1];
			GreyFoxKeywordArray[index] = value;
		}

		void IList.Remove(object value)
		{
			Remove((GreyFoxKeyword) value);
		}

		public void Remove(GreyFoxKeyword value)
		{
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("GreyFoxKeyword not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			for(int x = index + 1; x <= itemCount - 1; x++)
				GreyFoxKeywordArray[x-1] = GreyFoxKeywordArray[x];
			GreyFoxKeywordArray[itemCount - 1] = null;
			itemCount--;
		}

		public int Count
		{
			get
			{
				return itemCount;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return GreyFoxKeywordArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return GreyFoxKeywordArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			GreyFoxKeywordArray.CopyTo(array, index);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(GreyFoxKeywordArray, itemCount);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private GreyFoxKeyword[] GreyFoxKeywordArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(GreyFoxKeyword[] GreyFoxKeywordArray, int virtualCount)
			{
				this.GreyFoxKeywordArray = GreyFoxKeywordArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < GreyFoxKeywordArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public GreyFoxKeyword Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return GreyFoxKeywordArray[cursor];
				}
			}

			Object IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		public GreyFoxKeywordCollection Clone()
		{
			GreyFoxKeywordCollection clonedGreyFoxKeyword = new GreyFoxKeywordCollection(itemCount);
			foreach(GreyFoxKeyword item in this)
				clonedGreyFoxKeyword.Add(item);
			return clonedGreyFoxKeyword;
		}
		//--- Begin Custom Code ---
																
		public override string ToString()
		{
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			for(int x = 0; x < itemCount; x++)
			{
				if(x != 0)
					s.Append(",");
				s.Append(GreyFoxKeywordArray[x].keyword);
			}
			return s.ToString();
		}

		//--- End Custom Code ---
	}
}
