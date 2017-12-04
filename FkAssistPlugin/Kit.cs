using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using UnityEngine;

namespace FkAssistPlugin
{
    public static class Kit
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
            Tracer.Log(GetGameObjectPathAndPos(obj), obj.transform.childCount + "");
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
            var xa = Vector3.Angle(new Vector3(1, 0, 0), v);
            var ya = Vector3.Angle(new Vector3(0, 1, 0), v);
            var za = Vector3.Angle(new Vector3(0, 0, 1), v);
            return String.Format("({0,6:F3}, {1,6:F3}, {2,6:F3}) <{3,5:F2}, {4,5:F2}, {5,5:F2}> [{6,6:F3}]",
                v.x, v.y, v.z, xa, ya, za, v.magnitude);
        }

        public static String QuatStr(Quaternion q)
        {
            return String.Format("({0,6:F3}, {1,6:F3}, {2,6:F3}), {3,6:F3})",
                q.w, q.x, q.y, q.z);
        }

        public static String StackTrace()
        {
            StackTrace st = new StackTrace();
            var list = new List<String>();
            foreach (var frame in st.GetFrames())
            {
                list.Add(String.Format("\n{0}, {1}", frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name));
            }
            return String.Join("", list.ToArray());
        }

        public static Vector3 ScreenPoint(Vector3 world)
        {
            Vector3 screen = Camera.main.WorldToScreenPoint(world);
            return new Vector3(screen.x, Screen.height - screen.y, screen.z);
        }

        public static void GuiButton(Vector3 world, String text, int size = 25, Action action = null)
        {
            var height = size;
            var width = size * text.Length;
            Vector3 screen = Camera.main.WorldToScreenPoint(world);
            var rect = new Rect(screen.x - width / 2f, Screen.height - screen.y - height / 2f, width, height);
            if (screen.z > 0f && GUI.Button(rect, text))
            {
                if (action != null)
                {
                    action();
                }
            }
        }

        public static float Angle(float a, float b, float c)
        {
            var cos = (a * a + b * b - c * c) / Mathf.Abs(2 * a * b);
            return Mathf.Acos(cos) / Mathf.PI * 180;
        }

        public static float Angle(Vector3 a, Vector3 b, Vector3 c)
        {
            return Angle(a.magnitude, b.magnitude, c.magnitude);
        }

        public static T GetPrivateField<T>(this object instance, string fieldname)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldname, flag);
            return (T) field.GetValue(instance);
        }

        public static Assembly LoadAssembly(String path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                var buffer = new Byte[fs.Length];
                var pos = 0;
                while (pos < fs.Length - 1)
                {
                    pos += fs.Read(buffer, pos, (int) (fs.Length - pos));
                }
                var assembly = Assembly.Load(buffer);
                return assembly;
            }
        }

        #region backup

//        
//        public OCIChar FindOciChar()
//        {
//            foreach (var objectCtrlInfo in Context.Studio().dicInfo.Values)
//            {
//                if (objectCtrlInfo.kind == 0)
//                {
//                    Logger.Log("has kind = 0");
//                    OCIChar ocichar = objectCtrlInfo as OCIChar;
//                    if (ocichar == null)
//                    {
//                        Logger.Log("ocichar is null");
//                    }
//                    else if (ocichar.charInfo == null)
//                    {
//                        Logger.Log("ocichar info is null");
//                    }
//                    else
//                    {
//                        return ocichar;
//                    }
//                }
//            }
//            return null;
//        }


//        public GuideObject GetTargetObject()
//        {
//            GuideObject guideObject = Singleton<GuideObjectManager>.Instance.operationTarget;
//            if ((UnityEngine.Object) guideObject == (UnityEngine.Object) null)
//                guideObject = Singleton<GuideObjectManager>.Instance.selectObject;
//            return guideObject;
//        }
//
//        public ObjectCtrlInfo GetFirstObject()
//        {
//            Studio.Studio instance = Singleton<Studio.Studio>.Instance;
//            if ((UnityEngine.Object) instance != (UnityEngine.Object) null)
//            {
//                ObjectCtrlInfo[] selectObjectCtrl = instance.treeNodeCtrl.selectObjectCtrl;
//                if (selectObjectCtrl != null && selectObjectCtrl.Length != 0)
//                    return selectObjectCtrl[0];
//            }
//            return (ObjectCtrlInfo) null;
//        }

        public static void backup()
        {
            //                Kit.GUIButton(new Vector3(), "AA");
//                var rect = new Rect(40,40,40,40);
//                GUI.Button(rect, "YML");
//                Logger.Log("FkAssist Update");
//                Logger.Log(Context.GuideObjectManager().selectObjects.Length + "");
//                var ociCharchar = FindOciChar();
//                var bone = ociCharchar.listBones[0];
//                foreach (var b in ociCharchar.listBones)
//                {
//                    Logger.Log(Kit.GetGameObjectPathAndPos(b.guideObject.transformTarget.gameObject));
//                    if (b.guideObject == null || !b.guideObject.enableRot)
//                    {
//                        Logger.Log("Null Or Not Rot");
//                    }
//                }
//                Logger.Log(ociCharchar.listBones.Count);

//                Context.GuideObjectManager().AddObject(bone.guideObject);
//                Logger.Log(Context.GuideObjectManager().selectObjects.Length);
//                Rotate(bone.guideObject, 10, 10, 10);
//                bone.guideObject.isActive = false;
//                ociCharchar.listBones.ForEach(b =>
//                {
//                    Logger.Log("===============================================================");
//                    Logger.Log(Kit.GetGameObjectPathAndPos(b.guideObject.gameObject));
//                    Logger.Log(Kit.GetGameObjectPathAndPos(b.guideObject.transformTarget.gameObject));
//                    Logger.Log(b.guideObject.enablePos + "", b.guideObject.enableRot + "",
//                        b.guideObject.enableScale + "");
//                });
//                Logger.Log(ociCharchar.listBones.Count + "");
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

//        
//        private static void charaBones2()
//        {
//            foreach (var objectCtrlInfo in Context.Studio().dicInfo.Values)
//            {
//                if (objectCtrlInfo.kind == 0)
//                {
//                    Logger.Log("has kind = 0");
//                    OCIChar ocichar = objectCtrlInfo as OCIChar;
//                    if (ocichar == null)
//                    {
//                        Logger.Log("ocichar is null");
//                    }
//                    else if (ocichar.charInfo == null)
//                    {
//                        Logger.Log("ocichar info is null");
//                    }
//                    else
//                    {
//                        var character = ocichar.charInfo;
//                        string prefix = character is CharFemale ? "cf_" : "cm_";
//                        List<GameObject> normalTargets = new List<GameObject>();
//                        var list = new List<GameObject>();
//                        character.chaBody.objBone.transform.FindLoopAll(list);
//                        list.ForEach(item => { Logger.Log(Kit.GetGameObjectPathAndPos(item)); });
//                        Logger.Log(list.Count + "");
////                            foreach(string targetName in FileManager.GetNormalTargetNames())
////                            {
////                                GameObject bone = character.chaBody.objBone.transform.FindLoop(prefix + targetName);
////                                if(bone) normalTargets.Add(bone);
////                            }
////                            return normalTargets;
//                    }
//                    GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
//                    foreach (GuideObject guideObject in instance.selectObjects)
//                    {
//                        //                    if (guideObject.enableRot)
//                        //                    {
//                        var p0 = guideObject.transform.position;
//                        var p1 = guideObject.transformTarget.position;
//                        var g0 = guideObject.gameObject;
//                        var g1 = guideObject.transformTarget.gameObject;
//                        Logger.Log(Kit.VecStr(p0));
//                        Logger.Log(Kit.GetGameObjectPathAndPos(g0));
//                        Logger.Log(Kit.VecStr(p1));
//                        Logger.Log(Kit.GetGameObjectPathAndPos(g1));
//                        guideObject.transformTarget.gameObject.transform.Rotate(10, 0, 0);
//                        //                    }
//                    }
//                }
//            }
//        }

        #endregion
    }
}