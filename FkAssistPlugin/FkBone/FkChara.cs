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
            AttachChild();
            AttachMarker();
        }

        private FkBone CreateBone(Transform transform)
        {
            GuideObject go = Context.DicGuideObject()[transform];
            return new FkBone(go);
        }

        private void AttachMarker()
        {
            Limbs().Foreach(b =>
            {
                b.Marker = BoneMarkerMgr.Instance.Create(b.Transform);
                b.Marker.OnDrag = m =>
                {
                    var screenVec = m.MouseEndPos - m.MouseStartPos;
                    var pos = Kit.MapScreenVecToWorld(screenVec, b.Transform.position);
                    FkRotaterAssist.MoveEnd(b.GuideObject, pos);
                };
            });
        }

        private void AttachChild()
        {
            foreach (var fkBone in Bones())
            {
                switch (fkBone.Name)
                {
                    case "cm_J_Head":
                    case "cf_J_Head":
                        break;
                    case "cm_J_Neck":
                    case "cf_J_Neck":
                        _neck.Child = _head;
                        break;
                    case "cm_J_Spine01":
                    case "cf_J_Spine01":
                        break;
                    case "cm_J_Spine02":
                    case "cf_J_Spine02":
                        break;
                    case "cm_J_Shoulder_L":
                    case "cf_J_Shoulder_L":
                        _shoulderL.Child = _armUp00L;
                        break;
                    case "cm_J_Shoulder_R":
                    case "cf_J_Shoulder_R":
                        _shoulderR.Child = _armUp00R;
                        break;
                    case "cm_J_ArmUp00_L":
                    case "cf_J_ArmUp00_L":
                        _armUp00L.Child = _armLow01L;
                        break;
                    case "cm_J_ArmUp00_R":
                    case "cf_J_ArmUp00_R":
                        _armUp00R.Child = _armLow01R;
                        break;
                    case "cm_J_ArmLow01_L":
                    case "cf_J_ArmLow01_L":
                        _armLow01L.Child = _handL;
                        break;
                    case "cm_J_ArmLow01_R":
                    case "cf_J_ArmLow01_R":
                        _armLow01R.Child = _handR;
                        break;
                    case "cm_J_Hand_L":
                    case "cf_J_Hand_L":
                        break;
                    case "cm_J_Hand_R":
                    case "cf_J_Hand_R":
                        break;
                    case "cm_J_Kosi01":
                    case "cf_J_Kosi01":
                        break;
                    case "cm_J_LegUp00_L":
                    case "cf_J_LegUp00_L":
                        _legUp00L.Child = _legLow01L;
                        break;
                    case "cm_J_LegUp00_R":
                    case "cf_J_LegUp00_R":
                        _legUp00R.Child = _legLow01R;
                        break;
                    case "cm_J_LegLow01_L":
                    case "cf_J_LegLow01_L":
                        _legLow01L.Child = _foot01L;
                        break;
                    case "cm_J_LegLow01_R":
                    case "cf_J_LegLow01_R":
                        _legLow01R.Child = _foot01R;
                        break;
                    case "cm_J_Foot01_L":
                    case "cf_J_Foot01_L":
                        break;
                    case "cm_J_Foot01_R":
                    case "cf_J_Foot01_R":
                        break;
                    case "cm_J_Toes01_L":
                    case "cf_J_Toes01_L":
                        break;
                    case "cm_J_Toes01_R":
                    case "cf_J_Toes01_R":
                        break;
                }
            }
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
    }
}