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

namespace Amns.GreyFox.Security
{
	/// <summary>
	/// Datamanager for GreyFoxRole objects.
	/// </summary>
	public class GreyFoxRoleManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "sysGlobal_Roles";


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
			"GreyFoxRoleID",
			"Name",
			"Description",
			"IsDisabled"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "GreyFoxRoleID", "LONG", "-1" },
			{ "Name", "TEXT(25)", "" },
			{ "Description", "MEMO", "" },
			{ "IsDisabled", "BIT", "" }
		};

		#endregion

		#region Default DbModel Constructors

		static GreyFoxRoleManager()
		{
		}

		public GreyFoxRoleManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!GreyFoxRoleManager.isInitialized)
			{
				GreyFoxRoleManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a GreyFoxRole into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_GreyFoxRole">The GreyFoxRole to insert into the database.</param>
		internal static int _insert(GreyFoxRole greyFoxRole)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO sysGlobal_Roles " +
				"(" +
				"Name," +
				"Description," +
				"IsDisabled) VALUES (" +
				"@Name," +
				"@Description," +
				"@IsDisabled);";

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Access
				// Connection must remain open for IDENTITY to return correct value,
				// therefore use the dbCommand object's Connection directly to control
				// connection state.
				dbCommand = database.GetSqlStringCommand(query);
				fillParameters(database, dbCommand, greyFoxRole);
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
				fillParameters(database, dbCommand, greyFoxRole);
				database.AddOutParameter(dbCommand, "@LastID", DbType.Int32, 10);
				database.ExecuteNonQuery(dbCommand);
				id = (int)dbCommand.Parameters["@LastID"].Value;
			}
			// Store greyFoxRole in cache.
			if(cacheEnabled) cacheStore(greyFoxRole);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(GreyFoxRole greyFoxRole)
		{
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE sysGlobal_Roles SET Name=@Name," +
				"Description=@Description," +
				"IsDisabled=@IsDisabled WHERE GreyFoxRoleID=@GreyFoxRoleID;");

			fillParameters(database, dbCommand, greyFoxRole);
			database.AddInParameter(dbCommand, "GreyFoxRoleID", DbType.Int32, greyFoxRole.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			// Store greyFoxRole in cache.
			if (cacheEnabled) cacheStore(greyFoxRole);

			return greyFoxRole.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, GreyFoxRole greyFoxRole)
		{
			#region General

			addParameter(database, dbCommand, "@Name", DbType.String, greyFoxRole.name);
			addParameter(database, dbCommand, "@Description", DbType.String, greyFoxRole.description);
			addParameter(database, dbCommand, "@IsDisabled", DbType.Boolean, greyFoxRole.isDisabled);

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

		internal static bool _fill(GreyFoxRole greyFoxRole)
		{
			// Clone item from cache.
			if(cacheEnabled)
			{
				object cachedObject = cacheFind(greyFoxRole.iD);
				if(cachedObject != null)
				{
					((GreyFoxRole)cachedObject).CopyTo(greyFoxRole, true);
					return greyFoxRole.isSynced;
				}
			}

			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM sysGlobal_Roles WHERE GreyFoxRoleID=");
			query.Append(greyFoxRole.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find GreyFoxRoleID '{0}'.", 
					greyFoxRole.iD)));
			}

			FillFromReader(greyFoxRole, r, 0, 1);

			// Microsoft DAAB still needs to have the reader closed.
			r.Close();

			// Store greyFoxRole in cache.
			if(cacheEnabled) cacheStore(greyFoxRole);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public GreyFoxRoleCollection GetCollection(string whereClause, string sortClause)
		{
			return GetCollection(0, whereClause, sortClause);
		}

		public GreyFoxRoleCollection GetCollection(int topCount, string whereClause, string sortClause)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			GreyFoxRoleCollection greyFoxRoleCollection;


			query = new StringBuilder("SELECT ");

			if(topCount > 0)
			{
				query.Append("TOP ");
				query.Append(topCount);
				query.Append(" ");
			}

			foreach(string columnName in InnerJoinFields)
			{
				query.Append("GreyFoxRole.");
				query.Append(columnName);
				query.Append(",");
			}

			//
			// Remove trailing comma
			//
			query.Length--;
			query.Append(" FROM sysGlobal_Roles AS GreyFoxRole");
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

			greyFoxRoleCollection = new GreyFoxRoleCollection();

			while(r.Read())
			{
				GreyFoxRole greyFoxRole = ParseFromReader(r, 0, 1);

				greyFoxRoleCollection.Add(greyFoxRole);
			}

			// Microsoft DAAB still needs to close readers.
			r.Close();

			return greyFoxRoleCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static GreyFoxRole ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			GreyFoxRole greyFoxRole = new GreyFoxRole();
			FillFromReader(greyFoxRole, r, idOffset, dataOffset);
			return greyFoxRole;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(GreyFoxRole greyFoxRole, IDataReader r, int idOffset, int dataOffset)
		{
			greyFoxRole.iD = r.GetInt32(idOffset);
			greyFoxRole.isSynced = true;
			greyFoxRole.isPlaceHolder = false;

			greyFoxRole.name = r.GetString(0+dataOffset);
			if(!r.IsDBNull(1+dataOffset)) 
				greyFoxRole.description = r.GetString(1+dataOffset);
			else
				greyFoxRole.description = null;
			greyFoxRole.isDisabled = r.GetBoolean(2+dataOffset);
		}

		#endregion

		#region Default DbModel Delete Method

		internal static void _delete(int id)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("DELETE FROM sysGlobal_Roles WHERE GreyFoxRoleID=");
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

		public void CreateTable()
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Jet SQL
				query = new StringBuilder("CREATE TABLE sysGlobal_Roles ");
				query.Append(" (GreyFoxRoleID COUNTER(1,1) CONSTRAINT PK_sysGlobal_Roles PRIMARY KEY, " +
					"Name TEXT(25)," +
					"Description MEMO," +
					"IsDisabled BIT);");
			}
			else
			{
				// Microsoft SQL Server
				query = new StringBuilder("CREATE TABLE sysGlobal_Roles ");
				query.Append(" (GreyFoxRoleID INT IDENTITY(1,1) CONSTRAINT PK_sysGlobal_Roles PRIMARY KEY, " +
					"Name NVARCHAR(25)," +
					"Description NTEXT," +
					"IsDisabled BIT);");
			}

			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

		#region Cache Methods

		private static void cacheStore(GreyFoxRole greyFoxRole)
		{
			CacheManager cache = CacheFactory.GetCacheManager();
			cache.Add("sysGlobal_Roles_" + greyFoxRole.iD.ToString(), greyFoxRole);
		}

		private static GreyFoxRole cacheFind(int id)
		{
			object cachedObject;
			CacheManager cache = CacheFactory.GetCacheManager();
			cachedObject = cache.GetData("sysGlobal_Roles_" + id.ToString());
			if(cachedObject == null)
				return null;
			return (GreyFoxRole)cachedObject;
		}

		private static void cacheRemove(int id)
		{
			CacheManager cache = CacheFactory.GetCacheManager();
			cache.Remove("sysGlobal_Roles_" + id.ToString());
		}

		#endregion

		//--- Begin Custom Code ---

        #region Encoding

        public GreyFoxRoleCollection DecodeString(string roles, string separator)
        {
            GreyFoxRoleCollection encodedRoles = new GreyFoxRoleCollection();

            roles = roles.Trim();
            if (roles.Trim() == string.Empty)
                return encodedRoles;

            string[] names = roles.Split(new string[] { separator },
                StringSplitOptions.RemoveEmptyEntries);

            if (names.Length < 20)
                return fastDecode(names);

            GreyFoxRoleCollection allRoles =
                GetCollection(string.Empty, string.Empty);

            for (int x = 0; x < allRoles.Count; x++)
                for (int y = 0; y <= names.GetUpperBound(0); y++)
                    if (allRoles[x].Name == names[y])
                        encodedRoles.Add(allRoles[x]);

            return encodedRoles;
        }

        private GreyFoxRoleCollection fastDecode(string[] rolesNames)
        {
            StringBuilder query = new StringBuilder();

            for (int x = 0; x <= rolesNames.GetUpperBound(0); x++)
            {
                query.Append("GreyFoxRole.Name='");
                query.Append(rolesNames[x].Trim().Replace("'", "''"));
                query.Append("'");
                if (x != rolesNames.GetUpperBound(0))
                    query.Append(" OR ");
            }

            if (query.Length == 0)
            {
                throw (new Exception("No roles to encode."));
            }

            return GetCollection(query.ToString(), string.Empty);
        }

        #endregion

		//--- End Custom Code ---
	}
}

