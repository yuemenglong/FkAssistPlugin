namespace FkAssistPlugin
{
    public class Unused
    {
//        public static class FkJointRotater
//        {
//            public static void MoveTo(IFkJoint root, IFkJoint end, Vector3 pos)
//            {
//                var vec = root.Vector + end.Vector;
//                var target = pos - root.Transform.position;
//                var max = root.Vector.magnitude + end.Vector.magnitude;
//                if (max < target.magnitude)
//                {
//                    return;
//                }
//
//                var angle = Vector3.Angle(vec, target);
//                var axis = Vector3.Cross(vec, target).normalized;
//                root.RotateAround(root.Transform.position, axis, angle);
//                Forward(root, end, target.magnitude - vec.magnitude);
//            }
//
//            public static void Forward(IFkJoint root, IFkJoint end, float value)
//            {
//                var vec = root.Vector + end.Vector;
//                var target = vec.magnitude + value;
//                if (target > root.Vector.magnitude + end.Vector.magnitude)
//                {
//                    return;
//                }
//
//                {
//                    var old = Kit.Angle(root.Vector.magnitude, vec.magnitude, end.Vector.magnitude);
//                    var now = Kit.Angle(root.Vector.magnitude, target, end.Vector.magnitude);
//                    var angle = old - now;
//                    var axis = Vector3.Cross(root.Vector, end.Vector).normalized;
//                    root.RotateAround(root.Transform.position, axis, angle);
//                }
//                {
//                    var old = Kit.Angle(root.Vector.magnitude, end.Vector.magnitude, vec.magnitude);
//                    var now = Kit.Angle(root.Vector.magnitude, end.Vector.magnitude, target);
//                    var angle = old - now;
//                    var axis = Vector3.Cross(root.Vector, end.Vector).normalized;
//                    end.RotateAround(end.Transform.position, axis, angle);
//                }
//            }
//
//            public static void Revolution(IFkJoint root, IFkJoint end, float angle)
//            {
//                var vec = root.Vector + end.Vector;
//                root.RotateAround(root.Transform.position, vec, angle);
//            }
//
//            public static void Tangent(IFkJoint root, IFkJoint end, float angle)
//            {
//                var axis = Vector3.Cross(root.Vector, end.Vector);
//                root.RotateAround(root.Transform.position, axis, angle);
//            }
//
//            public static void Normals(IFkJoint root, IFkJoint end, float angle)
//            {
//                var vec = root.Vector + end.Vector;
//                var tan = Vector3.Project(root.Vector, vec);
//                var nor = root.Vector - tan;
//                root.RotateAround(root.Transform.position, nor, angle);
//            }
//        }
//
//        public class FkHeadRotater : IFkJointRotater
//        {
//            private FkBone _spine1;
//            private FkBone _spine2;
//            private FkBone _neck;
//
//            public FkHeadRotater(FkBone spine1, FkBone spine2, FkBone neck)
//            {
//                _spine1 = spine1;
//                _spine2 = spine2;
//                _neck = neck;
//            }
//
//            public void MoveTo(Vector3 pos)
//            {
//            }
//
//            public void Forward(float value)
//            {
//            }
//
//            public void Revolution(float angle)
//            {
//                //旋转
//            }
//
//            public void Tangent(float angle)
//            {
//                //切向
//            }
//
//            public void Normals(float angle)
//            {
//                //法向
//            }
//        }
    }
}