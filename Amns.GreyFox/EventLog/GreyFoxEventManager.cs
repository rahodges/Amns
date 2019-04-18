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

namespace Amns.GreyFox.EventLog
{
	#region Child Flags Enumeration

	public enum GreyFoxEventFlags : int { User,
				UserContact,
				UserRoles};

	#endregion

	/// <summary>
	/// Datamanager for GreyFoxEvent objects.
	/// </summary>
	public class GreyFoxEventManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName;


		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}

		#endregion

		#region Inner Join Field Array

		public static readonly string[] InnerJoinFields = new string[] {
			"GreyFoxEventID",
			"Type",
			"EventDate",
			"Source",
			"Category",
			"Description",
			"EventID",
			"UserID"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "GreyFoxEventID", "LONG", "-1" },
			{ "Type", "BYTE", "" },
			{ "EventDate", "DATETIME", "" },
			{ "Source", "TEXT(75)", "" },
			{ "Category", "TEXT(75)", "" },
			{ "Description", "MEMO", "" },
			{ "EventID", "LONG", "" },
			{ "UserID", "LONG", "null" }
		};

		#endregion

		#region Default DbModel Constructors

		static GreyFoxEventManager()
		{
		}

		public GreyFoxEventManager(string tableName)
		{
			this.tableName = tableName;
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!GreyFoxEventManager.isInitialized)
			{
				GreyFoxEventManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a GreyFoxEvent into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_GreyFoxEvent">The GreyFoxEvent to insert into the database.</param>
		internal static int _insert(GreyFoxEvent greyFoxEvent)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO " + greyFoxEvent.tableName +
				"(" +
				"Type," +
				"EventDate," +
				"Source," +
				"Category," +
				"Description," +
				"EventID," +
				"UserID) VALUES (" +
				"@Type," +
				"@EventDate," +
				"@Source," +
				"@Category," +
				"@Description," +
				"@EventID," +
				"@UserID);";

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Access
				// Connection must remain open for IDENTITY to return correct value,
				// therefore use the dbCommand object's Connection directly to control
				// connection state.
				dbCommand = database.GetSqlStringCommand(query);
				fillParameters(database, dbCommand, greyFoxEvent);
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
				fillParameters(database, dbCommand, greyFoxEvent);
				database.AddOutParameter(dbCommand, "@LastID", DbType.Int32, 10);
				database.ExecuteNonQuery(dbCommand);
				id = (int)dbCommand.Parameters["@LastID"].Value;
			}
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(GreyFoxEvent greyFoxEvent)
		{
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE " + greyFoxEvent.tableName + " SET Type=@Type," +
				"EventDate=@EventDate," +
				"Source=@Source," +
				"Category=@Category," +
				"Description=@Description," +
				"EventID=@EventID," +
				"UserID=@UserID WHERE GreyFoxEventID=@GreyFoxEventID;");

			fillParameters(database, dbCommand, greyFoxEvent);
			database.AddInParameter(dbCommand, "GreyFoxEventID", DbType.Int32, greyFoxEvent.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			return greyFoxEvent.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, GreyFoxEvent greyFoxEvent)
		{
			#region Default

			addParameter(database, dbCommand, "@Type", DbType.Byte, greyFoxEvent.type);
			addParameter(database, dbCommand, "@EventDate", DbType.Date, greyFoxEvent.eventDate);
			addParameter(database, dbCommand, "@Source", DbType.String, greyFoxEvent.source);
			addParameter(database, dbCommand, "@Category", DbType.String, greyFoxEvent.category);
			addParameter(database, dbCommand, "@Description", DbType.String, greyFoxEvent.description);
			addParameter(database, dbCommand, "@EventID", DbType.Int32, greyFoxEvent.eventID);
			if(greyFoxEvent.user == null)
			{
				addParameter(database, dbCommand, "@UserID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "@UserID", DbType.Int32, greyFoxEvent.user.ID);
			}

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

		internal static bool _fill(GreyFoxEvent greyFoxEvent)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM ");
			query.Append(greyFoxEvent.tableName);
			query.Append(" WHERE GreyFoxEventID=");
			query.Append(greyFoxEvent.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find GreyFoxEventID '{0}'.", 
					greyFoxEvent.iD)));
			}

			FillFromReader(greyFoxEvent, greyFoxEvent.tableName, r, 0, 1);

			// Microsoft DAAB still needs to have the reader closed.
			r.Close();

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public GreyFoxEventCollection GetCollection(string whereClause, string sortClause, params GreyFoxEventFlags[] optionFlags)
		{
			return GetCollection(0, whereClause, sortClause, optionFlags);
		}

		public GreyFoxEventCollection GetCollection(int topCount, string whereClause, string sortClause, params GreyFoxEventFlags[] optionFlags)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			GreyFoxEventCollection greyFoxEventCollection;

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
				query.Append("GreyFoxEvent.");
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
						case GreyFoxEventFlags.User:
							for(int i = 0; i <= GreyFoxUserManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("User.");
								query.Append(GreyFoxUserManager.InnerJoinFields[i]);
								query.Append(",");
							}
							userOffset = innerJoinOffset;
							innerJoinOffset = userOffset + GreyFoxUserManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case GreyFoxEventFlags.UserContact:
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

				query.Append(tableName);
				query.Append(" AS GreyFoxEvent");
			}
			else
			{
				query.Append(" FROM ");
				query.Append(tableName);
				query.Append(" AS GreyFoxEvent");
			}
			//
			// Finish INNER JOIN expressions
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case GreyFoxEventFlags.User:
							query.Append(" LEFT JOIN sysGlobal_Users ON ");
							query.Append(tableName);
							query.Append(" AS GreyFoxEvent");
							query.Append(".UserID = GreyFoxEvent.ID)");
							break;
						case GreyFoxEventFlags.UserContact:
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

			greyFoxEventCollection = new GreyFoxEventCollection();

			while(r.Read())
			{
				GreyFoxEvent greyFoxEvent = ParseFromReader(tableName, r, 0, 1);

				// Fill User
				if(userOffset != -1 && !r.IsDBNull(userOffset))
				{
					GreyFoxUserManager.FillFromReader(greyFoxEvent.user, r, userOffset, userOffset+1);

					// Fill 
					if(userContactOffset != -1 && !r.IsDBNull(userContactOffset))
						GreyFoxContactManager.FillFromReader(greyFoxEvent.user.Contact, "sysGlobal_Contacts", r, userContactOffset, userContactOffset+1);

				}

				greyFoxEventCollection.Add(greyFoxEvent);
			}

			// Microsoft DAAB still needs to close readers.
			r.Close();

			return greyFoxEventCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static GreyFoxEvent ParseFromReader(string tableName,IDataReader r, int idOffset, int dataOffset)
		{
			GreyFoxEvent greyFoxEvent = new GreyFoxEvent(tableName);
			FillFromReader(greyFoxEvent, tableName, r, idOffset, dataOffset);
			return greyFoxEvent;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(GreyFoxEvent greyFoxEvent, string tableName,IDataReader r, int idOffset, int dataOffset)
		{
			greyFoxEvent.tableName = tableName;
			greyFoxEvent.iD = r.GetInt32(idOffset);
			greyFoxEvent.isSynced = true;
			greyFoxEvent.isPlaceHolder = false;

			greyFoxEvent.type = r.GetByte(0+dataOffset);
			greyFoxEvent.eventDate = r.GetDateTime(1+dataOffset);
			greyFoxEvent.source = r.GetString(2+dataOffset);
			greyFoxEvent.category = r.GetString(3+dataOffset);
			greyFoxEvent.description = r.GetString(4+dataOffset);
			greyFoxEvent.eventID = r.GetInt32(5+dataOffset);
			if(!r.IsDBNull(6+dataOffset) && r.GetInt32(6+dataOffset) > 0)
			{
				greyFoxEvent.user = GreyFoxUser.NewPlaceHolder(r.GetInt32(6+dataOffset));
			}
		}

		#endregion

		#region Default DbModel Fill Methods

		#endregion

		#region Default DbModel Delete Method

		internal static void _delete(string tableName, int id)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("DELETE FROM ");
			query.Append(tableName);
			query.Append(" WHERE GreyFoxEventID=");
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

			dbConnection.Close();
			return msg.ToString();
		}

		#endregion

		#region Default DbModel Create Table Methods

		public void CreateReferences(string tableName)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder();
			database = DatabaseFactory.CreateDatabase();
			query.Append("ALTER TABLE ");
			query.Append(tableName);
			query.Append(" ADD ");
			query.Append(" CONSTRAINT FK_");
			query.Append(tableName);
			query.Append("_User FOREIGN KEY (UserID) REFERENCES sysGlobal_Users (GreyFoxUserID);");
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
				query = new StringBuilder("CREATE TABLE ");
				query.Append(tableName);
				query.Append(" (GreyFoxEventID COUNTER(1,1) CONSTRAINT PK_" + tableName + " PRIMARY KEY, " +
					"Type BYTE," +
					"EventDate DATETIME," +
					"Source TEXT(75)," +
					"Category TEXT(75)," +
					"Description MEMO," +
					"EventID LONG," +
					"UserID LONG);");
			}
			else
			{
				// Microsoft SQL Server
				query = new StringBuilder("CREATE TABLE ");
				query.Append(tableName);
				query.Append(" (GreyFoxEventID INT IDENTITY(1,1) CONSTRAINT PK_" + tableName + " PRIMARY KEY, " +
					"Type TINYINT," +
					"EventDate DATETIME," +
					"Source NVARCHAR(75)," +
					"Category NVARCHAR(75)," +
					"Description NTEXT," +
					"EventID INT," +
					"UserID INT);");
			}

			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

	}
}

