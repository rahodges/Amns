/* ********************************************************** *
 * AMNS DbModel v1.0 OleDbManager Data Tier                   *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web;
using System.Web.Caching;
using Amns.GreyFox.Data;
using Amns.GreyFox.Security;
using Amns.GreyFox.People;

namespace Amns.GreyFox
{
	#region Child Flags Enumeration

	public enum GreyFoxUserPreferenceFlags : int { User,
				UserContact,
				UserRoles};

	#endregion

	/// <summary>
	/// Datamanager for GreyFoxUserPreference objects.
	/// </summary>
	[ExposedManager("GreyFoxUserPreference", "", true, 1, 1, 6234)]
	public class GreyFoxUserPreferenceManager : IGreyFoxManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;
		static string connectionString;

		// Private Fields
		string tableName = "sysGlobal_UserPreferences";


		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}

		// Hashtable to cache separate tables
		static bool cacheEnabled	= true;
		public static bool CacheEnabled
		{
			get { return cacheEnabled; }
			set { cacheEnabled = value; }
		}

		#endregion

		#region Inner Join Field Array

		public static readonly string[] InnerJoinFields = new string[] {
			"GreyFoxUserPreferenceID", 
			"UserID", 
			"Name", 
			"Value" 
		};

		#endregion

		#region Default DbModel Constructors

		static GreyFoxUserPreferenceManager()
		{
			connectionString = "";
		}

		public GreyFoxUserPreferenceManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!GreyFoxUserPreferenceManager.isInitialized)
			{
				GreyFoxUserPreferenceManager.isInitialized = true;
				GreyFoxUserPreferenceManager.connectionString = connectionString;
			}
			else
			{
				throw(new Exception("Manager has already been initialized."));
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a GreyFoxUserPreference into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_GreyFoxUserPreference">The GreyFoxUserPreference to insert into the database.</param>
		internal static int _insert(GreyFoxUserPreference greyFoxUserPreference)
		{
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand();
			dbCommand.Connection = dbConnection;
			dbCommand.CommandText = "INSERT INTO sysGlobal_UserPreferences (UserID," +
				"Name," +
				"Value) VALUES (" +
				"inUserID," +
				"inName," +
				"inValue);";

			fillParameters(dbCommand, greyFoxUserPreference);

			dbConnection.Open();
			dbCommand.ExecuteNonQuery();
			dbCommand.CommandText = "SELECT @@IDENTITY AS IDVal";
			int id = (int) dbCommand.ExecuteScalar();

			dbConnection.Close();
			// Store greyFoxUserPreference in cache.
			if(cacheEnabled) cacheStore(greyFoxUserPreference);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(GreyFoxUserPreference greyFoxUserPreference)
		{
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand();
			dbCommand.Connection = dbConnection;
			dbCommand.CommandText = "UPDATE sysGlobal_UserPreferences SET UserID=inUserID," +
				"Name=inName," +
				"Value=inValue WHERE GreyFoxUserPreferenceID=inGreyFoxUserPreferenceID";

			fillParameters(dbCommand, greyFoxUserPreference);

			dbCommand.Parameters.Add("inGreyFoxUserPreferenceID", OleDbType.Integer).Value = greyFoxUserPreference.iD;
			dbConnection.Open();

			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (dbCommand.ExecuteNonQuery() == 0) return -1;

			dbConnection.Close();

			// Store greyFoxUserPreference in cache.
			if (cacheEnabled) cacheStore(greyFoxUserPreference);

			return greyFoxUserPreference.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(OleDbCommand command, GreyFoxUserPreference greyFoxUserPreference)
		{
			#region New Folder

			if(greyFoxUserPreference.user == null)
				command.Parameters.Add("inUserID", OleDbType.Integer).Value = DBNull.Value;
			else
				command.Parameters.Add("inUserID", OleDbType.Integer).Value = greyFoxUserPreference.user.ID;
			command.Parameters.Add("inName", OleDbType.VarChar).Value = greyFoxUserPreference.name;
			command.Parameters.Add("inValue", OleDbType.VarChar).Value = greyFoxUserPreference.value;
			#endregion

		}

		#endregion

		#region Default DbModel Fill Method

		internal static bool _fill(GreyFoxUserPreference greyFoxUserPreference)
		{
			// Clone item from cache.
			if(cacheEnabled)
			{
				GreyFoxUserPreference cachedGreyFoxUserPreference = cacheFind(greyFoxUserPreference.iD);
				if(cachedGreyFoxUserPreference != null)
				{
				cachedGreyFoxUserPreference.CopyTo(greyFoxUserPreference, true);
				return greyFoxUserPreference.isSynced;
				}
			}

			StringBuilder query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM sysGlobal_UserPreferences WHERE GreyFoxUserPreferenceID=");
			query.Append(greyFoxUserPreference.iD);
			query.Append(";");

			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query.ToString(), dbConnection);
			dbConnection.Open();
			OleDbDataReader r = dbCommand.ExecuteReader(CommandBehavior.SingleRow);

			if(!r.Read())
				throw(new Exception(string.Format("Cannot find GreyFoxUserPreferenceID '{0}'.", 
					greyFoxUserPreference.iD)));

			FillFromReader(greyFoxUserPreference, r, 0, 1);

			r.Close();
			dbConnection.Close();
			// Store greyFoxUserPreference in cache.
			if(cacheEnabled) cacheStore(greyFoxUserPreference);
			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public GreyFoxUserPreferenceCollection GetCollection(string whereClause, string sortClause, params GreyFoxUserPreferenceFlags[] optionFlags)
		{
			return GetCollection(0, whereClause, sortClause, optionFlags);
		}

		public GreyFoxUserPreferenceCollection GetCollection(int topCount, string whereClause, string sortClause, params GreyFoxUserPreferenceFlags[] optionFlags)
		{
			StringBuilder query = new StringBuilder("SELECT ");
			if(topCount > 0)
			{
				query.Append("TOP ");
				query.Append(topCount);
				query.Append(" ");
			}
			foreach(string columnName in InnerJoinFields)
			{
				query.Append("sysGlobal_UserPreferences.");
				query.Append(columnName);
				query.Append(",");
			}

			int innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;
			int userOffset = -1;
			int userContactOffset = -1;

			//
			// Append Option Flag Fields
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case GreyFoxUserPreferenceFlags.User:
							for(int i = 0; i <= GreyFoxUserManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("sysGlobal_Users.");
								query.Append(GreyFoxUserManager.InnerJoinFields[i]);
								query.Append(",");
							}
							userOffset = innerJoinOffset;
							innerJoinOffset = userOffset + GreyFoxUserManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case GreyFoxUserPreferenceFlags.UserContact:
							for(int i = 0; i <= GreyFoxContactManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("sysGlobal_Contacts.");
								query.Append(GreyFoxContactManager.InnerJoinFields[i]);
								query.Append(",");
							}
							userContactOffset = innerJoinOffset;
							innerJoinOffset = userContactOffset + GreyFoxContactManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
					}
				}

			//
			// Remove trailing comma
			//
			query.Length--;
			if(optionFlags != null)
			{
				query.Append(" FROM ");

				//
				// Start INNER JOIN expressions
				//
				for(int x = 0; x < optionFlags.Length; x++)
					query.Append("(");

				query.Append("sysGlobal_UserPreferences");
			}
			else
			{
				query.Append(" FROM sysGlobal_UserPreferences ");
			}
			//
			// Finish INNER JOIN expressions
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case GreyFoxUserPreferenceFlags.User:
							query.Append(" LEFT JOIN sysGlobal_Users ON sysGlobal_UserPreferences.UserID = sysGlobal_Users.GreyFoxUserID)");
							break;
						case GreyFoxUserPreferenceFlags.UserContact:
							query.Append(" LEFT JOIN sysGlobal_Contacts ON sysGlobal_Users.ContactID = sysGlobal_Contacts.GreyFoxContactID)");
							break;
					}
				}

			//
			// Render where clause
			//
			if(whereClause != string.Empty)
			{
				query.Append(" WHERE ");
				query.Append(whereClause);
			}

			//
			// Render sort clause 
			//
			if(sortClause != string.Empty)
			{
				query.Append(" ORDER BY ");
				query.Append(sortClause);
			}

			//
			// Render final semicolon
			//
			query.Append(";");
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query.ToString(), dbConnection);
			dbConnection.Open();

			#if DEBUG
			OleDbDataReader r;
			try
			{
				r = dbCommand.ExecuteReader();
			}
			catch (Exception e)
			{
				throw(new Exception(e.Message + " --- Query: " + query.ToString()));
			}
			#else
			OleDbDataReader r = dbCommand.ExecuteReader();
			#endif

			GreyFoxUserPreferenceCollection greyFoxUserPreferenceCollection = new GreyFoxUserPreferenceCollection();

			while(r.Read())
			{
				GreyFoxUserPreference greyFoxUserPreference = ParseFromReader(r, 0, 1);

				// Fill User
				if(userOffset != -1 && !r.IsDBNull(userOffset))
				{
					GreyFoxUserManager.FillFromReader(greyFoxUserPreference.user, r, userOffset, userOffset+1);

					// Fill 
					if(userContactOffset != -1 && !r.IsDBNull(userContactOffset))
						GreyFoxContactManager.FillFromReader(greyFoxUserPreference.user.Contact, "sysGlobal_Contacts", r, userContactOffset, userContactOffset+1);

				}

				greyFoxUserPreferenceCollection.Add(greyFoxUserPreference);
			}

			r.Close();
			dbConnection.Close();

			return greyFoxUserPreferenceCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static GreyFoxUserPreference ParseFromReader(OleDbDataReader r, int idOffset, int dataOffset)
		{
			GreyFoxUserPreference greyFoxUserPreference = new GreyFoxUserPreference();
			FillFromReader(greyFoxUserPreference, r, idOffset, dataOffset);
			return greyFoxUserPreference;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleDbDataReader.
		/// </summary>
		public static void FillFromReader(GreyFoxUserPreference greyFoxUserPreference, OleDbDataReader r, int idOffset, int dataOffset)
		{
			greyFoxUserPreference.iD = r.GetInt32(idOffset);
			greyFoxUserPreference.isSynced = true;
			greyFoxUserPreference.isPlaceHolder = false;

			//
			// Parse Children From Database
			//
			if(!r.IsDBNull(0+dataOffset) && r.GetInt32(0+dataOffset) > 0)
			{
				greyFoxUserPreference.user = GreyFoxUser.NewPlaceHolder(r.GetInt32(0+dataOffset));
			}

			//
			// Parse Fields From Database
			//
			greyFoxUserPreference.name = r.GetString(1+dataOffset);
			greyFoxUserPreference.value = r.GetString(2+dataOffset);

		}

		#endregion

		#region Default DbModel Fill Methods

		#endregion

		#region Default DbModel Delete Method

		internal static void _delete(int id)
		{
			StringBuilder s = new StringBuilder("DELETE * FROM sysGlobal_UserPreferences WHERE GreyFoxUserPreferenceID=");
			s.Append(id);
			s.Append(';');

			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(s.ToString(), dbConnection);
			dbConnection.Open();
			dbCommand.ExecuteNonQuery();

			dbConnection.Close();

			cacheRemove(id);
		}

		#endregion

		#region Default DbModel Create Table Methods

		public static void CreateTableReferences()
		{
			StringBuilder query = new StringBuilder("ALTER TABLE sysGlobal_UserPreferences ADD ");
			query.Append(" CONSTRAINT sysGlobal_UserPreferences_User_FK FOREIGN KEY (UserID) REFERENCES sysGlobal_Users(GreyFoxUserID);");
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query.ToString(), dbConnection);
			dbConnection.Open();
			dbCommand.ExecuteNonQuery();

			dbConnection.Close();
		}

		public void CreateTable()
		{
			StringBuilder query = new StringBuilder("CREATE TABLE sysGlobal_UserPreferences ");
			query.Append(" (GreyFoxUserPreferenceID COUNTER(1,1) CONSTRAINT GreyFoxUserPreferenceID PRIMARY KEY, " + 
				"UserID LONG," +
				"Name TEXT(75), " +
				"Value TEXT(255));");

			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query.ToString(), dbConnection);
			dbConnection.Open();
			dbCommand.ExecuteNonQuery();

			dbConnection.Close();
		}

		#endregion

		#region Cache Methods

		static System.Web.Caching.Cache _webCache;

		public static void CatchWebCache()
		{
			if(_webCache == null && HttpContext.Current != null)
			{
				_webCache = HttpContext.Current.Cache;
			}
		}

		private static void cacheStore(GreyFoxUserPreference greyFoxUserPreference)
		{
			CatchWebCache();
			if(_webCache == null) return;
			_webCache.Insert("GFX_GreyFoxUserPreference_" + greyFoxUserPreference.ID.ToString(), greyFoxUserPreference.Copy(true), null, DateTime.MaxValue, TimeSpan.FromSeconds(1800), CacheItemPriority.Normal, null);
		}

		private static GreyFoxUserPreference cacheFind(int id)
		{
			CatchWebCache();
			if(_webCache == null) return null;
			return (GreyFoxUserPreference) _webCache["GFX_GreyFoxUserPreference_" + id.ToString()];
		}

		private static void cacheRemove(int id)
		{
			CatchWebCache();
			if(_webCache == null) return;
			_webCache.Remove("GFX_GreyFoxUserPreference_" + id.ToString());
		}

		#endregion

	}
}

