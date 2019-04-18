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
	public class GreyFoxSetting : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal string name;
		internal string settingValue;
		internal GreyFoxSetting parent;
		internal GreyFoxRole modifyRole;
		internal bool isSystemSetting;

		#endregion

		#region Public Properties

		/// <summary>
		/// GreyFoxSetting Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the GreyFoxSetting as a Placeholder. Placeholders only contain 
		/// a GreyFoxSetting ID. Record late-binds data when it is accessed.
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
		public string SettingValue
		{
			get
			{
				EnsurePreLoad();
				return settingValue;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= settingValue == value;
				settingValue = value;
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxSetting Parent
		{
			get
			{
				EnsurePreLoad();
				return parent;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(parent == null)
					{
						return;
					}
					else
					{
						parent = value;
						isSynced = false;
					}
				}
				else
				{
					if(parent != null && value.ID == parent.ID)
					{
						return; 
					}
					else
					{
						parent = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxRole ModifyRole
		{
			get
			{
				EnsurePreLoad();
				return modifyRole;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(modifyRole == null)
					{
						return;
					}
					else
					{
						modifyRole = value;
						isSynced = false;
					}
				}
				else
				{
					if(modifyRole != null && value.ID == modifyRole.ID)
					{
						return; 
					}
					else
					{
						modifyRole = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public bool IsSystemSetting
		{
			get
			{
				EnsurePreLoad();
				return isSystemSetting;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= isSystemSetting == value;
				isSystemSetting = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of GreyFoxSetting.
		/// </summary>
		public GreyFoxSetting()
		{
		}

		public GreyFoxSetting(int id)
		{
			this.iD = id;
			isSynced = GreyFoxSettingManager._fill(this);
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

			GreyFoxSettingManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the GreyFoxSetting object state to the database.
		/// </summary>
		public int Save()
		{

			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = GreyFoxSettingManager._insert(this);
			else
				GreyFoxSettingManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			GreyFoxSettingManager._delete(this.iD);
			this.iD = 0;
			isSynced = false;
		}
		/// <summary>
		/// Duplicates GreyFoxSetting object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxSetting object reflecting the replicated GreyFoxSetting object.</returns>
		public GreyFoxSetting Duplicate()
		{
			GreyFoxSetting clonedGreyFoxSetting = this.Clone();

			// Insert must be called after children are replicated!
			clonedGreyFoxSetting.iD = GreyFoxSettingManager._insert(clonedGreyFoxSetting);
			clonedGreyFoxSetting.isSynced = true;
			return clonedGreyFoxSetting;
		}

		/// <summary>
		/// Overwrites and existing GreyFoxSetting object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			GreyFoxSettingManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones GreyFoxSetting object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxSetting object reflecting the replicated GreyFoxSetting object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones GreyFoxSetting object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxSetting object reflecting the replicated GreyFoxSetting object.</returns>
		public GreyFoxSetting Clone()
		{
			GreyFoxSetting clonedGreyFoxSetting = new GreyFoxSetting();
			clonedGreyFoxSetting.iD = iD;
			clonedGreyFoxSetting.isSynced = isSynced;
			clonedGreyFoxSetting.name = name;
			clonedGreyFoxSetting.settingValue = settingValue;
			clonedGreyFoxSetting.isSystemSetting = isSystemSetting;


			if(parent != null)
				clonedGreyFoxSetting.parent = parent;

			if(modifyRole != null)
				clonedGreyFoxSetting.modifyRole = modifyRole;

			return clonedGreyFoxSetting;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxSetting.
		/// </summary>
		/// <returns> A new GreyFoxSetting object reflecting the cloned GreyFoxSetting object.</returns>
		public GreyFoxSetting Copy()
		{
			GreyFoxSetting greyFoxSetting = new GreyFoxSetting();
			CopyTo(greyFoxSetting);
			return greyFoxSetting;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxSetting.
		/// </summary>
		/// <returns> A new GreyFoxSetting object reflecting the cloned GreyFoxSetting object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxSetting from its children.</param>
		public GreyFoxSetting Copy(bool isolation)
		{
			GreyFoxSetting greyFoxSetting = new GreyFoxSetting();
			CopyTo(greyFoxSetting, isolation);
			return greyFoxSetting;
		}

		/// <summary>
		/// Deep copies the current GreyFoxSetting to another instance of GreyFoxSetting.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="GreyFoxSetting">The GreyFoxSetting to copy to.</param>
		public void CopyTo(GreyFoxSetting greyFoxSetting)
		{
			CopyTo(greyFoxSetting, false);
		}

		/// <summary>
		/// Deep copies the current GreyFoxSetting to another instance of GreyFoxSetting.
		/// </summary>
		/// <param name="GreyFoxSetting">The GreyFoxSetting to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxSetting from its children.</param>
		public void CopyTo(GreyFoxSetting greyFoxSetting, bool isolation)
		{
			greyFoxSetting.iD = iD;
			greyFoxSetting.isPlaceHolder = isPlaceHolder;
			greyFoxSetting.isSynced = isSynced;
			greyFoxSetting.name = name;
			greyFoxSetting.settingValue = settingValue;
			if(parent != null)
			{
				if(isolation)
				{
					greyFoxSetting.parent = parent.NewPlaceHolder();
				}
				else
				{
					greyFoxSetting.parent = parent.Copy(false);
				}
			}
			if(modifyRole != null)
			{
				if(isolation)
				{
					greyFoxSetting.modifyRole = modifyRole.NewPlaceHolder();
				}
				else
				{
					greyFoxSetting.modifyRole = modifyRole.Copy(false);
				}
			}
			greyFoxSetting.isSystemSetting = isSystemSetting;
		}

		public GreyFoxSetting NewPlaceHolder()
		{
			GreyFoxSetting greyFoxSetting = new GreyFoxSetting();
			greyFoxSetting.iD = iD;
			greyFoxSetting.isPlaceHolder = true;
			greyFoxSetting.isSynced = true;
			return greyFoxSetting;
		}

		public static GreyFoxSetting NewPlaceHolder(int iD)
		{
			GreyFoxSetting greyFoxSetting = new GreyFoxSetting();
			greyFoxSetting.iD = iD;
			greyFoxSetting.isPlaceHolder = true;
			greyFoxSetting.isSynced = true;
			return greyFoxSetting;
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
			GreyFoxSetting greyFoxSetting = (GreyFoxSetting) obj;
			return this.iD - greyFoxSetting.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(GreyFoxSetting greyFoxSetting)
		{
			return this.iD - greyFoxSetting.iD;
		}

		public override bool Equals(object greyFoxSetting)
		{
			if(greyFoxSetting == null)
				return false;

			return Equals((GreyFoxSetting) greyFoxSetting);
		}

		public bool Equals(GreyFoxSetting greyFoxSetting)
		{
			if(greyFoxSetting == null)
				return false;

			return this.iD == greyFoxSetting.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

	}
}
