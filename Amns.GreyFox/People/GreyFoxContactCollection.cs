using System;
using System.Collections;

namespace Amns.GreyFox.People
{
	/// <summary>
	/// <summary>
	/// Summary of Contact.
	/// </summary>
	/// </summary>
	public class GreyFoxContactCollection : IList, ICloneable 
	{
		private int count = 0;
		private GreyFoxContact[] GreyFoxContactArray ;

		public GreyFoxContactCollection() : base()
		{
			GreyFoxContactArray = new GreyFoxContact[15];
		}

		public GreyFoxContactCollection(int capacity) : base()
		{
			GreyFoxContactArray = new GreyFoxContact[capacity];
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
				GreyFoxContactArray[index] = (GreyFoxContact) value;
			}
		}

		public GreyFoxContact this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return GreyFoxContactArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				GreyFoxContactArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((GreyFoxContact) value);
		}

		public int Add(GreyFoxContact value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxContactArray.GetUpperBound(0) + 1)
				{
					GreyFoxContact[] tempGreyFoxContactArray = new GreyFoxContact[count * 2];
					Array.Copy(GreyFoxContactArray, tempGreyFoxContactArray, count - 1);
					GreyFoxContactArray = tempGreyFoxContactArray;
				}
				GreyFoxContactArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				GreyFoxContactArray = new GreyFoxContact[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((GreyFoxContact) value);
		}

		public bool Contains(GreyFoxContact value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((GreyFoxContact) value);
		}

		public int IndexOf(GreyFoxContact value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxContactArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (GreyFoxContact) value);
		}

		public void Insert(int index, GreyFoxContact value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxContactArray.GetUpperBound(0) + 1)
				{
					GreyFoxContact[] tempGreyFoxContactArray = new GreyFoxContact[count * 2];
					Array.Copy(GreyFoxContactArray, tempGreyFoxContactArray, count - 1);
					GreyFoxContactArray = tempGreyFoxContactArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					GreyFoxContactArray[x] = GreyFoxContactArray[x - 1];
				GreyFoxContactArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((GreyFoxContact) value);
		}

		public void Remove(GreyFoxContact value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("GreyFoxContact not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					GreyFoxContactArray[x-1] = GreyFoxContactArray[x];
				GreyFoxContactArray[count - 1] = null;
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
				return GreyFoxContactArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return GreyFoxContactArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				GreyFoxContactArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(GreyFoxContactArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private GreyFoxContact[] GreyFoxContactArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(GreyFoxContact[] GreyFoxContactArray, int virtualCount)
			{
				this.GreyFoxContactArray = GreyFoxContactArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < GreyFoxContactArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public GreyFoxContact Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return GreyFoxContactArray[cursor];
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
		/// Makes a shallow copy of the current GreyFoxContactCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxContactCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current GreyFoxContactCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxContactCollection</returns>
		public GreyFoxContactCollection Clone()
		{
			GreyFoxContactCollection clonedGreyFoxContact = new GreyFoxContactCollection(count);
			lock(this)
			{
				foreach(GreyFoxContact item in this)
					clonedGreyFoxContact.Add(item);
			}
			return clonedGreyFoxContact;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxContact.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the GreyFoxContactCollection from their children.</param>
		public GreyFoxContactCollection Copy(bool isolated)
		{
			GreyFoxContactCollection isolatedCollection = new GreyFoxContactCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxContactArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxContactArray[i].Copy());
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
				Array.Sort(GreyFoxContactArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public GreyFoxContact Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxContactArray[x].ID == id)
						return GreyFoxContactArray[x];
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
				s.Append(GreyFoxContactArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
