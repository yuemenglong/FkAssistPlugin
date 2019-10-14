using FkAssistPlugin.Util;
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
            Tracer.Log("Camera Lock");
            IsLock = true;
        }

        public static void Unlock()
        {
            Tracer.Log("Camera Unlock");
            IsLock = false;
        }

        public static void Toggle()
        {
            IsLock = !IsLock;
            Tracer.Log("CameraLock", IsLock);
        }

        public static Camera MainCamera()
        {
            return Context.Studio().cameraCtrl.mainCmaera;
        }
    }
}