using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	/// </summary>
	public class DbContentStatusCollection : IList, ICloneable 
	{
		private int count = 0;
		private DbContentStatus[] DbContentStatusArray ;

		public DbContentStatusCollection() : base()
		{
			DbContentStatusArray = new DbContentStatus[15];
		}

		public DbContentStatusCollection(int capacity) : base()
		{
			DbContentStatusArray = new DbContentStatus[capacity];
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
				DbContentStatusArray[index] = (DbContentStatus) value;
			}
		}

		public DbContentStatus this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return DbContentStatusArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				DbContentStatusArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((DbContentStatus) value);
		}

		public int Add(DbContentStatus value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentStatusArray.GetUpperBound(0) + 1)
				{
					DbContentStatus[] tempDbContentStatusArray = new DbContentStatus[count * 2];
					Array.Copy(DbContentStatusArray, tempDbContentStatusArray, count - 1);
					DbContentStatusArray = tempDbContentStatusArray;
				}
				DbContentStatusArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				DbContentStatusArray = new DbContentStatus[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((DbContentStatus) value);
		}

		public bool Contains(DbContentStatus value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((DbContentStatus) value);
		}

		public int IndexOf(DbContentStatus value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentStatusArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (DbContentStatus) value);
		}

		public void Insert(int index, DbContentStatus value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentStatusArray.GetUpperBound(0) + 1)
				{
					DbContentStatus[] tempDbContentStatusArray = new DbContentStatus[count * 2];
					Array.Copy(DbContentStatusArray, tempDbContentStatusArray, count - 1);
					DbContentStatusArray = tempDbContentStatusArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					DbContentStatusArray[x] = DbContentStatusArray[x - 1];
				DbContentStatusArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((DbContentStatus) value);
		}

		public void Remove(DbContentStatus value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("DbContentStatus not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					DbContentStatusArray[x-1] = DbContentStatusArray[x];
				DbContentStatusArray[count - 1] = null;
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
				return DbContentStatusArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return DbContentStatusArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				DbContentStatusArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(DbContentStatusArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private DbContentStatus[] DbContentStatusArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(DbContentStatus[] DbContentStatusArray, int virtualCount)
			{
				this.DbContentStatusArray = DbContentStatusArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < DbContentStatusArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public DbContentStatus Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return DbContentStatusArray[cursor];
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
		/// Makes a shallow copy of the current DbContentStatusCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentStatusCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current DbContentStatusCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentStatusCollection</returns>
		public DbContentStatusCollection Clone()
		{
			DbContentStatusCollection clonedDbContentStatus = new DbContentStatusCollection(count);
			lock(this)
			{
				foreach(DbContentStatus item in this)
					clonedDbContentStatus.Add(item);
			}
			return clonedDbContentStatus;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentStatus.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the DbContentStatusCollection from their children.</param>
		public DbContentStatusCollection Copy(bool isolated)
		{
			DbContentStatusCollection isolatedCollection = new DbContentStatusCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentStatusArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentStatusArray[i].Copy());
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
				Array.Sort(DbContentStatusArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public DbContentStatus Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentStatusArray[x].ID == id)
						return DbContentStatusArray[x];
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
				s.Append(DbContentStatusArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
