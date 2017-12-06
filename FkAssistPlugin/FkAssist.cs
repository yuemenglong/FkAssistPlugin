using System;
using System.Collections.Generic;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
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
            Tracer.Log("FkAssistPlugin Init");
        }

        private void Rotate(float z, float y, float x)
        {
            GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
            foreach (GuideObject guideObject in instance.selectObjects)
            {
                guideObject.Rotate(z, y, x);
            }
        }

        private void Reset()
        {
            GuideObjectManager instance = Singleton<GuideObjectManager>.Instance;
            foreach (GuideObject guideObject in instance.selectObjects)
            {
                guideObject.Reset();
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
            try
            {
                InnerUpdate();
            }
            catch (Exception e)
            {
                Tracer.Log(e);
            }
        }

        private void InnerUpdate()
        {
            var go = Context.GuideObjectManager().selectObject;
            if (go == null)
            {
                return;
            }
            float angle = 0.5f;
            float dist = 0.002f;
            var isLimb = go.IsLimb();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                angle /= 4;
                dist /= 4;
            }
            _counter++;
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                Reset();
            }
            //
            else if (Input.GetKey(KeyCode.E) && Input.GetMouseButton(0))
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
                FkJointAssist.Forward(go, dist);
            }
            else if (Input.GetKey(KeyCode.X) && Input.GetMouseButton(1) && isLimb)
            {
                FkJointAssist.Forward(go, -dist);
            }
            //
            else if (Input.GetKey(KeyCode.C) && Input.GetMouseButton(0) && isLimb)
            {
                FkJointAssist.Tangent(go, angle);
            }
            else if (Input.GetKey(KeyCode.C) && Input.GetMouseButton(1) && isLimb)
            {
                FkJointAssist.Tangent(go, -angle);
            }
            //
            else if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(0) && isLimb)
            {
                FkJointAssist.Normals(go, angle);
            }
            else if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(1) && isLimb)
            {
                FkJointAssist.Normals(go, -angle);
            }
            //
            else if (Input.GetKey(KeyCode.B) && Input.GetMouseButton(0) && isLimb)
            {
                FkJointAssist.Revolution(go, angle);
            }
            else if (Input.GetKey(KeyCode.B) && Input.GetMouseButton(1) && isLimb)
            {
                FkJointAssist.Revolution(go, -angle);
            }
            //
            else if (Input.GetKey(KeyCode.G) && Input.GetMouseButton(0) && isLimb)
            {
                FkJointAssist.MoveEndX(go, dist);
            }
            else if (Input.GetKey(KeyCode.G) && Input.GetMouseButton(1) && isLimb)
            {
                FkJointAssist.MoveEndX(go, -dist);
            }
            //
            else if (Input.GetKey(KeyCode.Y) && Input.GetMouseButton(0) && isLimb)
            {
                FkJointAssist.MoveEndY(go, dist);
            }
            else if (Input.GetKey(KeyCode.Y) && Input.GetMouseButton(1) && isLimb)
            {
                FkJointAssist.MoveEndY(go, -dist);
            }
            //
            else if (Input.GetKey(KeyCode.H) && Input.GetMouseButton(0) && isLimb)
            {
                FkJointAssist.MoveEndZ(go, dist);
            }
            else if (Input.GetKey(KeyCode.H) && Input.GetMouseButton(1) && isLimb)
            {
                FkJointAssist.MoveEndZ(go, -dist);
            }
            //
            else
            {
                if (_counter > 1)
                {
                    Tracer.Log("FinishRotate");
                    FinishRotate();
                }
                _counter = 0;
                _oldRot = CollectOldRot();
            }
        }
    }
}