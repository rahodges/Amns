/* ********************************************************** *
 * AMNS DbModel v1.0 Collection                               *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;
using System.Collections;

namespace Amns.GreyFox.Yari
{
	/// <summary>
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	/// </summary>
	public class YariMediaKeywordCollection : IList, ICloneable 
	{
		private int count = 0;
		private YariMediaKeyword[] YariMediaKeywordArray ;

		public YariMediaKeywordCollection() : base()
		{
			YariMediaKeywordArray = new YariMediaKeyword[15];
		}

		public YariMediaKeywordCollection(int capacity) : base()
		{
			YariMediaKeywordArray = new YariMediaKeyword[capacity];
		}

		#region IList Implemenation

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
				YariMediaKeywordArray[index] = (YariMediaKeyword) value;
			}
		}

		public YariMediaKeyword this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return YariMediaKeywordArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				YariMediaKeywordArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((YariMediaKeyword) value);
		}

		public int Add(YariMediaKeyword value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > YariMediaKeywordArray.GetUpperBound(0) + 1)
				{
					YariMediaKeyword[] tempYariMediaKeywordArray = new YariMediaKeyword[count * 2];
					Array.Copy(YariMediaKeywordArray, tempYariMediaKeywordArray, count - 1);
					YariMediaKeywordArray = tempYariMediaKeywordArray;
				}
				YariMediaKeywordArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				YariMediaKeywordArray = new YariMediaKeyword[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((YariMediaKeyword) value);
		}

		public bool Contains(YariMediaKeyword value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((YariMediaKeyword) value);
		}

		public int IndexOf(YariMediaKeyword value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(YariMediaKeywordArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (YariMediaKeyword) value);
		}

		public void Insert(int index, YariMediaKeyword value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > YariMediaKeywordArray.GetUpperBound(0) + 1)
				{
					YariMediaKeyword[] tempYariMediaKeywordArray = new YariMediaKeyword[count * 2];
					Array.Copy(YariMediaKeywordArray, tempYariMediaKeywordArray, count - 1);
					YariMediaKeywordArray = tempYariMediaKeywordArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					YariMediaKeywordArray[x] = YariMediaKeywordArray[x - 1];
				YariMediaKeywordArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((YariMediaKeyword) value);
		}

		public void Remove(YariMediaKeyword value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("YariMediaKeyword not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					YariMediaKeywordArray[x-1] = YariMediaKeywordArray[x];
				YariMediaKeywordArray[count - 1] = null;
				count--;
			}
		}

		#endregion

		#region ICollection Implementation

		public int Count
		{
			get
			{
				return count;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return YariMediaKeywordArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return YariMediaKeywordArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				YariMediaKeywordArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(YariMediaKeywordArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private YariMediaKeyword[] YariMediaKeywordArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(YariMediaKeyword[] YariMediaKeywordArray, int virtualCount)
			{
				this.YariMediaKeywordArray = YariMediaKeywordArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < YariMediaKeywordArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public YariMediaKeyword Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return YariMediaKeywordArray[cursor];
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

		#endregion

		/// <summary>
		/// Makes a shallow copy of the current YariMediaKeywordCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>YariMediaKeywordCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current YariMediaKeywordCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>YariMediaKeywordCollection</returns>
		public YariMediaKeywordCollection Clone()
		{
			YariMediaKeywordCollection clonedYariMediaKeyword = new YariMediaKeywordCollection(count);
			lock(this)
			{
				foreach(YariMediaKeyword item in this)
					clonedYariMediaKeyword.Add(item);
			}
			return clonedYariMediaKeyword;
		}

		/// <summary>
		/// Makes a deep copy of the current YariMediaKeyword.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the YariMediaKeywordCollection from their children.</param>
		public YariMediaKeywordCollection Copy(bool isolated)
		{
			YariMediaKeywordCollection isolatedCollection = new YariMediaKeywordCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(YariMediaKeywordArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(YariMediaKeywordArray[i].Copy());
					}
				}
			}
			return isolatedCollection;
		}

		#endregion

		#region Events

		public event EventHandler CollectionChanged;

		protected virtual void OnCollectionChanged(EventArgs e)
		{
			if(CollectionChanged != null)
				CollectionChanged(this, e);
		}

		#endregion

		#region Sort Methods

		/// <summary>
		/// Sorts the collection by id.
		/// </summary>
		public void Sort()
		{
			lock(this)
			{
				Array.Sort(YariMediaKeywordArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public YariMediaKeyword Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(YariMediaKeywordArray[x].ID == id)
						return YariMediaKeywordArray[x];
			}
			return null;
		}

		#endregion

		#region ToString() Override Method

		public override string ToString()
		{
			string lineBreak;

			if(System.Web.HttpContext.Current != null)
			lineBreak = "<br />";
			else
			lineBreak = "\r\n";

			System.Text.StringBuilder s = new System.Text.StringBuilder();
			for(int x = 0; x < count; x++)
			{
				if(x != 0)
					s.Append(lineBreak);
				s.Append(YariMediaKeywordArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

		//--- Begin Custom Code ---
																																																																
//		public override string ToString()
//		{
//			System.Text.StringBuilder s = new System.Text.StringBuilder();
//			for(int x = 0; x < count; x++)
//			{
//				if(s.Length > 0)
//					s.Append(';');
//				s.Append(YariMediaKeywordArray[x].Keyword);
//			}
//			return s.ToString();
//		}

		//--- End Custom Code ---
	}
}
