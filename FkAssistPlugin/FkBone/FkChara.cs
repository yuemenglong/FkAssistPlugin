using System;
using System.Collections.Generic;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using Studio;
using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public class FkChara
    {
        private static Color _lockedColor = new Color(0.8f, 0f, 0f, 0.4f);

        #region Field

        private FkBone _root; //cm_J_Hips

        private FkBone _head; //cm_J_Head
        private FkBone _neck; //cm_J_Neck

        private FkBone _shoulderL; //cm_J_Shoulder_L
        private FkBone _shoulderR; //cm_J_Shoulder_R
        private FkBone _armUp00L; //cm_J_ArmUp00_L
        private FkBone _armUp00R; //cm_J_ArmUp00_R
        private FkBone _armLow01L; //cm_J_ArmLow01_L
        private FkBone _armLow01R; //cm_J_ArmLow01_R
        private FkBone _handL; //cm_J_Hand_L
        private FkBone _handR; //cm_J_Hand_R

        private FkBone _spine02; //cm_J_Spine02
        private FkBone _spine01; //cm_J_Spine01
        private FkBone _kosi01; //cm_J_Kosi01

        private FkBone _legUp00L; //cm_J_LegUp00_L
        private FkBone _legUp00R; //cm_J_LegUp00_R
        private FkBone _legLow01L; //cm_J_LegLow01_L
        private FkBone _legLow01R; //cm_J_LegLow01_R
        private FkBone _foot01L; //cm_J_Foot01_L
        private FkBone _foot01R; //cm_J_Foot01_R
        private FkBone _toes01L; //cm_J_Toes01_L
        private FkBone _toes01R; //cm_J_Toes01_R

        #endregion

        public Dictionary<GuideObject, FkBone> DicGuideBones = new Dictionary<GuideObject, FkBone>();

        public bool IsMale()
        {
            return _root.Name == "cm_J_Hips";
        }

        public bool IsFemale()
        {
            return _root.Name == "cf_J_Hips";
        }

        public FkChara(Transform root)
        {
            _root = CreateBone(root);
            if (!IsMale() && !IsFemale())
            {
                throw new Exception("Invalid Root");
            }
            LoopChildren(root);
            AttachRelation();
//            AttachMarker();
        }

        private FkBone CreateBone(Transform transform)
        {
            GuideObject go = Context.DicGuideObject()[transform];
            return new FkBone(go, this);
        }

        public void DetachMarker()
        {
            Bones().Foreach(b =>
            {
                if (b.Marker != null)
                {
                    b.Marker.Destroy();
                    b.Marker = null;
                    b.IsLocked = false;
                }
            });
        }

        public void AttachMarker()
        {
            Limbs().Foreach(b =>
            {
                b.Marker = BoneMarker.Create(b.Transform);
                b.Marker.OnDrag = m =>
                {
                    var screenVec = m.MouseEndPos - m.MouseStartPos;
                    var pos = Kit.MapScreenVecToWorld(screenVec, b.Transform.position);
                    FkJointAssist.MoveEnd(b.GuideObject, pos);
                };
                b.Marker.OnRightClick = marker =>
                {
                    b.IsLocked = !b.IsLocked;
                    if (b.IsLocked)
                    {
                        b.LockedPos = b.Transform.position;
                        b.LockedRot = b.Transform.rotation;
                        b.Marker.SetColor(_lockedColor);
                    }
                    else
                    {
                        b.Marker.SetDefaultColor();
                    }
                };
            });
            Legs().Foreach(b =>
            {
                b.Marker = BoneMarker.Create(b.Transform);
                b.Marker.OnRightClick = marker =>
                {
                    b.IsLocked = !b.IsLocked;
                    if (b.IsLocked)
                    {
                        b.LockedPos = b.Transform.position;
                        b.LockedRot = b.Transform.rotation;
                        b.Marker.SetColor(_lockedColor);
                    }
                    else
                    {
                        b.Marker.SetDefaultColor();
                    }
                };
            });
        }

        private void AttachRelation()
        {
//            _neck.Child = _head;
            _head.Parent = _neck;
            _neck.Parent = _spine02;
            _spine02.Parent = _spine01;

            _handL.Parent = _armLow01L;
            _armLow01L.Parent = _armUp00L;
            _armUp00L.Parent = _spine02;
            
            _handR.Parent = _armLow01R;
            _armLow01R.Parent = _armUp00R;
            _armUp00R.Parent = _spine02;

//            _shoulderL.Child = _armUp00L;
//            _shoulderR.Child = _armUp00R;
//            _armUp00L.Child = _armLow01L;
//            _armUp00R.Child = _armLow01R;
//            _armLow01L.Child = _handL;
//            _armLow01R.Child = _handR;
            _foot01L.Parent = _legLow01L;
            _legLow01L.Parent = _legUp00L;
            
            _foot01R.Parent = _legLow01R;
            _legLow01R.Parent = _legUp00R;

//            _legUp00L.Child = _legLow01L;
//            _legUp00R.Child = _legLow01R;
//            _legLow01L.Child = _foot01L;
//            _legLow01R.Child = _foot01R;
        }

        public FkBone[] Bones()
        {
            return new[]
            {
                _head,
                _neck,

                _shoulderL,
                _shoulderR,
                _armUp00L,
                _armUp00R,
                _armLow01L,
                _armLow01R,
                _handL,
                _handR,

                _spine02,
                _spine01,
                _kosi01,

                _legUp00L,
                _legUp00R,
                _legLow01L,
                _legLow01R,
                _foot01L,
                _foot01R,
                _toes01L,
                _toes01R,
            };
        }

        public GuideObject[] GuideObjects()
        {
            var list = new List<GuideObject>();
            foreach (var fkBone in Bones())
            {
                list.Add(fkBone.GuideObject);
            }
            return list.ToArray();
        }

        public FkBone[] Limbs()
        {
            return new[] {_handL, _handR, _foot01L, _foot01R};
        }

        public FkBone[] Legs()
        {
            return new[] {_legUp00L, _legUp00R};
        }

        private void LoopChildren(Transform transform)
        {
            switch (transform.name)
            {
                case "cm_J_Head":
                case "cf_J_Head":
                    _head = CreateBone(transform);
                    break;
                case "cm_J_Neck":
                case "cf_J_Neck":
                    _neck = CreateBone(transform);
                    break;
                case "cm_J_Spine01":
                case "cf_J_Spine01":
                    _spine01 = CreateBone(transform);
                    break;
                case "cm_J_Spine02":
                case "cf_J_Spine02":
                    _spine02 = CreateBone(transform);
                    break;
                case "cm_J_Shoulder_L":
                case "cf_J_Shoulder_L":
                    _shoulderL = CreateBone(transform);
                    break;
                case "cm_J_Shoulder_R":
                case "cf_J_Shoulder_R":
                    _shoulderR = CreateBone(transform);
                    break;
                case "cm_J_ArmUp00_L":
                case "cf_J_ArmUp00_L":
                    _armUp00L = CreateBone(transform);
                    break;
                case "cm_J_ArmUp00_R":
                case "cf_J_ArmUp00_R":
                    _armUp00R = CreateBone(transform);
                    break;
                case "cm_J_ArmLow01_L":
                case "cf_J_ArmLow01_L":
                    _armLow01L = CreateBone(transform);
                    break;
                case "cm_J_ArmLow01_R":
                case "cf_J_ArmLow01_R":
                    _armLow01R = CreateBone(transform);
                    break;
                case "cm_J_Hand_L":
                case "cf_J_Hand_L":
                    _handL = CreateBone(transform);
                    break;
                case "cm_J_Hand_R":
                case "cf_J_Hand_R":
                    _handR = CreateBone(transform);
                    break;
                case "cm_J_Kosi01":
                case "cf_J_Kosi01":
                    _kosi01 = CreateBone(transform);
                    break;
                case "cm_J_LegUp00_L":
                case "cf_J_LegUp00_L":
                    _legUp00L = CreateBone(transform);
                    break;
                case "cm_J_LegUp00_R":
                case "cf_J_LegUp00_R":
                    _legUp00R = CreateBone(transform);
                    break;
                case "cm_J_LegLow01_L":
                case "cf_J_LegLow01_L":
                    _legLow01L = CreateBone(transform);
                    break;
                case "cm_J_LegLow01_R":
                case "cf_J_LegLow01_R":
                    _legLow01R = CreateBone(transform);
                    break;
                case "cm_J_Foot01_L":
                case "cf_J_Foot01_L":
                    _foot01L = CreateBone(transform);
                    break;
                case "cm_J_Foot01_R":
                case "cf_J_Foot01_R":
                    _foot01R = CreateBone(transform);
                    break;
                case "cm_J_Toes01_L":
                case "cf_J_Toes01_L":
                    _toes01L = CreateBone(transform);
                    break;
                case "cm_J_Toes01_R":
                case "cf_J_Toes01_R":
                    _toes01R = CreateBone(transform);
                    break;
            }
            for (var i = 0; i < transform.childCount; i++)
            {
                LoopChildren(transform.GetChild(i));
            }
        }

        public void MoveLocked()
        {
            Limbs().Filter(b => { return b.IsLocked; }).Foreach(b =>
            {
                if (b.Transform.position != b.LockedPos)
                {
                    if (b.GuideObject.IsLimb())
                    {
                        FkJointAssist.LimbRotater(b.GuideObject).MoveLimbTo(b.LockedPos);
                    }
                }
                if (b.Transform.rotation != b.LockedRot)
                {
                    b.GuideObject.TurnTo(b.LockedRot);
                }
            });
            Legs().Filter(b => b.IsLocked).Foreach(b =>
            {
                if (b.Transform.rotation != b.LockedRot)
                {
                    b.GuideObject.TurnTo(b.LockedRot);
                }
            });
        }

        public void Destroy()
        {
            Bones().Foreach(b =>
            {
                if (b.Marker != null)
                {
                    b.Marker.Destroy();
                    b.Marker = null;
                    b.IsLocked = false;
                }
            });
        }
    }
}