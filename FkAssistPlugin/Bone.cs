using Studio;
using UniRx.Examples;
using UnityEngine;

namespace FkAssistPlugin
{
    public interface IBone
    {
        Transform Transform { get; }
        Vector3 Vector { get; }

        void Rotate(Vector3 axis, float angle, Space relativeTo = Space.Self);
        void RotateAround(Vector3 point, Vector3 axis, float angle);
    }

    public class BoneRotater
    {
        private IBone _root;
        private IBone _end;

        public BoneRotater(IBone root, IBone end)
        {
            _root = root;
            _end = end;
        }

        public bool IsValid(float dist)
        {
            var vec = _root.Vector + _end.Vector;
            if (vec.magnitude >= _root.Vector.magnitude + _end.Vector.magnitude)
            {
                return false;
            }
            var target = vec.magnitude + dist;
            if (target >= _root.Vector.magnitude + _end.Vector.magnitude)
            {
                return false;
            }
            return true;
        }

        public void Forward(float value)
        {
            if (!IsValid(value))
            {
                return;
            }
            var vec = _root.Vector + _end.Vector;
            var target = vec.magnitude + value;
            {
                var old = Kit.Angle(_root.Vector.magnitude, vec.magnitude, _end.Vector.magnitude);
                var now = Kit.Angle(_root.Vector.magnitude, target, _end.Vector.magnitude);
                var angle = old - now;
                var axis = Vector3.Cross(_root.Vector, _end.Vector).normalized;
                _root.RotateAround(_root.Transform.position, axis, angle);
            }
            {
                var old = Kit.Angle(_root.Vector.magnitude, _end.Vector.magnitude, vec.magnitude);
                var now = Kit.Angle(_root.Vector.magnitude, _end.Vector.magnitude, target);
                var angle = old - now;
                var axis = Vector3.Cross(_root.Vector, _end.Vector).normalized;
                _end.RotateAround(_end.Transform.position, axis, angle);
            }
        }

        public void Revolution(float angle)
        {
            var vec = _root.Vector + _end.Vector;
            _root.RotateAround(_root.Transform.position, vec, angle);
        }

        public void Tangent(float angle)
        {
            var axis = Vector3.Cross(_root.Vector, _end.Vector);
            _root.RotateAround(_root.Transform.position, axis, angle);
        }

        public void Normals(float angle)
        {
            var vec = _root.Vector + _end.Vector;
            var tan = Vector3.Project(_root.Vector, vec);
            var nor = _root.Vector - tan;
            _root.RotateAround(_root.Transform.position, nor, angle);
        }
    }

    public class TransformBone : IBone
    {
        private Transform _start;
        private Transform _end;

        public TransformBone(Transform start, Transform end)
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