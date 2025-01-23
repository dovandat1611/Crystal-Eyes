using System.Security.Cryptography;
using System.Text;

namespace Crystal_Eyes_Controller.Utils
{
	public class AesEncryption
	{
		private static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes key
		private static readonly byte[] Iv = Encoding.UTF8.GetBytes("1234567890123456");  // 16 bytes IV

		public static string Encrypt(string plainText)
		{
			using (Aes aes = Aes.Create())
			{
				aes.Key = Key;
				aes.IV = Iv;

				using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					using (var swEncrypt = new StreamWriter(csEncrypt))
					{
						swEncrypt.Write(plainText);
					}
					return Convert.ToBase64String(msEncrypt.ToArray());
				}
			}
		}

		public static string Decrypt(string cipherText)
		{
			using (Aes aes = Aes.Create())
			{
				aes.Key = Key;
				aes.IV = Iv;

				using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
				using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
				using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				using (var srDecrypt = new StreamReader(csDecrypt))
				{
					return srDecrypt.ReadToEnd();
				}
			}
		}
	}
}
