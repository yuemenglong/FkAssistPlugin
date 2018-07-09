using System;
using System.Collections.Generic;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    public class FkAssist : BaseMgr<FkAssist>
    {
        private int _counter;
        private bool _lightEnabled = true;
        private Vector2 lastMousePos;

        public override void Init()
        {
            Tracer.Log("FkAssistPlugin Init");
        }

        private void Update()
        {
            try
            {
                Rotate();
                Move();
            }
            catch (Exception e)
            {
                Tracer.Log(e);
            }
        }

        private void Move(Vector3 delta)
        {
            var go = Context.GuideObjectManager().selectObject;
            if (go == null || !go.enablePos)
            {
                return;
            }

            Camera camera = CameraAssist.MainCamera();
            if (camera == null)
            {
                return;
            }

            Vector3 vector31 = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, Input.mousePosition.z);
            Ray ray = camera.ScreenPointToRay(vector31);
            ray.direction = new Vector3(ray.direction.x, 0, ray.direction.z);
            Vector3 vector32 = ray.direction * -1 * delta.z;
            ray.direction = Quaternion.LookRotation(ray.direction) * Vector3.right;
            Vector3 vector33 = vector32 + ray.direction * -1 * delta.x;
            vector33.y = delta.y;
            delta = vector33;
            delta = delta * 2.0f;
            go.Move(delta);
        }

        private void Move()
        {
            Vector2 sub = GetMousePos() - lastMousePos;
            if (Input.GetKey(KeyCode.G))
            {
                Vector3 delta = new Vector3(-sub.x, 0, -sub.y);
                Move(delta);
            }
            else if (Input.GetKey(KeyCode.H))
            {
                Vector3 delta = new Vector3(0, sub.y, 0);
                Move(delta);
            }
            lastMousePos = GetMousePos();
        }

        private void Rotate()
        {
            var go = Context.GuideObjectManager().selectObject;
            if (go == null)
            {
                return;
            }

            float angle = 1.0f;
            float dist = 0.003f;
//            if (Input.anyKeyDown)
//            {
//                UndoRedoHelper.Record();
//            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                angle /= 4;
                dist /= 4;
            }

//            _counter++;
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                go.Reset();
                go.SetSelected();
            }
            //
            else if (Input.GetKey(KeyCode.E) && Input.GetMouseButton(0))
            {
                go.Rotate(angle, 0, 0);
            }
            else if (Input.GetKey(KeyCode.E) && Input.GetMouseButton(1))
            {
//                go.Rotate(-angle, 0, 0);
                go.Rotate(-angle, 0, 0);
            }
            //
            else if (Input.GetKey(KeyCode.S) && Input.GetMouseButton(0))
            {
                go.Rotate(0, angle, 0);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetMouseButton(1))
            {
                go.Rotate(0, -angle, 0);
            }
            //
            else if (Input.GetKey(KeyCode.D) && Input.GetMouseButton(0))
            {
                go.Rotate(0, 0, angle);
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetMouseButton(1))
            {
                go.Rotate(0, 0, -angle);
            }
            //
            else if (Input.GetKey(KeyCode.X) && Input.GetMouseButton(0))
            {
                FkJointAssist.Forward(go, dist);
            }
            else if (Input.GetKey(KeyCode.X) && Input.GetMouseButton(1))
            {
                FkJointAssist.Forward(go, -dist);
            }
            //
            else if (Input.GetKey(KeyCode.C) && Input.GetMouseButton(0))
            {
                FkJointAssist.Tangent(go, angle);
            }
            else if (Input.GetKey(KeyCode.C) && Input.GetMouseButton(1))
            {
                FkJointAssist.Tangent(go, -angle);
            }
            //
            else if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(0))
            {
                FkJointAssist.Normals(go, angle);
            }
            else if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(1))
            {
                FkJointAssist.Normals(go, -angle);
            }
            //
            else if (Input.GetKey(KeyCode.B) && Input.GetMouseButton(0))
            {
                FkJointAssist.Revolution(go, angle);
            }
            else if (Input.GetKey(KeyCode.B) && Input.GetMouseButton(1))
            {
                FkJointAssist.Revolution(go, -angle);
            }
            //
            else if (Input.GetKey(KeyCode.G) && Input.GetMouseButton(0))
            {
                FkJointAssist.MoveEndX(go, dist);
            }
            else if (Input.GetKey(KeyCode.G) && Input.GetMouseButton(1))
            {
                FkJointAssist.MoveEndX(go, -dist);
            }
            //
            else if (Input.GetKey(KeyCode.Y) && Input.GetMouseButton(0))
            {
                FkJointAssist.MoveEndY(go, dist);
            }
            else if (Input.GetKey(KeyCode.Y) && Input.GetMouseButton(1))
            {
                FkJointAssist.MoveEndY(go, -dist);
            }
            //
            else if (Input.GetKey(KeyCode.H) && Input.GetMouseButton(0))
            {
                FkJointAssist.MoveEndZ(go, dist);
            }
            else if (Input.GetKey(KeyCode.H) && Input.GetMouseButton(1))
            {
                FkJointAssist.MoveEndZ(go, -dist);
            }
            //
//            else
//            {
//                if (_counter > 1)
//                {
//                    UndoRedoHelper.Finish();
////                    FinishRotate();
//                }
//
//                _counter = 0;
////                _oldRot = CollectOldRot();
//            }
        }

        private Vector2 GetMousePos()
        {
            Vector2 vector2 = Input.mousePosition;
            return new Vector2(vector2.x / Screen.width, vector2.y / Screen.height);
        }
    }
}