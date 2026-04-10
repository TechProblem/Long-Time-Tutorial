using System.Runtime.InteropServices;
using UnityEngine;



public class SceneScriptRegistrar : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void LTTNative_AddCustomScript(string scriptId);
    [SerializeField] private GameObject customScriptsParent;

    [SerializeField] private string scriptId;

    void Awake()
    {
        if (customScriptsParent == null) return;
        var comps = customScriptsParent.GetComponentsInChildren<CustomScriptComponent>(includeInactive: true);
        foreach (var c in comps)
        {
            if (string.IsNullOrEmpty(c.scriptId)) continue;
            LTTNative_AddCustomScript(c.scriptId);
        }
    }
}