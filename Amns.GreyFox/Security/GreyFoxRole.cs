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

namespace Amns.GreyFox.Security
{
	/// <summary>
	/// Role class to hold permissions for users.
	/// </summary>
	public class GreyFoxRole : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal string name;
		internal string description;
		internal bool isDisabled;

		#endregion

		#region Public Properties

		/// <summary>
		/// GreyFoxRole Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the GreyFoxRole as a Placeholder. Placeholders only contain 
		/// a GreyFoxRole ID. Record late-binds data when it is accessed.
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
		/// The name of the role. Should be in the format [domain]/[role] if using 
		/// roles to separate access between certain application objects.

		/// 

		/// For example, "Tessen/Administrator" is an administrator role specific 
		/// to Tessen while "Administrator" is a global administrator account.
		/// </summary>
		public string Name
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return name;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= name == value;
					name = value;
				}
			}
		}

		/// <summary>
		/// The description of the role.
		/// </summary>
		public string Description
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return description;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= description == value;
					description = value;
				}
			}
		}

		/// <summary>
		/// Disables the role from being associated with user accounts. Note that 
		/// this does not disable the role from being checked. If you have users 
		/// already logged on, their roles will not be disabled by this setting until 
		/// the next login.
		/// </summary>
		public bool IsDisabled
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return isDisabled;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= isDisabled == value;
					isDisabled = value;
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of GreyFoxRole.
		/// </summary>
		public GreyFoxRole()
		{
		}

		public GreyFoxRole(int id)
		{
			this.iD = id;
			lock(this)
			{
				isSynced = GreyFoxRoleManager._fill(this);
			}
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

			lock(this)
			{
				GreyFoxRoleManager._fill(this);
				isPlaceHolder = false;
			}
		}

		/// <summary>
		/// Saves the GreyFoxRole object state to the database.
		/// </summary>
		public int Save()
		{
			lock(this)
			{
				if(isSynced)
					return iD;

				if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
				if(iD == 0)
					iD = GreyFoxRoleManager._insert(this);
				else
					GreyFoxRoleManager._update(this);
				isSynced = iD != -1;
			}
			return iD;
		}

		public void Delete()
		{
			GreyFoxRoleManager._delete(this.iD);
			this.iD = 0;
			isSynced = false;
		}
		/// <summary>
		/// Duplicates GreyFoxRole object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxRole object reflecting the replicated GreyFoxRole object.</returns>
		public GreyFoxRole Duplicate()
		{
			lock(this)
			{
			GreyFoxRole clonedGreyFoxRole = this.Clone();

			// Insert must be called after children are replicated!
			clonedGreyFoxRole.iD = GreyFoxRoleManager._insert(clonedGreyFoxRole);
			clonedGreyFoxRole.isSynced = true;
			return clonedGreyFoxRole;
			}
		}

		/// <summary>
		/// Overwrites and existing GreyFoxRole object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			GreyFoxRoleManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones GreyFoxRole object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxRole object reflecting the replicated GreyFoxRole object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones GreyFoxRole object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxRole object reflecting the replicated GreyFoxRole object.</returns>
		public GreyFoxRole Clone()
		{
			lock(this)
			{
			GreyFoxRole clonedGreyFoxRole = new GreyFoxRole();
			clonedGreyFoxRole.iD = iD;
			clonedGreyFoxRole.isSynced = isSynced;
			clonedGreyFoxRole.name = name;
			clonedGreyFoxRole.description = description;
			clonedGreyFoxRole.isDisabled = isDisabled;


			return clonedGreyFoxRole;
			}
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxRole.
		/// </summary>
		/// <returns> A new GreyFoxRole object reflecting the cloned GreyFoxRole object.</returns>
		public GreyFoxRole Copy()
		{
			GreyFoxRole greyFoxRole = new GreyFoxRole();
			CopyTo(greyFoxRole);
			return greyFoxRole;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxRole.
		/// </summary>
		/// <returns> A new GreyFoxRole object reflecting the cloned GreyFoxRole object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxRole from its children.</param>
		public GreyFoxRole Copy(bool isolation)
		{
			GreyFoxRole greyFoxRole = new GreyFoxRole();
			CopyTo(greyFoxRole, isolation);
			return greyFoxRole;
		}

		/// <summary>
		/// Deep copies the current GreyFoxRole to another instance of GreyFoxRole.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="GreyFoxRole">The GreyFoxRole to copy to.</param>
		public void CopyTo(GreyFoxRole greyFoxRole)
		{
			CopyTo(greyFoxRole, false);
		}

		/// <summary>
		/// Deep copies the current GreyFoxRole to another instance of GreyFoxRole.
		/// </summary>
		/// <param name="GreyFoxRole">The GreyFoxRole to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxRole from its children.</param>
		public void CopyTo(GreyFoxRole greyFoxRole, bool isolation)
		{
			lock(this)
			{
				greyFoxRole.iD = iD;
				greyFoxRole.isPlaceHolder = isPlaceHolder;
				greyFoxRole.isSynced = isSynced;
				greyFoxRole.name = name;
				greyFoxRole.description = description;
				greyFoxRole.isDisabled = isDisabled;
			}
		}

		public GreyFoxRole NewPlaceHolder()
		{
			GreyFoxRole greyFoxRole = new GreyFoxRole();
			greyFoxRole.iD = iD;
			greyFoxRole.isPlaceHolder = true;
			greyFoxRole.isSynced = true;
			return greyFoxRole;
		}

		public static GreyFoxRole NewPlaceHolder(int iD)
		{
			GreyFoxRole greyFoxRole = new GreyFoxRole();
			greyFoxRole.iD = iD;
			greyFoxRole.isPlaceHolder = true;
			greyFoxRole.isSynced = true;
			return greyFoxRole;
		}

		#endregion

		#region IComparable Methods

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		int IComparable.CompareTo(object obj)
		{
			GreyFoxRole greyFoxRole = (GreyFoxRole) obj;
			return this.iD - greyFoxRole.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(GreyFoxRole greyFoxRole)
		{
			return this.iD - greyFoxRole.iD;
		}

		public override bool Equals(object greyFoxRole)
		{
			if(greyFoxRole == null)
				return false;

			return Equals((GreyFoxRole) greyFoxRole);
		}

		public bool Equals(GreyFoxRole greyFoxRole)
		{
			if(greyFoxRole == null)
				return false;

			return this.iD == greyFoxRole.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

		//--- Begin Custom Code ---
																																																																																																																										
		public override string ToString()
		{
			return Name;
		}


		//--- End Custom Code ---
	}
}
