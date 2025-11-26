using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WaitForSeconds wait = new WaitForSeconds(3f);
        Debug.Log("Loading level");
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadTest");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
