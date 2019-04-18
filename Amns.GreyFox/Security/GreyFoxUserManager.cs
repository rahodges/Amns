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
using Amns.GreyFox.People;

namespace Amns.GreyFox.Security
{
	#region Child Flags Enumeration

	public enum GreyFoxUserFlags : int { Contact};

	#endregion

	/// <summary>
	/// Datamanager for GreyFoxUser objects.
	/// </summary>
	public class GreyFoxUserManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "sysGlobal_Users";
		public static readonly string ContactTable = "sysGlobal_Contacts";


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
			"GreyFoxUserID",
			"UserName",
			"IsDisabled",
			"LoginDate",
			"LoginCount",
			"LoginPassword",
			"ContactID",
			"ActivationID"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "GreyFoxUserID", "LONG", "-1" },
			{ "UserName", "TEXT(25)", "" },
			{ "IsDisabled", "BIT", "" },
			{ "LoginDate", "DATETIME", "" },
			{ "LoginCount", "LONG", "" },
			{ "LoginPassword", "TEXT(50)", "" },
			{ "ContactID", "LONG", "null" },
			{ "ActivationID", "TEXT(25)", "string.Empty" }
		};

		#endregion

		#region Default DbModel Constructors

		static GreyFoxUserManager()
		{
		}

		public GreyFoxUserManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!GreyFoxUserManager.isInitialized)
			{
				GreyFoxUserManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a GreyFoxUser into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_GreyFoxUser">The GreyFoxUser to insert into the database.</param>
		internal static int _insert(GreyFoxUser greyFoxUser)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO sysGlobal_Users " +
				"(" +
				"UserName," +
				"IsDisabled," +
				"LoginDate," +
				"LoginCount," +
				"LoginPassword," +
				"ContactID," +
				"ActivationID) VALUES (" +
				"@UserName," +
				"@IsDisabled," +
				"@LoginDate," +
				"@LoginCount," +
				"@LoginPassword," +
				"@ContactID," +
				"@ActivationID);";

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Access
				// Connection must remain open for IDENTITY to return correct value,
				// therefore use the dbCommand object's Connection directly to control
				// connection state.
				dbCommand = database.GetSqlStringCommand(query);
				fillParameters(database, dbCommand, greyFoxUser);
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
				fillParameters(database, dbCommand, greyFoxUser);
				database.AddOutParameter(dbCommand, "@LastID", DbType.Int32, 10);
				database.ExecuteNonQuery(dbCommand);
				id = (int)dbCommand.Parameters["@LastID"].Value;
			}

			// Save child relationships for Roles.
			if(greyFoxUser.roles != null)
			{
				dbCommand = database.GetSqlStringCommand("INSERT INTO sysGlobal_UsersChildren_Roles " +
					"(GreyFoxUserID, GreyFoxRoleID)" + 
					" VALUES (@GreyFoxUserID, @GreyFoxRoleID);");
				addParameter(database, dbCommand, "@GreyFoxUserID", DbType.Int32);
				addParameter(database, dbCommand, "@GreyFoxRoleID", DbType.Int32);
				foreach(GreyFoxRole item in greyFoxUser.roles)
				{
					dbCommand.Parameters["@GreyFoxUserID"].Value = id;
					dbCommand.Parameters["@GreyFoxRoleID"].Value = item.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}
			// Store greyFoxUser in cache.
			if(cacheEnabled) cacheStore(greyFoxUser);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(GreyFoxUser greyFoxUser)
		{
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE sysGlobal_Users SET UserName=@UserName," +
				"IsDisabled=@IsDisabled," +
				"LoginDate=@LoginDate," +
				"LoginCount=@LoginCount," +
				"LoginPassword=@LoginPassword," +
				"ContactID=@ContactID," +
				"ActivationID=@ActivationID WHERE GreyFoxUserID=@GreyFoxUserID;");

			fillParameters(database, dbCommand, greyFoxUser);
			database.AddInParameter(dbCommand, "GreyFoxUserID", DbType.Int32, greyFoxUser.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			if(greyFoxUser.roles != null)
			{

				// Delete child relationships for Roles.
				dbCommand = database.GetSqlStringCommand("DELETE  FROM sysGlobal_UsersChildren_Roles WHERE GreyFoxUserID=@GreyFoxUserID;");
				database.AddInParameter(dbCommand, "@GreyFoxUserID", DbType.Int32, greyFoxUser.iD);
				database.ExecuteNonQuery(dbCommand);

				// Save child relationships for Roles.
				dbCommand = database.GetSqlStringCommand("INSERT INTO sysGlobal_UsersChildren_Roles (GreyFoxUserID, GreyFoxRoleID) VALUES (@GreyFoxUserID, @GreyFoxRoleID);");
				database.AddInParameter(dbCommand, "@GreyFoxUserID", DbType.Int32, greyFoxUser.iD);
				database.AddInParameter(dbCommand, "@GreyFoxRoleID", DbType.Int32);
				foreach(GreyFoxRole greyFoxRole in greyFoxUser.roles)
				{
					dbCommand.Parameters["@GreyFoxRoleID"].Value = greyFoxRole.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}

			// Store greyFoxUser in cache.
			if (cacheEnabled) cacheStore(greyFoxUser);

			return greyFoxUser.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, GreyFoxUser greyFoxUser)
		{
			#region Default

			addParameter(database, dbCommand, "@UserName", DbType.String, greyFoxUser.userName);
			addParameter(database, dbCommand, "@IsDisabled", DbType.Boolean, greyFoxUser.isDisabled);
			addParameter(database, dbCommand, "@LoginDate", DbType.Date, greyFoxUser.loginDate);
			addParameter(database, dbCommand, "@LoginCount", DbType.Int32, greyFoxUser.loginCount);
			addParameter(database, dbCommand, "@LoginPassword", DbType.String, greyFoxUser.loginPassword);
			if(greyFoxUser.contact == null)
			{
				addParameter(database, dbCommand, "@ContactID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "@ContactID", DbType.Int32, greyFoxUser.contact.ID);
			}
			addParameter(database, dbCommand, "@ActivationID", DbType.String, greyFoxUser.activationID);

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

		internal static bool _fill(GreyFoxUser greyFoxUser)
		{
			// Clone item from cache.
			if(cacheEnabled)
			{
				object cachedObject = cacheFind(greyFoxUser.iD);
				if(cachedObject != null)
				{
					((GreyFoxUser)cachedObject).CopyTo(greyFoxUser, true);
					return greyFoxUser.isSynced;
				}
			}

			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM sysGlobal_Users WHERE GreyFoxUserID=");
			query.Append(greyFoxUser.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find GreyFoxUserID '{0}'.", 
					greyFoxUser.iD)));
			}

			FillFromReader(greyFoxUser, r, 0, 1);

			// Microsoft DAAB still needs to have the reader closed.
			r.Close();

			// Store greyFoxUser in cache.
			if(cacheEnabled) cacheStore(greyFoxUser);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public GreyFoxUserCollection GetCollection(string whereClause, string sortClause, params GreyFoxUserFlags[] optionFlags)
		{
			return GetCollection(0, whereClause, sortClause, optionFlags);
		}

		public GreyFoxUserCollection GetCollection(int topCount, string whereClause, string sortClause, params GreyFoxUserFlags[] optionFlags)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			GreyFoxUserCollection greyFoxUserCollection;

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
				query.Append("GreyFoxUser.");
				query.Append(columnName);
				query.Append(",");
			}

			innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;
			int contactOffset = -1;

			//
			// Append Option Flag Fields
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case GreyFoxUserFlags.Contact:
							for(int i = 0; i <= GreyFoxContactManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("Contact.");
								query.Append(GreyFoxContactManager.InnerJoinFields[i]);
								query.Append(",");
							}
							contactOffset = innerJoinOffset;
							innerJoinOffset = contactOffset + GreyFoxContactManager.InnerJoinFields.GetUpperBound(0) + 1;
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

				query.Append("sysGlobal_Users AS GreyFoxUser");
			}
			else
			{
				query.Append(" FROM sysGlobal_Users AS GreyFoxUser");
			}
			//
			// Finish INNER JOIN expressions
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case GreyFoxUserFlags.Contact:
							query.Append(" LEFT JOIN sysGlobal_Contacts AS Contact ON GreyFoxUser.ContactID = Contact.GreyFoxContactID)");
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

			greyFoxUserCollection = new GreyFoxUserCollection();

			while(r.Read())
			{
				GreyFoxUser greyFoxUser = ParseFromReader(r, 0, 1);

				// Fill Contact
				if(contactOffset != -1 && !r.IsDBNull(contactOffset))
					GreyFoxContactManager.FillFromReader(greyFoxUser.contact, "sysGlobal_Contacts", r, contactOffset, contactOffset+1);

				greyFoxUserCollection.Add(greyFoxUser);
			}

			// Microsoft DAAB still needs to close readers.
			r.Close();

			return greyFoxUserCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static GreyFoxUser ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			GreyFoxUser greyFoxUser = new GreyFoxUser();
			FillFromReader(greyFoxUser, r, idOffset, dataOffset);
			return greyFoxUser;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(GreyFoxUser greyFoxUser, IDataReader r, int idOffset, int dataOffset)
		{
			greyFoxUser.iD = r.GetInt32(idOffset);
			greyFoxUser.isSynced = true;
			greyFoxUser.isPlaceHolder = false;

			greyFoxUser.userName = r.GetString(0+dataOffset);
			if(!r.IsDBNull(1+dataOffset)) 
				greyFoxUser.isDisabled = r.GetBoolean(1+dataOffset);
			if(!r.IsDBNull(2+dataOffset)) 
				greyFoxUser.loginDate = r.GetDateTime(2+dataOffset);
			else
				greyFoxUser.loginDate = DateTime.MinValue;
			if(!r.IsDBNull(3+dataOffset)) 
				greyFoxUser.loginCount = r.GetInt32(3+dataOffset);
			if(!r.IsDBNull(4+dataOffset)) 
				greyFoxUser.loginPassword = r.GetString(4+dataOffset);
			else
				greyFoxUser.loginPassword = null;
			if(!r.IsDBNull(5+dataOffset) && r.GetInt32(5+dataOffset) > 0)
			{
				greyFoxUser.contact = GreyFoxContact.NewPlaceHolder("sysGlobal_Contacts", r.GetInt32(5+dataOffset));
			}
			if(!r.IsDBNull(6+dataOffset)) 
				greyFoxUser.activationID = r.GetString(6+dataOffset);
			else
				greyFoxUser.activationID = string.Empty;
		}

		#endregion

		#region Default DbModel Fill Methods

		public static void FillRoles(GreyFoxUser greyFoxUser)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			s = new StringBuilder("SELECT GreyFoxRoleID FROM sysGlobal_UsersChildren_Roles ");
			s.Append("WHERE GreyFoxUserID=");
			s.Append(greyFoxUser.iD);
			s.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(s.ToString());
			r = database.ExecuteReader(dbCommand);

			GreyFoxRoleCollection roles;
			if(greyFoxUser.roles != null)
			{
				roles = greyFoxUser.roles;
				roles.Clear();
			}
			else
			{
				roles = new GreyFoxRoleCollection();
				greyFoxUser.roles = roles;
			}

			while(r.Read())
				roles.Add(GreyFoxRole.NewPlaceHolder(r.GetInt32(0)));

			greyFoxUser.Roles = roles;
			// Store GreyFoxUser in cache.
			if(cacheEnabled) cacheStore(greyFoxUser);
		}

		public static void FillRoles(GreyFoxUserCollection greyFoxUserCollection)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			if(greyFoxUserCollection.Count > 0)
			{
				s = new StringBuilder("SELECT GreyFoxUserID, GreyFoxRoleID FROM sysGlobal_UsersChildren_Roles ORDER BY GreyFoxUserID; ");

				// Clone and sort collection by ID first to fill children in one pass
				GreyFoxUserCollection clonedCollection = greyFoxUserCollection.Clone();
				clonedCollection.Sort();

				database = DatabaseFactory.CreateDatabase();
				dbCommand = database.GetSqlStringCommand(s.ToString());
				r = database.ExecuteReader(dbCommand);

				bool more = r.Read();

				foreach(GreyFoxUser greyFoxUser in clonedCollection)
				{
					GreyFoxRoleCollection roles;
					if(greyFoxUser.roles != null)
					{
						roles = greyFoxUser.roles;
						roles.Clear();
					}
					else
					{
						roles = new GreyFoxRoleCollection();
						greyFoxUser.roles = roles;
					}

					while(more)
					{
						if(r.GetInt32(0) < greyFoxUser.iD)
						{
							more = r.Read();
						}
						else if(r.GetInt32(0) == greyFoxUser.iD)
						{
							roles.Add(GreyFoxRole.NewPlaceHolder(r.GetInt32(1)));
							more = r.Read();
						}
						else
						{
							break;
						}
					}

					// No need to continue if there are no more records
					if(!more) break;
				}

			}
		}

		#endregion

		#region Default DbModel Delete Method

		internal static void _delete(int id)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("DELETE FROM sysGlobal_Users WHERE GreyFoxUserID=");
			query.Append(id);
			query.Append(';');

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);


			// Delete child relationships for Roles.
			query.Length = 0;
			query.Append("DELETE FROM sysGlobal_UsersChildren_Roles WHERE ");
			query.Append("GreyFoxUserID=");
			query.Append(id);
			query.Append(";");
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

			GreyFoxContactManager contactManager = 
				new GreyFoxContactManager("sysGlobal_Contacts");
			msg.Append(contactManager.VerifyTable(repair));

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
			query.Append("ALTER TABLE sysGlobal_Users ADD ");
			query.Append(" CONSTRAINT FK_sysGlobal_Users_Contact FOREIGN KEY (ContactID) REFERENCES sysGlobal_Contacts (GreyFoxContactID);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			query.Length = 0;
			query.Append("ALTER TABLE sysGlobal_UsersChildren_Roles ADD");
			query.Append(" CONSTRAINT FK_sysGlobal_Users_sysGlobal_UsersChildren_Roles FOREIGN KEY (GreyFoxUserID) REFERENCES sysGlobal_Users (GreyFoxUserID) ON DELETE CASCADE, ");
			query.Append(" CONSTRAINT FK_sysGlobal_UsersChildren_Roles_sysGlobal_Roles FOREIGN KEY (GreyFoxRoleID) REFERENCES sysGlobal_Roles (GreyFoxRoleID) ON DELETE CASCADE;");
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
				query = new StringBuilder("CREATE TABLE sysGlobal_Users ");
				query.Append(" (GreyFoxUserID COUNTER(1,1) CONSTRAINT PK_sysGlobal_Users PRIMARY KEY, " +
					"UserName TEXT(25) CONSTRAINT UniqueUserName UNIQUE," +
					"IsDisabled BIT," +
					"LoginDate DATETIME," +
					"LoginCount LONG," +
					"LoginPassword TEXT(50)," +
					"ContactID LONG," +
					"ActivationID TEXT(25));");
			}
			else
			{
				// Microsoft SQL Server
				query = new StringBuilder("CREATE TABLE sysGlobal_Users ");
				query.Append(" (GreyFoxUserID INT IDENTITY(1,1) CONSTRAINT PK_sysGlobal_Users PRIMARY KEY, " +
					"UserName NVARCHAR(25) CONSTRAINT UniqueUserName UNIQUE," +
					"IsDisabled BIT," +
					"LoginDate DATETIME," +
					"LoginCount INT," +
					"LoginPassword NVARCHAR(50)," +
					"ContactID INT," +
					"ActivationID NVARCHAR(25));");
			}

			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			//
			// Create object level table for Contact.
			//
			GreyFoxContactManager contactManager = new GreyFoxContactManager("sysGlobal_Contacts");
			contactManager.CreateTable();

			//
			// Create children table for Roles.
			//
			query.Length = 0;
			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				query.Append("CREATE TABLE sysGlobal_UsersChildren_Roles ");
				query.Append("(GreyFoxUserID LONG, GreyFoxRoleID LONG);");
				dbCommand = database.GetSqlStringCommand(query.ToString());
				database.ExecuteNonQuery(dbCommand);

			}
			else
			{
				query.Append("CREATE TABLE sysGlobal_UsersChildren_Roles ");
				query.Append("(GreyFoxUserID INT, GreyFoxRoleID INT);");
				dbCommand = database.GetSqlStringCommand(query.ToString());
				database.ExecuteNonQuery(dbCommand);

			}
		}

		#endregion

		#region Cache Methods

		private static void cacheStore(GreyFoxUser greyFoxUser)
		{
			CacheManager cache = CacheFactory.GetCacheManager();
			cache.Add("sysGlobal_Users_" + greyFoxUser.iD.ToString(), greyFoxUser);
		}

		private static GreyFoxUser cacheFind(int id)
		{
			object cachedObject;
			CacheManager cache = CacheFactory.GetCacheManager();
			cachedObject = cache.GetData("sysGlobal_Users_" + id.ToString());
			if(cachedObject == null)
				return null;
			return (GreyFoxUser)cachedObject;
		}

		private static void cacheRemove(int id)
		{
			CacheManager cache = CacheFactory.GetCacheManager();
			cache.Remove("sysGlobal_Users_" + id.ToString());
		}

		#endregion

		//--- Begin Custom Code ---
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																										
		public static bool Exists(string userName)
		{
            StringBuilder query;
            Database database;
            DbCommand dbCommand;

			query = new StringBuilder("SELECT COUNT(*) FROM sysGlobal_Users WHERE Username='");
			query.Append(userName);
			query.Append("';");

            database = DatabaseFactory.CreateDatabase();
            dbCommand = database.GetSqlStringCommand(query.ToString());
            int countValue = (int)database.ExecuteScalar(dbCommand);
			return countValue != 0;
		}

		public GreyFoxUser GetByUsername(string userName)
		{
			userName = userName.Replace("'", "''");

			GreyFoxUserCollection results = 
                GetCollection("UserName='" + userName + "'", string.Empty, null);
			if(results.Count > 0)
				return results[0];
			else
				throw(new Exception(string.Format("Cannot find username '{0}'.", 
                    userName)));
		}

		public GreyFoxUser GetByEmailAddress(string emailAddress)
		{
			emailAddress = emailAddress.Replace("'", "''");

			GreyFoxUserCollection results = GetCollection("Email1='" + emailAddress + "'", string.Empty, GreyFoxUserFlags.Contact);
			if(results.Count > 0)
				return results[0];
			else
				throw(new Exception(string.Format("Cannot find user with email address '{0}'.", emailAddress)));
		}

		public void ChangePassword(GreyFoxUser user, string newPassword)
		{
			string ipAddress = "No Web Server";
			string clientDetails = "No Web Server";
			if(System.Web.HttpContext.Current != null)
			{
				ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
				clientDetails = System.Web.HttpContext.Current.Request.UserAgent;
			}

            // Log an event
			Amns.GreyFox.EventLog.GreyFoxEvent e = 
				new Amns.GreyFox.EventLog.GreyFoxEvent("sysGlobal_Events");
			e.Category = "Login";
			e.Description = "Lost password; username '" +
				user.UserName + "'.<BR>" +
//				"Old Password: " + user.LoginPassword + "<BR>" +
//				"New Password: " + newPassword + "<BR>" +
				"IP Address: " + ipAddress + "<BR>" +
				"Client: " + clientDetails;
			e.EventDate = DateTime.Now;
			e.EventID = 25201;
			e.Source = "AUDITOR";
			e.Type = 105;
			e.User = user;
			e.Save();

			user.LoginPassword = newPassword;
			user.Encrypt();
			user.Save();
		}

		/// <summary>
		/// Logs in a user to the system and optionally logs success and failures to the audit log.
		/// </summary>
		/// <returns>Returns associated user if one exists, or returns null if no user exists.</returns>
		public GreyFoxUser Login(string username, string password, string ipAddress, 
			string clientDetails, bool logSuccess, bool logFailure)
		{
			GreyFoxUser user = null;

			try
			{
				user = GetByUsername(username);
			}
			catch
			{
				if(logFailure)
				{
					Amns.GreyFox.EventLog.GreyFoxEvent e = 
						new Amns.GreyFox.EventLog.GreyFoxEvent("sysGlobal_Events");
					e.Category = "Login";
					e.Description = "Login failure; invalid username '" +
						username + "'.<BR>" +
						"IP Address: " + ipAddress + "<BR>" +
						"Client: " + clientDetails;
					e.EventDate = DateTime.Now;
					e.EventID = 25104;
					e.Source = "AUDITOR";
					e.Type = 105;
					e.User = null;
					e.Save();
				}

				throw(new Exception("Login failure; invalid username."));
			}

			if(GreyFoxPassword.DecodePassword(user.loginPassword).ToLower() != 
				password.ToLower())
			{
				if(logFailure)
				{
					Amns.GreyFox.EventLog.GreyFoxEvent e = 
						new Amns.GreyFox.EventLog.GreyFoxEvent("sysGlobal_Events");
					e.Category = "Login";
					e.Description = "Login failure; incorrect password for '" + username + "'.<BR>" +
						"Password used '" + password + "'.<BR>" +
						"IP Address: " + ipAddress + "<BR>" +
						"Client: " + clientDetails;
					e.EventDate = DateTime.Now;
					e.EventID = 25105;
					e.Source = "AUDITOR";
					e.Type = 105;
					e.User = user;
					e.Save();

					user.LoginCount++;
					user.Save();

					// Delay the user 15 seconds if he's tried in the last 24 hours
					if(user.LoginCount == 4)
					{
						System.Threading.Thread.Sleep(15 * 1000);
					}
					// Delay the user 15 seconds + 5 second increments
					else if(user.LoginCount > 5 & user.LoginCount <= 11)
					{
						System.Threading.Thread.Sleep(5 * 1000 * user.LoginCount + 15 * 1000);
					}
					// Delay the user 15 seconds + 10 second increments
					else if(user.LoginCount > 11)
					{
						System.Threading.Thread.Sleep(15 * 1000 * user.LoginCount + 15 * 1000);
					}
				}
				throw(new Exception("Login failure; incorrect password."));
			}
			if(user.isDisabled)
			{
				if(logFailure)
				{
					Amns.GreyFox.EventLog.GreyFoxEvent e = 
						new Amns.GreyFox.EventLog.GreyFoxEvent("sysGlobal_Events");
					e.Category = "Login";
					e.Description = "Login failure; '" + username + "' disabled.<BR>" +
						"IP Address: " + ipAddress + "<BR>" +
						"Client: " + clientDetails;
					e.EventDate = DateTime.Now;
					e.EventID = 25110;
					e.Source = "AUDITOR";
					e.Type = 105;
					e.User = user;
					e.Save();
				}

				throw(new Exception("Login failure; user disabled."));
			}

			if(logSuccess)
			{
				Amns.GreyFox.EventLog.GreyFoxEvent e = 
					new Amns.GreyFox.EventLog.GreyFoxEvent("sysGlobal_Events");
				e.Category = "Login";
				e.Description = "Login success; '" + username + "'.<BR>" + 
					"IP Address: " + ipAddress + "<BR>" +
					"Client: " + clientDetails;
				e.EventDate = DateTime.Now;
				e.EventID = 25001;
				e.Source = "AUDITOR";
				e.Type = 100;
				e.User = user;
				e.Save();

				// Delay the user 15 seconds if he's tried in the last 24 hours
				if(user.LoginCount == 4)
				{
					System.Threading.Thread.Sleep(15 * 1000);
				}
				// Delay the user 15 seconds + 5 second increments
				else if(user.LoginCount > 5 & user.LoginCount <= 11)
				{
					System.Threading.Thread.Sleep(5 * 1000 * user.LoginCount + 15 * 1000);
				}
				// Delay the user 15 seconds + 10 second increments
				else if(user.LoginCount > 11)
				{
					System.Threading.Thread.Sleep(15 * 1000 * user.LoginCount + 15 * 1000);
				}
			}

			user.loginCount = 1;
			user.loginDate = DateTime.Now;
			user.isSynced = false;
			user.Save();

			return user;
		}

		public GreyFoxUserCollection DecodeString(string users, string separator)
		{			
			GreyFoxUserCollection encodedUsers = new GreyFoxUserCollection();

			users = users.Trim();
			if(users.Trim() == string.Empty)
				return encodedUsers;

			users = users.Replace("'", "");
            string[] names = users.Split(new string[] { separator },
                StringSplitOptions.RemoveEmptyEntries);

			if(names.Length < 20)
				return fastDecode(names);

			GreyFoxUserCollection allUsers = GetCollection(string.Empty, string.Empty, null);

			for(int x = 0; x < allUsers.Count; x++)
				for(int y = 0; y <= names.GetUpperBound(0); y++)
					if(allUsers[x].UserName == names[y])
						encodedUsers.Add(allUsers[x]);

			return encodedUsers;
		}

		private GreyFoxUserCollection fastDecode(string[] users)
		{
			StringBuilder query = new StringBuilder();

            for (int x = 0; x <= users.GetUpperBound(0); x++)
            {
                query.Append("GreyFoxUser.Username='");
                query.Append(users[x].Trim());
                query.Append("'");
                if (x != users.GetUpperBound(0))
                    query.Append(" OR ");
            }

            if (query.Length == 0)
            {
                throw (new Exception("No usernames to encode."));
            }

			return GetCollection(query.ToString(), string.Empty, null);
		}

		//--- End Custom Code ---
	}
}

