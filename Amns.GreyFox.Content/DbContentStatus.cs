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

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class DbContentStatus : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal string name;
		internal bool isDraft;
		internal bool isPublished;
		internal bool feeEnabled;
		internal bool editEnabled;
		internal bool archiveEnabled;
		internal bool reviewEnabled;
		internal string icon;

		#endregion

		#region Public Properties

		/// <summary>
		/// DbContentStatus Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the DbContentStatus as a Placeholder. Placeholders only contain 
		/// a DbContentStatus ID. Record late-binds data when it is accessed.
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
		public string Name
		{
			get
			{
				EnsurePreLoad();
				return name;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= name == value;
				name = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool IsDraft
		{
			get
			{
				EnsurePreLoad();
				return isDraft;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= isDraft == value;
				isDraft = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool IsPublished
		{
			get
			{
				EnsurePreLoad();
				return isPublished;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= isPublished == value;
				isPublished = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool FeeEnabled
		{
			get
			{
				EnsurePreLoad();
				return feeEnabled;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= feeEnabled == value;
				feeEnabled = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool EditEnabled
		{
			get
			{
				EnsurePreLoad();
				return editEnabled;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= editEnabled == value;
				editEnabled = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool ArchiveEnabled
		{
			get
			{
				EnsurePreLoad();
				return archiveEnabled;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= archiveEnabled == value;
				archiveEnabled = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool ReviewEnabled
		{
			get
			{
				EnsurePreLoad();
				return reviewEnabled;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= reviewEnabled == value;
				reviewEnabled = value;
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

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of DbContentStatus.
		/// </summary>
		public DbContentStatus()
		{
			name = string.Empty;
			icon = string.Empty;
		}

		public DbContentStatus(int id)
		{
			this.iD = id;
			isSynced = DbContentStatusManager._fill(this);
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

			DbContentStatusManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the DbContentStatus object state to the database.
		/// </summary>
		public int Save()
		{
			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = DbContentStatusManager._insert(this);
			else
				DbContentStatusManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			DbContentStatusManager._delete(this.iD);
		}
		/// <summary>
		/// Duplicates DbContentStatus object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentStatus object reflecting the replicated DbContentStatus object.</returns>
		public DbContentStatus Duplicate()
		{
			DbContentStatus clonedDbContentStatus = this.Clone();

			// Insert must be called after children are replicated!
			clonedDbContentStatus.iD = DbContentStatusManager._insert(clonedDbContentStatus);
			clonedDbContentStatus.isSynced = true;
			return clonedDbContentStatus;
		}

		/// <summary>
		/// Overwrites and existing DbContentStatus object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			DbContentStatusManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones DbContentStatus object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentStatus object reflecting the replicated DbContentStatus object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones DbContentStatus object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentStatus object reflecting the replicated DbContentStatus object.</returns>
		public DbContentStatus Clone()
		{
			DbContentStatus clonedDbContentStatus = new DbContentStatus();
			clonedDbContentStatus.iD = iD;
			clonedDbContentStatus.isSynced = isSynced;
			clonedDbContentStatus.name = name;
			clonedDbContentStatus.isDraft = isDraft;
			clonedDbContentStatus.isPublished = isPublished;
			clonedDbContentStatus.feeEnabled = feeEnabled;
			clonedDbContentStatus.editEnabled = editEnabled;
			clonedDbContentStatus.archiveEnabled = archiveEnabled;
			clonedDbContentStatus.reviewEnabled = reviewEnabled;
			clonedDbContentStatus.icon = icon;


			return clonedDbContentStatus;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentStatus.
		/// </summary>
		/// <returns> A new DbContentStatus object reflecting the cloned DbContentStatus object.</returns>
		public DbContentStatus Copy()
		{
			DbContentStatus dbContentStatus = new DbContentStatus();
			CopyTo(dbContentStatus);
			return dbContentStatus;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentStatus.
		/// </summary>
		/// <returns> A new DbContentStatus object reflecting the cloned DbContentStatus object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the DbContentStatus from its children.</param>
		public DbContentStatus Copy(bool isolation)
		{
			DbContentStatus dbContentStatus = new DbContentStatus();
			CopyTo(dbContentStatus, isolation);
			return dbContentStatus;
		}

		/// <summary>
		/// Deep copies the current DbContentStatus to another instance of DbContentStatus.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="DbContentStatus">The DbContentStatus to copy to.</param>
		public void CopyTo(DbContentStatus dbContentStatus)
		{
			CopyTo(dbContentStatus, false);
		}

		/// <summary>
		/// Deep copies the current DbContentStatus to another instance of DbContentStatus.
		/// </summary>
		/// <param name="DbContentStatus">The DbContentStatus to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the DbContentStatus from its children.</param>
		public void CopyTo(DbContentStatus dbContentStatus, bool isolation)
		{
			dbContentStatus.iD = iD;
			dbContentStatus.isPlaceHolder = isPlaceHolder;
			dbContentStatus.isSynced = isSynced;
			dbContentStatus.name = name;
			dbContentStatus.isDraft = isDraft;
			dbContentStatus.isPublished = isPublished;
			dbContentStatus.feeEnabled = feeEnabled;
			dbContentStatus.editEnabled = editEnabled;
			dbContentStatus.archiveEnabled = archiveEnabled;
			dbContentStatus.reviewEnabled = reviewEnabled;
			dbContentStatus.icon = icon;

		}

		public DbContentStatus NewPlaceHolder()
		{
			DbContentStatus dbContentStatus = new DbContentStatus();
			dbContentStatus.iD = iD;
			dbContentStatus.isPlaceHolder = true;
			dbContentStatus.isSynced = true;
			return dbContentStatus;
		}

		public static DbContentStatus NewPlaceHolder(int iD)
		{
			DbContentStatus dbContentStatus = new DbContentStatus();
			dbContentStatus.iD = iD;
			dbContentStatus.isPlaceHolder = true;
			dbContentStatus.isSynced = true;
			return dbContentStatus;
		}

		#endregion

		#region IComparable Methods

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		int IComparable.CompareTo(object obj)
		{
			DbContentStatus dbContentStatus = (DbContentStatus) obj;
			return this.iD - dbContentStatus.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(DbContentStatus dbContentStatus)
		{
			return this.iD - dbContentStatus.iD;
		}

		public override bool Equals(object dbContentStatus)
		{
			if(dbContentStatus == null)
				return false;

			return Equals((DbContentStatus) dbContentStatus);
		}

		public bool Equals(DbContentStatus dbContentStatus)
		{
			if(dbContentStatus == null)
				return false;

			return this.iD == dbContentStatus.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

	}
}
