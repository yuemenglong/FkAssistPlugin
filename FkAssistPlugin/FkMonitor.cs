using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin
{
    struct LockRecord
    {
        public Vector3 LockedPos;
        public Quaternion LockedRot;
    }

    public class FkMonitor : BaseMgr<FkMonitor>
    {
        private static Color _lockedColor = new Color(0.8f, 0f, 0f, 0.4f);

        private bool _isMarkerEnable = false;
        private List<BoneMarker> _markers = new List<BoneMarker>();
        private Dictionary<FkBone.FkBone, LockRecord> _dicLockRecords = new Dictionary<FkBone.FkBone, LockRecord>();

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

        private void ToggleLockBone(FkBone.FkBone bone, BoneMarker m)
        {
            if (_dicLockRecords.ContainsKey(bone))
            {
                _dicLockRecords.Remove(bone);
                m.OnDrag = null;
                m.SetDefaultColor();
            }
            else
            {
                var record = new LockRecord();
                record.LockedPos = bone.Transform.position;
                record.LockedRot = bone.Transform.rotation;
                _dicLockRecords.Add(bone, record);
                m.SetColor(_lockedColor);
            }
        }

        private void InnerUpdate()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (_isMarkerEnable)
                {
                    _isMarkerEnable = false;
                    _markers.ForEach(m => { m.Destroy(); });
                    _markers.Clear();
                    _dicLockRecords.Clear();
                }
                else
                {
                    _isMarkerEnable = true;
                    var chara = FkCharaMgr.FindSelectChara();
                    chara.Limbs().Foreach(b =>
                    {
                        var marker = BoneMarker.Create(b.Transform);
                        marker.OnDrag = (m) =>
                        {
                            var screenVec = m.MouseEndPos - m.MouseStartPos;
                            var pos = Kit.MapScreenVecToWorld(screenVec, b.Transform.position);
                            FkJointAssist.MoveEnd(b.GuideObject, pos);
                        };
                        marker.OnRightClick = (m) => { ToggleLockBone(b, m); };
                    });
                }
            }
            // 移动到Lock的位置
            foreach (var pair in _dicLockRecords)
            {
                var b = pair.Key;
                var r = pair.Value;
                if (b.Transform.position != r.LockedPos && b.GuideObject.IsLimb())
                {
                    FkJointAssist.FkJointRotater(b.GuideObject).MoveTo(r.LockedPos);
                }
                if (b.Transform.rotation != r.LockedRot)
                {
                    b.GuideObject.TurnTo(r.LockedRot);
                }
            }
        }
    }
}

//                    b.IsLocked = !b.IsLocked;
//                    if (b.IsLocked)
//                    {
//                        b.LockedPos = b.Transform.position;
//                        b.LockedRot = b.Transform.rotation;
//                        b.Marker.SetColor(_lockedColor);
//                    }
//                    else
//                    {
//                        b.Marker.SetDefaultColor();
//                    }

//                });
//            }
//                if (FkCharaMgr.IsMarkerEnabled)
//                {
//                    FkCharaMgr.ClearChars();
////                    FkCharaMgr.IsMarkerEnabled = false;
//                    FkCharaMgr.DisableMarker();
//                }
//                else
//                {
//                    FkCharaMgr.RefreshSelectChara();
//                    FkCharaMgr.EnableMarker();
////                    FkCharaMgr.IsMarkerEnabled = true;
//                }
//        }

//            if (FkCharaMgr.IsMarkerEnabled)
//            {
//                FkCharaMgr.MoveLocked();
//            }