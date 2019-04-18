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
	public class YariMediaKeyword : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal string keyword;

		#endregion

		#region Public Properties

		/// <summary>
		/// YariMediaKeyword Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the YariMediaKeyword as a Placeholder. Placeholders only contain 
		/// a YariMediaKeyword ID. Record late-binds data when it is accessed.
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
		public string Keyword
		{
			get
			{
				EnsurePreLoad();
				return keyword;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= keyword == value;
				keyword = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of YariMediaKeyword.
		/// </summary>
		public YariMediaKeyword()
		{
		}

		public YariMediaKeyword(int id)
		{
			this.iD = id;
			isSynced = YariMediaKeywordManager._fill(this);
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

			YariMediaKeywordManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the YariMediaKeyword object state to the database.
		/// </summary>
		public int Save()
		{
			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = YariMediaKeywordManager._insert(this);
			else
				YariMediaKeywordManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			YariMediaKeywordManager._delete(this.iD);
			this.iD = 0;
			isSynced = false;
		}
		/// <summary>
		/// Duplicates YariMediaKeyword object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new YariMediaKeyword object reflecting the replicated YariMediaKeyword object.</returns>
		public YariMediaKeyword Duplicate()
		{
			YariMediaKeyword clonedYariMediaKeyword = this.Clone();

			// Insert must be called after children are replicated!
			clonedYariMediaKeyword.iD = YariMediaKeywordManager._insert(clonedYariMediaKeyword);
			clonedYariMediaKeyword.isSynced = true;
			return clonedYariMediaKeyword;
		}

		/// <summary>
		/// Overwrites and existing YariMediaKeyword object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			YariMediaKeywordManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones YariMediaKeyword object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new YariMediaKeyword object reflecting the replicated YariMediaKeyword object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones YariMediaKeyword object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new YariMediaKeyword object reflecting the replicated YariMediaKeyword object.</returns>
		public YariMediaKeyword Clone()
		{
			YariMediaKeyword clonedYariMediaKeyword = new YariMediaKeyword();
			clonedYariMediaKeyword.iD = iD;
			clonedYariMediaKeyword.isSynced = isSynced;
			clonedYariMediaKeyword.keyword = keyword;


			return clonedYariMediaKeyword;
		}

		/// <summary>
		/// Makes a deep copy of the current YariMediaKeyword.
		/// </summary>
		/// <returns> A new YariMediaKeyword object reflecting the cloned YariMediaKeyword object.</returns>
		public YariMediaKeyword Copy()
		{
			YariMediaKeyword yariMediaKeyword = new YariMediaKeyword();
			CopyTo(yariMediaKeyword);
			return yariMediaKeyword;
		}

		/// <summary>
		/// Makes a deep copy of the current YariMediaKeyword.
		/// </summary>
		/// <returns> A new YariMediaKeyword object reflecting the cloned YariMediaKeyword object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the YariMediaKeyword from its children.</param>
		public YariMediaKeyword Copy(bool isolation)
		{
			YariMediaKeyword yariMediaKeyword = new YariMediaKeyword();
			CopyTo(yariMediaKeyword, isolation);
			return yariMediaKeyword;
		}

		/// <summary>
		/// Deep copies the current YariMediaKeyword to another instance of YariMediaKeyword.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="YariMediaKeyword">The YariMediaKeyword to copy to.</param>
		public void CopyTo(YariMediaKeyword yariMediaKeyword)
		{
			CopyTo(yariMediaKeyword, false);
		}

		/// <summary>
		/// Deep copies the current YariMediaKeyword to another instance of YariMediaKeyword.
		/// </summary>
		/// <param name="YariMediaKeyword">The YariMediaKeyword to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the YariMediaKeyword from its children.</param>
		public void CopyTo(YariMediaKeyword yariMediaKeyword, bool isolation)
		{
			yariMediaKeyword.iD = iD;
			yariMediaKeyword.isPlaceHolder = isPlaceHolder;
			yariMediaKeyword.isSynced = isSynced;
			yariMediaKeyword.keyword = keyword;

		}

		public YariMediaKeyword NewPlaceHolder()
		{
			YariMediaKeyword yariMediaKeyword = new YariMediaKeyword();
			yariMediaKeyword.iD = iD;
			yariMediaKeyword.isPlaceHolder = true;
			yariMediaKeyword.isSynced = true;
			return yariMediaKeyword;
		}

		public static YariMediaKeyword NewPlaceHolder(int iD)
		{
			YariMediaKeyword yariMediaKeyword = new YariMediaKeyword();
			yariMediaKeyword.iD = iD;
			yariMediaKeyword.isPlaceHolder = true;
			yariMediaKeyword.isSynced = true;
			return yariMediaKeyword;
		}

		#endregion

		#region IComparable Methods

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		int IComparable.CompareTo(object obj)
		{
			YariMediaKeyword yariMediaKeyword = (YariMediaKeyword) obj;
			return this.iD - yariMediaKeyword.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(YariMediaKeyword yariMediaKeyword)
		{
			return this.iD - yariMediaKeyword.iD;
		}

		public override bool Equals(object yariMediaKeyword)
		{
			if(yariMediaKeyword == null)
				return false;

			return Equals((YariMediaKeyword) yariMediaKeyword);
		}

		public bool Equals(YariMediaKeyword yariMediaKeyword)
		{
			if(yariMediaKeyword == null)
				return false;

			return this.iD == yariMediaKeyword.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

		//--- Begin Custom Code ---
																																				
		#region ToString()

		public override string ToString()
		{
			return Keyword;
		}


		#endregion

		//--- End Custom Code ---
	}
}
