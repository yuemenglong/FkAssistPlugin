using System.Threading;
using FkAssistPlugin.HSStudioNEOAddno;
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
            IsLock = true;
        }

        public static void Unlock()
        {
            IsLock = false;
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