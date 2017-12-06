using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;
using IllusionUtility.GetUtility;
using Studio;
using UnityEngine;
using UnityEngine.Assertions;

namespace FkAssistPlugin.Bone
{
    public class CharaBoneMgr
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

        public static void EnableMarker()
        {
            var list = new List<Transform>();
            foreach (var fkChara in Charas)
            {
                foreach (var bone in fkChara.Limbs())
                {
                    list.Add(bone.Transform);
                }
            }
            var markers = BoneMarkerMgr.Instance.CreateFor(list.ToArray());
            markers.ForEach(m =>
            {
                m.OnDrag = marker =>
                {
//                                        var screenVec = m.MouseEndPos - m.MouseStartPos;
//                        var pos = Kit.MapScreenVecToWorld(screenVec, go.transformTarget.position);
//                        FkRotaterAssist.MoveEnd(go, pos);
                };
            });
        }
    }
}