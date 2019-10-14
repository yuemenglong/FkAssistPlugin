using System;
using System.Diagnostics;
using FkAssistPlugin.Util;
using UnityEngine;
using UnityEngine.Rendering;

namespace FkAssistPlugin.HSStudioNEOAddno
{
    public class BoneMarker : MonoBehaviour
    {
        public static Color DefaultColor = new Color(0.8f, 0.8f, 0.0f, 0.4f);
        public static Color HoverColor = new Color(0f, 0.8f, 0.0f, 0.4f);
        private Color _defaultColor = DefaultColor;
        public Action<BoneMarker> OnLeftClick;
        public Action<BoneMarker> OnRightClick;
        public Action<BoneMarker> OnMidClick;
        public Action<BoneMarker> OnLeftDown;
        public Action<BoneMarker> OnLeftUp;
        public Action<BoneMarker> OnDrag;
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

        private BoneMarker()
        {
        }

        public void SetColor(Color c)
        {
            _defaultColor = c;
            this._renderer.material.color = _defaultColor;
        }

        public void SetDefaultColor()
        {
            _defaultColor = DefaultColor;
            this._renderer.material.color = _defaultColor;
        }

        private void Update()
        {
            if (this.transform.parent != null)
                return;
            Destroy();
        }

        private void OnMouseEnter()
        {
            this._renderer.material.color = HoverColor;
        }

        private void OnMouseExit()
        {
            this._renderer.material.color = _defaultColor;
        }

        private void OnMouseDown()
        {
            MouseStartPos = Input.mousePosition;
            _isDraged = true;
            CameraMgr.Lock();
            if (OnLeftDown != null)
            {
                OnLeftDown(this);
            }
            if (OnLeftClick != null)
            {
                OnLeftClick(this);
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1) && OnRightClick != null)
            {
                OnRightClick(this);
            }
            if (Input.GetMouseButtonDown(2) && OnMidClick != null)
            {
                OnMidClick(this);
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
            if (OnLeftUp != null)
            {
                OnLeftUp(this);
            }
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void Destroy()
        {
            SetActive(false);
            UnityEngine.Object.Destroy(this);
        }
    }
}