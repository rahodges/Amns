using System;

namespace Amns.GreyFox.Content
{
	/// <summary>
	/// Keyword
	/// </summary>
	public class GreyFoxKeyword : ICloneable
	{
		// Fields
		internal int iD = 0;
		internal bool isPlaceHolder;		// Identifies objects with only filled IDs
		internal bool isSynced;			// Identifies if the object is currently synced to the loaded object
		internal string connectionString;		// Holds the connectionString for the object
		internal string keyword;
		internal string definition;
		internal string culture;
		internal GreyFoxKeywordCollection synonyms;
		internal GreyFoxKeywordCollection antonyms;
		internal GreyFoxKeywordCollection references;

		// Public Properties

		#region Default DbModel Public Properties

		public int ID
		{
			get
			{
				return iD;
			}
		}

		public bool IsPlaceHolder
		{
			get
			{
				return isPlaceHolder;
			}
		}

		public string Keyword
		{
			get
			{
				EnsurePreLoad();
				return keyword;
			}
			set
			{
				EnsurePreLoad();
				isSynced = false;
				keyword = value;
			}
		}

		public string Definition
		{
			get
			{
				EnsurePreLoad();
				return definition;
			}
			set
			{
				EnsurePreLoad();
				isSynced = false;
				definition = value;
			}
		}

		public string Culture
		{
			get
			{
				EnsurePreLoad();
				return culture;
			}
			set
			{
				EnsurePreLoad();
				isSynced = false;
				culture = value;
			}
		}

		public GreyFoxKeywordCollection Synonyms
		{
			get
			{
				if(synonyms == null)
					GreyFoxKeywordManager.FillSynonyms(this);
				return synonyms;
			}
			set
			{
				synonyms = value;
			}
		}

		public GreyFoxKeywordCollection Antonyms
		{
			get
			{
				if(antonyms == null)
					GreyFoxKeywordManager.FillAntonyms(this);
				return antonyms;
			}
			set
			{
				antonyms = value;
			}
		}

		public GreyFoxKeywordCollection References
		{
			get
			{
				if(references == null)
					GreyFoxKeywordManager.FillReferences(this);
				return references;
			}
			set
			{
				references = value;
			}
		}


		#endregion


		#region Constructors

		public GreyFoxKeyword()
		{
		}

		public GreyFoxKeyword(string connectionString)
		{
			this.connectionString = connectionString;
			keyword = string.Empty;
			definition = string.Empty;
			culture = string.Empty;
		}

		public GreyFoxKeyword(string connectionString, int id)
		{
			this.iD = id;
			this.connectionString = connectionString;
			isSynced = GreyFoxKeywordManager._fill(this);
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

			GreyFoxKeywordManager._fill(this);
			isPlaceHolder = false;
		}

		public void Load()
		{
			if(isPlaceHolder | !isSynced)
			{
				isSynced = Amns.GreyFoxKeywordManager._fill(this);
				isPlaceHolder = false;
			}
		}

		public int Save()
		{
			if(synonyms != null)
				foreach(GreyFoxKeyword item in synonyms)
					item.Save();
			if(antonyms != null)
				foreach(GreyFoxKeyword item in antonyms)
					item.Save();
			if(references != null)
				foreach(GreyFoxKeyword item in references)
					item.Save();

			if(isSynced)
				return iD;

			if(iD == 0)
				iD = Amns.GreyFoxKeywordManager._insert(this);
			else
				GreyFoxKeywordManager._update(this);
			isSynced = true;
			return iD;
		}

		/// <summary>
		/// Replicates Amns.GreyFoxKeyword object into a database; may or may not be the same database
		/// as the parent object.
		/// </summary>
		/// <returns> A new Amns.GreyFoxKeyword object reflecting the replicated Amns.GreyFoxKeyword object.</returns>
		public GreyFoxKeyword Replicate(string connectionString)
		{
			GreyFoxKeyword clonedGreyFoxKeyword = this.Clone();
			clonedGreyFoxKeyword.connectionString = connectionString;

			// Insert must be called after children are replicated!
			clonedGreyFoxKeyword.iD = Amns.GreyFoxKeywordManager._insert(clonedGreyFoxKeyword);
			clonedGreyFoxKeyword.isSynced = true;
			return clonedGreyFoxKeyword;
		}

		public void Overwrite(string connectionString, int id)
		{
			iD = id;
			this.connectionString = connectionString;
			GreyFoxKeywordManager._update(this);
			isSynced = true;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Clones Amns.GreyFoxKeyword object and clones child objects with cloning or replication.
		/// as the parent object.
		/// </summary>
		/// <returns> A new Amns.GreyFoxKeyword object reflecting the replicated Amns.GreyFoxKeyword object.</returns>
		public GreyFoxKeyword Clone()
		{
			GreyFoxKeyword clonedGreyFoxKeyword = new Amns.GreyFoxKeyword();
			clonedGreyFoxKeyword.iD = iD;
			clonedGreyFoxKeyword.connectionString = connectionString;
			clonedGreyFoxKeyword.isSynced = isSynced;
			clonedGreyFoxKeyword.keyword = keyword;
			clonedGreyFoxKeyword.definition = definition;
			clonedGreyFoxKeyword.culture = culture;

			if(synonyms != null)
				clonedGreyFoxKeyword.synonyms = synonyms.Clone();

			if(antonyms != null)
				clonedGreyFoxKeyword.antonyms = antonyms.Clone();

			if(references != null)
				clonedGreyFoxKeyword.references = references.Clone();

			return clonedGreyFoxKeyword;
		}

		public void Delete()
		{
			GreyFoxKeywordManager._delete(this.connectionString, this.iD);
		}
		public static GreyFoxKeyword NewPlaceHolder(string connectionString, int iD)
		{
			GreyFoxKeyword tempGreyFoxKeyword = new Amns.GreyFoxKeyword();
			tempGreyFoxKeyword.iD = iD;
			tempGreyFoxKeyword.connectionString = connectionString;
			tempGreyFoxKeyword.isPlaceHolder = true;
			tempGreyFoxKeyword.isSynced = true;
			return tempGreyFoxKeyword;
		}


		#endregion

	}
}
