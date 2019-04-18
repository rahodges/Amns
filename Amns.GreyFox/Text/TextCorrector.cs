using System;

namespace Amns.GreyFox.Text
{
	/// <summary>
	/// Text dictionary is a performance enhanced dictionary that only uses arrays.
	/// </summary>
	public class TextCorrector
	{		
		string[,]	_dictionary;			
		int			_commonIndex			= 0;	// Index of the common text in the dictionary, usually 0
		int			_matchIndex				= 1;	// Index of the match text in the dictionary, usually 1
		int			_abbreviationIndex		= -1;
		int			_columnCount;
		bool		_matchAllColumns;			// The searcher will match text against all columns
		
		char[]		_breakChars				= new char[] {' ', ',', ';'};
		char[]		_afterCapChars			= new char[] {' ', '.'};
		bool		_caseSensitivityEnabled;

		TextScanDirection _scanDirection	= TextScanDirection.LeftToRight;
		TextReplacementMode _correctionMode = TextReplacementMode.Literal;

		public bool MatchAllColumns
		{
			get { return _matchAllColumns; }
			set { _matchAllColumns = value; }
		}

		public int AbbreviationIndex
		{
			get { return _abbreviationIndex; }
			set { _abbreviationIndex = value; }
		}

		public bool AbbreviationsEnabled
		{
			get { return _abbreviationIndex != -1; }
		}

		public bool CaseSensitivityEnabled
		{
			get { return _caseSensitivityEnabled; }
			set { _caseSensitivityEnabled = value; }
		}

		public TextScanDirection ScanDirection
		{
			get { return _scanDirection; }
			set { _scanDirection = value; }
		}

		public TextReplacementMode CorrectionMode
		{
			get { return _correctionMode; }
			set { _correctionMode = value; }
		}

		public TextCorrector(string[,] dictionary)
		{
			if(dictionary == null)
				throw(new Exception("Dictionary cannot be null."));
			if(dictionary.Rank < 2)
				throw(new Exception("Illegal dictionary; " +
					"dictionary must have at least 2 columns."));

			_dictionary = dictionary;

			// Enable abbreviations on dictionaries with greater than 2 columns
			_columnCount = dictionary.GetUpperBound(1) + 1;
		}

		public string AutoCorrect(string text)
		{   


			throw(new NotImplementedException("SpellCheck not implemented."));
		}

		public string WordCorrection(string text)
		{
			return WordCorrection(text, _commonIndex);
		}

		public string WordCorrection(string text, int replacementIndex)
		{
			string testText = text;

			if(replacementIndex > _dictionary.GetLength(0) - 1)
				throw(new Exception("Illegal replacement index; " +
					"dictonary does have an entry for the column specified."));

			if(!_caseSensitivityEnabled)
				testText = text.ToLower();

			if(_matchAllColumns)
			{
				for(int x = 0; x <= _dictionary.GetUpperBound(0); x++)
				{
					for(int y = 0; y <= _dictionary.GetUpperBound(1); y++)
					{
						// There is no need to skip the replacement index column since
						// it will kick back searches faster than it would if skipped!
						if(_caseSensitivityEnabled && _dictionary[x, y] == testText)
							return applyCorrectionMode(_dictionary[x, replacementIndex]);
						else if(_dictionary[x, y].ToLower() == testText)
							return applyCorrectionMode(_dictionary[x, replacementIndex]);
					}
				}
			}
			else
			{
				for(int x = 0; x < _dictionary.GetUpperBound(0); x++)
					if(_caseSensitivityEnabled && _dictionary[x, _matchIndex] == testText)
						return applyCorrectionMode(_dictionary[x, replacementIndex]);
					else if(_dictionary[x, _matchIndex].ToLower() == testText)
						return applyCorrectionMode(_dictionary[x, replacementIndex]);
			}

			return text;
		}

//		private string WordCorrection(int startIndex, int endIndex, string matchText)
//		{
//			int start = startIndex;
//			int end = endIndex;
//			int middle;
//
//			while(start <= end)
//			{
//				middle = (start + end) / 2;
//				if(_dictionary[middle, matchIndex] == matchText)
//					return _dictionary[middle];
//				if(_dictionary[middle, matchIndex] > matchText)
//					start = middle + 1;
//				else
//					end = middle - 1;
//			}
//
//			return null;
//		}

		private string applyCorrectionMode(string dictionaryText)
		{
			string correctionText;
			TextReplacementMode correctionMode = _correctionMode;
			
			// Interpret flags
			if(dictionaryText.StartsWith("\\u")) 
			{
				correctionMode = TextReplacementMode.UppercaseAll;
				correctionText = dictionaryText.Substring(2);
			}
			else if(dictionaryText.StartsWith("\\l")) 
			{
				correctionMode = TextReplacementMode.LowercaseAll;
				correctionText = dictionaryText.Substring(2);				
			}
			else if(dictionaryText.StartsWith("\\f"))
			{
				correctionMode = TextReplacementMode.CapitalizeFirst;
				correctionText = dictionaryText.Substring(2);
			}
			else if(dictionaryText.StartsWith("\\L"))
			{
				correctionMode = TextReplacementMode.Literal;
				correctionText = dictionaryText.Substring(2);
			}
			else
				correctionText = dictionaryText;

			// Return correction

			switch(correctionMode)
			{
				case TextReplacementMode.CapitalizeFirst:
				{
					char[] chars = correctionText.ToCharArray();

					// Convert first character to uppercase
					chars[0] = char.ToUpper(chars[0]);
					// Convert remaining characters to lowercase
					for(int x = 1; x <= chars.GetUpperBound(0); x++)
						chars[x] = char.ToLower(chars[x]);

					return new string(chars);
				}
				case TextReplacementMode.UppercaseAll:
				{
					return correctionText.ToUpper();
				}
				case TextReplacementMode.LowercaseAll:
				{
					return correctionText.ToLower();
				}
				default:
					return correctionText;
			}
		}
	}
}
