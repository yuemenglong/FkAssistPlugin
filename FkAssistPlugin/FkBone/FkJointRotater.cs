using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public class FkJointRotater
    {
        private IFkJoint _root;
        private IFkJoint _end;

        public FkJointRotater(IFkJoint root, IFkJoint end)
        {
            _root = root;
            _end = end;
        }

        public bool Fix()
        {
            var vec = _root.Vector + _end.Vector;
            if (vec.magnitude < _root.Vector.magnitude + _end.Vector.magnitude)
            {
                // not need fix
                return false;
            }
            var go = Context.DicGuideObject()[_root.Transform];
            if (go == null || !(go.IsArm() || go.IsLeg()))
            {
                return false;
            }
            if (go.IsArm() && go.transformTarget.name.Contains("_L"))
            {
                _end.Rotate(_end.Transform.up, 1f);
            }
            else if (go.IsArm() && go.transformTarget.name.Contains("_R"))
            {
                _end.Rotate(_end.Transform.up, -1f);
            }
            else if (go.IsLeg())
            {
                _end.Rotate(Vector3.left, -1f);
            }
            return true;
        }

        public Vector3 Vector
        {
            get { return _root.Vector + _end.Vector; }
        }

        public float Angel
        {
            get { return 180.0f - Vector3.Angle(_root.Vector, _end.Vector); }
        }

        public void MoveEndTo(Vector3 pos)
        {
            var target = pos - _root.Transform.position;
            var max = _root.Vector.magnitude + _end.Vector.magnitude;
            if (max < target.magnitude)
            {
//                Tracer.Log("Reach Max");
                return;
            }
            var angle = Vector3.Angle(Vector, target);
            var axis = Vector3.Cross(Vector, target).normalized;
            _root.RotateAround(_root.Transform.position, axis, angle);
            Forward(target.magnitude - Vector.magnitude);
        }

        public void Forward(float value)
        {
            var vec = _root.Vector + _end.Vector;
            var target = vec.magnitude + value;
            if (target > _root.Vector.magnitude + _end.Vector.magnitude)
            {
                return;
            }
            if (Fix())
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
}