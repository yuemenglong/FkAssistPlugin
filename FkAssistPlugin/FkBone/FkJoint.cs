using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public interface IFkJoint
    {
        Transform Transform { get; }
        Vector3 Vector { get; }

        void Rotate(Vector3 axis, float angle, Space relativeTo = Space.Self);
        void RotateAround(Vector3 point, Vector3 axis, float angle);
    }
}