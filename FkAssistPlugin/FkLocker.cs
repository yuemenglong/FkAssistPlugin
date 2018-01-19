using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using RootMotion.FinalIK;
using UnityEngine;

namespace FkAssistPlugin
{
    struct LockRecord
    {
        public Vector3 LockedPos;
        public Quaternion LockedRot;
    }

    struct HangRecord
    {
        public FkBone.FkBone Bone;
        public Vector3 Pos;
        public Quaternion Rot;
    }

    struct AttachRecord
    {
        public FkBone.FkBone Leader;
        public FkBone.FkBone Follower;
        public Vector3 Pos;
    }

    public class FkLocker : BaseMgr<FkLocker>
    {
        private static Color _lockedColor = new Color(0.8f, 0f, 0f, 0.4f);
        private static Color _selectorColor = new Color(0.0f, 0.0f, 0.8f, 0.4f);
        private static Color _hangColor = new Color(0.5f, 0.0f, 0.5f, 0.4f);

        private bool _isLockerEnable = false;
        private List<BoneMarker> _limbMarkers = new List<BoneMarker>();
        private Dictionary<FkBone.FkBone, LockRecord> _dicLockRecords = new Dictionary<FkBone.FkBone, LockRecord>();

        private List<BoneMarker> _selectorMarkers = new List<BoneMarker>();
        private List<BoneMarker> _hangMarkers = new List<BoneMarker>();
        private List<AttachRecord> _attachRecords = new List<AttachRecord>();
        private FkBone.FkBone _follower = null;

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

        private void ClearLimbMarker()
        {
            _limbMarkers.ForEach(m => { m.Destroy(); });
            _limbMarkers.Clear();
            _dicLockRecords.Clear();
        }

        private void AttachLimbMarker()
        {
            var chara = FkCharaMgr.FindSelectChara();
            if (chara == null)
            {
                return;
            }
            chara.Limbs().Foreach(b =>
            {
                var marker = BoneMarker.Create(b.Transform);
                _limbMarkers.Add(marker);
                marker.OnDrag = (m) =>
                {
                    var screenVec = m.MouseEndPos - m.MouseStartPos;
                    var pos = Kit.MapScreenVecToWorld(screenVec, b.Transform.position);
                    FkJointAssist.MoveEnd(b.GuideObject, pos);
                };
                marker.OnMidClick = (m) =>
                {
//                    ClearLimbMarker();
                    DisableLimbMarker();
                    _follower = b;
                    AttachSelectorMarker();
                };
                marker.OnRightClick = (m) => { ToggleLockBone(b, m); };
                marker.OnLeftDown = (m) => { UndoRedoHelper.Record(); };
                marker.OnLeftUp = (m) => { UndoRedoHelper.Finish(); };
            });
        }

        private void DisableLimbMarker()
        {
            _limbMarkers.ForEach(m => m.SetActive(false));
        }

        private void EnableLimbMarker()
        {
            _limbMarkers.ForEach(m => m.SetActive(true));
        }

        private void AttachSelectorMarker()
        {
            var chars = FkCharaMgr.FindSelectCharas();
            chars.Foreach(c =>
            {
                c.Bones().Foreach(b =>
                {
                    var marker = BoneMarker.Create(b.Transform);
                    marker.SetColor(_selectorColor);
                    _selectorMarkers.Add(marker);
                    marker.OnLeftClick = (m) =>
                    {
                        var attach = new AttachRecord();
                        attach.Leader = b;
                        attach.Follower = _follower;
                        attach.Pos = attach.Follower.Transform.position - attach.Leader.Transform.position;
                        _attachRecords.Add(attach);
                        ClearSelectorMarker();
                        EnableLimbMarker();
                    };
                });
            });
        }

        private void ClearSelectorMarker()
        {
            _selectorMarkers.ForEach(m => m.Destroy());
            _selectorMarkers.Clear();
            _dicLockRecords.Clear();
        }

        private void AttachHangMarker()
        {
            DisableLimbMarker();
            FkCharaMgr.FindSelectChara().Bones().Foreach(b =>
            {
                var marker = BoneMarker.Create(b.Transform);
                marker.SetColor(_hangColor);
                _hangMarkers.Add(marker);
                marker.OnLeftClick = m =>
                {
                    var r = new HangRecord();
                    r.Bone = b;
                    r.Pos = b.Transform.position;
                    r.Rot = b.Transform.rotation;
                    _hangRecords.Add(r);
                    ClearHangMarker();
                    EnableLimbMarker();
                };
            });
        }

        private void ClearHangMarker()
        {
            _hangMarkers.ForEach(m => m.Destroy());
            _hangMarkers.Clear();
        }

        private void InnerUpdate()
        {
            if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftControl))
            {
                if (_isLockerEnable)
                {
                    AttachHangMarker();
                }
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                if (_isLockerEnable)
                {
                    _isLockerEnable = false;
                    ClearLimbMarker();
                    ClearSelectorMarker();
                    ClearHangMarker();
                    _attachRecords.Clear();
                    _hangRecords.Clear();
                    CameraMgr.Unlock();
                }
                else
                {
                    _isLockerEnable = true;
                    AttachLimbMarker();
                }
                Tracer.Log(_isLockerEnable);
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                CameraMgr.Toggle();
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
            _hangRecords.ForEach(r =>
            {
                var vec = r.Pos - r.Bone.Transform.position;
                if (vec != Vector3.zero)
                {
                    r.Bone.Chara.Root.GuideObject.Move(vec);
                }
                if (r.Rot != r.Bone.Transform.rotation)
                {
                    r.Bone.GuideObject.TurnTo(r.Rot);
                }
            });
            _attachRecords.ForEach(r =>
            {
//                if (r.Leader.Transform == r.Follower.Transform)
//                {
//                    var vec = r.Pos - r.Leader.Transform.position;
//                    r.Leader.Chara.Root.GuideObject.Move(vec);
//                }
//                else
//                {
                var target = r.Leader.Transform.position + r.Pos;
                r.Follower.GuideObject.MoveEnd(target);
//                }
            });
        }
    }
}