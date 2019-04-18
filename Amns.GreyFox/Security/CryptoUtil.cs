using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Amns.GreyFox.Security
{
	/// <summary>
	/// Summary description for CryptoUtil.
	/// </summary>
	public class CryptoUtil
	{
		static readonly byte[] KEY_64 = 
		{
            45, 78, 231, 122, 87, 34, 66, 132
		};

		static readonly byte[] IV_64 = 
		{
			53, 112, 246, 201, 35, 22, 87, 2
		};

		static readonly byte[] KEY_192 = 
		{
			56, 16, 93, 231, 78, 4, 211, 98,
            4, 167, 44, 80, 34, 250, 28, 112,
			65, 45, 208, 204, 119, 35, 42, 123
		};
		
		static readonly byte[] IV_192 = 
		{
			65, 23, 223, 34, 66, 78, 137, 45, 
            76, 78, 34, 196, 143, 65, 221, 43, 
			165, 205, 200, 222, 120, 15, 34, 22
		};

		public static string Encrypt(string text)
		{
			return Encrypt(text, KEY_64, IV_64);
		}

		public static string Encrypt(string text, byte[] key, byte[] iv)
		{
			if(text == "")
				return string.Empty;

			DESCryptoServiceProvider cryptoProvider =
				new DESCryptoServiceProvider();
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, 
				cryptoProvider.CreateEncryptor(key, iv),
				CryptoStreamMode.Write);
			StreamWriter sw = new StreamWriter(cs);

			sw.Write(text);
			sw.Flush();
			cs.FlushFinalBlock();
			ms.Flush();

			return Convert.ToBase64String(ms.GetBuffer(), 
				0, Convert.ToInt32(ms.Length));
		}
		
		public static string Decrypt(string text)
		{
			return Decrypt(text, KEY_64, IV_64);
		}

		public static string Decrypt(string text, byte[] key, byte[] iv)
		{
			if(text == "")
				return string.Empty;

			DESCryptoServiceProvider cryptoProvider = 
				new DESCryptoServiceProvider();
			byte[] buffer = Convert.FromBase64String(text);
			MemoryStream ms = new MemoryStream(buffer);
			CryptoStream cs = new CryptoStream(ms, 
				cryptoProvider.CreateDecryptor(key, iv),
				CryptoStreamMode.Read);
			StreamReader sr = new StreamReader(cs);

			return sr.ReadToEnd();		
		}

		public static string EncryptTripleDES(string text)
		{
			if(text == "")
				return string.Empty;

			TripleDESCryptoServiceProvider cryptoProvider =
				new TripleDESCryptoServiceProvider();
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, 
				cryptoProvider.CreateEncryptor(KEY_192, IV_192),
				CryptoStreamMode.Write);
			StreamWriter sw = new StreamWriter(cs);

			sw.Write(text);
			sw.Flush();
			cs.FlushFinalBlock();
			ms.Flush();

			return Convert.ToBase64String(ms.GetBuffer(), 0, 
				Convert.ToInt32(ms.Length));
		}

		public static string DecryptTripleDES(string text)
		{
			if(text == "")
				return string.Empty;

			TripleDESCryptoServiceProvider cryptoProvider = 
				new TripleDESCryptoServiceProvider();
			byte[] buffer = Convert.FromBase64String(text);
			MemoryStream ms = new MemoryStream(buffer);
			CryptoStream cs = new CryptoStream(ms, 
				cryptoProvider.CreateDecryptor(KEY_192, IV_192),
				CryptoStreamMode.Read);
			StreamReader sr = new StreamReader(cs);

			return sr.ReadToEnd();		
		}
	}
}