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

namespace Amns.GreyFox.Yari
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class YariMediaType : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal string name;

		#endregion

		#region Public Properties

		/// <summary>
		/// YariMediaType Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the YariMediaType as a Placeholder. Placeholders only contain 
		/// a YariMediaType ID. Record late-binds data when it is accessed.
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
		/// The name of the media reference type.
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

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of YariMediaType.
		/// </summary>
		public YariMediaType()
		{
		}

		public YariMediaType(int id)
		{
			this.iD = id;
			isSynced = YariMediaTypeManager._fill(this);
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

			YariMediaTypeManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the YariMediaType object state to the database.
		/// </summary>
		public int Save()
		{
			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = YariMediaTypeManager._insert(this);
			else
				YariMediaTypeManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			YariMediaTypeManager._delete(this.iD);
			this.iD = 0;
			isSynced = false;
		}
		/// <summary>
		/// Duplicates YariMediaType object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new YariMediaType object reflecting the replicated YariMediaType object.</returns>
		public YariMediaType Duplicate()
		{
			YariMediaType clonedYariMediaType = this.Clone();

			// Insert must be called after children are replicated!
			clonedYariMediaType.iD = YariMediaTypeManager._insert(clonedYariMediaType);
			clonedYariMediaType.isSynced = true;
			return clonedYariMediaType;
		}

		/// <summary>
		/// Overwrites and existing YariMediaType object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			YariMediaTypeManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones YariMediaType object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new YariMediaType object reflecting the replicated YariMediaType object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones YariMediaType object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new YariMediaType object reflecting the replicated YariMediaType object.</returns>
		public YariMediaType Clone()
		{
			YariMediaType clonedYariMediaType = new YariMediaType();
			clonedYariMediaType.iD = iD;
			clonedYariMediaType.isSynced = isSynced;
			clonedYariMediaType.name = name;


			return clonedYariMediaType;
		}

		/// <summary>
		/// Makes a deep copy of the current YariMediaType.
		/// </summary>
		/// <returns> A new YariMediaType object reflecting the cloned YariMediaType object.</returns>
		public YariMediaType Copy()
		{
			YariMediaType yariMediaType = new YariMediaType();
			CopyTo(yariMediaType);
			return yariMediaType;
		}

		/// <summary>
		/// Makes a deep copy of the current YariMediaType.
		/// </summary>
		/// <returns> A new YariMediaType object reflecting the cloned YariMediaType object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the YariMediaType from its children.</param>
		public YariMediaType Copy(bool isolation)
		{
			YariMediaType yariMediaType = new YariMediaType();
			CopyTo(yariMediaType, isolation);
			return yariMediaType;
		}

		/// <summary>
		/// Deep copies the current YariMediaType to another instance of YariMediaType.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="YariMediaType">The YariMediaType to copy to.</param>
		public void CopyTo(YariMediaType yariMediaType)
		{
			CopyTo(yariMediaType, false);
		}

		/// <summary>
		/// Deep copies the current YariMediaType to another instance of YariMediaType.
		/// </summary>
		/// <param name="YariMediaType">The YariMediaType to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the YariMediaType from its children.</param>
		public void CopyTo(YariMediaType yariMediaType, bool isolation)
		{
			yariMediaType.iD = iD;
			yariMediaType.isPlaceHolder = isPlaceHolder;
			yariMediaType.isSynced = isSynced;
			yariMediaType.name = name;

		}

		public YariMediaType NewPlaceHolder()
		{
			YariMediaType yariMediaType = new YariMediaType();
			yariMediaType.iD = iD;
			yariMediaType.isPlaceHolder = true;
			yariMediaType.isSynced = true;
			return yariMediaType;
		}

		public static YariMediaType NewPlaceHolder(int iD)
		{
			YariMediaType yariMediaType = new YariMediaType();
			yariMediaType.iD = iD;
			yariMediaType.isPlaceHolder = true;
			yariMediaType.isSynced = true;
			return yariMediaType;
		}

		#endregion

		#region IComparable Methods

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		int IComparable.CompareTo(object obj)
		{
			YariMediaType yariMediaType = (YariMediaType) obj;
			return this.iD - yariMediaType.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(YariMediaType yariMediaType)
		{
			return this.iD - yariMediaType.iD;
		}

		public override bool Equals(object yariMediaType)
		{
			if(yariMediaType == null)
				return false;

			return Equals((YariMediaType) yariMediaType);
		}

		public bool Equals(YariMediaType yariMediaType)
		{
			if(yariMediaType == null)
				return false;

			return this.iD == yariMediaType.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

	}
}
