using System;
using System.Runtime.InteropServices;
using MelonLoader;
using Steamworks;

namespace SendBufferFix
{
    public static class BuildInfo
    {
        public const string Name = "SendBuffer Fix";
        public const string Description = "Fixes k_EResultLimitExceeded by expanding Steam send buffer";
        public const string Author = "MausCD";
        public const string Company = null;
        public const string Version = "1.0.0";
        public const string DownloadLink = null;
    }

    public class SendBufferFix : MelonMod
    {
        // 4 MB Puffer (Steam-Default ist 524.288 Bytes / 512 KB)
        private const int TargetBufferSize = 4 * 1024 * 1024;
        private bool _isPatched = false;

        public override void OnInitializeMelon()
        {
            ApplyBufferPatch();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (!_isPatched)
            {
                ApplyBufferPatch();
            }

            if (buildIndex == 1)
            {
                MelonLogger.Msg("Game scene loaded, SendBufferFix active.");
            }
        }

        private void ApplyBufferPatch()
        {
            GCHandle handle = default;
            try
            {
                handle = GCHandle.Alloc(TargetBufferSize, GCHandleType.Pinned);

                bool success = SteamNetworkingUtils.SetConfigValue(
                    ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_SendBufferSize,
                    ESteamNetworkingConfigScope.k_ESteamNetworkingConfig_Global,
                    IntPtr.Zero,
                    ESteamNetworkingConfigDataType.k_ESteamNetworkingConfig_Int32,
                    handle.AddrOfPinnedObject()
                );

                if (success)
                {
                    _isPatched = true;
                    MelonLogger.Msg($"[SendBufferFix] SendBufferSize successfully set to {TargetBufferSize} Bytes!");
                }
                else
                {
                    MelonLogger.Warning("[SendBufferFix] SetConfigValue returned false (maybe SteamAPI not Initialised).");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[SendBufferFix] Error setting Puffers: {ex.Message}");
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