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
	/// Database clip for Amns.GreyFox CMS.
	/// </summary>
	public class DbContentClip : ICloneable, IComparable, IContentObject
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal DateTime createDate;
		internal DateTime modifyDate;
		internal string title;
		internal string description;
		internal string keywords;
		internal string icon;
		internal DbContentStatus status;
		internal string body;
		internal DbContentCatalog parentCatalog;
		internal DateTime publishDate;
		internal DateTime expirationDate;
		internal DateTime archiveDate;
		internal int priority;
		internal int sortOrder;
		internal DbContentRating rating;
		internal DbContentClipCollection references;
		internal bool commentsEnabled;
		internal bool notifyOnComments;
		internal DbContentClip workingDraft;
		internal string overrideUrl;
		internal GreyFoxUserCollection authors;
		internal GreyFoxUserCollection editors;
		internal string menuLabel;
		internal string menuTooltip;
		internal bool menuEnabled;
		internal int menuOrder;
		internal string menuLeftIcon;
		internal string menuLeftIconOver;
		internal string menuRightIcon;
		internal string menuRightIconOver;
		internal bool menuBreak;

		#endregion

		#region Public Properties

		/// <summary>
		/// DbContentClip Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the DbContentClip as a Placeholder. Placeholders only contain 
		/// a DbContentClip ID. Record late-binds data when it is accessed.
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
		/// Title of clip.
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
		/// Content description.
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
		/// Keywords
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
		public DbContentStatus Status
		{
			get
			{
				EnsurePreLoad();
				return status;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(status == null)
					{
						return;
					}
					else
					{
						status = value;
						isSynced = false;
					}
				}
				else
				{
					if(status != null && value.ID == status.ID)
					{
						return; 
					}
					else
					{
						status = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// Content to display for the clip
		/// </summary>
		public string Body
		{
			get
			{
				EnsurePreLoad();
				return body;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= body == value;
				body = value;
			}
		}

		/// <summary>
		/// Parent catalog of clip. Drafts do NOT have a parent catalog!
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
		/// Date when the content becomes available.
		/// </summary>
		public DateTime PublishDate
		{
			get
			{
				EnsurePreLoad();
				return publishDate;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= publishDate == value;
				publishDate = value;
			}
		}

		/// <summary>
		/// Date when the clip expires.
		/// </summary>
		public DateTime ExpirationDate
		{
			get
			{
				EnsurePreLoad();
				return expirationDate;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= expirationDate == value;
				expirationDate = value;
			}
		}

		/// <summary>
		/// Date to archive the clip to the archive table.
		/// </summary>
		public DateTime ArchiveDate
		{
			get
			{
				EnsurePreLoad();
				return archiveDate;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= archiveDate == value;
				archiveDate = value;
			}
		}

		/// <summary>
		/// </summary>
		public int Priority
		{
			get
			{
				EnsurePreLoad();
				return priority;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= priority == value;
				priority = value;
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
		public DbContentRating Rating
		{
			get
			{
				EnsurePreLoad();
				return rating;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(rating == null)
					{
						return;
					}
					else
					{
						rating = value;
						isSynced = false;
					}
				}
				else
				{
					if(rating != null && value.ID == rating.ID)
					{
						return; 
					}
					else
					{
						rating = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// Clips to cross reference this clip to.
		/// </summary>
		public DbContentClipCollection References
		{
			get
			{
				EnsurePreLoad();
				if(references == null)
				{
					DbContentClipManager.FillReferences(this);
					references.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
				}
				return references;
			}
			set
			{
				EnsurePreLoad();
				if(!object.Equals(references, value))
				{
					references = value;
					references.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
					isSynced = false;
				}
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
		public DbContentClip WorkingDraft
		{
			get
			{
				EnsurePreLoad();
				return workingDraft;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(workingDraft == null)
					{
						return;
					}
					else
					{
						workingDraft = value;
						isSynced = false;
					}
				}
				else
				{
					if(workingDraft != null && value.ID == workingDraft.ID)
					{
						return; 
					}
					else
					{
						workingDraft = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public string OverrideUrl
		{
			get
			{
				EnsurePreLoad();
				return overrideUrl;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= overrideUrl == value;
				overrideUrl = value;
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxUserCollection Authors
		{
			get
			{
				EnsurePreLoad();
				if(authors == null)
				{
					DbContentClipManager.FillAuthors(this);
					authors.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
				}
				return authors;
			}
			set
			{
				EnsurePreLoad();
				if(!object.Equals(authors, value))
				{
					authors = value;
					authors.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
					isSynced = false;
				}
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxUserCollection Editors
		{
			get
			{
				EnsurePreLoad();
				if(editors == null)
				{
					DbContentClipManager.FillEditors(this);
					editors.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
				}
				return editors;
			}
			set
			{
				EnsurePreLoad();
				if(!object.Equals(editors, value))
				{
					editors = value;
					editors.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
					isSynced = false;
				}
			}
		}

		/// <summary>
		/// The menu title of the clip; if none is specified, the title is used.
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
		public string MenuLeftIconOver
		{
			get
			{
				EnsurePreLoad();
				return menuLeftIconOver;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuLeftIconOver == value;
				menuLeftIconOver = value;
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
		public string MenuRightIconOver
		{
			get
			{
				EnsurePreLoad();
				return menuRightIconOver;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuRightIconOver == value;
				menuRightIconOver = value;
			}
		}

		/// <summary>
		/// Displays a menu break before the item.
		/// </summary>
		public bool MenuBreak
		{
			get
			{
				EnsurePreLoad();
				return menuBreak;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= menuBreak == value;
				menuBreak = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of DbContentClip.
		/// </summary>
		public DbContentClip()
		{
			createDate = DateTime.Now;
			modifyDate = DateTime.Now;
			title = string.Empty;
			description = string.Empty;
			keywords = string.Empty;
			icon = string.Empty;
			body = string.Empty;
			priority = 0;
			sortOrder = 0;
			commentsEnabled = true;
			notifyOnComments = true;
			overrideUrl = string.Empty;
			menuLabel = string.Empty;
			menuTooltip = string.Empty;
			menuEnabled = true;
			menuOrder = 0;
			menuLeftIcon = string.Empty;
			menuLeftIconOver = string.Empty;
			menuRightIcon = string.Empty;
			menuRightIconOver = string.Empty;
			menuBreak = false;
		}

		public DbContentClip(int id)
		{
			this.iD = id;
			isSynced = DbContentClipManager._fill(this);
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

			DbContentClipManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the DbContentClip object state to the database.
		/// </summary>
		public int Save()
		{

			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = DbContentClipManager._insert(this);
			else
				DbContentClipManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			DbContentClipManager._delete(this.iD);
			this.iD = 0;
			isSynced = false;
		}
		/// <summary>
		/// Duplicates DbContentClip object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentClip object reflecting the replicated DbContentClip object.</returns>
		public DbContentClip Duplicate()
		{
			DbContentClip clonedDbContentClip = this.Clone();

			// Insert must be called after children are replicated!
			clonedDbContentClip.iD = DbContentClipManager._insert(clonedDbContentClip);
			clonedDbContentClip.isSynced = true;
			return clonedDbContentClip;
		}

		/// <summary>
		/// Overwrites and existing DbContentClip object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			DbContentClipManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones DbContentClip object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentClip object reflecting the replicated DbContentClip object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones DbContentClip object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentClip object reflecting the replicated DbContentClip object.</returns>
		public DbContentClip Clone()
		{
			DbContentClip clonedDbContentClip = new DbContentClip();
			clonedDbContentClip.iD = iD;
			clonedDbContentClip.isSynced = isSynced;
			clonedDbContentClip.createDate = createDate;
			clonedDbContentClip.modifyDate = modifyDate;
			clonedDbContentClip.title = title;
			clonedDbContentClip.description = description;
			clonedDbContentClip.keywords = keywords;
			clonedDbContentClip.icon = icon;
			clonedDbContentClip.body = body;
			clonedDbContentClip.publishDate = publishDate;
			clonedDbContentClip.expirationDate = expirationDate;
			clonedDbContentClip.archiveDate = archiveDate;
			clonedDbContentClip.priority = priority;
			clonedDbContentClip.sortOrder = sortOrder;
			clonedDbContentClip.commentsEnabled = commentsEnabled;
			clonedDbContentClip.notifyOnComments = notifyOnComments;
			clonedDbContentClip.overrideUrl = overrideUrl;
			clonedDbContentClip.menuLabel = menuLabel;
			clonedDbContentClip.menuTooltip = menuTooltip;
			clonedDbContentClip.menuEnabled = menuEnabled;
			clonedDbContentClip.menuOrder = menuOrder;
			clonedDbContentClip.menuLeftIcon = menuLeftIcon;
			clonedDbContentClip.menuLeftIconOver = menuLeftIconOver;
			clonedDbContentClip.menuRightIcon = menuRightIcon;
			clonedDbContentClip.menuRightIconOver = menuRightIconOver;
			clonedDbContentClip.menuBreak = menuBreak;


			if(status != null)
				clonedDbContentClip.status = status;

			if(parentCatalog != null)
				clonedDbContentClip.parentCatalog = parentCatalog;

			if(rating != null)
				clonedDbContentClip.rating = rating;

			if(references != null)
				clonedDbContentClip.references = references.Clone();

			if(workingDraft != null)
				clonedDbContentClip.workingDraft = workingDraft;

			if(authors != null)
				clonedDbContentClip.authors = authors.Clone();

			if(editors != null)
				clonedDbContentClip.editors = editors.Clone();

			return clonedDbContentClip;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentClip.
		/// </summary>
		/// <returns> A new DbContentClip object reflecting the cloned DbContentClip object.</returns>
		public DbContentClip Copy()
		{
			DbContentClip dbContentClip = new DbContentClip();
			CopyTo(dbContentClip);
			return dbContentClip;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentClip.
		/// </summary>
		/// <returns> A new DbContentClip object reflecting the cloned DbContentClip object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the DbContentClip from its children.</param>
		public DbContentClip Copy(bool isolation)
		{
			DbContentClip dbContentClip = new DbContentClip();
			CopyTo(dbContentClip, isolation);
			return dbContentClip;
		}

		/// <summary>
		/// Deep copies the current DbContentClip to another instance of DbContentClip.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="DbContentClip">The DbContentClip to copy to.</param>
		public void CopyTo(DbContentClip dbContentClip)
		{
			CopyTo(dbContentClip, false);
		}

		/// <summary>
		/// Deep copies the current DbContentClip to another instance of DbContentClip.
		/// </summary>
		/// <param name="DbContentClip">The DbContentClip to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the DbContentClip from its children.</param>
		public void CopyTo(DbContentClip dbContentClip, bool isolation)
		{
			dbContentClip.iD = iD;
			dbContentClip.isPlaceHolder = isPlaceHolder;
			dbContentClip.isSynced = isSynced;
			dbContentClip.createDate = createDate;
			dbContentClip.modifyDate = modifyDate;
			dbContentClip.title = title;
			dbContentClip.description = description;
			dbContentClip.keywords = keywords;
			dbContentClip.icon = icon;
			dbContentClip.body = body;
			dbContentClip.publishDate = publishDate;
			dbContentClip.expirationDate = expirationDate;
			dbContentClip.archiveDate = archiveDate;
			dbContentClip.priority = priority;
			dbContentClip.sortOrder = sortOrder;
			dbContentClip.commentsEnabled = commentsEnabled;
			dbContentClip.notifyOnComments = notifyOnComments;
			dbContentClip.overrideUrl = overrideUrl;
			dbContentClip.menuLabel = menuLabel;
			dbContentClip.menuTooltip = menuTooltip;
			dbContentClip.menuEnabled = menuEnabled;
			dbContentClip.menuOrder = menuOrder;
			dbContentClip.menuLeftIcon = menuLeftIcon;
			dbContentClip.menuLeftIconOver = menuLeftIconOver;
			dbContentClip.menuRightIcon = menuRightIcon;
			dbContentClip.menuRightIconOver = menuRightIconOver;
			dbContentClip.menuBreak = menuBreak;

			if(status != null)
			{
				if(isolation)
				{
					dbContentClip.status = status.NewPlaceHolder();
				}
				else
				{
					dbContentClip.status = status.Copy(false);
				}
			}
			if(parentCatalog != null)
			{
				if(isolation)
				{
					dbContentClip.parentCatalog = parentCatalog.NewPlaceHolder();
				}
				else
				{
					dbContentClip.parentCatalog = parentCatalog.Copy(false);
				}
			}
			if(rating != null)
			{
				if(isolation)
				{
					dbContentClip.rating = rating.NewPlaceHolder();
				}
				else
				{
					dbContentClip.rating = rating.Copy(false);
				}
			}
			if(references != null)
			{
				if(isolation)
				{
					dbContentClip.references = references.Copy(true);
				}
				else
				{
					dbContentClip.references = references.Copy(false);
				}
			}
			if(workingDraft != null)
			{
				if(isolation)
				{
					dbContentClip.workingDraft = workingDraft.NewPlaceHolder();
				}
				else
				{
					dbContentClip.workingDraft = workingDraft.Copy(false);
				}
			}
			if(authors != null)
			{
				if(isolation)
				{
					dbContentClip.authors = authors.Copy(true);
				}
				else
				{
					dbContentClip.authors = authors.Copy(false);
				}
			}
			if(editors != null)
			{
				if(isolation)
				{
					dbContentClip.editors = editors.Copy(true);
				}
				else
				{
					dbContentClip.editors = editors.Copy(false);
				}
			}
		}

		public DbContentClip NewPlaceHolder()
		{
			DbContentClip dbContentClip = new DbContentClip();
			dbContentClip.iD = iD;
			dbContentClip.isPlaceHolder = true;
			dbContentClip.isSynced = true;
			return dbContentClip;
		}

		public static DbContentClip NewPlaceHolder(int iD)
		{
			DbContentClip dbContentClip = new DbContentClip();
			dbContentClip.iD = iD;
			dbContentClip.isPlaceHolder = true;
			dbContentClip.isSynced = true;
			return dbContentClip;
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
			DbContentClip dbContentClip = (DbContentClip) obj;
			return this.iD - dbContentClip.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(DbContentClip dbContentClip)
		{
			return this.iD - dbContentClip.iD;
		}

		public override bool Equals(object dbContentClip)
		{
			if(dbContentClip == null)
				return false;

			return Equals((DbContentClip) dbContentClip);
		}

		public bool Equals(DbContentClip dbContentClip)
		{
			if(dbContentClip == null)
				return false;

			return this.iD == dbContentClip.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

		//--- Begin Custom Code ---
																																																																																																																				
		public override string ToString()
		{
			return Title;
		}

		public bool IsPublished
		{
			get
			{
				return PublishCheck(DateTime.Now, DateTime.Now, DateTime.Now);
			}
		}

		public bool PublishCheck(DateTime minDate, DateTime maxDate)
		{
            return PublishCheck(minDate, maxDate, DateTime.Now);
		}

		public bool PublishCheck(DateTime minDate, DateTime maxDate, DateTime expireDate)
		{
			return PublishDate >= minDate &
				PublishDate <= maxDate &
				ExpirationDate > expireDate;
		}

		#region Hit Counter Methods

		public int SaveHit(System.Web.HttpRequest request, bool isUnique)
		{
			return SaveHit(request, isUnique, null);		
		}

		public int SaveHit(System.Web.HttpRequest request, bool isUnique, GreyFoxUser user)
		{
			DbContentHit hit = new DbContentHit();
			hit.User = user;
			hit.UserAgent = request.UserAgent;
			hit.UserHostAddress = request.UserHostAddress;
			hit.UserHostName = request.UserHostName;
			hit.RequestDate = DateTime.Now;
			hit.IsUnique = isUnique;
			if(request.UrlReferrer != null) 
			{
				hit.RequestReferrer = request.UrlReferrer.ToString();

				if(hit.RequestReferrer.Length > 255)
				{
					hit.RequestReferrer = hit.RequestReferrer.Substring(0, 255);
				}
			}
			else
			{
				hit.RequestReferrer = "None";
			}
			hit.RequestContent = this;
			return hit.Save();
		}

		public int GetHitCount()
		{
			// TODO: Get hit count on this clip

			DbContentHitManager hm = new DbContentHitManager();
			DbContentHitCollection hits = hm.GetCollection("RequestContentID=" + this.ID.ToString(), "RequestDate", null);
			return hits.Count;
		}

		#endregion

		//--- End Custom Code ---
	}
}
