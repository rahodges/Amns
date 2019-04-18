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
	/// Datamanager for DbContentComment objects.
	/// </summary>
	[ExposedManager("DbContentComment", "", true, 1, 1, 6234)]
	public class DbContentCommentManager : IGreyFoxManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "kitCms_Comments";


		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}

		#endregion

		#region Inner Join Field Array

		public static readonly string[] InnerJoinFields = new string[] {
			"DbContentCommentID",
			"CreateDate",
			"ModifyDate",
			"Name",
			"Email",
			"Url",
			"IP",
			"Body"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "DbContentCommentID", "LONG", "-1" },
			{ "CreateDate", "DATETIME", "DateTime.Now" },
			{ "ModifyDate", "DATETIME", "DateTime.Now" },
			{ "Name", "TEXT(50)", "string.Empty" },
			{ "Email", "TEXT(50)", "string.Empty" },
			{ "Url", "TEXT(255)", "string.Empty" },
			{ "IP", "TEXT(15)", "string.Empty" },
			{ "Body", "MEMO", "string.Empty" }
		};

		#endregion

		#region Default DbModel Constructors

		static DbContentCommentManager()
		{
		}

		public DbContentCommentManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!DbContentCommentManager.isInitialized)
			{
				DbContentCommentManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a DbContentComment into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_DbContentComment">The DbContentComment to insert into the database.</param>
		internal static int _insert(DbContentComment dbContentComment)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			// Set Create Date to Now
			dbContentComment.CreateDate = DateTime.Now.ToUniversalTime();

			// Set Modify Date to Now
			dbContentComment.ModifyDate = DateTime.Now.ToUniversalTime();

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO kitCms_Comments" +
				"(," +
				"CreateDate," +
				"ModifyDate," +
				"Name," +
				"Email," +
				"Url," +
				"IP," +
				"Body) VALUES (," +
				"@CreateDate," +
				"@ModifyDate," +
				"@Name," +
				"@Email," +
				"@Url," +
				"@IP," +
				"@Body);";

			dbCommand = database.GetSqlStringCommand(query);
			fillParameters(database, dbCommand, dbContentComment);
			database.ExecuteNonQuery(dbCommand);
			dbCommand = database.GetSqlStringCommand("SELECT @@IDENTITY AS IDVal");
			id = (int)database.ExecuteScalar(dbCommand);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(DbContentComment dbContentComment)
		{
			Database database;
			DbCommand dbCommand;

			// Set Modify Date to Now
			dbContentComment.ModifyDate = DateTime.Now.ToUniversalTime();

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE kitCms_Comments SET CreateDate=@CreateDate," +
				"ModifyDate=@ModifyDate," +
				"Name=@Name," +
				"Email=@Email," +
				"Url=@Url," +
				"IP=@IP," +
				"Body=@Body WHERE DbContentCommentID=@DbContentCommentID;");

			fillParameters(database, dbCommand, dbContentComment);
			database.AddInParameter(dbCommand, "DbContentCommentID", DbType.Int32, dbContentComment.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			return dbContentComment.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, DbContentComment dbContentComment)
		{
			#region _system

			addParameter(database, dbCommand, "CreateDate", DbType.Date, dbContentComment.createDate);
			addParameter(database, dbCommand, "ModifyDate", DbType.Date, dbContentComment.modifyDate);

			#endregion

			#region New Folder

			addParameter(database, dbCommand, "Name", DbType.String, dbContentComment.name);
			addParameter(database, dbCommand, "Email", DbType.String, dbContentComment.email);
			addParameter(database, dbCommand, "Url", DbType.String, dbContentComment.url);
			addParameter(database, dbCommand, "IP", DbType.String, dbContentComment.iP);
			addParameter(database, dbCommand, "Body", DbType.String, dbContentComment.body);

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

		internal static bool _fill(DbContentComment dbContentComment)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM kitCms_Comments WHERE DbContentCommentID=");
			query.Append(dbContentComment.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find DbContentCommentID '{0}'.", 
					dbContentComment.iD)));
			}

			FillFromReader(dbContentComment, r, 0, 1);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public DbContentCommentCollection GetCollection(string whereClause, string sortClause)
		{
			return GetCollection(0, whereClause, sortClause);
		}

		public DbContentCommentCollection GetCollection(int topCount, string whereClause, string sortClause)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			DbContentCommentCollection dbContentCommentCollection;


			query = new StringBuilder("SELECT ");

			if(topCount > 0)
			{
				query.Append("TOP ");
				query.Append(topCount);
				query.Append(" ");
			}

			foreach(string columnName in InnerJoinFields)
			{
				query.Append("DbContentComment.");
				query.Append(columnName);
				query.Append(",");
			}

			//
			// Remove trailing comma
			//
			query.Length--;
			query.Append(" FROM kitCms_Comments AS DbContentComment");
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

			dbContentCommentCollection = new DbContentCommentCollection();

			while(r.Read())
			{
				DbContentComment dbContentComment = ParseFromReader(r, 0, 1);

				dbContentCommentCollection.Add(dbContentComment);
			}

			return dbContentCommentCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static DbContentComment ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			DbContentComment dbContentComment = new DbContentComment();
			FillFromReader(dbContentComment, r, idOffset, dataOffset);
			return dbContentComment;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(DbContentComment dbContentComment, IDataReader r, int idOffset, int dataOffset)
		{
			dbContentComment.iD = r.GetInt32(idOffset);
			dbContentComment.isSynced = true;
			dbContentComment.isPlaceHolder = false;

			dbContentComment.createDate = r.GetDateTime(0+dataOffset);
			dbContentComment.modifyDate = r.GetDateTime(1+dataOffset);
			dbContentComment.name = r.GetString(2+dataOffset);
			dbContentComment.email = r.GetString(3+dataOffset);
			dbContentComment.url = r.GetString(4+dataOffset);
			dbContentComment.iP = r.GetString(5+dataOffset);
			dbContentComment.body = r.GetString(6+dataOffset);
		}

		#endregion

		#region Default DbModel Delete Method

		internal static void _delete(int id)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("DELETE * FROM kitCms_Comments WHERE DbContentCommentID=");
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

			query = new StringBuilder("CREATE TABLE kitCms_Comments ");
			query.Append(" (DbContentCommentID COUNTER(1,1) CONSTRAINT DbContentCommentID PRIMARY KEY, " +
				"CreateDate DATETIME," +
				"ModifyDate DATETIME," +
				"Name TEXT(50)," +
				"Email TEXT(50)," +
				"Url TEXT(255)," +
				"IP TEXT(15)," +
				"Body MEMO);");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

	}
}

