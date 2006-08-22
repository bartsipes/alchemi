#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	Utils.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  David Cumps, Krishna Nadiminti (kna@csse.unimelb.edu.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/ 
#endregion


using System;
using System.Security.Cryptography;
using System.Text;

namespace Alchemi.Core.Utility {

	// from the tutorial at http://www.developerfusion.co.uk/show/4601/
	// original author blog: http://weblogs.asp.net/CumpsD

	/// <summary>Class used to generate and check hashes.</summary>
	public sealed class HashUtil {
		/// <summary></summary>
		private HashUtil() { }
		
		#region Hash Choices
		/// <summary>The wanted hash function.</summary>
		public enum HashType :int {
			/// <summary>MD5 Hashing</summary>
			MD5,
			/// <summary>SHA1 Hashing</summary>
			SHA1,
			/// <summary>SHA256 Hashing</summary>
			SHA256,
			/// <summary>SHA384 Hashing</summary>
			SHA384,
			/// <summary>SHA512 Hashing</summary>
			SHA512
		} /* HashType */
		#endregion
		
		#region Public Methods
		/// <summary>Generates the hash of a text.</summary>
        /// <param name="input">The text of which to generate a hash of.</param>
		/// <param name="hashType">The hash function to use.</param>
		/// <returns>The hash as a hexadecimal string.</returns>
		public static string GetHash(string input, HashType hashType) {
			string result;
			switch (hashType) {
				case HashType.MD5: result = GetMD5(input);	break;
				case HashType.SHA1: result = GetSHA1(input);	break;
				case HashType.SHA256: result = GetSHA256(input); break;
				case HashType.SHA384: result = GetSHA384(input); break;
				case HashType.SHA512: result = GetSHA512(input); break;
				default: result = "Invalid HashType"; break;
			}
			return result;
		} /* GetHash */
		
		/// <summary>Checks a text with a hash.</summary>
		/// <param name="original">The text to compare the hash against.</param>
		/// <param name="hashed">The hash to compare against.</param>
		/// <param name="hashType">The type of hash.</param>
		/// <returns>True if the hash validates, false otherwise.</returns>
		public static bool CheckHash(string original, string hashed, HashType hashType) {
			string strOrigHash = GetHash(original, hashType);
			return (strOrigHash == hashed);
		} /* CheckHash */
		#endregion
		
		#region Hashers
		private static string GetMD5(string input)
        {
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] HashValue, MessageBytes = UE.GetBytes(input);
			MD5 md5 = new MD5CryptoServiceProvider();
            StringBuilder hex = new StringBuilder();
			
			HashValue = md5.ComputeHash(MessageBytes);
			foreach(byte b in HashValue) 
            {
				hex.AppendFormat("{0:x2}", b);
			}

			return hex.ToString();
		} /* GetMD5 */
		
		private static string GetSHA1(string input)
        {
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] HashValue, MessageBytes = UE.GetBytes(input);
			SHA1Managed SHhash = new SHA1Managed();
            StringBuilder hex = new StringBuilder();

			HashValue = SHhash.ComputeHash(MessageBytes);
			foreach(byte b in HashValue) 
            {
				hex.AppendFormat("{0:x2}", b);
			}
			return hex.ToString();
		} /* GetSHA1 */
		
		private static string GetSHA256(string input)
        {
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] HashValue, MessageBytes = UE.GetBytes(input);
			SHA256Managed SHhash = new SHA256Managed();
            StringBuilder hex = new StringBuilder();

			HashValue = SHhash.ComputeHash(MessageBytes);
			foreach(byte b in HashValue)
            {
				hex.AppendFormat("{0:x2}", b);
			}
			return hex.ToString();
		} /* GetSHA256 */
		
		private static string GetSHA384(string input) 
        {
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] HashValue, MessageBytes = UE.GetBytes(input);
			SHA384Managed SHhash = new SHA384Managed();
            StringBuilder hex = new StringBuilder();

			HashValue = SHhash.ComputeHash(MessageBytes);
			foreach(byte b in HashValue)
            {
				hex.AppendFormat("{0:x2}", b);
			}
			return hex.ToString();
		} /* GetSHA384 */
		
		private static string GetSHA512(string input) 
        {
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] HashValue, MessageBytes = UE.GetBytes(input);
			SHA512Managed SHhash = new SHA512Managed();
            StringBuilder hex = new StringBuilder();

			HashValue = SHhash.ComputeHash(MessageBytes);
			foreach(byte b in HashValue)
            {
				hex.AppendFormat("{0:x2}", b);
			}
			return hex.ToString();
		} /* GetSHA512 */
		#endregion
	} /* Hash */
} /* Hash */