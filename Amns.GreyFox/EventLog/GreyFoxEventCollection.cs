using System;
using System.Collections;

namespace Amns.GreyFox.EventLog
{
	/// <summary>
	/// <summary>
	/// Event class for Amns.GreyFox Event Log.
	/// </summary>
	/// </summary>
	public class GreyFoxEventCollection : IList, ICloneable 
	{
		private int count = 0;
		private GreyFoxEvent[] GreyFoxEventArray ;

		public GreyFoxEventCollection() : base()
		{
			GreyFoxEventArray = new GreyFoxEvent[15];
		}

		public GreyFoxEventCollection(int capacity) : base()
		{
			GreyFoxEventArray = new GreyFoxEvent[capacity];
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
				GreyFoxEventArray[index] = (GreyFoxEvent) value;
			}
		}

		public GreyFoxEvent this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return GreyFoxEventArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				GreyFoxEventArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((GreyFoxEvent) value);
		}

		public int Add(GreyFoxEvent value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxEventArray.GetUpperBound(0) + 1)
				{
					GreyFoxEvent[] tempGreyFoxEventArray = new GreyFoxEvent[count * 2];
					Array.Copy(GreyFoxEventArray, tempGreyFoxEventArray, count - 1);
					GreyFoxEventArray = tempGreyFoxEventArray;
				}
				GreyFoxEventArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				GreyFoxEventArray = new GreyFoxEvent[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((GreyFoxEvent) value);
		}

		public bool Contains(GreyFoxEvent value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((GreyFoxEvent) value);
		}

		public int IndexOf(GreyFoxEvent value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxEventArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (GreyFoxEvent) value);
		}

		public void Insert(int index, GreyFoxEvent value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxEventArray.GetUpperBound(0) + 1)
				{
					GreyFoxEvent[] tempGreyFoxEventArray = new GreyFoxEvent[count * 2];
					Array.Copy(GreyFoxEventArray, tempGreyFoxEventArray, count - 1);
					GreyFoxEventArray = tempGreyFoxEventArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					GreyFoxEventArray[x] = GreyFoxEventArray[x - 1];
				GreyFoxEventArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((GreyFoxEvent) value);
		}

		public void Remove(GreyFoxEvent value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("GreyFoxEvent not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					GreyFoxEventArray[x-1] = GreyFoxEventArray[x];
				GreyFoxEventArray[count - 1] = null;
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
				return GreyFoxEventArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return GreyFoxEventArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				GreyFoxEventArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(GreyFoxEventArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private GreyFoxEvent[] GreyFoxEventArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(GreyFoxEvent[] GreyFoxEventArray, int virtualCount)
			{
				this.GreyFoxEventArray = GreyFoxEventArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < GreyFoxEventArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public GreyFoxEvent Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return GreyFoxEventArray[cursor];
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
		/// Makes a shallow copy of the current GreyFoxEventCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxEventCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current GreyFoxEventCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxEventCollection</returns>
		public GreyFoxEventCollection Clone()
		{
			GreyFoxEventCollection clonedGreyFoxEvent = new GreyFoxEventCollection(count);
			lock(this)
			{
				foreach(GreyFoxEvent item in this)
					clonedGreyFoxEvent.Add(item);
			}
			return clonedGreyFoxEvent;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxEvent.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the GreyFoxEventCollection from their children.</param>
		public GreyFoxEventCollection Copy(bool isolated)
		{
			GreyFoxEventCollection isolatedCollection = new GreyFoxEventCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxEventArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxEventArray[i].Copy());
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
				Array.Sort(GreyFoxEventArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public GreyFoxEvent Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxEventArray[x].ID == id)
						return GreyFoxEventArray[x];
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
				s.Append(GreyFoxEventArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
