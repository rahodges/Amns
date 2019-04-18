using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	/// </summary>
	public class DbContentRatingCollection : IList, ICloneable 
	{
		private int count = 0;
		private DbContentRating[] DbContentRatingArray ;

		public DbContentRatingCollection() : base()
		{
			DbContentRatingArray = new DbContentRating[15];
		}

		public DbContentRatingCollection(int capacity) : base()
		{
			DbContentRatingArray = new DbContentRating[capacity];
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
				DbContentRatingArray[index] = (DbContentRating) value;
			}
		}

		public DbContentRating this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return DbContentRatingArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				DbContentRatingArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((DbContentRating) value);
		}

		public int Add(DbContentRating value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentRatingArray.GetUpperBound(0) + 1)
				{
					DbContentRating[] tempDbContentRatingArray = new DbContentRating[count * 2];
					Array.Copy(DbContentRatingArray, tempDbContentRatingArray, count - 1);
					DbContentRatingArray = tempDbContentRatingArray;
				}
				DbContentRatingArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				DbContentRatingArray = new DbContentRating[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((DbContentRating) value);
		}

		public bool Contains(DbContentRating value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((DbContentRating) value);
		}

		public int IndexOf(DbContentRating value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentRatingArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (DbContentRating) value);
		}

		public void Insert(int index, DbContentRating value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > DbContentRatingArray.GetUpperBound(0) + 1)
				{
					DbContentRating[] tempDbContentRatingArray = new DbContentRating[count * 2];
					Array.Copy(DbContentRatingArray, tempDbContentRatingArray, count - 1);
					DbContentRatingArray = tempDbContentRatingArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					DbContentRatingArray[x] = DbContentRatingArray[x - 1];
				DbContentRatingArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((DbContentRating) value);
		}

		public void Remove(DbContentRating value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("DbContentRating not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					DbContentRatingArray[x-1] = DbContentRatingArray[x];
				DbContentRatingArray[count - 1] = null;
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
				return DbContentRatingArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return DbContentRatingArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				DbContentRatingArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(DbContentRatingArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private DbContentRating[] DbContentRatingArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(DbContentRating[] DbContentRatingArray, int virtualCount)
			{
				this.DbContentRatingArray = DbContentRatingArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < DbContentRatingArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public DbContentRating Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return DbContentRatingArray[cursor];
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
		/// Makes a shallow copy of the current DbContentRatingCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentRatingCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current DbContentRatingCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>DbContentRatingCollection</returns>
		public DbContentRatingCollection Clone()
		{
			DbContentRatingCollection clonedDbContentRating = new DbContentRatingCollection(count);
			lock(this)
			{
				foreach(DbContentRating item in this)
					clonedDbContentRating.Add(item);
			}
			return clonedDbContentRating;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentRating.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the DbContentRatingCollection from their children.</param>
		public DbContentRatingCollection Copy(bool isolated)
		{
			DbContentRatingCollection isolatedCollection = new DbContentRatingCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentRatingArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(DbContentRatingArray[i].Copy());
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
				Array.Sort(DbContentRatingArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public DbContentRating Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(DbContentRatingArray[x].ID == id)
						return DbContentRatingArray[x];
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
				s.Append(DbContentRatingArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

	}
}
