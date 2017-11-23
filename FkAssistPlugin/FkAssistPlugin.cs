using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using IllusionPlugin;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ClassLibrary
{
    public class FkPlugin : IEnhancedPlugin
    {
        HashSet<String> _set = new HashSet<string>();
        GameObject _go = new GameObject();

        public void OnApplicationStart()
        {
            Logger.Log("YML-OnApplicationStart");
        }

        public void OnApplicationQuit()
        {
            Logger.Log("YML-OnApplicationQuit");
        }

        public void OnLevelWasLoaded(int level)
        {
            Logger.Log("YML-OnLevelWasLoaded, " + level);
        }

        public void OnLevelWasInitialized(int level)
        {
            if (!((Object) Singleton<Studio.Studio>.Instance != (Object) null))
                return;
            BaseMgr<FkAssist>.Install(new GameObject("FkPlugin"));
            Logger.Log("YML-OnLevelWasInitialized, " + level);
        }

        public void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Logger.Log("Plugin A");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
            }
        }

        public void OnFixedUpdate()
        {
        }

        public string Name
        {
            get { return "FkPlugin"; }
        }

        public string Version
        {
            get { return "0.0.1"; }
        }

        public void OnLateUpdate()
        {
        }

        public string[] Filter
        {
            get
            {
                return new[]
                {
                    "StudioNEO_32",
                    "StudioNEO_64",
                };
            }
        }
    }
}