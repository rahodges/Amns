/* ********************************************************** *
 * AMNS DbModel v1.0 Class Object Business Tier               *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class DbContentCatalog : ICloneable, IComparable, IContentObject
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal string title;
		internal string description;
		internal string keywords;
		internal byte status;
		internal byte workflowMode;
		internal bool commentsEnabled;
		internal bool notifyOnComments;
		internal bool enabled;
		internal int sortOrder;
		internal DbContentCatalog parentCatalog;
		internal DbContentCatalogCollection childCatalogs;
		internal DbContentClip defaultClip;
		internal string icon;
		internal DateTime createDate;
		internal DateTime modifyDate;
		internal TimeSpan defaultTimeToPublish;
		internal TimeSpan defaultTimeToExpire;
		internal TimeSpan defaultTimeToArchive;
		internal string defaultKeywords;
		internal DbContentStatus defaultStatus;
		internal DbContentRating defaultRating;
		internal DbContentCatalog defaultArchive;
		internal DbContentClipCollection templates;
		internal string defaultMenuLeftIcon;
		internal string defaultMenuRightIcon;
		internal string menuLabel;
		internal string menuTooltip;
		internal bool menuEnabled;
		internal int menuOrder;
		internal string menuLeftIcon;
		internal string menuRightIcon;
		internal string menuBreakImage;
		internal string menuBreakCssClass;
		internal string menuCssClass;
		internal string menuCatalogCssClass;
		internal string menuCatalogSelectedCssClass;
		internal string menuCatalogChildSelectedCssClass;
		internal string menuClipCssClass;
		internal string menuClipSelectedCssClass;
		internal string menuClipChildSelectedCssClass;
		internal string menuClipChildExpandedCssClass;
		internal byte menuOverrideFlags;
		internal byte menuIconFlags;
		internal GreyFoxRole authorRole;
		internal GreyFoxRole editorRole;
		internal GreyFoxRole reviewerRole;

		#endregion

		#region Public Properties

		/// <summary>
		/// DbContentCatalog Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the DbContentCatalog as a Placeholder. Placeholders only contain 
		/// a DbContentCatalog ID. Record late-binds data when it is accessed.
		/// </summary>
		public bool IsPlaceHolder
		{
			get
			{
				return isPlaceHolder;
			}
		}

		/// <summary>
		/// True if the object is synced with the database.
		/// </summary>
		public bool IsSynced
		{
			get
			{
				return isSynced;
			}
			set
			{
				if(value == true)
				{
					throw (new Exception("Cannot set IsSynced to true."));
				}
				isSynced = value;
			}
		}

		/// <summary>
		/// Catalog title.
		/// </summary>
		public string Title
		{
			get
			{
				EnsurePreLoad();
				return title;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= title == value;
				title = value;
			}
		}

		/// <summary>
		/// Catalog description.
		/// </summary>
		public string Description
		{
			get
			{
				EnsurePreLoad();
				return description;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= description == value;
				description = value;
			}
		}

		/// <summary>
		/// </summary>
		public string Keywords
		{
			get
			{
				EnsurePreLoad();
				return keywords;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= keywords == value;
				keywords = value;
			}
		}

		/// <summary>
		/// Status {Online=0, Offline=50}
		/// </summary>
		public byte Status
		{
			get
			{
				EnsurePreLoad();
				return status;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= status == value;
				status = value;
			}
		}

		/// <summary>
		/// A bit flag array sets this.
		/// </summary>
		public byte WorkflowMode
		{
			get
			{
				EnsurePreLoad();
				return workflowMode;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= workflowMode == value;
				workflowMode = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool CommentsEnabled
		{
			get
			{
				EnsurePreLoad();
				return commentsEnabled;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= commentsEnabled == value;
				commentsEnabled = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool NotifyOnComments
		{
			get
			{
				EnsurePreLoad();
				return notifyOnComments;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= notifyOnComments == value;
				notifyOnComments = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool Enabled
		{
			get
			{
				EnsurePreLoad();
				return enabled;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= enabled == value;
				enabled = value;
			}
		}

		/// <summary>
		/// </summary>
		public int SortOrder
		{
			get
			{
				EnsurePreLoad();
				return sortOrder;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= sortOrder == value;
				sortOrder = value;
			}
		}

		/// <summary>
		/// </summary>
		public DbContentCatalog ParentCatalog
		{
			get
			{
				EnsurePreLoad();
				return parentCatalog;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(parentCatalog == null)
					{
						return;
					}
					else
					{
						parentCatalog = value;
						isSynced = false;
					}
				}
				else
				{
					if(parentCatalog != null && value.ID == parentCatalog.ID)
					{
						return; 
					}
					else
					{
						parentCatalog = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public DbContentCatalogCollection ChildCatalogs
		{
			get
			{
				EnsurePreLoad();
				if(childCatalogs == null)
				{
					DbContentCatalogManager.FillChildCatalogs(this);
					childCatalogs.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
				}
				return childCatalogs;
			}
			set
			{
				EnsurePreLoad();
				if(!object.Equals(childCatalogs, value))
				{
					childCatalogs = value;
					childCatalogs.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
					isSynced = false;
				}
			}
		}

		/// <summary>
		/// </summary>
		public DbContentClip DefaultClip
		{
			get
			{
				EnsurePreLoad();
				return defaultClip;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(defaultClip == null)
					{
						return;
					}
					else
					{
						defaultClip = value;
						isSynced = false;
					}
				}
				else
				{
					if(defaultClip != null && value.ID == defaultClip.ID)
					{
						return; 
					}
					else
					{
						defaultClip = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Icon
		{
			get
			{
				EnsurePreLoad();
				return icon;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= icon == value;
				icon = value;
			}
		}

		/// <summary>
		/// </summary>
		public DateTime CreateDate
		{
			get
			{
				EnsurePreLoad();
				return createDate;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= createDate == value;
				createDate = value;
			}
		}

		/// <summary>
		/// </summary>
		public DateTime ModifyDate
		{
			get
			{
				EnsurePreLoad();
				return modifyDate;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= modifyDate == value;
				modifyDate = value;
			}
		}

		/// <summary>
		/// </summary>
		public TimeSpan DefaultTimeToPublish
		{
			get
			{
				EnsurePreLoad();
				return defaultTimeToPublish;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= defaultTimeToPublish == value;
				defaultTimeToPublish = value;
			}
		}

		/// <summary>
		/// </summary>
		public TimeSpan DefaultTimeToExpire
		{
			get
			{
				EnsurePreLoad();
				return defaultTimeToExpire;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= defaultTimeToExpire == value;
				defaultTimeToExpire = value;
			}
		}

		/// <summary>
		/// </summary>
		public TimeSpan DefaultTimeToArchive
		{
			get
			{
				EnsurePreLoad();
				return defaultTimeToArchive;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= defaultTimeToArchive == value;
				defaultTimeToArchive = value;
			}
		}

		/// <summary>
		/// </summary>
		public string DefaultKeywords
		{
			get
			{
				EnsurePreLoad();
				return defaultKeywords;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= defaultKeywords == value;
				defaultKeywords = value;
			}
		}

		/// <summary>
		/// </summary>
		public DbContentStatus DefaultStatus
		{
			get
			{
				EnsurePreLoad();
				return defaultStatus;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(defaultStatus == null)
					{
						return;
					}
					else
					{
						defaultStatus = value;
						isSynced = false;
					}
				}
				else
				{
					if(defaultStatus != null && value.ID == defaultStatus.ID)
					{
						return; 
					}
					else
					{
						defaultStatus = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public DbContentRating DefaultRating
		{
			get
			{
				EnsurePreLoad();
				return defaultRating;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(defaultRating == null)
					{
						return;
					}
					else
					{
						defaultRating = value;
						isSynced = false;
					}
				}
				else
				{
					if(defaultRating != null && value.ID == defaultRating.ID)
					{
						return; 
					}
					else
					{
						defaultRating = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public DbContentCatalog DefaultArchive
		{
			get
			{
				EnsurePreLoad();
				return defaultArchive;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(defaultArchive == null)
					{
						return;
					}
					else
					{
						defaultArchive = value;
						isSynced = false;
					}
				}
				else
				{
					if(defaultArchive != null && value.ID == defaultArchive.ID)
					{
						return; 
					}
					else
					{
						defaultArchive = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public DbContentClipCollection Templates
		{
			get
			{
				EnsurePreLoad();
				if(templates == null)
				{
					DbContentCatalogManager.FillTemplates(this);
					templates.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
				}
				return templates;
			}
			set
			{
				EnsurePreLoad();
				if(!object.Equals(templates, value))
				{
					templates = value;
					templates.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
					isSynced = false;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string DefaultMenuLeftIcon
		{
			get
			{
				EnsurePreLoad();
				return defaultMenuLeftIcon;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= defaultMenuLeftIcon == value;
				defaultMenuLeftIcon = value;
			}
		}

		/// <summary>
		/// </summary>
		public string DefaultMenuRightIcon
		{
			get
			{
				EnsurePreLoad();
				return defaultMenuRightIcon;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= defaultMenuRightIcon == value;
				defaultMenuRightIcon = value;
			}
		}

		/// <summary>
		/// </summary>
		public string MenuLabel
		{
			get
			{
				EnsurePreLoad();
				return menuLabel;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuLabel == value;
				menuLabel = value;
			}
		}

		/// <summary>
		/// </summary>
		public string MenuTooltip
		{
			get
			{
				EnsurePreLoad();
				return menuTooltip;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuTooltip == value;
				menuTooltip = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool MenuEnabled
		{
			get
			{
				EnsurePreLoad();
				return menuEnabled;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuEnabled == value;
				menuEnabled = value;
			}
		}

		/// <summary>
		/// </summary>
		public int MenuOrder
		{
			get
			{
				EnsurePreLoad();
				return menuOrder;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuOrder == value;
				menuOrder = value;
			}
		}

		/// <summary>
		/// </summary>
		public string MenuLeftIcon
		{
			get
			{
				EnsurePreLoad();
				return menuLeftIcon;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuLeftIcon == value;
				menuLeftIcon = value;
			}
		}

		/// <summary>
		/// </summary>
		public string MenuRightIcon
		{
			get
			{
				EnsurePreLoad();
				return menuRightIcon;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuRightIcon == value;
				menuRightIcon = value;
			}
		}

		/// <summary>
		/// </summary>
		public string MenuBreakImage
		{
			get
			{
				EnsurePreLoad();
				return menuBreakImage;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuBreakImage == value;
				menuBreakImage = value;
			}
		}

		/// <summary>
		/// </summary>
		public string MenuBreakCssClass
		{
			get
			{
				EnsurePreLoad();
				return menuBreakCssClass;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuBreakCssClass == value;
				menuBreakCssClass = value;
			}
		}

		/// <summary>
		/// </summary>
		public string MenuCssClass
		{
			get
			{
				EnsurePreLoad();
				return menuCssClass;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuCssClass == value;
				menuCssClass = value;
			}
		}

		/// <summary>
		/// Default CSS class to be used for all menu groups. 
		/// </summary>
		public string MenuCatalogCssClass
		{
			get
			{
				EnsurePreLoad();
				return menuCatalogCssClass;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuCatalogCssClass == value;
				menuCatalogCssClass = value;
			}
		}

		/// <summary>
		/// Default CSS class to be used for the menu group which contains a selected 
		/// item. 
		/// </summary>
		public string MenuCatalogSelectedCssClass
		{
			get
			{
				EnsurePreLoad();
				return menuCatalogSelectedCssClass;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuCatalogSelectedCssClass == value;
				menuCatalogSelectedCssClass = value;
			}
		}

		/// <summary>
		/// Default CSS class to be used for menu groups whose descendent item is 
		/// selected. 
		/// </summary>
		public string MenuCatalogChildSelectedCssClass
		{
			get
			{
				EnsurePreLoad();
				return menuCatalogChildSelectedCssClass;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuCatalogChildSelectedCssClass == value;
				menuCatalogChildSelectedCssClass = value;
			}
		}

		/// <summary>
		/// </summary>
		public string MenuClipCssClass
		{
			get
			{
				EnsurePreLoad();
				return menuClipCssClass;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuClipCssClass == value;
				menuClipCssClass = value;
			}
		}

		/// <summary>
		/// </summary>
		public string MenuClipSelectedCssClass
		{
			get
			{
				EnsurePreLoad();
				return menuClipSelectedCssClass;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuClipSelectedCssClass == value;
				menuClipSelectedCssClass = value;
			}
		}

		/// <summary>
		/// Default CSS class to be used for menu items whose descendent item is 
		/// selected. 
		/// </summary>
		public string MenuClipChildSelectedCssClass
		{
			get
			{
				EnsurePreLoad();
				return menuClipChildSelectedCssClass;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuClipChildSelectedCssClass == value;
				menuClipChildSelectedCssClass = value;
			}
		}

		/// <summary>
		/// Default CSS class to be used for menu items when the mouse is over their 
		/// descendent subgroup. 
		/// </summary>
		public string MenuClipChildExpandedCssClass
		{
			get
			{
				EnsurePreLoad();
				return menuClipChildExpandedCssClass;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuClipChildExpandedCssClass == value;
				menuClipChildExpandedCssClass = value;
			}
		}

		/// <summary>
		/// 1=CatalogStyleOverride, 2=ClipStyleOverride, 4=LeftIconOverride, 8=RightIconOverride, 
		/// 16=ChildCatalogInheritance
		/// </summary>
		public byte MenuOverrideFlags
		{
			get
			{
				EnsurePreLoad();
				return menuOverrideFlags;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuOverrideFlags == value;
				menuOverrideFlags = value;
			}
		}

		/// <summary>
		/// 1=LeftIconOn, 2=RightIconOn, 4=RightIconChildren, 5=LeftIconChildren
		/// </summary>
		public byte MenuIconFlags
		{
			get
			{
				EnsurePreLoad();
				return menuIconFlags;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuIconFlags == value;
				menuIconFlags = value;
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxRole AuthorRole
		{
			get
			{
				EnsurePreLoad();
				return authorRole;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(authorRole == null)
					{
						return;
					}
					else
					{
						authorRole = value;
						isSynced = false;
					}
				}
				else
				{
					if(authorRole != null && value.ID == authorRole.ID)
					{
						return; 
					}
					else
					{
						authorRole = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxRole EditorRole
		{
			get
			{
				EnsurePreLoad();
				return editorRole;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(editorRole == null)
					{
						return;
					}
					else
					{
						editorRole = value;
						isSynced = false;
					}
				}
				else
				{
					if(editorRole != null && value.ID == editorRole.ID)
					{
						return; 
					}
					else
					{
						editorRole = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxRole ReviewerRole
		{
			get
			{
				EnsurePreLoad();
				return reviewerRole;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(reviewerRole == null)
					{
						return;
					}
					else
					{
						reviewerRole = value;
						isSynced = false;
					}
				}
				else
				{
					if(reviewerRole != null && value.ID == reviewerRole.ID)
					{
						return; 
					}
					else
					{
						reviewerRole = value;
						isSynced = false;
					}
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of DbContentCatalog.
		/// </summary>
		public DbContentCatalog()
		{
			title = string.Empty;
			description = string.Empty;
			keywords = string.Empty;
			status = 0;
			workflowMode = 0;
			commentsEnabled = true;
			notifyOnComments = true;
			enabled = true;
			sortOrder = 0;
			icon = string.Empty;
			createDate = DateTime.Now;
			modifyDate = DateTime.Now;
			defaultTimeToPublish = TimeSpan.Zero;
			defaultTimeToExpire = TimeSpan.MaxValue;
			defaultTimeToArchive = TimeSpan.MaxValue;
			defaultKeywords = string.Empty;
			defaultMenuLeftIcon = string.Empty;
			defaultMenuRightIcon = string.Empty;
			menuLabel = string.Empty;
			menuTooltip = string.Empty;
			menuEnabled = true;
			menuOrder = 0;
			menuLeftIcon = string.Empty;
			menuRightIcon = string.Empty;
			menuBreakImage = string.Empty;
			menuBreakCssClass = string.Empty;
			menuCssClass = string.Empty;
			menuCatalogCssClass = string.Empty;
			menuCatalogSelectedCssClass = string.Empty;
			menuCatalogChildSelectedCssClass = string.Empty;
			menuClipCssClass = string.Empty;
			menuClipSelectedCssClass = string.Empty;
			menuClipChildSelectedCssClass = string.Empty;
			menuClipChildExpandedCssClass = string.Empty;
			menuOverrideFlags = 0;
			menuIconFlags = 0;
		}

		public DbContentCatalog(int id)
		{
			this.iD = id;
			isSynced = DbContentCatalogManager._fill(this);
		}
		#endregion

		#region Default DbModel Methods

		/// <summary>
		/// Ensures that the object's fields and children are 
		/// pre-loaded before any updates or reads.
		/// </summary>
		public void EnsurePreLoad()
		{
			if(!isPlaceHolder)
				return;

			DbContentCatalogManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the DbContentCatalog object state to the database.
		/// </summary>
		public int Save()
		{

			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = DbContentCatalogManager._insert(this);
			else
				DbContentCatalogManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			DbContentCatalogManager._delete(this.iD);
		}
		/// <summary>
		/// Duplicates DbContentCatalog object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentCatalog object reflecting the replicated DbContentCatalog object.</returns>
		public DbContentCatalog Duplicate()
		{
			DbContentCatalog clonedDbContentCatalog = this.Clone();

			// Insert must be called after children are replicated!
			clonedDbContentCatalog.iD = DbContentCatalogManager._insert(clonedDbContentCatalog);
			clonedDbContentCatalog.isSynced = true;
			return clonedDbContentCatalog;
		}

		/// <summary>
		/// Overwrites and existing DbContentCatalog object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			DbContentCatalogManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones DbContentCatalog object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentCatalog object reflecting the replicated DbContentCatalog object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones DbContentCatalog object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentCatalog object reflecting the replicated DbContentCatalog object.</returns>
		public DbContentCatalog Clone()
		{
			DbContentCatalog clonedDbContentCatalog = new DbContentCatalog();
			clonedDbContentCatalog.iD = iD;
			clonedDbContentCatalog.isSynced = isSynced;
			clonedDbContentCatalog.title = title;
			clonedDbContentCatalog.description = description;
			clonedDbContentCatalog.keywords = keywords;
			clonedDbContentCatalog.status = status;
			clonedDbContentCatalog.workflowMode = workflowMode;
			clonedDbContentCatalog.commentsEnabled = commentsEnabled;
			clonedDbContentCatalog.notifyOnComments = notifyOnComments;
			clonedDbContentCatalog.enabled = enabled;
			clonedDbContentCatalog.sortOrder = sortOrder;
			clonedDbContentCatalog.icon = icon;
			clonedDbContentCatalog.createDate = createDate;
			clonedDbContentCatalog.modifyDate = modifyDate;
			clonedDbContentCatalog.defaultTimeToPublish = defaultTimeToPublish;
			clonedDbContentCatalog.defaultTimeToExpire = defaultTimeToExpire;
			clonedDbContentCatalog.defaultTimeToArchive = defaultTimeToArchive;
			clonedDbContentCatalog.defaultKeywords = defaultKeywords;
			clonedDbContentCatalog.defaultMenuLeftIcon = defaultMenuLeftIcon;
			clonedDbContentCatalog.defaultMenuRightIcon = defaultMenuRightIcon;
			clonedDbContentCatalog.menuLabel = menuLabel;
			clonedDbContentCatalog.menuTooltip = menuTooltip;
			clonedDbContentCatalog.menuEnabled = menuEnabled;
			clonedDbContentCatalog.menuOrder = menuOrder;
			clonedDbContentCatalog.menuLeftIcon = menuLeftIcon;
			clonedDbContentCatalog.menuRightIcon = menuRightIcon;
			clonedDbContentCatalog.menuBreakImage = menuBreakImage;
			clonedDbContentCatalog.menuBreakCssClass = menuBreakCssClass;
			clonedDbContentCatalog.menuCssClass = menuCssClass;
			clonedDbContentCatalog.menuCatalogCssClass = menuCatalogCssClass;
			clonedDbContentCatalog.menuCatalogSelectedCssClass = menuCatalogSelectedCssClass;
			clonedDbContentCatalog.menuCatalogChildSelectedCssClass = menuCatalogChildSelectedCssClass;
			clonedDbContentCatalog.menuClipCssClass = menuClipCssClass;
			clonedDbContentCatalog.menuClipSelectedCssClass = menuClipSelectedCssClass;
			clonedDbContentCatalog.menuClipChildSelectedCssClass = menuClipChildSelectedCssClass;
			clonedDbContentCatalog.menuClipChildExpandedCssClass = menuClipChildExpandedCssClass;
			clonedDbContentCatalog.menuOverrideFlags = menuOverrideFlags;
			clonedDbContentCatalog.menuIconFlags = menuIconFlags;


			if(parentCatalog != null)
				clonedDbContentCatalog.parentCatalog = parentCatalog;

			if(childCatalogs != null)
				clonedDbContentCatalog.childCatalogs = childCatalogs.Clone();

			if(defaultClip != null)
				clonedDbContentCatalog.defaultClip = defaultClip;

			if(defaultStatus != null)
				clonedDbContentCatalog.defaultStatus = defaultStatus;

			if(defaultRating != null)
				clonedDbContentCatalog.defaultRating = defaultRating;

			if(defaultArchive != null)
				clonedDbContentCatalog.defaultArchive = defaultArchive;

			if(templates != null)
				clonedDbContentCatalog.templates = templates.Clone();

			if(authorRole != null)
				clonedDbContentCatalog.authorRole = authorRole;

			if(editorRole != null)
				clonedDbContentCatalog.editorRole = editorRole;

			if(reviewerRole != null)
				clonedDbContentCatalog.reviewerRole = reviewerRole;

			return clonedDbContentCatalog;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentCatalog.
		/// </summary>
		/// <returns> A new DbContentCatalog object reflecting the cloned DbContentCatalog object.</returns>
		public DbContentCatalog Copy()
		{
			DbContentCatalog dbContentCatalog = new DbContentCatalog();
			CopyTo(dbContentCatalog);
			return dbContentCatalog;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentCatalog.
		/// </summary>
		/// <returns> A new DbContentCatalog object reflecting the cloned DbContentCatalog object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the DbContentCatalog from its children.</param>
		public DbContentCatalog Copy(bool isolation)
		{
			DbContentCatalog dbContentCatalog = new DbContentCatalog();
			CopyTo(dbContentCatalog, isolation);
			return dbContentCatalog;
		}

		/// <summary>
		/// Deep copies the current DbContentCatalog to another instance of DbContentCatalog.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="DbContentCatalog">The DbContentCatalog to copy to.</param>
		public void CopyTo(DbContentCatalog dbContentCatalog)
		{
			CopyTo(dbContentCatalog, false);
		}

		/// <summary>
		/// Deep copies the current DbContentCatalog to another instance of DbContentCatalog.
		/// </summary>
		/// <param name="DbContentCatalog">The DbContentCatalog to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the DbContentCatalog from its children.</param>
		public void CopyTo(DbContentCatalog dbContentCatalog, bool isolation)
		{
			dbContentCatalog.iD = iD;
			dbContentCatalog.isPlaceHolder = isPlaceHolder;
			dbContentCatalog.isSynced = isSynced;
			dbContentCatalog.title = title;
			dbContentCatalog.description = description;
			dbContentCatalog.keywords = keywords;
			dbContentCatalog.status = status;
			dbContentCatalog.workflowMode = workflowMode;
			dbContentCatalog.commentsEnabled = commentsEnabled;
			dbContentCatalog.notifyOnComments = notifyOnComments;
			dbContentCatalog.enabled = enabled;
			dbContentCatalog.sortOrder = sortOrder;
			dbContentCatalog.icon = icon;
			dbContentCatalog.createDate = createDate;
			dbContentCatalog.modifyDate = modifyDate;
			dbContentCatalog.defaultTimeToPublish = defaultTimeToPublish;
			dbContentCatalog.defaultTimeToExpire = defaultTimeToExpire;
			dbContentCatalog.defaultTimeToArchive = defaultTimeToArchive;
			dbContentCatalog.defaultKeywords = defaultKeywords;
			dbContentCatalog.defaultMenuLeftIcon = defaultMenuLeftIcon;
			dbContentCatalog.defaultMenuRightIcon = defaultMenuRightIcon;
			dbContentCatalog.menuLabel = menuLabel;
			dbContentCatalog.menuTooltip = menuTooltip;
			dbContentCatalog.menuEnabled = menuEnabled;
			dbContentCatalog.menuOrder = menuOrder;
			dbContentCatalog.menuLeftIcon = menuLeftIcon;
			dbContentCatalog.menuRightIcon = menuRightIcon;
			dbContentCatalog.menuBreakImage = menuBreakImage;
			dbContentCatalog.menuBreakCssClass = menuBreakCssClass;
			dbContentCatalog.menuCssClass = menuCssClass;
			dbContentCatalog.menuCatalogCssClass = menuCatalogCssClass;
			dbContentCatalog.menuCatalogSelectedCssClass = menuCatalogSelectedCssClass;
			dbContentCatalog.menuCatalogChildSelectedCssClass = menuCatalogChildSelectedCssClass;
			dbContentCatalog.menuClipCssClass = menuClipCssClass;
			dbContentCatalog.menuClipSelectedCssClass = menuClipSelectedCssClass;
			dbContentCatalog.menuClipChildSelectedCssClass = menuClipChildSelectedCssClass;
			dbContentCatalog.menuClipChildExpandedCssClass = menuClipChildExpandedCssClass;
			dbContentCatalog.menuOverrideFlags = menuOverrideFlags;
			dbContentCatalog.menuIconFlags = menuIconFlags;

			if(parentCatalog != null)
			{
				if(isolation)
				{
					dbContentCatalog.parentCatalog = parentCatalog.NewPlaceHolder();
				}
				else
				{
					dbContentCatalog.parentCatalog = parentCatalog.Copy(false);
				}
			}
			if(childCatalogs != null)
			{
				if(isolation)
				{
					dbContentCatalog.childCatalogs = childCatalogs.Copy(true);
				}
				else
				{
					dbContentCatalog.childCatalogs = childCatalogs.Copy(false);
				}
			}
			if(defaultClip != null)
			{
				if(isolation)
				{
					dbContentCatalog.defaultClip = defaultClip.NewPlaceHolder();
				}
				else
				{
					dbContentCatalog.defaultClip = defaultClip.Copy(false);
				}
			}
			if(defaultStatus != null)
			{
				if(isolation)
				{
					dbContentCatalog.defaultStatus = defaultStatus.NewPlaceHolder();
				}
				else
				{
					dbContentCatalog.defaultStatus = defaultStatus.Copy(false);
				}
			}
			if(defaultRating != null)
			{
				if(isolation)
				{
					dbContentCatalog.defaultRating = defaultRating.NewPlaceHolder();
				}
				else
				{
					dbContentCatalog.defaultRating = defaultRating.Copy(false);
				}
			}
			if(defaultArchive != null)
			{
				if(isolation)
				{
					dbContentCatalog.defaultArchive = defaultArchive.NewPlaceHolder();
				}
				else
				{
					dbContentCatalog.defaultArchive = defaultArchive.Copy(false);
				}
			}
			if(templates != null)
			{
				if(isolation)
				{
					dbContentCatalog.templates = templates.Copy(true);
				}
				else
				{
					dbContentCatalog.templates = templates.Copy(false);
				}
			}
			if(authorRole != null)
			{
				if(isolation)
				{
					dbContentCatalog.authorRole = authorRole.NewPlaceHolder();
				}
				else
				{
					dbContentCatalog.authorRole = authorRole.Copy(false);
				}
			}
			if(editorRole != null)
			{
				if(isolation)
				{
					dbContentCatalog.editorRole = editorRole.NewPlaceHolder();
				}
				else
				{
					dbContentCatalog.editorRole = editorRole.Copy(false);
				}
			}
			if(reviewerRole != null)
			{
				if(isolation)
				{
					dbContentCatalog.reviewerRole = reviewerRole.NewPlaceHolder();
				}
				else
				{
					dbContentCatalog.reviewerRole = reviewerRole.Copy(false);
				}
			}
		}

		public DbContentCatalog NewPlaceHolder()
		{
			DbContentCatalog dbContentCatalog = new DbContentCatalog();
			dbContentCatalog.iD = iD;
			dbContentCatalog.isPlaceHolder = true;
			dbContentCatalog.isSynced = true;
			return dbContentCatalog;
		}

		public static DbContentCatalog NewPlaceHolder(int iD)
		{
			DbContentCatalog dbContentCatalog = new DbContentCatalog();
			dbContentCatalog.iD = iD;
			dbContentCatalog.isPlaceHolder = true;
			dbContentCatalog.isSynced = true;
			return dbContentCatalog;
		}

		private void childrenCollection_Changed(object sender, System.EventArgs e)
		{
			isSynced = false;
		}

		#endregion

		#region IComparable Methods

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		int IComparable.CompareTo(object obj)
		{
			DbContentCatalog dbContentCatalog = (DbContentCatalog) obj;
			return this.iD - dbContentCatalog.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(DbContentCatalog dbContentCatalog)
		{
			return this.iD - dbContentCatalog.iD;
		}

		public override bool Equals(object dbContentCatalog)
		{
			if(dbContentCatalog == null)
				return false;

			return Equals((DbContentCatalog) dbContentCatalog);
		}

		public bool Equals(DbContentCatalog dbContentCatalog)
		{
			if(dbContentCatalog == null)
				return false;

			return this.iD == dbContentCatalog.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

	}
}
