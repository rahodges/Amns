using System;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary description for CatalogTransversalMode.
	/// </summary>
	public enum CatalogTransversalMode
	{
		Free,			// Clips in all catalogs are displayed
		Root,			// Clips in root catalog are displayed
		Trunk,			// Clips in one catalog deep are displayed
		Tree			// Clips in root and all sub-catalogs are displayed
	}
}
