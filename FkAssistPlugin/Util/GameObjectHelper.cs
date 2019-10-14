using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FkAssistPlugin.Util
{
    public static class GameObjectHelper
    {
        public static Transform FindParentLoopByRegex(this Transform transform, String pattern)
        {
            if (transform == null)
            {
                return null;
            }
            if (Regex.IsMatch(transform.name, pattern))
            {
                return transform;
            }
            return FindParentLoopByRegex(transform.parent, pattern);
        }

        public static Transform FindChildLoopByRegex(this Transform transform, String pattern)
        {
            if (transform == null)
            {
                return null;
            }

            if (Regex.IsMatch(transform.name, pattern))
            {
                return transform;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = FindChildLoopByRegex(transform.GetChild(i), pattern);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }
    }
}