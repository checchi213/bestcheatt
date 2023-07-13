using System;
using System.Threading;

namespace GameWer.Helper
{
	public class Timer
	{
		private Action CurrentAction;

		private Action<Exception> OnException;

		private TimeSpan Time;

		private bool HasInterval = false;

		private bool HasStop = false;

		private Timer()
		{
		}

		private void Start()
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				while (!HasStop)
				{
					Thread.Sleep(Time);
					ApplicationManager.SetTaskInMainThread(delegate
					{
						try
						{
							CurrentAction?.Invoke();
						}
						catch (Exception obj)
						{
							OnException?.Invoke(obj);
						}
					});
					if (!HasInterval)
					{
						break;
					}
				}
			});
		}

		internal void Stop()
		{
			HasStop = true;
		}

		internal static Timer Timeout(Action action, Action<Exception> exception, float timeout)
		{
			Timer timer = new Timer();
			timer.CurrentAction = action;
			timer.OnException = exception;
			timer.Time = TimeSpan.FromMilliseconds(timeout * (float)int.Parse(DeProtectType.ArgValue_276));
			timer.Start();
			return timer;
		}

		internal static Timer Interval(Action action, Action<Exception> exception, float timeout)
		{
			Timer timer = new Timer();
			timer.CurrentAction = action;
			timer.Time = TimeSpan.FromMilliseconds(timeout * (float)int.Parse(DeProtectType.ArgValue_277));
			timer.OnException = exception;
			timer.HasInterval = true;
			timer.Start();
			return timer;
		}
	}
}
