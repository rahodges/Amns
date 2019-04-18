using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	/// </summary>
	public class DbContentHitCollection : IList, ICloneable 
	{
		private int count = 0;
		private DbContentHit[] DbContentHitArray ;

		public DbContentHitCollection() : base()
		{
			DbContentHitArray = new DbContentHit[15];
		}

		public DbContentHitCollection(int capacity) : base()
		{
			DbContentHitArray = new DbContentHit[capacity];
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
				DbContentHitArray[index] = (DbContentHit) value;
			}
		}

		public DbContentHit this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return DbContentHitArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				DbContentHitArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((DbContentHit) value);
		}

		public int Add(DbContentHit value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentHitArray.GetUpperBound(0) + 1)
				{
					DbContentHit[] tempDbContentHitArray = new DbContentHit[count * 2];
					Array.Copy(DbContentHitArray, tempDbContentHitArray, count - 1);
					DbContentHitArray = tempDbContentHitArray;
				}
				DbContentHitArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				DbContentHitArray = new DbContentHit[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((DbContentHit) value);
		}

		public bool Contains(DbContentHit value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((DbContentHit) value);
		}

		public int IndexOf(DbContentHit value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentHitArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (DbContentHit) value);
		}

		public void Insert(int index, DbContentHit value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentHitArray.GetUpperBound(0) + 1)
				{
					DbContentHit[] tempDbContentHitArray = new DbContentHit[count * 2];
					Array.Copy(DbContentHitArray, tempDbContentHitArray, count - 1);
					DbContentHitArray = tempDbContentHitArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					DbContentHitArray[x] = DbContentHitArray[x - 1];
				DbContentHitArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((DbContentHit) value);
		}

		public void Remove(DbContentHit value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("DbContentHit not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					DbContentHitArray[x-1] = DbContentHitArray[x];
				DbContentHitArray[count - 1] = null;
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
				return DbContentHitArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return DbContentHitArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				DbContentHitArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(DbContentHitArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private DbContentHit[] DbContentHitArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(DbContentHit[] DbContentHitArray, int virtualCount)
			{
				this.DbContentHitArray = DbContentHitArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < DbContentHitArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public DbContentHit Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return DbContentHitArray[cursor];
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
		/// Makes a shallow copy of the current DbContentHitCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentHitCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current DbContentHitCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentHitCollection</returns>
		public DbContentHitCollection Clone()
		{
			DbContentHitCollection clonedDbContentHit = new DbContentHitCollection(count);
			lock(this)
			{
				foreach(DbContentHit item in this)
					clonedDbContentHit.Add(item);
			}
			return clonedDbContentHit;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentHit.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the DbContentHitCollection from their children.</param>
		public DbContentHitCollection Copy(bool isolated)
		{
			DbContentHitCollection isolatedCollection = new DbContentHitCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentHitArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentHitArray[i].Copy());
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
				Array.Sort(DbContentHitArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public DbContentHit Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentHitArray[x].ID == id)
						return DbContentHitArray[x];
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
				s.Append(DbContentHitArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
