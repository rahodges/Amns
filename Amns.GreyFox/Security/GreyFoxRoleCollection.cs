using System;
using System.Collections;

namespace Amns.GreyFox.Security
{
	/// <summary>
	/// <summary>
	/// Role class to hold permissions for users.
	/// </summary>
	/// </summary>
	public class GreyFoxRoleCollection : IList, ICloneable 
	{
		private int count = 0;
		private GreyFoxRole[] GreyFoxRoleArray ;

		public GreyFoxRoleCollection() : base()
		{
			GreyFoxRoleArray = new GreyFoxRole[15];
		}

		public GreyFoxRoleCollection(int capacity) : base()
		{
			GreyFoxRoleArray = new GreyFoxRole[capacity];
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
				GreyFoxRoleArray[index] = (GreyFoxRole) value;
			}
		}

		public GreyFoxRole this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return GreyFoxRoleArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				GreyFoxRoleArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((GreyFoxRole) value);
		}

		public int Add(GreyFoxRole value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxRoleArray.GetUpperBound(0) + 1)
				{
					GreyFoxRole[] tempGreyFoxRoleArray = new GreyFoxRole[count * 2];
					Array.Copy(GreyFoxRoleArray, tempGreyFoxRoleArray, count - 1);
					GreyFoxRoleArray = tempGreyFoxRoleArray;
				}
				GreyFoxRoleArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				GreyFoxRoleArray = new GreyFoxRole[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((GreyFoxRole) value);
		}

		public bool Contains(GreyFoxRole value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((GreyFoxRole) value);
		}

		public int IndexOf(GreyFoxRole value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxRoleArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (GreyFoxRole) value);
		}

		public void Insert(int index, GreyFoxRole value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxRoleArray.GetUpperBound(0) + 1)
				{
					GreyFoxRole[] tempGreyFoxRoleArray = new GreyFoxRole[count * 2];
					Array.Copy(GreyFoxRoleArray, tempGreyFoxRoleArray, count - 1);
					GreyFoxRoleArray = tempGreyFoxRoleArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					GreyFoxRoleArray[x] = GreyFoxRoleArray[x - 1];
				GreyFoxRoleArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((GreyFoxRole) value);
		}

		public void Remove(GreyFoxRole value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("GreyFoxRole not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					GreyFoxRoleArray[x-1] = GreyFoxRoleArray[x];
				GreyFoxRoleArray[count - 1] = null;
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
				return GreyFoxRoleArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return GreyFoxRoleArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				GreyFoxRoleArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(GreyFoxRoleArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private GreyFoxRole[] GreyFoxRoleArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(GreyFoxRole[] GreyFoxRoleArray, int virtualCount)
			{
				this.GreyFoxRoleArray = GreyFoxRoleArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < GreyFoxRoleArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public GreyFoxRole Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return GreyFoxRoleArray[cursor];
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
		/// Makes a shallow copy of the current GreyFoxRoleCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxRoleCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current GreyFoxRoleCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxRoleCollection</returns>
		public GreyFoxRoleCollection Clone()
		{
			GreyFoxRoleCollection clonedGreyFoxRole = new GreyFoxRoleCollection(count);
			lock(this)
			{
				foreach(GreyFoxRole item in this)
					clonedGreyFoxRole.Add(item);
			}
			return clonedGreyFoxRole;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxRole.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the GreyFoxRoleCollection from their children.</param>
		public GreyFoxRoleCollection Copy(bool isolated)
		{
			GreyFoxRoleCollection isolatedCollection = new GreyFoxRoleCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxRoleArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxRoleArray[i].Copy());
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
				Array.Sort(GreyFoxRoleArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public GreyFoxRole Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxRoleArray[x].ID == id)
						return GreyFoxRoleArray[x];
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
				s.Append(GreyFoxRoleArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

		//--- Begin Custom Code ---
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																													
		public string[] RolesNamesToArray()
		{
			string[] rolenames = new string[this.count];
			for(int x = 0; x < count; x++)
				rolenames[x] = GreyFoxRoleArray[x].Name;
			return rolenames;
		}

        public string ToEncodedString(string separator, string lastSeparator)
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder();

            for (int i = 0; i < this.Count; i++)
            {
                if (i > 0)
                {
                    if (this.Count == 2 | this.Count == i+1)
                        s.Append(lastSeparator);
                    else
                        s.Append(separator);
                }

                s.Append(this[i].Name);
            }

            return s.ToString();
        }

		//--- End Custom Code ---
	}
}
