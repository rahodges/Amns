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
using Amns.GreyFox.People;

namespace Amns.GreyFox.Security
{
	/// <summary>
	/// Summary of User
	/// </summary>
	public class GreyFoxUser : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal string userName;
		internal bool isDisabled;
		internal DateTime loginDate;
		internal int loginCount;
		internal string loginPassword;
		internal GreyFoxContact contact;
		internal GreyFoxRoleCollection roles;
		internal string activationID;

		#endregion

		#region Public Properties

		/// <summary>
		/// GreyFoxUser Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the GreyFoxUser as a Placeholder. Placeholders only contain 
		/// a GreyFoxUser ID. Record late-binds data when it is accessed.
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
		/// Username
		/// </summary>
		public string UserName
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return userName;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= userName == value;
					userName = value;
				}
			}
		}

		/// <summary>
		/// Disables the user account on the system. Does not affect users already 
		/// logged in.
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

		/// <summary>
		/// </summary>
		public DateTime LoginDate
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return loginDate;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= loginDate == value;
					loginDate = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public int LoginCount
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return loginCount;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= loginCount == value;
					loginCount = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string LoginPassword
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return loginPassword;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= loginPassword == value;
					loginPassword = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxContact Contact
		{
			get
			{
				lock(this)
				{
					EnsurePreLoad();
					return contact;
				}
			}
			set
			{
				lock(this)
				{
					EnsurePreLoad();
					if(value == null)
					{
						if(contact == null)
						{
							return;
						}
						else
						{
							contact = value;
							isSynced = false;
						}
					}
					else
					{
						if(value.TableName != "sysGlobal_Contacts") throw(new Exception("Cannot set Contact. Table names mismatched."));
						if(contact != null && value.ID == contact.ID)
						{
							return; 
						}
						else
						{
							contact = value;
							isSynced = false;
						}
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxRoleCollection Roles
		{
			get
			{
				lock(this)
				{
					EnsurePreLoad();
					if(roles == null)
					{
						GreyFoxUserManager.FillRoles(this);
						roles.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
					}
					return roles;
				}
			}
			set
			{
				lock(this)
				{
					EnsurePreLoad();
					if(!object.Equals(roles, value))
					{
						roles = value;
						roles.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public string ActivationID
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return activationID;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= activationID == value;
					activationID = value;
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of GreyFoxUser.
		/// </summary>
		public GreyFoxUser()
		{
			activationID = string.Empty;
		}

		public GreyFoxUser(int id)
		{
			this.iD = id;
			lock(this)
			{
				isSynced = GreyFoxUserManager._fill(this);
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
				GreyFoxUserManager._fill(this);
				isPlaceHolder = false;
			}
		}

		/// <summary>
		/// Saves the GreyFoxUser object state to the database.
		/// </summary>
		public int Save()
		{
			lock(this)
			{
				if(contact != null)
					contact.Save();
				if(roles != null)
					foreach(GreyFoxRole item in roles)
						item.Save();

				if(isSynced)
					return iD;

				if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
				if(iD == 0)
					iD = GreyFoxUserManager._insert(this);
				else
					GreyFoxUserManager._update(this);
				isSynced = iD != -1;
			}
			return iD;
		}

		public void Delete()
		{
			GreyFoxUserManager._delete(this.iD);
			this.iD = 0;
			isSynced = false;
			contact.Delete();
		}
		/// <summary>
		/// Duplicates GreyFoxUser object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxUser object reflecting the replicated GreyFoxUser object.</returns>
		public GreyFoxUser Duplicate()
		{
			lock(this)
			{
			GreyFoxUser clonedGreyFoxUser = this.Clone();

			// Insert must be called after children are replicated!
			clonedGreyFoxUser.iD = GreyFoxUserManager._insert(clonedGreyFoxUser);
			clonedGreyFoxUser.isSynced = true;
			return clonedGreyFoxUser;
			}
		}

		/// <summary>
		/// Overwrites and existing GreyFoxUser object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			GreyFoxUserManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones GreyFoxUser object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxUser object reflecting the replicated GreyFoxUser object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones GreyFoxUser object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxUser object reflecting the replicated GreyFoxUser object.</returns>
		public GreyFoxUser Clone()
		{
			lock(this)
			{
			GreyFoxUser clonedGreyFoxUser = new GreyFoxUser();
			clonedGreyFoxUser.iD = iD;
			clonedGreyFoxUser.isSynced = isSynced;
			clonedGreyFoxUser.userName = userName;
			clonedGreyFoxUser.isDisabled = isDisabled;
			clonedGreyFoxUser.loginDate = loginDate;
			clonedGreyFoxUser.loginCount = loginCount;
			clonedGreyFoxUser.loginPassword = loginPassword;
			clonedGreyFoxUser.activationID = activationID;


			if(contact != null)
				clonedGreyFoxUser.contact = contact.Duplicate("sysGlobal_Contacts");

			if(roles != null)
				clonedGreyFoxUser.roles = roles.Clone();

			return clonedGreyFoxUser;
			}
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxUser.
		/// </summary>
		/// <returns> A new GreyFoxUser object reflecting the cloned GreyFoxUser object.</returns>
		public GreyFoxUser Copy()
		{
			GreyFoxUser greyFoxUser = new GreyFoxUser();
			CopyTo(greyFoxUser);
			return greyFoxUser;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxUser.
		/// </summary>
		/// <returns> A new GreyFoxUser object reflecting the cloned GreyFoxUser object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxUser from its children.</param>
		public GreyFoxUser Copy(bool isolation)
		{
			GreyFoxUser greyFoxUser = new GreyFoxUser();
			CopyTo(greyFoxUser, isolation);
			return greyFoxUser;
		}

		/// <summary>
		/// Deep copies the current GreyFoxUser to another instance of GreyFoxUser.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="GreyFoxUser">The GreyFoxUser to copy to.</param>
		public void CopyTo(GreyFoxUser greyFoxUser)
		{
			CopyTo(greyFoxUser, false);
		}

		/// <summary>
		/// Deep copies the current GreyFoxUser to another instance of GreyFoxUser.
		/// </summary>
		/// <param name="GreyFoxUser">The GreyFoxUser to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxUser from its children.</param>
		public void CopyTo(GreyFoxUser greyFoxUser, bool isolation)
		{
			lock(this)
			{
				greyFoxUser.iD = iD;
				greyFoxUser.isPlaceHolder = isPlaceHolder;
				greyFoxUser.isSynced = isSynced;
				greyFoxUser.userName = userName;
				greyFoxUser.isDisabled = isDisabled;
				greyFoxUser.loginDate = loginDate;
				greyFoxUser.loginCount = loginCount;
				greyFoxUser.loginPassword = loginPassword;
				if(contact != null)
				{
					if(isolation)
					{
						greyFoxUser.contact = contact.NewPlaceHolder();
					}
					else
					{
						greyFoxUser.contact = contact.Copy(false);
					}
				}
				if(roles != null)
				{
					if(isolation)
					{
						greyFoxUser.roles = roles.Copy(true);
					}
					else
					{
						greyFoxUser.roles = roles.Copy(false);
					}
				}
				greyFoxUser.activationID = activationID;
			}
		}

		public GreyFoxUser NewPlaceHolder()
		{
			GreyFoxUser greyFoxUser = new GreyFoxUser();
			greyFoxUser.iD = iD;
			greyFoxUser.isPlaceHolder = true;
			greyFoxUser.isSynced = true;
			return greyFoxUser;
		}

		public static GreyFoxUser NewPlaceHolder(int iD)
		{
			GreyFoxUser greyFoxUser = new GreyFoxUser();
			greyFoxUser.iD = iD;
			greyFoxUser.isPlaceHolder = true;
			greyFoxUser.isSynced = true;
			return greyFoxUser;
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
			GreyFoxUser greyFoxUser = (GreyFoxUser) obj;
			return this.iD - greyFoxUser.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(GreyFoxUser greyFoxUser)
		{
			return this.iD - greyFoxUser.iD;
		}

		public override bool Equals(object greyFoxUser)
		{
			if(greyFoxUser == null)
				return false;

			return Equals((GreyFoxUser) greyFoxUser);
		}

		public bool Equals(GreyFoxUser greyFoxUser)
		{
			if(greyFoxUser == null)
				return false;

			return this.iD == greyFoxUser.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

		//--- Begin Custom Code ---
																																																																																																																																																																																																																																																																																																																																																																																																																																			
		public bool IsInRole(int roleID)
		{		
			foreach(GreyFoxRole role in roles)
				if(role.ID == roleID)
					return true;
			return false;
		}

		public bool IsInRole(string roleName)
		{
			foreach(GreyFoxRole role in roles)
				if(role.Name == roleName)
					return true;
			return false;
		}
																																								
		public override string ToString()
		{
			if(!isPlaceHolder)
				return UserName;

			if(iD == 0)
				return base.ToString();
				
			return iD.ToString();			
		}

        /// <summary>
        /// Randomizes the password.
        /// </summary>
        /// <param name="length">Specified length for password.</param>
        /// <returns></returns>
		public string RandomizePassword(int length)
		{
			EnsurePreLoad();
			loginPassword = GreyFoxPassword.CreateRandomPassword(length);
			isSynced = false;
			return loginPassword;
		}

        /// <summary>
        /// Returns true if the password has been encrypted.
        /// </summary>
        /// <returns>True if password is encrypted.</returns>
		public bool IsEncrypted()
		{
			return GreyFoxPassword.CheckEncryption(this.LoginPassword);
		}

        /// <summary>
        /// Returns a decrypted version of the password.
        /// </summary>
        /// <returns>Decoded password.</returns>
		public string Decrypt()
		{
			return GreyFoxPassword.DecodePassword(this.LoginPassword);
		}

        /// <summary>
        /// Encrypts the currnt password.
        /// </summary>
		public void Encrypt()
		{
			this.LoginPassword = GreyFoxPassword.EncodePassword(this.LoginPassword);
		}

        /// <summary>
        /// Returns a default username. This does not check the username
        /// created against the database. If the user does not have a
        /// contact or a first name, a random username is created by
        /// returning a hashcode.
        /// </summary>
        /// <returns>Returns a username.</returns>
        public string CreateUserName()
        {
            if (this.contact != null)
            {
                if (contact.firstName.Length > 0)
                {
                    return contact.firstName.Substring(0, 1) +
                        contact.lastName;
                }
            }

            return this.GetHashCode().ToString();
        }

		//--- End Custom Code ---
	}
}
