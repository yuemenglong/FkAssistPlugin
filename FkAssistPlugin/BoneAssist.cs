using System;
using Studio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FkAssistPlugin
{
    public static class BoneAssist
    {
        public static bool IsHand(this GuideObject go)
        {
            var name = go.transformTarget.name;
            return name == "cf_J_Hand_L"
                   || name == "cf_J_Hand_R"
                   || name == "cm_J_Hand_L"
                   || name == "cm_J_Hand_R";
        }

        public static bool IsFoot(this GuideObject go)
        {
            var name = go.transformTarget.name;
            return name == "cf_J_Foot01_L"
                   || name == "cf_J_Foot01_R"
                   || name == "cm_J_Foot01_L"
                   || name == "cm_J_Foot01_R";
        }

        public static bool IsLimb(this GuideObject go)
        {
            return go.IsHand() || go.IsFoot();
        }

        public static GuideObject GuideObject(this Transform transform)
        {
            return Context.DicGuideObject()[transform];
        }

        public static void Rotate(this GuideObject guideObject, float z, float y, float x)
        {
            if (guideObject.enableRot)
            {
                guideObject.transformTarget.Rotate(z, y, x, Space.Self);
                guideObject.changeAmount.rot = guideObject.transformTarget.localEulerAngles;
            }
        }
        
        public static void Reset(this GuideObject guideObject)
        {
            if (guideObject.enableRot)
            {
                guideObject.transformTarget.localEulerAngles = Vector3.zero;
                guideObject.changeAmount.rot = guideObject.transformTarget.localEulerAngles;
            }
        }

        private static BoneRotater BoneRotater(GuideObject go)
        {
            if (go.IsHand() || go.transformTarget.name.StartsWith("cm_"))
            {
                var t = go.transformTarget;
                var tp = t.parent;
                var tpp = tp.parent;
                var root = new TransformBone(tpp, tp);
                var end = new TransformBone(tp, t);
                var rotater = new BoneRotater(root, end);
                return rotater;
            }
            else
            {
                var t = go.transformTarget;
                var tp = t.parent.parent;
                var tpp = tp.parent;
                var root = new TransformBone(tpp, tp);
                var end = new TransformBone(tp, t);
                var rotater = new BoneRotater(root, end);
                return rotater; 
            }
        }

        public static void Forward(this GuideObject go, float dist)
        {
            BoneRotater(go).Forward(dist);
        }

        public static void Revolution(this GuideObject go, float angle)
        {
            BoneRotater(go).Revolution(angle);
        }

        public static void Tangent(this GuideObject go, float angle)
        {
            BoneRotater(go).Tangent(angle);
        }

        public static void Normals(this GuideObject go, float angle)
        {
            BoneRotater(go).Normals(angle);
        }
    }
}