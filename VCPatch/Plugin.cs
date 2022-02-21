using Exiled.API.Features;
using Exiled.API.Interfaces;
using HarmonyLib;
using System;
using UnityEngine;

namespace VCPatch
{
    public class Config : IConfig
	{
        public bool IsEnabled { get; set; } = true;
	}

    public class Plugin : Plugin<Config>
    {
        private Harmony hInstance;

		public override void OnEnabled()
		{
			base.OnEnabled();

			hInstance = new Harmony("cyan.vcpatch");
			hInstance.PatchAll();
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			hInstance.UnpatchAll(hInstance.Id);
			hInstance = null;
		}

		public override string Name => "VcPatch";
		public override string Author => "Cyanox & Mitzey";
	}

	[HarmonyPatch(typeof(Mirror.NetworkConnection), nameof(Mirror.NetworkConnection.TransportReceive))]
	public class VCBugPatch
	{
		public static bool Prefix(Mirror.NetworkConnection __instance, ArraySegment<byte> buffer)
		{
			if (buffer.Count < 2)
			{
				Debug.LogError(string.Format("[PATCHED] ConnectionRecv {0} Message was too short (messages should start with message id)", __instance));
				//this.Disconnect();
				return false;
			}
			return true;
		}
	}
}
