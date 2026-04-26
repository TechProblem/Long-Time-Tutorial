using UnityEngine;

// Attach this component to the GameObject that should receive native activation messages.
// On Start it registers this GameObject's name and the method name with the native plugin so
// the plugin can call UnitySendMessage("gameObjectName", "methodName", "message").
public class LTTAutoRegister : MonoBehaviour
{
    // Name of the method that will be called via UnitySendMessage
    public string methodName = "OnNativeActivate";

    // Optionally enable native automatic activation
    public bool enableRunCustom = true;

    void Start()
    {
        // register this GameObject with the native plugin
        LTTNative.SetUnityTarget(gameObject.name);
        LTTNative.SetUnityMethod(methodName);

        if (enableRunCustom)
            LTTNative.RunCustom = true;

        Debug.Log($"LTTAutoRegister: registered target '{gameObject.name}' method '{methodName}' runCustom={LTTNative.RunCustom}");
    }
}
