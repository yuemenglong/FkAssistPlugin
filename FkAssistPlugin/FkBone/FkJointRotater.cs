using System.Security;
using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public interface IFkJointRotater
    {
        // 移动末端到指定位置
        void MoveTo(Vector3 pos);

        // 伸展
        void Forward(float value);

        // 法线方向移动
        void Revolution(float angle);

        // 切线方向移动
        void Tangent(float angle);

        // 自身旋转
        void Normals(float angle);
    }

    public class FkShoulderRotater : IFkJointRotater
    {
        private FkBone _spine1;
        private FkBone _spine2;
        private FkBone _arm;
        private FkBone _armAno;

        public FkShoulderRotater(FkBone spine1, FkBone spine2, FkBone arm, FkBone armAno)
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
            new FkShoulderRotater(_spine1, _spine2, _armAno, _arm).Normals(angle);
        }

        public void Tangent(float angle)
        {
            var v1 = _arm.Transform.position - _spine1.Transform.position;
            var v2 = _armAno.Transform.position - _spine1.Transform.position;
            var axis = Vector3.Cross(v1, v2);
            _spine1.RotateAroundSelf(axis, angle / 3 * 1);
            _spine2.RotateAroundSelf(axis, angle / 3 * 2);
        }

        public void Normals(float angle)
        {
            var axis1 = _armAno.Transform.position - _spine1.Transform.position;
            var axis2 = _armAno.Transform.position - _spine2.Transform.position;
            _spine1.RotateAroundSelf(axis1, angle / 3 * 1);
            _spine2.RotateAroundSelf(axis2, angle / 3 * 2);
        }
    }

    public class FkLimbRotater : IFkJointRotater
    {
        private IFkJoint _root;
        private IFkJoint _mid;
        private IFkJoint _end;

        public FkLimbRotater(IFkJoint root, IFkJoint mid, IFkJoint end)
        {
            _root = root;
            _mid = mid;
            _end = end;
        }

        public bool Fix()
        {
            var vec = _root.Vector + _mid.Vector;
            if (vec.magnitude < _root.Vector.magnitude + _mid.Vector.magnitude)
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
                _mid.Rotate(_mid.Transform.up, 1f);
            }
            else if (go.IsArm() && go.transformTarget.name.Contains("_R"))
            {
                _mid.Rotate(_mid.Transform.up, -1f);
            }
            else if (go.IsLeg())
            {
                _mid.Rotate(Vector3.left, -1f);
            }

            return true;
        }

        public Vector3 Vector
        {
            get { return _root.Vector + _mid.Vector; }
        }

        public float Angel
        {
            get { return 180.0f - Vector3.Angle(_root.Vector, _mid.Vector); }
        }

        public void MoveTo(Vector3 pos)
        {
            var target = pos - _root.Transform.position;
            var max = _root.Vector.magnitude + _mid.Vector.magnitude;
            if (max < target.magnitude)
            {
                return;
            }

            var oldRot = _end.Transform.rotation;
            var angle = Vector3.Angle(Vector, target);
            var axis = Vector3.Cross(Vector, target).normalized;
            _root.RotateAround(_root.Transform.position, axis, angle);
            Forward(target.magnitude - Vector.magnitude);
            _end.TurnTo(oldRot);
        }

        public void Forward(float value)
        {
            var vec = _root.Vector + _mid.Vector;
            var target = vec.magnitude + value;
            if (target > _root.Vector.magnitude + _mid.Vector.magnitude)
            {
                return;
            }

            if (Fix())
            {
                return;
            }

            // 利用三角函数计算出要移动的角度，相对法线进行移动
            {
                var old = Kit.Angle(_root.Vector.magnitude, vec.magnitude, _mid.Vector.magnitude);
                var now = Kit.Angle(_root.Vector.magnitude, target, _mid.Vector.magnitude);
                var angle = old - now;
                var axis = Vector3.Cross(_root.Vector, _mid.Vector).normalized;
                _root.RotateAround(_root.Transform.position, axis, angle);
            }
            {
                var old = Kit.Angle(_root.Vector.magnitude, _mid.Vector.magnitude, vec.magnitude);
                var now = Kit.Angle(_root.Vector.magnitude, _mid.Vector.magnitude, target);
                var angle = old - now;
                var axis = Vector3.Cross(_root.Vector, _mid.Vector).normalized;
                _mid.RotateAround(_mid.Transform.position, axis, angle);
            }
        }

        public void Revolution(float angle)
        {
            var vec = _root.Vector + _mid.Vector;
            _root.RotateAround(_root.Transform.position, vec, angle);
        }

        public void Tangent(float angle)
        {
            var axis = Vector3.Cross(_root.Vector, _mid.Vector);
            _root.RotateAround(_root.Transform.position, axis, angle);
        }

        public void Normals(float angle)
        {
            var vec = _root.Vector + _mid.Vector;
            var tan = Vector3.Project(_root.Vector, vec);
            var nor = _root.Vector - tan;
            var oldRot = _end.Transform.rotation;
            _root.RotateAround(_root.Transform.position, nor, angle);
            _end.TurnTo(oldRot);
        }
    }
}