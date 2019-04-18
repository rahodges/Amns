/* ********************************************************** *
 * AMNS DbModel v1.0 DAABManager Data Tier                   *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
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
	public class GreyFoxUserPreferenceManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

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
			"PrefValue"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "GreyFoxUserPreferenceID", "LONG", "-1" },
			{ "UserID", "LONG", "null" },
			{ "Name", "TEXT(75)", "" },
			{ "PrefValue", "TEXT(255)", "" }
		};

		#endregion

		#region Default DbModel Constructors

		static GreyFoxUserPreferenceManager()
		{
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
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO sysGlobal_UserPreferences " +
				"(" +
				"UserID," +
				"Name," +
				"PrefValue) VALUES (" +
				"@UserID," +
				"@Name," +
				"@PrefValue);";

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Access
				// Connection must remain open for IDENTITY to return correct value,
				// therefore use the dbCommand object's Connection directly to control
				// connection state.
				dbCommand = database.GetSqlStringCommand(query);
				fillParameters(database, dbCommand, greyFoxUserPreference);
				dbCommand.Connection = database.CreateConnection();
				dbCommand.Connection.Open();
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "SELECT @@IDENTITY AS LastID";
				id = (int)dbCommand.ExecuteScalar();
				dbCommand.Connection.Close();
			}
			else
			{
				//// Microsoft SQL Server
				dbCommand = database.GetSqlStringCommand(query + " SELECT @LastID = SCOPE_IDENTITY();");
				fillParameters(database, dbCommand, greyFoxUserPreference);
				database.AddOutParameter(dbCommand, "@LastID", DbType.Int32, 10);
				database.ExecuteNonQuery(dbCommand);
				id = (int)dbCommand.Parameters["@LastID"].Value;
			}
			// Store greyFoxUserPreference in cache.
			if(cacheEnabled) cacheStore(greyFoxUserPreference);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(GreyFoxUserPreference greyFoxUserPreference)
		{
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE sysGlobal_UserPreferences SET UserID=@UserID," +
				"Name=@Name," +
				"PrefValue=@PrefValue WHERE GreyFoxUserPreferenceID=@GreyFoxUserPreferenceID;");

			fillParameters(database, dbCommand, greyFoxUserPreference);
			database.AddInParameter(dbCommand, "GreyFoxUserPreferenceID", DbType.Int32, greyFoxUserPreference.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			// Store greyFoxUserPreference in cache.
			if (cacheEnabled) cacheStore(greyFoxUserPreference);

			return greyFoxUserPreference.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, GreyFoxUserPreference greyFoxUserPreference)
		{
			#region New Folder

			if(greyFoxUserPreference.user == null)
			{
				addParameter(database, dbCommand, "@UserID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "@UserID", DbType.Int32, greyFoxUserPreference.user.ID);
			}
			addParameter(database, dbCommand, "@Name", DbType.String, greyFoxUserPreference.name);
			addParameter(database, dbCommand, "@PrefValue", DbType.String, greyFoxUserPreference.prefValue);

			#endregion

		}

		#endregion

		#region Parameters

		private static void addParameter(Database database, DbCommand command,
			string name, DbType dbType)
		{
			database.AddInParameter(command, name, dbType);
		}

		private static void addParameter(Database database, DbCommand command,
			string name, DbType dbType, object value)
		{
			database.AddInParameter(command, name, dbType, value);
		}

		private static void addParameter(Database database, DbCommand command,
			string name, DbType dbType, object value, object nullValue)
		{
			if (value == null)
				database.AddInParameter(command, name, dbType, nullValue);
			else
				database.AddInParameter(command, name, dbType, value);
		}

		private static void addParameter(Database database, DbCommand command,
			string name, DbType dbType, object value, object nullValue, object nullSubValue)
		{
			if (value == null || value == nullSubValue)
				database.AddInParameter(command, name, dbType, nullValue);
			else
				database.AddInParameter(command, name, dbType, value);
		}

		#endregion

		#region Default DbModel Fill Method

		internal static bool _fill(GreyFoxUserPreference greyFoxUserPreference)
		{
			// Clone item from cache.
			if(cacheEnabled)
			{
				object cachedObject = cacheFind(greyFoxUserPreference.iD);
				if(cachedObject != null)
				{
					((GreyFoxUserPreference)cachedObject).CopyTo(greyFoxUserPreference, true);
					return greyFoxUserPreference.isSynced;
				}
			}

			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM sysGlobal_UserPreferences WHERE GreyFoxUserPreferenceID=");
			query.Append(greyFoxUserPreference.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find GreyFoxUserPreferenceID '{0}'.", 
					greyFoxUserPreference.iD)));
			}

			FillFromReader(greyFoxUserPreference, r, 0, 1);

			// Microsoft DAAB still needs to have the reader closed.
			r.Close();

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
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			GreyFoxUserPreferenceCollection greyFoxUserPreferenceCollection;

			int innerJoinOffset;

			query = new StringBuilder("SELECT ");

			if(topCount > 0)
			{
				query.Append("TOP ");
				query.Append(topCount);
				query.Append(" ");
			}

			foreach(string columnName in InnerJoinFields)
			{
				query.Append("GreyFoxUserPreference.");
				query.Append(columnName);
				query.Append(",");
			}

			innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;
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
								query.Append("User.");
								query.Append(GreyFoxUserManager.InnerJoinFields[i]);
								query.Append(",");
							}
							userOffset = innerJoinOffset;
							innerJoinOffset = userOffset + GreyFoxUserManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case GreyFoxUserPreferenceFlags.UserContact:
							for(int i = 0; i <= GreyFoxContactManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("User_Contact.");
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

				query.Append("sysGlobal_UserPreferences AS GreyFoxUserPreference");
			}
			else
			{
				query.Append(" FROM sysGlobal_UserPreferences AS GreyFoxUserPreference");
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
							query.Append(" LEFT JOIN sysGlobal_Users AS User ON GreyFoxUserPreference.UserID = User.GreyFoxUserID)");
							break;
						case GreyFoxUserPreferenceFlags.UserContact:
							query.Append(" LEFT JOIN sysGlobal_Contacts AS User_Contact ON User.ContactID = User_Contact.GreyFoxContactID)");
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
			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			#if DEBUG

			try
			{
				r = database.ExecuteReader(dbCommand);
			}
			catch (Exception e)
			{
				string msg = e.Message;
				throw(new Exception(msg + " --- Query: " + query.ToString()));

			}
			#else

			r = database.ExecuteReader(dbCommand);

			#endif

			greyFoxUserPreferenceCollection = new GreyFoxUserPreferenceCollection();

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

			// Microsoft DAAB still needs to close readers.
			r.Close();

			return greyFoxUserPreferenceCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static GreyFoxUserPreference ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			GreyFoxUserPreference greyFoxUserPreference = new GreyFoxUserPreference();
			FillFromReader(greyFoxUserPreference, r, idOffset, dataOffset);
			return greyFoxUserPreference;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(GreyFoxUserPreference greyFoxUserPreference, IDataReader r, int idOffset, int dataOffset)
		{
			greyFoxUserPreference.iD = r.GetInt32(idOffset);
			greyFoxUserPreference.isSynced = true;
			greyFoxUserPreference.isPlaceHolder = false;

			if(!r.IsDBNull(0+dataOffset) && r.GetInt32(0+dataOffset) > 0)
			{
				greyFoxUserPreference.user = GreyFoxUser.NewPlaceHolder(r.GetInt32(0+dataOffset));
			}
			greyFoxUserPreference.name = r.GetString(1+dataOffset);
			greyFoxUserPreference.prefValue = r.GetString(2+dataOffset);
		}

		#endregion

		#region Default DbModel Fill Methods

		#endregion

		#region Default DbModel Delete Method

		internal static void _delete(int id)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("DELETE FROM sysGlobal_UserPreferences WHERE GreyFoxUserPreferenceID=");
			query.Append(id);
			query.Append(';');

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			cacheRemove(id);
		}

		#endregion

		#region Verify Table Methods

		public string VerifyTable(bool repair)
		{
			Database database;
			DbConnection dbConnection;
			DbCommand dbCommand;
			bool match;
			string[] restrictions1;
			StringBuilder msg;

			msg = new StringBuilder();
			restrictions1 = new string[] { null, null, tableName, null };

			database = DatabaseFactory.CreateDatabase();
			dbConnection = database.CreateConnection();
			dbConnection.Open();

			System.Data.DataTable schemaTable = dbConnection.GetSchema("Columns", restrictions1);

			// Loop through the join fields and columns in the
			// table schema to find which fields are missing.
			// Note that this search cannot use BinarySearch due
			// to the fact that JoinFields is unsorted.
			// A sorted JoinFields need not be used because this
			// method should be used sparingly.

			for(int i = 0; i <= JoinFields.GetUpperBound(0); i++)
			{
				match = false;
				foreach(System.Data.DataRow row in schemaTable.Rows)
				{
					if(JoinFields[i,0] == row[3].ToString())
					{
						match = true;
						break;
					}
				}
				if(!match)
				{
					if(repair)
					{
						dbCommand = database.GetSqlStringCommand("ALTER TABLE " + tableName + " ADD COLUMN " + JoinFields[i,0] + " " + JoinFields[i,1] + ";");
						database.ExecuteNonQuery(dbCommand);
						msg.AppendFormat("Added column '{0}'.", JoinFields[i,0]);
					}
					else
					{
						msg.AppendFormat("Missing column '{0}'.", JoinFields[i,0]);
					}
				}
			}

			dbConnection.Close();
			return msg.ToString();
		}

		#endregion

		#region Default DbModel Create Table Methods

		public void CreateReferences()
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder();
			database = DatabaseFactory.CreateDatabase();
			query.Append("ALTER TABLE sysGlobal_UserPreferences ADD ");
			query.Append(" CONSTRAINT FK_sysGlobal_UserPreferences_User FOREIGN KEY (UserID) REFERENCES sysGlobal_Users (GreyFoxUserID);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);
		}

		public void CreateTable()
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Jet SQL
				query = new StringBuilder("CREATE TABLE sysGlobal_UserPreferences ");
				query.Append(" (GreyFoxUserPreferenceID COUNTER(1,1) CONSTRAINT PK_sysGlobal_UserPreferences PRIMARY KEY, " +
					"UserID LONG," +
					"Name TEXT(75)," +
					"PrefValue TEXT(255));");
			}
			else
			{
				// Microsoft SQL Server
				query = new StringBuilder("CREATE TABLE sysGlobal_UserPreferences ");
				query.Append(" (GreyFoxUserPreferenceID INT IDENTITY(1,1) CONSTRAINT PK_sysGlobal_UserPreferences PRIMARY KEY, " +
					"UserID INT," +
					"Name NVARCHAR(75)," +
					"PrefValue NVARCHAR(255));");
			}

			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

		#region Cache Methods

		private static void cacheStore(GreyFoxUserPreference greyFoxUserPreference)
		{
			CacheManager cache = CacheFactory.GetCacheManager();
			cache.Add("sysGlobal_UserPreferences_" + greyFoxUserPreference.iD.ToString(), greyFoxUserPreference);
		}

		private static GreyFoxUserPreference cacheFind(int id)
		{
			object cachedObject;
			CacheManager cache = CacheFactory.GetCacheManager();
			cachedObject = cache.GetData("sysGlobal_UserPreferences_" + id.ToString());
			if(cachedObject == null)
				return null;
			return (GreyFoxUserPreference)cachedObject;
		}

		private static void cacheRemove(int id)
		{
			CacheManager cache = CacheFactory.GetCacheManager();
			cache.Remove("sysGlobal_UserPreferences_" + id.ToString());
		}

		#endregion

	}
}

