using System.Collections.Generic;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using Studio;

namespace FkAssistPlugin.FkBone
{
    public class FkCharaMgr
    {
        public static FkChara[] Charas = new FkChara[0];
        public static bool IsEnabled = false;

        public static FkChara[] FindSelectChara()
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

        public static void RefreshSelectChara()
        {
            Charas = FindSelectChara();
        }

        public static void DisableMarker()
        {
            BoneMarkerMgr.Instance.ToggleEnabled(false);
        }

        public static void EnableMarker()
        {
            BoneMarkerMgr.Instance.ToggleEnabled(true);
        }

        public static void ClearChars()
        {
//            BoneMarkerMgr.Instance.ToggleEnabled(false);
//            DisableMarker();
//            BoneMarkerMgr.Instance.Clear();
            Charas.Foreach(c => c.Destroy());
            Charas = new FkChara[0];
        }

        public static void MoveLocked()
        {
            Charas.Foreach(c => c.MoveLocked());
        }
    }
}