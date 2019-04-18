/* ********************************************************** *
 * AMNS Yari Importer for EndNote XML                         *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;
using System.Xml;
using Amns.GreyFox.People;

namespace Amns.GreyFox.Yari
{
	/// <summary>
	/// Summary description for YariImporter.
	/// </summary>
	public class YariImporter
	{
		private string connectionString;
		private int externalRecordCount;

		public int ExternalRecordCount
		{
			get
			{
				return externalRecordCount;
			}
		}

		public YariImporter(string connectionString)
		{
			this.connectionString = connectionString;
		}

		#region Import Events For Debug Monitoring

		public YariImporterEventHandler Start;
		private void OnStart(YariImporterEventArgs e)
		{
			if(Start != null)
				Start(this, e);
		}

		public YariImporterEventHandler RecordImported;
		public void OnRecordImported(YariImporterEventArgs e)
		{
			if(RecordImported != null)
				RecordImported(this, e);
		}

		public YariImporterEventHandler RecordSkipped;
		public void OnRecordSkipped(YariImporterEventArgs e)
		{
			if(RecordSkipped != null)
				RecordSkipped(this, e);
		}

		public YariImporterEventHandler Finish;
		private void OnFinish(YariImporterEventArgs e)
		{
			if(Finish != null)
				Finish(this, e);
		}

		//		public EventHandler AuthorImported;
		//		public void OnAuthorImported(EventArgs e)
		//		{
		//			if(AuthorImported != null)
		//				AuthorImported(this, e);
		//		}
		//
		//		public EventHandler PublisherImported;
		//		public void OnPublisherImported(EventArgs e)
		//		{
		//			if(PublisherImported != null)
		//				PublisherImported(this, e);
		//		}
		//
		//		public EventHandler KeywordImported;
		//		public void OnKeywordImported(EventArgs e)
		//		{
		//            if(KeywordImported  != null)
		//				KeywordImported(this, e);
		//		}

		#endregion

		public void ImportEndNoteXmlText(string xml)
		{
			XmlDataDocument xmlDocument = new XmlDataDocument();
			xmlDocument.LoadXml(xml);

			ImportEndNoteXml(xmlDocument);
		}

		public void ImportEndNoteXmlFile(string fileName)
		{
			XmlDataDocument xmlDocument = new XmlDataDocument();
			xmlDocument.Load(fileName);

			ImportEndNoteXml(xmlDocument);
		}

		public void ImportEndNoteXml(XmlDataDocument xmlDocument)
		{
			GreyFoxContactManager authorsManager = 
				new GreyFoxContactManager("kitYari_Authors");
			GreyFoxContactManager publishersManager =
				new GreyFoxContactManager("kitYari_Publishers");
			YariMediaRecordManager mediaRecordManager =
				new YariMediaRecordManager();
			YariMediaKeywordManager mediaKeywordManager =
				new YariMediaKeywordManager();
            
			XmlNodeList bookNodes = xmlDocument.SelectNodes("//XML/RECORDS/RECORD");
			XmlNode bookNode;
			externalRecordCount = bookNodes.Count;

			OnStart(new YariImporterEventArgs("Started Import.", 0));

			for(int bookIndex = 0; bookIndex < bookNodes.Count; bookIndex++)
			{
				bookNode = bookNodes[bookIndex];

				YariMediaRecord r = new YariMediaRecord();
				
				foreach(XmlNode childNode in bookNode.ChildNodes)
				{
					// check for null records which signify existing titles of
					// the same name.
					if(r == null)
						break;
                    
					switch(childNode.Name)
					{
						case "ISBN":
							if(childNode.InnerText.Length > 15)
								r.Isbn = childNode.InnerText.Substring(0, 15);
							else
								r.Isbn = childNode.InnerText;
							break;
						case "REFNUM":
							r.EndNoteReferenceID = int.Parse(childNode.InnerText);
//							if(mediaRecordManager.EndNoteReferenceIDExists(r.EndNoteReferenceID))
//							{
//								OnRecordSkipped(new YariImporterEventArgs(
//									"Record Skipped - '" + r.EndNoteReferenceID + "'; EndNote Reference ID already exists.", bookIndex));
//								r = null;
//							}							
							break;
						case "YEAR":
							try
							{
								r.PublishYear = int.Parse(childNode.InnerText);
							}
							catch
							{
								r.PublishYear = 0;
							}
							break;
						case "TITLE":
							r.Title = childNode.InnerText;
							if(mediaRecordManager.TitleExists(r.title))
							{
								OnRecordSkipped(new YariImporterEventArgs(
									"Record Skipped - '" + r.title + "'; title already exists.", bookIndex));
								r = null;
							}                            
							break;
						case "PUBLISHER":
							r.Publisher = childNode.InnerText;
							break;
						case "PAGES":
							try { r.Pages = int.Parse(childNode.InnerText);	}
							catch {}
							break;
						case "EDITION":
							r.Edition = childNode.InnerText;
							break;
						case "LABEL":
							r.Label = childNode.InnerText;
							break;
						case "KEYWORDS":
							r.keywords = new YariMediaKeywordCollection();
							foreach(XmlNode keywordNode in childNode.ChildNodes)
							{								
								string[] keywords = 
									keywordNode.InnerText.Split(new char[] {',', ';'});
								
								foreach(string keyword in keywords)
									r.Keywords.Add(mediaKeywordManager.FindByKeyword(keyword.Trim().ToLower(), true));
							}
							break;
						case "ABSTRACT":
							r.AbstractText = childNode.InnerText;
							break;
						case "NOTES":
							r.ContentsText = childNode.InnerText;
							break;
						case "AUTHORS":
							foreach(XmlNode authorNode in childNode.ChildNodes)
							{
								//
								// Split author fields in case the firstName is joined with
								// an ampersand or 'and' text.
								//
								string authorText = authorNode.InnerText.Replace(" & ", " and ");
								int andIndex = authorText.ToLower().IndexOf(" and ");
								if(andIndex != -1)
								{
									string authorA = 
										authorText.Substring(0, andIndex).Trim();
									string authorB = 
										authorText.Substring(andIndex + 5,
										authorText.Length - (authorA.Length + 6)).Trim();
								}

								r.Authors += authorText + " ";
							}
							break;
					}
				}
				
				// save the record if it does not exist.
				if(r != null)
				{
					r.AmazonRefreshDate = DateTime.MinValue;
					r.AmazonFillDate = DateTime.MinValue;
					r.AmazonReleaseDate = DateTime.MinValue;
					r.Save();
					OnRecordImported(
						new YariImporterEventArgs("Imported Record - '" + r.Title + "'.",
						bookIndex, r));
				}
			}			

			OnFinish(new YariImporterEventArgs("Finished import.", bookNodes.Count));
		}
	}

	public delegate void YariImporterEventHandler(object sender,
		YariImporterEventArgs e);

	public class YariImporterEventArgs : EventArgs
	{
		private readonly string message;
		private readonly int importIndex;
		private readonly YariMediaRecord sourceRecord;

		public YariImporterEventArgs(string message, int importIndex)
		{
			this.message = message;
			this.importIndex = importIndex;
			this.sourceRecord = null;
		}

		public YariImporterEventArgs(string message, int importIndex, 
			YariMediaRecord sourceRecord)
		{
			this.message = message;
			this.importIndex = importIndex;
			this.sourceRecord = sourceRecord;
		}

		public string Message
		{
			get
			{
				return message;
			}
		}

		public int ImportIndex
		{
			get
			{
				return importIndex + 1;
			}
		}

		public YariMediaRecord SourceRecord
		{
			get
			{
				return sourceRecord;
			}
		}
	}
}
