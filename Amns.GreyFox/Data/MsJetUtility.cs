using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections.Specialized;
using JRO;

namespace Amns.GreyFox.Data
{
	/// <summary>
	/// Summary description for MsJetUtility.
	/// </summary>
	public class MsJetUtility
	{
		public MsJetUtility()
		{
			
		}

		~MsJetUtility()
		{

		}

        public static bool CreateDB(string filename)
        {
            ADOX.CatalogClass cat = new ADOX.CatalogClass();

            try
            {
                cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" +
                       "Data Source=" + filename + ";" +
                       "Jet OLEDB:Engine Type=5");
            }
            catch
            {
                return false;
            }

            cat = null;

            return true;
        }

		public static StringCollection GetTables(string connectionString)
		{
			StringCollection tables = new StringCollection();
            DataTable schemaTable = new DataTable("Tables");
		
            OleDbConnection con = new OleDbConnection(connectionString);
			con.Open();
            schemaTable = con.GetSchema("Tables", 
                new string[] { null, null, null, "TABLE" });
            con.Close();

            foreach (DataRow r in schemaTable.Rows)
            {
                tables.Add(r[2].ToString());
            }

			return tables;
		}

		public static bool CompactDB(string source)
		{
			try
			{
				JRO.JetEngine jro = new JRO.JetEngineClass();

				string destTemp = source.Replace(".mdb", "-temp.mdb");

				// Create connection strings
				string sourceConn = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" + source;
				string destConn = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" +	destTemp + ";Jet OLEDB.Engine Type=5";

				// Apply compact and repair
				jro.CompactDatabase(sourceConn, destConn);

				// Replace original source file
				FileInfo fi = new FileInfo(destTemp);
				fi.CopyTo(source, true);
				fi.Delete();

				return true;
			}
			catch(System.Runtime.InteropServices.COMException e)
			{
				if(e.ErrorCode == -2147467259)
					throw(new MSJetUtilityLockedException("Database compaction failed; database locked. '" + e.ErrorCode.ToString() + "'", e));
				else
					throw(e);
			}
		}

		public static bool CompactDB(string source, string destination)
		{
			try
			{
				JRO.JetEngine jro = new JRO.JetEngineClass();

				// Create connection strings
				string sourceConn = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" + source;
				string destConn = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" +	destination + ";Jet OLEDB.Engine Type=5";

				// Apply compact and repair
				jro.CompactDatabase(sourceConn, destConn);

				return true;
			}
			catch(System.Runtime.InteropServices.COMException e)
			{
				if(e.ErrorCode == -2147467259)
					throw(new MSJetUtilityLockedException("Database compaction failed; database locked. '" + e.ErrorCode.ToString() + "'", e));
				else
					throw e;
			}
		}
	}
}
