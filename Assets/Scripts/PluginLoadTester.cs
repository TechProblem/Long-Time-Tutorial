using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class PluginLoadTester : MonoBehaviour
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool FreeLibrary(IntPtr hModule);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
    static extern uint GetModuleFileNameA(IntPtr hModule, StringBuilder lpFilename, uint nSize);

    void Start()
    {
        Debug.Log("PluginLoadTester: Application.dataPath = " + Application.dataPath);
        TestLoad("LTT.dll");
        string explicitPath = System.IO.Path.Combine(Application.dataPath, "Plugins", "x86_64", "LTT.dll");
        TestLoad(explicitPath);
    }

    void TestLoad(string pathOrName)
    {
        Debug.Log($"PluginLoadTester: Attempting LoadLibrary(\"{pathOrName}\")");
        IntPtr h = LoadLibrary(pathOrName);
        if (h == IntPtr.Zero)
        {
            int err = Marshal.GetLastWin32Error();
            Debug.LogError($"PluginLoadTester: LoadLibrary failed for '{pathOrName}' — Win32 error {err}");
            return;
        }

        try
        {
            Debug.Log($"PluginLoadTester: Loaded '{pathOrName}' => 0x{h.ToString("X")}");
            // Get actual loaded module filename
            var sb = new StringBuilder(1024);
            uint r = GetModuleFileNameA(h, sb, (uint)sb.Capacity);
            Debug.Log("PluginLoadTester: Actual module path = " + (r > 0 ? sb.ToString() : "<unknown>"));

            foreach (var name in new[] { "LTT_main", "FileHandler", "LTTWrite" })
            {
                IntPtr p = GetProcAddress(h, name);
                Debug.Log($"PluginLoadTester: GetProcAddress('{name}') => {(p == IntPtr.Zero ? "missing" : $"0x{p.ToString("X")}")}");
            }
        }
        finally
        {
            FreeLibrary(h);
        }
    }
}