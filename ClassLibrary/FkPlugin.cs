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
            Logger.Log("YML-OnLevelWasInitialized, " + level);
        }

        public void OnUpdate()
        {
            return;
            if (Input.GetKeyDown(KeyCode.V))
            {
                Logger.Log("*************************************************************");
                var objs = Object.FindObjectsOfType<GameObject>();
                Logger.Log("Object Count: " + objs.Length);
                var localSet = new HashSet<String>();
                foreach (var obj in objs)
                {
                    localSet.Add(Kit.GetGameObjectPathAndPos(obj));
                }
                foreach (var s in _set)
                {
                    if (!localSet.Contains(s))
                    {
                        Logger.Log("- " + s);
                    }
                }
                Logger.Log("==============================================================");
                foreach (var s in localSet)
                {
                    if (!_set.Contains(s))
                    {
                        Logger.Log("+ " + s);
                    }
                }
                _set = localSet;
            }
            if (Input.GetMouseButtonDown(1))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Logger.Log(Kit.GetGameObjectPathAndPos(hit.transform.gameObject));
                    Logger.Log(hit.transform.tag, hit.transform.gameObject.GetType().FullName);
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
//                var go = GameObject.Find("/StudioScene/GuideObjectWorkplace/M Root(Clone)/rotation");
//                if (go != null)
//                {
//                    Logger.Log(go.GetType().FullName);
//                }
//                var gos = GameObject.FindObjectsOfType<GameObject>();
//                foreach (var go in gos)
//                {
//                    if (go.name == "rotation")
//                    {
//                        Logger.Log(Kit.GetGameObjectPathAndPos(go));
//                        go.transform.Rotate(10,10,10);
//                    }
//                }
                var cs = GameObject.Find("CommonSpace");
                Logger.Log("" + (cs == null));
                if (cs != null)
                {
                    foreach (var child in Kit.LoopChildren(cs))
                    {
                        Logger.Log(Kit.GetGameObjectPathAndPos(child));
                        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        var pos = Camera.main.WorldToScreenPoint(child.transform.position);
                        Logger.Log(Kit.VecStr(pos));
                        sphere.transform.position = child.transform.position;
                    }
                }
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