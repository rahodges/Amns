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
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace Amns.GreyFox.Yari
{
	/// <summary>
	/// Datamanager for YariMediaKeyword objects.
	/// </summary>
	public class YariMediaKeywordManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "kitYari_MediaKeywords";


		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}

		#endregion

		#region Inner Join Field Array

		public static readonly string[] InnerJoinFields = new string[] {
			"YariMediaKeywordID",
			"Keyword"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "YariMediaKeywordID", "LONG", "-1" },
			{ "Keyword", "TEXT(25)", "" }
		};

		#endregion

		#region Default DbModel Constructors

		static YariMediaKeywordManager()
		{
		}

		public YariMediaKeywordManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!YariMediaKeywordManager.isInitialized)
			{
				YariMediaKeywordManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a YariMediaKeyword into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_YariMediaKeyword">The YariMediaKeyword to insert into the database.</param>
		internal static int _insert(YariMediaKeyword yariMediaKeyword)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO kitYari_MediaKeywords " +
				"(" +
				"Keyword) VALUES (" +
				"@Keyword);";

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Access
				// Connection must remain open for IDENTITY to return correct value,
				// therefore use the dbCommand object's Connection directly to control
				// connection state.
				dbCommand = database.GetSqlStringCommand(query);
				fillParameters(database, dbCommand, yariMediaKeyword);
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
				fillParameters(database, dbCommand, yariMediaKeyword);
				database.AddOutParameter(dbCommand, "@LastID", DbType.Int32, 10);
				database.ExecuteNonQuery(dbCommand);
				id = (int)dbCommand.Parameters["@LastID"].Value;
			}
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(YariMediaKeyword yariMediaKeyword)
		{
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE kitYari_MediaKeywords SET Keyword=@Keyword WHERE YariMediaKeywordID=@YariMediaKeywordID;");

			fillParameters(database, dbCommand, yariMediaKeyword);
			database.AddInParameter(dbCommand, "YariMediaKeywordID", DbType.Int32, yariMediaKeyword.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			return yariMediaKeyword.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, YariMediaKeyword yariMediaKeyword)
		{
			#region Default

			addParameter(database, dbCommand, "Keyword", DbType.String, yariMediaKeyword.keyword);

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

		internal static bool _fill(YariMediaKeyword yariMediaKeyword)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM kitYari_MediaKeywords WHERE YariMediaKeywordID=");
			query.Append(yariMediaKeyword.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find YariMediaKeywordID '{0}'.", 
					yariMediaKeyword.iD)));
			}

			FillFromReader(yariMediaKeyword, r, 0, 1);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public YariMediaKeywordCollection GetCollection(string whereClause, string sortClause)
		{
			return GetCollection(0, whereClause, sortClause);
		}

		public YariMediaKeywordCollection GetCollection(int topCount, string whereClause, string sortClause)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			YariMediaKeywordCollection yariMediaKeywordCollection;


			query = new StringBuilder("SELECT ");

			if(topCount > 0)
			{
				query.Append("TOP ");
				query.Append(topCount);
				query.Append(" ");
			}

			foreach(string columnName in InnerJoinFields)
			{
				query.Append("kitYari_MediaKeywords.");
				query.Append(columnName);
				query.Append(",");
			}

			//
			// Remove trailing comma
			//
			query.Length--;
			query.Append(" FROM kitYari_MediaKeywords ");
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

			yariMediaKeywordCollection = new YariMediaKeywordCollection();

			while(r.Read())
			{
				YariMediaKeyword yariMediaKeyword = ParseFromReader(r, 0, 1);

				yariMediaKeywordCollection.Add(yariMediaKeyword);
			}

			return yariMediaKeywordCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static YariMediaKeyword ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			YariMediaKeyword yariMediaKeyword = new YariMediaKeyword();
			FillFromReader(yariMediaKeyword, r, idOffset, dataOffset);
			return yariMediaKeyword;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(YariMediaKeyword yariMediaKeyword, IDataReader r, int idOffset, int dataOffset)
		{
			yariMediaKeyword.iD = r.GetInt32(idOffset);
			yariMediaKeyword.isSynced = true;
			yariMediaKeyword.isPlaceHolder = false;

			yariMediaKeyword.keyword = r.GetString(0+dataOffset);
		}

		#endregion

		#region Default DbModel Delete Method

		internal static void _delete(int id)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("DELETE FROM kitYari_MediaKeywords WHERE YariMediaKeywordID=");
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

			database = DatabaseFactory.CreateDatabase();

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Jet SQL
				query = new StringBuilder("CREATE TABLE kitYari_MediaKeywords ");
				query.Append(" (YariMediaKeywordID COUNTER(1,1) CONSTRAINT PK_kitYari_MediaKeywords PRIMARY KEY, " +
					"Keyword TEXT(25));");
			}
			else
			{
				// Microsoft SQL Server
				query = new StringBuilder("CREATE TABLE kitYari_MediaKeywords ");
				query.Append(" (YariMediaKeywordID INT IDENTITY(1,1) CONSTRAINT PK_kitYari_MediaKeywords PRIMARY KEY, " +
					"Keyword NVARCHAR(25));");
			}

			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

		//--- Begin Custom Code ---
																																																																																																																																																		
		public YariMediaKeyword FindByKeyword(string keyword, bool placeHolderOnly)
		{
			StringBuilder query = new StringBuilder();
			query.Append("SELECT ");
			if(placeHolderOnly)
				query.Append(InnerJoinFields[0]);
			else
				query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM kitYari_MediaKeywords");
			query.Append(" WHERE Keyword=@Keyword;");

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(query.ToString());
            addParameter(database, dbCommand, "Keyword", DbType.String, keyword);
            IDataReader r = database.ExecuteReader(dbCommand);
			YariMediaKeyword k;
			if(r.Read())
			{
				if(placeHolderOnly)
					k = YariMediaKeyword.NewPlaceHolder(r.GetInt32(0));
				else
					k = YariMediaKeywordManager.ParseFromReader(r, 0, 1);
			}
			else
			{
				k = new YariMediaKeyword();
				k.Keyword = keyword;
			}
			return k;
		}

		//--- End Custom Code ---
	}
}

