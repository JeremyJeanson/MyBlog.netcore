﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace MyLib.Web.Security
{
    /// <summary>
    /// Helper to hash strings
    /// </summary>
    public static class HashHelper
    {
        private const Int32 SaltLength = 32;

        /// <summary>
        /// Hash password with a new salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static HashResult Hash(String password)
        {
            // Get a new Salt
            String salt = GetANewSalt();

            // Hash the password
            String hash = Hash(password, salt);

            // Return the result
            return new HashResult
            {
                Hash = hash,
                Salt = salt
            };
        }

        /// <summary>
        /// Hash password with an existing salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static String Hash(String password, String salt)
        {
            // String to bites
            Byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            Byte[] saltBytes = Encoding.Unicode.GetBytes(salt);

            // Full bytes array to hash
            Byte[] tohashBytes = new Byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, tohashBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, tohashBytes, saltBytes.Length, passwordBytes.Length);
            
            using (var provider = SHA512.Create())
            {
                // Hash            
                Byte[] hashedBytes = provider.ComputeHash(tohashBytes);

                // Return the hash as String
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// Get a new salt
        /// </summary>
        /// <returns></returns>
        private static String GetANewSalt()
        {
            // Set salt length
            Byte[] salt = new Byte[SaltLength];
            
            using (var provider = RandomNumberGenerator.Create())
            {
                // Build the salt array
                provider.GetBytes(salt);

                // Return the salt as string
                return Convert.ToBase64String(salt);
            }
        }
    }
}
