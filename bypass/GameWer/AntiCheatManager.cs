using GameWer.CustomSystem.Information;
using GameWer.CustomSystem.Steamwork;
using GameWer.Helper;
using GameWer.Struct;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace GameWer
{
	public class AntiCheatManager
	{
		internal static string LastKeySession = string.Empty;

		internal static string LastPublicKey = string.Empty;

		internal static string LastPrivateKey = string.Empty;

		private static BannedPlayerResultPacket CurrentBannedPlayerResultPacket
		{
			get;
			set;
		} = null;


		internal static void OnNetworkAuthResultPacket(NetworkAuthResultPacket packet)
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_8, DeProtectType.ArgValue_9 + packet.Result.ToString());
				LastKeySession = packet.SessionKey;
				LastPrivateKey = packet.PrivateKey;
				UIManager.ProxyForm.OnNetworkAuthSuccess();
				if (CurrentBannedPlayerResultPacket != null)
				{
					OnNetworkBannedPlayerPacket(CurrentBannedPlayerResultPacket);
				}
				string packet2 = new NetworkDetailsPlayerPacket
				{
					Hwid_list = string.Join(DeProtectType.ArgValue_10, GameWer.CustomSystem.Information.Interface.GetHWIDList),
					Modle = GameWer.CustomSystem.Information.Interface.Model,
					Driversname = GameWer.CustomSystem.Information.Interface.DriversName,
					Driverssize = int.Parse(GameWer.CustomSystem.Information.Interface.DriversSize),
					Machinename = GameWer.CustomSystem.Information.Interface.MachineName,
					Manufacturer = GameWer.CustomSystem.Information.Interface.Manufacturer,
					Memorysize = int.Parse(GameWer.CustomSystem.Information.Interface.MemorySize),
					Organization = GameWer.CustomSystem.Information.Interface.RegisteredOrganization,
					Owner = GameWer.CustomSystem.Information.Interface.RegisteredOwner,
					Processorid = GameWer.CustomSystem.Information.Interface.ProcessorID,
					Processorname = GameWer.CustomSystem.Information.Interface.ProcessorName,
					Productname = GameWer.CustomSystem.Information.Interface.ProductName,
					Systemroot = GameWer.CustomSystem.Information.Interface.SystemRoot,
					Username = GameWer.CustomSystem.Information.Interface.UserName,
					Videoid = GameWer.CustomSystem.Information.Interface.VideocardID,
					Videoname = GameWer.CustomSystem.Information.Interface.VideocardName,
					IsBit64 = GameWer.CustomSystem.Information.Interface.IsBit64OS,
					PrivateKeyHash = Crypto.GetMD5FromLine(LastPublicKey + LastPrivateKey + DeProtectType.ArgValue_11)
				}.ParseJSON();
				NetworkManager.Send(packet2);
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_12, DeProtectType.ArgValue_13 + ex?.ToString());
			}
		}

		internal static void OnNetworkBadVersionPacket(NetworkBadVersionPacket packet)
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_14, DeProtectType.ArgValue_15);
				UIManager.ProxyForm.OnApplicationState(DeProtectType.ArgValue_16);
				NetworkManager.NotNeedReconnect = true;
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_17, DeProtectType.ArgValue_18 + ex?.ToString());
			}
		}

		private static void OnNetworkProcessesSystemReady(NetworkProcessesSystemReady networkProcessesSystemReady)
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_19, DeProtectType.ArgValue_20);
				ProcessManager.ListSendPath = new HashSet<string>();
				ProcessManager.DoSendNewProcesses();
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_21, DeProtectType.ArgValue_22 + ex?.ToString());
			}
		}

		internal static void OnNetworkPacket(string method, string full_content, Dictionary<string, object> packet)
		{
			try
			{
				if (method != null)
				{
					if (!(method == DeProtectType.ArgValue_23))
					{
						if (!(method == DeProtectType.ArgValue_24))
						{
							if (!(method == DeProtectType.ArgValue_25))
							{
								if (method == DeProtectType.ArgValue_26)
								{
									BannedPlayerResultPacket bannedPlayerResultPacket = BannedPlayerResultPacket.ParseObject(packet);
									OnNetworkBannedPlayerPacket(bannedPlayerResultPacket);
								}
							}
							else
							{
								NetworkProcessesSystemReady networkProcessesSystemReady = new NetworkProcessesSystemReady();
								OnNetworkProcessesSystemReady(networkProcessesSystemReady);
							}
						}
						else
						{
							NetworkBadVersionPacket packet2 = new NetworkBadVersionPacket();
							OnNetworkBadVersionPacket(packet2);
						}
					}
					else
					{
						NetworkAuthResultPacket networkAuthResultPacket = NetworkAuthResultPacket.ParseObject(packet);
						if (networkAuthResultPacket.Result)
						{
							OnNetworkAuthResultPacket(networkAuthResultPacket);
						}
						else
						{
							NetworkManager.BaseSocket.CloseAsync(CloseStatusCode.UnsupportedData);
						}
					}
				}
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_27, DeProtectType.ArgValue_28 + ex?.ToString());
			}
		}

		private static void OnNetworkBannedPlayerPacket(BannedPlayerResultPacket bannedPlayerResultPacket)
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_29, DeProtectType.ArgValue_30 + bannedPlayerResultPacket.Reason + DeProtectType.ArgValue_31 + Date.UnixTimeStampToDateTime(bannedPlayerResultPacket.FinishAt).ToString());
				DateTime finish_at = Date.UnixTimeStampToDateTime(bannedPlayerResultPacket.FinishAt);
				CurrentBannedPlayerResultPacket = bannedPlayerResultPacket;
				UIManager.ProxyForm.OnIncomingBanned(bannedPlayerResultPacket.Reason, finish_at);
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_32, DeProtectType.ArgValue_33 + ex?.ToString());
			}
		}

		internal static void OnNetworkDisconnected(string reason)
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_34, DeProtectType.ArgValue_35 + reason + DeProtectType.ArgValue_36);
				ProcessManager.ListSendPath = null;
				CurrentBannedPlayerResultPacket = null;
				UIManager.ProxyForm.OnNetworkDisconnected(reason);
				if (ApplicationManager.IsWork)
				{
					Timer.Timeout(delegate
					{
						if (NetworkManager.NotNeedReconnect == (int.Parse(DeProtectType.ArgValue_45) == 1))
						{
							NetworkManager.BaseSocket.ConnectAsync();
						}
						else
						{
							OutputManager.Log(DeProtectType.ArgValue_46, DeProtectType.ArgValue_47);
						}
					}, delegate(Exception ex)
					{
						OutputManager.Log(DeProtectType.ArgValue_48, DeProtectType.ArgValue_49 + ex?.ToString());
					}, 3f);
				}
			}
			catch (Exception ex2)
			{
				OutputManager.Log(DeProtectType.ArgValue_37, DeProtectType.ArgValue_38 + ex2?.ToString());
			}
		}

		internal static void OnNetworkConnected()
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_39, DeProtectType.ArgValue_40);
				UIManager.ProxyForm.OnNetworkConnected();
				LastPublicKey = Crypto.GetMD5FromLine(DateTime.Now.ToString());
				NetworkAuthPacket networkAuthPacket = new NetworkAuthPacket
				{
					Version = DeProtectType.ArgValue_41,
					SteamID = GameWer.CustomSystem.Steamwork.Interface.GetSteamID().ToString().Substring(0, 17),
					HWID = GameWer.CustomSystem.Information.Interface.GetHWID,
					PCID = GameWer.CustomSystem.Information.Interface.PCID,
					DSID = DiscordManager.DSID,
					LastSessionKey = LastKeySession,
					PublicKey = LastPublicKey,
					PublicKeyHash = Crypto.GetMD5FromLine(LastPublicKey + DeProtectType.ArgValue_42)
				};
				NetworkManager.Send(networkAuthPacket.ParseJSON());
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_43, DeProtectType.ArgValue_44 + ex?.ToString());
			}
		}
	}
}
