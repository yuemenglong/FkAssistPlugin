﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace FkAssistPlugin.HSStudioNEOAddno
{
    public class BoneMarkerMgr
    {
        private static BoneMarkerMgr _instance = new BoneMarkerMgr();
        public List<BoneMarker> markers = new List<BoneMarker>();
        public bool markerEnabled = false;

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
                boneMarker.gameObject.SetActive(this.markerEnabled);
                this.markers.Add(boneMarker);
            }
            return this.markers;
        }

        public BoneMarker Create(Transform transform)
        {
            BoneMarker boneMarker = BoneMarker.Create(transform);
            boneMarker.gameObject.SetActive(this.markerEnabled);
            this.markers.Add(boneMarker);
            return boneMarker;
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

        public Boolean IsEnabled()
        {
            return this.markerEnabled;
        }
    }
}