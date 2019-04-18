using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Support
{
	/// <summary>
	/// Summary description for ContentBuilder.
	/// </summary>
	public class ContentBuilder
	{
//		string __outline;
		
		DbContentClip	__clip					= null;
		string			__body					= "Not Compiled";

		bool			__outlineEnabled		= false;
		string			__outlineBlock			= "Not Compiled";
		string			__outlineTags			= "h1,h2,h3";

		bool			__glossEnabled			= true;
		bool			__glossWordsCompiled	= false;
		string			__glossBlock			= string.Empty;
		string			__glossBodyFormat		= "<span style=\"cursor:help;\" onmouseover=\"gfx_glossCopy('gls_{0}');\">{1}</span>";
		string			__glossBlockFormat		= "<span id=\"gls_{0}\" style=\"display:none;\">{1}</span>";

		bool			__faqEnabled			= true;
		bool			__faqCompiled			= false;
		string			__faqQuestionFormat		= "<span style=\"cursor:hand;\" onmousedown=\"gfx_faqToggle('faq_{0}');\">{1}</span>";
		string			__faqAnswerFormat		= "<div id=\"faq_{0}\" style=\"display:none;\">{1}</div>";

//		string			__linkPostBackUrl		= "~/ref=?{0}";
//		string			__linkTagLeft			= "[[";
//		string			__linkTagRight			= "]]";

		#region public properties

		public string Output
		{
			get { return __body; }
		}

		#endregion

		#region outline properties

		public string OutlineBlock
		{
			get { return __outlineBlock; }
		}

		public string OutlineTags
		{
			get { return __outlineTags; }
			set { __outlineTags = value; }
		}

		#endregion

		#region gloss properties

		public bool GlossWordsCompiled
		{
			get { return __glossWordsCompiled; } 
		}

		public string GlossBlock
		{
			get { return __glossBlock; } 
		}

		#endregion

		#region faq properties

		public bool FaqCompiled
		{
			get { return __faqCompiled; }
		}

		#endregion

		public ContentBuilder(DbContentClip clip)
		{
			__clip = clip;
		}

		public void Compile()
		{
			__body = __clip.Body;

			if(__glossEnabled)
				glossaryMarkup();

			if(__outlineEnabled)
				outlineMarkup();

			if(__faqEnabled)
				faqMarkup();
		}

		#region Outline

		private void outlineMarkup()
		{
			StringBuilder outline = new StringBuilder();

			string tagName;
			string innerText;
			string[] tags = this.__outlineTags.Split(',');

			int tagLeftIndex = 0;		// index of the beginning of the tag
			int tagNameIndex = 0;		// index of the beginning of the tag name
			int tagWhiteSpaceIndex = 0; // index of the first whitespace in the tag
			int tagInnerTextIndex = 0;	// index of the beginning of the text
			int tagRightIndex = 0;

			int tagEndTagIndex = 0;		// index of the beginning of the end tag
			int tagEndIndex = 0;		// index of the beginning of the text after the tag

			int indent = 0;

			// Start Outline
			outline.Append("<OL>");

			while(true)
			{
				tagLeftIndex = __body.IndexOf("<", tagEndIndex);

				if(tagLeftIndex != -1)
				{
					tagNameIndex = tagLeftIndex + 1;
					tagRightIndex = __body.IndexOf(">", tagNameIndex);
					tagInnerTextIndex = tagRightIndex + 1;
					tagWhiteSpaceIndex = __body.IndexOf(" ", tagNameIndex, tagRightIndex - tagNameIndex);

					if(tagWhiteSpaceIndex == -1)
						tagName = __body.Substring(tagNameIndex, tagRightIndex - tagNameIndex);
					else
						tagName = __body.Substring(tagNameIndex, tagWhiteSpaceIndex - tagNameIndex);

					tagEndTagIndex = __body.IndexOf("</" + tagName + ">", tagInnerTextIndex);
					tagEndIndex = tagEndTagIndex + tagName.Length + 3;	// 3 for left forward slash and right

					innerText = __body.Substring(tagInnerTextIndex, tagEndTagIndex - tagInnerTextIndex);

					for(int i = 0; i < tags.Length; i++)
					{
						if(tags[i].ToLower() == tagName.ToLower())
						{
							// if indent less than index, increase indent and level
							// if indent is more than index, decrease indent and level
							// if indent matches the index, append on same level
							while(i < indent)
							{
								indent--;
								outline.Append("</OL>");
							
							}						
							while(i > indent)
							{
								indent ++;
								outline.Append("<OL>");
							}

							outline.Append("<LI>");
							outline.Append(innerText);
							outline.Append("</LI>");
						
							break;
						}
					}
				}
				else
				{
					break;
				}
			}

			// Finish the outline
			while(indent != 0)
			{
				indent--;
				outline.Append("</OL>");
			}

			// End the outline
			outline.Append("</OL>");

			// Set the outline block
			__outlineBlock = outline.ToString();

			// Replace an outline tag with the outline block
			__body = __body.Replace("[outline/]", __outlineBlock);
		}

		#endregion

		#region Gloss

		/// <summary>
		/// Marks up glosswords in the body. Glosswords have the following
		/// tags.
		/// <code>[word]mindfulness[gloss]sati[/gloss][/word]</code>
		/// </summary>
		private void glossaryMarkup()
		{
			StringBuilder bodyBlock = new StringBuilder();		// Rebuild body instead of using replace
			StringBuilder glossBlock = new StringBuilder();		// Build gloss block instead of concetating strings

			int wordTagIndex = 0;
			int wordIndex = 0;
			int wordEndTagIndex = 0;
			int wordEndIndex = 0;
			int glossTagIndex = 0;
			int glossIndex = 0;
			int glossEndTagIndex = 0;
			int glossEndIndex = 0;
			
			int lastWordEndIndex = 0;
						
			string word;
			string gloss;

			while(true)
			{
				wordTagIndex = __body.IndexOf("[word]", wordEndIndex);

				if(wordTagIndex != -1)
				{
					// Set the word index to the beginning of the word
					wordIndex = wordTagIndex + 6;

					// Get the index of next gloss tag
					glossTagIndex = __body.IndexOf("[gloss]", wordIndex);
					glossIndex = glossTagIndex + 7;
					
					// Get the index of the gloss end tag
                    glossEndTagIndex = __body.IndexOf("[/gloss]", glossIndex);
					glossEndIndex = glossEndTagIndex + 8;

					// Get the index of the word end tag
					wordEndTagIndex = __body.IndexOf("[/word]", glossEndIndex);
					wordEndIndex = wordEndTagIndex + 7;

					// Get the word and the gloss
					word = __body.Substring(wordIndex, glossTagIndex - wordIndex);
					gloss = __body.Substring(glossIndex, glossEndTagIndex - glossIndex);

					// Append body text with previous body text and the new gloss body format (javascript div tags)
					bodyBlock.Append(__body.Substring(lastWordEndIndex, wordTagIndex - lastWordEndIndex));
					bodyBlock.AppendFormat(__glossBodyFormat, word, word);

					// Append gloss block
					glossBlock.AppendFormat(__glossBlockFormat, word, gloss);

					lastWordEndIndex = wordEndIndex;

					__glossWordsCompiled = true;
				}
				else
				{					
					break;
				}
			}

			// Append remaining text to body block if there is some
			if(lastWordEndIndex != 0)
			{
				bodyBlock.Append(__body.Substring(lastWordEndIndex));
				__body = bodyBlock.ToString();
			}

			__glossBlock = glossBlock.ToString();
		}

		#endregion

		#region FAQ

		/// <summary>
		/// Marks up glosswords in the body. Glosswords have the following
		/// tags.
		/// <code>[word]mindfulness[gloss]sati[/gloss][/word]</code>
		/// </summary>
		private void faqMarkup()
		{
			StringBuilder bodyBlock = new StringBuilder();			// Rebuild body instead of using replace

			int questionID = 0;

			int questionTagIndex = 0;
			int questionIndex = 0;
			int questionEndTagIndex = 0;
			int questionEndIndex = 0;
			int answerTagIndex = 0;
			int answerIndex = 0;
			int answerEndTagIndex = 0;
			int answerEndIndex = 0;
			
			int lastQuestionEndIndex = 0;
						
			string question;
			string answer;

			while(true)
			{
				questionTagIndex = __body.IndexOf("[question]", questionEndIndex);

				if(questionTagIndex != -1)
				{
					questionID++;

					// Set the word index to the beginning of the word
					questionIndex = questionTagIndex + 10;

					// Get the index of next gloss tag
					answerTagIndex = __body.IndexOf("[answer]", questionIndex);
					answerIndex = answerTagIndex + 8;
					
					// Get the index of the gloss end tag
					answerEndTagIndex = __body.IndexOf("[/answer]", answerIndex);
					answerEndIndex = answerEndTagIndex + 9;

					// Get the index of the word end tag
					questionEndTagIndex = __body.IndexOf("[/question]", answerEndIndex);
					questionEndIndex = questionEndTagIndex + 11;

					// Get the word and the gloss
					question = __body.Substring(questionIndex, answerTagIndex - questionIndex);
					answer = __body.Substring(answerIndex, answerEndTagIndex - answerIndex);

					// Append body text with previous body text and the new gloss body format (javascript div tags)
					bodyBlock.Append(__body.Substring(lastQuestionEndIndex, questionTagIndex - lastQuestionEndIndex));
					bodyBlock.AppendFormat(__faqQuestionFormat, questionID, question);
					bodyBlock.AppendFormat(__faqAnswerFormat, questionID, answer);

					lastQuestionEndIndex = questionEndIndex;

					__faqCompiled = true;
				}
				else
				{					
					break;
				}
			}

			// Append remaining text to body block if there is some
			if(lastQuestionEndIndex != 0)
			{
				bodyBlock.Append(__body.Substring(lastQuestionEndIndex));
				__body = bodyBlock.ToString();
			}
		}

		#endregion

	}
}
