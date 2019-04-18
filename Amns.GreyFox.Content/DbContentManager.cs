using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Text;
using Amns.GreyFox.DataAccessLayer;
using Amns.GreyFox.Security;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Summary description for DbContentManager.
	/// </summary>
	public class DbContentManager : Amns.GreyFox.TableKit
	{
		private readonly string clipFields = "ID,Title,Subtitle,Description,AuthorID,OwnerID, ReviewerRoleID," +
			"EditorRoleID,PublisherRoleID,CreateDate,ModifyDate,PressDate,ExpireDate, " +
			"ArchiveDate,ContentStatus,ArchiveCatalogID,ScanDate,ContentString,ContentType, " +
			"HitCounterType,TrackChangesType";

		private readonly string catalogFields = "Name,Description,PressTime,PressDate," +
			"ExpireTime,ExpireDate,ArchiveTime,ArchiveDate,ArchiveOnExpiration,ArchiveCatalogID," +
			"AuthorRoleID,ReviewerRoleID,EditorRoleID,PublisherRoleID,AdminRoleID," +
			"AuthorDisabled,ReviewDisabled,EditDisabled,PublishDisabled," +
			"DynamicParseTypeDisabled,TrackChangesDisabled,HitCounterDisabled,ContentType," +
			"TrackChangesType,HitCounterType," +
			"enableHtmlAuthoring,enableCss,enableParagraphStyles," +
			"enableFontForeColor,enableFontBackColor,enableFormatting,enableImages," +
			"enableCustomCss,cssLink,cssStyles,cssNames,paragraphStyles,paragraphNames," +
			"fontColorValues,fontColorNames,imageLocation";

		private string dbDataStoreTableName					= string.Empty;
		private string dbXRefTableName						= string.Empty;
		private string dbKeywordsTableName					= string.Empty;
		private string dbContentKeywordsTableName			= string.Empty;
		private string dbLogTableName						= string.Empty;
		private int iD										= 0;
		private string name									= "Default";
		private string description							= "Default Content Catalog";
		private bool initialized							= false;
		private bool sync									= false;

		private TimeSpan pressTime							= TimeSpan.Zero;			// No print lag
		private DateTime pressDate							= DateTime.MinValue;		// No print date
		private TimeSpan expireTime							= TimeSpan.Zero;			// No Expiration
		private DateTime expireDate							= DateTime.MinValue;		// No Expiration
		private TimeSpan archiveTime						= TimeSpan.Zero;
		private DateTime archiveDate						= DateTime.MinValue;
		private int archiveCatalogID						= 0;
		private bool archiveOnExpiration					= false;

		private int authorRoleID							= 0;					
		private int reviewerRoleID							= 0;	// Reviewers not required
		private int editorRoleID							= 0;	// Editors not required.
		private int publisherRoleID							= 0;	// Publishers not required.
		private int adminRoleID								= 0;	// Administrator role for settings

		// Default state should make the catalog disable the review and editing process,
		// enable the editing process and make it so that authors are assigned publishing
		// rights straight to the site.

		private bool authorDisabled							= false;			
		private bool reviewDisabled							= true;			// Reconfigure existing articles?
		private bool editDisabled							= true;			// Reconfigure existing articles?
		private bool publishDisabled						= false;		// Trigger a warning that documents cannot be published

		private bool dynamicParseTypeDisabled				= true;
        private bool trackChangesDisabled					= true;
		private bool hitCounterDisabled						= false;
		private ContentType contentType						= ContentType.Static;
		private ContentTrackChangesType trackChangesType	= ContentTrackChangesType.Disabled;
		private ContentHitCounterType hitCounterType		= ContentHitCounterType.Normal;

		// Configuration options for the content enditor

		private bool enableHtmlAuthoring					= true;
		private bool enableCss								= true;
		private bool enableParagraphStyles					= true;
		private bool enableFontForeColor					= true;
 		private bool enableFontBackColor					= true;
		private bool enableFormatting						= true;			// Bold, Italic, Underline etc.
		private bool enableImages							= true;
		private bool enableCustomCss						= false;

		private string cssLink								= string.Empty;
		private string cssStyles							= "*";
		private string cssNames								= "*";
		private string paragraphStyles						= "*";
		private string paragraphNames						= "*";
		private string fontColorValues						= "*";
		private string fontColorNames						= "*";
		private string imageLocation						= "/images";

		private string sqlStatement							= string.Empty;

		#region Public Properties

		public int ID
		{
			get
			{
				if(!initialized)
					throw(new ContentManagerException("Catalog has not ben saved or initialized, the Catalog ID is undefined.", 104));

				return iD;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return sync;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				sync = false;
				name = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				sync = false;
				description = value;
			}
		}

		public TimeSpan PressTime
		{
			get
			{
				return pressTime;
			}
			set
			{
				sync = false;
				pressTime = value;
			}
		}

		public DateTime PressDate
		{
			get
			{
				return pressDate;
			}
			set
			{
				sync = false;
				pressDate = value;
			}
		}

		public TimeSpan ExpireTime
		{
			get
			{
				return expireTime;
			}
			set
			{
				sync = false;
				expireTime = value;
			}
		}

		public DateTime ExpireDate
		{
			get
			{
				return expireDate;
			}
			set
			{
				sync = false;
				expireDate = value;
			}
		}

		public TimeSpan ArchiveTime
		{
			get
			{
				return archiveTime;
			}
			set
			{
				sync = false;
				archiveTime = value;
			}
		}

		public DateTime ArchiveDate
		{
			get
			{
				return archiveDate;
			}
			set
			{
				sync = false;
				archiveDate = value;
			}
		}

		public int ArchiveCatalogID
		{
			get
			{
				return archiveCatalogID;
			}
			set
			{
				sync = false;
				archiveCatalogID = value;
			}
		}

		public int AuthorRoleID
		{
			get
			{
				return authorRoleID;
			}
			set
			{
				sync = false;
				authorRoleID = value;
			}
		}

		public int ReviewerRoleID
		{
			get
			{
				return reviewerRoleID;
			}
			set
			{
				sync = false;
				reviewerRoleID = value;
			}
		}

		public int EditorRoleID
		{
			get
			{
				return editorRoleID;
			}
			set
			{
				sync = false;
				editorRoleID = value;
			}
		}

		public int PublisherRoleID
		{
			get
			{
				return publisherRoleID;
			}
			set
			{
				sync = false;
				publisherRoleID = value;
			}
		}

		public int AdminRoleID
		{
			get
			{
				return adminRoleID;
			}
			set
			{
				sync = false;
				adminRoleID = value;
			}
		}

		public bool AuthorDisabled
		{
			get
			{
				return authorDisabled;
			}
			set
			{
				sync = false;
				authorDisabled = value;
			}
		}
        
		public bool ReviewDisabled
		{
			get
			{
				return reviewDisabled;
			}
			set
			{
				sync = false;
				reviewDisabled = value;
			}
		}

		public bool EditDisabled
		{
			get
			{
				return editDisabled;
			}
			set
			{
				sync = false;
				editDisabled = value;
			}
		}

		public bool PublishDisabled
		{
			get
			{
				return publishDisabled;
			}
			set
			{
				sync = false;
				publishDisabled = value;
			}
		}

		public bool DynamicParseTypeDisabled
		{
			get
			{
				return dynamicParseTypeDisabled;
			}
			set
			{
				sync = false;
				dynamicParseTypeDisabled = value;
			}
		}

		public bool TrackChangesDisabled
		{
			get
			{
				return trackChangesDisabled;
			}
			set
			{
				sync = false;
				trackChangesDisabled = value;
			}
		}

		public bool HitCounterDisabled
		{
			get
			{
				return hitCounterDisabled;
			}
			set
			{
				sync = false;
				hitCounterDisabled = value;
			}
		}

		public ContentHitCounterType HitCounterType
		{
			get
			{
				return hitCounterType;
			}
			set
			{
				sync = false;
				hitCounterType = value;
			}
		}
        
		public ContentType ContentType
		{
			get
			{
				return contentType;
			}
			set
			{
				sync = false;
				contentType = value;
			}
		}

		public ContentTrackChangesType TrackChangesType
		{
			get
			{
				return trackChangesType;
			}
			set
			{
				sync = false;
				trackChangesType = value;
			}
		}

		public bool EnableHtmlAuthoring
		{
			get
			{
				return enableHtmlAuthoring;
			}
			set
			{
				sync = false;
				enableHtmlAuthoring = value;
			}
		}

		public bool EnableCss
		{
			get
			{
				return enableCss;
			}
			set
			{
				sync = false;
				enableCss = value;
			}
		}

		public bool EnableParagraphStyles
		{
			get
			{
				return enableParagraphStyles;
			}
			set
			{
				sync = false;
				enableParagraphStyles = value;
			}
		}

		public bool EnableFontForeColor
		{
			get
			{
				return enableFontForeColor;
			}
			set
			{
				sync = false;
				enableFontForeColor = value;
			}
		}

		public bool EnableFontBackColor
		{
			get
			{
				return enableFontBackColor;
			}
			set
			{
				sync = false;
				enableFontBackColor = value;
			}
		}

		public bool EnableFormatting
		{
			get
			{
				return enableFormatting;
			}
			set
			{
				sync = false;
				enableFormatting = value;
			}
		}

		public bool EnableImages
		{
			get
			{
				return enableImages;
			}
			set
			{
				sync = false;
				enableImages = value;
			}
		}

		public bool EnableCustomCss
		{
			get
			{
				return enableCustomCss;
			}
			set
			{
				sync = false;
				enableCustomCss = value;
			}
		}

		public string CssLink
		{
			get
			{
				return cssLink;
			}
			set
			{
				sync = false;
				cssLink = value;
			}
		}

		public string CssStyles
		{
			get
			{
				return cssStyles;
			}
			set
			{
				sync = false;
				cssStyles = value;
			}
		}
		public string CssNames
		{
			get
			{
				return cssNames;
			}
			set
			{
				sync = false;
				cssNames = value;
			}
		}

		public string ParagraphStyles
		{
			get
			{
				return paragraphStyles;
			}
			set
			{
				sync = false;
				paragraphStyles = value;
			}
		}

		public string ParagraphNames
		{
			get
			{
				return paragraphNames;
			}
			set
			{
				sync = false;
				paragraphNames = value;
			}
		}

		public string FontColorValues
		{
			get
			{
				return fontColorValues;
			}
			set
			{
				sync = false;
				fontColorValues = value;
			}
		}

		public string FontColorNames
		{
			get
			{
				return fontColorNames;
			}
			set
			{
				sync = false;
				fontColorNames = value;
			}
		}

		public string ImageLocation
		{
			get
			{
				return imageLocation;
			}
			set
			{
				sync = false;
				imageLocation = value;
			}
		}
                
		#endregion

		private DbContentLog log;

		public DbContentLog Log
		{
			get
			{
				// Only create the content log instance when it is called.
				if(log == null)
					log = new DbContentLog(dbConnectionString, dbLogTableName);
				
				return log;
			}
		}

		#region Create Instance Methods

		private void parseTables(string catalog)
		{
			name = catalog;
			dbDataStoreTableName = string.Format("kitCms_Catalog_{0}_DataStore", catalog);
			dbXRefTableName = string.Format("kitCms_Catalog_{0}_XRef", catalog);
			dbKeywordsTableName = string.Format("kitCms_Catalog_{0}_Keywords", catalog);
			dbContentKeywordsTableName = string.Format("kitCms_Catalog_{0}_ContentKeywords", catalog);
			dbLogTableName = string.Format("kitCms_Catalog_{0}_Log", catalog);
		}

		public DbContentManager(string connectionString)
		{
			dbConnectionString = connectionString;
			parseTables("Default");
		}

		public DbContentManager(string connectionString, int catalogID)
		{
			dbConnectionString = connectionString;
			iD = catalogID;
		}

		#endregion

		#region Filter Methods

	public void ApplySqlFilter(DbContentClip clip)
{
	clip.Title = SqlFilter.FilterString(clip.Title, 255);
	clip.Subtitle = SqlFilter.FilterString(clip.Subtitle, 255);
	clip.ContentString = SqlFilter.FilterString(clip.ContentString);
	for(int x = 0; x <= clip.keywords.GetUpperBound(0); x++)
	clip.keywords[x] = SqlFilter.FilterString(clip.keywords[x]);
}

		#endregion

        /// <summary>
        /// Initializes the database with the necissary tables to support the content
        /// manager as well as creating a default content catalog and associated roles
        /// in the Amns.GreyFox user's database.
        /// </summary>
        /// <returns></returns>
		public void Install()
		{
			OleDbConnection dbConnection = new OleDbConnection(dbConnectionString);
			OleDbCommand dbCommand = new OleDbCommand();
			OleDbTransaction dbTransaction;
			dbCommand.Connection = dbConnection;

			// Try to open database
			try
			{
				dbConnection.Open();
                dbTransaction = dbConnection.BeginTransaction();
			}
			catch(OleDbException e)
			{
				throw(new ContentManagerException(e.Message, e.ErrorCode));
			}
            
			// Try to create table to hold catalogs
			try
			{
				//
				dbCommand.CommandText = createCatalogTable;
				dbCommand.Transaction = dbTransaction;
				dbCommand.ExecuteNonQuery();

				GreyFoxRole authorRole = new GreyFoxRole(dbConnectionString);
				authorRole.Name = "CMS/Author";
				authorRole.Description = "Default Author";
				authorRole.IsDisabled = false;
				authorRole.Save();

				GreyFoxRole reviewerRole = new GreyFoxRole(dbConnectionString);
				reviewerRole.Name = "CMS/Reviewer";
				reviewerRole.Description = "Default Reviewer";
				reviewerRole.IsDisabled = false;
				reviewerRole.Save();

				GreyFoxRole editorRole = new GreyFoxRole(dbConnectionString);
				editorRole.Name = "CMS/Editor";
				editorRole.Description = "Default CMS Editor";
				editorRole.IsDisabled = false;
				editorRole.Save();

				GreyFoxRole publisherRole = new GreyFoxRole(dbConnectionString);
				publisherRole.Name = "CMS/Publisher";
				publisherRole.Description = "Default CMS Publisher";
				publisherRole.IsDisabled = false;

				GreyFoxRole adminRole = new GreyFoxRole(dbConnectionString);
				adminRole.Name = "CMS/Admin";
				adminRole.Description = "Default CMS Administrator";
				adminRole.IsDisabled = false;
				adminRole.Save();

				iD = createCatalog(dbCommand);				
				
				dbTransaction.Commit();
				sync = true;
			}
			catch(OleDbException e)
			{
				iD = 0;
				dbTransaction.Rollback();
				throw(new ContentManagerException(e.Message + " SQL: " + sqlStatement, e.ErrorCode));
			}
			finally
			{
				dbConnection.Close();
			}

			Log.InsertGenericEvent(DateTime.Now.ToUniversalTime(), ContentLogEntryType.CatalogCreateSuccess, iD);            			
		}


		/// <summary>
		/// Loads the catalog's settings and sets up the class to take requests.
		/// </summary>
		/// <returns></returns>
		public void Load()
		{
			if(sync)
				throw(new ContentManagerException("Cannot create a synchronized catalog.", 991));

			StringBuilder s = new StringBuilder("SELECT ");
			s.Append(catalogFields);
			s.Append(" FROM kitCms_Catalogs WHERE CatalogID=");
			s.Append(iD);
			s.Append(";");

			OleDbDataReader r = this.runReader(s.ToString());

			if(r.Read())
			{
				name = r.GetString(0);
				description = r.GetString(1);
				pressTime = TimeSpan.FromTicks((long) r.GetDouble(2));
				pressDate = r.GetDateTime(3);
				expireTime = TimeSpan.FromTicks((long) r.GetDouble(4));
				expireDate = r.GetDateTime(5);
				archiveTime = TimeSpan.FromTicks((long) r.GetDouble(6));
				archiveDate = r.GetDateTime(7);
				archiveOnExpiration = r.GetBoolean(8);
				archiveCatalogID = r.GetInt32(9);
				authorRoleID = r.GetInt32(10);
				reviewerRoleID = r.GetInt32(11);
				editorRoleID = r.GetInt32(12);
				publisherRoleID = r.GetInt32(13);
				adminRoleID = r.GetInt32(14);
				authorDisabled = r.GetBoolean(15);
				reviewDisabled = r.GetBoolean(16);
				editDisabled = r.GetBoolean(17);
				publishDisabled = r.GetBoolean(18);
				dynamicParseTypeDisabled = r.GetBoolean(19);
				trackChangesDisabled = r.GetBoolean(20);
				hitCounterDisabled = r.GetBoolean(21);
				contentType = (ContentType) r.GetByte(22);
				trackChangesType = (ContentTrackChangesType) r.GetByte(23);
				hitCounterType = (ContentHitCounterType) r.GetByte(24);
				enableHtmlAuthoring = r.GetBoolean(25);
				enableCss = r.GetBoolean(26);
				enableParagraphStyles = r.GetBoolean(27);
				enableFontForeColor = r.GetBoolean(28);
				enableFontBackColor = r.GetBoolean(29);
				enableFormatting = r.GetBoolean(30);
				enableImages = r.GetBoolean(31);
				enableCustomCss = r.GetBoolean(32);
				cssLink = r.GetString(33);
				cssStyles = r.GetString(34);
				cssNames = r.GetString(35);
				paragraphStyles = r.GetString(36);
				paragraphNames = r.GetString(37);
				fontColorValues = r.GetString(38);
				fontColorNames = r.GetString(39);
				imageLocation = r.GetString(40);
				sync = true;
			}
		}

		public int CreateCatalog()
		{
			if(sync)
				throw(new ContentManagerException("Cannot create a synchronized catalog.", 991));

			OleDbConnection dbConnection = new OleDbConnection(dbConnectionString);
			OleDbCommand dbCommand = new OleDbCommand();
			dbCommand.Connection = dbConnection;
			
			if(dbConnection.State != ConnectionState.Open)
				dbConnection.Open();

			OleDbTransaction dbTransaction = dbConnection.BeginTransaction();
			dbCommand.Transaction = dbTransaction;

			try
			{
				iD = createCatalog(dbCommand);
				dbTransaction.Commit();
				sync = true;
			}
			catch(OleDbException e)
			{
				iD = 0;
				dbTransaction.Rollback();
				throw(e);				
			}
			finally
			{				
				dbConnection.Close();
			}

			

			return iD;
		}

		private int createCatalog(OleDbCommand dbCommand)
		{				
			dbCommand.CommandText = string.Format(_createCatalogDataStore, name);
			dbCommand.ExecuteNonQuery();
			dbCommand.CommandText = string.Format(_createCatalogXRef, name);
			dbCommand.ExecuteNonQuery();
			dbCommand.CommandText = string.Format(_createCatalogKeywords, name);
			dbCommand.ExecuteNonQuery();
			dbCommand.CommandText = string.Format(_createCatalogContentKeywords, name);
			dbCommand.ExecuteNonQuery();
			dbCommand.CommandText = string.Format(_createCatalogLog, name);
			dbCommand.ExecuteNonQuery();
				
			StringBuilder s = new StringBuilder();
			s.Append("INSERT INTO kitCms_Catalogs (");
			s.Append(catalogFields);
			s.Append(") VALUES (");
			s.AppendFormat("'{0}',", name);
			s.AppendFormat("'{0}',", description);
			s.AppendFormat("{0},", pressTime.Ticks);
			s.AppendFormat("'{0}',", pressDate.ToString());
			s.AppendFormat("{0},", expireTime.Ticks);
			s.AppendFormat("'{0}',", expireDate.ToString());
			s.AppendFormat("{0},", archiveTime.Ticks);
			s.AppendFormat("'{0}',", archiveDate.ToString());
			s.AppendFormat("{0},", archiveOnExpiration);
			s.AppendFormat("{0},", archiveCatalogID);
			s.AppendFormat("{0},", authorRoleID);
			s.AppendFormat("{0},", reviewerRoleID);
			s.AppendFormat("{0},", editorRoleID);
			s.AppendFormat("{0},", publisherRoleID);
			s.AppendFormat("{0},", adminRoleID);
			s.AppendFormat("{0},", authorDisabled);
			s.AppendFormat("{0},", reviewDisabled);
			s.AppendFormat("{0},", editDisabled);
			s.AppendFormat("{0},", publishDisabled);
			s.AppendFormat("{0},", dynamicParseTypeDisabled);
			s.AppendFormat("{0},", trackChangesDisabled);
			s.AppendFormat("{0},", hitCounterDisabled);
			s.AppendFormat("{0},", (byte) contentType);
			s.AppendFormat("{0},", (byte) trackChangesType);
			s.AppendFormat("{0},", (byte) hitCounterType);
			s.AppendFormat("{0},", enableHtmlAuthoring);
			s.AppendFormat("{0},", enableCss);
			s.AppendFormat("{0},", enableParagraphStyles);
			s.AppendFormat("{0},", enableFontForeColor);
			s.AppendFormat("{0},", enableFontBackColor);
			s.AppendFormat("{0},", enableFormatting);
			s.AppendFormat("{0},", enableImages);
			s.AppendFormat("{0},", enableCustomCss);
			s.AppendFormat("'{0}',", cssLink);
			s.AppendFormat("'{0}',", cssStyles);
			s.AppendFormat("'{0}',", cssNames);
			s.AppendFormat("'{0}',", paragraphStyles);
			s.AppendFormat("'{0}',", paragraphNames);
			s.AppendFormat("'{0}',", fontColorValues);
			s.AppendFormat("'{0}',", fontColorNames);
			s.AppendFormat("'{0}');", imageLocation);

			sqlStatement = s.ToString();
			dbCommand.CommandText = s.ToString();
			dbCommand.ExecuteNonQuery();
				
			dbCommand.CommandText = "SELECT @@IDENTITY AS IDVal";
			dbCommand.CommandType = CommandType.Text;
			return (int) dbCommand.ExecuteScalar();
		}

		private readonly string createCatalogTable = "CREATE TABLE kitCms_Catalogs (" +
			"CatalogID COUNTER(10000,1) CONSTRAINT CatalogIDKey PRIMARY KEY, " +
			"Name TEXT(255), " +
			"Description MEMO, " +
			"PressTime DOUBLE, " +
			"PressDate DATETIME, " +
			"ExpireTime DOUBLE, " +
			"ExpireDate DATETIME, " +
			"ArchiveTime DOUBLE, " +
			"ArchiveDate DATETIME, " +
			"ArchiveOnExpiration BIT, " +
			"ArchiveCatalogID LONG, " +			
			"AuthorRoleID LONG, " +
			"ReviewerRoleID LONG, " +
			"EditorRoleID LONG, " +
			"PublisherRoleID LONG, " +
			"AdminRoleID LONG, " +
			"AuthorDisabled BIT, " +
			"ReviewDisabled BIT, " +
			"EditDisabled BIT, " +
			"PublishDisabled BIT, " +
			"DynamicParseTypeDisabled BIT, " +
			"TrackChangesDisabled BIT, " +
			"HitCounterDisabled BIT, " +
			"ContentType BYTE, " +
			"TrackChangesType BYTE, " +
			"HitCounterType BYTE, " +
			"enableHtmlAuthoring BIT, " +
			"enableCss BIT, " +
			"enableParagraphStyles BIT, " +
			"enableFontForeColor BIT, " +
			"enableFontBackColor BIT, " +
			"enableFormatting BIT, " +
			"enableImages BIT, " +
			"enableCustomCss BIT, " +
			"cssLink TEXT(255), " +
			"cssStyles MEMO, " +
			"cssNames MEMO, " +
			"paragraphStyles MEMO, " +
			"paragraphNames MEMO, " +
			"fontColorValues MEMO, " +
			"fontColorNames MEMO, " +
			"imageLocation MEMO);";

		private readonly string _createCatalogDataStore = "CREATE TABLE kitCms_Catalog_{0}_DataStore" + 
			"(ClipID COUNTER(1000,5) CONSTRAINT ClipIDKey PRIMARY KEY, " +
			"Title TEXT(255), " +
			"Subtitle TEXT(255), " +
			"AuthorID LONG, " +				// The contactID of the author
			"OwnerID LONG, " +				// The userID of the owner (Used to reclaim orphaned content clips with this field)
			"ReviewerRoleID LONG, " +		// The role of authorized reviewers, use catalog variables if null
			"EditorRoleID LONG, " +			// The role of authorized editors, use catalog variables if null
			"PublisherRoleID LONG, " +		// The role of authorized publishers, use catalog variables if null
			"CreateDate DATETIME, " +		// Date when created
			"ModifyDate DATETIME, " +		// Anytime changes are made to the record, this is updated
			"PressDate DATETIME, " +		// Throws an exception to view panels if publish date has not passed. (null if do not publish) **** CHECKED WHEN HITTING CLIPS
			"ExpireDate DATETIME, " +		// DateTime.Min=NoExpiration, Date when record is expired and will throw a ContentExpired Exception **** CHECKED WHEN HITTING CLIPS
			"ArchiveDate DATETIME, " +		// DateTime.Min=NoArchive
			"ContentStatus BYTE, " +		// 0=Unpublished, 1=Published/Awaiting Print Date, 2=Live, 3=Expired
			"ArchiveCatalogID LONG, " +		// 0=DoNotArchive, Catalog to archive clip to when it expires (this will create a new clipID! and copy data associated with the clip to the new catalog)
			"ScanDate DATETIME," +			// DateTime.Min=NotScanned, Date when record has been last compiled and scanned for keywords (should match modify date)
            "ContentString MEMO, " +
			"ContentType BYTE, " +			// 0=Undefined, 1=Static, 2=Dynamic
			"TrackChangesType BYTE, " +		// 0=Disabled, 1=ReplaceContentMode (Scavenge the database, expire old clip and replace references to new clip), 2=ExpireContentMode (Copy current clip's expiration pointer/date, Expire current clip and replace the expiration pointer with the new ClipID)
			"HitCounterType BYTE);";		// 0=Disabled, 1=Record Hits, 2=Record Hits and Inheritance (Warn on performance hit, yet useful for checking inheritance performance)

		private readonly string _createCatalogXRef = "CREATE TABLE kitCms_Catalog_{0}_XRef" +
			"(XRefID COUNTER(1,1) CONSTRAINT XRefIDKey PRIMARY KEY, " +
			"OrigClipID LONG, " +			// Originating Clip for the XRef
			"TargetID LONG, " +				// TargetID for the XRef (Can point to different tables)
			"XRefType BYTE);";				// XRef Type (Default, ExpirationPointer)

		private readonly string _createCatalogKeywords = "CREATE TABLE kitCms_Catalog_{0}_Keywords" +
			"(KeywordID COUNTER(1,1) CONSTRAINT KeywordIDKey PRIMARY KEY, " +
			"KeywordText TEXT(255), " +		// Hmm...
			"RootClipID LONG);";			// Associates a clipID as the root of the keyword (Useful for dictionary content)

		private readonly string _createCatalogContentKeywords = "CREATE TABLE kitCms_Catalog_{0}_ContentKeywords" +
			"(ClipID LONG, " +
			"KeywordID LONG);";

		private readonly string _createCatalogLog = "CREATE TABLE kitCms_Catalog_{0}_Log" +
			"(EntryDate DATETIME, " +
			"EntryType BYTE, " +
			"OrigClipID LONG, " +				
			"ParentClipID LONG, " +			// null if not an inherited clip
			"ClientUserID LONG, " +			// null if anonymous or userid of client
			"Message MEMO);";

//		private static readonly string _createInterCatalogXRef = "CREATE TABLE kitCms_InterCatalogXRef" +
//			"(XRefID COUNTER(1,1) CONSTRAINT XRefIDKey PRIMARY KEY, " +
//			"OrigCatalogID LONG, " +
//			"OrigClipID LONG, " +
//			"TargetCatalogID LONG, " +
//			"TargetClipID LONG)";

		public DbContentClip PrepareNewClip(int authorID, int ownerID, string title)
		{
			if(!sync)
				throw(new ContentManagerException("Catalog is not synchronized, cannot prepare a new clip.", 114));

			DbContentClip clip = new DbContentClip();
			clip.iD = 0;
			clip.isSynced = false;

			clip.title = title;
			clip.subtitle = string.Empty;
			clip.description = string.Empty;
			clip.authorID = authorID;
			clip.ownerID = ownerID;
			clip.reviewerRoleID = reviewerRoleID;
			clip.editorRoleID = editorRoleID;			
			clip.publisherRoleID = publisherRoleID;
			clip.createDate = DateTime.Now.ToUniversalTime();
			clip.modifyDate = DateTime.Now.ToUniversalTime();
			
			if(pressDate != DateTime.MinValue)
                clip.pressDate = pressDate;
			else
				clip.pressDate = DateTime.Now.ToUniversalTime().Add(pressTime);
			
			if(expireDate != DateTime.MinValue)
				clip.expireDate = expireDate;
			else
				clip.expireDate = DateTime.Now.ToUniversalTime().Add(expireTime);

			if(archiveDate != DateTime.MinValue)
				clip.archiveDate = archiveDate;
			else
				clip.archiveDate = DateTime.Now.ToUniversalTime().Add(archiveTime);

			clip.contentStatus = ContentStatusCode.Authoring;
			
			clip.archiveCatalogID = archiveCatalogID;
			clip.archiveOnExpiration = archiveOnExpiration;
			clip.scanDate = DateTime.MinValue;
			clip.contentString = string.Empty;
			clip.contentType = contentType;
			clip.trackChangesType = trackChangesType;
			clip.hitCounterType = hitCounterType;

			return clip;
		}

		#region Insert, Select, Update, Delete Methods
		
		public int Insert(DbContentClip clip, bool ApplyFilter)
		{
			if(clip.isSynced)
				throw(new ContentManagerException("Clip is already synchronized.", 923));

			if(ApplyFilter)
				ApplySqlFilter(clip);

			StringBuilder s = new StringBuilder();
			s.AppendFormat("INSERT INTO {0} ", dbTableName);
			s.Append("(Title, Subtitle, Description, AuthorID, OwnerID, ReviewerRoleID, " +
				"EditorRoleID, PublisherRoleID, CreateDate, ModifyDate, PressDate, ExpireDate, " +
				"ArchiveDate, ContentStatus, ArchiveCatalogID, ScanDate, ContentString, ContentType, " +
				"HitCounterType, TrackChangesType) VALUES ");
			s.AppendFormat("('{0}',", clip.title);
			s.AppendFormat("'{0}',", clip.subtitle);
			s.AppendFormat("'{0}',", clip.description);
			s.AppendFormat("{0},", clip.authorID);
			s.AppendFormat("{0},", clip.ownerID);
			s.AppendFormat("{0},", clip.reviewerRoleID);
			s.AppendFormat("{0},", clip.editorRoleID);
			s.AppendFormat("{0},", clip.publisherRoleID);			
			
			s.AppendFormat("'{0}',", clip.createDate.ToString());
			s.AppendFormat("'{0}',", clip.modifyDate.ToString());
			s.AppendFormat("'{0}',", clip.pressDate.ToString()); 
			s.AppendFormat("'{0}',", clip.expireDate.ToString());
			s.AppendFormat("'{0}',", clip.archiveDate.ToString());

			s.AppendFormat("{0},", (byte) clip.contentStatus);
			s.AppendFormat("{0},", clip.archiveCatalogID);
			s.AppendFormat("'{0}',", clip.scanDate.ToString());		// ScanDate
			s.AppendFormat("'{0}',", clip.contentString);
			s.AppendFormat("{0},", (byte) clip.contentType);
			s.AppendFormat("{0},", (byte) clip.hitCounterType);
			s.AppendFormat("{0});", (byte) clip.trackChangesType);
			
			return runOleDbIdentityCommand(s.ToString());
		}

		public int Update(DbContentClip clip, bool ApplyFilter)
		{
			if(ApplyFilter)
				ApplySqlFilter(clip);

			StringBuilder s = new StringBuilder();
			s.AppendFormat("UPDATE {0} SET ", dbTableName);
			s.AppendFormat("Title='{0}',", clip.title);
			s.AppendFormat("Subtitle='{0}',", clip.subtitle);
			s.AppendFormat("Description='{0}',", clip.description);
			s.AppendFormat("AuthorID={0},", clip.authorID);
			s.AppendFormat("OwnerID={0},", clip.ownerID);
			s.AppendFormat("ReviewerRoleID={0},", clip.reviewerRoleID);
			s.AppendFormat("EditorRoleID={0},", clip.editorRoleID);
			s.AppendFormat("PublisherRoleID={0},", clip.publisherRoleID);			
			
			s.AppendFormat("CreateDate='{0}',", clip.createDate.ToString());
			s.AppendFormat("ModifyDate='{0}',", clip.modifyDate.ToString());
			s.AppendFormat("PressDate='{0}',", clip.pressDate.ToString()); 
			s.AppendFormat("ExpireDate='{0}',", clip.expireDate.ToString());
			s.AppendFormat("ArchiveDate='{0}',", clip.archiveDate.ToString());

			s.AppendFormat("ContentStatus={0},", (byte) clip.contentStatus);
			s.AppendFormat("ArchiveCatalogID={0},", clip.archiveCatalogID);
			s.AppendFormat("ScanDate='{0}',", clip.scanDate.ToString());		// ScanDate
			s.AppendFormat("ContentString='{0}',", clip.contentString);
			s.AppendFormat("ContentType={0},", (byte) clip.contentType);
			s.AppendFormat("HitCounterType={0},", (byte) clip.hitCounterType);
			s.AppendFormat("TrackChangesType={0} ", (byte) clip.trackChangesType);
			s.AppendFormat("WHERE ClipID='{0}';", clip.ID);

			return runOleDbCommand(s.ToString());
		}

		/// <summary>
		/// Parses a clip from the current row that the DataReader's cursor is on.
		/// </summary>
		/// <param name="r">The DataReader object to parse.</param>
		/// <returns>A DbClip parsed from the current DataReader.</returns>
		private DbContentClip parse(OleDbDataReader r)
		{
			DbContentClip clip = new DbContentClip();
			clip.iD = r.GetInt32(0);
			clip.title = r.GetString(1);
			clip.subtitle = r.GetString(2);
			clip.description = r.GetString(3);
			clip.authorID = r.GetInt32(4);
			clip.ownerID = r.GetInt32(5);
			clip.reviewerRoleID = r.GetInt32(6);
			clip.editorRoleID = r.GetInt32(7);
			clip.publisherRoleID = r.GetInt32(8);
            
			clip.createDate = r.GetDateTime(9);
			clip.modifyDate = r.GetDateTime(10);
			clip.pressDate = r.GetDateTime(11);
			clip.expireDate = r.GetDateTime(12);
			clip.archiveDate = r.GetDateTime(13);
			clip.contentStatus = (ContentStatusCode) r.GetByte(14);
			clip.archiveCatalogID = r.GetInt32(15);
			clip.scanDate = r.GetDateTime(16);

			clip.contentString = r.GetString(17);
			clip.contentType = (ContentType) r.GetByte(18);
			clip.hitCounterType = (ContentHitCounterType) r.GetByte(19);
			clip.trackChangesType = (ContentTrackChangesType) r.GetByte(20);
			return clip;
		}

		/// <summary>
		/// Gets a specific clip from the clip catalog.
		/// </summary>
		/// <param name="ClipID">ID of the Clip to select.</param>
		/// <returns>DbClip Clip</returns>
		public DbContentClip Get(int ClipID)
		{
			StringBuilder s = new StringBuilder("SELECT ");
			s.Append(clipFields);
			s.Append(" FROM ");
			s.AppendFormat(dbTableName);
			s.AppendFormat(" WHERE ClipID=");
			s.Append(ClipID);
            s.Append(";");

			OleDbDataReader r = runReader(s.ToString());
			
			if(!r.Read())
			{
				r.Close();
				throw(new Exception(string.Format("Specified DbClip '{0}' does not exist.", ClipID)));
			}

			DbContentClip clip = parse(r);
			r.Close();

			return clip;
		}

		/// <summary>
		/// Gets all clips in the clip catalog.
		/// </summary>
		/// <returns>DbClip[] ClipArray</returns>
		public DbContentClip[] Get()
		{
			OleDbDataReader r = this.runReader("SELECT " + clipFields + " FROM " + dbTableName + ";");
			ArrayList clips = new ArrayList();
			while(r.Read())
				clips.Add(this.parse(r));
			r.Close();
			return (DbContentClip[]) clips.ToArray();
		}

		#endregion

		#region Hit Counter Methods

		public int SetHitCounter(int ClipID, int Hits)
		{
			StringBuilder s = new StringBuilder("UPDATE ");
			s.Append(dbTableName);
			s.Append(" SET Hits=");
			s.Append(Hits); 
			s.Append(" WHERE ID=");
			s.Append(ClipID);
			s.Append(";");

			return runOleDbCommand(s.ToString());
		}

		#endregion

		#region Clip Locking Methods

		public int LockClip(int ClipID)
		{
			StringBuilder s = new StringBuilder("UPDATE ");
			s.Append(dbTableName);
			s.Append(" SET Locked=true;");

			return runOleDbCommand(s.ToString());
		}

		public int UnlockClip(int ClipID)
		{
			StringBuilder s = new StringBuilder("UPDATE ");
			s.Append(dbTableName);
			s.Append(" SET Locked=false;");

			return runOleDbCommand(s.ToString());
		}

		#endregion
	}
}