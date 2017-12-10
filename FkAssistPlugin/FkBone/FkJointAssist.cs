using FkAssistPlugin.Util;
using Studio;
using UnityEngine;
using Utility;

namespace FkAssistPlugin.FkBone
{
    public static class FkJointAssist
    {
        public static FkJointRotater LimbRotater(GuideObject go)
        {
            if (go.IsLimb())
            {
                if (go.IsHand() || go.IsMale())
                {
                    var t = go.transformTarget;
                    var tp = t.parent;
                    var tpp = tp.parent;
                    var root = new TransformFkJoint(tpp, tp);
                    var end = new TransformFkJoint(tp, t);
                    var rotater = new FkJointRotater(root, end);
                    return rotater;
                }
                else
                {
                    var t = go.transformTarget;
                    var tp = t.parent.parent;
                    var tpp = tp.parent;
                    var root = new TransformFkJoint(tpp, tp);
                    var end = new TransformFkJoint(tp, t);
                    var rotater = new FkJointRotater(root, end);
                    return rotater;
                }
            }
            else
            {
                return null;
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
            LimbRotater(go).MoveLimbTo(go.transformTarget.position + new Vector3(dist, 0, 0));
        }

        public static void MoveEndY(this GuideObject go, float dist)
        {
            LimbRotater(go).MoveLimbTo(go.transformTarget.position + new Vector3(0, dist, 0));
        }

        public static void MoveEndZ(this GuideObject go, float dist)
        {
            LimbRotater(go).MoveLimbTo(go.transformTarget.position + new Vector3(0, 0, dist));
        }

        public static void MoveEnd(this GuideObject go, Vector3 pos)
        {
            LimbRotater(go).MoveLimbTo(pos);
        }
    }
}