using System;
using Harmony;

namespace FkAssistPlugin.Patch
{
    public class CameraPatch
    {
        public static bool Prefix()
        {
//            Tracer.Log("Prefix");
            return !CameraMgr.IsLock;
        }
    }

    public class PatchMgr : Singleton<PatchMgr>
    {
        public static void Init()
        {
            try
            {
                var harmony = HarmonyInstance.Create("io.github.yuemenglong.test");
                var original = typeof(Studio.CameraControl).GetMethod("LateUpdate");
                var prefix = new HarmonyMethod(typeof(CameraPatch).GetMethod("Prefix"));
                harmony.Patch(original, prefix, null);
            }
            catch (Exception ex)
            {
                Tracer.Log("Patch Exception: " + ex);
            }
            Tracer.Log("Patch Init");
        }
//
//        static void Prefix(PauseCtrl.FileInfo __instance, OCIChar _char)
//        {
////            _char.LoadAnime(this.group, this.category, this.no, this.normalizedTime);
//            Tracer.Log("LoadAnime", __instance.group, __instance.category, __instance.no, __instance.normalizedTime);
//            Tracer.Log("activeIK", __instance.activeIK.Length, __instance.enableIK);
//            Tracer.Log("activeFK", __instance.activeFK.Length, __instance.enableFK);
//            Tracer.Log("bones Length", _char.oiCharInfo.bones.Count);
//            using (Dictionary<int, ChangeAmount>.Enumerator enumerator = __instance.dicFK.GetEnumerator())
//            {
//                while (enumerator.MoveNext())
//                {
//                    KeyValuePair<int, ChangeAmount> current = enumerator.Current;
//                    Tracer.Log("Key", current.Key);
////                    _char.oiCharInfo.bones[current.Key].changeAmount.Copy(current.Value, true, true, true);
//                }
//            }

//            for (int index = 0; index < this.activeIK.Length; ++index)
//                _char.ActiveIK((OIBoneInfo.BoneGroup) (1 << index), this.activeIK[index], false);
//            _char.ActiveKinematicMode(OICharInfo.KinematicMode.IK, this.enableIK, true);
//            using (Dictionary<int, ChangeAmount>.Enumerator enumerator = this.dicIK.GetEnumerator())
//            {
//                while (enumerator.MoveNext())
//                {
//                    KeyValuePair<int, ChangeAmount> current = enumerator.Current;
//                    _char.oiCharInfo.ikTarget[current.Key].changeAmount.Copy(current.Value, true, true, true);
//                }
//            }
//            for (int index = 0; index < this.activeFK.Length; ++index)
//                _char.ActiveFK(FKCtrl.parts[index], this.activeFK[index], false);
//            _char.ActiveKinematicMode(OICharInfo.KinematicMode.FK, this.enableFK, true);
//            using (Dictionary<int, ChangeAmount>.Enumerator enumerator = this.dicFK.GetEnumerator())
//            {
//                while (enumerator.MoveNext())
//                {
//                    KeyValuePair<int, ChangeAmount> current = enumerator.Current;
//                    _char.oiCharInfo.bones[current.Key].changeAmount.Copy(current.Value, true, true, true);
//                }
//            }
//            for (int _category = 0; _category < this.expression.Length; ++_category)
//                _char.EnableExpressionCategory(_category, this.expression[_category]);
//        }
    }
}