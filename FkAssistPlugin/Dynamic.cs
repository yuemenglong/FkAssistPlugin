using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
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
            var go = Context.GuideObjectManager().selectObject;
            if (go == null)
            {
                return;
            }
            if (go.transformTarget.IsHand())
            {
//                Logger.Log(go.transformTarget);
//                Logger.Log(go.transformTarget.parent);
//                Logger.Log(go.transformTarget.parent.parent);
//                var parent = go.transformTarget.parent;
//                parent.Rotate(10, 10, 10, Space.Self);
//                parent.GuideObject().changeAmount.rot = parent.localEulerAngles;
//                parent = parent.parent;
//                parent.Rotate(10, 10, 10, Space.Self);
//                parent.GuideObject().changeAmount.rot = parent.localEulerAngles;
                var t = go.transformTarget;
                var tp = t.parent;
                var tpp = tp.parent;
                var root = new TransformBone(tpp, tp);
                var end = new TransformBone(tp, t);
                var assist = new BoneAssist(root, end);
                assist.Forward(0.01f);
            }
            else
            {
                Logger.Log(go.transformTarget);
            }
//            Logger.Log(go.gameObject);
////[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Spine01/cf_J_Spine02/cf_J_Spine03/cf_J_ShoulderIK_L/cf_J_Shoulder_L/cf_J_ArmUp00_L/cf_J_ArmLow01_L/cf_J_Hand_L [-0.5565164,1.331407,-0.03151792]
////[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Spine01/cf_J_Spine02/cf_J_Spine03/cf_J_ShoulderIK_L/cf_J_Shoulder_L/cf_J_ArmUp00_L/cf_J_ArmLow01_L [-0.3511997,1.331407,-0.03151793]
////[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Spine01/cf_J_Spine02/cf_J_Spine03/cf_J_ShoulderIK_L/cf_J_Shoulder_L/cf_J_ArmUp00_L [-0.112564,1.331407,-0.03151792]
////[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Kosi01/cf_J_Kosi02/cf_J_LegUp00_L/cf_J_LegLow01_L/cf_J_LegLowRoll_L/cf_J_Foot01_L [-0.07924593,0.08603691,-0.02697031]
////[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Kosi01/cf_J_Kosi02/cf_J_LegUp00_L/cf_J_LegLow01_L [-0.07924539,0.5074378,-0.0270153]
////[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Kosi01/cf_J_Kosi02/cf_J_LegUp00_L [-0.07924507,0.8856537,-0.02701536]
//            if (go != null)
//            {
//                Logger.Log(go.transformTarget.gameObject);
//                var name = go.transformTarget.name;
//                if (name == "cf_J_Hand_L"
//                    || name == "cf_J_Hand_R"
//                    || name == "cm_J_Hand_L"
//                    || name == "cm_J_Hand_R")
////                        || name == "cf_J_Foot01_L"
////                        || name == "cf_J_Foot01_R"
////                        || name == "cm_J_Foot01_L"
////                        || name == "cm_J_Foot01_R")
//                {
////                        go.transformTarget.Rotate(10, 10, 10, Space.Self);
////                        go.changeAmount.rot = go.transformTarget.localEulerAngles;
//                    var t2 = go.transformTarget;
//                    var t1 = go.transformTarget.parent;
//                    var t0 = go.transformTarget.parent.parent;
//                    var dic = Context.DicGuideObject();
//                    if (dic == null)
//                    {
//                        Logger.Log("Dic NULL");
//                        return;
//                    }
//                    Logger.Log("Dic Not NULL");
//
//                    var g1 = dic[t1];
//                    var g0 = dic[t0];
//                    if (g1 == null)
//                    {
//                        Logger.Log("G1 NULL");
//                    }
//                    if (g0 == null)
//                    {
//                        Logger.Log("G0 NULL");
//                    }
//                    Logger.Log("Forward");
//                    var root = new BoneRoot(new GuideObjectBone(g0, t1));
//                    var end = new BoneEnd(new GuideObjectBone(g0, t2), root);
//                    root.Forward(-0.01f);
//                }
//            }
//            else
//            {
//                Logger.Log("NULL");
//            }
//            return;
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