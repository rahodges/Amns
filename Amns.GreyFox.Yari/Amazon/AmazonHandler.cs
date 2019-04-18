/* ********************************************************** *
 * AMNS Yari Amazon.Com Handler                               *
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
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Threading;
using System.Web;
using Amns.GreyFox.Yari;

namespace Amns.GreyFox.Yari.Amazon
{
	/// <summary>
	/// Summary description for BookDatabase.
	/// </summary>
	public class AmazonHandler
	{
		private static readonly string devAccount = "D2XSQQFKOQX7HK";
		private static readonly string ascAccount = "httpwwwaikids-20";
//		private static TimeSpan accessInterval = TimeSpan.FromSeconds(2);
		private static DateTime lastAccessTime;

//		private DateTime accessTime;
		
		private YariMediaRecord r;

		public string Asin
		{
			get
			{
				return Asin;
			}
		}

		public AmazonHandler(YariMediaRecord r)
		{
			this.r = r;
		}

		/// <summary>
		/// Updates the books database with Amazon.Com data.
		/// </summary>
		/// <param name="bookID"></param>
		/// <returns>The DateTime of the last Amazon query.</returns>
		public void Update()
		{
//			accessTime = lastAccessTime.Add(accessInterval);
//			lastAccessTime = DateTime.Now;
			
			if(r.Isbn == string.Empty)
			{
				if(r.AmazonFillDate != DateTime.MinValue |
					r.AmazonRefreshDate != DateTime.MinValue | 
					r.AmazonReleaseDate != DateTime.MinValue)
				{
					r.AmazonRefreshDate = DateTime.MinValue;
					r.AmazonFillDate = DateTime.MinValue;
					r.AmazonReleaseDate = DateTime.MinValue;
					r.Save();
				}
			}
			else if(r.AmazonFillDate.Add(TimeSpan.FromDays(5)) < DateTime.Now)
			{
				fill();
				r.Save();
			}
			else if(r.AmazonRefreshDate.Add(TimeSpan.FromHours(1)) < DateTime.Now)
			{				
				refresh();
				r.Save();
			}			
		}

		#region Fill and Refresh Methods

		private void fill()
		{
			//Create Instance of Proxy Class
			AmazonSearchService as1 = new AmazonSearchService();
			ProductInfo pi;

//			while(accessTime > DateTime.Now);

			//Call Service and Get Product Info
			try
			{
				pi = as1.AsinSearchRequest(GenerateAsinRequest("heavy"));
			}
			catch (Exception e)
			{
				r.amazonReleaseDate = DateTime.MinValue;
				r.AmazonRefreshDate = DateTime.Now;
				r.amazonFillDate = DateTime.MinValue;
				r.AmazonAvailability = e.Message;
				return;
			}
			finally
			{
				lastAccessTime = DateTime.Now;
			}

			if(pi.Details[0].Asin == null)
				return;
			
			if(pi.Details[0].ProductName != null && pi.Details[0].ProductName != string.Empty)
				r.Title = pi.Details[0].ProductName;
			r.ImageUrlLarge = pi.Details[0].ImageUrlLarge;
			r.ImageUrlMedium = pi.Details[0].ImageUrlMedium;
			r.ImageUrlSmall = pi.Details[0].ImageUrlSmall;
			
			try
			{
				r.AmazonListPrice = decimal.Parse(pi.Details[0].ListPrice);
			}
			catch
			{
			}

			try
			{
				r.AmazonOurPrice = decimal.Parse(pi.Details[0].OurPrice);
			}
			catch
			{
			}

			r.AmazonFillDate = DateTime.Now;
			r.AmazonRefreshDate = DateTime.Now;

			if(pi.Details[0].Media != null)
                r.AmazonMedia = pi.Details[0].Media;
			else
				r.AmazonMedia = null;
			
			try
			{
				r.AmazonReleaseDate = DateTime.Parse(pi.Details[0].ReleaseDate);
				r.PublishYear = r.AmazonReleaseDate.Year;
			}
			catch
			{
			}

			KeyPhrase[] amazonKeyPhrases = pi.Details[0].KeyPhrases;
			if(amazonKeyPhrases != null)
			{
				bool addKeyword;
				for(int x = 0; x < amazonKeyPhrases.Length; x++)
				{
					addKeyword = true;

					for(int y = 0; y < r.Keywords.Count; y++)
						if(amazonKeyPhrases[x].KeyPhrase1 == r.Keywords[y].Keyword)
						{
							addKeyword = false;
							break;
						}

					if(!addKeyword)
						continue;
						
					YariMediaKeyword k = new YariMediaKeyword();
					k.Keyword = amazonKeyPhrases[x].KeyPhrase1;
					r.Keywords.Add(k);
				}
			}
			
			if(pi.Details[0].Availability != null)
				r.AmazonAvailability = pi.Details[0].Availability;
			else
				r.AmazonAvailability = string.Empty;

			r.AmazonAsin = pi.Details[0].Asin;
			GreyFox.People.GreyFoxContactManager pM = 
				new Amns.GreyFox.People.GreyFoxContactManager("kitYari_Publishers");
			GreyFox.People.GreyFoxContactManager aM =
				new Amns.GreyFox.People.GreyFoxContactManager("kitYari_Authors");
			
			if(pi.Details[0].Manufacturer != null)
				r.Publisher = pi.Details[0].Manufacturer;
			else if(pi.Details[0].Publisher != null)
				r.Publisher = pi.Details[0].Publisher;

			r.Authors = string.Empty;
			if(pi.Details[0].Authors != null)
			{
				for(int x = 0; x < pi.Details[0].Authors.Length; x++)
				{
					if(r.Authors.Length > 0)
						r.Authors += "; " + pi.Details[0].Authors[x].Trim();
					else
						r.Authors = pi.Details[0].Authors[x].Trim();
				}
			}

			r.Save();
		}

		private void refresh()
		{
			//Create Instance of Proxy Class
			AmazonSearchService as1 = new AmazonSearchService();
			ProductInfo pi;

//			while(accessTime > DateTime.Now);

			try
			{
				//Call Service and Get Product Info 
				pi = as1.AsinSearchRequest(GenerateAsinRequest("lite"));
			}
			catch
			{
				r.AmazonRefreshDate = DateTime.Now;
				return;
			}
			finally
			{
				lastAccessTime = DateTime.Now;
			}

			if(pi.Details[0].Asin == null)
				return;
			
			r.ImageUrlLarge = pi.Details[0].ImageUrlLarge;
			r.ImageUrlMedium = pi.Details[0].ImageUrlMedium;
			r.ImageUrlSmall = pi.Details[0].ImageUrlSmall;
			
			try
			{
				r.AmazonListPrice = decimal.Parse(pi.Details[0].ListPrice);
			}
			catch
			{
			}

			try
			{
				r.AmazonOurPrice = decimal.Parse(pi.Details[0].OurPrice);
			}
			catch
			{
			}

			if(pi.Details[0].Availability != null)
				r.AmazonAvailability = pi.Details[0].Availability;
			else
				r.AmazonAvailability = string.Empty;

			r.AmazonRefreshDate = DateTime.Now;

			r.Save();

		}

		#endregion

		#region Helper Methods

		private Amazon.AsinRequest GenerateAsinRequest(string type) 
		{
			AsinRequest asin = new AsinRequest();
			asin.asin = r.Isbn.Trim().Replace("-", "");
			asin.devtag = devAccount;
			asin.type = type;
			asin.tag = ascAccount;
			return asin;
		}

		#endregion

	}
}