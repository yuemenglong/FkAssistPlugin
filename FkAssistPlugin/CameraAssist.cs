using System.Collections;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    class CameraAssist : MonoBehaviour
    {
        CameraAssist()
        {
            StudioCameraControl = Singleton<Studio.Studio>.Instance.cameraCtrl;
        }

        void OnEnable()
        {
            StartCoroutine(CameraLock());
        }
        void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator CameraLock()
        {
            while (true)
            {
                yield return null;
                if (windowdragflag)
                {
                    StudioCameraControl.noCtrlCondition = new CameraControl.NoCtrlFunc(CameraCtrlOff);
                }
                else if (previouswindowdragflag)
                {
                    StudioCameraControl.noCtrlCondition = new CameraControl.NoCtrlFunc(CameraCtrlOff);
                }
                previouswindowdragflag = windowdragflag;
            }
        }

        bool CameraCtrlOff()
        {
            return windowdragflag;
        }

        public static Camera MainCamera()
        {
            return Context.Studio().cameraCtrl.mainCmaera;
        }

        public static bool windowdragflag = false;
        bool previouswindowdragflag = false;
        internal CameraControl StudioCameraControl;
    }
}
