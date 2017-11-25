using System;
using System.Collections.Generic;
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

        public static UndoRedoManager UndoRedoManager()
        {
            return Singleton<UndoRedoManager>.Instance;
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