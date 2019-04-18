using System;
using System.Collections;

namespace Amns.GreyFox.Security
{
	/// <summary>
	/// <summary>
	/// Summary of User
	/// </summary>
	/// </summary>
	public class GreyFoxUserCollection : IList, ICloneable 
	{
		private int count = 0;
		private GreyFoxUser[] GreyFoxUserArray ;

		public GreyFoxUserCollection() : base()
		{
			GreyFoxUserArray = new GreyFoxUser[15];
		}

		public GreyFoxUserCollection(int capacity) : base()
		{
			GreyFoxUserArray = new GreyFoxUser[capacity];
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
				GreyFoxUserArray[index] = (GreyFoxUser) value;
			}
		}

		public GreyFoxUser this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return GreyFoxUserArray[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				GreyFoxUserArray[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((GreyFoxUser) value);
		}

		public int Add(GreyFoxUser value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxUserArray.GetUpperBound(0) + 1)
				{
					GreyFoxUser[] tempGreyFoxUserArray = new GreyFoxUser[count * 2];
					Array.Copy(GreyFoxUserArray, tempGreyFoxUserArray, count - 1);
					GreyFoxUserArray = tempGreyFoxUserArray;
				}
				GreyFoxUserArray[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				GreyFoxUserArray = new GreyFoxUser[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((GreyFoxUser) value);
		}

		public bool Contains(GreyFoxUser value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((GreyFoxUser) value);
		}

		public int IndexOf(GreyFoxUser value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxUserArray[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (GreyFoxUser) value);
		}

		public void Insert(int index, GreyFoxUser value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > GreyFoxUserArray.GetUpperBound(0) + 1)
				{
					GreyFoxUser[] tempGreyFoxUserArray = new GreyFoxUser[count * 2];
					Array.Copy(GreyFoxUserArray, tempGreyFoxUserArray, count - 1);
					GreyFoxUserArray = tempGreyFoxUserArray;
				}
				for(int x = index + 1; x == count - 2; x ++)
					GreyFoxUserArray[x] = GreyFoxUserArray[x - 1];
				GreyFoxUserArray[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((GreyFoxUser) value);
		}

		public void Remove(GreyFoxUser value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("GreyFoxUser not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					GreyFoxUserArray[x-1] = GreyFoxUserArray[x];
				GreyFoxUserArray[count - 1] = null;
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
				return GreyFoxUserArray.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return GreyFoxUserArray;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				GreyFoxUserArray.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(GreyFoxUserArray, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private GreyFoxUser[] GreyFoxUserArray;
			private int cursor;
			private int virtualCount;

			public Enumerator(GreyFoxUser[] GreyFoxUserArray, int virtualCount)
			{
				this.GreyFoxUserArray = GreyFoxUserArray;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < GreyFoxUserArray.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public GreyFoxUser Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return GreyFoxUserArray[cursor];
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
		/// Makes a shallow copy of the current GreyFoxUserCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxUserCollection</returns>
		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Makes a shallow copy of the current GreyFoxUserCollection.
		/// as the parent object.
		/// </summary>
		/// <returns>GreyFoxUserCollection</returns>
		public GreyFoxUserCollection Clone()
		{
			GreyFoxUserCollection clonedGreyFoxUser = new GreyFoxUserCollection(count);
			lock(this)
			{
				foreach(GreyFoxUser item in this)
					clonedGreyFoxUser.Add(item);
			}
			return clonedGreyFoxUser;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxUser.
		/// </summary>
		/// <param name="isolation">Placeholders are used to isolate the 
		/// items in the GreyFoxUserCollection from their children.</param>
		public GreyFoxUserCollection Copy(bool isolated)
		{
			GreyFoxUserCollection isolatedCollection = new GreyFoxUserCollection(count);

			lock(this)
			{
				if(isolated)
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxUserArray[i].NewPlaceHolder());
					}
				}
				else
				{
					for(int i = 0; i < count; i++)
					{
						isolatedCollection.Add(GreyFoxUserArray[i].Copy());
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
				Array.Sort(GreyFoxUserArray, 0, count);
			}
		}

		#endregion

		#region Find Methods

		/// <summary>
		/// Finds a record by ID.
		/// </summary>
		public GreyFoxUser Find(int id)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(GreyFoxUserArray[x].ID == id)
						return GreyFoxUserArray[x];
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
				s.Append(GreyFoxUserArray[x].ToString());
			}

			return s.ToString();
		}

		#endregion

		//--- Begin Custom Code ---
																																																																																																																										
		public string ToEncodedString(string separator, string lastSeparator)
		{
			System.Text.StringBuilder s = new System.Text.StringBuilder();

			for(int i = 0; i < this.Count; i++)
			{
				if(i > 0)
				{
					if(this.Count == 2 | this.Count == i+1)
                        s.Append(lastSeparator);
					else
                        s.Append(separator);
				}

				s.Append(this[i].UserName);
			}

			return s.ToString();
		}

		public string ToContactNameString()
		{

			System.Text.StringBuilder s = new System.Text.StringBuilder();

			for(int i = 0; i < this.Count; i++)
			{
				if(i > 0)
				{
					if(this.Count == 2 | this.Count == i+1)
						s.Append(" and ");
					else
						s.Append(", ");
				}

				s.Append(this[i].Contact.ConstructName("F L S"));
			}

			return s.ToString();
		}

		//--- End Custom Code ---
	}
}
