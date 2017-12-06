using System;
using System.IO;
using System.Reflection;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
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
                
//                    
//                var chars = FkCharaMgr.FindSelectChara();
//                foreach (var fkChara in chars)
//                {
//                    foreach (var guideObject in fkChara.GuideObjects())
//                    {
//                        Tracer.Log(guideObject.transformTarget);
//                    }
//                }
//                var go = Context.GuideObjectManager().selectObject;
//                if (go != null && go.IsLimb())
//                {
//                    var marker = BoneMarkerMgr.Instance.CreateFor(new[] {go.transformTarget.transform});
//                    BoneMarkerMgr.Instance.markerEnabled = true;
//                    marker[0].OnDrag = (m) =>
//                    {
//                        var screenVec = m.MouseEndPos - m.MouseStartPos;
//                        var pos = Kit.MapScreenVecToWorld(screenVec, go.transformTarget.position);
//                        BoneAssist.MoveEnd(go, pos);
//                    };
//                }
//                else
//                {
//                    BoneMarkerMgr.Instance.Clear();
//                }
            }
            catch (Exception e)
            {
                Tracer.Log(e);
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
            Tracer.Log("Proc..", length);
        }
    }
}