using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

public static class UtilsWindowsMonitor
{
    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct MONITORINFOEX
    {
        public int cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szDevice;
    }

    private const uint MONITORINFOF_PRIMARY = 0x1;

    private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

    [DllImport("user32.dll")]
    private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

    /// <summary>
    /// Data for a single monitor, in raw Win32 virtual-desktop coordinates (top-left origin, Y-down).
    /// </summary>
    public struct MonitorData
    {
        public int LeftToRightIndex; // rank when sorted left to right (0 = leftmost)
        public string DeviceName;    // e.g. "\\.\DISPLAY1"
        public bool IsPrimary;

        // Full monitor bounds
        public int X;
        public int Y;
        public int Width;
        public int Height;

        // Usable work area (excludes taskbar)
        public int WorkX;
        public int WorkY;
        public int WorkWidth;
        public int WorkHeight;

        public override string ToString()
        {
            return $"[{LeftToRightIndex}] {DeviceName} (primary={IsPrimary}) " +
                   $"monitor=({X},{Y},{Width}x{Height}) work=({WorkX},{WorkY},{WorkWidth}x{WorkHeight})";
        }
    }

    /// <summary>
    /// Returns all connected monitors, sorted left to right by their X position.
    /// </summary>
    public static List<MonitorData> GetMonitorsLeftToRight()
    {
        List<MonitorData> monitors = new List<MonitorData>();

        MonitorEnumProc callback = (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData) =>
        {
            MONITORINFOEX info = new MONITORINFOEX();
            info.cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));

            if (GetMonitorInfo(hMonitor, ref info))
            {
                monitors.Add(new MonitorData
                {
                    DeviceName = info.szDevice,
                    IsPrimary = (info.dwFlags & MONITORINFOF_PRIMARY) != 0,
                    X = info.rcMonitor.Left,
                    Y = info.rcMonitor.Top,
                    Width = info.rcMonitor.Right - info.rcMonitor.Left,
                    Height = info.rcMonitor.Bottom - info.rcMonitor.Top,
                    WorkX = info.rcWork.Left,
                    WorkY = info.rcWork.Top,
                    WorkWidth = info.rcWork.Right - info.rcWork.Left,
                    WorkHeight = info.rcWork.Bottom - info.rcWork.Top,
                });
            }

            return true; // continue enumeration
        };

        EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, IntPtr.Zero);

        // Sort left to right by X position
        monitors.Sort((a, b) => a.X.CompareTo(b.X));

        // Assign left-to-right rank after sorting
        for (int i = 0; i < monitors.Count; i++)
        {
            MonitorData m = monitors[i];
            m.LeftToRightIndex = i;
            monitors[i] = m;
        }

        return monitors;
    }
}

#endif