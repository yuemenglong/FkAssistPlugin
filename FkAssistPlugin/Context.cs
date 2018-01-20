using System;
using System.Collections.Generic;
using System.Reflection;
using FkAssistPlugin.Util;
using Studio;
using UnityEngine;

namespace FkAssistPlugin
{
    public class Context : MonoBehaviour
    {
        public static Studio.Studio Studio()
        {
            return Singleton<Studio.Studio>.Instance;
        }

        public static GuideObjectManager GuideObjectManager()
        {
            return Singleton<GuideObjectManager>.Instance;
        }

        public static Dictionary<Transform, GuideObject> DicGuideObject()
        {
            return GuideObjectManager().GetPrivateField<Dictionary<Transform, GuideObject>>("dicGuideObject");
        }
        
        public static HashSet<GuideObject> HashSelectObject()
        {
            return GuideObjectManager().GetPrivateField<HashSet<GuideObject>>("hashSelectObject");
        }

        public static UndoRedoManager UndoRedoManager()
        {
            return Singleton<UndoRedoManager>.Instance;
        }

        public static OCIChar[] Characters()
        {
            var list = new List<OCIChar>();
            foreach (var objectCtrlInfo in Studio().dicInfo.Values)
            {
                if (objectCtrlInfo.kind == 0)
                {
                    OCIChar ocichar = objectCtrlInfo as OCIChar;
                    if (ocichar != null)
                    {
                        list.Add(ocichar);
                    }
                }
            }
            return list.ToArray();
        }

        private static List<TreeNodeObject> GetCharaNodes<CharaType>()
        {
            var studio = Studio();
            var treeNodeCtrl = studio.treeNodeCtrl;
            List<TreeNodeObject> charaNodes = new List<TreeNodeObject>();

            int n = 0;
            TreeNodeObject nthNode;
            while ((nthNode = treeNodeCtrl.GetNode(n)) != null)
            {
                ObjectCtrlInfo objectCtrlInfo = null;
                if (nthNode.visible && studio.dicInfo.TryGetValue(nthNode, out objectCtrlInfo))
                {
                    if (objectCtrlInfo is CharaType)
                    {
                        charaNodes.Add(nthNode);
                    }
                }
                n++;
            }

            return charaNodes;
        }
    }
}