using System.Collections.Generic;
using FkAssistPlugin.FkBone;
using FkAssistPlugin.Util;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    public static class UndoRedoHelper
    {
        private static readonly Dictionary<int, Vector3> _lastRots = new Dictionary<int, Vector3>();
        private static readonly Dictionary<int, GuideObject> _lastTargets = new Dictionary<int, GuideObject>();

        private static void Record(GuideObject selectObject)
        {
            if (selectObject.enableRot)
            {
                _lastRots.Add(selectObject.dicKey, selectObject.changeAmount.rot);
                _lastTargets.Add(selectObject.dicKey, selectObject);
            }
        }

        public static void Record()
        {
            _lastRots.Clear();
            _lastTargets.Clear();
            var set = new HashSet<GuideObject>();
            foreach (var selectObject in FkCharaMgr.FindSelectChara().DicGuideBones.Keys)
            {
                Record(selectObject);
                set.Add(selectObject);
            }
            Context.GuideObjectManager().selectObjects.Filter(go => !set.Contains(go))
                .Foreach(go => Record(go));
        }

        public static void Finish()
        {
            var list = new List<GuideCommand.EqualsInfo>();
            foreach (var kv in _lastTargets)
            {
                var info = new GuideCommand.EqualsInfo();
                info.dicKey = kv.Key;
                info.oldValue = _lastRots[kv.Key];
                info.newValue = kv.Value.changeAmount.rot;
                list.Add(info);
            }
            var arr = list.ToArray();
            Context.UndoRedoManager().Push(new GuideCommand.RotationEqualsCommand(arr));
        }
    }
}