using System;
using System.Collections.Generic;
using IllusionUtility.GetUtility;
using RootMotion.FinalIK;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    public class FkAssist : BaseMgr<FkAssist>
    {
        private int _counter = 0;
        private Dictionary<int, Vector3> _oldRot = null;
        private Dictionary<int, GuideObject> _targets = new Dictionary<int, GuideObject>();

        public override void Init()
        {
//            try
//            {
//                Patch.Init();
//                Logger.Log(Kit.StackTrace());
//            }
//            catch (Exception ex)
//            {
//                Logger.Log("Exception: " + ex);
//            }
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
                Rotate(guideObject, z, y, x);
            }
        }

        private void Rotate(GuideObject guideObject, float z, float y, float x)
        {
            if (guideObject.enableRot)
            {
                guideObject.transformTarget.Rotate(z, y, x, Space.Self);
                guideObject.changeAmount.rot = guideObject.transformTarget.localEulerAngles;
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

        private void FinishRotate()
        {
            GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
            var list = new List<GuideCommand.EqualsInfo>();
            foreach (var kv in _targets)
            {
                var info = new GuideCommand.EqualsInfo();
                info.dicKey = kv.Key;
                info.oldValue = _oldRot[kv.Key];
                info.newValue = kv.Value.changeAmount.rot;
                list.Add(info);
            }
            var arr = list.ToArray();
            Context.UndoRedoManager().Push(new GuideCommand.RotationEqualsCommand(arr));
//            Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.RotationEqualsCommand(_targets
//                .Select<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>(
//                    (System.Func<KeyValuePair<int, GuideObject>, GuideCommand.EqualsInfo>) (v =>
//                        new GuideCommand.EqualsInfo()
//                        {
//                            dicKey = v.Key,
//                            oldValue = this._oldRot[v.Key],
//                            newValue = v.Value.changeAmount.rot
//                        })).ToArray<GuideCommand.EqualsInfo>()));
        }

        private Dictionary<int, Vector3> CollectOldRot()
        {
            Dictionary<int, Vector3> dictionary = new Dictionary<int, Vector3>();
            _targets = new Dictionary<int, GuideObject>();
            foreach (GuideObject selectObject in Singleton<GuideObjectManager>.Instance.selectObjects)
            {
                if (selectObject.enableRot)
                {
                    dictionary.Add(selectObject.dicKey, selectObject.changeAmount.rot);
                    _targets.Add(selectObject.dicKey, selectObject);
                }
            }
            return dictionary;
        }

        private void OnGUI()
        {
//            Kit.GuiButton(Vector3.zero, "text");
//            if (Input.GetKey(KeyCode.KeypadEnter))
//            {
////                foreach (var ociChar in Context.Characters())
////                {
////                    ociChar.listBones.ForEach(b => { Kit.GuiButton(b.guideObject.transformTarget.position, "K"); });
////                }
//                foreach (var ociChar in Context.Characters())
//                {
//                    ociChar.listBones.ForEach(b => { Kit.GuiButton(b.guideObject.transformTarget.position, "K"); });
//                }
//            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                var go = Context.GuideObjectManager().selectObject;
//[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Spine01/cf_J_Spine02/cf_J_Spine03/cf_J_ShoulderIK_L/cf_J_Shoulder_L/cf_J_ArmUp00_L/cf_J_ArmLow01_L/cf_J_Hand_L [-0.5565164,1.331407,-0.03151792]
//[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Spine01/cf_J_Spine02/cf_J_Spine03/cf_J_ShoulderIK_L/cf_J_Shoulder_L/cf_J_ArmUp00_L/cf_J_ArmLow01_L [-0.3511997,1.331407,-0.03151793]
//[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Spine01/cf_J_Spine02/cf_J_Spine03/cf_J_ShoulderIK_L/cf_J_Shoulder_L/cf_J_ArmUp00_L [-0.112564,1.331407,-0.03151792]
//[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Kosi01/cf_J_Kosi02/cf_J_LegUp00_L/cf_J_LegLow01_L/cf_J_LegLowRoll_L/cf_J_Foot01_L [-0.07924593,0.08603691,-0.02697031]
//[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Kosi01/cf_J_Kosi02/cf_J_LegUp00_L/cf_J_LegLow01_L [-0.07924539,0.5074378,-0.0270153]
//[FkPlugin] /CommonSpace/chaF00/BodyTop/p_cf_anim/cf_J_Root/cf_N_height/cf_J_Hips/cf_J_Kosi01/cf_J_Kosi02/cf_J_LegUp00_L [-0.07924507,0.8856537,-0.02701536]
                if (go != null)
                {
                    Logger.Log(go.transformTarget.gameObject);
                    var name = go.transformTarget.name;
                    if (name == "cf_J_Hand_L"
                        || name == "cf_J_Hand_R"
                        || name == "cm_J_Hand_L"
                        || name == "cm_J_Hand_R")
//                        || name == "cf_J_Foot01_L"
//                        || name == "cf_J_Foot01_R"
//                        || name == "cm_J_Foot01_L"
//                        || name == "cm_J_Foot01_R")
                    {
//                        go.transformTarget.Rotate(10, 10, 10, Space.Self);
//                        go.changeAmount.rot = go.transformTarget.localEulerAngles;
                        var t2 = go.transformTarget;
                        var t1 = go.transformTarget.parent;
                        var t0 = go.transformTarget.parent.parent;
                        var dic = Context.DicGuideObject();
                        if (dic == null)
                        {
                            Logger.Log("Dic NULL");
                            return;
                        }
                        Logger.Log("Dic Not NULL");

                        var g1 = dic[t1];
                        var g0 = dic[t0];
                        if (g1 == null)
                        {
                            Logger.Log("G1 NULL");
                        }
                        if (g0 == null)
                        {
                            Logger.Log("G0 NULL");
                        }
                        Logger.Log("Forward");
                        var root = new BoneRoot(new GuideObjectBone(g0, t1));
                        var end = new BoneEnd(new GuideObjectBone(g0, t2), root);
                        root.Forward(-0.01f);
                    }
                }
                else
                {
                    Logger.Log("NULL");
                }
                return;
            }
            if (Input.GetMouseButtonDown(2))
            {
                OCIChar.BoneInfo bone = null;
                foreach (var ociChar in Context.Characters())
                {
                    ociChar.listBones.ForEach(b => { });
                }
            }
            _counter++;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                float gap = 1;
                if (Input.GetKey(KeyCode.X) && Input.GetMouseButtonDown(0))
                {
                    Rotate(gap, 0, 0);
                }
                else if (Input.GetKey(KeyCode.X) && Input.GetMouseButtonDown(1))
                {
                    Rotate(-gap, 0, 0);
                }
                else if (Input.GetKey(KeyCode.C) && Input.GetMouseButtonDown(0))
                {
                    Rotate(0, gap, 0);
                }
                else if (Input.GetKey(KeyCode.C) && Input.GetMouseButtonDown(1))
                {
                    Rotate(0, -gap, 0);
                }
                else if (Input.GetKey(KeyCode.V) && Input.GetMouseButtonDown(0))
                {
                    Rotate(0, 0, gap);
                }
                else if (Input.GetKey(KeyCode.V) && Input.GetMouseButtonDown(1))
                {
                    Rotate(0, 0, -gap);
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    Reset();
                }
            }
            else
            {
                float gap = 0.5f;
                if (Input.GetKey(KeyCode.X) && Input.GetMouseButton(0))
                {
                    Rotate(gap, 0, 0);
                }
                else if (Input.GetKey(KeyCode.X) && Input.GetMouseButton(1))
                {
                    Rotate(-gap, 0, 0);
                }
                else if (Input.GetKey(KeyCode.C) && Input.GetMouseButton(0))
                {
                    Rotate(0, gap, 0);
                }
                else if (Input.GetKey(KeyCode.C) && Input.GetMouseButton(1))
                {
                    Rotate(0, -gap, 0);
                }
                else if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(0))
                {
                    Rotate(0, 0, gap);
                }
                else if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(1))
                {
                    Rotate(0, 0, -gap);
                }
                else
                {
                    if (_counter > 1)
                    {
                        Logger.Log("FinishRotate");
                        FinishRotate();
                    }
                    _counter = 0;
                    _oldRot = CollectOldRot();
                }
            }
        }
    }
}