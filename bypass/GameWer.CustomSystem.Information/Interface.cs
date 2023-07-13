using GameWer.Helper;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace GameWer.CustomSystem.Information
{
	public class Interface
	{
		internal static string GetHWID = string.Empty;

		internal static string PCID = string.Empty;

		internal static string Model = string.Empty;

		internal static string Manufacturer = string.Empty;

		internal static string ProductName = string.Empty;

		internal static string RegisteredOrganization = string.Empty;

		internal static string RegisteredOwner = string.Empty;

		internal static string SystemRoot = string.Empty;

		internal static string MachineName = string.Empty;

		internal static string UserName = string.Empty;

		internal static bool IsBit64OS = false;

		internal static string MemorySize = string.Empty;

		internal static string ProcessorName = string.Empty;

		internal static string ProcessorID = string.Empty;

		internal static string VideocardName = string.Empty;

		internal static string VideocardID = string.Empty;

		internal static string DriversName = string.Empty;

		internal static string DriversSize = string.Empty;

		internal static List<string> GetHWIDList
		{
			get;
		} = new List<string>();


		internal static void Init()
		{
			InitHWIDList();
			InitHWID();
			InitOther();
		}

		private static void InitOther()
		{
			DriversName = string.Join(DeProtectType.ArgValue_364, Environment.GetLogicalDrives());
			try
			{
				uint num = uint.Parse(DeProtectType.ArgValue_365);
				DriveInfo[] drives = DriveInfo.GetDrives();
				for (int i = 0; i < drives.Length; i++)
				{
					try
					{
						num = (uint)((int)num + (int)(drives[i].TotalSize / int.Parse(DeProtectType.ArgValue_366) / int.Parse(DeProtectType.ArgValue_367)));
					}
					catch
					{
					}
				}
				DriversSize = num.ToString();
			}
			catch (Exception)
			{
			}
			try
			{
				ManagementClass managementClass = new ManagementClass(DeProtectType.ArgValue_368);
				ManagementObjectCollection instances = managementClass.GetInstances();
				foreach (ManagementBaseObject item in instances)
				{
					foreach (PropertyData property in item.Properties)
					{
						string name = property.Name;
						string text = name;
						if (text != null)
						{
							if (!(text == DeProtectType.ArgValue_369))
							{
								if (text == DeProtectType.ArgValue_370 || text == DeProtectType.ArgValue_371 || text == DeProtectType.ArgValue_372)
								{
									ProcessorID += property.Value.ToString();
								}
							}
							else
							{
								ProcessorName = property.Value.ToString();
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			try
			{
				ManagementClass managementClass2 = new ManagementClass(DeProtectType.ArgValue_373);
				ManagementObjectCollection instances2 = managementClass2.GetInstances();
				foreach (ManagementBaseObject item2 in instances2)
				{
					try
					{
						foreach (PropertyData property2 in item2.Properties)
						{
							try
							{
								string name2 = property2.Name;
								string text2 = name2;
								if (text2 != null)
								{
									if (!(text2 == DeProtectType.ArgValue_374))
									{
										if (text2 == DeProtectType.ArgValue_375)
										{
											VideocardID = property2.Value.ToString();
										}
									}
									else
									{
										VideocardName = property2.Value.ToString();
									}
								}
							}
							catch
							{
							}
						}
					}
					catch
					{
					}
				}
			}
			catch (Exception)
			{
				Console.WriteLine(DeProtectType.ArgValue_376);
			}
			try
			{
				Model = Registry.GetValue(DeProtectType.ArgValue_377, DeProtectType.ArgValue_378, DeProtectType.ArgValue_379).ToString();
			}
			catch
			{
			}
			try
			{
				Manufacturer = Registry.GetValue(DeProtectType.ArgValue_380, DeProtectType.ArgValue_381, DeProtectType.ArgValue_382).ToString();
			}
			catch
			{
			}
			try
			{
				ProductName = Registry.GetValue(DeProtectType.ArgValue_383, DeProtectType.ArgValue_384, DeProtectType.ArgValue_385).ToString();
			}
			catch
			{
			}
			try
			{
				RegisteredOrganization = Registry.GetValue(DeProtectType.ArgValue_386, DeProtectType.ArgValue_387, DeProtectType.ArgValue_388).ToString();
			}
			catch
			{
			}
			try
			{
				RegisteredOwner = Registry.GetValue(DeProtectType.ArgValue_389, DeProtectType.ArgValue_390, DeProtectType.ArgValue_391).ToString();
			}
			catch
			{
			}
			try
			{
				SystemRoot = Registry.GetValue(DeProtectType.ArgValue_392, DeProtectType.ArgValue_393, DeProtectType.ArgValue_394).ToString();
			}
			catch
			{
			}
			try
			{
				MachineName = Environment.MachineName;
			}
			catch
			{
			}
			try
			{
				UserName = Environment.UserName;
			}
			catch
			{
			}
			try
			{
				IsBit64OS = Environment.Is64BitOperatingSystem;
			}
			catch
			{
			}
			try
			{
				MemorySize = ((int)(new ComputerInfo().TotalPhysicalMemory / ulong.Parse(DeProtectType.ArgValue_395) / ulong.Parse(DeProtectType.ArgValue_396))).ToString();
			}
			catch
			{
			}
			try
			{
				PCID = Crypto.GetMD5FromLine(MemorySize + DriversSize + DriversName + VideocardName + ProcessorName);
			}
			catch
			{
			}
		}

		private static void InitHWID()
		{
			if (GetHWIDList.Count > int.Parse(DeProtectType.ArgValue_397))
			{
				GetHWID = Crypto.GetMD5FromLine(GetHWIDList[int.Parse(DeProtectType.ArgValue_398)] + ((GetHWIDList.Count > int.Parse(DeProtectType.ArgValue_399)) ? GetHWIDList[int.Parse(DeProtectType.ArgValue_400)] : ""));
			}
		}

		private static void InitHWIDList()
		{
			string[] source = new string[3]
			{
				DeProtectType.ArgValue_401,
				DeProtectType.ArgValue_402,
				DeProtectType.ArgValue_403
			};
			try
			{
				StreamReader streamReader = ExecuteCommandLine(DeProtectType.ArgValue_404, DeProtectType.ArgValue_405);
				for (int i = 0; i < int.Parse(DeProtectType.ArgValue_406); i++)
				{
					streamReader.ReadLine();
				}
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine();
					if (text != null)
					{
						text = text.Trim();
						while (text.Contains(DeProtectType.ArgValue_409))
						{
							text = text.Replace(DeProtectType.ArgValue_407, DeProtectType.ArgValue_408);
						}
						string[] array = text.Trim().Split(' ');
						if (array.Length == int.Parse(DeProtectType.ArgValue_410))
						{
							string text2 = array[int.Parse(DeProtectType.ArgValue_411)];
							string text3 = array[int.Parse(DeProtectType.ArgValue_412)];
							if (!source.Contains(text3))
							{
								string[] array2 = text2.Split(new char[1]
								{
									'.'
								}, StringSplitOptions.RemoveEmptyEntries);
								if (array2.Length != int.Parse(DeProtectType.ArgValue_413) || ((!(array2[1] == DeProtectType.ArgValue_414) || !(array2[2] == DeProtectType.ArgValue_415)) && (!(array2[1] == DeProtectType.ArgValue_416) || !(array2[2] == DeProtectType.ArgValue_417))))
								{
									GetHWIDList.Add(Crypto.GetMD5FromLine(text3));
								}
							}
						}
					}
				}
			}
			catch
			{
			}
			string text4 = (from nic in NetworkInterface.GetAllNetworkInterfaces()
				where nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
				select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
			if (!source.Contains(text4))
			{
				GetHWIDList.Add(Crypto.GetMD5FromLine(text4));
			}
		}

		private static StreamReader ExecuteCommandLine(string file, string arguments = "")
		{
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				FileName = file,
				Arguments = arguments
			};
			return Process.Start(startInfo)?.StandardOutput;
		}
	}
}
