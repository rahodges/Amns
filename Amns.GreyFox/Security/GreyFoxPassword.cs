using System;
using System.Text;
using System.Security.Cryptography;

namespace Amns.GreyFox.Security
{
	/// <summary>
	/// Class for generating and hashing passwords
	/// </summary>
	public class GreyFoxPassword
	{
		static readonly byte[] KEY_64 = 
		{
			45, 78, 231, 122, 87, 34, 66, 132
		};

		static readonly byte[] IV_64 = 
		{
			53, 112, 246, 201, 35, 22, 87, 2
		};

		const string DES_PREFIX = "!des=";

		private string _password;
		private int _salt;

		public GreyFoxPassword(string strPassword, int nSalt)
		{
			_password = strPassword;
			_salt = nSalt;
		}

		public static bool CheckEncryption(string password)
		{
			return password.StartsWith(DES_PREFIX);
		}

		public static string DecodePassword(string encodedPassword)
		{
			string password;

			if(encodedPassword.StartsWith(DES_PREFIX))
			{
				password = encodedPassword.Substring(DES_PREFIX.Length);
				return CryptoUtil.Decrypt(password, KEY_64, IV_64);
			}
			else
			{
				return encodedPassword;
			}
		}

		public static string EncodePassword(string password)
		{
			if(!password.StartsWith(DES_PREFIX))
			{
				return DES_PREFIX + CryptoUtil.Encrypt(password, KEY_64, IV_64);
			}
			else
			{
				return password;
			}
		}

		public static string CreateRandomPassword(int PasswordLength)
		{
			String _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789";
			Byte[] randomBytes = new Byte[PasswordLength];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);
			char[] chars = new char[PasswordLength];
			int allowedCharCount = _allowedChars.Length;

			for(int i = 0;i<PasswordLength;i++)
			{
				chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
			}

			return new string(chars);
		}

		public static int CreateRandomSalt()
		{
			Byte[] _saltBytes = new Byte[4];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(_saltBytes);

			return ((((int)_saltBytes[0]) << 24) + (((int)_saltBytes[1]) << 16) + 
				(((int)_saltBytes[2]) << 8) + ((int)_saltBytes[3]));
		}

		public string ComputeSaltedHash()
		{
			// Create Byte array of password string
			ASCIIEncoding encoder = new ASCIIEncoding();
			Byte[] _secretBytes = encoder.GetBytes(_password);
			
			// Create a new salt
			Byte[] _saltBytes = new Byte[4];
			_saltBytes[0] = (byte)(_salt >> 24);
			_saltBytes[1] = (byte)(_salt >> 16);
			_saltBytes[2] = (byte)(_salt >> 8);
			_saltBytes[3] = (byte)(_salt);

			// append the two arrays
			Byte[] toHash = new Byte[_secretBytes.Length + _saltBytes.Length];
			Array.Copy(_secretBytes, 0, toHash, 0, _secretBytes.Length);
			Array.Copy(_saltBytes, 0, toHash, _secretBytes.Length, _saltBytes.Length);

			SHA1 sha1 = SHA1.Create();
			Byte[] computedHash = sha1.ComputeHash(toHash);

			return encoder.GetString(computedHash);
		}
	}
}