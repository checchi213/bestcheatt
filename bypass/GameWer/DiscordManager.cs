using GameWer.SDK.CustomSystem.Discord;
using System;
using System.Runtime.Remoting.Proxies;

namespace GameWer
{
	public class DiscordManager : ProxyAttribute
	{
		internal static string DSID = string.Empty;

		internal static void Init()
		{
			OutputManager.Log(DeProtectType.ArgValue_146, DeProtectType.ArgValue_147);
			try
			{
				Interface.OnIncomingDiscordAccount = OnIncomingDiscordAccount;
				Interface.Init(DeProtectType.ArgValue_148);
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_149, DeProtectType.ArgValue_150 + ex?.ToString());
			}
		}

		internal static void Shutdown()
		{
			OutputManager.Log(DeProtectType.ArgValue_151, DeProtectType.ArgValue_152);
			try
			{
				Interface.WorkerThread?.Abort();
			}
			catch
			{
			}
		}

		private static void OnIncomingDiscordAccount(string accountID)
		{
			OutputManager.Log(DeProtectType.ArgValue_153, DeProtectType.ArgValue_154 + accountID);
			DSID = accountID;
		}
	}
}
