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
        private ObjMoveRotAssistMgr _assist;

        public void OnApplicationStart()
        {
            _assist = _go.AddComponent<ObjMoveRotAssistMgr>();
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
            Logger.Log("YML-OnLevelWasInitialized, " + level);
        }

        public void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Logger.Log("Plugin A", _assist.enabled + "");
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