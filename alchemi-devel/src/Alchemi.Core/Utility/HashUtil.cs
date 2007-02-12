#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   HashUtil.cs
 * Project      :   Alchemi.Core.Utility
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   David Cumps
 *                  Krishna Nadiminti (kna@csse.unimelb.edu.au)
 * License      :   GPL
 *                  This program is free software; you can redistribute it and/or 
 *                  modify it under the terms of the GNU General Public
 *                  License as published by the Free Software Foundation;
 *                  See the GNU General Public License 
 *                  (http://www.gnu.org/copyleft/gpl.html) for more details.
 *
 */
#endregion

using System;
using System.Security.Cryptography;
using System.Text;

namespace Alchemi.Core.Utility
{
    // From the tutorial at http://www.developerfusion.co.uk/show/4601/
    // Original author blog: http://weblogs.asp.net/CumpsD
    // 2.9.07 MDV Refactored and removed redundant methods

    /// <summary>
    /// Class used to generate and check hashes.
    /// </summary>
    public sealed class HashUtil
    {
        /// <summary>
        /// Private constructor to prevent instantiation of this class.
        /// </summary>
        private HashUtil() { }

        #region Enum - HashType
        /// <summary>
        /// The wanted hash function.
        /// </summary>
        public enum HashType : int
        {
            MD5,
            SHA1,
            SHA256,
            SHA384,
            SHA512
        }
        #endregion


        /// <summary>
        /// Generates the hash of a text.
        /// </summary>
        /// <param name="input">The text for which to generate the hash.</param>
        /// <param name="hashType">The hash function to use.</param>
        /// <returns>The hash as a hexadecimal string.</returns>
        public static string GetHash(string input, HashType hashType)
        {
            string result;
            switch (hashType)
            {
                case HashType.MD5: result = DoHashingAlgorithm(input, new MD5CryptoServiceProvider()); break;
                case HashType.SHA1: result = DoHashingAlgorithm(input, new SHA1Managed()); break;
                case HashType.SHA256: result = DoHashingAlgorithm(input, new SHA256Managed()); break;
                case HashType.SHA384: result = DoHashingAlgorithm(input, new SHA384Managed()); break;
                case HashType.SHA512: result = DoHashingAlgorithm(input, new SHA512Managed()); break;
                default: result = "Invalid HashType"; break;                   
            }
            return result;
        }

        /// <summary>Checks a text with a hash.</summary>
        /// <param name="original">The text to compare the hash against.</param>
        /// <param name="hashed">The hash to compare against.</param>
        /// <param name="hashType">The type of hash.</param>
        /// <returns>True if the hash validates, false otherwise.</returns>
        public static bool CheckHash(string original, string hashed, HashType hashType)
        {
            string strOrigHash = GetHash(original, hashType);
            return (strOrigHash == hashed);
        }


        /// <summary>
        /// Performs the hashing operation on the inputs string using the specified HashAlgorithm.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <param name="algorithm">The hashing algorithm to use.</param>
        /// <returns></returns>
        private static string DoHashingAlgorithm(string input, HashAlgorithm algorithm)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] HashValue, MessageBytes = UE.GetBytes(input);
            StringBuilder hex = new StringBuilder();
            HashValue = algorithm.ComputeHash(MessageBytes);
            foreach (byte b in HashValue)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}