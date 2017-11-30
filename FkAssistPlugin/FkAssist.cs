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
            Logger.Log("FkAssistPlugin Init");
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Dynamic.DynamicProc();
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