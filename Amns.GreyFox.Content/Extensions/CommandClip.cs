using System;
using System.Xml;

namespace Amns.GreyFox.Content.Extensions
{
	public class DbCommandClip
	{
		private DbContentClip _clip;        
		private XmlDocument _xml;

		public DbCommandClip(DbContentClip clip)
		{
			if(clip == null)
				throw(new Exception("DbContentClip cannot be null."));

			_clip = clip;
			_xml = new XmlDocument();

			try
			{
				_xml.LoadXml(_clip.Body);
			}
			catch
			{
				throw(exceptionConstructor("Could not load XML Command Document from DbContentClip '{0}'."));
			}
		}

		/// <summary>
		/// Accesses the forward reference specified by the command document.
		/// </summary>
		public DbContentClip ForwardReference
		{
			get
			{
				string referenceString;
				
				XmlNodeList nodes = _xml.GetElementsByTagName("forwardreference");
				if(nodes.Count == 0)
					return null;
				referenceString = nodes[0].InnerText;

				int referenceID;

				try
				{
					referenceID = int.Parse(referenceString);
				}
				catch
				{
					throw(exceptionConstructor("Forward reference formatted incorrectly in DbContentClip '{0}'."));
				}

				return new DbContentClip(referenceID);
			}
		}

		private Exception exceptionConstructor(string message)
		{
			Exception e;
			if(_clip == null)
				e = new Exception(message);
			else
				e = new Exception(string.Format(message, _clip.ID));
			return e;
		}
	}
}
