using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using IllusionPlugin;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FkAssistPlugin
{
    public class FkPlugin : IEnhancedPlugin
    {
        HashSet<String> _set = new HashSet<string>();
        GameObject _go = new GameObject();

        public void OnApplicationStart()
        {
            Logger.Log("OnApplicationStart");
        }

        public void OnApplicationQuit()
        {
            Logger.Log("");
        }

        public void OnLevelWasLoaded(int level)
        {
            Logger.Log("OnLevelWasLoaded, " + level);
        }

        public void OnLevelWasInitialized(int level)
        {
            if (!((Object) Singleton<Studio.Studio>.Instance != (Object) null))
                return;
            BaseMgr<FkAssist>.Install(new GameObject("FkPlugin"));
            Logger.Log("OnLevelWasInitialized, " + level);
        }

        public void OnUpdate()
        {
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