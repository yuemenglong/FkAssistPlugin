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
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
            {
                var minDist = double.MaxValue;
                FkBone.FkBone minBone = null;
                FkCharaMgr.FindSelectCharas().Foreach(c =>
                {
                    c.MainBones().Foreach(b =>
                    {
                        var screenPoint = CameraMgr.MainCamera().WorldToScreenPoint(b.Transform.position);
                        var dist = (screenPoint - Input.mousePosition).magnitude;
                        if (dist < minDist)
                        {
                            minDist = dist;
                            minBone = b;
                        }
                    });
                });
                if (minBone != null)
                {
                    Context.GuideObjectManager().SetSelectObject(minBone.GuideObject, false);
                }
            }
        }
    }
}