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
	public class YariMediaRecordCollection : IList, ICloneable 
	{
		private int count = 0;
		private YariMediaRecord[] YariMediaRecordArray ;

		public YariMediaRecordCollection() : base()
		{
			YariMediaRecordArray = new YariMediaRecord[15];
		}

		public YariMediaRecordCollection(int capacity) : base()
		{
			YariMediaRecordArray = new YariMediaRecord[capacity];
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
				YariMediaRecordArray[index] = (YariMediaRecord) value;
			}
		}

		public YariMediaRecord this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return YariMediaRecordArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				YariMediaRecordArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((YariMediaRecord) value);
		}

		public int Add(YariMediaRecord value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > YariMediaRecordArray.GetUpperBound(0) + 1)
				{
					YariMediaRecord[] tempYariMediaRecordArray = new YariMediaRecord[count * 2];
					Array.Copy(YariMediaRecordArray, tempYariMediaRecordArray, count - 1);
					YariMediaRecordArray = tempYariMediaRecordArray;
				}
				YariMediaRecordArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				YariMediaRecordArray = new YariMediaRecord[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((YariMediaRecord) value);
		}

		public bool Contains(YariMediaRecord value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((YariMediaRecord) value);
		}

		public int IndexOf(YariMediaRecord value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(YariMediaRecordArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (YariMediaRecord) value);
		}

		public void Insert(int index, YariMediaRecord value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > YariMediaRecordArray.GetUpperBound(0) + 1)
				{
					YariMediaRecord[] tempYariMediaRecordArray = new YariMediaRecord[count * 2];
					Array.Copy(YariMediaRecordArray, tempYariMediaRecordArray, count - 1);
					YariMediaRecordArray = tempYariMediaRecordArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					YariMediaRecordArray[x] = YariMediaRecordArray[x - 1];
				YariMediaRecordArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((YariMediaRecord) value);
		}

		public void Remove(YariMediaRecord value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("YariMediaRecord not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					YariMediaRecordArray[x-1] = YariMediaRecordArray[x];
				YariMediaRecordArray[count - 1] = null;
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
				return YariMediaRecordArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return YariMediaRecordArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				YariMediaRecordArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(YariMediaRecordArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private YariMediaRecord[] YariMediaRecordArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(YariMediaRecord[] YariMediaRecordArray, int virtualCount)
			{
				this.YariMediaRecordArray = YariMediaRecordArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < YariMediaRecordArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public YariMediaRecord Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return YariMediaRecordArray[cursor];
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
		/// Makes a shallow copy of the current YariMediaRecordCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>YariMediaRecordCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current YariMediaRecordCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>YariMediaRecordCollection</returns>
		public YariMediaRecordCollection Clone()
		{
			YariMediaRecordCollection clonedYariMediaRecord = new YariMediaRecordCollection(count);
			lock(this)
			{
				foreach(YariMediaRecord item in this)
					clonedYariMediaRecord.Add(item);
			}
			return clonedYariMediaRecord;
		}

		/// <summary>
		/// Makes a deep copy of the current YariMediaRecord.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the YariMediaRecordCollection from their children.</param>
		public YariMediaRecordCollection Copy(bool isolated)
		{
			YariMediaRecordCollection isolatedCollection = new YariMediaRecordCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(YariMediaRecordArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(YariMediaRecordArray[i].Copy());
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
				Array.Sort(YariMediaRecordArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public YariMediaRecord Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(YariMediaRecordArray[x].ID == id)
						return YariMediaRecordArray[x];
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
				s.Append(YariMediaRecordArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
