using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameWer.Struct
{
	public class BannedPlayerResultPacket : BaseNetworkPacket
	{
		[JsonProperty("reason")]
		internal string Reason;

		[JsonProperty("finis_at")]
		internal uint FinishAt;

		public BannedPlayerResultPacket()
		{
			Method = DeProtectType.ArgValue_218;
		}

		internal override string ParseJSON()
		{
			return JsonConvert.SerializeObject(new Dictionary<string, object>
			{
				{
					DeProtectType.ArgValue_219,
					Method
				},
				{
					DeProtectType.ArgValue_220,
					Reason
				},
				{
					DeProtectType.ArgValue_221,
					FinishAt
				}
			});
		}

		internal static BannedPlayerResultPacket ParseObject(string content)
		{
			return ParseObject(JsonConvert.DeserializeObject<Dictionary<string, object>>(content));
		}

		internal static BannedPlayerResultPacket ParseObject(Dictionary<string, object> json)
		{
			return new BannedPlayerResultPacket
			{
				Reason = json[DeProtectType.ArgValue_222].ToString(),
				FinishAt = (uint)double.Parse(json[DeProtectType.ArgValue_223].ToString())
			};
		}
	}
}
