using System;
using System.Collections.Generic;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    public class FkLimbPos : BaseMgr<FkAssist>
    {
        
        private Vector2 lastMousePos;

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
        public static Camera MainCamera()
        {
            return Context.Studio().cameraCtrl.mainCmaera;
        }

        private void Move(Vector3 delta)
        {
            Camera camera = MainCamera();
            if (camera == null)
            {
                return;
            }
            var go = Context.GuideObjectManager().selectObject;
            if (go == null || !go.IsLimb())
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
            delta = delta * 2;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                delta = delta / 4;
            }

            var rotater = FkJointAssist.FkJointRotater(go);
            rotater.MoveTo(go.transformTarget.position + delta);
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

        private void InnerUpdate()
        {
            if (Context.GuideObjectManager() == null)
            {
                return;
            }
            var go = Context.GuideObjectManager().selectObject;
            if (go == null)
            {
                return;
            }
            if (!go.IsLimb())
            {
                return;
            }

            Move();
        }

        private Vector2 GetMousePos()
        {
            Vector2 vector2 = Input.mousePosition;
            return new Vector2(vector2.x / Screen.width, vector2.y / Screen.height);
        }
    }
}