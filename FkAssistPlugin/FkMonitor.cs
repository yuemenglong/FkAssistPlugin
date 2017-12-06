using System;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin
{
    public class FkMonitor : BaseMgr<FkMonitor>
    {
        public override void Init()
        {
            Tracer.Log("FkLocker Init");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                return;
            }
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
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (FkCharaMgr.IsMarkerEnabled())
                {
                    FkCharaMgr.ClearMarker();
                }
                else
                {
                    FkCharaMgr.RefreshSelectChara();
                    FkCharaMgr.EnableMarker();
                }
            }
            FkCharaMgr.MoveLocked();
        }
    }
}