using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using RootMotion.FinalIK;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    struct HangRecord
    {
        public FkBone.FkBone Bone;
        public Vector3 Pos;
        public Quaternion Rot;
    }

    public class FkLocker : BaseMgr<FkLocker>
    {
        private List<HangRecord> _hangRecords = new List<HangRecord>();

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
            if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftAlt))
            {
                var chara = FkCharaMgr.FindSelectChara();
                if (chara != null)
                {
                    chara.Limbs().Foreach(bone =>
                    {
                        var r = new HangRecord();
                        r.Bone = bone;
                        r.Pos = bone.Transform.position;
                        r.Rot = bone.Transform.rotation;
                        _hangRecords.Add(r);
                    });
                }
            }
            else if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftShift))
            {
                var go = Context.GuideObjectManager().selectObject;
                if (go != null && go.IsLimb())
                {
                    var chara = FkCharaMgr.FindSelectChara();
                    var bone = chara.DicGuideBones[go];
                    var r = new HangRecord();
                    r.Bone = bone;
                    r.Pos = bone.Transform.position;
                    r.Rot = bone.Transform.rotation;
                    _hangRecords.Add(r);
                }
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                _hangRecords.Clear();
            }

            _hangRecords.ForEach(r =>
            {
                if (r.Pos != r.Bone.Transform.position)
                {
                    r.Bone.MoveTo(r.Pos);
                }

                if (r.Rot != r.Bone.Transform.rotation)
                {
                    r.Bone.TurnTo(r.Rot);
                }
            });
        }
    }
}