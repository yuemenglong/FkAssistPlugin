using System.Threading;
using FkAssistPlugin.HSStudioNEOAddno;
using UnityEngine;

namespace FkAssistPlugin
{
    public class CameraMgr
    {
        public static Vector3 LastPos { get; private set; }
        public static Quaternion LastRot { get; private set; }
        public static bool IsLock { get; private set; }

        public CameraMgr()
        {
            IsLock = false;
        }

        public static void Lock()
        {
//            CameraControl().NoCtrlCondition = () => true;
            LastPos = MainCamera().transform.position;
            LastRot = MainCamera().transform.rotation;
            IsLock = true;
            Tracer.Log(LastPos);
            Tracer.Log(LastRot);
        }

        public static void Unlock()
        {
//            CameraControl().NoCtrlCondition = () => false;
            IsLock = false;
        }

        public static void Toggle()
        {
            IsLock = !IsLock;
        }

        public static CameraControl CameraControl()
        {
            return MainCamera().GetComponent<CameraControl>();
        }

        public static Camera MainCamera()
        {
            return Context.Studio().cameraCtrl.mainCmaera;
        }
    }
}