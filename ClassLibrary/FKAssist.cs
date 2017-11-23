using UnityEngine;

namespace ClassLibrary
{
    public class FkAssist : BaseMgr<FkAssist>
    {
        public void Init()
        {
            Logger.Log("FkAssist");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Logger.Log("FkAssist Update");
            }
        }
    }
}