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
using Amns.GreyFox.Security;
using Amns.GreyFox.People;

namespace Amns.GreyFox.Content
{
	#region Child Flags Enumeration

	public enum DbContentHitFlags : int { User,
				UserContact,
				UserRoles,
				RequestContent,
				RequestContentStatus,
				RequestContentParentCatalog,
				RequestContentRating,
				RequestContentReferences,
				RequestContentWorkingDraft,
				RequestContentAuthors,
				RequestContentEditors};

	#endregion

	/// <summary>
	/// Datamanager for DbContentHit objects.
	/// </summary>
	public class DbContentHitManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "kitCms_Hits";


		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}

		#endregion

		#region Inner Join Field Array

		public static readonly string[] InnerJoinFields = new string[] {
			"DbContentHitID",
			"UserID",
			"UserAgent",
			"UserHostAddress",
			"UserHostName",
			"RequestDate",
			"RequestReferrer",
			"RequestContentID",
			"IsUnique"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "DbContentHitID", "LONG", "-1" },
			{ "UserID", "LONG", "null" },
			{ "UserAgent", "TEXT(255)", "" },
			{ "UserHostAddress", "TEXT(15)", "" },
			{ "UserHostName", "TEXT(255)", "" },
			{ "RequestDate", "DATETIME", "" },
			{ "RequestReferrer", "TEXT(255)", "" },
			{ "RequestContentID", "LONG", "null" },
			{ "IsUnique", "BIT", "" }
		};

		#endregion

		#region Default DbModel Constructors

		static DbContentHitManager()
		{
		}

		public DbContentHitManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!DbContentHitManager.isInitialized)
			{
				DbContentHitManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a DbContentHit into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_DbContentHit">The DbContentHit to insert into the database.</param>
		internal static int _insert(DbContentHit dbContentHit)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO kitCms_Hits " +
				"(" +
				"UserID," +
				"UserAgent," +
				"UserHostAddress," +
				"UserHostName," +
				"RequestDate," +
				"RequestReferrer," +
				"RequestContentID," +
				"IsUnique) VALUES (" +
				"@UserID," +
				"@UserAgent," +
				"@UserHostAddress," +
				"@UserHostName," +
				"@RequestDate," +
				"@RequestReferrer," +
				"@RequestContentID," +
				"@IsUnique);";

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Access
				// Connection must remain open for IDENTITY to return correct value,
				// therefore use the dbCommand object's Connection directly to control
				// connection state.
				dbCommand = database.GetSqlStringCommand(query);
				fillParameters(database, dbCommand, dbContentHit);
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
				fillParameters(database, dbCommand, dbContentHit);
				database.AddOutParameter(dbCommand, "@LastID", DbType.Int32, 10);
				database.ExecuteNonQuery(dbCommand);
				id = (int)dbCommand.Parameters["@LastID"].Value;
			}
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(DbContentHit dbContentHit)
		{
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE kitCms_Hits SET UserID=@UserID," +
				"UserAgent=@UserAgent," +
				"UserHostAddress=@UserHostAddress," +
				"UserHostName=@UserHostName," +
				"RequestDate=@RequestDate," +
				"RequestReferrer=@RequestReferrer," +
				"RequestContentID=@RequestContentID," +
				"IsUnique=@IsUnique WHERE DbContentHitID=@DbContentHitID;");

			fillParameters(database, dbCommand, dbContentHit);
			database.AddInParameter(dbCommand, "DbContentHitID", DbType.Int32, dbContentHit.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			return dbContentHit.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, DbContentHit dbContentHit)
		{
			#region General

			if(dbContentHit.user == null)
			{
				addParameter(database, dbCommand, "UserID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "UserID", DbType.Int32, dbContentHit.user.ID);
			}
			addParameter(database, dbCommand, "UserAgent", DbType.String, dbContentHit.userAgent);
			addParameter(database, dbCommand, "UserHostAddress", DbType.String, dbContentHit.userHostAddress);
			addParameter(database, dbCommand, "UserHostName", DbType.String, dbContentHit.userHostName);
			addParameter(database, dbCommand, "RequestDate", DbType.Date, dbContentHit.requestDate);
			addParameter(database, dbCommand, "RequestReferrer", DbType.String, dbContentHit.requestReferrer);
			if(dbContentHit.requestContent == null)
			{
				addParameter(database, dbCommand, "RequestContentID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "RequestContentID", DbType.Int32, dbContentHit.requestContent.ID);
			}
			addParameter(database, dbCommand, "IsUnique", DbType.Boolean, dbContentHit.isUnique);

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

		internal static bool _fill(DbContentHit dbContentHit)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM kitCms_Hits WHERE DbContentHitID=");
			query.Append(dbContentHit.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find DbContentHitID '{0}'.", 
					dbContentHit.iD)));
			}

			FillFromReader(dbContentHit, r, 0, 1);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public DbContentHitCollection GetCollection(string whereClause, string sortClause, params DbContentHitFlags[] optionFlags)
		{
			return GetCollection(0, whereClause, sortClause, optionFlags);
		}

		public DbContentHitCollection GetCollection(int topCount, string whereClause, string sortClause, params DbContentHitFlags[] optionFlags)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			DbContentHitCollection dbContentHitCollection;

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
				query.Append("DbContentHit.");
				query.Append(columnName);
				query.Append(",");
			}

			innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;
			int userOffset = -1;
			int userContactOffset = -1;
			int requestContentOffset = -1;
			int requestContentStatusOffset = -1;
			int requestContentParentCatalogOffset = -1;
			int requestContentRatingOffset = -1;
			int requestContentWorkingDraftOffset = -1;

			//
			// Append Option Flag Fields
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case DbContentHitFlags.User:
							for(int i = 0; i <= GreyFoxUserManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("User.");
								query.Append(GreyFoxUserManager.InnerJoinFields[i]);
								query.Append(",");
							}
							userOffset = innerJoinOffset;
							innerJoinOffset = userOffset + GreyFoxUserManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentHitFlags.UserContact:
							for(int i = 0; i <= GreyFoxContactManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("User_Contact.");
								query.Append(GreyFoxContactManager.InnerJoinFields[i]);
								query.Append(",");
							}
							userContactOffset = innerJoinOffset;
							innerJoinOffset = userContactOffset + GreyFoxContactManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentHitFlags.RequestContent:
							for(int i = 0; i <= DbContentClipManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("RequestContent.");
								query.Append(DbContentClipManager.InnerJoinFields[i]);
								query.Append(",");
							}
							requestContentOffset = innerJoinOffset;
							innerJoinOffset = requestContentOffset + DbContentClipManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentHitFlags.RequestContentStatus:
							for(int i = 0; i <= DbContentStatusManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("RequestContent_Status.");
								query.Append(DbContentStatusManager.InnerJoinFields[i]);
								query.Append(",");
							}
							requestContentStatusOffset = innerJoinOffset;
							innerJoinOffset = requestContentStatusOffset + DbContentStatusManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentHitFlags.RequestContentParentCatalog:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("RequestContent_ParentCatalog.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							requestContentParentCatalogOffset = innerJoinOffset;
							innerJoinOffset = requestContentParentCatalogOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentHitFlags.RequestContentRating:
							for(int i = 0; i <= DbContentRatingManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("RequestContent_Rating.");
								query.Append(DbContentRatingManager.InnerJoinFields[i]);
								query.Append(",");
							}
							requestContentRatingOffset = innerJoinOffset;
							innerJoinOffset = requestContentRatingOffset + DbContentRatingManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentHitFlags.RequestContentWorkingDraft:
							for(int i = 0; i <= DbContentClipManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("RequestContent_WorkingDraft.");
								query.Append(DbContentClipManager.InnerJoinFields[i]);
								query.Append(",");
							}
							requestContentWorkingDraftOffset = innerJoinOffset;
							innerJoinOffset = requestContentWorkingDraftOffset + DbContentClipManager.InnerJoinFields.GetUpperBound(0) + 1;
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

				query.Append("kitCms_Hits AS DbContentHit");
			}
			else
			{
				query.Append(" FROM kitCms_Hits AS DbContentHit");
			}
			//
			// Finish INNER JOIN expressions
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case DbContentHitFlags.User:
							query.Append(" LEFT JOIN sysGlobal_Users AS User ON DbContentHit.UserID = User.GreyFoxUserID)");
							break;
						case DbContentHitFlags.UserContact:
							query.Append(" LEFT JOIN sysGlobal_Contacts AS User_Contact ON User.ContactID = User_Contact.GreyFoxContactID)");
							break;
						case DbContentHitFlags.RequestContent:
							query.Append(" LEFT JOIN kitCms_Clips AS RequestContent ON DbContentHit.RequestContentID = RequestContent.DbContentClipID)");
							break;
						case DbContentHitFlags.RequestContentStatus:
							query.Append(" LEFT JOIN kitCms_Statuses AS RequestContent_Status ON RequestContent.StatusID = RequestContent_Status.DbContentStatusID)");
							break;
						case DbContentHitFlags.RequestContentParentCatalog:
							query.Append(" LEFT JOIN kitCms_Catalogs AS RequestContent_ParentCatalog ON RequestContent.ParentCatalogID = RequestContent_ParentCatalog.DbContentCatalogID)");
							break;
						case DbContentHitFlags.RequestContentRating:
							query.Append(" LEFT JOIN kitCms_Ratings AS RequestContent_Rating ON RequestContent.RatingID = RequestContent_Rating.DbContentRatingID)");
							break;
						case DbContentHitFlags.RequestContentWorkingDraft:
							query.Append(" LEFT JOIN kitCms_Clips AS RequestContent_WorkingDraft ON RequestContent.WorkingDraftID = RequestContent_WorkingDraft.DbContentClipID)");
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

			dbContentHitCollection = new DbContentHitCollection();

			while(r.Read())
			{
				DbContentHit dbContentHit = ParseFromReader(r, 0, 1);

				// Fill User
				if(userOffset != -1 && !r.IsDBNull(userOffset))
				{
					GreyFoxUserManager.FillFromReader(dbContentHit.user, r, userOffset, userOffset+1);

					// Fill 
					if(userContactOffset != -1 && !r.IsDBNull(userContactOffset))
						GreyFoxContactManager.FillFromReader(dbContentHit.user.Contact, "sysGlobal_Contacts", r, userContactOffset, userContactOffset+1);

				}

				// Fill RequestContent
				if(requestContentOffset != -1 && !r.IsDBNull(requestContentOffset))
				{
					DbContentClipManager.FillFromReader(dbContentHit.requestContent, r, requestContentOffset, requestContentOffset+1);

					// Fill 
					if(requestContentStatusOffset != -1 && !r.IsDBNull(requestContentStatusOffset))
						DbContentStatusManager.FillFromReader(dbContentHit.requestContent.Status, r, requestContentStatusOffset, requestContentStatusOffset+1);

					// Fill Parent Catalog
					if(requestContentParentCatalogOffset != -1 && !r.IsDBNull(requestContentParentCatalogOffset))
						DbContentCatalogManager.FillFromReader(dbContentHit.requestContent.ParentCatalog, r, requestContentParentCatalogOffset, requestContentParentCatalogOffset+1);

					// Fill 
					if(requestContentRatingOffset != -1 && !r.IsDBNull(requestContentRatingOffset))
						DbContentRatingManager.FillFromReader(dbContentHit.requestContent.Rating, r, requestContentRatingOffset, requestContentRatingOffset+1);

					// Fill 
					if(requestContentWorkingDraftOffset != -1 && !r.IsDBNull(requestContentWorkingDraftOffset))
						DbContentClipManager.FillFromReader(dbContentHit.requestContent.WorkingDraft, r, requestContentWorkingDraftOffset, requestContentWorkingDraftOffset+1);

				}

				dbContentHitCollection.Add(dbContentHit);
			}

			return dbContentHitCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static DbContentHit ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			DbContentHit dbContentHit = new DbContentHit();
			FillFromReader(dbContentHit, r, idOffset, dataOffset);
			return dbContentHit;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(DbContentHit dbContentHit, IDataReader r, int idOffset, int dataOffset)
		{
			dbContentHit.iD = r.GetInt32(idOffset);
			dbContentHit.isSynced = true;
			dbContentHit.isPlaceHolder = false;

			if(!r.IsDBNull(0+dataOffset) && r.GetInt32(0+dataOffset) > 0)
			{
				dbContentHit.user = GreyFoxUser.NewPlaceHolder(r.GetInt32(0+dataOffset));
			}
			dbContentHit.userAgent = r.GetString(1+dataOffset);
			dbContentHit.userHostAddress = r.GetString(2+dataOffset);
			dbContentHit.userHostName = r.GetString(3+dataOffset);
			dbContentHit.requestDate = r.GetDateTime(4+dataOffset);
			dbContentHit.requestReferrer = r.GetString(5+dataOffset);
			if(!r.IsDBNull(6+dataOffset) && r.GetInt32(6+dataOffset) > 0)
			{
				dbContentHit.requestContent = DbContentClip.NewPlaceHolder(r.GetInt32(6+dataOffset));
			}
			dbContentHit.isUnique = r.GetBoolean(7+dataOffset);
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

			query = new StringBuilder("DELETE FROM kitCms_Hits WHERE DbContentHitID=");
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

		public void CreateTableReferences()
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder();
			database = DatabaseFactory.CreateDatabase();
			query.Append("ALTER TABLE kitCms_Hits ADD ");
			query.Append(" CONSTRAINT FK_kitCms_Hits_User FOREIGN KEY (UserID) REFERENCES sysGlobal_Users (GreyFoxUserID),");
			query.Append(" CONSTRAINT FK_kitCms_Hits_RequestContent FOREIGN KEY (RequestContentID) REFERENCES kitCms_Clips (DbContentClipID);");
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
				query = new StringBuilder("CREATE TABLE kitCms_Hits ");
				query.Append(" (DbContentHitID COUNTER(1,1) CONSTRAINT PK_kitCms_Hits PRIMARY KEY, " +
					"UserID LONG," +
					"UserAgent TEXT(255)," +
					"UserHostAddress TEXT(15)," +
					"UserHostName TEXT(255)," +
					"RequestDate DATETIME," +
					"RequestReferrer TEXT(255)," +
					"RequestContentID LONG," +
					"IsUnique BIT);");
			}
			else
			{
				// Microsoft SQL Server
				query = new StringBuilder("CREATE TABLE kitCms_Hits ");
				query.Append(" (DbContentHitID INT IDENTITY(1,1) CONSTRAINT PK_kitCms_Hits PRIMARY KEY, " +
					"UserID INT," +
					"UserAgent NVARCHAR(255)," +
					"UserHostAddress NVARCHAR(15)," +
					"UserHostName NVARCHAR(255)," +
					"RequestDate DATETIME," +
					"RequestReferrer NVARCHAR(255)," +
					"RequestContentID INT," +
					"IsUnique BIT);");
			}

			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

	}
}

