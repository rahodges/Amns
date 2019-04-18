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
using Amns.GreyFox;

namespace Amns.GreyFox.People
{
	/// <summary>
	/// Summary of Contact.
	/// </summary>
	public class GreyFoxContact : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal string tableName;
		internal string displayName;
		internal string prefix;
		internal string firstName;
		internal string middleName;
		internal string lastName;
		internal bool suffixCommaEnabled;
		internal string suffix;
		internal string title;
		internal string validationMemo;
		internal GreyFoxContactValidationFlag validationFlags;
		internal string address1;
		internal string address2;
		internal string city;
		internal string stateProvince;
		internal string country;
		internal string postalCode;
		internal string homePhone;
		internal string workPhone;
		internal string fax;
		internal string pager;
		internal string mobilePhone;
		internal string email1;
		internal string email2;
		internal string url;
		internal string businessName;
		internal string memoText;
		internal DateTime birthDate;
		internal GreyFoxContactMethod contactMethod;

		#endregion

		#region Public Properties

		/// <summary>
		/// GreyFoxContact Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the GreyFoxContact as a Placeholder. Placeholders only contain 
		/// a GreyFoxContact ID. Record late-binds data when it is accessed.
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
		/// Identifies the table the GreyFoxContact belongs to. Readonly. 
		/// Use Replicate to replicate the object into a different table.
		/// </summary>
		public string TableName
		{
			get
			{
				return tableName;
			}
		}

		/// <summary>
		/// </summary>
		public string DisplayName
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return displayName;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= displayName == value;
					displayName = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Prefix
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return prefix;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= prefix == value;
					prefix = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string FirstName
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return firstName;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= firstName == value;
					firstName = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string MiddleName
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return middleName;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= middleName == value;
					middleName = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string LastName
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return lastName;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= lastName == value;
					lastName = value;
				}
			}
		}

		/// <summary>
		/// Enables a comma before the suffix field. For example, "John Doe I" vs. 
		/// "John Doe, I". This is based on the contact's preference.
		/// </summary>
		public bool SuffixCommaEnabled
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return suffixCommaEnabled;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= suffixCommaEnabled == value;
					suffixCommaEnabled = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Suffix
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return suffix;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= suffix == value;
					suffix = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Title
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return title;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= title == value;
					title = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string ValidationMemo
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return validationMemo;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= validationMemo == value;
					validationMemo = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxContactValidationFlag ValidationFlags
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return validationFlags;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= validationFlags == value;
					validationFlags = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Address1
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return address1;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= address1 == value;
					address1 = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Address2
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return address2;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= address2 == value;
					address2 = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string City
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return city;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= city == value;
					city = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string StateProvince
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return stateProvince;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= stateProvince == value;
					stateProvince = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Country
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return country;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= country == value;
					country = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string PostalCode
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return postalCode;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= postalCode == value;
					postalCode = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string HomePhone
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return homePhone;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= homePhone == value;
					homePhone = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string WorkPhone
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return workPhone;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= workPhone == value;
					workPhone = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Fax
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return fax;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= fax == value;
					fax = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Pager
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return pager;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= pager == value;
					pager = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string MobilePhone
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return mobilePhone;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= mobilePhone == value;
					mobilePhone = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Email1
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return email1;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= email1 == value;
					email1 = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Email2
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return email2;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= email2 == value;
					email2 = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Url
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return url;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= url == value;
					url = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string BusinessName
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return businessName;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= businessName == value;
					businessName = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string MemoText
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return memoText;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= memoText == value;
					memoText = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public DateTime BirthDate
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return birthDate;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= birthDate == value;
					birthDate = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxContactMethod ContactMethod
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return contactMethod;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= contactMethod == value;
					contactMethod = value;
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of GreyFoxContact.
		/// </summary>
		public GreyFoxContact()
		{
		}

		public GreyFoxContact(string tableName)
		{
			this.tableName = tableName;
			isSynced = false;
			displayName = string.Empty;
			prefix = string.Empty;
			firstName = string.Empty;
			middleName = string.Empty;
			lastName = string.Empty;
			suffix = string.Empty;
			title = string.Empty;
			validationMemo = string.Empty;
			address1 = string.Empty;
			address2 = string.Empty;
			city = string.Empty;
			stateProvince = string.Empty;
			country = string.Empty;
			postalCode = string.Empty;
			homePhone = string.Empty;
			workPhone = string.Empty;
			fax = string.Empty;
			pager = string.Empty;
			mobilePhone = string.Empty;
			email1 = string.Empty;
			email2 = string.Empty;
			url = string.Empty;
			businessName = string.Empty;
			memoText = string.Empty;
			birthDate = new DateTime(1800, 1, 1);
		}

		public GreyFoxContact(string tableName, int id)
		{
			this.iD = id;
			this.tableName = tableName;
			lock(this)
			{
				isSynced = GreyFoxContactManager._fill(this);
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
				GreyFoxContactManager._fill(this);
				isPlaceHolder = false;
			}
		}

		/// <summary>
		/// Saves the GreyFoxContact object state to the database.
		/// </summary>
		public int Save()
		{
			lock(this)
			{
				if(isSynced)
					return iD;

				if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
				if(iD == 0)
					iD = GreyFoxContactManager._insert(this);
				else
					GreyFoxContactManager._update(this);
				isSynced = iD != -1;
			}
			return iD;
		}

		public void Delete()
		{
			GreyFoxContactManager._delete(this.tableName, this.iD);
			this.iD = 0;
			isSynced = false;
		}
		/// <summary>
		/// Duplicates GreyFoxContact object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxContact object reflecting the replicated GreyFoxContact object.</returns>
		public GreyFoxContact Duplicate(string tableName)
		{
			lock(this)
			{
			GreyFoxContact clonedGreyFoxContact = this.Clone();
			clonedGreyFoxContact.tableName = tableName;

			// Insert must be called after children are replicated!
			clonedGreyFoxContact.iD = GreyFoxContactManager._insert(clonedGreyFoxContact);
			clonedGreyFoxContact.isSynced = true;
			return clonedGreyFoxContact;
			}
		}

		/// <summary>
		/// Overwrites and existing GreyFoxContact object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			GreyFoxContactManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones GreyFoxContact object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxContact object reflecting the replicated GreyFoxContact object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones GreyFoxContact object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxContact object reflecting the replicated GreyFoxContact object.</returns>
		public GreyFoxContact Clone()
		{
			lock(this)
			{
			GreyFoxContact clonedGreyFoxContact = new GreyFoxContact();
			clonedGreyFoxContact.iD = iD;
			clonedGreyFoxContact.tableName = tableName;
			clonedGreyFoxContact.isSynced = isSynced;
			clonedGreyFoxContact.displayName = displayName;
			clonedGreyFoxContact.prefix = prefix;
			clonedGreyFoxContact.firstName = firstName;
			clonedGreyFoxContact.middleName = middleName;
			clonedGreyFoxContact.lastName = lastName;
			clonedGreyFoxContact.suffixCommaEnabled = suffixCommaEnabled;
			clonedGreyFoxContact.suffix = suffix;
			clonedGreyFoxContact.title = title;
			clonedGreyFoxContact.validationMemo = validationMemo;
			clonedGreyFoxContact.address1 = address1;
			clonedGreyFoxContact.address2 = address2;
			clonedGreyFoxContact.city = city;
			clonedGreyFoxContact.stateProvince = stateProvince;
			clonedGreyFoxContact.country = country;
			clonedGreyFoxContact.postalCode = postalCode;
			clonedGreyFoxContact.homePhone = homePhone;
			clonedGreyFoxContact.workPhone = workPhone;
			clonedGreyFoxContact.fax = fax;
			clonedGreyFoxContact.pager = pager;
			clonedGreyFoxContact.mobilePhone = mobilePhone;
			clonedGreyFoxContact.email1 = email1;
			clonedGreyFoxContact.email2 = email2;
			clonedGreyFoxContact.url = url;
			clonedGreyFoxContact.businessName = businessName;
			clonedGreyFoxContact.memoText = memoText;
			clonedGreyFoxContact.birthDate = birthDate;

			clonedGreyFoxContact.validationFlags = validationFlags;
			clonedGreyFoxContact.contactMethod = contactMethod;

			return clonedGreyFoxContact;
			}
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxContact.
		/// </summary>
		/// <returns> A new GreyFoxContact object reflecting the cloned GreyFoxContact object.</returns>
		public GreyFoxContact Copy()
		{
			GreyFoxContact greyFoxContact = new GreyFoxContact();
			CopyTo(greyFoxContact);
			return greyFoxContact;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxContact.
		/// </summary>
		/// <returns> A new GreyFoxContact object reflecting the cloned GreyFoxContact object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxContact from its children.</param>
		public GreyFoxContact Copy(bool isolation)
		{
			GreyFoxContact greyFoxContact = new GreyFoxContact();
			CopyTo(greyFoxContact, isolation);
			return greyFoxContact;
		}

		/// <summary>
		/// Deep copies the current GreyFoxContact to another instance of GreyFoxContact.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="GreyFoxContact">The GreyFoxContact to copy to.</param>
		public void CopyTo(GreyFoxContact greyFoxContact)
		{
			CopyTo(greyFoxContact, false);
		}

		/// <summary>
		/// Deep copies the current GreyFoxContact to another instance of GreyFoxContact.
		/// </summary>
		/// <param name="GreyFoxContact">The GreyFoxContact to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxContact from its children.</param>
		public void CopyTo(GreyFoxContact greyFoxContact, bool isolation)
		{
			lock(this)
			{
				greyFoxContact.iD = iD;
				greyFoxContact.tableName = tableName;
				greyFoxContact.isPlaceHolder = isPlaceHolder;
				greyFoxContact.isSynced = isSynced;
				greyFoxContact.displayName = displayName;
				greyFoxContact.prefix = prefix;
				greyFoxContact.firstName = firstName;
				greyFoxContact.middleName = middleName;
				greyFoxContact.lastName = lastName;
				greyFoxContact.suffixCommaEnabled = suffixCommaEnabled;
				greyFoxContact.suffix = suffix;
				greyFoxContact.title = title;
				greyFoxContact.validationMemo = validationMemo;
				greyFoxContact.validationFlags = validationFlags;
				greyFoxContact.address1 = address1;
				greyFoxContact.address2 = address2;
				greyFoxContact.city = city;
				greyFoxContact.stateProvince = stateProvince;
				greyFoxContact.country = country;
				greyFoxContact.postalCode = postalCode;
				greyFoxContact.homePhone = homePhone;
				greyFoxContact.workPhone = workPhone;
				greyFoxContact.fax = fax;
				greyFoxContact.pager = pager;
				greyFoxContact.mobilePhone = mobilePhone;
				greyFoxContact.email1 = email1;
				greyFoxContact.email2 = email2;
				greyFoxContact.url = url;
				greyFoxContact.businessName = businessName;
				greyFoxContact.memoText = memoText;
				greyFoxContact.birthDate = birthDate;
				greyFoxContact.contactMethod = contactMethod;
			}
		}

		public GreyFoxContact NewPlaceHolder()
		{
			GreyFoxContact greyFoxContact = new GreyFoxContact();
			greyFoxContact.iD = iD;
			greyFoxContact.tableName = tableName;
			greyFoxContact.isPlaceHolder = true;
			greyFoxContact.isSynced = true;
			return greyFoxContact;
		}

		public static GreyFoxContact NewPlaceHolder(string tableName, int iD)
		{
			GreyFoxContact greyFoxContact = new GreyFoxContact();
			greyFoxContact.iD = iD;
			greyFoxContact.tableName = tableName;
			greyFoxContact.isPlaceHolder = true;
			greyFoxContact.isSynced = true;
			return greyFoxContact;
		}

		#endregion

		#region IComparable Methods

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		int IComparable.CompareTo(object obj)
		{
			GreyFoxContact greyFoxContact = (GreyFoxContact) obj;
			return this.iD - greyFoxContact.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(GreyFoxContact greyFoxContact)
		{
			return this.iD - greyFoxContact.iD;
		}

		public override bool Equals(object greyFoxContact)
		{
			if(greyFoxContact == null)
				return false;

			return Equals((GreyFoxContact) greyFoxContact);
		}

		public bool Equals(GreyFoxContact greyFoxContact)
		{
			if(greyFoxContact == null)
				return false;

			return this.iD == greyFoxContact.iD &&
				this.tableName == greyFoxContact.tableName;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode() +
				tableName.GetHashCode();
		}

		#endregion

		//--- Begin Custom Code ---
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																														
		#region Parse Name

		public void ParseName(string name)
		{
			// Ensure that the values are loaded.
			this.EnsurePreLoad();

			// cache old name values to make "sync" property work correctly.
			string oldPrefix = prefix;
			string oldLastName = lastName;
			string oldFirstName = firstName;
			string oldMiddleName = middleName;
			string oldSuffix = suffix;
			bool oldSuffixCommaEnabled = suffixCommaEnabled;

			NameParser parser = new NameParser();
			parser.Parse(name);
            prefix = parser.Prefix;
			firstName = parser.First;
			middleName = parser.Middle;
			lastName = parser.Last;
			suffixCommaEnabled = parser.SuffixCommaEnabled;
			suffix = parser.Suffix;
			
			// Update isSynced to reflect any changes
			isSynced = isSynced &&
				(prefix == oldPrefix) &&
				(lastName == oldLastName) &&
				(firstName == oldFirstName) &&
				(middleName == oldMiddleName) &&
				(suffixCommaEnabled == oldSuffixCommaEnabled) &&
				(suffix == oldSuffix);
		}

		#endregion

		#region Parse Address

		public void ParseAddress(string address1, string address2, string city, 
			string stateProvince, string postalCode, string country)
		{
			ParseAddress1(address1);
			ParseAddress2(address2);
			ParseCity(city);
			ParseStateProvince(stateProvince);
			ParsePostalCode(postalCode);
			ParseCountry(country);
		}

		#region Address1

		public void ParseAddress1(string text)
		{
			Address1 = Amns.GreyFox.Text.AddressCorrector.Parse(text);
		}

		#endregion

		#region Address2
        
		public void ParseAddress2(string text)
		{
			Address2 = Amns.GreyFox.Text.AddressCorrector.Parse(text);
		}

		#endregion

		#region City

		public void ParseCity(string text)
		{
			City = Amns.GreyFox.Text.AddressCorrector.Parse(text, false);
		}

		#endregion

		#region State/Province

		public void ParseStateProvince(string text)
		{
			text = text.Trim();

			//
			// Change two letter postal codes into uppercase
			//
			if(text.Length == 2)
			{
				StateProvince = text.ToUpper();
				return;
			}

			//
			// Search for states and convert the state to the two letter postal code
			//
			for(int x = 0; x <= States.GetUpperBound(0); x++)
			{
				if(States[x,0].ToLower() == text.ToLower())
				{
					StateProvince = States[x,1];
					Country = "United States";
					return;
				}
			}

			//
			// Search for provinces and convert the province to the two letter postal code
			//
			for(int x = 0; x <= CanadianProvinces.GetUpperBound(0); x++)
			{
				if(CanadianProvinces[x,0].ToLower() == text.ToLower())
				{
					StateProvince = CanadianProvinces[x,1];
					Country = "Canada";
					return;
				}
			}

			StateProvince = text;
		}

		#endregion

		#region Postal Code

		public void ParsePostalCode(string text)
		{
			PostalCode = text;
		}

		#endregion

		#region Country

		public void ParseCountry(string text)
		{
			if(text == string.Empty)
			{
				Country = "United States";
				return;
			}

			//
			// Search for countries and misspellings.
			//
			for(int x = 0; x <= Countries.GetUpperBound(0); x++)
			{
				if(Countries[x,1].ToLower() == text.ToLower())
				{
					Country = Countries[x,0];
					return;
				}

				if(Countries[x,2].ToLower() == text.ToLower())
				{
					Country = Countries[x,0];
					return;
				}
			}

			Country = text;
		}

		#endregion		

		#endregion

		#region Parse Coms

		public void ParsePhones(string homePhone, string workPhone, string fax, string pager, string mobilePhone)
		{
			this.HomePhone = parsePhone(homePhone);
			this.WorkPhone = parsePhone(workPhone);
			this.Fax = parsePhone(fax);
			this.Pager = parsePhone(pager);
			this.MobilePhone = parsePhone(mobilePhone);
		}

		private string parsePhone(string phoneNumber)
		{
			phoneNumber = phoneNumber.Trim();

			switch(Country)
			{
				case "":
					goto case "United States";
				case "United States":
					string nums = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "");
                    if(nums.Length == 10)
						return "(" + nums.Substring(0, 3) + ")" + " " + 
							nums.Substring(3, 3) + "-" + 
							nums.Substring(6, 4);
					else if(nums.Length == 7)
						return nums.Substring(0, 3) + "-" + 
							nums.Substring(3, 4);
					return phoneNumber;
				default:
					return phoneNumber;
			}
		}

		#endregion

		#region Parse Data

		public static readonly string[,] Countries = new string[,]
			{
				{"Andorra", "Andorra", "AD"},
				{"Andorra", "Andrra", "AD"},
				{"United Arab Emirates", "United Arab Emirates", "AE"},
				{"Afghanistan", "Afghanistan", "AF"},
				{"Antigua and Barbuda", "Antigua and Barbuda", "AG"},
				{"Anguilla", "Anguilla", "AI"},
				{"Albania", "Albania", "AL"},
				{"Armenia", "Armenia", "AM"},
				{"Netherland Antilles", "Netherland Antilles", "AN"},
				{"Angola", "Angola", "AO"},
				{"Antarctica", "Antarctica", "AQ"},
				{"Argentina", "Argentina", "AR"},
				{"American Samoa", "American Samoa", "AS"},
				{"Austria", "Austria", "AT"},
				{"Australia", "Australia", "AU"},
				{"Aruba", "Aruba", "AW"},
				{"Azerbaidjan", "Azerbaidjan", "AZ"},
				{"Bosnia-Herzegovina", "Bosnia-Herzegovina", "BA"},
				{"Barbados", "Barbados", "BB"},
				{"Bangladesh", "Bangladesh", "BD"},
				{"Belgium", "Belgium", "BE"},
				{"Burkina Faso", "Burkina Faso", "BF"},
				{"Bulgaria", "Bulgaria", "BG"},
				{"Bahrain", "Bahrain", "BH"},
				{"Burundi", "Burundi", "BI"},
				{"Benin", "Benin", "BJ"},
				{"Bermuda", "Bermuda", "BM"},
				{"Brunei Darussalam", "Brunei Darussalam", "BN"},
				{"Bolivia", "Bolivia", "BO"},
				{"Brazil", "Brazil", "BR"},
				{"Bahamas", "Bahamas", "BS"},
				{"Buthan", "Buthan", "BT"},
				{"Bouvet Island", "Bouvet Island", "BV"},
				{"Botswana", "Botswana", "BW"},
				{"Belarus", "Belarus", "BY"},
				{"Belize", "Belize", "BZ"},
				{"Canada", "Canada", "CA"},
				{"Cocos (Keeling) Isl.", "Cocos (Keeling) Isl.", "CC"},
				{"Central African Rep.", "Central African Rep.", "CF"},
				{"Congo", "Congo", "CG"},
				{"Switzerland", "Switzerland", "CH"},
				{"Ivory Coast", "Ivory Coast", "CI"},
				{"Cook Islands", "Cook Islands", "CK"},
				{"Chile", "Chile", "CL"},
				{"Cameroon", "Cameroon", "CM"},
				{"China", "China", "CN"},
				{"Colombia", "Colombia", "CO"},
				{"Costa Rica", "Costa Rica", "CR"},
				{"Czechoslovakia", "Czechoslovakia", "CS"},
				{"Cuba", "Cuba", "CU"},
				{"Cape Verde", "Cape Verde", "CV"},
				{"Christmas Island", "Christmas Island", "CX"},
				{"Cyprus", "Cyprus", "CY"},
				{"Czech Republic", "Czech Republic", "CZ"},
				{"Germany", "Germany", "DE"},
				{"Djibouti", "Djibouti", "DJ"},
				{"Denmark", "Denmark", "DK"},
				{"Dominica", "Dominica", "DM"},
				{"Dominican Republic", "Dominican Republic", "DO"},
				{"Algeria", "Algeria", "DZ"},
				{"Ecuador", "Ecuador", "EC"},
				{"Estonia", "Estonia", "EE"},
				{"Egypt", "Egypt", "EG"},
				{"Western Sahara", "Western Sahara", "EH"},
				{"Spain", "Spain", "ES"},
				{"Ethiopia", "Ethiopia", "ET"},
				{"Finland", "Finland", "FI"},
				{"Fiji", "Fiji", "FJ"},
				{"Falkland Isl.(Malvinas)", "Falkland Isl.(Malvinas)", "FK"},
				{"Micronesia", "Micronesia", "FM"},
				{"Faroe Islands", "Faroe Islands", "FO"},
				{"France", "France", "FR"},
				{"France", "Franc", "FR"},
				{"France", "Frnc", "FR"},
				{"France (European Ter.)", "France (European Ter.)", "FX"},
				{"Gabon", "Gabon", "GA"},
				{"Great Britain (UK)", "Great Britain (UK)", "GB"},
				{"Grenada", "Grenada", "GD"},
				{"Georgia", "Georgia", "GE"},
				{"Ghana", "Ghana", "GH"},
				{"Gibraltar", "Gibraltar", "GI"},
				{"Greenland", "Greenland", "GL"},
				{"Guadeloupe (Fr.)", "Guadeloupe (Fr.)", "GP"},
				{"Equatorial Guinea", "Equatorial Guinea", "GQ"},
				{"Guyana (Fr.)", "Guyana (Fr.)", "GF"},
				{"Guyana (Fr.)", "Guyana", "GF"},
				{"Guyana (Fr.)", "Gyana", "GF"},
				{"Gambia", "Gambia", "GM"},
				{"Guinea", "Guinea", "GN"},
				{"Greece", "Greece", "GR"},
				{"Guatemala", "Guatemala", "GT"},
				{"Guam (US)", "Guam (US)", "GU"},
				{"Guinea Bissau", "Guinea Bissau", "GW"},
				{"Guyana", "Guyana", "GY"},
				{"Hong Kong", "Hong Kong", "HK"},
				{"Heard & McDonald Isl.", "Heard & McDonald Isl.", "HM"},
				{"Honduras", "Honduras", "HN"},
				{"Croatia", "Croatia", "HR"},
				{"Haiti", "Haiti", "HT"},
				{"Hungary", "Hungary", "HU"},
				{"Indonesia", "Indonesia", "ID"},
				{"Ireland", "Ireland", "IE"},
				{"Israel", "Israel", "IL"},
				{"India", "India", "IN"},
				{"British Indian O. Terr.", "British Indian O. Terr.", "IO"},
				{"Iraq", "Iraq", "IQ"},
				{"Iran", "Iran", "IR"},
				{"Iceland", "Iceland", "IS"},
				{"Italy", "Italy", "IT"},
				{"Jamaica", "Jamaica", "JM"},
				{"Jordan", "Jordan", "JO"},
				{"Japan", "Japan", "JP"},
				{"Kenya", "Kenya", "KE"},
				{"Kirgistan", "Kirgistan", "KG"},
				{"Cambodia", "Cambodia", "KH"},
				{"Kiribati", "Kiribati", "KI"},
				{"Comoros", "Comoros", "KM"},
				{"St.Kitts Nevis Anguilla", "St.Kitts Nevis Anguilla", "KN"},
				{"Korea (North)", "Korea (North)", "KP"},
				{"Korea (South)", "Korea (South)", "KR"},
				{"Kuwait", "Kuwait", "KW"},
				{"Cayman Islands", "Cayman Islands", "KY"},
				{"Kazachstan", "Kazachstan", "KZ"},
				{"Laos", "Laos", "LA"},
				{"Lebanon", "Lebanon", "LB"},
				{"Saint Lucia", "Saint Lucia", "LC"},
				{"Liechtenstein", "Liechtenstein", "LI"},
				{"Sri Lanka", "Sri Lanka", "LK"},
				{"Liberia", "Liberia", "LR"},
				{"Lesotho", "Lesotho", "LS"},
				{"Lithuania", "Lithuania", "LT"},
				{"Luxembourg", "Luxembourg", "LU"},
				{"Latvia", "Latvia", "LV"},
				{"Libya", "Libya", "LY"},
				{"Morocco", "Morocco", "MA"},
				{"Monaco", "Monaco", "MC"},
				{"Moldavia", "Moldavia", "MD"},
				{"Madagascar", "Madagascar", "MG"},
				{"Marshall Islands", "Marshall Islands", "MH"},
				{"Mali", "Mali", "ML"},
				{"Myanmar", "Myanmar", "MM"},
				{"Mongolia", "Mongolia", "MN"},
				{"Macau", "Macau", "MO"},
				{"Northern Mariana Isl.", "Northern Mariana Isl.", "MP"},
				{"Martinique (Fr.)", "Martinique (Fr.)", "MQ"},
				{"Mauritania", "Mauritania", "MR"},
				{"Montserrat", "Montserrat", "MS"},
				{"Malta", "Malta", "MT"},
				{"Mauritius", "Mauritius", "MU"},
				{"Maldives", "Maldives", "MV"},
				{"Malawi", "Malawi", "MW"},
				{"Mexico", "Mexico", "MX"},
				{"Malaysia", "Malaysia", "MY"},
				{"Mozambique", "Mozambique", "MZ"},
				{"Mozambique", "Mozambiq", "MZ"},
				{"Namibia", "Namibia", "NA"},
				{"New Caledonia (Fr.)", "New Caledonia (Fr.)", "NC"},
				{"Niger", "Niger", "NE"},
				{"Norfolk Island", "Norfolk Island", "NF"},
				{"Nigeria", "Nigeria", "NG"},
				{"Nicaragua", "Nicaragua", "NI"},
				{"Netherlands", "Netherlands", "NL"},
				{"Norway", "Norway", "NO"},
				{"Nepal", "Nepal", "NP"},
				{"Nauru", "Nauru", "NR"},
				{"Neutral Zone", "Neutral Zone", "NT"},
				{"Niue", "Niue", "NU"},
				{"New Zealand", "New Zealand", "NZ"},
				{"Oman", "Oman", "OM"},
				{"Panama", "Panama", "PA"},
				{"Peru", "Peru", "PE"},
				{"Polynesia (Fr.)", "Polynesia (Fr.)", "PF"},
				{"Papua New Guinea", "Papua New Guinea", "PG"},
				{"Philippines", "Philippines", "PH"},
				{"Pakistan", "Pakistan", "PK"},
				{"Poland", "Poland", "PL"},
				{"St. Pierre & Miquelon", "St. Pierre & Miquelon", "PM"},
				{"Pitcairn", "Pitcairn", "PN"},
				{"Portugal", "Portugal", "PT"},
				{"Puerto Rico (US)", "Puerto Rico (US)", "PR"},
				{"Palau", "Palau", "PW"},
				{"Paraguay", "Paraguay", "PY"},
				{"Qatar", "Qatar", "QA"},
				{"Reunion (Fr.)", "Reunion (Fr.)", "RE"},
				{"Romania", "Romania", "RO"},
				{"Russian Federation", "Russian Federation", "RU"},
				{"Rwanda", "Rwanda", "RW"},
				{"Saudi Arabia", "Saudi Arabia", "SA"},
				{"Solomon Islands", "Solomon Islands", "SB"},
				{"Seychelle", "Seychelles", "SC"},
				{"Sudan", "Sudan", "SD"},
				{"Sweden", "Sweden", "SE"},
				{"Singapore", "Singapore", "SG"},
				{"St. Helena", "St. Helena", "SH"},
				{"Slovenia", "Slovenia", "SI"},
				{"Svalbard & Jan Mayen Islands", "Svalbard & Jan Mayen Islands", "SJ"},
				{"Slovak Republic", "Slovak Republic", "SK"},
				{"Sierra Leone", "Sierra Leone", "SL"},
				{"San Marino", "San Marino", "SM"},
				{"Senegal", "Senegal", "SN"},
				{"Somalia", "Somalia", "SO"},
				{"Suriname", "Suriname", "SR"},
				{"St. Tome and Principe", "St. Tome and Principe", "ST"},
				{"Soviet Union", "Soviet Union", "SU"},
				{"El Salvador", "El Salvador", "SV"},
				{"Syria", "Syria", "SY"},
				{"Swaziland", "Swaziland", "SZ"},
				{"Turks & Caicos Islands", "Turks & Caicos Islands", "TC"},
				{"Chad", "Chad", "TD"},
				{"French Southern Terr.", "French Southern Terr.", "TF"},
				{"Togo", "Togo", "TG"},
				{"Thailand", "Thailand", "TH"},
				{"Tadjikistan", "Tadjikistan", "TJ"},
				{"Tokelau", "Tokelau", "TK"},
				{"Turkmenistan", "Turkmenistan", "TM"},
				{"Tunisia", "Tunisia", "TN"},
				{"Tonga", "Tonga", "TG"},
				{"East Timor", "East Timor", "TP"},
				{"Turkey", "Turkey", "TR"},
				{"Turkey", "Turky", "TR"},
				{"Trinidad & Tobago", "Trinidad & Tobago", "TT"},
				{"Tuvalu", "Tuvalu", "TV"},
				{"Taiwan", "Taiwan", "TW"},
				{"Tanzania", "Tanzania", "TZ"},
				{"Ukraine", "Ukraine", "UA"},
				{"Uganda", "Uganda", "UG"},
				{"United Kingdom", "United Kingdom", "UK"},
				{"United Kingdom", "England", "UK"},
				{"US Minor outlying Isl.", "US Minor outlying Isl.", "UM"},
				{"United States", "United States", "US"},
				{"United States", "USA", "US"},
				{"United States", "U.S.A.", "US"},
				{"United States", "United States of America", "US"},
				{"Uruguay", "Uruguay", "UY"},
				{"Uzbekistan", "Uzbekistan", "UZ"},
				{"Vatican City State", "Vatican City State", "VA"},
				{"St.Vincent & Grenadines", "St.Vincent", "VC"},
				{"St.Vincent & Grenadines", "Grenadines", "VC"},
				{"St.Vincent & Grenadines", "St.Vincent and Grenadines", "VC"},
				{"St.Vincent & Grenadines", "St.Vincent & Grenadines", "VC"},
				{"St.Vincent & Grenadines", "St.Vincent&Grenadines", "VC"},
				{"Venezuela", "Venezuela", "VE"},
				{"Virgin Islands (British)", "Virgin Islands (British)", "VG"},
				{"Virgin Islands (US)", "Virgin Islands (US)", "VI"},
				{"Vietnam", "Vietnam", "VN"},
				{"Vanuatu", "Vanuatu", "VU"},
				{"Wallis & Futuna Islands", "Wallis & Futuna Islands", "WF"},
				{"Samoa", "Samoa", "WS"},
				{"Yemen", "Yemen", "YE"},
				{"Yugoslavia", "Yugoslavia", "YU"},
				{"South Africa", "South Africa", "ZA"},
				{"South Africa", "SouthAfrica", "ZA"},
				{"Zambia", "Zambia", "ZM"},
				{"Zaire", "Zaire", "ZR"},
				{"Zimbabwe", "Zimbabwe", "ZW"}
			};

		public static readonly String[,] States = new String[,]
			{
				{"Alabama", "AL"},
				{"Alaska", "AK"},
				{"American Samoa", "AS"},
				{"Arizona", "AZ"},
				{"Arkansas", "AR"},
				{"California", "CA"},
				{"Colorado", "CO"},
				{"Connecticut", "CT"},
				{"Deleware", "DE"},
				{"District of Columbia", "DC"},
				{"Federated States of Micronesia", "FM"},
				{"Florida", "FL"},
				{"Georgia", "GA"},
				{"Guam", "GU"},
				{"Hawaii", "HI"},
				{"Idaho", "ID"},
				{"Illinois", "IL"},
				{"Indiana", "IN"},
				{"Iowa", "IA"},
				{"Kansas", "KS"},
				{"Kentucky", "KY"},
				{"Lousiana", "LA"},
				{"Maine", "ME"},
				{"Marshall Islands", "MH"},
				{"Maryland", "MD"},
				{"Massachusetts", "MA"},
				{"Michigan", "MI"},
				{"Minnesota", "MN"},
				{"Mississippi", "MS"},
				{"Missouri", "MO"},
				{"Montana", "MT"},
				{"Nebraska", "NE"},
				{"Nevada", "NV"},
				{"New Hampshire", "NH"},
				{"New Jersey", "NJ"},
				{"New Mexico", "NM"},
				{"New York", "NY"},
				{"North Carolina", "NC"},
				{"North Dakota", "ND"},
				{"Northern Mariana Islands", "MP"},
				{"Ohio", "OH"},
				{"Oregon", "OR"},
				{"Palau", "PW"},
				{"Pennsylvania", "PA"},
				{"Puerto Rico", "PR"},
				{"Rhode Island", "RI"},
				{"South Carolina", "SC"},
				{"South Dakota", "SD"},
				{"Tennessee", "TN"},
				{"Texas", "TX"},
				{"Utah", "UT"},
				{"Vermont", "VT"},
				{"Virgin Islands", "VI"},
				{"Virginia", "VA"},
				{"Washington", "WA"},
				{"West Virginia", "WV"},
				{"Wisconsin", "WI"},
				{"Wyoming", "WY"},
				{"Armed Forces Africa", "AE"},
				{"Armed Forces Americas", "AA"},
				{"Armed Forces Canada", "AE"},
				{"Armed Forces Europe", "AE"},
				{"Armed Forces Middle East", "AE"},
				{"Armed Forces Pacific", "AP"}
			};

		public static readonly String[,] CanadianProvinces = new String[,]
			{
				{"Alberta", "AB"},
				{"British Columbia", "BC"},
				{"Manitoba", "MB"},
				{"New Brunswick", "NB"},
				{"Newfoundland and Labrador", "NL"},
				{"Nova Scotia", "NS"},
				{"Northwest Territories", "NT"},
				{"Nanavut", "NU"},
				{"Ontario", "ON"},
				{"Prince Edward Island", "PE"},
				{"Quebec", "QC"},
				{"Saskatchewan", "SK"},
				{"Yukon", "YT"}
			};



		#endregion

        #region Aggregate Properties

        public bool IsBusiness()
        {
            string temp;

            temp = lastName.ToLower();

            switch (temp)
            {
                case "corp.":
                    return true;
                case "corporation":
                    return true;
                case "inc.":
                    return true;
                case "incorporated":
                    return true;
                case "llc":
                    return true;
            }

            return false;
        }

        //		public enum ContactNameFormat
		//		{
		//			F,			// First
		//			L,			// Last
		//			FL,			// First Last
		//			FML,		// First Middle Last
		//			L,F,		// Last, First
		//			L,F.,		// Last, First Initial
		//			L,FM,		// Last, First Middle
		//			L,FMi.		// Last, First Middle Initial
		//			LScFMi.		// Last Suffix, First Middle Initial
		//		};

		/// <summary>
		/// Constructs a name following the format specifier provided. 
		/// P	= Prefix
		/// S	= Suffix
		/// F	= First 
		/// M	= Middle
		/// L	= Last
		/// i	= Generates an initial
		/// i.	= Generates an initial with period
		/// c/,	= Comma
		/// 
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public string ConstructName(string format)
		{
			EnsurePreLoad();

            if (IsBusiness())
            {
                return DisplayName;
            }

			// Split tags into array
			char[] tags = format.ToCharArray();

			// initialFlag is used to flag the previous tag as an initial
			bool initialFlag = false;
			
			// initialPeriodFlag is used to flag the previous tag as an initial with period
			bool initialPeriodFlag = false;

			System.Text.StringBuilder s = new System.Text.StringBuilder();

			for(int x = 0; x < tags.Length; x++)
			{
				if(x+1 < tags.Length)
				{
					if(x+2 < tags.Length)
						initialPeriodFlag = tags[x+2] == '.';
					initialFlag = tags[x+1] == 'i';
				}

				switch(tags[x])
				{
					case 'P':
						if(prefix != null && prefix.Length > 0)
						{
							if(s.Length > 0) s.Append(' ');
							s.Append(prefix);
						}
						break;

					case 'S':
						if(suffix != null && suffix.Length > 0)
						{
							if(s.Length > 0) s.Append(' ');
							s.Append(suffix);
						}
						break;

					case 'F':
						if(firstName != null && firstName.Length > 0)
						{
							if(initialFlag)
							{
								if(s.Length > 0) s.Append(' ');
								s.Append(firstName.Substring(0,1));
								if(initialPeriodFlag)
								{
									s.Append('.');
									x++;
								}
								x++;
							}
							else
							{
								if(s.Length > 0) s.Append(' ');
								s.Append(firstName);
							}
						}
						break;

					case 'M':
						if(middleName != null && middleName.Length > 0)
						{
							if(initialFlag)
							{
								if(s.Length > 0) s.Append(' ');
								s.Append(middleName.Substring(0,1));
								if(initialPeriodFlag)
								{
									s.Append('.');
									x++;
								}
								x++;
							}
							else
							{
								if(s.Length > 0) s.Append(' ');
								s.Append(middleName);
							}
						}
						break;

					case 'L':
						if(lastName != null && lastName.Length > 0)
						{
							if(initialFlag)
							{
								if(s.Length > 0) s.Append(' ');
								s.Append(lastName.Substring(0,1));
								if(initialPeriodFlag)
								{
									s.Append('.');
									x++;
								}
								x++;
							}
							else
							{
								if(s.Length > 0) s.Append(' ');
								s.Append(lastName);
							}
						}
						break;

					case ',':
						s.Append(',');
						break;

					case 'c':
						s.Append(',');
						break;
				}				
			}

			return s.ToString();
		}

		/// <summary>
		/// Returns the full name of the contact, without prefixes.
		/// </summary>
		public string FullName
		{
			get
			{
				EnsurePreLoad();

                if (IsBusiness())
                {
                    return DisplayName;
                }

				System.Text.StringBuilder s = new System.Text.StringBuilder();

				// TEST FOR NULLS FIRST
				if(prefix == null ||
					firstName == null ||
					middleName == null ||
					lastName == null ||
					suffix == null)
				{
					throw(new Exception("Name firstName is null; cannot process full name."));
				}

				if(prefix.Length > 0)
				{
					s.Append(prefix);
				}

				if(firstName.Length > 0)
				{
					if(s.Length != 0)
						s.Append(' ');
					s.Append(firstName);
				}
				
				if(middleName.Length > 0)
				{
					if(s.Length != 0)
						s.Append(' ');
					s.Append(middleName);
				}

				if(lastName.Length > 0)
				{
					if(s.Length != 0)
						s.Append(' ');
					s.Append(lastName);
				}

				if(suffix.Length > 0)
				{
					if(suffixCommaEnabled)
						s.Append(", ");
					else
						s.Append(' ');

					s.Append(suffix);
				}

				return s.ToString();
			}
		}

		public string ConstructAddress(string separator)
		{
			return ConstructAddress(separator, false, false);
		}

		public string ConstructAddress(string separator, bool phones, bool email)
		{
			this.EnsurePreLoad();

			System.Text.StringBuilder s = new System.Text.StringBuilder();

			if(address1 != string.Empty)
				s.Append(address1);

			if(address2 != string.Empty)
			{
				if(s.Length > 0)
					s.Append(separator);
				s.Append(address2);
			}
				
			if(city != string.Empty)
			{
				if(s.Length > 0)
					s.Append(separator);
				s.Append(city);

				if(stateProvince != string.Empty)
				{
					s.Append(", ");
					s.Append(stateProvince);
				}

				if(postalCode != string.Empty)
				{
					s.Append(" ");
					s.Append(postalCode);
				}
			}
			else
			{
				if(s.Length > 0)
					s.Append(separator);

				if(stateProvince != string.Empty)
					s.Append(stateProvince);
				if(postalCode != string.Empty)
				{
					s.Append(' ');
					s.Append(postalCode);
				}
			}
			
			if(country != string.Empty)
			{
				s.Append(' ');
				s.Append(country);
			}

			if(phones)
			{
				if(homePhone != string.Empty)
				{
					s.Append(separator);
					s.Append(homePhone);
					s.Append(" (h)");
				}

				if(workPhone != string.Empty)
				{
					s.Append(separator);
					s.Append(workPhone);
					s.Append(" (w)");					
				}
			}

			if(email)
			{
				if(email1 != string.Empty)
				{
					s.Append(separator);
					s.Append("<a href=\"mailto:");
					s.Append(email1);
					s.Append("\">");
					s.Append(email1);
					s.Append("</a>");
					s.Append(separator);
				}
			}

			return s.ToString();
		}

		#endregion

		#region ToString()

		public override string ToString()
		{
			return ToString(false);
		}

		public string ToString(bool webHrefEnabled)
		{
			string separator = "\r\n";
			System.Text.StringBuilder s = new System.Text.StringBuilder();

			if(System.Web.HttpContext.Current != null)
				separator = "<br />";

			s.Append(FullName);
            s.Append(separator);

			if(BusinessName != string.Empty)
			{
				s.Append(BusinessName);
				s.Append(separator);
			}

			s.Append(this.ConstructAddress(separator));

			if(HomePhone != string.Empty)
			{
				s.Append(separator);
				s.Append(HomePhone);
				s.Append(" (h)");
			}

			if(WorkPhone != string.Empty)
			{
				s.Append(separator);
				s.Append(WorkPhone);
				s.Append(" (w)");
			}

			if(Fax != string.Empty)
			{
				s.Append(separator);
				s.Append(Fax);
				s.Append(" (fax)");
			}

			if(Email1 != string.Empty)
			{
				s.Append(separator);
				if(webHrefEnabled)
				{
					s.Append("<a href=\"mailto:");
					s.Append(Email1);
					s.Append("\">");
					s.Append(Email1);
					s.Append("</a>");
				}
				else
					s.Append(Email1);
			}

			if(Url != string.Empty)
			{
				s.Append(separator);
				if(webHrefEnabled)
				{
					s.Append("<a href=\"");
					s.Append(Url);
					s.Append("\">");
					s.Append(Url);
					s.Append("</a>");
				}
				else
					s.Append(Url);
			}

			return s.ToString();
		}

		#endregion

        #region CopyValues

        public void CopyValuesTo(GreyFoxContact contact, bool copyMemo)
        {            
            contact.DisplayName = DisplayName;
            contact.BusinessName = BusinessName;
            contact.Title = Title;
            contact.Prefix = Prefix;
            contact.FirstName = FirstName;
            contact.MiddleName = MiddleName;
            contact.LastName = LastName;
            contact.Suffix = Suffix;
            contact.SuffixCommaEnabled = SuffixCommaEnabled;

            contact.Address1 = Address1;
            contact.Address2 = Address2;
            contact.City = City;
            contact.StateProvince = StateProvince;
            contact.PostalCode = PostalCode;
            contact.Country = Country;

            contact.HomePhone = HomePhone;
            contact.WorkPhone = WorkPhone;
            contact.MobilePhone = MobilePhone;
            contact.Pager = Pager;
            contact.Fax = Fax;
            contact.Email1 = Email1;
            contact.Email2 = Email2;
            contact.Url = Url;
            contact.ValidationFlags = ValidationFlags;
            contact.ValidationMemo = ValidationMemo;
            contact.BirthDate = BirthDate;
            contact.ContactMethod = ContactMethod;
            
            if(copyMemo)
            {
                contact.MemoText = MemoText;
            }

        }

        #endregion

        #region Flags

        public bool IsBadAddress
        {
            get
            {
                return (ValidationFlags & GreyFoxContactValidationFlag.BadAddress) == 
                    GreyFoxContactValidationFlag.BadAddress;
            }
            set
            {
                if (value)
                {
                    ValidationFlags |= GreyFoxContactValidationFlag.BadAddress;
                }
                else
                {
                    ValidationFlags &= ~GreyFoxContactValidationFlag.BadAddress;
                }
            }
        }

        public bool IsBadEmail
        {
            get
            {
                return (ValidationFlags & GreyFoxContactValidationFlag.BadEmail) ==
                    GreyFoxContactValidationFlag.BadEmail;
            }
            set
            {
                if (value)
                {
                    ValidationFlags |= GreyFoxContactValidationFlag.BadEmail;
                }
                else
                {
                    ValidationFlags &= ~GreyFoxContactValidationFlag.BadEmail;
                }
            }
        }

        public bool IsBadHomePhone
        {
            get
            {
                return (ValidationFlags & GreyFoxContactValidationFlag.BadHomePhone) ==
                    GreyFoxContactValidationFlag.BadHomePhone;
            }
            set
            {
                if (value)
                {
                    ValidationFlags |= GreyFoxContactValidationFlag.BadHomePhone;
                }
                else
                {
                    ValidationFlags &= ~GreyFoxContactValidationFlag.BadHomePhone;
                }
            }
        }

        public bool IsBadWorkPhone
        {
            get
            {
                return (ValidationFlags & GreyFoxContactValidationFlag.BadWorkPhone) ==
                    GreyFoxContactValidationFlag.BadWorkPhone;
            }
            set
            {
                if (value)
                {
                    ValidationFlags |= GreyFoxContactValidationFlag.BadWorkPhone;
                }
                else
                {
                    ValidationFlags &= ~GreyFoxContactValidationFlag.BadWorkPhone;
                }
            }
        }

        public bool IsBadMobilePhone
        {
            get
            {
                return (ValidationFlags & GreyFoxContactValidationFlag.BadMobilePhone) ==
                    GreyFoxContactValidationFlag.BadMobilePhone;
            }
            set
            {
                if (value)
                {
                    ValidationFlags |= GreyFoxContactValidationFlag.BadMobilePhone;
                }
                else
                {
                    ValidationFlags &= ~GreyFoxContactValidationFlag.BadMobilePhone;
                }
            }
        }

        public bool IsBadUrl
        {
            get
            {
                return (ValidationFlags & GreyFoxContactValidationFlag.BadUrl) ==
                    GreyFoxContactValidationFlag.BadUrl;
            }
            set
            {
                if (value)
                {
                    ValidationFlags |= GreyFoxContactValidationFlag.BadUrl;
                }
                else
                {
                    ValidationFlags &= ~GreyFoxContactValidationFlag.BadUrl;
                }
            }
        }

        public string ValidationFlagsToString()
        {
            int badFlags = 0;
            System.Text.StringBuilder s;
            string final;
            int lastComma;

            EnsurePreLoad();

            s = new System.Text.StringBuilder();
            badFlags = appendValidationString(s, badFlags, GreyFoxContactValidationFlag.BadAddress);
            badFlags = appendValidationString(s, badFlags, GreyFoxContactValidationFlag.BadHomePhone);
            badFlags = appendValidationString(s, badFlags, GreyFoxContactValidationFlag.BadWorkPhone);
            badFlags = appendValidationString(s, badFlags, GreyFoxContactValidationFlag.BadMobilePhone);
            badFlags = appendValidationString(s, badFlags, GreyFoxContactValidationFlag.BadEmail);
            badFlags = appendValidationString(s, badFlags, GreyFoxContactValidationFlag.BadUrl);

            if (badFlags > 1)
            {
                final = s.ToString();                
                lastComma = final.LastIndexOf(Localization.PeopleStrings.Comma);
                s.Replace(Localization.PeopleStrings.Comma,
                    Localization.PeopleStrings.Space +
                    Localization.PeopleStrings.And,
                    lastComma, Localization.PeopleStrings.Comma.Length);
                return s.ToString();
            }

            return s.ToString().ToLower();
        }

        private int appendValidationString(System.Text.StringBuilder s, int badFlags,
            GreyFoxContactValidationFlag flag)
        {
            if ((validationFlags & flag) == flag)
            {
                if (badFlags > 0)
                {
                    s.Append(Localization.PeopleStrings.Comma);
                    s.Append(Localization.PeopleStrings.Space);
                }
                else
                {
                    s.Append(Localization.PeopleStrings.Invalid);
                    s.Append(Localization.PeopleStrings.Space);
                }

                switch (flag)
                {
                    case GreyFoxContactValidationFlag.BadAddress:
                        s.Append(Localization.PeopleStrings.Address);
                        break;
                    case GreyFoxContactValidationFlag.BadHomePhone:
                        s.Append(Localization.PeopleStrings.HomePhone);
                        break;
                    case GreyFoxContactValidationFlag.BadWorkPhone:
                        s.Append(Localization.PeopleStrings.WorkPhone);
                        break;
                    case GreyFoxContactValidationFlag.BadMobilePhone:
                        s.Append(Localization.PeopleStrings.MobilePhone);
                        break;
                    case GreyFoxContactValidationFlag.BadEmail:
                        s.Append(Localization.PeopleStrings.Email);
                        break;
                    case GreyFoxContactValidationFlag.BadUrl:
                        s.Append(Localization.PeopleStrings.WebsiteUrl);
                        break;
                }

                return badFlags + 1;
            }

            return badFlags;
        }

        #endregion

		//--- End Custom Code ---
	}
}
