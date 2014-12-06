using System.IO;
using System.Security.Cryptography;
using System;

namespace Loveii.Utils
{
    /// <summary>
    /// 文件加解密帮助类
    /// </summary>
    public class CryptoHelper
    {
        /// <summary>
        /// des加密
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="password">文件密码</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] byteBuffer, string password)
        {
            if ((byteBuffer == null) || (password == null)) return null;

            DES des = new DESCryptoServiceProvider();
            CreateKey_IV(des, password);
            return CryptoTransform(des.CreateEncryptor(), byteBuffer);
        }

        /// <summary>
        /// des解密
        /// </summary>
        /// <param name="encryptBuffer"></param>
        /// <param name="password">文件密码</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] encryptBuffer, string password)
        {
            if ((encryptBuffer == null) || (password == null)) return null;

            DES des = new DESCryptoServiceProvider();
            CreateKey_IV(des, password);
            return CryptoTransform(des.CreateDecryptor(), encryptBuffer);
        }

        private static void CreateKey_IV(SymmetricAlgorithm key, string password)
        {
            int nKeySize = key.LegalKeySizes[key.LegalKeySizes.Length - 1].MaxSize / 8;
            int nBlockSize = key.LegalBlockSizes[key.LegalBlockSizes.Length - 1].MaxSize / 8;
            byte[] desKey = new byte[nKeySize];
            byte[] desIV = new byte[nBlockSize];

            int salt = 0x5a770872;
            int i;
            byte k, s;
            for (i = 0; i < nKeySize; i++)
            {
                s = (byte)((salt & (0xff << ((i % 4) * 8))) >> (((i % 4) * 8)));
                k = (byte)password[i % password.Length];
                desKey[i] = (byte)(k ^ s);
            }

            for (i = 0; i < nBlockSize; i++)
            {
                s = (byte)((salt & (0xff << ((i % 4) * 8))) >> (((i % 4) * 8)));
                k = (byte)desKey[(i + 1) % desKey.Length];
                desIV[i] = (byte)(k ^ s);
            }

            key.Key = desKey;
            key.IV = desIV;
        }

        private static byte[] CryptoTransform(ICryptoTransform transform, byte[] source)
        {
            try
            {
                byte[] buffer = null;
                using (MemoryStream stream = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                {
                    cs.Write(source, 0, source.Length);
                    cs.FlushFinalBlock();
                    buffer = stream.ToArray();
                }

                return buffer;
            }
            catch (Exception ex)
            {
                throw new Exception("Res密码错误！[" + ex.Message + "]");
            }
        }
    }
}
