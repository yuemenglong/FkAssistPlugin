using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FkAssistPlugin
{
    public static class Logger
    {
        static FileStream fs = new FileStream("D:/hs-log.txt", FileMode.Append);

        public static void Log(params object[] ss)
        {
            var list = new List<String>();
            foreach (var s in ss)
            {
                list.Add(s.ToString());
            }
            var msg = "[FkPlugin] " + String.Join(", ", list.ToArray());
            Debug.Log(msg);

            byte[] data = System.Text.Encoding.Default.GetBytes(msg + "\n");
            fs.Write(data, 0, data.Length);
            fs.Flush();
        }
    }
}