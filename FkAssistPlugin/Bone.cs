using Studio;
using UniRx.Examples;
using UnityEngine;

namespace FkAssistPlugin
{
    public interface IBone
    {
        Transform Transform { get; }
        Vector3 Vector { get; }

        void Rotate(Vector3 axis, float angle, Space relativeTo);
        void RotateAround(Vector3 point, Vector3 axis, float angle);
    }

    public class BoneAssist
    {
        private IBone _root;
        private IBone _end;

        public BoneAssist(IBone root, IBone end)
        {
            _root = root;
            _end = end;
        }

        public void Forward(float value)
        {
            var vec = _root.Vector + _end.Vector;
            if (vec.magnitude >= _root.Vector.magnitude + _end.Vector.magnitude)
            {
                return;
            }
            var target = vec.magnitude + value;
            if (target >= _root.Vector.magnitude + _end.Vector.magnitude)
            {
                return;
            }
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

        public void Rotate(float angle)
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

    public class GuideObjectBone : IBone
    {
        private Transform _start;
        private Transform _end;

        public GuideObjectBone(Transform start, Transform end)
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