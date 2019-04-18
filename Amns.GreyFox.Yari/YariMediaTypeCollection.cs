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
	public class YariMediaTypeCollection : IList, ICloneable 
	{
		private int count = 0;
		private YariMediaType[] YariMediaTypeArray ;

		public YariMediaTypeCollection() : base()
		{
			YariMediaTypeArray = new YariMediaType[15];
		}

		public YariMediaTypeCollection(int capacity) : base()
		{
			YariMediaTypeArray = new YariMediaType[capacity];
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
				YariMediaTypeArray[index] = (YariMediaType) value;
			}
		}

		public YariMediaType this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return YariMediaTypeArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				YariMediaTypeArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((YariMediaType) value);
		}

		public int Add(YariMediaType value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > YariMediaTypeArray.GetUpperBound(0) + 1)
				{
					YariMediaType[] tempYariMediaTypeArray = new YariMediaType[count * 2];
					Array.Copy(YariMediaTypeArray, tempYariMediaTypeArray, count - 1);
					YariMediaTypeArray = tempYariMediaTypeArray;
				}
				YariMediaTypeArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				YariMediaTypeArray = new YariMediaType[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((YariMediaType) value);
		}

		public bool Contains(YariMediaType value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((YariMediaType) value);
		}

		public int IndexOf(YariMediaType value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(YariMediaTypeArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (YariMediaType) value);
		}

		public void Insert(int index, YariMediaType value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > YariMediaTypeArray.GetUpperBound(0) + 1)
				{
					YariMediaType[] tempYariMediaTypeArray = new YariMediaType[count * 2];
					Array.Copy(YariMediaTypeArray, tempYariMediaTypeArray, count - 1);
					YariMediaTypeArray = tempYariMediaTypeArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					YariMediaTypeArray[x] = YariMediaTypeArray[x - 1];
				YariMediaTypeArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((YariMediaType) value);
		}

		public void Remove(YariMediaType value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("YariMediaType not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					YariMediaTypeArray[x-1] = YariMediaTypeArray[x];
				YariMediaTypeArray[count - 1] = null;
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
				return YariMediaTypeArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return YariMediaTypeArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				YariMediaTypeArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(YariMediaTypeArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private YariMediaType[] YariMediaTypeArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(YariMediaType[] YariMediaTypeArray, int virtualCount)
			{
				this.YariMediaTypeArray = YariMediaTypeArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < YariMediaTypeArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public YariMediaType Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return YariMediaTypeArray[cursor];
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
		/// Makes a shallow copy of the current YariMediaTypeCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>YariMediaTypeCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current YariMediaTypeCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>YariMediaTypeCollection</returns>
		public YariMediaTypeCollection Clone()
		{
			YariMediaTypeCollection clonedYariMediaType = new YariMediaTypeCollection(count);
			lock(this)
			{
				foreach(YariMediaType item in this)
					clonedYariMediaType.Add(item);
			}
			return clonedYariMediaType;
		}

		/// <summary>
		/// Makes a deep copy of the current YariMediaType.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the YariMediaTypeCollection from their children.</param>
		public YariMediaTypeCollection Copy(bool isolated)
		{
			YariMediaTypeCollection isolatedCollection = new YariMediaTypeCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(YariMediaTypeArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(YariMediaTypeArray[i].Copy());
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
				Array.Sort(YariMediaTypeArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public YariMediaType Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(YariMediaTypeArray[x].ID == id)
						return YariMediaTypeArray[x];
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
				s.Append(YariMediaTypeArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
