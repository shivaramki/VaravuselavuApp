using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace VaravuselavuStandard.Util
{
    public static class Helpers
    {
		public static String salt;
		public static String HashPassword(string password)
		{
			byte[] saltByte;
			new RNGCryptoServiceProvider().GetBytes(saltByte = new byte[16]);

			salt = Convert.ToBase64String(saltByte);

			var pbkdf2 = new Rfc2898DeriveBytes(password, saltByte, 10000);

			byte[] hash = pbkdf2.GetBytes(20);
			byte[] hashBytes = new byte[36];

			Array.Copy(saltByte, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);

			return Convert.ToBase64String(hashBytes);
		}

		public static bool ValidatePassword(String password,string hashPassword,string salt)
		{
			Boolean isAuthentic = false;
			byte[] saltByte = new byte[16];

			byte[] hashBytes = Convert.FromBase64String(hashPassword);
			Array.Copy(hashBytes, 0, saltByte, 0, 16);	
			var pbkdf2 = new Rfc2898DeriveBytes(password, saltByte, 10000);	
			byte[] hash = pbkdf2.GetBytes(20);

			int ok = 1;
			for (int i = 0; i < 20; i++)
			{	
				if (hashBytes[i + 16] != hash[i])
					ok = 0;
			}

			if (ok == 1)
			{
				isAuthentic = true;
			}

			return isAuthentic;
		}
    }
}
