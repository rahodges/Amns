using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	/// </summary>
	public class DbContentCommentCollection : IList, ICloneable 
	{
		private int count = 0;
		private DbContentComment[] DbContentCommentArray ;

		public DbContentCommentCollection() : base()
		{
			DbContentCommentArray = new DbContentComment[15];
		}

		public DbContentCommentCollection(int capacity) : base()
		{
			DbContentCommentArray = new DbContentComment[capacity];
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
				DbContentCommentArray[index] = (DbContentComment) value;
			}
		}

		public DbContentComment this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return DbContentCommentArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				DbContentCommentArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((DbContentComment) value);
		}

		public int Add(DbContentComment value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentCommentArray.GetUpperBound(0) + 1)
				{
					DbContentComment[] tempDbContentCommentArray = new DbContentComment[count * 2];
					Array.Copy(DbContentCommentArray, tempDbContentCommentArray, count - 1);
					DbContentCommentArray = tempDbContentCommentArray;
				}
				DbContentCommentArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				DbContentCommentArray = new DbContentComment[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((DbContentComment) value);
		}

		public bool Contains(DbContentComment value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((DbContentComment) value);
		}

		public int IndexOf(DbContentComment value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentCommentArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (DbContentComment) value);
		}

		public void Insert(int index, DbContentComment value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentCommentArray.GetUpperBound(0) + 1)
				{
					DbContentComment[] tempDbContentCommentArray = new DbContentComment[count * 2];
					Array.Copy(DbContentCommentArray, tempDbContentCommentArray, count - 1);
					DbContentCommentArray = tempDbContentCommentArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					DbContentCommentArray[x] = DbContentCommentArray[x - 1];
				DbContentCommentArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((DbContentComment) value);
		}

		public void Remove(DbContentComment value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("DbContentComment not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					DbContentCommentArray[x-1] = DbContentCommentArray[x];
				DbContentCommentArray[count - 1] = null;
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
				return DbContentCommentArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return DbContentCommentArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				DbContentCommentArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(DbContentCommentArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private DbContentComment[] DbContentCommentArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(DbContentComment[] DbContentCommentArray, int virtualCount)
			{
				this.DbContentCommentArray = DbContentCommentArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < DbContentCommentArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public DbContentComment Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return DbContentCommentArray[cursor];
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
		/// Makes a shallow copy of the current DbContentCommentCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentCommentCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current DbContentCommentCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentCommentCollection</returns>
		public DbContentCommentCollection Clone()
		{
			DbContentCommentCollection clonedDbContentComment = new DbContentCommentCollection(count);
			lock(this)
			{
				foreach(DbContentComment item in this)
					clonedDbContentComment.Add(item);
			}
			return clonedDbContentComment;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentComment.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the DbContentCommentCollection from their children.</param>
		public DbContentCommentCollection Copy(bool isolated)
		{
			DbContentCommentCollection isolatedCollection = new DbContentCommentCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentCommentArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentCommentArray[i].Copy());
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
				Array.Sort(DbContentCommentArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public DbContentComment Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentCommentArray[x].ID == id)
						return DbContentCommentArray[x];
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
				s.Append(DbContentCommentArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
