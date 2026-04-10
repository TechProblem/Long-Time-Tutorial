using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Attach to a persistent manager GameObject
public class Logging : MonoBehaviour
{
    [SerializeField] private List<LogExample> scripts = new List<LogExample>();

    [DllImport("__Internal")]
    private static extern void LTTNative_AddCustomScript(string scriptId);
    [DllImport("__Internal")]
    private static extern void LTTNative_AddCustomScriptPayload(string payloadJson);
    void Awake()
    {
        foreach (var s in scripts)
        {
            if (s == null) continue;
            if (string.IsNullOrEmpty(s.scriptId)) { Debug.LogWarning("CustomScript with empty id"); continue; }
            LTTNative_AddCustomScript(s.scriptId); // safe: sends string to native
            // Optionally send payload:
            LTTNative_AddCustomScript(s.payloadJson);
        }
    }
}