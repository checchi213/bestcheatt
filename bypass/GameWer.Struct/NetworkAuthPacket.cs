using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameWer.Struct
{
	public class NetworkAuthPacket : BaseNetworkPacket
	{
		[JsonProperty("version")]
		internal string Version;

		[JsonProperty("steamid")]
		internal string SteamID;

		[JsonProperty("hwid")]
		internal string HWID;

		[JsonProperty("pcid")]
		internal string PCID;

		[JsonProperty("dsid")]
		internal string DSID;

		[JsonProperty("lastSessionKey")]
		internal string LastSessionKey;

		[JsonProperty("publicKey")]
		internal string PublicKey;

		[JsonProperty("publicKeyHash")]
		internal string PublicKeyHash;

		public NetworkAuthPacket()
		{
			Method = DeProtectType.ArgValue_224;
		}

		internal override string ParseJSON()
		{
			return JsonConvert.SerializeObject(new Dictionary<string, object>
			{
				{
					DeProtectType.ArgValue_225,
					Method
				},
				{
					DeProtectType.ArgValue_226,
					Version
				},
				{
					DeProtectType.ArgValue_227,
					SteamID
				},
				{
					DeProtectType.ArgValue_228,
					HWID
				},
				{
					DeProtectType.ArgValue_229,
					PCID
				},
				{
					DeProtectType.ArgValue_230,
					DSID
				},
				{
					DeProtectType.ArgValue_231,
					LastSessionKey
				},
				{
					DeProtectType.ArgValue_232,
					PublicKey
				},
				{
					DeProtectType.ArgValue_233,
					PublicKeyHash
				}
			});
		}
	}
}
