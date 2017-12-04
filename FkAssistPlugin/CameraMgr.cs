using System.Threading;
using FkAssistPlugin.HSStudioNEOAddno;
using UnityEngine;

namespace FkAssistPlugin
{
    public class CameraMgr
    {
        public static bool IsLock { get; private set; }

        public CameraMgr()
        {
            IsLock = false;
        }

        public static void Lock()
        {
            IsLock = true;
            Tracer.Log("CameraLock", IsLock);
        }

        public static void Unlock()
        {
            IsLock = false;
            Tracer.Log("CameraLock", IsLock);
        }

        public static void Toggle()
        {
            IsLock = !IsLock;
            Tracer.Log("CameraLock", IsLock);
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