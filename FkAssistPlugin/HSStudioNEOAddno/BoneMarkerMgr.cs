using System;
using System.Collections.Generic;
using UnityEngine;

namespace FkAssistPlugin.HSStudioNEOAddno
{
    public class BoneMarkerMgr
    {
        private static BoneMarkerMgr _instance = new BoneMarkerMgr();
        public List<BoneMarker> markers = new List<BoneMarker>();
        public bool markerEnabled = true;

        public static BoneMarkerMgr Instance
        {
            get
            {
                return BoneMarkerMgr._instance;
            }
        }

        public List<BoneMarker> CreateFor(List<Transform> transforms, Action<BoneMarker> onClick)
        {
            this.Clear();
            foreach (Transform transform in transforms)
            {
                BoneMarker boneMarker = BoneMarker.Create(transform, onClick);
                boneMarker.gameObject.SetActive(this.markerEnabled);
                this.markers.Add(boneMarker);
            }
            return this.markers;
        }

        public void Clear()
        {
            foreach (Component marker in this.markers)
                UnityEngine.Object.Destroy((UnityEngine.Object) marker.gameObject);
            this.markers = new List<BoneMarker>();
        }

        public void ToggleEnabled(bool enabled)
        {
            this.markerEnabled = enabled;
            foreach (Component marker in this.markers)
                marker.gameObject.SetActive(this.markerEnabled);
        }
    }
}