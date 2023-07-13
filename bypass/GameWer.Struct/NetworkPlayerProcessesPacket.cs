using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameWer.Struct
{
	public class NetworkPlayerProcessesPacket : BaseNetworkPacket
	{
		[JsonProperty("processes")]
		internal PlayerProcess[] Processes;

		public NetworkPlayerProcessesPacket()
		{
			Method = DeProtectType.ArgValue_263;
		}

		internal override string ParseJSON()
		{
			return JsonConvert.SerializeObject(new Dictionary<string, object>
			{
				{
					DeProtectType.ArgValue_264,
					Method
				},
				{
					DeProtectType.ArgValue_265,
					Processes
				}
			});
		}
	}
}
