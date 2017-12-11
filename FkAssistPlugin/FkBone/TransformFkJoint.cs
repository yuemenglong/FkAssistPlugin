using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public class TransformFkJoint : IFkJoint
    {
        private Transform _start;
        private Transform _end;

        public TransformFkJoint(Transform start, Transform end)
        {
            _start = start;
            _end = end;
        }

        public Transform Transform
        {
            get { return _start; }
        }

        public Vector3 Vector
        {
            get { return _end.position - _start.position; }
        }

        public void Rotate(Vector3 axis, float angle, Space relativeTo)
        {
            _start.Rotate(axis, angle, relativeTo);
            _start.GuideObject().changeAmount.rot = _start.localEulerAngles;
        }

        public void RotateAround(Vector3 point, Vector3 axis, float angle)
        {
            _start.RotateAround(point, axis, angle);
            _start.GuideObject().changeAmount.rot = _start.localEulerAngles;
        }
    }
}