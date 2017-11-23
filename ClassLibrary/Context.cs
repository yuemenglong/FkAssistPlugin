using System;
using System.Collections.Generic;
using Studio;
using UnityEngine;

namespace ClassLibrary
{
    public class Context : MonoBehaviour
    {
        private Studio.Studio studio()
        {
            return Singleton<Studio.Studio>.Instance;
        }

        private List<TreeNodeObject> GetCharaNodes<CharaType>()
        {
            var studio = this.studio();
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