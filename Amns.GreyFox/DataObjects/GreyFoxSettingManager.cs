/* ********************************************************** *
 * AMNS DbModel v1.0 OleDbManager Data Tier                   *
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
using System.Data.OleDb;
using System.Text;
using System.Web;
using System.Web.Caching;
using Amns.GreyFox.Data;
using Amns.GreyFox.Security;

namespace Amns.GreyFox
{
	#region Child Flags Enumeration

	public enum GreyFoxSettingFlags : int { ModifyRole};

	#endregion

	/// <summary>
	/// Datamanager for GreyFoxSetting objects.
	/// </summary>
	[ExposedManager("GreyFoxSetting", "", true, 1, 1, 6234)]
	public class GreyFoxSettingManager : IGreyFoxManager
	{
		#region Manager Fields

		// Static Fields
		static bool isInitialized;
		static string connectionString;

		// Private Fields
		string tableName = "sysGlobal_Settings";


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
			"GreyFoxSettingID", 
			"ModifyRoleID", 
			"Name", 
			"Value" 
		};

		#endregion

		#region Default DbModel Constructors

		static GreyFoxSettingManager()
		{
			connectionString = "";
		}

		public GreyFoxSettingManager()
		{
		}

		#endregion

		#region Default DbModel Constructors

		// Initialize
		public void Initialize(string connectionString)
		{
			if(!GreyFoxSettingManager.isInitialized)
			{
				GreyFoxSettingManager.isInitialized = true;
				GreyFoxSettingManager.connectionString = connectionString;
			}
			else
			{
				throw(new Exception("Manager has already been initialized."));
			}
		}
		#endregion

		#region Default DbModel Insert Method

		/// <summary>
		/// Inserts a GreyFoxSetting into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_GreyFoxSetting">The GreyFoxSetting to insert into the database.</param>
		internal static int _insert(GreyFoxSetting greyFoxSetting)
		{
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand();
			dbCommand.Connection = dbConnection;
			dbCommand.CommandText = "INSERT INTO sysGlobal_Settings (Name," +
				"Value," +
				"ModifyRoleID) VALUES (" +
				"inName," +
				"inValue," +
				"inModifyRoleID);";

			fillParameters(dbCommand, greyFoxSetting);

			dbConnection.Open();
			dbCommand.ExecuteNonQuery();
			dbCommand.CommandText = "SELECT @@IDENTITY AS IDVal";
			int id = (int) dbCommand.ExecuteScalar();

			dbConnection.Close();
			// Store greyFoxSetting in cache.
			if(cacheEnabled) cacheStore(greyFoxSetting);
			return id;
		}

		#endregion

		#region Default DbModel Update Method

		internal static int _update(GreyFoxSetting greyFoxSetting)
		{
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand();
			dbCommand.Connection = dbConnection;
			dbCommand.CommandText = "UPDATE sysGlobal_Settings SET Name=inName," +
				"Value=inValue," +
				"ModifyRoleID=inModifyRoleID WHERE GreyFoxSettingID=inGreyFoxSettingID";

			fillParameters(dbCommand, greyFoxSetting);

			dbCommand.Parameters.Add("inGreyFoxSettingID", OleDbType.Integer).Value = greyFoxSetting.iD;
			dbConnection.Open();

			// Abandon remaining updates if no rows have been updated by returning false immediately.
			if (dbCommand.ExecuteNonQuery() == 0) return -1;

			dbConnection.Close();

			// Store greyFoxSetting in cache.
			if (cacheEnabled) cacheStore(greyFoxSetting);

			return greyFoxSetting.iD;
		}

		#endregion

		#region Default DbModel Fill Parameters Method

		private static void fillParameters(OleDbCommand command, GreyFoxSetting greyFoxSetting)
		{
			#region General

			command.Parameters.Add("inName", OleDbType.VarChar).Value = greyFoxSetting.name;
			command.Parameters.Add("inValue", OleDbType.VarChar).Value = greyFoxSetting.value;
			if(greyFoxSetting.modifyRole == null)
				command.Parameters.Add("inModifyRoleID", OleDbType.Integer).Value = DBNull.Value;
			else
				command.Parameters.Add("inModifyRoleID", OleDbType.Integer).Value = greyFoxSetting.modifyRole.ID;
			#endregion

		}

		#endregion

		#region Default DbModel Fill Method

		internal static bool _fill(GreyFoxSetting greyFoxSetting)
		{
			// Clone item from cache.
			if(cacheEnabled)
			{
				GreyFoxSetting cachedGreyFoxSetting = cacheFind(greyFoxSetting.iD);
				if(cachedGreyFoxSetting != null)
				{
				cachedGreyFoxSetting.CopyTo(greyFoxSetting, true);
				return greyFoxSetting.isSynced;
				}
			}

			StringBuilder query = new StringBuilder("SELECT ");
			query.Append(string.Join(",", InnerJoinFields));
			query.Append(" FROM sysGlobal_Settings WHERE GreyFoxSettingID=");
			query.Append(greyFoxSetting.iD);
			query.Append(";");

			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query.ToString(), dbConnection);
			dbConnection.Open();
			OleDbDataReader r = dbCommand.ExecuteReader(CommandBehavior.SingleRow);

			if(!r.Read())
				throw(new Exception(string.Format("Cannot find GreyFoxSettingID '{0}'.", 
					greyFoxSetting.iD)));

			FillFromReader(greyFoxSetting, r, 0, 1);

			r.Close();
			dbConnection.Close();
			// Store greyFoxSetting in cache.
			if(cacheEnabled) cacheStore(greyFoxSetting);
			return true;
		}

		#endregion

		#region Default DbModel GetCollection Method

		public GreyFoxSettingCollection GetCollection(string whereClause, string sortClause, params GreyFoxSettingFlags[] optionFlags)
		{
			return GetCollection(0, whereClause, sortClause, optionFlags);
		}

		public GreyFoxSettingCollection GetCollection(int topCount, string whereClause, string sortClause, params GreyFoxSettingFlags[] optionFlags)
		{
			StringBuilder query = new StringBuilder("SELECT ");
			if(topCount > 0)
			{
				query.Append("TOP ");
				query.Append(topCount);
				query.Append(" ");
			}
			foreach(string columnName in InnerJoinFields)
			{
				query.Append("sysGlobal_Settings.");
				query.Append(columnName);
				query.Append(",");
			}

			int innerJoinOffset = InnerJoinFields.GetUpperBound(0) + 1;
			int modifyRoleOffset = -1;

			//
			// Append Option Flag Fields
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case GreyFoxSettingFlags.ModifyRole:
							for(int i = 0; i <= GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0); i++)
							{
								query.Append("sysGlobal_Roles.");
								query.Append(GreyFoxRoleManager.InnerJoinFields[i]);
								query.Append(",");
							}
							modifyRoleOffset = innerJoinOffset;
							innerJoinOffset = modifyRoleOffset + GreyFoxRoleManager.InnerJoinFields.GetUpperBound(0) + 1;
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

				query.Append("sysGlobal_Settings");
			}
			else
			{
				query.Append(" FROM sysGlobal_Settings ");
			}
			//
			// Finish INNER JOIN expressions
			//
			if(optionFlags != null)
				for(int x = 0; x < optionFlags.Length; x++)
				{
					switch(optionFlags[x])
					{
						case GreyFoxSettingFlags.ModifyRole:
							query.Append(" LEFT JOIN sysGlobal_Roles ON sysGlobal_Settings.ModifyRoleID = sysGlobal_Roles.GreyFoxRoleID)");
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
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query.ToString(), dbConnection);
			dbConnection.Open();

			#if DEBUG
			OleDbDataReader r;
			try
			{
				r = dbCommand.ExecuteReader();
			}
			catch (Exception e)
			{
				throw(new Exception(e.Message + " --- Query: " + query.ToString()));
			}
			#else
			OleDbDataReader r = dbCommand.ExecuteReader();
			#endif

			GreyFoxSettingCollection greyFoxSettingCollection = new GreyFoxSettingCollection();

			while(r.Read())
			{
				GreyFoxSetting greyFoxSetting = ParseFromReader(r, 0, 1);

				// Fill ModifyRole
				if(modifyRoleOffset != -1 && !r.IsDBNull(modifyRoleOffset))
					GreyFoxRoleManager.FillFromReader(greyFoxSetting.modifyRole, r, modifyRoleOffset, modifyRoleOffset+1);

				greyFoxSettingCollection.Add(greyFoxSetting);
			}

			r.Close();
			dbConnection.Close();

			return greyFoxSettingCollection;
		}

		#endregion

		#region Default DbModel ParseFromReader Method

		public static GreyFoxSetting ParseFromReader(OleDbDataReader r, int idOffset, int dataOffset)
		{
			GreyFoxSetting greyFoxSetting = new GreyFoxSetting();
			FillFromReader(greyFoxSetting, r, idOffset, dataOffset);
			return greyFoxSetting;
		}

		#endregion

		#region Default DbModel FillFromReader Method

		/// <summary>
		/// Fills the {0} from a OleDbDataReader.
		/// </summary>
		public static void FillFromReader(GreyFoxSetting greyFoxSetting, OleDbDataReader r, int idOffset, int dataOffset)
		{
			greyFoxSetting.iD = r.GetInt32(idOffset);
			greyFoxSetting.isSynced = true;
			greyFoxSetting.isPlaceHolder = false;

			//
			// Parse Children From Database
			//
			if(!r.IsDBNull(0+dataOffset) && r.GetInt32(0+dataOffset) > 0)
			{
				greyFoxSetting.modifyRole = GreyFoxRole.NewPlaceHolder(r.GetInt32(0+dataOffset));
			}

			//
			// Parse Fields From Database
			//
			greyFoxSetting.name = r.GetString(1+dataOffset);
			greyFoxSetting.value = r.GetString(2+dataOffset);

		}

		#endregion

		#region Default DbModel Fill Methods

		#endregion

		#region Default DbModel Delete Method

		internal static void _delete(int id)
		{
			StringBuilder s = new StringBuilder("DELETE * FROM sysGlobal_Settings WHERE GreyFoxSettingID=");
			s.Append(id);
			s.Append(';');

			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(s.ToString(), dbConnection);
			dbConnection.Open();
			dbCommand.ExecuteNonQuery();

			dbConnection.Close();

			cacheRemove(id);
		}

		#endregion

		#region Default DbModel Create Table Methods

		public static void CreateTableReferences()
		{
			StringBuilder query = new StringBuilder("ALTER TABLE sysGlobal_Settings ADD ");
			query.Append(" CONSTRAINT sysGlobal_Settings_ModifyRole_FK FOREIGN KEY (ModifyRoleID) REFERENCES sysGlobal_Roles(GreyFoxRoleID);");
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query.ToString(), dbConnection);
			dbConnection.Open();
			dbCommand.ExecuteNonQuery();

			dbConnection.Close();
		}

		public void CreateTable()
		{
			StringBuilder query = new StringBuilder("CREATE TABLE sysGlobal_Settings ");
			query.Append(" (GreyFoxSettingID COUNTER(1,1) CONSTRAINT GreyFoxSettingID PRIMARY KEY, " + 
				"ModifyRoleID LONG," +
				"Name TEXT(75), " +
				"Value TEXT(75));");

			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query.ToString(), dbConnection);
			dbConnection.Open();
			dbCommand.ExecuteNonQuery();

			dbConnection.Close();
		}

		#endregion

		#region Cache Methods

		static System.Web.Caching.Cache _webCache;

		public static void CatchWebCache()
		{
			if(_webCache == null && HttpContext.Current != null)
			{
				_webCache = HttpContext.Current.Cache;
			}
		}

		private static void cacheStore(GreyFoxSetting greyFoxSetting)
		{
			CatchWebCache();
			if(_webCache == null) return;
			_webCache.Insert("GFX_GreyFoxSetting_" + greyFoxSetting.ID.ToString(), greyFoxSetting.Copy(true), null, DateTime.MaxValue, TimeSpan.FromSeconds(1800), CacheItemPriority.Normal, null);
		}

		private static GreyFoxSetting cacheFind(int id)
		{
			CatchWebCache();
			if(_webCache == null) return null;
			return (GreyFoxSetting) _webCache["GFX_GreyFoxSetting_" + id.ToString()];
		}

		private static void cacheRemove(int id)
		{
			CatchWebCache();
			if(_webCache == null) return;
			_webCache.Remove("GFX_GreyFoxSetting_" + id.ToString());
		}

		#endregion

	}
}

