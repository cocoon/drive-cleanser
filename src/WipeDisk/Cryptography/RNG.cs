using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DriveCleanser.Cryptography
{
    internal static class RNG
    {
        /// <summary>
        /// Fills a byte array with Cryptographically secure random data
        /// </summary>
        /// <param name="arrayLength"></param>
        /// <returns></returns>
        internal static byte[] FillByteArray(int arrayLength)
        {
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[arrayLength];
            rng.GetBytes(tokenData);
            return tokenData;
        }

        /// <summary>
        /// Fills the byte array with the Department of Defense drive wiping pattern (0x00, 0xFF, Random Byte)
        /// </summary>
        /// <param name="arrayLength">Length of the array.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static byte[] FillDoD5220ByteArray(int arrayLength)
        {
            int location = 0;
            RandomNumberGenerator rng = null;
            byte[] tokenData = new byte[arrayLength];
            for (int i=0;i<arrayLength;i++)
            {
                location++;
                if (location > 3) location = 1;
                switch (location)
                {
                    case 1:
                        tokenData[i] = 0x00;
                        break;
                    case 2:
                        tokenData[i] = 0x01;
                        break;
                    case 3:
                        rng = new RNGCryptoServiceProvider();
                        byte[] single = new byte[1];
                        rng.GetBytes(single);
                        tokenData[i] = single[0];
                        break;
                }
            }
            return tokenData;
        }

        /// <summary>
        /// Fills the byte array with the Department of Defense drive wiping pattern ()
        /// </summary>
        /// <param name="arrayLength">Length of the array.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static byte[] FillDoD5200ByteArray(int arrayLength)
        {
            int location = 0;
            RandomNumberGenerator rng = null;
            byte[] tokenData = new byte[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                location++;
                if (location > 7) location = 1;
                switch (location)
                {
                    case 1:
                        tokenData[i] = 0x35;
                        break;
                    case 2:
                        tokenData[i] = 0xCA;
                        break;
                    case 3:
                        tokenData[i] = 0x97;
                        break;
                    case 4:
                       tokenData[i] = 0x68;
                        break;
                    case 5:
                        tokenData[i] = 0xAC;
                        break;
                    case 6:
                     tokenData[i] = 0x53;
                        break;
                    case 7:
                        rng = new RNGCryptoServiceProvider();
                        byte[] single = new byte[1];
                        rng.GetBytes(single);
                        tokenData[i] = single[0];
                        break;
                }
            }
            return tokenData;
        }
    }
}
