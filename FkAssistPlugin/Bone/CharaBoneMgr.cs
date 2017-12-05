using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FkAssistPlugin.Util;
using IllusionUtility.GetUtility;
using Studio;
using UnityEngine;
using UnityEngine.Assertions;

namespace FkAssistPlugin.Bone
{
    public class FkChara
    {
        private GuideObject _root; //cm_J_Hips

        private GuideObject _head; //cm_J_Head
        private GuideObject _neck; //cm_J_Neck

        private GuideObject _spine01; //cm_J_Spine01
        private GuideObject _spine02; //cm_J_Spine02

        private GuideObject _shoulderL; //cm_J_Shoulder_L
        private GuideObject _shoulderR; //cm_J_Shoulder_R
        private GuideObject _armUp00L; //cm_J_ArmUp00_L
        private GuideObject _armUp00R; //cm_J_ArmUp00_R
        private GuideObject _armLow01L; //cm_J_ArmLow01_L
        private GuideObject _armLow01R; //cm_J_ArmLow01_R
        private GuideObject _handL; //cm_J_Hand_L
        private GuideObject _handR; //cm_J_Hand_R

        private GuideObject _kosi01; //cm_J_Kosi01

        private GuideObject _legUp00L; //cm_J_LegUp00_L
        private GuideObject _legUp00R; //cm_J_LegUp00_R
        private GuideObject _legLow01L; //cm_J_LegLow01_L
        private GuideObject _legLow01R; //cm_J_LegLow01_R
        private GuideObject _foot01L; //cm_J_Foot01_L
        private GuideObject _foot01R; //cm_J_Foot01_R
        private GuideObject _toes01L; //cm_J_Toes01_L
        private GuideObject _toes01R; //cm_J_Toes01_R

        public bool IsMale()
        {
            return _root.transformTarget.name == "cm_J_Hips";
        }

        public bool IsFemale()
        {
            return _root.transformTarget.name == "cf_J_Hips";
        }

        public FkChara(GuideObject root)
        {
            _root = root;
            if (!IsMale() && !IsFemale())
            {
                throw new Exception("Invalid Root");
            }
            LoopChildren(_root.transformTarget);
        }

        private GuideObject GuideObject(Transform transform)
        {
            Tracer.Log("Find", transform);
            return Context.DicGuideObject()[transform];
        }

        public GuideObject[] GuideObjects()
        {
            return new GuideObject[]
            {
                _head,
                _neck,

                _spine01,
                _spine02,

                _shoulderL,
                _shoulderR,
                _armUp00L,
                _armUp00R,
                _armLow01L,
                _armLow01R,
                _handL,
                _handR,

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

        public GuideObject[] Limbs()
        {
            return new[] {_handL, _handR, _foot01L, _foot01R};
        }

        private void LoopChildren(Transform transform)
        {
            switch (transform.name)
            {
                case "cm_J_Head":
                case "cf_J_Head":
                    _head = GuideObject(transform);
                    break;
                case "cm_J_Neck":
                case "cf_J_Neck":
                    _neck = GuideObject(transform);
                    break;
                case "cm_J_Spine01":
                case "cf_J_Spine01":
                    _spine01 = GuideObject(transform);
                    break;
                case "cm_J_Spine02":
                case "cf_J_Spine02":
                    _spine02 = GuideObject(transform);
                    break;
                case "cm_J_Shoulder_L":
                case "cf_J_Shoulder_L":
                    _shoulderL = GuideObject(transform);
                    break;
                case "cm_J_Shoulder_R":
                case "cf_J_Shoulder_R":
                    _shoulderR = GuideObject(transform);
                    break;
                case "cm_J_ArmUp00_L":
                case "cf_J_ArmUp00_L":
                    _armUp00L = GuideObject(transform);
                    break;
                case "cm_J_ArmUp00_R":
                case "cf_J_ArmUp00_R":
                    _armUp00R = GuideObject(transform);
                    break;
                case "cm_J_ArmLow01_L":
                case "cf_J_ArmLow01_L":
                    _armLow01L = GuideObject(transform);
                    break;
                case "cm_J_ArmLow01_R":
                case "cf_J_ArmLow01_R":
                    _armLow01R = GuideObject(transform);
                    break;
                case "cm_J_Hand_L":
                case "cf_J_Hand_L":
                    _handL = GuideObject(transform);
                    break;
                case "cm_J_Hand_R":
                case "cf_J_Hand_R":
                    _handR = GuideObject(transform);
                    break;
                case "cm_J_Kosi01":
                case "cf_J_Kosi01":
                    _kosi01 = GuideObject(transform);
                    break;
                case "cm_J_LegUp00_L":
                case "cf_J_LegUp00_L":
                    _legUp00L = GuideObject(transform);
                    break;
                case "cm_J_LegUp00_R":
                case "cf_J_LegUp00_R":
                    _legUp00R = GuideObject(transform);
                    break;
                case "cm_J_LegLow01_L":
                case "cf_J_LegLow01_L":
                    _legLow01L = GuideObject(transform);
                    break;
                case "cm_J_LegLow01_R":
                case "cf_J_LegLow01_R":
                    _legLow01R = GuideObject(transform);
                    break;
                case "cm_J_Foot01_L":
                case "cf_J_Foot01_L":
                    _foot01L = GuideObject(transform);
                    break;
                case "cm_J_Foot01_R":
                case "cf_J_Foot01_R":
                    _foot01R = GuideObject(transform);
                    break;
                case "cm_J_Toes01_L":
                case "cf_J_Toes01_L":
                    _toes01L = GuideObject(transform);
                    break;
                case "cm_J_Toes01_R":
                case "cf_J_Toes01_R":
                    _toes01R = GuideObject(transform);
                    break;
            }
            for (var i = 0; i < transform.childCount; i++)
            {
                LoopChildren(transform.GetChild(i));
            }
        }
    }

    public class CharaBoneMgr
    {
        public static FkChara[] FindSelectChara()
        {
            var set = new HashSet<GuideObject>();
            if (Context.GuideObjectManager().selectObjects == null)
            {
                return new FkChara[] { };
            }
            foreach (var guideObject in Context.GuideObjectManager().selectObjects)
            {
                var transform = guideObject.transformTarget.FindParentLoopByRegex(@"^c[fm]_J_Hips$");
                if (transform == null)
                {
                    continue;
                }
                if (!Context.DicGuideObject().ContainsKey(transform))
                {
                    continue;
                }
                set.Add(Context.DicGuideObject()[transform]);
            }
            var list = new List<FkChara>();
            foreach (var guideObject in set)
            {
                Tracer.Log("1", guideObject.transformTarget);
                list.Add(new FkChara(guideObject));
            }
            return list.ToArray();
        }
    }
}