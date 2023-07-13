using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameWer.Struct
{
	public class NetworkDetailsPlayerPacket : BaseNetworkPacket
	{
		[JsonProperty("hwid_list")]
		internal string Hwid_list;

		[JsonProperty("modle")]
		internal string Modle;

		[JsonProperty("manufacturer")]
		internal string Manufacturer;

		[JsonProperty("productname")]
		internal string Productname;

		[JsonProperty("organization")]
		internal string Organization;

		[JsonProperty("owner")]
		internal string Owner;

		[JsonProperty("systemroot")]
		internal string Systemroot;

		[JsonProperty("machinename")]
		internal string Machinename;

		[JsonProperty("username")]
		internal string Username;

		[JsonProperty("isbit64")]
		internal bool IsBit64;

		[JsonProperty("memorysize")]
		internal int Memorysize;

		[JsonProperty("processorname")]
		internal string Processorname;

		[JsonProperty("processorid")]
		internal string Processorid;

		[JsonProperty("videoname")]
		internal string Videoname;

		[JsonProperty("videoid")]
		internal string Videoid;

		[JsonProperty("driversname")]
		internal string Driversname;

		[JsonProperty("driverssize")]
		internal int Driverssize;

		[JsonProperty("privateKeyHash")]
		internal string PrivateKeyHash;

		public NetworkDetailsPlayerPacket()
		{
			Method = DeProtectType.ArgValue_243;
		}

		internal override string ParseJSON()
		{
			return JsonConvert.SerializeObject(new Dictionary<string, object>
			{
				{
					DeProtectType.ArgValue_244,
					Method
				},
				{
					DeProtectType.ArgValue_245,
					Hwid_list
				},
				{
					DeProtectType.ArgValue_246,
					Modle
				},
				{
					DeProtectType.ArgValue_247,
					Manufacturer
				},
				{
					DeProtectType.ArgValue_248,
					Productname
				},
				{
					DeProtectType.ArgValue_249,
					Organization
				},
				{
					DeProtectType.ArgValue_250,
					Owner
				},
				{
					DeProtectType.ArgValue_251,
					Systemroot
				},
				{
					DeProtectType.ArgValue_252,
					Machinename
				},
				{
					DeProtectType.ArgValue_253,
					Username
				},
				{
					DeProtectType.ArgValue_254,
					IsBit64
				},
				{
					DeProtectType.ArgValue_255,
					Memorysize
				},
				{
					DeProtectType.ArgValue_256,
					Processorname
				},
				{
					DeProtectType.ArgValue_257,
					Processorid
				},
				{
					DeProtectType.ArgValue_258,
					Videoname
				},
				{
					DeProtectType.ArgValue_259,
					Videoid
				},
				{
					DeProtectType.ArgValue_260,
					Driversname
				},
				{
					DeProtectType.ArgValue_261,
					Driverssize
				},
				{
					DeProtectType.ArgValue_262,
					PrivateKeyHash
				}
			});
		}
	}
}
