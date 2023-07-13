using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameWer.Struct
{
	public class NetworkAuthResultPacket : BaseNetworkPacket
	{
		[JsonProperty("result")]
		internal bool Result;

		[JsonProperty("privateKey")]
		internal string PrivateKey;

		[JsonProperty("sessionKey")]
		public string SessionKey;

		public NetworkAuthResultPacket()
		{
			Method = DeProtectType.ArgValue_234;
		}

		internal override string ParseJSON()
		{
			return JsonConvert.SerializeObject(new Dictionary<string, object>
			{
				{
					DeProtectType.ArgValue_235,
					Method
				},
				{
					DeProtectType.ArgValue_236,
					Result
				},
				{
					DeProtectType.ArgValue_237,
					PrivateKey
				},
				{
					DeProtectType.ArgValue_238,
					SessionKey
				}
			});
		}

		internal static NetworkAuthResultPacket ParseObject(string content)
		{
			return ParseObject(JsonConvert.DeserializeObject<Dictionary<string, object>>(content));
		}

		internal static NetworkAuthResultPacket ParseObject(Dictionary<string, object> json)
		{
			return new NetworkAuthResultPacket
			{
				Result = (bool)json[DeProtectType.ArgValue_239],
				PrivateKey = (string)json[DeProtectType.ArgValue_240],
				SessionKey = (string)json[DeProtectType.ArgValue_241]
			};
		}
	}
}
