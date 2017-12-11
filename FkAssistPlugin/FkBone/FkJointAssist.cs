using FkAssistPlugin.Util;
using Studio;
using UnityEngine;
using Utility;

namespace FkAssistPlugin.FkBone
{
    public static class FkJointAssist
    {
        public static IFkJointRotater FkJointRotater(GuideObject go)
        {
            var chara = FkCharaMgr.CreateChara(go.transformTarget);
            var point = chara.DicTransBones[go.transformTarget];
            if (go.IsLimb())
            {
                return new FkLimbRotater(point.Parent.Parent, point.Parent);
            }
            if (go.IsArm())
            {
//                var root = new TransformFkJoint(point.Parent.Parent.Transform, point.Parent.Transform);
//                var end = new TransformFkJoint(point.Parent.Transform, point.Transform);
//                return new FkLimbRotater(root, end);
                var ano = point.Parent.Children.Filter(c => c != point)[0];
                return new FkArmRotater(point.Parent.Parent, point.Parent, point, ano);
            }
            return null;
        }

        public static void Forward(this GuideObject go, float dist)
        {
            if (go.IsLimb() || go.IsArm())
                FkJointRotater(go).Forward(dist);
        }

        public static void Revolution(this GuideObject go, float angle)
        {
            if (go.IsLimb() || go.IsArm())
                FkJointRotater(go).Revolution(angle);
        }

        public static void Tangent(this GuideObject go, float angle)
        {
            if (go.IsLimb() || go.IsArm())
                FkJointRotater(go).Tangent(angle);
        }

        public static void Normals(this GuideObject go, float angle)
        {
            if (go.IsLimb() || go.IsArm())
                FkJointRotater(go).Normals(angle);
        }

        public static void MoveEndX(this GuideObject go, float dist)
        {
            if (go.IsLimb() || go.IsArm())
                FkJointRotater(go).MoveTo(go.transformTarget.position + new Vector3(dist, 0, 0));
        }

        public static void MoveEndY(this GuideObject go, float dist)
        {
            if (go.IsLimb() || go.IsArm())
                FkJointRotater(go).MoveTo(go.transformTarget.position + new Vector3(0, dist, 0));
        }

        public static void MoveEndZ(this GuideObject go, float dist)
        {
            if (go.IsLimb() || go.IsArm())
                FkJointRotater(go).MoveTo(go.transformTarget.position + new Vector3(0, 0, dist));
        }

        public static void MoveEnd(this GuideObject go, Vector3 pos)
        {
            if (go.IsLimb() || go.IsArm())
                FkJointRotater(go).MoveTo(pos);
        }
    }
}