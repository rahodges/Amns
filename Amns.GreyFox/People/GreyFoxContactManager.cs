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

namespace Amns.GreyFox.People
{
	/// <summary>
	/// Datamanager for GreyFoxContact objects.
	/// </summary>
	public class GreyFoxContactManager
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
			"GreyFoxContactID",
			"DisplayName",
			"Prefix",
			"FirstName",
			"MiddleName",
			"LastName",
			"SuffixCommaEnabled",
			"Suffix",
			"Title",
			"ValidationMemo",
			"ValidationFlags",
			"Address1",
			"Address2",
			"City",
			"StateProvince",
			"Country",
			"PostalCode",
			"HomePhone",
			"WorkPhone",
			"Fax",
			"Pager",
			"MobilePhone",
			"Email1",
			"Email2",
			"Url",
			"BusinessName",
			"MemoText",
			"BirthDate",
			"ContactMethod"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "GreyFoxContactID", "LONG", "-1" },
			{ "DisplayName", "TEXT(255)", "string.Empty" },
			{ "Prefix", "TEXT(75)", "string.Empty" },
			{ "FirstName", "TEXT(75)", "string.Empty" },
			{ "MiddleName", "TEXT(75)", "string.Empty" },
			{ "LastName", "TEXT(75)", "string.Empty" },
			{ "SuffixCommaEnabled", "BIT", "" },
			{ "Suffix", "TEXT(75)", "string.Empty" },
			{ "Title", "TEXT(255)", "string.Empty" },
			{ "ValidationMemo", "TEXT(255)", "string.Empty" },
			{ "ValidationFlags", "BYTE", "0" },
			{ "Address1", "TEXT(75)", "string.Empty" },
			{ "Address2", "TEXT(75)", "string.Empty" },
			{ "City", "TEXT(75)", "string.Empty" },
			{ "StateProvince", "TEXT(75)", "string.Empty" },
			{ "Country", "TEXT(75)", "string.Empty" },
			{ "PostalCode", "TEXT(75)", "string.Empty" },
			{ "HomePhone", "TEXT(75)", "string.Empty" },
			{ "WorkPhone", "TEXT(75)", "string.Empty" },
			{ "Fax", "TEXT(75)", "string.Empty" },
			{ "Pager", "TEXT(75)", "string.Empty" },
			{ "MobilePhone", "TEXT(75)", "string.Empty" },
			{ "Email1", "TEXT(75)", "string.Empty" },
			{ "Email2", "TEXT(75)", "string.Empty" },
			{ "Url", "TEXT(75)", "string.Empty" },
			{ "BusinessName", "TEXT(255)", "string.Empty" },
			{ "MemoText", "MEMO", "string.Empty" },
			{ "BirthDate", "DATETIME", "new DateTime(1800, 1, 1)" },
			{ "ContactMethod", "BYTE", "0" }
		};

		#endregion

		#region Default DbModel Constructors

		static GreyFoxContactManager()
		{
		}

		public GreyFoxContactManager(string tableName)
		{
			this.tableName = tableName;
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!GreyFoxContactManager.isInitialized)
			{
				GreyFoxContactManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a GreyFoxContact into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_GreyFoxContact">The GreyFoxContact to insert into the database.</param>
		internal static int _insert(GreyFoxContact greyFoxContact)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO " + greyFoxContact.tableName +
				"(" +
				"DisplayName," +
				"Prefix," +
				"FirstName," +
				"MiddleName," +
				"LastName," +
				"SuffixCommaEnabled," +
				"Suffix," +
				"Title," +
				"ValidationMemo," +
				"ValidationFlags," +
				"Address1," +
				"Address2," +
				"City," +
				"StateProvince," +
				"Country," +
				"PostalCode," +
				"HomePhone," +
				"WorkPhone," +
				"Fax," +
				"Pager," +
				"MobilePhone," +
				"Email1," +
				"Email2," +
				"Url," +
				"BusinessName," +
				"MemoText," +
				"BirthDate," +
				"ContactMethod) VALUES (" +
				"@DisplayName," +
				"@Prefix," +
				"@FirstName," +
				"@MiddleName," +
				"@LastName," +
				"@SuffixCommaEnabled," +
				"@Suffix," +
				"@Title," +
				"@ValidationMemo," +
				"@ValidationFlags," +
				"@Address1," +
				"@Address2," +
				"@City," +
				"@StateProvince," +
				"@Country," +
				"@PostalCode," +
				"@HomePhone," +
				"@WorkPhone," +
				"@Fax," +
				"@Pager," +
				"@MobilePhone," +
				"@Email1," +
				"@Email2," +
				"@Url," +
				"@BusinessName," +
				"@MemoText," +
				"@BirthDate," +
				"@ContactMethod);";

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Access
				// Connection must remain open for IDENTITY to return correct value,
				// therefore use the dbCommand object's Connection directly to control
				// connection state.
				dbCommand = database.GetSqlStringCommand(query);
				fillParameters(database, dbCommand, greyFoxContact);
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
				fillParameters(database, dbCommand, greyFoxContact);
				database.AddOutParameter(dbCommand, "@LastID", DbType.Int32, 10);
				database.ExecuteNonQuery(dbCommand);
				id = (int)dbCommand.Parameters["@LastID"].Value;
			}
			// Store greyFoxContact in cache.
			if(cacheEnabled) cacheStore(greyFoxContact);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(GreyFoxContact greyFoxContact)
		{
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE " + greyFoxContact.tableName + " SET DisplayName=@DisplayName," +
				"Prefix=@Prefix," +
				"FirstName=@FirstName," +
				"MiddleName=@MiddleName," +
				"LastName=@LastName," +
				"SuffixCommaEnabled=@SuffixCommaEnabled," +
				"Suffix=@Suffix," +
				"Title=@Title," +
				"ValidationMemo=@ValidationMemo," +
				"ValidationFlags=@ValidationFlags," +
				"Address1=@Address1," +
				"Address2=@Address2," +
				"City=@City," +
				"StateProvince=@StateProvince," +
				"Country=@Country," +
				"PostalCode=@PostalCode," +
				"HomePhone=@HomePhone," +
				"WorkPhone=@WorkPhone," +
				"Fax=@Fax," +
				"Pager=@Pager," +
				"MobilePhone=@MobilePhone," +
				"Email1=@Email1," +
				"Email2=@Email2," +
				"Url=@Url," +
				"BusinessName=@BusinessName," +
				"MemoText=@MemoText," +
				"BirthDate=@BirthDate," +
				"ContactMethod=@ContactMethod WHERE GreyFoxContactID=@GreyFoxContactID;");

			fillParameters(database, dbCommand, greyFoxContact);
			database.AddInParameter(dbCommand, "GreyFoxContactID", DbType.Int32, greyFoxContact.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			// Store greyFoxContact in cache.
			if (cacheEnabled) cacheStore(greyFoxContact);

			return greyFoxContact.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, GreyFoxContact greyFoxContact)
		{
			#region Name

			addParameter(database, dbCommand, "@DisplayName", DbType.String, greyFoxContact.displayName);
			addParameter(database, dbCommand, "@Prefix", DbType.String, greyFoxContact.prefix);
			addParameter(database, dbCommand, "@FirstName", DbType.String, greyFoxContact.firstName);
			addParameter(database, dbCommand, "@MiddleName", DbType.String, greyFoxContact.middleName);
			addParameter(database, dbCommand, "@LastName", DbType.String, greyFoxContact.lastName);
			addParameter(database, dbCommand, "@SuffixCommaEnabled", DbType.Boolean, greyFoxContact.suffixCommaEnabled);
			addParameter(database, dbCommand, "@Suffix", DbType.String, greyFoxContact.suffix);
			addParameter(database, dbCommand, "@Title", DbType.String, greyFoxContact.title);
			addParameter(database, dbCommand, "@ValidationMemo", DbType.String, greyFoxContact.validationMemo);
			addParameter(database, dbCommand, "@ValidationFlags", DbType.Byte, (Byte)greyFoxContact.validationFlags);

			#endregion

			#region Address

			addParameter(database, dbCommand, "@Address1", DbType.String, greyFoxContact.address1);
			addParameter(database, dbCommand, "@Address2", DbType.String, greyFoxContact.address2);
			addParameter(database, dbCommand, "@City", DbType.String, greyFoxContact.city);
			addParameter(database, dbCommand, "@StateProvince", DbType.String, greyFoxContact.stateProvince);
			addParameter(database, dbCommand, "@Country", DbType.String, greyFoxContact.country);
			addParameter(database, dbCommand, "@PostalCode", DbType.String, greyFoxContact.postalCode);

			#endregion

			#region Voice

			addParameter(database, dbCommand, "@HomePhone", DbType.String, greyFoxContact.homePhone);
			addParameter(database, dbCommand, "@WorkPhone", DbType.String, greyFoxContact.workPhone);
			addParameter(database, dbCommand, "@Fax", DbType.String, greyFoxContact.fax);
			addParameter(database, dbCommand, "@Pager", DbType.String, greyFoxContact.pager);
			addParameter(database, dbCommand, "@MobilePhone", DbType.String, greyFoxContact.mobilePhone);

			#endregion

			#region Internet

			addParameter(database, dbCommand, "@Email1", DbType.String, greyFoxContact.email1);
			addParameter(database, dbCommand, "@Email2", DbType.String, greyFoxContact.email2);
			addParameter(database, dbCommand, "@Url", DbType.String, greyFoxContact.url);

			#endregion

			#region Business

			addParameter(database, dbCommand, "@BusinessName", DbType.String, greyFoxContact.businessName);

			#endregion

			#region Default

			addParameter(database, dbCommand, "@MemoText", DbType.String, greyFoxContact.memoText);
			addParameter(database, dbCommand, "@BirthDate", DbType.Date, greyFoxContact.birthDate);
			addParameter(database, dbCommand, "@ContactMethod", DbType.Byte, (Byte)greyFoxContact.contactMethod);

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

		internal static bool _fill(GreyFoxContact greyFoxContact)
		{
			// Clone item from cache.
			if(cacheEnabled)
			{
				object cachedObject = cacheFind(greyFoxContact.tableName, greyFoxContact.iD);
				if(cachedObject != null)
				{
					((GreyFoxContact)cachedObject).CopyTo(greyFoxContact, true);
					return greyFoxContact.isSynced;
				}
			}

			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM ");
			query.Append(greyFoxContact.tableName);
			query.Append(" WHERE GreyFoxContactID=");
			query.Append(greyFoxContact.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find GreyFoxContactID '{0}'.", 
					greyFoxContact.iD)));
			}

			FillFromReader(greyFoxContact, greyFoxContact.tableName, r, 0, 1);

			// Microsoft DAAB still needs to have the reader closed.
			r.Close();

			// Store greyFoxContact in cache.
			if(cacheEnabled) cacheStore(greyFoxContact);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public GreyFoxContactCollection GetCollection(string whereClause, string sortClause)
		{
			return GetCollection(0, whereClause, sortClause);
		}

		public GreyFoxContactCollection GetCollection(int topCount, string whereClause, string sortClause)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			GreyFoxContactCollection greyFoxContactCollection;


			query = new StringBuilder("SELECT ");

			if(topCount > 0)
			{
				query.Append("TOP ");
				query.Append(topCount);
				query.Append(" ");
			}

			foreach(string columnName in InnerJoinFields)
			{
				query.Append("GreyFoxContact.");
				query.Append(columnName);
				query.Append(",");
			}

			//
			// Remove trailing comma
			//
			query.Length--;
			query.Append(" FROM ");
			query.Append(tableName);
			query.Append(" AS GreyFoxContact");
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

			greyFoxContactCollection = new GreyFoxContactCollection();

			while(r.Read())
			{
				GreyFoxContact greyFoxContact = ParseFromReader(tableName, r, 0, 1);

				greyFoxContactCollection.Add(greyFoxContact);
			}

			// Microsoft DAAB still needs to close readers.
			r.Close();

			return greyFoxContactCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static GreyFoxContact ParseFromReader(string tableName,IDataReader r, int idOffset, int dataOffset)
		{
			GreyFoxContact greyFoxContact = new GreyFoxContact(tableName);
			FillFromReader(greyFoxContact, tableName, r, idOffset, dataOffset);
			return greyFoxContact;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(GreyFoxContact greyFoxContact, string tableName,IDataReader r, int idOffset, int dataOffset)
		{
			greyFoxContact.tableName = tableName;
			greyFoxContact.iD = r.GetInt32(idOffset);
			greyFoxContact.isSynced = true;
			greyFoxContact.isPlaceHolder = false;

			if(!r.IsDBNull(0+dataOffset)) 
				greyFoxContact.displayName = r.GetString(0+dataOffset);
			else
				greyFoxContact.displayName = string.Empty;
			if(!r.IsDBNull(1+dataOffset)) 
				greyFoxContact.prefix = r.GetString(1+dataOffset);
			else
				greyFoxContact.prefix = string.Empty;
			if(!r.IsDBNull(2+dataOffset)) 
				greyFoxContact.firstName = r.GetString(2+dataOffset);
			else
				greyFoxContact.firstName = string.Empty;
			if(!r.IsDBNull(3+dataOffset)) 
				greyFoxContact.middleName = r.GetString(3+dataOffset);
			else
				greyFoxContact.middleName = string.Empty;
			if(!r.IsDBNull(4+dataOffset)) 
				greyFoxContact.lastName = r.GetString(4+dataOffset);
			else
				greyFoxContact.lastName = string.Empty;
			greyFoxContact.suffixCommaEnabled = r.GetBoolean(5+dataOffset);
			if(!r.IsDBNull(6+dataOffset)) 
				greyFoxContact.suffix = r.GetString(6+dataOffset);
			else
				greyFoxContact.suffix = string.Empty;
			if(!r.IsDBNull(7+dataOffset)) 
				greyFoxContact.title = r.GetString(7+dataOffset);
			else
				greyFoxContact.title = string.Empty;
			if(!r.IsDBNull(8+dataOffset)) 
				greyFoxContact.validationMemo = r.GetString(8+dataOffset);
			else
				greyFoxContact.validationMemo = string.Empty;
			greyFoxContact.validationFlags = (GreyFoxContactValidationFlag)r.GetByte(9+dataOffset);
			if(!r.IsDBNull(10+dataOffset)) 
				greyFoxContact.address1 = r.GetString(10+dataOffset);
			else
				greyFoxContact.address1 = string.Empty;
			if(!r.IsDBNull(11+dataOffset)) 
				greyFoxContact.address2 = r.GetString(11+dataOffset);
			else
				greyFoxContact.address2 = string.Empty;
			if(!r.IsDBNull(12+dataOffset)) 
				greyFoxContact.city = r.GetString(12+dataOffset);
			else
				greyFoxContact.city = string.Empty;
			if(!r.IsDBNull(13+dataOffset)) 
				greyFoxContact.stateProvince = r.GetString(13+dataOffset);
			else
				greyFoxContact.stateProvince = string.Empty;
			if(!r.IsDBNull(14+dataOffset)) 
				greyFoxContact.country = r.GetString(14+dataOffset);
			else
				greyFoxContact.country = string.Empty;
			if(!r.IsDBNull(15+dataOffset)) 
				greyFoxContact.postalCode = r.GetString(15+dataOffset);
			else
				greyFoxContact.postalCode = string.Empty;
			if(!r.IsDBNull(16+dataOffset)) 
				greyFoxContact.homePhone = r.GetString(16+dataOffset);
			else
				greyFoxContact.homePhone = string.Empty;
			if(!r.IsDBNull(17+dataOffset)) 
				greyFoxContact.workPhone = r.GetString(17+dataOffset);
			else
				greyFoxContact.workPhone = string.Empty;
			if(!r.IsDBNull(18+dataOffset)) 
				greyFoxContact.fax = r.GetString(18+dataOffset);
			else
				greyFoxContact.fax = string.Empty;
			if(!r.IsDBNull(19+dataOffset)) 
				greyFoxContact.pager = r.GetString(19+dataOffset);
			else
				greyFoxContact.pager = string.Empty;
			if(!r.IsDBNull(20+dataOffset)) 
				greyFoxContact.mobilePhone = r.GetString(20+dataOffset);
			else
				greyFoxContact.mobilePhone = string.Empty;
			if(!r.IsDBNull(21+dataOffset)) 
				greyFoxContact.email1 = r.GetString(21+dataOffset);
			else
				greyFoxContact.email1 = string.Empty;
			if(!r.IsDBNull(22+dataOffset)) 
				greyFoxContact.email2 = r.GetString(22+dataOffset);
			else
				greyFoxContact.email2 = string.Empty;
			if(!r.IsDBNull(23+dataOffset)) 
				greyFoxContact.url = r.GetString(23+dataOffset);
			else
				greyFoxContact.url = string.Empty;
			if(!r.IsDBNull(24+dataOffset)) 
				greyFoxContact.businessName = r.GetString(24+dataOffset);
			else
				greyFoxContact.businessName = string.Empty;
			if(!r.IsDBNull(25+dataOffset)) 
				greyFoxContact.memoText = r.GetString(25+dataOffset);
			else
				greyFoxContact.memoText = string.Empty;
			if(!r.IsDBNull(26+dataOffset)) 
				greyFoxContact.birthDate = r.GetDateTime(26+dataOffset);
			else
				greyFoxContact.birthDate = DateTime.MinValue;
			greyFoxContact.contactMethod = (GreyFoxContactMethod)r.GetByte(27+dataOffset);
		}

		#endregion

		#region Default DbModel Delete Method

		internal static void _delete(string tableName, int id)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("DELETE FROM ");
			query.Append(tableName);
			query.Append(" WHERE GreyFoxContactID=");
			query.Append(id);
			query.Append(';');

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			cacheRemove(tableName, id);
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
				query = new StringBuilder("CREATE TABLE ");
				query.Append(tableName);
				query.Append(" (GreyFoxContactID COUNTER(1,1) CONSTRAINT PK_" + tableName + " PRIMARY KEY, " +
					"DisplayName TEXT(255)," +
					"Prefix TEXT(75)," +
					"FirstName TEXT(75)," +
					"MiddleName TEXT(75)," +
					"LastName TEXT(75)," +
					"SuffixCommaEnabled BIT," +
					"Suffix TEXT(75)," +
					"Title TEXT(255)," +
					"ValidationMemo TEXT(255)," +
					"ValidationFlags BYTE," +
					"Address1 TEXT(75)," +
					"Address2 TEXT(75)," +
					"City TEXT(75)," +
					"StateProvince TEXT(75)," +
					"Country TEXT(75)," +
					"PostalCode TEXT(75)," +
					"HomePhone TEXT(75)," +
					"WorkPhone TEXT(75)," +
					"Fax TEXT(75)," +
					"Pager TEXT(75)," +
					"MobilePhone TEXT(75)," +
					"Email1 TEXT(75)," +
					"Email2 TEXT(75)," +
					"Url TEXT(75)," +
					"BusinessName TEXT(255)," +
					"MemoText MEMO," +
					"BirthDate DATETIME," +
					"ContactMethod BYTE);");
			}
			else
			{
				// Microsoft SQL Server
				query = new StringBuilder("CREATE TABLE ");
				query.Append(tableName);
				query.Append(" (GreyFoxContactID INT IDENTITY(1,1) CONSTRAINT PK_" + tableName + " PRIMARY KEY, " +
					"DisplayName NVARCHAR(255)," +
					"Prefix NVARCHAR(75)," +
					"FirstName NVARCHAR(75)," +
					"MiddleName NVARCHAR(75)," +
					"LastName NVARCHAR(75)," +
					"SuffixCommaEnabled BIT," +
					"Suffix NVARCHAR(75)," +
					"Title NVARCHAR(255)," +
					"ValidationMemo NVARCHAR(255)," +
					"ValidationFlags TINYINT," +
					"Address1 NVARCHAR(75)," +
					"Address2 NVARCHAR(75)," +
					"City NVARCHAR(75)," +
					"StateProvince NVARCHAR(75)," +
					"Country NVARCHAR(75)," +
					"PostalCode NVARCHAR(75)," +
					"HomePhone NVARCHAR(75)," +
					"WorkPhone NVARCHAR(75)," +
					"Fax NVARCHAR(75)," +
					"Pager NVARCHAR(75)," +
					"MobilePhone NVARCHAR(75)," +
					"Email1 NVARCHAR(75)," +
					"Email2 NVARCHAR(75)," +
					"Url NVARCHAR(75)," +
					"BusinessName NVARCHAR(255)," +
					"MemoText NTEXT," +
					"BirthDate DATETIME," +
					"ContactMethod TINYINT);");
			}

			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

		#region Cache Methods

		private static void cacheStore(GreyFoxContact greyFoxContact)
		{
			CacheManager cache = CacheFactory.GetCacheManager();
			cache.Add(greyFoxContact.tableName + "_" + greyFoxContact.iD.ToString(), greyFoxContact);
		}

		private static GreyFoxContact cacheFind(string tableName, int id)
		{
			object cachedObject;
			CacheManager cache = CacheFactory.GetCacheManager();
			cachedObject = cache.GetData(tableName + "_" + id.ToString());
			if(cachedObject == null)
				return null;
			return (GreyFoxContact)cachedObject;
		}

		private static void cacheRemove(string tableName, int id)
		{
			CacheManager cache = CacheFactory.GetCacheManager();
			cache.Remove(tableName + "_" + id.ToString());
		}

		#endregion

		//--- Begin Custom Code ---
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																													
		public bool FullNameExists(string fullName)
		{
            bool exists;
            Database database;
            DbCommand dbCommand;            
			GreyFoxContact c; 
            
            c = new GreyFoxContact();
            c.ParseName(fullName);

            database = DatabaseFactory.CreateDatabase();
            dbCommand = database.GetSqlStringCommand("SELECT COUNT(*) " +
				"FROM " + tableName + 
				"WHERE FirstName=@FirstName " +
				"	AND MiddleName=@MiddleName " +
				"	AND LastName=@LastName " +
				"	AND Suffix=@Suffix;");

            addParameter(database, dbCommand, "FirstName", DbType.String, c.firstName);
            addParameter(database, dbCommand, "MiddleName", DbType.String, c.middleName);
            addParameter(database, dbCommand, "LastName", DbType.String, c.lastName);
            addParameter(database, dbCommand, "Suffix", DbType.String, c.Suffix);

            exists = ((int) database.ExecuteScalar(dbCommand)) > 0;

			c = null;

			return exists;
		}

		public bool BusinessNameExists(string businessName)
		{
            Database database;
            DbCommand dbCommand;

            database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand("SELECT COUNT(*) " +
				"FROM " + tableName + 
				"WHERE BusinessName=@BusinessName;");

            addParameter(database, dbCommand, "BusinessName", DbType.String, businessName);
			bool exists = ((int) database.ExecuteScalar(dbCommand)) > 0;			
			return exists;
		}

		/// <summary>
		/// An object to cache the parameterized query for full name queries.
		/// </summary>
		public GreyFoxContact FindByFullName(string fullName, bool placeHolderOnly)
		{
            StringBuilder query;
            Database database;
            DbCommand dbCommand;
            IDataReader r;
            GreyFoxContact c;

            // create a new contact to parse a name into.
			c = new GreyFoxContact(tableName);
			c.ParseName(fullName);

			query = new StringBuilder();
			query.Append("SELECT ");
			if(placeHolderOnly)
				query.Append(InnerJoinFields[0]);
			else
				query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM ");
			query.Append(tableName);
			query.Append(" WHERE FirstName=@FirstName AND " +
				"MiddleName=@MiddleName AND " +
				"LastName=@LastName AND " +
				"Suffix=@Suffix;");

            database = DatabaseFactory.CreateDatabase();
            dbCommand = database.GetSqlStringCommand(query.ToString());

            addParameter(database, dbCommand, "FirstName", DbType.String, c.firstName);
            addParameter(database, dbCommand, "MiddleName", DbType.String, c.middleName);
            addParameter(database, dbCommand, "LastName", DbType.String, c.lastName);
            addParameter(database, dbCommand, "Suffix", DbType.String, c.Suffix);

            r = database.ExecuteReader(dbCommand);
			if(r.Read())
			{
				if(placeHolderOnly)
					c = GreyFoxContact.NewPlaceHolder(tableName, r.GetInt32(0));
				else
					c = GreyFoxContactManager.ParseFromReader(tableName, r, 0, 1);
			}
			return c;

		}

		public GreyFoxContact FindByBusinessName(string businessName, bool placeHolderOnly)
		{
            StringBuilder query;
            Database database;
            DbCommand dbCommand;
            IDataReader r;
            GreyFoxContact c;

			query = new StringBuilder();
			query.Append("SELECT ");
			if(placeHolderOnly)
				query.Append(InnerJoinFields[0]);
			else
				query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM ");
			query.Append(tableName);
			query.Append(" WHERE BusinessName=@BusinessName;");

            database = DatabaseFactory.CreateDatabase();
            dbCommand = database.GetSqlStringCommand(query.ToString());
            addParameter(database, dbCommand, "BusinessName", DbType.String, businessName);
			r = database.ExecuteReader(dbCommand);
			
			if(r.Read())
			{
				if(placeHolderOnly)
					c = GreyFoxContact.NewPlaceHolder(tableName, r.GetInt32(0));
				else
					c = GreyFoxContactManager.ParseFromReader(tableName, r, 0, 1);
			}
			else
			{
				c = new GreyFoxContact(tableName);
				c.BusinessName = businessName;
			}
			
			return c;
		}
																										
		public void CorrectStateProvinceFields()
		{
            Database database;
            DbCommand dbCommand;

            database = DatabaseFactory.CreateDatabase();

			for(int x = 0; x <= GreyFoxContact.States.GetUpperBound(0); x++)
			{
				dbCommand = database.GetSqlStringCommand(string.Format("UPDATE {0} " +
					"SET StateProvince='{1}' WHERE StateProvince LIKE '{2}';",
					tableName, GreyFoxContact.States[x,1], GreyFoxContact.States[x,0]));
                database.ExecuteNonQuery(dbCommand);
			}
			for(int x = 0; x <= GreyFoxContact.CanadianProvinces.GetUpperBound(0); x++)
			{
				dbCommand = database.GetSqlStringCommand(string.Format("UPDATE {0} " +
					"SET StateProvince='{1}' WHERE StateProvince LIKE '{2}';",
					tableName, GreyFoxContact.CanadianProvinces[x,1], GreyFoxContact.CanadianProvinces[x,0]));
                database.ExecuteNonQuery(dbCommand);
			}
		}

		public void Delete(int contactID)
		{
			_delete(tableName, contactID);
		}

		//--- End Custom Code ---
	}
}

