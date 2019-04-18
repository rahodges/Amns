using System;
using System.Collections;

namespace Amns.GreyFox
{
	/// <summary>
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	/// </summary>
	public class GreyFoxSettingCollection : IList, ICloneable 
	{
		private int count = 0;
		private GreyFoxSetting[] GreyFoxSettingArray ;

		public GreyFoxSettingCollection() : base()
		{
			GreyFoxSettingArray = new GreyFoxSetting[15];
		}

		public GreyFoxSettingCollection(int capacity) : base()
		{
			GreyFoxSettingArray = new GreyFoxSetting[capacity];
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
				GreyFoxSettingArray[index] = (GreyFoxSetting) value;
			}
		}

		public GreyFoxSetting this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return GreyFoxSettingArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				GreyFoxSettingArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((GreyFoxSetting) value);
		}

		public int Add(GreyFoxSetting value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxSettingArray.GetUpperBound(0) + 1)
				{
					GreyFoxSetting[] tempGreyFoxSettingArray = new GreyFoxSetting[count * 2];
					Array.Copy(GreyFoxSettingArray, tempGreyFoxSettingArray, count - 1);
					GreyFoxSettingArray = tempGreyFoxSettingArray;
				}
				GreyFoxSettingArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				GreyFoxSettingArray = new GreyFoxSetting[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((GreyFoxSetting) value);
		}

		public bool Contains(GreyFoxSetting value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((GreyFoxSetting) value);
		}

		public int IndexOf(GreyFoxSetting value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxSettingArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (GreyFoxSetting) value);
		}

		public void Insert(int index, GreyFoxSetting value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxSettingArray.GetUpperBound(0) + 1)
				{
					GreyFoxSetting[] tempGreyFoxSettingArray = new GreyFoxSetting[count * 2];
					Array.Copy(GreyFoxSettingArray, tempGreyFoxSettingArray, count - 1);
					GreyFoxSettingArray = tempGreyFoxSettingArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					GreyFoxSettingArray[x] = GreyFoxSettingArray[x - 1];
				GreyFoxSettingArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((GreyFoxSetting) value);
		}

		public void Remove(GreyFoxSetting value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("GreyFoxSetting not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					GreyFoxSettingArray[x-1] = GreyFoxSettingArray[x];
				GreyFoxSettingArray[count - 1] = null;
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
				return GreyFoxSettingArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return GreyFoxSettingArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				GreyFoxSettingArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(GreyFoxSettingArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private GreyFoxSetting[] GreyFoxSettingArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(GreyFoxSetting[] GreyFoxSettingArray, int virtualCount)
			{
				this.GreyFoxSettingArray = GreyFoxSettingArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < GreyFoxSettingArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public GreyFoxSetting Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return GreyFoxSettingArray[cursor];
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
		/// Makes a shallow copy of the current GreyFoxSettingCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxSettingCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current GreyFoxSettingCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxSettingCollection</returns>
		public GreyFoxSettingCollection Clone()
		{
			GreyFoxSettingCollection clonedGreyFoxSetting = new GreyFoxSettingCollection(count);
			lock(this)
			{
				foreach(GreyFoxSetting item in this)
					clonedGreyFoxSetting.Add(item);
			}
			return clonedGreyFoxSetting;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxSetting.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the GreyFoxSettingCollection from their children.</param>
		public GreyFoxSettingCollection Copy(bool isolated)
		{
			GreyFoxSettingCollection isolatedCollection = new GreyFoxSettingCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxSettingArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxSettingArray[i].Copy());
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
				Array.Sort(GreyFoxSettingArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public GreyFoxSetting Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxSettingArray[x].ID == id)
						return GreyFoxSettingArray[x];
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
				s.Append(GreyFoxSettingArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
