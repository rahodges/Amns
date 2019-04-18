using System;
using System.Data;
using System.Data.OleDb;

namespace Amns.GreyFox.DataAccessLayer
{
	/// <summary>
	/// Summary description for DbConnector.
	/// </summary>
	public class DataLayer
	{
		public static int RunOleDbCommand(string connectionString, string query)
		{
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
			dbConnection.Open();
			int rowsAffected = dbCommand.ExecuteNonQuery();
			dbConnection.Close();
			return rowsAffected;
		}

		public static OleDbDataReader RunOleDbReader(string connectionString, string query)
		{
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
			dbConnection.Open();
			return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public static int RunOleDbIdentityCommand(string connectionString, string query)
		{
			return RunOleDbIdentityCommand(connectionString, new OleDbCommand(query));
		}

		public static int RunOleDbIdentityCommand(string connectionString, OleDbCommand dbCommand)
		{
			OleDbConnection dbConnection = new OleDbConnection(connectionString);
			dbCommand.Connection = dbConnection;

			dbConnection.Open();

			try
			{
				dbCommand.ExecuteNonQuery();
				dbCommand.CommandText = "SELECT @@IDENTITY AS IDVal";
				dbCommand.CommandType = CommandType.Text;
				int lastID = (int) dbCommand.ExecuteScalar();
				return lastID;
			}
			catch (OleDbException e)
			{				
				throw(e);
			}
			finally
			{
				dbConnection.Close();
			}
		}
	}
}
