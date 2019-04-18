using System;

namespace Amns.GreyFox.Data
{
	/// <summary>
	/// Summary description for GreyFoxManager.
	/// </summary>
	public interface IGreyFoxManager
	{
		string TableName {get;}

		/// <summary>
		/// Initializes the default connection string for the manager.
		/// </summary>
		/// <param name="connectionString">Connection String for database.</param>
		void Initialize(string connectionString);

		/// <summary>
		/// Creates the table which the manager is associated with.
		/// </summary>
		void CreateTable();

        string VerifyTable(bool repair);
	}
}