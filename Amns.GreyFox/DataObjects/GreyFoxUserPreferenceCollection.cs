using System;
using System.Collections;

namespace Amns.GreyFox
{
	/// <summary>
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	/// </summary>
	public class GreyFoxUserPreferenceCollection : IList, ICloneable 
	{
		private int count = 0;
		private GreyFoxUserPreference[] GreyFoxUserPreferenceArray ;

		public GreyFoxUserPreferenceCollection() : base()
		{
			GreyFoxUserPreferenceArray = new GreyFoxUserPreference[15];
		}

		public GreyFoxUserPreferenceCollection(int capacity) : base()
		{
			GreyFoxUserPreferenceArray = new GreyFoxUserPreference[capacity];
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
				GreyFoxUserPreferenceArray[index] = (GreyFoxUserPreference) value;
			}
		}

		public GreyFoxUserPreference this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return GreyFoxUserPreferenceArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				GreyFoxUserPreferenceArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((GreyFoxUserPreference) value);
		}

		public int Add(GreyFoxUserPreference value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxUserPreferenceArray.GetUpperBound(0) + 1)
				{
					GreyFoxUserPreference[] tempGreyFoxUserPreferenceArray = new GreyFoxUserPreference[count * 2];
					Array.Copy(GreyFoxUserPreferenceArray, tempGreyFoxUserPreferenceArray, count - 1);
					GreyFoxUserPreferenceArray = tempGreyFoxUserPreferenceArray;
				}
				GreyFoxUserPreferenceArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				GreyFoxUserPreferenceArray = new GreyFoxUserPreference[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((GreyFoxUserPreference) value);
		}

		public bool Contains(GreyFoxUserPreference value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((GreyFoxUserPreference) value);
		}

		public int IndexOf(GreyFoxUserPreference value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxUserPreferenceArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (GreyFoxUserPreference) value);
		}

		public void Insert(int index, GreyFoxUserPreference value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxUserPreferenceArray.GetUpperBound(0) + 1)
				{
					GreyFoxUserPreference[] tempGreyFoxUserPreferenceArray = new GreyFoxUserPreference[count * 2];
					Array.Copy(GreyFoxUserPreferenceArray, tempGreyFoxUserPreferenceArray, count - 1);
					GreyFoxUserPreferenceArray = tempGreyFoxUserPreferenceArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					GreyFoxUserPreferenceArray[x] = GreyFoxUserPreferenceArray[x - 1];
				GreyFoxUserPreferenceArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((GreyFoxUserPreference) value);
		}

		public void Remove(GreyFoxUserPreference value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("GreyFoxUserPreference not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					GreyFoxUserPreferenceArray[x-1] = GreyFoxUserPreferenceArray[x];
				GreyFoxUserPreferenceArray[count - 1] = null;
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
				return GreyFoxUserPreferenceArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return GreyFoxUserPreferenceArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				GreyFoxUserPreferenceArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(GreyFoxUserPreferenceArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private GreyFoxUserPreference[] GreyFoxUserPreferenceArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(GreyFoxUserPreference[] GreyFoxUserPreferenceArray, int virtualCount)
			{
				this.GreyFoxUserPreferenceArray = GreyFoxUserPreferenceArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < GreyFoxUserPreferenceArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public GreyFoxUserPreference Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return GreyFoxUserPreferenceArray[cursor];
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
		/// Makes a shallow copy of the current GreyFoxUserPreferenceCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxUserPreferenceCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current GreyFoxUserPreferenceCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxUserPreferenceCollection</returns>
		public GreyFoxUserPreferenceCollection Clone()
		{
			GreyFoxUserPreferenceCollection clonedGreyFoxUserPreference = new GreyFoxUserPreferenceCollection(count);
			lock(this)
			{
				foreach(GreyFoxUserPreference item in this)
					clonedGreyFoxUserPreference.Add(item);
			}
			return clonedGreyFoxUserPreference;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxUserPreference.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the GreyFoxUserPreferenceCollection from their children.</param>
		public GreyFoxUserPreferenceCollection Copy(bool isolated)
		{
			GreyFoxUserPreferenceCollection isolatedCollection = new GreyFoxUserPreferenceCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxUserPreferenceArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxUserPreferenceArray[i].Copy());
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
				Array.Sort(GreyFoxUserPreferenceArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public GreyFoxUserPreference Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxUserPreferenceArray[x].ID == id)
						return GreyFoxUserPreferenceArray[x];
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
				s.Append(GreyFoxUserPreferenceArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
