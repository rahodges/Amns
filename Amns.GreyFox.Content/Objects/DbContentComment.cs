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
	public class DbContentComment : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal DateTime createDate;
		internal DateTime modifyDate;
		internal string name;
		internal string email;
		internal string url;
		internal string iP;
		internal string body;

		#endregion

		#region Public Properties

		/// <summary>
		/// DbContentComment Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the DbContentComment as a Placeholder. Placeholders only contain 
		/// a DbContentComment ID. Record late-binds data when it is accessed.
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
		public string Email
		{
			get
			{
				EnsurePreLoad();
				return email;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= email == value;
				email = value;
			}
		}

		/// <summary>
		/// </summary>
		public string Url
		{
			get
			{
				EnsurePreLoad();
				return url;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= url == value;
				url = value;
			}
		}

		/// <summary>
		/// </summary>
		public string IP
		{
			get
			{
				EnsurePreLoad();
				return iP;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= iP == value;
				iP = value;
			}
		}

		/// <summary>
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

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of DbContentComment.
		/// </summary>
		public DbContentComment()
		{
			createDate = DateTime.Now;
			modifyDate = DateTime.Now;
			name = string.Empty;
			email = string.Empty;
			url = string.Empty;
			iP = string.Empty;
			body = string.Empty;
		}

		public DbContentComment(int id)
		{
			this.iD = id;
			isSynced = DbContentCommentManager._fill(this);
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

			DbContentCommentManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the DbContentComment object state to the database.
		/// </summary>
		public int Save()
		{
			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = DbContentCommentManager._insert(this);
			else
				DbContentCommentManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			DbContentCommentManager._delete(this.iD);
			this.iD = 0;
			isSynced = false;
		}
		/// <summary>
		/// Duplicates DbContentComment object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentComment object reflecting the replicated DbContentComment object.</returns>
		public DbContentComment Duplicate()
		{
			DbContentComment clonedDbContentComment = this.Clone();

			// Insert must be called after children are replicated!
			clonedDbContentComment.iD = DbContentCommentManager._insert(clonedDbContentComment);
			clonedDbContentComment.isSynced = true;
			return clonedDbContentComment;
		}

		/// <summary>
		/// Overwrites and existing DbContentComment object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			DbContentCommentManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones DbContentComment object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentComment object reflecting the replicated DbContentComment object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones DbContentComment object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new DbContentComment object reflecting the replicated DbContentComment object.</returns>
		public DbContentComment Clone()
		{
			DbContentComment clonedDbContentComment = new DbContentComment();
			clonedDbContentComment.iD = iD;
			clonedDbContentComment.isSynced = isSynced;
			clonedDbContentComment.createDate = createDate;
			clonedDbContentComment.modifyDate = modifyDate;
			clonedDbContentComment.name = name;
			clonedDbContentComment.email = email;
			clonedDbContentComment.url = url;
			clonedDbContentComment.iP = iP;
			clonedDbContentComment.body = body;


			return clonedDbContentComment;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentComment.
		/// </summary>
		/// <returns> A new DbContentComment object reflecting the cloned DbContentComment object.</returns>
		public DbContentComment Copy()
		{
			DbContentComment dbContentComment = new DbContentComment();
			CopyTo(dbContentComment);
			return dbContentComment;
		}

		/// <summary>
		/// Makes a deep copy of the current DbContentComment.
		/// </summary>
		/// <returns> A new DbContentComment object reflecting the cloned DbContentComment object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the DbContentComment from its children.</param>
		public DbContentComment Copy(bool isolation)
		{
			DbContentComment dbContentComment = new DbContentComment();
			CopyTo(dbContentComment, isolation);
			return dbContentComment;
		}

		/// <summary>
		/// Deep copies the current DbContentComment to another instance of DbContentComment.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="DbContentComment">The DbContentComment to copy to.</param>
		public void CopyTo(DbContentComment dbContentComment)
		{
			CopyTo(dbContentComment, false);
		}

		/// <summary>
		/// Deep copies the current DbContentComment to another instance of DbContentComment.
		/// </summary>
		/// <param name="DbContentComment">The DbContentComment to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the DbContentComment from its children.</param>
		public void CopyTo(DbContentComment dbContentComment, bool isolation)
		{
			dbContentComment.iD = iD;
			dbContentComment.isPlaceHolder = isPlaceHolder;
			dbContentComment.isSynced = isSynced;
			dbContentComment.createDate = createDate;
			dbContentComment.modifyDate = modifyDate;
			dbContentComment.name = name;
			dbContentComment.email = email;
			dbContentComment.url = url;
			dbContentComment.iP = iP;
			dbContentComment.body = body;

		}

		public DbContentComment NewPlaceHolder()
		{
			DbContentComment dbContentComment = new DbContentComment();
			dbContentComment.iD = iD;
			dbContentComment.isPlaceHolder = true;
			dbContentComment.isSynced = true;
			return dbContentComment;
		}

		public static DbContentComment NewPlaceHolder(int iD)
		{
			DbContentComment dbContentComment = new DbContentComment();
			dbContentComment.iD = iD;
			dbContentComment.isPlaceHolder = true;
			dbContentComment.isSynced = true;
			return dbContentComment;
		}

		#endregion

		#region IComparable Methods

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		int IComparable.CompareTo(object obj)
		{
			DbContentComment dbContentComment = (DbContentComment) obj;
			return this.iD - dbContentComment.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(DbContentComment dbContentComment)
		{
			return this.iD - dbContentComment.iD;
		}

		public override bool Equals(object dbContentComment)
		{
			if(dbContentComment == null)
				return false;

			return Equals((DbContentComment) dbContentComment);
		}

		public bool Equals(DbContentComment dbContentComment)
		{
			if(dbContentComment == null)
				return false;

			return this.iD == dbContentComment.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

	}
}
