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

    public class BoneEnd
    {
        private IBone _bone;
        private BoneRoot _root;

        public BoneEnd(IBone bone, BoneRoot root)
        {
            _bone = bone;
            _root = root;
            root.End = this;
        }

        public IBone Bone
        {
            get { return _bone; }
        }

        public BoneRoot Root
        {
            get { return _root; }
        }

        public void Forward(float value)
        {
            Root.Forward(value);
        }

        public void Rotate(float value)
        {
            Root.Rotate(value);
        }
    }

    public class BoneRoot
    {
        private IBone _bone;
        private BoneEnd _end;

        public BoneRoot(IBone bone)
        {
            _bone = bone;
        }

        public BoneEnd End
        {
            get { return _end; }
            set { _end = value; }
        }

        public IBone Bone
        {
            get { return _bone; }
        }

        public void Forward(float value)
        {
            var b1 = Bone;
            var b2 = End.Bone;
            var vec = b1.Vector + b2.Vector;
            if (vec.magnitude >= b1.Vector.magnitude + b2.Vector.magnitude)
            {
                return;
            }
            var target = vec.magnitude + value;
            if (target >= b1.Vector.magnitude + b2.Vector.magnitude)
            {
                return;
            }
//            {
//                var old = Kit.Angle(b1.Vector.magnitude, vec.magnitude, b2.Vector.magnitude);
//                var now = Kit.Angle(b1.Vector.magnitude, target, b2.Vector.magnitude);
//                var angle = old - now;
//                var axis = Vector3.Cross(b1.Vector, b2.Vector).normalized;
//                b1.RotateAround(b1.Transform.position, axis, angle);
//            }
            {
                var old = Kit.Angle(b1.Vector.magnitude, b2.Vector.magnitude, vec.magnitude);
                var now = Kit.Angle(b1.Vector.magnitude, b2.Vector.magnitude, target);
                var angle = old - now;
                var axis = Vector3.Cross(b1.Vector, b2.Vector).normalized;
                b2.RotateAround(b2.Transform.position, axis, angle);
            }
        }

        public void Rotate(float angle)
        {
            var b1 = Bone;
            var b2 = End.Bone;
            var vec = b1.Vector + b2.Vector;
            b1.RotateAround(b1.Transform.position, vec, angle);
        }

        public void Tangent(float angle)
        {
            var b1 = Bone;
            var b2 = End.Bone;
            var axis = Vector3.Cross(b1.Vector, b2.Vector);
            b1.RotateAround(b1.Transform.position, axis, angle);
        }

        public void Normals(float angle)
        {
            var b1 = Bone;
            var b2 = End.Bone;
            var vec = b1.Vector + b2.Vector;
            var tan = Vector3.Project(b1.Vector, vec);
            var nor = b1.Vector - tan;
            b1.RotateAround(b1.Transform.position, nor, angle);
        }
    }

    public class GuideObjectBone : IBone
    {
        private GuideObject _go;
        private Transform _child;

        public GuideObjectBone(GuideObject go, Transform child)
        {
            _go = go;
            _child = child;
        }

        public Transform Transform
        {
            get { return _go.transformTarget; }
        }

        public Vector3 Vector
        {
            get { return _child.position - Transform.position; }
        }

        public void Rotate(Vector3 axis, float angle, Space relativeTo)
        {
            _go.transformTarget.Rotate(axis, angle, relativeTo);
            _go.changeAmount.rot = _go.transformTarget.localEulerAngles;
        }

        public void RotateAround(Vector3 point, Vector3 axis, float angle)
        {
            _go.transformTarget.RotateAround(point, axis, angle);
            _go.changeAmount.rot = _go.transformTarget.localEulerAngles;
        }
    }
}