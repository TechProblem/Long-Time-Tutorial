using UnityEngine;

[CreateAssetMenu(menuName = "Scripts/LogExample")]
public class LogExample : ScriptableObject
{
    [Header("Identifier")]
    public string scriptId;        // unique id or name

    [Header("Script Text / JSON")]
    [TextArea(4, 12)]
    public string scriptText;
    public string payloadJson;

    [Tooltip("If true, this text will be logged when the threshold is met.")]
    public bool logScriptText = true;




}
