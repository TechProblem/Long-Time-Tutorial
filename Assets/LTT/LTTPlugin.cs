using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;


public class LTTPlugin : MonoBehaviour
{
    // Named imports (native exports must match exactly)
    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FileHandler();

    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTT_main(int customScriptsInstanceId);


    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTTWrite();

    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTTRead();

    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTTSetThresholds(
        int years, int months, int days, int hours, int minutes, int seconds);
    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTTGetThresholds(
        out int years, out int months, out int days, out int hours, out int minutes, out int seconds);


    // Use persistentDataPath so it works in Editor and builds
    public string directoryPath;
    public string path;
    public int years;
    public int months;
    public int days;
    public int hours;
    public int minutes;
    public int seconds;

    //public int Threshold_years;
    //public int Threshold_months;
    //public int Threshold_days;
    //public int Threshold_hours;
    //public int Threshold_minutes;
    //public int Threshold_seconds;
    // Reference a GameObject that contains the registrar / custom-scripts component
    // The native plugin can be passed the instance id of this GameObject so it can
    // call back into Unity if needed.
    public GameObject Custom_Scripts;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        

        // initialize paths at runtime

        // Use the instance fields (not an undefined `LTT` class)
        directoryPath = Path.Combine(Application.persistentDataPath, "LTTFiles");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        path = Path.Combine(directoryPath, "LTTTime.txt");
        if (!File.Exists(path))
        {
           
            File.WriteAllText(path, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        // Native plugin calls can throw DllNotFoundException when the native library isn't present.
        try
        {
            FileHandler();
            LTTRead();

            // Pass the instance id of the GameObject holding the registrar (or 0)
            var instanceId = (Custom_Scripts != null) ? Custom_Scripts.GetInstanceID() : 0;
            try
            {
                LTT_main(instanceId);
                Debug.Log($"LTTPlugin: called LTT_main with instance id={instanceId}");
            }
            catch (EntryPointNotFoundException e)
            {
                Debug.LogError("LTTPlugin: native entry point LTT_main not found: " + e.Message);
            }

            Debug.Log(path);
        }
        catch (DllNotFoundException e)
        {
            Debug.LogError("LTTPlugin: native library not found. Make sure 'ltt.dll' (or the correct native plugin) is placed in a Plugins folder for the target platform. Exception: " + e.Message);
        }

        //catch (DllNotFoundException e)
        //{
        //    Debug.LogError("LTTPlugin: DllNotFoundException: " + e.Message);
        //}
    }

    private void OnApplicationQuit()
    {

        LTTWrite();
        Debug.Log("Application is quitting, time written to file.");
        //}
        //catch (DllNotFoundException e)
        //{
        //    Debug.LogError("LTTPlugin: DllNotFoundException on quit: " + e.Message);
        //}
    }

    //void Start() { }
    //void Update() { }
}