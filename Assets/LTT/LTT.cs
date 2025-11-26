using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is used to check the time in Unity

public class LTT : MonoBehaviour
{
    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FileHandler();

    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTT_main();

    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTTWrite();

    
    
    // CHANGE THIS PATH TO YOUR DESIRED DIRECTORY
    public static string directoryPath = ".../Assets/Resources/";

    

    // Public Time Thresholds for customizing the thresholds
    public int Threshold_years;
    public int Threshold_months;
    public int Threshold_days;
    public int Threshold_hours;
    public int Threshold_minutes;
    public int Threshold_seconds;
    // Custom Scripts is an optional field to enable a script when the threshold is exceeded
    public GameObject Custom_Scripts;
 
    public static string path = Path.Combine(directoryPath, "Text.txt");
    void Awake()
    {
        LTT_main();
    }
    private void OnApplicationQuit()
    {
        LTTWrite();
        Debug.Log("Application is quitting, time written to file.");
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}

