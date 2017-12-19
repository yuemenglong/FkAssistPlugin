using System.Security;
using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public interface IFkJointRotater
    {
        void MoveTo(Vector3 pos);
        void Forward(float value);
        void Revolution(float angle);
        void Tangent(float angle);
        void Normals(float angle);
    }

    public static class FkJointRotater
    {
        public static void MoveTo(IFkJoint root, IFkJoint end, Vector3 pos)
        {
            var vec = root.Vector + end.Vector;
            var target = pos - root.Transform.position;
            var max = root.Vector.magnitude + end.Vector.magnitude;
            if (max < target.magnitude)
            {
                return;
            }
            var angle = Vector3.Angle(vec, target);
            var axis = Vector3.Cross(vec, target).normalized;
            root.RotateAround(root.Transform.position, axis, angle);
            Forward(root, end, target.magnitude - vec.magnitude);
        }

        public static void Forward(IFkJoint root, IFkJoint end, float value)
        {
            var vec = root.Vector + end.Vector;
            var target = vec.magnitude + value;
            if (target > root.Vector.magnitude + end.Vector.magnitude)
            {
                return;
            }
            {
                var old = Kit.Angle(root.Vector.magnitude, vec.magnitude, end.Vector.magnitude);
                var now = Kit.Angle(root.Vector.magnitude, target, end.Vector.magnitude);
                var angle = old - now;
                var axis = Vector3.Cross(root.Vector, end.Vector).normalized;
                root.RotateAround(root.Transform.position, axis, angle);
            }
            {
                var old = Kit.Angle(root.Vector.magnitude, end.Vector.magnitude, vec.magnitude);
                var now = Kit.Angle(root.Vector.magnitude, end.Vector.magnitude, target);
                var angle = old - now;
                var axis = Vector3.Cross(root.Vector, end.Vector).normalized;
                end.RotateAround(end.Transform.position, axis, angle);
            }
        }

        public static void Revolution(IFkJoint root, IFkJoint end, float angle)
        {
            var vec = root.Vector + end.Vector;
            root.RotateAround(root.Transform.position, vec, angle);
        }

        public static void Tangent(IFkJoint root, IFkJoint end, float angle)
        {
            var axis = Vector3.Cross(root.Vector, end.Vector);
            root.RotateAround(root.Transform.position, axis, angle);
        }

        public static void Normals(IFkJoint root, IFkJoint end, float angle)
        {
            var vec = root.Vector + end.Vector;
            var tan = Vector3.Project(root.Vector, vec);
            var nor = root.Vector - tan;
            root.RotateAround(root.Transform.position, nor, angle);
        }
    }

    public class FkHeadRotater : IFkJointRotater
    {
        private FkBone _spine1;
        private FkBone _spine2;
        private FkBone _neck;

        public FkHeadRotater(FkBone spine1, FkBone spine2, FkBone neck)
        {
            _spine1 = spine1;
            _spine2 = spine2;
            _neck = neck;
        }

        public void MoveTo(Vector3 pos)
        {
        }

        public void Forward(float value)
        {
        }

        public void Revolution(float angle)
        {
            //旋转
        }

        public void Tangent(float angle)
        {
            //切向
        }

        public void Normals(float angle)
        {
            //法向
        }
    }

    public class FkArmRotater : IFkJointRotater
    {
        private FkBone _spine1;
        private FkBone _spine2;
        private FkBone _arm;
        private FkBone _armAno;

        public FkArmRotater(FkBone spine1, FkBone spine2, FkBone arm, FkBone armAno)
        {
            _spine1 = spine1;
            _spine2 = spine2;
            _arm = arm;
            _armAno = armAno;
        }

        public void MoveTo(Vector3 pos)
        {
            // 肩膀也不适合移动
//            var root = new TransformFkJoint(_spine1.Transform, _spine2.Transform);
//            var end = new TransformFkJoint(_spine2.Transform, _arm.Transform);
//            FkJointRotater.MoveTo(root, end, pos);
        }

        public void Forward(float value)
        {
            // 肩膀拉伸效果不好
//            var root = new TransformFkJoint(_spine1.Transform, _spine2.Transform);
//            var end = new TransformFkJoint(_spine2.Transform, _arm.Transform);
//            FkJointRotater.Forward(root, end, value);
        }

        public void Revolution(float angle)
        {
            new FkArmRotater(_spine1, _spine2, _armAno, _arm).Normals(angle);
        }

        public void Tangent(float angle)
        {
            var v1 = _arm.Transform.position - _spine1.Transform.position;
            var v2 = _armAno.Transform.position - _spine1.Transform.position;
            var axis = Vector3.Cross(v1, v2);
            _spine1.RotateSelf(axis, angle / 3 * 1);
            _spine2.RotateSelf(axis, angle / 3 * 2);
        }

        public void Normals(float angle)
        {
            var axis1 = _armAno.Transform.position - _spine1.Transform.position;
            var axis2 = _armAno.Transform.position - _spine2.Transform.position;
            _spine1.RotateSelf(axis1, angle / 3 * 1);
            _spine2.RotateSelf(axis2, angle / 3 * 2);
        }
    }

    public class FkLimbRotater : IFkJointRotater
    {
        private IFkJoint _root;
        private IFkJoint _end;

        public FkLimbRotater(IFkJoint root, IFkJoint end)
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

        public void MoveTo(Vector3 pos)
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