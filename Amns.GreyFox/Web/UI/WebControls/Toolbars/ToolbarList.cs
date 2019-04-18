using System;
using System.Collections;

namespace Amns.GreyFox.Web.UI.WebControls
{
	/// <summary>
	/// Collection of Toolbars
	/// </summary>
	public class ToolbarList : IEnumerator, IEnumerable 
	{
		public ToolbarList() {}

		private ArrayList itemArray = new ArrayList(); 
		private int index = -1; 
		// Return this class as the Enumerator 

		public IEnumerator GetEnumerator() 
		{ 
			return (IEnumerator) this; 
		}

		public bool MoveNext() 
		{ 
			index++; 
			if (index < itemArray.Count) 
			{
				return true; 
			} 
			else  
			{
				return false; 
			}
		} 

		public void Reset() 
		{ 
			index = -1; 
		} 

		object IEnumerator.Current
		{
			get { return itemArray[index]; }
		}

		public object Current 
		{ 
			get{ return (Toolbar) itemArray[index]; } 
		}

		public void Add(Toolbar item) 
		{
			itemArray.Add(item);
		}

		public void Remove(Toolbar item) 
		{
			itemArray.Remove(item);
		}

		public void Clear() 
		{
			itemArray.Clear();
		}

		public Toolbar this[Int32 index] 
		{
			get { return (Toolbar) itemArray[index];}
			set { itemArray[index] = value;}
		}

		public int Count 
		{
			get {return itemArray.Count;}
		}
	}
}
