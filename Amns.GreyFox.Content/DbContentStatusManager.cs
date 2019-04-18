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

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Datamanager for DbContentStatus objects.
	/// </summary>
	[ExposedManager("DbContentStatus", "", true, 1, 1, 6234)]
	public class DbContentStatusManager : IGreyFoxManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "kitCms_Statuses";


		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}

		#endregion

		#region Inner Join Field Array

		public static readonly string[] InnerJoinFields = new string[] {
			"DbContentStatusID",
			"Name",
			"IsDraft",
			"IsPublished",
			"FeeEnabled",
			"EditEnabled",
			"ArchiveEnabled",
			"ReviewEnabled",
			"Icon"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "DbContentStatusID", "LONG", "-1" },
			{ "Name", "TEXT(75)", "string.Empty" },
			{ "IsDraft", "BIT", "" },
			{ "IsPublished", "BIT", "" },
			{ "FeeEnabled", "BIT", "" },
			{ "EditEnabled", "BIT", "" },
			{ "ArchiveEnabled", "BIT", "" },
			{ "ReviewEnabled", "BIT", "" },
			{ "Icon", "TEXT(255)", "string.Empty" }
		};

		#endregion

		#region Default DbModel Constructors

		static DbContentStatusManager()
		{
		}

		public DbContentStatusManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!DbContentStatusManager.isInitialized)
			{
				DbContentStatusManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a DbContentStatus into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_DbContentStatus">The DbContentStatus to insert into the database.</param>
		internal static int _insert(DbContentStatus dbContentStatus)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO kitCms_Statuses" +
				"(," +
				"Name," +
				"IsDraft," +
				"IsPublished," +
				"FeeEnabled," +
				"EditEnabled," +
				"ArchiveEnabled," +
				"ReviewEnabled," +
				"Icon) VALUES (," +
				"@Name," +
				"@IsDraft," +
				"@IsPublished," +
				"@FeeEnabled," +
				"@EditEnabled," +
				"@ArchiveEnabled," +
				"@ReviewEnabled," +
				"@Icon);";

			dbCommand = database.GetSqlStringCommand(query);
			fillParameters(database, dbCommand, dbContentStatus);
			database.ExecuteNonQuery(dbCommand);
			dbCommand = database.GetSqlStringCommand("SELECT @@IDENTITY AS IDVal");
			id = (int)database.ExecuteScalar(dbCommand);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(DbContentStatus dbContentStatus)
		{
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE kitCms_Statuses SET Name=@Name," +
				"IsDraft=@IsDraft," +
				"IsPublished=@IsPublished," +
				"FeeEnabled=@FeeEnabled," +
				"EditEnabled=@EditEnabled," +
				"ArchiveEnabled=@ArchiveEnabled," +
				"ReviewEnabled=@ReviewEnabled," +
				"Icon=@Icon WHERE DbContentStatusID=@DbContentStatusID;");

			fillParameters(database, dbCommand, dbContentStatus);
			database.AddInParameter(dbCommand, "DbContentStatusID", DbType.Int32, dbContentStatus.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			return dbContentStatus.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, DbContentStatus dbContentStatus)
		{
			#region System

			addParameter(database, dbCommand, "Name", DbType.String, dbContentStatus.name);
			addParameter(database, dbCommand, "IsDraft", DbType.Boolean, dbContentStatus.isDraft);
			addParameter(database, dbCommand, "IsPublished", DbType.Boolean, dbContentStatus.isPublished);
			addParameter(database, dbCommand, "FeeEnabled", DbType.Boolean, dbContentStatus.feeEnabled);
			addParameter(database, dbCommand, "EditEnabled", DbType.Boolean, dbContentStatus.editEnabled);
			addParameter(database, dbCommand, "ArchiveEnabled", DbType.Boolean, dbContentStatus.archiveEnabled);
			addParameter(database, dbCommand, "ReviewEnabled", DbType.Boolean, dbContentStatus.reviewEnabled);
			addParameter(database, dbCommand, "Icon", DbType.String, dbContentStatus.icon);

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

		internal static bool _fill(DbContentStatus dbContentStatus)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM kitCms_Statuses WHERE DbContentStatusID=");
			query.Append(dbContentStatus.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find DbContentStatusID '{0}'.", 
					dbContentStatus.iD)));
			}

			FillFromReader(dbContentStatus, r, 0, 1);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public DbContentStatusCollection GetCollection(string whereClause, string sortClause)
		{
			return GetCollection(0, whereClause, sortClause);
		}

		public DbContentStatusCollection GetCollection(int topCount, string whereClause, string sortClause)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			DbContentStatusCollection dbContentStatusCollection;


			query = new StringBuilder("SELECT ");

			if(topCount > 0)
			{
				query.Append("TOP ");
				query.Append(topCount);
				query.Append(" ");
			}

			foreach(string columnName in InnerJoinFields)
			{
				query.Append("DbContentStatus.");
				query.Append(columnName);
				query.Append(",");
			}

			//
			// Remove trailing comma
			//
			query.Length--;
			query.Append(" FROM kitCms_Statuses AS DbContentStatus");
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

			dbContentStatusCollection = new DbContentStatusCollection();

			while(r.Read())
			{
				DbContentStatus dbContentStatus = ParseFromReader(r, 0, 1);

				dbContentStatusCollection.Add(dbContentStatus);
			}

			return dbContentStatusCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static DbContentStatus ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			DbContentStatus dbContentStatus = new DbContentStatus();
			FillFromReader(dbContentStatus, r, idOffset, dataOffset);
			return dbContentStatus;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(DbContentStatus dbContentStatus, IDataReader r, int idOffset, int dataOffset)
		{
			dbContentStatus.iD = r.GetInt32(idOffset);
			dbContentStatus.isSynced = true;
			dbContentStatus.isPlaceHolder = false;

			dbContentStatus.name = r.GetString(0+dataOffset);
			dbContentStatus.isDraft = r.GetBoolean(1+dataOffset);
			dbContentStatus.isPublished = r.GetBoolean(2+dataOffset);
			dbContentStatus.feeEnabled = r.GetBoolean(3+dataOffset);
			dbContentStatus.editEnabled = r.GetBoolean(4+dataOffset);
			dbContentStatus.archiveEnabled = r.GetBoolean(5+dataOffset);
			dbContentStatus.reviewEnabled = r.GetBoolean(6+dataOffset);
			dbContentStatus.icon = r.GetString(7+dataOffset);
		}

		#endregion

		#region Default DbModel Delete Method

		internal static void _delete(int id)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("DELETE * FROM kitCms_Statuses WHERE DbContentStatusID=");
			query.Append(id);
			query.Append(';');

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

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

		public void CreateTable()
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("CREATE TABLE kitCms_Statuses ");
			query.Append(" (DbContentStatusID COUNTER(1,1) CONSTRAINT DbContentStatusID PRIMARY KEY, " +
				"Name TEXT(75)," +
				"IsDraft BIT," +
				"IsPublished BIT," +
				"FeeEnabled BIT," +
				"EditEnabled BIT," +
				"ArchiveEnabled BIT," +
				"ReviewEnabled BIT," +
				"Icon TEXT(255));");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

	}
}

