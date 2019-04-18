using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// <summary>
	/// Database clip for Amns.GreyFox CMS.
	/// </summary>
	/// </summary>
	public class DbContentClipCollection : IList, ICloneable 
	{
		private int count = 0;
		private DbContentClip[] DbContentClipArray ;

		public DbContentClipCollection() : base()
		{
			DbContentClipArray = new DbContentClip[15];
		}

		public DbContentClipCollection(int capacity) : base()
		{
			DbContentClipArray = new DbContentClip[capacity];
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
				DbContentClipArray[index] = (DbContentClip) value;
			}
		}

		public DbContentClip this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return DbContentClipArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				DbContentClipArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((DbContentClip) value);
		}

		public int Add(DbContentClip value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentClipArray.GetUpperBound(0) + 1)
				{
					DbContentClip[] tempDbContentClipArray = new DbContentClip[count * 2];
					Array.Copy(DbContentClipArray, tempDbContentClipArray, count - 1);
					DbContentClipArray = tempDbContentClipArray;
				}
				DbContentClipArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				DbContentClipArray = new DbContentClip[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((DbContentClip) value);
		}

		public bool Contains(DbContentClip value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((DbContentClip) value);
		}

		public int IndexOf(DbContentClip value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentClipArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (DbContentClip) value);
		}

		public void Insert(int index, DbContentClip value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentClipArray.GetUpperBound(0) + 1)
				{
					DbContentClip[] tempDbContentClipArray = new DbContentClip[count * 2];
					Array.Copy(DbContentClipArray, tempDbContentClipArray, count - 1);
					DbContentClipArray = tempDbContentClipArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					DbContentClipArray[x] = DbContentClipArray[x - 1];
				DbContentClipArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((DbContentClip) value);
		}

		public void Remove(DbContentClip value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("DbContentClip not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					DbContentClipArray[x-1] = DbContentClipArray[x];
				DbContentClipArray[count - 1] = null;
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
				return DbContentClipArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return DbContentClipArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				DbContentClipArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(DbContentClipArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private DbContentClip[] DbContentClipArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(DbContentClip[] DbContentClipArray, int virtualCount)
			{
				this.DbContentClipArray = DbContentClipArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < DbContentClipArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public DbContentClip Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return DbContentClipArray[cursor];
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
		/// Makes a shallow copy of the current DbContentClipCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentClipCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current DbContentClipCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentClipCollection</returns>
		public DbContentClipCollection Clone()
		{
			DbContentClipCollection clonedDbContentClip = new DbContentClipCollection(count);
			lock(this)
			{
				foreach(DbContentClip item in this)
					clonedDbContentClip.Add(item);
			}
			return clonedDbContentClip;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentClip.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the DbContentClipCollection from their children.</param>
		public DbContentClipCollection Copy(bool isolated)
		{
			DbContentClipCollection isolatedCollection = new DbContentClipCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentClipArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentClipArray[i].Copy());
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
				Array.Sort(DbContentClipArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public DbContentClip Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentClipArray[x].ID == id)
						return DbContentClipArray[x];
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
				s.Append(DbContentClipArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
