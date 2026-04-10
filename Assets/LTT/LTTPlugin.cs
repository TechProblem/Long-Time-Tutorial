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
    private static extern void LTT_main(int instanceId);


    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTTWrite();

    [DllImport("LTT", CallingConvention = CallingConvention.Cdecl)]
    private static extern void LTTRead();


    // Use persistentDataPath so it works in Editor and builds
    public string directoryPath;
    public string path;

    public int Threshold_years;
    public int Threshold_months;
    public int Threshold_days;
    public int Threshold_hours;
    public int Threshold_minutes;
    public int Threshold_seconds;
    public ScriptableObject Custom_Scripts;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        

        // initialize paths at runtime

        LTT.directoryPath = Path.Combine(Application.persistentDataPath, "LTTFiles");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        LTT.path = Path.Combine(directoryPath, "LTTTime.txt");
        if (!File.Exists(path))
        {
            File.WriteAllText(path, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        FileHandler();
        LTTRead();
        
        LTT_main(Custom_Scripts.GetInstanceID());
        Debug.Log(path);

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