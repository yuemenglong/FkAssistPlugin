using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin
{
    public class FkLimbLocker : BaseMgr<FkLimbLocker>
    {

        struct HangRecord
        {
            public FkBone.FkBone Bone;
            public Vector3 Pos;
            public Quaternion Rot;
        }

        private List<HangRecord> _hangRecords = new List<HangRecord>();

        private Rect _windowRect = new Rect(
            Screen.width * 0.8f,
            Screen.height * 0.8f,
            Screen.width * 0.2f,
            Screen.height * 0.2f);

        private int wid = 18539;

        public override void Init()
        {
            Tracer.Log("FkLimbLocker Init");
        }

        private void OnGUI()
        {
            if (_hangRecords.Count > 0)
            {
                _windowRect = GUI.Window(wid, _windowRect, ShowWindow, "LimbLocker");
            }
        }

        private void ShowWindow(int id)
        {
            try
            {
                _hangRecords.ForEach(r =>
                {
                    GUIX.Label(r.Bone.GuideObject.transformTarget.name, 10);
                });
            }
            catch (Exception e)
            {
                Tracer.Log(e);
            }
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
            if (Input.GetKeyDown(KeyCode.P) &&
                (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
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
            else if (Input.GetKeyDown(KeyCode.P) &&
                (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
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
            else if (Input.GetKeyDown(KeyCode.P))
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