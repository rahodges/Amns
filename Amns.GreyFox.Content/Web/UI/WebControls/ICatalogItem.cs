using System;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Used to mark a class as sortable with other classes in a content catalog.
	/// </summary>
	public interface ICatalogItem
	{
		int Title { get; }
		int MenuOrder { get; }
		int PublishDate { get; }
	}
}