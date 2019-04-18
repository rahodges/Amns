using System;
using System.Collections;

namespace Amns.GreyFox.Data
{
	public class ExposedManagerCollection : IList, ICloneable 
	{
		private int count = 0;
		private ExposedManager[] managers;

		public ExposedManagerCollection() : base()
		{
			managers = new ExposedManager[15];
		}

		public ExposedManagerCollection(int capacity) : base()
		{
			managers = new ExposedManager[capacity];
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
				managers[index] = (ExposedManager) value;
			}
		}

		public ExposedManager this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return managers[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				managers[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((ExposedManager) value);
		}

		public int Add(ExposedManager value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > managers.GetUpperBound(0) + 1)
				{
					ExposedManager[] temps = new ExposedManager[count * 2];
					Array.Copy(managers, temps, count - 1);
					managers = temps;
				}
				managers[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				managers = new ExposedManager[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((ExposedManager) value);
		}

		public bool Contains(ExposedManager value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((ExposedManager) value);
		}

		public int IndexOf(ExposedManager value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(managers[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (ExposedManager) value);
		}

		public void Insert(int index, ExposedManager value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > managers.GetUpperBound(0) + 1)
				{
					ExposedManager[] temps = new ExposedManager[count * 2];
					Array.Copy(managers, temps, count - 1);
					managers = temps;
				}
				for(int x = index + 1; x == count - 2; x ++)
					managers[x] = managers[x - 1];
				managers[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((ExposedManager) value);
		}

		public void Remove(ExposedManager value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("ExposedManager not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					managers[x-1] = managers[x];
				managers[count - 1] = null;
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
				return managers.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return managers;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				managers.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(managers, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private ExposedManager[] managers;
			private int cursor;
			private int virtualCount;

			public Enumerator(ExposedManager[] managers, int virtualCount)
			{
				this.managers = managers;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < managers.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public ExposedManager Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return managers[cursor];
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
		/// Makes a shallow copy of the current ExposedManagerCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>ExposedManagerCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current ExposedManagerCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>ExposedManagerCollection</returns>
		public ExposedManagerCollection Clone()
		{
			ExposedManagerCollection clonedGreyFoxContact = new ExposedManagerCollection(count);
			lock(this)
			{
				foreach(ExposedManager item in this)
					clonedGreyFoxContact.Add(item);
			}
			return clonedGreyFoxContact;
		}

		/// <summary>
		/// Makes a deep copy of the current ExposedManager.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the ExposedManagerCollection from their children.</param>
		public ExposedManagerCollection Copy(bool isolated)
		{
			ExposedManagerCollection isolatedCollection = new ExposedManagerCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(managers[i]);
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(managers[i]);
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
				Array.Sort(managers, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
//		public ExposedManager Find(int id)
//		{
//			lock(this)
//			{
//				for(int x = 0; x < count; x++)
//					if(managers[x].ID == id)
//						return managers[x];
//			}
//			return null;
//		}

		#endregion

		#region ToString() Override Apply

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
				s.Append(managers[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
