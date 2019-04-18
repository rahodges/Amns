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

namespace Amns.GreyFox
{
	#region Child Flags Enumeration

	public enum GreyFoxSettingFlags : int { Parent,
				ParentParent,
				ParentModifyRole,
				ModifyRole};

	#endregion

	/// <summary>
	/// Datamanager for GreyFoxSetting objects.
	/// </summary>
	public class GreyFoxSettingManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "sysGlobal_Settings";


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
			"GreyFoxSettingID",
			"Name",
			"SettingValue",
			"ParentID",
			"ModifyRoleID",
			"IsSystemSetting"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "GreyFoxSettingID", "LONG", "-1" },
			{ "Name", "TEXT(75)", "" },
			{ "SettingValue", "TEXT(75)", "" },
			{ "ParentID", "LONG", "null" },
			{ "ModifyRoleID", "LONG", "null" },
			{ "IsSystemSetting", "BIT", "" }
		};

		#endregion

		#region Default DbModel Constructors

		static GreyFoxSettingManager()
		{
		}

		public GreyFoxSettingManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!GreyFoxSettingManager.isInitialized)
			{
				GreyFoxSettingManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a GreyFoxSetting into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_GreyFoxSetting">The GreyFoxSetting to insert into the database.</param>
		internal static int _insert(GreyFoxSetting greyFoxSetting)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO sysGlobal_Settings " +
				"(" +
				"Name," +
				"SettingValue," +
				"ParentID," +
				"ModifyRoleID," +
				"IsSystemSetting) VALUES (" +
				"@Name," +
				"@SettingValue," +
				"@ParentID," +
				"@ModifyRoleID," +
				"@IsSystemSetting);";

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Access
				// Connection must remain open for IDENTITY to return correct value,
				// therefore use the dbCommand object's Connection directly to control
				// connection state.
				dbCommand = database.GetSqlStringCommand(query);
				fillParameters(database, dbCommand, greyFoxSetting);
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
				fillParameters(database, dbCommand, greyFoxSetting);
				database.AddOutParameter(dbCommand, "@LastID", DbType.Int32, 10);
				database.ExecuteNonQuery(dbCommand);
				id = (int)dbCommand.Parameters["@LastID"].Value;
			}
			// Store greyFoxSetting in cache.
			if(cacheEnabled) cacheStore(greyFoxSetting);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(GreyFoxSetting greyFoxSetting)
		{
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE sysGlobal_Settings SET Name=@Name," +
				"SettingValue=@SettingValue," +
				"ParentID=@ParentID," +
				"ModifyRoleID=@ModifyRoleID," +
				"IsSystemSetting=@IsSystemSetting WHERE GreyFoxSettingID=@GreyFoxSettingID;");

			fillParameters(database, dbCommand, greyFoxSetting);
			database.AddInParameter(dbCommand, "GreyFoxSettingID", DbType.Int32, greyFoxSetting.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			// Store greyFoxSetting in cache.
			if (cacheEnabled) cacheStore(greyFoxSetting);

			return greyFoxSetting.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, GreyFoxSetting greyFoxSetting)
		{
			#region General

			addParameter(database, dbCommand, "@Name", DbType.String, greyFoxSetting.name);
			addParameter(database, dbCommand, "@SettingValue", DbType.String, greyFoxSetting.settingValue);
			if(greyFoxSetting.parent == null)
			{
				addParameter(database, dbCommand, "@ParentID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "@ParentID", DbType.Int32, greyFoxSetting.parent.ID);
			}
			if(greyFoxSetting.modifyRole == null)
			{
				addParameter(database, dbCommand, "@ModifyRoleID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "@ModifyRoleID", DbType.Int32, greyFoxSetting.modifyRole.ID);
			}
			addParameter(database, dbCommand, "@IsSystemSetting", DbType.Boolean, greyFoxSetting.isSystemSetting);

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

		internal static bool _fill(GreyFoxSetting greyFoxSetting)
		{
			// Clone item from cache.
			if(cacheEnabled)
			{
				object cachedObject = cacheFind(greyFoxSetting.iD);
				if(cachedObject != null)
				{
					((GreyFoxSetting)cachedObject).CopyTo(greyFoxSetting, true);
					return greyFoxSetting.isSynced;
				}
			}

			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM sysGlobal_Settings WHERE GreyFoxSettingID=");
			query.Append(greyFoxSetting.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find GreyFoxSettingID '{0}'.", 
					greyFoxSetting.iD)));
			}

			FillFromReader(greyFoxSetting, r, 0, 1);

			// Microsoft DAAB still needs to have the reader closed.
			r.Close();

			// Store greyFoxSetting in cache.
			if(cacheEnabled) cacheStore(greyFoxSetting);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public GreyFoxSettingCollection GetCollection(string whereClause, string sortClause, params GreyFoxSettingFlags[] optionFlags)
		{
			return GetCollection(0, whereClause, sortClause, optionFlags);
		}

		public GreyFoxSettingCollection GetCollection(int topCount, string whereClause, string sortClause, params GreyFoxSettingFlags[] optionFlags)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			GreyFoxSettingCollection greyFoxSettingCollection;

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
				query.Append("GreyFoxSetting.");
				query.Append(columnName);
				query.Append(",");
			}

			innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;
			int parentOffset = -1;
			int parentParentOffset = -1;
			int parentModifyRoleOffset = -1;
			int modifyRoleOffset = -1;

			//
			// Append Option Flag Fields
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case GreyFoxSettingFlags.Parent:
							for(int i = 0; i <= GreyFoxSettingManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("Parent.");
								query.Append(GreyFoxSettingManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentOffset = innerJoinOffset;
							innerJoinOffset = parentOffset + GreyFoxSettingManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case GreyFoxSettingFlags.ParentParent:
							for(int i = 0; i <= GreyFoxSettingManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("Parent_Parent.");
								query.Append(GreyFoxSettingManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentParentOffset = innerJoinOffset;
							innerJoinOffset = parentParentOffset + GreyFoxSettingManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case GreyFoxSettingFlags.ParentModifyRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("Parent_ModifyRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentModifyRoleOffset = innerJoinOffset;
							innerJoinOffset = parentModifyRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case GreyFoxSettingFlags.ModifyRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ModifyRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							modifyRoleOffset = innerJoinOffset;
							innerJoinOffset = modifyRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
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

				query.Append("sysGlobal_Settings AS GreyFoxSetting");
			}
			else
			{
				query.Append(" FROM sysGlobal_Settings AS GreyFoxSetting");
			}
			//
			// Finish INNER JOIN expressions
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case GreyFoxSettingFlags.Parent:
							query.Append(" LEFT JOIN sysGlobal_Settings AS Parent ON GreyFoxSetting.ParentID = Parent.GreyFoxSettingID)");
							break;
						case GreyFoxSettingFlags.ParentParent:
							query.Append(" LEFT JOIN sysGlobal_Settings AS Parent_Parent ON Parent.ParentID = Parent_Parent.GreyFoxSettingID)");
							break;
						case GreyFoxSettingFlags.ParentModifyRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS Parent_ModifyRole ON Parent.ModifyRoleID = Parent_ModifyRole.GreyFoxRoleID)");
							break;
						case GreyFoxSettingFlags.ModifyRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS ModifyRole ON GreyFoxSetting.ModifyRoleID = ModifyRole.GreyFoxRoleID)");
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

			greyFoxSettingCollection = new GreyFoxSettingCollection();

			while(r.Read())
			{
				GreyFoxSetting greyFoxSetting = ParseFromReader(r, 0, 1);

				// Fill Parent
				if(parentOffset != -1 && !r.IsDBNull(parentOffset))
				{
					GreyFoxSettingManager.FillFromReader(greyFoxSetting.parent, r, parentOffset, parentOffset+1);

					// Fill 
					if(parentParentOffset != -1 && !r.IsDBNull(parentParentOffset))
						GreyFoxSettingManager.FillFromReader(greyFoxSetting.parent.Parent, r, parentParentOffset, parentParentOffset+1);

					// Fill 
					if(parentModifyRoleOffset != -1 && !r.IsDBNull(parentModifyRoleOffset))
						GreyFoxRoleManager.FillFromReader(greyFoxSetting.parent.ModifyRole, r, parentModifyRoleOffset, parentModifyRoleOffset+1);

				}

				// Fill ModifyRole
				if(modifyRoleOffset != -1 && !r.IsDBNull(modifyRoleOffset))
					GreyFoxRoleManager.FillFromReader(greyFoxSetting.modifyRole, r, modifyRoleOffset, modifyRoleOffset+1);

				greyFoxSettingCollection.Add(greyFoxSetting);
			}

			// Microsoft DAAB still needs to close readers.
			r.Close();

			return greyFoxSettingCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static GreyFoxSetting ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			GreyFoxSetting greyFoxSetting = new GreyFoxSetting();
			FillFromReader(greyFoxSetting, r, idOffset, dataOffset);
			return greyFoxSetting;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(GreyFoxSetting greyFoxSetting, IDataReader r, int idOffset, int dataOffset)
		{
			greyFoxSetting.iD = r.GetInt32(idOffset);
			greyFoxSetting.isSynced = true;
			greyFoxSetting.isPlaceHolder = false;

			greyFoxSetting.name = r.GetString(0+dataOffset);
			greyFoxSetting.settingValue = r.GetString(1+dataOffset);
			if(!r.IsDBNull(2+dataOffset) && r.GetInt32(2+dataOffset) > 0)
			{
				greyFoxSetting.parent = GreyFoxSetting.NewPlaceHolder(r.GetInt32(2+dataOffset));
			}
			if(!r.IsDBNull(3+dataOffset) && r.GetInt32(3+dataOffset) > 0)
			{
				greyFoxSetting.modifyRole = GreyFoxRole.NewPlaceHolder(r.GetInt32(3+dataOffset));
			}
			greyFoxSetting.isSystemSetting = r.GetBoolean(4+dataOffset);
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

			query = new StringBuilder("DELETE FROM sysGlobal_Settings WHERE GreyFoxSettingID=");
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
			query.Append("ALTER TABLE sysGlobal_Settings ADD ");
			query.Append(" CONSTRAINT FK_sysGlobal_Settings_Parent FOREIGN KEY (ParentID) REFERENCES sysGlobal_Settings (GreyFoxSettingID),");
			query.Append(" CONSTRAINT FK_sysGlobal_Settings_ModifyRole FOREIGN KEY (ModifyRoleID) REFERENCES sysGlobal_Roles (GreyFoxRoleID);");
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
				query = new StringBuilder("CREATE TABLE sysGlobal_Settings ");
				query.Append(" (GreyFoxSettingID COUNTER(1,1) CONSTRAINT PK_sysGlobal_Settings PRIMARY KEY, " +
					"Name TEXT(75)," +
					"SettingValue TEXT(75)," +
					"ParentID LONG," +
					"ModifyRoleID LONG," +
					"IsSystemSetting BIT);");
			}
			else
			{
				// Microsoft SQL Server
				query = new StringBuilder("CREATE TABLE sysGlobal_Settings ");
				query.Append(" (GreyFoxSettingID INT IDENTITY(1,1) CONSTRAINT PK_sysGlobal_Settings PRIMARY KEY, " +
					"Name NVARCHAR(75)," +
					"SettingValue NVARCHAR(75)," +
					"ParentID INT," +
					"ModifyRoleID INT," +
					"IsSystemSetting BIT);");
			}

			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

		#region Cache Methods

		private static void cacheStore(GreyFoxSetting greyFoxSetting)
		{
			CacheManager cache = CacheFactory.GetCacheManager();
			cache.Add("sysGlobal_Settings_" + greyFoxSetting.iD.ToString(), greyFoxSetting);
		}

		private static GreyFoxSetting cacheFind(int id)
		{
			object cachedObject;
			CacheManager cache = CacheFactory.GetCacheManager();
			cachedObject = cache.GetData("sysGlobal_Settings_" + id.ToString());
			if(cachedObject == null)
				return null;
			return (GreyFoxSetting)cachedObject;
		}

		private static void cacheRemove(int id)
		{
			CacheManager cache = CacheFactory.GetCacheManager();
			cache.Remove("sysGlobal_Settings_" + id.ToString());
		}

		#endregion

		//--- Begin Custom Code ---

		public static GreyFoxSetting GetSetting(string key)
		{
           return GetSetting(null, key);		
        }

        public static GreyFoxSetting GetSetting(GreyFoxSetting parent, string key)
        {
            GreyFoxSettingManager settingManager;
            GreyFoxSettingCollection settingCollection;

            settingManager = new GreyFoxSettingManager();

            if (parent == null)
            {
                settingCollection = settingManager.GetCollection("Name='" +
                    key + "'", string.Empty, null);
            }
            else
            {
                settingCollection = settingManager.GetCollection("ParentID=" +
                    parent.ID.ToString() +
                    " AND Name='" +
                    key + "'", string.Empty, null);
            }

            if (settingCollection.Count == 1)
            {
                return settingCollection[0];
            }
            else
            {
                return null;
            }
        }

        public static GreyFoxSettingCollection GetSettings(string parentKey)
        {
            GreyFoxSettingManager settingManager =
                new GreyFoxSettingManager();
            GreyFoxSettingCollection settingCollection = 
                settingManager.GetCollection("GreyFoxSetting.Name='" +
                parentKey + "'", "GreyFoxSetting.Name", GreyFoxSettingFlags.Parent);
            return settingCollection;
        }        

		//--- End Custom Code ---
	}
}

