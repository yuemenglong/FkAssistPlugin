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
        private int _counter;
        private Dictionary<int, Vector3> _oldRot;
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
                guideObject.Rotate(z, y, x);
            }
        }

//        private void Rotate(GuideObject guideObject, float z, float y, float x)
//        {
//            if (guideObject.enableRot)
//            {
//                guideObject.transformTarget.Rotate(z, y, x, Space.Self);
//                guideObject.changeAmount.rot = guideObject.transformTarget.localEulerAngles;
//            }
//        }

        private void Reset()
        {
            GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
            foreach (GuideObject guideObject in instance.selectObjects)
            {
                guideObject.Rotate(0, 0, 0);
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
            var go = Context.GuideObjectManager().selectObject;
            if (go == null)
            {
                return;
            }
            _counter++;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Reset();
                }
            }
            else
            {
                float angle = 0.5f;
                float dist = 0.002f;
                var isLimb = go.IsLimb();
                if (Input.GetKey(KeyCode.E) && Input.GetMouseButton(0))
                {
                    Rotate(angle, 0, 0);
                }
                else if (Input.GetKey(KeyCode.E) && Input.GetMouseButton(1))
                {
                    Rotate(-angle, 0, 0);
                }
                //
                else if (Input.GetKey(KeyCode.S) && Input.GetMouseButton(0))
                {
                    Rotate(0, angle, 0);
                }
                else if (Input.GetKey(KeyCode.S) && Input.GetMouseButton(1))
                {
                    Rotate(0, -angle, 0);
                }
                //
                else if (Input.GetKey(KeyCode.D) && Input.GetMouseButton(0))
                {
                    Rotate(0, 0, angle);
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetMouseButton(1))
                {
                    Rotate(0, 0, -angle);
                }
                //
                else if (Input.GetKey(KeyCode.X) && Input.GetMouseButton(0) && isLimb)
                {
                    BoneAssist.Forward(go, dist);
                }
                else if (Input.GetKey(KeyCode.X) && Input.GetMouseButton(1) && isLimb)
                {
                    BoneAssist.Forward(go, -dist);
                }
                //
                else if (Input.GetKey(KeyCode.C) && Input.GetMouseButton(0) && isLimb)
                {
                    BoneAssist.Tangent(go, angle);
                }
                else if (Input.GetKey(KeyCode.C) && Input.GetMouseButton(1) && isLimb)
                {
                    BoneAssist.Tangent(go, -angle);
                }
                //
                else if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(0) && isLimb)
                {
                    BoneAssist.Normals(go, angle);
                }
                else if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(1) && isLimb)
                {
                    BoneAssist.Normals(go, -angle);
                }
                //
                else if (Input.GetKey(KeyCode.B) && Input.GetMouseButton(0) && isLimb)
                {
                    BoneAssist.Revolution(go, angle);
                }
                else if (Input.GetKey(KeyCode.B) && Input.GetMouseButton(1) && isLimb)
                {
                    BoneAssist.Revolution(go, -angle);
                }
                //
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