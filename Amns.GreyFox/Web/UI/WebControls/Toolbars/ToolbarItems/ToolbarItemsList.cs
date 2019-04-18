using System;
using System.Collections;

namespace Amns.GreyFox.Web.UI.WebControls {
	/// <summary>
	/// Collection of ToolbarItems for Toolbar
	/// </summary>
	public class ToolbarItemsList  : IEnumerator, IEnumerable
	{
		public ToolbarItemsList() {}
	
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
			get { return (ToolbarItem) itemArray[index]; } 
		}

		public void Add(ToolbarItem item) 
		{
			itemArray.Add(item);
		}

		public void Remove(ToolbarItem item) 
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
		public ToolbarItem this[Int32 index] 
		{
			get { return (ToolbarItem) itemArray[index];}
			set { itemArray[index] = value;}
		}
		public int Count 
		{
			get {return itemArray.Count;}
		}
	}
}
