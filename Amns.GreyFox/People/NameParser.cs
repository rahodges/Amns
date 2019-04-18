using System;

namespace Amns.GreyFox.People
{
	/// <summary>
	/// Summary description for NameParser.
	/// </summary>
	public class NameParser
	{
		string _prefix;
		string _first;
		string _middle;
		string _last;
		bool _suffixCommaEnabled;
		string _suffix;
		
		#region Public Properties

		public string Prefix
		{
			get { return _prefix; }
		}

		public string First
		{
			get { return _first; }
		}

		public string Middle
		{
			get { return _middle; }
		}

		public string Last
		{
			get { return _last; }
		}

		public bool SuffixCommaEnabled
		{
			get { return _suffixCommaEnabled; }
		}

		public string Suffix
		{
			get { return _suffix; }
		}

		#endregion

		public NameParser()
		{
		}

		public void Parse(string name)
		{
			/* =================================================== *
			 * This is may seem complicated, but if it is examined *
			 * with proper logic, carefully, it is really simple.  *
			 * Examine the comments carefully.					   *
			 *										  - Roy Hodges *
			 * =================================================== */

			// MAKE SURE THAT THE NAME FIELDS ARE RESET!
			this._prefix = string.Empty;
			this._first = string.Empty;
			this._middle = string.Empty;
			this._last = string.Empty;			
			this._suffixCommaEnabled = false;
            this._suffix = string.Empty;

			bool ignoreDelimeter = true;						// Ignores next text delimeter (spaces, commas etc.)
			bool ignoreThisCapital = false;						// Ignores this capital letter
			bool ignoreNextCapital = false;						// Ignores next capital letter
			bool commaInUse = false;

			bool isPrefixCaptured = false;
			bool isLastCaptured = false;
			bool isFirstCaptured = false;
			bool isMiddleCaptured = false;
			bool isTempLastCaptured = false;
			bool isSuffixCaptured = false;

			string capture = string.Empty;
			string formattedPrefix = string.Empty;
			string formattedSuffix = string.Empty;

			char[] c = name.ToCharArray();
			System.Text.StringBuilder s = new System.Text.StringBuilder();

			bool isMatched = true;

			for(int index = 0; index <= c.GetUpperBound(0); index++)
			{
				#region Punctuation Handling

				// Detect commas first!
				// Detect apostrophies, periods and dashes.
				// Set caps handling on next character on punctuation
				if(c[index] == ',')
				{
					commaInUse = true;
				}				
				else if(c[index] == '\'' | c[index] == '.' | c[index] == '-')
				{
					// Ignore this firstName delimeter if there is still text remaining to parse and the next character is not whitespace.
					ignoreDelimeter = index != c.GetUpperBound(0) && c[index+1] != ' ';

					// Ignore the next capital letter
					ignoreNextCapital = true;
				}
				else
					ignoreNextCapital = false;

				#endregion

				#region Multi-Prefix Handling ('Mc', 'Mac', 'von ', 'of ' etc.)

				// Since the string buffer should always be empty before
				// prefix handling, we only process prefixes if it is empty.
				if(s.Length == 0)
				{
					for(int i = 0; i < MultiCasePrefixes.Length; i++)
					{
						// Check to make sure that the checking length
						// is less than what's left in the string to check.
						if(index + MultiCasePrefixes[i].Length - 1 <
							c.GetUpperBound(0))
						{
							// Set isMatched to true to AND the match results together
							isMatched = true;

							// Loop through characters in prefix and search for a match
							for(int mcp_i = 0; mcp_i < MultiCasePrefixes[i].Length; mcp_i++)
							{
								if(MultiCasePrefixes[i][mcp_i] == '^')
								{
									isMatched &= char.IsUpper(c[index + mcp_i]);
								}
								else if(char.IsUpper(MultiCasePrefixes[i][mcp_i]))
								{
									isMatched &= char.ToUpper(c[index + mcp_i]) ==
										MultiCasePrefixes[i][mcp_i];
								}
								else if(char.IsLower(MultiCasePrefixes[i][mcp_i]))
								{
									isMatched &= char.ToLower(c[index + mcp_i]) ==
										MultiCasePrefixes[i][mcp_i];
								}
								else
								{
									isMatched &= c[index + mcp_i] ==
										MultiCasePrefixes[i][mcp_i];
								}

								// There's no need to continue matching if it's already
								// false.
								if(!isMatched)
								{
									break;
								}
							}
						}

						// If there is a match, add the prefix and increment the index
						// to just after the matched prefix. If there is an upper case
						// delimeter in the match firstName, ignore it when adding the prefix.
						if(isMatched)
						{
							if(MultiCasePrefixes[i].EndsWith("^"))
							{
								s.Append(MultiCasePrefixes[i].Substring(0, 
									MultiCasePrefixes[i].Length - 1));	
								index = index + MultiCasePrefixes[i].Length - 2;
							}
							else
							{
								s.Append(MultiCasePrefixes[i]);
								index = index + MultiCasePrefixes[i].Length - 1;
							}

							// There's no need to find another match, one
							// has already been found.
							break;
						}
					}

					if(isMatched)
					{
						continue;
					}

					#region Old Prefix Handler
					
//					// Fix "Mc" prefixes for "McCoullagh"
//					if(index+1 <= c.GetUpperBound(0) && 
//						Char.ToUpper(c[index]) == 'M' && 
//						Char.ToLower(c[index+1]) == 'c')
//					{
//						s.Append("Mc");
//						index++;
//						ignoreDelimeter = true;
//						continue;
//					}
//
//					// Fix "Di" prefixes on names like "DiCaprio"
//					if(index+2 <= c.GetUpperBound(0) &&
//						c[index] == 'D' &&
//						c[index+1] == 'i' && 
//						Char.IsUpper(c[index + 2]))
//					{
//						s.Append("Di");
//						index++;
//						ignoreDelimeter = true;
//						continue;
//					}
//
//					// Fix "De" prefixes on names like "DeCaprio"
//					if(index+2 <= c.GetUpperBound(0) &&
//						c[index] == 'D' &&
//						c[index+1] == 'e' && 
//						Char.IsUpper(c[index + 2]))
//					{
//						s.Append("De");
//						index++;
//						ignoreDelimeter = true;
//						continue;
//					}
//
//					// Fix "Mac" prefixes on names like "MacDonald"
//					if(index+3 <= c.GetUpperBound(0) &&
//						c[index] == 'M' &&
//						c[index+1] == 'a' && 
//						c[index+2] == 'c' && 
//						char.IsUpper(c[index+3]))
//					{
//						s.Append("Mac");
//						index = index + 2;
//						ignoreDelimeter = true;
//
//						continue;
//					}
//
//					// Fix "Le" prefixes on names like "LeFevre"
//					if(index+2 <= c.GetUpperBound(0) &&
//						c[index] == 'L' &&
//						c[index+1] == 'e' && 
//						Char.IsUpper(c[index + 2]))
//					{
//						s.Append("Le");
//						index++;
//						ignoreDelimeter = true;
//
//						continue;
//					}
//
//					// Fix "von" for "Eric von Guten"
//					if(index+3 <= c.GetUpperBound(0) && 
//						Char.ToLower(c[index]) == 'v' && 
//						Char.ToLower(c[index+1]) == 'o'  &&
//						Char.ToLower(c[index+2]) == 'n' &&
//						(Char.IsUpper(c[index+3]) | Char.IsWhiteSpace(c[index+3])))
//					{
//						s.Append("von ");
//
//						// Add Indexed Character
//						if(Char.IsUpper(c[index+3]))
//							s.Append(c[index+3]);
//												
//						index = index + 3;
//						ignoreDelimeter = true;
//
//						continue;
//					}
//
//					// Fix "van" for "Eric van Guten"
//					if(index+3 <= c.GetUpperBound(0) && 
//						Char.ToLower(c[index]) == 'v' && 
//						Char.ToLower(c[index+1]) == 'a'  &&
//						Char.ToLower(c[index+2]) == 'n' &&
//						(Char.IsUpper(c[index+3]) | Char.IsWhiteSpace(c[index+3])))
//					{
//						s.Append("van ");
//
//						// Add Indexed Character
//						if(Char.IsUpper(c[index+3]))
//							s.Append(c[index+3]);
//						
//						index = index + 3;
//						ignoreDelimeter = true;
//
//						continue;
//					}
//
//					// Fix "the" for "Eric the Guten"
//					if(index+3 <= c.GetUpperBound(0) && 
//						Char.ToLower(c[index]) == 't' && 
//						Char.ToLower(c[index+1]) == 'h'  &&
//						Char.ToLower(c[index+2]) == 'e' &&
//						(Char.IsUpper(c[index+3]) | Char.IsWhiteSpace(c[index+3])))
//					{
//						s.Append("the ");
//
//						// Add Indexed Character
//						if(Char.IsUpper(c[index+3]))
//							s.Append(c[index+3]);
//
//						index = index + 3;
//						ignoreDelimeter = true;
//
//						continue;
//					}
//
//					// Fix "of" for "William of Orange"
//					if(index+3 <= c.GetUpperBound(0) && 
//						Char.ToLower(c[index]) == 'o' && 
//						Char.ToLower(c[index+1]) == 'f' &&
//						(Char.IsUpper(c[index+2]) | Char.IsWhiteSpace(c[index+2])))
//					{
//						s.Append("of ");
//
//						// Add Indexed Character
//						if(Char.IsUpper(c[index+2]))
//							s.Append(c[index+2]);
//
//						index = index + 2;
//						ignoreDelimeter = true;
//
//						continue;
//					}
//
//					// Fix "el" for "Guzman el Bueno"
//					if(index+3 <= c.GetUpperBound(0) && 
//						Char.ToLower(c[index]) == 'e' && 
//						Char.ToLower(c[index+1]) == 'l' &&
//						(Char.IsUpper(c[index+2]) | Char.IsWhiteSpace(c[index+2])))
//					{						
//						s.Append("el ");
//
//						// Add Indexed Character
//						if(Char.IsUpper(c[index+2]))
//							s.Append(c[index+2]);
//
//						index = index + 2;
//						ignoreDelimeter = true;						
//
//						continue;
//					}

					#endregion
				}

				#endregion

				// Since the current enumerator is not a firstName delimeter, increment capture length
				// and set ignore space to false to trigger a capture at the next detected space.
				if(c[index] != ',' && c[index] != ' ')
				{
					if(ignoreDelimeter | ignoreThisCapital)
						s.Append(Char.ToUpper(c[index]));
					else
						s.Append(Char.ToLower(c[index]));
					ignoreDelimeter = false;

					ignoreThisCapital = ignoreNextCapital;
				}

				// If this is a comma and... 
				// ... the first and last name hasn't been captured...
				// ... set this as the last name.
				if(c[index] == ',' && !isLastCaptured && !isFirstCaptured)
				{
					_last = s.ToString();
					isLastCaptured = true;
					ignoreDelimeter = true;
					s.Length = 0;
					continue;
				}

				// If the character is a space, skip it and increment the capture start position
				if(!ignoreDelimeter && (c[index] == ',' | c[index] == ' ' | index == c.GetUpperBound(0)))
				{
					#region Prefix Handling Using isPrefix

					if(!isFirstCaptured && 
						isPrefix(s.ToString(), out formattedPrefix))
					{
						if(!isPrefixCaptured)
						{
							_prefix = formattedPrefix;
						}
						else
						{
							_prefix += " " + formattedPrefix;
						}

						formattedPrefix = string.Empty;
						isPrefixCaptured = true;
						ignoreDelimeter = true;
						s.Length = 0;
						continue;
					}

					#endregion

					#region Suffix Handling Using isSuffix

					if(isFirstCaptured &&
						(isSuffixCaptured | isSuffix(s.ToString(), out formattedSuffix)))
					{
						if(!isSuffixCaptured)
						{
							// Detect comma preference if any.
							if(commaInUse)
							{
								_suffixCommaEnabled = true;
								_suffix = formattedSuffix;
								commaInUse = false;
							}
							else
								_suffix = formattedSuffix;
						}
						else
						{
							_suffix += " " + formattedSuffix;
						}

						formattedSuffix = string.Empty;
						isSuffixCaptured = true;
						ignoreDelimeter = true;
						s.Length = 0;
						continue;
					}

					#endregion

					#region Name Handling
					
					// If the first name was not captured...
					// then set this as the first name.
					if(!isFirstCaptured)
					{
						_first = s.ToString();
						isFirstCaptured = true;
						ignoreDelimeter = true;
						s.Length = 0;
						continue;
					}

					// If the first name was captured...
					// ... and the last name was captured...
					// ... and the middle not, then set this as the middle name.
					// ... and the middle is, then add this to the middle name.
					if(isLastCaptured)
					{
						if(!isMiddleCaptured)
						{
							_middle = s.ToString();
						}
						else
						{
							_middle += " " + s.ToString();
						}

						isMiddleCaptured = true;
						ignoreDelimeter = true;
						s.Length = 0;
						continue;
					}

					// If the first name was captured...
					// ... and the last name wasn't captured...
					// ... and the temporary last name was not captured then
					// set this as the last name.
					if(!isTempLastCaptured)
					{
						_last = s.ToString();
						isTempLastCaptured = true;
						ignoreDelimeter = true;
						s.Length = 0;
						continue;
					}

					// If the first name was captured...
					// ... and the last name wasn't captured...
					// ... and the temporary last name was captured...
					// ... and if the middle name was not captured, 
					//	   set the last name to the middle name
					// ... otherwise
					//     add the last name to the middle name
					if(!isMiddleCaptured)
					{
						_middle = _last;
					}
					else
					{
						_middle += " " + _last;
					}
					
					// If the first name was captured...
					// ... and the last name wasn't captured...
					// ... and the temporary last name was captured...
					// set this to the last name and...
					// ... set the middle capture to true
					_last = s.ToString();
					isMiddleCaptured = true;
					ignoreDelimeter = true;
					s.Length = 0;
				}

				#endregion
			}
		}

		/// <summary>
		/// Tests to see if the string is a prefix and returns true if it is. In addition,
		/// it will provide a Formatted Version of a prefix for abbreviations.
		/// </summary>
		/// <param name="s">String to test.</param>
		/// <param name="formattedPrefix">String reference to pass formatted prefix.</param>
		/// <returns>True if string is a prefix; false if not.</returns>
		private bool isPrefix(string s, out string formattedPrefix)
		{
			string testPrefix;
			for(int x = 0; x <= Prefixes.GetUpperBound(0); x++)
			{
				testPrefix = s.ToUpper();
				if(testPrefix == Prefixes[x,0].ToUpper() |
					testPrefix.Replace(".", string.Empty) == Prefixes[x,1].ToUpper().Replace(".", string.Empty))
				{
					formattedPrefix = Prefixes[x,1];
					return true;
				}
			}
			
			formattedPrefix = s;

			return false;
		}

		/// <summary>
		/// Tests to see if the string is a suffix and returns true if it is. In addition,
		/// it will provide a Formatted Version of a suffix for abbreviations.
		/// </summary>
		/// <param name="s">String to test.</param>
		/// <param name="formattedSuffix">String reference to pass formatted suffix.</param>
		/// <returns>True if string is a suffix; false if not.</returns>
		private bool isSuffix(string s, out string formattedSuffix)
		{
			foreach(string suffix in SimpleSuffixes)
				if(s.ToUpper() == suffix.ToUpper())
				{
					formattedSuffix = suffix;
					return true;
				}

			for(int x = 0; x <= Suffixes.GetUpperBound(0); x++)
			{
				if(s.ToUpper().Replace(".", string.Empty) == Suffixes[x,0])
				{
					formattedSuffix = Suffixes[x,1];
					return true;
				}
			}	
			
			formattedSuffix = s;

			return false;
		}

		public static readonly String[] SimpleSuffixes = new String[]
			{
				"I",
				"II",
				"III",
				"IV",
				"V",
				"VI",
				"VII",
				"VIII",
				"IX",
				"X",
				"XI",
				"XII"
			};

		public static readonly String[,] Suffixes = new String[,]
			{
				{"ABPS", "ABPS"},
				{"ACNP", "ACNP"},
				{"ACSM", "ACSM"},
				{"AIYS", "AIYS"},
				{"AP", "AP"},
				{"AOBTA", "AOBTA"},
				{"AOCN", "AOCN"},
				{"ARP", "ARP"},
				{"ATC", "ATC"},
				{"ATR", "ATR"},
				{"ATR-BC", "ATR-BC"},
				{"AuD", "Au.D."},
				{"BA", "B.A."},
				{"BAMS", "BAMS"},
				{"BCEN", "BCEN"},
				{"BCNP", "BCNP"},
				{"BCNSP", "BCNSP"},
				{"BCPP", "BCPP"},
				{"BCPS", "BCPS"},
				{"BS", "B.S."},
				{"BSN", "BSN"},
				{"CA", "CA"},
				{"CAc", "CAc"},
				{"CAMT", "CAMT"},
				{"CAPA", "CAPA"},
				{"CCH", "CCH"},
				{"CMD", "CMD"},
				{"FAAD", "FAAD"},
				{"FAAHPM", "FAAHPM"},
				{"FAAO", "FAAO"},
				{"FAAP", "FAAP"},
				{"FACE", "FACE"},
				{"FACP", "FACP"},
				{"FACS", "FACS"},
				{"FADA", "FADA"},
				{"FAGD", "FAGD"},
				{"FAPhA", "FAPhA"},
				{"FASHP", "FASHP"},
				{"FIACA", "FIAcA"},
				{"FNP", "FNP"},
				{"MD", "M.D."},
				{"MBBS", "MBBS"},
				{"JR", "Jr."},
				{"SR", "Sr."},
				{"DO", "D.O."},
				{"DDS", "D.D.S."},
				{"NP", "N.P."},
				{"PA", "P.A."},
				{"PHD", "Ph.D."},
				{"BA", "B.A."},
				{"BS", "B.S."},
				{"MA", "M.A."},
				{"MBA", "M.B.A."},
				{"RN", "R.N."},
				{"EDD", "Ed.D"},
				{"SHIHAN", "Shihan"},
				{"SENSEI", "Sensei"},
				{"DOSHU", "Doshu"},
				{"SOKE", "Soke"}
			};

		public static readonly String[] MultiCasePrefixes = new string[]
			{	
				"Di^",
				"De^",
				"el ",
				"Fitz^",
				"La^",
				"Le^",
				"Mc",
				"Mac^",
				"of ",
				"the ",
				"von ",
				"van "
			};

		public static readonly String[,] Prefixes = new String[,]
			{
				{"AMBASSADOR", "Amb."},
				{"ASSISTANT", "Asst."},
				{"ASSOCIATE", "Assoc."},
				{"BISHOP", "Bp."},
				{"BROTHER", "Br."},
				{"CHANCELLOR", "Chanc."},
				{"CHAPLAIN", "Ch."},
				{"COMMISSIONER", "Comr."},
				{"DEAN", "Dean"},
				{"DEMOCRAT", "Dem."},
				{"DEPUTY", "D."},
				{"DIRECTOR", "Dir."},
				{"DOCTOR", "Dr."},
				{"FATHER", "Fr."},
				{"GOVERNOR", "Gov."},
				{"SISTER", "Sr."},
				{"HONORABLE", "Hon."},
				{"JUDGE", "J."},
				{"JUSTICE", "J."},
				{"MAYOR", "Mayor"},
				{"MISS", "Ms."},
				{"MISSES", "Mrs."},
				{"MISTER", "Mr."},
				{"PASTOR", "Pastor"},
				{"PRESIDENT", "Pres."},
				{"PRIEST", "Pr."},
				{"PROFESSOR", "Prof."},
				{"REPRESENTATIVE", "Rep."},
				{"REVEREND", "Rev."},
				{"SAINT", "St."},
				{"SENATOR", "Sen."}
			};
	}
}
