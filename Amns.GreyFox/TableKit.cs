using System;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Amns.GreyFox
{
	public interface ITableKit
	{
		int CreateTable();
	}

	/// <summary>
	/// Summary description for Table.
	/// </summary>
	public class TableKit
	{
		protected string dbConnectionString;
		protected string dbTableName;

//		private string parseAccessType(OleDbType dbType)
//		{
//			switch(dbType)
//			{
//				case OleDbType.Boolean:
//					return "BIT";
//				case OleDbType.VarChar:
//					return "VARCHAR";
//				case OleDbType.Date:
//					return "DATETIME";
//				case OleDbType.Integer:
//					return "LONG";
//			}
//		}
//
//		protected string createInsertProcedure(OleDbCommand dbCommand, string tableName)
//		{
//			if(dbCommand.CommandType != CommandType.StoredProcedure)
//				throw(new InvalidOperationException("Cannot create a procedure for CommandTypes other than StoredProcedures."));
//
//			StringBuilder query = new StringBuilder();
//			query.AppendFormat("CREATE PROC {0} (", dbCommand.CommandText);
//            
//			OleDbParameterCollection parameters = dbCommand.Parameters;
//			for(int parameterCount = 0; parameterCount < parameters.Count; parameterCount++)
//			{
//                query.AppendFormat("in{0} {1}, ", 
//					parameters[x].ParameterName,
//					parseAccessType(parameters[x].OleDbType));
//			}
//
//			query.AppendFormat("in{0} {1}", 
//				parameters[parameters.Count].ParameterName,
//				parseAccessType(parameters[parameters.Count].OleDbType));
//
//			query.AppendFormat(") AS INSERT INTO {0} (", tableName);
//
//
//
//		}

		protected OleDbDataReader runReader(string query)
		{
			return runReader(query, CommandType.Text);
		}

		protected OleDbDataReader runReader(string query, CommandType commandType)
		{
			OleDbConnection dbConnection = new OleDbConnection(dbConnectionString);
			OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
			dbCommand.CommandType = commandType;

			dbConnection.Open();
			return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
		}

		protected object runOleDbScalar(string query, CommandType commandType)
		{
			object result;
			OleDbConnection dbConnection = new OleDbConnection(dbConnectionString);
			OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
			dbCommand.CommandType = commandType;

			dbConnection.Open();
			result = dbCommand.ExecuteScalar();
			dbConnection.Close();

			return result;			
		}

		protected int runOleDbCommand(string query)
		{
			return runOleDbCommand(new OleDbCommand(query));
		}

		protected int runOleDbCommand(string queryFormat, params object[] args)
		{
			return runOleDbCommand(new OleDbCommand(string.Format(queryFormat, args)));
		}

		protected int runOleDbIdentityCommand(string query)
		{
			return runOleDbIdentityCommand(new OleDbCommand(query));
		}

		protected int runOleDbIdentityCommand(OleDbCommand dbCommand)
		{
			OleDbConnection dbConnection = new OleDbConnection(dbConnectionString);
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

		protected int runOleDbCommand(OleDbCommand dbCommand)
		{
			OleDbConnection dbConnection = new OleDbConnection(dbConnectionString);
			dbCommand.Connection = dbConnection;

			dbConnection.Open();

			try
			{
				return dbCommand.ExecuteNonQuery();
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

		protected void runOleDbTransaction(string[] commandTextArray)
		{
			OleDbConnection dbConnection = new OleDbConnection(dbConnectionString);
			OleDbCommand dbCommand = new OleDbCommand();
			dbCommand.Connection = dbConnection;

			dbConnection.Open();
			OleDbTransaction dbTransaction = dbConnection.BeginTransaction();
			dbCommand.Transaction = dbTransaction;

			string errorQuery = "";

			try
			{
				foreach(string query in commandTextArray)
				{
					dbCommand.CommandText = query;
					errorQuery = query;
					dbCommand.ExecuteNonQuery();					
				}
				dbTransaction.Commit();
			}
			catch(OleDbException e)
			{
				dbTransaction.Rollback();
				throw(new Exception(string.Format("{0} - QUERY: {1}", e.Message, errorQuery)));
			}
			finally
			{
				dbConnection.Close();
			}
		}
	}
}