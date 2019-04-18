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
	public class DbContentRating : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal DateTime createDate;
		internal DateTime modifyDate;
		internal string name;
		internal string description;
		internal string iconUrl;
		internal GreyFoxRole requiredRole;

		#endregion

		#region Public Properties

		/// <summary>
		/// DbContentRating Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the DbContentRating as a Placeholder. Placeholders only contain 
		/// a DbContentRating ID. Record late-binds data when it is accessed.
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
		/// Specifies a location for a rating icon.
		/// </summary>
		public string IconUrl
		{
			get
			{
				EnsurePreLoad();
				return iconUrl;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= iconUrl == value;
				iconUrl = value;
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxRole RequiredRole
		{
			get
			{
				EnsurePreLoad();
				return requiredRole;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(requiredRole == null)
					{
						return;
					}
					else
					{
						requiredRole = value;
						isSynced = false;
					}
				}
				else
				{
					if(requiredRole != null && value.ID == requiredRole.ID)
					{
						return; 
					}
					else
					{
						requiredRole = value;
						isSynced = false;
					}
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of DbContentRating.
		/// </summary>
		public DbContentRating()
		{
			createDate = DateTime.Now;
			modifyDate = DateTime.Now;
			name = string.Empty;
			description = string.Empty;
			iconUrl = string.Empty;
		}

		public DbContentRating(int id)
		{
			this.iD = id;
			isSynced = DbContentRatingManager._fill(this);
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

			DbContentRatingManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the DbContentRating object state to the database.
		/// </summary>
		public int Save()
		{

			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = DbContentRatingManager._insert(this);
			else
				DbContentRatingManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			DbContentRatingManager._delete(this.iD);
		}
		/// <summary>
		/// Duplicates DbContentRating object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentRating object reflecting the replicated DbContentRating object.</returns>
		public DbContentRating Duplicate()
		{
			DbContentRating clonedDbContentRating = this.Clone();

			// Insert must be called after children are replicated!
			clonedDbContentRating.iD = DbContentRatingManager._insert(clonedDbContentRating);
			clonedDbContentRating.isSynced = true;
			return clonedDbContentRating;
		}

		/// <summary>
		/// Overwrites and existing DbContentRating object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			DbContentRatingManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones DbContentRating object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentRating object reflecting the replicated DbContentRating object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones DbContentRating object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentRating object reflecting the replicated DbContentRating object.</returns>
		public DbContentRating Clone()
		{
			DbContentRating clonedDbContentRating = new DbContentRating();
			clonedDbContentRating.iD = iD;
			clonedDbContentRating.isSynced = isSynced;
			clonedDbContentRating.createDate = createDate;
			clonedDbContentRating.modifyDate = modifyDate;
			clonedDbContentRating.name = name;
			clonedDbContentRating.description = description;
			clonedDbContentRating.iconUrl = iconUrl;


			if(requiredRole != null)
				clonedDbContentRating.requiredRole = requiredRole;

			return clonedDbContentRating;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentRating.
		/// </summary>
		/// <returns> A new DbContentRating object reflecting the cloned DbContentRating object.</returns>
		public DbContentRating Copy()
		{
			DbContentRating dbContentRating = new DbContentRating();
			CopyTo(dbContentRating);
			return dbContentRating;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentRating.
		/// </summary>
		/// <returns> A new DbContentRating object reflecting the cloned DbContentRating object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the DbContentRating from its children.</param>
		public DbContentRating Copy(bool isolation)
		{
			DbContentRating dbContentRating = new DbContentRating();
			CopyTo(dbContentRating, isolation);
			return dbContentRating;
		}

		/// <summary>
		/// Deep copies the current DbContentRating to another instance of DbContentRating.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="DbContentRating">The DbContentRating to copy to.</param>
		public void CopyTo(DbContentRating dbContentRating)
		{
			CopyTo(dbContentRating, false);
		}

		/// <summary>
		/// Deep copies the current DbContentRating to another instance of DbContentRating.
		/// </summary>
		/// <param name="DbContentRating">The DbContentRating to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the DbContentRating from its children.</param>
		public void CopyTo(DbContentRating dbContentRating, bool isolation)
		{
			dbContentRating.iD = iD;
			dbContentRating.isPlaceHolder = isPlaceHolder;
			dbContentRating.isSynced = isSynced;
			dbContentRating.createDate = createDate;
			dbContentRating.modifyDate = modifyDate;
			dbContentRating.name = name;
			dbContentRating.description = description;
			dbContentRating.iconUrl = iconUrl;

			if(requiredRole != null)
			{
				if(isolation)
				{
					dbContentRating.requiredRole = requiredRole.NewPlaceHolder();
				}
				else
				{
					dbContentRating.requiredRole = requiredRole.Copy(false);
				}
			}
		}

		public DbContentRating NewPlaceHolder()
		{
			DbContentRating dbContentRating = new DbContentRating();
			dbContentRating.iD = iD;
			dbContentRating.isPlaceHolder = true;
			dbContentRating.isSynced = true;
			return dbContentRating;
		}

		public static DbContentRating NewPlaceHolder(int iD)
		{
			DbContentRating dbContentRating = new DbContentRating();
			dbContentRating.iD = iD;
			dbContentRating.isPlaceHolder = true;
			dbContentRating.isSynced = true;
			return dbContentRating;
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
			DbContentRating dbContentRating = (DbContentRating) obj;
			return this.iD - dbContentRating.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(DbContentRating dbContentRating)
		{
			return this.iD - dbContentRating.iD;
		}

		public override bool Equals(object dbContentRating)
		{
			if(dbContentRating == null)
				return false;

			return Equals((DbContentRating) dbContentRating);
		}

		public bool Equals(DbContentRating dbContentRating)
		{
			if(dbContentRating == null)
				return false;

			return this.iD == dbContentRating.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

	}
}
