using UnityEngine;

public class PassScene : MonoBehaviour
{
    void Start()
    {
        
    }
    public void PassTheScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("BarcoCutscene");
    }
}
