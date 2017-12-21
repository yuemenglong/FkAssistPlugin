using System.Collections.Generic;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using Studio;
using UnityEngine;
using Utility;

namespace FkAssistPlugin.FkBone
{
    public class FkCharaMgr
    {
//        public static FkChara[] Charas = new FkChara[0];
//        public static bool IsMarkerEnabled = false;
        private static string regex = @"^cha[FM]\d{2}$";

        public static FkChara BuildChara(Transform transform)
        {
//            var regex = @"^c[fm]_J_Hips$";
            var root = transform.FindParentLoopByRegex(regex);
            if (root == null)
            {
                root = transform.FindChildLoopByRegex(regex);
                if (root == null)
                {
                    return null;
                }
            }
//            if (!Context.DicGuideObject().ContainsKey(root))
//            {
//                return null;
//            }
            return new FkChara(root);
        }

        public static FkChara FindSelectChara()
        {
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
//            if (!Context.DicGuideObject().ContainsKey(transform))
//            {
//                return null;
//            }
            return new FkChara(transform);
        }

        public static FkChara[] FindSelectCharas()
        {
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
//                if (!Context.DicGuideObject().ContainsKey(transform))
//                {
//                    continue;
//                }
                set.Add(Context.SafeGetGuideObject(transform));
            }
            var list = new List<FkChara>();
            foreach (var guideObject in set)
            {
                list.Add(new FkChara(guideObject.transformTarget));
            }
            return list.ToArray();
        }
    }
}