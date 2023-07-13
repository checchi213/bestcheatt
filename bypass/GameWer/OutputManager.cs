using System;
using System.IO;

namespace GameWer
{
	public class OutputManager
	{
		private static OutputManager Instance
		{
			get;
		} = new OutputManager();


		private OutputManager()
		{
		}

		internal static void Log(string outputOwner, string line)
		{
			lock (Instance)
			{
				try
				{
					File.AppendAllText(DeProtectType.ArgValue_185, $"\n[{DateTime.Now}] [{outputOwner}]: " + line);
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
