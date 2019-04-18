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
	public class DbContentHit : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal GreyFoxUser user;
		internal string userAgent;
		internal string userHostAddress;
		internal string userHostName;
		internal DateTime requestDate;
		internal string requestReferrer;
		internal DbContentClip requestContent;
		internal bool isUnique;

		#endregion

		#region Public Properties

		/// <summary>
		/// DbContentHit Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the DbContentHit as a Placeholder. Placeholders only contain 
		/// a DbContentHit ID. Record late-binds data when it is accessed.
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
		public GreyFoxUser User
		{
			get
			{
				EnsurePreLoad();
				return user;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(user == null)
					{
						return;
					}
					else
					{
						user = value;
						isSynced = false;
					}
				}
				else
				{
					if(user != null && value.ID == user.ID)
					{
						return; 
					}
					else
					{
						user = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public string UserAgent
		{
			get
			{
				EnsurePreLoad();
				return userAgent;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= userAgent == value;
				userAgent = value;
			}
		}

		/// <summary>
		/// </summary>
		public string UserHostAddress
		{
			get
			{
				EnsurePreLoad();
				return userHostAddress;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= userHostAddress == value;
				userHostAddress = value;
			}
		}

		/// <summary>
		/// </summary>
		public string UserHostName
		{
			get
			{
				EnsurePreLoad();
				return userHostName;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= userHostName == value;
				userHostName = value;
			}
		}

		/// <summary>
		/// </summary>
		public DateTime RequestDate
		{
			get
			{
				EnsurePreLoad();
				return requestDate;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= requestDate == value;
				requestDate = value;
			}
		}

		/// <summary>
		/// </summary>
		public string RequestReferrer
		{
			get
			{
				EnsurePreLoad();
				return requestReferrer;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= requestReferrer == value;
				requestReferrer = value;
			}
		}

		/// <summary>
		/// </summary>
		public DbContentClip RequestContent
		{
			get
			{
				EnsurePreLoad();
				return requestContent;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(requestContent == null)
					{
						return;
					}
					else
					{
						requestContent = value;
						isSynced = false;
					}
				}
				else
				{
					if(requestContent != null && value.ID == requestContent.ID)
					{
						return; 
					}
					else
					{
						requestContent = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public bool IsUnique
		{
			get
			{
				EnsurePreLoad();
				return isUnique;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= isUnique == value;
				isUnique = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of DbContentHit.
		/// </summary>
		public DbContentHit()
		{
		}

		public DbContentHit(int id)
		{
			this.iD = id;
			isSynced = DbContentHitManager._fill(this);
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

			DbContentHitManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the DbContentHit object state to the database.
		/// </summary>
		public int Save()
		{

			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = DbContentHitManager._insert(this);
			else
				DbContentHitManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			DbContentHitManager._delete(this.iD);
		}
		/// <summary>
		/// Duplicates DbContentHit object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentHit object reflecting the replicated DbContentHit object.</returns>
		public DbContentHit Duplicate()
		{
			DbContentHit clonedDbContentHit = this.Clone();

			// Insert must be called after children are replicated!
			clonedDbContentHit.iD = DbContentHitManager._insert(clonedDbContentHit);
			clonedDbContentHit.isSynced = true;
			return clonedDbContentHit;
		}

		/// <summary>
		/// Overwrites and existing DbContentHit object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			DbContentHitManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones DbContentHit object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentHit object reflecting the replicated DbContentHit object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones DbContentHit object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentHit object reflecting the replicated DbContentHit object.</returns>
		public DbContentHit Clone()
		{
			DbContentHit clonedDbContentHit = new DbContentHit();
			clonedDbContentHit.iD = iD;
			clonedDbContentHit.isSynced = isSynced;
			clonedDbContentHit.userAgent = userAgent;
			clonedDbContentHit.userHostAddress = userHostAddress;
			clonedDbContentHit.userHostName = userHostName;
			clonedDbContentHit.requestDate = requestDate;
			clonedDbContentHit.requestReferrer = requestReferrer;
			clonedDbContentHit.isUnique = isUnique;


			if(user != null)
				clonedDbContentHit.user = user;

			if(requestContent != null)
				clonedDbContentHit.requestContent = requestContent;

			return clonedDbContentHit;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentHit.
		/// </summary>
		/// <returns> A new DbContentHit object reflecting the cloned DbContentHit object.</returns>
		public DbContentHit Copy()
		{
			DbContentHit dbContentHit = new DbContentHit();
			CopyTo(dbContentHit);
			return dbContentHit;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentHit.
		/// </summary>
		/// <returns> A new DbContentHit object reflecting the cloned DbContentHit object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the DbContentHit from its children.</param>
		public DbContentHit Copy(bool isolation)
		{
			DbContentHit dbContentHit = new DbContentHit();
			CopyTo(dbContentHit, isolation);
			return dbContentHit;
		}

		/// <summary>
		/// Deep copies the current DbContentHit to another instance of DbContentHit.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="DbContentHit">The DbContentHit to copy to.</param>
		public void CopyTo(DbContentHit dbContentHit)
		{
			CopyTo(dbContentHit, false);
		}

		/// <summary>
		/// Deep copies the current DbContentHit to another instance of DbContentHit.
		/// </summary>
		/// <param name="DbContentHit">The DbContentHit to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the DbContentHit from its children.</param>
		public void CopyTo(DbContentHit dbContentHit, bool isolation)
		{
			dbContentHit.iD = iD;
			dbContentHit.isPlaceHolder = isPlaceHolder;
			dbContentHit.isSynced = isSynced;
			dbContentHit.userAgent = userAgent;
			dbContentHit.userHostAddress = userHostAddress;
			dbContentHit.userHostName = userHostName;
			dbContentHit.requestDate = requestDate;
			dbContentHit.requestReferrer = requestReferrer;
			dbContentHit.isUnique = isUnique;

			if(user != null)
			{
				if(isolation)
				{
					dbContentHit.user = user.NewPlaceHolder();
				}
				else
				{
					dbContentHit.user = user.Copy(false);
				}
			}
			if(requestContent != null)
			{
				if(isolation)
				{
					dbContentHit.requestContent = requestContent.NewPlaceHolder();
				}
				else
				{
					dbContentHit.requestContent = requestContent.Copy(false);
				}
			}
		}

		public DbContentHit NewPlaceHolder()
		{
			DbContentHit dbContentHit = new DbContentHit();
			dbContentHit.iD = iD;
			dbContentHit.isPlaceHolder = true;
			dbContentHit.isSynced = true;
			return dbContentHit;
		}

		public static DbContentHit NewPlaceHolder(int iD)
		{
			DbContentHit dbContentHit = new DbContentHit();
			dbContentHit.iD = iD;
			dbContentHit.isPlaceHolder = true;
			dbContentHit.isSynced = true;
			return dbContentHit;
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
			DbContentHit dbContentHit = (DbContentHit) obj;
			return this.iD - dbContentHit.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(DbContentHit dbContentHit)
		{
			return this.iD - dbContentHit.iD;
		}

		public override bool Equals(object dbContentHit)
		{
			if(dbContentHit == null)
				return false;

			return Equals((DbContentHit) dbContentHit);
		}

		public bool Equals(DbContentHit dbContentHit)
		{
			if(dbContentHit == null)
				return false;

			return this.iD == dbContentHit.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

	}
}
