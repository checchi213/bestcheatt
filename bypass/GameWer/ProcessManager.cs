using GameWer.CustomSystem.Process32;
using GameWer.Data;
using GameWer.Helper;
using GameWer.Struct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GameWer
{
	public class ProcessManager
	{
		internal static HashSet<string> ListSendPath = null;

		internal static void Init()
		{
			OutputManager.Log(DeProtectType.ArgValue_186, DeProtectType.ArgValue_187);
			PreStartCheck();
			Interface.OnProcess = OnProcessIncoming;
			Interface.Init();
			Timer.Interval(DoSendNewProcesses, delegate
			{
			}, 15f);
		}

		private static void PreStartCheck()
		{
			OutputManager.Log(DeProtectType.ArgValue_188, DeProtectType.ArgValue_189);
			Process[] processes = Process.GetProcesses();
			for (int i = 0; i < processes.Length; i++)
			{
				try
				{
					string fullName = new FileInfo(processes[i].MainModule.FileName).Directory.FullName;
					if (File.Exists(fullName + DeProtectType.ArgValue_190))
					{
						processes[i].Kill();
						Environment.Exit(0);
						return;
					}
				}
				catch
				{
				}
			}
		}

		internal static void Shutdown()
		{
			OutputManager.Log(DeProtectType.ArgValue_191, DeProtectType.ArgValue_192);
			try
			{
				Interface.WorkerThread.Abort();
			}
			catch
			{
			}
		}

		private static void OnProcessIncoming(EntryItem process)
		{
			FindAndKillGame(process);
			if (!AntiBanKiller(process) && ListSendPath != null)
			{
				lock (ListSendPath)
				{
					if (!ListSendPath.Contains(process.FilePath))
					{
						ListSendPath.Add(process.FilePath);
						NetworkPlayerProcessesPacket networkPlayerProcessesPacket = new NetworkPlayerProcessesPacket();
						networkPlayerProcessesPacket.Processes = new PlayerProcess[1]
						{
							new PlayerProcess
							{
								Hash = (string.IsNullOrEmpty(process.Info) ? Crypto.GetMD5FromLine(process.Name) : process.Info),
								Name = process.Name,
								Path = (string.IsNullOrEmpty(process.FilePath) ? process.Name : process.FilePath),
								Secure = process.Secure,
								Size = (int)(process.Length / 1024),
								Class = process.Class,
								Title = process.Title,
								Origin = process.Origin
							}
						};
						NetworkManager.Send(networkPlayerProcessesPacket.ParseJSON());
					}
				}
			}
		}

		internal static void DoSendNewProcesses()
		{
			if (ListSendPath != null)
			{
				EntryItem[] processesList = Interface.GetProcessesList();
				List<PlayerProcess> list = new List<PlayerProcess>();
				for (int i = 0; i < processesList.Length; i++)
				{
					try
					{
						lock (ListSendPath)
						{
							if (!ListSendPath.Contains(processesList[i].FilePath))
							{
								ListSendPath.Add(processesList[i].FilePath);
								list.Add(new PlayerProcess
								{
									Hash = processesList[i].Info,
									Name = processesList[i].Name,
									Path = processesList[i].FilePath,
									Secure = processesList[i].Secure,
									Size = (int)(processesList[i].Length / 1024),
									Class = processesList[i].Class,
									Title = processesList[i].Title,
									Origin = processesList[i].Origin
								});
							}
						}
					}
					catch (Exception ex)
					{
						OutputManager.Log(DeProtectType.ArgValue_193, DeProtectType.ArgValue_194 + ex?.ToString());
					}
				}
				if (list.Count > 0)
				{
					NetworkPlayerProcessesPacket networkPlayerProcessesPacket = new NetworkPlayerProcessesPacket();
					networkPlayerProcessesPacket.Processes = list.ToArray();
					NetworkManager.Send(networkPlayerProcessesPacket.ParseJSON());
				}
			}
		}

		private static bool AntiBanKiller(EntryItem process)
		{
			string text = process.Name.ToLower();
			for (int i = 0; i < Processes.AntiBanProcesses.Length; i++)
			{
				if (text.StartsWith(Processes.AntiBanProcesses[i]))
				{
					OutputManager.Log(DeProtectType.ArgValue_195, DeProtectType.ArgValue_196 + process.Name);
					return KillProcess(process);
				}
			}
			return false;
		}

		private static bool KillProcess(EntryItem process)
		{
			if (process.ID != 0)
			{
				OutputManager.Log(DeProtectType.ArgValue_197, DeProtectType.ArgValue_198 + process.Name + DeProtectType.ArgValue_199);
				try
				{
					Process processById = Process.GetProcessById((int)process.ID);
					processById.Kill();
					return true;
				}
				catch
				{
					try
					{
						Process.Start(DeProtectType.ArgValue_200, DeProtectType.ArgValue_201 + process.ID.ToString());
						return true;
					}
					catch (Exception)
					{
					}
				}
			}
			return false;
		}

		private static void FindAndKillGame(EntryItem process)
		{
			int num = (int)DateTime.Now.Subtract(ApplicationManager.StartApplicationTime).TotalSeconds;
			if (num < int.Parse(DeProtectType.ArgValue_202) && string.IsNullOrEmpty(process.DirectoryPath) == (int.Parse(DeProtectType.ArgValue_203) == 1) && Directory.Exists(process.DirectoryPath + DeProtectType.ArgValue_204 + process.Name + DeProtectType.ArgValue_205))
			{
				OutputManager.Log(DeProtectType.ArgValue_206, DeProtectType.ArgValue_207 + process.DirectoryPath);
				KillProcess(process);
			}
		}
	}
}
