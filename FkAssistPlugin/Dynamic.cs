using System;
using System.IO;
using System.Reflection;
using FkAssistPlugin.HSStudioNEOAddno;
using UnityEngine;

namespace FkAssistPlugin
{
    public class Dynamic
    {
//        private static String _path = @"D:\workspace\unity\FkAssistPlugin\DynamicHandler\bin\Debug\DynamicHandler.dll";
        private static String _path = @"D:\workspace\unity\FkAssistPlugin\FkAssistPlugin\bin\Debug\FkAssistPlugin.dll";

        private static Assembly _assembly;
        private static DateTime _lastModified;

        private static DateTime GetLastModifieDateTime()
        {
            return new FileInfo(_path).LastWriteTime;
        }

        public static void Init()
        {
//            _assembly = Kit.LoadAssembly(_path);
//            _lastModified = GetLastModifieDateTime();
//            Logger.Log("Init Dynamic Succ");
        }


        public static void DynamicProc()
        {
            try
            {
//                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//                RaycastHit hit;
//                if (Physics.Raycast(ray, out hit))
//                {
//                    Logger.Log(hit.transform);
//                    Logger.Log(hit.transform.position);
//                }
//                else
//                {
//                    Logger.Log("Not Hit");
//                }
                var go = Context.GuideObjectManager().selectObject;
                if (go != null && go.IsLimb())
                {
//                    var screenPoint = Camera.main.WorldToScreenPoint(go.transformTarget.position);
//                    Logger.Log("ScreenPoint", screenPoint);
//                    screenPoint += new Vector3(100f, 0, 0);
//                    var worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
//                    Logger.Log("WorldPoint", worldPoint);
//                    var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//                    sphere.transform.position = Vector3.zero;
//                    sphere.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    var marker = BoneMarkerMgr.Instance.CreateFor(new[] {go.transformTarget.transform});
                    BoneMarkerMgr.Instance.markerEnabled = true;
                    marker[0].OnDrag = (m) =>
                    {
                        var screenVec = m.MouseEndPos - m.MouseStartPos;
//                        Logger.Log("Screen Vec", screenVec);
                        var pos = BoneAssist.MapScreenVec(screenVec, go.transformTarget.position);
//                        Logger.Log("Trans From", sphere.transform.position);
//                        Logger.Log("Trans To", pos);
//                        sphere.transform.position = pos;
                        BoneAssist.MoveEnd(go, pos);
                    };
                }
                else
                {
                    BoneMarkerMgr.Instance.Clear();
                }
            }
            catch (Exception e)
            {
                Logger.Log(e);
            }
        }

//        public static void DynamicProc()
//        {
//            try
//            {
//                var last = GetLastModifieDateTime();
//                if (_lastModified != last)
//                {
//                    _assembly = Kit.LoadAssembly(_path);
//                    _lastModified = last;
//                    Logger.Log("Load Dynamic Succ");
//                }
////                var type = _assembly.GetType("DynamicHandler.Handler");
//                var type = _assembly.GetType("FkAssistPlugin.Dynamic");
//                var method = type.GetMethod("Proc");
//                var obj = Activator.CreateInstance(type);
//                method.Invoke(obj, new object[] { });
//            }
//            catch (Exception e)
//            {
//                Logger.Log(e.Message);
//                Logger.Log(e.StackTrace);
//            }
//        }

        public void Proc()
        {
            var length = Context.Characters().Length;
            Logger.Log("Proc..", length);
        }
    }
}