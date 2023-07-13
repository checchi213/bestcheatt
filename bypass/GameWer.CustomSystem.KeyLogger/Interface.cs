using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace GameWer.CustomSystem.KeyLogger
{
	public class Interface
	{
		internal static Thread WorkerThread;

		private static bool HasInitialized = false;

		private static HashSet<Keys> ListActiveKeys = new HashSet<Keys>();

		internal static Action<Keys> OnKeyPress;

		internal static void Init()
		{
			if (!HasInitialized)
			{
				HasInitialized = true;
				WorkerThread = new Thread(WorkerUpdater);
				WorkerThread.IsBackground = true;
				WorkerThread.Priority = ThreadPriority.Highest;
				WorkerThread.Start();
			}
		}

		private static void WorkerUpdater()
		{
			while (ApplicationManager.IsWork)
			{
				try
				{
					WorkerTick();
				}
				catch
				{
				}
				Thread.Sleep(int.Parse(DeProtectType.ArgValue_360));
			}
		}

		private static void WorkerTick()
		{
			UpdateKeyState(Keys.Insert);
		}

		private static void UpdateKeyState(Keys key)
		{
			bool flag = Native.GetAsyncKeyState((int)key) != int.Parse(DeProtectType.ArgValue_361);
			if (flag && !ListActiveKeys.Contains(key))
			{
				ListActiveKeys.Add(key);
				try
				{
					OnKeyPress?.Invoke(key);
				}
				catch (Exception ex)
				{
					OutputManager.Log(DeProtectType.ArgValue_362, DeProtectType.ArgValue_363 + ex?.ToString());
				}
			}
			else if (!flag && ListActiveKeys.Contains(key))
			{
				ListActiveKeys.Remove(key);
			}
		}
	}
}
