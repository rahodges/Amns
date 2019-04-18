using System;

namespace Amns.GreyFox.People
{
	/// <summary>
	/// Summary description for AddressParser.
	/// </summary>
	public class AddressParser
	{
		string _fullAddress;

		public AddressParser()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void Parse(string address)
		{	
			bool caps = true;			// Capitalize this character
			bool holdCaps = false;		// Capitalize next character
			bool wordBreak = false;		// Wordbreak

			char c;
			char[] chars = address.ToCharArray();
			System.Text.StringBuilder buffer = new System.Text.StringBuilder();
			string word;
			System.Text.StringBuilder output = new System.Text.StringBuilder();
			
			string[,] dic = new string[,]
				{
					{"Mc^", "Mc"},
					{"#rd", "rd"},
					{"#th", "th"},
					{"#st", "st"},
					{"ne", "NE"},
					{"nw", "NW"},
					{"se", "SE"},
					{"sw", "SW"},
				};

			for(int index = 0; index <= chars.GetUpperBound(0); index++)
			{
				c = chars[index];

				wordBreak = 
					char.IsWhiteSpace(c) |
					char.IsSeparator(c);

				holdCaps = 
					wordBreak |
					char.IsPunctuation(c);
					char.IsNumber(c);
				
				if(!wordBreak)
				{
					if(caps)
					{
						c = char.ToUpper(c);
					}

					buffer.Append(c);
				}
				else
				{
					word = buffer.ToString();

					for(int x = 0; x < dic.GetUpperBound(0); x++)
					{
						if(dic[x,0].EndsWith("^"))
						{
							if(word == dic[x,0].Substring(0, dic[x,0].Length - 1))
							{
								holdCaps = true;
								word = dic[x,1];
								break;
							}
						}
						if(dic[x,0].StartsWith("#"))
						{
							if(word == dic[x,0].Substring(1))
							{
								word = dic[x,1];
								break;
							}
						}
						if(dic[x,0].StartsWith("."))
						{
							if(word.Replace(".", "") == dic[x,0].Substring(1))
							{
								word = dic[x,1];
								break;
							}
						}
					}
					
					output.Append(word);
					output.Append(c);

					buffer.Length = 0;
				}				

				caps = holdCaps;
			}

			_fullAddress = output.ToString();
		}
	}
}