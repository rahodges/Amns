/* ********************************************************** *
 * AMNS DbModel v1.0 Class Object Business Tier               *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;

namespace Amns.GreyFox.Yari
{
	/// <summary>
	/// Summary of MyClass
	/// </summary>
	public class YariMediaRecord : ICloneable, IComparable
	{
		#region Private Fields

		internal int iD = 0;
		internal bool isPlaceHolder;		// Placeholders only store an ID; marked as unsynced.
		internal bool isSynced;				// Shows that data is synced with database.
		internal int endNoteReferenceID;
		internal int publishYear;
		internal string title;
		internal int pages;
		internal string edition;
		internal string isbn;
		internal string label;
		internal string abstractText;
		internal string contentsText;
		internal string notesText;
		internal DateTime amazonFillDate;
		internal DateTime amazonRefreshDate;
		internal string imageUrlSmall;
		internal string imageUrlMedium;
		internal string imageUrlLarge;
		internal decimal amazonListPrice;
		internal decimal amazonOurPrice;
		internal string amazonAvailability;
		internal string amazonMedia;
		internal DateTime amazonReleaseDate;
		internal string amazonAsin;
		internal bool abstractEnabled;
		internal bool contentsEnabled;
		internal bool notesEnabled;
		internal string authors;
		internal string secondaryAuthors;
		internal string publisher;
		internal YariMediaType mediaType;
		internal YariMediaKeywordCollection keywords;

		#endregion

		#region Public Properties

		/// <summary>
		/// YariMediaRecord Record ID, assigned by database. Readonly.
		/// </summary>
		public int ID
		{
			get
			{
				return iD;
			}
		}

		/// <summary>
		/// Identifies the YariMediaRecord as a Placeholder. Placeholders only contain 
		/// a YariMediaRecord ID. Record late-binds data when it is accessed.
		/// </summary>
		public bool IsPlaceHolder
		{
			get
			{
				return isPlaceHolder;
			}
		}

		/// <summary>
		/// True if the object is synced with the database.
		/// </summary>
		public bool IsSynced
		{
			get
			{
				return isSynced;
			}
			set
			{
				if(value == true)
				{
					throw (new Exception("Cannot set IsSynced to true."));
				}
				isSynced = value;
			}
		}

		/// <summary>
		/// </summary>
		public int EndNoteReferenceID
		{
			get
			{
				EnsurePreLoad();
				return endNoteReferenceID;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= endNoteReferenceID == value;
				endNoteReferenceID = value;
			}
		}

		/// <summary>
		/// Year published.
		/// </summary>
		public int PublishYear
		{
			get
			{
				EnsurePreLoad();
				return publishYear;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= publishYear == value;
				publishYear = value;
			}
		}

		/// <summary>
		/// Title of media.
		/// </summary>
		public string Title
		{
			get
			{
				EnsurePreLoad();
				return title;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= title == value;
				title = value;
			}
		}

		/// <summary>
		/// If the media is print media, the page count of the media.
		/// </summary>
		public int Pages
		{
			get
			{
				EnsurePreLoad();
				return pages;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= pages == value;
				pages = value;
			}
		}

		/// <summary>
		/// </summary>
		public string Edition
		{
			get
			{
				EnsurePreLoad();
				return edition;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= edition == value;
				edition = value;
			}
		}

		/// <summary>
		/// </summary>
		public string Isbn
		{
			get
			{
				EnsurePreLoad();
				return isbn;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= isbn == value;
				isbn = value;
			}
		}

		/// <summary>
		/// </summary>
		public string Label
		{
			get
			{
				EnsurePreLoad();
				return label;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= label == value;
				label = value;
			}
		}

		/// <summary>
		/// </summary>
		public string AbstractText
		{
			get
			{
				EnsurePreLoad();
				return abstractText;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= abstractText == value;
				abstractText = value;
			}
		}

		/// <summary>
		/// </summary>
		public string ContentsText
		{
			get
			{
				EnsurePreLoad();
				return contentsText;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= contentsText == value;
				contentsText = value;
			}
		}

		/// <summary>
		/// </summary>
		public string NotesText
		{
			get
			{
				EnsurePreLoad();
				return notesText;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= notesText == value;
				notesText = value;
			}
		}

		/// <summary>
		/// </summary>
		public DateTime AmazonFillDate
		{
			get
			{
				EnsurePreLoad();
				return amazonFillDate;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= amazonFillDate == value;
				amazonFillDate = value;
			}
		}

		/// <summary>
		/// </summary>
		public DateTime AmazonRefreshDate
		{
			get
			{
				EnsurePreLoad();
				return amazonRefreshDate;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= amazonRefreshDate == value;
				amazonRefreshDate = value;
			}
		}

		/// <summary>
		/// </summary>
		public string ImageUrlSmall
		{
			get
			{
				EnsurePreLoad();
				return imageUrlSmall;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= imageUrlSmall == value;
				imageUrlSmall = value;
			}
		}

		/// <summary>
		/// </summary>
		public string ImageUrlMedium
		{
			get
			{
				EnsurePreLoad();
				return imageUrlMedium;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= imageUrlMedium == value;
				imageUrlMedium = value;
			}
		}

		/// <summary>
		/// </summary>
		public string ImageUrlLarge
		{
			get
			{
				EnsurePreLoad();
				return imageUrlLarge;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= imageUrlLarge == value;
				imageUrlLarge = value;
			}
		}

		/// <summary>
		/// </summary>
		public decimal AmazonListPrice
		{
			get
			{
				EnsurePreLoad();
				return amazonListPrice;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= amazonListPrice == value;
				amazonListPrice = value;
			}
		}

		/// <summary>
		/// </summary>
		public decimal AmazonOurPrice
		{
			get
			{
				EnsurePreLoad();
				return amazonOurPrice;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= amazonOurPrice == value;
				amazonOurPrice = value;
			}
		}

		/// <summary>
		/// </summary>
		public string AmazonAvailability
		{
			get
			{
				EnsurePreLoad();
				return amazonAvailability;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= amazonAvailability == value;
				amazonAvailability = value;
			}
		}

		/// <summary>
		/// </summary>
		public string AmazonMedia
		{
			get
			{
				EnsurePreLoad();
				return amazonMedia;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= amazonMedia == value;
				amazonMedia = value;
			}
		}

		/// <summary>
		/// </summary>
		public DateTime AmazonReleaseDate
		{
			get
			{
				EnsurePreLoad();
				return amazonReleaseDate;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= amazonReleaseDate == value;
				amazonReleaseDate = value;
			}
		}

		/// <summary>
		/// </summary>
		public string AmazonAsin
		{
			get
			{
				EnsurePreLoad();
				return amazonAsin;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= amazonAsin == value;
				amazonAsin = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool AbstractEnabled
		{
			get
			{
				EnsurePreLoad();
				return abstractEnabled;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= abstractEnabled == value;
				abstractEnabled = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool ContentsEnabled
		{
			get
			{
				EnsurePreLoad();
				return contentsEnabled;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= contentsEnabled == value;
				contentsEnabled = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool NotesEnabled
		{
			get
			{
				EnsurePreLoad();
				return notesEnabled;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= notesEnabled == value;
				notesEnabled = value;
			}
		}

		/// <summary>
		/// Authors
		/// </summary>
		public string Authors
		{
			get
			{
				EnsurePreLoad();
				return authors;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= authors == value;
				authors = value;
			}
		}

		/// <summary>
		/// Secondary Authors
		/// </summary>
		public string SecondaryAuthors
		{
			get
			{
				EnsurePreLoad();
				return secondaryAuthors;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= secondaryAuthors == value;
				secondaryAuthors = value;
			}
		}

		/// <summary>
		/// Publisher
		/// </summary>
		public string Publisher
		{
			get
			{
				EnsurePreLoad();
				return publisher;
			}
			set
			{
				EnsurePreLoad();
				isSynced &= publisher == value;
				publisher = value;
			}
		}

		/// <summary>
		/// Type of reference this book is.
		/// </summary>
		public YariMediaType MediaType
		{
			get
			{
				EnsurePreLoad();
				return mediaType;
			}
			set
			{
				EnsurePreLoad();
				if(value == null)
				{
					if(mediaType == null)
					{
						return;
					}
					else
					{
						mediaType = value;
						isSynced = false;
					}
				}
				else
				{
					if(mediaType != null && value.ID == mediaType.ID)
					{
						return; 
					}
					else
					{
						mediaType = value;
						isSynced = false;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public YariMediaKeywordCollection Keywords
		{
			get
			{
				EnsurePreLoad();
				if(keywords == null)
				{
					YariMediaRecordManager.FillKeywords(this);
					keywords.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
				}
				return keywords;
			}
			set
			{
				EnsurePreLoad();
				if(!object.Equals(keywords, value))
				{
					keywords = value;
					keywords.CollectionChanged += new System.EventHandler(childrenCollection_Changed);
					isSynced = false;
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a new instance of YariMediaRecord.
		/// </summary>
		public YariMediaRecord()
		{
			title = string.Empty;
			edition = string.Empty;
			isbn = string.Empty;
			label = string.Empty;
			abstractText = string.Empty;
			contentsText = string.Empty;
			notesText = string.Empty;
			amazonMedia = string.Empty;
		}

		public YariMediaRecord(int id)
		{
			this.iD = id;
			isSynced = YariMediaRecordManager._fill(this);
		}
		#endregion

		#region Default DbModel Methods

		/// <summary>
		/// Ensures that the object's fields and children are 
		/// pre-loaded before any updates or reads.
		/// </summary>
		public void EnsurePreLoad()
		{
			if(!isPlaceHolder)
				return;

			YariMediaRecordManager._fill(this);
			isPlaceHolder = false;
		}

		/// <summary>
		/// Saves the YariMediaRecord object state to the database.
		/// </summary>
		public int Save()
		{
			if(mediaType != null)
				mediaType.Save();
			if(keywords != null)
				foreach(YariMediaKeyword item in keywords)
					item.Save();

			if(isSynced)
				return iD;

			if(iD == -1) throw (new Exception("Invalid record; cannot be saved."));
			if(iD == 0)
				iD = YariMediaRecordManager._insert(this);
			else
				YariMediaRecordManager._update(this);
			isSynced = iD != -1;
			return iD;
		}

		public void Delete()
		{
			YariMediaRecordManager._delete(this.iD);
			this.iD = 0;
			isSynced = false;
		}
		/// <summary>
		/// Duplicates YariMediaRecord object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new YariMediaRecord object reflecting the replicated YariMediaRecord object.</returns>
		public YariMediaRecord Duplicate()
		{
			YariMediaRecord clonedYariMediaRecord = this.Clone();

			// Insert must be called after children are replicated!
			clonedYariMediaRecord.iD = YariMediaRecordManager._insert(clonedYariMediaRecord);
			clonedYariMediaRecord.isSynced = true;
			return clonedYariMediaRecord;
		}

		/// <summary>
		/// Overwrites and existing YariMediaRecord object in the database.
		/// </summary>
		public void Overwrite(int id)
		{
			iD = id;
			YariMediaRecordManager._update(this);
			isSynced = true;
		}

		/// <summary>
		/// Clones YariMediaRecord object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new YariMediaRecord object reflecting the replicated YariMediaRecord object.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones YariMediaRecord object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new YariMediaRecord object reflecting the replicated YariMediaRecord object.</returns>
		public YariMediaRecord Clone()
		{
			YariMediaRecord clonedYariMediaRecord = new YariMediaRecord();
			clonedYariMediaRecord.iD = iD;
			clonedYariMediaRecord.isSynced = isSynced;
			clonedYariMediaRecord.endNoteReferenceID = endNoteReferenceID;
			clonedYariMediaRecord.publishYear = publishYear;
			clonedYariMediaRecord.title = title;
			clonedYariMediaRecord.pages = pages;
			clonedYariMediaRecord.edition = edition;
			clonedYariMediaRecord.isbn = isbn;
			clonedYariMediaRecord.label = label;
			clonedYariMediaRecord.abstractText = abstractText;
			clonedYariMediaRecord.contentsText = contentsText;
			clonedYariMediaRecord.notesText = notesText;
			clonedYariMediaRecord.amazonFillDate = amazonFillDate;
			clonedYariMediaRecord.amazonRefreshDate = amazonRefreshDate;
			clonedYariMediaRecord.imageUrlSmall = imageUrlSmall;
			clonedYariMediaRecord.imageUrlMedium = imageUrlMedium;
			clonedYariMediaRecord.imageUrlLarge = imageUrlLarge;
			clonedYariMediaRecord.amazonListPrice = amazonListPrice;
			clonedYariMediaRecord.amazonOurPrice = amazonOurPrice;
			clonedYariMediaRecord.amazonAvailability = amazonAvailability;
			clonedYariMediaRecord.amazonMedia = amazonMedia;
			clonedYariMediaRecord.amazonReleaseDate = amazonReleaseDate;
			clonedYariMediaRecord.amazonAsin = amazonAsin;
			clonedYariMediaRecord.abstractEnabled = abstractEnabled;
			clonedYariMediaRecord.contentsEnabled = contentsEnabled;
			clonedYariMediaRecord.notesEnabled = notesEnabled;
			clonedYariMediaRecord.authors = authors;
			clonedYariMediaRecord.secondaryAuthors = secondaryAuthors;
			clonedYariMediaRecord.publisher = publisher;


			if(mediaType != null)
				clonedYariMediaRecord.mediaType = mediaType;

			if(keywords != null)
				clonedYariMediaRecord.keywords = keywords.Clone();

			return clonedYariMediaRecord;
		}

		/// <summary>
		/// Makes a deep copy of the current YariMediaRecord.
		/// </summary>
		/// <returns> A new YariMediaRecord object reflecting the cloned YariMediaRecord object.</returns>
		public YariMediaRecord Copy()
		{
			YariMediaRecord yariMediaRecord = new YariMediaRecord();
			CopyTo(yariMediaRecord);
			return yariMediaRecord;
		}

		/// <summary>
		/// Makes a deep copy of the current YariMediaRecord.
		/// </summary>
		/// <returns> A new YariMediaRecord object reflecting the cloned YariMediaRecord object.</returns>
		/// <param name="isolation">Placeholders are used to isolate the YariMediaRecord from its children.</param>
		public YariMediaRecord Copy(bool isolation)
		{
			YariMediaRecord yariMediaRecord = new YariMediaRecord();
			CopyTo(yariMediaRecord, isolation);
			return yariMediaRecord;
		}

		/// <summary>
		/// Deep copies the current YariMediaRecord to another instance of YariMediaRecord.
		/// This method does not provide isolated copies; use overriden method for this feature.
		/// </summary>
		/// <param name="YariMediaRecord">The YariMediaRecord to copy to.</param>
		public void CopyTo(YariMediaRecord yariMediaRecord)
		{
			CopyTo(yariMediaRecord, false);
		}

		/// <summary>
		/// Deep copies the current YariMediaRecord to another instance of YariMediaRecord.
		/// </summary>
		/// <param name="YariMediaRecord">The YariMediaRecord to copy to.</param>
		/// <param name="isolation">Placeholders are used to isolate the YariMediaRecord from its children.</param>
		public void CopyTo(YariMediaRecord yariMediaRecord, bool isolation)
		{
			yariMediaRecord.iD = iD;
			yariMediaRecord.isPlaceHolder = isPlaceHolder;
			yariMediaRecord.isSynced = isSynced;
			yariMediaRecord.endNoteReferenceID = endNoteReferenceID;
			yariMediaRecord.publishYear = publishYear;
			yariMediaRecord.title = title;
			yariMediaRecord.pages = pages;
			yariMediaRecord.edition = edition;
			yariMediaRecord.isbn = isbn;
			yariMediaRecord.label = label;
			yariMediaRecord.abstractText = abstractText;
			yariMediaRecord.contentsText = contentsText;
			yariMediaRecord.notesText = notesText;
			yariMediaRecord.amazonFillDate = amazonFillDate;
			yariMediaRecord.amazonRefreshDate = amazonRefreshDate;
			yariMediaRecord.imageUrlSmall = imageUrlSmall;
			yariMediaRecord.imageUrlMedium = imageUrlMedium;
			yariMediaRecord.imageUrlLarge = imageUrlLarge;
			yariMediaRecord.amazonListPrice = amazonListPrice;
			yariMediaRecord.amazonOurPrice = amazonOurPrice;
			yariMediaRecord.amazonAvailability = amazonAvailability;
			yariMediaRecord.amazonMedia = amazonMedia;
			yariMediaRecord.amazonReleaseDate = amazonReleaseDate;
			yariMediaRecord.amazonAsin = amazonAsin;
			yariMediaRecord.abstractEnabled = abstractEnabled;
			yariMediaRecord.contentsEnabled = contentsEnabled;
			yariMediaRecord.notesEnabled = notesEnabled;
			yariMediaRecord.authors = authors;
			yariMediaRecord.secondaryAuthors = secondaryAuthors;
			yariMediaRecord.publisher = publisher;

			if(mediaType != null)
			{
				if(isolation)
				{
					yariMediaRecord.mediaType = mediaType.NewPlaceHolder();
				}
				else
				{
					yariMediaRecord.mediaType = mediaType.Copy(false);
				}
			}
			if(keywords != null)
			{
				if(isolation)
				{
					yariMediaRecord.keywords = keywords.Copy(true);
				}
				else
				{
					yariMediaRecord.keywords = keywords.Copy(false);
				}
			}
		}

		public YariMediaRecord NewPlaceHolder()
		{
			YariMediaRecord yariMediaRecord = new YariMediaRecord();
			yariMediaRecord.iD = iD;
			yariMediaRecord.isPlaceHolder = true;
			yariMediaRecord.isSynced = true;
			return yariMediaRecord;
		}

		public static YariMediaRecord NewPlaceHolder(int iD)
		{
			YariMediaRecord yariMediaRecord = new YariMediaRecord();
			yariMediaRecord.iD = iD;
			yariMediaRecord.isPlaceHolder = true;
			yariMediaRecord.isSynced = true;
			return yariMediaRecord;
		}

		private void childrenCollection_Changed(object sender, System.EventArgs e)
		{
			isSynced = false;
		}

		#endregion

		#region IComparable Methods

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		int IComparable.CompareTo(object obj)
		{
			YariMediaRecord yariMediaRecord = (YariMediaRecord) obj;
			return this.iD - yariMediaRecord.iD;
		}

		/// <summary>
		/// Compares the object's ID to another object's ID.
		/// </summary>
		public int CompareTo(YariMediaRecord yariMediaRecord)
		{
			return this.iD - yariMediaRecord.iD;
		}

		public override bool Equals(object yariMediaRecord)
		{
			if(yariMediaRecord == null)
				return false;

			return Equals((YariMediaRecord) yariMediaRecord);
		}

		public bool Equals(YariMediaRecord yariMediaRecord)
		{
			if(yariMediaRecord == null)
				return false;

			return this.iD == yariMediaRecord.iD;
		}

		public override int GetHashCode()
		{
			return iD.GetHashCode();
		}

		#endregion

		//--- Begin Custom Code ---
																																																																																																		
		public void SetKeywords(string keywordString)
		{
			if(keywords == null)
				keywords = new YariMediaKeywordCollection();
			else
				keywords.Clear();

			string[] keywordArray = keywordString.Split(';');

			YariMediaKeywordManager m = new YariMediaKeywordManager();

			for(int x = 0; x < keywordArray.Length; x++)
				keywords.Add(m.FindByKeyword(keywordArray[x], true));
		}
        
		//--- End Custom Code ---
	}
}
