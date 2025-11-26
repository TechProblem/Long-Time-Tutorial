using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LTTPlugin : MonoBehaviour
{
    // Named imports (native exports must match exactly)
    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FileHandler();

    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTT_main();

    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTTWrite();

    // Use persistentDataPath so it works in Editor and builds
    public string directoryPath;
    public string path;

    public int Threshold_years;
    public int Threshold_months;
    public int Threshold_days;
    public int Threshold_hours;
    public int Threshold_minutes;
    public int Threshold_seconds;
    public GameObject Custom_Scripts;

    void Awake()
    {
        // initialize paths at runtime
        directoryPath = Path.Combine(Application.persistentDataPath, "LTTFiles");
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        path = Path.Combine(directoryPath, "Text.txt");

        try
        {
            LTT_main();
        }
        catch (DllNotFoundException e)
        {
            Debug.LogError("LTTPlugin: DllNotFoundException: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        try
        {
            LTTWrite();
            Debug.Log("Application is quitting, time written to file.");
        }
        catch (DllNotFoundException e)
        {
            Debug.LogError("LTTPlugin: DllNotFoundException on quit: " + e.Message);
        }
    }

    void Start() { }
    void Update() { }
}