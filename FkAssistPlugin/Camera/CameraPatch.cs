using System;

namespace FkAssistPlugin.Patch
{
    public class CameraPatch : IPatch
    {
        public static bool Prefix()
        {
//            Tracer.Log("Prefix");
            return !CameraMgr.IsLock;
        }

        public Type PatchType()
        {
            return typeof(Studio.CameraControl);
        }

        public string PatchMethod()
        {
            return "LateUpdate";
        }

        public Type[] PatchParams()
        {
            return new Type[] { };
        }
    }
}