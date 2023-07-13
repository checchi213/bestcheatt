using System.Security.Cryptography;
using System.Text;

namespace GameWer.Helper
{
	public class Crypto
	{
		public static string GetMD5FromLine(string input)
		{
			using (MD5 mD = MD5.Create())
			{
				byte[] bytes = Encoding.ASCII.GetBytes(input);
				byte[] array = mD.ComputeHash(bytes);
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString(DeProtectType.ArgValue_268));
				}
				return stringBuilder.ToString().ToLower();
			}
		}
	}
}
