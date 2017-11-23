using System;
using System.Security.Cryptography.X509Certificates;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    public class FkAssist : BaseMgr<FkAssist>
    {
        private int _gap = 2;

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
//                    Vector3 vector3 = guideObject.transformTarget.localEulerAngles += new Vector3(z, y, x);
//                    guideObject.transformTarget.localEulerAngles = vector3;
                    guideObject.transformTarget.Rotate(z, y, x, Space.Self);
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
            if (Input.GetKeyDown(KeyCode.T))
            {
                Logger.Log("FkAssist Update");
            }
            if (Input.GetKey(KeyCode.P) ||
                Input.GetKeyDown(KeyCode.P) && Input.GetKey(KeyCode.RightAlt))
            {
                Rotate(_gap, 0, 0);
            }
            if (Input.GetKey(KeyCode.Colon)||
                Input.GetKeyDown(KeyCode.Colon) && Input.GetKey(KeyCode.RightAlt))
            {
                Rotate(-_gap, 0, 0);
            }
            if (Input.GetKey(KeyCode.L) ||
                Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.RightAlt))
            {
                Rotate(0, _gap, 0);
            }
            if (Input.GetKey(KeyCode.Comma)||
                Input.GetKeyDown(KeyCode.Comma) && Input.GetKey(KeyCode.RightAlt))
            {
                Rotate(0, -_gap, 0);
            }
            if (Input.GetKey(KeyCode.O) ||
                Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.RightAlt))
            {
                Rotate(0, 0, _gap);
            }
            if (Input.GetKey(KeyCode.LeftBracket)||
                Input.GetKeyDown(KeyCode.LeftBracket) && Input.GetKey(KeyCode.RightAlt))
            {
                Rotate(0, 0, -_gap);
            }
            if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift))
            {
                Reset();
            }
        }
    }
}