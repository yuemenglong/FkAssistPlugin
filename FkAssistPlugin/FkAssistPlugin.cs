using System;
using System.Collections.Generic;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Patch;
using Harmony;
using IllusionPlugin;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FkAssistPlugin
{
    public class Test
    {
        public static bool pre()
        {
//            Logger.Log("Pre");
            return false;
        }
    }

    public class FkPlugin : IEnhancedPlugin
    {
        HashSet<String> _set = new HashSet<string>();
        GameObject _go = new GameObject();

        public void OnApplicationStart()
        {
            Tracer.Log("OnApplicationStart");
            PatchMgr.Init();
        }

        public void OnApplicationQuit()
        {
            Tracer.Log("");
        }

        public void OnLevelWasLoaded(int level)
        {
            Tracer.Log("OnLevelWasLoaded, " + level);
        }

        public void OnLevelWasInitialized(int level)
        {
            if (!((Object) Singleton<Studio.Studio>.Instance != (Object) null))
                return;
            BaseMgr<FkAssist>.Install(new GameObject("FkPlugin"));
            Tracer.Log("OnLevelWasInitialized, " + level);
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