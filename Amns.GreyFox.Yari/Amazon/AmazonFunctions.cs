/* ********************************************************** *
 * AMNS Yari Amazon.Com Functions                             *
 * Copyright © 2003-2006 Roy A.E. Hodges                      *
 * All Rights Reserved                                        *
 *                                                            *
 * HARD CODED FOR AIKIDO SHOBUKAN DOJO                        *
 * ---------------------------------------------------------- *
 * Source code may not be reproduced or redistributed without *
 * written expressed permission from the author.              *
 * Permission is granted to modify source code by licencee.   *
 * These permissions do not extend to third parties.          *
 * ********************************************************** */

using System;
using System.Text;

namespace Amns.GreyFox.Yari.Amazon
{
	/// <summary>
	/// Summary description for AmazonFunctions.
	/// </summary>
	public class AmazonFunctions
	{
//		private static readonly string devAccount = "D2XSQQFKOQX7HK";
		private static readonly string ascAccount = "httpwwwaikids-20";

		public AmazonFunctions()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string RenderBuyButton(string asin)
		{
			return RenderBuyButton(asin, ascAccount);
		}

		public static string RenderBuyButton(string asin, string associateAccount)
		{
			StringBuilder s = new StringBuilder();
			s.Append("<script language=\"JavaScript\">");
			s.Append("function popUp(URL,NAME) {");
			s.Append("amznwin=window.open(URL,NAME,'location=yes,scrollbars=yes,status=yes,toolbar=yes,resizable=yes,width=380,height=450,screenX=10,screenY=10,top=10,left=10');");
			s.Append("amznwin.focus();}");
			s.Append("document.open();");
			s.Append("document.write(\"<a href=javascript:popUp(");
			s.Append("'http://buybox.amazon.com/exec/obidos/redirect");
			s.Append("?tag=httpwwwaikids-20");
			s.Append("&link_code=qcb");
			s.Append("@creative=23424");
			s.Append("&camp=2025");
			s.Append("&path=/dt/assoc/tg/aa/xml/assoc/-/");
			s.Append(asin);
			s.Append("/");
			s.Append(associateAccount);
			s.Append("/ref=ac_bb6_,_amazon')>");
			s.Append("<img src=http://rcm-images.amazon.com/images/G/01/associates/remote-buy-box/buy6.gif border=0 alt='Buy from Amazon.com' ></a>\");");
			s.Append("document.close();");
			s.Append("</script>");
			return s.ToString();
		}
	}
}
