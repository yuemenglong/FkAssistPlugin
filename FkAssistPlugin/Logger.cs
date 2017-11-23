using System;
using System.IO;

namespace FkAssistPlugin
{
    public static class Logger
    {
        static FileStream fs = new FileStream("D:/hs-log.txt", FileMode.Append);

        public static void Log(params String[] ss)
        {
            var msg = String.Join(", ", ss);
            byte[] data = System.Text.Encoding.Default.GetBytes(msg + "\n");
            fs.Write(data, 0, data.Length);
            fs.Flush();
        }
    }
}