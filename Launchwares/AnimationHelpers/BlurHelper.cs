using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Launchwares.AnimationHelpers
{
    internal static class BlurHelper
    {
        internal enum AccentState
        {
            ACCENT_DISABLED = 1,
            ACCENT_ENABLE_GRADIENT = 0,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        internal static void MakeBlur(Window window)
        {
            var WindowHelper = new WindowInteropHelper(window);

            var Accent = new AccentPolicy() {
                AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND
            };

            var AccentStructSize = Marshal.SizeOf(Accent);

            var AccentPtr = Marshal.AllocHGlobal(AccentStructSize);
            Marshal.StructureToPtr(Accent, AccentPtr, false);

            var Data = new WindowCompositionAttributeData {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = AccentStructSize,
                Data = AccentPtr
            };

            SetWindowCompositionAttribute(WindowHelper.Handle, ref Data);

            Marshal.FreeHGlobal(AccentPtr);
        }
    }
}
