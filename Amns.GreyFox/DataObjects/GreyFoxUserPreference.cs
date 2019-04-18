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

namespace Amns.GreyFox
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class GreyFoxUserPreference : ICloneable, IComparable
	{
		// Fields
		internal int iD = 0;
		internal bool isPlaceHolder;		// Identifies objects with only filled IDs
		internal bool isSynced;			// Identifies if the object is currently synced to the loaded object
		internal string name;
		internal string value;
		internal GreyFoxUser user;

		// Public Properties
		#region Default DbModel Public Properties

		/// <summary>
		/// GreyFoxUserPreference Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the GreyFoxUserPreference as a Placeholder. Placeholders only contain 
		/// a GreyFoxUserPreference ID. Record late-binds data when it is accessed.
		/// </summary>
		public bool IsPlaceHolder
		{
			get
			{
				return isPlaceHolder;
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
		public string Value
		{
			get
			{
				EnsurePreLoad();
				return value;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= value == value;
				value = value;
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

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of GreyFoxUserPreference.
		/// </summary>
		public GreyFoxUserPreference()
		{
		}

		public GreyFoxUserPreference(int id)
		{
			this.iD = id;
			isSynced = GreyFoxUserPreferenceManager._fill(this);
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

			GreyFoxUserPreferenceManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the GreyFoxUserPreference object state to the database.
		/// </summary>
		public int Save()
		{

			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = GreyFoxUserPreferenceManager._insert(this);
			else
				GreyFoxUserPreferenceManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			GreyFoxUserPreferenceManager._delete(this.iD);
		}
		/// <summary>
		/// Duplicates GreyFoxUserPreference object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxUserPreference object reflecting the replicated GreyFoxUserPreference object.</returns>
		public GreyFoxUserPreference Duplicate()
		{
			GreyFoxUserPreference clonedGreyFoxUserPreference = this.Clone();

			// Insert must be called after children are replicated!
			clonedGreyFoxUserPreference.iD = GreyFoxUserPreferenceManager._insert(clonedGreyFoxUserPreference);
			clonedGreyFoxUserPreference.isSynced = true;
			return clonedGreyFoxUserPreference;
		}

		/// <summary>
		/// Overwrites and existing GreyFoxUserPreference object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			GreyFoxUserPreferenceManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones GreyFoxUserPreference object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxUserPreference object reflecting the replicated GreyFoxUserPreference object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones GreyFoxUserPreference object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxUserPreference object reflecting the replicated GreyFoxUserPreference object.</returns>
		public GreyFoxUserPreference Clone()
		{
			GreyFoxUserPreference clonedGreyFoxUserPreference = new GreyFoxUserPreference();
			clonedGreyFoxUserPreference.iD = iD;
			clonedGreyFoxUserPreference.isSynced = isSynced;
			clonedGreyFoxUserPreference.name = name;
			clonedGreyFoxUserPreference.value = value;

			if(user != null)
				clonedGreyFoxUserPreference.user = user;

			return clonedGreyFoxUserPreference;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxUserPreference.
		/// </summary>
		/// <returns> A new GreyFoxUserPreference object reflecting the cloned GreyFoxUserPreference object.</returns>
		public GreyFoxUserPreference Copy()
		{
			GreyFoxUserPreference greyFoxUserPreference = new GreyFoxUserPreference();
			CopyTo(greyFoxUserPreference);
			return greyFoxUserPreference;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxUserPreference.
		/// </summary>
		/// <returns> A new GreyFoxUserPreference object reflecting the cloned GreyFoxUserPreference object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxUserPreference from its children.</param>
		public GreyFoxUserPreference Copy(bool isolation)
		{
			GreyFoxUserPreference greyFoxUserPreference = new GreyFoxUserPreference();
			CopyTo(greyFoxUserPreference, isolation);
			return greyFoxUserPreference;
		}

		/// <summary>
		/// Deep copies the current GreyFoxUserPreference to another instance of GreyFoxUserPreference.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="GreyFoxUserPreference">The GreyFoxUserPreference to copy to.</param>
		public void CopyTo(GreyFoxUserPreference greyFoxUserPreference)
		{
			CopyTo(greyFoxUserPreference, false);
		}

		/// <summary>
		/// Deep copies the current GreyFoxUserPreference to another instance of GreyFoxUserPreference.
		/// </summary>
		/// <param name="GreyFoxUserPreference">The GreyFoxUserPreference to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxUserPreference from its children.</param>
		public void CopyTo(GreyFoxUserPreference greyFoxUserPreference, bool isolation)
		{
			greyFoxUserPreference.iD = iD;
			greyFoxUserPreference.isPlaceHolder = isPlaceHolder;
			greyFoxUserPreference.isSynced = isSynced;
			greyFoxUserPreference.name = name;
			greyFoxUserPreference.value = value;

			if(user != null)
			{
				if(isolation)
				{
					greyFoxUserPreference.user = user.NewPlaceHolder();
				}
				else
				{
					greyFoxUserPreference.user = user.Copy(false);
				}
			}
		}

		public GreyFoxUserPreference NewPlaceHolder()
		{
			GreyFoxUserPreference greyFoxUserPreference = new GreyFoxUserPreference();
			greyFoxUserPreference.iD = iD;
			greyFoxUserPreference.isPlaceHolder = true;
			greyFoxUserPreference.isSynced = true;
			return greyFoxUserPreference;
		}

		public static GreyFoxUserPreference NewPlaceHolder(int iD)
		{
			GreyFoxUserPreference greyFoxUserPreference = new GreyFoxUserPreference();
			greyFoxUserPreference.iD = iD;
			greyFoxUserPreference.isPlaceHolder = true;
			greyFoxUserPreference.isSynced = true;
			return greyFoxUserPreference;
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
			GreyFoxUserPreference greyFoxUserPreference = (GreyFoxUserPreference) obj;
			return this.iD - greyFoxUserPreference.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(GreyFoxUserPreference greyFoxUserPreference)
		{
			return this.iD - greyFoxUserPreference.iD;
		}

		public override bool Equals(object greyFoxUserPreference)
		{
			if(greyFoxUserPreference == null)
				return false;

			return Equals((GreyFoxUserPreference) greyFoxUserPreference);
		}

		public bool Equals(GreyFoxUserPreference greyFoxUserPreference)
		{
			if(greyFoxUserPreference == null)
				return false;

			return this.iD == greyFoxUserPreference.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

	}
}
