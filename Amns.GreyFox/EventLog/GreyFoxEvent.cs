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

namespace Amns.GreyFox.EventLog
{
	/// <summary>
	/// Event class for Amns.GreyFox Event Log.
	/// </summary>
	public class GreyFoxEvent : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal string tableName;
		internal byte type;
		internal DateTime eventDate;
		internal string source;
		internal string category;
		internal string description;
		internal int eventID;
		internal GreyFoxUser user;

		#endregion

		#region Public Properties

		/// <summary>
		/// GreyFoxEvent Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the GreyFoxEvent as a Placeholder. Placeholders only contain 
		/// a GreyFoxEvent ID. Record late-binds data when it is accessed.
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
		/// Identifies the table the GreyFoxEvent belongs to. Readonly. 
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
		/// Type of event.
		/// </summary>
		public byte Type
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return type;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= type == value;
					type = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public DateTime EventDate
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return eventDate;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= eventDate == value;
					eventDate = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Source
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return source;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= source == value;
					source = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public string Category
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return category;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= category == value;
					category = value;
				}
			}
		}

		/// <summary>
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
		/// </summary>
		public int EventID
		{
			get
			{
				EnsurePreLoad();
				lock(this)
				{
					return eventID;
				}
			}
			set
			{
				EnsurePreLoad();
				lock(this)
				{
					isSynced &= eventID == value;
					eventID = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public GreyFoxUser User
		{
			get
			{
				lock(this)
				{
					EnsurePreLoad();
					return user;
				}
			}
			set
			{
				lock(this)
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
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of GreyFoxEvent.
		/// </summary>
		public GreyFoxEvent()
		{
		}

		public GreyFoxEvent(string tableName)
		{
			this.tableName = tableName;
			isSynced = false;
		}

		public GreyFoxEvent(string tableName, int id)
		{
			this.iD = id;
			this.tableName = tableName;
			lock(this)
			{
				isSynced = GreyFoxEventManager._fill(this);
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
				GreyFoxEventManager._fill(this);
				isPlaceHolder = false;
			}
		}

		/// <summary>
		/// Saves the GreyFoxEvent object state to the database.
		/// </summary>
		public int Save()
		{
			lock(this)
			{
				if(user != null)
					user.Save();

				if(isSynced)
					return iD;

				if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
				if(iD == 0)
					iD = GreyFoxEventManager._insert(this);
				else
					GreyFoxEventManager._update(this);
				isSynced = iD != -1;
			}
			return iD;
		}

		public void Delete()
		{
			GreyFoxEventManager._delete(this.tableName, this.iD);
			this.iD = 0;
			isSynced = false;
		}
		/// <summary>
		/// Duplicates GreyFoxEvent object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxEvent object reflecting the replicated GreyFoxEvent object.</returns>
		public GreyFoxEvent Duplicate(string tableName)
		{
			lock(this)
			{
			GreyFoxEvent clonedGreyFoxEvent = this.Clone();
			clonedGreyFoxEvent.tableName = tableName;

			// Insert must be called after children are replicated!
			clonedGreyFoxEvent.iD = GreyFoxEventManager._insert(clonedGreyFoxEvent);
			clonedGreyFoxEvent.isSynced = true;
			return clonedGreyFoxEvent;
			}
		}

		/// <summary>
		/// Overwrites and existing GreyFoxEvent object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			GreyFoxEventManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones GreyFoxEvent object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxEvent object reflecting the replicated GreyFoxEvent object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones GreyFoxEvent object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new GreyFoxEvent object reflecting the replicated GreyFoxEvent object.</returns>
		public GreyFoxEvent Clone()
		{
			lock(this)
			{
			GreyFoxEvent clonedGreyFoxEvent = new GreyFoxEvent();
			clonedGreyFoxEvent.iD = iD;
			clonedGreyFoxEvent.tableName = tableName;
			clonedGreyFoxEvent.isSynced = isSynced;
			clonedGreyFoxEvent.type = type;
			clonedGreyFoxEvent.eventDate = eventDate;
			clonedGreyFoxEvent.source = source;
			clonedGreyFoxEvent.category = category;
			clonedGreyFoxEvent.description = description;
			clonedGreyFoxEvent.eventID = eventID;


			if(user != null)
				clonedGreyFoxEvent.user = user;

			return clonedGreyFoxEvent;
			}
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxEvent.
		/// </summary>
		/// <returns> A new GreyFoxEvent object reflecting the cloned GreyFoxEvent object.</returns>
		public GreyFoxEvent Copy()
		{
			GreyFoxEvent greyFoxEvent = new GreyFoxEvent();
			CopyTo(greyFoxEvent);
			return greyFoxEvent;
		}

		/// <summary>
		/// Makes a deep copy of the current GreyFoxEvent.
		/// </summary>
		/// <returns> A new GreyFoxEvent object reflecting the cloned GreyFoxEvent object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxEvent from its children.</param>
		public GreyFoxEvent Copy(bool isolation)
		{
			GreyFoxEvent greyFoxEvent = new GreyFoxEvent();
			CopyTo(greyFoxEvent, isolation);
			return greyFoxEvent;
		}

		/// <summary>
		/// Deep copies the current GreyFoxEvent to another instance of GreyFoxEvent.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="GreyFoxEvent">The GreyFoxEvent to copy to.</param>
		public void CopyTo(GreyFoxEvent greyFoxEvent)
		{
			CopyTo(greyFoxEvent, false);
		}

		/// <summary>
		/// Deep copies the current GreyFoxEvent to another instance of GreyFoxEvent.
		/// </summary>
		/// <param name="GreyFoxEvent">The GreyFoxEvent to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the GreyFoxEvent from its children.</param>
		public void CopyTo(GreyFoxEvent greyFoxEvent, bool isolation)
		{
			lock(this)
			{
				greyFoxEvent.iD = iD;
				greyFoxEvent.tableName = tableName;
				greyFoxEvent.isPlaceHolder = isPlaceHolder;
				greyFoxEvent.isSynced = isSynced;
				greyFoxEvent.type = type;
				greyFoxEvent.eventDate = eventDate;
				greyFoxEvent.source = source;
				greyFoxEvent.category = category;
				greyFoxEvent.description = description;
				greyFoxEvent.eventID = eventID;
				if(user != null)
				{
					if(isolation)
					{
						greyFoxEvent.user = user.NewPlaceHolder();
					}
					else
					{
						greyFoxEvent.user = user.Copy(false);
					}
				}
			}
		}

		public GreyFoxEvent NewPlaceHolder()
		{
			GreyFoxEvent greyFoxEvent = new GreyFoxEvent();
			greyFoxEvent.iD = iD;
			greyFoxEvent.tableName = tableName;
			greyFoxEvent.isPlaceHolder = true;
			greyFoxEvent.isSynced = true;
			return greyFoxEvent;
		}

		public static GreyFoxEvent NewPlaceHolder(string tableName, int iD)
		{
			GreyFoxEvent greyFoxEvent = new GreyFoxEvent();
			greyFoxEvent.iD = iD;
			greyFoxEvent.tableName = tableName;
			greyFoxEvent.isPlaceHolder = true;
			greyFoxEvent.isSynced = true;
			return greyFoxEvent;
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
			GreyFoxEvent greyFoxEvent = (GreyFoxEvent) obj;
			return this.iD - greyFoxEvent.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(GreyFoxEvent greyFoxEvent)
		{
			return this.iD - greyFoxEvent.iD;
		}

		public override bool Equals(object greyFoxEvent)
		{
			if(greyFoxEvent == null)
				return false;

			return Equals((GreyFoxEvent) greyFoxEvent);
		}

		public bool Equals(GreyFoxEvent greyFoxEvent)
		{
			if(greyFoxEvent == null)
				return false;

			return this.iD == greyFoxEvent.iD &&
				this.tableName == greyFoxEvent.tableName;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode() +
				tableName.GetHashCode();
		}

		#endregion

		//--- Begin Custom Code ---
																																																																																										
		#region WebError

		public void SetWebError(System.Exception error, System.Web.HttpRequest request)
		{
			System.Text.StringBuilder output = new System.Text.StringBuilder();

			type = 2;
			eventDate = DateTime.Now;
			source = "Generic";
			category = "Web";

			output.Append("An exception occured. \r\n");
			output.Append("\r\n");
			output.Append("URL : " + request.Url.ToString() + "\r\n");	
			output.Append("Referrer : " + request.UrlReferrer.ToString() + "\r\n");
			output.Append("IP : " + request.UserHostAddress + "\r\n");
			output.Append("Client : " + request.UserAgent + "\r\n");
			output.Append("\r\n");
			
			Exception err = error;
			int count = 0;
			while(err != null)
			{
				count += 1;
				output.Append(count.ToString() + " Error Description: " + error.Message + "\r\n");
				output.Append(count.ToString() + " Source: " + error.Source.Replace("\n", "\n" + count.ToString() + ": ") + "\r\n");
				output.Append(count.ToString() + " Stack Trace: " + error.StackTrace.Replace("\n", "\n" + count.ToString() + ": ") + "\r\n");
				output.Append(count.ToString() + " Target Site: " + error.TargetSite.ToString().Replace("\n", "\n" + count.ToString() + ": ") + "\r\n");
				err = err.InnerException;
			}

			description = output.ToString();
		}

		//		private string getStringFromArray(System.Web.HttpRequest request)
		//		{
		//			System.Text.StringBuilder output = new System.Text.StringBuilder();
		//
		//			for(int i = 0; i < array.Length; i++)
		//			{
		//				output.Append(array[i]);
		//				output.Append(array[i] + " - " + request.Form[array[i])
		//			}
		//
		//		}

		#endregion

		//--- End Custom Code ---
	}
}
