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
//        private static Color _lockedColor = new Color(0.8f, 0f, 0f, 0.4f);

        #region Field

        private FkBone _root;

        private FkBone _hips; //cm_J_Hips

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

        private List<FkBone> _fingers = new List<FkBone>();

        #endregion

        public Dictionary<GuideObject, FkBone> DicGuideBones = new Dictionary<GuideObject, FkBone>();
        public Dictionary<Transform, FkBone> DicTransBones = new Dictionary<Transform, FkBone>();

        public bool IsMale()
        {
            return _root.Name.StartsWith("Male");
        }

        public bool IsFemale()
        {
            return _root.Name.StartsWith("Female");
        }

        public FkBone Root
        {
            get { return _root; }
        }

        public FkChara(Transform root)
        {
            _root = CreateBone(root);
            if (!IsMale() && !IsFemale())
            {
                throw new Exception("Invalid Root");
            }

            Transform hips = null;
            if (IsFemale())
            {
                hips = root.Find("p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips");
            }
            else
            {
                hips = root.Find("p_cm_anim/cm_J_Root/cm_N_height/cm_J_Hips");
            }

            if (hips == null)
            {
                throw new Exception("Can't Find Hips");
            }

            LoopChildren(hips);
            AttachRelation();
        }

        private FkBone CreateBone(Transform transform)
        {
            if (!Context.DicGuideObject().ContainsKey(transform))
            {
//                Tracer.Log("Not Contains", transform);
                return null;
            }

            GuideObject go = Context.DicGuideObject()[transform];
            return new FkBone(go, this);
        }

        private void AttachRelation()
        {
            _head.Parent = _neck;

            _handL.Parent = _armLow01L;
            _armLow01L.Parent = _armUp00L;
            _armUp00L.Parent = _spine02;

            _handR.Parent = _armLow01R;
            _armLow01R.Parent = _armUp00R;
            _armUp00R.Parent = _spine02;

            _spine02.Parent = _spine01;
            _spine01.Parent = _hips;

            _foot01L.Parent = _legLow01L;
            _legLow01L.Parent = _legUp00L;

            _foot01R.Parent = _legLow01R;
            _legLow01R.Parent = _legUp00R;

//            _fingers.ToArray().Filter(f => f != null && !f.Name.Contains("01")).Foreach(f =>
//            {
//                f.Parent = DicTransBones[f.Transform.parent];
//            });

            MainBones().Foreach(b =>
            {
                DicGuideBones.Add(b.GuideObject, b);
                DicTransBones.Add(b.Transform, b);
            });
        }

        public FkBone[] MainBones()
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
                _hips,
                _kosi01,

                _legUp00L,
                _legUp00R,
                _legLow01L,
                _legLow01R,
                _foot01L,
                _foot01R,
                _toes01L,
                _toes01R,
            }.Filter(b => b != null);
        }

        public FkBone[] Limbs()
        {
            return new[] {_handL, _handR, _foot01L, _foot01R};
        }

        private void CheckFinger(Transform transform)
        {
            var names = new String[] {"Thumb", "Index", "Middle", "Ring", "Little"}.Map(n =>
                String.Format("Hand_{0}", n));
            if (names.Filter(n => transform.name.IndexOf(n, StringComparison.Ordinal) > 0).Length == 1)
            {
                _fingers.Add(CreateBone(transform));
            }
        }

        private void LoopChildren(Transform transform)
        {
            if (transform.name == "pcAnimator")
            {
                return;
            }

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
                case "cm_J_Hips":
                case "cf_J_Hips":
                    _hips = CreateBone(transform);
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
                default:
                    CheckFinger(transform);
                    break;
            }

            for (var i = 0; i < transform.childCount; i++)
            {
                LoopChildren(transform.GetChild(i));
            }
        }
    }
}