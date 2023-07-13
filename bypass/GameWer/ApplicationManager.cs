using GameWer.CustomSystem.Information;
using GameWer.CustomSystem.Steamwork;
using GameWer.Data;
using GameWer.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace GameWer
{
	public class ApplicationManager
	{
		internal static bool IsWork = true;

		private static readonly Queue<Action> ListTaskInMainThread = new Queue<Action>();

		internal static DateTime StartApplicationTime
		{
			get;
		} = DateTime.Now;


		internal static void Init()
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_50, DeProtectType.ArgValue_51);
				InitPreInitialization();
				InitPolicyAccess();
				CheckPrimitivePrivilege();
				CheckSteam();
				GameWer.CustomSystem.Information.Interface.Init();
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_52, DeProtectType.ArgValue_53 + ex?.ToString());
			}
		}

		private static void InitPreInitialization()
		{
			OutputManager.Log(DeProtectType.ArgValue_54, DeProtectType.ArgValue_55);
			if (AppInfo.TargetConnectIP != DeProtectType.ArgValue_56)
			{
				string fileName = Process.GetCurrentProcess().MainModule.FileName;
				string directoryName = new FileInfo(fileName).DirectoryName;
				if (!ValidateObject(fileName, string.Empty))
				{
					OutputManager.Log(DeProtectType.ArgValue_57, DeProtectType.ArgValue_58);
					Shutdown();
				}
				if (!ValidateObject(Path.Combine(directoryName, DeProtectType.ArgValue_59), DeProtectType.ArgValue_60))
				{
					OutputManager.Log(DeProtectType.ArgValue_61, DeProtectType.ArgValue_62);
					Shutdown();
				}
				if (!ValidateObject(Path.Combine(directoryName, DeProtectType.ArgValue_63), DeProtectType.ArgValue_64))
				{
					OutputManager.Log(DeProtectType.ArgValue_65, DeProtectType.ArgValue_66);
					Shutdown();
				}
				if (!ValidateObject(Path.Combine(directoryName, DeProtectType.ArgValue_67), DeProtectType.ArgValue_68))
				{
					OutputManager.Log(DeProtectType.ArgValue_69, DeProtectType.ArgValue_70);
					Shutdown();
				}
				if (!ValidateObject(Path.Combine(directoryName, DeProtectType.ArgValue_71), DeProtectType.ArgValue_72))
				{
					OutputManager.Log(DeProtectType.ArgValue_73, DeProtectType.ArgValue_74);
					Shutdown();
				}
				if (!ValidateObject(Path.Combine(directoryName, DeProtectType.ArgValue_75), DeProtectType.ArgValue_76))
				{
					OutputManager.Log(DeProtectType.ArgValue_77, DeProtectType.ArgValue_78);
					Shutdown();
				}
				if (!ValidateObject(Path.Combine(directoryName, DeProtectType.ArgValue_79), DeProtectType.ArgValue_80))
				{
					OutputManager.Log(DeProtectType.ArgValue_81, DeProtectType.ArgValue_82);
					Shutdown();
				}
			}
		}

		private static bool ValidateObject(string path, string valid)
		{
			if (!string.IsNullOrEmpty(valid))
			{
				try
				{
					using (MD5 mD = MD5.Create())
					{
						using (FileStream inputStream = File.OpenRead(path))
						{
							byte[] value = mD.ComputeHash(inputStream);
							if (BitConverter.ToString(value).Replace(DeProtectType.ArgValue_83, "").ToLowerInvariant() != valid.ToLower())
							{
								return false;
							}
						}
					}
				}
				catch
				{
					return false;
				}
			}
			try
			{
				X509Certificate certificate = X509Certificate.CreateFromSignedFile(path);
				X509Certificate2 x509Certificate = new X509Certificate2(certificate);
			}
			catch
			{
				return false;
			}
			return true;
		}

		private static void InitBaseAccount()
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_84, DeProtectType.ArgValue_85);
				ulong steamID = GameWer.CustomSystem.Steamwork.Interface.GetSteamID();
				OutputManager.Log(DeProtectType.ArgValue_86, DeProtectType.ArgValue_87 + steamID.ToString());
				if (steamID == 0)
				{
					OutputManager.Log(DeProtectType.ArgValue_88, DeProtectType.ArgValue_89);
					MessageBox.Show("Ошибка работы со Steam. Перезапустите Steam от имени админа и войдите в свою учетную запись!\n\nError working with Steam. Restart Steam as administrator and log in to your account!", DeProtectType.ArgValue_90, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					Shutdown();
				}
				UIManager.ProxyForm.OnIncomingSteamID(steamID);
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_91, DeProtectType.ArgValue_92 + ex?.ToString());
			}
		}

		private static void CheckPrimitivePrivilege()
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_93, DeProtectType.ArgValue_94);
				bool flag = int.Parse(DeProtectType.ArgValue_95) == 1;
				using (WindowsIdentity ntIdentity = WindowsIdentity.GetCurrent())
				{
					WindowsPrincipal windowsPrincipal = new WindowsPrincipal(ntIdentity);
					flag = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
				}
				if (flag == (int.Parse(DeProtectType.ArgValue_96) == 1))
				{
					OutputManager.Log(DeProtectType.ArgValue_97, DeProtectType.ArgValue_98);
					MessageBox.Show("Для работы античита - необходимо запустить античит от имени администратора.\n\nFor anti-cheat to work, you must run anti-cheat on behalf of the administrator.", DeProtectType.ArgValue_99, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					Shutdown();
				}
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_100, DeProtectType.ArgValue_101 + ex?.ToString());
			}
		}

		private static void CheckSteam()
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_102, DeProtectType.ArgValue_103);
				if (GameWer.CustomSystem.Steamwork.Interface.HasSteamRunned() == (int.Parse(DeProtectType.ArgValue_104) == 1))
				{
					OutputManager.Log(DeProtectType.ArgValue_105, DeProtectType.ArgValue_106);
					MessageBox.Show("Для работы античита - необходимо запустить Steam и авторизоваться в нем. Если Steam у вас запущен, перезапустите его от имени администратора.\n\nFor anti-cheat to work, you need to start Steam and log in to it. If you have Steam running, restart it as administrator.", DeProtectType.ArgValue_107, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					Shutdown();
				}
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_108, DeProtectType.ArgValue_109 + ex?.ToString());
			}
		}

		private static void InitPolicyAccess()
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_110, DeProtectType.ArgValue_111);
				bool flag = int.Parse(DeProtectType.ArgValue_112) == 1;
				try
				{
					if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DeProtectType.ArgValue_113)))
					{
						flag = (int.Parse(DeProtectType.ArgValue_114) == 1);
					}
				}
				catch
				{
				}
				if (!flag)
				{
					OutputManager.Log(DeProtectType.ArgValue_115, DeProtectType.ArgValue_116);
					DialogResult dialogResult = MessageBox.Show("Вы даете согласие на сбор и обработку ваших персональных данных и информации собраной с этого устройства для автоматического анализа и предоставления администраторам игровых серверов?\n\nDo you consent to the collection and processing of your personal data and information collected from this device to automatically analyze and provide game server administrators?", DeProtectType.ArgValue_117, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
					if (dialogResult == DialogResult.No)
					{
						OutputManager.Log(DeProtectType.ArgValue_118, DeProtectType.ArgValue_119);
						MessageBox.Show("Данная программа является античитом, и без сбора информации к сожалению - не может работоать. Так как - вы не дали согласия, данная программа(античит) будет закрыта.\n\nThis program is an anti-cheat, and unfortunately it cannot work without collecting information. Since - you did not give consent, this program (anti-cheat) will be closed.", DeProtectType.ArgValue_120, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						Shutdown();
					}
					else
					{
						OutputManager.Log(DeProtectType.ArgValue_121, DeProtectType.ArgValue_122);
						File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DeProtectType.ArgValue_123), DateTime.Now.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_124, DeProtectType.ArgValue_125 + ex?.ToString());
			}
		}

		internal static void Start()
		{
			InitBaseAccount();
			UIManager.ProxyForm.OnApplicationState(DeProtectType.ArgValue_126);
		}

		internal static void StartApplicationWorker()
		{
			OutputManager.Log(DeProtectType.ArgValue_127, DeProtectType.ArgValue_128);
			GameWer.Helper.Timer.Timeout(OnApplicationInitialized, OnApplicationInitializationException, 10f);
			while (IsWork)
			{
				try
				{
					Action action = null;
					while ((action = GetTaskInMainThread()) != null)
					{
						try
						{
							action?.Invoke();
						}
						catch (Exception ex)
						{
							OutputManager.Log(DeProtectType.ArgValue_129, DeProtectType.ArgValue_130 + ((action != null) ? action.Method.ToString() : DeProtectType.ArgValue_131) + DeProtectType.ArgValue_132 + ex?.ToString());
						}
					}
				}
				catch (Exception ex2)
				{
					OutputManager.Log(DeProtectType.ArgValue_133, DeProtectType.ArgValue_134 + ex2?.ToString());
				}
				Thread.Sleep(33);
			}
		}

		private static void OnApplicationInitializationException(Exception exception)
		{
			OutputManager.Log(DeProtectType.ArgValue_135, DeProtectType.ArgValue_136 + exception?.ToString());
		}

		private static void OnApplicationInitialized()
		{
			OutputManager.Log(DeProtectType.ArgValue_137, DeProtectType.ArgValue_138);
			NetworkManager.Start();
		}

		internal static void Shutdown()
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_139, DeProtectType.ArgValue_140);
				IsWork = (int.Parse(DeProtectType.ArgValue_141) == 1);
				KeyManager.Shutdown();
				ProcessManager.Shutdown();
				DiscordManager.Shutdown();
				UIManager.Shutdown();
				NetworkManager.Shutdown();
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_142, DeProtectType.ArgValue_143 + ex?.ToString());
			}
			Environment.Exit(int.Parse(DeProtectType.ArgValue_144));
		}

		internal static void SetTaskInMainThread(Action action)
		{
			lock (ListTaskInMainThread)
			{
				ListTaskInMainThread.Enqueue(action);
			}
		}

		internal static Action GetTaskInMainThread()
		{
			Action result = null;
			lock (ListTaskInMainThread)
			{
				if (ListTaskInMainThread.Count > int.Parse(DeProtectType.ArgValue_145))
				{
					result = ListTaskInMainThread.Dequeue();
				}
			}
			return result;
		}
	}
}
