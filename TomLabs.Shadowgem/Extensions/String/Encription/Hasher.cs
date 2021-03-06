﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace TomLabs.Shadowgem.Extensions.String.Encription
{
	/// <summary>
	/// Provides methods for string hashing
	/// </summary>
	public static class Hasher
	{
		/// <summary>
		/// Supported hash algorithms
		/// </summary>
		public enum EHashType
		{
			HMAC,
			HMACMD5,
			HMACSHA1,
			HMACSHA256,
			HMACSHA384,
			HMACSHA512,
			MACTripleDES,
			RIPEMD160,
			SHA256,
			SHA384,
			SHA512,

			/// <summary>
			/// Should not be used. 
			/// </summary>
			[Obsolete("Has cryptographic weaknesses", false)]
			MD5,
			/// <summary>
			/// Should not be used. 
			/// https://www.howtogeek.com/238705/what-is-sha-1-and-why-will-retiring-it-kick-thousands-off-the-internet/
			/// </summary>
			[Obsolete("Has cryptographic weaknesses", false)]
			SHA1,
		}

		private static byte[] GetHash(string input, EHashType hash)
		{
			byte[] inputBytes = Encoding.ASCII.GetBytes(input);

			switch (hash)
			{
				case EHashType.HMAC:
					return HMAC.Create().ComputeHash(inputBytes);

				case EHashType.HMACMD5:
					return HMACMD5.Create().ComputeHash(inputBytes);

				case EHashType.HMACSHA1:
					return HMACSHA1.Create().ComputeHash(inputBytes);

				case EHashType.HMACSHA256:
					return HMACSHA256.Create().ComputeHash(inputBytes);

				case EHashType.HMACSHA384:
					return HMACSHA384.Create().ComputeHash(inputBytes);

				case EHashType.HMACSHA512:
					return HMACSHA512.Create().ComputeHash(inputBytes);

				case EHashType.MACTripleDES:
					return MACTripleDES.Create().ComputeHash(inputBytes);

				case EHashType.MD5:
					return MD5.Create().ComputeHash(inputBytes);

				case EHashType.RIPEMD160:
					return RIPEMD160.Create().ComputeHash(inputBytes);

				case EHashType.SHA1:
					return SHA1.Create().ComputeHash(inputBytes);

				case EHashType.SHA256:
					return SHA256.Create().ComputeHash(inputBytes);

				case EHashType.SHA384:
					return SHA384.Create().ComputeHash(inputBytes);

				case EHashType.SHA512:
					return SHA512.Create().ComputeHash(inputBytes);

				default:
					return inputBytes;
			}
		}

		/// <summary>
		/// Computes the hash of the string using a specified hash algorithm
		/// </summary>
		/// <param name="input">The string to hash</param>
		/// <param name="hashType">The hash algorithm to use</param>
		/// <returns>The resulting hash or an empty string on error</returns>
		public static string ComputeHash(this string input, EHashType hashType)
		{
			try
			{
				byte[] hash = GetHash(input, hashType);
				var ret = new StringBuilder();

				for (int i = 0; i < hash.Length; i++)
				{
					ret.Append(hash[i].ToString("x2"));
				}

				return ret.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}
