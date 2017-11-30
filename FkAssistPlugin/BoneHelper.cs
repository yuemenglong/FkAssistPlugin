using System;
using Studio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FkAssistPlugin
{
    public static class BoneHelper
    {
        public static bool IsHand(this Transform transform)
        {
            var name = transform.name;
            return name == "cf_J_Hand_L"
                   || name == "cf_J_Hand_R"
                   || name == "cm_J_Hand_L"
                   || name == "cm_J_Hand_R";
        }

        public static GuideObject GuideObject(this Transform transform)
        {
            return Context.DicGuideObject()[transform];
        }
    }
}