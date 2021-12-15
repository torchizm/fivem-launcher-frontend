using System;
using System.Runtime.InteropServices;
using System.Timers;

namespace Launchwares.Core
{
    static class RamOptimization
    {
        static Timer RamTimer = new Timer() { Interval = 5000 };

        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();

        static RamOptimization()
        {
            RamTimer.Elapsed += OptimizingTimer_Elapsed;
            RamTimer.Start();

            //IntPtr pHandle = GetCurrentProcess();
            //SetProcessWorkingSetSize(pHandle, -1, -1);
        }

        private static void OptimizingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();
        }
    }
}
