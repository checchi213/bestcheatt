using GameWer.SDK;
using System;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace GameWer
{
	public class UIManager
	{
		internal static Thread UIThread;

		internal static AppDomain UIDomain = null;

		internal static GameWerUI GameWerUIInstance;

		internal static IGameWerForm ProxyForm;

		private static PermissionSet CreateDomainPermission()
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.AllFlags));
			return permissionSet;
		}

		private static AppDomainSetup CreateDomainSetup()
		{
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			return appDomainSetup;
		}

		private static void InitThread()
		{
			try
			{
				Type typeFromHandle = typeof(GameWerUI);
				GameWerUIInstance = (GameWerUI)UIDomain.CreateInstanceAndUnwrap(typeFromHandle.Assembly.FullName, typeFromHandle.FullName);
				ProxyForm = new GameWerProxy(GameWerUIInstance, UIDomain);
				GameWerUIInstance.InitUI();
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_208, DeProtectType.ArgValue_209 + ex?.ToString());
			}
			ApplicationManager.Shutdown();
		}

		internal static void Init()
		{
			OutputManager.Log(DeProtectType.ArgValue_210, DeProtectType.ArgValue_211);
			try
			{
				PermissionSet permissionSet = CreateDomainPermission();
				AppDomainSetup info = CreateDomainSetup();
				UIDomain = AppDomain.CreateDomain(DeProtectType.ArgValue_212, null, info);
				UIThread = new Thread(InitThread);
				UIThread.SetApartmentState(ApartmentState.STA);
				UIThread.Start();
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_213, DeProtectType.ArgValue_214 + ex?.ToString());
				ApplicationManager.Shutdown();
			}
		}

		internal static void Shutdown()
		{
			OutputManager.Log(DeProtectType.ArgValue_215, DeProtectType.ArgValue_216);
			try
			{
				UIThread?.Abort();
			}
			catch
			{
			}
			try
			{
				AppDomain.Unload(UIDomain);
			}
			catch
			{
			}
		}
	}
}
