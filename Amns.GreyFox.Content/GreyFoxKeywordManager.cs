using System;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Datamanager for GreyFoxKeyword objects.
	/// </summary>
	public class GreyFoxKeywordManager
	{
		// Fields
		private string connectionString;

		public static readonly string[] InnerJoinFields = new string[] {
			"GreyFoxKeywordID", 
			"Keyword", 
			"Definition", 
			"Culture" 
		};

		#region Default DbModel Constructors

		public GreyFoxKeywordManager(string connectionString)
		{
			this.connectionString = connectionString;
		}

		#endregion

		#region Default DbModel Methods

		/// <summary>
		/// Inserts a GreyFoxKeyword into the database. All children should have been
		/// saved to the database before insertion. New children will not be
		/// related to this object in the database.
		/// </summary>
		/// <param name="_GreyFoxKeyword">The GreyFoxKeyword to insert into the database.</param>
		internal static int _insert(GreyFoxKeyword greyFoxKeyword)
		{
			OleDbConnection dbConnection = new OleDbConnection(greyFoxKeyword.connectionString);
			OleDbCommand dbCommand = new OleDbCommand();
			dbCommand.Connection = dbConnection;
			dbCommand.CommandText = "INSERT INTO kitCms_Keywords (Keyword,Definition,Culture) VALUES (@Keyword,@Definition,@Culture);";

			dbCommand.Parameters.Add("@Keyword", OleDbType.VarChar);
			dbCommand.Parameters["@Keyword"].Value = greyFoxKeyword.keyword;
			dbCommand.Parameters.Add("@Definition", OleDbType.VarChar);
			dbCommand.Parameters["@Definition"].Value = greyFoxKeyword.definition;
			dbCommand.Parameters.Add("@Culture", OleDbType.VarChar);
			dbCommand.Parameters["@Culture"].Value = greyFoxKeyword.culture;

			dbConnection.Open();
			dbCommand.ExecuteNonQuery();
			dbCommand.CommandText = "SELECT @@IDENTITY AS IDVal";
			int id = (int) dbCommand.ExecuteScalar();

			// Save child relationships for Synonyms.
			if(greyFoxKeyword.synonyms != null)
				foreach(GreyFoxKeyword item in greyFoxKeyword.synonyms)
				{
					dbCommand.Parameters.Clear();
					dbCommand.CommandText = "INSERT INTO kitCms_KeywordsChildren_Synonyms " +
						"(GreyFoxKeywordID, GreyFoxKeywordID)" + 
						" VALUES (@GreyFoxKeywordID, @GreyFoxKeywordID);";
					dbCommand.Parameters.Add("@GreyFoxKeywordID", OleDbType.Integer);
					dbCommand.Parameters["@GreyFoxKeywordID"].Value = id;
					dbCommand.Parameters.Add("@GreyFoxKeywordID", OleDbType.Integer);
					dbCommand.Parameters["@GreyFoxKeywordID"].Value = item.ID;
					dbCommand.ExecuteNonQuery();
				}

			// Save child relationships for Antonyms.
			if(greyFoxKeyword.antonyms != null)
				foreach(GreyFoxKeyword item in GreyFoxKeyword.antonyms)
				{
					dbCommand.Parameters.Clear();
					dbCommand.CommandText = "INSERT INTO kitCms_KeywordsChildren_Antonyms " +
						"(GreyFoxKeywordID, GreyFoxKeywordID)" + 
						" VALUES (@GreyFoxKeywordID, @GreyFoxKeywordID);";
					dbCommand.Parameters.Add("@GreyFoxKeywordID", OleDbType.Integer);
					dbCommand.Parameters["@GreyFoxKeywordID"].Value = id;
					dbCommand.Parameters.Add("@GreyFoxKeywordID", OleDbType.Integer);
					dbCommand.Parameters["@GreyFoxKeywordID"].Value = item.ID;
					dbCommand.ExecuteNonQuery();
				}

			// Save child relationships for References.
			if(greyFoxKeyword.references != null)
				foreach(GreyFoxKeyword item in GreyFoxKeyword.references)
				{
					dbCommand.Parameters.Clear();
					dbCommand.CommandText = "INSERT INTO kitCms_KeywordsChildren_References " +
						"(GreyFoxKeywordID, GreyFoxKeywordID)" + 
						" VALUES (@GreyFoxKeywordID, @GreyFoxKeywordID);";
					dbCommand.Parameters.Add("@GreyFoxKeywordID", OleDbType.Integer);
					dbCommand.Parameters["@GreyFoxKeywordID"].Value = id;
					dbCommand.Parameters.Add("@GreyFoxKeywordID", OleDbType.Integer);
					dbCommand.Parameters["@GreyFoxKeywordID"].Value = item.ID;
					dbCommand.ExecuteNonQuery();
				}

			dbConnection.Close();
			return id;
		}

		internal static void _update(GreyFoxKeyword GreyFoxKeyword)
		{
			OleDbConnection dbConnection = new OleDbConnection(greyFoxKeyword.connectionString);
			OleDbCommand dbCommand = new OleDbCommand();
			dbCommand.Connection = dbConnection;
			dbCommand.CommandText = "UPDATE kitCms_Keywords SET Keyword=@Keyword,Definition=@Definition,Culture=@Culture WHERE GreyFoxKeywordID=@GreyFoxKeywordID";

			dbCommand.Parameters.Add("@Keyword", OleDbType.VarChar);
			dbCommand.Parameters["@Keyword"].Value = GreyFoxKeyword.keyword;
			dbCommand.Parameters.Add("@Definition", OleDbType.VarChar);
			dbCommand.Parameters["@Definition"].Value = GreyFoxKeyword.definition;
			dbCommand.Parameters.Add("@Culture", OleDbType.VarChar);
			dbCommand.Parameters["@Culture"].Value = GreyFoxKeyword.culture;
			dbCommand.Parameters.Add("@GreyFoxKeywordID", OleDbType.Integer);
			dbCommand.Parameters["@GreyFoxKeywordID"].Value = GreyFoxKeyword.iD;
			dbConnection.Open();
			dbCommand.ExecuteNonQuery();

			if(greyFoxKeyword.synonyms != null)
			{

				// Delete child relationships for Synonyms.
				dbCommand.CommandText = "DELETE * FROM kitCms_KeywordsChildren_Synonyms WHERE GreyFoxKeywordID_1=@GreyFoxKeywordID_1;";
				dbCommand.Parameters.Clear();
				dbCommand.Parameters.Add("@GreyFoxKeywordID_1", OleDbType.Integer);
				dbCommand.Parameters["@GreyFoxKeywordID_1"].Value = GreyFoxKeyword.iD;
				dbCommand.ExecuteNonQuery();

				// Save child relationships for Synonyms.
				dbCommand.CommandText = "INSERT INTO kitCms_KeywordsChildren_Synonyms (GreyFoxKeywordID_1, GreyFoxKeywordID_2) VALUES (@GreyFoxKeywordID_1, @GreyFoxKeywordID_2);";
				dbCommand.Parameters.Add("@GreyFoxKeywordID_2", OleDbType.Integer);
				foreach(GreyFoxKeyword childGreyFoxKeyword in GreyFoxKeyword.synonyms)
				{
					dbCommand.Parameters["@GreyFoxKeywordID_2"].Value = childGreyFoxKeyword.ID;
					dbCommand.ExecuteNonQuery();
				}
			}

			if(greyFoxKeyword.antonyms != null)
			{

				// Delete child relationships for Antonyms.
				dbCommand.CommandText = "DELETE * FROM kitCms_KeywordsChildren_Antonyms WHERE GreyFoxKeywordID_1=@GreyFoxKeywordID_1;";
				dbCommand.Parameters.Clear();
				dbCommand.Parameters.Add("@GreyFoxKeywordID_1", OleDbType.Integer);
				dbCommand.Parameters["@GreyFoxKeywordID_1"].Value = GreyFoxKeyword.iD;
				dbCommand.ExecuteNonQuery();

				// Save child relationships for Antonyms.
				dbCommand.CommandText = "INSERT INTO kitCms_KeywordsChildren_Antonyms (GreyFoxKeywordID_1, GreyFoxKeywordID_2) VALUES (@GreyFoxKeywordID_1, @GreyFoxKeywordID_2);";
				dbCommand.Parameters.Add("@GreyFoxKeywordID_2", OleDbType.Integer);
				foreach(GreyFoxKeyword childGreyFoxKeyword in GreyFoxKeyword.antonyms)
				{
					dbCommand.Parameters["@GreyFoxKeywordID_2"].Value = childGreyFoxKeyword.ID;
					dbCommand.ExecuteNonQuery();
				}
			}

			if(greyFoxKeyword.references != null)
			{

				// Delete child relationships for References.
				dbCommand.CommandText = "DELETE * FROM kitCms_KeywordsChildren_References WHERE GreyFoxKeywordID_1=@GreyFoxKeywordID_1;";
				dbCommand.Parameters.Clear();
				dbCommand.Parameters.Add("@GreyFoxKeywordID_1", OleDbType.Integer);
				dbCommand.Parameters["@GreyFoxKeywordID_1"].Value = GreyFoxKeyword.iD;
				dbCommand.ExecuteNonQuery();

				// Save child relationships for References.
				dbCommand.CommandText = "INSERT INTO kitCms_KeywordsChildren_References (GreyFoxKeywordID_1, GreyFoxKeywordID_2) VALUES (@GreyFoxKeywordID_1, @GreyFoxKeywordID_2);";
				dbCommand.Parameters.Add("@GreyFoxKeywordID_2", OleDbType.Integer);
				foreach(GreyFoxKeyword childGreyFoxKeyword in GreyFoxKeyword.references)
				{
					dbCommand.Parameters["@GreyFoxKeywordID_2"].Value = childGreyFoxKeyword.ID;
					dbCommand.ExecuteNonQuery();
				}
			}

			dbConnection.Close();
		}

		internal static bool _fill(GreyFoxKeyword GreyFoxKeyword)
		{
			StringBuilder query = new StringBuilder("SELECT " +
				"Keyword, " +
				"Definition, " +
				"Culture " +
				" FROM kitCms_Keywords WHERE GreyFoxKeywordID=");
			query.Append(greyFoxKeyword.iD);
			query.Append(";");

			OleDbConnection dbConnection = new OleDbConnection(greyFoxKeyword.connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query.ToString(), dbConnection);
			dbConnection.Open();
			OleDbDataReader r = dbCommand.ExecuteReader();

			if(!r.Read())
				throw(new Exception(string.Format("Cannot find GreyFoxKeywordID '{0}'.", 
					greyFoxKeyword.iD)));

			greyFoxKeyword.keyword = r.GetString(0);

			greyFoxKeyword.definition = r.GetString(1);

			greyFoxKeyword.culture = r.GetString(2);

			r.Close();
			dbConnection.Close();
			return true;
		}

		public GreyFoxKeywordCollection GetCollection(string whereClause, string sortClause)
		{
			StringBuilder query = new StringBuilder("SELECT ");
			foreach(string columnName in InnerJoinFields)
			{
				query.Append("kitCms_Keywords.");
				query.Append(columnName);
				query.Append(",");
			}

			//
			// Remove trailing comma
			//
			query.Length--;
			query.Append(" FROM kitCms_Keywords ");
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
			OleDbDataReader r = dbCommand.ExecuteReader();
			GreyFoxKeywordCollection GreyFoxKeywordCollection = new GreyFoxKeywordCollection();
			while(r.Read())
			{
				GreyFoxKeyword GreyFoxKeyword = ParseFromReader(connectionString, r, 0, 1);

				greyFoxKeywordCollection.Add(greyFoxKeyword);
			}

			r.Close();
			dbConnection.Close();
			return GreyFoxKeywordCollection;
		}

		public static GreyFoxKeyword ParseFromReader(string connectionString, OleDbDataReader r, int idOffset, int dataOffset)
		{
			GreyFoxKeyword GreyFoxKeyword = new GreyFoxKeyword(connectionString);
			greyFoxKeyword.iD = r.GetInt32(idOffset);
			greyFoxKeyword.isSynced = true;
			greyFoxKeyword.isPlaceHolder = false;

			//
			// Parse Children From Database
			//

			//
			// Parse Fields From Database
			//
			greyFoxKeyword.keyword = r.GetString(0+dataOffset);

			greyFoxKeyword.definition = r.GetString(1+dataOffset);

			greyFoxKeyword.culture = r.GetString(2+dataOffset);


			return GreyFoxKeyword;
		}

		public static void FillFromReader(GreyFoxKeyword GreyFoxKeyword, string connectionString, OleDbDataReader r, int idOffset, int dataOffset)
		{
			greyFoxKeyword.connectionString = connectionString;
			greyFoxKeyword.iD = r.GetInt32(idOffset);
			greyFoxKeyword.isSynced = true;
			greyFoxKeyword.isPlaceHolder = false;

			//
			// Parse Children From Database
			//

			//
			// Parse Fields From Database
			//
			greyFoxKeyword.keyword = r.GetString(0+dataOffset);

			greyFoxKeyword.definition = r.GetString(1+dataOffset);

			greyFoxKeyword.culture = r.GetString(2+dataOffset);


		}

		internal static void FillSynonyms(GreyFoxKeyword _GreyFoxKeyword)
		{
			StringBuilder s = new StringBuilder("SELECT GreyFoxKeywordChildID FROM kitCms_KeywordsChildren_Synonyms ");
			s.Append("WHERE GreyFoxKeywordID=");
			s.Append(_GreyFoxKeyword.iD);
			s.Append(";");

			OleDbConnection dbConnection = new OleDbConnection(_GreyFoxKeyword.connectionString);
			OleDbCommand dbCommand = new OleDbCommand(s.ToString(), dbConnection);
			dbConnection.Open();
			OleDbDataReader r = dbCommand.ExecuteReader();

			GreyFoxKeywordCollection synonyms;
			if(_GreyFoxKeyword.synonyms != null)
			{
				synonyms = _GreyFoxKeyword.synonyms;
				synonyms.Clear();
			}
			else
			{
				synonyms = new GreyFoxKeywordCollection();
				_GreyFoxKeyword.synonyms = synonyms;
			}

			while(r.Read())
				synonyms.Add(GreyFoxKeyword.NewPlaceHolder(_GreyFoxKeyword.connectionString, r.GetInt32(0)));

			dbConnection.Close();
			_GreyFoxKeyword.synonyms = synonyms;
		}

		internal static void FillAntonyms(GreyFoxKeyword _GreyFoxKeyword)
		{
			StringBuilder s = new StringBuilder("SELECT GreyFoxKeywordChildID FROM kitCms_KeywordsChildren_Antonyms ");
			s.Append("WHERE GreyFoxKeywordID=");
			s.Append(_GreyFoxKeyword.iD);
			s.Append(";");

			OleDbConnection dbConnection = new OleDbConnection(_GreyFoxKeyword.connectionString);
			OleDbCommand dbCommand = new OleDbCommand(s.ToString(), dbConnection);
			dbConnection.Open();
			OleDbDataReader r = dbCommand.ExecuteReader();

			GreyFoxKeywordCollection antonyms;
			if(_GreyFoxKeyword.antonyms != null)
			{
				antonyms = _GreyFoxKeyword.antonyms;
				antonyms.Clear();
			}
			else
			{
				antonyms = new GreyFoxKeywordCollection();
				_GreyFoxKeyword.antonyms = antonyms;
			}

			while(r.Read())
				antonyms.Add(GreyFoxKeyword.NewPlaceHolder(_GreyFoxKeyword.connectionString, r.GetInt32(0)));

			dbConnection.Close();
			_GreyFoxKeyword.antonyms = antonyms;
		}

		internal static void FillReferences(GreyFoxKeyword _GreyFoxKeyword)
		{
			StringBuilder s = new StringBuilder("SELECT GreyFoxKeywordChildID FROM kitCms_KeywordsChildren_References ");
			s.Append("WHERE GreyFoxKeywordID=");
			s.Append(_GreyFoxKeyword.iD);
			s.Append(";");

			OleDbConnection dbConnection = new OleDbConnection(_GreyFoxKeyword.connectionString);
			OleDbCommand dbCommand = new OleDbCommand(s.ToString(), dbConnection);
			dbConnection.Open();
			OleDbDataReader r = dbCommand.ExecuteReader();

			GreyFoxKeywordCollection references;
			if(_GreyFoxKeyword.references != null)
			{
				references = _GreyFoxKeyword.references;
				references.Clear();
			}
			else
			{
				references = new GreyFoxKeywordCollection();
				_GreyFoxKeyword.references = references;
			}

			while(r.Read())
				references.Add(GreyFoxKeyword.NewPlaceHolder(_GreyFoxKeyword.connectionString, r.GetInt32(0)));

			dbConnection.Close();
			_GreyFoxKeyword.references = references;
		}

		internal static void _delete(string connectionString, int id)
		{
			StringBuilder s = new StringBuilder("DELETE * FROM kitCms_Keywords WHERE GreyFoxKeywordID=");
			s.Append(id);
			s.Append(';');

			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(s.ToString(), dbConnection);
			dbConnection.Open();
			dbCommand.ExecuteNonQuery();

			// Delete child relationships for Synonyms.
			s.Length = 0;
			s.Append("DELETE * FROM GreyFoxKeywordChildren_Synonyms WHERE ");
			s.Append("GreyFoxKeywordID=");
			s.Append(id);
			s.Append(";");
			dbCommand.CommandText = s.ToString();
			dbCommand.ExecuteNonQuery();

			// Delete child relationships for Antonyms.
			s.Length = 0;
			s.Append("DELETE * FROM GreyFoxKeywordChildren_Antonyms WHERE ");
			s.Append("GreyFoxKeywordID=");
			s.Append(id);
			s.Append(";");
			dbCommand.CommandText = s.ToString();
			dbCommand.ExecuteNonQuery();

			// Delete child relationships for References.
			s.Length = 0;
			s.Append("DELETE * FROM GreyFoxKeywordChildren_References WHERE ");
			s.Append("GreyFoxKeywordID=");
			s.Append(id);
			s.Append(";");
			dbCommand.CommandText = s.ToString();
			dbCommand.ExecuteNonQuery();

			dbConnection.Close();
		}

		public static void CreateTable(string connectionString)
		{
			StringBuilder query = new StringBuilder("CREATE TABLE kitCms_Keywords ");
			query.Append(" (GreyFoxKeywordID COUNTER(1,1) CONSTRAINT GreyFoxKeywordID PRIMARY KEY, " + 
				"Keyword TEXT(255), " +
				"Definition MEMO, " +
				"Culture TEXT(10));");

			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query.ToString(), dbConnection);
			dbConnection.Open();
			dbCommand.ExecuteNonQuery();

			//
			// Create children table for Synonyms.
			//
			query.Length = 0;
			query.Append("CREATE TABLE kitCms_KeywordsChildren_Synonyms ");
			query.Append("(GreyFoxKeywordID LONG, GreyFoxKeywordChildID LONG);");
			dbCommand.CommandText = query.ToString();
			dbCommand.ExecuteNonQuery();

			//
			// Create children table for Antonyms.
			//
			query.Length = 0;
			query.Append("CREATE TABLE kitCms_KeywordsChildren_Antonyms ");
			query.Append("(GreyFoxKeywordID LONG, GreyFoxKeywordChildID LONG);");
			dbCommand.CommandText = query.ToString();
			dbCommand.ExecuteNonQuery();

			//
			// Create children table for References.
			//
			query.Length = 0;
			query.Append("CREATE TABLE kitCms_KeywordsChildren_References ");
			query.Append("(GreyFoxKeywordID LONG, GreyFoxKeywordChildID LONG);");
			dbCommand.CommandText = query.ToString();
			dbCommand.ExecuteNonQuery();

			dbConnection.Close();
		}

		#endregion

		//--- Begin Custom Code ---
														
		public int GetKeywordID(string keyword)
		{
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
            OleDbCommand dbCommand = new OleDbCommand("SELECT GreyFoxKeywordID FROM kitCms_Keywords WHERE kitCms_Keywords.Keyword='" +
				keyword + "';", dbConnection);
			dbConnection.Open();
			object iD = dbCommand.ExecuteScalar();
			dbConnection.Close();
			if(iD == null)
				throw(new Exception("Keyword does not exist."));
			else
				return (int) iD;
		}

		//--- End Custom Code ---
	}
}

