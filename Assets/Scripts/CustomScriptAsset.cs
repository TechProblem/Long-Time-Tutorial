using UnityEngine;

[CreateAssetMenu(menuName = "LTT/CustomScriptAsset")]
public class CustomScriptAsset : ScriptableObject
{
    [Header("Identifier")]
    public string scriptId;

    [Header("Script Text / JSON")]
    [TextArea(4, 12)]
    public string scriptText;

    [Tooltip("If true the script text will be logged at Awake by the registrar.")]
    public bool logOnRegister = true;
}