using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using RootMotion.FinalIK;
using Studio;
using Unity.Linq;
using UnityEngine;

namespace FkAssistPlugin
{
    public class FkLocker : BaseMgr<FkLocker>
    {
        private List<FkChara> selectChara = new List<FkChara>();

        public override void Init()
        {
            Tracer.Log("FkLocker Init");
        }

        private void Update()
        {
            try
            {
                InnerUpdate();
            }
            catch (Exception e)
            {
                Tracer.Log(e);
            }
        }

        private void InnerUpdate()
        {
            Camera camera = CameraMgr.MainCamera();
            if (!selectChara.IsNullOrEmpty())
            {
                for (int i = 0; i < selectChara.Count; i++)
                {
                    var chara = selectChara[i];
                    var go = new GameObject();
                    var target = camera.transform.position - chara.Head.Transform.position;
                    var v1 = go.transform.forward;
                    var v2 = new Vector3(target.x, 0, target.z);
                    var axis = Vector3.Cross(v1, v2);
                    var angel = Vector3.Angle(v1, v2);
                    go.transform.RotateAround(go.transform.position, axis, angel);
                    axis = Vector3.Cross(v2, target);
                    angel = Vector3.Angle(v2, target);
                    go.transform.RotateAround(go.transform.position, axis, angel);
                    chara.Head.TurnTo(go.transform.rotation);
                    go.Destroy();
                }
            }

//            if (selectChara != null)
//            {
//                Vector3 lastV = lastCameraPos - selectChara.Head.Transform.position;
//                Vector3 thisV = camera.transform.position - selectChara.Head.Transform.position;
//                Vector3 lastVx = new Vector3(lastV.x, 0, lastV.z);
//                Vector3 thisVx = new Vector3(thisV.x, 0, thisV.z);
//                float angel = Vector3.Angle(lastVx, thisVx);
//                Vector3 axis = Vector3.Cross(lastVx, thisVx);
//                if (lastCameraPos != camera.transform.position)
//                {
//                    Tracer.Log(lastVx, thisVx, angel, axis);
//                }
//
//                selectChara.Head.RotateSelf(axis, angel);
//
//                lastCameraPos = camera.transform.position;
//            }


            if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftShift))
            {
                var chara = FkCharaMgr.FindSelectChara();
                if (chara == null || !chara.Head.GuideObject.enableRot)
                {
                    return;
                }

                if (selectChara.Contains(chara))
                {
                    return;
                }

                selectChara.Add(chara);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                selectChara.Clear();
            }
        }
    }
}