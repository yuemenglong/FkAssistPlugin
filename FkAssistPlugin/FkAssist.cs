using System;
using System.Security.Cryptography.X509Certificates;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    public class FkAssist : BaseMgr<FkAssist>
    {
        private float _gap = 0.3f;

        public override void Init()
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
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Logger.Log("FkAssist Update");
            }
            if (Input.GetKey(KeyCode.X) && Input.GetMouseButton(0) ||
                Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.LeftShift))
            {
                Rotate(_gap, 0, 0);
            }
            if (Input.GetKey(KeyCode.X) && Input.GetMouseButton(1) ||
                Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.LeftControl))
            {
                Rotate(-_gap, 0, 0);
            }
            if (Input.GetKey(KeyCode.C) && Input.GetMouseButton(0) ||
                Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.LeftShift))
            {
                Rotate(0, _gap, 0);
            }
            if (Input.GetKey(KeyCode.C) && Input.GetMouseButton(1) ||
                Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.LeftControl))
            {
                Rotate(0, -_gap, 0);
            }
            if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(0) ||
                Input.GetKeyDown(KeyCode.V) && Input.GetKey(KeyCode.LeftShift))
            {
                Rotate(0, 0, _gap);
            }
            if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(1) ||
                Input.GetKeyDown(KeyCode.V) && Input.GetKey(KeyCode.LeftControl))
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