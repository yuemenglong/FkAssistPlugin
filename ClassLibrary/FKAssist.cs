using System;
using System.Security.Cryptography.X509Certificates;
using Studio;
using UnityEngine;

namespace ClassLibrary
{
    public class FkAssist : BaseMgr<FkAssist>
    {
        public void Init()
        {
            Logger.Log("FkAssist");
        }

        public GuideObject GetTargetObject()
        {
            GuideObject guideObject = Singleton<GuideObjectManager>.Instance.operationTarget;
            if ((UnityEngine.Object) guideObject == (UnityEngine.Object) null)
                guideObject = Singleton<GuideObjectManager>.Instance.selectObject;
            return guideObject;
        }

        public ObjectCtrlInfo GetFirstObject()
        {
            Studio.Studio instance = Singleton<Studio.Studio>.Instance;
            if ((UnityEngine.Object) instance != (UnityEngine.Object) null)
            {
                ObjectCtrlInfo[] selectObjectCtrl = instance.treeNodeCtrl.selectObjectCtrl;
                if (selectObjectCtrl != null && selectObjectCtrl.Length != 0)
                    return selectObjectCtrl[0];
            }
            return (ObjectCtrlInfo) null;
        }

        private void Rotate(float z, float y, float x)
        {
            GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
            foreach (GuideObject guideObject in instance.selectObjects)
            {
                if (guideObject.enableRot)
                {
                    Vector3 vector3 = guideObject.transformTarget.localEulerAngles += new Vector3(z, y, x);
                    guideObject.transformTarget.localEulerAngles = vector3;
                    guideObject.changeAmount.rot = guideObject.transformTarget.localEulerAngles;
                }
            }
        }

        private void Reset()
        {
            GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
            foreach (GuideObject guideObject in instance.selectObjects)
            {
                if (guideObject.enableRot)
                {
                    guideObject.transformTarget.localEulerAngles = new Vector3();
                    guideObject.changeAmount.rot = guideObject.transformTarget.localEulerAngles;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Logger.Log("FkAssist Update");
            }
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                Rotate(1, 0, 0);
            }
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
            {
                Rotate(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
            {
                Rotate(0, 1, 0);
            }
            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
            {
                Rotate(0, -1, 0);
            }
            if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift))
            {
                Rotate(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))
            {
                Rotate(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.LeftShift))
            {
                Reset();
            }
        }
    }
}