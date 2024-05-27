using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Apple.Core
{
    public static class Availability
    {
        private static RuntimeEnvironment _runtimeEnvironment;

#if UNITY_EDITOR_OSX && (UNITY_IOS || UNITY_TVOS || UNITY_STANDALONE_OSX)
        [DllImport(InteropUtility.DLLName, EntryPoint = "AppleCore_GetRuntimeEnvironment")]
        private static extern RuntimeEnvironment AppleCore_GetRuntimeEnvironment();
#endif

        /// <summary>
        /// Use to ensure API methods are only called on platforms which support those calls.
        /// </summary>
        public static bool Available(RuntimeOperatingSystem targetOperatingSystem, int targetMajorVersion, int targetMinorVersion = 0)
        {
            if (_runtimeEnvironment.RuntimeOperatingSystem == targetOperatingSystem)
            {
                if (_runtimeEnvironment.VersionNumber.Major > targetMajorVersion)
                {
                    return true;
                }
                else if ((_runtimeEnvironment.VersionNumber.Major == targetMajorVersion) && (_runtimeEnvironment.VersionNumber.Minor >= targetMinorVersion))
                {
                    return true;
                }
            }

            return false;
        }

        #region Init & Shutdown
        [RuntimeInitializeOnLoadMethod]
        private static void OnApplicationStart()
        {
            Debug.Log("[Apple.Core Plug-In] Initializing API Availability Checking");

            try
            {
#if UNITY_EDITOR_OSX && (UNITY_IOS || UNITY_TVOS || UNITY_STANDALONE_OSX)
            _runtimeEnvironment = AppleCore_GetRuntimeEnvironment();
#endif
            }
            catch (Exception e)
            {
#if !UNITY_EDITOR
				throw e;
#endif
            }
            
            Debug.Log($"[Apple.Core Plug-In] Availability Runtime Environment: {_runtimeEnvironment.RuntimeOperatingSystem.ToString()} {_runtimeEnvironment.VersionNumber.Major}.{_runtimeEnvironment.VersionNumber.Minor}");
        }
        #endregion
    }
}
