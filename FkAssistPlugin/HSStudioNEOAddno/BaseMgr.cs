// Decompiled with JetBrains decompiler
// Type: HSStudioNEOAddon.BaseMgr`1
// Assembly: HSStudioNEOAddon, Version=0.8.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C00929C9-6BA1-439E-957D-40D65F705202
// Assembly location: C:\Users\Administrator\Desktop\HSStudioNEOAddon.dll

using UnityEngine;

namespace FkAssistPlugin.HSStudioNEOAddno
{
    public class BaseMgr<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }

        public static T Install(GameObject container)
        {
            if ((Object) BaseMgr<T>.Instance == (Object) null)
            {
                BaseMgr<T>.Instance = container.AddComponent<T>();
                BaseMgr<T>.Instance.Invoke("Init", 0.0f);
            }
            return BaseMgr<T>.Instance;
        }

        public virtual void Init()
        {
        }
    }
}
