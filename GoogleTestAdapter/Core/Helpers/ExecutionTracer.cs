using System;
using System.Collections.Generic;
using System.IO;

namespace GoogleTestAdapter.Helpers
{
    public static class ExecutionTracer
    {
        public static readonly string TraceFile = Path.Combine(Path.GetTempPath(), "GTA_ExecutionTrace.txt");

        private static readonly List<string> Messages = new List<string>(); 

        public static void Trace(string msg)
        {
            lock (TraceFile)
            {
                Messages.Add(DateTime.Now.ToString("HH:mm:ss.ffffff") + ": " + msg);
            }
        }

        public static void Flush()
        {
            lock (TraceFile)
            {
                File.AppendAllLines(TraceFile, Messages);
                Messages.Clear();
            }
        }

    }

}