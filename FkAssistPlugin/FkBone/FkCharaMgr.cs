using System.Collections.Generic;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using Studio;

namespace FkAssistPlugin.FkBone
{
    public class FkCharaMgr
    {
        public static FkChara[] Charas = new FkChara[0];

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

        public static bool IsMarkerEnabled()
        {
            return BoneMarkerMgr.Instance.IsEnabled();
        }

        public static void DisableMarker()
        {
            BoneMarkerMgr.Instance.ToggleEnabled(false);
        }

        public static void EnableMarker()
        {
            BoneMarkerMgr.Instance.ToggleEnabled(true);
        }

        public static void ClearMarker()
        {
            BoneMarkerMgr.Instance.ToggleEnabled(false);
            BoneMarkerMgr.Instance.Clear();
        }

        public static void ReAttachMarker()
        {
            BoneMarkerMgr.Instance.Clear();
            var bones = Charas.FlatMap(c => { return c.Limbs(); });
            bones.Foreach(b =>
            {
                b.Marker = BoneMarkerMgr.Instance.Create(b.Transform);
                b.Marker.OnDrag = m =>
                {
                    var screenVec = m.MouseEndPos - m.MouseStartPos;
                    var pos = Kit.MapScreenVecToWorld(screenVec, b.Transform.position);
                    FkJointAssist.MoveEnd(b.GuideObject, pos);
                };
            });
            EnableMarker();
        }

        public static void MoveLocked()
        {
            Charas.Foreach(c => c.MoveLocked());
        }
    }
}