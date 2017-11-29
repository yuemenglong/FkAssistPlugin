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

    public class BoneEndPoint
    {
        private IBone _bone;
        private BoneRootPoint _root;

        public BoneEndPoint(IBone bone, BoneRootPoint root)
        {
            _bone = bone;
            _root = root;
            root.End = this;
        }

        public IBone Bone
        {
            get { return _bone; }
        }

        public BoneRootPoint Root
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

    public class BoneRootPoint
    {
        private IBone _bone;
        private BoneEndPoint _end;

        public BoneRootPoint(IBone bone)
        {
            _bone = bone;
        }

        public BoneEndPoint End
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
            var target = vec.magnitude + value;
            if (target >= b1.Vector.magnitude + b2.Vector.magnitude)
            {
                return;
            }
            {
                var old = Kit.Angle(b1.Vector.magnitude, vec.magnitude, b2.Vector.magnitude);
                var now = Kit.Angle(b1.Vector.magnitude, target, b2.Vector.magnitude);
                var angle = old - now;
                var axis = Vector3.Cross(b1.Vector, b2.Vector).normalized;
                b1.Transform.RotateAround(b1.Transform.position, axis, angle);
            }
            {
                var old = Kit.Angle(b1.Vector.magnitude, b2.Vector.magnitude, vec.magnitude);
                var now = Kit.Angle(b1.Vector.magnitude, b2.Vector.magnitude, target);
                var angle = old - now;
                var axis = Vector3.Cross(b1.Vector, b2.Vector).normalized;
                b2.Transform.RotateAround(b2.Transform.position, axis, angle);
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
}