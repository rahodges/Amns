using System;
using System.Collections;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary description for XmlClipCollection.
	/// </summary>
	public class XmlClipCollection : IList
	{
		private XmlClip[] xmlClips;
		private bool locked;

		public XmlClipCollection()
		{
			
		}

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			if(array = null)
                array = Array.CreateInstance(typeof(XmlClip), xmlClips.Length);
			// Remains to be finished
		}

		public int Count
		{
			get
			{
				return xmlClips.Length;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		#endregion

		#region IEnumberable Members

		public IEnumerator GetEnumerator()
		{
			return xmlClips.GetEnumerator();
		}

		#endregion

		#region IList Members

		public void Add(XmlClip xClip)
		{

		}

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

		public XmlClip this[int index]
		{
			get
			{
				return xmlClips[index];
			}
		}

		#endregion
		}
}