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
using Amns.GreyFox.People;

namespace Amns.GreyFox.Content
{
	#region Child Flags Enumeration

	public enum DbContentClipFlags : int { Status,
				ParentCatalog,
				ParentCatalogParentCatalog,
				ParentCatalogChildCatalogs,
				ParentCatalogDefaultClip,
				ParentCatalogDefaultStatus,
				ParentCatalogDefaultRating,
				ParentCatalogDefaultArchive,
				ParentCatalogTemplates,
				ParentCatalogAuthorRole,
				ParentCatalogEditorRole,
				ParentCatalogReviewerRole,
				Rating,
				RatingRequiredRole,
				WorkingDraft,
				WorkingDraftStatus,
				WorkingDraftParentCatalog,
				WorkingDraftRating,
				WorkingDraftReferences,
				WorkingDraftWorkingDraft,
				WorkingDraftAuthors,
				WorkingDraftEditors};

	#endregion

	/// <summary>
	/// Datamanager for DbContentClip objects.
	/// </summary>
	[ExposedManager("DbContentClip", "", true, 1, 1, 6234)]
	public class DbContentClipManager : IGreyFoxManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "kitCms_Clips";


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
			"DbContentClipID",
			"CreateDate",
			"ModifyDate",
			"Title",
			"Description",
			"Keywords",
			"Icon",
			"StatusID",
			"Body",
			"ParentCatalogID",
			"PublishDate",
			"ExpirationDate",
			"ArchiveDate",
			"Priority",
			"SortOrder",
			"RatingID",
			"CommentsEnabled",
			"NotifyOnComments",
			"WorkingDraftID",
			"OverrideUrl",
			"MenuLabel",
			"MenuTooltip",
			"MenuEnabled",
			"MenuOrder",
			"MenuLeftIcon",
			"MenuLeftIconOver",
			"MenuRightIcon",
			"MenuRightIconOver",
			"MenuBreak"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "DbContentClipID", "LONG", "-1" },
			{ "CreateDate", "DATETIME", "DateTime.Now" },
			{ "ModifyDate", "DATETIME", "DateTime.Now" },
			{ "Title", "TEXT(255)", "string.Empty" },
			{ "Description", "MEMO", "string.Empty" },
			{ "Keywords", "TEXT(255)", "string.Empty" },
			{ "Icon", "TEXT(255)", "string.Empty" },
			{ "StatusID", "LONG", "null" },
			{ "Body", "MEMO", "string.Empty" },
			{ "ParentCatalogID", "LONG", "null" },
			{ "PublishDate", "DATETIME", "" },
			{ "ExpirationDate", "DATETIME", "" },
			{ "ArchiveDate", "DATETIME", "" },
			{ "Priority", "LONG", "0" },
			{ "SortOrder", "LONG", "0" },
			{ "RatingID", "LONG", "null" },
			{ "CommentsEnabled", "BIT", "true" },
			{ "NotifyOnComments", "BIT", "true" },
			{ "WorkingDraftID", "LONG", "null" },
			{ "OverrideUrl", "TEXT(255)", "string.Empty" },
			{ "MenuLabel", "TEXT(75)", "string.Empty" },
			{ "MenuTooltip", "TEXT(255)", "string.Empty" },
			{ "MenuEnabled", "BIT", "true" },
			{ "MenuOrder", "LONG", "0" },
			{ "MenuLeftIcon", "TEXT(255)", "string.Empty" },
			{ "MenuLeftIconOver", "TEXT(255)", "string.Empty" },
			{ "MenuRightIcon", "TEXT(255)", "string.Empty" },
			{ "MenuRightIconOver", "TEXT(255)", "string.Empty" },
			{ "MenuBreak", "BIT", "false" }
		};

		#endregion

		#region Default DbModel Constructors

		static DbContentClipManager()
		{
		}

		public DbContentClipManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!DbContentClipManager.isInitialized)
			{
				DbContentClipManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a DbContentClip into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_DbContentClip">The DbContentClip to insert into the database.</param>
		internal static int _insert(DbContentClip dbContentClip)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			// Set Create Date to Now
			dbContentClip.CreateDate = DateTime.Now.ToUniversalTime();

			// Set Modify Date to Now
			dbContentClip.ModifyDate = DateTime.Now.ToUniversalTime();

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO kitCms_Clips" +
				"(," +
				"CreateDate," +
				"ModifyDate," +
				"Title," +
				"Description," +
				"Keywords," +
				"Icon," +
				"StatusID," +
				"Body," +
				"ParentCatalogID," +
				"PublishDate," +
				"ExpirationDate," +
				"ArchiveDate," +
				"Priority," +
				"SortOrder," +
				"RatingID," +
				"CommentsEnabled," +
				"NotifyOnComments," +
				"WorkingDraftID," +
				"OverrideUrl," +
				"MenuLabel," +
				"MenuTooltip," +
				"MenuEnabled," +
				"MenuOrder," +
				"MenuLeftIcon," +
				"MenuLeftIconOver," +
				"MenuRightIcon," +
				"MenuRightIconOver," +
				"MenuBreak) VALUES (," +
				"@CreateDate," +
				"@ModifyDate," +
				"@Title," +
				"@Description," +
				"@Keywords," +
				"@Icon," +
				"@StatusID," +
				"@Body," +
				"@ParentCatalogID," +
				"@PublishDate," +
				"@ExpirationDate," +
				"@ArchiveDate," +
				"@Priority," +
				"@SortOrder," +
				"@RatingID," +
				"@CommentsEnabled," +
				"@NotifyOnComments," +
				"@WorkingDraftID," +
				"@OverrideUrl," +
				"@MenuLabel," +
				"@MenuTooltip," +
				"@MenuEnabled," +
				"@MenuOrder," +
				"@MenuLeftIcon," +
				"@MenuLeftIconOver," +
				"@MenuRightIcon," +
				"@MenuRightIconOver," +
				"@MenuBreak);";

			dbCommand = database.GetSqlStringCommand(query);
			fillParameters(database, dbCommand, dbContentClip);
			database.ExecuteNonQuery(dbCommand);
			dbCommand = database.GetSqlStringCommand("SELECT @@IDENTITY AS IDVal");
			id = (int)database.ExecuteScalar(dbCommand);

			// Save child relationships for References.
			if(dbContentClip.references != null)
			{
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitCms_ClipsChildren_References " +
					"(DbContentClipID, DbContentClipChildID)" + 
					" VALUES (DbContentClipID, DbContentClipChildID);";
				addParameter(database, dbCommand, "DbContentClipID", DbType.Int32);
				addParameter(database, dbCommand, "DbContentClipChildID", DbType.Int32);
				foreach(DbContentClip item in dbContentClip.references)
				{
					dbCommand.Parameters["DbContentClipID"].Value = id;
					dbCommand.Parameters["DbContentClipChildID"].Value = item.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}

			// Save child relationships for Authors.
			if(dbContentClip.authors != null)
			{
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitCms_ClipsChildren_Authors " +
					"(DbContentClipID, GreyFoxUserID)" + 
					" VALUES (DbContentClipID, GreyFoxUserID);");
				addParameter(database, dbCommand, "DbContentClipID", DbType.Int32);
				addParameter(database, dbCommand, "GreyFoxUserID", DbType.Int32);
				foreach(GreyFoxUser item in dbContentClip.authors)
				{
					dbCommand.Parameters["DbContentClipID"].Value = id;
					dbCommand.Parameters["GreyFoxUserID"].Value = item.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}

			// Save child relationships for Editors.
			if(dbContentClip.editors != null)
			{
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitCms_ClipsChildren_Editors " +
					"(DbContentClipID, GreyFoxUserID)" + 
					" VALUES (DbContentClipID, GreyFoxUserID);");
				addParameter(database, dbCommand, "DbContentClipID", DbType.Int32);
				addParameter(database, dbCommand, "GreyFoxUserID", DbType.Int32);
				foreach(GreyFoxUser item in dbContentClip.editors)
				{
					dbCommand.Parameters["DbContentClipID"].Value = id;
					dbCommand.Parameters["GreyFoxUserID"].Value = item.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}
			// Store dbContentClip in cache.
			if(cacheEnabled) cacheStore(dbContentClip);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(DbContentClip dbContentClip)
		{
			Database database;
			DbCommand dbCommand;

			// Set Modify Date to Now
			dbContentClip.ModifyDate = DateTime.Now.ToUniversalTime();

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE kitCms_Clips SET CreateDate=@CreateDate," +
				"ModifyDate=@ModifyDate," +
				"Title=@Title," +
				"Description=@Description," +
				"Keywords=@Keywords," +
				"Icon=@Icon," +
				"StatusID=@StatusID," +
				"Body=@Body," +
				"ParentCatalogID=@ParentCatalogID," +
				"PublishDate=@PublishDate," +
				"ExpirationDate=@ExpirationDate," +
				"ArchiveDate=@ArchiveDate," +
				"Priority=@Priority," +
				"SortOrder=@SortOrder," +
				"RatingID=@RatingID," +
				"CommentsEnabled=@CommentsEnabled," +
				"NotifyOnComments=@NotifyOnComments," +
				"WorkingDraftID=@WorkingDraftID," +
				"OverrideUrl=@OverrideUrl," +
				"MenuLabel=@MenuLabel," +
				"MenuTooltip=@MenuTooltip," +
				"MenuEnabled=@MenuEnabled," +
				"MenuOrder=@MenuOrder," +
				"MenuLeftIcon=@MenuLeftIcon," +
				"MenuLeftIconOver=@MenuLeftIconOver," +
				"MenuRightIcon=@MenuRightIcon," +
				"MenuRightIconOver=@MenuRightIconOver," +
				"MenuBreak=@MenuBreak WHERE DbContentClipID=@DbContentClipID;");

			fillParameters(database, dbCommand, dbContentClip);
			database.AddInParameter(dbCommand, "DbContentClipID", DbType.Int32, dbContentClip.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			if(dbContentClip.references != null)
			{

				// Delete child relationships for References.
				dbCommand = database.GetSqlStringCommand("DELETE * FROM kitCms_ClipsChildren_References WHERE DbContentClipID=@DbContentClipID;");
				database.AddInParameter(dbCommand, "DbContentClipID", DbType.Int32, dbContentClip.iD);
				database.ExecuteNonQuery(dbCommand);

				// Save child relationships for References.
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitCms_ClipsChildren_References (DbContentClipID, DbContentClipChildID) VALUES (@DbContentClipID, @DbContentClipChildID);");
				database.AddInParameter(dbCommand, "DbContentClipID", DbType.Int32, dbContentClip.iD);
				database.AddInParameter(dbCommand, "DbContentClipID", DbType.Int32);
				foreach(DbContentClip dbContentClip in dbContentClip.references)
				{
					dbCommand.Parameters["@DbContentClipChildID"].Value = dbContentClip.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}

			if(dbContentClip.authors != null)
			{

				// Delete child relationships for Authors.
				dbCommand = database.GetSqlStringCommand("DELETE * FROM kitCms_ClipsChildren_Authors WHERE DbContentClipID=@DbContentClipID;");
				database.AddInParameter(dbCommand, "DbContentClipID", DbType.Int32, dbContentClip.iD);
				database.ExecuteNonQuery(dbCommand);

				// Save child relationships for Authors.
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitCms_ClipsChildren_Authors (DbContentClipID, GreyFoxUserID) VALUES (@DbContentClipID, @GreyFoxUserID);");
				database.AddInParameter(dbCommand, "DbContentClipID", DbType.Int32, dbContentClip.iD);
				database.AddInParameter(dbCommand, "GreyFoxUserID", DbType.Int32);
				foreach(GreyFoxUser greyFoxUser in dbContentClip.authors)
				{
					dbCommand.Parameters["@GreyFoxUserID"].Value = greyFoxUser.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}

			if(dbContentClip.editors != null)
			{

				// Delete child relationships for Editors.
				dbCommand = database.GetSqlStringCommand("DELETE * FROM kitCms_ClipsChildren_Editors WHERE DbContentClipID=@DbContentClipID;");
				database.AddInParameter(dbCommand, "DbContentClipID", DbType.Int32, dbContentClip.iD);
				database.ExecuteNonQuery(dbCommand);

				// Save child relationships for Editors.
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitCms_ClipsChildren_Editors (DbContentClipID, GreyFoxUserID) VALUES (@DbContentClipID, @GreyFoxUserID);");
				database.AddInParameter(dbCommand, "DbContentClipID", DbType.Int32, dbContentClip.iD);
				database.AddInParameter(dbCommand, "GreyFoxUserID", DbType.Int32);
				foreach(GreyFoxUser greyFoxUser in dbContentClip.editors)
				{
					dbCommand.Parameters["@GreyFoxUserID"].Value = greyFoxUser.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}

			// Store dbContentClip in cache.
			if (cacheEnabled) cacheStore(dbContentClip);

			return dbContentClip.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, DbContentClip dbContentClip)
		{
			#region _system

			addParameter(database, dbCommand, "CreateDate", DbType.Date, dbContentClip.createDate);
			addParameter(database, dbCommand, "ModifyDate", DbType.Date, dbContentClip.modifyDate);

			#endregion

			#region General

			addParameter(database, dbCommand, "Title", DbType.String, dbContentClip.title);
			addParameter(database, dbCommand, "Description", DbType.String, dbContentClip.description);
			addParameter(database, dbCommand, "Keywords", DbType.String, dbContentClip.keywords);
			addParameter(database, dbCommand, "Icon", DbType.String, dbContentClip.icon);
			if(dbContentClip.status == null)
			{
				addParameter(database, dbCommand, "StatusID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "StatusID", DbType.Int32, dbContentClip.status.ID);
			}

			#endregion

			#region Body

			addParameter(database, dbCommand, "Body", DbType.String, dbContentClip.body);

			#endregion

			#region Publishing

			if(dbContentClip.parentCatalog == null)
			{
				addParameter(database, dbCommand, "ParentCatalogID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "ParentCatalogID", DbType.Int32, dbContentClip.parentCatalog.ID);
			}
			addParameter(database, dbCommand, "PublishDate", DbType.Date, dbContentClip.publishDate);
			addParameter(database, dbCommand, "ExpirationDate", DbType.Date, dbContentClip.expirationDate);
			addParameter(database, dbCommand, "ArchiveDate", DbType.Date, dbContentClip.archiveDate);
			addParameter(database, dbCommand, "Priority", DbType.Int32, dbContentClip.priority);
			addParameter(database, dbCommand, "SortOrder", DbType.Int32, dbContentClip.sortOrder);
			if(dbContentClip.rating == null)
			{
				addParameter(database, dbCommand, "RatingID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "RatingID", DbType.Int32, dbContentClip.rating.ID);
			}
			addParameter(database, dbCommand, "CommentsEnabled", DbType.Boolean, dbContentClip.commentsEnabled);
			addParameter(database, dbCommand, "NotifyOnComments", DbType.Boolean, dbContentClip.notifyOnComments);
			if(dbContentClip.workingDraft == null)
			{
				addParameter(database, dbCommand, "WorkingDraftID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "WorkingDraftID", DbType.Int32, dbContentClip.workingDraft.ID);
			}
			addParameter(database, dbCommand, "OverrideUrl", DbType.String, dbContentClip.overrideUrl);

			#endregion

			#region Contributors


			#endregion

			#region Menu

			addParameter(database, dbCommand, "MenuLabel", DbType.String, dbContentClip.menuLabel);
			addParameter(database, dbCommand, "MenuTooltip", DbType.String, dbContentClip.menuTooltip);
			addParameter(database, dbCommand, "MenuEnabled", DbType.Boolean, dbContentClip.menuEnabled);
			addParameter(database, dbCommand, "MenuOrder", DbType.Int32, dbContentClip.menuOrder);
			addParameter(database, dbCommand, "MenuLeftIcon", DbType.String, dbContentClip.menuLeftIcon);
			addParameter(database, dbCommand, "MenuLeftIconOver", DbType.String, dbContentClip.menuLeftIconOver);
			addParameter(database, dbCommand, "MenuRightIcon", DbType.String, dbContentClip.menuRightIcon);
			addParameter(database, dbCommand, "MenuRightIconOver", DbType.String, dbContentClip.menuRightIconOver);
			addParameter(database, dbCommand, "MenuBreak", DbType.Boolean, dbContentClip.menuBreak);

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

		internal static bool _fill(DbContentClip dbContentClip)
		{
			// Clone item from cache.
			if(cacheEnabled)
			{
				object cachedObject = cacheFind(dbContentClip.iD);
				if(cachedObject != null)
				{
					((DbContentClip)cachedObject).CopyTo(dbContentClip, true);
					return dbContentClip.isSynced;
				}
			}

			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM kitCms_Clips WHERE DbContentClipID=");
			query.Append(dbContentClip.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find DbContentClipID '{0}'.", 
					dbContentClip.iD)));
			}

			FillFromReader(dbContentClip, r, 0, 1);

			// Store dbContentClip in cache.
			if(cacheEnabled) cacheStore(dbContentClip);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public DbContentClipCollection GetCollection(string whereClause, string sortClause, params DbContentClipFlags[] optionFlags)
		{
			return GetCollection(0, whereClause, sortClause, optionFlags);
		}

		public DbContentClipCollection GetCollection(int topCount, string whereClause, string sortClause, params DbContentClipFlags[] optionFlags)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			DbContentClipCollection dbContentClipCollection;

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
				query.Append("DbContentClip.");
				query.Append(columnName);
				query.Append(",");
			}

			innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;
			int statusOffset = -1;
			int parentCatalogOffset = -1;
			int parentCatalogParentCatalogOffset = -1;
			int parentCatalogDefaultClipOffset = -1;
			int parentCatalogDefaultStatusOffset = -1;
			int parentCatalogDefaultRatingOffset = -1;
			int parentCatalogDefaultArchiveOffset = -1;
			int parentCatalogAuthorRoleOffset = -1;
			int parentCatalogEditorRoleOffset = -1;
			int parentCatalogReviewerRoleOffset = -1;
			int ratingOffset = -1;
			int ratingRequiredRoleOffset = -1;
			int workingDraftOffset = -1;
			int workingDraftStatusOffset = -1;
			int workingDraftParentCatalogOffset = -1;
			int workingDraftRatingOffset = -1;
			int workingDraftWorkingDraftOffset = -1;

			//
			// Append Option Flag Fields
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case DbContentClipFlags.Status:
							for(int i = 0; i <= DbContentStatusManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("Status.");
								query.Append(DbContentStatusManager.InnerJoinFields[i]);
								query.Append(",");
							}
							statusOffset = innerJoinOffset;
							innerJoinOffset = statusOffset + DbContentStatusManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.ParentCatalog:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.ParentCatalogParentCatalog:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_ParentCatalog.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogParentCatalogOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogParentCatalogOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.ParentCatalogDefaultClip:
							for(int i = 0; i <= DbContentClipManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_DefaultClip.");
								query.Append(DbContentClipManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogDefaultClipOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogDefaultClipOffset + DbContentClipManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.ParentCatalogDefaultStatus:
							for(int i = 0; i <= DbContentStatusManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_DefaultStatus.");
								query.Append(DbContentStatusManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogDefaultStatusOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogDefaultStatusOffset + DbContentStatusManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.ParentCatalogDefaultRating:
							for(int i = 0; i <= DbContentRatingManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_DefaultRating.");
								query.Append(DbContentRatingManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogDefaultRatingOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogDefaultRatingOffset + DbContentRatingManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.ParentCatalogDefaultArchive:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_DefaultArchive.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogDefaultArchiveOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogDefaultArchiveOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.ParentCatalogAuthorRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_AuthorRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogAuthorRoleOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogAuthorRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.ParentCatalogEditorRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_EditorRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogEditorRoleOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogEditorRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.ParentCatalogReviewerRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_ReviewerRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogReviewerRoleOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogReviewerRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.Rating:
							for(int i = 0; i <= DbContentRatingManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("Rating.");
								query.Append(DbContentRatingManager.InnerJoinFields[i]);
								query.Append(",");
							}
							ratingOffset = innerJoinOffset;
							innerJoinOffset = ratingOffset + DbContentRatingManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.RatingRequiredRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("Rating_RequiredRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							ratingRequiredRoleOffset = innerJoinOffset;
							innerJoinOffset = ratingRequiredRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.WorkingDraft:
							for(int i = 0; i <= DbContentClipManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("WorkingDraft.");
								query.Append(DbContentClipManager.InnerJoinFields[i]);
								query.Append(",");
							}
							workingDraftOffset = innerJoinOffset;
							innerJoinOffset = workingDraftOffset + DbContentClipManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.WorkingDraftStatus:
							for(int i = 0; i <= DbContentStatusManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("WorkingDraft_Status.");
								query.Append(DbContentStatusManager.InnerJoinFields[i]);
								query.Append(",");
							}
							workingDraftStatusOffset = innerJoinOffset;
							innerJoinOffset = workingDraftStatusOffset + DbContentStatusManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.WorkingDraftParentCatalog:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("WorkingDraft_ParentCatalog.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							workingDraftParentCatalogOffset = innerJoinOffset;
							innerJoinOffset = workingDraftParentCatalogOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.WorkingDraftRating:
							for(int i = 0; i <= DbContentRatingManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("WorkingDraft_Rating.");
								query.Append(DbContentRatingManager.InnerJoinFields[i]);
								query.Append(",");
							}
							workingDraftRatingOffset = innerJoinOffset;
							innerJoinOffset = workingDraftRatingOffset + DbContentRatingManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentClipFlags.WorkingDraftWorkingDraft:
							for(int i = 0; i <= DbContentClipManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("WorkingDraft_WorkingDraft.");
								query.Append(DbContentClipManager.InnerJoinFields[i]);
								query.Append(",");
							}
							workingDraftWorkingDraftOffset = innerJoinOffset;
							innerJoinOffset = workingDraftWorkingDraftOffset + DbContentClipManager.InnerJoinFields.GetUpperBound(0) + 1;
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

				query.Append("kitCms_Clips AS DbContentClip");
			}
			else
			{
				query.Append(" FROM kitCms_Clips AS DbContentClip");
			}
			//
			// Finish INNER JOIN expressions
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case DbContentClipFlags.Status:
							query.Append(" LEFT JOIN kitCms_Statuses AS Status ON DbContentClip.StatusID = Status.DbContentStatusID)");
							break;
						case DbContentClipFlags.ParentCatalog:
							query.Append(" LEFT JOIN kitCms_Catalogs AS ParentCatalog ON DbContentClip.ParentCatalogID = ParentCatalog.DbContentCatalogID)");
							break;
						case DbContentClipFlags.ParentCatalogParentCatalog:
							query.Append(" LEFT JOIN kitCms_Catalogs AS ParentCatalog_ParentCatalog ON ParentCatalog.ParentCatalogID = ParentCatalog_ParentCatalog.DbContentCatalogID)");
							break;
						case DbContentClipFlags.ParentCatalogDefaultClip:
							query.Append(" LEFT JOIN kitCms_Clips AS ParentCatalog_DefaultClip ON ParentCatalog.DefaultClipID = ParentCatalog_DefaultClip.DbContentClipID)");
							break;
						case DbContentClipFlags.ParentCatalogDefaultStatus:
							query.Append(" LEFT JOIN kitCms_Statuses AS ParentCatalog_DefaultStatus ON ParentCatalog.DefaultStatusID = ParentCatalog_DefaultStatus.DbContentStatusID)");
							break;
						case DbContentClipFlags.ParentCatalogDefaultRating:
							query.Append(" LEFT JOIN kitCms_Ratings AS ParentCatalog_DefaultRating ON ParentCatalog.DefaultRatingID = ParentCatalog_DefaultRating.DbContentRatingID)");
							break;
						case DbContentClipFlags.ParentCatalogDefaultArchive:
							query.Append(" LEFT JOIN kitCms_Catalogs AS ParentCatalog_DefaultArchive ON ParentCatalog.DefaultArchiveID = ParentCatalog_DefaultArchive.DbContentCatalogID)");
							break;
						case DbContentClipFlags.ParentCatalogAuthorRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS ParentCatalog_AuthorRole ON ParentCatalog.AuthorRoleID = ParentCatalog_AuthorRole.GreyFoxRoleID)");
							break;
						case DbContentClipFlags.ParentCatalogEditorRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS ParentCatalog_EditorRole ON ParentCatalog.EditorRoleID = ParentCatalog_EditorRole.GreyFoxRoleID)");
							break;
						case DbContentClipFlags.ParentCatalogReviewerRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS ParentCatalog_ReviewerRole ON ParentCatalog.ReviewerRoleID = ParentCatalog_ReviewerRole.GreyFoxRoleID)");
							break;
						case DbContentClipFlags.Rating:
							query.Append(" LEFT JOIN kitCms_Ratings AS Rating ON DbContentClip.RatingID = Rating.DbContentRatingID)");
							break;
						case DbContentClipFlags.RatingRequiredRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS Rating_RequiredRole ON Rating.RequiredRoleID = Rating_RequiredRole.GreyFoxRoleID)");
							break;
						case DbContentClipFlags.WorkingDraft:
							query.Append(" LEFT JOIN kitCms_Clips AS WorkingDraft ON DbContentClip.WorkingDraftID = WorkingDraft.DbContentClipID)");
							break;
						case DbContentClipFlags.WorkingDraftStatus:
							query.Append(" LEFT JOIN kitCms_Statuses AS WorkingDraft_Status ON WorkingDraft.StatusID = WorkingDraft_Status.DbContentStatusID)");
							break;
						case DbContentClipFlags.WorkingDraftParentCatalog:
							query.Append(" LEFT JOIN kitCms_Catalogs AS WorkingDraft_ParentCatalog ON WorkingDraft.ParentCatalogID = WorkingDraft_ParentCatalog.DbContentCatalogID)");
							break;
						case DbContentClipFlags.WorkingDraftRating:
							query.Append(" LEFT JOIN kitCms_Ratings AS WorkingDraft_Rating ON WorkingDraft.RatingID = WorkingDraft_Rating.DbContentRatingID)");
							break;
						case DbContentClipFlags.WorkingDraftWorkingDraft:
							query.Append(" LEFT JOIN kitCms_Clips AS WorkingDraft_WorkingDraft ON WorkingDraft.WorkingDraftID = WorkingDraft_WorkingDraft.DbContentClipID)");
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

			dbContentClipCollection = new DbContentClipCollection();

			while(r.Read())
			{
				DbContentClip dbContentClip = ParseFromReader(r, 0, 1);

				// Fill Status
				if(statusOffset != -1 && !r.IsDBNull(statusOffset))
					DbContentStatusManager.FillFromReader(dbContentClip.status, r, statusOffset, statusOffset+1);

				// Fill ParentCatalog
				if(parentCatalogOffset != -1 && !r.IsDBNull(parentCatalogOffset))
				{
					DbContentCatalogManager.FillFromReader(dbContentClip.parentCatalog, r, parentCatalogOffset, parentCatalogOffset+1);

					// Fill 
					if(parentCatalogParentCatalogOffset != -1 && !r.IsDBNull(parentCatalogParentCatalogOffset))
						DbContentCatalogManager.FillFromReader(dbContentClip.parentCatalog.ParentCatalog, r, parentCatalogParentCatalogOffset, parentCatalogParentCatalogOffset+1);

					// Fill 
					if(parentCatalogDefaultClipOffset != -1 && !r.IsDBNull(parentCatalogDefaultClipOffset))
						DbContentClipManager.FillFromReader(dbContentClip.parentCatalog.DefaultClip, r, parentCatalogDefaultClipOffset, parentCatalogDefaultClipOffset+1);

					// Fill 
					if(parentCatalogDefaultStatusOffset != -1 && !r.IsDBNull(parentCatalogDefaultStatusOffset))
						DbContentStatusManager.FillFromReader(dbContentClip.parentCatalog.DefaultStatus, r, parentCatalogDefaultStatusOffset, parentCatalogDefaultStatusOffset+1);

					// Fill 
					if(parentCatalogDefaultRatingOffset != -1 && !r.IsDBNull(parentCatalogDefaultRatingOffset))
						DbContentRatingManager.FillFromReader(dbContentClip.parentCatalog.DefaultRating, r, parentCatalogDefaultRatingOffset, parentCatalogDefaultRatingOffset+1);

					// Fill Default Archive
					if(parentCatalogDefaultArchiveOffset != -1 && !r.IsDBNull(parentCatalogDefaultArchiveOffset))
						DbContentCatalogManager.FillFromReader(dbContentClip.parentCatalog.DefaultArchive, r, parentCatalogDefaultArchiveOffset, parentCatalogDefaultArchiveOffset+1);

					// Fill Author Roles
					if(parentCatalogAuthorRoleOffset != -1 && !r.IsDBNull(parentCatalogAuthorRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentClip.parentCatalog.AuthorRole, r, parentCatalogAuthorRoleOffset, parentCatalogAuthorRoleOffset+1);

					// Fill Editor Roles
					if(parentCatalogEditorRoleOffset != -1 && !r.IsDBNull(parentCatalogEditorRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentClip.parentCatalog.EditorRole, r, parentCatalogEditorRoleOffset, parentCatalogEditorRoleOffset+1);

					// Fill Reviewer Role
					if(parentCatalogReviewerRoleOffset != -1 && !r.IsDBNull(parentCatalogReviewerRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentClip.parentCatalog.ReviewerRole, r, parentCatalogReviewerRoleOffset, parentCatalogReviewerRoleOffset+1);

				}

				// Fill Rating
				if(ratingOffset != -1 && !r.IsDBNull(ratingOffset))
				{
					DbContentRatingManager.FillFromReader(dbContentClip.rating, r, ratingOffset, ratingOffset+1);

					// Fill 
					if(ratingRequiredRoleOffset != -1 && !r.IsDBNull(ratingRequiredRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentClip.rating.RequiredRole, r, ratingRequiredRoleOffset, ratingRequiredRoleOffset+1);

				}

				// Fill WorkingDraft
				if(workingDraftOffset != -1 && !r.IsDBNull(workingDraftOffset))
				{
					DbContentClipManager.FillFromReader(dbContentClip.workingDraft, r, workingDraftOffset, workingDraftOffset+1);

					// Fill 
					if(workingDraftStatusOffset != -1 && !r.IsDBNull(workingDraftStatusOffset))
						DbContentStatusManager.FillFromReader(dbContentClip.workingDraft.Status, r, workingDraftStatusOffset, workingDraftStatusOffset+1);

					// Fill Parent Catalog
					if(workingDraftParentCatalogOffset != -1 && !r.IsDBNull(workingDraftParentCatalogOffset))
						DbContentCatalogManager.FillFromReader(dbContentClip.workingDraft.ParentCatalog, r, workingDraftParentCatalogOffset, workingDraftParentCatalogOffset+1);

					// Fill 
					if(workingDraftRatingOffset != -1 && !r.IsDBNull(workingDraftRatingOffset))
						DbContentRatingManager.FillFromReader(dbContentClip.workingDraft.Rating, r, workingDraftRatingOffset, workingDraftRatingOffset+1);

					// Fill 
					if(workingDraftWorkingDraftOffset != -1 && !r.IsDBNull(workingDraftWorkingDraftOffset))
						DbContentClipManager.FillFromReader(dbContentClip.workingDraft.WorkingDraft, r, workingDraftWorkingDraftOffset, workingDraftWorkingDraftOffset+1);

				}

				dbContentClipCollection.Add(dbContentClip);
			}

			return dbContentClipCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static DbContentClip ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			DbContentClip dbContentClip = new DbContentClip();
			FillFromReader(dbContentClip, r, idOffset, dataOffset);
			return dbContentClip;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(DbContentClip dbContentClip, IDataReader r, int idOffset, int dataOffset)
		{
			dbContentClip.iD = r.GetInt32(idOffset);
			dbContentClip.isSynced = true;
			dbContentClip.isPlaceHolder = false;

			dbContentClip.createDate = r.GetDateTime(0+dataOffset);
			dbContentClip.modifyDate = r.GetDateTime(1+dataOffset);
			dbContentClip.title = r.GetString(2+dataOffset);
			dbContentClip.description = r.GetString(3+dataOffset);
			dbContentClip.keywords = r.GetString(4+dataOffset);
			dbContentClip.icon = r.GetString(5+dataOffset);
			if(!r.IsDBNull(6+dataOffset) && r.GetInt32(6+dataOffset) > 0)
			{
				dbContentClip.status = DbContentStatus.NewPlaceHolder(r.GetInt32(6+dataOffset));
			}
			dbContentClip.body = r.GetString(7+dataOffset);
			if(!r.IsDBNull(8+dataOffset) && r.GetInt32(8+dataOffset) > 0)
			{
				dbContentClip.parentCatalog = DbContentCatalog.NewPlaceHolder(r.GetInt32(8+dataOffset));
			}
			dbContentClip.publishDate = r.GetDateTime(9+dataOffset);
			dbContentClip.expirationDate = r.GetDateTime(10+dataOffset);
			dbContentClip.archiveDate = r.GetDateTime(11+dataOffset);
			dbContentClip.priority = r.GetInt32(12+dataOffset);
			dbContentClip.sortOrder = r.GetInt32(13+dataOffset);
			if(!r.IsDBNull(14+dataOffset) && r.GetInt32(14+dataOffset) > 0)
			{
				dbContentClip.rating = DbContentRating.NewPlaceHolder(r.GetInt32(14+dataOffset));
			}
			dbContentClip.commentsEnabled = r.GetBoolean(15+dataOffset);
			dbContentClip.notifyOnComments = r.GetBoolean(16+dataOffset);
			if(!r.IsDBNull(17+dataOffset) && r.GetInt32(17+dataOffset) > 0)
			{
				dbContentClip.workingDraft = DbContentClip.NewPlaceHolder(r.GetInt32(17+dataOffset));
			}
			if(!r.IsDBNull(18+dataOffset)) 
				dbContentClip.overrideUrl = r.GetString(18+dataOffset);
			else
				dbContentClip.overrideUrl = null;
			dbContentClip.menuLabel = r.GetString(19+dataOffset);
			dbContentClip.menuTooltip = r.GetString(20+dataOffset);
			dbContentClip.menuEnabled = r.GetBoolean(21+dataOffset);
			dbContentClip.menuOrder = r.GetInt32(22+dataOffset);
			dbContentClip.menuLeftIcon = r.GetString(23+dataOffset);
			dbContentClip.menuLeftIconOver = r.GetString(24+dataOffset);
			dbContentClip.menuRightIcon = r.GetString(25+dataOffset);
			dbContentClip.menuRightIconOver = r.GetString(26+dataOffset);
			dbContentClip.menuBreak = r.GetBoolean(27+dataOffset);
		}

		#endregion

		#region Default DbModel Fill Methods

		public static void FillReferences(DbContentClip dbContentClip)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			s = new StringBuilder("SELECT DbContentClipChildID FROM kitCms_ClipsChildren_References ");
			s.Append("WHERE DbContentClipID=");
			s.Append(dbContentClip.iD);
			s.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(s.ToString());
			r = database.ExecuteReader(dbCommand);

			DbContentClipCollection references;
			if(dbContentClip.references != null)
			{
				references = dbContentClip.references;
				references.Clear();
			}
			else
			{
				references = new DbContentClipCollection();
				dbContentClip.references = references;
			}

			while(r.Read())
				references.Add(DbContentClip.NewPlaceHolder(r.GetInt32(0)));

			dbContentClip.References = references;
			// Store DbContentClip in cache.
			if(cacheEnabled) cacheStore(dbContentClip);
		}

		public static void FillReferences(DbContentClipCollection dbContentClipCollection)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			if(dbContentClipCollection.Count > 0)
			{
				s = new StringBuilder("SELECT DbContentClipID, DbContentClipChildID FROM kitCms_ClipsChildren_References ORDER BY DbContentClipID; ");

				// Clone and sort collection by ID first to fill children in one pass
				DbContentClipCollection clonedCollection = dbContentClipCollection.Clone();
				clonedCollection.Sort();

				database = DatabaseFactory.CreateDatabase();
				dbCommand = database.GetSqlStringCommand(s.ToString());
				r = database.ExecuteReader(dbCommand);

				bool more = r.Read();

				foreach(DbContentClip dbContentClip in clonedCollection)
				{
					DbContentClipCollection references;
					if(dbContentClip.references != null)
					{
						references = dbContentClip.references;
						references.Clear();
					}
					else
					{
						references = new DbContentClipCollection();
						dbContentClip.references = references;
					}

					while(more)
					{
						if(r.GetInt32(0) < dbContentClip.iD)
						{
							more = r.Read();
						}
						else if(r.GetInt32(0) == dbContentClip.iD)
						{
							references.Add(DbContentClip.NewPlaceHolder(r.GetInt32(1)));
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

		public static void FillAuthors(DbContentClip dbContentClip)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			s = new StringBuilder("SELECT GreyFoxUserID FROM kitCms_ClipsChildren_Authors ");
			s.Append("WHERE DbContentClipID=");
			s.Append(dbContentClip.iD);
			s.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(s.ToString());
			r = database.ExecuteReader(dbCommand);

			GreyFoxUserCollection authors;
			if(dbContentClip.authors != null)
			{
				authors = dbContentClip.authors;
				authors.Clear();
			}
			else
			{
				authors = new GreyFoxUserCollection();
				dbContentClip.authors = authors;
			}

			while(r.Read())
				authors.Add(GreyFoxUser.NewPlaceHolder(r.GetInt32(0)));

			dbContentClip.Authors = authors;
			// Store DbContentClip in cache.
			if(cacheEnabled) cacheStore(dbContentClip);
		}

		public static void FillAuthors(DbContentClipCollection dbContentClipCollection)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			if(dbContentClipCollection.Count > 0)
			{
				s = new StringBuilder("SELECT DbContentClipID, GreyFoxUserID FROM kitCms_ClipsChildren_Authors ORDER BY DbContentClipID; ");

				// Clone and sort collection by ID first to fill children in one pass
				DbContentClipCollection clonedCollection = dbContentClipCollection.Clone();
				clonedCollection.Sort();

				database = DatabaseFactory.CreateDatabase();
				dbCommand = database.GetSqlStringCommand(s.ToString());
				r = database.ExecuteReader(dbCommand);

				bool more = r.Read();

				foreach(DbContentClip dbContentClip in clonedCollection)
				{
					GreyFoxUserCollection authors;
					if(dbContentClip.authors != null)
					{
						authors = dbContentClip.authors;
						authors.Clear();
					}
					else
					{
						authors = new GreyFoxUserCollection();
						dbContentClip.authors = authors;
					}

					while(more)
					{
						if(r.GetInt32(0) < dbContentClip.iD)
						{
							more = r.Read();
						}
						else if(r.GetInt32(0) == dbContentClip.iD)
						{
							authors.Add(GreyFoxUser.NewPlaceHolder(r.GetInt32(1)));
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

		public static void FillEditors(DbContentClip dbContentClip)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			s = new StringBuilder("SELECT GreyFoxUserID FROM kitCms_ClipsChildren_Editors ");
			s.Append("WHERE DbContentClipID=");
			s.Append(dbContentClip.iD);
			s.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(s.ToString());
			r = database.ExecuteReader(dbCommand);

			GreyFoxUserCollection editors;
			if(dbContentClip.editors != null)
			{
				editors = dbContentClip.editors;
				editors.Clear();
			}
			else
			{
				editors = new GreyFoxUserCollection();
				dbContentClip.editors = editors;
			}

			while(r.Read())
				editors.Add(GreyFoxUser.NewPlaceHolder(r.GetInt32(0)));

			dbContentClip.Editors = editors;
			// Store DbContentClip in cache.
			if(cacheEnabled) cacheStore(dbContentClip);
		}

		public static void FillEditors(DbContentClipCollection dbContentClipCollection)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			if(dbContentClipCollection.Count > 0)
			{
				s = new StringBuilder("SELECT DbContentClipID, GreyFoxUserID FROM kitCms_ClipsChildren_Editors ORDER BY DbContentClipID; ");

				// Clone and sort collection by ID first to fill children in one pass
				DbContentClipCollection clonedCollection = dbContentClipCollection.Clone();
				clonedCollection.Sort();

				database = DatabaseFactory.CreateDatabase();
				dbCommand = database.GetSqlStringCommand(s.ToString());
				r = database.ExecuteReader(dbCommand);

				bool more = r.Read();

				foreach(DbContentClip dbContentClip in clonedCollection)
				{
					GreyFoxUserCollection editors;
					if(dbContentClip.editors != null)
					{
						editors = dbContentClip.editors;
						editors.Clear();
					}
					else
					{
						editors = new GreyFoxUserCollection();
						dbContentClip.editors = editors;
					}

					while(more)
					{
						if(r.GetInt32(0) < dbContentClip.iD)
						{
							more = r.Read();
						}
						else if(r.GetInt32(0) == dbContentClip.iD)
						{
							editors.Add(GreyFoxUser.NewPlaceHolder(r.GetInt32(1)));
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

			query = new StringBuilder("DELETE * FROM kitCms_Clips WHERE DbContentClipID=");
			query.Append(id);
			query.Append(';');

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);


			// Delete child relationships for References.
			query.Length = 0;
			query.Append("DELETE * FROM kitCms_ClipsChildren_References WHERE ");
			query.Append("DbContentClipID=");
			query.Append(id);
			query.Append(";");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			// Delete child relationships for Authors.
			query.Length = 0;
			query.Append("DELETE * FROM kitCms_ClipsChildren_Authors WHERE ");
			query.Append("DbContentClipID=");
			query.Append(id);
			query.Append(";");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			// Delete child relationships for Editors.
			query.Length = 0;
			query.Append("DELETE * FROM kitCms_ClipsChildren_Editors WHERE ");
			query.Append("DbContentClipID=");
			query.Append(id);
			query.Append(";");
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

			query = new StringBuilder("ALTER TABLE kitCms_Clips ADD ");
			query.Append(" CONSTRAINT kitCms_Clips_Status_FK FOREIGN KEY (StatusID) REFERENCES kitCms_Statuses(DbContentStatusID),");
			query.Append(" CONSTRAINT kitCms_Clips_ParentCatalog_FK FOREIGN KEY (ParentCatalogID) REFERENCES kitCms_Catalogs(DbContentCatalogID),");
			query.Append(" CONSTRAINT kitCms_Clips_Rating_FK FOREIGN KEY (RatingID) REFERENCES kitCms_Ratings(DbContentRatingID),");
			query.Append(" CONSTRAINT kitCms_Clips_WorkingDraft_FK FOREIGN KEY (WorkingDraftID) REFERENCES kitCms_Clips(DbContentClipID),");
			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			query.Length = 0;
			query.Append("ALTER TABLE kitCms_ClipsChildren_References ");
			query.Append(" ADD CONSTRAINT DbContentClipChildren_References_DbContentClip_FK FOREIGN KEY (DbContentClipID) REFERENCES kitCms_Clips(DbContentClipID), CONSTRAINT DbContentClipChildren_References_DbContentClip_FK FOREIGN KEY (DbContentClipID) REFERENCES kitCms_Clips(DbContentClipID);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			query.Length = 0;
			query.Append("ALTER TABLE kitCms_ClipsChildren_Authors ");
			query.Append(" ADD CONSTRAINT DbContentClipChildren_Authors_DbContentClip_FK FOREIGN KEY (DbContentClipID) REFERENCES kitCms_Clips(DbContentClipID), CONSTRAINT DbContentClipChildren_Authors_GreyFoxUser_FK FOREIGN KEY (GreyFoxUserID) REFERENCES sysGlobal_Users(GreyFoxUserID);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			query.Length = 0;
			query.Append("ALTER TABLE kitCms_ClipsChildren_Editors ");
			query.Append(" ADD CONSTRAINT DbContentClipChildren_Editors_DbContentClip_FK FOREIGN KEY (DbContentClipID) REFERENCES kitCms_Clips(DbContentClipID), CONSTRAINT DbContentClipChildren_Editors_GreyFoxUser_FK FOREIGN KEY (GreyFoxUserID) REFERENCES sysGlobal_Users(GreyFoxUserID);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		public void CreateTable()
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("CREATE TABLE kitCms_Clips ");
			query.Append(" (DbContentClipID COUNTER(1,1) CONSTRAINT DbContentClipID PRIMARY KEY, " +
				"CreateDate DATETIME," +
				"ModifyDate DATETIME," +
				"Title TEXT(255)," +
				"Description MEMO," +
				"Keywords TEXT(255)," +
				"Icon TEXT(255)," +
				"StatusID LONG," +
				"Body MEMO," +
				"ParentCatalogID LONG," +
				"PublishDate DATETIME," +
				"ExpirationDate DATETIME," +
				"ArchiveDate DATETIME," +
				"Priority LONG," +
				"SortOrder LONG," +
				"RatingID LONG," +
				"CommentsEnabled BIT," +
				"NotifyOnComments BIT," +
				"WorkingDraftID LONG," +
				"OverrideUrl TEXT(255)," +
				"MenuLabel TEXT(75)," +
				"MenuTooltip TEXT(255)," +
				"MenuEnabled BIT," +
				"MenuOrder LONG," +
				"MenuLeftIcon TEXT(255)," +
				"MenuLeftIconOver TEXT(255)," +
				"MenuRightIcon TEXT(255)," +
				"MenuRightIconOver TEXT(255)," +
				"MenuBreak BIT);");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			//
			// Create children table for References.
			//
			query.Length = 0;
			query.Append("CREATE TABLE kitCms_ClipsChildren_References ");
			query.Append("(DbContentClipID LONG, DbContentClipChildID LONG);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			//
			// Create children table for Authors.
			//
			query.Length = 0;
			query.Append("CREATE TABLE kitCms_ClipsChildren_Authors ");
			query.Append("(DbContentClipID LONG, GreyFoxUserID LONG);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			//
			// Create children table for Editors.
			//
			query.Length = 0;
			query.Append("CREATE TABLE kitCms_ClipsChildren_Editors ");
			query.Append("(DbContentClipID LONG, GreyFoxUserID LONG);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

		#region CacheManager Methods

		private static void cacheStore(DbContentClip dbContentClip)
		{
			CacheManager cache = CacheFactory.GetCacheManager("DbContentClip");
			cache.Add(dbContentClip.iD.ToString(), dbContentClip);
		}

		private static DbContentClip cacheFind(int id)
		{
			object cachedObject;
			CacheManager cache = CacheFactory.GetCacheManager("DbContentClip");
			cachedObject = cache.GetData(id.ToString());
			if(cachedObject == null)
				return null;
			return (DbContentClip)cachedObject;
		}

		private static void cacheRemove(int id)
		{
			CacheManager cache = CacheFactory.GetCacheManager("DbContentClip");
			cache.Remove(id.ToString());
		}

		#endregion

	}
}

