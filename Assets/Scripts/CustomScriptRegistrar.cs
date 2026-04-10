using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Attach this to a persistent manager GameObject (for example the same object that holds LTTPlugin)
public class CustomScriptRegistrar : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void LTTNative_AddCustomScript(string scriptId);
    [DllImport("__Internal")]
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
                LTTNative_AddCustomScriptJson(text);
            }
            else
            {
                // Fallback: register the identifier or raw text as a simple script entry
                // Prefer id if present, otherwise send the text
                if (!string.IsNullOrEmpty(s.scriptId))
                    LTTNative_AddCustomScript(s.scriptId);
                else if (!string.IsNullOrEmpty(text))
                    LTTNative_AddCustomScript(text);
            }
        }
    }
}