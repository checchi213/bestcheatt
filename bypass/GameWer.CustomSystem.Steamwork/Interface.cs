using GameWer.CustomSystem.Process32;
using Microsoft.Win32;
using Steamworks;
using System;
using System.Diagnostics;
using System.IO;

namespace GameWer.CustomSystem.Steamwork
{
	public class Interface
	{
		private static string SteamPath = null;

		internal static string GetSteamPath()
		{
			if (string.IsNullOrEmpty(SteamPath))
			{
				SteamPath = GetUserValue();
				if (string.IsNullOrEmpty(SteamPath))
				{
					SteamPath = GetFullValue();
					if (string.IsNullOrEmpty(SteamPath))
					{
						SteamPath = GetProcessValue();
						if (string.IsNullOrEmpty(SteamPath))
						{
							SteamPath = GetFinishValue();
						}
					}
				}
			}
			return SteamPath;
		}

		internal static bool HasSteamRunned()
		{
			try
			{
				return Process.GetProcessesByName(DeProtectType.ArgValue_294).Length != int.Parse(DeProtectType.ArgValue_295);
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_296, DeProtectType.ArgValue_297 + ex?.ToString());
			}
			return false;
		}

		internal static ulong GetSteamID()
		{
			if (string.IsNullOrEmpty(SteamPath))
			{
				GetSteamPath();
			}
			if (!string.IsNullOrEmpty(SteamPath))
			{
				string text = Path.Combine(SteamPath, DeProtectType.ArgValue_298, DeProtectType.ArgValue_299);
				if (File.Exists(text))
				{
					try
					{
						File.Copy(text, text + DeProtectType.ArgValue_300);
						string[] array = File.ReadAllLines(text + DeProtectType.ArgValue_301);
						File.Delete(text + DeProtectType.ArgValue_302);
						for (int num = array.Length - int.Parse(DeProtectType.ArgValue_303); num >= int.Parse(DeProtectType.ArgValue_310); num--)
						{
							try
							{
								string text2 = array[num];
								int num2 = text2.IndexOf(DeProtectType.ArgValue_304);
								if (num2 != -1 && text2.IndexOf(DeProtectType.ArgValue_305) != -1)
								{
									text2 = text2.Substring(num2 + int.Parse(DeProtectType.ArgValue_306) + int.Parse(DeProtectType.ArgValue_307));
									text2 = text2.Substring(int.Parse(DeProtectType.ArgValue_308), text2.IndexOf(']'));
									if (ulong.TryParse(text2, out ulong result))
									{
										return result + ulong.Parse(DeProtectType.ArgValue_309);
									}
								}
							}
							catch
							{
							}
						}
					}
					catch (Exception ex)
					{
						OutputManager.Log(DeProtectType.ArgValue_311, DeProtectType.ArgValue_312 + ex?.ToString());
					}
				}
			}
			ulong result2 = 0uL;
			try
			{
				SteamClient.Init(uint.Parse(DeProtectType.ArgValue_313));
				result2 = SteamClient.SteamId;
				SteamClient.Shutdown();
			}
			catch
			{
				try
				{
					SteamClient.Shutdown();
				}
				catch
				{
				}
			}
			return result2;
		}

		private static string GetProcessValue()
		{
			try
			{
				Process[] processesByName = Process.GetProcessesByName(DeProtectType.ArgValue_314);
				if (processesByName.Length != 0)
				{
					return new FileInfo(processesByName[int.Parse(DeProtectType.ArgValue_315)].MainModule.FileName).DirectoryName;
				}
			}
			catch
			{
			}
			return string.Empty;
		}

		private static string GetFinishValue()
		{
			try
			{
				EntryItem[] processesList = GameWer.CustomSystem.Process32.Interface.GetProcessesList();
				for (int i = 0; i < processesList.Length; i++)
				{
					if (processesList[i].Name.ToLower() == DeProtectType.ArgValue_316 && !string.IsNullOrEmpty(processesList[i].DirectoryPath))
					{
						return processesList[i].DirectoryPath;
					}
				}
			}
			catch
			{
			}
			return string.Empty;
		}

		private static string GetFullValue()
		{
			string text = "";
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(DeProtectType.ArgValue_317);
				text = (string)registryKey.GetValue(DeProtectType.ArgValue_318);
				registryKey.Close();
			}
			catch
			{
			}
			if (string.IsNullOrEmpty(text) == (int.Parse(DeProtectType.ArgValue_319) == 0))
			{
				text = new DirectoryInfo(text).FullName;
			}
			return text;
		}

		private static string GetUserValue()
		{
			string text = "";
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(DeProtectType.ArgValue_320);
				text = (string)registryKey.GetValue(DeProtectType.ArgValue_321);
				registryKey.Close();
			}
			catch
			{
			}
			if (string.IsNullOrEmpty(text) == (int.Parse(DeProtectType.ArgValue_322) == 0))
			{
				text = new DirectoryInfo(text).FullName;
			}
			return text;
		}
	}
}
