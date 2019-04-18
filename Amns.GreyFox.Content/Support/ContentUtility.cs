//using System;
//
//namespace Amns.GreyFox.Content.Support
//{
//	/// <summary>
//	/// Summary description for ContentUtility.
//	/// </summary>
//	public class ContentUtility
//	{
//		public static void Copy(object src, object dest, 
//			bool recurse, bool contents, bool updateLinks)
//		{
//			DbContentCatalog srcCatalog;
//			DbContentCatalog destCatalog;
//			DbContentClip srcClip;
//			DbContentClip destClip;
//
//			if(src is DbContentCatalog)
//			{
//				srcCatalog = (DbContentCatalog) src;
//
//				if(dest is DbContentCatalog)
//				{
//					destCatalog = (DbContentCatalog) dest;
//
//                    Copy(srcCatalog, destCatalog);
//				}
//				else if(dest is DbContentClip)
//				{
//					destClip = (DbContentClip) dest;
//
//					Copy(srcCatalog, destClip.ParentCatalog);
//				}
//			}
//			else if(src is DbContentClip)
//			{
//				srcClip = (DbContentClip) src;
//
//				if(dest is DbContentCatalog)
//				{
//					destCatalog = (DbContentCatalog) dest;
//
//					Copy(srcClip, destCatalog);
//				}
//				else if(dest is DbContentClip)
//				{
//					destClip = (DbContentClip) dest;
//
//					Copy(srcClip, destClip.ParentCatalog);
//				}
//			}
//		}
//
//		public DbContentCatalog Copy(DbContentCatalog src, DbContentCatalog dest)
//		{
//			DbContentCatalog newCatalog;
//			
//			newCatalog = new DbContentCatalog();
//
//			newCatalog.AuthorRole = srcCatalog.AuthorRole;
//			newCatalog.CommentsEnabled = srcCatalog.CommentsEnabled;
//			newCatalog.CreateDate = DateTime.Now;
//			newCatalog.DefaultArchive = srcCatalog.DefaultArchive;
//			newCatalog.DefaultClip = srcCatalog.DefaultClip;
//			newCatalog.DefaultKeywords = srcCatalog.DefaultKeywords;
//			newCatalog.DefaultMenuLeftIcon = srcCatalog.DefaultMenuLeftIcon;
//			newCatalog.DefaultMenuRightIcon = srcCatalog.DefaultMenuRightIcon;
//			newCatalog.DefaultRating = srcCatalog.DefaultRating;
//			newCatalog.DefaultStatus = srcCatalog.DefaultStatus;
//			newCatalog.DefaultTimeToArchive = srcCatalog.DefaultTimeToArchive;
//			newCatalog.DefaultTimeToExpire = srcCatalog.DefaultTimeToExpire;
//			newCatalog.DefaultTimeToPublish = srcCatalog.DefaultTimeToPublish;
//			newCatalog.Description = srcCatalog.Description;
//			newCatalog.EditorRole = srcCatalog.EditorRole;
//			newCatalog.Enabled = srcCatalog.Enabled;
//			newCatalog.Icon = srcCatalog.Icon;
//			newCatalog.Keywords = srcCatalog.Keywords;
//			newCatalog.MenuBreakCssClass = srcCatalog.MenuBreakCssClass;
//			newCatalog.MenuBreakImage = srcCatalog.MenuBreakImage;
//			newCatalog.MenuCatalogChildSelectedCssClass = srcCatalog.MenuCatalogChildSelectedCssClass;
//			newCatalog.MenuCatalogCssClass = srcCatalog.MenuCatalogCssClass;
//			newCatalog.MenuClipChildExpandedCssClass = srcCatalog.MenuClipChildExpandedCssClass;
//			newCatalog.MenuClipChildSelectedCssClass = srcCatalog.MenuClipChildSelectedCssClass;
//			newCatalog.MenuClipCssClass = srcCatalog.MenuClipCssClass;
//			newCatalog.MenuClipSelectedCssClass = srcCatalog.MenuClipSelectedCssClass;
//			newCatalog.MenuCssClass = srcCatalog.MenuCssClass;
//			newCatalog.MenuEnabled = srcCatalog.MenuEnabled;
//			newCatalog.MenuIconFlags = srcCatalog.MenuIconFlags;
//			newCatalog.MenuLabel = srcCatalog.MenuLabel;
//			newCatalog.MenuLeftIcon = srcCatalog.MenuLeftIcon;
//			newCatalog.MenuOrder = srcCatalog.MenuOrder;
//			newCatalog.MenuOverrideFlags = srcCatalog.MenuOverrideFlags;
//			newCatalog.MenuRightIcon = srcCatalog.MenuRightIcon;
//			newCatalog.MenuTooltip = srcCatalog.MenuTooltip;
//			newCatalog.modifyDate = DateTime.Now;
//			newCatalog.NotifyOnComments = srcCatalog.NotifyOnComments;
//			newCatalog.ParentCatalog = destCatalog;
//			newCatalog.ReviewerRole = srcCatalog.ReviewerRole;
//			newCatalog.SortOrder = srcCatalog.SortOrder;
//			newCatalog.Status = srcCatalog.Status;
//			newCatalog.Templates = srcCatalog.Templates;
//			newCatalog.Title = srcCatalog.Title;
//			newCatalog.ViewerRoles = srcCatalog.ViewerRoles;
//			newCatalog.WorkflowMode = srcCatalog.WorkflowMode;
//		
//			newCatalog.Save();
//
//			return newCatalog;
//		}
//
//		public DbContentClip Copy(DbContentClip src, DbContentCatalog dest)
//		{
//			DbContentClip newClip = new DbContentClip();
//				
//			newClip.ArchiveDate = src.ArchiveDate;
//			newClip.Authors = src.Authors;
//			newClip.Body = src.Body;
//			newClip.CommentsEnabled = src.CommentsEnabled;
//			newClip.CreateDate = DateTime.Now;
//			newClip.Description = src.Description;
//			newClip.Editors = src.Editors;
//			newClip.ExpirationDate = src.ExpirationDate;
//			newClip.Icon = src.Icon;
//			newClip.Keywords = src.Keywords;
//			newClip.MenuBreak = src.MenuBreak;
//			newClip.MenuEnabled = src.MenuEnabled;
//			newClip.MenuLabel = src.MenuLabel;
//			newClip.MenuLeftIcon = src.MenuLeftIcon;
//			newClip.MenuLeftIconOver = src.MenuLeftIconOver;
//			newClip.MenuRightIcon = src.MenuRightIcon;
//			newClip.MenuRightIconOver = src.MenuRightIconOver;
//			newClip.MenuTooltip = src.MenuTooltip;
//			newClip.ModifyDate = DateTime.Now;
//			newClip.NotifyOnComments = src.NotifyOnComments;
//			newClip.OverrideUrl = src.OverrideUrl;
//			newClip.ParentCatalog = dest;
//			newClip.Priority = dest.Priority;
//			newClip.PublishDate = dest.PublishDate;
//			newClip.Rating = dest.Rating;
//			newClip.References = dest.References;
//			newClip.SortOrder = dest.SortOrder;
//			newClip.Status = dest.Status;
//			newClip.Title = dest.Title;
//			newClip.WorkingDraft = dest.WorkingDraft;
//
//			newClip.Save();							
//
//			return newClip;
//		}		
//	}
//}
