using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin
{
    public class FkSelector : BaseMgr<FkSelector>
    {
        public override void Init()
        {
            Tracer.Log("FkSelector Init");
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
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(2))
            {
                Tracer.Log("Mouse Middle");
                var go = Context.GuideObjectManager().selectObject;
                if (go != null)
                {
                    go.SetActive(false);
                }
//                var minDist = double.MaxValue;
//                FkBone.FkBone minBone = null;
//                FkCharaMgr.FindSelectCharas().Foreach(c =>
//                {
//                    c.Bones().Foreach(b =>
//                    {
//                        b.GuideObject.SetActive(false);
//                        var screenPoint = CameraMgr.MainCamera().WorldToScreenPoint(b.Transform.position);
//                        var dist = (screenPoint - Input.mousePosition).magnitude;
//                        Tracer.Log(b.Transform.name, dist);
//                        if (dist < minDist)
//                        {
//                            minDist = dist;
//                            minBone = b;
//                        }
//                    });
//                });
//                if (minBone != null)
//                {
//                    minBone.GuideObject.SetActive(true);
//                }
            }
        }
    }
}