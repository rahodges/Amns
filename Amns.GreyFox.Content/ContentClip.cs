using System;
using System.Text;

namespace Amns.GreyFox.Content
{
	public enum ContentLocation: byte {InternalHtml, ExternalHtmlPath};

	/// <summary>
	/// Use when displaying cached clips instead of calling the class.
	/// </summary>
	public interface IContentClip
	{
		string ToHtml();
	}

	/// <summary>
	/// Summary description for ContentClip.
	/// </summary>
	public class ContentClip
	{
		protected internal string title;
		protected internal string subtitle;				
		protected internal string contentString;
		protected internal string description;
		protected internal string clientCssLink = string.Empty;
		protected internal ContentType contentType;

		protected internal DateTime createDate = DateTime.Now;			// Date the content was first created
		protected internal DateTime modifyDate = DateTime.Now;			// Date the content was last modified
		protected internal DateTime pressDate = DateTime.MinValue;		// Date to publish content - minvalue if immediate
		protected internal DateTime expireDate = DateTime.MaxValue;	// Date to expire content - maxvalue if not expired
				
		protected internal string[] keywords;							// Keywords for content, stored in separate table

		protected internal bool cacheDisabled;						// Disables caching on a clip by clip basis
		internal bool isSynced							= false;

		#region Public Properties

		public bool CacheDisabled
		{
			get
			{
				return cacheDisabled;
			}
		}

		public string ClientCssLink
		{
			get
			{
				return clientCssLink;
			}
		}

		public ContentClip()
		{
		}

		public ContentClip(string title, string subtitle, string contentString,
			DateTime createDate, DateTime modifyDate, DateTime expireDate, string[] keywords)
		{
			this.title = title;
            this.subtitle = subtitle;
            this.contentString = contentString;
            this.createDate = createDate;
            this.modifyDate = modifyDate;
            this.expireDate = expireDate;
            this.keywords = keywords;
		}

		public string[] Keywords
		{
			get
			{
				return keywords;
			}
		}

		public DateTime CreateDate
		{
			get
			{
				return createDate;
			}
			set
			{
				createDate = value;
			}
		}

		public DateTime ModifyDate
		{
			get
			{
				return modifyDate;
			}
			set
			{
				modifyDate = value;
			}
		}


		public DateTime ExpireDate
		{
			get
			{
				return expireDate;
			}
			set
			{
				expireDate = value;
			}
		}


		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		public string Subtitle
		{
			get
			{
				return subtitle;
			}
			set
			{
				subtitle = value;
			}
		}

		public string ContentString
		{
			get
			{
				return contentString;
			}
			set
			{
				contentString = value;
			}
		}

		public ContentType ContentType
		{
			get
			{
				return contentType;
			}
			set
			{
				contentType = value;
			}
		}
		


		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		#endregion

		//--- Begin Custom Code ---

		public override string ToString()
		{
			return title;
		}

		//--- End Custom Code ---
	}

	
}