using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameWer.Struct
{
	public class NetworkProcessesSystemReady : BaseNetworkPacket
	{
		public NetworkProcessesSystemReady()
		{
			Method = DeProtectType.ArgValue_266;
		}

		internal override string ParseJSON()
		{
			return JsonConvert.SerializeObject(new Dictionary<string, object>
			{
				{
					DeProtectType.ArgValue_267,
					Method
				}
			});
		}

		internal static NetworkProcessesSystemReady ParseObject(string content)
		{
			return ParseObject(JsonConvert.DeserializeObject<Dictionary<string, object>>(content));
		}

		internal static NetworkProcessesSystemReady ParseObject(Dictionary<string, object> json)
		{
			return new NetworkProcessesSystemReady();
		}
	}
}
