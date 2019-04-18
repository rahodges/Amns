using System;
using System.Collections;

namespace Amns.GreyFox.Web.UI.WebControls {
	/// <summary>
	/// Collection of Tabs for Toolbar
	/// </summary>
	public class TabList  : IEnumerator, IEnumerable
	{
		public TabList() {}
	
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
			get { return (Tab) itemArray[index]; } 
		}

		public void Add(Tab item) 
		{
			itemArray.Add(item);
		}

		public void Remove(Tab item) 
		{
			itemArray.Remove(item);
		}

		public void Clear() 
		{
			itemArray.Clear();
		}
		/// <summary>
		/// The Items (ToolbarButton, ToolbarDropDownList, ToolbarSeparator) in the toolbar
		/// </summary>
		public Tab this[Int32 index] 
		{
			get { return (Tab) itemArray[index];}
			set { itemArray[index] = value;}
		}
		public int Count 
		{
			get {return itemArray.Count;}
		}
	}
}
