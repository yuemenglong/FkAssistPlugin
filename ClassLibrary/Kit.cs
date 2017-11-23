using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClassLibrary
{
    static class Kit
    {
        public static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        public static string GetGameObjectPathAndPos(GameObject obj)
        {
            return String.Format("{0} [{1},{2},{3}]",
                GetGameObjectPath(obj),
                obj.transform.position.x,
                obj.transform.position.y,
                obj.transform.position.z
            );
        }

        public static GameObject[] LoopChildren(GameObject obj, int level = 9999)
        {
            if (level == 0)
            {
                return new GameObject[] { };
            }
            var list = new List<GameObject>();
            Logger.Log(GetGameObjectPathAndPos(obj), obj.transform.childCount + "");
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                var child = obj.transform.GetChild(i);
                list.Add(child.gameObject);
                list.AddRange(LoopChildren(child.gameObject, level - 1));
            }
            return list.ToArray();
        }

        public static String VecStr(Vector3 v)
        {
            return String.Format("[{0},{1},{2}]", v.x, v.y, v.z);
        }

        public static void backup()
        {
//            if (Input.GetKeyDown(KeyCode.V))
//            {
//                Logger.Log("*************************************************************");
//                var objs = Object.FindObjectsOfType<GameObject>();
//                Logger.Log("Object Count: " + objs.Length);
//                var localSet = new HashSet<String>();
//                foreach (var obj in objs)
//                {
//                    localSet.Add(Kit.GetGameObjectPathAndPos(obj));
//                }
//                foreach (var s in _set)
//                {
//                    if (!localSet.Contains(s))
//                    {
//                        Logger.Log("- " + s);
//                    }
//                }
//                Logger.Log("==============================================================");
//                foreach (var s in localSet)
//                {
//                    if (!_set.Contains(s))
//                    {
//                        Logger.Log("+ " + s);
//                    }
//                }
//                _set = localSet;
//            }
//            if (Input.GetMouseButtonDown(1))
//            {
//                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//                RaycastHit hit;
//                if (Physics.Raycast(ray, out hit))
//                {
//                    Logger.Log(Kit.GetGameObjectPathAndPos(hit.transform.gameObject));
//                    Logger.Log(hit.transform.tag, hit.transform.gameObject.GetType().FullName);
//                }
//            }
//            if (Input.GetKeyDown(KeyCode.X))
//            {
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
//                var cs = GameObject.Find("CommonSpace");
//                Logger.Log("" + (cs == null));
//                if (cs != null)
//                {
//                    foreach (var child in Kit.LoopChildren(cs))
//                    {
//                        Logger.Log(Kit.GetGameObjectPathAndPos(child));
//                        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//                        var pos = Camera.main.WorldToScreenPoint(child.transform.position);
//                        Logger.Log(Kit.VecStr(pos));
//                        sphere.transform.position = child.transform.position;
//                    }
//                }
//            }
        }
    }
}