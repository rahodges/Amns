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
	#region Child Flags Enumeration

	public enum YariMediaRecordFlags : int { MediaType};

	#endregion

	/// <summary>
	/// Datamanager for YariMediaRecord objects.
	/// </summary>
	public class YariMediaRecordManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "kitYari_Books";


		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}

		#endregion

		#region Inner Join Field Array

		public static readonly string[] InnerJoinFields = new string[] {
			"YariMediaRecordID",
			"EndNoteReferenceID",
			"PublishYear",
			"Title",
			"Pages",
			"Edition",
			"Isbn",
			"Label",
			"AbstractText",
			"ContentsText",
			"NotesText",
			"AmazonFillDate",
			"AmazonRefreshDate",
			"ImageUrlSmall",
			"ImageUrlMedium",
			"ImageUrlLarge",
			"AmazonListPrice",
			"AmazonOurPrice",
			"AmazonAvailability",
			"AmazonMedia",
			"AmazonReleaseDate",
			"AmazonAsin",
			"AbstractEnabled",
			"ContentsEnabled",
			"NotesEnabled",
			"Authors",
			"SecondaryAuthors",
			"Publisher",
			"MediaTypeID"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "YariMediaRecordID", "LONG", "-1" },
			{ "EndNoteReferenceID", "LONG", "" },
			{ "PublishYear", "LONG", "" },
			{ "Title", "TEXT(255)", "string.Empty" },
			{ "Pages", "LONG", "" },
			{ "Edition", "TEXT(75)", "string.Empty" },
			{ "Isbn", "TEXT(15)", "string.Empty" },
			{ "Label", "TEXT(5)", "string.Empty" },
			{ "AbstractText", "MEMO", "string.Empty" },
			{ "ContentsText", "MEMO", "string.Empty" },
			{ "NotesText", "MEMO", "string.Empty" },
			{ "AmazonFillDate", "DATETIME", "" },
			{ "AmazonRefreshDate", "DATETIME", "" },
			{ "ImageUrlSmall", "TEXT(255)", "" },
			{ "ImageUrlMedium", "TEXT(255)", "" },
			{ "ImageUrlLarge", "TEXT(255)", "" },
			{ "AmazonListPrice", "CURRENCY", "" },
			{ "AmazonOurPrice", "CURRENCY", "" },
			{ "AmazonAvailability", "TEXT(75)", "" },
			{ "AmazonMedia", "TEXT(75)", "string.Empty" },
			{ "AmazonReleaseDate", "DATETIME", "" },
			{ "AmazonAsin", "TEXT(15)", "" },
			{ "AbstractEnabled", "BIT", "" },
			{ "ContentsEnabled", "BIT", "" },
			{ "NotesEnabled", "BIT", "" },
			{ "Authors", "TEXT(255)", "" },
			{ "SecondaryAuthors", "TEXT(255)", "" },
			{ "Publisher", "TEXT(255)", "" },
			{ "MediaTypeID", "LONG", "null" }
		};

		#endregion

		#region Default DbModel Constructors

		static YariMediaRecordManager()
		{
		}

		public YariMediaRecordManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!YariMediaRecordManager.isInitialized)
			{
				YariMediaRecordManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a YariMediaRecord into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_YariMediaRecord">The YariMediaRecord to insert into the database.</param>
		internal static int _insert(YariMediaRecord yariMediaRecord)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO kitYari_Books " +
				"(" +
				"EndNoteReferenceID," +
				"PublishYear," +
				"Title," +
				"Pages," +
				"Edition," +
				"Isbn," +
				"Label," +
				"AbstractText," +
				"ContentsText," +
				"NotesText," +
				"AmazonFillDate," +
				"AmazonRefreshDate," +
				"ImageUrlSmall," +
				"ImageUrlMedium," +
				"ImageUrlLarge," +
				"AmazonListPrice," +
				"AmazonOurPrice," +
				"AmazonAvailability," +
				"AmazonMedia," +
				"AmazonReleaseDate," +
				"AmazonAsin," +
				"AbstractEnabled," +
				"ContentsEnabled," +
				"NotesEnabled," +
				"Authors," +
				"SecondaryAuthors," +
				"Publisher," +
				"MediaTypeID) VALUES (" +
				"@EndNoteReferenceID," +
				"@PublishYear," +
				"@Title," +
				"@Pages," +
				"@Edition," +
				"@Isbn," +
				"@Label," +
				"@AbstractText," +
				"@ContentsText," +
				"@NotesText," +
				"@AmazonFillDate," +
				"@AmazonRefreshDate," +
				"@ImageUrlSmall," +
				"@ImageUrlMedium," +
				"@ImageUrlLarge," +
				"@AmazonListPrice," +
				"@AmazonOurPrice," +
				"@AmazonAvailability," +
				"@AmazonMedia," +
				"@AmazonReleaseDate," +
				"@AmazonAsin," +
				"@AbstractEnabled," +
				"@ContentsEnabled," +
				"@NotesEnabled," +
				"@Authors," +
				"@SecondaryAuthors," +
				"@Publisher," +
				"@MediaTypeID);";

			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				// Microsoft Access
				// Connection must remain open for IDENTITY to return correct value,
				// therefore use the dbCommand object's Connection directly to control
				// connection state.
				dbCommand = database.GetSqlStringCommand(query);
				fillParameters(database, dbCommand, yariMediaRecord);
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
				fillParameters(database, dbCommand, yariMediaRecord);
				database.AddOutParameter(dbCommand, "@LastID", DbType.Int32, 10);
				database.ExecuteNonQuery(dbCommand);
				id = (int)dbCommand.Parameters["@LastID"].Value;
			}

			// Save child relationships for Keywords.
			if(yariMediaRecord.keywords != null)
			{
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitYari_BooksChildren_Keywords " +
					"(YariMediaRecordID, YariMediaKeywordID)" + 
					" VALUES (@YariMediaRecordID, @YariMediaKeywordID);");
				addParameter(database, dbCommand, "@YariMediaRecordID", DbType.Int32);
				addParameter(database, dbCommand, "@YariMediaKeywordID", DbType.Int32);
				foreach(YariMediaKeyword item in yariMediaRecord.keywords)
				{
					dbCommand.Parameters["@YariMediaRecordID"].Value = id;
					dbCommand.Parameters["@YariMediaKeywordID"].Value = item.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(YariMediaRecord yariMediaRecord)
		{
			Database database;
			DbCommand dbCommand;

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE kitYari_Books SET EndNoteReferenceID=@EndNoteReferenceID," +
				"PublishYear=@PublishYear," +
				"Title=@Title," +
				"Pages=@Pages," +
				"Edition=@Edition," +
				"Isbn=@Isbn," +
				"Label=@Label," +
				"AbstractText=@AbstractText," +
				"ContentsText=@ContentsText," +
				"NotesText=@NotesText," +
				"AmazonFillDate=@AmazonFillDate," +
				"AmazonRefreshDate=@AmazonRefreshDate," +
				"ImageUrlSmall=@ImageUrlSmall," +
				"ImageUrlMedium=@ImageUrlMedium," +
				"ImageUrlLarge=@ImageUrlLarge," +
				"AmazonListPrice=@AmazonListPrice," +
				"AmazonOurPrice=@AmazonOurPrice," +
				"AmazonAvailability=@AmazonAvailability," +
				"AmazonMedia=@AmazonMedia," +
				"AmazonReleaseDate=@AmazonReleaseDate," +
				"AmazonAsin=@AmazonAsin," +
				"AbstractEnabled=@AbstractEnabled," +
				"ContentsEnabled=@ContentsEnabled," +
				"NotesEnabled=@NotesEnabled," +
				"Authors=@Authors," +
				"SecondaryAuthors=@SecondaryAuthors," +
				"Publisher=@Publisher," +
				"MediaTypeID=@MediaTypeID WHERE YariMediaRecordID=@YariMediaRecordID;");

			fillParameters(database, dbCommand, yariMediaRecord);
			database.AddInParameter(dbCommand, "YariMediaRecordID", DbType.Int32, yariMediaRecord.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			if(yariMediaRecord.keywords != null)
			{

				// Delete child relationships for Keywords.
				dbCommand = database.GetSqlStringCommand("DELETE  FROM kitYari_BooksChildren_Keywords WHERE YariMediaRecordID=@YariMediaRecordID;");
				database.AddInParameter(dbCommand, "@YariMediaRecordID", DbType.Int32, yariMediaRecord.iD);
				database.ExecuteNonQuery(dbCommand);

				// Save child relationships for Keywords.
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitYari_BooksChildren_Keywords (YariMediaRecordID, YariMediaKeywordID) VALUES (@YariMediaRecordID, @YariMediaKeywordID);");
				database.AddInParameter(dbCommand, "@YariMediaRecordID", DbType.Int32, yariMediaRecord.iD);
				database.AddInParameter(dbCommand, "@YariMediaKeywordID", DbType.Int32);
				foreach(YariMediaKeyword yariMediaKeyword in yariMediaRecord.keywords)
				{
					dbCommand.Parameters["@YariMediaKeywordID"].Value = yariMediaKeyword.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}

			return yariMediaRecord.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, YariMediaRecord yariMediaRecord)
		{
			#region Default

			addParameter(database, dbCommand, "EndNoteReferenceID", DbType.Int32, yariMediaRecord.endNoteReferenceID);
			addParameter(database, dbCommand, "PublishYear", DbType.Int32, yariMediaRecord.publishYear);
			addParameter(database, dbCommand, "Title", DbType.String, yariMediaRecord.title);
			addParameter(database, dbCommand, "Pages", DbType.Int32, yariMediaRecord.pages);
			addParameter(database, dbCommand, "Edition", DbType.String, yariMediaRecord.edition);
			addParameter(database, dbCommand, "Isbn", DbType.String, yariMediaRecord.isbn);
			addParameter(database, dbCommand, "Label", DbType.String, yariMediaRecord.label);
			addParameter(database, dbCommand, "AbstractText", DbType.String, yariMediaRecord.abstractText);
			addParameter(database, dbCommand, "ContentsText", DbType.String, yariMediaRecord.contentsText);
			addParameter(database, dbCommand, "NotesText", DbType.String, yariMediaRecord.notesText);
			addParameter(database, dbCommand, "AmazonFillDate", DbType.Date, yariMediaRecord.amazonFillDate);
			addParameter(database, dbCommand, "AmazonRefreshDate", DbType.Date, yariMediaRecord.amazonRefreshDate);
			addParameter(database, dbCommand, "ImageUrlSmall", DbType.String, yariMediaRecord.imageUrlSmall);
			addParameter(database, dbCommand, "ImageUrlMedium", DbType.String, yariMediaRecord.imageUrlMedium);
			addParameter(database, dbCommand, "ImageUrlLarge", DbType.String, yariMediaRecord.imageUrlLarge);
			addParameter(database, dbCommand, "AmazonListPrice", DbType.Currency, yariMediaRecord.amazonListPrice);
			addParameter(database, dbCommand, "AmazonOurPrice", DbType.Currency, yariMediaRecord.amazonOurPrice);
			addParameter(database, dbCommand, "AmazonAvailability", DbType.String, yariMediaRecord.amazonAvailability);
			addParameter(database, dbCommand, "AmazonMedia", DbType.String, yariMediaRecord.amazonMedia);
			addParameter(database, dbCommand, "AmazonReleaseDate", DbType.Date, yariMediaRecord.amazonReleaseDate);
			addParameter(database, dbCommand, "AmazonAsin", DbType.String, yariMediaRecord.amazonAsin);
			addParameter(database, dbCommand, "AbstractEnabled", DbType.Boolean, yariMediaRecord.abstractEnabled);
			addParameter(database, dbCommand, "ContentsEnabled", DbType.Boolean, yariMediaRecord.contentsEnabled);
			addParameter(database, dbCommand, "NotesEnabled", DbType.Boolean, yariMediaRecord.notesEnabled);
			addParameter(database, dbCommand, "Authors", DbType.String, yariMediaRecord.authors);
			addParameter(database, dbCommand, "SecondaryAuthors", DbType.String, yariMediaRecord.secondaryAuthors);
			addParameter(database, dbCommand, "Publisher", DbType.String, yariMediaRecord.publisher);
			if(yariMediaRecord.mediaType == null)
			{
				addParameter(database, dbCommand, "MediaTypeID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "MediaTypeID", DbType.Int32, yariMediaRecord.mediaType.ID);
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

		internal static bool _fill(YariMediaRecord yariMediaRecord)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM kitYari_Books WHERE YariMediaRecordID=");
			query.Append(yariMediaRecord.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find YariMediaRecordID '{0}'.", 
					yariMediaRecord.iD)));
			}

			FillFromReader(yariMediaRecord, r, 0, 1);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public YariMediaRecordCollection GetCollection(string whereClause, string sortClause, params YariMediaRecordFlags[] optionFlags)
		{
			return GetCollection(0, whereClause, sortClause, optionFlags);
		}

		public YariMediaRecordCollection GetCollection(int topCount, string whereClause, string sortClause, params YariMediaRecordFlags[] optionFlags)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			YariMediaRecordCollection yariMediaRecordCollection;

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
				query.Append("kitYari_Books.");
				query.Append(columnName);
				query.Append(",");
			}

			innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;
			int mediaTypeOffset = -1;

			//
			// Append Option Flag Fields
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case YariMediaRecordFlags.MediaType:
							for(int i = 0; i <= YariMediaTypeManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("kitYari_MediaTypes.");
								query.Append(YariMediaTypeManager.InnerJoinFields[i]);
								query.Append(",");
							}
							mediaTypeOffset = innerJoinOffset;
							innerJoinOffset = mediaTypeOffset + YariMediaTypeManager.InnerJoinFields.GetUpperBound(0) + 1;
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

				query.Append("kitYari_Books");
			}
			else
			{
				query.Append(" FROM kitYari_Books ");
			}
			//
			// Finish INNER JOIN expressions
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case YariMediaRecordFlags.MediaType:
							query.Append(" LEFT JOIN kitYari_MediaTypes ON kitYari_Books.MediaTypeID = kitYari_MediaTypes.YariMediaTypeID)");
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

			yariMediaRecordCollection = new YariMediaRecordCollection();

			while(r.Read())
			{
				YariMediaRecord yariMediaRecord = ParseFromReader(r, 0, 1);

				// Fill MediaType
				if(mediaTypeOffset != -1 && !r.IsDBNull(mediaTypeOffset))
					YariMediaTypeManager.FillFromReader(yariMediaRecord.mediaType, r, mediaTypeOffset, mediaTypeOffset+1);

				yariMediaRecordCollection.Add(yariMediaRecord);
			}

			return yariMediaRecordCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static YariMediaRecord ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			YariMediaRecord yariMediaRecord = new YariMediaRecord();
			FillFromReader(yariMediaRecord, r, idOffset, dataOffset);
			return yariMediaRecord;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(YariMediaRecord yariMediaRecord, IDataReader r, int idOffset, int dataOffset)
		{
			yariMediaRecord.iD = r.GetInt32(idOffset);
			yariMediaRecord.isSynced = true;
			yariMediaRecord.isPlaceHolder = false;

			if(!r.IsDBNull(0+dataOffset)) 
				yariMediaRecord.endNoteReferenceID = r.GetInt32(0+dataOffset);
			if(!r.IsDBNull(1+dataOffset)) 
				yariMediaRecord.publishYear = r.GetInt32(1+dataOffset);
			if(!r.IsDBNull(2+dataOffset)) 
				yariMediaRecord.title = r.GetString(2+dataOffset);
			else
				yariMediaRecord.title = null;
			if(!r.IsDBNull(3+dataOffset)) 
				yariMediaRecord.pages = r.GetInt32(3+dataOffset);
			if(!r.IsDBNull(4+dataOffset)) 
				yariMediaRecord.edition = r.GetString(4+dataOffset);
			else
				yariMediaRecord.edition = null;
			if(!r.IsDBNull(5+dataOffset)) 
				yariMediaRecord.isbn = r.GetString(5+dataOffset);
			else
				yariMediaRecord.isbn = null;
			if(!r.IsDBNull(6+dataOffset)) 
				yariMediaRecord.label = r.GetString(6+dataOffset);
			else
				yariMediaRecord.label = null;
			if(!r.IsDBNull(7+dataOffset)) 
				yariMediaRecord.abstractText = r.GetString(7+dataOffset);
			else
				yariMediaRecord.abstractText = null;
			if(!r.IsDBNull(8+dataOffset)) 
				yariMediaRecord.contentsText = r.GetString(8+dataOffset);
			else
				yariMediaRecord.contentsText = null;
			if(!r.IsDBNull(9+dataOffset)) 
				yariMediaRecord.notesText = r.GetString(9+dataOffset);
			else
				yariMediaRecord.notesText = null;
			if(!r.IsDBNull(10+dataOffset)) 
				yariMediaRecord.amazonFillDate = r.GetDateTime(10+dataOffset);
			else
				yariMediaRecord.amazonFillDate = DateTime.MinValue;
			if(!r.IsDBNull(11+dataOffset)) 
				yariMediaRecord.amazonRefreshDate = r.GetDateTime(11+dataOffset);
			else
				yariMediaRecord.amazonRefreshDate = DateTime.MinValue;
			if(!r.IsDBNull(12+dataOffset)) 
				yariMediaRecord.imageUrlSmall = r.GetString(12+dataOffset);
			else
				yariMediaRecord.imageUrlSmall = null;
			if(!r.IsDBNull(13+dataOffset)) 
				yariMediaRecord.imageUrlMedium = r.GetString(13+dataOffset);
			else
				yariMediaRecord.imageUrlMedium = null;
			if(!r.IsDBNull(14+dataOffset)) 
				yariMediaRecord.imageUrlLarge = r.GetString(14+dataOffset);
			else
				yariMediaRecord.imageUrlLarge = null;
			if(!r.IsDBNull(15+dataOffset)) 
				yariMediaRecord.amazonListPrice = r.GetDecimal(15+dataOffset);
			if(!r.IsDBNull(16+dataOffset)) 
				yariMediaRecord.amazonOurPrice = r.GetDecimal(16+dataOffset);
			if(!r.IsDBNull(17+dataOffset)) 
				yariMediaRecord.amazonAvailability = r.GetString(17+dataOffset);
			else
				yariMediaRecord.amazonAvailability = null;
			if(!r.IsDBNull(18+dataOffset)) 
				yariMediaRecord.amazonMedia = r.GetString(18+dataOffset);
			else
				yariMediaRecord.amazonMedia = null;
			if(!r.IsDBNull(19+dataOffset)) 
				yariMediaRecord.amazonReleaseDate = r.GetDateTime(19+dataOffset);
			else
				yariMediaRecord.amazonReleaseDate = DateTime.MinValue;
			if(!r.IsDBNull(20+dataOffset)) 
				yariMediaRecord.amazonAsin = r.GetString(20+dataOffset);
			else
				yariMediaRecord.amazonAsin = null;
			if(!r.IsDBNull(21+dataOffset)) 
				yariMediaRecord.abstractEnabled = r.GetBoolean(21+dataOffset);
			if(!r.IsDBNull(22+dataOffset)) 
				yariMediaRecord.contentsEnabled = r.GetBoolean(22+dataOffset);
			if(!r.IsDBNull(23+dataOffset)) 
				yariMediaRecord.notesEnabled = r.GetBoolean(23+dataOffset);
			if(!r.IsDBNull(24+dataOffset)) 
				yariMediaRecord.authors = r.GetString(24+dataOffset);
			else
				yariMediaRecord.authors = null;
			if(!r.IsDBNull(25+dataOffset)) 
				yariMediaRecord.secondaryAuthors = r.GetString(25+dataOffset);
			else
				yariMediaRecord.secondaryAuthors = null;
			if(!r.IsDBNull(26+dataOffset)) 
				yariMediaRecord.publisher = r.GetString(26+dataOffset);
			else
				yariMediaRecord.publisher = null;
			if(!r.IsDBNull(27+dataOffset) && r.GetInt32(27+dataOffset) > 0)
			{
				yariMediaRecord.mediaType = YariMediaType.NewPlaceHolder(r.GetInt32(27+dataOffset));
			}
		}

		#endregion

		#region Default DbModel Fill Methods

		public static void FillKeywords(YariMediaRecord yariMediaRecord)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			s = new StringBuilder("SELECT YariMediaKeywordID FROM kitYari_BooksChildren_Keywords ");
			s.Append("WHERE YariMediaRecordID=");
			s.Append(yariMediaRecord.iD);
			s.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(s.ToString());
			r = database.ExecuteReader(dbCommand);

			YariMediaKeywordCollection keywords;
			if(yariMediaRecord.keywords != null)
			{
				keywords = yariMediaRecord.keywords;
				keywords.Clear();
			}
			else
			{
				keywords = new YariMediaKeywordCollection();
				yariMediaRecord.keywords = keywords;
			}

			while(r.Read())
				keywords.Add(YariMediaKeyword.NewPlaceHolder(r.GetInt32(0)));

			yariMediaRecord.Keywords = keywords;
		}

		public static void FillKeywords(YariMediaRecordCollection yariMediaRecordCollection)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			if(yariMediaRecordCollection.Count > 0)
			{
				s = new StringBuilder("SELECT YariMediaRecordID, YariMediaKeywordID FROM kitYari_BooksChildren_Keywords ORDER BY YariMediaRecordID; ");

				// Clone and sort collection by ID first to fill children in one pass
				YariMediaRecordCollection clonedCollection = yariMediaRecordCollection.Clone();
				clonedCollection.Sort();

				database = DatabaseFactory.CreateDatabase();
				dbCommand = database.GetSqlStringCommand(s.ToString());
				r = database.ExecuteReader(dbCommand);

				bool more = r.Read();

				foreach(YariMediaRecord yariMediaRecord in clonedCollection)
				{
					YariMediaKeywordCollection keywords;
					if(yariMediaRecord.keywords != null)
					{
						keywords = yariMediaRecord.keywords;
						keywords.Clear();
					}
					else
					{
						keywords = new YariMediaKeywordCollection();
						yariMediaRecord.keywords = keywords;
					}

					while(more)
					{
						if(r.GetInt32(0) < yariMediaRecord.iD)
						{
							more = r.Read();
						}
						else if(r.GetInt32(0) == yariMediaRecord.iD)
						{
							keywords.Add(YariMediaKeyword.NewPlaceHolder(r.GetInt32(1)));
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

			query = new StringBuilder("DELETE FROM kitYari_Books WHERE YariMediaRecordID=");
			query.Append(id);
			query.Append(';');

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);


			// Delete child relationships for Keywords.
			query.Length = 0;
			query.Append("DELETE FROM kitYari_BooksChildren_Keywords WHERE ");
			query.Append("YariMediaRecordID=");
			query.Append(id);
			query.Append(";");
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
			query.Append("ALTER TABLE kitYari_Books ADD ");
			query.Append(" CONSTRAINT FK_kitYari_Books_MediaType FOREIGN KEY (MediaTypeID) REFERENCES kitYari_MediaTypes (YariMediaTypeID);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			query.Length = 0;
			query.Append("ALTER TABLE kitYari_BooksChildren_Keywords ADD");
			query.Append(" CONSTRAINT FK_kitYari_Books_kitYari_BooksChildren_Keywords FOREIGN KEY (YariMediaRecordID) REFERENCES kitYari_Books (YariMediaRecordID) ON DELETE CASCADE, ");
			query.Append(" CONSTRAINT FK_kitYari_BooksChildren_Keywords_kitYari_MediaKeywords FOREIGN KEY (YariMediaKeywordID) REFERENCES kitYari_MediaKeywords (YariMediaKeywordID) ON DELETE CASCADE;");
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
				query = new StringBuilder("CREATE TABLE kitYari_Books ");
				query.Append(" (YariMediaRecordID COUNTER(1,1) CONSTRAINT PK_kitYari_Books PRIMARY KEY, " +
					"EndNoteReferenceID LONG," +
					"PublishYear LONG," +
					"Title TEXT(255)," +
					"Pages LONG," +
					"Edition TEXT(75)," +
					"Isbn TEXT(15)," +
					"Label TEXT(5)," +
					"AbstractText MEMO," +
					"ContentsText MEMO," +
					"NotesText MEMO," +
					"AmazonFillDate DATETIME," +
					"AmazonRefreshDate DATETIME," +
					"ImageUrlSmall TEXT(255)," +
					"ImageUrlMedium TEXT(255)," +
					"ImageUrlLarge TEXT(255)," +
					"AmazonListPrice CURRENCY," +
					"AmazonOurPrice CURRENCY," +
					"AmazonAvailability TEXT(75)," +
					"AmazonMedia TEXT(75)," +
					"AmazonReleaseDate DATETIME," +
					"AmazonAsin TEXT(15)," +
					"AbstractEnabled BIT," +
					"ContentsEnabled BIT," +
					"NotesEnabled BIT," +
					"Authors TEXT(255)," +
					"SecondaryAuthors TEXT(255)," +
					"Publisher TEXT(255)," +
					"MediaTypeID LONG);");
			}
			else
			{
				// Microsoft SQL Server
				query = new StringBuilder("CREATE TABLE kitYari_Books ");
				query.Append(" (YariMediaRecordID INT IDENTITY(1,1) CONSTRAINT PK_kitYari_Books PRIMARY KEY, " +
					"EndNoteReferenceID INT," +
					"PublishYear INT," +
					"Title NVARCHAR(255)," +
					"Pages INT," +
					"Edition NVARCHAR(75)," +
					"Isbn NVARCHAR(15)," +
					"Label NVARCHAR(5)," +
					"AbstractText NTEXT," +
					"ContentsText NTEXT," +
					"NotesText NTEXT," +
					"AmazonFillDate DATETIME," +
					"AmazonRefreshDate DATETIME," +
					"ImageUrlSmall NVARCHAR(255)," +
					"ImageUrlMedium NVARCHAR(255)," +
					"ImageUrlLarge NVARCHAR(255)," +
					"AmazonListPrice MONEY," +
					"AmazonOurPrice MONEY," +
					"AmazonAvailability NVARCHAR(75)," +
					"AmazonMedia NVARCHAR(75)," +
					"AmazonReleaseDate DATETIME," +
					"AmazonAsin NVARCHAR(15)," +
					"AbstractEnabled BIT," +
					"ContentsEnabled BIT," +
					"NotesEnabled BIT," +
					"Authors NVARCHAR(255)," +
					"SecondaryAuthors NVARCHAR(255)," +
					"Publisher NVARCHAR(255)," +
					"MediaTypeID INT);");
			}

			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			//
			// Create children table for Keywords.
			//
			query.Length = 0;
			if (database.ConnectionStringWithoutCredentials.StartsWith("provider=microsoft.jet.oledb.4.0"))
			{
				query.Append("CREATE TABLE kitYari_BooksChildren_Keywords ");
				query.Append("(YariMediaRecordID LONG, YariMediaKeywordID LONG);");
				dbCommand = database.GetSqlStringCommand(query.ToString());
				database.ExecuteNonQuery(dbCommand);

			}
			else
			{
				query.Append("CREATE TABLE kitYari_BooksChildren_Keywords ");
				query.Append("(YariMediaRecordID INT, YariMediaKeywordID INT);");
				dbCommand = database.GetSqlStringCommand(query.ToString());
				database.ExecuteNonQuery(dbCommand);

			}
		}

		#endregion

		//--- Begin Custom Code ---
																																																						
		public int GetCurrentCount()
		{
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand("SELECT COUNT(*) FROM kitYari_Books;");
            int count = (int)database.ExecuteScalar(dbCommand);
			return count;
		}

		public YariMediaRecordCollection SearchByTitle(string title)
		{
			return GetCollection(GreyFox.Data.SearchQueryBuilder.StandardSearch(
				new string[] {"Title"}, title.Split(' ', ',', ';')),
				"Title", null);
		}

		public YariMediaRecordCollection SearchByPublishDate(int maxResults, DateTime minDate, DateTime maxDate)
		{
			StringBuilder query = new StringBuilder();
			query.Append("SELECT TOP " + maxResults.ToString() + " ");
			foreach(string columnName in InnerJoinFields)
			{
				query.Append("kitYari_Books.");
				query.Append(columnName);
				query.Append(",");
			}
			query.Length--;

			query.Append(" FROM kitYari_Books " +
				"WHERE (AmazonReleaseDate>=#" + minDate.ToString() + "# " +
				"	AND AmazonReleaseDate<=#" + maxDate.ToString() + "#) "+
				"	OR (PublishYear>=" + minDate.Year.ToString() + " " +
				"	AND PublishYear<=" + maxDate.Year.ToString() + ") " +
				"ORDER BY kitYari_Books.AmazonReleaseDate DESC, kitYari_Books.Title;");

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(query.ToString());
            IDataReader r = database.ExecuteReader(dbCommand);
			YariMediaRecordCollection c = new YariMediaRecordCollection();
			while(r.Read())
				c.Add(YariMediaRecordManager.ParseFromReader(r, 0, 1));
			return c;
		}

		public YariMediaRecordCollection SearchByAuthor(string author)
		{
			return GetCollection(GreyFox.Data.SearchQueryBuilder.StandardSearch(
				new string[] {"Authors", "SecondaryAuthors"}, author.Split(' ', ',', ';')),
				"Title", null);
		}

		public YariMediaRecordCollection SearchByKeywords(string keywords)
		{
			StringBuilder query = new StringBuilder("SELECT DISTINCT ");
			foreach(string columnName in InnerJoinFields)
			{
				query.Append("kitYari_Books.");
				query.Append(columnName);
				query.Append(",");
			}
			query.Length--;

			query.Append(" FROM (kitYari_MediaKeywords " +
				"INNER JOIN kitYari_BooksChildren_Keywords " +
				"	ON kitYari_MediaKeywords.YariMediaKeywordID = kitYari_BooksChildren_Keywords.YariMediaKeywordID) " +
				"INNER JOIN kitYari_Books " +
				"ON kitYari_BooksChildren_Keywords.YariMediaRecordID = kitYari_Books.YariMediaRecordID " +
				"WHERE kitYari_MediaKeywords.Keyword Like '%'+inKeyword+'%' " +
				"ORDER BY kitYari_Books.Title;");

            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(query.ToString());
            IDataReader r = database.ExecuteReader(dbCommand);
			YariMediaRecordCollection c = new YariMediaRecordCollection();
			while(r.Read())
				c.Add(YariMediaRecordManager.ParseFromReader(r, 0, 1));
			return c;
		}

		public YariMediaRecordCollection SearchByContents(string keywords)
		{
			return GetCollection(GreyFox.Data.SearchQueryBuilder.StandardSearch(
				new string[] {"ContentsText"}, keywords.Split(' ', ',', ';')), 
				"Title", null);
		}

		public YariMediaRecordCollection SearchByAbstract(string keywords)
		{
			return GetCollection(GreyFox.Data.SearchQueryBuilder.StandardSearch(
				new string[] {"AbstractText"}, keywords.Split(' ', ',', ';')),
				"Title", null);
		}
																																																																																		
		public bool TitleExists(string title)
		{
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(
                "SELECT COUNT(*) FROM kitYari_Books" +
                " WHERE Title=inTitle;");
            addParameter(database, dbCommand, "Title", DbType.String, title);
			bool exists = ((int)database.ExecuteScalar(dbCommand)) > 0;
			return exists;
		}

		public bool EndNoteReferenceIDExists(int iD)
		{
			Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(
                "SELECT COUNT(*) FROM kitYari_Books" +
				" WHERE EndNoteReferenceID=inEndNoteReferenceID;");
            addParameter(database, dbCommand, "EndNoteReferenceID", DbType.Int32, iD);
			bool exists = ((int)database.ExecuteScalar(dbCommand)) > 0;
			return exists;
		}

		public YariMediaRecord FindByTitle(string title, bool placeHolderOnly)
		{
			StringBuilder query = new StringBuilder();
			query.Append("SELECT ");
			if(placeHolderOnly)
				query.Append(InnerJoinFields[0]);
			else
				query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM kitYari_Books");
			query.Append(" WHERE Title=@Title;");
            
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = database.GetSqlStringCommand(query.ToString());
            addParameter(database, dbCommand, "Title", DbType.String, title);
            IDataReader r = database.ExecuteReader(dbCommand);
			YariMediaRecord mediaRecord;
			if(r.Read())
			{
				if(placeHolderOnly)
					mediaRecord = YariMediaRecord.NewPlaceHolder(r.GetInt32(0));
				else
					mediaRecord = YariMediaRecordManager.ParseFromReader(r, 0, 1);
			}
			else
			{
				mediaRecord = new YariMediaRecord();
				mediaRecord.Title = title;
			}
			return mediaRecord;
		}

		//--- End Custom Code ---
	}
}

