using System;
using FkAssistPlugin.Util;
using Studio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FkAssistPlugin
{
    public static class BoneAssist
    {
        public static LimbBoneRotater LimbBoneRotater(GuideObject go)
        {
            if (go.IsHand() || go.transformTarget.name.StartsWith("cm_"))
            {
                var t = go.transformTarget;
                var tp = t.parent;
                var tpp = tp.parent;
                var root = new TransformBone(tpp, tp);
                var end = new TransformBone(tp, t);
                var rotater = new LimbBoneRotater(root, end);
                return rotater;
            }
            else
            {
                var t = go.transformTarget;
                var tp = t.parent.parent;
                var tpp = tp.parent;
                var root = new TransformBone(tpp, tp);
                var end = new TransformBone(tp, t);
                var rotater = new LimbBoneRotater(root, end);
                return rotater;
            }
        }

        public static void Forward(this GuideObject go, float dist)
        {
            LimbBoneRotater(go).Forward(dist);
        }

        public static void Revolution(this GuideObject go, float angle)
        {
            LimbBoneRotater(go).Revolution(angle);
        }

        public static void Tangent(this GuideObject go, float angle)
        {
            LimbBoneRotater(go).Tangent(angle);
        }

        public static void Normals(this GuideObject go, float angle)
        {
            LimbBoneRotater(go).Normals(angle);
        }

        public static void MoveEndX(this GuideObject go, float dist)
        {
            LimbBoneRotater(go).MoveEndTo(go.transformTarget.position + new Vector3(dist, 0, 0));
        }

        public static void MoveEndY(this GuideObject go, float dist)
        {
            LimbBoneRotater(go).MoveEndTo(go.transformTarget.position + new Vector3(0, dist, 0));
        }

        public static void MoveEndZ(this GuideObject go, float dist)
        {
            LimbBoneRotater(go).MoveEndTo(go.transformTarget.position + new Vector3(0, 0, dist));
        }

        public static void MoveEnd(this GuideObject go, Vector3 pos)
        {
            LimbBoneRotater(go).MoveEndTo(pos);
        }
    }
}