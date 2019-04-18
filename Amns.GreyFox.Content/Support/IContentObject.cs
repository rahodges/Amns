using System;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary description for IContentObject.
	/// </summary>
	public interface IContentObject
	{
		int ID { get; }
		string Title { get; }
		GreyFoxUserCollection Authors { get; }
		DateTime PublishDate { get; }
		DateTime ExpirationDate { get; }
		int SortOrder { get; }
		int MenuOrder { get; }
		bool MenuEnabled { get; }
	}
}
