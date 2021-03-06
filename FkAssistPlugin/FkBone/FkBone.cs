﻿using System;
using System.Collections.Generic;
using FkAssistPlugin.HSStudioNEOAddno;
using RootMotion.Demos;
using Studio;
using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public class FkBone : IFkJoint
    {
        public GuideObject GuideObject { get; private set; }
        private List<FkBone> _children = new List<FkBone>();
        private FkBone _parent;

        public FkBone Child
        {
            get { return _children[0]; }
            set
            {
                _children.Add(value);
                value._parent = this;
            }
        }

        public FkBone[] Children
        {
            get { return _children.ToArray(); }
        }

        public FkBone Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                value._children.Add(this);
            }
        }

        public FkChara Chara { get; private set; }

        public FkBone(GuideObject go, FkChara chara)
        {
            GuideObject = go;
            Chara = chara;
        }

        public String Name
        {
            get { return GuideObject.transformTarget.name; }
        }

        public Transform Transform
        {
            get { return GuideObject.transformTarget; }
        }

        public Vector3 Vector
        {
            get { return Child.Transform.position - GuideObject.transformTarget.position; }
        }

        public void Rotate(Vector3 axis, float angle, Space relativeTo = Space.Self)
        {
            GuideObject.transformTarget.Rotate(axis, angle, relativeTo);
            GuideObject.changeAmount.rot = GuideObject.transformTarget.localEulerAngles;
        }

        public void RotateAround(Vector3 point, Vector3 axis, float angle)
        {
            GuideObject.transformTarget.RotateAround(point, axis, angle);
            GuideObject.changeAmount.rot = GuideObject.transformTarget.localEulerAngles;
        }

        public void RotateSelf(Vector3 axis, float angle)
        {
            RotateAround(Transform.position, axis, angle);
        }

//        public void ActiveMarker()
//        {
//            Marker = BoneMarker.Create(Transform);
//        }

//        public void DestroyMarker()
//        {
//            Marker.Destroy();
//            Marker = null;
//            IsLocked = false;
//        }
    }
}