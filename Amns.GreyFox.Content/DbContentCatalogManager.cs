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

	public enum DbContentCatalogFlags : int { ParentCatalog,
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
				DefaultClip,
				DefaultClipStatus,
				DefaultClipParentCatalog,
				DefaultClipRating,
				DefaultClipReferences,
				DefaultClipWorkingDraft,
				DefaultClipAuthors,
				DefaultClipEditors,
				DefaultStatus,
				DefaultRating,
				DefaultRatingRequiredRole,
				DefaultArchive,
				DefaultArchiveParentCatalog,
				DefaultArchiveChildCatalogs,
				DefaultArchiveDefaultClip,
				DefaultArchiveDefaultStatus,
				DefaultArchiveDefaultRating,
				DefaultArchiveDefaultArchive,
				DefaultArchiveTemplates,
				DefaultArchiveAuthorRole,
				DefaultArchiveEditorRole,
				DefaultArchiveReviewerRole,
				AuthorRole,
				EditorRole,
				ReviewerRole};

	#endregion

	/// <summary>
	/// Datamanager for DbContentCatalog objects.
	/// </summary>
	[ExposedManager("DbContentCatalog", "", true, 1, 1, 6234)]
	public class DbContentCatalogManager : IGreyFoxManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;

		// Private Fields
		string tableName = "kitCms_Catalogs";


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
			"DbContentCatalogID",
			"Title",
			"Description",
			"Keywords",
			"Status",
			"WorkflowMode",
			"CommentsEnabled",
			"NotifyOnComments",
			"Enabled",
			"SortOrder",
			"ParentCatalogID",
			"DefaultClipID",
			"Icon",
			"CreateDate",
			"ModifyDate",
			"DefaultTimeToPublish",
			"DefaultTimeToExpire",
			"DefaultTimeToArchive",
			"DefaultKeywords",
			"DefaultStatusID",
			"DefaultRatingID",
			"DefaultArchiveID",
			"DefaultMenuLeftIcon",
			"DefaultMenuRightIcon",
			"MenuLabel",
			"MenuTooltip",
			"MenuEnabled",
			"MenuOrder",
			"MenuLeftIcon",
			"MenuRightIcon",
			"MenuBreakImage",
			"MenuBreakCssClass",
			"MenuCssClass",
			"MenuCatalogCssClass",
			"MenuCatalogSelectedCssClass",
			"MenuCatalogChildSelectedCssClass",
			"MenuClipCssClass",
			"MenuClipSelectedCssClass",
			"MenuClipChildSelectedCssClass",
			"MenuClipChildExpandedCssClass",
			"MenuOverrideFlags",
			"MenuIconFlags",
			"AuthorRoleID",
			"EditorRoleID",
			"ReviewerRoleID"
		};

		#endregion

		#region Join Field Array

		public static readonly string[,] JoinFields = new string[,] {
			{ "DbContentCatalogID", "LONG", "-1" },
			{ "Title", "TEXT(255)", "string.Empty" },
			{ "Description", "MEMO", "string.Empty" },
			{ "Keywords", "TEXT(255)", "string.Empty" },
			{ "Status", "BYTE", "0" },
			{ "WorkflowMode", "BYTE", "0" },
			{ "CommentsEnabled", "BIT", "true" },
			{ "NotifyOnComments", "BIT", "true" },
			{ "Enabled", "BIT", "true" },
			{ "SortOrder", "LONG", "0" },
			{ "ParentCatalogID", "LONG", "null" },
			{ "DefaultClipID", "LONG", "null" },
			{ "Icon", "TEXT(255)", "string.Empty" },
			{ "CreateDate", "DATETIME", "DateTime.Now" },
			{ "ModifyDate", "DATETIME", "DateTime.Now" },
			{ "DefaultTimeToPublish", "DOUBLE", "TimeSpan.Zero" },
			{ "DefaultTimeToExpire", "DOUBLE", "TimeSpan.MaxValue" },
			{ "DefaultTimeToArchive", "DOUBLE", "TimeSpan.MaxValue" },
			{ "DefaultKeywords", "TEXT(75)", "string.Empty" },
			{ "DefaultStatusID", "LONG", "null" },
			{ "DefaultRatingID", "LONG", "null" },
			{ "DefaultArchiveID", "LONG", "null" },
			{ "DefaultMenuLeftIcon", "TEXT(255)", "string.Empty" },
			{ "DefaultMenuRightIcon", "TEXT(255)", "string.Empty" },
			{ "MenuLabel", "TEXT(75)", "string.Empty" },
			{ "MenuTooltip", "TEXT(255)", "string.Empty" },
			{ "MenuEnabled", "BIT", "true" },
			{ "MenuOrder", "LONG", "0" },
			{ "MenuLeftIcon", "TEXT(255)", "string.Empty" },
			{ "MenuRightIcon", "TEXT(255)", "string.Empty" },
			{ "MenuBreakImage", "TEXT(255)", "string.Empty" },
			{ "MenuBreakCssClass", "TEXT(255)", "string.Empty" },
			{ "MenuCssClass", "TEXT(75)", "string.Empty" },
			{ "MenuCatalogCssClass", "TEXT(75)", "string.Empty" },
			{ "MenuCatalogSelectedCssClass", "TEXT(75)", "string.Empty" },
			{ "MenuCatalogChildSelectedCssClass", "TEXT(75)", "string.Empty" },
			{ "MenuClipCssClass", "TEXT(75)", "string.Empty" },
			{ "MenuClipSelectedCssClass", "TEXT(75)", "string.Empty" },
			{ "MenuClipChildSelectedCssClass", "TEXT(75)", "string.Empty" },
			{ "MenuClipChildExpandedCssClass", "TEXT(75)", "string.Empty" },
			{ "MenuOverrideFlags", "BYTE", "0" },
			{ "MenuIconFlags", "BYTE", "0" },
			{ "AuthorRoleID", "LONG", "null" },
			{ "EditorRoleID", "LONG", "null" },
			{ "ReviewerRoleID", "LONG", "null" }
		};

		#endregion

		#region Default DbModel Constructors

		static DbContentCatalogManager()
		{
		}

		public DbContentCatalogManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!DbContentCatalogManager.isInitialized)
			{
				DbContentCatalogManager.isInitialized = true;
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a DbContentCatalog into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_DbContentCatalog">The DbContentCatalog to insert into the database.</param>
		internal static int _insert(DbContentCatalog dbContentCatalog)
		{
			int id;
			string query;
			Database database;
			DbCommand dbCommand;

			// Set Create Date to Now
			dbContentCatalog.CreateDate = DateTime.Now.ToUniversalTime();

			// Set Modify Date to Now
			dbContentCatalog.ModifyDate = DateTime.Now.ToUniversalTime();

			database = DatabaseFactory.CreateDatabase();

			query = "INSERT INTO kitCms_Catalogs" +
				"(," +
				"Title," +
				"Description," +
				"Keywords," +
				"Status," +
				"WorkflowMode," +
				"CommentsEnabled," +
				"NotifyOnComments," +
				"Enabled," +
				"SortOrder," +
				"ParentCatalogID," +
				"DefaultClipID," +
				"Icon," +
				"CreateDate," +
				"ModifyDate," +
				"DefaultTimeToPublish," +
				"DefaultTimeToExpire," +
				"DefaultTimeToArchive," +
				"DefaultKeywords," +
				"DefaultStatusID," +
				"DefaultRatingID," +
				"DefaultArchiveID," +
				"DefaultMenuLeftIcon," +
				"DefaultMenuRightIcon," +
				"MenuLabel," +
				"MenuTooltip," +
				"MenuEnabled," +
				"MenuOrder," +
				"MenuLeftIcon," +
				"MenuRightIcon," +
				"MenuBreakImage," +
				"MenuBreakCssClass," +
				"MenuCssClass," +
				"MenuCatalogCssClass," +
				"MenuCatalogSelectedCssClass," +
				"MenuCatalogChildSelectedCssClass," +
				"MenuClipCssClass," +
				"MenuClipSelectedCssClass," +
				"MenuClipChildSelectedCssClass," +
				"MenuClipChildExpandedCssClass," +
				"MenuOverrideFlags," +
				"MenuIconFlags," +
				"AuthorRoleID," +
				"EditorRoleID," +
				"ReviewerRoleID) VALUES (," +
				"@Title," +
				"@Description," +
				"@Keywords," +
				"@Status," +
				"@WorkflowMode," +
				"@CommentsEnabled," +
				"@NotifyOnComments," +
				"@Enabled," +
				"@SortOrder," +
				"@ParentCatalogID," +
				"@DefaultClipID," +
				"@Icon," +
				"@CreateDate," +
				"@ModifyDate," +
				"@DefaultTimeToPublish," +
				"@DefaultTimeToExpire," +
				"@DefaultTimeToArchive," +
				"@DefaultKeywords," +
				"@DefaultStatusID," +
				"@DefaultRatingID," +
				"@DefaultArchiveID," +
				"@DefaultMenuLeftIcon," +
				"@DefaultMenuRightIcon," +
				"@MenuLabel," +
				"@MenuTooltip," +
				"@MenuEnabled," +
				"@MenuOrder," +
				"@MenuLeftIcon," +
				"@MenuRightIcon," +
				"@MenuBreakImage," +
				"@MenuBreakCssClass," +
				"@MenuCssClass," +
				"@MenuCatalogCssClass," +
				"@MenuCatalogSelectedCssClass," +
				"@MenuCatalogChildSelectedCssClass," +
				"@MenuClipCssClass," +
				"@MenuClipSelectedCssClass," +
				"@MenuClipChildSelectedCssClass," +
				"@MenuClipChildExpandedCssClass," +
				"@MenuOverrideFlags," +
				"@MenuIconFlags," +
				"@AuthorRoleID," +
				"@EditorRoleID," +
				"@ReviewerRoleID);";

			dbCommand = database.GetSqlStringCommand(query);
			fillParameters(database, dbCommand, dbContentCatalog);
			database.ExecuteNonQuery(dbCommand);
			dbCommand = database.GetSqlStringCommand("SELECT @@IDENTITY AS IDVal");
			id = (int)database.ExecuteScalar(dbCommand);

			// Save child relationships for ChildCatalogs.
			if(dbContentCatalog.childCatalogs != null)
			{
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitCms_CatalogsChildren_ChildCatalogs " +
					"(DbContentCatalogID, DbContentCatalogChildID)" + 
					" VALUES (DbContentCatalogID, DbContentCatalogChildID);";
				addParameter(database, dbCommand, "DbContentCatalogID", DbType.Int32);
				addParameter(database, dbCommand, "DbContentCatalogChildID", DbType.Int32);
				foreach(DbContentCatalog item in dbContentCatalog.childCatalogs)
				{
					dbCommand.Parameters["DbContentCatalogID"].Value = id;
					dbCommand.Parameters["DbContentCatalogChildID"].Value = item.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}

			// Save child relationships for Templates.
			if(dbContentCatalog.templates != null)
			{
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitCms_CatalogsChildren_Templates " +
					"(DbContentCatalogID, DbContentClipID)" + 
					" VALUES (DbContentCatalogID, DbContentClipID);");
				addParameter(database, dbCommand, "DbContentCatalogID", DbType.Int32);
				addParameter(database, dbCommand, "DbContentClipID", DbType.Int32);
				foreach(DbContentClip item in dbContentCatalog.templates)
				{
					dbCommand.Parameters["DbContentCatalogID"].Value = id;
					dbCommand.Parameters["DbContentClipID"].Value = item.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}
			// Store dbContentCatalog in cache.
			if(cacheEnabled) cacheStore(dbContentCatalog);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(DbContentCatalog dbContentCatalog)
		{
			Database database;
			DbCommand dbCommand;

			// Set Modify Date to Now
			dbContentCatalog.ModifyDate = DateTime.Now.ToUniversalTime();

			database = DatabaseFactory.CreateDatabase();

			dbCommand = database.GetSqlStringCommand("UPDATE kitCms_Catalogs SET Title=@Title," +
				"Description=@Description," +
				"Keywords=@Keywords," +
				"Status=@Status," +
				"WorkflowMode=@WorkflowMode," +
				"CommentsEnabled=@CommentsEnabled," +
				"NotifyOnComments=@NotifyOnComments," +
				"Enabled=@Enabled," +
				"SortOrder=@SortOrder," +
				"ParentCatalogID=@ParentCatalogID," +
				"DefaultClipID=@DefaultClipID," +
				"Icon=@Icon," +
				"CreateDate=@CreateDate," +
				"ModifyDate=@ModifyDate," +
				"DefaultTimeToPublish=@DefaultTimeToPublish," +
				"DefaultTimeToExpire=@DefaultTimeToExpire," +
				"DefaultTimeToArchive=@DefaultTimeToArchive," +
				"DefaultKeywords=@DefaultKeywords," +
				"DefaultStatusID=@DefaultStatusID," +
				"DefaultRatingID=@DefaultRatingID," +
				"DefaultArchiveID=@DefaultArchiveID," +
				"DefaultMenuLeftIcon=@DefaultMenuLeftIcon," +
				"DefaultMenuRightIcon=@DefaultMenuRightIcon," +
				"MenuLabel=@MenuLabel," +
				"MenuTooltip=@MenuTooltip," +
				"MenuEnabled=@MenuEnabled," +
				"MenuOrder=@MenuOrder," +
				"MenuLeftIcon=@MenuLeftIcon," +
				"MenuRightIcon=@MenuRightIcon," +
				"MenuBreakImage=@MenuBreakImage," +
				"MenuBreakCssClass=@MenuBreakCssClass," +
				"MenuCssClass=@MenuCssClass," +
				"MenuCatalogCssClass=@MenuCatalogCssClass," +
				"MenuCatalogSelectedCssClass=@MenuCatalogSelectedCssClass," +
				"MenuCatalogChildSelectedCssClass=@MenuCatalogChildSelectedCssClass," +
				"MenuClipCssClass=@MenuClipCssClass," +
				"MenuClipSelectedCssClass=@MenuClipSelectedCssClass," +
				"MenuClipChildSelectedCssClass=@MenuClipChildSelectedCssClass," +
				"MenuClipChildExpandedCssClass=@MenuClipChildExpandedCssClass," +
				"MenuOverrideFlags=@MenuOverrideFlags," +
				"MenuIconFlags=@MenuIconFlags," +
				"AuthorRoleID=@AuthorRoleID," +
				"EditorRoleID=@EditorRoleID," +
				"ReviewerRoleID=@ReviewerRoleID WHERE DbContentCatalogID=@DbContentCatalogID;");

			fillParameters(database, dbCommand, dbContentCatalog);
			database.AddInParameter(dbCommand, "DbContentCatalogID", DbType.Int32, dbContentCatalog.iD);
			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (database.ExecuteNonQuery(dbCommand) == 0) return -1;

			if(dbContentCatalog.childCatalogs != null)
			{

				// Delete child relationships for ChildCatalogs.
				dbCommand = database.GetSqlStringCommand("DELETE * FROM kitCms_CatalogsChildren_ChildCatalogs WHERE DbContentCatalogID=@DbContentCatalogID;");
				database.AddInParameter(dbCommand, "DbContentCatalogID", DbType.Int32, dbContentCatalog.iD);
				database.ExecuteNonQuery(dbCommand);

				// Save child relationships for ChildCatalogs.
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitCms_CatalogsChildren_ChildCatalogs (DbContentCatalogID, DbContentCatalogChildID) VALUES (@DbContentCatalogID, @DbContentCatalogChildID);");
				database.AddInParameter(dbCommand, "DbContentCatalogID", DbType.Int32, dbContentCatalog.iD);
				database.AddInParameter(dbCommand, "DbContentCatalogID", DbType.Int32);
				foreach(DbContentCatalog dbContentCatalog in dbContentCatalog.childCatalogs)
				{
					dbCommand.Parameters["@DbContentCatalogChildID"].Value = dbContentCatalog.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}

			if(dbContentCatalog.templates != null)
			{

				// Delete child relationships for Templates.
				dbCommand = database.GetSqlStringCommand("DELETE * FROM kitCms_CatalogsChildren_Templates WHERE DbContentCatalogID=@DbContentCatalogID;");
				database.AddInParameter(dbCommand, "DbContentCatalogID", DbType.Int32, dbContentCatalog.iD);
				database.ExecuteNonQuery(dbCommand);

				// Save child relationships for Templates.
				dbCommand = database.GetSqlStringCommand("INSERT INTO kitCms_CatalogsChildren_Templates (DbContentCatalogID, DbContentClipID) VALUES (@DbContentCatalogID, @DbContentClipID);");
				database.AddInParameter(dbCommand, "DbContentCatalogID", DbType.Int32, dbContentCatalog.iD);
				database.AddInParameter(dbCommand, "DbContentClipID", DbType.Int32);
				foreach(DbContentClip dbContentClip in dbContentCatalog.templates)
				{
					dbCommand.Parameters["@DbContentClipID"].Value = dbContentClip.ID;
					database.ExecuteNonQuery(dbCommand);
				}
			}

			// Store dbContentCatalog in cache.
			if (cacheEnabled) cacheStore(dbContentCatalog);

			return dbContentCatalog.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(Database database, DbCommand dbCommand, DbContentCatalog dbContentCatalog)
		{
			#region General

			addParameter(database, dbCommand, "Title", DbType.String, dbContentCatalog.title);
			addParameter(database, dbCommand, "Description", DbType.String, dbContentCatalog.description);
			addParameter(database, dbCommand, "Keywords", DbType.String, dbContentCatalog.keywords);
			addParameter(database, dbCommand, "Status", DbType.Byte, dbContentCatalog.status);
			addParameter(database, dbCommand, "WorkflowMode", DbType.Byte, dbContentCatalog.workflowMode);
			addParameter(database, dbCommand, "CommentsEnabled", DbType.Boolean, dbContentCatalog.commentsEnabled);
			addParameter(database, dbCommand, "NotifyOnComments", DbType.Boolean, dbContentCatalog.notifyOnComments);
			addParameter(database, dbCommand, "Enabled", DbType.Boolean, dbContentCatalog.enabled);
			addParameter(database, dbCommand, "SortOrder", DbType.Int32, dbContentCatalog.sortOrder);
			if(dbContentCatalog.parentCatalog == null)
			{
				addParameter(database, dbCommand, "ParentCatalogID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "ParentCatalogID", DbType.Int32, dbContentCatalog.parentCatalog.ID);
			}
			if(dbContentCatalog.defaultClip == null)
			{
				addParameter(database, dbCommand, "DefaultClipID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "DefaultClipID", DbType.Int32, dbContentCatalog.defaultClip.ID);
			}
			addParameter(database, dbCommand, "Icon", DbType.String, dbContentCatalog.icon);

			#endregion

			#region _system

			addParameter(database, dbCommand, "CreateDate", DbType.Date, dbContentCatalog.createDate);
			addParameter(database, dbCommand, "ModifyDate", DbType.Date, dbContentCatalog.modifyDate);

			#endregion

			#region Defaults

			addParameter(database, dbCommand, "DefaultTimeToPublish", DbType.Double, dbContentCatalog.defaultTimeToPublish.Ticks);
			addParameter(database, dbCommand, "DefaultTimeToExpire", DbType.Double, dbContentCatalog.defaultTimeToExpire.Ticks);
			addParameter(database, dbCommand, "DefaultTimeToArchive", DbType.Double, dbContentCatalog.defaultTimeToArchive.Ticks);
			addParameter(database, dbCommand, "DefaultKeywords", DbType.String, dbContentCatalog.defaultKeywords);
			if(dbContentCatalog.defaultStatus == null)
			{
				addParameter(database, dbCommand, "DefaultStatusID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "DefaultStatusID", DbType.Int32, dbContentCatalog.defaultStatus.ID);
			}
			if(dbContentCatalog.defaultRating == null)
			{
				addParameter(database, dbCommand, "DefaultRatingID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "DefaultRatingID", DbType.Int32, dbContentCatalog.defaultRating.ID);
			}
			if(dbContentCatalog.defaultArchive == null)
			{
				addParameter(database, dbCommand, "DefaultArchiveID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "DefaultArchiveID", DbType.Int32, dbContentCatalog.defaultArchive.ID);
			}
			addParameter(database, dbCommand, "DefaultMenuLeftIcon", DbType.String, dbContentCatalog.defaultMenuLeftIcon);
			addParameter(database, dbCommand, "DefaultMenuRightIcon", DbType.String, dbContentCatalog.defaultMenuRightIcon);

			#endregion

			#region Menu

			addParameter(database, dbCommand, "MenuLabel", DbType.String, dbContentCatalog.menuLabel);
			addParameter(database, dbCommand, "MenuTooltip", DbType.String, dbContentCatalog.menuTooltip);
			addParameter(database, dbCommand, "MenuEnabled", DbType.Boolean, dbContentCatalog.menuEnabled);
			addParameter(database, dbCommand, "MenuOrder", DbType.Int32, dbContentCatalog.menuOrder);
			addParameter(database, dbCommand, "MenuLeftIcon", DbType.String, dbContentCatalog.menuLeftIcon);
			addParameter(database, dbCommand, "MenuRightIcon", DbType.String, dbContentCatalog.menuRightIcon);
			addParameter(database, dbCommand, "MenuBreakImage", DbType.String, dbContentCatalog.menuBreakImage);
			addParameter(database, dbCommand, "MenuBreakCssClass", DbType.String, dbContentCatalog.menuBreakCssClass);
			addParameter(database, dbCommand, "MenuCssClass", DbType.String, dbContentCatalog.menuCssClass);
			addParameter(database, dbCommand, "MenuCatalogCssClass", DbType.String, dbContentCatalog.menuCatalogCssClass);
			addParameter(database, dbCommand, "MenuCatalogSelectedCssClass", DbType.String, dbContentCatalog.menuCatalogSelectedCssClass);
			addParameter(database, dbCommand, "MenuCatalogChildSelectedCssClass", DbType.String, dbContentCatalog.menuCatalogChildSelectedCssClass);
			addParameter(database, dbCommand, "MenuClipCssClass", DbType.String, dbContentCatalog.menuClipCssClass);
			addParameter(database, dbCommand, "MenuClipSelectedCssClass", DbType.String, dbContentCatalog.menuClipSelectedCssClass);
			addParameter(database, dbCommand, "MenuClipChildSelectedCssClass", DbType.String, dbContentCatalog.menuClipChildSelectedCssClass);
			addParameter(database, dbCommand, "MenuClipChildExpandedCssClass", DbType.String, dbContentCatalog.menuClipChildExpandedCssClass);
			addParameter(database, dbCommand, "MenuOverrideFlags", DbType.Byte, dbContentCatalog.menuOverrideFlags);
			addParameter(database, dbCommand, "MenuIconFlags", DbType.Byte, dbContentCatalog.menuIconFlags);

			#endregion

			#region Security

			if(dbContentCatalog.authorRole == null)
			{
				addParameter(database, dbCommand, "AuthorRoleID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "AuthorRoleID", DbType.Int32, dbContentCatalog.authorRole.ID);
			}
			if(dbContentCatalog.editorRole == null)
			{
				addParameter(database, dbCommand, "EditorRoleID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "EditorRoleID", DbType.Int32, dbContentCatalog.editorRole.ID);
			}
			if(dbContentCatalog.reviewerRole == null)
			{
				addParameter(database, dbCommand, "ReviewerRoleID", DbType.Int32, DBNull.Value);
			}
			else
			{
				addParameter(database, dbCommand, "ReviewerRoleID", DbType.Int32, dbContentCatalog.reviewerRole.ID);
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

		internal static bool _fill(DbContentCatalog dbContentCatalog)
		{
			// Clone item from cache.
			if(cacheEnabled)
			{
				object cachedObject = cacheFind(dbContentCatalog.iD);
				if(cachedObject != null)
				{
					((DbContentCatalog)cachedObject).CopyTo(dbContentCatalog, true);
					return dbContentCatalog.isSynced;
				}
			}

			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM kitCms_Catalogs WHERE DbContentCatalogID=");
			query.Append(dbContentCatalog.iD);
			query.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			IDataReader r = database.ExecuteReader(dbCommand);

			if(!r.Read())
			{
				throw(new Exception(string.Format("Cannot find DbContentCatalogID '{0}'.", 
					dbContentCatalog.iD)));
			}

			FillFromReader(dbContentCatalog, r, 0, 1);

			// Store dbContentCatalog in cache.
			if(cacheEnabled) cacheStore(dbContentCatalog);

			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public DbContentCatalogCollection GetCollection(string whereClause, string sortClause, params DbContentCatalogFlags[] optionFlags)
		{
			return GetCollection(0, whereClause, sortClause, optionFlags);
		}

		public DbContentCatalogCollection GetCollection(int topCount, string whereClause, string sortClause, params DbContentCatalogFlags[] optionFlags)
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			DbContentCatalogCollection dbContentCatalogCollection;

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
				query.Append("DbContentCatalog.");
				query.Append(columnName);
				query.Append(",");
			}

			innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;
			int parentCatalogOffset = -1;
			int parentCatalogParentCatalogOffset = -1;
			int parentCatalogDefaultClipOffset = -1;
			int parentCatalogDefaultStatusOffset = -1;
			int parentCatalogDefaultRatingOffset = -1;
			int parentCatalogDefaultArchiveOffset = -1;
			int parentCatalogAuthorRoleOffset = -1;
			int parentCatalogEditorRoleOffset = -1;
			int parentCatalogReviewerRoleOffset = -1;
			int defaultClipOffset = -1;
			int defaultClipStatusOffset = -1;
			int defaultClipParentCatalogOffset = -1;
			int defaultClipRatingOffset = -1;
			int defaultClipWorkingDraftOffset = -1;
			int defaultStatusOffset = -1;
			int defaultRatingOffset = -1;
			int defaultRatingRequiredRoleOffset = -1;
			int defaultArchiveOffset = -1;
			int defaultArchiveParentCatalogOffset = -1;
			int defaultArchiveDefaultClipOffset = -1;
			int defaultArchiveDefaultStatusOffset = -1;
			int defaultArchiveDefaultRatingOffset = -1;
			int defaultArchiveDefaultArchiveOffset = -1;
			int defaultArchiveAuthorRoleOffset = -1;
			int defaultArchiveEditorRoleOffset = -1;
			int defaultArchiveReviewerRoleOffset = -1;
			int authorRoleOffset = -1;
			int editorRoleOffset = -1;
			int reviewerRoleOffset = -1;

			//
			// Append Option Flag Fields
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case DbContentCatalogFlags.ParentCatalog:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.ParentCatalogParentCatalog:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_ParentCatalog.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogParentCatalogOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogParentCatalogOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.ParentCatalogDefaultClip:
							for(int i = 0; i <= DbContentClipManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_DefaultClip.");
								query.Append(DbContentClipManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogDefaultClipOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogDefaultClipOffset + DbContentClipManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.ParentCatalogDefaultStatus:
							for(int i = 0; i <= DbContentStatusManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_DefaultStatus.");
								query.Append(DbContentStatusManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogDefaultStatusOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogDefaultStatusOffset + DbContentStatusManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.ParentCatalogDefaultRating:
							for(int i = 0; i <= DbContentRatingManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_DefaultRating.");
								query.Append(DbContentRatingManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogDefaultRatingOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogDefaultRatingOffset + DbContentRatingManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.ParentCatalogDefaultArchive:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_DefaultArchive.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogDefaultArchiveOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogDefaultArchiveOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.ParentCatalogAuthorRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_AuthorRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogAuthorRoleOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogAuthorRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.ParentCatalogEditorRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_EditorRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogEditorRoleOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogEditorRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.ParentCatalogReviewerRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ParentCatalog_ReviewerRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							parentCatalogReviewerRoleOffset = innerJoinOffset;
							innerJoinOffset = parentCatalogReviewerRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultClip:
							for(int i = 0; i <= DbContentClipManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultClip.");
								query.Append(DbContentClipManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultClipOffset = innerJoinOffset;
							innerJoinOffset = defaultClipOffset + DbContentClipManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultClipStatus:
							for(int i = 0; i <= DbContentStatusManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultClip_Status.");
								query.Append(DbContentStatusManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultClipStatusOffset = innerJoinOffset;
							innerJoinOffset = defaultClipStatusOffset + DbContentStatusManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultClipParentCatalog:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultClip_ParentCatalog.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultClipParentCatalogOffset = innerJoinOffset;
							innerJoinOffset = defaultClipParentCatalogOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultClipRating:
							for(int i = 0; i <= DbContentRatingManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultClip_Rating.");
								query.Append(DbContentRatingManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultClipRatingOffset = innerJoinOffset;
							innerJoinOffset = defaultClipRatingOffset + DbContentRatingManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultClipWorkingDraft:
							for(int i = 0; i <= DbContentClipManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultClip_WorkingDraft.");
								query.Append(DbContentClipManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultClipWorkingDraftOffset = innerJoinOffset;
							innerJoinOffset = defaultClipWorkingDraftOffset + DbContentClipManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultStatus:
							for(int i = 0; i <= DbContentStatusManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultStatus.");
								query.Append(DbContentStatusManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultStatusOffset = innerJoinOffset;
							innerJoinOffset = defaultStatusOffset + DbContentStatusManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultRating:
							for(int i = 0; i <= DbContentRatingManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultRating.");
								query.Append(DbContentRatingManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultRatingOffset = innerJoinOffset;
							innerJoinOffset = defaultRatingOffset + DbContentRatingManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultRatingRequiredRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultRating_RequiredRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultRatingRequiredRoleOffset = innerJoinOffset;
							innerJoinOffset = defaultRatingRequiredRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultArchive:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultArchive.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultArchiveOffset = innerJoinOffset;
							innerJoinOffset = defaultArchiveOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultArchiveParentCatalog:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultArchive_ParentCatalog.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultArchiveParentCatalogOffset = innerJoinOffset;
							innerJoinOffset = defaultArchiveParentCatalogOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultArchiveDefaultClip:
							for(int i = 0; i <= DbContentClipManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultArchive_DefaultClip.");
								query.Append(DbContentClipManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultArchiveDefaultClipOffset = innerJoinOffset;
							innerJoinOffset = defaultArchiveDefaultClipOffset + DbContentClipManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultArchiveDefaultStatus:
							for(int i = 0; i <= DbContentStatusManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultArchive_DefaultStatus.");
								query.Append(DbContentStatusManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultArchiveDefaultStatusOffset = innerJoinOffset;
							innerJoinOffset = defaultArchiveDefaultStatusOffset + DbContentStatusManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultArchiveDefaultRating:
							for(int i = 0; i <= DbContentRatingManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultArchive_DefaultRating.");
								query.Append(DbContentRatingManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultArchiveDefaultRatingOffset = innerJoinOffset;
							innerJoinOffset = defaultArchiveDefaultRatingOffset + DbContentRatingManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultArchiveDefaultArchive:
							for(int i = 0; i <= DbContentCatalogManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultArchive_DefaultArchive.");
								query.Append(DbContentCatalogManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultArchiveDefaultArchiveOffset = innerJoinOffset;
							innerJoinOffset = defaultArchiveDefaultArchiveOffset + DbContentCatalogManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultArchiveAuthorRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultArchive_AuthorRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultArchiveAuthorRoleOffset = innerJoinOffset;
							innerJoinOffset = defaultArchiveAuthorRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultArchiveEditorRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultArchive_EditorRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultArchiveEditorRoleOffset = innerJoinOffset;
							innerJoinOffset = defaultArchiveEditorRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.DefaultArchiveReviewerRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("DefaultArchive_ReviewerRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							defaultArchiveReviewerRoleOffset = innerJoinOffset;
							innerJoinOffset = defaultArchiveReviewerRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.AuthorRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("AuthorRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							authorRoleOffset = innerJoinOffset;
							innerJoinOffset = authorRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.EditorRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("EditorRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							editorRoleOffset = innerJoinOffset;
							innerJoinOffset = editorRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
							break;
						case DbContentCatalogFlags.ReviewerRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("ReviewerRole.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							reviewerRoleOffset = innerJoinOffset;
							innerJoinOffset = reviewerRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
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

				query.Append("kitCms_Catalogs AS DbContentCatalog");
			}
			else
			{
				query.Append(" FROM kitCms_Catalogs AS DbContentCatalog");
			}
			//
			// Finish INNER JOIN expressions
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case DbContentCatalogFlags.ParentCatalog:
							query.Append(" LEFT JOIN kitCms_Catalogs AS ParentCatalog ON DbContentCatalog.ParentCatalogID = ParentCatalog.DbContentCatalogID)");
							break;
						case DbContentCatalogFlags.ParentCatalogParentCatalog:
							query.Append(" LEFT JOIN kitCms_Catalogs AS ParentCatalog_ParentCatalog ON ParentCatalog.ParentCatalogID = ParentCatalog_ParentCatalog.DbContentCatalogID)");
							break;
						case DbContentCatalogFlags.ParentCatalogDefaultClip:
							query.Append(" LEFT JOIN kitCms_Clips AS ParentCatalog_DefaultClip ON ParentCatalog.DefaultClipID = ParentCatalog_DefaultClip.DbContentClipID)");
							break;
						case DbContentCatalogFlags.ParentCatalogDefaultStatus:
							query.Append(" LEFT JOIN kitCms_Statuses AS ParentCatalog_DefaultStatus ON ParentCatalog.DefaultStatusID = ParentCatalog_DefaultStatus.DbContentStatusID)");
							break;
						case DbContentCatalogFlags.ParentCatalogDefaultRating:
							query.Append(" LEFT JOIN kitCms_Ratings AS ParentCatalog_DefaultRating ON ParentCatalog.DefaultRatingID = ParentCatalog_DefaultRating.DbContentRatingID)");
							break;
						case DbContentCatalogFlags.ParentCatalogDefaultArchive:
							query.Append(" LEFT JOIN kitCms_Catalogs AS ParentCatalog_DefaultArchive ON ParentCatalog.DefaultArchiveID = ParentCatalog_DefaultArchive.DbContentCatalogID)");
							break;
						case DbContentCatalogFlags.ParentCatalogAuthorRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS ParentCatalog_AuthorRole ON ParentCatalog.AuthorRoleID = ParentCatalog_AuthorRole.GreyFoxRoleID)");
							break;
						case DbContentCatalogFlags.ParentCatalogEditorRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS ParentCatalog_EditorRole ON ParentCatalog.EditorRoleID = ParentCatalog_EditorRole.GreyFoxRoleID)");
							break;
						case DbContentCatalogFlags.ParentCatalogReviewerRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS ParentCatalog_ReviewerRole ON ParentCatalog.ReviewerRoleID = ParentCatalog_ReviewerRole.GreyFoxRoleID)");
							break;
						case DbContentCatalogFlags.DefaultClip:
							query.Append(" LEFT JOIN kitCms_Clips AS DefaultClip ON DbContentCatalog.DefaultClipID = DefaultClip.DbContentClipID)");
							break;
						case DbContentCatalogFlags.DefaultClipStatus:
							query.Append(" LEFT JOIN kitCms_Statuses AS DefaultClip_Status ON DefaultClip.StatusID = DefaultClip_Status.DbContentStatusID)");
							break;
						case DbContentCatalogFlags.DefaultClipParentCatalog:
							query.Append(" LEFT JOIN kitCms_Catalogs AS DefaultClip_ParentCatalog ON DefaultClip.ParentCatalogID = DefaultClip_ParentCatalog.DbContentCatalogID)");
							break;
						case DbContentCatalogFlags.DefaultClipRating:
							query.Append(" LEFT JOIN kitCms_Ratings AS DefaultClip_Rating ON DefaultClip.RatingID = DefaultClip_Rating.DbContentRatingID)");
							break;
						case DbContentCatalogFlags.DefaultClipWorkingDraft:
							query.Append(" LEFT JOIN kitCms_Clips AS DefaultClip_WorkingDraft ON DefaultClip.WorkingDraftID = DefaultClip_WorkingDraft.DbContentClipID)");
							break;
						case DbContentCatalogFlags.DefaultStatus:
							query.Append(" LEFT JOIN kitCms_Statuses AS DefaultStatus ON DbContentCatalog.DefaultStatusID = DefaultStatus.DbContentStatusID)");
							break;
						case DbContentCatalogFlags.DefaultRating:
							query.Append(" LEFT JOIN kitCms_Ratings AS DefaultRating ON DbContentCatalog.DefaultRatingID = DefaultRating.DbContentRatingID)");
							break;
						case DbContentCatalogFlags.DefaultRatingRequiredRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS DefaultRating_RequiredRole ON DefaultRating.RequiredRoleID = DefaultRating_RequiredRole.GreyFoxRoleID)");
							break;
						case DbContentCatalogFlags.DefaultArchive:
							query.Append(" LEFT JOIN kitCms_Catalogs AS DefaultArchive ON DbContentCatalog.DefaultArchiveID = DefaultArchive.DbContentCatalogID)");
							break;
						case DbContentCatalogFlags.DefaultArchiveParentCatalog:
							query.Append(" LEFT JOIN kitCms_Catalogs AS DefaultArchive_ParentCatalog ON DefaultArchive.ParentCatalogID = DefaultArchive_ParentCatalog.DbContentCatalogID)");
							break;
						case DbContentCatalogFlags.DefaultArchiveDefaultClip:
							query.Append(" LEFT JOIN kitCms_Clips AS DefaultArchive_DefaultClip ON DefaultArchive.DefaultClipID = DefaultArchive_DefaultClip.DbContentClipID)");
							break;
						case DbContentCatalogFlags.DefaultArchiveDefaultStatus:
							query.Append(" LEFT JOIN kitCms_Statuses AS DefaultArchive_DefaultStatus ON DefaultArchive.DefaultStatusID = DefaultArchive_DefaultStatus.DbContentStatusID)");
							break;
						case DbContentCatalogFlags.DefaultArchiveDefaultRating:
							query.Append(" LEFT JOIN kitCms_Ratings AS DefaultArchive_DefaultRating ON DefaultArchive.DefaultRatingID = DefaultArchive_DefaultRating.DbContentRatingID)");
							break;
						case DbContentCatalogFlags.DefaultArchiveDefaultArchive:
							query.Append(" LEFT JOIN kitCms_Catalogs AS DefaultArchive_DefaultArchive ON DefaultArchive.DefaultArchiveID = DefaultArchive_DefaultArchive.DbContentCatalogID)");
							break;
						case DbContentCatalogFlags.DefaultArchiveAuthorRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS DefaultArchive_AuthorRole ON DefaultArchive.AuthorRoleID = DefaultArchive_AuthorRole.GreyFoxRoleID)");
							break;
						case DbContentCatalogFlags.DefaultArchiveEditorRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS DefaultArchive_EditorRole ON DefaultArchive.EditorRoleID = DefaultArchive_EditorRole.GreyFoxRoleID)");
							break;
						case DbContentCatalogFlags.DefaultArchiveReviewerRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS DefaultArchive_ReviewerRole ON DefaultArchive.ReviewerRoleID = DefaultArchive_ReviewerRole.GreyFoxRoleID)");
							break;
						case DbContentCatalogFlags.AuthorRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS AuthorRole ON DbContentCatalog.AuthorRoleID = AuthorRole.GreyFoxRoleID)");
							break;
						case DbContentCatalogFlags.EditorRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS EditorRole ON DbContentCatalog.EditorRoleID = EditorRole.GreyFoxRoleID)");
							break;
						case DbContentCatalogFlags.ReviewerRole:
							query.Append(" LEFT JOIN sysGlobal_Roles AS ReviewerRole ON DbContentCatalog.ReviewerRoleID = ReviewerRole.GreyFoxRoleID)");
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

			dbContentCatalogCollection = new DbContentCatalogCollection();

			while(r.Read())
			{
				DbContentCatalog dbContentCatalog = ParseFromReader(r, 0, 1);

				// Fill ParentCatalog
				if(parentCatalogOffset != -1 && !r.IsDBNull(parentCatalogOffset))
				{
					DbContentCatalogManager.FillFromReader(dbContentCatalog.parentCatalog, r, parentCatalogOffset, parentCatalogOffset+1);

					// Fill 
					if(parentCatalogParentCatalogOffset != -1 && !r.IsDBNull(parentCatalogParentCatalogOffset))
						DbContentCatalogManager.FillFromReader(dbContentCatalog.parentCatalog.ParentCatalog, r, parentCatalogParentCatalogOffset, parentCatalogParentCatalogOffset+1);

					// Fill 
					if(parentCatalogDefaultClipOffset != -1 && !r.IsDBNull(parentCatalogDefaultClipOffset))
						DbContentClipManager.FillFromReader(dbContentCatalog.parentCatalog.DefaultClip, r, parentCatalogDefaultClipOffset, parentCatalogDefaultClipOffset+1);

					// Fill 
					if(parentCatalogDefaultStatusOffset != -1 && !r.IsDBNull(parentCatalogDefaultStatusOffset))
						DbContentStatusManager.FillFromReader(dbContentCatalog.parentCatalog.DefaultStatus, r, parentCatalogDefaultStatusOffset, parentCatalogDefaultStatusOffset+1);

					// Fill 
					if(parentCatalogDefaultRatingOffset != -1 && !r.IsDBNull(parentCatalogDefaultRatingOffset))
						DbContentRatingManager.FillFromReader(dbContentCatalog.parentCatalog.DefaultRating, r, parentCatalogDefaultRatingOffset, parentCatalogDefaultRatingOffset+1);

					// Fill Default Archive
					if(parentCatalogDefaultArchiveOffset != -1 && !r.IsDBNull(parentCatalogDefaultArchiveOffset))
						DbContentCatalogManager.FillFromReader(dbContentCatalog.parentCatalog.DefaultArchive, r, parentCatalogDefaultArchiveOffset, parentCatalogDefaultArchiveOffset+1);

					// Fill Author Roles
					if(parentCatalogAuthorRoleOffset != -1 && !r.IsDBNull(parentCatalogAuthorRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentCatalog.parentCatalog.AuthorRole, r, parentCatalogAuthorRoleOffset, parentCatalogAuthorRoleOffset+1);

					// Fill Editor Roles
					if(parentCatalogEditorRoleOffset != -1 && !r.IsDBNull(parentCatalogEditorRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentCatalog.parentCatalog.EditorRole, r, parentCatalogEditorRoleOffset, parentCatalogEditorRoleOffset+1);

					// Fill Reviewer Role
					if(parentCatalogReviewerRoleOffset != -1 && !r.IsDBNull(parentCatalogReviewerRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentCatalog.parentCatalog.ReviewerRole, r, parentCatalogReviewerRoleOffset, parentCatalogReviewerRoleOffset+1);

				}

				// Fill DefaultClip
				if(defaultClipOffset != -1 && !r.IsDBNull(defaultClipOffset))
				{
					DbContentClipManager.FillFromReader(dbContentCatalog.defaultClip, r, defaultClipOffset, defaultClipOffset+1);

					// Fill 
					if(defaultClipStatusOffset != -1 && !r.IsDBNull(defaultClipStatusOffset))
						DbContentStatusManager.FillFromReader(dbContentCatalog.defaultClip.Status, r, defaultClipStatusOffset, defaultClipStatusOffset+1);

					// Fill Parent Catalog
					if(defaultClipParentCatalogOffset != -1 && !r.IsDBNull(defaultClipParentCatalogOffset))
						DbContentCatalogManager.FillFromReader(dbContentCatalog.defaultClip.ParentCatalog, r, defaultClipParentCatalogOffset, defaultClipParentCatalogOffset+1);

					// Fill 
					if(defaultClipRatingOffset != -1 && !r.IsDBNull(defaultClipRatingOffset))
						DbContentRatingManager.FillFromReader(dbContentCatalog.defaultClip.Rating, r, defaultClipRatingOffset, defaultClipRatingOffset+1);

					// Fill 
					if(defaultClipWorkingDraftOffset != -1 && !r.IsDBNull(defaultClipWorkingDraftOffset))
						DbContentClipManager.FillFromReader(dbContentCatalog.defaultClip.WorkingDraft, r, defaultClipWorkingDraftOffset, defaultClipWorkingDraftOffset+1);

				}

				// Fill DefaultStatus
				if(defaultStatusOffset != -1 && !r.IsDBNull(defaultStatusOffset))
					DbContentStatusManager.FillFromReader(dbContentCatalog.defaultStatus, r, defaultStatusOffset, defaultStatusOffset+1);

				// Fill DefaultRating
				if(defaultRatingOffset != -1 && !r.IsDBNull(defaultRatingOffset))
				{
					DbContentRatingManager.FillFromReader(dbContentCatalog.defaultRating, r, defaultRatingOffset, defaultRatingOffset+1);

					// Fill 
					if(defaultRatingRequiredRoleOffset != -1 && !r.IsDBNull(defaultRatingRequiredRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentCatalog.defaultRating.RequiredRole, r, defaultRatingRequiredRoleOffset, defaultRatingRequiredRoleOffset+1);

				}

				// Fill DefaultArchive
				if(defaultArchiveOffset != -1 && !r.IsDBNull(defaultArchiveOffset))
				{
					DbContentCatalogManager.FillFromReader(dbContentCatalog.defaultArchive, r, defaultArchiveOffset, defaultArchiveOffset+1);

					// Fill 
					if(defaultArchiveParentCatalogOffset != -1 && !r.IsDBNull(defaultArchiveParentCatalogOffset))
						DbContentCatalogManager.FillFromReader(dbContentCatalog.defaultArchive.ParentCatalog, r, defaultArchiveParentCatalogOffset, defaultArchiveParentCatalogOffset+1);

					// Fill 
					if(defaultArchiveDefaultClipOffset != -1 && !r.IsDBNull(defaultArchiveDefaultClipOffset))
						DbContentClipManager.FillFromReader(dbContentCatalog.defaultArchive.DefaultClip, r, defaultArchiveDefaultClipOffset, defaultArchiveDefaultClipOffset+1);

					// Fill 
					if(defaultArchiveDefaultStatusOffset != -1 && !r.IsDBNull(defaultArchiveDefaultStatusOffset))
						DbContentStatusManager.FillFromReader(dbContentCatalog.defaultArchive.DefaultStatus, r, defaultArchiveDefaultStatusOffset, defaultArchiveDefaultStatusOffset+1);

					// Fill 
					if(defaultArchiveDefaultRatingOffset != -1 && !r.IsDBNull(defaultArchiveDefaultRatingOffset))
						DbContentRatingManager.FillFromReader(dbContentCatalog.defaultArchive.DefaultRating, r, defaultArchiveDefaultRatingOffset, defaultArchiveDefaultRatingOffset+1);

					// Fill Default Archive
					if(defaultArchiveDefaultArchiveOffset != -1 && !r.IsDBNull(defaultArchiveDefaultArchiveOffset))
						DbContentCatalogManager.FillFromReader(dbContentCatalog.defaultArchive.DefaultArchive, r, defaultArchiveDefaultArchiveOffset, defaultArchiveDefaultArchiveOffset+1);

					// Fill Author Roles
					if(defaultArchiveAuthorRoleOffset != -1 && !r.IsDBNull(defaultArchiveAuthorRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentCatalog.defaultArchive.AuthorRole, r, defaultArchiveAuthorRoleOffset, defaultArchiveAuthorRoleOffset+1);

					// Fill Editor Roles
					if(defaultArchiveEditorRoleOffset != -1 && !r.IsDBNull(defaultArchiveEditorRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentCatalog.defaultArchive.EditorRole, r, defaultArchiveEditorRoleOffset, defaultArchiveEditorRoleOffset+1);

					// Fill Reviewer Role
					if(defaultArchiveReviewerRoleOffset != -1 && !r.IsDBNull(defaultArchiveReviewerRoleOffset))
						GreyFoxRoleManager.FillFromReader(dbContentCatalog.defaultArchive.ReviewerRole, r, defaultArchiveReviewerRoleOffset, defaultArchiveReviewerRoleOffset+1);

				}

				// Fill AuthorRole
				if(authorRoleOffset != -1 && !r.IsDBNull(authorRoleOffset))
					GreyFoxRoleManager.FillFromReader(dbContentCatalog.authorRole, r, authorRoleOffset, authorRoleOffset+1);

				// Fill EditorRole
				if(editorRoleOffset != -1 && !r.IsDBNull(editorRoleOffset))
					GreyFoxRoleManager.FillFromReader(dbContentCatalog.editorRole, r, editorRoleOffset, editorRoleOffset+1);

				// Fill ReviewerRole
				if(reviewerRoleOffset != -1 && !r.IsDBNull(reviewerRoleOffset))
					GreyFoxRoleManager.FillFromReader(dbContentCatalog.reviewerRole, r, reviewerRoleOffset, reviewerRoleOffset+1);

				dbContentCatalogCollection.Add(dbContentCatalog);
			}

			return dbContentCatalogCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static DbContentCatalog ParseFromReader(IDataReader r, int idOffset, int dataOffset)
		{
			DbContentCatalog dbContentCatalog = new DbContentCatalog();
			FillFromReader(dbContentCatalog, r, idOffset, dataOffset);
			return dbContentCatalog;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleIDataReader.
		/// </summary>
		public static void FillFromReader(DbContentCatalog dbContentCatalog, IDataReader r, int idOffset, int dataOffset)
		{
			dbContentCatalog.iD = r.GetInt32(idOffset);
			dbContentCatalog.isSynced = true;
			dbContentCatalog.isPlaceHolder = false;

			dbContentCatalog.title = r.GetString(0+dataOffset);
			dbContentCatalog.description = r.GetString(1+dataOffset);
			dbContentCatalog.keywords = r.GetString(2+dataOffset);
			dbContentCatalog.status = r.GetByte(3+dataOffset);
			dbContentCatalog.workflowMode = r.GetByte(4+dataOffset);
			dbContentCatalog.commentsEnabled = r.GetBoolean(5+dataOffset);
			dbContentCatalog.notifyOnComments = r.GetBoolean(6+dataOffset);
			dbContentCatalog.enabled = r.GetBoolean(7+dataOffset);
			dbContentCatalog.sortOrder = r.GetInt32(8+dataOffset);
			if(!r.IsDBNull(9+dataOffset) && r.GetInt32(9+dataOffset) > 0)
			{
				dbContentCatalog.parentCatalog = DbContentCatalog.NewPlaceHolder(r.GetInt32(9+dataOffset));
			}
			if(!r.IsDBNull(10+dataOffset) && r.GetInt32(10+dataOffset) > 0)
			{
				dbContentCatalog.defaultClip = DbContentClip.NewPlaceHolder(r.GetInt32(10+dataOffset));
			}
			dbContentCatalog.icon = r.GetString(11+dataOffset);
			dbContentCatalog.createDate = r.GetDateTime(12+dataOffset);
			dbContentCatalog.modifyDate = r.GetDateTime(13+dataOffset);
			dbContentCatalog.defaultTimeToPublish = TimeSpan.FromTicks((long) r.GetDouble(14+dataOffset));
			dbContentCatalog.defaultTimeToExpire = TimeSpan.FromTicks((long) r.GetDouble(15+dataOffset));
			dbContentCatalog.defaultTimeToArchive = TimeSpan.FromTicks((long) r.GetDouble(16+dataOffset));
			dbContentCatalog.defaultKeywords = r.GetString(17+dataOffset);
			if(!r.IsDBNull(18+dataOffset) && r.GetInt32(18+dataOffset) > 0)
			{
				dbContentCatalog.defaultStatus = DbContentStatus.NewPlaceHolder(r.GetInt32(18+dataOffset));
			}
			if(!r.IsDBNull(19+dataOffset) && r.GetInt32(19+dataOffset) > 0)
			{
				dbContentCatalog.defaultRating = DbContentRating.NewPlaceHolder(r.GetInt32(19+dataOffset));
			}
			if(!r.IsDBNull(20+dataOffset) && r.GetInt32(20+dataOffset) > 0)
			{
				dbContentCatalog.defaultArchive = DbContentCatalog.NewPlaceHolder(r.GetInt32(20+dataOffset));
			}
			dbContentCatalog.defaultMenuLeftIcon = r.GetString(21+dataOffset);
			dbContentCatalog.defaultMenuRightIcon = r.GetString(22+dataOffset);
			dbContentCatalog.menuLabel = r.GetString(23+dataOffset);
			dbContentCatalog.menuTooltip = r.GetString(24+dataOffset);
			dbContentCatalog.menuEnabled = r.GetBoolean(25+dataOffset);
			dbContentCatalog.menuOrder = r.GetInt32(26+dataOffset);
			dbContentCatalog.menuLeftIcon = r.GetString(27+dataOffset);
			dbContentCatalog.menuRightIcon = r.GetString(28+dataOffset);
			dbContentCatalog.menuBreakImage = r.GetString(29+dataOffset);
			dbContentCatalog.menuBreakCssClass = r.GetString(30+dataOffset);
			dbContentCatalog.menuCssClass = r.GetString(31+dataOffset);
			dbContentCatalog.menuCatalogCssClass = r.GetString(32+dataOffset);
			dbContentCatalog.menuCatalogSelectedCssClass = r.GetString(33+dataOffset);
			dbContentCatalog.menuCatalogChildSelectedCssClass = r.GetString(34+dataOffset);
			dbContentCatalog.menuClipCssClass = r.GetString(35+dataOffset);
			dbContentCatalog.menuClipSelectedCssClass = r.GetString(36+dataOffset);
			dbContentCatalog.menuClipChildSelectedCssClass = r.GetString(37+dataOffset);
			dbContentCatalog.menuClipChildExpandedCssClass = r.GetString(38+dataOffset);
			dbContentCatalog.menuOverrideFlags = r.GetByte(39+dataOffset);
			dbContentCatalog.menuIconFlags = r.GetByte(40+dataOffset);
			if(!r.IsDBNull(41+dataOffset) && r.GetInt32(41+dataOffset) > 0)
			{
				dbContentCatalog.authorRole = GreyFoxRole.NewPlaceHolder(r.GetInt32(41+dataOffset));
			}
			if(!r.IsDBNull(42+dataOffset) && r.GetInt32(42+dataOffset) > 0)
			{
				dbContentCatalog.editorRole = GreyFoxRole.NewPlaceHolder(r.GetInt32(42+dataOffset));
			}
			if(!r.IsDBNull(43+dataOffset) && r.GetInt32(43+dataOffset) > 0)
			{
				dbContentCatalog.reviewerRole = GreyFoxRole.NewPlaceHolder(r.GetInt32(43+dataOffset));
			}
		}

		#endregion

		#region Default DbModel Fill Methods

		public static void FillChildCatalogs(DbContentCatalog dbContentCatalog)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			s = new StringBuilder("SELECT DbContentCatalogChildID FROM kitCms_CatalogsChildren_ChildCatalogs ");
			s.Append("WHERE DbContentCatalogID=");
			s.Append(dbContentCatalog.iD);
			s.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(s.ToString());
			r = database.ExecuteReader(dbCommand);

			DbContentCatalogCollection childCatalogs;
			if(dbContentCatalog.childCatalogs != null)
			{
				childCatalogs = dbContentCatalog.childCatalogs;
				childCatalogs.Clear();
			}
			else
			{
				childCatalogs = new DbContentCatalogCollection();
				dbContentCatalog.childCatalogs = childCatalogs;
			}

			while(r.Read())
				childCatalogs.Add(DbContentCatalog.NewPlaceHolder(r.GetInt32(0)));

			dbContentCatalog.ChildCatalogs = childCatalogs;
			// Store DbContentCatalog in cache.
			if(cacheEnabled) cacheStore(dbContentCatalog);
		}

		public static void FillChildCatalogs(DbContentCatalogCollection dbContentCatalogCollection)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			if(dbContentCatalogCollection.Count > 0)
			{
				s = new StringBuilder("SELECT DbContentCatalogID, DbContentCatalogChildID FROM kitCms_CatalogsChildren_ChildCatalogs ORDER BY DbContentCatalogID; ");

				// Clone and sort collection by ID first to fill children in one pass
				DbContentCatalogCollection clonedCollection = dbContentCatalogCollection.Clone();
				clonedCollection.Sort();

				database = DatabaseFactory.CreateDatabase();
				dbCommand = database.GetSqlStringCommand(s.ToString());
				r = database.ExecuteReader(dbCommand);

				bool more = r.Read();

				foreach(DbContentCatalog dbContentCatalog in clonedCollection)
				{
					DbContentCatalogCollection childCatalogs;
					if(dbContentCatalog.childCatalogs != null)
					{
						childCatalogs = dbContentCatalog.childCatalogs;
						childCatalogs.Clear();
					}
					else
					{
						childCatalogs = new DbContentCatalogCollection();
						dbContentCatalog.childCatalogs = childCatalogs;
					}

					while(more)
					{
						if(r.GetInt32(0) < dbContentCatalog.iD)
						{
							more = r.Read();
						}
						else if(r.GetInt32(0) == dbContentCatalog.iD)
						{
							childCatalogs.Add(DbContentCatalog.NewPlaceHolder(r.GetInt32(1)));
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

		public static void FillTemplates(DbContentCatalog dbContentCatalog)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			s = new StringBuilder("SELECT DbContentClipID FROM kitCms_CatalogsChildren_Templates ");
			s.Append("WHERE DbContentCatalogID=");
			s.Append(dbContentCatalog.iD);
			s.Append(";");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(s.ToString());
			r = database.ExecuteReader(dbCommand);

			DbContentClipCollection templates;
			if(dbContentCatalog.templates != null)
			{
				templates = dbContentCatalog.templates;
				templates.Clear();
			}
			else
			{
				templates = new DbContentClipCollection();
				dbContentCatalog.templates = templates;
			}

			while(r.Read())
				templates.Add(DbContentClip.NewPlaceHolder(r.GetInt32(0)));

			dbContentCatalog.Templates = templates;
			// Store DbContentCatalog in cache.
			if(cacheEnabled) cacheStore(dbContentCatalog);
		}

		public static void FillTemplates(DbContentCatalogCollection dbContentCatalogCollection)
		{
			StringBuilder s;
			Database database;
			DbCommand dbCommand;
			IDataReader r;
			if(dbContentCatalogCollection.Count > 0)
			{
				s = new StringBuilder("SELECT DbContentCatalogID, DbContentClipID FROM kitCms_CatalogsChildren_Templates ORDER BY DbContentCatalogID; ");

				// Clone and sort collection by ID first to fill children in one pass
				DbContentCatalogCollection clonedCollection = dbContentCatalogCollection.Clone();
				clonedCollection.Sort();

				database = DatabaseFactory.CreateDatabase();
				dbCommand = database.GetSqlStringCommand(s.ToString());
				r = database.ExecuteReader(dbCommand);

				bool more = r.Read();

				foreach(DbContentCatalog dbContentCatalog in clonedCollection)
				{
					DbContentClipCollection templates;
					if(dbContentCatalog.templates != null)
					{
						templates = dbContentCatalog.templates;
						templates.Clear();
					}
					else
					{
						templates = new DbContentClipCollection();
						dbContentCatalog.templates = templates;
					}

					while(more)
					{
						if(r.GetInt32(0) < dbContentCatalog.iD)
						{
							more = r.Read();
						}
						else if(r.GetInt32(0) == dbContentCatalog.iD)
						{
							templates.Add(DbContentClip.NewPlaceHolder(r.GetInt32(1)));
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

			query = new StringBuilder("DELETE * FROM kitCms_Catalogs WHERE DbContentCatalogID=");
			query.Append(id);
			query.Append(';');

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);


			// Delete child relationships for ChildCatalogs.
			query.Length = 0;
			query.Append("DELETE * FROM kitCms_CatalogsChildren_ChildCatalogs WHERE ");
			query.Append("DbContentCatalogID=");
			query.Append(id);
			query.Append(";");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			// Delete child relationships for Templates.
			query.Length = 0;
			query.Append("DELETE * FROM kitCms_CatalogsChildren_Templates WHERE ");
			query.Append("DbContentCatalogID=");
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

			query = new StringBuilder("ALTER TABLE kitCms_Catalogs ADD ");
			query.Append(" CONSTRAINT kitCms_Catalogs_ParentCatalog_FK FOREIGN KEY (ParentCatalogID) REFERENCES kitCms_Catalogs(DbContentCatalogID),");
			query.Append(" CONSTRAINT kitCms_Catalogs_DefaultClip_FK FOREIGN KEY (DefaultClipID) REFERENCES kitCms_Clips(DbContentClipID),");
			query.Append(" CONSTRAINT kitCms_Catalogs_DefaultStatus_FK FOREIGN KEY (DefaultStatusID) REFERENCES kitCms_Statuses(DbContentStatusID),");
			query.Append(" CONSTRAINT kitCms_Catalogs_DefaultRating_FK FOREIGN KEY (DefaultRatingID) REFERENCES kitCms_Ratings(DbContentRatingID),");
			query.Append(" CONSTRAINT kitCms_Catalogs_DefaultArchive_FK FOREIGN KEY (DefaultArchiveID) REFERENCES kitCms_Catalogs(DbContentCatalogID),");
			query.Append(" CONSTRAINT kitCms_Catalogs_AuthorRole_FK FOREIGN KEY (AuthorRoleID) REFERENCES sysGlobal_Roles(GreyFoxRoleID),");
			query.Append(" CONSTRAINT kitCms_Catalogs_EditorRole_FK FOREIGN KEY (EditorRoleID) REFERENCES sysGlobal_Roles(GreyFoxRoleID),");
			query.Append(" CONSTRAINT kitCms_Catalogs_ReviewerRole_FK FOREIGN KEY (ReviewerRoleID) REFERENCES sysGlobal_Roles(GreyFoxRoleID);");
			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			query.Length = 0;
			query.Append("ALTER TABLE kitCms_CatalogsChildren_ChildCatalogs ");
			query.Append(" ADD CONSTRAINT DbContentCatalogChildren_ChildCatalogs_DbContentCatalog_FK FOREIGN KEY (DbContentCatalogID) REFERENCES kitCms_Catalogs(DbContentCatalogID), CONSTRAINT DbContentCatalogChildren_ChildCatalogs_DbContentCatalog_FK FOREIGN KEY (DbContentCatalogID) REFERENCES kitCms_Catalogs(DbContentCatalogID);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			query.Length = 0;
			query.Append("ALTER TABLE kitCms_CatalogsChildren_Templates ");
			query.Append(" ADD CONSTRAINT DbContentCatalogChildren_Templates_DbContentCatalog_FK FOREIGN KEY (DbContentCatalogID) REFERENCES kitCms_Catalogs(DbContentCatalogID), CONSTRAINT DbContentCatalogChildren_Templates_DbContentClip_FK FOREIGN KEY (DbContentClipID) REFERENCES kitCms_Clips(DbContentClipID);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		public void CreateTable()
		{
			StringBuilder query;
			Database database;
			DbCommand dbCommand;

			query = new StringBuilder("CREATE TABLE kitCms_Catalogs ");
			query.Append(" (DbContentCatalogID COUNTER(1,1) CONSTRAINT DbContentCatalogID PRIMARY KEY, " +
				"Title TEXT(255)," +
				"Description MEMO," +
				"Keywords TEXT(255)," +
				"Status BYTE," +
				"WorkflowMode BYTE," +
				"CommentsEnabled BIT," +
				"NotifyOnComments BIT," +
				"Enabled BIT," +
				"SortOrder LONG," +
				"ParentCatalogID LONG," +
				"DefaultClipID LONG," +
				"Icon TEXT(255)," +
				"CreateDate DATETIME," +
				"ModifyDate DATETIME," +
				"DefaultTimeToPublish DOUBLE," +
				"DefaultTimeToExpire DOUBLE," +
				"DefaultTimeToArchive DOUBLE," +
				"DefaultKeywords TEXT(75)," +
				"DefaultStatusID LONG," +
				"DefaultRatingID LONG," +
				"DefaultArchiveID LONG," +
				"DefaultMenuLeftIcon TEXT(255)," +
				"DefaultMenuRightIcon TEXT(255)," +
				"MenuLabel TEXT(75)," +
				"MenuTooltip TEXT(255)," +
				"MenuEnabled BIT," +
				"MenuOrder LONG," +
				"MenuLeftIcon TEXT(255)," +
				"MenuRightIcon TEXT(255)," +
				"MenuBreakImage TEXT(255)," +
				"MenuBreakCssClass TEXT(255)," +
				"MenuCssClass TEXT(75)," +
				"MenuCatalogCssClass TEXT(75)," +
				"MenuCatalogSelectedCssClass TEXT(75)," +
				"MenuCatalogChildSelectedCssClass TEXT(75)," +
				"MenuClipCssClass TEXT(75)," +
				"MenuClipSelectedCssClass TEXT(75)," +
				"MenuClipChildSelectedCssClass TEXT(75)," +
				"MenuClipChildExpandedCssClass TEXT(75)," +
				"MenuOverrideFlags BYTE," +
				"MenuIconFlags BYTE," +
				"AuthorRoleID LONG," +
				"EditorRoleID LONG," +
				"ReviewerRoleID LONG);");

			database = DatabaseFactory.CreateDatabase();
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			//
			// Create children table for ChildCatalogs.
			//
			query.Length = 0;
			query.Append("CREATE TABLE kitCms_CatalogsChildren_ChildCatalogs ");
			query.Append("(DbContentCatalogID LONG, DbContentCatalogChildID LONG);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

			//
			// Create children table for Templates.
			//
			query.Length = 0;
			query.Append("CREATE TABLE kitCms_CatalogsChildren_Templates ");
			query.Append("(DbContentCatalogID LONG, DbContentClipID LONG);");
			dbCommand = database.GetSqlStringCommand(query.ToString());
			database.ExecuteNonQuery(dbCommand);

		}

		#endregion

		#region CacheManager Methods

		private static void cacheStore(DbContentCatalog dbContentCatalog)
		{
			CacheManager cache = CacheFactory.GetCacheManager("DbContentCatalog");
			cache.Add(dbContentCatalog.iD.ToString(), dbContentCatalog);
		}

		private static DbContentCatalog cacheFind(int id)
		{
			object cachedObject;
			CacheManager cache = CacheFactory.GetCacheManager("DbContentCatalog");
			cachedObject = cache.GetData(id.ToString());
			if(cachedObject == null)
				return null;
			return (DbContentCatalog)cachedObject;
		}

		private static void cacheRemove(int id)
		{
			CacheManager cache = CacheFactory.GetCacheManager("DbContentCatalog");
			cache.Remove(id.ToString());
		}

		#endregion

	}
}

