using FkAssistPlugin.Util;
using Studio;
using UnityEngine;
using Utility;

namespace FkAssistPlugin.FkBone
{
    public static class FkJointAssist
    {
        public static FkJointRotater FkJointRotater(GuideObject go)
        {
            var chara = FkCharaMgr.CreateChara(go.transformTarget);
            var end = chara.DicTransBones[go.transformTarget];
            return new FkJointRotater(end.Parent.Parent, end.Parent);
        }

        public static void Forward(this GuideObject go, float dist)
        {
            if (go.IsLimb())
                FkJointRotater(go).Forward(dist);
        }

        public static void Revolution(this GuideObject go, float angle)
        {
            if (go.IsLimb())
                FkJointRotater(go).Revolution(angle);
        }

        public static void Tangent(this GuideObject go, float angle)
        {
            if (go.IsLimb())
                FkJointRotater(go).Tangent(angle);
        }

        public static void Normals(this GuideObject go, float angle)
        {
            if (go.IsLimb())
                FkJointRotater(go).Normals(angle);
        }

        public static void MoveEndX(this GuideObject go, float dist)
        {
            if (go.IsLimb())
                FkJointRotater(go).MoveEndTo(go.transformTarget.position + new Vector3(dist, 0, 0));
        }

        public static void MoveEndY(this GuideObject go, float dist)
        {
            if (go.IsLimb())
                FkJointRotater(go).MoveEndTo(go.transformTarget.position + new Vector3(0, dist, 0));
        }

        public static void MoveEndZ(this GuideObject go, float dist)
        {
            if (go.IsLimb())
                FkJointRotater(go).MoveEndTo(go.transformTarget.position + new Vector3(0, 0, dist));
        }

        public static void MoveEnd(this GuideObject go, Vector3 pos)
        {
            if (go.IsLimb())
                FkJointRotater(go).MoveEndTo(pos);
        }
    }
}