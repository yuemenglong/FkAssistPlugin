using System;
using RootMotion.Demos;
using UnityEngine;
using UnityEngine.Rendering;

namespace FkAssistPlugin.HSStudioNEOAddno
{
    public class BoneMarker : MonoBehaviour
    {
        public Color defaultColor = new Color(0.8f, 0.8f, 0.0f, 0.4f);
        public Color hoverColor = new Color(1f, 0.0f, 0.0f, 0.4f);
        public Action<BoneMarker> OnClick;
        public Action<BoneMarker> OnDrag;
        private MeshRenderer renderer;
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
            MeshRenderer component = primitive.GetComponent<MeshRenderer>();
            Material material = new Material(ShaderUtil.TransparentZAlways);
            BoneMarker boneMarker = primitive.AddComponent<BoneMarker>();
            boneMarker.renderer = component;
            material.color = boneMarker.defaultColor;
            component.material = material;
            component.receiveShadows = false;
            component.shadowCastingMode = ShadowCastingMode.Off;
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
            this.renderer.material.color = this.hoverColor;
        }

        private void OnMouseExit()
        {
            this.renderer.material.color = this.defaultColor;
        }

        private void OnMouseDown()
        {
//            if (this.OnClick == null)
//                return;
//            this.OnClick(this);
//            Logger.Log("MouseDown", Input.mousePosition);
            MouseStartPos = Input.mousePosition;
            _isDraged = true;
            CameraMgr.Lock();
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
//            if (this.OnClick == null)
//                return;
//            this.OnClick(this);
//            Logger.Log("MouseUp", Input.mousePosition);
            _isDraged = false;
            CameraMgr.Unlock();
        }
    }
}