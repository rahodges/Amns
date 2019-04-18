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
using System.Data;
using System.Data.Common;
using System.Text;
using Amns.GreyFox.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Content
{
	#region Child Flags Enumeration

	public enum DbContentRatingFlags : int { RequiredRole};

	#endregion

	/// <summary>
	/// Datamanager for DbContentRating objects.
	/// </summary>
	[ExposedManager("DbContentRating", "Rating", true, 1, 1, 6234)]
	public class DbContentRatingManager : IGreyFoxManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "kitCms_Ratings";


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
			"DbContentRatingID",
			"CreateDate",
			"ModifyDate",
			"Name",
			"Description",
			"IconUrl",
			"RequiredRoleID"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "DbContentRatingID", "LONG", "-1" },
			{ "CreateDate", "DATETIME", "DateTime.Now" },
			{ "ModifyDate", "DATETIME", "DateTime.Now" },
			{ "Name", "TEXT(25)", "string.Empty" },
			{ "Description", "TEXT(255)", "string.Empty" },
			{ "IconUrl", "TEXT(255)", "string.Empty" },
			{ "RequiredRoleID", "LONG", "null" }
		};

		#endregion

		#region Default DbModel Constructors

		static DbContentRatingManager()
		{
		}

		public DbContentRatingManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!DbContentRatingManager.isInitialized)
			{
				DbContentRatingManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a DbContentRating into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_DbContentRating">The DbContentRating to insert into the database.</param>
		internal static int _insert(DbContentRating dbContentRating)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			// Set Create Date to Now
			dbContentRating.CreateDate = DateTime.Now.ToUniversalTime();

			// Set Modify Date to Now
			dbContentRating.ModifyDate = DateTime.Now.ToUniversalTime();

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO kitCms_Ratings" +
				"(," +
				"CreateDate," +
				"ModifyDate," +
				"Name," +
				"Description," +
				"IconUrl," +
				"RequiredRoleID) VALUES (," +
				"@CreateDate," +
				"@ModifyDate," +
				"@Name," +
				"@Description," +
				"@IconUrl," +
				"@RequiredRoleID);";

			dbCommand = database.GetSqlStringCommand(query);
			fillParameters(database, dbCommand, dbContentRating);
			database.ExecuteNonQuery(dbCommand);
			dbCommand = database.GetSqlStringCommand("SELECT @@IDENTITY AS IDVal");
			id = (int)database.ExecuteScalar(dbCommand);
			// Store dbContentRating in cache.
			if(cacheEnabled) cacheStore(dbContentRating);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(DbContentRating dbContentRating)
		{
			Database database;
			DbCommand dbCommand;

			// Set Modify Date to Now
			dbContentRating.ModifyDate = DateTime.Now.ToUniversalTime();

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE kitCms_Ratings SET CreateDate=@CreateDate," +
				"ModifyDate=@ModifyDate," +
				"Name=@Name," +
				"Description=@Description," +
				"IconUrl=@IconUrl," +
				"RequiredRoleID=@RequiredRoleID WHERE DbContentRatingID=@DbContentRatingID;");

			fillParameters(database, dbCommand, dbContentRating);
			database.AddInParameter(dbCommand, "DbContentRatingID", DbType.Int32, dbContentRating.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			// Store dbContentRating in cache.
			if (cacheEnabled) cacheStore(dbContentRating);

			return dbContentRating.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, DbContentRating dbContentRating)
		{
			#region _system

			addParameter(database, dbCommand, "CreateDate", DbType.Date, dbContentRating.createDate);
			addParameter(database, dbCommand, "ModifyDate", DbType.Date, dbContentRating.modifyDate);

			#endregion

			#region General

			addParameter(database, dbCommand, "Name", DbType.String, dbContentRating.name);
			addParameter(database, dbCommand, "Description", DbType.String, dbContentRating.description);
			addParameter(database, dbCommand, "IconUrl", DbType.String, dbContentRating.iconUrl);
			if(dbContentRating.requiredRole == null)
			{
				addParameter(database, dbCommand, "RequiredRoleID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "RequiredRoleID", DbType.Int32, dbContentRating.requiredRole.ID);
			}

			#endregion

		}

		#endregion

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

		#region Default DbModel Fill Method

		internal static bool _fill(DbContentRating dbContentRating)
		{
			// Clone item from cache.
			if(cacheEnabled)
			{
				object cachedObject = cacheFind(dbContentRating.iD);
				if(cachedObject != null)
				{
					((DbContentRating)cachedObject).CopyTo(dbContentRating, true);
					return dbContentRating.isSynced;
				}
			}

			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM kitCms_Ratings WHERE DbContentRatingID=");
			query.Append(dbContentRating.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find DbContentRatingID '{0}'.", 
					dbContentRating.iD)));
			}

			FillFromReader(dbContentRating, r, 0, 1);

			// Store dbContentRating in cache.
			if(cacheEnabled) cacheStore(dbContentRating);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public DbContentRatingCollection GetCollection(string whereClause, string sortClause, params DbContentRatingFlags[] optionFlags)
		{
			return GetCollection(0, whereClause, sortClause, optionFlags);
		}

		public DbContentRatingCollection GetCollection(int topCount, string whereClause, string sortClause, params DbContentRatingFlags[] optionFlags)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			DbContentRatingCollection dbContentRatingCollection;

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
				query.Append("DbContentRating.");
				query.Append(columnName);
				query.Append(",");
			}

			innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;
			int requiredRoleOffset = -1;

			//
			// Append Option Flag Fields
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case DbContentRatingFlags.RequiredRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("RequiredRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							requiredRoleOffset = innerJoinOffset;
							innerJoinOffset = requiredRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
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

				query.Append("kitCms_Ratings AS DbContentRating");
			}
			else
			{
				query.Append(" FROM kitCms_Ratings AS DbContentRating");
			}
			//
			// Finish INNER JOIN expressions
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case DbContentRatingFlags.RequiredRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS RequiredRole ON DbContentRating.RequiredRoleID = RequiredRole.GreyFoxRoleID)");
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

			dbContentRatingCollection = new DbContentRatingCollection();

			while(r.Read())
			{
				DbContentRating dbContentRating = ParseFromReader(r, 0, 1);

				// Fill RequiredRole
				if(requiredRoleOffset != -1 && !r.IsDBNull(requiredRoleOffset))
					GreyFoxRoleManager.FillFromReader(dbContentRating.requiredRole, r, requiredRoleOffset, requiredRoleOffset+1);

				dbContentRatingCollection.Add(dbContentRating);
			}

			return dbContentRatingCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static DbContentRating ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			DbContentRating dbContentRating = new DbContentRating();
			FillFromReader(dbContentRating, r, idOffset, dataOffset);
			return dbContentRating;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(DbContentRating dbContentRating, IDataReader r, int idOffset, int dataOffset)
		{
			dbContentRating.iD = r.GetInt32(idOffset);
			dbContentRating.isSynced = true;
			dbContentRating.isPlaceHolder = false;

			dbContentRating.createDate = r.GetDateTime(0+dataOffset);
			dbContentRating.modifyDate = r.GetDateTime(1+dataOffset);
			dbContentRating.name = r.GetString(2+dataOffset);
			dbContentRating.description = r.GetString(3+dataOffset);
			dbContentRating.iconUrl = r.GetString(4+dataOffset);
			if(!r.IsDBNull(5+dataOffset) && r.GetInt32(5+dataOffset) > 0)
			{
				dbContentRating.requiredRole = GreyFoxRole.NewPlaceHolder(r.GetInt32(5+dataOffset));
			}
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

			query = new StringBuilder("DELETE * FROM kitCms_Ratings WHERE DbContentRatingID=");
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

			return msg.ToString();
		}

		#endregion

		#region Default DbModel Create Table Methods

		public static void CreateTableReferences()
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("ALTER TABLE kitCms_Ratings ADD ");
			query.Append(" CONSTRAINT kitCms_Ratings_RequiredRole_FK FOREIGN KEY (RequiredRoleID) REFERENCES sysGlobal_Roles(GreyFoxRoleID);");
			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		public void CreateTable()
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("CREATE TABLE kitCms_Ratings ");
			query.Append(" (DbContentRatingID COUNTER(1,1) CONSTRAINT DbContentRatingID PRIMARY KEY, " +
				"CreateDate DATETIME," +
				"ModifyDate DATETIME," +
				"Name TEXT(25)," +
				"Description TEXT(255)," +
				"IconUrl TEXT(255)," +
				"RequiredRoleID LONG);");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

		#region CacheManager Methods

		private static void cacheStore(DbContentRating dbContentRating)
		{
			CacheManager cache = CacheFactory.GetCacheManager("DbContentRating");
			cache.Add(dbContentRating.iD.ToString(), dbContentRating);
		}

		private static DbContentRating cacheFind(int id)
		{
			object cachedObject;
			CacheManager cache = CacheFactory.GetCacheManager("DbContentRating");
			cachedObject = cache.GetData(id.ToString());
			if(cachedObject == null)
				return null;
			return (DbContentRating)cachedObject;
		}

		private static void cacheRemove(int id)
		{
			CacheManager cache = CacheFactory.GetCacheManager("DbContentRating");
			cache.Remove(id.ToString());
		}

		#endregion

	}
}

