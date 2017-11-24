using System;
using System.Collections.Generic;
using IllusionUtility.GetUtility;
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

        private void charaBones()
        {
            foreach (var objectCtrlInfo in Context.Studio().dicInfo.Values)
            {
                if (objectCtrlInfo.kind == 0)
                {
                    Logger.Log("has kind = 0");
                    OCIChar ocichar = objectCtrlInfo as OCIChar;
                    if (ocichar == null)
                    {
                        Logger.Log("ocichar is null");
                    }
                    else if (ocichar.charInfo == null)
                    {
                        Logger.Log("ocichar info is null");
                    }
                    else
                    {
                        var character = ocichar.charInfo;
                        string prefix = character is CharFemale ? "cf_" : "cm_";
                        List<GameObject> normalTargets = new List<GameObject>();
                        var list = new List<GameObject>();
                        character.chaBody.objBone.transform.FindLoopAll(list);
                        list.ForEach(item => { Logger.Log(Kit.GetGameObjectPathAndPos(item)); });
                        Logger.Log(list.Count + "");
//                            foreach(string targetName in FileManager.GetNormalTargetNames())
//                            {
//                                GameObject bone = character.chaBody.objBone.transform.FindLoop(prefix + targetName);
//                                if(bone) normalTargets.Add(bone);
//                            }
//                            return normalTargets;
                    }
                    GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
                    foreach (GuideObject guideObject in instance.selectObjects)
                    {
                        //                    if (guideObject.enableRot)
                        //                    {
                        var p0 = guideObject.transform.position;
                        var p1 = guideObject.transformTarget.position;
                        var g0 = guideObject.gameObject;
                        var g1 = guideObject.transformTarget.gameObject;
                        Logger.Log(Kit.VecStr(p0));
                        Logger.Log(Kit.GetGameObjectPathAndPos(g0));
                        Logger.Log(Kit.VecStr(p1));
                        Logger.Log(Kit.GetGameObjectPathAndPos(g1));
                        guideObject.transformTarget.gameObject.transform.Rotate(10, 0, 0);
                        //                    }
                    }
                }
            }
        }


        public OCIChar FindOciChar()
        {
            foreach (var objectCtrlInfo in Context.Studio().dicInfo.Values)
            {
                if (objectCtrlInfo.kind == 0)
                {
                    Logger.Log("has kind = 0");
                    OCIChar ocichar = objectCtrlInfo as OCIChar;
                    if (ocichar == null)
                    {
                        Logger.Log("ocichar is null");
                    }
                    else if (ocichar.charInfo == null)
                    {
                        Logger.Log("ocichar info is null");
                    }
                    else
                    {
                        return ocichar;
                    }
                }
            }
            return null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Logger.Log("FkAssist Update");
                var ocichar = FindOciChar();
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