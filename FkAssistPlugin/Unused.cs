namespace FkAssistPlugin
{
    public class Unused
    {
//        public static class FkJointRotater
//        {
//            public static void MoveTo(IFkJoint root, IFkJoint end, Vector3 pos)
//            {
//                var vec = root.Vector + end.Vector;
//                var target = pos - root.Transform.position;
//                var max = root.Vector.magnitude + end.Vector.magnitude;
//                if (max < target.magnitude)
//                {
//                    return;
//                }
//
//                var angle = Vector3.Angle(vec, target);
//                var axis = Vector3.Cross(vec, target).normalized;
//                root.RotateAround(root.Transform.position, axis, angle);
//                Forward(root, end, target.magnitude - vec.magnitude);
//            }
//
//            public static void Forward(IFkJoint root, IFkJoint end, float value)
//            {
//                var vec = root.Vector + end.Vector;
//                var target = vec.magnitude + value;
//                if (target > root.Vector.magnitude + end.Vector.magnitude)
//                {
//                    return;
//                }
//
//                {
//                    var old = Kit.Angle(root.Vector.magnitude, vec.magnitude, end.Vector.magnitude);
//                    var now = Kit.Angle(root.Vector.magnitude, target, end.Vector.magnitude);
//                    var angle = old - now;
//                    var axis = Vector3.Cross(root.Vector, end.Vector).normalized;
//                    root.RotateAround(root.Transform.position, axis, angle);
//                }
//                {
//                    var old = Kit.Angle(root.Vector.magnitude, end.Vector.magnitude, vec.magnitude);
//                    var now = Kit.Angle(root.Vector.magnitude, end.Vector.magnitude, target);
//                    var angle = old - now;
//                    var axis = Vector3.Cross(root.Vector, end.Vector).normalized;
//                    end.RotateAround(end.Transform.position, axis, angle);
//                }
//            }
//
//            public static void Revolution(IFkJoint root, IFkJoint end, float angle)
//            {
//                var vec = root.Vector + end.Vector;
//                root.RotateAround(root.Transform.position, vec, angle);
//            }
//
//            public static void Tangent(IFkJoint root, IFkJoint end, float angle)
//            {
//                var axis = Vector3.Cross(root.Vector, end.Vector);
//                root.RotateAround(root.Transform.position, axis, angle);
//            }
//
//            public static void Normals(IFkJoint root, IFkJoint end, float angle)
//            {
//                var vec = root.Vector + end.Vector;
//                var tan = Vector3.Project(root.Vector, vec);
//                var nor = root.Vector - tan;
//                root.RotateAround(root.Transform.position, nor, angle);
//            }
//        }
//
//        public class FkHeadRotater : IFkJointRotater
//        {
//            private FkBone _spine1;
//            private FkBone _spine2;
//            private FkBone _neck;
//
//            public FkHeadRotater(FkBone spine1, FkBone spine2, FkBone neck)
//            {
//                _spine1 = spine1;
//                _spine2 = spine2;
//                _neck = neck;
//            }
//
//            public void MoveTo(Vector3 pos)
//            {
//            }
//
//            public void Forward(float value)
//            {
//            }
//
//            public void Revolution(float angle)
//            {
//                //旋转
//            }
//
//            public void Tangent(float angle)
//            {
//                //切向
//            }
//
//            public void Normals(float angle)
//            {
//                //法向
//            }
//        }
    }

//    public class TransformFkJoint : IFkJoint
//    {
//        private Transform _start;
//        private Transform _end;
//
//        public TransformFkJoint(Transform start, Transform end)
//        {
//            _start = start;
//            _end = end;
//        }
//
//        public Transform Transform
//        {
//            get { return _start; }
//        }
//
//        public Vector3 Vector
//        {
//            get { return _end.position - _start.position; }
//        }
//
//        public void Rotate(Vector3 axis, float angle, Space relativeTo)
//        {
//            _start.Rotate(axis, angle, relativeTo);
//            _start.GuideObject().changeAmount.rot = _start.localEulerAngles;
//        }
//
//        public void RotateAround(Vector3 point, Vector3 axis, float angle)
//        {
//            _start.RotateAround(point, axis, angle);
//            _start.GuideObject().changeAmount.rot = _start.localEulerAngles;
//        }
//    }
}


/*
 * using System;
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
                c.MainBones().Foreach(b =>
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
            FkCharaMgr.FindSelectChara().MainBones().Foreach(b =>
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
*/