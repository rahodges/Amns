using System;
using System.IO;
using System.Xml;
using System.Web;

namespace Amns.GreyFox.Content
{
	public class XmlClip : ContentClip
	{
		protected internal String _XmlPath;			// Path that clip information was read from
		protected internal String _GroupID;
		protected internal String _ClipID;

		protected internal String author;
		protected internal String owner;

		private bool overrideEnabled;
		private string overrideUrl;

		#region Public Properties

		public string XmlPath
		{
			get
			{
				return _XmlPath;
			}
		}

		public bool IsOverriden
		{
			get
			{
				return overrideEnabled;
			}
		}

		public string OverrideUrl
		{
			get
			{
				return overrideUrl;
			}
		}

		public bool HasClientScriptBlock
		{
			get
			{
				return clientScriptBlockOverriden | clientScriptBlockEnabled;
			}
		}

		public string ClientScriptBlock
		{
			get
			{
				if(clientScriptBlockOverriden && !clientScriptBlockLoaded)
				{
					try
					{
						StreamReader r = new StreamReader(HttpContext.Current.Server.MapPath(clientScriptBlockOverrideUrl), System.Text.Encoding.UTF8);
						clientScriptBlock = r.ReadToEnd();
						r.Close();
						clientScriptBlockLoaded = true;
					}
					catch (FileNotFoundException)
					{
						clientScriptBlock = string.Format("Could not find file \"{0}\".", clientScriptBlockOverrideUrl);
					}
				}
				return clientScriptBlock;
			}
		}

		#endregion

		private bool clientScriptBlockOverriden;
		private bool clientScriptBlockLoaded;
		private bool clientScriptBlockEnabled;
		private string clientScriptBlockOverrideUrl = string.Empty;
		private string clientScriptBlock = string.Empty;

		public XmlClip()
		{
		}

		public XmlClip(string xmlPath, string groupID, string clipID)
		{
			_XmlPath = xmlPath;
			_GroupID = groupID;
			_ClipID = clipID;
		}

		public void Load(string xmlPath, string groupID, string clipID)
		{
			_XmlPath = xmlPath;
			_GroupID = groupID;
			_ClipID = clipID;

			Load();
		}

		public void Load()
		{
			XmlDocument myXmlDocument = new XmlDocument();
			myXmlDocument.Load(_XmlPath);

			XmlNode clipNode = myXmlDocument.SelectSingleNode(string.Format("SiteContent/ContentGroup[@ID='{0}']/Clip[@ID='{1}']", _GroupID, _ClipID));
			
			if(clipNode == null)
			{
				FileInfo file = new FileInfo(_XmlPath);
				throw(new FileLoadException(string.Format("Cannot load clip requested '{0}/{1}' from '{2}'.", _GroupID, _ClipID, file.Name), _XmlPath));
			}

			foreach(XmlAttribute attribute in clipNode.Attributes)
			{
				switch(attribute.Name)
				{
					case "Author":
						author = attribute.Value;
						break;
					case "Owner":
						owner = attribute.Value;
						break;
					case "CreateDate":
						createDate = DateTime.Parse(attribute.Value);
						break;
					case "ModifyDate":
						modifyDate = DateTime.Parse(attribute.Value);
						break;
					case "PressDate":
						pressDate = DateTime.Parse(attribute.Value);
						break;
					case "ExpireDate":
						expireDate = DateTime.Parse(attribute.Value);
						break;
					case "CacheDisabled":
						if(attribute.Value.ToLower() == "True")
							cacheDisabled = true;
						break;
				}
			}

			foreach(XmlNode clipChildNode in clipNode.ChildNodes)
			{
				switch(clipChildNode.Name)
				{
					case "Title":
						title = clipChildNode.InnerText;
						break;
					case "OverrideUrl":
						overrideEnabled = true;
						overrideUrl = clipChildNode.InnerText;
						break;
					case "ClientScriptBlockOverrideUrl":
						clientScriptBlockOverriden = true;
						clientScriptBlockOverrideUrl = clipChildNode.InnerText;
						break;
					case "ClientScriptBlock":
						clientScriptBlockEnabled = true;
						clientScriptBlock = clipChildNode.InnerText;
						break;
					case "ClientCssLink":
						clientCssLink = clipChildNode.InnerText;
						break;
					case "Content":
						contentString = clipChildNode.InnerText;
						break;
				}
			}
			
			isSynced = true;
		}

		public string ToHtml()
		{
			if(!isSynced)
				throw(new InvalidOperationException("The clip requested has not been synchronized with a source xml file."));
			if(overrideEnabled)
				throw(new InvalidOperationException("The clip requested is overidden, internal content text is null."));

			return contentString;
		}
	}
}