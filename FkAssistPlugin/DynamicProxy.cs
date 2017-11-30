using System;
using System.IO;

namespace FkAssistPlugin
{
    public class DynamicProxy
    {
        private static String _path = @"D:\workspace\unity\FkAssistPlugin\DynamicHandler\bin\Debug\DynamicHandler.dll";

        public static void Proc()
        {
            var assembly = Kit.LoadAssembly(_path);
            var type = assembly.GetType("DynamicHandler.Handler");
            var method = type.GetMethod("Proc");
            var obj = Activator.CreateInstance(type);
            method.Invoke(obj, new object[] { });
        }
    }
}