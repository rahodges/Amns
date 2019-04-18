using System;
using System.Collections;

namespace Amns.GreyFox.Web.UI.WebControls.Tooltips
{
	/// <summary>
	/// <summary>
	/// Summary of Contact.
	/// </summary>
	/// </summary>
	public class TooltipCollection : IList, ICloneable 
	{
		private int count = 0;
		private Tooltip[] tooltips ;

		public TooltipCollection() : base()
		{
			tooltips = new Tooltip[15];
		}

		public TooltipCollection(int capacity) : base()
		{
			tooltips = new Tooltip[capacity];
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
				tooltips[index] = (Tooltip) value;
			}
		}

		public Tooltip this[int index]
		{
			get
			{
				lock(this)
				{
					if(index > count - 1)
						throw(new Exception("Index out of bounds."));
					return tooltips[index];
				}
			}
			set
			{
				OnCollectionChanged(EventArgs.Empty);
				tooltips[index] = value;
			}
		}

		int IList.Add(object value)
		{
			return Add((Tooltip) value);
		}

		public int Add(Tooltip value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > tooltips.GetUpperBound(0) + 1)
				{
					Tooltip[] temptooltips = new Tooltip[count * 2];
					Array.Copy(tooltips, temptooltips, count - 1);
					tooltips = temptooltips;
				}
				tooltips[count - 1] = value;
			}
			return count -1;
		}

		public void Clear()
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count = 0;
				tooltips = new Tooltip[15];
			}
		}

		bool IList.Contains(object value)
		{
			return Contains((Tooltip) value);
		}

		public bool Contains(Tooltip value)
		{
			return IndexOf(value) != -1;
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((Tooltip) value);
		}

		public int IndexOf(Tooltip value)
		{
			lock(this)
			{
				for(int x = 0; x < count; x++)
					if(tooltips[x].Equals(value))
						return x;
				return -1;
			}
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (Tooltip) value);
		}

		public void Insert(int index, Tooltip value)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				count++;
				// Resize the array if the count is greater than the length 
				// of the array.
				if(count > tooltips.GetUpperBound(0) + 1)
				{
					Tooltip[] temptooltips = new Tooltip[count * 2];
					Array.Copy(tooltips, temptooltips, count - 1);
					tooltips = temptooltips;
				}
				for(int x = index + 1; x == count - 2; x ++)
					tooltips[x] = tooltips[x - 1];
				tooltips[index] = value;
			}
		}

		void IList.Remove(object value)
		{
			Remove((Tooltip) value);
		}

		public void Remove(Tooltip value)
		{
			OnCollectionChanged(EventArgs.Empty);
			int index = IndexOf(value);
			if(index == -1)
				throw(new Exception("Tooltip not found in collection."));
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			OnCollectionChanged(EventArgs.Empty);
			lock(this)
			{
				for(int x = index + 1; x <= count - 1; x++)
					tooltips[x-1] = tooltips[x];
				tooltips[count - 1] = null;
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
				return tooltips.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return tooltips;
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock(this)
			{
				tooltips.CopyTo(array, index);
			}
		}

		#endregion

		#region IEnumerator Implementation

		public Enumerator GetEnumerator()
		{
			return new Enumerator(tooltips, count);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public class Enumerator : IEnumerator
		{
			private Tooltip[] tooltips;
			private int cursor;
			private int virtualCount;

			public Enumerator(Tooltip[] tooltips, int virtualCount)
			{
				this.tooltips = tooltips;
				this.virtualCount = virtualCount;
				cursor = -1;
			}

			public void Reset()
			{
				cursor = -1;
			}

			public bool MoveNext()
			{
				if(cursor < tooltips.Length)
					cursor++;
				return(!(cursor == virtualCount));
			}

			public Tooltip Current
			{
				get
				{
					if((cursor < 0) || (cursor == virtualCount))
						throw(new InvalidOperationException());
					return tooltips[cursor];
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

		#region ICloneable Implementation

		object ICloneable.Clone()
		{
			return Clone();
		}

		public TooltipCollection Clone()
		{
			TooltipCollection clonedTooltip = new TooltipCollection(count);
			lock(this)
			{
				foreach(Tooltip item in this)
					clonedTooltip.Add(item);
			}
			return clonedTooltip;
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
				Array.Sort(tooltips, 0, count);
			}
		}

		#endregion

	}
}