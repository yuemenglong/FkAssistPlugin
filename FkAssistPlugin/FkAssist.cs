using System;
using System.Collections.Generic;
using IllusionUtility.GetUtility;
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
            try
            {
                Patch.Init();
                Logger.Log(Kit.StackTrace());
            }
            catch (Exception ex)
            {
                Logger.Log("Exception: " + ex);
            }
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
            Kit.GuiButton(Vector3.zero, "text");
            if (Input.GetKey(KeyCode.KeypadEnter))
            {
                foreach (var ociChar in Context.Characters())
                {
                    ociChar.listBones.ForEach(b => { Kit.GuiButton(b.guideObject.transformTarget.position, "K"); });
                }
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.KeypadEnter))
            {
//                Kit.GUIButton(new Vector3(), "AA");
//                var rect = new Rect(40,40,40,40);
//                GUI.Button(rect, "YML");
//                Logger.Log("FkAssist Update");
//                Logger.Log(Context.GuideObjectManager().selectObjects.Length + "");
//                var ociCharchar = FindOciChar();
//                var bone = ociCharchar.listBones[0];
//                foreach (var b in ociCharchar.listBones)
//                {
//                    Logger.Log(Kit.GetGameObjectPathAndPos(b.guideObject.transformTarget.gameObject));
//                    if (b.guideObject == null || !b.guideObject.enableRot)
//                    {
//                        Logger.Log("Null Or Not Rot");
//                    }
//                }
//                Logger.Log(ociCharchar.listBones.Count);

//                Context.GuideObjectManager().AddObject(bone.guideObject);
//                Logger.Log(Context.GuideObjectManager().selectObjects.Length);
//                Rotate(bone.guideObject, 10, 10, 10);
//                bone.guideObject.isActive = false;
//                ociCharchar.listBones.ForEach(b =>
//                {
//                    Logger.Log("===============================================================");
//                    Logger.Log(Kit.GetGameObjectPathAndPos(b.guideObject.gameObject));
//                    Logger.Log(Kit.GetGameObjectPathAndPos(b.guideObject.transformTarget.gameObject));
//                    Logger.Log(b.guideObject.enablePos + "", b.guideObject.enableRot + "",
//                        b.guideObject.enableScale + "");
//                });
//                Logger.Log(ociCharchar.listBones.Count + "");
                return;
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