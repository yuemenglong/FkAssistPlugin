using System.Collections.Generic;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using Studio;
using UnityEngine;

namespace FkAssistPlugin.FkBone
{
    public class FkCharaMgr
    {
//        public static FkChara[] Charas = new FkChara[0];
//        public static bool IsMarkerEnabled = false;

        public static FkChara BuildChara(Transform transform)
        {
            var regex = @"^c[fm]_J_Hips$";
            var root = transform.FindParentLoopByRegex(regex);
            if (root == null)
            {
                root = transform.FindChildLoopByRegex(regex);
                if (root == null)
                {
                    return null;
                }
            }
            if (!Context.DicGuideObject().ContainsKey(root))
            {
                return null;
            }
            return new FkChara(root);
        }

        public static FkChara FindSelectChara()
        {
            var regex = @"^c[fm]_J_Hips$";
            if (Context.GuideObjectManager().selectObject == null)
            {
                return null;
            }
            var guideObject = Context.GuideObjectManager().selectObject;
            var transform = guideObject.transformTarget.FindParentLoopByRegex(regex);
            if (transform == null)
            {
                transform = guideObject.transformTarget.FindChildLoopByRegex(regex);
                if (transform == null)
                {
                    return null;
                }
            }
            if (!Context.DicGuideObject().ContainsKey(transform))
            {
                return null;
            }
            return new FkChara(transform);
        }

        public static FkChara[] FindSelectCharas()
        {
            var regex = @"^c[fm]_J_Hips$";
            var set = new HashSet<GuideObject>();
            if (Context.GuideObjectManager().selectObjects == null)
            {
                return new FkChara[] { };
            }
            foreach (var guideObject in Context.GuideObjectManager().selectObjects)
            {
                var transform = guideObject.transformTarget.FindParentLoopByRegex(regex);
                if (transform == null)
                {
                    transform = guideObject.transformTarget.FindChildLoopByRegex(regex);
                    if (transform == null)
                    {
                        continue;
                    }
                }
                if (!Context.DicGuideObject().ContainsKey(transform))
                {
                    continue;
                }
                set.Add(Context.DicGuideObject()[transform]);
            }
            var list = new List<FkChara>();
            foreach (var guideObject in set)
            {
                list.Add(new FkChara(guideObject.transformTarget));
            }
            return list.ToArray();
        }

//        public static void RefreshSelectChara()
//        {
//            Charas = FindSelectCharas();
//        }

//        public static void DisableMarker()
//        {
//            IsMarkerEnabled = false;
//            Charas.Foreach(c => c.DetachMarker());
////            BoneMarkerMgr.Instance.ToggleEnabled(false);
//        }
//
//        public static void EnableMarker()
//        {
//            IsMarkerEnabled = true;
//            Charas.Foreach(c => c.AttachMarker());
////            BoneMarkerMgr.Instance.ToggleEnabled(true);
//        }

//        public static void ClearChars()
//        {
////            BoneMarkerMgr.Instance.ToggleEnabled(false);
////            DisableMarker();
////            BoneMarkerMgr.Instance.Clear();
//            Charas.Foreach(c => c.Destroy());
//            Charas = new FkChara[0];
//        }

//        public static void MoveLocked()
//        {
//            Charas.Foreach(c => c.MoveLocked());
//        }
    }
}