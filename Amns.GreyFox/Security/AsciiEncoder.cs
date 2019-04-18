using System;
using System.Text;

namespace Amns.GreyFox.Security
{
	/// <summary>
	/// Summary description for EmailEncoder.
	/// </summary>
	public class AciiEncoder
	{
		public AciiEncoder()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string EncodeText(string text)
		{
			StringBuilder s = new StringBuilder();

			char[] chars = text.ToCharArray();
			for(int x = 0; x < chars.Length; x++)
			{
				s.Append("&#");
				s.Append(((int) chars[x]).ToString());
				s.Append(";");
			}

			return s.ToString();
		}

		public static void EncodeText(string text, StringBuilder s)
		{
			char[] chars = text.ToCharArray();
			for(int x = 0; x < chars.Length; x++)
			{
				s.Append("&#");
				s.Append(((int) chars[x]).ToString());
				s.Append(";");
			}
		}

		public static string EncodeMailTo(string email)
		{
			StringBuilder s = new StringBuilder();

			s.Append("<a href=\"");
			s.Append(EncodeText("mailto:"));
			s.Append(EncodeText(email));
			s.Append(">");
			s.Append(email);
			s.Append("</a>");

			return s.ToString();
		}

		public static string EncodeMailTo(string email, string text)
		{
			StringBuilder s = new StringBuilder();

			s.Append("<a href=\"");
			s.Append(EncodeText("mailto:"));
			s.Append(EncodeText(email));
			s.Append(">");
			s.Append(text);
			s.Append("</a>");

			return s.ToString();
		}
	}
}
