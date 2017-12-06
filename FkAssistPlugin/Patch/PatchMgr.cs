using System;
using FkAssistPlugin.Util;
using Harmony;
using UnityEngine;

namespace FkAssistPlugin.Patch
{
    public interface IPatch
    {
        Type PatchType();
        String PatchMethod();
        Type[] PatchParams();
    }

    public class PatchMgr
    {
        private static HarmonyInstance _harmony = HarmonyInstance.Create("io.github.yuemenglong.test");

        public static void Patch(IPatch patch)
        {
            try
            {
                var original = patch.PatchType().GetMethod(patch.PatchMethod(), patch.PatchParams());
                var prefix = new HarmonyMethod(patch.GetType().GetMethod("Prefix"));
                var postfix = new HarmonyMethod(patch.GetType().GetMethod("Postfix"));
                _harmony.Patch(original, prefix, postfix);
                Tracer.Log("Patch ", patch.PatchType(), patch.PatchMethod());
            }
            catch (Exception e)
            {
                Tracer.Log(e);
            }
        }
    }
}