using FkAssistPlugin.Util;
using Studio;
using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public static class FkJointAssist
    {
        public static FkLimbRotater LimbRotater(GuideObject go)
        {
            if (go.IsHand() || go.transformTarget.name.StartsWith("cm_"))
            {
                var t = go.transformTarget;
                var tp = t.parent;
                var tpp = tp.parent;
                var root = new TransformFkJoint(tpp, tp);
                var end = new TransformFkJoint(tp, t);
                var rotater = new FkLimbRotater(root, end);
                return rotater;
            }
            else
            {
                var t = go.transformTarget;
                var tp = t.parent.parent;
                var tpp = tp.parent;
                var root = new TransformFkJoint(tpp, tp);
                var end = new TransformFkJoint(tp, t);
                var rotater = new FkLimbRotater(root, end);
                return rotater;
            }
        }

        public static void Forward(this GuideObject go, float dist)
        {
            LimbRotater(go).Forward(dist);
        }

        public static void Revolution(this GuideObject go, float angle)
        {
            LimbRotater(go).Revolution(angle);
        }

        public static void Tangent(this GuideObject go, float angle)
        {
            LimbRotater(go).Tangent(angle);
        }

        public static void Normals(this GuideObject go, float angle)
        {
            LimbRotater(go).Normals(angle);
        }

        public static void MoveEndX(this GuideObject go, float dist)
        {
            LimbRotater(go).MoveEndTo(go.transformTarget.position + new Vector3(dist, 0, 0));
        }

        public static void MoveEndY(this GuideObject go, float dist)
        {
            LimbRotater(go).MoveEndTo(go.transformTarget.position + new Vector3(0, dist, 0));
        }

        public static void MoveEndZ(this GuideObject go, float dist)
        {
            LimbRotater(go).MoveEndTo(go.transformTarget.position + new Vector3(0, 0, dist));
        }

        public static void MoveEnd(this GuideObject go, Vector3 pos)
        {
            LimbRotater(go).MoveEndTo(pos);
        }
    }
}