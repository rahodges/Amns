using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	/// </summary>
	public class DbContentCatalogCollection : IList, ICloneable 
	{
		private int count = 0;
		private DbContentCatalog[] DbContentCatalogArray ;

		public DbContentCatalogCollection() : base()
		{
			DbContentCatalogArray = new DbContentCatalog[15];
		}

		public DbContentCatalogCollection(int capacity) : base()
		{
			DbContentCatalogArray = new DbContentCatalog[capacity];
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
				DbContentCatalogArray[index] = (DbContentCatalog) value;
			}
		}

		public DbContentCatalog this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return DbContentCatalogArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				DbContentCatalogArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((DbContentCatalog) value);
		}

		public int Add(DbContentCatalog value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentCatalogArray.GetUpperBound(0) + 1)
				{
					DbContentCatalog[] tempDbContentCatalogArray = new DbContentCatalog[count * 2];
					Array.Copy(DbContentCatalogArray, tempDbContentCatalogArray, count - 1);
					DbContentCatalogArray = tempDbContentCatalogArray;
				}
				DbContentCatalogArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				DbContentCatalogArray = new DbContentCatalog[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((DbContentCatalog) value);
		}

		public bool Contains(DbContentCatalog value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((DbContentCatalog) value);
		}

		public int IndexOf(DbContentCatalog value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentCatalogArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (DbContentCatalog) value);
		}

		public void Insert(int index, DbContentCatalog value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentCatalogArray.GetUpperBound(0) + 1)
				{
					DbContentCatalog[] tempDbContentCatalogArray = new DbContentCatalog[count * 2];
					Array.Copy(DbContentCatalogArray, tempDbContentCatalogArray, count - 1);
					DbContentCatalogArray = tempDbContentCatalogArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					DbContentCatalogArray[x] = DbContentCatalogArray[x - 1];
				DbContentCatalogArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((DbContentCatalog) value);
		}

		public void Remove(DbContentCatalog value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("DbContentCatalog not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					DbContentCatalogArray[x-1] = DbContentCatalogArray[x];
				DbContentCatalogArray[count - 1] = null;
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
				return DbContentCatalogArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return DbContentCatalogArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				DbContentCatalogArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(DbContentCatalogArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private DbContentCatalog[] DbContentCatalogArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(DbContentCatalog[] DbContentCatalogArray, int virtualCount)
			{
				this.DbContentCatalogArray = DbContentCatalogArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < DbContentCatalogArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public DbContentCatalog Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return DbContentCatalogArray[cursor];
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
		/// Makes a shallow copy of the current DbContentCatalogCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentCatalogCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current DbContentCatalogCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentCatalogCollection</returns>
		public DbContentCatalogCollection Clone()
		{
			DbContentCatalogCollection clonedDbContentCatalog = new DbContentCatalogCollection(count);
			lock(this)
			{
				foreach(DbContentCatalog item in this)
					clonedDbContentCatalog.Add(item);
			}
			return clonedDbContentCatalog;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentCatalog.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the DbContentCatalogCollection from their children.</param>
		public DbContentCatalogCollection Copy(bool isolated)
		{
			DbContentCatalogCollection isolatedCollection = new DbContentCatalogCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentCatalogArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentCatalogArray[i].Copy());
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
				Array.Sort(DbContentCatalogArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public DbContentCatalog Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentCatalogArray[x].ID == id)
						return DbContentCatalogArray[x];
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
				s.Append(DbContentCatalogArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

		//--- Begin Custom Code ---
																																																																														
		public void Sort(params ContentCompareKey[] keys)
		{
			if(count > 0)
				Array.Sort(DbContentCatalogArray, 0, count, new ContentComparer(keys));
		}

		//--- End Custom Code ---
	}
}
