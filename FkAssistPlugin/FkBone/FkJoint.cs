using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public interface IFkJoint
    {
        Transform Transform { get; }
        Vector3 Vector { get; }

        // 围绕自身坐标系旋转，例如Vector3.up就是自己的y轴
        void Rotate(Vector3 axis, float angle, Space relativeTo = Space.Self);
        // 围绕某个点转，轴可以是标准向量
        void RotateAround(Vector3 point, Vector3 axis, float angle);

        Quaternion GetQuaternion();

        void setQuaternion(Quaternion q);
    }
}