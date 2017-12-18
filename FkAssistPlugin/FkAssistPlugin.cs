using System;
using System.Collections.Generic;
using System.Security;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Patch;
using FkAssistPlugin.Util;
using Harmony;
using IllusionPlugin;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FkAssistPlugin
{
    public class FkPlugin : IEnhancedPlugin
    {
        public void OnApplicationStart()
        {
            Tracer.Log("OnApplicationStart");
            var p = new CameraPatch();
            PatchMgr.Patch(p);
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
            if (Context.Studio() == null)
                return;
            BaseMgr<FkAssist>.Install(new GameObject("FkPlugin"));
            BaseMgr<FkMonitor>.Install(new GameObject("FkMonitor"));
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
            get { return "1.0.0"; }
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