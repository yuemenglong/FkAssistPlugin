using System.Threading;
using UnityEngine;

namespace FkAssistPlugin
{
    public static class CameraMgr
    {
        public static void Lock()
        {
            CameraControl().NoCtrlCondition = () => true;
        }

        public static void Unlock()
        {
            CameraControl().NoCtrlCondition = () => false;
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