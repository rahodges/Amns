using System;
using System.Data.OleDb;
using System.Text;
using Amns.GreyFox;

namespace Amns.GreyFox.Content
{
	public class DbContentLog : TableKit
	{
		public DbContentLog (string connectionString, string tableName)
		{
			dbConnectionString = connectionString;
			dbTableName = tableName;
		}

		#region Insert Events

		public int InsertGenericEvent(DateTime entryDate, ContentLogEntryType entryType, int catalogID)
		{
			StringBuilder s = new StringBuilder("INSERT INTO ");
			s.Append(dbTableName);
			s.Append(" (EntryDate, EntryType, OrigClipID, ClientUserID, Message ) VALUES (");
			s.AppendFormat("'{0}',", entryDate);
			s.AppendFormat("{0},", (int) entryType);
			s.AppendFormat("0,0,");
			s.Append("'");
			s.AppendFormat(DbContentLog.LogEntries[(int) entryType,2], catalogID);
			s.Append("');");

			return runOleDbCommand(s.ToString());
		}

		private int InsertClipEvent(DateTime entryDate, ContentLogEntryType entryType, int clipID, int clientUserID)
		{
			StringBuilder s = new StringBuilder("INSERT INTO ");
			s.Append(dbTableName);
			s.Append(" (EntryDate, EntryType, OrigClipID, ClientUserID, Message ) VALUES (");
			s.AppendFormat("'{0}',", entryDate);
			s.AppendFormat("{0},", (int) entryType);
			s.AppendFormat("{0},", clipID);
			s.AppendFormat("{0},",clientUserID);

			s.Append("'");
			s.AppendFormat(DbContentLog.LogEntries[(int) entryType,2], clipID,  clientUserID);
			s.Append("');");

			return runOleDbCommand(s.ToString());
		}

		private int InsertClipHit(DateTime entryDate, ContentLogEntryType entryType, int clipID, int parentClipID, int clientUserID, string clientData)
		{
			StringBuilder s = new StringBuilder("INSERT INTO ");
			s.Append(dbTableName);
			s.Append(" (EntryDate, EntryType, OrigClipID, ParentClipID, ClientUserID, Message ) VALUES (");
			s.AppendFormat("'{0}',", entryDate);
			s.AppendFormat("{0},", (int) entryType);
			s.AppendFormat("{0},", clipID);
			s.AppendFormat("{0},", parentClipID);
			s.AppendFormat("{0},", clientUserID);

			s.Append("'");
			s.AppendFormat(DbContentLog.LogEntries[(int) entryType,2], clipID,  clientUserID, parentClipID, clientData);
			s.Append("');");

			return runOleDbCommand(s.ToString());
		}

		private int InsertTcsEvent(DateTime entryDate, ContentLogEntryType entryType, int clipID, int parentClipID, int clientUserID)
		{
			throw(new NotImplementedException("TCS Event Logging is not implemented in this version."));
			
			//			StringBuilder s = new StringBuilder("INSERT INTO ");
			//			s.Append(dbTableName);
			//			s.Append(" (EntryDate, EntryType, OrigClipID, ParentClipID, ClientUserID, Message ) VALUES (");
			//			s.AppendFormat("'{0}',", entryDate);
			//			s.AppendFormat("{0},", (int) entryType);
			//			s.AppendFormat("{0},", clipID);
			//            s.AppendFormat("{0},",parentClipID);
			//			s.AppendFormat("{0},",clientUserID);
			//
			//			s.Append("'");
			//			s.AppendFormat(DbContentLog.LogEntries[(int) entryType,2], clipID,  clientUserID);
			//			s.Append("');");
			//
			//			return runOleDbCommand(s.ToString());
		}

		#endregion

		public static readonly string[,] LogEntries = new string[,]
{
{"100", "Clip Created", "Clip ''{0}'' created by ''{1}''."},
{"101", "Clip Updated", "Clip ''{0}'' updated by ''{1}''."},
{"102", "Clip Deleted", "Clip ''{0}'' deleted by ''{1}''."},
{"120", "Clip Opened for Editing", "Clip ''{0}'' is opened for editing by ''{1}''."},
{"121", "Clip Updated by Editor", "Clip ''{0}'' updaded by ''{1}."},
{"122", "Clip Rejected by Editor", "Clip ''{0}'' has been rejected by editor ''{1}''."},
{"123", "Clip Approved by Editor", "Clip ''{0}'' has been approved by editor ''{1}''."},
{"130", "Clip Opened for Review", "Clip ''{0}'' has been opened for review by ''{1}''."},
{"132", "Clip Rejected by Reviewer", "Clip ''{0}'' rejected by reviewer ''{1}''."},
{"133", "Clip Approved by Reviewer", "Clip ''{0}'' approved by reviewer ''{1}''."},
{"140", "Clip Published by Publisher", "Clip ''{0}'' published by ''{1}''."},
{"142", "Clip Rejected by Publisher", "Clip ''{0}'' rejected by ''{1}''."},
                                                                        
{"200", "Clip Hit", "Clip ''{0}'' hit by ''{1}'':<br>Client Data:<blockquote>{3}</blockquote>"},
{"202", "Clip Not Found", "Clip ''{0}'' does not exist."},
{"210", "Child Clip Hit", "Clip ''{0}'' hit by parent clip ''{2}''."},
{"212", "Child Clip Not Found", "Child Clip ''{0}'' not found by parent clip ''{2}''."},
{"220", "Clip Expired XRef Lookup", "XRef lookup for clip ''{0}'' by ''{1}''."},
{"222", "Clip Expired XRef Lookup Not Found", "XRef lookup for clip ''{0}'' by ''{1}'' reported not found."},
{"223", "Clip Expired XRef Lookup Success", "XRef lookup for clip ''{0}'' by ''{1}'' success, forwarding to clip ''{2}''."},
			
{"240", "TCS Event Started", "Tracked Changes Service Started in mode ''{2}'' for clip ''{0}'' by ''{1}''."},
{"241", "TCS Archived Clip", "Tracked Changes Service archived clip ''{0}'' to ''{1}''."},
{"242", "TCS Replaced Occurences", "Tracked Changes Service replaced all occurences for ''{0}'' with ''{1}''; {2} occurrences replaced."},
{"243", "TCS Created InterCatalog XRef", "Tracked Changes Service created XRef Redirection pointer for clip ''{0}'' pointing to clip ''{1}''."},
{"244", "TCS Created XRef Expiration Pointer", "Tracked Changes Service created XRef Expiration pointer for clip ''{0}'' pointing to clip ''{1}''."},
{"245", "TCS Created XRef Replacement Pointer", "Tracked Changes Service created XRef Redirection pointer for clip ''{0}'' pointing to clip ''{1}''."},
{"248", "TCS Event Complete", "Tracked Changes Service Complete for clip ''{0}''."},
{"249", "TCS Event Error", "Tracked Changes Service reported the following error, the service has been stopped for clip ''{0}''."},

{"50", "Created CMS Catalog", "GreyFox CMS has successfully created catalog ''{0}''."},			
{"51", "Initialized Catalog", "Catalog ''{0}'' initialized."},
{"55", "Catalog Service Started", "Catalog ''{0}'' service has been started and is ready to take requests."},
{"56", "Catalog Service Failed", "Catalog ''{0}'' reported a failure outside of known faults."},
{"57", "Catalog Service Database Connection Failure", "Catalog Service cannot connect to database."},		// Unreportable
{"59", "Catalog Service Datastore Failure", "Catalog Service cannot find the datastore for catalog ''{0}''."},
{"62", "Catalog Service Stopped", "Catalog ''{0}'' service has been stopped."},
{"63", "Catalog Service Reconfigured", "Catalog ''{0}'' has been updated with new settings."}
};
	}
}