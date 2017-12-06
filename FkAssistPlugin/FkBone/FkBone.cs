using System;
using FkAssistPlugin.HSStudioNEOAddno;
using Studio;
using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public class FkBone : IFkJoint
    {
        public GuideObject GuideObject { get; private set; }
        public FkBone Child;
        public BoneMarker Marker;
        public bool IsLocked;

        public FkBone(GuideObject go)
        {
            GuideObject = go;
            IsLocked = false;
        }

        public String Name
        {
            get { return GuideObject.transformTarget.name; }
        }

        public Transform Transform
        {
            get { return GuideObject.transformTarget; }
        }

        public Vector3 Vector
        {
            get { return Child.Transform.position - GuideObject.transformTarget.position; }
        }

        public void Rotate(Vector3 axis, float angle, Space relativeTo = Space.Self)
        {
            GuideObject.transformTarget.Rotate(axis, angle, relativeTo);
            GuideObject.changeAmount.rot = GuideObject.transformTarget.localEulerAngles;
        }

        public void RotateAround(Vector3 point, Vector3 axis, float angle)
        {
            GuideObject.transformTarget.RotateAround(point, axis, angle);
            GuideObject.changeAmount.rot = GuideObject.transformTarget.localEulerAngles;
        }
    }
}