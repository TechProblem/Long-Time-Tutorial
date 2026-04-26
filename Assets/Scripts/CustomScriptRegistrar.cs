using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Attach this to a persistent manager GameObject (for example the same object that holds LTTPlugin)
public class CustomScriptRegistrar : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    private const string LTT_NATIVE = "__Internal";
#else
    private const string LTT_NATIVE = "LTT";
#endif

    [DllImport(LTT_NATIVE, CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTTNative_AddCustomScript(string scriptId);
    [DllImport(LTT_NATIVE, CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTTNative_AddCustomScriptJson(string scriptJson);
    [SerializeField] private CustomScriptAsset[] scripts = Array.Empty<CustomScriptAsset>();

    void Awake()
    {
        foreach (var s in scripts)
        {
            if (s == null) continue;

            var id = string.IsNullOrEmpty(s.scriptId) ? "<no-id>" : s.scriptId;
            var text = s.scriptText ?? string.Empty;

            if (s.logOnRegister)
            {
                Debug.Log($"CustomScript register: id={id}, text={text}");
            }

            // Choose how to register with native: JSON vs simple identifier
            var trimmed = text.TrimStart();
            if (!string.IsNullOrEmpty(trimmed) && (trimmed[0] == '{' || trimmed[0] == '['))
            {
                // If the script text is JSON, use the JSON-aware native call
                try
                {
                    LTTNative_AddCustomScriptJson(text);
                }
                catch (EntryPointNotFoundException e)
                {
                    Debug.LogError("LTT native entry point not found: LTTNative_AddCustomScriptJson. " +
                        "Check that the native plugin exports this symbol and plugin import settings. Exception: " + e.Message);
                }
                catch (DllNotFoundException e)
                {
                    Debug.LogError("LTT native library not found when calling LTTNative_AddCustomScriptJson. " + e.Message);
                }
            }
            else
            {
                // Fallback: register the identifier or raw text as a simple script entry
                // Prefer id if present, otherwise send the text
                try
                {
                    if (!string.IsNullOrEmpty(s.scriptId))
                    {
                        LTTNative_AddCustomScript(s.scriptId);
                    }
                    else if (!string.IsNullOrEmpty(text))
                    {
                        LTTNative_AddCustomScript(text);
                    }
                }
                catch (EntryPointNotFoundException e)
                {
                    Debug.LogError("LTT native entry point not found: LTTNative_AddCustomScript. " +
                        "Check that the native plugin exports this symbol and plugin import settings. Exception: " + e.Message);
                }
                catch (DllNotFoundException e)
                {
                    Debug.LogError("LTT native library not found when calling LTTNative_AddCustomScript. " + e.Message);
                }
            }
        }
    }
}
