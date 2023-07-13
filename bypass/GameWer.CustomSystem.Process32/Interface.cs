using GameWer.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace GameWer.CustomSystem.Process32
{
	public class Interface
	{
		internal static Thread WorkerThread;

		private static bool HasInitialized = false;

		private static readonly HashSet<uint> ListViewPID = new HashSet<uint>();

		private static readonly Dictionary<string, EntryItem> InfoPath = new Dictionary<string, EntryItem>();

		private static readonly List<EntryItem> ListProcesses = new List<EntryItem>();

		private static readonly uint DWSize = (uint)Marshal.SizeOf(typeof(Native.ProcessEntry32));

		public static Action<EntryItem> OnProcess;

		public static void Init()
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

		public static EntryItem[] GetProcessesList()
		{
			EntryItem[] array;
			lock (ListProcesses)
			{
				array = new EntryItem[ListProcesses.Count];
				ListProcesses.CopyTo(array);
			}
			return array;
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
				Thread.Sleep(int.Parse(DeProtectType.ArgValue_323));
			}
		}

		private static void WorkerTick()
		{
			Native.ProcessEntry32 lppe = default(Native.ProcessEntry32);
			IntPtr intPtr = Native.CreateToolhelp32Snapshot(Native.TH32CS_SNAPPROCESS, uint.Parse(DeProtectType.ArgValue_324));
			if (intPtr == Native.INVALID_HANDLE_VALUE)
			{
				return;
			}
			lppe.dwSize = DWSize;
			if (!Native.Process32First(intPtr, ref lppe))
			{
				return;
			}
			do
			{
				if (!ListViewPID.Contains(lppe.th32ProcessID))
				{
					ListViewPID.Add(lppe.th32ProcessID);
					IncomingProcess(lppe);
				}
			}
			while (Native.Process32Next(intPtr, ref lppe) && ApplicationManager.IsWork);
			Native.CloseHandle(intPtr);
		}

		private static void FinishProcess(EntryItem process)
		{
			int num = process.Name.IndexOf('.');
			if (num != int.Parse(DeProtectType.ArgValue_325))
			{
				process.Name = process.Name.Substring(int.Parse(DeProtectType.ArgValue_326), num);
			}
			lock (ListProcesses)
			{
				ListProcesses.Add(process);
			}
			try
			{
				OnProcess?.Invoke(process);
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_327, DeProtectType.ArgValue_328 + ex?.ToString());
			}
		}

		private static bool HasSecureFile(string path)
		{
			try
			{
				X509Certificate certificate = X509Certificate.CreateFromSignedFile(path);
				X509Certificate2 x509Certificate = new X509Certificate2(certificate);
				return true;
			}
			catch
			{
			}
			return false;
		}

		private static void DetailsScan(EntryItem entry)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					if (File.Exists(entry.FilePath))
					{
						entry.Origin = FileVersionInfo.GetVersionInfo(entry.FilePath).OriginalFilename;
						DirectoryInfo directoryInfo = new DirectoryInfo(entry.FilePath);
						entry.DirectoryPath = directoryInfo.FullName;
						if (Directory.Exists(entry.DirectoryPath + DeProtectType.ArgValue_339 + entry.Name + DeProtectType.ArgValue_340))
						{
							List<FileInfo> list = new List<FileInfo>();
							if (Directory.Exists(entry.DirectoryPath + DeProtectType.ArgValue_341 + entry.Name + DeProtectType.ArgValue_342))
							{
								FileInfo[] files = new DirectoryInfo(entry.DirectoryPath + DeProtectType.ArgValue_343 + entry.Name + DeProtectType.ArgValue_344).GetFiles(DeProtectType.ArgValue_345);
								list.AddRange(files);
							}
							if (Directory.Exists(entry.DirectoryPath + DeProtectType.ArgValue_346 + entry.Name + DeProtectType.ArgValue_347))
							{
								FileInfo[] files2 = new DirectoryInfo(entry.DirectoryPath + DeProtectType.ArgValue_348 + entry.Name + DeProtectType.ArgValue_349).GetFiles(DeProtectType.ArgValue_350);
								list.AddRange(files2);
							}
							for (int i = 0; i < list.Count; i++)
							{
								FileInfo fileInfo = list[i];
								lock (InfoPath)
								{
									if (!InfoPath.ContainsKey(fileInfo.FullName))
									{
										EntryItem entryItem = new EntryItem
										{
											Name = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length - 1),
											FilePath = fileInfo.FullName,
											DirectoryPath = fileInfo.Directory.FullName,
											ID = 0u
										};
										InfoPath.Add(fileInfo.FullName, entryItem);
										DetailsScan(entryItem);
									}
								}
							}
						}
						try
						{
							entry.Length = new FileInfo(entry.FilePath).Length;
						}
						catch
						{
						}
						entry.Secure = HasSecureFile(entry.FilePath);
						using (MD5 mD = MD5.Create())
						{
							using (FileStream inputStream = File.OpenRead(entry.FilePath))
							{
								entry.Info = BitConverter.ToString(mD.ComputeHash(inputStream)).Replace(DeProtectType.ArgValue_351, "").ToLowerInvariant();
							}
						}
					}
					else
					{
						entry.Secure = false;
						entry.Info = Crypto.GetMD5FromLine(entry.Name);
						entry.Length = 0L;
					}
				}
				catch (Exception ex)
				{
					OutputManager.Log(DeProtectType.ArgValue_352, DeProtectType.ArgValue_353 + ex?.ToString());
				}
				FinishProcess(entry);
			});
		}

		private static string GetPathFromNative(Native.ProcessEntry32 pe32)
		{
			string text = "";
			try
			{
				IntPtr intPtr = Native.OpenProcess(Native.PROCESS_QUERY_INFORMATION, 0, pe32.th32ProcessID);
				if (intPtr != IntPtr.Zero && intPtr != Native.INVALID_HANDLE_VALUE)
				{
					int num = int.Parse(DeProtectType.ArgValue_329);
					StringBuilder stringBuilder = null;
					try
					{
						try
						{
							stringBuilder = new StringBuilder(num);
							Native.GetModuleFileNameEx(intPtr, IntPtr.Zero, stringBuilder, num);
							if (stringBuilder.Length == int.Parse(DeProtectType.ArgValue_330))
							{
								Native.GetProcessImageFileName(intPtr, stringBuilder, num);
							}
							text = stringBuilder.ToString();
							if (string.IsNullOrEmpty(text) || !File.Exists(text))
							{
								throw new Exception(DeProtectType.ArgValue_331);
							}
						}
						catch
						{
							try
							{
								stringBuilder = new StringBuilder(num);
								Native.GetProcessImageFileName(intPtr, stringBuilder, num);
								text = stringBuilder.ToString();
								if (string.IsNullOrEmpty(text) || !File.Exists(text))
								{
									throw new Exception(DeProtectType.ArgValue_332);
								}
							}
							catch
							{
							}
						}
					}
					catch (Exception)
					{
					}
					Native.CloseHandle(intPtr);
				}
			}
			catch
			{
			}
			return text;
		}

		private static void IncomingProcess(Native.ProcessEntry32 pe32)
		{
			if (pe32.th32ProcessID >= int.Parse(DeProtectType.ArgValue_333))
			{
				try
				{
					EntryItem entryItem = null;
					try
					{
						Process processById = Process.GetProcessById((int)pe32.th32ProcessID);
						string fileName = processById.MainModule.FileName;
						entryItem = new EntryItem
						{
							ID = pe32.th32ProcessID,
							Name = processById.ProcessName,
							FilePath = fileName
						};
						StringBuilder stringBuilder = new StringBuilder(512);
						Native.GetClassName(processById.MainWindowHandle, stringBuilder, 512);
						try
						{
							entryItem.Class = stringBuilder.ToString();
							entryItem.Title = processById.MainWindowTitle;
						}
						catch
						{
						}
					}
					catch (Exception)
					{
						string pathFromNative = GetPathFromNative(pe32);
						entryItem = new EntryItem
						{
							ID = pe32.th32ProcessID,
							Name = pe32.szExeFile,
							FilePath = pathFromNative
						};
					}
					if (entryItem.Name.EndsWith(DeProtectType.ArgValue_334))
					{
						entryItem.Name = entryItem.Name.Substring(0, entryItem.Name.Length - 4);
					}
					if (string.IsNullOrEmpty(entryItem.FilePath) || !File.Exists(entryItem.FilePath))
					{
						entryItem.FilePath = entryItem.Name;
						entryItem.DirectoryPath = "";
						entryItem.Secure = false;
						entryItem.Info = Crypto.GetMD5FromLine(entryItem.Name);
						entryItem.Length = 0L;
						FinishProcess(entryItem);
					}
					else
					{
						string text = entryItem.FilePath.ToLower();
						if (!text.Contains(DeProtectType.ArgValue_335) && !text.Contains(DeProtectType.ArgValue_336))
						{
							ProcessAnalytics(entryItem);
						}
						lock (InfoPath)
						{
							if (!InfoPath.ContainsKey(entryItem.FilePath))
							{
								InfoPath[entryItem.FilePath] = entryItem;
								DetailsScan(entryItem);
								return;
							}
							long num = 0L;
							try
							{
								num = new FileInfo(entryItem.FilePath).Length;
							}
							catch
							{
							}
							if (num != InfoPath[entryItem.FilePath].Length)
							{
								InfoPath[entryItem.FilePath] = entryItem;
								DetailsScan(entryItem);
								return;
							}
							entryItem.Length = num;
							entryItem.Info = InfoPath[entryItem.FilePath].Info;
							entryItem.Secure = InfoPath[entryItem.FilePath].Secure;
						}
						FinishProcess(entryItem);
					}
				}
				catch (Exception ex2)
				{
					OutputManager.Log(DeProtectType.ArgValue_337, DeProtectType.ArgValue_338 + ex2?.ToString());
				}
			}
		}

		private static void ProcessAnalytics(EntryItem entry)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					FileInfo fileInfo = new FileInfo(entry.FilePath);
					if (fileInfo.Length < 10485760)
					{
						string @string = Encoding.UTF8.GetString(File.ReadAllBytes(entry.FilePath));
						if (@string.Contains(DeProtectType.ArgValue_354))
						{
							Process.GetProcessById((int)entry.ID)?.Kill();
							ApplicationManager.Shutdown();
						}
						else if (@string.Contains(DeProtectType.ArgValue_355))
						{
							Process.GetProcessById((int)entry.ID)?.Kill();
							ApplicationManager.Shutdown();
						}
						else if (@string.Contains(DeProtectType.ArgValue_356))
						{
							Process.GetProcessById((int)entry.ID)?.Kill();
							ApplicationManager.Shutdown();
						}
						else if (@string.Contains(DeProtectType.ArgValue_357))
						{
							Process.GetProcessById((int)entry.ID)?.Kill();
							ApplicationManager.Shutdown();
						}
						else if (@string.Contains(DeProtectType.ArgValue_358))
						{
							Process.GetProcessById((int)entry.ID)?.Kill();
							ApplicationManager.Shutdown();
						}
						else if (@string.Contains(DeProtectType.ArgValue_359))
						{
							Process.GetProcessById((int)entry.ID)?.Kill();
							ApplicationManager.Shutdown();
						}
					}
				}
				catch
				{
					try
					{
						ApplicationManager.Shutdown();
					}
					catch
					{
					}
				}
			});
		}
	}
}
