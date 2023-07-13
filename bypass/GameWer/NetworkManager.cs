using GameWer.Data;
using GameWer.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace GameWer
{
	public class NetworkManager
	{
		internal static WebSocket BaseSocket;

		internal static bool HasConnected = false;

		internal static bool NotNeedReconnect = false;

		internal static void Init()
		{
			OutputManager.Log(DeProtectType.ArgValue_159, DeProtectType.ArgValue_160);
			BaseSocket = new WebSocket(string.Format(DeProtectType.ArgValue_161, AppInfo.TargetConnectIP, AppInfo.TargetConnectPort));
			BaseSocket.OnClose += OnNetworkClose;
			BaseSocket.OnMessage += OnNetworkMessage;
			BaseSocket.OnOpen += OnNetworkConnected;
			BaseSocket.OnError += OnSocketError;
		}

		internal static void Send(string packet)
		{
			BaseSocket?.SendAsync(Cryptography.OpenSSLEncrypt(packet, DeProtectType.ArgValue_162), delegate
			{
			});
		}

		private static void OnSocketError(object sender, ErrorEventArgs e)
		{
			OutputManager.Log(DeProtectType.ArgValue_163, DeProtectType.ArgValue_164 + e.Message + "\n" + e.Exception?.ToString());
		}

		internal static void Start()
		{
			OutputManager.Log(DeProtectType.ArgValue_165, DeProtectType.ArgValue_166);
			Timer.Interval(delegate
			{
				BaseSocket?.Ping();
			}, delegate
			{
			}, float.Parse(DeProtectType.ArgValue_167));
			BaseSocket.ConnectAsync();
		}

		private static void OnNetworkConnected(object sender, EventArgs e)
		{
			try
			{
				OutputManager.Log(DeProtectType.ArgValue_168, DeProtectType.ArgValue_169);
				HasConnected = (int.Parse(DeProtectType.ArgValue_170) == 1);
				ApplicationManager.SetTaskInMainThread(AntiCheatManager.OnNetworkConnected);
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_171, DeProtectType.ArgValue_172 + ex?.ToString());
				BaseSocket?.CloseAsync(CloseStatusCode.InvalidData);
			}
		}

		private static void OnNetworkMessage(object sender, MessageEventArgs e)
		{
			try
			{
				string content_json = e.Data;
				Dictionary<string, object> list = JsonConvert.DeserializeObject<Dictionary<string, object>>(content_json);
				ApplicationManager.SetTaskInMainThread(delegate
				{
					string text = default(string);
					int num;
					if (list.TryGetValue(DeProtectType.ArgValue_184, out object value))
					{
						text = (value as string);
						num = ((text != null) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					if (num != 0)
					{
						AntiCheatManager.OnNetworkPacket(text, content_json, list);
					}
				});
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_173, DeProtectType.ArgValue_174 + ex?.ToString());
				BaseSocket?.CloseAsync(CloseStatusCode.InvalidData);
			}
		}

		private static void OnNetworkClose(object sender, CloseEventArgs e)
		{
			try
			{
				string reason = e.Reason;
				OutputManager.Log(DeProtectType.ArgValue_175, DeProtectType.ArgValue_176 + ((CloseStatusCode)e.Code).ToString() + DeProtectType.ArgValue_177 + reason + DeProtectType.ArgValue_178);
				HasConnected = (int.Parse(DeProtectType.ArgValue_179) == 1);
				ApplicationManager.SetTaskInMainThread(delegate
				{
					AntiCheatManager.OnNetworkDisconnected(reason);
				});
			}
			catch (Exception ex)
			{
				OutputManager.Log(DeProtectType.ArgValue_180, DeProtectType.ArgValue_181 + ex?.ToString());
				BaseSocket?.CloseAsync(CloseStatusCode.InvalidData);
			}
		}

		internal static void Shutdown()
		{
			OutputManager.Log(DeProtectType.ArgValue_182, DeProtectType.ArgValue_183);
			try
			{
				BaseSocket.Close();
			}
			catch
			{
			}
		}
	}
}
