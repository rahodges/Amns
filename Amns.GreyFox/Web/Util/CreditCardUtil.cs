using System;

namespace Amns.GreyFox.Web.Util
{
	/// <summary>
	/// Summary description for CreditCardUtil.
	/// </summary>
	public class CreditCardUtil
	{
		public static bool IsValidCardNumber(string ccNum)
		{
			int nCheck = 0;
			int nDigit = 0;
			bool bEven = false;

			for(int n = ccNum.Length - 1; n >= 0; n--)
			{
				if(char.IsNumber(ccNum[n]))
				{
					nDigit = int.Parse(ccNum[n].ToString());

					if(bEven)
					{
						if((nDigit *= 2) > 9)
						{
							nDigit -= 9;
						}
					}

					nCheck += nDigit;
					bEven = !bEven;
				}
				else if(ccNum[n] != ' ' && ccNum[n] != '.' && ccNum[n] != '-')
				{
					return false;
				}
			}

			return (nCheck % 10) == 0;
		}

		public static bool IsValidCardType(string ccNum, string type)
		{
			int nLen = 0;

			type = type.ToUpper();
            
			for(int n = 0; n < ccNum.Length; n++)
			{
				if(char.IsNumber(ccNum[n]))
				{
					nLen++;
				}
			}

			if(type == "VISA")
			{
				return ((ccNum.Substring(0,1) == "4") && (nLen == 13 || nLen == 16));
			}
			else if(type == "AMEX")
			{
				return ((ccNum.Substring(0,2) == "34" || ccNum.Substring(0,2) == "37") && (nLen == 15));
			}
			else if(type == "MASTER CARD")
			{
				return ((ccNum.Substring(0,2) == "51" || ccNum.Substring(0,2) == "52" ||
					ccNum.Substring(0,2) == "53" || ccNum.Substring(0,2) == "54" ||
					ccNum.Substring(0,2) == "55") && (nLen == 16));
			}
			else
			{
				return false;
			}
		}
	}
}
