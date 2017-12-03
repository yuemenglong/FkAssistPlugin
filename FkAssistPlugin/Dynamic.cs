using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Security;
using ADVSystem;
using FkAssistPlugin.HSStudioNEOAddno;
using Microsoft.CSharp;
using RootMotion.FinalIK;
using Studio;
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
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Logger.Log(hit.transform);
                    Logger.Log(hit.transform.position);
                }
                else
                {
                    Logger.Log("Not Hit");
                }
                var go = Context.GuideObjectManager().selectObject;
                if (go != null)
                {
                    BoneMarker.Create(go.transformTarget, null);
                    BoneMarkerMgr.Instance.markerEnabled = true;
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