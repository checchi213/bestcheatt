using GameWer.CustomSystem.KeyLogger;
using System.Windows.Forms;

namespace GameWer
{
	public class KeyManager
	{
		internal static void Init()
		{
			OutputManager.Log(DeProtectType.ArgValue_155, DeProtectType.ArgValue_156);
			Interface.OnKeyPress = OnKeyState;
			Interface.Init();
		}

		internal static void Shutdown()
		{
			OutputManager.Log(DeProtectType.ArgValue_157, DeProtectType.ArgValue_158);
			try
			{
				Interface.WorkerThread?.Abort();
			}
			catch
			{
			}
		}

		private static void OnKeyState(Keys key)
		{
			ApplicationManager.SetTaskInMainThread(delegate
			{
			});
		}
	}
}
