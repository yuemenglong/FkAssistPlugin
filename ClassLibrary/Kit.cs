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
    }
}