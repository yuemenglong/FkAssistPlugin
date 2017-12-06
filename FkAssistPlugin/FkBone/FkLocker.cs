using System;
using FkAssistPlugin.HSStudioNEOAddno;
using FkAssistPlugin.Util;

namespace FkAssistPlugin.FkBone
{
    public class FkLocker : BaseMgr<FkAssist>
    {
        public override void Init()
        {
            Tracer.Log("FkLocker Init");
        }

        private void Update()
        {
            try
            {
                InnerUpdate();
            }
            catch (Exception e)
            {
                Tracer.Log(e);
            }
        }

        private void InnerUpdate()
        {
            
        }
    }
}