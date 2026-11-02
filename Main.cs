using System;
using System.Runtime.InteropServices;
using MelonLoader;
using Steamworks;

namespace SendBufferFix
{
    public static class BuildInfo
    {
        public const string Name = "SendBuffer Fix";
        public const string Description = "Fixes k_EResultLimitExceeded for modded sessions by expanding buffer and send rates. Every player needs this mod.";
        public const string Author = "MausCD";
        public const string Company = null;
        public const string Version = "1.1.0";
        public const string DownloadLink = null;
    }

    public class SendBufferFix : MelonMod
    {
        private const int TargetBufferSize = 32 * 1024 * 1024;

        private const int TargetSendRateMin = 10 * 1024 * 1024;
        private const int TargetSendRateMax = 50 * 1024 * 1024;

        public override void OnInitializeMelon()
        {
            ApplyBufferPatch();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            ApplyBufferPatch();

            if (buildIndex == 1)
            {
                MelonLogger.Msg("Game scene loaded, SendBuffer & SendRate settings active.");
            }
        }

        private void ApplyBufferPatch()
        {
            try
            {
                bool bufferOk = SetSteamConfigInt(
                    ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_SendBufferSize,
                    TargetBufferSize
                );

                bool rateMinOk = SetSteamConfigInt(
                    ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_SendRateMin,
                    TargetSendRateMin
                );

                bool rateMaxOk = SetSteamConfigInt(
                    ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_SendRateMax,
                    TargetSendRateMax
                );

                if (bufferOk && rateMinOk && rateMaxOk)
                {
                    MelonLogger.Msg($"Network limits successfully set (Buffer: 32 MB, MinRate: 10 MB/s, MaxRate: 50 MB/s).");
                }
                else
                {
                    MelonLogger.Warning("One or more settings returned false (Steamworks API might not be initialized yet).");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Exception while setting network values: {ex.Message}");
            }
        }

        private bool SetSteamConfigInt(ESteamNetworkingConfigValue configValue, int value)
        {
            GCHandle handle = default;
            try
            {
                handle = GCHandle.Alloc(value, GCHandleType.Pinned);
                return SteamNetworkingUtils.SetConfigValue(
                    configValue,
                    ESteamNetworkingConfigScope.k_ESteamNetworkingConfig_Global,
                    IntPtr.Zero,
                    ESteamNetworkingConfigDataType.k_ESteamNetworkingConfig_Int32,
                    handle.AddrOfPinnedObject()
                );
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
        }
    }
}