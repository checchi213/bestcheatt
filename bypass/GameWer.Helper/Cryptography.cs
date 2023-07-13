using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GameWer.Helper
{
	public class Cryptography
	{
		public static string OpenSSLEncrypt(string plainText, string passphrase)
		{
			byte[] array = new byte[8];
			RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
			rNGCryptoServiceProvider.GetNonZeroBytes(array);
			DeriveKeyAndIV(passphrase, array, out byte[] key, out byte[] iv);
			byte[] array2 = EncryptStringToBytesAes(plainText, key, iv);
			byte[] array3 = new byte[array.Length + array2.Length + 8];
			Buffer.BlockCopy(Encoding.ASCII.GetBytes(DeProtectType.ArgValue_269), 0, array3, 0, 8);
			Buffer.BlockCopy(array, 0, array3, 8, array.Length);
			Buffer.BlockCopy(array2, 0, array3, array.Length + 8, array2.Length);
			return Convert.ToBase64String(array3);
		}

		public static string OpenSSLDecrypt(string encrypted, string passphrase)
		{
			byte[] array = Convert.FromBase64String(encrypted);
			byte[] array2 = new byte[8];
			byte[] array3 = new byte[array.Length - array2.Length - 8];
			Buffer.BlockCopy(array, 8, array2, 0, array2.Length);
			Buffer.BlockCopy(array, array2.Length + 8, array3, 0, array3.Length);
			DeriveKeyAndIV(passphrase, array2, out byte[] key, out byte[] iv);
			return DecryptStringFromBytesAes(array3, key, iv);
		}

		private static void DeriveKeyAndIV(string passphrase, byte[] salt, out byte[] key, out byte[] iv)
		{
			List<byte> list = new List<byte>(48);
			byte[] bytes = Encoding.UTF8.GetBytes(passphrase);
			byte[] array = new byte[0];
			MD5 mD = MD5.Create();
			bool flag = false;
			while (!flag)
			{
				int num = array.Length + bytes.Length + salt.Length;
				byte[] array2 = new byte[num];
				Buffer.BlockCopy(array, 0, array2, 0, array.Length);
				Buffer.BlockCopy(bytes, 0, array2, array.Length, bytes.Length);
				Buffer.BlockCopy(salt, 0, array2, array.Length + bytes.Length, salt.Length);
				array = mD.ComputeHash(array2);
				list.AddRange(array);
				if (list.Count >= 48)
				{
					flag = true;
				}
			}
			key = new byte[32];
			iv = new byte[16];
			list.CopyTo(0, key, 0, 32);
			list.CopyTo(32, iv, 0, 16);
			mD.Clear();
			mD = null;
		}

		private static byte[] EncryptStringToBytesAes(string plainText, byte[] key, byte[] iv)
		{
			if (plainText == null || plainText.Length <= 0)
			{
				throw new ArgumentNullException(DeProtectType.ArgValue_270);
			}
			if (key == null || key.Length == 0)
			{
				throw new ArgumentNullException(DeProtectType.ArgValue_271);
			}
			if (iv == null || iv.Length == 0)
			{
				throw new ArgumentNullException(DeProtectType.ArgValue_272);
			}
			RijndaelManaged rijndaelManaged = null;
			MemoryStream memoryStream;
			try
			{
				rijndaelManaged = new RijndaelManaged
				{
					Mode = CipherMode.CBC,
					KeySize = 256,
					BlockSize = 128,
					Key = key,
					IV = iv
				};
				ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
				memoryStream = new MemoryStream();
				using (CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
				{
					using (StreamWriter streamWriter = new StreamWriter(stream))
					{
						streamWriter.Write(plainText);
						streamWriter.Flush();
						streamWriter.Close();
					}
				}
			}
			finally
			{
				rijndaelManaged?.Clear();
			}
			return memoryStream.ToArray();
		}

		private static string DecryptStringFromBytesAes(byte[] cipherText, byte[] key, byte[] iv)
		{
			if (cipherText == null || cipherText.Length == 0)
			{
				throw new ArgumentNullException(DeProtectType.ArgValue_273);
			}
			if (key == null || key.Length == 0)
			{
				throw new ArgumentNullException(DeProtectType.ArgValue_274);
			}
			if (iv == null || iv.Length == 0)
			{
				throw new ArgumentNullException(DeProtectType.ArgValue_275);
			}
			RijndaelManaged rijndaelManaged = null;
			string result;
			try
			{
				rijndaelManaged = new RijndaelManaged
				{
					Mode = CipherMode.CBC,
					KeySize = 256,
					BlockSize = 128,
					Key = key,
					IV = iv
				};
				ICryptoTransform transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
				using (MemoryStream stream = new MemoryStream(cipherText))
				{
					using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader(stream2))
						{
							result = streamReader.ReadToEnd();
							streamReader.Close();
						}
					}
				}
			}
			finally
			{
				rijndaelManaged?.Clear();
			}
			return result;
		}
	}
}
