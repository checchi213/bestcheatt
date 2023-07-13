using GameWer.Data;
using System;
using System.IO;
using System.Threading;

namespace GameWer
{
	internal static class Bootstrap
	{
		private static Mutex MutexInstance;

		[STAThread]
		private static void Main(string[] args)
		{
			try
			{
				DeProtectType.DeProtect(args);
				DoInit();
			}
			catch (Exception ex)
			{
				File.AppendAllText("./output.log", $"\n[{DateTime.Now}] [Main]: Exception: " + ex?.ToString());
			}
		}

		private static void DoInit()
		{
			OutputManager.Log(DeProtectType.ArgValue_0, DeProtectType.ArgValue_1);
			MutexInstance = new Mutex(initiallyOwned: true, DeProtectType.ArgValue_2);
			if (MutexInstance.WaitOne(TimeSpan.Zero, exitContext: true) || AppInfo.TargetConnectIP == DeProtectType.ArgValue_3)
			{
				ApplicationManager.Init();
				UIManager.Init();
				ProcessManager.Init();
				KeyManager.Init();
				DiscordManager.Init();
				NetworkManager.Init();
				ApplicationManager.Start();
				ApplicationManager.StartApplicationWorker();
				MutexInstance.ReleaseMutex();
				OutputManager.Log(DeProtectType.ArgValue_4, DeProtectType.ArgValue_5);
			}
			else
			{
				OutputManager.Log(DeProtectType.ArgValue_6, DeProtectType.ArgValue_7);
			}
		}
	}
}
