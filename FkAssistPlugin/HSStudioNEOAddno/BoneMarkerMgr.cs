using System;
using System.Collections.Generic;
using FkAssistPlugin.Util;
using UnityEngine;

namespace FkAssistPlugin.HSStudioNEOAddno
{
    public class BoneMarkerMgr
    {
        private static BoneMarkerMgr _instance = new BoneMarkerMgr();
        public List<BoneMarker> markers = new List<BoneMarker>();
        public bool MarkerEnabled = true;

        public static BoneMarkerMgr Instance
        {
            get { return BoneMarkerMgr._instance; }
        }

        public List<BoneMarker> CreateFor(Transform[] transforms)
        {
            this.Clear();
            foreach (Transform transform in transforms)
            {
                BoneMarker boneMarker = BoneMarker.Create(transform);
                boneMarker.gameObject.SetActive(this.MarkerEnabled);
                this.markers.Add(boneMarker);
            }
            return this.markers;
        }

        public BoneMarker Create(Transform transform)
        {
            BoneMarker boneMarker = BoneMarker.Create(transform);
            boneMarker.gameObject.SetActive(this.MarkerEnabled);
            this.markers.Add(boneMarker);
            return boneMarker;
        }

        public void Clear()
        {
            this.markers.ForEach(m => Destroy(m));
//            foreach (Component marker in this.markers)
//                UnityEngine.Object.Destroy((UnityEngine.Object) marker.gameObject);
            this.markers = new List<BoneMarker>();
        }

        public void ToggleEnabled(bool enabled)
        {
            this.MarkerEnabled = enabled;
            foreach (Component marker in this.markers)
                marker.gameObject.SetActive(this.MarkerEnabled);
        }

        public Boolean IsEnabled()
        {
            return this.MarkerEnabled;
        }

        public void Destroy(BoneMarker m)
        {
            Tracer.Log("Distroy Marker");
            m.SetActive(false);
            UnityEngine.Object.Destroy(m);
            markers.Remove(m);
        }
    }
}