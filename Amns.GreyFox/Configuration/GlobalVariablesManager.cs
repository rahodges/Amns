using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using Amns.GreyFox.DataAccessLayer;

namespace Amns.GreyFox.Configuration
{
	/// <summary>
	/// Summary description for RuntimeVariables.
	/// </summary>
	public class GlobalVariablesManager : Amns.GreyFox.TableKit
	{
		public GlobalVariablesManager(string connectionString)
		{
			dbConnectionString = connectionString;
			dbTableName = "sysGlobal_Variables";
		}

		public void ApplySqlFilter(GlobalVariable variable)
		{
            variable.Name = SqlFilter.FilterString(variable.Name, 25);
			variable.Value = SqlFilter.FilterString(variable.Value, 255);
		}

		public int Insert(GlobalVariable variable, bool ApplyFilter)
		{
			if(ApplyFilter)
				ApplySqlFilter(variable);

			StringBuilder s = new StringBuilder();
			s.AppendFormat("INSERT INTO {0} ", dbTableName);
			s.Append("(VariableName, VariableValue) VALUES ");
			s.AppendFormat("('{0}', '{1}');", variable.Name, variable.Value);

			return runOleDbIdentityCommand(s.ToString());
		}

		public int Update(GlobalVariable variable, bool ApplyFilter)
		{
			if(ApplyFilter)
				ApplySqlFilter(variable);

			StringBuilder s = new StringBuilder();
			s.AppendFormat("UPDATE {0} ", dbTableName);
			s.AppendFormat("SET VariableValue='{0}' WHERE VariableName='{1}';",
				variable.Value, variable.Name);

			return runOleDbCommand(s.ToString());
		}

		private GlobalVariable parse(OleDbDataReader r)
		{
			GlobalVariable g = new GlobalVariable();

			g.Name = r.GetString(0);
			g.Value = r.GetString(1);

			return g;
		}

		public GlobalVariable Get(string Name)
		{
			StringBuilder s = new StringBuilder("SELECT VariableName, VariableValue FROM ");
			s.AppendFormat(dbTableName);
			s.AppendFormat(" WHERE VariableName='");
			s.Append(Name);
			s.Append("';");

			OleDbDataReader r = runReader(s.ToString());
			
			if(!r.Read())
			{
				r.Close();
				throw(new Exception(string.Format("Specified global variable '{0}' does not exist.", Name)));
			}

            GlobalVariable g = parse(r);
			r.Close();
			return g;
		}
	}
}