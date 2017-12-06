using System;
using System.Diagnostics;
using FkAssistPlugin.Util;
using RootMotion.Demos;
using UnityEngine;
using UnityEngine.Rendering;

namespace FkAssistPlugin.HSStudioNEOAddno
{
    public class BoneMarker : MonoBehaviour
    {
        public static Color DefaultColor = new Color(0.8f, 0.8f, 0.0f, 0.4f);
        public static Color HoverColor = new Color(0f, 0.8f, 0.0f, 0.4f);
        public Action<BoneMarker> OnClick;
        public Action<BoneMarker> OnDrag;
        public Action<BoneMarker> OnRightClick;
        private MeshRenderer _renderer;
        public Vector3 MouseStartPos;
        public Vector3 MouseEndPos;
        private bool _isDraged;

        public static BoneMarker Create(Transform parent)
        {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            string str = "_BoneMarker_" + parent.name;
            primitive.name = str;
            primitive.GetComponent<Collider>().isTrigger = true;
            primitive.AddComponent<Rigidbody>().isKinematic = true;
            primitive.transform.localScale = Vector3.one * 0.075f;
            primitive.transform.position = parent.position;
            primitive.transform.rotation = parent.rotation;
            primitive.transform.parent = parent;

            Material material = new Material(ShaderUtil.TransparentZAlways);
            material.color = DefaultColor;

            MeshRenderer renderer = primitive.GetComponent<MeshRenderer>();
            renderer.material = material;
            renderer.receiveShadows = false;
            renderer.shadowCastingMode = ShadowCastingMode.Off;

            BoneMarker boneMarker = primitive.AddComponent<BoneMarker>();
            boneMarker._renderer = renderer;
            return boneMarker;
        }

        private void FixedUpdate()
        {
        }

        private void Update()
        {
            if (!((UnityEngine.Object) this.transform.parent == (UnityEngine.Object) null))
                return;
            UnityEngine.Object.Destroy((UnityEngine.Object) this);
        }

        private void OnDestroy()
        {
            BoneMarkerMgr.Instance.markers.Remove(this);
        }

        private void OnMouseEnter()
        {
            this._renderer.material.color = HoverColor;
        }

        private void OnMouseExit()
        {
            this._renderer.material.color = DefaultColor;
        }

        private void OnMouseDown()
        {
            MouseStartPos = Input.mousePosition;
            _isDraged = true;
            CameraMgr.Lock();
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Tracer.Log("Get Mouse Button Down", OnRightClick);
                if (OnRightClick != null)
                {
                    Tracer.Log("Call Right Click");
                    OnRightClick(this);
                    Tracer.Log("Call Right Click End");
                }
                else
                {
                    Tracer.Log("OnRight Is NULL");
                }
            }
        }

        private void OnMouseDrag()
        {
            if (!_isDraged || OnDrag == null)
            {
                return;
            }
            MouseEndPos = Input.mousePosition;
            OnDrag(this);
            MouseStartPos = MouseEndPos;
        }

        private void OnMouseUp()
        {
            _isDraged = false;
            CameraMgr.Unlock();
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}